using Geospatial;
using H5Assist;
using Ras.Layers;
using RasMapperLib;

namespace AEPGG.Model
{
    public static class RasTools
    {
        public static float[][] GetMaxOrMinWSEForAll2DCells(string filePath, bool getMax)
        {
            string[] meshNames = GetMeshNames(filePath);
            float[][] WSEs = new float[meshNames.Length][];
            for(int i = 0; i < meshNames.Length; i++)
            {
                WSEs[i] = GetMaxOrMinWSEsFor2DCells(filePath, meshNames[i], getMax);
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
            return GetDataFromHDF(filePath, hdfPathToData);
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
            for(int i = 0; i < featureCount; i++)
            {
                names[i] = result.Geometry.D2FlowArea.GetFeatureName(i);
            }
            return names;
        }
        private static float[] GetDataFromHDF(string filePath, string hdfPathToData, int rowID = 0)
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
        private static bool ContainsXS(string filePath)
        {
            RASResults result = new(filePath);
            bool hasXS = false;
            if (result.Geometry.XS.FeatureCount() > 0)
            {
                hasXS = true;
            }
            return hasXS;
        }
        private static bool ContainsSA(string filePath)
        {
            RASResults result = new(filePath);
            bool hasSA = false;
            if (result.Geometry.StorageArea.FeatureCount() > 0)
            {
                hasSA = true;
            }
            return hasSA;
        }
        private static bool Contains2D(string filePath)
        {
            RASResults result = new(filePath);
            bool has2D = false;
            if (result.Geometry.D2FlowArea.FeatureCount() > 0)
            {
                has2D = true;
            }
            return has2D;
        }


    }
}
