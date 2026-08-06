namespace JVMLibrary
{
    public class ClassFile
    {
        #region Baloney
        public uint Magic { get; set; }
        public ushort MinorVersion { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort ConstantPoolCount { get; set; }
        public ConstantPoolInfo[] ConstantPool { get; set; } = [];
        public AccessFlags AccessFlags { get; set; }
        public ushort ThisClass { get; set; }
        public ushort SuperClass { get; set; }
        public ushort InterfacesCount { get; set; }
        public ushort[] Interfaces { get; set; } = [];
        public ushort FieldsCount { get; set; }
        public FieldInfo[] Fields { get; set; } = [];
        public ushort MethodsCount { get; set; }
        public MethodInfo[] Methods { get; set; } = [];
        public ushort AttributesCount { get; set; }
        public AttributeInfo[] Attributes { get; set; } = [];
        #endregion

        public ClassFile Parse(ReadOnlySpan<byte> bytecode)
        {
            Magic = bytecode.CutU4();

            if (Magic != 0xCAFEBABE) throw new InvalidProgramException($"Invalid class file: Magic number '{Magic}' does not match 0xCAFEBABE.");

            MinorVersion = bytecode.CutU2();
            MajorVersion = bytecode.CutU2();
            ConstantPoolCount = bytecode.CutU2();
            ConstantPool = new ConstantPoolInfo[ConstantPoolCount - 1];

            for (int i = 0; i < ConstantPool.Length; i++) ConstantPool[i] = ConstantPoolInfo.TagInfoPairs[(ConstantPoolTag)bytecode.CutU1()](ref bytecode);

            AccessFlags = (AccessFlags)bytecode.CutU2();
            ThisClass = bytecode.CutU2();
            SuperClass = bytecode.CutU2();
            InterfacesCount = bytecode.CutU2();




            return this;
        }

        public List<byte> EmitBytes()
        {
            List<byte> bytecode = [];

            bytecode.AddBytes(Magic)
                    .AddBytes(MinorVersion)
                    .AddBytes(MajorVersion)
                    .AddBytes(ConstantPoolCount);

            foreach (ConstantPoolInfo info in ConstantPool) bytecode.AddRange(info.EmitBytes());

            bytecode.AddBytes((byte)AccessFlags)
                    .AddBytes(ThisClass)
                    .AddBytes(SuperClass)
                    .AddBytes(InterfacesCount);

            return bytecode;
        }
    }



    public class FieldInfo
    {
        public AccessFlags AccessFlags { get; set; }

        public ushort NameIndex { get; set; }

        public ushort DescriptorIndex { get; set; }

        public ushort AttributesCount { get; set; }

        public AttributeInfo[] Attributes { get; set; } = [];
    }

    public class MethodInfo
    {
        public AccessFlags AccessFlags { get; set; }

        public ushort NameIndex { get; set; }

        public ushort DescriptorIndex { get; set; }

        public ushort AttributesCount { get; set; }

        public AttributeInfo[] Attributes { get; set; } = [];
    }

    public class AttributeInfo
    {
        public ushort AttributeNameIndex { get; set; }

        public uint AttributeLength { get; set; }

        public byte[] Info { get; set; } = [];
    }
}
