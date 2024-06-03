using AEPGG.Model.Interfaces;

namespace AEPGG.Model.Computers;

public abstract class BaseComputer
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

    public BaseComputer(IHydraulicResults result, float binWidth, float range, IGeometry mockGeometry = null)
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
    public abstract void AddResultsXS(IHydraulicResults result);
    public abstract void AddResults2D(IHydraulicResults result);

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
