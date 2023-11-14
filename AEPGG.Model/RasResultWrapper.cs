using System.Reflection.Metadata.Ecma335;

namespace AEPGG.Model
{
    public class RasResultWrapper : IHydraulicResults
    {
        private readonly string _hdfFilePath;
        public int[] CellCounts2D => RasTools.GetCellCount(_hdfFilePath);
        public float[][] Max2DWSEs => RasTools.GetMaxOrMinWSEForAll2DCells(_hdfFilePath, true);
        public float[][] Min2DWSEs => RasTools.GetMaxOrMinWSEForAll2DCells(_hdfFilePath, false);
        public RasResultWrapper(string hdfFilePath)
        {
            _hdfFilePath = hdfFilePath;
        }


    }
}
