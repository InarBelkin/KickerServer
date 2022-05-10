using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Battle;

public class UserBattle
{
    public Guid UserId { get; set; }
    [ForeignKey("UserId")] public User? User { get; set; }

    public Guid BattleId { get; set; }
    [ForeignKey("UserId")] public Entities.Battle.Battle? Battle { get; set; }

    public int Side { get; set; }
    public int Role { get; set; }

    public bool IsInitiator { get; set; }
}