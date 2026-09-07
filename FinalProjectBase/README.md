# Data Processing and Quality Platform

## 1. Project Objective

This starter solution is a console application for Data Analyst, Data Engineer, and Data Scientist students. It is designed for a final exam project that evaluates C# fundamentals, collections, OOP, abstraction, inheritance, overriding, polymorphism, UML relationships, MVC, design patterns, exceptions, Entity Framework Core, and unit testing.

The starter solution is intentionally incomplete. A successful build does not mean that the project requirements are complete.

Students must complete the methods marked with `TODO(STUDENT)` and `throw new NotImplementedException("TODO: Student implementation.");`.

## 2. Solution Structure

- `src/DataQualityPlatform`: console application.
- `tests/DataQualityPlatform.Tests`: visible xUnit tests and CSV fixtures.
- `Data/Input`: place the final dataset here.
- `README.md`: project instructions.

The project uses `.NET 10` and targets `net10.0`.

## 3. Restore Packages

```bash
dotnet restore
```

## 4. Build

```bash
dotnet build
```

## 5. Run

```bash
dotnet run --project src/DataQualityPlatform
```

## 6. Run Tests

```bash
dotnet test
```

The mandatory tests compile immediately, but many tests fail at first because the starter project intentionally leaves business logic unimplemented.

## 7. Dataset Location

Place the provided final dataset at:

```text
Data/Input/customers.csv
```

The expected CSV format is simple:

- comma separator;
- first line contains headers;
- no multiline fields;
- no quoted commas.

Do not use a third-party CSV parser.

## 8. Files Students Must Not Modify

Students must not modify:

- public method signatures;
- interfaces used by tests;
- mandatory tests;
- test fixture files.

## 9. What Students May Add

Students may add classes and methods when needed, as long as the required public APIs continue to work and the architecture remains clear.

## 10. Architecture Requirements

MVC is mandatory:

- Models hold data.
- Views handle console input and output only.
- Controllers coordinate workflows and delegate business operations to services.

`Program.cs` must only create dependencies, configure the pipeline, create the controller, and call `controller.Run()`.

## 11. Design Pattern Requirements

Factory is mandatory through `QualityRuleFactory`.

At least one additional pattern must be implemented. The starter provides Observer extension points through `IPipelineObserver` and `ConsolePipelineObserver`, and a Singleton example through `ApplicationConfiguration`.

## 12. Testing Guidance

The provided tests are not exhaustive. Additional teacher tests will use different datasets and will verify behavior, not hard-coded answers.

Hard-coded solutions will not pass additional tests.

## 13. Grading Guidance

- Project does not compile: maximum 20/100.
- Project compiles but fewer than 50% of mandatory tests pass: maximum 40/100.
- At least 50% of mandatory tests pass: normal grading applies.
- All mandatory tests pass: eligible for the full 100 points.

## 14. Main Student Implementation Points

Students are expected to implement:

- CSV loading and validation in `CsvDataSource`.
- quality rules in `Quality/*Rule.cs`.
- transformations in `Transformations`.
- profiling, quality analysis, scoring, and pipeline execution in `Services`.
- repository CRUD operations in `EfProcessingRunRepository`.
- factory creation logic in `QualityRuleFactory`.
- controller workflow methods in `DataProcessingController`.
- exporters in `Exporters`.
