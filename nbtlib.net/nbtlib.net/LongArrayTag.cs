using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Immutable;

namespace NbtLib
{
    public class LongArrayTag : AbstractListTag<LongTag, long>
    {
        public static readonly ITagReader READER = new LongArrayType();
        private class LongArrayType : ITagReader
        {
            public ITag Read(BinaryReader reader, int _)
            {
                var i = reader.ReadInt32();
                var along = new List<long>(i);

                for (var j = 0; j < i; j++)
                    along.Add(reader.ReadInt64());

                return new LongArrayTag(along);
            }
            public string CrashReportName =>
                "LONG[]";
            public string CommandFeedbackName =>
                "TAG_Long_Array";
        }

        public LongArrayTag(long[] array)
            : this(array.ToList()) { }
        public LongArrayTag(List<long?> list)
            : this(ToArray(list)) { }
        public LongArrayTag(List<long> list)
            : base(list) { }

        private static List<long> ToArray(List<long?> list)
        {
            var aint = new List<long>(list.Count);

            for (var i = 0; i < list.Count; i++)
                aint.Add(list[i] ?? 0);

            return aint;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(value.Count);

            foreach (var i in value)
                writer.Write(i);
        }

        public override byte Type => 12;

        public override ITagReader Reader => READER;

        public override string ToString()
        {
            var sb = new StringBuilder("[L;");

            for (var i = 0; i < value.Count; i++)
            {
                if (i != 0)
                    sb.Append(',');
                sb.Append(value[i]).Append('L');
            }

            return sb.Append(']').ToString();
        }

        public override LongArrayTag Copy()
        {
            var along = new List<long>(value);
            return new LongArrayTag(along);
        }

        public ImmutableList<long> GetLongArray() =>
            value.ToImmutableList();

        public override int Size =>
            value.Count;

        public override byte ElementType => 4;

        public override LongTag Get(int index) =>
            LongTag.Of(value[index]);

        public override LongTag Set(int index, LongTag tag)
        {
            changed = true;
            var i = value[index];
            value[index] = tag.GetInt();
            return LongTag.Of(i);
        }
        public override void Add(int index, LongTag tag)
        {
            changed = true;
            value.Insert(index, tag.GetLong());
        }

        public override bool SetTag(int index, ITag tag)
        {
            if (tag is AbstractNumberTag nt)
            {
                changed = true;
                value[index] = nt.GetLong();
                return true;
            }
            return false;
        }

        public override bool AddTag(int index, ITag tag)
        {
            if (tag is AbstractNumberTag nt)
            {
                changed = true;
                value.Insert(index, nt.GetLong());
                return true;
            }
            return false;
        }

        public override LongTag Remove(int index)
        {
            changed = true;
            var i = value[index];
            value.RemoveAt(index);
            return LongTag.Of(i);
        }
    }
}
