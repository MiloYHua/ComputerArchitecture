using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using JVMLibrary.Utility;

namespace JVMLibrary.Instructions
{
    public abstract class Instruction
    {
        private static Dictionary<byte, Func<Instruction>> opCodeInstructionMap = new()
        {
            [0x10] = () => new JVM_bipush(),
            [0x3c] = () => new JVM_istore_1(),
        };

        public static Instruction InstructionFactory(byte opcode) 
        {
            return opCodeInstructionMap[opcode]();
        }

        public abstract byte OpCode { get; }
        public abstract byte Length { get; }

        public abstract void Decode(ReadOnlySpan<byte> bytecode);

        public abstract void Execute(Stack<byte> operands, long[] variables);
    }
}
