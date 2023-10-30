namespace AEPGG.Model
{
    public class Histogram
    {
        public int[] BinCounts { get; } 
        public float BinInterval { get; }
        public float Min { get; }
        public Histogram(float binInterval, float min, int numBins)
        {
            BinCounts = new int[numBins];
            BinInterval= binInterval;
            Min = min;
        }
        public void Add(float value)
        {
            int binIndex = (int)Math.Ceiling((value - Min) / BinInterval);
            BinCounts[binIndex]++;
        }
    }
}
