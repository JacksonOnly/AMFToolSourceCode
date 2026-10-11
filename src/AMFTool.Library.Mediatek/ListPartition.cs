using System.Collections.Generic;

namespace AMFTool.Library.Mediatek;

public class ListPartition
{
	public static List<ListPartition> List = new List<ListPartition>();

	public int Value;

	public string PartName;

	public string FileName;

	public long Start;

	public long Size;

	public string FilePath;

	public ListPartition(int value, string partName, string fileName, long start, long size, string filePath = "")
	{
		Value = value;
		PartName = partName;
		FileName = fileName;
		Start = start;
		Size = size;
		FilePath = filePath;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
