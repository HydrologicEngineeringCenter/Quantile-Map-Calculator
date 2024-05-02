using H5Assist;
using Ras.Layers;

namespace AEPGG.Model.RasTools;

public static class H5ReaderTools
{


    /// <param name="filePath"> An HEC-RAS 6.x HDF file</param>
    /// <param name="getMax"> true gets max, false gets min</param>
    /// <param name="meshNames" 2D Area Names</param>
    /// <returns>WSEs [2D Area Index][Cell Index]</returns>
    public static float[][] GetMaxOrMinWSEForAll2DCells(string filePath, bool getMax, string[] meshNames)
    {
        float[][] WSEs = new float[meshNames.Length][];
        for (int i = 0; i < meshNames.Length; i++)
        {
            WSEs[i] = GetMaxOrMinWSEForAll2DCells(filePath, meshNames[i], getMax);
        }
        return WSEs;
    }


    private static float[] GetMaxOrMinWSEForAll2DCells(string filePath, string meshName, bool getMax)
    {
        string hdfPathToData;
        if (getMax)
        {
            hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MaxWaterSurface.Name(meshName);
        }
        else
        {
            hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MinWaterSurface.Name(meshName);
        }
        return GetRowFromHDF(filePath, hdfPathToData, 0);
    }

    //probably faster to pull this H5 reader up a level and not reopen it very time we read a row. 
    /// <param name="rowID"> typically 0 for Max WSE from MaxWSE records</param>
    private static float[] GetRowFromHDF(string filePath, string hdfPathToData, int rowID = 0)
    {
        float[] dataOut = null;
        using H5Reader hr = new(filePath);
        hr.ReadRow(hdfPathToData, rowID, ref dataOut);
        return dataOut;
    }

    /// <param name="filePath"> must have .hdf extension</param>
    public static float[] GetMaxWSEForAllXS(string filePath)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MaxWaterSurface.Name;
        float[] data = GetRowFromHDF(filePath, hdfPathToData, 0);
        return data;
    }

    /// <param name="filePath"> must have .hdf extension</param>
    public static float[] GetMinWSEForAllXS(string filePath)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MinWaterSurface.Name;
        float[] data = GetRowFromHDF(filePath, hdfPathToData, 0);
        return data;
    }
}
