using JVMLibrary.Utility;

namespace JVMLibrary.Instructions
{
    public class JVM_bipush : Instruction
    {
        public override byte OpCode { get; } = 0x10;
        public override byte Length { get; } = 2;

        public byte Value { get; set; }

        public override void Decode(ReadOnlySpan<byte> bytecode)
        {
            Value = bytecode.CutU1();
        }

        public override void Execute(Stack<byte> operands, long[] variables)
        {
            operands.Push(Value);
        }
    }
}