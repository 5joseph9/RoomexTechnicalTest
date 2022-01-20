using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoomexTechnicalTest.Application.Features.GeoLocation.Command.CalculateGeoDistance;
using RoomexTechnicalTest.Application.Models.Validation;

namespace RoomexTechnicalTest.Api.Controllers {

    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(string), 500)]
    [Route("api/v{api-version:apiVersion}/[controller]")]

    public class GeoLocationController : ControllerBase {
        private readonly IMediator _mediator;

        public GeoLocationController(IMediator mediator) => _mediator = mediator;

        [HttpPost("GeoDistanceInKm", Name = nameof(CalculateGeoDistanceInKm))]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateGeoDistanceInKm([FromBody] CalculateGeoDistanceCommand calculateGeoDistanceCommand) {
            var distance = await _mediator.Send(calculateGeoDistanceCommand);

            return Ok(distance);
        }
    }
}