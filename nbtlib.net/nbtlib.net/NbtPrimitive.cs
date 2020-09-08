using NbtLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbtLib
{
    public class NbtPrimitive : NbtValue
    {
        public NbtPrimitive(byte value) =>
            Value = value;
        public NbtPrimitive(byte[] value) =>
            Value = value;
        public NbtPrimitive(double value) =>
            Value = value;
        public NbtPrimitive(float value) =>
            Value = value;
        public NbtPrimitive(int value) =>
            Value = value;
        public NbtPrimitive(int[] value) =>
            Value = value;
        public NbtPrimitive(long value) =>
            Value = value;
        public NbtPrimitive(long[] value) =>
            Value = value;
        public NbtPrimitive(short value) =>
            Value = value;
        public NbtPrimitive(string value) =>
            Value = value;

        internal object Value { get; }

        public override NbtType NbtType =>
            Value is null || Value is string ? NbtType.String : NbtType.Number;

        public override void Save(BinaryWriter writer)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            switch (NbtType)
            {
                case NbtType.String:
                    writer.Write((short)((string)Value).Length);
                    writer.Write((string)Value);
                    break;
            }
        }
    }
}
