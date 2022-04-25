using BLL.Models.Stats;

namespace BLL.Dtos.Stats;

public class MyPageDto
{
    public UserDetailsDto UserDetails { get; set; } = new UserDetailsDto();
}