using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace RentFlex.Web.Filters;

public class CacheResourceFilter : IResourceFilter
{
    private readonly IMemoryCache _cache;
    private readonly int _cacheDurationInMinutes = 5;

    public CacheResourceFilter(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var cacheKey = context.HttpContext.Request.Path.ToString();

        if (_cache.TryGetValue(cacheKey, out var cachedResponse))
        {
            context.Result = (IActionResult)cachedResponse!;
            return;
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        var cacheKey = context.HttpContext.Request.Path.ToString();
        _cache.Set(cacheKey, context.Result, TimeSpan.FromMinutes(_cacheDurationInMinutes));
    }
}
