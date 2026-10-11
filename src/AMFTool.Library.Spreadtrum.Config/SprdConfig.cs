namespace AMFTool.Library.Spreadtrum.Config;

public class SprdConfig
{
	public ChipConfig ChipCfg;

	public bool InitCPUName(string name)
	{
		if (!string.IsNullOrWhiteSpace(name))
		{
			return ChipConfig.HwConfig.TryGetValue(name.Trim(), out ChipCfg);
		}
		return false;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
