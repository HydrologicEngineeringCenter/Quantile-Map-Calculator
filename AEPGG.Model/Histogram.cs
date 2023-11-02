namespace AEPGG.Model
{
    public class Histogram
    {
        public int[] BinCounts { get; }
        public float BinWidth { get; }
        public float Min { get; set; }
        public int NumSamples { get; set; }//We may just know this apriori
        public Histogram(float binWidth, float min, int numBins)
        {
            BinCounts = new int[numBins];
            BinWidth = binWidth;
            Min = min;
        }
        public void Add(float value)
        {
            int binIndex = (int)Math.Ceiling((value - Min) / BinWidth);
            BinCounts[binIndex]++;
            NumSamples++; //We may just know this apriori
        }
        public float InverseCDF(float exceedenceProbability)
        {
            int binIndex = 0;
            float binStartingPercentile = 0;
            int numSamplesSoFar = 0;
            while (binStartingPercentile > exceedenceProbability)
            {
                numSamplesSoFar += BinCounts[binIndex];
                binStartingPercentile = 1 - (numSamplesSoFar / BinCounts.Length);
                binIndex++;
            }
            return (binIndex * BinWidth) + (BinWidth / 2);
        }
    }
}
