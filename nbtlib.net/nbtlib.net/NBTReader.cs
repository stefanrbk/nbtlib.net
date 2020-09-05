using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace nbtlib.net
{
    public class NBTReader
    {
        private string _path;
        public NBTReader(string path)
        {
            _path = path;
        }
        public NBTBase ReadFile()
        {
            using var stream = new GZipStream(File.OpenRead(_path), CompressionMode.Decompress);
            using var reader = new BinaryReader(stream);

            return ReadTag(reader);
        }

        private static NBTBase ReadTag(BinaryReader reader)
        {
            var tagType = (TagType)reader.ReadSByte();
            if (tagType == TagType.End)
                return new NBTTagEnd();

            var nameLength = BitConverter.ToUInt16(Endianness(reader.ReadBytes(2)), 0);
            var name = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));
            return tagType switch
            {
                TagType.Byte => new NBTTagByte(name, ReadByte(reader)),
                TagType.Short => new NBTTagShort(name, ReadShort(reader)),
                TagType.Int => new NBTTagInt(name, ReadInt(reader)),
                TagType.Long => new NBTTagLong(name, ReadLong(reader)),
                TagType.Float => new NBTTagFloat(name, ReadFloat(reader)),
                TagType.Double => new NBTTagDouble(name, ReadDouble(reader)),
                TagType.ByteArray => new NBTTagByteArray(name, ReadByteArray(reader)),
                TagType.String => new NBTTagString(name, ReadString(reader)),
                TagType.List => NewList(reader, name),
                TagType.Compound => new NBTTagCompound(name, ReadCompound(reader)),
                TagType.IntArray => new NBTTagIntArray(name, ReadIntArray(reader)),
                TagType.LongArray => new NBTTagLongArray(name, ReadLongArray(reader)),
                _ => null
            };
        }
        private static string ReadString(BinaryReader reader)
        {
            var charCount = BitConverter.ToInt16(Endianness(reader.ReadBytes(2)), 0);
            return Encoding.UTF8.GetString(reader.ReadBytes(charCount));
        }
        private static double ReadDouble(BinaryReader reader) => BitConverter.ToDouble(Endianness(reader.ReadBytes(8)), 0);
        private static float ReadFloat(BinaryReader reader) => BitConverter.ToSingle(Endianness(reader.ReadBytes(4)), 0);
        private static long ReadLong(BinaryReader reader) => BitConverter.ToInt64(Endianness(reader.ReadBytes(8)), 0);
        private static short ReadShort(BinaryReader reader) => BitConverter.ToInt16(Endianness(reader.ReadBytes(2)), 0);
        private static sbyte[] ReadByteArray(BinaryReader reader)
        {
            var count = ReadInt(reader);
            var val = new sbyte[count];
            for (var i = 0; i < count; i++)
                val[i] = ReadByte(reader);
            return val;
        }
        private static int[] ReadIntArray(BinaryReader reader)
        {
            var count = ReadInt(reader);
            var val = new int[count];
            for (var i = 0; i < count; i++)
                val[i] = ReadInt(reader);
            return val;
        }
        private static long[] ReadLongArray(BinaryReader reader)
        {
            var count = ReadInt(reader);
            var val = new long[count];
            for (var i = 0; i < count; i++)
                val[i] = ReadLong(reader);
            return val;
        }
        private static int ReadInt(BinaryReader reader) => BitConverter.ToInt32(Endianness(reader.ReadBytes(4)), 0);
        private static sbyte ReadByte(BinaryReader reader) => reader.ReadSByte();
        private static List<NBTBase> ReadList(BinaryReader reader, out TagType type)
        {
            type = (TagType)reader.ReadSByte();
            var count = BitConverter.ToInt16(Endianness(reader.ReadBytes(2)), 0);
            var val = new List<NBTBase>(count);
            for(var i = 0; i <= count; i++)
            {
                val.Add(type switch
                {
                TagType.Byte =>         new NBTTagByte(i.ToString(), ReadByte(reader)),
                TagType.ByteArray =>    new NBTTagByteArray(i.ToString(), ReadByteArray(reader)),
                TagType.Compound =>     new NBTTagCompound(i.ToString(), ReadCompound(reader)),
                TagType.Double =>       new NBTTagDouble(i.ToString(), ReadDouble(reader)),
                TagType.End =>          new NBTTagEnd(),
                TagType.Float =>        new NBTTagFloat(i.ToString(), ReadFloat(reader)),
                TagType.Int =>          new NBTTagInt(i.ToString(), ReadInt(reader)),
                TagType.IntArray =>     new NBTTagIntArray(i.ToString(), ReadIntArray(reader)),
                TagType.List =>         NewList(reader, i.ToString()),
                TagType.Long =>         new NBTTagLong(i.ToString(), ReadLong(reader)),
                TagType.LongArray =>    new NBTTagLongArray(i.ToString(), ReadLongArray(reader)),
                TagType.Short =>        new NBTTagShort(i.ToString(), ReadShort(reader)),
                TagType.String =>       new NBTTagString(i.ToString(), ReadString(reader)),
                _ =>                    null
                });
            }
            return val;
        }
        private static NBTTagList NewList(BinaryReader reader, string name)
        {
            var list = ReadList(reader, out var t);
            return new NBTTagList(name, t, list);
        }
        private static Dictionary<string, NBTBase> ReadCompound(BinaryReader reader)
        {
            var val = new Dictionary<string, NBTBase>();
            NBTBase tag;
            do
            {
                tag = ReadTag(reader);
                val.Add(tag.Name, tag);
            } while (tag.Id != TagType.End);
            return val;
        }
        private static byte[] Endianness(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                return bytes.Reverse().ToArray();
            return bytes;
        }
    }
}
