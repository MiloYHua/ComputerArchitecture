using JVMLibrary.Attributes;
using JVMLibrary.Utility;
using System.Text;

namespace JVMLibrary.Information
{
    public class MethodInfo
    {
        public AccessFlags AccessFlags { get; set; }

        public ushort NameIndex { get; set; }

        public ushort DescriptorIndex { get; set; }

        public ushort AttributesCount { get; set; }

        public AttributeInfo[] Attributes { get; set; } = [];

        public MethodInfo(ref ReadOnlySpan<byte> bytecode, ConstantPoolInfo[] constantPool)
        {
            AccessFlags = (AccessFlags)bytecode.CutU2();
            NameIndex = bytecode.CutU2();
            DescriptorIndex = bytecode.CutU2();
            AttributesCount = bytecode.CutU2();
            Attributes = new AttributeInfo[AttributesCount];

            for (int i = 0; i < AttributesCount; i++)
            {
                ushort bob = Conversions.ToUShort(bytecode.Slice(0, 2));
                if (constantPool[bob] is not ConstUtf8Info utf8Info)
                {
                    throw new InvalidOperationException($"Expected '{nameof(ConstUtf8Info)}' at '{Conversions.ToUShort(bytecode.Slice(0, 2))}'. Got '{constantPool[Conversions.ToUShort(bytecode.Slice(0, 2))].GetType()}' instead.");
                }
                string str = Encoding.UTF8.GetString(utf8Info.Bytes);
                Attributes[i] = AttributeInfo.NameToInfo[str](ref bytecode, constantPool);
            }
        }

        public byte[] EmitBytes()
        {
            List<byte> toEmit = [];

            toEmit.AddBytes((ushort)AccessFlags)
                  .AddBytes(NameIndex)
                  .AddBytes(DescriptorIndex)
                  .AddBytes(AttributesCount);

            foreach (AttributeInfo info in Attributes)
            {
                toEmit.AddRange(info.EmitBytes());
            }

            return toEmit.ToArray();
        }
    }

    
}
