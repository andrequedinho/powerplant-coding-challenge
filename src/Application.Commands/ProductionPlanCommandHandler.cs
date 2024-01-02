namespace Application.Commands;

using System;
using Application.Commands.Interfaces;
using Application.DTO.Request;
using Application.DTO.Response;
using Infrastructure.CrossCutting;

public class ProductionPlanCommandHandler : ICommandHandler<ProductionPlanCommand, List<PowerPlantResult>>
{
    private double _remainingLoad;

    public async Task<List<PowerPlantResult>> Handle(ProductionPlanCommand command)
    {
        CalculateCostByFuel(command.ProductionPlanRequest);
        return await Task.FromResult(AllocatePower(command.ProductionPlanRequest));
    }

    private void CalculateCostByFuel(ProductionPlanRequest payload)
    {
        payload.PowerPlants.ForEach(x =>
        {
            switch (x.Type)
            {
                case Constants.WIND_TURBINE_TYPE:
                    x.Cost = 0;
                    break;

                case Constants.GAS_FIRED_TYPE:
                    x.Cost = Math.Round((payload.Fuels.GasPrice / x.Efficiency) + payload.Fuels.CO2Price * Constants.CO2_TON_BY_MWH, 1);
                    break;

                case Constants.TURBO_JET_TYPE:
                    x.Cost = Math.Round(payload.Fuels.KerosinePrice / x.Efficiency, 1);
                    break;
                default:
                    break;
            }
        });
    }

    private List<PowerPlantResult> AllocatePower(ProductionPlanRequest payload)
    {
        var orderedPowerPlants = OrderPowerPlantsByCost(payload);
        _remainingLoad = payload.Load;

        foreach (var powerPlant in orderedPowerPlants)
        {
            CalculatePower(payload, powerPlant);

            if (_remainingLoad <= 0)
            {
                break;
            }
        }

        VerifyAndAdjustBalance(orderedPowerPlants);

        return orderedPowerPlants.Select(p => new PowerPlantResult()
        {
            Name = p.Name,
            Power = p.PowerProduced,
            TotalCost = p.TotalCost,
        }).ToList();
    }

    private List<Powerplant> OrderPowerPlantsByCost(ProductionPlanRequest payload)
    {
        // Sort power plants by cost and name
        var orderedPowerPlants = payload.PowerPlants
            .OrderBy(p => p.Cost).ThenByDescending(p => p.Pmax).ThenBy(p => p.Name);

        if (payload.Fuels.WindPercentage == 0)
        {
            orderedPowerPlants = payload.PowerPlants
            .OrderBy(p => p.Type).ThenBy(p => p.Cost).ThenByDescending(p => p.Pmax);
        }

        return orderedPowerPlants.ToList();
    }

    private void CalculatePower(ProductionPlanRequest payload, Powerplant powerPlant)
    {
        if (_remainingLoad > 0)
        {
            powerPlant.PowerProduced = 0.0;

            switch (powerPlant.Type)
            {
                case Constants.WIND_TURBINE_TYPE:
                    powerPlant.PowerProduced = Math.Round(powerPlant.Pmax * payload.Fuels.WindPercentage * powerPlant.Efficiency / 100, 1);
                    break;

                case Constants.GAS_FIRED_TYPE:
                    powerPlant.PowerProduced = powerPlant.Pmin + Math.Round(payload.Load * powerPlant.Efficiency, 1);
                    break;

                case Constants.TURBO_JET_TYPE:
                    powerPlant.PowerProduced = powerPlant.Pmin + Math.Round(payload.Load * powerPlant.Efficiency, 1);
                    break;

                default:
                    break;
            }

            GetTotalPowerProduced(powerPlant);
            powerPlant.TotalCost = Math.Round(powerPlant.PowerProduced * powerPlant.Cost, 1);
            _remainingLoad -= powerPlant.PowerProduced;
        }
    }

    private void GetTotalPowerProduced(Powerplant powerPlant)
    {
        if (powerPlant.Type != Constants.WIND_TURBINE_TYPE)
        {
            if (
                (powerPlant.PowerProduced > powerPlant.Pmax) ||
                (powerPlant.PowerProduced < powerPlant.Pmax && powerPlant.PowerProduced <= _remainingLoad)
               )
            {
                powerPlant.PowerProduced = powerPlant.Pmax;
                powerPlant.PowerProduced = Math.Round(Math.Min(powerPlant.PowerProduced, _remainingLoad), 1);
            }
            else if (powerPlant.PowerProduced > _remainingLoad && _remainingLoad <= powerPlant.Pmin)
            {
                powerPlant.PowerProduced = powerPlant.Pmin;
            }
        }
    }

    private void VerifyAndAdjustBalance(List<Powerplant> powerPlants)
    {
        if (_remainingLoad < 0)
        {
            var extraPower = Math.Abs(_remainingLoad);
            var powerPlantToReduce = powerPlants.Where(p => (p.PowerProduced - extraPower) >= p.Pmin && p.Type != Constants.WIND_TURBINE_TYPE).First();

            powerPlantToReduce.PowerProduced -= extraPower;
        }
    }
}
