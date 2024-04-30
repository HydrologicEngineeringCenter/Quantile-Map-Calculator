using RasMapperLib;

namespace AEPGG.Model
{
    public class AEPComputer
    {
        /// <summary>
        /// Contain results for [2D area index][each cell index]
        /// </summary>
        public Histogram[][] Histograms { get; private set; }

        /// <summary>
        /// contains the geometry of the model which does not change between results in a single compute.
        /// </summary>
        public RasGeometryWrapper Geometry { get; set; }

        /// <summary>
        /// Project contains the jagged arryas of results histograms, the settings for those histograms, and the 
        /// </summary>
        /// <param name="binWidth"></param>
        /// <param name="range"></param>
        public AEPComputer(IHydraulicResults result, float binWidth, float range)
        {
            Geometry = new(result.FilePath);
            InitializeHistograms(result, range, binWidth);
        }

        public void AddResults(IHydraulicResults result)
        {
            float[][] data = result.GetMax2DWSE(Geometry.MeshNames);
            for (int i = 0; i < data.Length; i++)//for each 2D area
            {
                for (int j = 0; j < data[i].Length; j++) //for each cell in the 2D area
                {
                    Histograms[i][j].AddValue(data[i][j]);
                }
            }
        }

        /// <summary>
        /// Histograms initialize based on the minimum cell elevations in the first result, which are will be consistent across all results, the range of values expected for the WSE in a cell, and the bin width.
        /// </summary>
        /// <param name="result"></param>
        private void InitializeHistograms(IHydraulicResults result, float range, float binWidth)
        {
            float[][] data = result.GetMin2DWSE(Geometry.MeshNames);
            Histograms = new Histogram[data.Length][];
            for (int i = 0; i < data.Length; i++) //for each 2D area
            {
                Histogram[] specificAreaHistograms = new Histogram[data[i].Length];
                for (int j = 0; j < data[i].Length; j++) //for each cell in the 2D area
                {
                    float min = data[i][j];
                    float max = min + range;
                    specificAreaHistograms[j] = new(binWidth, min, max);
                }
                Histograms[i] = specificAreaHistograms;
            }
        }

        /// <summary>
        /// returns the WSE for each cell in each 2D area for the specified AEP. [2D Area Index][Cell Index]
        /// </summary>
        /// <param name="aep"></param>
        /// <returns></returns>
        public float[][] GetResultsForAEP(float aep)
        {
            float[][] resultsForAEP = new float[Histograms.Length][];
            for (int i = 0; i < Histograms.Length; i++) //for each 2D area
            {
                resultsForAEP[i] = new float[Histograms[i].Length];
                for (int j = 0; j < Histograms[i].Length; j++) //for each cell in the 2D area
                {
                    resultsForAEP[i][j] = Histograms[i][j].InverseCDF(aep);
                }
            }
            return resultsForAEP;
        }

    }
}
