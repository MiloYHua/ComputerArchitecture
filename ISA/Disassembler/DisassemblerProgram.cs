using System.Text.RegularExpressions;
using System.Text;
using ISALibrary;
using System.Runtime.Serialization;

namespace Disassembler
{
    public class DisassemblerProgram
    {
        static void Main(string[] args)
        {
            byte[] machineCode = File.ReadAllBytes(args[0]);

            if ((machineCode.Length >> 2) << 2 != machineCode.Length) throw new ArgumentException("Incorrect Machine Code Length");

            ReadOnlySpan<byte> machineCodeSpan = new(machineCode);

            Dictionary<ushort, string> labelPairs = new();

            for (ushort i = 0; i < machineCode.Length; i += 4)
            {
                ReadOnlySpan<byte> current = machineCodeSpan.Slice(i, 4);
                string bob = current[0].ToString();
                    string billy = ((int)OpCode.JMP).ToString();
                Match match = Regex.Match(current[0].ToString(), ((int)OpCode.JMP).ToString());

                if (match.Success) labelPairs.Add(Register.SetFromHBLB(current[1], current[2]), $"label_pos_{Register.SetFromHBLB(current[1], current[2])}");
            }

            StringBuilder sb = new StringBuilder();

            for (ushort i = 0; i < machineCode.Length; i += 4)
            {
                ReadOnlySpan<byte> current = machineCodeSpan.Slice(0, 4);

                if (labelPairs.TryGetValue((ushort)(i/4+1), out string? label))
                {
                    sb.AppendLine($"{label}:");
                    continue;
                }

                OpCode opCode = (OpCode)current[0];
                string? name = Enum.GetName(opCode);

                if (name is null) throw new Exception($"This is weird!... Why don't we have opcode {opCode}");

                Instruction instruction = Instruction.NameToInstruction[name]();

                instruction.DeCode(current);

                sb.AppendLine(instruction.Disassemble(current.ToArray()));

                machineCodeSpan = machineCodeSpan[4..];
            }

            File.WriteAllText(args[1], sb.ToString());
        }
    }
}
