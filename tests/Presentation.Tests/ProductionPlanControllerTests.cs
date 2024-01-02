namespace Tests;

using Application.Commands.Mediator;
using Application.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PowerPlantService.Controllers;

public class ProductionPlanControllerTests
{
    [Fact]
    public async Task Post_WithValidPayload_ShouldReturnStatusOk()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<ProductionPlanController>>();
        var target = new ProductionPlanController(mediator, logger);
        var request = new ProductionPlanRequest();

        // Act
        var result = await target.Post(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}