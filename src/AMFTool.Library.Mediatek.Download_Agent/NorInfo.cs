namespace AMFTool.Library.Mediatek.Download_Agent;

public class NorInfo
{
	public int Type { get; set; } = 1;

	public int PageSize { get; set; } = 0;

	public long AvailableSize { get; set; } = 0L;

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
