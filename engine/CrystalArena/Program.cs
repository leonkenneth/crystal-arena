using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
        /*BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);*/
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost
            .UseSentry(o =>
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
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
        var app = builder.Build();

        app.UseCors("AllowAll");
        app.MapGet("/games", () =>
        {
            return GameRepository.GameIds().Select(id => new { Id = id });
        });
        app.MapGet("/games/{id}", (int id) =>
        {
            var ui = GameRepository.ResolveUi(id);
            return ui.Shell.ToJson();
        });
        app.MapGet("/games/{id}/save", (int id) =>
        {
            var ui = GameRepository.ResolveUi(id);
            var savedGame = ui.Match.Game.Save();
            
        });
        app.MapPost("/games", () =>
        {
            var nextGameId = GameRepository.NextId();
            var ui = GameRepository.ResolveUi(nextGameId);
            var startScreenVM = ui.Dialogs.StartScreen.Create();
            Task.Run(() => startScreenVM.PlayRandom());
            return new { Id = nextGameId };
        });
        app.MapGet("/games/{gameId}/callback/{id}/{result}", (int gameId, string id, string result) =>
        {
            var ui = GameRepository.ResolveUi(gameId);
            ui.Shell.ProcessCallback(id, result);
            return "callbacked";
        });
        app.MapGet("/games/{gameId}/oidcallback/{oid}/{result}", (int gameId, string oid, string result) =>
        {
            var ui = GameRepository.ResolveUi(gameId);
            ui.Shell.ProcessOidCallback(oid, result);
            return "oid callbacked";
        });
        app.MapPost("/testsentry", () =>
        {
            SentrySdk.CaptureMessage("This is a test, clearly");
        });

        app.Run();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}