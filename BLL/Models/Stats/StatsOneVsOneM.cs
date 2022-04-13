using System.Linq.Expressions;
using DAL.Entities;

namespace BLL.Models.Stats;

public class StatsOneVsOneM
{
    public StatsOneVsOneM()
    {
    }

    public StatsOneVsOneM(StatsOneVsOne stats)
    {
        ELO = stats.ELO;
        BattlesCount = stats.BattlesCount;
        WinsCount = stats.WinsCount;
    }


    public int ELO { get; set; }
    public int BattlesCount { get; set; }
    public int WinsCount { get; set; }
}