using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;

using WorkOrderX.API;


var builder = WebApplication.CreateSlimBuilder(args);
var confic = builder.Configuration;
// �������� ����������� �� �����
builder.Services.AddAuthorizationBuilder()
	.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Supervisor"))
	.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"))
	.AddPolicy("ExecuterOnly", policy => policy.RequireRole("Executer"));

// ����������� �������������� JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.TokenValidationParameters = new TokenValidationParameters
		{
			// ���������, ����� �� �������������� �������� ��� ��������� ������
			ValidateIssuer = true,
			// ������, �������������� ��������
			ValidIssuer = confic["JwtSettings:Issuer"],
			// ����� �� �������������� ����������� ������
			ValidateAudience = true,
			// ��������� ����������� ������
			ValidAudience = confic["JwtSettings:Audience"],
			// ����� �� �������������� ����� �������������
			ValidateLifetime = true,
			// ��������� ����� ������������
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confic["JwtSettings:SecretKey"])),
			// ��������� ����� ������������
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