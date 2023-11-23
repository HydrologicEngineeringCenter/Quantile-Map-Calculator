using AEPGG.Model;
using Utility.Extensions;

namespace AEPGG.ModelTest
{
    [Trait("RunsOn", "Local")]
    public class RasToolsShould
    {
        private const string filePath = @"..\..\..\Resources\Muncie.p04.hdf";

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetWSEsForAllNodes_ReturnsData(bool getMax)
        {
            var result = RasTools.GetMaxOrMinWSEForAll2DCells(filePath, getMax);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }
        [Fact]
        public void GetCellCount_ReturnsPositiveValue()
        {
            var result = RasTools.GetCellCount(filePath);
            Assert.True(result[0] > 0);
        }
        [Fact]
        public void IdentifyXS()
        {
            Assert.True(RasTools.ContainsXS(filePath));
        }
        [Fact]
        public void IdentifySA()
        {
            Assert.False(RasTools.ContainsSA(filePath));
        }
        [Fact]
        public void Identify2D()
        {
            Assert.True(RasTools.Contains2D(filePath));
        }
        [Fact]
        public void OverwriteMaxWSE()
        {
            string newOutputFilePath = @"..\..\..\Resources\MuncieTEMP.p04.hdf" ;
            File.Copy(filePath, newOutputFilePath, true);
            int[] cellCounts = RasTools.GetCellCount(filePath);
            float[] newWSEs = new float[cellCounts[0]];
            newWSEs.Fill(9.0f);
            RasTools.OverwriteMaxWSEForAll2DCells(newOutputFilePath, newWSEs);
            float[][] result = RasTools.GetMaxOrMinWSEForAll2DCells(newOutputFilePath, true);
            Assert.Equal(9.0f, result[0][0]);

        }
    }
}