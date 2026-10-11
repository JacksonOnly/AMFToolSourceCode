namespace AMFTool.Library.Qualcomm;

public static class PktA71
{
	public static string Uerdata()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<data>\r\n<eraseuserdata StorageDrive=\"0\" start_sector=\"10780672\" />\r\n</data>";
	}

	public static string Ping()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<data>\r\n<nop value=\"ping\" />\r\n</data>";
	}

	public static string Patch()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<data>\r\n  <patch SECTOR_SIZE_IN_BYTES=\"512\" byte_offset=\"16\"filename=\"DISK\"physical_partition_number=\"0\"size_in_bytes=\"4\"start_sector=\"NUM_DISK_SECTORS-1.\"value=\"0\"/>\r\n</data>";
	}

	public static string Imei()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<data>\r\n  <read SECTOR_SIZE_IN_BYTES=\"512\" num_partition_sectors=\"1\" physical_partition_number=\"0\" start_sector=\"1560577\" />\r\n</data>";
	}

	public static string Configure()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<data>\r\n<configure ZlpAwareHost=\"1\" MaxPayloadSizeToTargetInBytes=\"1048576\" AckRawDataEveryNumPackets=\"1000\" SkipWrite=\"0\" Verbose=\"0\" MaxDigestTableSizeInBytes=\"8192\" AlwaysValidata=\"0\" MemoryName=\"eMMC\" />\r\n</data>";
	}

	public static string CDR()
	{
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<data>\r\n  <read SECTOR_SIZE_IN_BYTES=\"512\" num_partition_sectors=\"2\" physical_partition_number=\"2\" start_sector=\"34\" />\r\n</data>";
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
