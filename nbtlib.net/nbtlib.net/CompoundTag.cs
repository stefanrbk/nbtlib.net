using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Map = System.Collections.Generic.Dictionary<string, NbtLib.ITag>;
using System.Collections.Immutable;

namespace NbtLib
{
    public class CompoundTag : ITag
    {
        private bool changed = true;
        private int hash;

        private static readonly Regex PATTERN = new Regex("[A-Za-z0-9._+-]+");
        public static readonly ITagReader READER = new CompoundType();
        private class CompoundType : ITagReader
        {
            public ITag Read(BinaryReader reader, int depth)
            {
                if (depth > 512)
                    throw new Exception("Tried to read NBT tag with too high complexity, depth > 512");

                var map = new Map();

                byte b;
                while ((b = CompoundTag.ReadByte(reader)) != 0)
                {
                    var s = ReadString(reader);
                    var inbt = CompoundTag.Read(TagReaders.Of(b), s, reader, depth + 1);
                    map.TryAdd(s, inbt);
                }

                return new CompoundTag(map);
            }

            public string CrashReportName =>
                "COMPOUND";

            public string CommandFeedbackName =>
                "TAG_Compound";
        }

        private readonly Map tags;

        private CompoundTag(Map tags) =>
            this.tags = tags;

        public CompoundTag()
            : this(new Map()) { }

        public void Write(BinaryWriter writer)
        {
            foreach (var kvp in tags)
                Write(kvp.Key, kvp.Value, writer);

            writer.Write((byte)0);
        }

        public List<string> Keys =>
            tags.Keys.ToList();
        public byte Type =>
            10;
        public ITagReader Reader =>
            READER;

        public int Size =>
            tags.Count;

        public ITag Add(string key, ITag value)
        {
            changed = true;
            tags.AddOrReplace(key, value);
            return value;
        }

        public void AddByte(string key, byte value)
        {
            changed = true;
            tags.AddOrReplace(key, ByteTag.Of(value));
        }

        public void AddShort(string key, short value)
        {
            changed = true;
            tags.AddOrReplace(key, ShortTag.Of(value));
        }

        public void AddInt(string key, int value)
        {
            changed = true;
            tags.AddOrReplace(key, IntTag.Of(value));
        }

        public void AddLong(string key, long value)
        {
            changed = true;
            tags.AddOrReplace(key, LongTag.Of(value));
        }

        public void AddGuid(string key, Guid value)
        {
            changed = true;
            var (MSB, LSB) = GetGuidBits(value);
            AddLong(key + "Most", MSB);
            AddLong(key + "Least", LSB);
        }
        public Guid GetGuid(string key) =>
            BuildGuid(GetLong(key + "Most"), GetLong(key + "Least"));
        public bool ContainsGuid(string key) =>
            Contains(key + "Most", 99) && Contains(key + "Least", 99);
        public void RemoveGuid(string key)
        {
            changed = true;
            Remove(key + "Most");
            Remove(key + "Least");
        }
        public void AddFloat(string key, float value)
        {
            changed = true;
            tags.AddOrReplace(key, FloatTag.Of(value));
        }

        public void AddDouble(string key, double value)
        {
            changed = true;
            tags.AddOrReplace(key, DoubleTag.Of(value));
        }

        public void AddString(string key, string value)
        {
            changed = true;
            tags.AddOrReplace(key, StringTag.Of(value));
        }

        public void AddByteArray(string key, byte[] value)
        {
            changed = true;
            tags.AddOrReplace(key, new ByteArrayTag(value));
        }

        public void AddByteArray(string key, List<byte> value)
        {
            changed = true;
            tags.AddOrReplace(key, new ByteArrayTag(value));
        }

        public void AddByteArray(string key, List<byte?> value)
        {
            changed = true;
            tags.AddOrReplace(key, new ByteArrayTag(value));
        }

