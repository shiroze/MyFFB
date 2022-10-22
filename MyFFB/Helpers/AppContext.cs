using Microsoft.AspNetCore.Http;

namespace MyAttd.Helpers
{
    public static class AppContext
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            //_httpContextAccessor = httpContextAccessor.HttpContext.Request.Host.Value;
        }

        public static HttpContext Current => _httpContextAccessor.HttpContext;
    }
}
