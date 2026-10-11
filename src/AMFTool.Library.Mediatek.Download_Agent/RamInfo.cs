namespace AMFTool.Library.Mediatek.Download_Agent;

public class RamInfo
{
	public ulong Type { get; set; } = 0uL;

	public ulong BaseAddress { get; set; } = 0uL;

	public ulong Size { get; set; } = 0uL;

	public string SizeFormat => utils.FileSize(Size);
}
