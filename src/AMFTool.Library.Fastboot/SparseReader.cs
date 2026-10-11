using System.IO;
using System.Runtime.InteropServices;

namespace AMFTool.Library.Fastboot;

public static class SparseReader
{
	public static T ReadStruct<T>(BinaryReader reader) where T : struct
	{
		string format = "Unable to read structure of size {0} bytes.";
		int num = Marshal.SizeOf(typeof(T));
		byte[] array = reader.ReadBytes(num);
		if (array.Length != num)
		{
			throw new EndOfStreamException(string.Format(format, num));
		}
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		try
		{
			return (T)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(T));
		}
		finally
		{
			gCHandle.Free();
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
