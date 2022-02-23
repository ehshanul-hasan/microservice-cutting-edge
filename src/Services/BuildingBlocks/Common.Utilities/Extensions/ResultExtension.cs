using Microsoft.AspNetCore.Mvc;

namespace Common.Utilities.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToResult<T>(this T model, int status = 200, string? message = default)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            return new Result(model, status, message);
        }

        public static ActionResult ToOkResult<T>(this T model, string? message = default, int status = 200)
        {
            return new OkObjectResult(model.ToResult(status, message));
        }

        public static ActionResult ToCreatedResult<T>(this T value, string location = "", string? message = default, int status = 201)
        {
            return new CreatedResult(location, value.ToResult(status, message));
        }

        public static ActionResult ToCreatedAtActionResult<T>(this T value, string actionName, string controllerName, object routeValues, string? message = default, int status = 201)
        {
            return new CreatedAtActionResult(actionName, controllerName, routeValues, value.ToResult(status, message));
        }
    }


    public interface IResult
    {
        int Status { get; set; }
        string Message { get; set; }
        object Data { get; set; }
    }

    public class Result : IResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public Result(object data, int status = 200, string? message = default)
        {
            Data = data;
            Status = status;
            Message = message ?? string.Empty;
        }
    }
}
