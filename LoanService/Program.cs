using LoanService.Application.Commands;
using LoanService.Application.Interfaces;
using LoanService.Application.Queries;
using LoanService.Infrastructure.Messaging;
using LoanService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Base de datos préstamos ────────────────────────────────────────────────────
builder.Services.AddDbContext<LoanDbContext>(options =>
    options.UseInMemoryDatabase("LoanServiceDb"));

// ── Base de datos académica ───────────────────────────────────────────────────
builder.Services.AddDbContext<AcademicDbContext>(options =>
    options.UseInMemoryDatabase("AcademicDb"));

// ── Repositorio ────────────────────────────────────────────────────────────────
builder.Services.AddScoped<ILoanRepository, LoanRepository>();

// ── Handlers (CQRS manual) ────────────────────────────────────────────────────
builder.Services.AddScoped<CreateLoanHandler>();
builder.Services.AddScoped<ReturnLoanHandler>();
builder.Services.AddScoped<GetLoanHandler>();

// ── Bus de eventos (MassTransit + RabbitMQ) ───────────────────────────────────
var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
var rabbitUser = builder.Configuration["RabbitMQ:Username"] ?? "guest";
var rabbitPass = builder.Configuration["RabbitMQ:Password"] ?? "guest";

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LoanCreatedConsumer>();
    x.AddConsumer<BookReturnedConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });

        cfg.ReceiveEndpoint("loan-created-queue", e =>
            e.ConfigureConsumer<LoanCreatedConsumer>(ctx));

        cfg.ReceiveEndpoint("book-returned-queue", e =>
            e.ConfigureConsumer<BookReturnedConsumer>(ctx));
    });
});

builder.Services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

// ── API ────────────────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "LoanService + Academic API",
        Version     = "v1",
        Description = "Microservicio de préstamos y módulo académico – UAC"
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoanService + Academic v1"));

app.MapControllers();
app.Run();

public partial class Program { }
