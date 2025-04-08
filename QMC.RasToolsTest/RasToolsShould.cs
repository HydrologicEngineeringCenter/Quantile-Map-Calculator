using H5Assist;
using QMC.RasTools;
using RasMapperLib;
using RasMapperLib.Names;

namespace QMC.ModelTest
{
    [Trait("RunsOn", "Local")]
    public class RasToolsShould
    {
        private const string filePath = @"..\..\..\Resources\Muncie.p04.hdf";
        private static RASResults RAsResult = new(filePath);
        private static string[] meshNames = RASResultsTools.GetMeshNames(RAsResult.Geometry);

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetWSEsForAllNodes_ReturnsData(bool getMax)
        {
            var meshNames = RASResultsTools.GetMeshNames(RAsResult.Geometry);
            using H5io reader = new(filePath);
            var result = reader.GetMaxOrMinWSEForAll2DCells(getMax, meshNames);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }
        [Fact]
        public void IdentifyXS()
        {
            Assert.True(RASResultsTools.ContainsXS(RAsResult.Geometry));
        }
        [Fact]
        public void IdentifySA()
        {
            Assert.False(RASResultsTools.ContainsSA(RAsResult.Geometry));
        }
        [Fact]
        public void Identify2D()
        {
            Assert.True(RASResultsTools.Contains2D(RAsResult.Geometry));
        }
        [Fact]
        public void OverwriteMaxWSE()
        {
            //arrange
            string newOutputFilePath = @"..\..\..\Resources\MuncieTEMP.p04.hdf";
            File.Copy(filePath, newOutputFilePath, true);
            using H5io originalFile = new(filePath);
            using H5io newFile = new(newOutputFilePath);

            //act
            float[][] currentWSEs = originalFile.GetMaxOrMinWSEForAll2DCells(true, meshNames);
            float[][] newWSEs = currentWSEs.Select(row => Enumerable.Repeat(9.0f, row.Length).ToArray()).ToArray();
            newFile.OverwriteMaxWSEForAll2DCells(newWSEs, meshNames);

            //assert
            float[][] result = newFile.GetMaxOrMinWSEForAll2DCells(true, meshNames);
            Assert.Equal(9.0f, result[0][0]);
        }

        [Fact]
        public void GetMaxWSEForAllXS_ShouldReturnData()
        {
            // Act
            using H5io reader = new(filePath);
            float[] result = reader.GetMaxWSEForAllXS();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetMinWSEForAllXS_ShouldReturnData()
        {
            // Act
            using H5io reader = new(filePath);
            float[] result = reader.GetMinWSEForAllXS();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void OverwriteSingleTimestepWS()
        {
            // Arrange
            string newOutputFilePath = @"..\..\..\Resources\MuncieTEMP.p04.hdf";
            File.Copy(filePath, newOutputFilePath, true);
            using H5io originalFile = new(filePath);
            using H5io newFile = new(newOutputFilePath);

            // Act
            float[][] currentWSEs = originalFile.GetMaxOrMinWSEForAll2DCells(true, meshNames);
            float[][] newWSEs = currentWSEs.Select(row => Enumerable.Repeat(9.0f, row.Length).ToArray()).ToArray();
            newFile.OverwriteSingleProfile2D(meshNames, newWSEs, 0);
            float[] result = newFile.GetWSEFor2DProfile(meshNames[0], 0);

            // Assert
            Assert.Equal(9.0f, result[0]);
        }
    }
}