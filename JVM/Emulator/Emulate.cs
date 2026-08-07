using System.Security.Cryptography;
using JVMLibrary.JVMExceptions;
using JVMLibrary.Information;
using JVMLibrary.Attributes;
using System.Text;
using JVMLibrary;
using JVMLibrary.Instructions;
//look through methods
//in order for speed:
//access flags
//name to be main
//utf8 at index descriptor index correct signiture
namespace Emulator
{
    internal class Emulate
    {
        static bool IdentitfyMain(ClassFile classFile, MethodInfo method)
        {
            if (!method.AccessFlags.HasFlag(AccessFlags.ACC_PUBLIC))
                return false;

            if (!method.AccessFlags.HasFlag(AccessFlags.ACC_STATIC))
                return false;

            if (classFile.ConstantPool[method.NameIndex] is not ConstUtf8Info nameIndexUtf8)
                throw new InvalidMethodException($"Invalid Method NameIndex, expected {ConstantPoolTag.Utf8}, got '{method.NameIndex}' instead.");

            if (Encoding.UTF8.GetString(nameIndexUtf8.Bytes) != "main")
                return false;

            if (classFile.ConstantPool[method.DescriptorIndex] is not ConstUtf8Info descriptorIndexUtf8)
                throw new InvalidOperationException($"Invalid Method DescriptorIndex, expected {ConstantPoolTag.Utf8}, got '{method.DescriptorIndex}' instead.");

            if (Encoding.UTF8.GetString(descriptorIndexUtf8.Bytes) != "([Ljava/lang/String;)V")
                return false;

            return true;
        }

        static void Main(string[] args)
        {
            byte[] bytecode = File.ReadAllBytes(args[0]);
            ClassFile classFile = new ClassFile();

            classFile.Parse(bytecode);


            MethodInfo mainMethod = new MethodInfo();
            bool mainMethodFound = false;

            foreach (MethodInfo method in classFile.Methods)
            {
                if (IdentitfyMain(classFile, method))
                {
                    if(mainMethodFound) throw new MultipleMainMethodsException($"Multiple main methods found, '{method}' and '{mainMethod}'.");
                    mainMethodFound = true;
                    mainMethod = method;
                }
            }

            if (!mainMethodFound)
                throw new MissingMainMethodException();


            CodeAttributeInfo codeInfo = new CodeAttributeInfo();

            foreach (AttributeInfo info in mainMethod.Attributes)
            {
                if (info is not CodeAttributeInfo tempCodeInfo) continue;

                codeInfo = tempCodeInfo;
            }


            Stack<byte> operands = new Stack<byte>(codeInfo.MaxStack);
            long[] variables = new long[codeInfo.MaxLocals];

            while (true) //temp
            {
                Instruction current = Instruction.InstructionFactory(codeInfo.Code[0]); //fetch

                current.Decode(codeInfo.Code); //decode
                current.Execute(operands, variables); //execute
            }
        }
    }
}
