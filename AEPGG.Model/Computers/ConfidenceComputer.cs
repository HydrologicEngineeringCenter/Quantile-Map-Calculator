using AEPGG.Model.Interfaces;

namespace AEPGG.Model.Computers;

public class ConfidenceComputer : BaseComputer
{
    public int ProfileOfInterest { set; get; }

    /// <summary>
    /// This computer performs the second loop of the AEP compute, creating a series of grids which show the uncertainty of a given AEP at desired quantiles. 
    /// </summary>
    /// <param name="result">A seed result with the proper geometry for the compute.</param>
    /// <param name="binWidth">~resolution of the results</param>
    /// <param name="range"> the range of expected WSEs from dry cell to max WSE</param>
    /// <param name="profileOfInterest"> the index of the AEP for which to compute confidence. Should relate to the index from the list of AEPs the AEPComputer calculated in the first loop.</param>
    /// <param name="mockGeometry"> ONLY USED FOR UNIT TESTING</param>
    public ConfidenceComputer(IHydraulicResults result, float binWidth, float range, int profileOfInterest, IGeometry mockGeometry = null) : base(result, binWidth, range, mockGeometry) 
    {
        ProfileOfInterest = profileOfInterest;
    }


    public override void AddResults2D(IHydraulicResults result)
    {
        float[][] data = result.Get2DWSE(ProfileOfInterest, Geometry.MeshNames);
        for (int i = 0; i < data.Length; i++) //for each mesh
        {
            for (int j = 0; j < data[i].Length; j++) //for each cell in the mesh
            {
                Histograms2DAreas[i][j].AddValue(data[i][j]);
            }
        }
    }

    public override void AddResultsXS(IHydraulicResults result)
    {
        float[] data = result.GetXSWSE(ProfileOfInterest);
        for (int i = 0; i < data.Length; i++) //for each XS
        {
            HistogramsXS[i].AddValue(data[i]);
        }
    }
}
