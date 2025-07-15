# WorkOrderX

# �������� ����� ��������, <Initial> ��� ������ ������, ����������� ��������.
dotnet ef migrations add Initial --project "src\WorkOrderX.EFCoreDb\WorkOrderX.EFCoreDb.csproj" --startup-project  "src\WorkOrderX.API\WorkOrderX.API.csproj"

# ����� ��������� �������� ��������, ���������� �� �� ���������, ����������� ��������� ������ ����������� � ��� � � ������� DbContext'�
dotnet ef database update --project "src\WorkOrderX.EFCoreDb\WorkOrderX.EFCoreDb.csproj" --startup-project  "src\WorkOrderX.API\WorkOrderX.API.csproj"

# ���� ���� �������� ��������� ��������� � �� (����� UPDATE), ����� ������� ��������� ��������
  dotnet ef migrations remove --project "src\WorkOrderX.EFCoreDb\WorkOrderX.EFCoreDb.csproj" --startup-project  "src\WorkOrderX.API\WorkOrderX.API.csproj" --force
  
# ������� ��������� ��������, ������������, ���� �� �������� � �� ��
dotnet ef migrations remove --project "src\WorkOrderX.EFCoreDb\WorkOrderX.EFCoreDb.csproj" --startup-project  "src\WorkOrderX.API\WorkOrderX.API.csproj"