using QPC.Model.Interfaces;
using QPC.Model.RasTools;
using RasMapperLib;

namespace QPC.Model;
/// <summary>
/// This class stores the important bits of a RAS Geometry file, so we only have to dig into it once. 
/// </summary>
public class RasGeometryWrapper : IGeometry
{
    public bool HasXSs { get; }
    public bool HasSAs { get; }
    public bool Has2Ds { get; }
    public string[] MeshNames { get; }
    public RasGeometryWrapper(string filename)
    {
        RASResults result = new(filename);
        if (!result.RanSuccessfully)  //RASResults always constructs, even with a fake file. Really I just want to see that it loaded properly.
        {
            throw new Exception("invalid results file");
        }
        RASGeometry Geometry = result.Geometry;
        HasXSs = RASResultsTools.ContainsXS(Geometry);
        HasSAs = RASResultsTools.ContainsSA(Geometry);
        Has2Ds = RASResultsTools.Contains2D(Geometry);
        if (Has2Ds)
        {
            MeshNames = RASResultsTools.GetMeshNames(Geometry);
        }
    }
}
