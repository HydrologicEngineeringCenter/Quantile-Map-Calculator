using RasMapperLib;

namespace AEPGG.Model.RasTools;

public static class RASResultsTools
{
    public static string[] GetMeshNames(RASGeometry geom)
    {
        int featureCount = geom.D2FlowArea.FeatureCount();
        string[] names = new string[featureCount];
        for (int i = 0; i < featureCount; i++)
        {
            names[i] = geom.D2FlowArea.GetFeatureName(i);
        }
        //Order matters here. The writer is going to depend on these being in the same order 
        return names;
    }
    public static bool ContainsXS(RASGeometry geom)
    {
        bool hasXS = false;
        if (geom.XS.FeatureCount() > 0)
        {
            hasXS = true;
        }
        return hasXS;
    }
    public static bool ContainsSA(RASGeometry geom)
    {
        bool hasSA = false;
        if (geom.StorageArea.FeatureCount() > 0)
        {
            hasSA = true;
        }
        return hasSA;
    }
    public static bool Contains2D(RASGeometry geom)
    {
        bool has2D = false;
        if (geom.D2FlowArea.FeatureCount() > 0)
        {
            has2D = true;
        }
        return has2D;
    }
}
