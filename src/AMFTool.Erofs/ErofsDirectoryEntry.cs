namespace AMFTool.Erofs;

public class ErofsDirectoryEntry
{
	public ulong Nid { get; set; }

	public string Name { get; set; }

	public ErofsFileType FileType { get; set; }

	public bool IsDirectory => FileType == ErofsFileType.EROFS_FT_DIR;

	public bool IsFile => FileType == ErofsFileType.RegularFile;

	public bool IsSymlink => FileType == ErofsFileType.Symlink;

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
