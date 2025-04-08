using QMC.Model.Computers;
using QMC.RasTools;

namespace QMC.Model;

internal class QMCResultsFileWriter
{
    /// <summary>
    /// The file to which we will write. Must be an existing HDF file if using a write to HDF method. Will create the file if not existing if using a write to CSV method.
    /// </summary>
    public string OutputFilePath { get; set; }

    public QMCResultsFileWriter(string outputFilePath)
    {
        OutputFilePath = outputFilePath;
    }

    /// <summary>
    /// Overwrites the max water surface elevation for all 2D cells in the HEC-RAS result file with the results from the project for the specified AEP. Project must have results. Output file must have a matching geometry to the project.
    /// </summary>
    public bool OverwriteMaxWSEinHDFResults(AEPComputer project, float AEP)
    {
        if (!File.Exists(OutputFilePath) && !(Path.GetExtension(OutputFilePath) == ".hdf"))
        {
            throw new Exception("Output file must be an exisitng ras result HDF file.");
        }

        using H5io h5Io = new(OutputFilePath);
        if (project.Geometry.Has2Ds)
        {
            float[][] result = project.GetResultsForAEP2D(AEP); //only using 1 AEP.
            h5Io.OverwriteMaxWSEForAll2DCells( result, project.Geometry.MeshNames);
        }
        if (project.Geometry.HasXSs)
        {
            float[] result = project.GetResultsForAEPXS(AEP);
            h5Io.OverwriteMaxWSEforAllXs(result);
        }
        if (project.Geometry.HasSAs)
        {
            throw new NotImplementedException("Haven't bothered with SA's yet");
        }
        return true;
    }

    /// <summary>
    /// overwrites the timeseries data in the HEC-RAS result file with the results from the project for the specified AEP. Project must have results. Output file must have a matching geometry to the project.
    /// </summary>
    public bool OverwriteTimeseriesInHDFResults(BaseComputer project, float[] AEPs)
    {
        if (!File.Exists(OutputFilePath) && !(Path.GetExtension(OutputFilePath) == ".hdf"))
        {
            throw new Exception("Output file must be an exisitng ras result HDF file.");
        }

        using H5io h5Io = new(OutputFilePath);
        for (int i = 0; i < AEPs.Length; i++)
        {
            if (project.Geometry.Has2Ds)
            {
                float[][] result = project.GetResultsForAEP2D(AEPs[i]);
                h5Io.OverwriteSingleProfile2D(project.Geometry.MeshNames, result, i);
            }
            if (project.Geometry.HasXSs)
            {
                float[] result = project.GetResultsForAEPXS(AEPs[i]);
                h5Io.OverwriteSingleProfileXS(result, i);
            }
            if (project.Geometry.HasSAs)
            {
                throw new NotImplementedException("Haven't bothered with SA's yet");
            }
        }
        return true;
    }

}
