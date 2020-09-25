using System.IO;
using System.Text;

namespace NbtLib
{
    public static class NBTUtil
    {
        public static string ReadUTF(this BinaryReader reader)
        {
            var length = reader.ReadUInt16();
            var bytes = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }
        public static void WriteUTF(this BinaryWriter writer, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            writer.Write((ushort)bytes.Length);
            writer.Write(bytes);
        }
    }
}
