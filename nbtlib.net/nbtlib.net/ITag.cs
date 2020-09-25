using System.IO;

namespace NbtLib
{
    public interface ITag
    {
        void Write(BinaryWriter writer);
        string? ToString();
        byte Type { get; }

        ITagReader Reader { get; }
        ITag Copy();
        string AsString =>
            ToString() ?? "";
    }
}
