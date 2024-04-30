using AEPGG.Model;
using RasMapperLib;
using Utility.Extensions;

namespace AEPGG.ModelTest
{
    [Trait("RunsOn", "Local")]
    public class RasToolsShould
    {
        private const string filePath = @"..\..\..\Resources\Muncie.p04.hdf";
        private RasGeometryWrapper Geometry = new(filePath);

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetWSEsForAllNodes_ReturnsData(bool getMax)
        {

            var result = RasTools.GetMaxOrMinWSEForAll2DCells(filePath, getMax, Geometry.MeshNames);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }
        [Fact]
        public void IdentifyXS()
        {
            Assert.True(RasTools.ContainsXS(Geometry.Geometry));
        }
        [Fact]
        public void IdentifySA()
        {
            Assert.False(RasTools.ContainsSA(Geometry.Geometry));
        }
        [Fact]
        public void Identify2D()
        {
            Assert.True(RasTools.Contains2D(Geometry.Geometry));
        }
        [Fact]
        public void OverwriteMaxWSE()
        {
            string newOutputFilePath = @"..\..\..\Resources\MuncieTEMP.p04.hdf" ;
            File.Copy(filePath, newOutputFilePath, true);
            float[][] currentWSEs = RasTools.GetMaxOrMinWSEForAll2DCells(newOutputFilePath,true, Geometry.MeshNames);
            float[][] newWSEs = (float[][])currentWSEs.Clone();
            for (int i = 0; i < newWSEs.Length; i++)
            {
                for (int j = 0; j < newWSEs[i].Length; j++)
                {
                    newWSEs[i][j] = 9.0f;
                }
            }
            RasTools.OverwriteMaxWSEForAll2DCells(newOutputFilePath, newWSEs, Geometry.MeshNames);
            float[][] result = RasTools.GetMaxOrMinWSEForAll2DCells(newOutputFilePath, true, Geometry.MeshNames);
            Assert.Equal(9.0f, result[0][0]);

        }
    }
}