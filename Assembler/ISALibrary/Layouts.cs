using System.Text.RegularExpressions;
using System.Text;

namespace ISALibrary
{
    public class Register
    {
        public byte Index { get; set; }
        public ushort Value { get; set; }
    }
    public abstract class Instruction
    {
        public int Length = 4;
        public abstract byte OpCode { get; }
        public abstract string Name { get; }

        public abstract List<byte> Parse(string instruction);
        public abstract string Disassemble(byte[] machineCode);
        public abstract void Execute(Register[] registers);

        public static Dictionary<string, Func<Instruction>> NameToInstruction = new()
        {
            [nameof(ADD)] = () => { return new ADD(); },
        };
    }

    public abstract class MathLayout : Instruction
    {
        public byte DestinationRegisterIndex { get; set; }
        public byte SourceRegister1Index { get; set; }
        public byte SourceRegister2Index { get; set; }

        public override List<byte> Parse(string instruction)
        {
            Match match = Regex.Match(instruction, @"[rR](\d+) +[rR](\d+) +[rR](\d+)");           

            DestinationRegisterIndex = byte.Parse(match.Groups[1].Value);
            SourceRegister1Index = byte.Parse(match.Groups[2].Value);
            SourceRegister2Index = byte.Parse(match.Groups[3].Value);

            return [OpCode, DestinationRegisterIndex, SourceRegister1Index, SourceRegister2Index];
        }

        public override string Disassemble(byte[] machineCode)
        {
            StringBuilder toReturn = new();

            OpCode opCode = (OpCode)machineCode[0];

            string? name = Enum.GetName(opCode);

            if (name is null) throw new Exception($"This is weird!... Why don't we have opcode {opCode}");

            toReturn.Append(name);

            for (int i = 1; i < 4; i++)
            {
                toReturn.Append($" R{machineCode[i]}");
            }

            return toReturn.ToString();
        }


    }

    public class ADD : MathLayout
    {
        public override byte OpCode => 0x10;
        public override string Name { get; } = "ADD";
    }
}
