namespace AMFTool.Library.Odin.PIT;

public class PitEntry
{
	public long BinaryTypeId { get; set; }

	public long DeviceTypeId { get; set; }

	public long PartitionId { get; set; }

	public long AttributeFlags { get; set; }

	public long UpdateAttributeFlags { get; set; }

	public long BlockSize { get; set; }

	public long BlockCount { get; set; }

	public long FileOffset { get; set; }

	public long FileSize { get; set; }

	public string PartitionName { get; set; }

	public string FlashFileName { get; set; }

	public string FotaFileName { get; set; }

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
