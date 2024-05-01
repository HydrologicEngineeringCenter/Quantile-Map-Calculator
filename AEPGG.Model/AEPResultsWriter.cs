using System.Linq;

namespace AEPGG.Model;

public class AEPResultsWriter
{
    /// <summary>
    /// The file to which we will write. Must be an existing HDF file if using a write to HDF method. Will create the file if not existing if using a write to CSV method.
    /// </summary>
    public string OutputFilePath { get; set; }

    public AEPResultsWriter(string outputFilePath)
    {
        OutputFilePath = outputFilePath;
    }


    /// <summary>
    /// Overwrites the max water surface elevation for all 2D cells in the HEC-RAS result file with the results from the project for the specified AEP. Project must have results. Output file must have a matching geometry to the project.
    /// </summary>
    /// <param name="project"></param>
    /// <param name="outputFilePath"></param>
    /// <param name="AEP"></param>
    public bool OverwriteMaxWSEinHDFResults(AEPComputer project, float AEP)
    {
        bool sucess = false;
        if (File.Exists(OutputFilePath) && Path.GetExtension(OutputFilePath) == "hdf")
        {
            float[][] result = project.GetResultsForAEP(AEP); //only using 1 AEP for now. 
            RasTools.OverwriteMaxWSEForAll2DCells(OutputFilePath, result, project.Geometry.MeshNames);
            sucess = true;
        }
        return sucess;

    }

    public bool OverwriteTimeseriesInHDFResults(AEPComputer project, float AEP)
    {
        bool sucess = false;
        if (File.Exists(OutputFilePath) && Path.GetExtension(OutputFilePath).Equals(".hdf"))
        {
            float[][] results = project.GetResultsForAEP(AEP);
            RasTools.OverwriteSingleProfile(OutputFilePath, project.Geometry.MeshNames, results );
            sucess = true;
        }
        return sucess;
    }

}
