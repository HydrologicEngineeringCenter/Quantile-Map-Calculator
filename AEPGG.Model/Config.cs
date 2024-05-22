
using System.Text.RegularExpressions;

namespace AEPGG.Model;

public class Config
{
    /// <summary>
    /// Realization computes aggregate the Max WSEs. Project Level computes need to access the timestep data for each saved AEP from previous realization computes. 
    /// </summary>
    public bool IsRealizationCompute { get; set; }
    /// <summary>
    /// Directory where the results are stored. All .hdf files will be scraped and added to the results histograms. Includes subdirectories. 
    /// </summary>
    public string ResultsDirectory { get; set; }
    /// <summary>
    /// Path and filename for the output file.
    /// </summary>
    public string OutputPath { get; set; }
    /// <summary>
    /// The AEPs which will be pulled from the results histograms. 
    /// </summary>
    public float[] DesiredAEPs { get; set; }
    /// <summary>
    /// Histogram bin width.
    /// </summary>
    public float BinWidth { get; set; }
    /// <summary>
    /// Maximum range of WSEs we expect our histograms to capture. This is ~= the maximum depth we expect to see. It determines number of bins.
    /// </summary>
    public float Range { get; set; }

    /// <summary>
    /// Holds all necessary information for a compute. 
    /// </summary>
    internal Config()
    {
        //Internal because I want people outside deserielizing this object from JSON.
    }
    /// <summary>
    /// Copies the first results file to the output file, Creates histograms of all results files which exist recursively in the ResultsDirectory, and writes the results to the output file at the locations requested.
    /// </summary>
    public void Compute()
    {
        string[] filteredFiles = GetAllResultsFiles();

        //copy the seed file to the output file
        string seedFile = filteredFiles[0];
        File.Copy(seedFile, OutputPath, true);

        //initialize the computer
        RasResultWrapper seedResult = new(seedFile);
        AEPComputer computer = new(seedResult, BinWidth, Range);
        CompileResults(filteredFiles, computer);
        Write(computer);
    }

    private string[] GetAllResultsFiles()
    {
        //get all the results files. Need to use the Regex to avoid a .tmp.hdf sneaking in.
        string stringPattern = "*.p*.hdf";
        var regexPattern = @"^.*\.p\d+\.hdf$";
        var regex = new Regex(regexPattern);
        string[] resultsFiles = Directory.GetFiles(ResultsDirectory, stringPattern, SearchOption.AllDirectories);
        var filteredFiles = resultsFiles.Where(file => regex.IsMatch(file)).ToArray();
        return filteredFiles;
    }

    private void Write(AEPComputer computer)
    {
        AEPResultsWriter writer = new(OutputPath);
        bool success = writer.OverwriteTimeseriesInHDFResults(computer, DesiredAEPs); // .5 = 2yr event, .02 = 50yr event, .04 = 25yr event
        Console.WriteLine(success);
    }

    private void CompileResults(string[] resultsFiles, AEPComputer computer)
    {
        foreach(string resultsfile in resultsFiles)
        {
            RasResultWrapper rasResult = new(resultsfile);
            computer.AddResults(rasResult);
        }
    }
}
