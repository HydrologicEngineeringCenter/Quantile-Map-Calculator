namespace Scratch.EntryPoints;

public static class Beam
{
    public static void EntryPoint()
    {
        string cliArgs = @"C:\Projects\MoRiv_Stage_Freq\MR_Monte_Carlo_2025-03-07\MR_Monte_Carlo_2025-03-07\ras\MR_Monte_Carlo.p02.hdf -o C:\Projects\test.hdf --overwrite";
        QMC.BatchDataFilter.Program.CallMain(cliArgs);
    }
}
