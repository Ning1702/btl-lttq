using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace btl_lttq.FacebookLite
{
    public class AppSettings
    {
        public string Theme { get; set; } = "Light";
        public bool NotificationSound { get; set; } = false;
        public string ServerMode { get; set; } = "Active";
        public string Language { get; set; } = "vi-VN";
        public int FontSize { get; set; } = 10;
        public string FontFamily { get; set; } = "Segoe UI";
    }

    public static class AppSettingsStorage
    {
        private static string PathFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(PathFile))
                {
                    string json = File.ReadAllText(PathFile);
                    var s = JsonSerializer.Deserialize<AppSettings>(json);
                    if (s != null) return s;
                }
            }
            catch {}
            return new AppSettings();
        }

        public static void Save(AppSettings s)
        {
            try
            {
                string json = JsonSerializer.Serialize(s, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(PathFile, json);
            }
            catch {}
        }
    }

    public static class UIFontApplier
    {
        public static void ApplyFont(Control root, string family, int size)
        {
            try
            {
                var f = new Font(family, size, FontStyle.Regular);
                Apply(root, f);
            }
            catch {}
        }

        private static void Apply(Control c, Font f)
        {
            c.Font = f;
            foreach (Control child in c.Controls)
                Apply(child, f);
        }
    }
}
