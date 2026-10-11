using System;

namespace AMFTool.Library.Mediatek.Download_Agent;

public class NandInfo
{
	public int Type { get; set; } = 1;

	public int PageSize { get; set; } = 0;

	public int BlockSize { get; set; } = 512;

	public int SpareSize { get; set; } = 0;

	public long TotalSize { get; set; } = 0L;

	public long AvailableSize { get; set; } = 0L;

	public byte NandBmtExist { get; set; } = 0;

	public byte[] NandId { get; set; } = Array.Empty<byte>();

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
