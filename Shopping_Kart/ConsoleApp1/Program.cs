// See https://aka.ms/new-console-template for more information
using Datastore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Shopping_Kart.Services;

static class Program
{

    static void Main()
    {
        var curDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        var logger = Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.File(curDir + "\\log.json")
            .CreateLogger();

        try
        {
            Console.WriteLine("Building Configuration");
            logger.Information("Building Configuration");

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();

            var connectionString = string.Format(config.GetConnectionString("ShoppingKartConnection"), curDir);

            Console.WriteLine("Configuring Services");
            Log.Logger.Information("Configuring Services");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services
                    .AddLogging(builder => builder.AddFilter("Microsoft", LogLevel.None))
                    .AddDbContext<DataStoreContext>(options => options
                        .UseSqlServer(connectionString)
                        .LogTo(logger.Information, LogLevel.Information, null))
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

            //TODO - Need to consider alternative implementation
            Log.Logger.Information("Moving to main navigation menu");
            var app = ActivatorUtilities.CreateInstance<NavigationService>(host.Services);
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Startup terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        //todo - logging
        //todo - refactoring
        //todo - unit tests
        //todo - async data store
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




