using AEPGG.Model;

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
    }
}