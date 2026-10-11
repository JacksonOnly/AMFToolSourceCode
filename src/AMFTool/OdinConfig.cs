namespace AMFTool;

public class OdinConfig
{
	public static string PitPath { get; set; } = string.Empty;

	public static bool Repartition { get; set; } = false;

	public static bool EraseEFS { get; set; } = false;

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
