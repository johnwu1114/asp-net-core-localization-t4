using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MyWebsite.Tests
{
    [TestFixture]
    public class T4ScriptsTests
    {
        [Test]
        public void T4_GetResourcesByCulture()
        {
            // Arrange
            var expected = "Hello~ This message from Text.en-GB.resx";
            var culture = "en-gb";
            var resourceFolder = "Resources";
            var files = Directory.GetFiles(resourceFolder, "*.resx");

            // Act
            var actual = GetResourcesByCulture(culture, resourceFolder);

            // Assert
            Assert.AreEqual(expected, actual["Text"]["Hello"]);
        }

        private Dictionary<string, Dictionary<string, string>> GetResourcesByCulture(string culture, string resourceFolder)
        {
            var files = Directory.GetFiles(resourceFolder, "*.resx");
            var resources = files.GroupBy(file =>
            {
                var fileName = Path.GetFileNameWithoutExtension(file).Split('.');
                return fileName.First();
            }).ToDictionary(g => g.Key, g =>
            {
                var defaultFile = g.Single(s => s.IndexOf(culture, StringComparison.CurrentCultureIgnoreCase) != -1);
                var xdoc = XDocument.Load(defaultFile);
                var dictionary = xdoc.Root.Elements("data").ToDictionary(e => e.Attribute("name").Value, e => e.Element("value").Value);
                return dictionary;
            });
            return resources;
        }
    }
}