using WorkOrderX.API;


var builder = WebApplication.CreateSlimBuilder(args);
var config = builder.Configuration;


builder.Services.AddInfrastructureAuthorization(config)
				.AddInfrastructureDbContext(config)
				.AddInfrastructureMediatR()
				.AddInfrastructureMailServices()
				.AddInfrastructureHostedServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();



app.MapGet("/", () => "WorkOrderX API is running!");

app.AddInfrastructureAuthEmployee()
   .AddInfrastructureProcessRequest();

app.Run();