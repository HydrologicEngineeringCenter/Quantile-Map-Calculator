# AEP-Grid-Generator
An application that creates AEP Grids from some number of HEC-RAS events.

# Build Instruction
This application is most easily build in Visual Studio. You will need a DotNet 7.0 or later SDK available here: https://dotnet.microsoft.com/en-us/download. 
You will need to add two public NuGet sources to your IDE. This can be done using the Visual Studio GUI - Tools | Options | NuGet Package Manager | Package Sources , or using the commands in a command prompt shown below

        dotnet nuget add source --name fda-nuget "https://www.hec.usace.army.mil/nexus/repository/fda-nuget/"
        dotnet nuget add source --name dss "https://www.hec.usace.army.mil/nexus/repository/dss/"

 ![image](https://github.com/HydrologicEngineeringCenter/AEP-Grid-Generator/assets/64556723/eab99fbf-b630-454a-96ac-5e23ac692440)
