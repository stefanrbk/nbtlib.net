using System;
using System.Collections.Generic;
using System.Text;

namespace nbtlib
{
    public class NBTTagString : NBTBase
    {
        private readonly string _payload;

        public NBTTagString(string name) : base(TagType.String, name) => _payload = "";
        public NBTTagString(string name, string data) : this(name) => _payload = data;
        public string GetString() => _payload;
    }
}
