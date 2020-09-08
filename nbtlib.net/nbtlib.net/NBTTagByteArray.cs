using System;
using System.Collections.Generic;
using System.Text;

namespace NbtLib
{
    public class NBTTagByteArray : NbtValue
    {
        private sbyte[] _payload;
        public sbyte[] GetByteArray() => _payload;

        public NBTTagByteArray(string name, sbyte[] value) : base(TagType.ByteArray, name) => this._payload = value;
    }
}
