using System.Collections.Generic;

namespace Misc.Tests
{
    public class TestConstants
    {
        #region Mqtt Topics

        public static IEnumerable<string> ValidMqttTopics
        {
            get
            {
                yield return "+";
                yield return "+/+";
                yield return "#";
                yield return "A";
                yield return "A/";
                yield return "A/+";
                yield return "A/#";
                yield return "A/B";
                yield return "A/B/C";
                yield return "A/+/C";
                yield return "A/+/C/#";
                yield return "A//+//C/#";
                yield return "/A/+//////C/#/";
                yield return "/A/$%^&/#/";
            }
        }

        public static IEnumerable<string> InvalidMqttTopics
        {
            get
            {
                yield return string.Empty;
                yield return "/";
                yield return "A/#/C";
                yield return "A/##/C";
                yield return "A/B/##";
                yield return "A/foo#";
                yield return "A/+/++/D";
                yield return "foo/bar+/D/";
                yield return "A/кир/+/D";
                yield return "A/한자/D";
            }
        }

        #endregion
    }
}
