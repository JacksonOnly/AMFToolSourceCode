using System.Runtime.InteropServices;

public static class Lz4Decompresso
{
	[DllImport("Data\\bin\\lz4.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int LZ4_hadikit_iReverse_decompress(byte[] src, byte[] dst, int isize);

	public static int lz4_hadikit_decompress(byte[] src, ref byte[] dst)
	{
		return LZ4_hadikit_iReverse_decompress(src, dst, src.Length);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
