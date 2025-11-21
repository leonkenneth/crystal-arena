using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;
using CrystalArena.UserInterface;
using CrystalArena.UserInterface.Shell;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sentry;

namespace CrystalArena;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
        {
            builder.WebHost.UseUrls("http://localhost:5001");
        }

        builder.WebHost.UseSentry(o =>
        {
            var dsnEnvVar = Environment.GetEnvironmentVariable("SENTRY_DSN");
            var sampleRateEnvVar = Environment.GetEnvironmentVariable("SENTRY_TRACE_SAMPLE_RATE");

            o.Dsn = dsnEnvVar ?? "";
            if (sampleRateEnvVar != null)
            {
                o.TracesSampleRate = float.Parse(sampleRateEnvVar);
            }

            o.Environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            o.Debug = false;
        });
        builder
            .Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;
            });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
        });
        var app = builder.Build();

        app.UseCors("AllowAll");

        // Add exception handling middleware
        app.Use(
            async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (GameRepository.GameNotFoundException ex)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message });
                }
            }
        );

        // Public API
        app.MapGet(
            "/games/{id}",
            (string id) =>
            {
                var ui = GameRepository.ResolveUi(id);
                return ui.Shell.ToJson();
            }
        );
        app.MapGet(
            "/games/{gameId}/callback/{id}/{result}",
            (string gameId, string id, string result) =>
            {
                var ui = GameRepository.ResolveUi(gameId);
                ui.Shell.ProcessCallback(id, result);
                return "callbacked";
            }
        );
        app.MapGet(
            "/games/{gameId}/oidcallback/{oid}/{result}",
            (string gameId, string oid, string result) =>
            {
                var ui = GameRepository.ResolveUi(gameId);
                ui.Shell.ProcessOidCallback(oid, result);
                return "oid callbacked";
            }
        );

        app.MapGet(
            "/internal/region",
            () => new { Region = Environment.GetEnvironmentVariable("FLY_REGION") }
        );
        app.MapPost(
            "/internal/games",
            () =>
            {
                var nextGameId = GameRepository.NextId();
                var ui = GameRepository.ResolveUi(nextGameId, createIfMissing: true);
                var startScreenVM = ui.Dialogs.StartScreen.Create();
                Task.Run(() => startScreenVM.PlayRandom());
                return new { Uuid = nextGameId, ui.PlayerToken };
            }
        );
        app.MapPost(
            "/internal/testsentry",
            () =>
            {
                SentrySdk.CaptureMessage("This is a test, clearly");
            }
        );

        app.Run();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
