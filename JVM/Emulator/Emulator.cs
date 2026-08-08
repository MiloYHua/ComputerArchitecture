using JVMLibrary.Instructions;
using JVMLibrary.Information;
using JVMLibrary.Attributes;
using JVMLibrary.Utility;
using JVMLibrary;

namespace Emulator
{
    public class Emulator
    {
        public void RunMain(ClassFile classFile, MethodInfo mainMethod)
        {

            CodeAttributeInfo codeInfo = Identify.IdentifyCodeInfo(classFile, mainMethod);

            Stack<object> operands = new Stack<object>(codeInfo.MaxStack);
            long[] variables = new long[codeInfo.MaxLocals];
            ReadOnlySpan<byte> code = codeInfo.Code;

            while (code.Length > 0) //temp
            {
                Instruction current = Instruction.InstructionFactory(code[0]); //Fetch, decode, execute loop.

                current.Decode(code);

                current.Execute(operands, variables);

                code.Shorten(current.Length);
            }
        }
    }
}
