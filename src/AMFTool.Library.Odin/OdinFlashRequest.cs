using System.Collections.Generic;

namespace AMFTool.Library.Odin;

public static class OdinFlashRequest
{
	private static readonly object sync = new object();

	private static List<OdinList> selectionValue = new List<OdinList>();

	public static List<OdinList> selection
	{
		get
		{
			lock (sync)
			{
				return new List<OdinList>(selectionValue);
			}
		}
	}

	public static void ConfigureFlash(List<OdinList> selection)
	{
		lock (sync)
		{
			selectionValue = ((selection != null) ? new List<OdinList>(selection) : new List<OdinList>());
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
