using System.Runtime.InteropServices;

namespace AMFTool.Library.Mediatek.Download_Agent;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DaHeader
{
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string Title;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
	public string Version;

	public uint Unknown;

	public uint Magic;

	public uint Count;
}
