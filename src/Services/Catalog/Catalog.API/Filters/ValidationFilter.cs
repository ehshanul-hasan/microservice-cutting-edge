using Common.Utilities.Extensions;
using Common.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Catalog.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly ILogger<ValidationFilter> _logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
            }
            else
            {
                var data = from kvp in context.ModelState
                           from err in kvp.Value.Errors
                           let k = kvp.Key
                           select new ValidationError(k, string.IsNullOrEmpty(err.ErrorMessage) ? "Invalid Input" : err.ErrorMessage);

                var response = new BadRequestObjectResult(new Result(data, (int)HttpStatusCode.BadRequest, "Entity is not valid"));
                _logger.LogError(context.ActionDescriptor.DisplayName + " - " + string.Join(',', data.Select(s => s.Message).ToList()));
                context.Result = response;
            }

        }
    }
}
