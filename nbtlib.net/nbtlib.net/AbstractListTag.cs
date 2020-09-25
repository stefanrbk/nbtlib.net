using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NbtLib
{
    public abstract class AbstractListTag<TTag, TType> : ITag, IEnumerable<TType> where TTag : ITag
    {
        protected bool changed = true;
        protected int hash;

        protected readonly List<TType> value;
        protected AbstractListTag(List<TType> list) =>
            value = list;

        public abstract ITag Get(int index);
        public abstract TTag Set(int index, TTag tag);
        public abstract void Add(int index, TTag tag);
        public abstract TTag Remove(int index);
        public abstract bool SetTag(int index, ITag tag);
        public abstract bool AddTag(int index, ITag tag);
        public abstract int Size { get; }

        public abstract byte ElementType { get; }
        public abstract byte Type { get; }
        public abstract ITagReader Reader { get; }
        public abstract void Write(BinaryWriter writer);
        public abstract ITag Copy();

        public sealed override bool Equals(object? obj) =>
            this == obj || obj is AbstractListTag<TTag, TType> tag && value.SequenceEqual(tag.value);
        public sealed override int GetHashCode()
        {
            if (changed)
            {
                hash = 0;
                for (var i = 0; i < Size; i++)
                    hash = HashCode.Combine(hash, Get(i));
                changed = false;
            }
            return hash;
        }

        public void Clear()
        {
            changed = true;
            value.Clear();
        }

        public IEnumerator<TType> GetEnumerator()
            => value.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
