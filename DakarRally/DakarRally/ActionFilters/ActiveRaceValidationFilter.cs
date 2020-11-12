using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;


namespace DakarRally.ActionFilters
{
    public class ActiveRaceValidationFilter : IActionFilter
    {

        private IRepositoryWrapper _repository;

        public ActiveRaceValidationFilter(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var simulation = _repository.Simulation.FindByCondition(o => o.EndTime == null).FirstOrDefault();
            if (simulation == null)
            {
                context.Result = new BadRequestObjectResult("There is no race that is running.");
            }
            else
            {
                context.HttpContext.Items.Add("simulation", simulation);
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
