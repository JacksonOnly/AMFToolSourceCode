using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

[StructLayout(LayoutKind.Explicit)]
public struct GNSS_WB_FFT_Stats_v2
{
	[FieldOffset(0)]
	public byte iVersion;

	[FieldOffset(1)]
	public uint uint_0;

	[FieldOffset(5)]
	public int iGPSFreq;

	[FieldOffset(9)]
	public uint uint_1;

	[FieldOffset(13)]
	public int iGLOFreq;

	[FieldOffset(17)]
	public uint uint_2;

	[FieldOffset(21)]
	public int iBDSFreq;

	[FieldOffset(25)]
	public uint uint_3;

	[FieldOffset(29)]
	public int iGALFreq;

	[FieldOffset(33)]
	public byte iGNSSConfigMask;

	[FieldOffset(34)]
	public int int_0;

	[FieldOffset(38)]
	public int int_1;

	[FieldOffset(42)]
	public int iADCAmpI;

	[FieldOffset(46)]
	public int iADCAmpQ;
}
