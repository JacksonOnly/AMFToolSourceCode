using System;

namespace AMFTool.Library.Mediatek.Download_Agent;

public class UfsInfo
{
	public int Type { get; set; } = 1;

	public long BlockSize { get; set; } = 0L;

	public long Lu0Size { get; set; } = 0L;

	public long Lu1Size { get; set; } = 0L;

	public long Lu2Size { get; set; } = 0L;

	public long Lu3Size { get; set; } = 0L;

	public byte[] Cid { get; set; } = Array.Empty<byte>();

	public byte[] Revision { get; set; } = Array.Empty<byte>();

	public byte[] MrfId { get; set; } = Array.Empty<byte>();

	public byte[] Vendor { get; set; } = Array.Empty<byte>();

	public byte[] FwVer { get; set; } = Array.Empty<byte>();

	public byte[] LfTime { get; set; } = Array.Empty<byte>();

	public byte[] Slc { get; set; } = Array.Empty<byte>();

	public byte[] Mlc { get; set; } = Array.Empty<byte>();

	public byte[] Serial { get; set; } = Array.Empty<byte>();

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
