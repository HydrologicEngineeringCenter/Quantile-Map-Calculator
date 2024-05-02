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
        /// <param name="rowID"> typically 0 for Max WSE from MaxWSE records</param>
        private static float[] GetRowFromHDF(string filePath, string hdfPathToData, int rowID = 0)
        {
            float[] dataOut = null;
            using H5Reader hr = new(filePath);
            hr.ReadRow(hdfPathToData, rowID, ref dataOut);
            return dataOut;
        }
        //probably faster to pull this H5 writer up a level and not reopen it very time we read a row. 
        private static void WriteDataToHDF(string filePath, string hdfPathToData, float[,] data) 
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

        /// <param name="filePath"> must have .hdf extension</param>
        public static float[] GetMaxWSEForAllXS(string filePath)
        {
            string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MaxWaterSurface.Name;
            float[] data = GetRowFromHDF(filePath, hdfPathToData, 0);
            return data;
        }

        /// <param name="filePath"> must have .hdf extension</param>
        public static float[] GetMinWSEForAllXS(string filePath)
        {
            string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MinWaterSurface.Name;
            float[] data = GetRowFromHDF(filePath, hdfPathToData, 0);
            return data;
        }

        /// <param name="filePath"> must have .hdf extension</param>
        public static void OverwriteMaxWSEforAllXs(string filePath, float[] data)
        {
            string hdfPathToData = ResultsDatasets.Unsteady.SummaryOutput.CrossSections.MaxWaterSurface.Name;
            float[,] dataTable = new float[3, data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                dataTable[0, i] = data[i]; //Max WSE
                dataTable[1,i] = 0; //Flow at Max (Just bullshit for now)
                dataTable[2,i] = 0; //Time of Max (Just bullshit for now)
            }
            WriteDataToHDF(filePath, hdfPathToData, dataTable);
        }


        #region I'm suspcious of this strategy. This isn't fleshed out. 
        /// <summary> Overwrites the water surface elevation in the results timeseries with a single profile for each of the specified meshes </summary>
        /// <param name="data">[2D Area Index][Cell Index]</param>
        public static void OverwriteSingleProfile(string filePath, string[] meshNames, float[][] data)
        {
            for (int i = 0; i < meshNames.Length; i++)
            {
                OverwriteSingleProfile(filePath, meshNames[i], data[i]);
            }
        }

        /// <summary> Overwrites the water surface elevation in the results timeseries with a single profile for a single mesh </summary>
        /// <param name="data"> [Cell Index] </param>
        private static void OverwriteSingleProfile(string filePath, string meshName, float[] data)
        {
            string hdfPathToData = ResultsDatasets.Unsteady.TimeSeriesOutput.FlowAreas.WaterSurface(meshName);
            float[,] dataTable = new float[1, data.Length];// write method re
            for (int i = 0; i < data.Length; i++)
            {
                dataTable[0, i] = data[i]; //Max WSE
            }
            WriteDataToHDF(filePath, hdfPathToData, dataTable);
        }
        #endregion

        #endregion
    }
}
