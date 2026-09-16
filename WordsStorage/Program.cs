
using Microsoft.EntityFrameworkCore;
using Serilog;
using WordsStorage.Database;
using WordsStorage.Services;

namespace WordsStorage;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<StorageContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
        builder.Services.AddScoped<TopicRepository>();
        builder.Services.AddScoped<WordRepository>();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Seq("http://192.168.200.54:7515");
        });

        //builder.WebHost.UseUrls("https://192.168.200.54:7511");
        var app = builder.Build();

        app.UseExceptionHandler();
        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
