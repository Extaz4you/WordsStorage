
using Repeater.Services;

namespace Repeater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi("repeater");
            builder.Services.AddHttpClient<TopicService>(o=>
            {
                o.BaseAddress = new Uri("http://192.168.200.54:7511/Topic/");
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(o =>
                {
                    o.SwaggerEndpoint("/openapi/repeater.json", "repeater_v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
