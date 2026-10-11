using AMFTool.PartitionManager;

namespace AMFTool.Ext4;

public class Ext4FileSystemAdapter : FileSystem
{
	private Ext4FileSystem FileSystem;

	public Ext4FileSystemAdapter(Ext4FileSystem fs)
	{
		FileSystem = fs;
	}

	public bool FileExists(string path)
	{
		return FileSystem.FileExists(path);
	}

	public byte[] ReadFile(string path)
	{
		return FileSystem.ReadFile(path);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
