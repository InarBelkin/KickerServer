using DAL.Entities.Auth;

namespace DAL.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public StatsOneVsOne? StatsOneVsOne { get; set; }
    public StatsTwoVsTwo? StatsTwoVsTwo { get; set; }
    public IEnumerable<AuthInfo> AuthInfos { get; set; } = new List<AuthInfo>();    //Какой тип будет на месте ienumberable? 
}