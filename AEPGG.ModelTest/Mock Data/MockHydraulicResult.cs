using AEPGG.Model.Interfaces;

namespace AEPGG.Model
{
    public class MockHydraulicResult:IHydraulicResults
    {
        public float[][] Max2DWSEs { get; }
        public float[][] Min2DWSEs { get; }
        public string FilePath => Path.GetRandomFileName();

        public MockHydraulicResult(int multiplier)
        {
            Min2DWSEs = new float[1][];
            Min2DWSEs[0] = [0, 0, 0, 0, 0];
            Max2DWSEs = new float[1][];
            Max2DWSEs[0] = [1, 1, 1, 1, 1];
            for(int i= 0; i < Max2DWSEs[0].Length; i++)
            {
                Max2DWSEs[0][i] *= multiplier;
            }   
        }

        public float[][] GetMax2DWSE(string[] meshNames)
        {
            return Max2DWSEs;
        }

        public float[][] GetMin2DWSE(string[] meshNames)
        {
            return Min2DWSEs;
        }

        public float[] GetMaxXSWSE()
        {
            throw new NotImplementedException();
        }

        public float[] GetMinXSWSE()
        {
            throw new NotImplementedException();
        }

        public float[] GetXSWSE(int profileIndex)
        {
            throw new NotImplementedException();
        }

        public float[][] Get2DWSE(int profileIndex, string[] meshNames)
        {
            throw new NotImplementedException();
        }
    }
}
