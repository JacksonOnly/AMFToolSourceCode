namespace AMFTool.Library.Mediatek.Download_Agent;

public class StorageInfo
{
	public string StorageType { get; set; }

	public long BlockSize { get; set; }

	public string Cid { get; set; }

	public long Lua0Size { get; set; }

	public long Lua1Size { get; set; }

	public long Lua2Size { get; set; }

	public long Lua3Size { get; set; }

	public string RamSize { get; set; }

	public string VendorId { get; set; }

	public string FwVer { get; set; }

	public string BatteryVoltage { get; set; }

	public string RandomId { get; set; }

	public string ProductId { get; set; }

	public long Boot1Size { get; set; }

	public long Boot2Size { get; set; }

	public long RpmbSize { get; set; }

	public long UserSize { get; set; }

	public long Gp1Size { get; set; }

	public long Gp2Size { get; set; }

	public long Gp3Size { get; set; }

	public long Gp4Size { get; set; }

	public long PageSize { get; set; }

	public long SpareSize { get; set; }

	public long TotalSize { get; set; }

	public long PageParitySize { get; set; }

	public string SubType { get; set; }

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
