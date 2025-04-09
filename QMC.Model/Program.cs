using CommandLine;
using System.Text.Json;
using Utility.Progress;

namespace QMC.Model;

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
        [Value(0, MetaName = "config file",  HelpText = "This is the configuration file. Should be formatted to example spec. in JSON. ")]
        public string ConfigFile { get; set; }
    }

    static void RunOptions(Options opts)
    {
        if (!File.Exists(opts.ConfigFile))
        {
            Console.WriteLine("Input file is required.");
            return;
        }
        Config config = JsonSerializer.Deserialize<Config>(File.ReadAllText(opts.ConfigFile));
        if (config == null)
        {
            Console.WriteLine("Config file is not valid.");
            return;
        }
        ComputeManager computeManager = new(config);
        ProgressReporter progressReporter = ProgressReporter.ConsoleWrite();
        computeManager.Compute();
    }

    static void HandleParseError(IEnumerable<Error> errs)
    {
        //handle errors
    }
}