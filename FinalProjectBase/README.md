# Data Processing and Quality Platform

**Student:** Ujwal Shyam Kantimohanthy  
**Student ID:** 0301643  
**Technology:** C# / .NET 10 / Entity Framework Core / SQLite  
**Application type:** Console application

## 1. Project Objective

The Data Processing and Quality Platform loads customer data from CSV files, validates its structure and contents, profiles its columns, detects data-quality problems, applies transformations, calculates quality scores, exports cleaned datasets and stores processing history in SQLite.

The project demonstrates C# collections, LINQ, object-oriented programming, SOLID principles, MVC, custom exceptions, Entity Framework Core, unit testing and multiple design patterns.

## 2. Main Features

- Load a customer dataset from CSV.
- Validate file existence, required columns and row structure.
- Display a preview of the first 10 records.
- Generate column-level dataset statistics.
- Run configurable data-quality rules.
- Calculate dataset quality scores.
- Apply transformations in insertion order.
- Remove duplicate and invalid records.
- Undo transformations using saved snapshots.
- Create, read, update and delete quality-rule configurations.
- Store processing runs and quality issues in SQLite.
- Export cleaned datasets to CSV and JSON.
- Display previous processing history.
- Notify observers when pipeline steps start, complete or fail.

## 3. Required Dataset Columns

The input CSV must contain these columns:

```text
CustomerId
Name
Email
Age
Country
SignupDate
TotalSpent
```

The project uses a simple CSV format:

- Comma-separated values
- One header row
- One data record per line
- No multiline fields
- No quoted commas

## 4. Solution Structure

```text
FinalProjectBase
├── Data
│   ├── Input
│   └── Output
├── src
│   └── DataQualityPlatform
│       ├── Configuration
│       ├── Controllers
│       ├── DataSources
│       ├── Exceptions
│       ├── Exporters
│       ├── Factories
│       ├── Models
│       ├── Observers
│       ├── Persistence
│       ├── Quality
│       ├── Repositories
│       ├── Services
│       ├── Transformations
│       ├── Views
│       └── Program.cs
├── tests
│   └── DataQualityPlatform.Tests
├── DataQualityPlatform.sln
└── README.md
```

## 5. Architecture

The application follows the Model-View-Controller pattern.

### Model

The Models and Persistence entities hold application data, including:

- `Dataset`
- `DataRecord`
- `ColumnDefinition`
- `QualityIssue`
- `QualityReport`
- `DatasetProfile`
- `ProcessingRunEntity`
- `QualityRuleConfigurationEntity`

### View

`ConsoleView` handles console input and output. It displays menus, dataset previews, profiles, quality reports, execution history and quality-rule configurations.

### Controller

`DataProcessingController` coordinates the complete workflow. It delegates data loading, profiling, validation, transformation, persistence and exporting to specialised components.

## 6. Design Patterns

### Factory Pattern

`QualityRuleFactory` creates the correct `IQualityRule` implementation from a `QualityRuleConfiguration`.

Supported rules:

- `MissingValueRule`
- `UniqueValueRule`
- `RangeRule`
- `RegexRule`
- `DateFormatRule`

This removes rule-construction logic from the controller.

### Observer Pattern

`ProcessingPipeline` notifies registered `IPipelineObserver` implementations when a transformation:

- Starts
- Completes successfully
- Fails

`ConsolePipelineObserver` displays pipeline progress without coupling transformations to the console.

### Adapter Pattern

`LegacyCsvReaderAdapter` converts the incompatible `LegacyCsvReader` interface into the `IDataSource` interface expected by the application.

This allows legacy CSV-reading functionality to be used without changing the controller.

### Singleton Pattern

`ApplicationConfiguration` provides one shared configuration instance through its `Instance` property.

### Repository Pattern

Repository interfaces separate persistence logic from the controller:

- `IProcessingRunRepository`
- `IQualityRuleRepository`

Their Entity Framework implementations handle SQLite CRUD operations.

## 7. SOLID Principles

- **Single Responsibility:** Loading, validation, transformation, profiling, exporting, persistence and display are handled by separate classes.
- **Open/Closed:** New quality rules, transformations, exporters and observers can be added without rewriting existing implementations.
- **Liskov Substitution:** Implementations can be used through interfaces such as `IQualityRule`, `IDataTransformation`, `IDataExporter` and `IDataSource`.
- **Interface Segregation:** Interfaces expose only the operations needed by their clients.
- **Dependency Inversion:** The controller depends on abstractions and receives dependencies through constructor injection.

## 8. Quality Rules

### Missing Value Rule

Detects null, empty or whitespace-only required values.

### Unique Value Rule

Keeps the first occurrence of a value as valid and reports every later duplicate occurrence. Missing values are ignored.

### Range Rule

Uses invariant-culture decimal parsing and validates values against inclusive minimum and maximum limits.

### Regex Rule

Validates non-missing values against a configured regular expression.

### Date Format Rule

Requires dates to use the exact format:

```text
yyyy-MM-dd
```

## 9. Transformations

The configured processing pipeline executes:

1. `TrimStringTransformation`
2. `NormalizeCountryTransformation`
3. `RemoveDuplicateTransformation`
4. `RemoveInvalidRecordTransformation`

Transformations return new dataset objects to avoid unexpected modification of the original dataset.

`TransformationHistory` stores snapshots using a stack, allowing transformations to be undone in last-in, first-out order.

## 10. Database

The application uses SQLite through Entity Framework Core.

Database filename:

```text
data-quality-platform.db
```

The database is created automatically at application startup using:

```csharp
context.Database.EnsureCreated();
```

Stored information includes:

- Processing runs
- Processing status
- Input and output record counts
- Quality scores
- Quality issues
- Quality-rule configurations

Quality-rule configurations support Create, Read, Update and Delete operations through the console menu.

## 11. Build Instructions

From the solution directory, run:

```powershell
dotnet restore
dotnet build "DataQualityPlatform.sln"
```

## 12. Run Instructions

```powershell
dotnet run --project ".\src\DataQualityPlatform\DataQualityPlatform.csproj"
```

The application displays the following options:

```text
1. Load dataset
2. Show preview
3. Show profile
4. Run quality checks
5. Apply transformations
6. Show quality report
7. Export dataset
8. Show execution history
9. Undo last transformation
10. Manage quality rules
0. Exit
```

## 13. Test Instructions

Run all automated tests with:

```powershell
dotnet test "DataQualityPlatform.sln"
```

Verified result:

```text
Total tests: 34
Passed: 34
Failed: 0
Skipped: 0
```

The tests cover:

- CSV loading and validation
- Quality rules
- Quality score calculation
- Rule factory
- Dataset transformations
- Processing pipeline
- Observer notifications
- Entity Framework repository operations

## 14. Exported Files

Example cleaned outputs are stored in:

```text
Data/Output/cleaned-customers.csv
Data/Output/cleaned-customers.json
```

The CSV exporter writes headers followed by one line per record.

The JSON exporter serializes the complete dataset, including columns and records, using indented JSON.

## 15. Exception Handling

The application uses validation and custom exceptions for failures including:

- Missing dataset files
- Invalid CSV structure
- Missing columns
- Unsupported quality-rule types
- Pipeline transformation failures

The controller catches workflow errors and displays clear messages without terminating the application unexpectedly.

## 16. Final Verification

Before submission, the following were verified:

- The solution builds successfully.
- All 34 automated tests pass.
- CSV loading works through the adapter.
- Dataset preview and profiling work.
- Quality checks and transformations work.
- CSV and JSON exports are generated.
- Processing history is stored in SQLite.
- Transformation undo works.
- Quality-rule CRUD operations work.