using Geospatial;
using H5Assist;
using Ras.Layers;
using RasMapperLib;

namespace AEPGG.Model
{
    public static class RasTools
    {
        #region RAS Results API
        public static int[] GetCellCount(string filePath)
        {
            string[] meshNames = GetMeshNames(filePath);
            int[] cellCounts = new int[meshNames.Length];
            for (int i = 0; i < meshNames.Length; i++)
            {
                cellCounts[i] = GetCellCount(filePath, meshNames[i]);
            }
            return cellCounts;
        }
        public static int GetCellCount(string filePath, string meshName)
        {
            string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MaxWaterSurface.Name(meshName);
            using var hr = new H5Reader(filePath);
            hr.ReadDataset(hdfPathToData, out float[,] data);
            return data.GetLength(1);
        }
        public static string[] GetMeshNames(string filePath)
        {
            RASResults result = new(filePath);
            int featureCount = result.Geometry.D2FlowArea.FeatureCount();
            string[] names = new string[featureCount];
            for (int i = 0; i < featureCount; i++)
            {
                names[i] = result.Geometry.D2FlowArea.GetFeatureName(i);
            }
            return names;
        }
        public static bool ContainsXS(string filePath)
        {
            RASResults result = new(filePath);
            bool hasXS = false;
            if (result.Geometry.XS.FeatureCount() > 0)
            {
                hasXS = true;
            }
            return hasXS;
        }
        public static bool ContainsSA(string filePath)
        {
            RASResults result = new(filePath);
            bool hasSA = false;
            if (result.Geometry.StorageArea.FeatureCount() > 0)
            {
                hasSA = true;
            }
            return hasSA;
        }
        public static bool Contains2D(string filePath)
        {
            RASResults result = new(filePath);
            bool has2D = false;
            if (result.Geometry.D2FlowArea.FeatureCount() > 0)
            {
                has2D = true;
            }
            return has2D;
        }
        #endregion

        #region HDF File Access
        public static float[][] GetMaxOrMinWSEForAll2DCells(string filePath, bool getMax)
        {
            string[] meshNames = GetMeshNames(filePath);
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
        private static float[] GetRowFromHDF(string filePath, string hdfPathToData, int rowID = 0)
        {
            float[] dataOut;
            using H5Reader hr = new(filePath);
            hr.ReadDataset(hdfPathToData, out float[,] data);
            int cellCount = data.GetLength(1);
            dataOut = new float[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                dataOut[i] = data[rowID, (i)];
            }
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
        public static void OverwriteMaxWSEForAll2DCells(string filePath, float[] data)
        {
            foreach (string meshName in GetMeshNames(filePath))
            {
                OverwriteMaxWSEForAll2DCells(filePath, meshName, data);
            }   
        }
        #endregion
    }
}
