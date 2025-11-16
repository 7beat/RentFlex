using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RentFlex.Web.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        string message;
        HttpStatusCode statusCode;

        switch (context.Exception)
        {
            case ArgumentNullException:
                message = "Required data was missing. Please check your input.";
                statusCode = HttpStatusCode.BadRequest;
                break;
            default:
                message = "An unknown error occured.";
                statusCode = HttpStatusCode.InternalServerError;
                break;
        }

        context.Result = new ViewResult
        {
            ViewName = "CustomError",
            ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            {
                ["Message"] = message,
                ["StatusCode"] = (int)statusCode
            }
        };

        context.ExceptionHandled = true;
    }
}
