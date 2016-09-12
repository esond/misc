using Flamtap.Extensions;
using NUnit.Framework;

namespace Flamtap.UnitTests.Extensions
{
    [TestFixture]
    public class StringExtensionsTester
    {
        [Test]
        public void ToDisplayText_splits_strings_are_split_on_capital_letters()
        {
            Assert.AreEqual("Homer Simpson", "HomerSimpson".ToDisplayText());
            Assert.AreEqual("lower Case First", "lowerCaseFirst".ToDisplayText());
        }

        [Test]
        public void ToDisplayText_preserves_acronyms()
        {
            Assert.AreEqual("SQL Server", "SQLServer".ToDisplayText());
            Assert.AreEqual("Calgary Flames NHL", "CalgaryFlamesNHL".ToDisplayText());
        }

        [Test]
        public void ToDisplayText_splits_on_numbers()
        {
            Assert.AreEqual("The Year 2000", "TheYear2000".ToDisplayText());
            Assert.AreEqual("Nhl 2016", "Nhl2016".ToDisplayText());
            Assert.AreEqual("March 18th 1992", "March18th1992".ToDisplayText());
        }
    }
}
