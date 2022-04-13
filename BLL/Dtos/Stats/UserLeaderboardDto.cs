namespace BLL.Dtos.Stats;

public class UserLeaderboardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Elo { get; set; }
    public int CountOfBattles { get; set; }
    public double WinsCount { get; set; }
    public int Cups { get; set; }
}