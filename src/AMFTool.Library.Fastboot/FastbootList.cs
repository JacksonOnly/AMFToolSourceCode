using System.Collections.Generic;

namespace AMFTool.Library.Fastboot;

public class FastbootList
{
	public static List<FastbootList> Partitions = new List<FastbootList>();

	public string Partition;

	public string FileName;

	public string Location;

	public string Command;

	public FastbootList(string Partition, string FileName, string Location, string Command)
	{
		this.Partition = Partition;
		this.FileName = FileName;
		this.Location = Location;
		this.Command = Command;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
