using System.Collections.Generic;

namespace AMFTool;

public class EDLConfig
{
	public static string Storage { get; set; }

	public static string PathLoader { get; set; }

	public static List<string> PatchXML { get; }

	public static string PathFirmware { get; set; }

	public static bool EDLServer { get; set; }

	static EDLConfig()
	{
		string text = "emmc";
		Storage = text;
		PathLoader = string.Empty;
		PatchXML = new List<string>();
		PathFirmware = string.Empty;
		EDLServer = true;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
