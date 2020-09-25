using System.IO;

namespace NbtLib
{
    public class IntTag : AbstractNumberTag
    {
        public static readonly ITagReader READER = new IntType();
        private class IntType : ITagReader
        {
            public ITag Read(BinaryReader reader, int _) =>
                Of(reader.ReadInt32());

            public string CrashReportName =>
                "INT";

            public string CommandFeedbackName =>
                "TAG_Int";
            public bool IsImmutable =>
                true;
        }

        private readonly int value;

        private IntTag(int value) =>
            this.value = value;

        public static IntTag Of(int value) =>
            value is >= -128 and <= 1024 ? Cache.VALUES[value + 128] : new IntTag(value);
        public override void Write(BinaryWriter writer) =>
            writer.Write(value);

        public override byte Type => 3;
        public override ITagReader Reader => READER;


        public override string ToString() =>
            $"{value}";

        public override IntTag Copy() =>
            this;

        public override bool Equals(object? obj) =>
            this == obj || obj is IntTag it && value == it.value;

        public override int GetHashCode() => value;
        public override long GetLong() => value;
        public override int GetInt() => value;
        public override short GetShort() => (short)value;
        public override byte GetByte() => (byte)value;
        public override double GetDouble() => value;
        public override float GetFloat() => value;

        private static class Cache
        {
            public static readonly IntTag[] VALUES = new IntTag[1153];

            static Cache()
            {
                for (var i = 0; i < VALUES.Length; ++i)
                    VALUES[i] = new IntTag(-128 + i);
            }
        }
    }
}
