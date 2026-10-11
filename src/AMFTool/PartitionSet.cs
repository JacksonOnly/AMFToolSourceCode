using System.Collections.Generic;

namespace AMFTool;

public class PartitionSet
{
	public static readonly List<string> MTKReset;

	public static readonly List<string> MTKSafeFormat;

	public static readonly List<string> MTKFrp;

	public static readonly List<string> MTKNVBackup;

	public static readonly List<string> MTKNVErase;

	public static readonly List<string> RebuildUserDATA;

	public static readonly List<string> MTKID;

	public static readonly List<string> MTKLK;

	public static readonly List<string> MTKDemo;

	public static readonly List<string> MTKUBL;

	public static readonly List<string> SPDReset;

	public static readonly List<string> SPDfrp;

	public static readonly List<string> EDLReset;

	public static readonly List<string> EDLFrp;

	public static readonly List<string> EDLEfs;

	public static readonly List<string> EDLMicloud1;

	public static readonly List<string> EDLMicloud2;

	public static readonly List<string> EDLCantdownload;

	public static readonly List<string> EDLLostMode;

	static PartitionSet()
	{
		string item = "userdata";
		string item2 = "frp";
		string item3 = "persistent";
		string item4 = "misc";
		string item5 = "para";
		string item6 = "config";
		string item7 = "preloader";
		string item8 = "nvram";
		string item9 = "nvdata";
		string item10 = "md1img";
		string item11 = "md1img_a";
		string item12 = "protect1";
		string item13 = "protect2";
		string item14 = "seccfg";
		string item15 = "lk";
		string item16 = "lk_a";
		string item17 = "persist";
		string item18 = "backup";
		string item19 = "metadata";
		string item20 = "md_udc";
		string item21 = "cache";
		string item22 = "modemst1";
		string item23 = "modemst2";
		string item24 = "fsg";
		string item25 = "modem";
		string item26 = "modem_a";
		string item27 = "modem_b";
		string item28 = "param";
		string item29 = "up_param";
		string item30 = "steady";
		string item31 = "dtbo";
		MTKReset = new List<string> { item, item2, item3 };
		MTKSafeFormat = new List<string> { item4, item5, item2, item3 };
		MTKFrp = new List<string> { item2, item3, item6 };
		MTKNVBackup = new List<string> { item7, item8, item9, item10, item11, item12, item13, item14, item15, item16 };
		MTKNVErase = new List<string> { item8, item9 };
		RebuildUserDATA = new List<string> { item5, item };
		MTKID = new List<string> { item2, item17 };
		MTKLK = new List<string> { item15, item16 };
		MTKDemo = new List<string> { item2, item18 };
		MTKUBL = new List<string> { item, item19, item20 };
		SPDReset = new List<string> { item, item21, item19, item17 };
		SPDfrp = new List<string> { item17 };
		EDLReset = new List<string> { item2, item6, item3, item4 };
		EDLFrp = new List<string> { item2, item6, item3 };
		EDLEfs = new List<string> { item22, item23, item24 };
		EDLMicloud1 = new List<string> { item25, item26, item27, item17 };
		EDLMicloud2 = new List<string> { item17 };
		EDLCantdownload = new List<string> { item28, item29, item30, item31 };
		EDLLostMode = new List<string> { item2, item6, item3, item4, item28, item29, item30, item31 };
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
