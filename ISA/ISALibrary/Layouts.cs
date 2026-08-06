using System.Text.RegularExpressions;
using System.Text;

namespace ISALibrary
{
    public abstract class Instruction
    {
        public int Length = 4;
        public abstract byte OpCode { get; }
        public abstract string Name { get; }

        public abstract List<byte> Parse(InstructionMetadata metadata, string instruction);
        public abstract string Disassemble(byte[] machineCode);
        public abstract void Execute(EmulatorStatus status);
        public abstract void DeCode(ReadOnlySpan<byte> bytes);

        public static Dictionary<string, Func<Instruction>> NameToInstruction = new()
        {
            [nameof(ADD)] = () => { return new ADD(); },
            [nameof(JMP)] = () => { return new JMP(); },
            [nameof(SET)] = () => { return new SET(); },
        };
    }

    #region Math
    public abstract class MathInstruction : Instruction
    {
        public byte DestinationRegisterIndex { get; set; }
        public byte SourceRegister1Index { get; set; }
        public byte SourceRegister2Index { get; set; }

        public override List<byte> Parse(InstructionMetadata metadata, string instruction)
        {
            Match match = Regex.Match(instruction, @"[rR](\d+) +[rR](\d+) +[rR](\d+)$");

            DestinationRegisterIndex = byte.Parse(match.Groups[1].Value);
            SourceRegister1Index = byte.Parse(match.Groups[2].Value);
            SourceRegister2Index = byte.Parse(match.Groups[3].Value);

            return [OpCode, DestinationRegisterIndex, SourceRegister1Index, SourceRegister2Index];
        }

        public override string Disassemble(byte[] machineCode)
        {
            StringBuilder toReturn = new();

            toReturn.Append(Name);

            for (int i = 1; i < 4; i++)
            {
                toReturn.Append($" R{machineCode[i]}");
            }

            return toReturn.ToString();
        }

        public override void DeCode(ReadOnlySpan<byte> bytes)
        {
            DestinationRegisterIndex = bytes[1];
            SourceRegister1Index = bytes[2];
            SourceRegister2Index = bytes[3];
        }
    }

    public class ADD : MathInstruction
    {
        public override byte OpCode => 0x10;
        public override string Name { get; } = nameof(ADD);

        public override void Execute(EmulatorStatus status) => status.registers[DestinationRegisterIndex].Value = Register.AddFromRegisters(status.registers[SourceRegister1Index], status.registers[SourceRegister2Index]);
    }
    #endregion

    public abstract class RegRegPadLayout : Instruction
    {
        //PUSH, JUMPi, POP
    }

    public abstract class RegValValLayout : Instruction
    {
        //SET, LOAD, STR, JUMPT
        public byte DestinationOrSourceRegisterIndex { get; set; }
        public byte ValueHB { get; set; }
        public byte ValueLB { get; set; }

        public override List<byte> Parse(InstructionMetadata metadata, string instruction)
        {
            Match match = Regex.Match(instruction, @"[rR](\d+) +(\d+) +(\d+)$");

            DestinationOrSourceRegisterIndex = byte.Parse(match.Groups[1].Value);
            ValueHB = byte.Parse(match.Groups[2].Value);
            ValueLB = byte.Parse(match.Groups[3].Value);

            return [OpCode, DestinationOrSourceRegisterIndex, ValueHB, ValueLB];
        }

        public override string Disassemble(byte[] machineCode)
        {
            StringBuilder toReturn = new();

            toReturn.Append(Name);

            toReturn.Append($" R{machineCode[1]}");

            for (int i = 2; i < 4; i++)
            {
                toReturn.Append($" {machineCode[i]}");
            }

            return toReturn.ToString();
        }

        public override void DeCode(ReadOnlySpan<byte> bytes)
        {
            DestinationOrSourceRegisterIndex = bytes[1];
            ValueHB = bytes[2];
            ValueLB = bytes[3];
        }
    }

    public class SET : RegValValLayout
    {
        public override byte OpCode => 0x40;

        public override string Name { get; } = nameof(SET);

        public override void Execute(EmulatorStatus status) => status.registers[DestinationOrSourceRegisterIndex].Value = Register.SetFromHBLB(ValueHB, ValueLB);
    }

    public abstract class RegPadPadLayout : Instruction
    {
        //COPY, LOADi, STRi, JMPTi, NOT
    }

    public class JMP : Instruction
    {
        public override byte OpCode { get; } = 0x30;
        public override string Name { get; } = nameof(JMP);

        public byte ValueHB { get; set; }
        public byte ValueLB { get; set; }

        public override void DeCode(ReadOnlySpan<byte> bytes)
        {
            ValueHB = bytes[1];
            ValueLB = bytes[2];
        }

        public override string Disassemble(byte[] machineCode)
        {
            StringBuilder toReturn = new();

            toReturn.Append(Name);

            toReturn.Append($" #label_pos_{Register.SetFromHBLB(ValueHB, ValueLB)}");

            return toReturn.ToString();
        }

        public override void Execute(EmulatorStatus status) => status.IP = Register.SetFromHBLB(ValueHB, ValueLB);

        public override List<byte> Parse(InstructionMetadata metadata, string instruction)
        {
            Match match = Regex.Match(instruction, @"#\w+$");

            ushort address = metadata.labelAddressMaps[match.Value];

            ValueHB = (byte)(address >> 8);
            ValueLB = (byte)(address & 0xFF);

            return [OpCode, ValueHB, ValueLB, 0xFF];
        }
    }

    public class NOP : Instruction
    {
        public override byte OpCode => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        public override void DeCode(ReadOnlySpan<byte> bytes)
        {
            throw new NotImplementedException();
        }

        public override string Disassemble(byte[] machineCode)
        {
            throw new NotImplementedException();
        }

        public override void Execute(EmulatorStatus status)
        {
            throw new NotImplementedException();
        }

        public override List<byte> Parse(InstructionMetadata metadata, string instruction)
        {
            throw new NotImplementedException();
        }
    }
}
