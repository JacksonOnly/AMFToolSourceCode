using System.Collections.Generic;

namespace AMFTool.ThreadWorker;

public class Device
{
	public string Model { get; set; }

	public string Platform { get; set; }

	public string SOC { get; set; }

	public List<string> Operation { get; set; } = new List<string>();

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
