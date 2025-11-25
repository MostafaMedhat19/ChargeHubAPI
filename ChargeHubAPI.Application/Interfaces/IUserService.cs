using ChargeHubAPI.Application.Contracts.Requests;
using ChargeHubAPI.Application.Contracts.Responses;

namespace ChargeHubAPI.Application.Interfaces;

public interface IUserService
{
    Task<SignUpResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<StandardResponse> RegisterEsp32Async(RegisterEsp32Request request, CancellationToken cancellationToken);
    Task<StandardResponse> UpdateCarChargeAsync(UpdateCarChargeRequest request, CancellationToken cancellationToken);
    Task<StandardResponse> UpdateStatusPositionAsync(UpdateStatusPositionRequest request, CancellationToken cancellationToken);
    Task<UserInfoResponse?> GetUserAsync(string userId, CancellationToken cancellationToken);
    Task<IdentecationResponse?> GetIdentecationAsync(string userId, CancellationToken cancellationToken);
    Task<StandardResponse> DeleteUserAsync(string userId, CancellationToken cancellationToken);
    Task<StandardResponse> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken);
    Task<StandardResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken);
    Task<StandardResponse> VerifySignupAsync(VerifySignupRequest request, CancellationToken cancellationToken);
}



