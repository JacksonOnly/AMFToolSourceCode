using AMFTool.Library.Odin.PIT;

namespace AMFTool.Library.Odin.Protocol;

public class PitPatchContext
{
	public byte[] OriginalPitBytes { get; set; }

	public byte[] PatchedPitBytes { get; set; }

	public PitEntry PitPartitionEntry { get; set; }

	public PitEntry PersistentPartitionEntry { get; set; }

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
