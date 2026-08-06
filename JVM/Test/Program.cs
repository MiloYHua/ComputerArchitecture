using System.Text;
using JVMLibrary;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Encoding.UTF8.GetString([0, 6]));
        }
    }
}