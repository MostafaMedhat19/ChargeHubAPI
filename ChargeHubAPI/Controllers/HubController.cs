using ChargeHubAPI.Application.Contracts.Requests;
using ChargeHubAPI.Application.Contracts.Responses;
using ChargeHubAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChargeHubAPI.Controllers;

[ApiController]
[Route("api/hub")]
public class HubController : ControllerBase
{
    private readonly IUserService _userService;

    public HubController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("update-position")]
    [SwaggerOperation(
        Summary = "Update Status Position",
        Description = """
            Layer: API + Application.
            Request JSON Example:
            {
              "identecation": "84219932",
              "status_position": {
                "north": 30.5,
                "east": 15.25,
                "south": 10.75,
                "west": 5
              }
            }
            Response JSON Example:
            {
              "success": true,
              "message": "Position updated"
            }
            """)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePosition([FromBody] UpdateStatusPositionRequest request, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        var response = await _userService.UpdateStatusPositionAsync(request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}



