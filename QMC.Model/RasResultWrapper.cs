using QMC.Model.Interfaces;
using QMC.Model.RasTools;

namespace QMC.Model;

public class RasResultWrapper : IHydraulicResults, IDisposable
{
    private H5io _h5ReadWrite;
    public string FilePath => _h5ReadWrite.FilePath;

    public RasResultWrapper(string hdfFilePath)
    {
        _h5ReadWrite = new(hdfFilePath);
    }

    //not making these properties because I don't want to query the mesh names for every result file. They'll all be the same. Want to hand in mesh names.  
    /// <returns>[MeshIndex][CellIndex]</returns>
    public float[][] GetMax2DWSE(string[] meshNames)
    {
        return _h5ReadWrite.GetMaxOrMinWSEForAll2DCells(true, meshNames);
    }
    /// <returns>[MeshIndex][CellIndex]</returns>
    public float[][] GetMin2DWSE(string[] meshNames)
    {
        return _h5ReadWrite.GetMaxOrMinWSEForAll2DCells( false, meshNames);
    }
    public float[] GetMaxXSWSE()
    {
        return _h5ReadWrite.GetMaxWSEForAllXS();
    }
    public float[] GetMinXSWSE()
    {
        return _h5ReadWrite.GetMinWSEForAllXS();
    }
    /// <summary>
    /// Gets the water surface elevations for a specific profile in the results timeseries. 
    /// </summary>
    public float[] GetXSWSE(int profileIndex)
    {
        return _h5ReadWrite.GetWSEForXSProfile( profileIndex);
    }
    /// <summary>
    /// Gets the water surface elevations for a specific profile in the results timeseries.
    /// </summary>
    /// <returns>[MeshIndex][CellIndex]</returns>
    public float[][] Get2DWSE(int profileIndex, string[] meshNames)
    {
        float[][] results = new float[meshNames.Length][];
        for (int i = 0; i < meshNames.Length; i++)
        {
            results[i] = _h5ReadWrite.GetWSEFor2DProfile( meshNames[i], profileIndex);
        }
        return results;
    }

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
    // ~RasResultWrapper()
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
}
