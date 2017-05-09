﻿using System;

namespace mcThings.Extensions
{
    public static class ByteExtensions
    {
        public static byte ReadFirstNBits(this byte value, byte n)
        {
            if (n > 8)
                throw new ArgumentException($"{nameof(n)} cannot be greater than 8.", nameof(n));

            return (byte) (value >> (8 - n));
        }

        public static byte ReadLastNBits(this byte value, byte n)
        {
            if (n > 8)
                throw new ArgumentException($"{nameof(n)} cannot be greater than 8.", nameof(n));

            byte mask = (byte) ((1 << n) - 1);
            return (byte) (value & mask);
        }
    }
}