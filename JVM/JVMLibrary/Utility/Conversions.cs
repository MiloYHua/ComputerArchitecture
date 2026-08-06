namespace JVMLibrary.Utility
{
    public struct Conversions
    {
        public static byte[] ToBytes(byte value)
        {
            return [value];
        }
        public static byte[] ToBytes(ushort value)
        {
            return [(byte)(value >> 8), (byte)(value << 8 >> 8)];
        }

        public static byte[] ToBytes(uint value)
        {
            return [(byte)(value >> 24), (byte)(value << 8 >> 24), (byte)(value << 16 >> 24), (byte)(value << 24 >> 24)];
        }

        public static ushort ToUShort(byte[] bytes)
        {
            if (bytes.Length != 2) throw new ArgumentException("Byte array must be exactly 2 bytes long to convert to ushort.");
            return (ushort)((bytes[0] << 8) | bytes[1]);
        }

        public static ushort ToUShort(ReadOnlySpan<byte> bytes) => ToUShort(bytes.ToArray());
    }
}