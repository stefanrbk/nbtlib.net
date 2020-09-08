using System;
using System.Collections.Generic;
using System.Text;

namespace NbtLib
{
    public class NBTTagShort : NbtValue.NbtPrimitive
    {
        private readonly short _payload;
        public override short GetShort() => _payload;
        public NBTTagShort(string name, short value) : base(TagType.Short, name) => _payload = value;
    }
}
