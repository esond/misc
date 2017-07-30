using System;

namespace Flamtap.Tests
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random)
        {
            double mantissa = random.NextDouble() * 2.0 - 1.0;
            double exponent = Math.Pow(2.0, random.Next(-126, 128));
            return (float) (mantissa * exponent);
        }

        public static uint NextUInt32(this Random random)
        {
            byte[] randomBytes = new byte[32];
            random.NextBytes(randomBytes);

            return BitConverter.ToUInt32(randomBytes, 0);
        }

        public static long NextLong(this Random random)
        {
            byte[] randomBytes = new byte[32];
            random.NextBytes(randomBytes);

            return BitConverter.ToUInt32(randomBytes, 0);
        }
    }
}
