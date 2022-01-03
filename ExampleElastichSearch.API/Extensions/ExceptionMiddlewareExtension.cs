using ExampleElastichSearch.API.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ExampleElastichSearch.API.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseMemberApiExceptionMiddleware(this IApplicationBuilder builder) =>
            builder.UseMiddleware<ExceptionMiddleware>();
    }
}