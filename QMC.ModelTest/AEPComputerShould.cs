﻿using QPC.Model;
using QPC.Model.Computers;
using QPC.Model.Interfaces;
using QPC.ModelTest.Mock_Data;

namespace QPC.ModelTest
{
    [Trait("RunsOn", "Remote")]
    public class AEPComputerShould
    {
        public const string outFilePath = @".\output.txt";
        public float[] theAEPs = [.99f, .5f, .2f, .1f, .02f, .01f, .005f, .002f];

        [Fact]
        public void AddProperly()
        {
            MockRasGeometryWrapper mockRasGeometryWrapper = new(false, false, true, ["fake"]);
            MockHydraulicResult mockHydraulicResult = new(1);
            AEPComputer project = new(mockHydraulicResult, 0.5f, 100, mockRasGeometryWrapper);
            project.AddResults(GetMockResults(1)[0]);
            Assert.Equal(1, project.Histograms2DAreas[0][0].NumSamples); //We have a sample
        }

        [Fact]
        public void AddLotsProperly()
        {
            MockRasGeometryWrapper mockRasGeometryWrapper = new(false, false, true, ["fake"]);
            int numResults = 5000;
            IHydraulicResults[] mockResults = GetMockResults(numResults);
            AEPComputer project = new(mockResults[0], 0.5f, 100, mockRasGeometryWrapper);
            foreach (var mockResult in mockResults)
            {
                project.AddResults(mockResult);
            }
            Assert.Equal(5, project.Histograms2DAreas[0].Length);
            Assert.Equal(project.Histograms2DAreas[0][0].NumSamples, numResults);
            float valueOfThe100yr = project.Histograms2DAreas[0][0].InverseCDF(.01f);
            Assert.True(valueOfThe100yr > 5);
        }

        /// <summary>
        /// returns fake, random hydraulic results
        /// </summary>
        /// <param name="numResults"></param>
        /// <returns></returns>
        private static IHydraulicResults[] GetMockResults(int numResults)
        {
            MockHydraulicResult[] mockHydraulicResults = new MockHydraulicResult[numResults];
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
