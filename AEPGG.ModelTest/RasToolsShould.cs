using AEPGG.Model;
using RasMapperLib;
using Utility.Extensions;

namespace AEPGG.ModelTest
{
    [Trait("RunsOn", "Local")]
    public class RasToolsShould
    {
        private const string filePath = @"..\..\..\Resources\Muncie.p04.hdf";
        private static RASResults RAsResult = new(filePath);
        private static string[] meshNames = RasTools.GetMeshNames(RAsResult.Geometry);

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetWSEsForAllNodes_ReturnsData(bool getMax)
        {
            var meshNames = RasTools.GetMeshNames(RAsResult.Geometry);
            var result = RasTools.GetMaxOrMinWSEForAll2DCells(filePath, getMax, meshNames);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }
        [Fact]
        public void IdentifyXS()
        {
            Assert.True(RasTools.ContainsXS(RAsResult.Geometry));
        }
        [Fact]
        public void IdentifySA()
        {
            Assert.False(RasTools.ContainsSA(RAsResult.Geometry));
        }
        [Fact]
        public void Identify2D()
        {
            Assert.True(RasTools.Contains2D(RAsResult.Geometry));
        }
        [Fact]
        public void OverwriteMaxWSE()
        {
            string newOutputFilePath = @"..\..\..\Resources\MuncieTEMP.p04.hdf" ;
            File.Copy(filePath, newOutputFilePath, true);
            float[][] currentWSEs = RasTools.GetMaxOrMinWSEForAll2DCells(newOutputFilePath,true, meshNames);
            float[][] newWSEs = (float[][])currentWSEs.Clone();
            for (int i = 0; i < newWSEs.Length; i++)
            {
                for (int j = 0; j < newWSEs[i].Length; j++)
                {
                    newWSEs[i][j] = 9.0f;
                }
            }
            RasTools.OverwriteMaxWSEForAll2DCells(newOutputFilePath, newWSEs, meshNames);
            float[][] result = RasTools.GetMaxOrMinWSEForAll2DCells(newOutputFilePath, true, meshNames);
            Assert.Equal(9.0f, result[0][0]);

        }
    }
}