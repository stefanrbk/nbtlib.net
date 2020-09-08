using System;
using System.Collections.Generic;
using System.Text;

namespace NbtLib
{
    public class NBTTagLong : NbtValue.NbtPrimitive
    {
        private readonly long _payload;
        public override long GetLong() => _payload;
        public NBTTagLong(string name, long value) : base(TagType.Long, name) => _payload = value;
    }
}
