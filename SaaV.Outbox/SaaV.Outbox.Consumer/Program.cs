using Microsoft.EntityFrameworkCore;
using SaaV.Outbox.Consumer.Domain;
using SaaV.Outbox.Consumer.Events;
using SaaV.Outbox.Consumer.MessageBroker;
using SaaV.Outbox.Consumer.Persistence;
using SaaV.Outbox.Consumer.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<RabbitMQSettings>().BindConfiguration("RabbitMQ").ValidateDataAnnotations();

builder.Services.AddDbContext<ConsumerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConsumerConnectionString")));
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<CreateDummyHandler>());
builder.Services.AddSingleton<IMessageBroker, RabbitMQClient>();

builder.Services.AddHostedService<EventsHandlerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/test", () => Results.NoContent())
.WithName("test")
.WithOpenApi();

app.Run();

