using JVMLibrary.JVMExceptions;

namespace JVMLibrary.Instructions
{
    internal class JVM_istore_1 : Instruction
    {
        public override byte OpCode => 0x3c;
        public override byte Length => 1;

        public override void Decode(ReadOnlySpan<byte> bytecode)
        {
            //No variables to set
        }

        public override void Execute(Stack<object> operands, long[] variables)
        {
            if (!operands.TryPop(out object? value)) throw new StackUnderflowException<object>(operands);

            variables[1] = Convert.ToInt64(value);
        }
    }
}