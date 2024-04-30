using Geospatial;
using H5Assist;
using Ras.Layers;
using RasMapperLib;

namespace AEPGG.Model
{
    public static class RasTools
    {
        #region RAS Results API
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
        #endregion

        #region HDF File Access
        public static float[][] GetMaxOrMinWSEForAll2DCells(string filePath, bool getMax, string[] meshNames)
        {
            float[][] WSEs = new float[meshNames.Length][];
            for (int i = 0; i < meshNames.Length; i++)
            {
                WSEs[i] = GetMaxOrMinWSEForAll2DCells(filePath, meshNames[i], getMax);
            }
            return WSEs;
        }
        public static float[] GetMaxOrMinWSEForAll2DCells(string filePath, string meshName, bool getMax)
        {
            string hdfPathToData;
            if (getMax)
            {
                hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MaxWaterSurface.Name(meshName);
            }
            else
            {
                hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MinWaterSurface.Name(meshName);
            }
            return GetRowFromHDF(filePath, hdfPathToData, 0);
        }

        //probably faster to pull this H5 reader up a level and not reopen it very time we read a row. 
        private static float[] GetRowFromHDF(string filePath, string hdfPathToData, int rowID = 0)
        {
            float[] dataOut = null;
            using H5Reader hr = new(filePath);
            hr.ReadRow(hdfPathToData, rowID, ref dataOut);
            return dataOut;
        }
        private static void WriteDataToHDF(string filePath, string hdfPathToData, float[,] data) //will probably need to add a second row even when just doing the max WSE
        {
            using H5Writer hw = new(filePath);
            hw.WriteDataset(hdfPathToData, data);
        }
        private static void OverwriteMaxWSEForAll2DCells(string filePath, string meshName, float[] data)
        {
            string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MaxWaterSurface.Name(meshName);
            float[,] dataWithTimeRow = new float[2, data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                dataWithTimeRow[0, i] = data[i]; //Max WSE
                dataWithTimeRow[1, i] = 0; //Time of Max (Just bullshit for now)
            }

            WriteDataToHDF(filePath, hdfPathToData, dataWithTimeRow);
        }
        public static void OverwriteMaxWSEForAll2DCells(string filePath, float[][] data, string[] meshNames)
        {
            for(int i = 0; i < meshNames.Length; i++)
            {
                OverwriteMaxWSEForAll2DCells(filePath, meshNames[i], data[i]);
            }
        }
        #endregion
    }
}
