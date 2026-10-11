using System.Collections.Generic;

namespace AMFTool.Library.Mediatek;

public class GptAddr
{
	public static List<GptAddr> Addr = new List<GptAddr>();

	public string Name;

	public long Start;

	public long Size;

	public GptAddr(string name, long start, long size)
	{
		Name = name;
		Start = start;
		Size = size;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
