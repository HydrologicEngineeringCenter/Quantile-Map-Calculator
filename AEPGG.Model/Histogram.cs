using Utility.DataStructures;

namespace AEPGG.Model
{
    public class Histogram
    {
        public int[] BinCounts { get; }
        public float BinWidth { get; }
        public float Min { get; set; }
        public int NumSamples { get; set; }
        public Histogram(float binWidth, float min, float max)
        {
            int numBins = (int)Math.Ceiling((max - min) / binWidth);
            BinCounts = new int[numBins];
            BinWidth = binWidth;
            Min = min;
        }
        public void Add(float value)
        {
            int binIndex = (int)Math.Floor((value - Min) / BinWidth);
            BinCounts[binIndex]++;
            NumSamples++; 
        }
        public float InverseCDF(float exceedenceProbability)
        {
            int binIndex = 0;
            float binExceedenceProb = 1;
            int numSamplesSoFar = 0;
            while (binExceedenceProb > exceedenceProbability)
            {
                numSamplesSoFar += BinCounts[binIndex];
                binExceedenceProb = 1f - ((float)numSamplesSoFar / NumSamples);
                binIndex++;
            }
            return (binIndex * BinWidth);
        }
    }
}
