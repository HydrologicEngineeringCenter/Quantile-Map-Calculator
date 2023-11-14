using System.Diagnostics.CodeAnalysis;

namespace AEPGG.Model
{
    public class Project
    {
        public string OutputFilePath { get;}
        public float[] Probabilities { get;}
        public Histogram[] Histograms { get; private set;} = Array.Empty<Histogram>();//Will need to do jagged array if we have multiple 2D areas later. 
        public float BinWidth { get; }
        public float Range { get; }//This is the expected range of depths/vel/etc for a given cell.
        public Project(string outputFilePath, float[] probabilities, float binWidth, float range)
        {
            OutputFilePath = outputFilePath;
            Probabilities = probabilities;
            Range = range;
            BinWidth = binWidth;
        }
        public void AddResults(IHydraulicResults result) 
        {
            if (Histograms.Length == 0)
                InitializeHistograms(result);
            float[][] data = result.Max2DWSEs;
            for (int i = 0; i < data[0].Length; i++)//hard coding 1 2D area for now.
            {
                Histograms![i].Add(data[0][i]);//hard coding 1 2D area for now.
            }
        }
        private void InitializeHistograms(IHydraulicResults result)
        {
            float[][] data = result.Min2DWSEs;
            Histograms = new Histogram[data[0].Length];//hard coding 1 2D area for now.
            for (int i = 0; i < data[0].Length; i++) //hard coding 1 2D area for now.
            {
                float min = data[0][i];//hard coding 1 2D area for now.
                float max = data[0][i] + Range;//hard coding 1 2D area for now.
                Histograms[i] = new(BinWidth, min, max);
            }
        }
        public void SaveResults(string saveToFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
