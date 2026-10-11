using System.Runtime.InteropServices;

namespace Restored.Namespace11;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct Struct3
{
	public uint field_01;

	public uint field_02;

	public uint field_03;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	public byte[] field_04;

	public uint field_05;

	public uint field_06;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public byte[] field_07;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public byte[] field_08;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public byte[] field_09;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public byte[] field_10;

	public ushort field_11;

	public ushort field_12;

	public ushort field_13;

	public static readonly int field_14 = Marshal.SizeOf(typeof(Struct3));

	public static Struct3 method_01()
	{
		return new Struct3
		{
			field_04 = new byte[8],
			field_07 = new byte[16],
			field_08 = new byte[16],
			field_09 = new byte[16],
			field_10 = new byte[16]
		};
	}

	public static int method_02(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
