using System;
using System.Collections.Generic;
using System.Linq;
using Flamtap.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace Flamtap.Tests.Collections
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
            var ints = new List<int>();

            for (var i = 0; i < 100; i++)
                ints.Add(_random.Next());

            var sut = new BinaryCollection<int>(ints);

            int nextInt = _random.Next();
            ints.Add(nextInt);
            sut.Add(BitConverter.GetBytes(nextInt));

            sut.ToList().ShouldBeEquivalentTo(ints);
        }

        [Test]
        public void should_convert_collection_of_bool()
        {
            var toConvert = new List<bool>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add(Convert.ToBoolean(_random.Next(0, 2)));

            var bytes = new BinaryCollection<bool>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(bool));

            var fromBytes = new BinaryCollection<bool>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_byte()
        {
            var toConvert = new List<byte>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add((byte) _random.Next(byte.MinValue, byte.MaxValue));

            var bytes = new BinaryCollection<byte>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(byte));

            var fromBytes = new BinaryCollection<byte>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_char()
        {
            var toConvert = new List<char>();

            for (var i = 0; i < 100; i++)
                toConvert.AddRange(Guid.NewGuid().ToString().ToCharArray());

            var bytes = new BinaryCollection<char>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(char));

            var fromBytes = new BinaryCollection<char>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_decimal()
        {
            var toConvert = new List<decimal>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add(new decimal(_random.NextDouble()));

            var bytes = new BinaryCollection<decimal>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(decimal));

            var fromBytes = new BinaryCollection<decimal>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_double()
        {
            var toConvert = new List<double>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add(_random.NextDouble());

            var bytes = new BinaryCollection<double>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(double));

            var fromBytes = new BinaryCollection<double>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_float()
        {
            var toConvert = new List<float>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add(_random.NextFloat());

            var bytes = new BinaryCollection<float>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(float));

            var fromBytes = new BinaryCollection<float>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_int32()
        {
            var toConvert = new List<int>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add(_random.Next());

            var bytes = new BinaryCollection<int>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(int));

            var fromBytes = new BinaryCollection<int>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_long()
        {
            var toConvert = new List<long>();

            for (var i = 0; i < 1000; i++)
            {
                var randomBytes = new byte[64];
                _random.NextBytes(randomBytes);
                toConvert.Add(BitConverter.ToInt64(randomBytes, 0));
            }

            var bytes = new BinaryCollection<long>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(long));

            var fromBytes = new BinaryCollection<long>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_sbyte()
        {
            var toConvert = new List<sbyte>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add((sbyte) _random.Next(sbyte.MinValue, sbyte.MaxValue));

            var bytes = new BinaryCollection<sbyte>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(sbyte));

            var fromBytes = new BinaryCollection<sbyte>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_short()
        {
            var toConvert = new List<short>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add((short) _random.Next(short.MinValue, short.MaxValue));

            var bytes = new BinaryCollection<short>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(short));

            var fromBytes = new BinaryCollection<short>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_uint()
        {
            var toConvert = new List<uint>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add(_random.NextUInt32());

            var bytes = new BinaryCollection<uint>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(uint));

            var fromBytes = new BinaryCollection<uint>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_convert_collection_of_ushort()
        {
            var toConvert = new List<ushort>();

            for (var i = 0; i < 1000; i++)
                toConvert.Add((ushort) _random.Next(ushort.MinValue, ushort.MaxValue));

            var bytes = new BinaryCollection<ushort>(toConvert).ToBytes();
            bytes.Length.ShouldBeEquivalentTo(toConvert.Count * sizeof(ushort));

            var fromBytes = new BinaryCollection<ushort>(bytes);
            fromBytes.ToList().ShouldBeEquivalentTo(toConvert);
        }

        [Test]
        public void should_not_throw_exception_if_list_is_not_multiple_of_value_size()
        {
            var bytes = new byte[4];
            _random.NextBytes(bytes);

            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => { new BinaryCollection<int>(bytes); };
            action.ShouldNotThrow<ArgumentException>();
        }

        [Test]
        public void should_throw_exception_if_list_is_not_multiple_of_value_size()
        {
            var bytes = new byte[3];
            _random.NextBytes(bytes);

            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => { new BinaryCollection<int>(bytes); };
            action.ShouldThrow<ArgumentException>();
        }
    }
}
