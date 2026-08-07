using JVMLibrary.ConstantPoolInfos;
using JVMLibrary.Utility;

namespace JVMLibrary.Attributes
{
    public class LineNumberTableAttributeInfo : AttributeInfo
    {
        public ushort LineNumberTableLength { get; set; }
        public LineNumberEntry[] LineNumberTable { get; set; }

        public class LineNumberEntry
        {
            public ushort StartProgramCounter { get; set; }
            public ushort LineNumber { get; set; }
        }

        public LineNumberTableAttributeInfo(ref ReadOnlySpan<byte> bytecode, ConstantPoolInfo[] ConstantPool)
        {
            AttributeNameIndex = bytecode.CutU2();
            AttributeLength = bytecode.CutU4();
            LineNumberTableLength = bytecode.CutU2();
            LineNumberTable = new LineNumberEntry[LineNumberTableLength];
            for (int i = 0; i < LineNumberTableLength; i++)
            {
                LineNumberTable[i] = new LineNumberEntry
                {
                    StartProgramCounter = bytecode.CutU2(),
                    LineNumber = bytecode.CutU2()
                };
            }
        }

        public override byte[] EmitBytes()
        {
            List<byte> toReturn = [];

            toReturn.AddBytes(AttributeNameIndex)
                    .AddBytes(AttributeLength)
                    .AddBytes(LineNumberTableLength);

            foreach (LineNumberEntry entry in LineNumberTable)
            {
                toReturn.AddBytes(entry.StartProgramCounter)
                        .AddBytes(entry.LineNumber);
            }

            return toReturn.ToArray();
        }
    }
}