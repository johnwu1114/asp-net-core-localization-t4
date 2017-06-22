using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MyWebsite.Tests.T4
{
    [TestFixture]
    public class ResourcesGeneratorTests
    {
        [Test]
        public void GetCulture()
        {
            // Arrange
            var expected = "en-GB";
            var fileName = new FileInfo(@"Resources\Text.en-GB.resx");
            var culturePattern = ".[A-Za-z]{2}-[A-Za-z]{2}.resx$";

            // Act
            var match = Regex.Match(fileName.FullName, culturePattern);
            var actual = match.Value.Substring(1, 5);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoadResourceFile()
        {
            // Arrange
            var expected = "Hello~ This message from Text.en-GB.resx";
            var fileName = new FileInfo(@"Resources\Text.en-GB.resx");

            // Act
            var xdoc = XDocument.Load(fileName.FullName);
            var root = xdoc.Root;
            Dictionary<string, string> dictionary = root.Elements("data").ToDictionary(e => e.Attribute("name").Value, e => e.Element("value").Value);

            var actual = dictionary["Hello"];

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}