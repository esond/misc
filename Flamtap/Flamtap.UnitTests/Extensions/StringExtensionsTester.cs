using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Test]
        public void SplitAndKeep_keeps_delimiters()
        {
            IEnumerable<string> substrings = "-george-constanza".SplitAndKeep('-');

            foreach (string substring in substrings)
                Assert.True(substring.StartsWith("-"));
        }

        [Test]
        public void SplitUnixArgs_handles_emtpty_strings()
        {
            IEnumerable<string> args = string.Empty.SplitUnixArgs();

            Assert.True(!args.Any());
        }

        [Test]
        public void SplitUnixArgs_splits_args()
        {
            IEnumerable<string> args = "-u 123 -m message".SplitUnixArgs();

            Assert.True(args.First() == "-u 123");
            Assert.True(args.Last() == "-m message");
        }

        [Test]
        public void SplitUnixArgs_handles_verbs()
        {
            IEnumerable<string> args = "commit -a".SplitUnixArgs();

            Assert.True(args.First() == "commit");
            Assert.True(args.Last() == "-a");

            args = "push --set-upstream-to".SplitUnixArgs().ToList();

            Assert.True(args.First() == "push");
            Assert.True(args.Last() == "--set-upstream-to");
        }
    }
}
