using System;
using System.IO;

namespace NbtLib
{
    public interface ITagReader
    {
        ITag Read(BinaryReader reader, int depth);
        bool IsImmutable =>
            false;

        string CrashReportName { get; }

        string CommandFeedbackName { get; }
        static ITagReader CreateInvalid(int type) =>
            new Invalid() { Value = type };

        private class Invalid : ITagReader
        {
            public int Value { get; init; }

            public string CrashReportName =>
                $"INVALID[{Value}]";
            public string CommandFeedbackName =>
                $"UNKNOWN_{Value}";

            public ITag Read(BinaryReader reader, int depth) => 
                throw new ArgumentException($"Invalid tad id: {Value}");
        }
    
    }
}
