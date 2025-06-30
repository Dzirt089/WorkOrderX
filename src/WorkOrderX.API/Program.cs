using MailerVKT;

using MediatR;

using WorkOrderX.Application.Handlers;
using WorkOrderX.Application.Services.Email.Implementation;
using WorkOrderX.Application.Services.Email.Interfaces;


var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(typeof(ProcessRequestStatusChangedDomainEventHandler).Assembly));

builder.Services.AddScoped<Sender>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();

var app = builder.Build();

app.Run();

