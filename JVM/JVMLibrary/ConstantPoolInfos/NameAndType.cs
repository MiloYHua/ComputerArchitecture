using JVMLibrary.Information;
using JVMLibrary.Utility;

namespace JVMLibrary.ConstantPoolInfos
{
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