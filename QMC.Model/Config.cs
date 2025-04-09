using System.Text.Json.Serialization;

namespace QMC.Model;

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
}
