namespace AEPGG.Model
{
    public class Histogram
    {
        public int[] BinCounts { get; } 
        public float BinWidth { get; }
        public float Min { get; }
        public int NumSamples { get; }
        public Histogram(float binWidth, float min, int numBins)
        {
            BinCounts = new int[numBins];
            BinWidth= binWidth;
            Min = min;
        }
        public void Add(float value)
        {
            int binIndex = (int)Math.Ceiling((value - Min) / BinWidth);
            BinCounts[binIndex]++;
        }
        public float InverseCDF(float probability)
        {
            int binIndex = 0;
            float binStartingPercentile = 0;
            int numSamplesSoFar = 0;
            while(binStartingPercentile > probability)
            {
                numSamplesSoFar += BinCounts[binIndex];
                binStartingPercentile = 1 - (numSamplesSoFar/BinCounts.Length);
                binIndex++;
            }
            return (binIndex * BinWidth) + (BinWidth/2) ;
        }

    }
}
