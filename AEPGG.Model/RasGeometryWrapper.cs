﻿using AEPGG.Model.Interfaces;
using RasMapperLib;

namespace AEPGG.Model;

public class RasGeometryWrapper: IGeometry
{
    public bool HasXSs {get;}
    public bool HasSAs { get; }
    public bool Has2Ds { get; }
    public string[] MeshNames { get; }
    public RasGeometryWrapper(string filename)
    {
        RASResults result = new(filename);
        if (!result.ContainsData)
        {
            throw new ArgumentException("invalid results file");
        }
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
