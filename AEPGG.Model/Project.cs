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
        private float[] GetResultsForAEP(float aep)
        {
            float[] resultsForAEP = new float[Histograms.Length]; //1 x 2d area
            for (int i = 0; i < Histograms.Length; i++) //1 x 2d area
            {
                resultsForAEP[i] = Histograms[i].InverseCDF(aep);
            }
            return resultsForAEP;
        }
        public void SaveResults()
        {
            float[] result = GetResultsForAEP(Probabilities[^1]);
            var list = result.ToList();
            list.Sort();
            list.Reverse();
            for(int i = 0; i < 5; i++)
            {
                Console.WriteLine(list[i]);
            }
            Console.WriteLine();
        }
    }
}
