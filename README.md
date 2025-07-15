# WorkOrderX

# Создание новой миграции, <Initial> имя только другое, описывающая действие.
dotnet ef migrations add Initial --project "src\WorkOrderX.EFCoreDb\WorkOrderX.EFCoreDb.csproj" --startup-project  "src\WorkOrderX.API\WorkOrderX.API.csproj"

# После успешного создания миграции, накатывает на БД изменения, обязательно проверить строки подключения в АПИ и в Фабрике DbContext'а
dotnet ef database update --project "src\WorkOrderX.EFCoreDb\WorkOrderX.EFCoreDb.csproj" --startup-project  "src\WorkOrderX.API\WorkOrderX.API.csproj"

# Если надо откатить последние изменения в БД (после UPDATE), также удаляет последнюю миграцию
  dotnet ef migrations remove --project "src\WorkOrderX.EFCoreDb\WorkOrderX.EFCoreDb.csproj" --startup-project  "src\WorkOrderX.API\WorkOrderX.API.csproj" --force
  
# Удаляет последнюю миграцию, использовать, если не накатили её на БД
dotnet ef migrations remove --project "src\WorkOrderX.EFCoreDb\WorkOrderX.EFCoreDb.csproj" --startup-project  "src\WorkOrderX.API\WorkOrderX.API.csproj"