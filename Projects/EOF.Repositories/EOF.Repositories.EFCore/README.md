# Generic Repository and Unit of Work NuGet Package

This package provides a reusable and generic implementation of the Repository pattern, along with the Unit of Work pattern, to simplify data access and management in your .NET applications.

## License

This project is licensed under the [MIT License](./LICENSE).

## Features

- **Generic Repository Pattern**: A reusable implementation of the repository pattern to avoid redundant code.
- **Unit of Work**: Manages transaction handling and ensures a consistent and easy-to-use approach for committing changes.

## Installation

You can install the package from NuGet using the .NET CLI:

```bash
dotnet add package EOF.Repositories.EFCore --version 1.0.6

## Usage

Below is an example of how to use the generic repository and unit of work in your project:

```csharp
public class YourService
{
    private readonly IUnitOfWork _unitOfWork;

    public YourService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddEntityAsync(YourEntity entity)
    {
    var yourEntityRepository = uow.GetRepository<YourEntity>();
        await yourEntityRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}
