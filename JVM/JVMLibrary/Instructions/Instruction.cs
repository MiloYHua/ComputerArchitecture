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
            [0x60] = () => new JVM_iadd(),
            [0x3c] = () => new JVM_istore_1(),
            [0x3d] = () => new JVM_istore_2(),
            [0x3e] = () => new JVM_istore_3(),
            [0x1c] = () => new JVM_iload_2(),
            [0x1b] = () => new JVM_iload_1(),
            [0xb1] = () => new JVM_return(),
        };

        public static Instruction InstructionFactory(byte opcode) 
        {
            return opCodeInstructionMap[opcode]();
        }

        public abstract byte OpCode { get; }
        public abstract byte Length { get; }

        public abstract void Decode(ReadOnlySpan<byte> bytecode);

        public abstract void Execute(Stack<object> operands, long[] variables);
    }
}
