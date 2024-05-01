namespace AEPGG.Model.Interfaces;

/// <summary>
/// This interface exists solely to mock in data for unit testing. 
/// </summary>
public interface IGeometry
{
    public bool HasXSs { get; }
    public bool HasSAs { get; }
    public bool Has2Ds { get; }
    public string[] MeshNames { get; }
}
