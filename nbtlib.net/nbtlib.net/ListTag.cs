using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace NbtLib
{
    public class ListTag : AbstractListTag<ITag, ITag>
    {
        public static readonly ITagReader READER = new ListType();
        private class ListType : ITagReader
        {
            public ITag Read(BinaryReader reader, int depth)
            {
                if (depth > 512)
                    throw new Exception("Tried to read NBT tag with too high complexity, depth > 512");
                else
                {
                    var listType = reader.ReadByte();
                    var count = reader.ReadInt32();
                    if (listType == 0 && count > 0)
                        throw new Exception("Missing type on ListTag");
                    else
                    {
                        ITagReader tagReader = TagReaders.Of(listType);
                        var list = new List<ITag>(count);

                        for (var j = 0; j < count; j++)
                            list.Add(tagReader.Read(reader, depth + 1));

                        return new ListTag(list, listType);
                    }
                }
            }
            public string CrashReportName =>
                "LIST";

            public string CommandFeedbackName =>
                "TAG_List";

        }

        private ListTag(List<ITag> list, byte type)
            : base(list)
        {
            foreach (var tag in value)
                if (tag.Type != type)
                    throw new ArgumentException($"The list \"{nameof(list)}\" contains different tag types, which is not supported. " +
                        $"Expected type was \"{type}\" and found a tag of type \"{tag.Type}\".", nameof(list));
        }

        public ListTag()
            : this(new List<ITag>(), 0) { }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(ElementType);
            writer.Write(value.Count);

            foreach (var tag in value)
                tag.Write(writer);
        }

        public override byte Type => 9;
        public override ITagReader Reader => READER;

        public override string ToString()
        {
            var sb = new StringBuilder("[");

            for (var i = 0; i < value.Count; i++)
            {
                if (i != 0)
                    sb.Append(',');

                sb.Append(value[i]);
            }
            return sb.Append(']').ToString();
        }

        public override ITag Remove(int index)
        {
            changed = true;
            var tag = value[index];
            value.RemoveAt(index);
            return tag;
        }

        public bool IsEmpty =>
            value.Count == 0;

        private T? Get<T>(int i, byte id) where T:ITag
        {
            if (i >= 0 && i < value.Count)
            {
                var tag = value[i];
                if (tag.Type == id)
                    return (T)tag;
            }
            return default;
        }

        public CompoundTag GetCompound(int index) =>
            Get<CompoundTag>(index, 10) ?? new CompoundTag();
        public ListTag GetList(int index) =>
            Get<ListTag>(index, 9) ?? new ListTag();
        public short GetShort(int index) =>
            Get<ShortTag>(index, 2)?.GetShort() ?? 0;
        public int GetInt(int index) =>
            Get<IntTag>(index, 3)?.GetInt() ?? 0;
        public ImmutableList<int> GetIntArray(int index) =>
            Get<IntArrayTag>(index, 11)?.GetIntArray() ?? new List<int>().ToImmutableList();
        public double GetDouble(int index) =>
            Get<DoubleTag>(index, 6)?.GetDouble() ?? 0;
        public float GetFloat(int index) =>
            Get<FloatTag>(index, 5)?.GetFloat() ?? 0;
        public string GetString(int index)
        {
            if (index >= 0 && index < value.Count)
            {
                var tag = value[index];
                return tag.Type == 8 ? tag.AsString : tag.ToString() ?? "";
            }
            else
                return "";
        }

        public override int Size =>
            value.Count;

        public override ITag Get(int index) =>
            value[index];

        public override ITag Set(int index, ITag tag)
        {
            var tag2 = value[index];
            if (!SetTag(index, tag))
                throw new InvalidOperationException($"Trying to add tag of type {tag.Type} to list of {ElementType}");
            return tag2;
        }

        public override void Add(int index, ITag tag)
        {
            if (!AddTag(index, tag))
                throw new InvalidOperationException($"Trying to add tag of type {tag.Type} to list of {ElementType}");
        }

        public override bool SetTag(int index, ITag tag)
        {
            if (CanAdd(tag))
            {
                changed = true;
                value[index] = tag;
                return true;
            }
            return false;
        }

        public override bool AddTag(int index, ITag tag)
        {
            if (CanAdd(tag))
            {
                changed = true;
                value.Insert(index, tag);
                return true;
            }
            return false;
        }

        private bool CanAdd(ITag tag)
        {
            if (tag.Type == 0)
                return false;
            else if (ElementType == 0)
                return true;
            return ElementType == tag.Type;
        }

        public override AbstractListTag<ITag, ITag> Copy()
        {
            var iter = TagReaders.Of(ElementType).IsImmutable ? value : value.Select(a => a.Copy());
            var list = new List<ITag>(iter);
            return new ListTag(list, ElementType);
        }

        public override byte ElementType =>
            value.Count > 0 ? value[0].Type : 0;


    }
}