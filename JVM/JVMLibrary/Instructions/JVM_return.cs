
using System;
using System.Collections.Generic;
using System.Text;

namespace JVMLibrary.Instructions
{
	public class JVM_return : Instruction
	{
		public override byte OpCode => 0xb1;

		public override byte Length => 1;

		public override void Decode(ReadOnlySpan<byte> bytecode)
		{
			//No variables to set
		}

		public override void Execute(Stack<object> operands, long[] variables)
		{
			throw new NotImplementedException();
		}
	}
}
