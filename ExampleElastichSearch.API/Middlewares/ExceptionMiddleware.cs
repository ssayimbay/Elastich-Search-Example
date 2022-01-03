using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ExampleElastichSearch.API.Middlewares
{
    public class ExceptionMiddleware 
    {
        protected readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await GenerateResponseAsync(context, HttpStatusCode.BadRequest,
                    string.IsNullOrEmpty(e.InnerException?.Message) ? e.Message : e.InnerException.Message);
            }
        }

        protected virtual async Task GenerateResponseAsync(HttpContext context, HttpStatusCode statusCode,
            string message)
        {
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(message);
        }
    }
}