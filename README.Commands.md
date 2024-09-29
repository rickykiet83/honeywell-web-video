## Migrations commands:
- Create new migration: ```dotnet ef migrations add -p src/Domain/Honeywell.DataAccess/Honeywell.DataAccess.csproj -s src/Application/HoneywellWeb/HoneywellWeb.csproj Initial -o Persistences/Migrations```
- Update database: ```dotnet ef database update -p src/Domain/Honeywell.DataAccess/Honeywell.DataAccess.csproj -s src/Application/HoneywellWeb/HoneywellWeb.csproj -c Honeywell.DataAccess.Data.ApplicationDbContext```


## Running Tests:
Use the following command to run the unit tests:
```bash
dotnet test
```
The unit tests use:
•	xUnit for test execution.
•	NSubstitute for mocking dependencies.
•	FluentAssertions for readable assertions.

# Generate Code Coverage Report
- Run the following command to generate the code coverage report:
```bash
dotnet tool install --global dotnet-reportgenerator-globaltool

dotnet test --collect:"XPlat Code Coverage"

reportgenerator "-reports:test/**/coverage.cobertura.xml" "-targetdir:coverage-report" "-reporttypes:Html"
```
- The code coverage report will be available in the `TestResults` folder and coverage-report folder.
- Open the `index.html` file in the coverage-report folder to view the code coverage report.