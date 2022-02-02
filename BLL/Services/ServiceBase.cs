using Microsoft.AspNetCore.Http;

namespace BLL.Services;

public abstract class ServiceBase
{
    protected readonly IHttpContextAccessor _accessor;

    protected ServiceBase(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
}