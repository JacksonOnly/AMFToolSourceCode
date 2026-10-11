namespace AMFTool.Ext4;

public sealed class Ext4Stat
{
	public string Path { get; set; }

	public uint Inode { get; set; }

	public long Size { get; set; }

	public bool IsDirectory { get; set; }

	public bool IsFile { get; set; }

	public bool IsSymlink { get; set; }

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
