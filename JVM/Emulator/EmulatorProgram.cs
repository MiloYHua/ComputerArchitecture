using JVMLibrary.Instructions;
using JVMLibrary.Information;
using JVMLibrary.Attributes;
using JVMLibrary.Utility;
using JVMLibrary;

namespace Emulator
{
	public class EmulatorProgram
	{
		static void Main(string[] args)
		{
			byte[] bytecode = File.ReadAllBytes(args[1]); //args[0] for GMR (need to replace path), args[1] for home
			ClassFile classFile = new ClassFile();

			classFile.Parse(bytecode);

			MethodInfo mainMethod = Identify.FindMainMethod(classFile);
			CodeAttributeInfo codeInfo = Identify.IdentifyCodeInfo(classFile, mainMethod);

			Stack<byte> operands = new Stack<byte>(codeInfo.MaxStack);
			long[] variables = new long[codeInfo.MaxLocals];
			ReadOnlySpan<byte> code = codeInfo.Code;

			while (true) //temp
			{
				Instruction current = Instruction.InstructionFactory(code[0]); //Fetch, decode, execute loop.

				current.Decode(codeInfo.Code);

				current.Execute(operands, variables);

				code.Shorten(current.Length);
			}
		}
	}
}