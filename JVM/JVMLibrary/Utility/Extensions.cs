namespace JVMLibrary.Utility
{
	public static class SpanExtensions
	{
		public static byte CutU1(this ref ReadOnlySpan<byte> span)
		{
			byte u1 = span[0];
			span = span.Slice(sizeof(byte));
			return u1;
		}

		public static ushort CutU2(this ref ReadOnlySpan<byte> span)
		{
			ushort u2 = (ushort)(span[0] << 8 | span[1]);
			span = span.Slice(sizeof(ushort));
			return u2;
		}

		public static uint CutU4(this ref ReadOnlySpan<byte> span)
		{
			uint u4 = (uint)(span[0] << 24 | span[1] << 16 | span[2] << 8 | span[3]);
			span = span.Slice(sizeof(uint));
			return u4;
		}

		public static void Shorten(this ref ReadOnlySpan<byte> span, int maxLength)
		{
			span = span.Slice(maxLength);
		}
	}

	public static class ListExtensions
	{
		public static List<byte> AddBytes(this List<byte> list, byte value)
		{
			list.Add(value);
			return list;
		}

		public static List<byte> AddBytes(this List<byte> list, ushort value)
		{
			list.Add((byte)(value >> 8));
			list.Add((byte)(value & 0xFF));
			return list;
		}

		public static List<byte> AddBytes(this List<byte> list, uint value)
		{
			list.Add((byte)(value >> 24));
			list.Add((byte)((value >> 16) & 0xFF));
			list.Add((byte)((value >> 8) & 0xFF));
			list.Add((byte)(value & 0xFF));
			return list;
		}

		public static List<byte> AddBytes(this List<byte> list, byte[] bytes)
		{
			list.AddRange(bytes);
			return list;
		}

		public static List<byte> AddBytes(this List<byte> list, List<byte> bytes)
		{
			list.AddRange(bytes);
			return list;
		}
	}
}