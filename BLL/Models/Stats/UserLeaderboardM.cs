namespace BLL.Models.Stats;

public class UserLeaderboardM
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Elo { get; set; }
    public int CountOfBattles { get; set; }
    public double WinRate { get; set; }
    public int Cups { get; set; }
}