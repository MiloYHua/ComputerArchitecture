using System.Text;
using ISALibrary;

namespace Emulator
{
    public class EmulatorProgram
    {
        static void Main(string[] args)
        {
            byte[] RAM = File.ReadAllBytes(args[0]);
            EmulatorStatus status = new EmulatorStatus();

            if ((RAM.Length >> 2) << 2 != RAM.Length) throw new ArgumentException("Incorrect machine code length.");

            ReadOnlySpan<byte> machineCodeSpan = new(RAM);

            while (status.IP + 4 <= machineCodeSpan.Length)
            {
                ReadOnlySpan<byte> line = machineCodeSpan.Slice(status.IP, 4);

                OpCode opCode = (OpCode)line[0];
                string? name = Enum.GetName(opCode);

                if (name is null) throw new Exception($"{opCode} is not a valid instruction.");

                Instruction instruction = Instruction.NameToInstruction[name]();
                instruction.DeCode(line);
                instruction.Execute(status);
                status.IP += 4;
            }
        }
    }
}