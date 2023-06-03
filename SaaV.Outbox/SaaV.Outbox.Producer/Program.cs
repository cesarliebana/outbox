using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using SaaV.Outbox.Producer.Domain;
using SaaV.Outbox.Producer.Endpoints;
using SaaV.Outbox.Producer.Middlewares;
using SaaV.Outbox.Producer.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add hangfire services
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnectionString"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true,
        PrepareSchemaIfNecessary = true
    }));

builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[] { "outbox-queue" };
    options.WorkerCount = builder.Configuration.GetValue<int>("Hangfire:MaxWorkers");
});

builder.Services.AddDbContext<ProducerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProducerConnectionString")));
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<CreateDummyHandler>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/dummies", DummiesEndpoints.CreateDummy)
    .WithName("CreateDummy")
    .WithSummary("Creates a dummy")
    .Produces<CreateDummyResponse>(StatusCodes.Status201Created)
    .WithOpenApi();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
