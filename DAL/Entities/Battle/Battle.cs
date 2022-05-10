using static System.String;

namespace DAL.Entities.Battle;

public class Battle : BaseEntity
{
    public List<User> Users { get; set; } = new();
    public List<UserBattle> UserBattles { get; set; } = new();

    public string Message { get; set; } = Empty;

    public DateTime BattleTime { get; set; }
    public double BattleTimeSeconds { get; set; }


    public bool IsWinnerA { get; set; }
    public int LoserGoalsCount { get; set; }
}