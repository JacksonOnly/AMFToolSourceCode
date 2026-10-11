using System.Collections.Generic;

namespace AMFTool.Library.Mediatek;

public class MtkSettings
{
	public static string PathScatter = string.Empty;

	public static List<MtkSettings> ListMtk = new List<MtkSettings>();

	public string PartitionIndex;

	public string PartitionName;

	public string FileName;

	public string IsDownload;

	public string IsUpgradable;

	public string LinearStartAddr;

	public string PartitionSize;

	public MtkSettings(string partitionIndex, string partitionName, string fileName, string isDownload, string isUpgradable, string linearStartAddr, string partitionSize)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		PartitionIndex = partitionIndex;
		PartitionName = partitionName;
		FileName = fileName;
		IsDownload = isDownload;
		IsUpgradable = isUpgradable;
		LinearStartAddr = linearStartAddr;
		LinearStartAddr = linearStartAddr;
		PartitionSize = partitionSize;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
