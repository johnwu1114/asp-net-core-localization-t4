using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Resources;

namespace MyWebsite.Tests
{
    [TestFixture]
    public class T4ScriptsTests
    {
        [Test]
        [TestCase(new object[] { "Text", "Hello", "en-gb", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "Text", "Hello", "zh-tw", "您好~ 這段文字來自 Text.zh-TW.resx" })]
        public void T4_GetResourcesByCulture(string category, string resourceKey, string culture, string expected)
        {
            // Arrange
            var resourceFolder = "Resources";
            var files = Directory.GetFiles(resourceFolder, "*.resx");

            // Act
            var actual = GetResourcesByCulture(culture, resourceFolder)[category.ToString()][resourceKey];

            // Assert
            Assert.AreEqual(expected, actual);
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