using DAL.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(Name), IsUnique = true)]
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public StatsOneVsOne? StatsOneVsOne { get; set; }
    public StatsTwoVsTwo? StatsTwoVsTwo { get; set; }
    public List<AuthInfo> AuthInfos { get; set; } = new();
}