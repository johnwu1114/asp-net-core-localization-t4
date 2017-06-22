namespace Resources
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

	public class Text {  
		private const string _defaultCulture = "en-gb";
        private const string _resourceFolder = @"Resources";

        private readonly static Dictionary<string, Dictionary<string, string>> _resources;
        private CultureInfo _culture;

        static Text()
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
        }

        public Text(string culture = _defaultCulture)
        {
            _culture = new CultureInfo(culture);
        }

		public string Hello { get { return GetString("Hello"); } }

        public string GetString(string resourceKey)
        {
            return GetString(resourceKey, _culture.Name);
        }

        public string GetString(string resourceKey, string culture)
        {
            var resource = _resources.ContainsKey(culture) ? _resources[culture] : _resources[_defaultCulture];
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
