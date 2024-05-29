using AEPGG.Model.Interfaces;

namespace AEPGG.Model.Computers;

public class ConfidenceComputer : BaseComputer
{
    public int ProfileOfInterest { set; get; }
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
