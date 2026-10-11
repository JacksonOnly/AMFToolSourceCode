using System.Runtime.InteropServices;

namespace AMFTool.Library.Fastboot;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ChunkHeader
{
	public ushort ChunkType;

	public ushort Reserved1;

	public uint ChunkSize;

	public uint TotalSize;
}