        public void AddIntArray(string key, int[] value)
        {
            changed = true;
            tags.AddOrReplace(key, new IntArrayTag(value));
        }

        public void AddIntArray(string key, List<int?> value)
        {
            changed = true;
            tags.AddOrReplace(key, new IntArrayTag(value));
        }

        public void AddIntArray(string key, List<int> value)
        {
            changed = true;
            tags.AddOrReplace(key, new IntArrayTag(value));
        }

        public void AddLongArray(string key, long[] value)
        {
            changed = true;
            tags.AddOrReplace(key, new LongArrayTag(value));
        }

        public void AddLongArray(string key, List<long?> value)
        {
            changed = true;
            tags.AddOrReplace(key, new LongArrayTag(value));
        }

        public void AddLongArray(string key, List<long> value)
        {
            changed = true;
            tags.AddOrReplace(key, new LongArrayTag(value));
        }

        public void AddBoolean(string key, bool value)
        {
            changed = true;
            tags.AddOrReplace(key, ByteTag.Of(value));
        }

        public ITag? Get(string key) =>
            tags.ContainsKey(key) ? tags[key] : null;

        public byte GetType(string key)
        {
            var tag = tags[key];
            return tag is null ? 0 : tag.Type;
        }

        public bool Contains(string key) =>
            tags.ContainsKey(key);

        public bool Contains(string key, int type)
        {
            var i = GetType(key);
            if (i == type)
                return true;
            else if (type != 99)
                return true;
            else
                return i is >= 1 and <= 6;
        }
        public byte GetByte(string key)
        {
            if (Contains(key, 99))
                return (tags[key] as AbstractNumberTag)?.GetByte() ?? 0;
            return 0;
        }
        public short GetShort(string key)
        {
            if (Contains(key, 99))
                return (tags[key] as AbstractNumberTag)?.GetShort() ?? 0;
            return 0;
        }
        public int GetInt(string key)
        {
            if (Contains(key, 99))
                return (tags[key] as AbstractNumberTag)?.GetInt() ?? 0;
            return 0;
        }
        public long GetLong(string key)
        {
            if (Contains(key, 99))
                return (tags[key] as AbstractNumberTag)?.GetLong() ?? 0;
            return 0;
        }
        public float GetFloat(string key)
        {
            if (Contains(key, 99))
                return (tags[key] as AbstractNumberTag)?.GetFloat() ?? 0;
            return 0;
        }
        public double GetDouble(string key)
        {
            if (Contains(key, 99))
                return (tags[key] as AbstractNumberTag)?.GetDouble() ?? 0;
            return 0;
        }
        public string GetString(string key)
        {
            if (Contains(key, 8))
                return tags[key].ToString() ?? "";
            return "";
        }
        public ImmutableList<byte>? GetByteArray(string key) =>
            Contains(key, 7)
                ? (tags[key] as ByteArrayTag)?.GetByteArray() ?? throw ExceptionBuilder(key, ByteArrayTag.READER)
                : null;
        public ImmutableList<int>? GetIntArray(string key) =>
            Contains(key, 11)
                ? (tags[key] as IntArrayTag)?.GetIntArray() ?? throw ExceptionBuilder(key, IntArrayTag.READER)
                : null;
        public ImmutableList<long>? GetLongArray(string key) =>
            Contains(key, 12)
                ? (tags[key] as LongArrayTag)?.GetLongArray() ?? throw ExceptionBuilder(key, LongArrayTag.READER)
                : null;
        public CompoundTag GetCompound(string key) =>
            Contains(key, 10)
                ? (tags[key] as CompoundTag) ?? throw ExceptionBuilder(key, READER)
                : new CompoundTag();
        public ListTag GetList(string key, int type) =>
            GetType(key) == 9
                ? (tags[key] is ListTag lt) && !lt.IsEmpty && lt.ElementType != type
                    ? new ListTag()
                    : (ListTag)tags[key]
                : throw ExceptionBuilder(key, ListTag.READER);
        public bool GetBoolean(string key) =>
            GetByte(key) != 0;

