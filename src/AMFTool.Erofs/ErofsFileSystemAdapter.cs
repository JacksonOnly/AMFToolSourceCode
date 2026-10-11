using System;
using AMFTool.PartitionManager;

namespace AMFTool.Erofs;

public class ErofsFileSystemAdapter : FileSystem
{
	private readonly ErofsFileSystem fileSystem;

	public ErofsFileSystemAdapter(ErofsFileSystem fileSystem)
	{
		string paramName = "fileSystem";

		this.fileSystem = fileSystem ?? throw new ArgumentNullException(paramName);
	}

	public bool FileExists(string path)
	{
		return fileSystem.FileExists(path);
	}

	public byte[] ReadFile(string path)
	{
		return fileSystem.ReadFile(path);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
