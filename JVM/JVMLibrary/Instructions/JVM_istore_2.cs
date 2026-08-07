using JVMLibrary.JVMExceptions;

namespace JVMLibrary.Instructions
{
	public class JVM_istore_2 : Instruction
	{
		public override byte OpCode => 0x3d;

		public override byte Length => 1;

		public override void Decode(ReadOnlySpan<byte> bytecode)
		{
			//No variables to set
		}

		public override void Execute(Stack<object> operands, long[] variables)
		{
			if (!operands.TryPop(out object? value)) throw new StackUnderflowException<object>(operands);

			variables[2] = Convert.ToInt64(value);
		}
	}
}