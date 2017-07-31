using System;
using System.Collections.Generic;
using System.Linq;
using Flamtap.Extensions;

namespace Flamtap.Validation
{
    /// <summary>
    ///     Exposes methods for determining if a string is a valid MQTT topic as per the official
    ///     specification.
    /// <see cref="https://public.dhe.ibm.com/software/dw/webservices/ws-mqtt/mqtt-v3r1.html#appendix-a"/>
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

            if (!tokens.Any())
                return false;

            return tokens.All(IsValidMqttLevel);
        }

        private static bool IsValidMqttLevel(string level)
        {
            if ((level.Contains('#') || level.Contains('+')) && level.Length > 1)
                return false;

            if (!level.IsUtf8())
                return false;

            return true;
        }
    }
}
