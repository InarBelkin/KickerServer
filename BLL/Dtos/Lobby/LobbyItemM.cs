using System.ComponentModel.DataAnnotations;
using BLL.Dtos.Lobby.Validation;
using BLL.Dtos.Stats;
using GeneralLibrary.Enums;
using static System.String;

namespace BLL.Dtos.Lobby;

public class LobbyItemM : IValidatableObject
{
    public DateTime? DateStart { get; set; }
    public bool IAmMember { get; set; } = false;
    public string Message { get; set; } = Empty;
    public LobbyUserShortInfo Initiator { get; set; } = new();

    public List<LobbyUserShortInfo> SideA { get; set; } = new();
    public List<LobbyUserShortInfo> SideB { get; set; } = new();


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