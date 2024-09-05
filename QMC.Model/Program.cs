using QPC.Model;
using System.Text.Json;

string jsonString = ScriptSetup();
EntryPoint(jsonString);

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

//Imagining this would be the whole Program file entry point if this were published as a console app. 
static void EntryPoint(string jsonString)
{
    Config jsonConfig;
    try
    {
        jsonConfig = JsonSerializer.Deserialize<Config>(jsonString);
    }
    catch (JsonException e)
    {
        Console.WriteLine("Failed to deserialize");
        return;
    }
    jsonConfig.Compute();
}