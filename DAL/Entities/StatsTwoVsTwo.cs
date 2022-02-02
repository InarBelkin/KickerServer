using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class StatsTwoVsTwo : BaseEntity
{
    public Guid UserId { get; set; }
    [ForeignKey("UserId")] public User? User { get; set; }

    public int ELO { get; set; }
    public int BattlesCountInAttack { get; set; }
    public int WinsCountInAttack { get; set; }
    public int BattlesCountInDefense { get; set; }
    public int WinsCountInDefense { get; set; }
}