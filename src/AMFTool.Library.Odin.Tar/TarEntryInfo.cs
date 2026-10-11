namespace AMFTool.Library.Odin.Tar;

public class TarEntryInfo
{
	public int Index { get; set; }

	public string Name { get; set; }

	public string Type { get; set; }

	public long Size { get; set; }

	public ulong PacketStartOffset { get; set; }

	public ulong PacketEndOffset { get; set; }

	public long DataStartOffset { get; set; }

	public long DataEndOffset { get; set; }

	public ulong DataOffset { get; set; }

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
