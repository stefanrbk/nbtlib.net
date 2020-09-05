using java.util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nbtlib
{
    public class NBTTagCompound : NBTBase
    {
        private Dictionary<string, NBTBase> _payload;
        public NBTTagCompound(string name) : base(TagType.Compound, name) => _payload = new Dictionary<string, NBTBase>();
        internal NBTTagCompound(string name, Dictionary<string, NBTBase> value) : this(name) => _payload = value;
        public bool GetBoolean(string key)
        {
            var val = GetByte(key);
            return val != 0;
        }
        public sbyte GetByte(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.Byte)
                throw new InvalidOperationException();
            return ((NBTTagByte)val).GetByte();
        }
        public sbyte[] GetByteArray(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.ByteArray)
                throw new InvalidOperationException();
            return ((NBTTagByteArray)val).GetByteArray();
        }
        public NBTTagCompound GetCompoundTag(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.Compound)
                throw new InvalidOperationException();
            return (NBTTagCompound)val;
        }
        public double GetDouble(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.Double)
                throw new InvalidOperationException();
            return ((NBTTagDouble)val).GetDouble();
        }
        public float GetFloat(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.Float)
                throw new InvalidOperationException();
            return ((NBTTagFloat)val).GetFloat();
        }
        public int[] GetIntArray(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.IntArray)
                throw new InvalidOperationException();
            return ((NBTTagIntArray)val).GetIntArray();
        }
        public int GetInteger(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.Int)
                throw new InvalidOperationException();
            return ((NBTTagInt)val).GetInt();
        }
        public List<string> GetKeySet()
        {
            return _payload.Keys.ToList();
        }
        public long GetLong(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.Long)
                throw new InvalidOperationException();
            return ((NBTTagLong)val).GetLong();
        }
        public long[] GetLongArray(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.LongArray)
                throw new InvalidOperationException();
            return ((NBTTagLongArray)val).GetLongArray();
        }
        public short GetShort(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.Short)
                throw new InvalidOperationException();
            return ((NBTTagShort)val).GetShort();
        }
        public int GetSize() => _payload.Count;
        public string GetString(string key)
        {
            var val = GetTag(key);
            if (val.Id != TagType.String)
                throw new InvalidOperationException();
            return ((NBTTagString)val).GetString();
        }
        public NBTBase GetTag(string key) => _payload[key];
        public TagType GetTagId(string key) => GetTag(key).Id;
        public NBTTagList GetTagList(string key, TagType type)
        {
            var val = GetTag(key);
            if (val.Id != TagType.List || ((NBTTagList)val).GetTagType() != type)
                throw new InvalidOperationException();
            return (NBTTagList)val;
        }
        public UUID GetUniqueId(string key)
        {
            return null;
        }
        public bool HasKey(string key) => _payload.ContainsKey(key);
        public bool HasKey(string key, TagType type) => HasKey(key) && GetTagId(key) == type;
        public bool HasUniqueId(string key)
        {
            return false;
        }
        public void Merge(NBTTagCompound other)
        {
            if (other != null)
                this._payload.Concat(other._payload);
        }
        public void RemoveTag(string key) => _payload.Remove(key);
        public void SetBoolean(string key, bool value) => SetByte(key, (sbyte)(value ? 1 : 0));
        public void SetByte(string key, sbyte value) => SetTag(key, new NBTTagByte(key, value));
        public void SetByteArray(string key, sbyte[] value) => SetTag(key, new NBTTagByteArray(key, value));
        public void SetDouble(string key, double value) => SetTag(key, new NBTTagDouble(key, value));
        public void SetFloat(string key, float value) => SetTag(key, new NBTTagFloat(key, value));
        public void SetIntArray(string key, int[] value) => SetTag(key, new NBTTagIntArray(key, value));
        public void SetInteger(string key, int value) => SetTag(key, new NBTTagInt(key, value));
        public void SetLong(string key, long value) => SetTag(key, new NBTTagLong(key, value));
        public void SetLongArray(string key, long[] value) => SetTag(key, new NBTTagLongArray(key, value));
        public void SetShort(string key, short value) => SetTag(key, new NBTTagShort(key, value));
        public void SetString(string key, string value) => SetTag(key, new NBTTagString(value));
        public void SetTag(string key, NBTBase value) => _payload.Add(key, value);
        public void SetUniqueId(string key, UUID value)
        {

        }
    }
}
