using DAL.Entities.Auth;
using DAL.Entities.Battle;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(Name), IsUnique = true)]
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public StatsOneVsOne? StatsOneVsOne { get; set; }
    public StatsTwoVsTwo? StatsTwoVsTwo { get; set; }
    public List<AuthInfo> AuthInfos { get; set; } = new();

    public List<Battle.Battle> Battles { get; set; } = new();
    public List<UserBattle> UserBattles { get; set; } = new();
}