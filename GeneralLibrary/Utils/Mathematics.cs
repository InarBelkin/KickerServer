namespace GeneralLibrary.Utils;

public static class Mathematics
{
    public static int CountNewElo(double Ra, double Rb, bool isWinner)
    {
        double Sa = isWinner ? 1 : 0;
        const int K = 40;
        var Ea = 1.0 / (1.0 + Math.Pow(10, (Rb - Ra) / 400.0));
        var RnewA = Ra + K * (Sa - Ea);
        return (int) RnewA;
    }
}