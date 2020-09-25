using System.IO;

namespace NbtLib
{
    public class EndTag : ITag
    {
        public static readonly ITagReader READER = new EndType();
        private class EndType : ITagReader
        {
            public ITag Read(BinaryReader _1, int _2) => 
                INSTANCE;

            public string CrashReportName =>
                "END";

            public string CommandFeedbackName =>
                "TAG_End";

            public bool IsImmutable =>
                true;

        }

        public static readonly EndTag INSTANCE = new EndTag();

        private EndTag() { }

        public void Write(BinaryWriter writer) { }

        public byte Type => 0;
        public ITagReader Reader => READER;

        ITagReader ITag.Reader => Reader;

        public override string ToString() =>
            "END";

        public ITag Copy() =>
            this;

    }
}
