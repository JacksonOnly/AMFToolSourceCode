namespace AMFTool.Library.Qualcomm;

public class Pkts
{
	public static string PktSha256FinalVip()
	{
		return "<?xml version=\"1.0\" ?><data>\n<sha256final />\n</data>\n";
	}

	public static string PktSha256InitVip()
	{
		return "<?xml version=\"1.0\" ?><data>\n<sha256init />\n</data>\n";
	}

	public static string PktTransferCfg()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><data><transfercfg reboot_type=\"off\" timeout_in_sec=\"90\" /></data>";
	}

	public static string PktVerify()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><data><verify value=\"ping\" EnableVip=\"1\"/></data>";
	}

	public static string PktSha256Init()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><data><sha256init Verbose=\"1\"/></data>";
	}

	public static string PktFhConfig(string storage, int payload = 16384)
	{
		string format = "<?xml version=\"1.0\" ?><data><configure MemoryName=\"{0}\" Verbose=\"0\" AlwaysValidate=\"0\" MaxPayloadSizeToTargetInBytes=\"{1}\" MaxPayloadSizeFromTargetInBytes=\"{2}\" AckRawDataEveryNumPackets=\"0\" ZlpAwareHost=\"1\" SkipStorageInit=\"0\" /></data>";
		return string.Format(format, storage, payload, payload);
	}

	public static string PktStorage(int drive)
	{
		string format = "<?xml version=\"1.0\" ?><data><getstorageinfo physical_partition_number=\"{0}\"/></data>";
		return string.Format(format, drive);
	}

	public static string PktNop()
	{
		return "<?xml version=\"1.0\" ?><data><nop verbose=\"0\"  value=\"ping\"/></data>";
	}

	public static string Sign()
	{
		return "<?xml version=\"1.0\" ?><data><sig TargetName=\"sig\" size_in_bytes=\"256\" verbose=\"1\"/></data>";
	}

	public static string PktSendNop()
	{
		return "<?xml version=\"1.0\" ?><data><nop /></data>";
	}

	public static string PktSendReset()
	{
		return "<?xml version=\"1.0\" ?><data><power value=\"reset\"/></data>";
	}

	public static string PktTag()
	{
		return "<?xml version=\"1.0\" ?><data> <poke SizeInBytes=\"4\" address64=\"0x08062F04\" value=\"1\"/></data>";
	}

	public static string PktRead(int sectorSize, int numPartitionSectors, int physicalPartitionNumber, string startSector)
	{
		string format = "<?xml version = \"1.0\"?><data><read SECTOR_SIZE_IN_BYTES=\"{0}\" num_partition_sectors=\"{1}\" physical_partition_number=\"{2}\" start_sector=\"{3}\"/></data>";
		return string.Format(format, sectorSize, numPartitionSectors, physicalPartitionNumber, startSector);
	}

	public static string PktBuffer(int sectorSize, int numPartitionSectors, int physicalPartitionNumber, string startSector, string filename, string label)
	{
		double field_ = Class38.field_540;
		string format = "<?xml version=\"1.0\" ?><data><read SECTOR_SIZE_IN_BYTES=\"{0}\" file_sector_offset=\"0\" filename=\"{1}\" label=\"{2}\" num_partition_sectors=\"{3}\" partofsingleimage=\"true\" physical_partition_number=\"{4}\" readbackverify=\"false\" size_in_KB=\"{5:F1}\" sparse=\"false\" start_byte_hex=\"0x{6:X}\" start_sector=\"{7}\" /></data>";
		long num = long.Parse(startSector);
		double num2 = (double)numPartitionSectors * (double)sectorSize / field_;
		return string.Format(format, sectorSize, filename, label, numPartitionSectors, physicalPartitionNumber, num2, num, startSector);
	}

	public static string PktReadVip(int sectorSize, int numPartitionSectors, int physicalPartitionNumber, string startSector, string sizeInKb, string label, string filename)
	{
		string format = "<?xml version=\"1.0\" ?><data><read SECTOR_SIZE_IN_BYTES=\"{0}\" num_partition_sectors=\"{1}\" physical_partition_number=\"{2}\" size_in_KB=\"{3}\" start_sector=\"{4}\" /></data>";
		string format2 = "<?xml version=\"1.0\" ?><data><read SECTOR_SIZE_IN_BYTES=\"{0}\" filename=\"{1}\" label=\"{2}\" num_partition_sectors=\"{3}\" physical_partition_number=\"{4}\" size_in_KB=\"{5}\" sparse=\"false\" start_sector=\"{6}\" /></data>";
		if (!string.IsNullOrEmpty(label))
		{
			return string.Format(format2, sectorSize, filename, label, numPartitionSectors, physicalPartitionNumber, sizeInKb, startSector);
		}
		return string.Format(format, sectorSize, numPartitionSectors, physicalPartitionNumber, sizeInKb, startSector);
	}

	public static string BootConSet(string boot)
	{
		string text = "<?xml version=\"1.0\" ?><data><setbootablestoragedrive value=\"";
		string text2 = "\"/></data>";
		return text + boot + text2;
	}

	public static string PktProgram(string sectorSize, string numPartitionSectors, string physicalPartitionNumber, string startSector)
	{
		string text = "<?xml version = \"1.0\"?><data><program SECTOR_SIZE_IN_BYTES=\"";
		string text2 = "\" file_sector_offset=\"0\" num_partition_sectors=\"";
		string text3 = "\" physical_partition_number=\"";
		string text4 = "\" start_sector=\"";
		string text5 = "\"/></data>";
		return text + sectorSize + text2 + numPartitionSectors + text3 + physicalPartitionNumber + text4 + startSector + text5;
	}

	public static string PktPatch(string sectorSize, string byteOffset, string filename, string physicalPartitionNumber, string sizeInBytes, string startSector, string value, string what)
	{
		string text = "<?xml version = \"1.0\"?><data><patch SECTOR_SIZE_IN_BYTES=\"";
		string text2 = "\" byte_offset=\"";
		string text3 = "\" filename=\"";
		string text4 = "\" physical_partition_number=\"";
		string text5 = "\" size_in_bytes=\"";
		string text6 = "\" start_sector=\"";
		string text7 = "\" value=\"";
		string text8 = "\" what=\"";
		string text9 = "\"/></data>";
		return text + sectorSize + text2 + byteOffset + text3 + filename + text4 + physicalPartitionNumber + text5 + sizeInBytes + text6 + startSector + text7 + value + text8 + what + text9;
	}

	public static string PktErase(string sectorSize, string numPartitionSectors, string physicalPartitionNumber, string startSector)
	{
		string text = "<?xml version = \"1.0\"?><data><erase SECTOR_SIZE_IN_BYTES=\"";
		string text2 = "\" num_partition_sectors=\"";
		string text3 = "\" physical_partition_number=\"";
		string text4 = "\" start_sector=\"";
		string text5 = "\"/></data>";
		return text + sectorSize + text2 + numPartitionSectors + text3 + physicalPartitionNumber + text4 + startSector + text5;
	}

	public static string PktEraseVip(long sectorSize, long numPartitionSectors, int physicalPartitionNumber, long startSector, string label, string filename)
	{
		string format = "<?xml version=\"1.0\" ?><data><erase SECTOR_SIZE_IN_BYTES=\"{0}\" num_partition_sectors=\"{1}\" physical_partition_number=\"{2}\" start_sector=\"{3}\" label=\"{4}\" filename=\"{5}\" /></data>";
		return string.Format(format, sectorSize, numPartitionSectors, physicalPartitionNumber, startSector, label, filename);
	}

	public static string PktWriteVip(long sectorSize, long numPartitionSectors, int physicalPartitionNumber, long startSector, string label, string filename)
	{
		string format = "<?xml version=\"1.0\"?><data><program SECTOR_SIZE_IN_BYTES=\"{0}\" num_partition_sectors=\"{1}\" physical_partition_number=\"{2}\" start_sector=\"{3}\" filename=\"{4}\" label=\"{5}\" partofsingleimage=\"true\" read_back_verify=\"true\" sparse=\"false\"/></data>";
		return string.Format(format, sectorSize, numPartitionSectors, physicalPartitionNumber, startSector, filename, label);
	}

	public static string PktEnableFlashivo()
	{
		return "<?xml version=\"1.0\" ?><data> <poke SizeInBytes=\"4\" address64=\"0x08062F04\" value=\"1\"/></data>";
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
