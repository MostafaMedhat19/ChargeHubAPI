using ChargeHubAPI.Application.Contracts.Requests;
using ChargeHubAPI.Application.Contracts.Responses;
using ChargeHubAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChargeHubAPI.Controllers;

[ApiController]
[Route("api/esp32")]
public class Esp32Controller : ControllerBase
{
    private readonly IUserService _userService;

    public Esp32Controller(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("update-charge")]
    [SwaggerOperation(
        Summary = "Update Car Charge",
        Description = """
            Layer: API + Application.
            Request JSON Example:
            {
              "identecation": "84219932",
              "car_charge": 87
            }
            Response JSON Example:
            {
              "success": true,
              "message": "Car charge updated"
            }
            """)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCharge([FromBody] UpdateCarChargeRequest request, CancellationToken cancellationToken)
    {
        // Layer: API + Application
        var response = await _userService.UpdateCarChargeAsync(request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}




