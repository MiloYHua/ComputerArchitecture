using JVMLibrary.Information;
using JVMLibrary.Utility;

namespace JVMLibrary.ConstantPoolInfos
{
	public class ConstMethodRefInfo : ConstantPoolInfo
	{
		public override byte Tag { get; set; } = (byte)ConstantPoolTag.Methodref;
		public ushort ClassIndex { get; set; }
		public ushort NameAndTypeIndex { get; set; }

		public ConstMethodRefInfo(ref ReadOnlySpan<byte> span)
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
}