# ics0010-23f

# Daniil Fetissov
## 223367IVSB
## dafeti@taltech.ee
## dafeti






```
dotnet tool update --global dotnet-ef

dotnet ef migrations add --project DAL --startup-project ConsoleApp InitialCreate
dotnet ef migrations add --project DAL --startup-project WApp InitialCreate

dotnet aspnet-codegenerator razorpage -m Domain.Database.Game -dc AppDbContext -udl -outDir Pages/Games --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Database.Player -dc AppDbContext -udl -outDir Pages/Players --referenceScriptLibraries
```

