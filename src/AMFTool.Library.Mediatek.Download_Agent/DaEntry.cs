using System.Runtime.InteropServices;

namespace AMFTool.Library.Mediatek.Download_Agent;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DaEntry
{
	public ushort Magic;

	public ushort HwCode;

	public ushort HwSub;

	public ushort HwVer;

	public ushort SwVer;

	public ushort Unknown;

	public ushort PageSize;

	public ushort Reserved;

	public ushort RegionIndex;

	public ushort RegionCount;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
	public DaGeometry[] MGeometries;
}
