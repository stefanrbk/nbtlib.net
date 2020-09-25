using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NbtLib
{
    public class StringTag : ITag
    {
        private int? hash;
        public static readonly ITagReader READER = new StringType();
        private class StringType : ITagReader
        {
            public ITag Read(BinaryReader reader, int _) => 
                Of(reader.ReadUTF());
            public string CrashReportName =>
                "STRING";
            public string CommandFeedbackName =>
                "TAG_String";
            public bool IsImmutable =>
                true;
        }

        private static readonly StringTag EMPTY = new StringTag("");
        private readonly string value;

        private StringTag(string value) =>
            this.value = value;

        public static StringTag Of(string value) =>
            value == "" ? EMPTY : new StringTag(value);

        public void Write(BinaryWriter writer) =>
            writer.WriteUTF(value);

        public byte Type => 8;
        public ITagReader Reader => READER;

        public override string ToString() =>
            Escape(value);

        public StringTag Copy() =>
            this;

        public override bool Equals(object? obj) =>
            this == obj || obj is StringTag st && value.SequenceEqual(st.value);

        public override int GetHashCode()
        {
            if (hash is null)
            {
                hash = 0;
                var i = 0;
                var n = value.Length;
                foreach (var s in value)
                    hash += s * 31 ^ (n - ++i);
            }
            return hash ?? 0;
        }

        public string AsString() =>
            value;

        public static string Escape(string s)
        {
            var sb = new StringBuilder(" ");
            char c0 = default;

            for (var i = 0; i < s.Length; i++)
            {
                var c1 = s[i];
                if (c1 == '\\')
                    sb.Append('\\');
                else if (c1 == '"' || c1 == '\'')
                {
                    if (c0 == default)
                        c0 = (char)(c1 == '"' ? 39 : 34);
                    if (c0 == c1)
                        sb.Append('\\');
                }

                sb.Append(c1);
            }

            if (c0 == default)
                c0 = '"';

            sb[0] = c0;
            sb.Append(c0);
            return sb.ToString();
        }

        ITag ITag.Copy() =>
            Copy();

    }
}
