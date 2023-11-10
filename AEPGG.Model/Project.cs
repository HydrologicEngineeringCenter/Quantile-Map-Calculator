namespace AEPGG.Model
{
    public class Project
    {
        public string OutputFilePath { get;}
        public float[] Probabilities { get;}
        public Histogram[]? Histograms { get; private set;} //Will need to do jagged array if we have multiple 2D areas later. 
        public float BinWidth { get; }
        public int NumBins { get; }
        public Project(string outputFilePath, float[] probabilities, float binWidth, int numBins)
        {
            OutputFilePath = outputFilePath;
            Probabilities = probabilities;
            NumBins = numBins;
            BinWidth = binWidth;
        }
        public void AddResults(IHydraulicResults result) 
        {
            if (Histograms == null)
                InitializeHistograms(result);
            float[] data = result.MaxWSEs;
            for (int i = 0; i < data.Length; i++)
            {
                Histograms[i].Add(data[i]);
            }
        }
        private void InitializeHistograms(IHydraulicResults result)
        {
            float[] data = result.MinWSEs;
            Histograms = new Histogram[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                Histograms[i] = new(BinWidth, data[i], NumBins);
            }
        }
        public void SaveResults(string saveToFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
