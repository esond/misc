using System;
using System.Collections.Generic;
using System.Linq;
using Misc.Extensions;

namespace Misc.Validation
{
    /// <summary>
    ///     Exposes methods for determining if a string is a valid MQTT topic as per the official
    ///     specification.
    /// <see cref="http://public.dhe.ibm.com/software/dw/webservices/ws-mqtt/mqtt-v3r1.html#appendix-a"/>
    /// </summary>
    public static class MqttTopicValidator
    {
        public static bool IsValid(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                return false;

            if (topic.Contains('#') && !topic.EndsWith("#") && !topic.EndsWith("#/"))
                return false;

            IEnumerable<string> tokens = topic.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return tokens.Any() && tokens.All(IsValidMqttLevel);
        }

        private static bool IsValidMqttLevel(string level)
        {
            if ((level.Contains('#') || level.Contains('+')) && level.Length > 1)
                return false;

            return level.IsUtf8();
        }
    }
}
