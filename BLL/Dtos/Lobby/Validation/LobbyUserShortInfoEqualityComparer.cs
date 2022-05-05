namespace BLL.Dtos.Lobby.Validation;

public class LobbyUserShortInfoEqualityComparer : IEqualityComparer<LobbyUserShortInfo>
{
    public bool Equals(LobbyUserShortInfo? x, LobbyUserShortInfo? y)
    {
        if (x != null && y != null && x.Id != null && y.Id != null)
            return x.Id == y.Id;

        return false;
    }

    public int GetHashCode(LobbyUserShortInfo obj)
    {
        return obj.Id.GetHashCode();
    }
}