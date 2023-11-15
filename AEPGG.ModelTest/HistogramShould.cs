using AEPGG.Model;

namespace AEPGG.ModelTest
{
    [Trait("RunsOn", "Remote")]
    public class HistogramShould
    {
        [Fact]
        public void Add_Values()
        {
            Histogram histogram = new(1, 0, 10);
            histogram.Add(.99f); // bin 0
            histogram.Add(1.01f); // bin 1
            histogram.Add(1); // bin 1
            Assert.Equal(1, histogram.BinCounts[0]);
            Assert.Equal(2, histogram.BinCounts[1]);
        }

        [Fact]
        public void ReturnExceedenceProbabilityForAValue()
        {
            Histogram histogram = new(1, 0, 10);
            histogram.Add(.99f); // bin 0
            for(int i =1; i < 10; i++)
            {
                histogram.Add(i);
            }
            Assert.Equal(4.5, histogram.InverseCDF(.5f));
            Assert.Equal(9.5, histogram.InverseCDF(.1f));
        }

        [Fact]
        public void ReturnLastBinValueForExceedenceProbabilityTooSmall()
        {
              Histogram histogram = new(1, 0, 20);
            histogram.Add(.99f); // bin 0
            for (int i = 1; i < 10; i++)
            {
                histogram.Add(i);
            }
            Assert.Equal(9.5, histogram.InverseCDF(.01f));
        }
        [Fact]
        public void ReturnMinValueForElementsDryForAllEvents()
        {
            Histogram histogram = new(1, 0, 20);
            histogram.Add(0f); // bin 0
            Assert.Equal(0, histogram.InverseCDF(.01f));
        }

    }
}
