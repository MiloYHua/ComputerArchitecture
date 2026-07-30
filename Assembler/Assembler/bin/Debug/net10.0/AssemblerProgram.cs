using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ISALibrary;

namespace Assembler
{
    public class AssemblerProgram
    {
        static List<byte> Assemble(string[] instructions)
        {
            List<byte> machineCode = [];

            foreach (string instructionLine in instructions)
            {
                Match match = Regex.Match(instructionLine, @"^\w+");

                if (Instruction.NameToInstruction.TryGetValue(match.Value, out var func))
                {
                    Instruction instruction = func();
                    machineCode.AddRange(instruction.Parse(instructionLine));
                    continue;
                }

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
