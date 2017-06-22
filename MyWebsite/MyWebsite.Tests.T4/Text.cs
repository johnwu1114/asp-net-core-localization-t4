using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace MyWebsite.Tests.T4
{
    public static class Text
    {
        private const string _defaultCulture = "en-gb";
        private const string _resourceFolder = @"MyWebsite.Tests.T4\Resources\";

        private static Dictionary<string, Dictionary<string, string>> _resources;

        private static CultureInfo Culture
        {
            get
            {
                var culture = Thread.CurrentThread.CurrentCulture;
                if (culture == null)
                {
                    culture = new CultureInfo(_defaultCulture);
                }
                return culture;
            }
        }

        private static Dictionary<string, Dictionary<string, string>> Resources
        {
            get
            {
                if (_resources == null)
                {
                    _resources = new Dictionary<string, Dictionary<string, string>>(StringComparer.CurrentCultureIgnoreCase);

                    var files = Directory.GetFiles(_resourceFolder, $"{nameof(Text)}*.resx");
                    foreach (var file in files)
                    {
                        var xdoc = XDocument.Load(file);
                        var dictionary = xdoc.Root.Elements("data").ToDictionary(e => e.Attribute("name").Value, e => e.Element("value").Value);
                        _resources.Add(GetCulture(file), dictionary);
                    }
                }
                return _resources;
            }
        }

        public static string Hello { get { return GetString("Hello"); } }

        public static string GetString(string resourceKey)
        {
            return GetString(resourceKey, Culture.Name);
        }

        public static string GetString(string resourceKey, string culture)
        {
            var resource = Resources.ContainsKey(culture) ? Resources[culture] : Resources[_defaultCulture];
            return resource.ContainsKey(resourceKey) ? resource[resourceKey] : resourceKey;
        }

        private static string GetCulture(string path)
        {
            var culturePattern = "[A-Za-z]{2}-[A-Za-z]{2}.resx$";
            var match = Regex.Match(path, culturePattern);
            return match.Value.Replace(".resx", "");
        }
    }
}