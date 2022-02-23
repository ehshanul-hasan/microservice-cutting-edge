using Catalog.API.Respository;
using Common.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.API.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(IUnitOfWork unitOfWork, ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            _unitOfWork.RollBackAsync();

            if (context.Exception is BusinessExceptionBase)
            {
                var exception = (BusinessExceptionBase)context.Exception;
                context.HttpContext.Response.StatusCode = exception.Status;
                context.Result = new ObjectResult(new
                {
                    exception.Status,
                    exception.Message
                })
                {
                    StatusCode = exception.Status
                };
            }
            else
            {
                ObjectResult result;
                result = new ObjectResult(new
                {
                    Status = 500,
                    Message = "Something went wrong",
                    Errors = CollectErrors(context)
                })
                {
                    StatusCode = 500
                };
                context.Result = result;
            }
            return Task.CompletedTask;
        }

        private List<string> CollectErrors(ExceptionContext context)
        {
            List<string> errors = new List<string>();
            Exception? ex = context.Exception;
            while (ex != null)
            {
                _logger.LogError(ex.ToString());
                errors.Add(ex.Message);
                ex = ex.InnerException;
            }
            return errors;
        }
    }
}
