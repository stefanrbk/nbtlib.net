using System;
using System.Collections.Generic;
using System.Text;

namespace NbtLib
{
    public class NBTTagByte : NbtValue.NbtPrimitive
    {
        private sbyte _payload;
        public override sbyte GetByte() => _payload;
        public NBTTagByte(string name, sbyte value) : base(TagType.Byte, name) => this._payload = value;
    }
}
