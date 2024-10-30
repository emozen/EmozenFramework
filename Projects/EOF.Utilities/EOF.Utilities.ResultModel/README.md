# EmozenFramework

A brief description of your library. This library provides a standardized way to handle results and errors in asynchronous operations using interfaces and concrete implementations.

## Installation

You can install the package from NuGet using the .NET CLI:

```bash
dotnet add package EOF.Utilities.ResultModel --version 1.0.1

## Usage

Below is an example of how to use the generic repository and unit of work in your project:

```csharp
public async Task<IEOFResult> TestAsync()
{
    try
    {
        var response = await TestMethodAsync();
        return new SuccessResult();
        or
        return new SuccessDataResult<YourModel>(response, "success"));
    }
    catch(Exception ex)
    {
        return new ErrorResult("Custom error message");
        or
        return new ErrorDataResult<YourModel>(ex,"Custom error message");
    }
}
