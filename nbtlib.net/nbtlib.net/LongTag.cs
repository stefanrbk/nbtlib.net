using System.IO;

namespace NbtLib
{
    public class LongTag : AbstractNumberTag
    {
        public static readonly ITagReader READER = new LongType();
        private class LongType : ITagReader
        {
            public ITag Read(BinaryReader reader, int _) => 
                Of(reader.ReadInt64());

            public bool IsImmutable =>
                true;
            public string CrashReportName =>
                "LONG";

            public string CommandFeedbackName =>
                "TAG_Long";
        }

        private readonly long value;

        private LongTag(long value) =>
            this.value = value;

        public static LongTag Of(long value) =>
            value is >= -128L and <= 1024L ? Cache.VALUES[value + 128] : new LongTag(value);
        public override void Write(BinaryWriter writer) =>
            writer.Write(value);

        public override byte Type => 4;
        public override ITagReader Reader => READER;

        public override string ToString() =>
            $"{value}L";

        public override LongTag Copy() =>
            this;

        public override bool Equals(object? obj) =>
            this == obj || obj is LongTag lt && value == lt.value;

        public override int GetHashCode() => (int)(value ^ value >> 32);
        public override long GetLong() => value;
        public override int GetInt() => (int)value;
        public override short GetShort() => (short)value;
        public override byte GetByte() => (byte)value;
        public override double GetDouble() => value;
        public override float GetFloat() => value;

        private static class Cache
        {
            public static readonly LongTag[] VALUES = new LongTag[1153];

            static Cache()
            {
                for (var i = 0; i < VALUES.Length; ++i)
                    VALUES[i] = new LongTag(-128 + i);
            }
        }
    }
}
