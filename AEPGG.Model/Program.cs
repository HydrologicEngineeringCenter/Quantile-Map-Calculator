using AEPGG.Model;

string lifecycleDirectoryPath = "D:\\AEP Grid\\Muncie_WAT\\runs\\Without_Project_Conditions\\FRA_50yr\\realization 1\\lifecycle 1\\";
string rasFilePath = "\\RAS\\Muncie.p08.hdf";
string outputFilePath = "D:\\AEP Grid\\munciePartialResult.hdf";
float[] theAEPs = new float[] { .99f, .5f, .2f, .1f, .02f };

Project proj = new(outputFilePath, theAEPs, .5f, 20);
string[] eventdirs = Directory.GetDirectories(lifecycleDirectoryPath);

for (int i = 0; i < eventdirs.Length; i++)
{
    string rasFile = eventdirs[i] + rasFilePath;
    if (File.Exists(rasFile))
    {
        proj.AddResults(new RasResultWrapper(rasFile));
    }
}
proj.SaveResults();
Console.WriteLine("poopy");
Console.ReadLine();