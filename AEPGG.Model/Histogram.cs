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
            int binIndex = -1; // start at -1 because we increment before checking
            float binExceedenceProb = 1;
            int numSamplesSoFar = 0;
            while (binExceedenceProb > exceedenceProbability)
            {
                binIndex++;
                numSamplesSoFar += BinCounts[binIndex];
                binExceedenceProb = 1f - ((float)numSamplesSoFar / NumSamples);
                
            }
            if(binIndex == 0) // if the answer is in the first bin, return the min. Assume at this resolution the cell is dry. 
                return Min;
            else
                return Min + (binIndex * BinWidth) + (BinWidth/2); //if the answer is in any other bin, return the center value of that bin. 
        }
    }
}
