using NUnit.Framework;
using WinBlocks.Game;

namespace WinBlocks.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase("B", "BBCDEFG")]
        [TestCase("BD", "BDCDEFG")]
        public void Replace_CreatesNewStringWithValuesReplaced(string replace, string expected)
        {
            var replaced = "ABCDEFG".Replace(0, replace);

            Assert.That(replaced, Is.EqualTo(expected));
        }
    }
}