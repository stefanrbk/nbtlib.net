using System;
using System.Collections.Generic;
using System.Text;

namespace nbtlib
{
    public class NBTTagLong : NBTBase.NBTPrimitive
    {
        private readonly long _payload;
        public override long GetLong() => _payload;
        public NBTTagLong(string name, long value) : base(TagType.Long, name) => _payload = value;
    }
}
