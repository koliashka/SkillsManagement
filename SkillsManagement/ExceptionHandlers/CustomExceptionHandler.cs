using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace SkillsManagement.ExceptionHandlers
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellation)
        {
            var data = exception switch
            {
                PersonNotFoundException => new { exception.Message, StatusCode = 404 },
                InvalidPersonDataException => new { exception.Message, StatusCode = 400 },
                PersonOperationException => new { exception.Message, StatusCode = 500 },
                _ => new { Message = "An unknown error has occured", StatusCode = 500 }
                
             };

            context.Response.StatusCode = data.StatusCode;
            await context.Response.WriteAsJsonAsync(new { data.Message }, cancellation);
            return true;

                       
            //switch (exception)
            //{
            //    case PersonNotFoundException _:
            //        var personNotFoundError = new { exception.Message };
            //        context.Response.StatusCode = 404;
            //        await context.Response.WriteAsJsonAsync(personNotFoundError, cancellation);
            //        return true;

            //    case InvalidPersonDataException:
            //        var invalidDataError = new { exception.Message };
            //        context.Response.StatusCode = 400;
            //        await context.Response.WriteAsJsonAsync(invalidDataError, cancellation);
            //        return true;

            //    case PersonOperationException:
            //        var personOperationError = new { exception.Message };
            //        context.Response.StatusCode = 500;
            //        await context.Response.WriteAsJsonAsync(personOperationError, cancellation);
            //        return true;


            //    default:
            //        var error = new { Message = "An unknown error has occured" };
            //        context.Response.StatusCode = 500;
            //        await context.Response.WriteAsJsonAsync(error, cancellation);
            //        return true;
            //}
         }
        
    }
}
