using WorkOrderX.API;
using WorkOrderX.API.Middlewares;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructureAuthorization(config)
				.AddInfrastructureJsonOptionsWithCompressions()
				.AddInfrastructureDbContext(config)
				.AddServicesAndRepositories()
				.AddInfrastructureMediatRPipeline()
				.AddInfrastructureMailServices()
				.AddInfrastructureHostedServices();

var app = builder.Build();
app.UseResponseCompression(); //middleware сжатия в конвейер обработки запросов
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();



app.MapGet("/", () => "WorkOrderX API is running!");
app.AddInfrastructureAuthEmployee()
   .AddInfrastructureProcessRequest()
   .AddInfrastructureReferenceDatas();

app.Run();