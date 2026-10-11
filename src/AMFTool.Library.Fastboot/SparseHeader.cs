using System.Runtime.InteropServices;

namespace AMFTool.Library.Fastboot;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SparseHeader
{
	public uint Magic;

	public ushort MajorVersion;

	public ushort MinorVersion;

	public ushort FileHeaderSize;

	public ushort ChunkHeaderSize;

	public uint BlockSize;

	public uint TotalBlocks;

	public uint TotalChunks;

	public uint ImageChecksum;
}
