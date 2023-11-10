namespace AEPGG.Model
{
    public interface IHydraulicResults
    {
        public int CellCounts { get; }
        public float[] MaxWSEs { get; }
        public float[] MinWSEs { get; }

    }
}