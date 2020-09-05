using System;
using System.Collections.Generic;
using System.Text;

namespace nbtlib
{
    public class NBTTagShort : NBTBase.NBTPrimitive
    {
        private readonly short _payload;
        public override short GetShort() => _payload;
        public NBTTagShort(string name, short value) : base(TagType.Short, name) => _payload = value;
    }
}
