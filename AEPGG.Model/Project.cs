using System.Numerics;

namespace AEPGG.Model
{
    public class Project
    {
        public string OutputFilePath { get; }
        public float[] Probabilities { get; }
        public Histogram[][] Histograms { get; private set; }
        public float BinWidth { get; }
        public float Range { get; }//This is the expected range of WSE for a given cell.
        public Project(string outputFilePath, float[] probabilities, float binWidth, float range)
        {
            OutputFilePath = outputFilePath;
            Probabilities = probabilities;
            Range = range;
            BinWidth = binWidth;
        }

        public void AddResults(IHydraulicResults result)
        {
            if (Histograms == null) // Then this is the first result we are adding, and it's a real result with a file.
            {
                if (File.Exists(result.FilePath))
                {
                    File.Copy(result.FilePath, OutputFilePath, true); // copy the first result over to the output directory. We'll use this as the starting point for our output file. 
                }
                InitializeHistograms(result);
            }
            float[][] data = result.Max2DWSEs;
            for (int i = 0; i < data.Length; i++)//for each 2D area
            {
                for (int j = 0; j < data[i].Length; j++) //for each cell in the 2D area
                {
                    Histograms[i][j].Add(data[i][j]);
                }
            }
        }

        private void InitializeHistograms(IHydraulicResults result)
        {
            float[][] data = result.Min2DWSEs;
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
        private float[][] GetResultsForAEP(float aep)
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
        public void SaveResults()
        {
            float[][] result = GetResultsForAEP(Probabilities[^1]); //only using 1 AEP for now. 
            RasTools.OverwriteMaxWSEForAll2DCells(OutputFilePath, result[0]);//only writing to one 2D area for now.
        }
    }
}
