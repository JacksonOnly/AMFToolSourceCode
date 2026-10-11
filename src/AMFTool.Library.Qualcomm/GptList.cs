using System.Collections.Generic;

namespace AMFTool.Library.Qualcomm;

public class GptList
{
	public static List<GptList> GptPartList;

	public List<GptEfi> Partition { get; set; }

	public GptList(List<GptEfi> Partition = null)
	{
		this.Partition = Partition;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
