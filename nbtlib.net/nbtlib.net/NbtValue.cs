using java.nio.channels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using NbtPair = System.Collections.Generic.KeyValuePair<string, NbtLib.NbtValue?>;

namespace NbtLib
{
    public abstract class NbtValue : IEnumerable
    {
        private static readonly UTF8Encoding _utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false,
                                                                      throwOnInvalidBytes: true);

        public static NbtValue Load(Stream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            using var reader = new BinaryReader(stream, _utf8);
            return Load(reader);
        }

        public static NbtValue Load(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return ToNbtValue(new NbtTagReader(reader).Read());
        }

        private static IEnumerable<NbtPair> ToNbtPairEnumerable(IEnumerable<KeyValuePair<string, object>> kvpc)
        {
            foreach (var kvp in kvpc)
                yield return new NbtPair(kvp.Key, ToNbtValue(kvp.Value));
        }

        private static IEnumerable<NbtValue?> ToNbtValueEnumerable(IEnumerable<object> arr)
        {
            foreach (var obj in arr)
            {
                yield return ToNbtValue(obj);
            }
        }

        private static NbtValue? ToNbtValue(object ret)
        {
            if (ret is null)
                return null;

            if (ret is IEnumerable<KeyValuePair<string, object>> kvpc)
                return new NbtCompound(ToNbtPairEnumerable(kvpc));

            if (ret is IEnumerable<object> arr)
                return new NbtList(ToNbtValueEnumerable(arr));

            if (ret is byte) return new NbtPrimitive((byte)ret);
            if (ret is byte[]) return new NbtPrimitive((byte[])ret);
            if (ret is double) return new NbtPrimitive((double)ret);
            if (ret is float) return new NbtPrimitive((float)ret);
            if (ret is int) return new NbtPrimitive((int)ret);
            if (ret is int[]) return new NbtPrimitive((int[])ret);
            if (ret is long) return new NbtPrimitive((long)ret);
            if (ret is long[]) return new NbtPrimitive((long[])ret);
            if (ret is short) return new NbtPrimitive((short)ret);
            if (ret is string) return new NbtPrimitive((string)ret);

            throw new ArgumentException($"{nameof(ret)} is of type {ret.GetType()} and is not a NbtValue type");
        }

        public virtual int Count =>
            throw new InvalidOperationException();

        public abstract NbtType NbtType { get; }

        public virtual NbtValue this[int index]
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }

        public virtual NbtValue this[string key]
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }

        public virtual bool ContainsKey(string key) =>
            throw new InvalidOperationException();

        public virtual void Save(Stream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            using var writer = new BinaryWriter(stream, _utf8);
            Save(writer);
        }

        public virtual void Save(BinaryWriter writer)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            SaveInternal(writer);
        }

        private void SaveInternal(BinaryWriter w)
        {
            switch(NbtType)
            {
                case NbtType.Object:
                    break;
                case NbtType.List:
                    break;
                case NbtType.String:
                    break;
                case NbtType.Number:
                    break;
            }
        }
        protected static byte[] Endianness(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                return bytes.Reverse().ToArray();
            return bytes;
        }



        IEnumerator IEnumerable.GetEnumerator() =>
            throw new InvalidOperationException();

        public static implicit operator NbtValue(byte value) => new NbtPrimitive(value);
        public static implicit operator NbtValue(byte[] value) => new NbtPrimitive(value);
        public static implicit operator NbtValue(double value) => new NbtPrimitive(value);
        public static implicit operator NbtValue(float value) => new NbtPrimitive(value);
        public static implicit operator NbtValue(int value) => new NbtPrimitive(value);
        public static implicit operator NbtValue(int[] value) => new NbtPrimitive(value);
        public static implicit operator NbtValue(long value) => new NbtPrimitive(value);
        public static implicit operator NbtValue(long[] value) => new NbtPrimitive(value);
        public static implicit operator NbtValue(short value) => new NbtPrimitive(value);
        public static implicit operator NbtValue(string value) => new NbtPrimitive(value);
    }
}
