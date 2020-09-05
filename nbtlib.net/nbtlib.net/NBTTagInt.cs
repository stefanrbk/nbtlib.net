﻿using System;
using System.Collections.Generic;
using System.Text;

namespace nbtlib.net
{
    public class NBTTagInt : NBTBase.NBTPrimitive
    {
        private readonly int _payload;
        public override int GetInt() => _payload;
        public NBTTagInt(string name, int value) : base(TagType.Int, name) => _payload = value;
    }
}