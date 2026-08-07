using JVMLibrary.JVMExceptions;

namespace JVMLibrary.Instructions
{
	public class JVM_iadd : Instruction
	{
		public override byte OpCode => 0x60;

		public override byte Length => 1;

		public override void Decode(ReadOnlySpan<byte> bytecode)
		{
			//Nothing to decode
		}

		public override void Execute(Stack<object> operands, long[] variables)
		{
			if (!operands.TryPop(out object? val1)) throw new StackUnderflowException<object>(operands);
			if (!operands.TryPop(out object? val2)) throw new StackUnderflowException<object>(operands);

			int a = Convert.ToInt32(val1);
			int b = Convert.ToInt32(val2);

			operands.Push(a + b);
		}
	}
}