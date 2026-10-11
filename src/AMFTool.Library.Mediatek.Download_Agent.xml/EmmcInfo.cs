using System;

namespace AMFTool.Library.Mediatek.Download_Agent.xml;

public class EmmcInfo
{
	public long Type { get; set; } = 1L;

	public long BlockSize { get; set; } = 512L;

	public long Boot1Size { get; set; } = 0L;

	public long Boot2Size { get; set; } = 0L;

	public long RpmbSize { get; set; } = 0L;

	public long Gp1Size { get; set; } = 0L;

	public long Gp2Size { get; set; } = 0L;

	public long Gp3Size { get; set; } = 0L;

	public long Gp4Size { get; set; } = 0L;

	public long UserSize { get; set; } = 0L;

	public byte[] Cid { get; set; } = Array.Empty<byte>();

	public long FwVer { get; set; } = 0L;

	public byte[] LifeTime { get; set; } = Array.Empty<byte>();

	public byte[] Slc { get; set; } = Array.Empty<byte>();

	public byte[] Mlc { get; set; } = Array.Empty<byte>();

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
