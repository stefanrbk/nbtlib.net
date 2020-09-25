using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbtLib
{
    public class TagReaders
    {
        private static readonly ITagReader[] types = new ITagReader[]
        {
            EndTag.READER,
            ByteTag.READER,
            ShortTag.READER,
            IntTag.READER,
            LongTag.READER,
            FloatTag.READER,
            DoubleTag.READER,
            ByteArrayTag.READER,
            StringTag.READER,
            ListTag.READER,
            CompoundTag.READER,
            IntArrayTag.READER,
            LongArrayTag.READER
        };

        public static ITagReader Of(int type) =>
            type >= 0 && type < types.Length ? types[type] : ITagReader.CreateInvalid(type);
    }
}
