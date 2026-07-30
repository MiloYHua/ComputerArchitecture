using System.Text;
using ISALibrary;

namespace Disassembler
{
    public class DisassemblerProgram
    {
        static void Main(string[] args)
        {
            byte[] machineCode = File.ReadAllBytes(args[0]);

            if ((machineCode.Length >> 2) << 2 != machineCode.Length) throw new ArgumentException("Incorrect Machine Code Length");

            ReadOnlySpan<byte> machineCodeSpan = new(machineCode);

            StringBuilder sb = new StringBuilder();

            while (machineCodeSpan.Length > 0)
            {
                ReadOnlySpan<byte> current = machineCodeSpan.Slice(0, 4);

                OpCode opCode = (OpCode)current[0];
                string? name = Enum.GetName(opCode);

                if (name is null) throw new Exception($"This is weird!... Why don't we have opcode {opCode}");

                Instruction instruction = Instruction.NameToInstruction[name]();

                sb.AppendLine(instruction.Disassemble(current.ToArray()));

                machineCodeSpan = machineCodeSpan[4..];
            }

            File.WriteAllText(args[1], sb.ToString());
        }
    }
}
