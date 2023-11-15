namespace AEPGG.Model
{
    public class MockHydraulicResult:IHydraulicResults
    {
        public int[] CellCounts2D => new int[] { 5 };
        public float[][] Max2DWSEs { get; }
        public float[][] Min2DWSEs { get; }

        public MockHydraulicResult(int multiplier)
        {
            Min2DWSEs = new float[1][];
            Min2DWSEs[0] = new float[] { 0, 0, 0, 0, 0 };
            Max2DWSEs = new float[1][];
            Max2DWSEs[0] = new float[] { 1, 1, 1, 1, 1 };
            for(int i= 0; i < Max2DWSEs[0].Length; i++)
            {
                Max2DWSEs[0][i] *= multiplier;
            }   
        }
    }
}
