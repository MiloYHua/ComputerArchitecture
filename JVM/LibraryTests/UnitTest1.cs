using JVMLibrary;
using System.Text;

namespace LibraryTests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(2345, 1234546, 1234123, 12245)]
        [InlineData(345, 6345634, 56534, 2356654, 4365)]
        public void UShortToBytesTest(params int[] seeds)
        {
            foreach (int seed in seeds)
            {
                Random randy = new Random(seed);

                ushort expected = (ushort)randy.Next(ushort.MaxValue);

                byte[] result = Utility.ToBytes(expected);

                ushort actual = 0;

                for (int i = 0; i < result.Length; i++)
                    actual += (ushort)(result[i] << ((1 - i) * 8));

                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(2345, 1234546, 1234123, 12245)]
        [InlineData(345, 6345634, 56534, 2356654, 4365)]
        public void UIntToBytesTest(params int[] seeds)
        {
            foreach (int seed in seeds)
            {
                Random randy = new Random(seed);

                uint expected = (uint)randy.NextInt64(uint.MaxValue);

                byte[] result = Utility.ToBytes(expected);

                uint actual = 0;

                for (int i = 0; i < result.Length; i++)
                    actual += (uint)result[i] << ((3 - i) * 8);

                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(345, 6345634, 56534, 2356654, 4365)]
        public void UIntToBytesTest(params string[] args)
        {
            foreach (string s in args)
            {
                byte[] code = File.ReadAllBytes(args[0]);
                ClassFile classFile = new ClassFile();

                classFile.Parse(code);
                byte[] johnnyBytes = classFile.EmitBytes().ToArray();
                bool bobby = true;

                for (int i = 0; i < johnnyBytes.Length; i++)
                {
                    if (code[i] != johnnyBytes[i]) bobby = false;
                }
            }
        }
    }
}
