using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Polly;
using Application.Services;
using SkillsManagement.ExceptionHandlers;
using Infrastracture.Persistence.Context;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
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

builder.Host.UseSerilog();

try
{
    // Configure services
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));

    builder.Services.AddScoped<IPersonRepository, PersonRepository>();
    builder.Services.AddScoped<IPeopleServices, PeopleServices>();
    builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
    builder.Services.AddSwaggerGen();
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else 
    {
        app.UseExceptionHandler(_ => { });
    }
    
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkillsManagement v1"));
    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    // Apply migrations with retry policy
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Policy
            .Handle<Exception>()
            .WaitAndRetry(10, _ => TimeSpan.FromSeconds(3), (_, _) =>
                Log.Warning("Migration failed. Retrying."))
            .Execute(() => db.Database.Migrate());
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
