using AEPGG.Model;

namespace AEPGG.ModelTest
{
    public class ProjectShould
    {
        public const string outFilePath = @".\output.txt";
        private const string filePath = @"C:\Users\q0hecbbb\Projects\RAS Examples\HEC-RAS_64_Example_Projects\Example_Projects\2D Unsteady Flow Hydraulics\Muncie\Muncie.p04.hdf";
        public float[] theAEPs = new float[] { .99f, .5f, .2f, .1f, .02f, .01f, .005f, .002f };

        [Fact]
        public void AddProperly()
        {
            Project project = new(outFilePath, theAEPs, 0.5f, 100);
            project.AddResults(new RasResultWrapper(filePath));
            Assert.Equal(1, project.Histograms[0].NumSamples); //We have a sample
            Assert.True(project.Histograms[0].Min > 0);//We have a min
        }

        [Fact]
        public void AddLotsProperly()
        {
            IHydraulicResults[] mockResults = GetMockResults();
            Project project = new(outFilePath, theAEPs, 0.5f, 100);
            foreach(var mockResult in mockResults)
            {
                project.AddResults(mockResult);
            }
            Assert.Equal(project.Histograms.Count(),5);
            Assert.Equal(project.Histograms[0].NumSamples, 500);
            Assert.True(project.Histograms[0].InverseCDF(.01f) > 5);
        }

        private IHydraulicResults[] GetMockResults()
        {
            MockHydraulicResult[] mockHydraulicResults = new MockHydraulicResult[500];
            Random random = new(1234);
            for (int i = 0; i < mockHydraulicResults.Length; i++)
            {
                int multiplier = random.Next(1, 40);
                mockHydraulicResults[i] = new MockHydraulicResult(multiplier);
            }
            return mockHydraulicResults;
        }

    }
}
