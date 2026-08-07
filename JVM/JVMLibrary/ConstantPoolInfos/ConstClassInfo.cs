using JVMLibrary.Information;
using JVMLibrary.Utility;

namespace JVMLibrary.ConstantPoolInfos
{
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
}