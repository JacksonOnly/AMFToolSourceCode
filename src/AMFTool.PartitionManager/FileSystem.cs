namespace AMFTool.PartitionManager;

public interface FileSystem
{
	bool FileExists(string path);

	byte[] ReadFile(string path);
}
