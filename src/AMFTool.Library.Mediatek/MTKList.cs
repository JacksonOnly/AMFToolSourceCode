using System.Collections.Generic;

namespace AMFTool.Library.Mediatek;

public class MTKList
{
	public static List<MTKList> Partition = new List<MTKList>();

	public string PartName;

	public string FileName;

	public string FilePath;

	public string StartLba;

	public string LastLba;

	public MTKList(string partName, string fileName, string startLba, string lastLba, string filePath)
	{
		PartName = partName;
		FileName = fileName;
		StartLba = startLba;
		LastLba = lastLba;
		FilePath = filePath;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
