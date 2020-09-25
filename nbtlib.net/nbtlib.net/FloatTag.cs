using System;
using System.IO;

namespace NbtLib
{
    public class FloatTag : AbstractNumberTag
    {
        public static readonly FloatTag ZERO = new FloatTag(0.0f);
        public static readonly ITagReader READER = new FloatType();
        private class FloatType : ITagReader
        {
            public ITag Read(BinaryReader reader, int _) =>
                Of(reader.ReadSingle());

            public string CrashReportName =>
                "FLOAT";

            public string CommandFeedbackName =>
                "TAG_Float";

            public bool IsImmutable =>
                true;
        }

        private readonly float value;

        private FloatTag(float value) =>
            this.value = value;

        public static FloatTag Of(float value) =>
            value == 0.0f ? ZERO : new FloatTag(value);

        public override void Write(BinaryWriter writer) =>
            writer.Write(value);

        public override byte Type => 5;
        public override ITagReader Reader => READER;
        public override string ToString() =>
            $"{value}f";

        public override FloatTag Copy() =>
            this;
        public override bool Equals(object? obj) =>
            this == obj || obj is FloatTag ft && value == ft.value;
        public override int GetHashCode() =>
            BitConverter.SingleToInt32Bits(value);

        public override long GetLong() => (long)MathF.Floor(value);
        public override int GetInt() => (int)MathF.Floor(value);
        public override short GetShort() => (short)MathF.Floor(value);
        public override byte GetByte() => (byte)MathF.Floor(value);
        public override double GetDouble() => value;
        public override float GetFloat() => value;
    }
}
