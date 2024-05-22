using AEPGG.Model;

//Hard coded to local data. too big to upload to github. 
//Do this as JSON for acutal computes. 
string lifecycleDirectoryPath = @"D:\AEP Grid\All2DMuncie\Muncie_WAT\runs\Without_Project_Conditions\FRA_50yr\realization 1\lifecycle 1\";
string rasFilePath = "RAS\\Muncie.p13.hdf";
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
config.Compute();
Console.WriteLine("Done");


