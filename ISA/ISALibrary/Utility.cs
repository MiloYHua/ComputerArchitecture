

using System.Runtime.Loader;

namespace ISALibrary
{
    public struct EmulatorStatus
    {
        public Register[] registers;
        public ushort IP;

        public EmulatorStatus()
        {
            registers = new Register[256];
            IP = 0;
        }
    }
    public struct InstructionMetadata
    {
        public Dictionary<string, ushort> labelAddressMaps { get; set; }
        public Dictionary<ushort, string> addressLabelMaps { get; set; }

        public InstructionMetadata(Dictionary<string, ushort> labelAddressMaps)
        {
            this.labelAddressMaps = labelAddressMaps;
            addressLabelMaps = new();
        }

        public InstructionMetadata(Dictionary<ushort, string> addressLabelMaps)
        {
            this.addressLabelMaps = addressLabelMaps;
            labelAddressMaps = new();
        }
    }

    public struct Register
    {
        public byte Index { get; set; }
        public ushort Value { get; set; }

        public string Name { get => "R " + Index; }

        public static ushort AddFromRegisters(Register a, Register b) => (ushort)(a.Value + b.Value);
        public static ushort SetFromHBLB(ushort a, ushort b) => (ushort)((a << 8) | b);

        //public static ushort operator +(Register lhs, Register rhs) => (ushort)(lhs.Value + rhs.Value);
    }

    public enum OpCode
    {
        ADD = 0x10,
        JMP = 0x30,
        SET = 0x40,
    }
}
