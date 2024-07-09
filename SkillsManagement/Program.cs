using Application.Data; // ������������ ���� ��� IPersonRepository
using Application.Services; // ������������ ���� ��� IPeopleServices � PeopleServices
using Infrastructure.Data; // ������������ ���� ��� ���������� IPersonRepository � ApplicationDbContext
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
            // ������������ Serilog
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
                        // ��������� ������ ����������� �� ������������
                        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                        // ����������� ��������� ���� ������ � �������������� PostgreSQL
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseNpgsql(connectionString));

                        // ����������� �����������
                        services.AddScoped<IPersonRepository, PersonRepository>();

                        // ����������� ���������� ����
                        services.AddScoped<IPeopleServices, PeopleServices>();

                        // ���������� ������������
                        services.AddControllers();

                        // ���������� Swagger ��� ���������������� API
                        services.AddSwaggerGen();
                    })
                    .Configure((context, app) =>
                    {
                        // ������������� Developer Exception Page � Swagger � ������ ����������
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();    
                        }

                        app.UseSwagger();
                        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkillsManagement v1"));

                        // ��������� �������������
                        app.UseRouting();

                        app.UseEndpoints(endpoints =>
                        {
                            // ������������� ������������
                            endpoints.MapControllers();
                        });

                        // ������������� ���� ������
                        using (var scope = app.ApplicationServices.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            db.Database.Migrate(); // ���������� ��������
                        }
                    });
                });
    }
}
