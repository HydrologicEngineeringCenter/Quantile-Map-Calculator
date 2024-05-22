using AEPGG.Model.Interfaces;

namespace AEPGG.Model.Computers;

internal class ConfidenceComputer : BaseComputer
{
    public ConfidenceComputer(IHydraulicResults result, float binWidth, float range, IGeometry mockGeometry = null) : base(result, binWidth, range, mockGeometry)
    {
    }

    public override void AddResults2D(IHydraulicResults result)
    {
        throw new NotImplementedException();
    }

    public override void AddResultsXS(IHydraulicResults result)
    {
        throw new NotImplementedException();
    }
}
