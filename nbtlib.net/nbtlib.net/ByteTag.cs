using System.IO;

namespace NbtLib
{
    public class ByteTag : AbstractNumberTag
    {
        public static readonly ITagReader READER = new ByteType();
        private class ByteType : ITagReader
        {
            public bool IsImmutable =>
                true;
            public string CrashReportName =>
                "BYTE";

            public string CommandFeedbackName =>
                "TAG_Byte";

            public ITag Read(BinaryReader reader, int depth) =>
                Of(reader.ReadByte());
        }

        public static readonly ByteTag ZERO = Of(0);
        public static readonly ByteTag ONE = Of(1);
        private readonly byte value;

        private ByteTag(byte value) =>
            this.value = value;

        public static ByteTag Of(byte value) =>
            Cache.VALUES[128 + value];

        public static ByteTag Of(bool value) =>
            value ? ONE : ZERO;
        public override void Write(BinaryWriter writer) =>
            writer.Write(value);

        public override byte Type => 1;
        public override ITagReader Reader => READER;

        public override string ToString() =>
            $"{value}b";

        public override ByteTag Copy() =>
            this;

        public override bool Equals(object? obj) =>
            this == obj || obj is ByteTag bt && value == bt.value;

        public override int GetHashCode() => value;
        public override long GetLong() => value;
        public override int GetInt() => value;
        public override short GetShort() => value;
        public override byte GetByte() => value;
        public override double GetDouble() => value;
        public override float GetFloat() => value;

        private static class Cache
        {
            public static readonly ByteTag[] VALUES = new ByteTag[256];

            static Cache()
            {
                for (var i = 0; i < VALUES.Length; ++i)
                    VALUES[i] = new ByteTag((byte)(i - 128));
            }
        }
    }
}
