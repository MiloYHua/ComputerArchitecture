namespace DoMath
{
    internal class Program
    {
        static byte DoMath(uint num)
        {
            uint val = num & 0b1111;
            num >>= 4;
            uint nums = num & 0b11111111;
            num >>= 8;
            uint lResult = num & nums;
            uint rResult = (num & (nums * 2)) - lResult;

            switch (val)
            {
                case 0b1:
                    return (byte)(lResult + rResult);

                case 0b10:
                    return (byte)(lResult - rResult);

                case 0b100:
                    return (byte)(lResult * rResult);

                case 0b1000:
                    return (byte)(lResult / rResult);
            }
            return 0;
        }

        byte GetByte(uint num, int index)
        {
            return (byte)((num >> (index - 1)) & 0b1);
        }

        static void Main(string[] args)
        {
            int jimbob = byte.MaxValue;
            int jimmy = DoMath(0b1111_1111_1111_1111_1111_1111_1000);
        }
    }
}
