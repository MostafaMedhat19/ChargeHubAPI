using System.Security.Claims;
using ChargeHubAPI.Application.Contracts.Requests;
using ChargeHubAPI.Application.Contracts.Responses;
using ChargeHubAPI.Application.Interfaces;
using ChargeHubAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChargeHubAPI.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register-esp32")]
    [SwaggerOperation(
        Summary = "ESP32 Registration",
        Description = """
            Layer: API + Application.
            Request JSON Example:
            {
              "identecation": "84219932",
              "btName": "ESP32-Charger",
              "btAddress": "AA:BB:CC:DD:EE:FF"
            }
            Response JSON Example:
            {
              "success": true,
              "message": "ESP32 registered successfully"
            }
            """)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterEsp32([FromBody] RegisterEsp32Request request, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        var response = await _userService.RegisterEsp32Async(request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpGet("{userId}")]
    [SwaggerOperation(
        Summary = "Get User Info",
        Description = """
            Layer: API + Application.
            Response JSON Example:
            {
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
            """)]
    [ProducesResponseType(typeof(UserInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(string userId, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        var user = await _userService.GetUserAsync(userId, cancellationToken);
        if (user is null)
        {
            return NotFound(new StandardResponse
            {
                Success = false,
                Message = "User not found"
            });
        }

        return Ok(user);
    }

    [Authorize]
    [HttpGet("{userId}/identecation")]
    [SwaggerOperation(
        Summary = "Get Identecation Code",
        Description = """
            Layer: API + Application.
            Requires Authorization header with Bearer token.
            Response JSON Example:
            {
              "success": true,
              "message": "Identecation retrieved",
              "userId": "USR-551201",
              "identecation": "84219932"
            }
            """)]
    [ProducesResponseType(typeof(IdentecationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIdentecationCode(string userId, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        if (!IsCurrentUser(userId))
        {
            return Forbid();
        }

        var response = await _userService.GetIdentecationAsync(userId, cancellationToken);
        return response is null
            ? NotFound(new StandardResponse { Success = false, Message = "User not found" })
            : Ok(response);
    }

    [Authorize]
    [HttpDelete("{userId}")]
    [SwaggerOperation(
        Summary = "Delete Account",
        Description = """
            Layer: API + Application.
            Requires Authorization header with Bearer token.
            Response JSON Example:
            {
              "success": true,
              "message": "Account deleted"
            }
            """)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAccount(string userId, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        if (!IsCurrentUser(userId))
        {
            return Forbid();
        }

        var response = await _userService.DeleteUserAsync(userId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    private bool IsCurrentUser(string userId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return currentUserId is not null && string.Equals(currentUserId, userId, StringComparison.Ordinal);
    }

    [HttpGet("by-identecation/{identecation}")]
    public async Task<IActionResult> GetNameByIdentecation(string identecation, CancellationToken cancellationToken)
    {

        var response = await _userService.GetNameByIdentecationAsync(identecation, cancellationToken);
        if (response is null)
        {
            return NotFound(new StandardResponse
            {
                Success = false,
                Message = "User not found"
            });
        }
        return Ok(response);
    }
}



