using System;
using System.Collections.Generic;
using System.Text;

namespace NbtLib
{
    public class NBTTagFloat : NbtValue.NbtPrimitive
    {
        private readonly float _payload;
        public override float GetFloat() => _payload;
        public NBTTagFloat(string name, float value) : base(TagType.Float, name) => _payload = value;
    }
}
