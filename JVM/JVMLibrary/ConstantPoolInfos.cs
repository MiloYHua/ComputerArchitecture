using JVMLibrary.Utility;

namespace JVMLibrary
{
    public abstract class ConstantPoolInfo
    {
        public delegate ConstantPoolInfo ConstantPoolInfoFactory(ref ReadOnlySpan<byte> span);

        public static Dictionary<ConstantPoolTag, ConstantPoolInfoFactory> TagInfoPairs = new()
        {
            [ConstantPoolTag.Utf8] = (ref span) => new ConstUtf8Info(ref span),
            //[ConstantPoolTag.Integer] = (ref span) => new ConstIntegerInfo(ref span),
            //[ConstantPoolTag.Float] = (ref span) => new ConstFloatInfo(ref span),
            //[ConstantPoolTag.Long] = (ref span) => new ConstLongInfo(ref span),
            //[ConstantPoolTag.Double] = (ref span) => new ConstDoubleInfo(ref span),
            [ConstantPoolTag.Class] = (ref span) => new ConstClassInfo(ref span),
            //[ConstantPoolTag.String] = (ref span) => new ConstStringInfo(ref span),
            //[ConstantPoolTag.Fieldref] = (ref span) => new ConstFieldrefInfo(ref span),
            [ConstantPoolTag.Methodref] = (ref span) => new ConstMethodrefInfo(ref span),
            //[ConstantPoolTag.InterfaceMethodref] = (ref span) => new ConstInterfaceMethodrefInfo(ref span),
            [ConstantPoolTag.NameAndType] = (ref span) => new ConstNameAndTypeInfo(ref span),
            //[ConstantPoolTag.MethodHandle] = (ref span) => new ConstMethodHandleInfo(ref span),
            //[ConstantPoolTag.MethodType] = (ref span) => new ConstMethodTypeInfo(ref span),
            //[ConstantPoolTag.InvokeDynamic] = (ref span) => new ConstInvokeDynamicInfo(ref span),
        };

        public abstract byte Tag { get; set; }

        public abstract byte[] EmitBytes();
    }

    public class ConstMethodrefInfo : ConstantPoolInfo
    {
        public override byte Tag { get; set; } = (byte)ConstantPoolTag.Methodref;
        public ushort ClassIndex { get; set; }
        public ushort NameAndTypeIndex { get; set; }

        public ConstMethodrefInfo(ref ReadOnlySpan<byte> span)
        {
            ClassIndex = span.CutU2();
            NameAndTypeIndex = span.CutU2();
        }

        public override byte[] EmitBytes()
        {
            List<byte> combineHelper = [];
            combineHelper.AddBytes(Tag)
                         .AddBytes(ClassIndex)
                         .AddBytes(NameAndTypeIndex);
            return combineHelper.ToArray();
        }
    }

    public class ConstClassInfo : ConstantPoolInfo
    {
        public override byte Tag { get; set; } = (byte)ConstantPoolTag.Class;
        public ushort NameIndex { get; set; }

        public ConstClassInfo(ref ReadOnlySpan<byte> span)
        {
            NameIndex = span.CutU2();
        }

        public override byte[] EmitBytes()
        {
            List<byte> combineHelper = [];
            combineHelper.AddBytes(Tag)
                         .AddBytes(NameIndex);
            return combineHelper.ToArray();
        }
    }

    public class ConstUtf8Info : ConstantPoolInfo
    {
        public override byte Tag { get; set; } = (byte)ConstantPoolTag.Utf8;
        public ushort Length { get; set; }
        public byte[] Bytes { get; set; }

        public ConstUtf8Info(ref ReadOnlySpan<byte> span)
        {
            Length = span.CutU2();
            Bytes = span.Slice(0, Length).ToArray();
            span = span.Slice(Length);
        }

        public override byte[] EmitBytes()
        {
            List<byte> combineHelper = [];
            combineHelper.AddBytes(Tag)
                         .AddBytes(Length)
                         .AddBytes(Bytes);

            return combineHelper.ToArray();
        }
    }

    public class ConstNameAndTypeInfo : ConstantPoolInfo
    {
        public override byte Tag { get; set; } = (byte)ConstantPoolTag.NameAndType;
        public ushort NameIndex { get; set; }
        public ushort DescriptorIndex { get; set; }

        public ConstNameAndTypeInfo(ref ReadOnlySpan<byte> span)
        {
            NameIndex = span.CutU2();
            DescriptorIndex = span.CutU2();
        }

        public override byte[] EmitBytes()
        {
            List<byte> combineHelper = [];
            combineHelper.AddBytes(Tag)
                         .AddBytes(NameIndex)
                         .AddBytes(DescriptorIndex);
            return combineHelper.ToArray();
        }
    }
}