## Migrations commands:
- Create new migration: dotnet ef migrations add -p src/Honeywell.DataAccess/Honeywell.DataAccess.csproj -s src/HoneywellWeb/HoneywellWeb.csproj Initial -o Migrations
- Update database: dotnet ef database update -p src/Honeywell.DataAccess/Honeywell.DataAccess.csproj -s src/HoneywellWeb/HoneywellWeb.csproj -c Honeywell.DataAccess.Data.ApplicationDbContext