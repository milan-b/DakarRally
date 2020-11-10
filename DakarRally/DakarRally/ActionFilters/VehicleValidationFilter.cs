using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;


namespace DakarRally.ActionFilters
{
    public class VehicleValidationFilter : IActionFilter
    {

        private IRepositoryWrapper _repository;

        public VehicleValidationFilter(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var vehicleDTO = context.ActionArguments.SingleOrDefault(o => o.Value is VehicleDTO).Value as VehicleDTO;
            if (vehicleDTO == null)
            {
                context.Result = new BadRequestObjectResult("You need to provide vehicle information.");
                return;
            }
            if (!_repository.Race.FindAll().Any(o => o.Id == vehicleDTO.RaceId))
            {
                context.Result = new BadRequestObjectResult($"Race with id:{vehicleDTO.RaceId} does not exist.");
                return;
            }
            if (_repository.Simulation.FindAll().Any(o => o.RaceId == vehicleDTO.RaceId))
            {
                context.Result = new BadRequestObjectResult("Vehicle can be added only to races that are not started.");
                return;
            }
            var vehicleTypes = _repository.VehicleType.FindAll().Select(o => o.Name);
            if (!vehicleTypes.Any(n => n == vehicleDTO.VehicleType))
            {
                context.Result = new BadRequestObjectResult($"Bad vehicle type! \nAvailable vehicle types are:\n\n{vehicleTypes.Join(",\n")}");
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
