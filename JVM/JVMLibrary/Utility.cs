using System.Security.Cryptography;

namespace JVMLibrary
{
    public struct Utility
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
    }
}