using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Flamtap.Extensions
{
    public static class EnumUtil
    {
        private static FieldInfo GetEnumField<TEnum>(TEnum enumValue)
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be a type of enum.");

            return typeof(TEnum).GetField(enumValue.ToString());
        }

        public static string GetDisplayName<TEnum>(TEnum enumValue)
            where TEnum : struct
        {
            FieldInfo fieldInfo = GetEnumField(enumValue);

            DescriptionAttribute descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();

            return descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString().ToDisplayText();
        }

        public static IEnumerable<KeyValuePair<TEnum, string>> GetValueDisplayNamePairs<TEnum>()
            where TEnum : struct
        {
            foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
            {
                FieldInfo fieldInfo = typeof(TEnum).GetField(enumValue.ToString());

                if (fieldInfo.GetCustomAttribute<BrowsableAttribute>()?.Browsable == false)
                    continue;

                yield return new KeyValuePair<TEnum, string>(enumValue, GetDisplayName(enumValue));
            }
        }

        /// <summary>
        ///     Parses a string value to it's equivalent enum value. Case insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns> The enum value with specified name.</returns>
        public static TEnum Parse<TEnum>(string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }
    }
}
