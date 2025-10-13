using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Clients;
using OrderService.API.Mappings;
using OrderService.API.Services;
using OrderService.DataAccess.Postgres;
using Refit;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        // Старт
        var builder = WebApplication.CreateBuilder(args);

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // DbContext
        var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
        builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(conn));
        builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        // MediatR
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        // FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // AutoMapper
        // builder.Services.AddAutoMapper(typeof(OrderMappingProfile).Assembly);

        // Kafka
        builder.Services.AddSingleton<KafkaProducer>();

        // Подключение контроллеров
        builder.Services.AddControllers();

        // Клиент платежного сервиса
        var paymentBaseUrl = builder.Configuration["Services:PaymentService"];
        if (string.IsNullOrEmpty(paymentBaseUrl)) throw new InvalidOperationException("Payment service URL is not configured");

        builder.Services.AddRefitClient<IPaymentClient>().ConfigureHttpClient(c => c.BaseAddress = new Uri(paymentBaseUrl));

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ошибка применения миграций во время старта");
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler("/error");

        app.UseHttpsRedirection();
        app.MapControllers();

        //app.Urls.Add("http://0.0.0.0:80");

        app.Run();

    }
}
