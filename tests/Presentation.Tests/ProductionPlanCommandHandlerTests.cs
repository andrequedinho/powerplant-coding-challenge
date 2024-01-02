namespace Tests;

using System.Text.Json;
using System.Threading.Tasks;
using Application.Commands;
using Application.DTO.Request;
using Application.DTO.Response;

public class ProductionPlanCommandHandlerTests
{
    [Fact]
    public async Task CommandHandler_Payload1_ShouldReturnsCorrectProductionPlan()
    {
        // Arrange
        var commandHandler = new ProductionPlanCommandHandler();
        var request = await GetPayload("payload1.json");
        var command = new ProductionPlanCommand(request);
        var response = await GetResponse("response1.json");

        // Act
        var result = await commandHandler.Handle(command);

        Assert.IsType<List<PowerPlantResult>>(result);
        Assert.Equal(response.Count, result.Count);

        for (int i = 0; i < result.Count; i++)
        {
            Assert.Equal(response[i].Name, result[i].Name);
            Assert.Equal(response[i].Power, result[i].Power);
        }
    }

    [Fact]
    public async Task CommandHandler_Payload2_ShouldReturnsCorrectProductionPlan()
    {
        // Arrange
        var commandHandler = new ProductionPlanCommandHandler();
        var request = await GetPayload("payload2.json");
        var command = new ProductionPlanCommand(request);
        var response = await GetResponse("response2.json");

        // Act
        var result = await commandHandler.Handle(command);

        Assert.IsType<List<PowerPlantResult>>(result);
        Assert.Equal(response.Count, result.Count);

        for (int i = 0; i < result.Count; i++)
        {
            Assert.Equal(response[i].Name, result[i].Name);
            Assert.Equal(response[i].Power, result[i].Power);
        }
    }

    [Fact]
    public async Task CommandHandler_Payload3_ShouldReturnsCorrectProductionPlan()
    {
        // Arrange
        var commandHandler = new ProductionPlanCommandHandler();
        var request = await GetPayload("payload3.json");
        var command = new ProductionPlanCommand(request);
        var response = await GetResponse("response3.json");

        // Act
        var result = await commandHandler.Handle(command);

        Assert.IsType<List<PowerPlantResult>>(result);
        Assert.Equal(response.Count, result.Count);

        for (int i = 0; i < result.Count; i++)
        {
            Assert.Equal(response[i].Name, result[i].Name);
            Assert.Equal(response[i].Power, result[i].Power);
        }
    }

    private async Task<ProductionPlanRequest> GetPayload(string name)
    {
        var jsonPayload = await File.ReadAllTextAsync(Path.Combine("example_payloads", name));
        return JsonSerializer.Deserialize<ProductionPlanRequest>(jsonPayload);
    }

    private async Task<List<PowerPlantResult>> GetResponse(string name)
    {
        var jsonResponse = await File.ReadAllTextAsync(Path.Combine("example_payloads", name));
        return JsonSerializer.Deserialize<List<PowerPlantResult>>(jsonResponse);
    }
}
