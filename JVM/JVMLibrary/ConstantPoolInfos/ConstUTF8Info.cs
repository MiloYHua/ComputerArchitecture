using JVMLibrary.Information;
using JVMLibrary.Utility;

namespace JVMLibrary.ConstantPoolInfos
{
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
}