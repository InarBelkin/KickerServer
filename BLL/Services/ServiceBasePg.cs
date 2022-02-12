using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services;

public abstract class ServiceBasePg
{
    protected HttpContext HttpContext { get; }
    protected KickerContext Db { get; }

    protected ServiceBasePg(IHttpContextAccessor accessor)
    {
        HttpContext = accessor.HttpContext;
        Db = HttpContext.RequestServices.GetService<KickerContext>() ??
             throw new NullReferenceException("DbContext was null");
    }
}