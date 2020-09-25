using System;
using Be.IO;
using System.IO.Compression;
using System.Xml;
using System.IO;

namespace NbtLib
{
    public static class NbtIO
    {
        public static CompoundTag ReadCompressed(FileInfo file)
        {
            try
            {
                using var inputStream = file.OpenRead();
                return ReadCompressed(inputStream);
            }
            catch
            {
                throw;
            }
        }
        public static CompoundTag ReadCompressed(Stream stream)
        {
            try
            {
                using var compressed = new GZipStream(stream, CompressionMode.Decompress);
                using var reader = new BeBinaryReader(compressed);

                return Read(reader);
            }
            catch
            {
                throw;
            }
        }
        public static void WriteCompressed(CompoundTag tag, FileInfo file)
        {
            try
            {
                using var outputStream = file.OpenWrite();
                WriteCompressed(tag, outputStream);
            }
            catch
            {
                throw;
            }
        }
        public static void WriteCompressed(CompoundTag tag, Stream stream)
        {
            try
            {
                using var compressor = new GZipStream(stream, CompressionMode.Compress);
                using var writer = new BeBinaryWriter(compressor);

                Write(tag, writer);
            }
            catch 
            {
                throw;
            }
        }
        public static void Write(CompoundTag tag, FileInfo file)
        {
            try
            {
                using var outputStream = file.OpenWrite();
                using var writer = new BeBinaryWriter(outputStream);

                Write(tag, writer);
            }
            catch 
            {
                throw;
            }
        }
        public static CompoundTag Read(FileInfo file)
        {
            try
            {
                using var inputStream = file.OpenRead();
                using var reader = new BinaryReader(inputStream);

                return Read(reader);
            }
            catch
            {
                throw;
            }
        }
        public static CompoundTag Read(BinaryReader reader)
        {
            var tag = Read(reader, 0);
            if (tag is CompoundTag ct)
                return ct;
            else
                throw new IOException("Root tag must be a named compound tag");
        }
        public static void Write(CompoundTag tag, BinaryWriter writer) =>
            Write((ITag)tag, writer);
        private static void Write(ITag tag, BinaryWriter writer)
        {
            writer.Write(tag.Type);
            if (tag.Type != 0)
            {
                writer.WriteUTF("");
                tag.Write(writer);
            }
        }
        private static ITag Read(BinaryReader reader, int depth)
        {
            var b = reader.ReadByte();
            if (b == 0)
                return EndTag.INSTANCE;
            else
            {
                reader.ReadUTF();

                try
                {
                    return TagReaders.Of(b).Read(reader, depth);
                }
                catch (IOException)
                {
                    throw;
                }
            }
        }
    }
}
