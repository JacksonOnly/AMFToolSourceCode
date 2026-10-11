namespace AMFTool.Library.Mediatek;

public class GptSettings
{
	public int GptNumPartEntries;

	public int GptPartEntrySize;

	public int GptPartEntryStartLba;

	public GptSettings(string gptNumPartEntries = "0", string gptPartEntrySize = "0", string gptPartEntryStartLba = "0")
	{
		GptNumPartEntries = int.Parse(gptNumPartEntries);
		GptPartEntrySize = int.Parse(gptPartEntrySize);
		GptPartEntryStartLba = int.Parse(gptPartEntryStartLba);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
