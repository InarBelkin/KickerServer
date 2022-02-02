using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class StatsOneVsOne : BaseEntity
{
    public Guid UserId { get; set; }
    [ForeignKey("UserId")] public User? User { get; set; }

    public int ELO { get; set; }
    public int BattlesCount { get; set; }
    public int WinsCount { get; set; }
}