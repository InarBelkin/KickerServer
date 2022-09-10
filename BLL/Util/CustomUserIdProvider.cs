using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace BLL.Util;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirst(ClaimTypes.Sid)?.Value;
    }
}