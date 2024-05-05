namespace Infrastructure.Commons;

public class ValidationFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            List<string> errors = context.ModelState.Values
             .Where(modelState => modelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
             .SelectMany(modelState => modelState.Errors)
             .Select(error => error.ErrorMessage)
            .ToList();

            ApiResult<object> response = new(false, default!, nameof(StatusCode.InvalidArgument), Languages.vi, (int)StatusCode.InvalidArgument);
            context.Result = new OkObjectResult(response);
        }
    }
}