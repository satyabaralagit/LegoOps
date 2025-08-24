
# LegoOps (Clean Architecture)

A .NET 8 Clean Architecture solution that mocks SAP data sources and exposes an aggregation endpoint.

## Projects
- **LegoOps.Domain** — entities, DTOs & value objects
- **LegoOps.Services** — MaterialService (GetMaterialStatusByUnit, GetAllUnitsAndMaterials), interface
- **LegoOps.Infrastructure.MockSap** — in-memory mock providers for SAP Units & its Material/material shortage
- **LegoOps.WebApi** — thin API layer exposing `/api/MaterialPlanner/MaterialShortageOverview/{unitId}` 
and `/api/MaterialPlanner/AllUnitsMaterialOverview`
- **LegoOps.Application.Tests** — xUnit + Moq tests for the Application layer

## Run
```
dotnet build
dotnet run --project src/LegoOps.WebApi
# Swagger at http://localhost:5250/swagger
```

## Test
```
dotnet test
```
