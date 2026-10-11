using System.Runtime.InteropServices;

namespace AMFTool.Library.Mediatek.Download_Agent;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DaGeometry
{
	public int Offset;

	public int Length;

	public uint StartAddr;

	public uint StartOffset;

	public uint SignLen;
}
