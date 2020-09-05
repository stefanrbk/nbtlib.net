using System;
using System.Collections.Generic;
using System.Text;

namespace nbtlib
{
    public class NBTTagByte : NBTBase.NBTPrimitive
    {
        private sbyte _payload;
        public override sbyte GetByte() => _payload;
        public NBTTagByte(string name, sbyte value) : base(TagType.Byte, name) => this._payload = value;
    }
}
