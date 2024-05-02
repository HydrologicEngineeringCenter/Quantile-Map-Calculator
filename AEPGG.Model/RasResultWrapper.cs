using AEPGG.Model.Interfaces;

namespace AEPGG.Model
{
    public class RasResultWrapper : IHydraulicResults
    {
        public string FilePath { get; }
        public RasResultWrapper(string hdfFilePath)
        {
            FilePath = hdfFilePath;
        }

        //not making these properties because I don't want to query the mesh names for every result file. They'll all be the same. Want to hand in mesh names.  
        public float[][] GetMax2DWSE(string[] meshNames )
        {
            return RasTools.H5ReaderTools.GetMaxOrMinWSEForAll2DCells(FilePath, true, meshNames);
        }
        public float[][] GetMin2DWSE(string[] meshNames )
        {
            return RasTools.H5ReaderTools.GetMaxOrMinWSEForAll2DCells(FilePath, false, meshNames);
        }
        public float[] GetMaxXSWSE()
        {
            return RasTools.H5ReaderTools.GetMaxWSEForAllXS(FilePath);
        }
        public float[] GetMinXSWSE()
        {
            return RasTools.H5ReaderTools.GetMinWSEForAllXS(FilePath);
        }


    }
}
