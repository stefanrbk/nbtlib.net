using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace nbtlib.net
{
    public abstract class NBTBase
    {
        public TagType Id;
        public string Name;
        public NBTBase(TagType type, string name)
        {
            this.Id = type;
            this.Name = name;
        }
        private NBTBase() { }
        public abstract class NBTPrimitive : NBTBase
        {
            public NBTPrimitive(TagType type, string name) : base(type, name) { }
            private NBTPrimitive() { }
            public virtual sbyte GetByte() => throw new InvalidOperationException();
            public virtual double GetDouble() => throw new InvalidOperationException();
            public virtual float GetFloat() => throw new InvalidOperationException();
            public virtual int GetInt() => throw new InvalidOperationException();
            public virtual long GetLong() => throw new InvalidOperationException();
            public virtual short GetShort() => throw new InvalidOperationException();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "This enum is stored as a byte in the file format.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This enum is of data types.")]
    public enum TagType : sbyte
    {
        End,
        Byte,
        Short,
        Int,
        Long,
        Float,
        Double,
        ByteArray,
        String,
        List,
        Compound,
        IntArray,
        LongArray
    }
}
