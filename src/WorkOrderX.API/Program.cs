using WorkOrderX.API;
using WorkOrderX.API.Middlewares;


var builder = WebApplication.CreateSlimBuilder(args);
var config = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructureAuthorization(config)
				.AddInfrastructureDbContext(config)
				.AddServicesAndRepositories()
				.AddInfrastructureMediatRPipeline()
				.AddInfrastructureMailServices()
				.AddInfrastructureHostedServices();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();



app.MapGet("/", () => "WorkOrderX API is running!");
app.AddInfrastructureAuthEmployee()
   .AddInfrastructureProcessRequest()
   .AddInfrastructureReferenceDatas();

app.Run();