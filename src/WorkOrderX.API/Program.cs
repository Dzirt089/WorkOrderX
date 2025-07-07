using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;

using WorkOrderX.API;


var builder = WebApplication.CreateSlimBuilder(args);
var confic = builder.Configuration;
// Политики авторизации по полям
builder.Services.AddAuthorizationBuilder()
	.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Supervisor"))
	.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"))
	.AddPolicy("ExecuterOnly", policy => policy.RequireRole("Executer"));

// Регистрация аутентификации JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.TokenValidationParameters = new TokenValidationParameters
		{
			// указывает, будет ли валидироваться издатель при валидации токена
			ValidateIssuer = true,
			// строка, представляющая издателя
			ValidIssuer = confic["JwtSettings:Issuer"],
			// будет ли валидироваться потребитель токена
			ValidateAudience = true,
			// установка потребителя токена
			ValidAudience = confic["JwtSettings:Audience"],
			// будет ли валидироваться время существования
			ValidateLifetime = true,
			// установка ключа безопасности
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confic["JwtSettings:SecretKey"])),
			// валидация ключа безопасности
			ValidateIssuerSigningKey = true,
		};
	});

builder.Services.AddInfrastructureMediatR()
				.AddInfrastructureMailServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();



app.MapGet("/", () => "WorkOrderX API is running!");

app.AddInfrastructureAuthEmployee()
   .AddInfrastructureProcessRequest();

app.Run();