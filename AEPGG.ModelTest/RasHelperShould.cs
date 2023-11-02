using AEPGG.Model;

namespace AEPGG.ModelTest
{
    public class RasHelperShould
    {
        private const string filePath = @"C:\Users\q0hecbbb\Projects\RAS Examples\HEC-RAS_64_Example_Projects\Example_Projects\2D Unsteady Flow Hydraulics\Muncie\Muncie.p04.hdf";
        private const string meshName = @"2D Interior Area";

        [Fact]
        public void GetWSEsForAllNodes_ReturnsData()
        {
            // Act
            var result = RasHelper.GetWSEsForAllNodes(filePath, meshName);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public void GetMinWSEForAllNodes_ReturnsData()
        {
            var result = RasHelper.GetMinWSEForAllNodes(filePath, meshName);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public void GetCellCount_ReturnsPositiveValue()
        {
            var result = RasHelper.GetCellCount(filePath, meshName);
            Assert.True(result > 0);
        }
    }
}