using System.Reflection;

namespace NearestPow2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int stinkyPooPoo = 67;
            int count = 0;
            int isNotPowerOf2 = 0;
            while (stinkyPooPoo > 1)
            {
                isNotPowerOf2 = (stinkyPooPoo & 0b1) | isNotPowerOf2;
                count++;
                stinkyPooPoo >>= 1;
            }
            int nearestPow2 = 1 << (count+ isNotPowerOf2);
        }
    }
}