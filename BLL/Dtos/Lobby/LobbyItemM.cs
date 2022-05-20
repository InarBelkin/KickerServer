using System.ComponentModel.DataAnnotations;
using BLL.Dtos.Lobby.Validation;
using BLL.Dtos.Stats;
using DAL.Entities.Battle;
using GeneralLibrary.Enums;
using static System.String;

namespace BLL.Dtos.Lobby;

public class LobbyItemM : IValidatableObject
{
    public LobbyItemM()
    {
    }

    public LobbyItemM(Battle battle)
    {
        Message = battle.Message;
        var uInitiator = battle.UserBattles.FirstOrDefault(ub => ub.IsInitiator)!.User;
        if (uInitiator != null) Initiator = new(uInitiator);
        SideA = battle.UserBattles.Where(ub => ub.Side == 0)
            .Select(ub => new LobbyUserShortInfo(ub.User!)).ToList();
        SideB = battle.UserBattles.Where(ub => ub.Side == 1)
            .Select(ub => new LobbyUserShortInfo(ub.User!)).ToList();

        TimeStamps = new LobbyTimeStampM[]
        {
            new()
            {
                BattleState = BattleStatus.Ended, BattleTime = battle.BattleTimeSeconds, GlobalTime = battle.BattleTime
            }
        };
        Result = new() {IsWinnerA = battle.IsWinnerA, CountOfGoalsLoser = battle.LoserGoalsCount};
    }

    public LobbyTimeStampM[] TimeStamps { get; set; } = Array.Empty<LobbyTimeStampM>();

    public LobbyResultM Result { get; set; } = new();

    public DateTime? DateStart { get; set; }
    public bool IAmMember { get; set; } = false;
    public string Message { get; set; } = Empty;
    public LobbyUserShortInfo Initiator { get; set; } = new();

    public List<LobbyUserShortInfo> SideA { get; set; } = new();
    public List<LobbyUserShortInfo> SideB { get; set; } = new();


    public IEnumerable<LobbyUserShortInfo> GetAllUsers() => SideA.Concat(SideB).Concat(new[] {Initiator});

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();
        var sides = SideA.Concat(SideB)
            .Where(u => u.Accepted is IsAccepted.Accepted or IsAccepted.Invited);
        if (sides.Distinct(new LobbyUserShortInfoEqualityComparer()).Count() != sides.Count())
        {
            errors.Add(new ValidationResult("Invited and accepted users must be unique"));
        }

        return errors;
    }
}