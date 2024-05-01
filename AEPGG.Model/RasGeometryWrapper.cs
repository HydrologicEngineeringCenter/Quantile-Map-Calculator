using RasMapperLib;

namespace AEPGG.Model;

public class RasGeometryWrapper
{
    public bool HasXSs {get;}
    public bool HasSAs { get; }
    public bool Has2Ds { get; }
    public string[] MeshNames { get; }
    public RasGeometryWrapper(string filename)
    {
        RASResults result = new(filename);
        RASGeometry Geometry = result.Geometry;
        HasXSs = RasTools.ContainsXS(Geometry);
        HasSAs = RasTools.ContainsSA(Geometry);
        Has2Ds = RasTools.Contains2D(Geometry);
        if(Has2Ds)
        {
            MeshNames = RasTools.GetMeshNames(Geometry);
        }
    }
}
