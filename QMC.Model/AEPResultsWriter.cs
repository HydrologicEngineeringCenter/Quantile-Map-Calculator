﻿using QPC.Model.Computers;
using QPC.Model.RasTools;

namespace QPC.Model;

internal class AEPResultsWriter
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
        if (!File.Exists(OutputFilePath) && !(Path.GetExtension(OutputFilePath) == ".hdf"))
        {
            return false;
        }
        if (project.Geometry.Has2Ds)
        {
            float[][] result = project.GetResultsForAEP2D(AEP); //only using 1 AEP.
            H5WriterTools.OverwriteMaxWSEForAll2DCells(OutputFilePath, result, project.Geometry.MeshNames);
        }
        if (project.Geometry.HasXSs)
        {
            float[] result = project.GetResultsForAEPXS(AEP);
            H5WriterTools.OverwriteMaxWSEforAllXs(OutputFilePath, result);
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
            return false;
        }
        for (int i = 0; i < AEPs.Length; i++)
        {
            if (project.Geometry.Has2Ds)
            {
                float[][] result = project.GetResultsForAEP2D(AEPs[i]);
                H5WriterTools.OverwriteSingleProfile2D(OutputFilePath, project.Geometry.MeshNames, result, i);
            }
            if (project.Geometry.HasXSs)
            {
                float[] result = project.GetResultsForAEPXS(AEPs[i]);
                H5WriterTools.OverwriteSingleProfileXS(OutputFilePath, result, i);
            }
            if (project.Geometry.HasSAs)
            {
                throw new NotImplementedException("Haven't bothered with SA's yet");
            }
        }
        return true;
    }

}
