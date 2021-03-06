## AsyncRedux


[![NuGet](https://img.shields.io/nuget/v/AsyncRedux.svg)](https://www.nuget.org/packages/AsyncRedux)
[![Build status](https://ci.appveyor.com/api/projects/status/6xk2xyqm0049oexk?svg=true)](https://ci.appveyor.com/project/TAGC/asyncredux)

AsyncRedux is an asynchronous C# port of the fantastic [Redux library](https://github.com/reactjs/redux). A Redux C# port already exists as [redux.NET](https://github.com/GuillaumeSalles/redux.NET) and this project has drawn heavily from that. However, it's 2018 and I needed something that played well with asynchronous codebases and supported .NET Standard.

### Installation

AsyncRedux targets the .NET Standard and can be used within .NET Core and .NET Framework applications. It is available on the standard NuGet feed and can be installed from there. For example, using the dotnet CLI:

```
dotnet add package AsyncRedux
```