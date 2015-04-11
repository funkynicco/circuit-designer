using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitDesign
{
    public static class Configuration
    {
        private const string RegistryKey = @"Software\nProg\CircuitDesign";

        #region Registry Functions
        private static Dictionary<string, object> _cache = new Dictionary<string, object>();

        private static int ReadInteger(string name, int defaultValue = 0)
        {
            object cachedValue;
            if (_cache.TryGetValue(name, out cachedValue) &&
                cachedValue is int)
                return (int)cachedValue;

            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryKey))
                {
                    var val = key.GetValue(name);
                    if (val != null &&
                        val is int)
                        return (int)val;
                }
            }
            catch { }

            return defaultValue;
        }

        private static void WriteInteger(string name, int value)
        {
            _cache[name] = value;
            using (var key = Registry.CurrentUser.CreateSubKey(RegistryKey))
                key.SetValue(name, value, RegistryValueKind.DWord);
        }

        private static string ReadString(string name, string defaultValue = null)
        {
            object cachedValue;
            if (_cache.TryGetValue(name, out cachedValue) &&
                cachedValue is string)
                return (string)cachedValue;

            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryKey))
                {
                    var val = key.GetValue(name);
                    if (val != null &&
                        val is string)
                        return (string)val;
                }
            }
            catch { }

            return defaultValue;
        }

        private static void WriteString(string name, string value)
        {
            _cache[name] = value;
            using (var key = Registry.CurrentUser.CreateSubKey(RegistryKey))
                key.SetValue(name, value, RegistryValueKind.String);
        }
        #endregion

        public static string ComponentsDirectory
        {
            get
            {
                return ReadString("ComponentsDirectory", "Components");
            }
            set
            {
                WriteString("ComponentsDirectory", value);
            }
        }
    }
}
