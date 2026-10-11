namespace AMFTool.Ext4;

public class GroupDescriptor
{
	public ulong InodeTable { get; private set; }

	public static GroupDescriptor Parse(byte[] buffer, bool has64)
	{
		int offset = 8;
		uint num = Binary.ReadU32LE(buffer, ref offset);
		uint num2 = 0u;
		if (!has64 || buffer.Length < 44)
		{
			return new GroupDescriptor
			{
				InodeTable = (0uL | (ulong)num)
			};
		}
		offset = 40;
		num2 = Binary.ReadU32LE(buffer, ref offset);
		return new GroupDescriptor
		{
			InodeTable = (((ulong)num2 << 32) | num)
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
