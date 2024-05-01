namespace AEPGG.Model.Interfaces;

public interface IGeometry
{
    public bool HasXSs { get; }
    public bool HasSAs { get; }
    public bool Has2Ds { get; }
    public string[] MeshNames { get; }
}
