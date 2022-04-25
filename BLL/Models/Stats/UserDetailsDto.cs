using DAL.Entities;
using static System.String;

namespace BLL.Models.Stats;

public class UserDetailsDto
{
    public Guid Id { get; set; }
    public string ImgSource { get; set; } = Empty;

    public string Name { get; set; } = Empty;
    public bool IsMe { get; set; } = false;
    public StatsOneVsOneM StatsOneVsOne { get; set; } = new();
    public StatsTwoVsTwoM StatsTwoVsTwo { get; set; } = new();
}