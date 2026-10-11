namespace AMFTool.Library.Fastboot;

public class SparseChunk
{
	public ChunkHeader Header { get; set; }

	public long StartPosition { get; set; }

	public long DataStartPosition { get; set; }

	public int DataSize { get; set; }

	public byte[] Data { get; set; }

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
