using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using ChargeHubAPI.Application.Contracts.Requests;
using ChargeHubAPI.Application.Contracts.Responses;
using ChargeHubAPI.Application.Dtos;
using ChargeHubAPI.Application.Interfaces;
using ChargeHubAPI.Application.Templates;
using ChargeHubAPI.Domain.Entities;

namespace ChargeHubAPI.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;

    public UserService(IUserRepository repository, ITokenProvider tokenProvider, IMapper mapper, IEmailSender emailSender)
    {
        _repository = repository;
        _tokenProvider = tokenProvider;
        _mapper = mapper;
        _emailSender = emailSender;
    }

    public async Task<SignUpResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken)
    {
        if (!NameRegex.IsMatch(request.Name))
        {
            return SignUpFailure("Name must contain only letters and spaces.");
        }

        var existingUser = await _repository.GetByUsernameAsync(request.Username, cancellationToken);
        if (existingUser is not null)
        {
            return SignUpFailure("Username already exists");
        }

        var existingPhone = await _repository.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (existingPhone is not null)
        {
            return SignUpFailure("Phone number already exists");
        }

        var existingEmail = await _repository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingEmail is not null)
        {
            return SignUpFailure("Email already exists");
        }

        var user = new User
        {
            UserId = GenerateUserId(),
            Identecation = GenerateIdentecation(),
            Username = request.Username,
            Name = request.Name.Trim(),
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            CarCharge = 0,
            PasswordHash = HashPassword(request.Password),
            Esp32 = null,
            StatusPosition = null,
            SignupVerificationCode = GenerateVerificationCode(),
            SignupVerificationExpiresAt = DateTimeOffset.UtcNow.Add(VerificationCodeLifetime)
        };

        await _repository.AddAsync(user, cancellationToken);

        var html = EmailTemplateBuilder.BuildVerificationTemplate(
            user.Name,
            user.SignupVerificationCode!,
            "complete your ChargeHub sign up");

        await _emailSender.SendVerificationCodeAsync(
            user.Email,
            "ChargeHub Sign-Up Verification Code",
            html,
            cancellationToken);

        return new SignUpResponse
        {
            Success = true,
            Message = "User created successfully",
            UserId = user.UserId,
            Identecation = user.Identecation
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid credentials"
            };
        }

        var dto = _mapper.Map<UserDto>(user);

        return new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            Token = _tokenProvider.GenerateToken(user),
            User = dto
        };
    }

    public async Task<StandardResponse> RegisterEsp32Async(RegisterEsp32Request request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdentecationAsync(request.Identecation, cancellationToken);
        if (user is null)
        {
            return Failure("User not found");
        }

        user.Esp32 = new Esp32Device
        {
            BtName = request.BtName,
            BtAddress = request.BtAddress
        };

        await _repository.UpdateAsync(user, cancellationToken);

        return Success("ESP32 registered successfully");
    }

    public async Task<StandardResponse> UpdateCarChargeAsync(UpdateCarChargeRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdentecationAsync(request.Identecation, cancellationToken);
        if (user is null)
        {
            return Failure("User not found");
        }

        user.CarCharge = request.CarCharge;
        await _repository.UpdateAsync(user, cancellationToken);
        return Success("Car charge updated");
    }

    public async Task<StandardResponse> UpdateStatusPositionAsync(UpdateStatusPositionRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdentecationAsync(request.Identecation, cancellationToken);
        if (user is null)
        {
            return Failure("User not found");
        }

        user.StatusPosition ??= new StatusPosition();
        user.StatusPosition.North = request.StatusPosition.North;
        user.StatusPosition.East = request.StatusPosition.East;
        user.StatusPosition.South = request.StatusPosition.South;
        user.StatusPosition.West = request.StatusPosition.West;

        await _repository.UpdateAsync(user, cancellationToken);
        return Success("Position updated");
    }

    public async Task<UserInfoResponse?> GetUserAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByUserIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return null;
        }

        return _mapper.Map<UserInfoResponse>(user);
    }

    public async Task<IdentecationResponse?> GetIdentecationAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByUserIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return null;
        }

        return new IdentecationResponse
        {
            Success = true,
            Message = "Identecation retrieved",
            UserId = user.UserId,
            Identecation = user.Identecation
        };
    }

    public async Task<StandardResponse> VerifySignupAsync(VerifySignupRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Failure("User not found");
        }

        if (user.SignupVerificationCode is null || user.SignupVerificationExpiresAt is null)
        {
            return Failure("No verification pending");
        }

        if (user.SignupVerificationExpiresAt < DateTimeOffset.UtcNow)
        {
            return Failure("Verification code expired");
        }

        if (!string.Equals(user.SignupVerificationCode, request.VerificationCode, StringComparison.Ordinal))
        {
            return Failure("Invalid verification code");
        }

        user.SignupVerificationCode = null;
        user.SignupVerificationExpiresAt = null;

        await _repository.UpdateAsync(user, cancellationToken);
        return Success("Account verified");
    }

    public async Task<StandardResponse> DeleteUserAsync(string userId, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(userId, cancellationToken);
        return deleted
            ? Success("Account deleted")
            : Failure("User not found");
    }

    public async Task<StandardResponse> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (user is null)
        {
            return Failure("Phone number not found");
        }

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            return Failure("Phone/email combination not found");
        }

        user.PasswordResetCode = GenerateResetCode();
        user.PasswordResetCodeExpiresAt = DateTimeOffset.UtcNow.Add(ResetCodeLifetime);

        await _repository.UpdateAsync(user, cancellationToken);

        var html = EmailTemplateBuilder.BuildVerificationTemplate(
            user.Name,
            user.PasswordResetCode!,
            "reset your ChargeHub password");

        await _emailSender.SendVerificationCodeAsync(
            user.Email,
            "ChargeHub Password Reset Code",
            html,
            cancellationToken);
        return Success("Reset code generated and sent");
    }

    public async Task<StandardResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (user is null)
        {
            return Failure("Phone number not found");
        }

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            return Failure("Phone/email combination not found");
        }

        if (user.PasswordResetCode is null || user.PasswordResetCodeExpiresAt is null)
        {
            return Failure("No reset request found");
        }

        if (!string.Equals(user.PasswordResetCode, request.ResetCode, StringComparison.Ordinal))
        {
            return Failure("Invalid reset code");
        }

        if (user.PasswordResetCodeExpiresAt < DateTimeOffset.UtcNow)
        {
            return Failure("Reset code expired");
        }

        user.PasswordHash = HashPassword(request.NewPassword);
        user.PasswordResetCode = null;
        user.PasswordResetCodeExpiresAt = null;

        await _repository.UpdateAsync(user, cancellationToken);

        return Success("Password reset successfully");
    }

    private static string GenerateUserId() => $"USR-{Random.Shared.Next(100000, 999999)}";

    private static string GenerateIdentecation() => Random.Shared.Next(10000000, 99999999).ToString();

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string password, string hash) => HashPassword(password) == hash;

    private static StandardResponse Success(string message) => new()
    {
        Success = true,
        Message = message
    };

    private static StandardResponse Failure(string message) => new()
    {
        Success = false,
        Message = message
    };

    private static SignUpResponse SignUpFailure(string message) => new()
    {
        Success = false,
        Message = message
    };

    public async Task<NameResponse?> GetNameByIdentecationAsync(string identecation, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdentecationAsync(identecation, cancellationToken);

        if (user is null)
        {
            return null;   
        }

        return new NameResponse
        {
            Success = true,
            Message = "Name retrieved successfully",
            Name = user.Name,
            Username = user.Username
        };
    }


    private static string GenerateResetCode() => Random.Shared.Next(100000, 999999).ToString("D6");

    private static string GenerateVerificationCode() => Random.Shared.Next(100000, 999999).ToString("D6");

    private static readonly Regex NameRegex = new("^[A-Za-z ]{2,100}$", RegexOptions.Compiled);
    private static readonly TimeSpan ResetCodeLifetime = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan VerificationCodeLifetime = TimeSpan.FromMinutes(15);
}

