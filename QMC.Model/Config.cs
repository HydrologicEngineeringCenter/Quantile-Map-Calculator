using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using QPC.Model.Computers;

namespace QPC.Model;

public class Config
{
    /// <summary>
    /// Realization computes aggregate the Max WSEs. Project Level computes need to access the timestep data for each saved AEP from previous realization computes. 
    /// </summary>
    [JsonPropertyName("isRealizationCompute")]
    public bool IsRealizationCompute { get; set; }
    /// <summary>
    /// Directory where the results are stored. All .hdf files will be scraped and added to the results histograms. Includes subdirectories. 
    /// </summary>
    [JsonPropertyName("resultsDirectory")]
    public string ResultsDirectory { get; set; }
    /// <summary>
    /// Path and filename for the output file.
    /// </summary>
    [JsonPropertyName("outputPath")]
    public string OutputPath { get; set; }
    /// <summary>
    /// The AEPs which will be pulled from the results histograms. If doing a confidence compute, these should be the AEPs recorded in the input results files. 
    /// </summary>
    [JsonPropertyName("desiredAeps")]
    public float[] DesiredAEPs { get; set; }
    /// <summary>
    /// Histogram bin width.
    /// </summary>
    [JsonPropertyName("binWidth")]
    public float BinWidth { get; set; }
    /// <summary>
    /// Maximum range of WSEs we expect our histograms to capture. This is ~= the maximum depth we expect to see. It determines number of bins.
    /// </summary>
    [JsonPropertyName("range")]
    public float Range { get; set; }

    /// <summary>
    /// The desired quatiles for the confidence compute. Not used for a realization compute. OPTIONAL PARAMETER
    /// </summary>
    [JsonPropertyName("desiredQuantiles")]
    public float[] DesiredQuantiles { get; set; }

    /// <summary>
    /// Holds all necessary information for a compute. 
    /// </summary>
    public Config()
    {
    }
    /// <summary>
    /// Copies the first results file to the output file, Creates histograms of all results files which exist recursively in the ResultsDirectory, and writes the results to the output file at the locations requested.
    /// </summary>
    public void Compute()
    {
        string[] filteredFiles = GetAllResultsFiles();
        RasResultWrapper seedResult = new(filteredFiles[0]);
        if (IsRealizationCompute)
        {
            ComputeRealizationResult(filteredFiles, seedResult);
        }
        else
        {
            ComputeConfidenceResult(filteredFiles, seedResult);
        }
    }
    private void ComputeRealizationResult(string[] filteredFiles, RasResultWrapper seedResult)
    {
        //copy the seed file to the output file
        string seedFile = filteredFiles[0];
        File.Copy(seedFile, OutputPath, true);

        AEPComputer computer = new(seedResult, BinWidth, Range);
        CompileResults(filteredFiles, computer);
        WriteRealizationResult(computer);
    }
    private void ComputeConfidenceResult(string[] filteredFiles, RasResultWrapper seedResult)
    {
        for (int i = 0; i < DesiredAEPs.Length; i++)
        {
            ConfidenceComputer computer = new(seedResult, BinWidth, Range, profileOfInterest: i);
            string outputFile = GetConfidenceFileName(DesiredAEPs[i]);
            File.Copy(filteredFiles[0], outputFile, true);
            CompileResults(filteredFiles, computer);
            WriteConfidenceResult(computer, outputFile);
        }
    }

    private string GetConfidenceFileName(float AEP)
    {
        string[] splitString = OutputPath.Split("\\");
        splitString[^1] = "ConfidenceOfAEP" + AEP + ".hdf";
        return string.Join("\\", splitString);
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

    private void WriteRealizationResult(BaseComputer computer)
    {
        AEPResultsWriter writer = new(OutputPath);
        bool _ = writer.OverwriteTimeseriesInHDFResults(computer, DesiredAEPs); // .5 = 2yr event, .02 = 50yr event, .04 = 25yr event
        //TODO: Add a check for success.
    }
    private void WriteConfidenceResult(ConfidenceComputer computer, string outputPath)
    {
        AEPResultsWriter writer = new(outputPath);
        bool _ = writer.OverwriteTimeseriesInHDFResults(computer, DesiredQuantiles);
        //TODO: Add a check for success.
    }
    private static void CompileResults(string[] resultsFiles, BaseComputer computer)
    {
        foreach (string resultsfile in resultsFiles)
        {
            RasResultWrapper rasResult = new(resultsfile);
            computer.AddResults(rasResult);
        }
    }
}
