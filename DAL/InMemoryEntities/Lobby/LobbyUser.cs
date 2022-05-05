using GeneralLibrary.Enums;

namespace DAL.InMemoryEntities.Lobby;

public class LobbyUser
{
    public Guid? Id { get; set; }
    public Role Role { get; set; } = Role.Attack;
    public IsAccepted Accepted { get; set; } = IsAccepted.Empty;
}