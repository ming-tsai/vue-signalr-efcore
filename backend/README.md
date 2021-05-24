# Backend

## Prerequisities
- [DotNet 5](https://dotnet.microsoft.com/download/dotnet/5.0)
- [EF Core tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

## Installation
1. Install all [prerequisities](prerequisities)
2. Go to directory on terminal `backend`
3. Create a file with name `appsettings.Development.json` on the `backend` folder and with context as below
```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "ConnectionStrings": {
        "SqlServer" : "Server=<server>;Database=<database-name>;User Id=<username>;Password=<password>;"
    }
}
```
3. Update database inside of the `backend` folder
```bash
dotnet ef database update -p ./src/SignalR.API
```
4. Run as debug the project and you will see it on endpoint https://localhost:5001
