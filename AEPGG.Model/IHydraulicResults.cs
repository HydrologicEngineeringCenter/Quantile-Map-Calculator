namespace AEPGG.Model
{
    public interface IHydraulicResults
    {
        public int[] CellCounts2D { get; }
        public float[][] Max2DWSEs { get; }
        public float[][] Min2DWSEs { get; }

    }
}