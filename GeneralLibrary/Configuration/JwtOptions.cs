using System.Text;
using Microsoft.IdentityModel.Tokens;
using static System.String;

namespace GeneralLibrary.Configuration;

public record JwtOptions()
{
    public bool ValidateIssuer { get; init; }
    public string Issuer { get; init; } = Empty;
    public bool ValidateAudience { get; init; }
    public string Audience { get; init; } = Empty;
    public bool ValidateLifetime { get; init; }
    public string Key { get; init; } = Empty;

    public SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
}