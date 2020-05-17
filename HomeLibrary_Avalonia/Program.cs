using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.Services;
using System;
using System.IO;

namespace HomeLibrary_Avalonia
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            if (!SettingsService.UpdateServiceConfig())
            {
                try
                {
                    using (var sw = new StreamWriter("../StartupFailure.txt", true))
                    {
                        sw.WriteLine($"{DateTime.Now}: Invalid configuration!");
                    }
                }
                catch { }
            }
            else
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();
    }
}