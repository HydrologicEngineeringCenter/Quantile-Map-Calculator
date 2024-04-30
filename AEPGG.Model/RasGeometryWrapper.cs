using RasMapperLib;

namespace AEPGG.Model;

public class RasGeometryWrapper
{
    public readonly RASGeometry Geometry;
    public bool HasXSs => RasTools.ContainsXS(Geometry);
    public bool HasSAs => RasTools.ContainsSA(Geometry);
    public bool Has2Ds => RasTools.Contains2D(Geometry);
    public string[] MeshNames => RasTools.GetMeshNames(Geometry);

    public RasGeometryWrapper(string filename)
    {
        RASResults result = new(filename);
        Geometry = result.Geometry;
    }
}
