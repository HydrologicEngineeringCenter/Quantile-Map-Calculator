using AEPGG.Model.RasTools;
using RasMapperLib;
using Utility.Extensions;

namespace AEPGG.ModelTest
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
            var result = H5ReaderTools.GetMaxOrMinWSEForAll2DCells(filePath, getMax, meshNames);
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
            string newOutputFilePath = @"..\..\..\Resources\MuncieTEMP.p04.hdf" ;
            File.Copy(filePath, newOutputFilePath, true);
            float[][] currentWSEs = H5ReaderTools.GetMaxOrMinWSEForAll2DCells(newOutputFilePath,true, meshNames);
            float[][] newWSEs = (float[][])currentWSEs.Clone();
            for (int i = 0; i < newWSEs.Length; i++)
            {
                for (int j = 0; j < newWSEs[i].Length; j++)
                {
                    newWSEs[i][j] = 9.0f;
                }
            }
            H5WriterTools.OverwriteMaxWSEForAll2DCells(newOutputFilePath, newWSEs, meshNames);
            float[][] result = H5ReaderTools.GetMaxOrMinWSEForAll2DCells(newOutputFilePath, true, meshNames);
            Assert.Equal(9.0f, result[0][0]);
        }

        [Fact]
        public void GetMaxWSEForAllXS_ShouldReturnData()
        {
            // Act
            float[] result = H5ReaderTools.GetMaxWSEForAllXS(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetMinWSEForAllXS_ShouldReturnData()
        {
            // Act
            float[] result = H5ReaderTools.GetMinWSEForAllXS(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}