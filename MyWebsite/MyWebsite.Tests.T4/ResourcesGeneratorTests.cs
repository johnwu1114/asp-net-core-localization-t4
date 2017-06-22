using NUnit.Framework;
using System.Globalization;
using System.Threading;

namespace MyWebsite.Tests.T4
{
    [TestFixture]
    public class ResourcesGeneratorTests
    {
        private const string _resourceFolder = @"MyWebsite.Tests.T4\Resources\";
        private const string _resourceFile = @"Text.en-GB.resx";

        [TestCase(new object[] { "en-gb", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "ZH-TW", "您好~ 這段文字來自 Text.zh-TW.resx" })]
        [TestCase(new object[] { "en-US", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "ZH-cn", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "Ja-Jp", "Hello" })]
        [TestCase(new object[] { "", "Hello~ This message from Text.en-GB.resx" })]
        public void GetResource(string culture, string expected)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

            // Act
            var actual = Text.Hello;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}