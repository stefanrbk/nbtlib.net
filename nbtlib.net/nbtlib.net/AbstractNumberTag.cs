using System.IO;

namespace NbtLib
{
    public abstract class AbstractNumberTag : ITag
    {
        protected AbstractNumberTag() { }
        public abstract long GetLong();
        public abstract int GetInt();
        public abstract short GetShort();
        public abstract byte GetByte();
        public abstract double GetDouble();
        public abstract float GetFloat();

        public abstract byte Type { get; }
        public abstract ITagReader Reader { get; }

        public abstract ITag Copy();
        public abstract void Write(BinaryWriter writer);
    }
}
