using JVMLibrary.Instructions;
using JVMLibrary.Information;
using JVMLibrary.Attributes;
using JVMLibrary.Utility;
using JVMLibrary;
using System.Runtime.Serialization;

namespace Emulator
{
	public class EmulatorProgram
	{
		static void Main(string[] args)
		{
			byte[] bytecode = File.ReadAllBytes(args[0]); //args[0] for GMR (need to replace path), args[1] for home
			ClassFile classFile = new ClassFile();
			Emulator emulator = new Emulator();

			classFile.Parse(bytecode);

			MethodInfo mainMethod = Identify.IdentifyMain(classFile);

			emulator.RunMain(classFile, mainMethod);
		}
	}
}