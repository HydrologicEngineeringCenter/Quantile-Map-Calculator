namespace AEPGG.Model
{
    public class MockHydraulicResult:IHydraulicResults
    {
        public int CellCounts => 5;
        public float[] MaxWSEs { get; }
        public float[] MinWSEs => new float[] { 0, 0, 0, 0, 0 };

        public MockHydraulicResult(int multiplier)
        {
            MaxWSEs = new float[] { 1, 1, 1, 1, 1 };
            for(int i= 0; i < MaxWSEs.Length; i++)
            {
                MaxWSEs[i] *= multiplier;
            }   
        }
    }
}
