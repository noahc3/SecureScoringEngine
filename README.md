# Secure Scoring Engine

An alternative, platform agnostic scoring engine for cyber security competitions with security and extensibility in mind. Written with .NET Core 3.0.

## What is it

Secure Scoring Engine (SSE) is a scoring engine for cyber security competitions implemented with a client-server model. The engine tracks and scores security vulnerabilities on competitor virtual machines with an easy-to-extend scripting backend. The goal of SSE is to offer a cross-platform client service which can all connect to a common server. SSE also focuses on being secure, leaving no scoring information on the client and implementing a server-based anticheat. Hacked clients should not be practical with a properly configured server. The server can also be run locally inside of the competitor virtual machine if security is not a concern and standalone images are desired for practice, for example.

## Features
* General features
    * Server supports as many teams and scored images as your server can handle.
    * All scored items are fully customizable through C# scripting.
    * Teams are identified with configurable unique identifiers.
        * Each team can have a different set of images available to them. This allows for multiple tiers, for example.
* User Interface
    * Template "Scoring Report" files can be created with special tokens which are replaced by the client using information provided by the server to generate up-to-date, nicely formatted reports. These scoring reports are automatically saved to a configurable location on competitor virtual machines.
    * README files can be specified for each individual scored virtual machine which are automatically downloaded by the client to a configurable location.
    * Only interaction required by competitors is to input their unique team ID and to stop scoring at the end of their run time. Everything else is configured prior to the competition.
    * Competitors are notified with desktop toast notifications when they gain or lose points, or when the client is having issues connecting to the server.
* Extensibility
    * All scored items are simple C# scripts being executed on the client and server. 
        1. Server tells client to execute a script.
        2. Client sends scripts output back to server.
        3. Server script checks the output for the desired result.
    * Scripts do not need to be compiled.
    * C# scripts can act as bootstrappers for other binaries or commands if necessary.
    * Scripts can be added, edited, or removed while a competition is underway in case bugs are discovered. <sup>[p]</sup>
* Security
    * All sensitive client-server communications are encrypted to prevent man-in-the-middle attacks and information leaks.
    * Server actively checks for out of line communications by the client to resist custom clients, if enabled. <sup>[p]</sup>
    * Additional anticheat modules are available to prevent client modifications. <sup>[p]</sup>
* Debug mode for client service available for inital setup and debugging of virtual machines and scoring payloads. <sup>[p]</sup> Client must be compiled from source with the `DEBUG` flag defined, server can enter debug mode through the configuration file. <sup>[p]</sup>

<sup>[p]: planned</sup>


## Platform Support

The scoring engine client service is verified working on Windows 7, Windows 10, Ubuntu 16, Ubuntu 18, and Debian 9. It should work on any other Windows or Linux platform that .NET Core 3.0 officially supports.

The server software is currently Windows only due to encryption currently relying on Windows' proprietary CNG crypto API. Linux support will be added in the near future.

All other tools and utilities are designed to be Windows only but may work on other platforms through Mono.

## Included projects

#### SSEService
Client service installed on competitor virtual machines. Facilitates client-side scoring operations and user interface.

#### SSEBackend
Server software which clients connect to. Most configuration is done for this software.

#### SSECommon
Utility code used across two or more SSE projects.

#### SSE Configuration Tool
Tool for configuring virtual machines and scored items. Not ready for use.

## Building

.NET Core 3.0 (currently in preview) is required to build this project. Visual Studio 2019 is currently required for debugging until .NET Core 3.0 is officially released.

To create binaries for use in real competitions, run the following command with .NET Core 3.0 CLI tools in the directory of the project you are trying to build, or the solution root to build all projects:
```dotnet publish -c Release -r <runtime id> --self-contained```
where <runtime id> is the RID for the target platform (see: [.NET Core RID catalogue](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)). The `--self-contained` flag will ensure all .NET runtime dependencies are packaged alongside the executable.

Published binaries will have a lot of files, many of which are actually unneccessary. You can use a tool like [warp](https://github.com/dgiagio/warp) to bundle all of the files into a single portable executable.

## Runtime Dependencies

These are the additional dependencies not present on default operating system installations that must be installed inside competitor VMs for the scoring engine to function correctly.

### Basic dependencies for any functionality:

**Windows**
* Microsoft Visual C++ 2015 Redistributable Update 3 x86 <sup>[.net]</sup>
* Microsoft Visual C++ 2015 Redistributable Update 3 x64 <sup>[.net]</sup>
* `notifu64.exe` in path or next to the executable <sup>[sse]</sup>
* Windows 7 Only: KB2999226 <sup>[.net]</sup>
* Windows 7 Only: KB2533623 <sup>[.net]</sup>

**Linux**
* `notify-send` in path <sup>[sse]</sup>
* `su` in path <sup>[sse]</sup>

### Dependencies to use provided scored item templates

**Windows**
* `PsExec64.exe` in path or next to the executable <sup>[sse]</sup>

**Linux**
* None

Note: PsExec64 can easily be seen as a vulnerable application by competitors. You should convey that it is required by the scoring engine to function so they do not remove it, or put it in the scoring engine install directory.

<sup>[.net] - .NET Core 3.0 requirement | [sse] - SSE requirement</sup>

## License

All source code files in this repository are licensed under the Mozilla Public License 2.0. See [LICENSE.md](https://github.com/noahc3/SecureScoringEngine/blob/master/LICENSE.md) for details.
