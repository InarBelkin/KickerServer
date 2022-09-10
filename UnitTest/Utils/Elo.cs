using GeneralLibrary.Utils;
using Xunit;

namespace UnitTest.Utils;

public class Elo
{
    [Fact]
    public void TestElo()
    {
        int EloA = 1000;
        int EloB = 1000;
        var res1 = Mathematics.CountNewElo(EloA, EloB, true);
        var res2 = Mathematics.CountNewElo(EloA, EloB, true);
    }
}