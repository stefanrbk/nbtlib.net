using System;
using System.Collections.Generic;

namespace NbtLib
{
    public class NBTTagList : NbtValue
    {
        private readonly List<NbtValue> _payload = new List<NbtValue>();
        private readonly TagType _type;

        public void AppendTag(NbtValue nbt) => _payload.Add(nbt);
        public NbtValue Get(int idx) => _payload[idx];
        public NBTTagCompound GetCompoundTagAt(int i)
        {
            if (_type != TagType.Compound)
                throw new InvalidOperationException();
            return (NBTTagCompound)_payload[i];
        }
        public double GetDoubleAt(int i)
        {
            if (_type != TagType.Double)
                throw new InvalidOperationException();
            return ((NBTTagDouble)_payload[i]).GetDouble();
        }
        public float GetFloatAt(int i)
        {
            if (_type != TagType.Float)
                throw new InvalidOperationException();
            return ((NBTTagFloat)_payload[i]).GetFloat();
        }
        public int[] GetIntArrayAt(int i)
        {
            if (_type != TagType.IntArray)
                throw new InvalidOperationException();
            return ((NBTTagIntArray)_payload[i]).GetIntArray();
        }
        public int GetIntegerAt(int i)
        {
            if (_type != TagType.Int)
                throw new InvalidOperationException();
            return ((NBTTagInt)_payload[i]).GetInt();
        }
        public NBTTagList GetListAt(int i)
        {
            if (_type != TagType.List)
                throw new InvalidOperationException();
            return (NBTTagList)_payload[i];
        }
        public string GetStringAt(int i)
        {
            if (_type != TagType.String)
                throw new InvalidOperationException();
            return ((NBTTagString)_payload[i]).GetString();
        }
        public TagType GetTagType() => _type;
        public NbtValue RemoveTag(int i)
        {
            var val = _payload[i];
            _payload.RemoveAt(i);
            return val;
        }
        public void Set(int idx, NbtValue nbt) => _payload[idx] = nbt;
        public int TagCount() => _payload.Count;
        public NBTTagList(string name, TagType type) : base(TagType.List, name) => _type = type;
        public NBTTagList(string name, TagType type, List<NbtValue> list) : this(name, type) => _payload = list;
    }
}