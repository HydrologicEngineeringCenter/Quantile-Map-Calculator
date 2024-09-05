using H5Assist;
using Ras.Layers;

namespace QPC.Model.RasTools;

public class H5WriterTools
{
    public static void OverwriteMaxWSEForAll2DCells(string filePath, float[][] data, string[] meshNames)
    {
        for (int i = 0; i < meshNames.Length; i++)
        {
            OverwriteMaxWSEForAll2DCells(filePath, meshNames[i], data[i]);
        }
    }

    /// <param name="filePath"> must have .hdf extension</param>
    public static void OverwriteMaxWSEforAllXs(string filePath, float[] data)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MaxWaterSurface.Name;
        WriteDataToHDF(filePath, hdfPathToData, data, 0);
    }

    /// <summary> Overwrites the water surface elevation in the results timeseries with a single profile for each of the specified meshes </summary>
    /// <param name="data">[2D Area Index][Cell Index]</param>
    public static void OverwriteSingleProfile2D(string filePath, string[] meshNames, float[][] data, int rowID)
    {
        for (int i = 0; i < meshNames.Length; i++)
        {
            OverwriteSingleProfile2D(filePath, meshNames[i], data[i], rowID);
        }
    }

    private static void WriteDataToHDF(string filePath, string hdfPathToData, float[] data, int rowID)
    {
        using H5Writer hw = new(filePath);
        //does this overwrite the row, or just add a row at this position?
        hw.AddRow(hdfPathToData, data, rowID);
    }

    private static void OverwriteMaxWSEForAll2DCells(string filePath, string meshName, float[] data)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MaxWaterSurface.Name(meshName);
        WriteDataToHDF(filePath, hdfPathToData, data, 0); //Max WSE is always at row 0
    }

    /// <summary> Overwrites the water surface elevation in the results timeseries with a single profile for a single mesh </summary>
    /// <param name="data"> [Cell Index] </param>
    private static void OverwriteSingleProfile2D(string filePath, string meshName, float[] data, int rowID)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.TimeSeriesOutput.FlowAreas.WaterSurface(meshName);
        WriteDataToHDF(filePath, hdfPathToData, data, rowID);
    }

    /// <summary> Overwrites the water surface elevation in the XS results timeseries with a single profile</summary>
    /// <param name="data"> [XS Index] </param>
    public static void OverwriteSingleProfileXS(string filePath, float[] data, int rowID)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.TimeSeriesOutput.CrossSections.WaterSurface;
        WriteDataToHDF(filePath, hdfPathToData, data, rowID);
    }
}
