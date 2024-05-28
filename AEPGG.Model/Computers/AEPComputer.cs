using AEPGG.Model.Interfaces;

namespace AEPGG.Model.Computers;

public class AEPComputer: BaseComputer
{
    public AEPComputer(IHydraulicResults result, float binWidth, float range, bool isRealizationCompute, IGeometry mockGeometry = null):base(result, binWidth, range, mockGeometry){}

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
