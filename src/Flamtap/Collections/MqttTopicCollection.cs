using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Flamtap.Extensions;

namespace Flamtap.Collections
{
    public class MqttTopicDictionary<TValue> : IDictionary<string, TValue>
    {
        private const string Separator = "/";
        private const string MultiLevel = "#";
        private const string SingleLevel = "+";

        private readonly IDictionary<string, TValue> _dict;

        public MqttTopicDictionary() => _dict = new Dictionary<string, TValue>();

        public MqttTopicDictionary(IDictionary<string, TValue> dictionary) => _dict =
            new Dictionary<string, TValue>(dictionary);

        public MqttTopicDictionary(int capacity) => _dict = new Dictionary<string, TValue>(capacity);

        public IEnumerable<KeyValuePair<string, TValue>> GetMatches(string topic)
        {
            Regex regex = MakeRegex(topic);

            IEnumerable<KeyValuePair<string, TValue>> keyValuePairs = _dict.Where(d => regex.IsMatch(d.Key));

            return _dict.Where(d => regex.IsMatch(d.Key));
        }

        private static Regex MakeRegex(string topic)
        {
            IEnumerable<string> tokens = topic.Split(Separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<string> parts = new List<string>();

            foreach (string token in tokens)
            {
                bool isLast = token == tokens.Last();

                if (token == SingleLevel)
                {
                    parts.Add(isLast ? @"([^/#+]+/)" : @".*/");

                    continue;
                }

                if (token == MultiLevel)
                {
                    parts.Add(@"((?:[^/]/?)*)");
                    break;
                }

                if (isLast)
                    parts.Add(token);
                else
                    parts.Add($"{token}/");
            }

            string pattern = $"^{string.Join(string.Empty, parts)}$";

            return new Regex(pattern);
        }

        #region Implementation of IDictionary<string,TValue>

        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        public void Add(string key, TValue value)
        {
            if (!key.IsValidMqttTopic())
                throw new ArgumentException("Invalid MQTT topic.", nameof(key));

            _dict.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _dict.Remove(key);
        }

        public bool TryGetValue(string key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public TValue this[string key]
        {
            get => _dict[key];
            set
            {
                if (!key.IsValidMqttTopic())
                    throw new ArgumentException("Invalid MQTT topic.", nameof(key));

                _dict[key] = value;
            }
        }

        public ICollection<string> Keys => _dict.Keys;

        public ICollection<TValue> Values => _dict.Values;

        #endregion

        #region Implementation of ICollection<KeyValuePair<string,TValue>>

        public void Add(KeyValuePair<string, TValue> item)
        {
            if (!item.Key.IsValidMqttTopic())
                throw new ArgumentException("Invalid MQTT topic.", nameof(item));

            _dict.Add(item);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return _dict.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            _dict.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return _dict.Remove(item);
        }

        public int Count => _dict.Count;

        public bool IsReadOnly => _dict.IsReadOnly;

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dict).GetEnumerator();
        }

        #endregion
    }
}

