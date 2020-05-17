using HomeLibrary_Avalonia.Services;
using ReactiveUI;
using Splat;
using System;
using System.Configuration;
using System.IO;
using System.Reactive;

namespace HomeLibrary_Avalonia.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "/configuration";

        public IScreen HostScreen { get; }

        public SettingsViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            string dir;
            try
            {
                dir = ConfigurationManager.ConnectionStrings["PdfDir"].ConnectionString;
                dir = Path.GetFullPath(dir);
                SettingsService.UpdatePath(dir);
            }
            catch
            {
                SettingsService.UpdateKey("PdfDir", "Invalid path!");
            }
            ApplyChanges();

            ReadKey("CoreBaseUrl", "CoreUrl");
            ReadKey("CoreApiKey", "CoreApiKey");
            ReadKey("ElasticHost", "ElasticHost");
            ReadKey("ElasticPort", "ElasticPort");
            ReadKey("PdfDir", "Directory");

            SaveChanges = ReactiveCommand.Create(() =>
            {
                clicked++;
                try
                {
                    if (SettingsService.IsConnectionSectionAbsent)
                    {
                        SettingsService.AddConnectionSection();
                    }

                    SettingsService.UpdateKey("CoreBaseUrl", CoreUrl);
                    SettingsService.UpdateKey("CoreApiKey", CoreApiKey);
                    SettingsService.UpdateKey("ElasticHost", ElasticHost);
                    SettingsService.UpdateKey("ElasticPort", ElasticPort);
                    SettingsService.UpdatePath(Directory);

                    ApplyChanges();
                    ReadKey("PdfDir", "Directory");
                }
                catch (Exception ex)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter("log.txt", true))
                        {
                            sw.WriteLine($"{DateTime.Now}: {ex.Message}");
                        }
                    }
                    catch { }
                }

            });
        }

        private int clicked = 0;

        private string coreUrl;
        public string CoreUrl
        {
            get => coreUrl;
            set => this.RaiseAndSetIfChanged(ref coreUrl, value);
        }

        private string coreApiKey;
        public string CoreApiKey
        {
            get => coreApiKey;
            set => this.RaiseAndSetIfChanged(ref coreApiKey, value);
        }

        private string elasticHost;
        public string ElasticHost
        {
            get => elasticHost;
            set => this.RaiseAndSetIfChanged(ref elasticHost, value);
        }

        private string elasticPort;
        public string ElasticPort
        {
            get => elasticPort;
            set => this.RaiseAndSetIfChanged(ref elasticPort, value);
        }

        private string directory;
        public string Directory
        {
            get => directory;
            set => this.RaiseAndSetIfChanged(ref directory, value);
        }

        private string status = string.Empty;
        public string Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        private void ReadKey(string key, string propName)
        {
            string propValue;
            try
            {
                propValue = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch
            {
                propValue = $"Unable to read {key} from configuration file!";
            }
            GetType().GetProperty(propName).SetValue(this, propValue);
        }

        public ReactiveCommand<Unit, Unit> SaveChanges { get; }

        private void ApplyChanges()
        {
            try
            {
                SettingsService.ApplyChanges();
            }
            catch (Exception ex)
            {
                Status = $"Cannot apply changes: config file was changed from the outside.{Environment.NewLine}Please, restart the app!";
            }
        }
    }
}
