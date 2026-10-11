namespace AMFTool;

public class Portinfo
{
	public static string COM { get; set; } = string.Empty;

	public static string Name { get; set; } = string.Empty;

	public static string PID { get; set; } = string.Empty;

	public static string VID { get; set; } = string.Empty;

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
