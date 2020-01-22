using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TendersFromEis.BuilderApp
{
    public class Builder
    {
        [Required] public static string FileLog { get; set; }
        [Required] public static string EmailFrom { get; set; }
        [Required] public static string SmtpServer { get; set; }
        [Required] public static string SmtpPass { get; set; }
        [Required] public static string LogDir { get; set; }
        [Required] public static string EmailTo { get; set; }
        [Required] public static string SearchString { get; set; }
        protected internal static int SmtpPort;
        private static Builder _b;
        public static readonly string Path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName()
            .CodeBase.Substring(5));
        private Builder()
        {
            GetSettings();
            CreateDirs();
        }
        private static void GetSettings()
        {
            var nameFile = $"{Path}{System.IO.Path.DirectorySeparatorChar}settings.json";
            using (var reader = File.OpenText(nameFile))
            {
                var o = (JObject) JToken.ReadFrom(new JsonTextReader(reader));
                EmailFrom = (string) o["email_from"];
                SmtpServer = (string) o["smtp_server"];
                SmtpPass = (string) o["smtp_pass"];
                EmailTo = (string) o["email_to"];
                SearchString = (string) o["search_string"];
                SmtpPort = int.TryParse((string) o["smtp_port"], out SmtpPort) ? int.Parse((string) o["smtp_port"]) : throw new Exception("smtp port not found in config file");
                LogDir = $"{Path}{System.IO.Path.DirectorySeparatorChar}logging";
                
                FileLog = $"{LogDir}{System.IO.Path.DirectorySeparatorChar}eis_parser_{DateTime.Now:dd_MM_yyyy}.log";
               
            }
        }
        private static void CreateDirs()
        {
            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
        }
        public static Builder GetBuilder()
        {
            if (_b == null)
            {
                _b = new Builder();
                var results = new List<ValidationResult>();
                var context = new ValidationContext(_b);
                if (!Validator.TryValidateObject(_b, context, results, true))
                {
                    foreach (var error in results)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }

                    Environment.Exit(0);
                }
            }

            return _b;
        }

    }
}