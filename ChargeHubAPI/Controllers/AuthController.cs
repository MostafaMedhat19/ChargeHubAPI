using ChargeHubAPI.Application.Contracts.Requests;
using ChargeHubAPI.Application.Contracts.Responses;
using ChargeHubAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChargeHubAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("signup")]
    [SwaggerOperation(
        Summary = "User Sign Up",
        Description = """
            Layer: API + Application.
            Request JSON Example:
            {
              "username": "mostafa123",
              "name": "Mostafa Medhat",
              "phoneNumber": "+201234567890",
              "email": "mostafa@example.com",
              "password": "12345678",
              "confirmPassword": "12345678"
            }
            Response JSON Example:
            {
              "success": true,
              "message": "User created successfully",
              "userId": "USR-551201",
              "identecation": "84219932"
            }
            Notes:
            - Sends a 6-digit HTML formatted verification code to the signup email.
            """)]
    [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        var response = await _userService.SignUpAsync(request, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "User Sign In",
        Description = """
            Layer: API + Application.
            Request JSON Example:
            {
              "username": "mostafa123",
              "password": "12345678"
            }
            Response JSON Example:
            {
              "success": true,
              "token": "JWT_TOKEN",
              "user": {
                "userId": "USR-551201",
                "identecation": "84219932",
                "username": "mostafa123",
                "name": "Mostafa Medhat",
                "phoneNumber": "+201234567890",
                "email": "mostafa@example.com",
                "car_charge": 87,
                "esp32": {
                  "btName": "ESP32-Charger",
                  "btAddress": "AA:BB:CC:DD:EE:FF"
                },
                "status_position": {
                  "north": 30.5,
                  "east": 15.25,
                  "south": 10.75,
                  "west": 5
                }
              }
            }
            """)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        var response = await _userService.LoginAsync(request, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("forgot-password")]
    [SwaggerOperation(
        Summary = "Forgot Password",
        Description = """
            Layer: API + Application.
            Request JSON Example:
            {
              "phoneNumber": "+201234567890",
              "email": "mostafa@example.com"
            }
            Response JSON Example:
            {
              "success": true,
              "message": "Reset code generated and sent"
            }
            Notes:
            - Uses the email + phone combination to send an HTML verification code.
            """)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        var response = await _userService.ForgotPasswordAsync(request, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("reset-password")]
    [SwaggerOperation(
        Summary = "Reset Password",
        Description = """
            Layer: API + Application.
            Request JSON Example:
            {
              "phoneNumber": "+201234567890",
              "email": "mostafa@example.com",
              "resetCode": "123456",
              "newPassword": "StrongPass#1",
              "confirmPassword": "StrongPass#1"
            }
            Response JSON Example:
            {
              "success": true,
              "message": "Password reset successfully"
            }
            """)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        var response = await _userService.ResetPasswordAsync(request, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("verify-signup")]
    [SwaggerOperation(
        Summary = "Verify Signup Code",
        Description = """
            Layer: API + Application.
            Request JSON Example:
            {
              "userId": "USR-551201",
              "verificationCode": "654321"
            }
            Response JSON Example:
            {
              "success": true,
              "message": "Account verified"
            }
            """)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifySignup([FromBody] VerifySignupRequest request, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        var response = await _userService.VerifySignupAsync(request, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}



