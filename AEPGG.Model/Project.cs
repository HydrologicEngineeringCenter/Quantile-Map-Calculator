namespace AEPGG.Model
{
    public class Project
    {
        public string OutputFilePath { get;}
        public float[] Probabilities { get;}
        public Histogram[] Histograms { get; private set;}
        public float BinWidth { get; }
        public int NumBins { get; }
        public Project(string outputFilePath, float[] probabilities, float binWidth, int numBins)
        {
            OutputFilePath = outputFilePath;
            Probabilities = probabilities;
            NumBins = numBins;
            BinWidth = binWidth;
        }

        public void AddResults(string pathToResultHDF) 
        {
            if (Histograms[0] == null)
                InitializeHistograms(pathToResultHDF);

            string meshName = RasWrapper.GetMeshNames(pathToResultHDF)[0]; // Only working with 1 2D area for now. 
            float[] data = RasWrapper.GetWSEsForAllNodes(pathToResultHDF, meshName);
            for (int i = 0; i < data.Length; i++)
            {
                Histograms[i].Add(data[i]);
            }
        }

        private void InitializeHistograms(string pathToResultHDF)
        {
            string meshName = RasWrapper.GetMeshNames(pathToResultHDF)[0]; // Only working with 1 2D area for now. 
            float[] data = RasWrapper.GetMinWSEForAllNodes(pathToResultHDF, meshName);
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
