namespace JVMLibrary.Attributes
{
    public abstract class AttributeInfo
    {
        public delegate AttributeInfo AttributeInfoFactory(ref ReadOnlySpan<byte> bytecode, ConstantPoolInfo[] constantPool);

        public static Dictionary<string, AttributeInfoFactory> NameToInfo = new()
        {
            ["Code"] = (ref bytecode, constantPool) => new CodeAttributeInfo(ref bytecode, constantPool),
            ["SourceFile"] = (ref bytecode, constantPool) => new SourceFileAttributeInfo(ref bytecode, constantPool),
            ["LineNumberTable"] = (ref bytecode, constantPool) => new LineNumberTableAttributeInfo(ref bytecode, constantPool),
        };

        public ushort AttributeNameIndex { get; set; }

        public uint AttributeLength { get; set; }

        abstract public byte[] EmitBytes();
    }
}
