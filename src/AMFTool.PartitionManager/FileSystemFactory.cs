using System.IO;
using AMFTool.Erofs;
using AMFTool.Ext4;

namespace AMFTool.PartitionManager;

public static class FileSystemFactory
{
	public static FileSystem Open(Stream stream)
	{
		object obj = null;
		long position = stream.Position;
		FileSystemType fileSystemType = FileSystemDetect.Detect(stream);
		stream.Position = position;
		return fileSystemType switch
		{
			FileSystemType.Ext4 => new Ext4FileSystemAdapter(Ext4FileSystem.Open(stream)), 
			(FileSystemType)2 => new ErofsFileSystemAdapter(ErofsFileSystem.Open(stream)), 
			_ => (FileSystem)obj, 
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
