using System;
using System.Configuration;
using System.IO;

namespace HomeLibrary_Avalonia.Services
{
    public static class SettingsService
    {
        /*static SettingsService()
        {
            UpdateServiceConfig();
        }*/
        /// <summary>
        /// Checks the validity of app configuration.
        /// </summary>
        /// <returns>
        /// First item - validity of Core settings.
        /// Second item - validity of Elastic settings.
        /// </returns>
        public static (bool, bool) CheckConfigPresence()
        {
            bool elasticPresence = true;
            try
            {
                string elasticHost = ConfigurationManager.ConnectionStrings["ElasticHost"].ConnectionString;
                string elasticPort = ConfigurationManager.ConnectionStrings["ElasticPort"].ConnectionString;
            }
            catch (ConfigurationErrorsException)
            {
                elasticPresence = false;
            }
            bool corePresence = true;
            try
            {
                string coreBaseUrl = ConfigurationManager.ConnectionStrings["CoreBaseUrl"].ConnectionString;
                string coreApiKey = ConfigurationManager.ConnectionStrings["CoreApiKey"].ConnectionString;
            }
            catch (ConfigurationErrorsException)
            {
                corePresence = false;
            }
            return (corePresence, elasticPresence);
        }

        private static Configuration config;

        public static bool UpdateServiceConfig()
        {
            try
            {
                config = ConfigurationManager
                    .OpenExeConfiguration(ConfigurationUserLevel.None);
                return true;
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter("log.txt", true))
                {
                    sw.WriteLine($"{DateTime.Now}: Config loading failure.txt - {ex.Message}.");
                }
                return false;
            }
        }

        public static bool IsConnectionSectionAbsent =>
            config.Sections["connectionSettings"] == null;

        public static void AddConnectionSection()
        {
            config.Sections.Add("connectionSettings", new ConnectionStringsSection());
        }

        public static string ReadKey(string key)
        {
            string propValue;
            try
            {
                propValue = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch
            {
                propValue = null;
            }
            return propValue;
        }

        public static void UpdateKey(string connStringName, string connString)
        {
            if (config.ConnectionStrings.ConnectionStrings[connStringName] == null)
            {
                config.ConnectionStrings.ConnectionStrings
                            .Add(CreateConnectionString(connStringName, connString));
            }
            else
            {
                config.ConnectionStrings.ConnectionStrings[connStringName]
                        .ConnectionString = connString;
            }
        }

        public static void UpdatePath(string value)
        {
            string path = Path.GetFullPath(value);
            if (!Directory.Exists(path))
            {
                path = "Directory doesn't exist!";
            }
            UpdateKey("PdfDir", path);
        }

        /// <summary>
        /// Returns connection information.
        /// </summary>
        /// <returns> First item - URL, second - API key. </returns>
        public static (string, string) GetCoreInfo()
        {
            string coreUrl = ReadKey("CoreBaseUrl");
            string coreApiKey = ReadKey("CoreApiKey");
            return (coreUrl, coreApiKey);
        }

        public static (string, string) GetElasticInfo()
        {
            string elasticHost = ReadKey("ElasticHost");
            string elasticPort = ReadKey("ElasticPort");
            return (elasticHost, elasticPort);
        }

        public static string GetDirectoryInfo()
        {
            return ReadKey("PdfDir");
        }

        public static void ApplyChanges()
        {
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        private static ConnectionStringSettings CreateConnectionString(string name, string value)
        {
            ConnectionStringSettings connString = new ConnectionStringSettings();
            connString.Name = name;
            connString.ConnectionString = value;
            return connString;
        }
    }
}
