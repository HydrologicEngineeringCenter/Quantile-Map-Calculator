using Amazon.S3.Model;
using RasMapperLib;
using System.Reflection.Metadata.Ecma335;

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
            return RasTools.GetMaxOrMinWSEForAll2DCells(FilePath, true, meshNames);
        }
        public float[][] GetMin2DWSE(string[] meshNames )
        {
            return RasTools.GetMaxOrMinWSEForAll2DCells(FilePath, false, meshNames);
        }


    }
}
