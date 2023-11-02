using H5Assist;
using Ras.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEPGG.Model
{
    public class Project
    {
        public string Name { get;}
        public float[] Probabilities { get;}
        public Histogram[] Histograms { get; private set;}
        public float BinWidth { get; }
        public int NumBins { get; }
        public string MeshName { get; set; }//fuck this 
        public Project(string name, float[] probabilities, float binWidth, int numBins, string meshName) //dont want to keep mesh name. 
        {
            Name = name;
            Probabilities = probabilities;
            NumBins = numBins;
            BinWidth = binWidth;
            MeshName = meshName;
        }

        public void AddResults(string pathToResultHDF, string meshName) // don't want to keep mesh name as a parameter. Solve this problem later. 
        {
            //If this is the first result, initialize the histos. 
            if (Histograms[0] == null)
                InitializeHistograms(pathToResultHDF, meshName);

            //Do the thing. 
            float[] data = RasHelper.GetWSEsForAllNodes(pathToResultHDF, meshName);
            for (int i = 0; i < data.Length; i++)
            {
                Histograms[i].Add(data[i]);
            }
        }

        private void InitializeHistograms(string pathToResultHDF, string meshName)
        {
            float[] data = RasHelper.GetMinWSEForAllNodes(pathToResultHDF, meshName);
            for (int i = 0; i < data.Length; i++)
            {
                Histograms[i] = new(BinWidth, data[i], NumBins);
            }
        }

        public void SaveResults(string saveToFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
