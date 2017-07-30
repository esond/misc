using System;
using System.Collections.Generic;
using System.Linq;
using Flamtap.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace Flamtap.UnitTests.Collections
{
    [TestFixture]
    public class BinaryCollectionTester
    {
        [SetUp]
        public void SetUp()
        {
            _random = new Random();
        }

        private Random _random;

        [Test]
        public void add_should_add_new_item()
        {
            List<int> ints = new List<int>();

            for (int i = 0; i < 100; i++)
                ints.Add(_random.Next());

            BinaryCollection<int> sut = new BinaryCollection<int>(ints);

            int nextInt = _random.Next();
            ints.Add(nextInt);
            sut.Add(BitConverter.GetBytes(nextInt));

            sut.ToList().ShouldBeEquivalentTo(ints);
        }

        [Test]
        public void should_convert_collection_of_bool()
        {
            List<bool> toConvert = new List<bool>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add(Convert.ToBoolean(_random.Next(0, 2)));

            byte[] bytes = new BinaryCollection<bool>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(bool));

            BinaryCollection<bool> fromBytes = new BinaryCollection<bool>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_byte()
        {
            List<byte> toConvert = new List<byte>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add((byte) _random.Next(byte.MinValue, byte.MaxValue));

            byte[] bytes = new BinaryCollection<byte>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(byte));

            BinaryCollection<byte> fromBytes = new BinaryCollection<byte>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_char()
        {
            List<char> toConvert = new List<char>();

            for (int i = 0; i < 100; i++)
                toConvert.AddRange(Guid.NewGuid().ToString().ToCharArray());

            byte[] bytes = new BinaryCollection<char>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(char));

            BinaryCollection<char> fromBytes = new BinaryCollection<char>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_decimal()
        {
            List<decimal> toConvert = new List<decimal>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add(new decimal(_random.NextDouble()));

            byte[] bytes = new BinaryCollection<decimal>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(decimal));

            BinaryCollection<decimal> fromBytes = new BinaryCollection<decimal>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_double()
        {
            List<double> toConvert = new List<double>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add(_random.NextDouble());

            byte[] bytes = new BinaryCollection<double>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(double));

            BinaryCollection<double> fromBytes = new BinaryCollection<double>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_float()
        {
            List<float> toConvert = new List<float>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add(_random.NextFloat());

            byte[] bytes = new BinaryCollection<float>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(float));

            BinaryCollection<float> fromBytes = new BinaryCollection<float>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_int32()
        {
            List<int> toConvert = new List<int>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add(_random.Next());

            byte[] bytes = new BinaryCollection<int>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(int));

            BinaryCollection<int> fromBytes = new BinaryCollection<int>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_long()
        {
            List<long> toConvert = new List<long>();

            for (int i = 0; i < 1000; i++)
            {
                byte[] randomBytes = new byte[64];
                _random.NextBytes(randomBytes);
                toConvert.Add(BitConverter.ToInt64(randomBytes, 0));
            }

            byte[] bytes = new BinaryCollection<long>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(long));

            BinaryCollection<long> fromBytes = new BinaryCollection<long>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_sbyte()
        {
            List<sbyte> toConvert = new List<sbyte>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add((sbyte) _random.Next(sbyte.MinValue, sbyte.MaxValue));

            byte[] bytes = new BinaryCollection<sbyte>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(sbyte));

            BinaryCollection<sbyte> fromBytes = new BinaryCollection<sbyte>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_short()
        {
            List<short> toConvert = new List<short>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add((short) _random.Next(short.MinValue, short.MaxValue));

            byte[] bytes = new BinaryCollection<short>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(short));

            BinaryCollection<short> fromBytes = new BinaryCollection<short>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_uint()
        {
            List<uint> toConvert = new List<uint>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add(_random.NextUInt32());

            byte[] bytes = new BinaryCollection<uint>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(uint));

            BinaryCollection<uint> fromBytes = new BinaryCollection<uint>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_ushort()
        {
            List<ushort> toConvert = new List<ushort>();

            for (int i = 0; i < 1000; i++)
                toConvert.Add((ushort) _random.Next(ushort.MinValue, ushort.MaxValue));

            byte[] bytes = new BinaryCollection<ushort>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(ushort));

            BinaryCollection<ushort> fromBytes = new BinaryCollection<ushort>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_not_throw_exception_if_list_is_not_multiple_of_value_size()
        {
            byte[] bytes = new byte[4];
            _random.NextBytes(bytes);

            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => { new BinaryCollection<int>(bytes); };
            action.ShouldNotThrow<ArgumentException>();
        }

        [Test]
        public void should_throw_exception_if_list_is_not_multiple_of_value_size()
        {
            byte[] bytes = new byte[3];
            _random.NextBytes(bytes);

            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => { new BinaryCollection<int>(bytes); };
            action.ShouldThrow<ArgumentException>();
        }
    }
}
