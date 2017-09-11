using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace MyWebsite.Tests
{
    [TestFixture]
    public class T4ScriptsTests
    {
        private static string _assemblyPath = Directory.GetCurrentDirectory();

        [Test]
        public void T4_LoadResourceManager()
        {
            // Arrange
            var expected = "Hello~ This message from Text.en-GB.resx";
            var culture = "en-gb";
            var category = "Text";
            var key = "Hello";

            // Act
            var actual = LoadResourceManager()[$"{category}.{culture}"].GetString(key);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private Dictionary<string, ResourceManager> LoadResourceManager()
        {
            var directory = Path.GetDirectoryName(_assemblyPath);
            var files = Directory.GetFiles(directory, "*.resources.dll", SearchOption.AllDirectories);

            var resources = new Dictionary<string, ResourceManager>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var file in files)
            {
                var culture = Path.GetFileName(Path.GetDirectoryName(file));
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                foreach (var resourceName in assembly.GetManifestResourceNames().Select(s => Regex.Replace(s, ".resources$", "")))
                {
                    var category = Regex.Match(resourceName, $".*Resources\\.(.*)\\.{culture}").Groups[1].Value;
                    var resourceManager = new ResourceManager(resourceName, assembly);
                    resources.Add($"{category}.{culture}", resourceManager);
                }
            }

            return resources;
        }
    }
}