using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections.Immutable;

namespace NbtLib
{
    public class ByteArrayTag : AbstractListTag<ByteTag, byte>
    {
        public static readonly ITagReader READER = new ByteArrayType();
        private class ByteArrayType : ITagReader
        {
            public ITag Read(BinaryReader reader, int depth) => 
                new ByteArrayTag(reader.ReadBytes(reader.ReadInt32()));

            public string CrashReportName =>
                "BYTE[]";
            public string CommandFeedbackName =>
                "TAG_Byte_Array";
        }

        public ByteArrayTag(byte[] value)
            : this(new List<byte>(value)) { }

        public ByteArrayTag(List<byte?> list)
            : this(ToArray(list)) { }

        public ByteArrayTag(List<byte> list)
            : base(list) { }

        private static List<byte> ToArray(List<byte?> list)
        {
            var abyte = new List<byte>(list.Count);

            for (var i = 0; i < list.Count; i++)
                abyte.Add(list[i] ?? 0);

            return abyte;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(value.Count);
            writer.Write(value.ToArray());
        }

        public override byte Type => 7;
        public override ITagReader Reader => READER;

        public override string ToString()
        {
            var sb = new StringBuilder("[B;");

            for (var i = 0; i < value.Count; i++)
            {
                if (i != 0)
                    sb.Append(',');
                sb.Append(value[i]).Append('B');
            }
            return sb.Append(']').ToString();
        }

        public override ByteArrayTag Copy() => 
            new ByteArrayTag(new List<byte>(value));

        public ImmutableList<byte> GetByteArray() =>
            value.ToImmutableList();

        public override int Size =>
            value.Count;

        public override ByteTag Get(int index) =>
            ByteTag.Of(value[index]);

        public override ByteTag Set(int index, ByteTag tag)
        {
            changed = true;
            var b0 = value[index];
            value[index] = tag.GetByte();
            return ByteTag.Of(b0);
        }

        public override void Add(int index, ByteTag item)
        {
            changed = true;
            value.Insert(index, item.GetByte());
        }

        public override bool SetTag(int index, ITag tag)
        {
            if (tag is AbstractNumberTag nt)
            {
                changed = true;
                value[index] = nt.GetByte();
                return true;
            }
            return false;
        }

        public override bool AddTag(int index, ITag tag)
        {
            if (tag is AbstractNumberTag nt)
            {
                changed = true;
                value.Insert(index, nt.GetByte());
                return true;
            }
            return false;
        }

        public override ByteTag Remove(int index)
        {
            changed = true;
            var b0 = value[index];
            value.RemoveAt(index);
            return ByteTag.Of(b0);
        }

        public override byte ElementType => 1;
    }
}
