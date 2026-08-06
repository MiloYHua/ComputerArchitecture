using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ISALibrary;

namespace Assembler
{
    public class AssemblerProgram
    {
        static List<byte> Assemble(string[] instructions)
        {
            Dictionary<string, ushort> labelPairs = new();
            List<byte> machineCode = [];

            for (ushort i = 0; i < instructions.Length; i++)
            {
                Match match = Regex.Match(instructions[i], @"^#\w+");

                if (match.Value != "" && !labelPairs.TryAdd(match.Value, i)) throw new Exception($"Label {match.Value} already exists.");
            }

            foreach (string instructionLine in instructions)
            {
                Match match = Regex.Match(instructionLine, @"^\w+|$#|$ |$^");

                if (Instruction.NameToInstruction.TryGetValue(match.Value, out var func))
                {
                    Instruction instruction = func();
                    machineCode.AddRange(instruction.Parse(new InstructionMetadata(labelPairs), instructionLine));
                    continue;
                }
                else if (match.Value is "#" or " " or "") continue;

                throw new ArgumentException($"'{match.Value}' is not a recognized command");
            }
            return machineCode;
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid argument(s)");
                return;
            }

            string filePath = args[0];
            string machineCodeFilePath = args[1];
            string[] assemblyText;

            try
            {
                assemblyText = File.ReadAllLines(filePath);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            byte[] bytes = Assemble(assemblyText).ToArray();

            File.WriteAllBytes(machineCodeFilePath, bytes);
        }
    }
}