        public void Remove(string key)
        {
            changed = true;
            tags.Remove(key);
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder("{");

            foreach (var s in tags.Keys)
            {
                if (sb.Length != 1)
                    sb.Append(',');
                sb.Append(EscapeTagKey(s)).Append(':').Append(tags[s]);
            }
            return sb.Append('}').ToString();
        }

        public bool IsEmpty =>
            tags.Count == 0;

        private InvalidCastException ExceptionBuilder(string key, ITagReader reader) =>
            new InvalidCastException($"Tag type found \"{tags[key].Reader.CrashReportName}\" when expecting " +
                $"\"{reader.CrashReportName}\" on tag named \"{key}\"");

        public CompoundTag Copy() => 
            new CompoundTag(new Map(tags));

        public override bool Equals(object? obj) =>
            this == obj || obj is CompoundTag ct && tags.SequenceEqual(ct.tags);

        public override int GetHashCode()
        {
            if (changed)
            {
                changed = false;
                hash = 0;
                foreach (var kvp in tags)
                    hash += StringHash(kvp.Key) ^ kvp.Value.GetHashCode();
            }
            return hash;
        }
        private static int StringHash(string value)
        {
            var h = 0;
            var i = 0;
            var n = value.Length;
            foreach (var s in value)
                h += s * 31 ^ (n - ++i);
            return h;
        }

        private static (long MSB, long LSB) GetGuidBits(Guid value)
        {
            var b = value.ToByteArray();
            byte[] msBits = new byte[]
            {
                b[3], b[2], b[1], b[0], b[5], b[4], b[7], b[6]
            };
            var lsBits = new byte[]
            {
                b[8], b[9], b[10], b[11], b[12], b[13], b[14], b[15]
            };
            return (BinaryPrimitives.ReadInt64BigEndian(msBits), BinaryPrimitives.ReadInt64BigEndian(lsBits));
        }
        private static Guid BuildGuid(long msb, long lsb)
        {
            var mb = new byte[8];
            var lb = new byte[8];
            BinaryPrimitives.WriteInt64BigEndian(mb, msb);
            BinaryPrimitives.WriteInt64BigEndian(lb, lsb);
            var bits = new byte[]
            {
                mb[3], mb[2], mb[1], mb[0], mb[5], mb[4], mb[7], mb[6],
                lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7]
            };
            return new Guid(bits);
        }

        private static void Write(string key, ITag tag, BinaryWriter writer)
        {
            writer.Write(tag.Type);
            if (tag.Type != 0)
            {
                writer.WriteUTF(key);
                tag.Write(writer);
            }
        }

        private static byte ReadByte(BinaryReader reader) => 
            reader.ReadByte();

        private static string ReadString(BinaryReader reader) =>
            reader.ReadUTF();

        private static ITag Read(ITagReader reader, string key, BinaryReader input, int depth)
        {
            try
            {
                return reader.Read(input, depth);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public CompoundTag CopyFrom(CompoundTag source)
        {
            foreach (var s in source.tags.Keys)
            {
                var tag = source.tags[s];
                if (tag.Type == 10)
                {
                    if (Contains(s, 10))
                    {
                        var compound = GetCompound(s);
                        compound.CopyFrom((CompoundTag)tag);
                    }
                    else
                        Add(s, tag.Copy());
                }
                else
                    Add(s, tag.Copy());
            }

            return this;
        }

        protected static string EscapeTagKey(string s) =>
            PATTERN.IsMatch(s) ? s : StringTag.Escape(s);

        protected IDictionary<string, ITag> AsDictionary =>
            new ReadOnlyDictionary<string, ITag>(tags);

        ITag ITag.Copy() =>
            Copy();

    }
    internal static class DictionaryExtension
    {
        public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) where TKey : notnull
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }
    }
}
