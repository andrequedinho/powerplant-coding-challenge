namespace Application.Commands;

using FluentValidation;

public class ProductionPlanCommandValidator : AbstractValidator<ProductionPlanCommand>
{
    public ProductionPlanCommandValidator()
    {
        RuleFor(x => x.ProductionPlanRequest).NotNull().WithMessage("Request cannot be null");
        RuleFor(x => x.ProductionPlanRequest.Load).GreaterThan(0).WithMessage("Load cannot be less or equal to zero");

        RuleFor(x => x.ProductionPlanRequest.PowerPlants).NotNull().WithMessage("PowerPlants cannot be null");
        RuleFor(x => x.ProductionPlanRequest.PowerPlants).NotEmpty().WithMessage("You must define at least one PowerPlant");

        RuleFor(x => x.ProductionPlanRequest.Fuels).NotNull().WithMessage("Fuels cannot be null");
    }
}