using QMC.Model;
using System.Text.Json;

namespace Scratch.EntryPoints;

public static class Beam
{
    public static void EntryPoint()
    {
        string cliArgs = @"C:\Projects\MoRiv_Stage_Freq\MR_Monte_Carlo_2025-03-07\MR_Monte_Carlo_2025-03-07\ras\MR_Monte_Carlo.p02.hdf -o C:\Projects\test.hdf --overwrite";
        QMC.DataFilter.Program.CallMain(cliArgs);
    }

    //This is just creating a JSON string to pass into the entry point. In a real compute, this string would be written by the user outside the library. 
    static string ScriptSetup()
    {
        //Hard coded to local data. too big to upload to github. 
        //Do this as JSON for acutal computes. 
        string lifecycleDirectoryPath = @"D:\AEP Grid\All2DMuncie\Muncie_WAT\runs\Without_Project_Conditions\FRA_50yr\realization 1\lifecycle 1\";
        string outputFilePath = "D:\\AEP Grid\\muncieAll2D_50_WriteMultipleAEPS.hdf";
        float[] theAEPs = [.99f, .5f, .2f, .1f, .02f];
        Config config = new()
        {
            ResultsDirectory = lifecycleDirectoryPath,
            OutputPath = outputFilePath,
            DesiredAEPs = theAEPs,
            IsRealizationCompute = true,
            BinWidth = 0.1f,
            Range = 20f
        };
        string jsonString = JsonSerializer.Serialize(config);
        return jsonString;
    }
}
