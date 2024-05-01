using AEPGG.Model.Interfaces;
using RasMapperLib;

namespace AEPGG.Model
{
    public class AEPComputer
    {
        /// <summary>
        /// Contain results for [2D area index][each cell index].
        /// </summary>
        public Histogram[][] Histograms2DAreas { get; private set; }
        /// <summary>
        /// Contains results for [XS index]
        /// </summary>
        public Histogram[] HistogramsXS { get; private set; }

        /// <summary>
        /// contains the geometry of the model which does not change between results in a single compute.
        /// setter remains public for testing purposes.
        /// </summary>
        public IGeometry Geometry { get; set; }

        /// <summary>
        /// Project contains the jagged arryas of results histograms, the settings for those histograms, and the 
        /// </summary>
        /// <param name="binWidth"></param>
        /// <param name="range"></param>
        public AEPComputer(IHydraulicResults result, float binWidth, float range, IGeometry mockGeometry = null)
        {
            if(mockGeometry != null)
            {
                Geometry = mockGeometry;
            }
            else
            {
                Geometry = new RasGeometryWrapper(result.FilePath);
            }
            InitializeHistograms(result, range, binWidth);
        }

        public void AddResults(IHydraulicResults result)
        {
            if(Geometry.Has2Ds)
            {
                AddResuls2D(result);
            }  
            if(Geometry.HasXSs)
            {
                AddResultsXS(result);
            }
            if(Geometry.HasSAs)
            {
                throw new NotImplementedException("Haven't bothered with SA's yet");
            }
        }
        public void AddResultsXS(IHydraulicResults result)
        {
            float[] data = result.GetMaxXSWSE();
            for (int i = 0; i < data.Length; i++) //for each XS
            {
                HistogramsXS[i].AddValue(data[i]);
            }
        }
        public void AddResuls2D(IHydraulicResults result)
        {
            float[][] data = result.GetMax2DWSE(Geometry.MeshNames);
            for (int i = 0; i < data.Length; i++)//for each 2D area
            {
                for (int j = 0; j < data[i].Length; j++) //for each cell in the 2D area
                {
                    Histograms2DAreas[i][j].AddValue(data[i][j]);
                }
            }
        }

        private void InitializeHistograms(IHydraulicResults result, float range, float binWidth)
        {
            if (Geometry.Has2Ds)
            {
                InitializeHistograms2D(result, range, binWidth);
            }
            if (Geometry.HasXSs)
            {
                InitializeHistogramsXS(result, range, binWidth);
            }
            if (Geometry.HasSAs)
            {
                throw new NotImplementedException("Haven't bothered with SA's yet");
            }
        }


        /// <summary>
        /// Histograms initialize based on the minimum cell elevations in the first result, which are will be consistent across all results, the range of values expected for the WSE in a cell, and the bin width.
        /// </summary>
        private void InitializeHistograms2D(IHydraulicResults result, float range, float binWidth)
        {
            float[][] data = result.GetMin2DWSE(Geometry.MeshNames);
            Histograms2DAreas = new Histogram[data.Length][];
            for (int i = 0; i < data.Length; i++) //for each 2D area
            {
                Histogram[] specificAreaHistograms = new Histogram[data[i].Length];
                for (int j = 0; j < data[i].Length; j++) //for each cell in the 2D area
                {
                    float min = data[i][j];
                    float max = min + range;
                    specificAreaHistograms[j] = new(binWidth, min, max);
                }
                Histograms2DAreas[i] = specificAreaHistograms;
            }
        }

        /// <summary>
        /// Histograms initialize based on the minimum cell elevations in the first result, which are will be consistent across all results, the range of values expected for the WSE in a cell, and the bin width.
        /// </summary>
        private void InitializeHistogramsXS(IHydraulicResults result, float range, float binWidth)
        {
            float[] data = result.GetMinXSWSE();
            HistogramsXS = new Histogram[data.Length];
            for (int i = 0; i < data.Length; i++) //for each XS
            {
                float min = data[i];
                float max = min + range;
                HistogramsXS[i] = new Histogram(binWidth, min, max);
            }
        }

        /// <summary>
        /// returns the WSE for each cell in each 2D area for the specified AEP. [2D Area Index][Cell Index]
        /// </summary>
        /// <param name="aep"></param>
        /// <returns></returns>
        public float[][] GetResultsForAEP(float aep)
        {
            float[][] resultsForAEP = new float[Histograms2DAreas.Length][];
            for (int i = 0; i < Histograms2DAreas.Length; i++) //for each 2D area
            {
                resultsForAEP[i] = new float[Histograms2DAreas[i].Length];
                for (int j = 0; j < Histograms2DAreas[i].Length; j++) //for each cell in the 2D area
                {
                    resultsForAEP[i][j] = Histograms2DAreas[i][j].InverseCDF(aep);
                }
            }
            return resultsForAEP;
        }

    }
}
