using JVMLibrary.JVMExceptions;

namespace JVMLibrary.Instructions
{
    internal class JVM_istore_1 : Instruction
    {
        public override byte OpCode { get; } = 0x3c;
        public override byte Length { get; } = 1;

        public override void Decode(ReadOnlySpan<byte> bytecode)
        {
            //No variables to set
        }

        public override void Execute(Stack<byte> operands, long[] variables)
        {
            if (operands.TryPop(out byte value)) throw new StackUnderflowException<byte>(operands);

                variables[1] = value;
        }
    }
}