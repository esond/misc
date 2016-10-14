using System;

namespace Flamtap.Extensions
{
    public static class EnumUtil
    {
        /// <summary>
        /// Parses a string value to it's equivalent enum value. Case insensitive.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns> The enum value with specified name.</returns>
    	  public static T Parse<T>(string value)
    	  {
    		    return (T) Enum.Parse(typeof (T), value, true);
    	  }
    }
}
