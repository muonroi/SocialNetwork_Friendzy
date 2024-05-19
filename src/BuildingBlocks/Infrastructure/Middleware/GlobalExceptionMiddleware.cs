
namespace Infrastructure.Middleware
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unhandled exception occurred while processing the request.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            var response = new
            {
                error = new
                {
                    message = new ApiErrorResult<string>(),
                    details = ex.Message
                }
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }

}
