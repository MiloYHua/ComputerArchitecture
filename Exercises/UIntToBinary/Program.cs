using System.Text;

namespace UIntToBinary
{
    internal class Program
    {
        static void Main(string[] args)
        {
            uint num = 696761420;
            uint mask = 0b1000_0000_0000_0000_0000_0000_0000_0000;

            StringBuilder result = new();

            for(int i = 31; i >= 0; i--)
            {
                uint something = num & mask;
                something >>= i;
                result.Append(something);

                mask >>= 1;
            }

            for (int i = 0; i < result.Length; i++)
            {
                Console.Write(result[i]);

                if ((i + 1 & 0b11) == 0)
                {
                    Console.Write(" ");
                }
            }
        }
    }
}