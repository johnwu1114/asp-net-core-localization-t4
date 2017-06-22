using NUnit.Framework;
using Resources;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace MyWebsite.Tests.T4
{
    [TestFixture]
    public class ResourcesGeneratorTests
    {
        [TestCase(new object[] { "en-gb", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "ZH-TW", "您好~ 這段文字來自 Text.zh-TW.resx" })]
        [TestCase(new object[] { "en-US", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "ZH-cn", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "Ja-Jp", "Hello" })]
        [TestCase(new object[] { "", "Hello~ This message from Text.en-GB.resx" })]
        public void GetResource(string culture, string expected)
        {
            // Arrange
            var text = new Text(culture);

            // Act
            var actual = text.Hello;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetResourceFiles_For_T4()
        {
            // Arrange
            var expected = "Hello~ This message from Text.en-GB.resx";
            var defaultCulture = "en-gb";
            var path = @"MyWebsite.Tests.T4\Resources\";
            var files = Directory.GetFiles(path, "*.resx");

            // Act
            var actual = files.GroupBy(file =>
                {
                    var fileName = Path.GetFileNameWithoutExtension(file).Split('.');
                    return fileName.First();
                }).ToDictionary(g => g.Key, g =>
                {
                    var defaultFile = g.Single(s => s.IndexOf(defaultCulture, StringComparison.CurrentCultureIgnoreCase) != -1);
                    var xdoc = XDocument.Load(defaultFile);
                    var dictionary = xdoc.Root.Elements("data").ToDictionary(e => e.Attribute("name").Value, e => e.Element("value").Value);
                    return dictionary;
                });

            // Assert
            Assert.AreEqual(expected, actual["Text"]["Hello"]);
        }
    }
}