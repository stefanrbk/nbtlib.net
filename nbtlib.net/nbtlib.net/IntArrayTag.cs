using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Immutable;

namespace NbtLib
{
    public class IntArrayTag : AbstractListTag<IntTag, int>
    {
        public static readonly ITagReader READER = new IntArrayType();
        private class IntArrayType : ITagReader
        {
            public ITag Read(BinaryReader reader, int _)
            {
                var i = reader.ReadInt32();
                var aint = new List<int>(i);

                for (var j = 0; j < i; j++)
                    aint.Add(reader.ReadInt32());

                return new IntArrayTag(aint);
            }
            public string CrashReportName =>
                "INT[]";
            public string CommandFeedbackName =>
                "TAG_Int_Array";
        }

        public IntArrayTag(int[] array)
            : this(array.ToList()) { }
        public IntArrayTag(List<int?> list)
            : this(ToArray(list)) { }
        public IntArrayTag(List<int> list)
            : base(list) { }

        private static List<int> ToArray(List<int?> list)
        {
            var aint = new List<int>(list.Count);

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

        public override byte Type => 11;

        public override ITagReader Reader => READER;

        public override string ToString()
        {
            var sb = new StringBuilder("[I;");

            for (var i = 0; i < value.Count; i++)
            {
                if (i != 0)
                    sb.Append(',');
                sb.Append(value[i]);
            }

            return sb.Append(']').ToString();
        }

        public override IntArrayTag Copy() => 
            new IntArrayTag(new List<int>(value));

        public ImmutableList<int> GetIntArray() =>
            value.ToImmutableList();

        public override int Size =>
            value.Count;

        public override byte ElementType =>
            3;

        public override IntTag Get(int index) =>
            IntTag.Of(value[index]);

        public override IntTag Set(int index, IntTag tag)
        {
            changed = true;
            var i = value[index];
            value[index] = tag.GetInt();
            return IntTag.Of(i);
        }
        public override void Add(int index, IntTag tag)
        {
            changed = true;
            value.Insert(index, tag.GetInt());
        }

        public override bool SetTag(int index, ITag tag)
        {
            if (tag is AbstractNumberTag nt)
            {
                changed = true;
                value[index] = nt.GetInt();
                return true;
            }
            return false;
        }

        public override bool AddTag(int index, ITag tag)
        {
            if (tag is AbstractNumberTag nt)
            {
                changed = true;
                value.Insert(index, nt.GetInt());
                return true;
            }
            return false;
        }

        public override IntTag Remove(int index)
        {
            changed = true;
            var i = value[index];
            value.RemoveAt(index);
            return IntTag.Of(i);
        }
    }
}
