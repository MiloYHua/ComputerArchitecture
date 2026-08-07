using JVMLibrary.ConstantPoolInfos;
using JVMLibrary.Utility;

namespace JVMLibrary.Attributes
{
    public class SourceFileAttributeInfo : AttributeInfo
    {
        public ushort SourceFileIndex { get; set; }

        public SourceFileAttributeInfo(ref ReadOnlySpan<byte> bytecode, ConstantPoolInfo[] constantPool)
        {
            AttributeNameIndex = bytecode.CutU2();
            AttributeLength = bytecode.CutU4();
            SourceFileIndex = bytecode.CutU2();
        }

        public override byte[] EmitBytes()
        {
            List<byte> toReturn = [];

            toReturn.AddBytes(AttributeNameIndex)
                    .AddBytes(AttributeLength)
                    .AddBytes(SourceFileIndex);
            return toReturn.ToArray();
        }
    }
}