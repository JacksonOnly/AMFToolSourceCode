namespace AMFTool.Library.Mediatek.Download_Agent;

public class EntryRegion
{
	public uint Buff;

	public uint Len;

	public uint StartAddr;

	public uint StartOffset;

	public uint SignLen;

	public EntryRegion(byte[] data)
	{
		StructHelper structHelper = new StructHelper(data);
		Buff = structHelper.Dword();
		Len = structHelper.Dword();
		StartAddr = structHelper.Dword();
		StartOffset = structHelper.Dword();
		SignLen = structHelper.Dword();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
