using System;
using System.Collections.Generic;
using System.Text;

namespace NbtLib
{
    public class NBTTagDouble : NbtValue.NbtPrimitive
    {
        private readonly double _payload;
        public override double GetDouble() => _payload;
        public NBTTagDouble(string name, double value) : base(TagType.Double, name) => _payload = value;
    }
}
