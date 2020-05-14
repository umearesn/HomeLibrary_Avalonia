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

                    using (StreamWriter sw = new StreamWriter("entered.txt"))
                    {
                        sw.WriteLine($"entered {clicked}");
                    }

                    if (SettingsService.IsConnectionSectionAbsent)
                    {
                        SettingsService.AddConnectionSection();
                    }

                    SettingsService.UpdateKey("CoreBaseUrl", CoreUrl);
                    SettingsService.UpdateKey("CoreApiKey", CoreApiKey);
                    SettingsService.UpdateKey("ElasticHost", ElasticHost);
                    SettingsService.UpdateKey("ElasticPort", ElasticPort);
                    SettingsService.UpdateKey("PdfDir", Directory);

                    using (StreamWriter sw = new StreamWriter("ConfigDebug.txt", true))
                    {
                        sw.WriteLine(DateTime.Now);
                        sw.WriteLine(CoreUrl);
                        sw.WriteLine(CoreApiKey);
                        sw.WriteLine(ElasticHost);
                        sw.WriteLine(ElasticPort);
                        sw.WriteLine(Directory);
                    }

                    SettingsService.ApplyChanges();
                    //config.Save(ConfigurationSaveMode.Modified);
                    //ConfigurationManager.RefreshSection("connectionStrings");
                }
                catch(Exception ex)
                {
                    using (StreamWriter sw = new StreamWriter("Settings_catch.txt", true))
                    {
                        sw.WriteLine(DateTime.Now);
                        sw.WriteLine(ex.Message);
                    }
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

        private ConnectionStringSettings CreateConnectionString(string name, string value)
        {
            ConnectionStringSettings connString = new ConnectionStringSettings();
            connString.Name = name;
            connString.ConnectionString = value;
            return connString;
        }

    }
}
