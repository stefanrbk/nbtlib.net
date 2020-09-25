using System.IO;

namespace NbtLib
{
    public class ShortTag : AbstractNumberTag
    {
        public static readonly ITagReader READER = new ShortType();
        private class ShortType : ITagReader
        {
            public ITag Read(BinaryReader reader, int _) => 
                Of(reader.ReadByte());

            public bool IsImmutable =>
                true;
            public string CrashReportName =>
                "SHORT";

            public string CommandFeedbackName =>
                "TAG_Short";

        }

        private readonly short value;

        private ShortTag(short value) =>
            this.value = value;

        public static ShortTag Of(short value) =>
            value is >= -128 and <= 1024 ? Cache.VALUE[128 + value] : new ShortTag(value);
        public override void Write(BinaryWriter writer) =>
            writer.Write(value);

        public override byte Type => 2;
        public override ITagReader Reader => READER;


        public override string ToString() =>
            $"{value}s";

        public override ShortTag Copy() =>
            this;

        public override bool Equals(object? obj) =>
            this == obj || obj is ShortTag bt && value == bt.value;

        public override int GetHashCode() => value;
        public override byte GetByte() => (byte)value;
        public override double GetDouble() => value;
        public override float GetFloat() => value;
        public override int GetInt() => value;
        public override long GetLong() => value;
        public override short GetShort() => value;

        private static class Cache
        {
            public static readonly ShortTag[] VALUE = new ShortTag[256];

            static Cache()
            {
                for (var i = 0; i < VALUE.Length; ++i)
                    VALUE[i] = new ShortTag((byte)(i - 128));
            }
        }
    }
}
