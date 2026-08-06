using JVMLibrary.Utility;
using System.Text;

namespace JVMLibrary.Attributes
{
    public class CodeAttributeInfo : AttributeInfo
    {
        public ushort MaxStack { get; set; }
        public ushort MaxLocals { get; set; }

        public uint CodeLength { get; set; }
        public byte[] Code { get; set; } = [];

        public ushort ExceptionTableLength { get; set; }
        public ExceptionTableEntry[] ExceptionTable { get; set; } = [];

        public ushort AttributesCount { get; set; }
        public AttributeInfo[] Attributes { get; set; } = [];

        public class ExceptionTableEntry
        {
            public ushort StartProgramCounter { get; set; }
            public ushort EndProgramCounter { get; set; }
            public ushort HandlerProgramCounter { get; set; }
            public ushort CatchType { get; set; }
        }

        public CodeAttributeInfo(ref ReadOnlySpan<byte> bytecode, ConstantPoolInfo[] ConstantPool)
        {
            AttributeNameIndex = bytecode.CutU2();
            AttributeLength = bytecode.CutU4();
            MaxStack = bytecode.CutU2();
            MaxLocals = bytecode.CutU2();
            CodeLength = bytecode.CutU4();
            Code = new byte[CodeLength];

            for (int i = 0; i < CodeLength; i++) Code[i] = bytecode.CutU1();

            ExceptionTableLength = bytecode.CutU2();
            ExceptionTable = new ExceptionTableEntry[ExceptionTableLength];

            for (int i = 0; i < ExceptionTableLength; i++)
            {
                ExceptionTable[i] = new ExceptionTableEntry
                {
                    StartProgramCounter = bytecode.CutU2(),
                    EndProgramCounter = bytecode.CutU2(),
                    HandlerProgramCounter = bytecode.CutU2(),
                    CatchType = bytecode.CutU2()
                };
            }

            AttributesCount = bytecode.CutU2();

            Attributes = new AttributeInfo[AttributesCount];

            for (int i = 0; i < AttributesCount; i++)
            {
                if (ConstantPool[Conversions.ToUShort(bytecode.Slice(0, 2))] is not ConstUtf8Info utf8Info)
                {
                    throw new InvalidOperationException($"Expected '{nameof(ConstUtf8Info)}' at '{Attributes[i].AttributeNameIndex}'. Got '{ConstantPool[Attributes[i].AttributeNameIndex].GetType()}' instead.");
                }
                Attributes[i] = NameToInfo[Encoding.UTF8.GetString(utf8Info.Bytes)](ref bytecode, ConstantPool);
            }
        }

        public override byte[] EmitBytes()
        {
            List<byte> toReturn = [];

            toReturn.AddBytes(AttributeNameIndex)
                    .AddBytes(AttributeLength)
                    .AddBytes(MaxStack)
                    .AddBytes(MaxLocals)
                    .AddBytes(CodeLength)
                    .AddBytes(Code)
                    .AddBytes(ExceptionTableLength);

            foreach (ExceptionTableEntry entry in ExceptionTable)
            {
                toReturn.AddBytes(entry.StartProgramCounter)
                        .AddBytes(entry.EndProgramCounter)
                        .AddBytes(entry.HandlerProgramCounter)
                        .AddBytes(entry.CatchType);
            }

            toReturn.AddBytes(AttributesCount);

            foreach (AttributeInfo attribute in Attributes) toReturn.AddRange(attribute.EmitBytes());

            return toReturn.ToArray();
        }
    }
}
