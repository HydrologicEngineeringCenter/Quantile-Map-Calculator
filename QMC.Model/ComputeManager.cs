using QMC.Model.Computers;
using QMC.RasTools;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Utility.Progress;

namespace QMC.Model;

internal class ComputeManager
{
    private readonly Config _config;
    public ComputeManager(Config config)
    {
        _config = config;
    }
    /// <summary>
    /// Copies the first results file to the output file, Creates histograms of all results files which exist recursively in the ResultsDirectory, and writes the results to the output file at the locations requested.
    /// </summary>
    public void Compute(ProgressReporter pr = null)
    {
        if (pr == null)
        {
            pr = ProgressReporter.None();
        }
        string[] filteredFiles = GetAllResultsFiles(pr);
        RasResultWrapper seedResult = new(filteredFiles[0]);
        if (_config.IsRealizationCompute)
        {
            ComputeRealizationResult(filteredFiles, seedResult, pr);
        }
        else
        {
            ComputeConfidenceResult(filteredFiles, seedResult, pr);
        }
    }
    private void ComputeRealizationResult(string[] filteredFiles, RasResultWrapper seedResult, ProgressReporter pr)
    {
        pr.ReportMessage("Staring Realization Compute.");
        var sw = Stopwatch.StartNew();
        //copy the seed file to the output file
        string seedFile = filteredFiles[0];
        File.Copy(seedFile, _config.OutputPath, true);

        AEPComputer computer = new(seedResult, _config.BinWidth, _config.Range);
        CompileResults(filteredFiles, computer,pr);
        WriteRealizationResult(computer, pr);
        pr.ReportMessage("Finished Realization Compute in " + sw.ElapsedMilliseconds.ToString());   
    }
    private void ComputeConfidenceResult(string[] filteredFiles, RasResultWrapper seedResult, ProgressReporter pr)
    {
        for (int i = 0; i < _config.DesiredAEPs.Length; i++)
        {
            ConfidenceComputer computer = new(seedResult, _config.BinWidth, _config.Range, profileOfInterest: i);
            string outputFile = GetConfidenceFileName(_config.DesiredAEPs[i]);
            File.Copy(filteredFiles[0], outputFile, true);
            CompileResults(filteredFiles, computer, pr);
            WriteConfidenceResult(computer, outputFile, pr);
        }
    }

    private string GetConfidenceFileName(float AEP)
    {
        string[] splitString = _config.OutputPath.Split("\\");
        splitString[^1] = "ConfidenceOfAEP" + AEP + ".hdf";
        return string.Join("\\", splitString);
    }
    private string[] GetAllResultsFiles(ProgressReporter pr)
    {
        pr.ReportMessage("Getting all results files in " + _config.ResultsDirectory);
        var sw = Stopwatch.StartNew();

        //get all the results files. Need to use the Regex to avoid a .tmp.hdf sneaking in.
        string stringPattern = "*.p*.hdf";
        var regexPattern = @"^.*\.p\d+\.hdf$";
        var regex = new Regex(regexPattern);
        string[] resultsFiles = Directory.GetFiles(_config.ResultsDirectory, stringPattern, SearchOption.AllDirectories);
        var filteredFiles = resultsFiles.Where(file => regex.IsMatch(file)).ToArray();

        pr.ReportMessage("Found " + filteredFiles.Length + " results files in " + _config.ResultsDirectory + "after " + sw.ElapsedMilliseconds.ToString());
        sw.Stop();
        return filteredFiles;
    }

    private void WriteRealizationResult(BaseComputer computer, ProgressReporter pr)
    {
        pr.ReportMessage("Writing results to " + _config.OutputPath);
        QMCResultsFileWriter writer = new(_config.OutputPath);
        bool _ = writer.OverwriteTimeseriesInHDFResults(computer, _config.DesiredAEPs); // .5 = 2yr event, .02 = 50yr event, .04 = 25yr event
                                                                                //TODO: Add a check for success.
    }
    private void WriteConfidenceResult(ConfidenceComputer computer, string outputPath, ProgressReporter pr)
    {
        pr.ReportMessage("Writing results to " + outputPath);
        QMCResultsFileWriter writer = new(outputPath);
        bool _ = writer.OverwriteTimeseriesInHDFResults(computer, _config.DesiredQuantiles);
        //TODO: Add a check for success.
    }
    private static void CompileResults(string[] resultsFiles, BaseComputer computer, ProgressReporter pr)
    {
        int resultCount = resultsFiles.Length;
        var sw = Stopwatch.StartNew();
        pr.ReportMessage("Compiling results from " + resultCount + " files.");

        for( int i = 0; i< resultCount; i++)
        {
            RasResultWrapper rasResult = new(resultsFiles[i]);
            computer.AddResults(rasResult);
            pr.ReportProgressFraction(i, resultCount);
        }
        pr.ReportMessage("Finished compiling in " + sw.ElapsedMilliseconds.ToString());
        sw.Stop();
    }
}
