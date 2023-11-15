using AEPGG.Model;

string lifecycleDirectoryPath = "D:\\AEP Grid\\Muncie_WAT\\runs\\Without_Project_Conditions\\FRA_50yr\\realization 1\\lifecycle 1\\";
string rasFilePath = "\\RAS\\Muncie.p08.hdf";
float[] theAEPs = new float[] { .99f, .5f, .2f, .1f, .02f };

Project proj = new("", theAEPs, .5f, 20);
string[] eventdirs = Directory.GetDirectories(lifecycleDirectoryPath);

for (int i = 0; i < eventdirs.Length; i++)
{
    proj.AddResults(new RasResultWrapper(eventdirs[i] + rasFilePath));
}
Console.WriteLine("poopy");
Console.ReadLine();