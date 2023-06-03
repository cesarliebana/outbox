using Microsoft.EntityFrameworkCore;
using SaaV.Outbox.Consumer.Domain;
using SaaV.Outbox.Consumer.Events;
using SaaV.Outbox.Consumer.MessageBroker;
using SaaV.Outbox.Consumer.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ConsumerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConsumerConnectionString")));
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<CreateDummyHandler>());
builder.Services.AddSingleton<IMessageBroker, RabbitMQClient>(sp => new RabbitMQClient(builder.Configuration["RabbitMQ:HostName"]));

builder.Services.AddHostedService<CreateDummyEventHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/test", () => Results.NoContent())
.WithName("test")
.WithOpenApi();

app.Run();

