using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Flamtap.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Flamtap.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTester
    {
        #region AfterLast

        [Test]
        public void AfterLast_should_return_all_characters_after_specified_string()
        {
            "Homer Simpson".AfterLast(" ").ShouldBeEquivalentTo("Simpson");
            "George Louis Costanza".AfterLast("Louis").ShouldBeEquivalentTo(" Costanza");
            "mqtt/topic/example".AfterLast("/").ShouldBeEquivalentTo("example");
            "underscores_ and_ spaces_".AfterLast("_ ").ShouldBeEquivalentTo("spaces_");
            "an|empty|string|should|be|returned|".AfterLast("|").ShouldBeEquivalentTo(string.Empty);
        }

        #endregion

        #region Between

        [Test]
        public void between_should_get_substring_between_left_and_right_1()
        {
            "(I Can’t Get No) Satisfaction".Between("(", ")").ShouldBeEquivalentTo("I Can’t Get No");
        }

        [Test]
        public void between_should_get_substring_between_left_and_right_2()
        {
            "public void foo(){print 'baz';}".Between("{", "}").ShouldBeEquivalentTo("print 'baz';");
        }

        [Test]
        public void between_should_get_substring_between_left_and_right_3()
        {
            "A George divided against itself cannot stand!".Between("A George", "itself")
                .ShouldBeEquivalentTo(" divided against ");
        }

        [Test]
        public void between_should_return_empty_string_if_left_not_found()
        {
            "Gold, Jerry! Gold!".Between("Silver", "Gold").ShouldBeEquivalentTo(string.Empty);
        }

        [Test]
        public void between_should_return_empty_string_if_right_not_found()
        {
            "Gold, Jerry! Gold!".Between("Gold", "Silver").ShouldBeEquivalentTo(string.Empty);
        }

        #endregion

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
            var sb = new StringBuilder();

            for (var i = 0; i <= 127; i++)
                sb.Append((char) i);

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
            Action action = () => { ((string) null).IsAscii(); };
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

        #region IsBase64

        [Test]
        [Repeat(5)]
        public void IsBase64_should_return_true_for_valid_base64_strings()
        {
            var bytes = new byte[20];
            RandomNumberGenerator.Create().GetBytes(bytes);

            string base64 = Convert.ToBase64String(bytes);
            base64.IsBase64().Should().BeTrue();

            base64 = base64.Substring(0, base64.Length - 2);
            base64.IsBase64().Should().BeFalse();
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

        #region SplitAndKeep

        [Test]
        public void SplitAndKeep_joining_result_with_same_separator_should_be_equivalent_to_original_value()
        {
            var value = "George Louis Costanza";
            var result = value.SplitAndKeep(" ");
            value.ShouldBeEquivalentTo(string.Join("", result));
        }

        [Test]
        public void SplitAndKeep_should_return_items_with_separator()
        {
            "split|me|please".SplitAndKeep("|").ShouldBeEquivalentTo(new []{"split|", "me|", "please"});
            "trailing+separators+test+".SplitAndKeep("+").ShouldBeEquivalentTo(new []{"trailing+", "separators+", "test+", ""});
            "trailing+separators+test+".SplitAndKeep("+", StringSplitOptions.RemoveEmptyEntries)
                .ShouldBeEquivalentTo(new[] {"trailing+", "separators+", "test+"});
        }

        #endregion

        #region SplitUnixArgs

        [Test]
        public void SplitUnixArgs_handles_emtpty_strings()
        {
            var args = string.Empty.SplitUnixArgs();

            args.Length.ShouldBeEquivalentTo(0);
        }

        [Test]
        public void SplitUnixArgs_splits_args()
        {
            var args = "-u 123 -m message".SplitUnixArgs();

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
            var args = "commit -a".SplitUnixArgs();

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

        #region StripDiacratics

        [Test]
        public static void StripDiacratics_should_normalize_strings_with_diacratics()
        {
            "Éric Söndergard".StripDiacritics().ShouldBeEquivalentTo("Eric Sondergard");
            "Gêorgé Costanzà".StripDiacritics().ShouldBeEquivalentTo("George Costanza");
            "Hafthór Júlíus Björnsson".StripDiacritics().ShouldBeEquivalentTo("Hafthor Julius Bjornsson");
        }

        [Test]
        public static void StripDiacratics_should_not_modify_strings_with_no_diacratics()
        {
            "Eric Sondergard".StripDiacritics().ShouldBeEquivalentTo("Eric Sondergard");
            "George Costanza".StripDiacritics().ShouldBeEquivalentTo("George Costanza");
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

        #region ToValidFileName

        public static IEnumerable<string> InvalidFileNames
        {
            get
            {
                yield return "24/05/2017";
                yield return @"24\05\2017";
                yield return "24>05<2017:24|05?*2017";
                yield return "Fo" + '"' + "o";
            }
        }

        [Test]
        [TestCaseSource(nameof(InvalidFileNames))]
        public void ToValidFileName_should_convert_value_to_valid_filename(string value)
        {
            value.ToValidFileName().ToCharArray().Intersect(Path.GetInvalidFileNameChars()).Should().BeEmpty();
        }

        [Test]
        public void ToValidFileName_throws_ArgumentException_if_replacement_string_contains_invalid_filename_chars()
        {
            const string value = "17/05/2017";

            Action action = () => value.ToValidFileName("/");
            action.ShouldThrowExactly<ArgumentException>();

            action = () => value.ToValidFileName(":\\");
            action.ShouldThrowExactly<ArgumentException>();
        }

        #endregion
    }
}
