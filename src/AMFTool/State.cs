namespace AMFTool;

public class State
{
	public static Job Job;

	public static SelectMode Mode;

	public static Downloadmode FlashingMode;

	public static IMEIType ImeiType;

	public static string SavePath { get; set; }

	public static string FilePath { get; set; }

	public static object Data { get; set; }

	public static string Operation { get; set; }

	static State()
	{
		object obj = null;
		Job = (Job)0;
		Mode = (SelectMode)0;
		FlashingMode = (Downloadmode)0;
		ImeiType = (IMEIType)0;
		SavePath = string.Empty;
		FilePath = string.Empty;
		Data = obj;
		Operation = string.Empty;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
