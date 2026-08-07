using JVMLibrary.Attributes;
using JVMLibrary.ConstantPoolInfos;
using JVMLibrary.Utility;
using System.Text;

namespace JVMLibrary.Information
{
    public class FieldInfo
    {
        public AccessFlags AccessFlags { get; set; }

        public ushort NameIndex { get; set; }

        public ushort DescriptorIndex { get; set; }

        public ushort AttributesCount { get; set; }

        public AttributeInfo[] Attributes { get; set; } = [];

        public FieldInfo(ref ReadOnlySpan<byte> bytecode, ConstantPoolInfo[] ConstantPool)
        {
            AccessFlags = (AccessFlags)bytecode.CutU2();
            NameIndex = bytecode.CutU2();
            DescriptorIndex = bytecode.CutU2();
            AttributesCount = bytecode.CutU2();
            Attributes = new AttributeInfo[AttributesCount];
            for (int i = 0; i < AttributesCount; i++)
            {
                if (ConstantPool[Conversions.ToUShort(bytecode.Slice(0, 2))] is not ConstUtf8Info utf8Info)
                {
                    throw new InvalidOperationException($"Expected '{nameof(ConstUtf8Info)}' at '{Attributes[i].AttributeNameIndex}'. Got '{ConstantPool[Attributes[i].AttributeNameIndex].GetType()}' instead.");
                }
                Attributes[i] = AttributeInfo.NameToInfo[Encoding.UTF8.GetString(utf8Info.Bytes)](ref bytecode, ConstantPool);
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