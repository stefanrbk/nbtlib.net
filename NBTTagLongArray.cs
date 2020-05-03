using System;
using System.Collections.Generic;
using System.Text;

namespace nbtlib.net
{
    public class NBTTagLongArray : NBTBase
    {
        private readonly long[] _payload;

        public long[] GetLongArray() => _payload;
        public NBTTagLongArray(string name, long[] value) : base(TagType.LongArray, name) => _payload = value;
    }
}
