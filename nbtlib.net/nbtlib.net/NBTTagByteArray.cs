using System;
using System.Collections.Generic;
using System.Text;

namespace nbtlib
{
    public class NBTTagByteArray : NBTBase
    {
        private sbyte[] _payload;
        public sbyte[] GetByteArray() => _payload;

        public NBTTagByteArray(string name, sbyte[] value) : base(TagType.ByteArray, name) => this._payload = value;
    }
}
