﻿namespace QPC.Model.Interfaces;

/// <summary>
/// This interface exists solely to mock in data for unit testing. 
/// </summary>
public interface IHydraulicResults
{
    //not making these properties because I don't want to query the mesh names for every result file. They'll all be the same. Want to hand in mesh names.  
    public float[][] GetMax2DWSE(string[] meshNames);
    public float[][] GetMin2DWSE(string[] meshNames);
    public float[] GetMaxXSWSE();
    public float[] GetMinXSWSE();
    float[][] Get2DWSE(int profileOfInterest, string[] meshNames);
    float[] GetXSWSE(int profileOfInterest);

    /// <summary>
    /// File Path to an HEC-RAS 6.x unsteady hdf result. 
    /// </summary>
    public string FilePath { get; }

}