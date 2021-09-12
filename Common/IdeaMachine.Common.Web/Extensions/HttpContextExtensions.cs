using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace IdeaMachine.Common.Web.Extensions
{
    public static class HttpContextExtensions
    {
	    public static async Task<string> ReadBodyAsStringAsync(this HttpContext httpContext)
	    {
		    return await httpContext.Request.Body.ReadAsStringAsync();
	    }
    }
}
