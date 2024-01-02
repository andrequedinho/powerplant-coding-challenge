namespace Application.Commands;

using Application.Commands.Interfaces;
using Application.DTO.Request;
using Application.DTO.Response;

public class ProductionPlanCommand : ICommand<List<PowerPlantResult>>
{
    public ProductionPlanCommand(ProductionPlanRequest request)
    {
        ProductionPlanRequest = request;
    }

    public ProductionPlanRequest ProductionPlanRequest { get; private set; }

}
