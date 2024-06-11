using AEPGG.Model.Interfaces;

namespace AEPGG.Model;

public class RasResultWrapper : IHydraulicResults
{
    public string FilePath { get; }
    public RasResultWrapper(string hdfFilePath)
    {
        FilePath = hdfFilePath;
    }

    //not making these properties because I don't want to query the mesh names for every result file. They'll all be the same. Want to hand in mesh names.  
    /// <returns>[MeshIndex][CellIndex]</returns>
    public float[][] GetMax2DWSE(string[] meshNames )
    {
        return RasTools.H5ReaderTools.GetMaxOrMinWSEForAll2DCells(FilePath, true, meshNames);
    }
    /// <returns>[MeshIndex][CellIndex]</returns>
    public float[][] GetMin2DWSE(string[] meshNames )
    {
        return RasTools.H5ReaderTools.GetMaxOrMinWSEForAll2DCells(FilePath, false, meshNames);
    }
    public float[] GetMaxXSWSE()
    {
        return RasTools.H5ReaderTools.GetMaxWSEForAllXS(FilePath);
    }
    public float[] GetMinXSWSE()
    {
        return RasTools.H5ReaderTools.GetMinWSEForAllXS(FilePath);
    }
    /// <summary>
    /// Gets the water surface elevations for a specific profile in the results timeseries. 
    /// </summary>
    public float[] GetXSWSE(int profileIndex)
    {
        return RasTools.H5ReaderTools.GetWSEForXSProfile(FilePath,profileIndex);
    }
    /// <summary>
    /// Gets the water surface elevations for a specific profile in the results timeseries.
    /// </summary>
    /// <returns>[MeshIndex][CellIndex]</returns>
    public float[][] Get2DWSE(int profileIndex, string[] meshNames)
    {
        float[][] results = new float[meshNames.Length][];
        for( int i = 0; i < meshNames.Length; i++)
        {
            results[i] = RasTools.H5ReaderTools.GetWSEFor2DProfile(FilePath, meshNames[i], profileIndex);
        }
        return results;
    }
}
