using System.Threading.Tasks;
using LNCDApp.MainApp.Helpers;
using Microsoft.AspNetCore.Http;

namespace LNCDApp.MainApp.Handlers
{
    public static class VersionHandler
    {
        public static Task HandleAsync(HttpContext ctx)
        {
            ctx.Response.StatusCode = 200;
            return ctx.Response.WriteAsync(VersionHelper.Version);
        }
    }
}
