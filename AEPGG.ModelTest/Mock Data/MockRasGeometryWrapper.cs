namespace AEPGG.ModelTest.Mock_Data;

internal class MockRasGeometryWrapper : Model.Interfaces.IGeometry
{
    public bool HasXSs { get; } 
    public bool HasSAs { get; }
    public bool Has2Ds { get; } 
    public string[] MeshNames { get; }

    public MockRasGeometryWrapper(bool hasXSs, bool hasSAs, bool has2Ds, string[] meshNames)
    {
        HasXSs = hasXSs;
        HasSAs = hasSAs;
        Has2Ds = has2Ds;
        MeshNames = meshNames;
    }
}

