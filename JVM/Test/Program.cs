using JVMLibrary;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
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