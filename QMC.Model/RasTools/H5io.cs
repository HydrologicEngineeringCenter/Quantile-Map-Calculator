using H5Assist;
using RasMapperLib.Names;

namespace QMC.Model.RasTools;

public class H5io : IDisposable
{
    private H5Writer _h5ReadWrite;

    public string FilePath => _h5ReadWrite.Filename;
    public H5io(string filePath)
    {
        _h5ReadWrite = new H5Writer(filePath);
    }

    #region READ
    /// <param name="getMax"> true gets max, false gets min</param>
    /// <param name="meshNames" 2D Area Names</param>
    /// <returns>WSEs [2D Area Index][Cell Index]</returns>
    public float[][] GetMaxOrMinWSEForAll2DCells(bool getMax, string[] meshNames)
    {
        float[][] WSEs = new float[meshNames.Length][];
        for (int i = 0; i < meshNames.Length; i++)
        {
            WSEs[i] = GetMaxOrMinWSEForAll2DCells(meshNames[i], getMax);
        }
        return WSEs;
    }

    public float[] GetMaxWSEForAllXS()
    {
        string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MaxWaterSurface.Name;
        float[] data = GetRowFromHDF(hdfPathToData, 0);
        return data;
    }

    public float[] GetMinWSEForAllXS()
    {
        string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MinWaterSurface.Name;
        float[] data = GetRowFromHDF(hdfPathToData, 0);
        return data;
    }

    private float[] GetMaxOrMinWSEForAll2DCells(string meshName, bool getMax)
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
        return GetRowFromHDF(hdfPathToData, 0);
    }

    /// <param name="rowID"> typically 0 for Max WSE from MaxWSE records</param>
    private float[] GetRowFromHDF(string hdfPathToData, int rowID = 0)
    {
        float[] dataOut = null;
        _h5ReadWrite.ReadRow(hdfPathToData, rowID, ref dataOut);
        return dataOut;
    }

    public float[] GetWSEFor2DProfile(string meshName, int rowID)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.TimeSeriesOutput.FlowAreas.WaterSurface(meshName);
        return GetRowFromHDF(hdfPathToData, rowID);
    }

    public float[] GetWSEForXSProfile(int rowID)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.TimeSeriesOutput.CrossSections.WaterSurface;
        return GetRowFromHDF(hdfPathToData, rowID);
    }
    #endregion

    #region WRITE
    public void OverwriteMaxWSEForAll2DCells(float[][] data, string[] meshNames)
    {
        for (int i = 0; i < meshNames.Length; i++)
        {
            OverwriteMaxWSEForAll2DCells(meshNames[i], data[i]);
        }
    }

    /// <param name="filePath"> must have .hdf extension</param>
    public void OverwriteMaxWSEforAllXs(float[] data)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MaxWaterSurface.Name;
        WriteDataToHDF(hdfPathToData, data, 0);
    }

    /// <summary> Overwrites the water surface elevation in the results timeseries with a single profile for each of the specified meshes </summary>
    /// <param name="data">[2D Area Index][Cell Index]</param>
    public void OverwriteSingleProfile2D(string[] meshNames, float[][] data, int rowID)
    {
        for (int i = 0; i < meshNames.Length; i++)
        {
            OverwriteSingleProfile2D(meshNames[i], data[i], rowID);
        }
    }

    private void WriteDataToHDF(string hdfPathToData, float[] data, int rowID)
    {
        //does this overwrite the row, or just add a row at this position?
        _h5ReadWrite.AddRow(hdfPathToData, data, rowID);
    }

    private void OverwriteMaxWSEForAll2DCells(string meshName, float[] data)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MaxWaterSurface.Name(meshName);
        WriteDataToHDF(hdfPathToData, data, 0); //Max WSE is always at row 0
    }

    /// <summary> Overwrites the water surface elevation in the results timeseries with a single profile for a single mesh </summary>
    /// <param name="data"> [Cell Index] </param>
    private void OverwriteSingleProfile2D(string meshName, float[] data, int rowID)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.TimeSeriesOutput.FlowAreas.WaterSurface(meshName);
        WriteDataToHDF(hdfPathToData, data, rowID);
    }

    /// <summary> Overwrites the water surface elevation in the XS results timeseries with a single profile</summary>
    /// <param name="data"> [XS Index] </param>
    public void OverwriteSingleProfileXS(float[] data, int rowID)
    {
        string hdfPathToData = ResultsDatasets.Unsteady.TimeSeriesOutput.CrossSections.WaterSurface;
        WriteDataToHDF(hdfPathToData, data, rowID);
    }
    #endregion

    #region Dispose
    private bool disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                _h5ReadWrite?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~H5ReaderTools()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}

