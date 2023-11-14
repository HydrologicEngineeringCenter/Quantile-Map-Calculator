using System.Reflection.Metadata.Ecma335;

namespace AEPGG.Model
{
    public class RasResultWrapper : IHydraulicResults
    {
        private readonly string _hdfFilePath;
        private readonly string meshName;
        public int CellCounts => RasTools.GetCellCount(_hdfFilePath, meshName);
        public float[] MaxWSEs => RasTools.GetMaxOrMinWSEForAll2DCells(_hdfFilePath, meshName, true);
        public float[] MinWSEs => RasTools.GetMaxOrMinWSEForAll2DCells(_hdfFilePath, meshName, false);

        public RasResultWrapper(string hdfFilePath)
        {
            _hdfFilePath = hdfFilePath;
            meshName = RasTools.GetMeshNames(_hdfFilePath)[0]; //only working with 1 2D area for now.
        }


    }
}
