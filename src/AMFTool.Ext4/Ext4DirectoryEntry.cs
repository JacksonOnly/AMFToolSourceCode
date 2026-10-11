namespace AMFTool.Ext4;

public sealed class Ext4DirectoryEntry
{
	public uint Inode { get; set; }

	public string Name { get; set; }

	public byte FileType { get; set; }

	public bool IsDirectory { get; set; }

	public bool IsFile { get; set; }

	public bool IsSymlink { get; set; }

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
