using NUnit.Framework;
using Resources;
using System;

namespace MyWebsite.Tests
{
    [TestFixture]
    public class LocalizerTests
    {
        private ILocalizer _localizer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _localizer = new Localizer();
        }

        [TestCase(new object[] { "en-gb", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "ZH-TW", "您好~ 這段文字來自 Text.zh-TW.resx" })]
        [TestCase(new object[] { "en-US", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "ZH-cn", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { "Ja-Jp", "Hello" })]
        [TestCase(new object[] { "", "Hello~ This message from Text.en-GB.resx" })]
        public void Localizer_Text(string culture, string expected)
        {
            // Arrange
            _localizer.Culture = culture;

            // Act
            var actual = _localizer.Text.Hello;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new object[] { typeof(Text), "Hello", "en-gb", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { typeof(Text), "Hello", "ZH-TW", "您好~ 這段文字來自 Text.zh-TW.resx" })]
        [TestCase(new object[] { typeof(Text), "Hello", "en-US", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { typeof(Text), "Hello", "ZH-cn", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { typeof(Text), "Hello", "Ja-Jp", "Hello" })]
        [TestCase(new object[] { typeof(Text), "Hello", "", "Hello~ This message from Text.en-GB.resx" })]
        [TestCase(new object[] { typeof(Object), "Hello", "en-gb", "Hello" })]
        public void Localizer_GetString(Type category, string resourceKey, string culture, string expected)
        {
            // Arrange

            // Act
            var actual = _localizer.GetString(category, resourceKey, culture);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}