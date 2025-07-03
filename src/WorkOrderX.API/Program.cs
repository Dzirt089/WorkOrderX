using WorkOrderX.API;


var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddInfrastructureMediatR()
				.AddInfrastructureMailServices();

var app = builder.Build();

app.MapGet("/", () => "WorkOrderX API is running!");

app.AddInfrastructureMapEmployee()
   .AddInfrastructureProcessRequest();



app.Run();