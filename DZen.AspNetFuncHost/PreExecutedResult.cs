using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DZen.AspNetFuncHost
{
    public class PreExecutedResult : IActionResult
    {
        private readonly HttpContext _executedContext;

        public PreExecutedResult(HttpContext executedContext)
        {
            _executedContext = executedContext;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = _executedContext.Response.StatusCode;
            foreach (var header in _executedContext.Response.Headers)
            {
                context.HttpContext.Response.Headers.Append(header.Key, header.Value);
            }
            _executedContext.Response.Body.Seek(0, SeekOrigin.Begin);
            await _executedContext.Response.Body.CopyToAsync(context.HttpContext.Response.Body);
        }
    }
}
