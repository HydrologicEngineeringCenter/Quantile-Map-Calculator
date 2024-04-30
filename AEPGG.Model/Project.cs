using RasMapperLib;

namespace AEPGG.Model
{
    public class Project
    {
        /// <summary>
        /// Contain results for [2D area index][each cell index]
        /// </summary>
        public Histogram[][] Histograms { get; private set; }
        /// <summary>
        /// The width of each bin in the resultant historgrams
        /// </summary>
        public float BinWidth { get; }
        /// <summary>
        /// The expected span of values for the WSE in a cell. Ex. if the expected values are between 50ft and 150ft, the range would be 100ft.
        /// </summary>
        public float Range { get; }
        /// <summary>
        /// contains the geometry of the model which does not change between results in a single compute.
        /// </summary>
        public RasGeometryWrapper Geometry { get; set; }

        public Project(float binWidth, float range)
        {
            Range = range;
            BinWidth = binWidth;
        }

        /// <summary>
        /// First determines whether results histograms exist yet, if they don't we copy the current result file to the output file path, and initialize histograms. Histograms are initialized 
        /// </summary>
        /// <param name="result"></param>
        public void AddResults(IHydraulicResults result)
        {
            if (Histograms == null) // Then this is the first result we are adding, and it's a real result with a file.
            {
                Geometry = new(result.FilePath); //needs to run before we initialize histograms
                InitializeHistograms(result);
            }

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
        private void InitializeHistograms(IHydraulicResults result)
        {
            float[][] data = result.GetMin2DWSE(Geometry.MeshNames);
            Histograms = new Histogram[data.Length][];
            for (int i = 0; i < data.Length; i++) //for each 2D area
            {
                Histogram[] specificAreaHistograms = new Histogram[data[i].Length];
                for (int j = 0; j < data[i].Length; j++) //for each cell in the 2D area
                {
                    float min = data[i][j];
                    float max = min + Range;
                    specificAreaHistograms[j] = new(BinWidth, min, max);
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
