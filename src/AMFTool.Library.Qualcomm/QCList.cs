using System.Collections.Generic;

namespace AMFTool.Library.Qualcomm;

public class QCList
{
	public static List<QCList> Partition = new List<QCList>();

	public string SectorSize;

	public string FileName;

	public string Label;

	public string NumPartitionSectors;

	public string PhysicalPartitionNumber;

	public string StartSector;

	public string SizeInKB;

	public QCList(string SectorSize, string FileName, string Label, string NumPartitionSectors, string PhysicalPartitionNumber, string StartSector, string SizeInKB)
	{
		this.SectorSize = SectorSize;
		this.FileName = FileName;
		this.Label = Label;
		this.NumPartitionSectors = NumPartitionSectors;
		this.PhysicalPartitionNumber = PhysicalPartitionNumber;
		this.StartSector = StartSector;
		this.SizeInKB = SizeInKB;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
