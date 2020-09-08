using System;
using System.Collections.Generic;
using System.Text;

namespace NbtLib
{
    public class NBTTagEnd : NbtValue
    {
        public NBTTagEnd() : base(TagType.End, "_") { }
    }
}
