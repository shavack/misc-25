using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public static class ErrorHandling
{
    public static WebApplication AddErrorHandling(this WebApplication app)
    {
        app.Use(async (context, next) =>
         {
             try
             {
                 await next();
             }
             catch (ValidationException ex)
             {
                 context.Response.StatusCode = StatusCodes.Status400BadRequest;
                 context.Response.ContentType = "application/json";
                 var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                 await context.Response.WriteAsJsonAsync(errors);
             }
         });
        return app;
    }
}