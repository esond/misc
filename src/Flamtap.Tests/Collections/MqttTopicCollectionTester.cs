using System;
using System.Collections.Generic;
using System.Linq;
using Flamtap.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace Flamtap.Tests.Collections
{
    [TestFixture]
    public class MqttTopicCollectionTester
    {
        #region Test Data

        public static IEnumerable<string> ValidTopics => TestConstants.ValidMqttTopics;

        public static IEnumerable<string> InvalidTopics => TestConstants.InvalidMqttTopics;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _collection = new MqttTopicCollection<string>();
        }

        private MqttTopicCollection<string> _collection;

        [Test]
        [TestCaseSource(nameof(ValidTopics))]
        public void add_should_not_throw_exception_for_valid_topics(string validTopic)
        {
            Action add = () => _collection.Add(validTopic, "foo");
            add.ShouldNotThrow<Exception>();
        }

        //[Test]
        //[TestCaseSource(nameof(ValidTopics))]
        //public void add_by_index_should_not_throw_exception_for_valid_topics(string validTopic)
        //{
        //    Action add = () => _collection[validTopic] = "foo";
        //    add.ShouldNotThrow<Exception>();
        //}

        //[Test]
        //[TestCaseSource(nameof(ValidTopics))]
        //public void add_by_keyValuePair_should_not_throw_exception_for_valid_topics(string validTopic)
        //{
        //    Action add = () => _collection.Add(new KeyValuePair<string, string>(validTopic, "foo"));
        //    add.ShouldNotThrow<Exception>();
        //}

        [Test]
        [TestCaseSource(nameof(InvalidTopics))]
        public void add_should_throw_exception_for_invalid_topics(string invalidTopic)
        {
            Action add = () => _collection.Add(invalidTopic, "foo");
            add.ShouldThrow<ArgumentException>();
        }

        //[Test]
        //[TestCaseSource(nameof(InvalidTopics))]
        //public void add_by_index_should_throw_exception_for_invalid_topics(string invalidTopic)
        //{
        //    Action add = () => _collection[invalidTopic] = "foo";
        //    add.ShouldThrow<ArgumentException>();
        //}

        //[Test]
        //[TestCaseSource(nameof(InvalidTopics))]
        //public void add_by_keyValuePair_should_throw_exception_for_invalid_topics(string invalidTopic)
        //{
        //    Action add = () => _collection.Add(new KeyValuePair<string, string>(invalidTopic, "foo"));
        //    add.ShouldThrow<ArgumentException>();
        //}

        [Test]
        public void getMatches_should_match_multi_level_wildcards()
        {
            const string match = "match";
            const string noMatch = "nomatch";

            const string queryTopic = "foo/bar/#";

            _collection.Add("#", match);
            _collection.Add("foo/#", match);
            _collection.Add("foo/bar", match);
            _collection.Add("foo/bar/1/#", match);
            _collection.Add("foo/bar/1/2/3", match);
            _collection.Add("foo/+/", match);

            _collection.Add("sut/baz/1", noMatch);
            _collection.Add("foo", noMatch);

            IEnumerable<KeyValuePair<string, string>> matches = _collection.GetMatches(queryTopic).ToList();
            
            matches.Select(m => m.Value).ShouldAllBeEquivalentTo(match);
        }

        [Test]
        public void getMatches_should_match_single_level_wildcards()
        {
            const string match = "match";
            const string noMatch = "nomatch";

            const string queryTopic = "foo/bar/+";

            _collection.Add("#", match);
            _collection.Add("foo/#", match);
            _collection.Add("foo/bar/1", match);

            _collection.Add("foo/bar", noMatch);
            _collection.Add("foo/bar/1/#", noMatch);
            _collection.Add("foo/bar/1/2/3", noMatch);
            _collection.Add("foo/+/", noMatch);
            _collection.Add("sut/baz/1", noMatch);
            _collection.Add("foo", noMatch);

            IEnumerable<KeyValuePair<string, string>> matches = _collection.GetMatches(queryTopic).ToList();

            matches.Select(m => m.Value).ShouldAllBeEquivalentTo(match);
        }

        [Test]
        public void getMatches_should_match_single_and_multi_level_wildcards()
        {
            const string match = "match";
            const string noMatch = "nomatch";

            const string queryTopic = "foo/bar/+/#";

            _collection.Add("#", match);
            _collection.Add("foo/#", match);
            _collection.Add("foo/bar/1", match);
            _collection.Add("foo/bar/#", match);
            _collection.Add("foo/bar/1/#", match);
            _collection.Add("foo/bar/1/2/3", match);

            _collection.Add("foo/+/", noMatch);
            _collection.Add("foo", noMatch);
            _collection.Add("foo/bar", noMatch);
            _collection.Add("sut/baz/1", noMatch);

            IEnumerable<KeyValuePair<string, string>> matches = _collection.GetMatches(queryTopic).ToList();

            matches.Select(m => m.Value).ShouldAllBeEquivalentTo(match);
        }
    }
}

