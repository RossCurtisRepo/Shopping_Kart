// See https://aka.ms/new-console-template for more information
using Datastore;
using Microsoft.Extensions.DependencyInjection;
using Shopping_Kart;
using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Configuration;

using Shopping_Kart.Services;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Microsoft.Extensions.Logging;

class Program
{

    static void Main()
    {
        var curDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        var logger = Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .WriteTo.File(curDir+"\\log.json")
        .CreateLogger();

        try
        {
            Console.WriteLine("Building Configuration");

            logger.Information("Building Configuration");
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var config = builder.Build();
            var connectionString = String.Format(config.GetConnectionString("ShoppingKartConnection"), curDir);
            Console.WriteLine("Configuring Services");
            Log.Logger.Information("Configuring Services");
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services
                    .AddLogging(builder => builder.AddFilter("Microsoft", LogLevel.None))
                .AddDbContext<DataStoreContext>(options =>
                options
                .UseSqlServer(connectionString)
                .LogTo(logger.Information, LogLevel.Information, null)
                )
                 .AddScoped<IDataStore, DataStore>()
                 .AddTransient<INavigationService, NavigationService>()
                 .AddTransient<IProductService, ProductService>()
                 .AddTransient<ITransactionService, TransactionService>();
                })
                .UseSerilog()
                .Build();

            Console.WriteLine("Configuring Data Repository");
            Log.Logger.Information("Configuring Data Repository");
            MigrateDB(host);

            Log.Logger.Information("Moving to main navigation menu");
            var app = ActivatorUtilities.CreateInstance<NavigationService>(host.Services);
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Startup terminated unexpectedly");
            return;
        }
        finally
        {
            Log.CloseAndFlush();
        }

        //todo - logging
        //todo  - refactoring
        //todo - unit tests
    }
    static void MigrateDB(IHost host)
    {

        DbContext _db = host.Services.GetRequiredService<DataStoreContext>();
        Log.Logger.Information("Ensuring database existence");
        _db.Database.EnsureCreated();
        Log.Logger.Information("Migrating");
        _db.Database.Migrate();
    }
}




