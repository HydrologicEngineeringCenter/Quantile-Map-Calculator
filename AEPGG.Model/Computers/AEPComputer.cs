using AEPGG.Model.Interfaces;
using RasMapperLib;

namespace AEPGG.Model.Computers;

public class AEPComputer
{
    /// <summary>
    /// Contain results for [2D area index][each cell index].
    /// </summary>
    public Histogram[][] Histograms2DAreas { get; private set; }
    /// <summary>
    /// Contains results for [XS index]
    /// </summary>
    public Histogram[] HistogramsXS { get; private set; }

    /// <summary>
    /// The geometry basically provides the information about the model necessary to know how many histogram arrays to set up, and where that data is saved in the results.
    /// contains the geometry of the model which does not change between results in a single compute.
    /// setter remains public for testing purposes.
    /// </summary>
    public IGeometry Geometry { get; set; }

    /// <summary>
    /// <cref name="AEPComputer"/> sets up arrays of histograms to store results.
    /// </summary>
    /// <param name="result"> this is a seed result, perhaps the first result from the compute, that will be used to intialize the histograms</param>
    /// <param name="binWidth"> this is the bin width of the histograms. this roughly equates to the resolution we'll be storing our results.</param>
    /// <param name="range"> this is the maximum range of WSEs we expect our histograms to capture. This is approximately the maximum depth we expect to see. It determines number of bins </param>
    public AEPComputer(IHydraulicResults result, float binWidth, float range, bool isRealizationCompute, IGeometry mockGeometry = null)
    {
        if (mockGeometry != null)
        {
            Geometry = mockGeometry;
        }
        else
        {
            Geometry = new RasGeometryWrapper(result.FilePath);
        }
        InitializeHistograms(result, range, binWidth);
    }

    public void AddResults(IHydraulicResults result)
    {
        if (Geometry.Has2Ds)
        {
            AddResults2D(result);
        }
        if (Geometry.HasXSs)
        {
            AddResultsXS(result);
        }
        if (Geometry.HasSAs)
        {
            throw new NotImplementedException("Haven't bothered with SA's yet");
        }
    }
    public void AddResultsXS(IHydraulicResults result)
    {
        float[] data = result.GetMaxXSWSE();
        for (int i = 0; i < data.Length; i++) //for each XS
        {
            HistogramsXS[i].AddValue(data[i]);
        }
    }
    public void AddResults2D(IHydraulicResults result)
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

    private void InitializeHistograms(IHydraulicResults result, float range, float binWidth)
    {
        if (Geometry.Has2Ds)
        {
            InitializeHistograms2D(result, range, binWidth);
        }
        if (Geometry.HasXSs)
        {
            InitializeHistogramsXS(result, range, binWidth);
        }
        if (Geometry.HasSAs)
        {
            throw new NotImplementedException("Haven't bothered with SA's yet");
        }
    }

    /// <summary>
    /// Histograms initialize based on the minimum cell elevations in the first result, which are will be consistent across all results, the range of values expected for the WSE in a cell, and the bin width.
    /// </summary>
    private void InitializeHistograms2D(IHydraulicResults result, float range, float binWidth)
    {
        float[][] data = result.GetMin2DWSE(Geometry.MeshNames);
        Histograms2DAreas = new Histogram[data.Length][];
        for (int i = 0; i < data.Length; i++) //for each 2D area
        {
            Histogram[] specificAreaHistograms = new Histogram[data[i].Length];
            for (int j = 0; j < data[i].Length; j++) //for each cell in the 2D area
            {
                float min = data[i][j];
                float max = min + range;
                specificAreaHistograms[j] = new(binWidth, min, max);
            }
            Histograms2DAreas[i] = specificAreaHistograms;
        }
    }

    /// <summary>
    /// Histograms initialize based on the minimum cell elevations in the first result, which are will be consistent across all results, the range of values expected for the WSE in a cell, and the bin width.
    /// </summary>
    private void InitializeHistogramsXS(IHydraulicResults result, float range, float binWidth)
    {
        float[] data = result.GetMinXSWSE();
        HistogramsXS = new Histogram[data.Length];
        for (int i = 0; i < data.Length; i++) //for each XS
        {
            float min = data[i];
            float max = min + range;
            HistogramsXS[i] = new Histogram(binWidth, min, max);
        }
    }

    /// <param name="aep"> an annual Exceedence Probability </param>
    /// <returns> returns the WSE for each cell in each 2D area for the specified AEP. [2D Area Index][Cell Index]</returns>
    public float[][] GetResultsForAEP2D(float aep)
    {
        float[][] resultsForAEP = new float[Histograms2DAreas.Length][];
        for (int i = 0; i < Histograms2DAreas.Length; i++) //for each 2D area
        {
            resultsForAEP[i] = new float[Histograms2DAreas[i].Length];
            for (int j = 0; j < Histograms2DAreas[i].Length; j++) //for each cell in the 2D area
            {
                resultsForAEP[i][j] = Histograms2DAreas[i][j].InverseCDF(aep);
            }
        }
        return resultsForAEP;
    }

    /// <param name="aep"> an annual Exceedence Probability </param>
    /// <returns> returns the WSE for each cell in each 2D area for the specified AEP. [XS Index]</returns>
    public float[] GetResultsForAEPXS(float aep)
    {
        float[] resultsForAEP = new float[HistogramsXS.Length];
        for (int i = 0; i < HistogramsXS.Length; i++) //for each cell in the 2D area
        {
            resultsForAEP[i] = HistogramsXS[i].InverseCDF(aep);
        }

        return resultsForAEP;
    }

}
