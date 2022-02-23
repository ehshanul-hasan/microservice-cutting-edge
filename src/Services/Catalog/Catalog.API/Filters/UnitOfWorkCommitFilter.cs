using Catalog.API.Respository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.API.Filters
{
    public class UnitOfWorkCommitFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();

            if (result.Exception == null && context.HttpContext.Request.Method != "GET")
            {
                var unitOfWork = result.HttpContext.RequestServices.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

                if (unitOfWork == null)
                    throw new NullReferenceException();

                await unitOfWork.CommitAsync();
            }
        }
    }
}
