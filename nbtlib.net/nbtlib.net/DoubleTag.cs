using System;
using System.IO;

namespace NbtLib
{
    public class DoubleTag : AbstractNumberTag
    {
        public static readonly DoubleTag ZERO = new DoubleTag(0.0);
        public static readonly ITagReader READER = new DoubleType();
        private class DoubleType : ITagReader
        {
            public ITag Read(BinaryReader reader, int _) => 
                Of(reader.ReadDouble());

            public string CrashReportName =>
                "DOUBLE";

            public string CommandFeedbackName =>
                "TAG_Double";

            public bool IsImmutable =>
                true;
        }

        private readonly double value;

        private DoubleTag(double value) =>
            this.value = value;

        public static DoubleTag Of(double value) =>
            value == 0.0 ? ZERO : new DoubleTag(value);

        public override void Write(BinaryWriter writer) =>
            writer.Write(value);

        public override byte Type => 6;
        public override ITagReader Reader => READER;
        public override string ToString() =>
            $"{value}d";

        public override DoubleTag Copy() =>
            this;
        public override bool Equals(object? obj) =>
            this == obj || obj is DoubleTag dt && value == dt.value;
        public override int GetHashCode()
        {
            var i = BitConverter.DoubleToInt64Bits(value);
            return (int)(i ^ i >> 32);
        }

        public override long GetLong() => (long)Math.Floor(value);
        public override int GetInt() => (int)Math.Floor(value);
        public override short GetShort() => (short)Math.Floor(value);
        public override byte GetByte() => (byte)Math.Floor(value);
        public override double GetDouble() => value;
        public override float GetFloat() => (float)value;
    }
}
