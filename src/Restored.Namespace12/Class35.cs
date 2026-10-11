using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AMFTool.Library.Hisilicon;

namespace Restored.Namespace12;

internal static class Class35
{
	public const int field_01 = 2;

	public static readonly UpdateCrc16 field_02 = new UpdateCrc16(ushort.MaxValue, 33800);

	public static bool method_01<T>(BinaryReader FEE8F79E, out T FA3EFF8A)
	{
		int result = 1;
		int num = Marshal.SizeOf(typeof(T));
		byte[] array = FEE8F79E.ReadBytes(num);
		if (array.Length == 0 || array.Length != num)
		{
			FA3EFF8A = default(T);
			return false;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		try
		{
			Marshal.Copy(array, 0, intPtr, num);
			FA3EFF8A = (T)Marshal.PtrToStructure(intPtr, typeof(T));
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		return (byte)result != 0;
	}

	public static bool method_02<T>(T DA75277B, out byte[] A178A9DF)
	{
		int result = 1;
		int num = Marshal.SizeOf(typeof(T));
		A178A9DF = new byte[num];
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		try
		{
			Marshal.StructureToPtr(DA75277B, intPtr, fDeleteOld: true);
			Marshal.Copy(intPtr, A178A9DF, 0, num);
			return (byte)result != 0;
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public static void method_03(string AC244709, byte[] CFCAE7FE)
	{
		for (int i = 0; i < CFCAE7FE.Length; i++)
		{
			CFCAE7FE[i] = 0;
		}
		int length = Math.Min(AC244709.Length, CFCAE7FE.Length);
		Array.Copy(Encoding.ASCII.GetBytes(AC244709.ToCharArray(0, length)), CFCAE7FE, length);
	}

	public static string method_04(byte[] E34C70C3)
	{
		int num = Array.FindIndex(E34C70C3, (byte b) => b == 0);
		if (num != -1)
		{
			return Encoding.ASCII.GetString(E34C70C3, 0, num);
		}
		num = E34C70C3.Length;
		return Encoding.ASCII.GetString(E34C70C3, 0, num);
	}

	public static uint method_05(UpdateEntry AF1BD36F)
	{
		return 4 - (AF1BD36F.HeaderSize + AF1BD36F.FileSize) % 4;
	}

	public static int method_06(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
