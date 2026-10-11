namespace AMFTool.Library.Qualcomm;

public static class PktVip
{
	public static string PktVerify()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" tsmtoolis=\"power\"?><data><verify EnableVip=\"0\"/></data>";
	}

	public static string PktNop()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" tsmtoolis=\"power\"?><data><NOP value=\"ping\"/></data>";
	}

	public static string PktStorage(int drive)
	{
		string format = "<?xml version=\"1.0\" encoding=\"UTF-8\" tsmtoolis=\"power\"?><data><getstorageinfo physical_partition_number=\"{0}\"/></data>";
		return string.Format(format, drive);
	}

	public static string PktFhConfig(string storage, int payload = 16384)
	{
		string format = "<?xml version=\"1.0\" encoding=\"UTF-8\" tsmtoolis=\"power\"?><data><configure AckRawDataEveryNumPackets=\"1000\" AlwaysValidate=\"0\" EnableFlash=\"1\" EnableVip=\"0\" MaxDigestTableSizeInBytes=\"8192\" MaxPayloadSizeToTargetInBytes=\"{0}\" MemoryName=\"{1}\" Oem=\"OPPO\" SkipStorageInit=\"0\" SkipWrite=\"0\" TargetName=\"MFDL\" Verbose=\"0\" ZlpAwareHost=\"1\"/></data>";
		return string.Format(format, payload, storage);
	}

	public static string PktBuffer(int sectorSize, int numPartitionSectors, int physicalPartitionNumber, string startSector, string filename, string label)
	{
		double field_ = Class38.field_540;
		string format = "<?xml version=\"1.0\" encoding=\"UTF-8\" tsmtoolis=\"power\"?><data><read SECTOR_SIZE_IN_BYTES=\"{0}\" file_sector_offset=\"0\" filename=\"{1}\" label=\"{2}\" num_partition_sectors=\"{3}\" partofsingleimage=\"true\" physical_partition_number=\"{4}\" readbackverify=\"false\" size_in_KB=\"{5:F1}\" sparse=\"false\" start_byte_hex=\"0x{6:X}\" start_sector=\"{7}\" /></data>";
		long num = long.Parse(startSector);
		double num2 = (double)numPartitionSectors * (double)sectorSize / field_;
		return string.Format(format, sectorSize, filename, label, numPartitionSectors, physicalPartitionNumber, num2, num, startSector);
	}

	public static string PktRead(int sectorSize, int numPartitionSectors, int physicalPartitionNumber, string startSector, string sizeInKb, string label, string filename)
	{
		string format = "<?xml version=\"1.0\" encoding=\"UTF-8\" tsmtoolis=\"power\"?><data><read SECTOR_SIZE_IN_BYTES=\"{0}\" num_partition_sectors=\"{1}\" physical_partition_number=\"{2}\" size_in_KB=\"{3}\" start_sector=\"{4}\" /></data>";
		string format2 = "<?xml version=\"1.0\" encoding=\"UTF-8\" tsmtoolis=\"power\"?><data><read SECTOR_SIZE_IN_BYTES=\"{0}\" filename=\"{1}\" label=\"{2}\" num_partition_sectors=\"{3}\" physical_partition_number=\"{4}\" size_in_KB=\"{5}\" sparse=\"false\" start_sector=\"{6}\" /></data>";
		if (!string.IsNullOrEmpty(label))
		{
			return string.Format(format2, sectorSize, filename, label, numPartitionSectors, physicalPartitionNumber, sizeInKb, startSector);
		}
		return string.Format(format, sectorSize, numPartitionSectors, physicalPartitionNumber, sizeInKb, startSector);
	}

	public static string PktErase(long sectorSize, long numPartitionSectors, int physicalPartitionNumber, long startSector, string label, string filename)
	{
		string format = "<?xml version=\"1.0\" encoding=\"UTF-8\" tsmtoolis=\"power\"?><data><erase SECTOR_SIZE_IN_BYTES=\"{0}\" num_partition_sectors=\"{1}\" physical_partition_number=\"{2}\" start_sector=\"{3}\" label=\"{4}\" filename=\"{5}\" /></data>";
		return string.Format(format, sectorSize, numPartitionSectors, physicalPartitionNumber, startSector, label, filename);
	}

	public static string PktWrite(long sectorSize, long numPartitionSectors, int physicalPartitionNumber, long startSector, string label, string filename)
	{
		string format = "<?xml version=\"1.0\" encoding=\"UTF-8\" tsmtoolis=\"power\"?><data><program SECTOR_SIZE_IN_BYTES=\"{0}\" num_partition_sectors=\"{1}\" physical_partition_number=\"{2}\" start_sector=\"{3}\" filename=\"{4}\" label=\"{5}\" partofsingleimage=\"true\" read_back_verify=\"true\" sparse=\"false\"/></data>";
		return string.Format(format, sectorSize, numPartitionSectors, physicalPartitionNumber, startSector, filename, label);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
