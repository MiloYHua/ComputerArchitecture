using JVMLibrary.Information;
using JVMLibrary.Attributes;
using JVMLibrary.Utility;
using System.Text;

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
            ConstantPool = new ConstantPoolInfo[ConstantPoolCount];

            for (int i = 1; i < ConstantPool.Length; i++) ConstantPool[i] = ConstantPoolInfo.TagInfoPairs[(ConstantPoolTag)bytecode.CutU1()](ref bytecode);

            AccessFlags = (AccessFlags)bytecode.CutU2();
            ThisClass = bytecode.CutU2();
            SuperClass = bytecode.CutU2();
            InterfacesCount = bytecode.CutU2();
            Interfaces = new ushort[InterfacesCount];

            for (int i = 0; i < InterfacesCount; i++) Interfaces[i] = bytecode.CutU2();

            FieldsCount = bytecode.CutU2();
            Fields = new FieldInfo[FieldsCount];

            for (int i = 0; i < FieldsCount; i++) Fields[i] = new FieldInfo(ref bytecode, ConstantPool);

            MethodsCount = bytecode.CutU2();
            Methods = new MethodInfo[MethodsCount];

            for (int i = 0; i < MethodsCount; i++) Methods[i] = new MethodInfo(ref bytecode, ConstantPool);

            AttributesCount = bytecode.CutU2();
            Attributes = new AttributeInfo[AttributesCount];

            for (int i = 0; i < AttributesCount; i++)
            {
                if (ConstantPool[Conversions.ToUShort(bytecode.Slice(0, 2))] is not ConstUtf8Info utf8Info)
                {
                    throw new InvalidOperationException($"Expected '{nameof(ConstUtf8Info)}' at '{Conversions.ToUShort(bytecode.Slice(0, 2))}'. Got '{ConstantPool[Conversions.ToUShort(bytecode.Slice(0, 2))].GetType()}' instead.");
                }
                string str = Encoding.UTF8.GetString(utf8Info.Bytes);
                Attributes[i] = AttributeInfo.NameToInfo[str](ref bytecode, ConstantPool);
            }


            return this;
        }

        public List<byte> EmitBytes()
        {
            List<byte> bytecode = [];

            bytecode.AddBytes(Magic)
                    .AddBytes(MinorVersion)
                    .AddBytes(MajorVersion)
                    .AddBytes(ConstantPoolCount);

            for (int i = 1; i < ConstantPool.Length; i++)
            {
                ConstantPoolInfo info = ConstantPool[i];
                bytecode.AddRange(info.EmitBytes());
            }

            bytecode.AddBytes((ushort)AccessFlags)
                    .AddBytes(ThisClass)
                    .AddBytes(SuperClass)
                    .AddBytes(InterfacesCount);

            foreach (byte b in Interfaces) bytecode.AddBytes(b);

            bytecode.AddBytes(FieldsCount);

            foreach (FieldInfo field in Fields) bytecode.AddRange(field.EmitBytes());

            bytecode.AddBytes(MethodsCount);

            foreach (MethodInfo method in Methods) bytecode.AddRange(method.EmitBytes());

            bytecode.AddBytes(AttributesCount);

            foreach (AttributeInfo attribute in Attributes) bytecode.AddRange(attribute.EmitBytes());

            return bytecode;
        }
    }



    
}
