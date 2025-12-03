using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters
{
    public class ApiResponseWrapperFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
                {
                    context.Result = new ObjectResult(new { success = true, data = objectResult.Value })
                    {
                        StatusCode = objectResult.StatusCode
                    };
                }
                else
                {
                    context.Result = new ObjectResult(new { success = false, error = objectResult.Value })
                    {
                        StatusCode = objectResult.StatusCode
                    };
                }
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new { success = true }) { StatusCode = 200 };
            }
        }
    }
}
