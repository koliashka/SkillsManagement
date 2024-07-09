using Application.Data; // Пространство имен для IPersonRepository
using Application.Services; // Пространство имен для IPeopleServices и PeopleServices
using Infrastructure.Data; // Пространство имен для реализации IPersonRepository и ApplicationDbContext
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillsManagement.Data;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Hosting;


namespace SkillsManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Конфигурация Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(
                    new CompactJsonFormatter(),
                    "Logs/logs.json",
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 5)
                .CreateLogger();

            try
            {
                Log.Information("Starting up the host. Waiting DB for 10 sec.");
                var host = CreateHostBuilder(args).Build();
                Task.Delay(10000).Wait();
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        // Получение строки подключения из конфигурации
                        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                        // Регистрация контекста базы данных с использованием PostgreSQL
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseNpgsql(connectionString));

                        // Регистрация репозитория
                        services.AddScoped<IPersonRepository, PersonRepository>();

                        // Регистрация сервисного слоя
                        services.AddScoped<IPeopleServices, PeopleServices>();

                        // Добавление контроллеров
                        services.AddControllers();

                        // Добавление Swagger для документирования API
                        services.AddSwaggerGen();
                    })
                    .Configure((context, app) =>
                    {
                        // Использование Developer Exception Page и Swagger в режиме разработки
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();    
                        }

                        app.UseSwagger();
                        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkillsManagement v1"));

                        // Настройка маршрутизации
                        app.UseRouting();

                        app.UseEndpoints(endpoints =>
                        {
                            // Маршрутизация контроллеров
                            endpoints.MapControllers();
                        });

                        // Инициализация базы данных
                        using (var scope = app.ApplicationServices.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            db.Database.Migrate(); // Применение миграций
                        }
                    });
                });
    }
}
