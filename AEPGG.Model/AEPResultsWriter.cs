using System.Linq;

namespace AEPGG.Model;

public static class AEPResultsWriter
{
    /// <summary>
    /// Overwrites the max water surface elevation for all 2D cells in the HEC-RAS result file with the results from the project for the specified AEP. Project must have results. Output file must have a matching geometry to the project.
    /// </summary>
    /// <param name="project"></param>
    /// <param name="outputFilePath"></param>
    /// <param name="AEP"></param>
    public static void OverwriteHDFResults(Project project, string outputFilePath, float AEP)
    {
            float[][] result = project.GetResultsForAEP(AEP); //only using 1 AEP for now. 
            RasTools.OverwriteMaxWSEForAll2DCells(outputFilePath, result, project.Geometry.MeshNames);
    }
}
