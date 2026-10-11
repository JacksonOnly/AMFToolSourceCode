using System;
using System.Collections.Generic;

namespace AMFTool.Library.Qualcomm;

public class GptEfi
{
	public static List<GptEfi> Uefi;

	public int SectorSize { get; set; }

	public int FileSectorOffset { get; set; }

	public string FileName { get; set; }

	public string Label { get; set; }

	public string NumPartitionSectors { get; set; }

	public bool PartOfSingleImage { get; set; }

	public int PhysicalPartitionNumber { get; set; }

	public bool ReadBackVerify { get; set; }

	public string SizeInKB { get; set; }

	public bool Sparse { get; set; }

	public string StartByteHex { get; set; }

	public string StartSector { get; set; }

	public Guid Guid { get; set; }

	public GptEfi(int SectorSize, int FileSectorOffset, string FileName, string Label, string NumPartitionSectors, bool PartOfSingleImage, int PhysicalPartitionNumber, bool ReadBackVerify, string sizeInKB, bool Sparse, string StartByteHex, string StartSector, Guid Guid)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		this.SectorSize = SectorSize;
		this.FileSectorOffset = FileSectorOffset;
		this.FileName = FileName;
		this.Label = Label;
		this.NumPartitionSectors = NumPartitionSectors;
		this.PartOfSingleImage = PartOfSingleImage;
		this.PhysicalPartitionNumber = PhysicalPartitionNumber;
		this.ReadBackVerify = ReadBackVerify;
		SizeInKB = sizeInKB;
		this.Sparse = Sparse;
		this.StartByteHex = StartByteHex;
		this.StartSector = StartSector;
		this.Guid = Guid;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
