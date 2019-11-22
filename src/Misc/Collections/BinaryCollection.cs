using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Misc.Collections
{
    public class BinaryCollection<T> : ICollection<T> where T : struct
    {
        private readonly List<T> _items;
        private readonly TypeCode _typeCode;

        public BinaryCollection()
        {
            _items = new List<T>();
            _typeCode = Type.GetTypeCode(typeof(T));

            VerifyType();
        }

        public BinaryCollection(IEnumerable<T> collection)
        {
            _items = new List<T>(collection);
            _typeCode = Type.GetTypeCode(typeof(T));

            VerifyType();
        }

        public BinaryCollection(IEnumerable<byte> collection)
        {
            _typeCode = Type.GetTypeCode(typeof(T));
            VerifyType();

            _items = FromBytes(collection).ToList();
        }

        private void VerifyType()
        {
            if (_typeCode == TypeCode.String ||
                _typeCode == TypeCode.Object ||
                _typeCode == TypeCode.DBNull ||
                _typeCode == TypeCode.DateTime)
                throw new NotSupportedException($"Unsupported type '{_typeCode}'. Only value types are supported.");
        }

        public byte[] ToBytes()
        {
            var result = new List<byte>();

            foreach (var item in _items)
                result.AddRange(GetBytes(item));

            return result.ToArray();
        }

        public void Add(byte[] bytes)
        {
            if (bytes.Length != SizeOf(_typeCode))
            {
                throw new ArgumentException(
                    $"{nameof(bytes)} was not equal to the binary size of a single {_typeCode}");
            }

            _items.AddRange(FromBytes(bytes));
        }

        #region Private Conversion Methods

        private IEnumerable<T> FromBytes(IEnumerable<byte> collection)
        {
            var valueSize = SizeOf(_typeCode);

            var bytes = collection.ToArray();

            if (bytes.Length % valueSize != 0)
                throw new ArgumentException($"Length of {nameof(collection)} is invalid for value type {_typeCode}.");

            var result = new List<object>();

            for (var i = 0; i < bytes.Length; i += valueSize)
            {
                switch (_typeCode)
                {
                    case TypeCode.Empty:
                        return Enumerable.Empty<T>();
                    case TypeCode.Boolean:
                        result.Add(BitConverter.ToBoolean(bytes, i));
                        break;
                    case TypeCode.Char:
                        result.Add(BitConverter.ToChar(bytes, i));
                        break;
                    case TypeCode.SByte:
                        result.Add((sbyte) bytes[i]);
                        break;
                    case TypeCode.Byte:
                        return bytes.AsEnumerable().Cast<T>();
                    case TypeCode.Int16:
                        result.Add(BitConverter.ToInt16(bytes, i));
                        break;
                    case TypeCode.UInt16:
                        result.Add(BitConverter.ToUInt16(bytes, i));
                        break;
                    case TypeCode.Int32:
                        result.Add(BitConverter.ToInt32(bytes, i));
                        break;
                    case TypeCode.UInt32:
                        result.Add(BitConverter.ToUInt32(bytes, i));
                        break;
                    case TypeCode.Int64:
                        result.Add(BitConverter.ToInt64(bytes, i));
                        break;
                    case TypeCode.UInt64:
                        result.Add(BitConverter.ToUInt64(bytes, i));
                        break;
                    case TypeCode.Single:
                        result.Add(BitConverter.ToSingle(bytes, i));
                        break;
                    case TypeCode.Double:
                        result.Add(BitConverter.ToDouble(bytes, i));
                        break;
                    case TypeCode.Decimal:
                        var bits = new int[4];
                        for (var j = 0; j <= 15; j += 4)
                            bits[j / 4] = BitConverter.ToInt32(bytes, i + j);
                        result.Add(new decimal(bits));
                        break;
                    case TypeCode.DateTime:
                    case TypeCode.Object:
                    case TypeCode.String:
                    case TypeCode.DBNull:
                        throw new NotSupportedException(
                            $"Unsupported type '{_typeCode}'. Only value types are supported.");
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_typeCode), _typeCode, null);
                }
            }

            return result.Cast<T>();
        }

        private IEnumerable<byte> GetBytes(object value)
        {
            switch (_typeCode)
            {
                case TypeCode.Empty:
                    return new byte[0];
                case TypeCode.Boolean:
                    return BitConverter.GetBytes((bool) value);
                case TypeCode.Char:
                    return BitConverter.GetBytes((char) value);
                case TypeCode.SByte:
                    unchecked
                    {
                        return new[] {(byte) (sbyte) value};
                    }
                case TypeCode.Byte:
                    return new[] {(byte) value};
                case TypeCode.Int16:
                    return BitConverter.GetBytes((short) value);
                case TypeCode.UInt16:
                    return BitConverter.GetBytes((ushort) value);
                case TypeCode.Int32:
                    return BitConverter.GetBytes((int) value);
                case TypeCode.UInt32:
                    return BitConverter.GetBytes((uint) value);
                case TypeCode.Int64:
                    return BitConverter.GetBytes((long) value);
                case TypeCode.UInt64:
                    return BitConverter.GetBytes((ulong) value);
                case TypeCode.Single:
                    return BitConverter.GetBytes((float) value);
                case TypeCode.Double:
                    return BitConverter.GetBytes((double) value);
                case TypeCode.Decimal:
                    return decimal.GetBits((decimal) value).SelectMany(BitConverter.GetBytes).ToArray();
                case TypeCode.DateTime:
                case TypeCode.Object:
                case TypeCode.String:
                case TypeCode.DBNull:
                    throw new NotSupportedException($"Unsupported type '{_typeCode}'. Only value types are supported.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(_typeCode), _typeCode, null);
            }
        }

        private static int SizeOf(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Empty:
                    return 0;
                case TypeCode.Boolean:
                    return sizeof(bool);
                case TypeCode.Char:
                    return sizeof(char);
                case TypeCode.SByte:
                    return sizeof(sbyte);
                case TypeCode.Byte:
                    return sizeof(byte);
                case TypeCode.Int16:
                    return sizeof(short);
                case TypeCode.UInt16:
                    return sizeof(ushort);
                case TypeCode.Int32:
                    return sizeof(int);
                case TypeCode.UInt32:
                    return sizeof(uint);
                case TypeCode.Int64:
                    return sizeof(long);
                case TypeCode.UInt64:
                    return sizeof(ulong);
                case TypeCode.Single:
                    return sizeof(float);
                case TypeCode.Double:
                    return sizeof(double);
                case TypeCode.Decimal:
                    return sizeof(decimal);
                case TypeCode.DateTime:
                case TypeCode.Object:
                case TypeCode.String:
                case TypeCode.DBNull:
                    throw new NotSupportedException($"Unsupported type '{typeCode}'. Only value types are supported.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeCode), typeCode, null);
            }
        }

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _items).GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<T>

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        #endregion
    }
}
