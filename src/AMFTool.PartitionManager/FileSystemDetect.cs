using System.IO;

namespace AMFTool.PartitionManager;

public static class FileSystemDetect
{
	public static FileSystemType Detect(Stream stream)
	{
		if (stream == null || !stream.CanRead || !stream.CanSeek)
		{
			return (FileSystemType)0;
		}
		long position = stream.Position;
		try
		{
			if (!IsErofs(stream))
			{
				if (!IsExt4(stream))
				{
					if (IsF2Fs(stream))
					{
						return FileSystemType.F2fs;
					}
					return (FileSystemType)0;
				}
				return FileSystemType.Ext4;
			}
			return (FileSystemType)2;
		}
		finally
		{
			try
			{
				stream.Position = position;
			}
			catch
			{
			}
		}
	}

	private static bool IsExt4(Stream stream)
	{
		if (ReadU16(stream, 1080L, out var value))
		{
			return value == 61267;
		}
		return false;
	}

	private static bool IsF2Fs(Stream stream)
	{
		if (ReadU32(stream, 1024L, out var value))
		{
			return value == 4076150800u;
		}
		return false;
	}

	private static bool IsErofs(Stream stream)
	{
		byte[] array = new byte[128];
		if (!ReadAt(stream, 1024L, array, array.Length))
		{
			return false;
		}
		int offset = 0;
		uint num = Binary.ReadU32LE(array, ref offset);
		if (num != 3774210530u)
		{
			return false;
		}
		byte b = array[12];
		int offset2 = 14;
		ushort num2 = Binary.ReadU16LE(array, ref offset2);
		int offset3 = 36;
		uint num3 = Binary.ReadU32LE(array, ref offset3);
		if (b >= 9)
		{
			if (b > 16)
			{
				return false;
			}
			if (num2 != 0)
			{
				if (num3 != 0)
				{
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	private static bool ReadU16(Stream stream, long offset, out ushort value)
	{
		value = 0;
		byte[] buffer = new byte[2];
		if (!ReadAt(stream, offset, buffer, 2))
		{
			return false;
		}
		int offset2 = 0;
		value = Binary.ReadU16LE(buffer, ref offset2);
		return true;
	}

	private static bool ReadU32(Stream stream, long offset, out uint value)
	{
		value = 0u;
		byte[] buffer = new byte[4];
		if (ReadAt(stream, offset, buffer, 4))
		{
			int offset2 = 0;
			value = Binary.ReadU32LE(buffer, ref offset2);
			return true;
		}
		return false;
	}

	private static bool ReadAt(Stream stream, long offset, byte[] buffer, int count)
	{
		int result = 0;
		if (buffer == null || buffer.Length < count)
		{
			return false;
		}
		if (offset < 0L)
		{
			return false;
		}
		try
		{
			stream.Seek(offset, SeekOrigin.Begin);
			int num = 0;
			while (true)
			{
				if (num < count)
				{
					int num2 = stream.Read(buffer, num, count - num);
					if (num2 <= 0)
					{
						break;
					}
					num += num2;
					continue;
				}
				return true;
			}
			return false;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
