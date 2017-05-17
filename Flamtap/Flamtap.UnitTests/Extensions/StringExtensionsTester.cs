using System;
using System.Collections.Generic;
using System.Text;
using Flamtap.Extensions;
using NUnit.Framework;
using FluentAssertions;

namespace Flamtap.UnitTests.Extensions
{
    [TestFixture]
    public class StringExtensionsTester
    {
        #region IsAscii

        #region IsAscii Test Values

        public static IEnumerable<string> AsciiStrings
        {
            get
            {
                yield return "Foo";
                yield return " ";
                yield return "\r\n\t";
                yield return "!@#$$%^&*()_+=-~`[]{}|\\/?;\"<>,.";
                yield return GetBigAsciiString();
            }
        }

        private static string GetBigAsciiString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <= 127; i++)
                sb.Append((char)i);

            return sb.ToString();
        }

        public static IEnumerable<string> NonAsciiStrings
        {
            get
            {
                yield return "ƒoo";
                yield return "Hafþór Júlíus Björnsson";
                yield return "議㣻눬";
            }
        }

        #endregion

        [Test]
        public void IsAscii_should_throw_ArgumentNullException_when_given_null_or_empty_value()
        {
            Action action = () =>
            {
                ((string)null).IsAscii();
            };
            action.ShouldThrowExactly<ArgumentNullException>();

            action = () => { string.Empty.IsAscii(); };
            action.ShouldThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCaseSource(nameof(AsciiStrings))]
        public void IsAscii_should_return_true_for_ASCII_only_strings(string value)
        {
            value.IsAscii().ShouldBeEquivalentTo(true);
        }

        [Test]
        [TestCaseSource(nameof(NonAsciiStrings))]
        public void IsAscii_should_return_false_for_non_ascii_strings(string value)
        {
            value.IsAscii().ShouldBeEquivalentTo(false);
        }

        #endregion

        #region RemoveNonAlphanumeric

        [Test]
        public void RemoveNonAlphanumeric_should_remove_symbols()
        {
            "1234!".RemoveNonAlphanumeric().ShouldBeEquivalentTo("1234");
            "$%#!%!*()&^(".RemoveNonAlphanumeric().ShouldBeEquivalentTo(string.Empty);
            "-foo_bar_".RemoveNonAlphanumeric().ShouldBeEquivalentTo("foobar");

            "abc123".RemoveNonAlphanumeric().ShouldBeEquivalentTo("abc123");
        }

        #endregion

        #region SplitUnixArgs

        [Test]
        public void SplitUnixArgs_handles_emtpty_strings()
        {
            string[] args = string.Empty.SplitUnixArgs();

            args.Length.ShouldBeEquivalentTo(0);
        }

        [Test]
        public void SplitUnixArgs_splits_args()
        {
            string[] args = "-u 123 -m message".SplitUnixArgs();

            args.Length.ShouldBeEquivalentTo(2);
            args[0].ShouldBeEquivalentTo("-u 123");
            args[1].ShouldBeEquivalentTo("-m message");

            args = "-am somevalue --id 123 -m \"message\"".SplitUnixArgs();

            args.Length.ShouldBeEquivalentTo(3);
            args[0].ShouldBeEquivalentTo("-am somevalue");
            args[1].ShouldBeEquivalentTo("--id 123");
            args[2].ShouldBeEquivalentTo("-m \"message\"");
        }

        [Test]
        public void SplitUnixArgs_handles_verbs()
        {
            string[] args = "commit -a".SplitUnixArgs();

            args.Length.ShouldBeEquivalentTo(2);
            args[0].ShouldBeEquivalentTo("commit");
            args[1].ShouldBeEquivalentTo("-a");

            args = "push --set-upstream-to".SplitUnixArgs();

            args.Length.ShouldBeEquivalentTo(2);
            args[0].ShouldBeEquivalentTo("push");
            args[1].ShouldBeEquivalentTo("--set-upstream-to");

            args = "tag --version 12 -arg=asdfasdf -m \"super cool\"".SplitUnixArgs();

            args.Length.ShouldBeEquivalentTo(4);
            args[0].ShouldBeEquivalentTo("tag");
            args[1].ShouldBeEquivalentTo("--version 12");
            args[2].ShouldBeEquivalentTo("-arg=asdfasdf");
            args[3].ShouldBeEquivalentTo("-m \"super cool\"");
        }

        #endregion

        #region ToDisplayText

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

        #endregion
    }
}
