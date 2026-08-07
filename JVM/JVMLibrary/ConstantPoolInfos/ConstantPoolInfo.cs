using JVMLibrary.Information;

namespace JVMLibrary.ConstantPoolInfos
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
            [ConstantPoolTag.Methodref] = (ref span) => new ConstMethodRefInfo(ref span),
            //[ConstantPoolTag.InterfaceMethodref] = (ref span) => new ConstInterfaceMethodrefInfo(ref span),
            [ConstantPoolTag.NameAndType] = (ref span) => new ConstNameAndTypeInfo(ref span),
            //[ConstantPoolTag.MethodHandle] = (ref span) => new ConstMethodHandleInfo(ref span),
            //[ConstantPoolTag.MethodType] = (ref span) => new ConstMethodTypeInfo(ref span),
            //[ConstantPoolTag.InvokeDynamic] = (ref span) => new ConstInvokeDynamicInfo(ref span),
        };

        public abstract byte Tag { get; set; }

        public abstract byte[] EmitBytes();
    }
}