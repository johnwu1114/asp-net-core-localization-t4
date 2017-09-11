﻿namespace Resources
{
	using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.Loader;
    using System.Text.RegularExpressions;

    public interface ILocalizer
    {
		string Culture { get; set; }

		Message Message { get; }

		Text Text { get; }

        string GetString(Type category, string resourceKey);

        string GetString(string category, string resourceKey);

        string GetString(Type category, string resourceKey, string culture);

        string GetString(string category, string resourceKey, string culture);
    }

    public class Localizer : ILocalizer
    {
        private const string DefaultCulture = "en-gb";
        private const string _resourceFolder = "Resources";
        private static readonly Lazy<Dictionary<string, ResourceManager>> _resources = new Lazy<Dictionary<string, ResourceManager>>(LoadResourceManager);
        private string _culture;
        private Message _Message;
        private Text _Text;

        #region ILocalizer

		public string Culture
        {
            get
            {
                if (string.IsNullOrEmpty(_culture))
                {
                    _culture = DefaultCulture;
                }
                return _culture;
            }
            set
            {
                var culture = value;
                if (Regex.IsMatch(culture, @"^[A-Za-z]{2}-[A-Za-z]{2}$"))
                {
                    _culture = culture;
                }
                else
                {
                    _culture = DefaultCulture;
                }
            }
        }

		public Message Message { get { if (_Message == null) { _Message = new Message(this); } return _Message; } }

		public Text Text { get { if (_Text == null) { _Text = new Text(this); } return _Text; } }

        public string GetString(Type category, string resourceKey)
        {
            return GetString(category.Name.ToString(), resourceKey);
        }

        public string GetString(string category, string resourceKey)
        {
            return GetString(category, resourceKey, _culture);
        }

        public string GetString(Type category, string resourceKey, string culture)
        {
            return GetString(category.Name.ToString(), resourceKey, culture);
        }

        public string GetString(string category, string resourceKey, string culture)
        {
            var resource = GetResource($"{category}.{culture}") ?? GetResource($"{category}.{DefaultCulture}");
            if (resource == null)
            {
                return resourceKey;
            }
            else
            {
                return resource.GetString(resourceKey);
            }
        }

        #endregion ILocalizer

        #region Private Methods

        private static Dictionary<string, ResourceManager> LoadResourceManager()
        {
            var location = Assembly.GetEntryAssembly().Location;
            var directory = Path.GetDirectoryName(location);
            var files = Directory.GetFiles(directory, "*.resources.dll", SearchOption.AllDirectories);
            
            var resources = new Dictionary<string, ResourceManager>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var file in files)
            {
                var culture = Path.GetFileName(Path.GetDirectoryName(file));
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                foreach(var resourceName in assembly.GetManifestResourceNames().Select(s=> Regex.Replace(s, ".resources$", "")))
                {
                    var category = Regex.Match(resourceName, $".*Resources\\.(.*)\\.{culture}").Groups[1].Value;
                    var resourceManager = new ResourceManager(resourceName, assembly);
                    resources.Add($"{category}.{culture}", resourceManager);
                }
            }

            return resources;
        }

        private ResourceManager GetResource(string key)
        {
            return _resources.Value[key];
        }

        #endregion
    }

    public abstract class ResourceBase
    {
        protected ResourceBase(ILocalizer localizer)
        {
            Localizer = localizer;
        }

        protected ILocalizer Localizer { get; private set; }

        protected string GetString(string resourceKey)
        {
            return Localizer.GetString(GetType(), resourceKey);
        }
    }

    public class Message : ResourceBase
    {
        public Message(ILocalizer localizer) : base(localizer)
        {
        }

		public string Hello { get { return GetString("Hello"); } }
    }

    public class Text : ResourceBase
    {
        public Text(ILocalizer localizer) : base(localizer)
        {
        }

		public string Hello { get { return GetString("Hello"); } }
    }
}
