namespace QPC.Model;

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
    /// <summary>
    /// Subtracts the minimum value of the histogram from the value, divides it by the bin width, then takes the floor of that to get the bin index before incrementing that bin and the <cref>NumSamples</cref>
    /// </summary>
    /// <param name="value"></param>
    public void AddValue(float value)
    {
        int binIndex = (int)Math.Floor((value - Min) / BinWidth);
        BinCounts[binIndex]++;
        NumSamples++;
    }
    /// <summary>
    /// returns the value of the histogram for the exceedence probability specified. Ex. .01 would return the 100 Year return interval WSE
    /// </summary>
    /// <param name="exceedenceProbability"></param>
    /// <returns></returns>
    public float InverseCDF(float exceedenceProbability)
    {
        int binIndex = -1; // start at -1 because we increment before checking
        float binExceedenceProb = 1;
        int numSamplesSoFar = 0;
        while (binExceedenceProb > exceedenceProbability)
        {
            binIndex++; //increment first
            numSamplesSoFar += BinCounts[binIndex];
            binExceedenceProb = 1f - (float)numSamplesSoFar / NumSamples;

        }
        float ret;
        if (binIndex == 0) // if the answer is in the first bin, return the min. Assume at this resolution the cell is dry. 
            ret = Min;
        else
            ret = Min + binIndex * BinWidth + BinWidth / 2; //if the answer is in any other bin, return the center value of that bin.
        return ret;
    }
}
