using Geospatial;
using H5Assist;
using Ras.Layers;
using RasMapperLib;

namespace AEPGG.Model
{
    public static class RasWrapper
    {
        public static float[] GetWSEsForAllNodes(string filePath, string meshName)
        {
            string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MaxWaterSurface.Name(meshName);
            return GetDataFromHDF(filePath, hdfPathToData);
        }
        public static float[] GetMinWSEForAllNodes(string filePath, string meshName)
        {
            string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.FlowAreas.MinWaterSurface.Name(meshName);
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

        private static float[] GetDataFromHDF(string filePath, string hdfPathToData)
        {
            float[] dataOut;
            using H5Reader hr = new(filePath);
            hr.ReadDataset(hdfPathToData, out float[,] data);
            int cellCount = data.GetLength(1);
            dataOut = new float[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                dataOut[i] = data[0, (i)];
            }
            return dataOut;
        }
    }
}
