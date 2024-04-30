using AEPGG.Model;

//Hard coded to local data. too big to upload to github. 
string lifecycleDirectoryPath = "D:\\AEP Grid\\All2DMuncie\\Muncie_WAT\\runs\\Without_Project_Conditions\\FRA_50yr\\realization 1\\lifecycle 1\\";
string rasFilePath = "RAS\\Muncie.p13.hdf";
string outputFilePath = "D:\\AEP Grid\\munciePartialResult.hdf";
float[] theAEPs = [.99f, .5f, .2f, .1f, .02f];

Project proj = new(.5f, 20);
string[] eventdirs = Directory.GetDirectories(lifecycleDirectoryPath); //returns directories without trailing "\\"

for (int i = 0; i < eventdirs.Length; i++)
{
    string rasFile = eventdirs[i] + "\\" + rasFilePath;
    if (File.Exists(rasFile))
    {
        RasResultWrapper rasResult = new(rasFile);
        proj.AddResults(rasResult);
    }
}

Console.WriteLine("poopy");
Console.ReadLine();