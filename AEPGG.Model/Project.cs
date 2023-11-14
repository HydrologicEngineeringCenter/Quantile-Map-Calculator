namespace AEPGG.Model
{
    public class Project
    {
        public string OutputFilePath { get;}
        public float[] Probabilities { get;}
        public Histogram[]? Histograms { get; private set;} //Will need to do jagged array if we have multiple 2D areas later. 
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
                float min = data[i];
                float max = data[i] + Range;
                Histograms[i] = new(BinWidth, min, max);
            }
        }
        public void SaveResults(string saveToFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
