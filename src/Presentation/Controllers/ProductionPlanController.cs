namespace PowerPlantService.Controllers;

using Application.Commands;
using Application.Commands.Mediator;
using Application.DTO.Request;
using Application.DTO.Response;
using Infrastructure.CrossCutting.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("productionplan")]
public class ProductionPlanController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductionPlanController> _logger;

    public ProductionPlanController(IMediator mediator, ILogger<ProductionPlanController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [SwaggerOperation("Calculates the Production Plan for the specifc Load")]
    [ProducesResponseType(typeof(List<PowerPlantResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromBody][SwaggerRequestBody("ProductionPlanRequest")] ProductionPlanRequest productionPlanRequest, CancellationToken ct = default)
    {
        try
        {
            var command = new ProductionPlanCommand(productionPlanRequest);
            var result = await _mediator.Send<ProductionPlanCommand, List<PowerPlantResult>>(command);

            return Ok(result);
        }
        catch (CustomValidationException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            _logger.LogError("Error calculating the Production Plan",
                () => new
                {
                    Exception = ex,
                    Request = productionPlanRequest
                });

            return StatusCode(500);
        }
    }
}