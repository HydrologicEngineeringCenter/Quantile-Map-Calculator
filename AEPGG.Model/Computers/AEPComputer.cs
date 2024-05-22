using AEPGG.Model.Interfaces;
using RasMapperLib;

namespace AEPGG.Model.Computers;

internal class AEPComputer:BaseComputer
{
    /// <summary>
    /// <cref name="AEPComputer"/> sets up arrays of histograms to store results.
    /// </summary>
    /// <param name="result"> this is a seed result, perhaps the first result from the compute, that will be used to intialize the histograms</param>
    /// <param name="binWidth"> this is the bin width of the histograms. this roughly equates to the resolution we'll be storing our results.</param>
    /// <param name="range"> this is the maximum range of WSEs we expect our histograms to capture. This is approximately the maximum depth we expect to see. It determines number of bins </param>
    public AEPComputer(IHydraulicResults result, float binWidth, float range, IGeometry mockGeometry = null):base(result,binWidth,range,mockGeometry)
    {
        //base class sets up the histograms in constructor
    }

    public override void AddResultsXS(IHydraulicResults result)
    {
        float[] data = result.GetMaxXSWSE();
        for (int i = 0; i < data.Length; i++) //for each XS
        {
            HistogramsXS[i].AddValue(data[i]);
        }
    }
    public override void AddResults2D(IHydraulicResults result)
    {
        float[][] data = result.GetMax2DWSE(Geometry.MeshNames);
        for (int i = 0; i < data.Length; i++)//for each 2D area
        {
            for (int j = 0; j < data[i].Length; j++) //for each cell in the 2D area
            {
                Histograms2DAreas[i][j].AddValue(data[i][j]);
            }
        }
    }
}
