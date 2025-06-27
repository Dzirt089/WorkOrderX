using MailerVKT;

using WorkOrderX.Application.Services.Email.Implementation;
using WorkOrderX.Application.Services.Email.Interfaces;


var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddScoped<Sender>();
builder.Services.AddScoped<IMailService, MailService>();

var app = builder.Build();

app.Run();

