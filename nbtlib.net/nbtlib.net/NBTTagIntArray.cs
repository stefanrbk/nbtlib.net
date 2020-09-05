using System;
using System.Collections.Generic;
using System.Text;

namespace nbtlib
{
    public class NBTTagIntArray : NBTBase
    {
        private readonly int[] _payload;

        public int[] GetIntArray() => _payload;
        public NBTTagIntArray(string name, int[] value) : base(TagType.IntArray, name) => _payload = value;
    }
}
