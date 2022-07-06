using LncdApp.MainApp.Helpers;
using Microsoft.AspNetCore.Http;

namespace LncdApp.MainApp.Handlers;

public static class VersionHandler
{
    public static Task HandleAsync(HttpContext ctx)
    {
        ctx.Response.StatusCode = 200;
        return ctx.Response.WriteAsync(VersionHelper.Version);
    }
}
