using System.Collections.Generic;
using FluentAssertions;
using Misc.Validation;
using NUnit.Framework;

namespace Misc.Tests.Validation
{
    [TestFixture]
    public class MqttTopicValidatorTester
    {
        #region Test Data

        public static IEnumerable<string> ValidTopics => TestConstants.ValidMqttTopics;

        public static IEnumerable<string> InvalidTopics => TestConstants.InvalidMqttTopics;

        #endregion

        [Test]
        [TestCaseSource(nameof(ValidTopics))]
        public void should_return_true_for_valid_topics(string validTopic)
        {
            MqttTopicValidator.IsValid(validTopic).Should().BeTrue();
        }
        
        [Test]
        [TestCaseSource(nameof(InvalidTopics))]
        public void should_return_false_for_invalid_topics(string invalidTopic)
        {
            MqttTopicValidator.IsValid(invalidTopic).Should().BeFalse();
        }
    }
}
