using System.Reflection.Metadata.Ecma335;

namespace AEPGG.Model
{
    public class RasResultWrapper : IHydraulicResults
    {
        public string FilePath { get; }
        public int[] CellCounts2D => RasTools.GetCellCount(FilePath);
        public float[][] Max2DWSEs => RasTools.GetMaxOrMinWSEForAll2DCells(FilePath, true);
        public float[][] Min2DWSEs => RasTools.GetMaxOrMinWSEForAll2DCells(FilePath, false);
        public RasResultWrapper(string hdfFilePath)
        {
            FilePath = hdfFilePath;
        }


    }
}
