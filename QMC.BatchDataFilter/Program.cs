using CommandLine;
using H5Assist;
using QMC.RasTools;
using RasMapperLib.Names;
using System.Diagnostics;
using Utility.Reflection;

namespace QMC.BatchDataFilter;

public class Program
{
    /// <summary>
    /// CLI Entry Point
    /// </summary>
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
          .WithParsed(RunOptions)
          .WithNotParsed(HandleParseError);
    }

    /// <summary>
    /// This exists to give a programmatic way to debug, rather than running from true commandline.
    /// </summary>
    public static void CallMain(string args)
    {
        string[] convertedToCommandLine = Utility.CommandLine.CommandLineHelpers.SplitIntoCLIArgs(args);
        Main(convertedToCommandLine);
    }

    class Options
    {
        [Value(0, MetaName = "input file", HelpText = "This is the file to be filtered. Should be an HEC-RAS 6.x result file (*.hdf).")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "This is the output file to save. Should be a *.hdf")]
        public string OutputFile { get; set; }

        [Option("overwrite", Default = false, Required = false, HelpText = "Overwrite the output file if it exists.")]
        public bool Overwrite { get; set; }

    }

    static void RunOptions(Options opts)
    {
        if (string.IsNullOrWhiteSpace(opts.InputFile))
        {
            Console.WriteLine("Input file is required.");
            return;
        }
        if (string.IsNullOrWhiteSpace(opts.InputFile))
        {
            Console.WriteLine("Output file is required.");
            return;
        }
        if (File.Exists(opts.OutputFile) && !opts.Overwrite)
        {
            Console.WriteLine("Output file already exists. Use --overwrite to overwrite.");
            return;
        }
        Filter(opts.InputFile, opts.OutputFile);
    }
    static void HandleParseError(IEnumerable<Error> errs)
    {
        //handle errors
    }

    static void Filter(string inputFile, string outputFile)
    {
        //get the data we want
        using RasResultWrapper result = new(inputFile);
        RasGeometryWrapper geometry = new(result.FilePath);
        float[][] maxs = result.GetMax2DWSE(geometry.MeshNames);
        float[][] mins = result.GetMin2DWSE(geometry.MeshNames);
        float[] maxXS = result.GetMaxXSWSE();
        float[] minXS = result.GetMinXSWSE();

        //write the data to the output file
        using H5Writer writer = new(outputFile);
        string[] meshNames = geometry.MeshNames;

        //write the max and min 2D WSE if it exists
        if (maxs != null && mins != null)
        {
            for (int i = 0; i < meshNames.Length; i++)
            {

                writer.WriteDataset(ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MaxWaterSurface.Name(meshNames[i]), maxs[i]);
                writer.WriteDataset(ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MinWaterSurface.Name(meshNames[i]), mins[i]);
            }
        }

        //write the max and min XS WSE if it exists
        if (maxXS != null && minXS != null)
        {
            writer.WriteDataset(ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MaxWaterSurface.Name, maxXS);
            writer.WriteDataset(ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MinWaterSurface.Name, minXS);
        }

        writer.Flush();
    }
}