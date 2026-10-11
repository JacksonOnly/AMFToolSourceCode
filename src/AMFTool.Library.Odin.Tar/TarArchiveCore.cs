using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AMFTool.Library.Odin.Tar;

public static class TarArchiveCore
{
	public static IList<TarEntryInfo> ReadEntriesFromBytes(byte[] tarBytes)
	{
		try
		{
			if (tarBytes != null && tarBytes.Length != 0)
			{
				using MemoryStream readStream = new MemoryStream(tarBytes, writable: false);
				return ReadEntriesFromStream(readStream);
			}
			return new List<TarEntryInfo>();
		}
		catch
		{
			return new List<TarEntryInfo>();
		}
	}

	public static IList<TarEntryInfo> ReadEntries(string tarPath)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(tarPath) && File.Exists(tarPath))
			{
				using FileStream readStream = File.Open(tarPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				return ReadEntriesFromStream(readStream);
			}
			return new List<TarEntryInfo>();
		}
		catch
		{
			return new List<TarEntryInfo>();
		}
	}

	public static byte[] ExtractEntry(string tarPath, TarEntryInfo tarEntry)
	{
		string message = "Unexpected End of TAR file while extracting entry.";
		byte[] array = new byte[1048576];
		long num = tarEntry.Size;
		long num2 = 0L;
		using FileStream fileStream = File.Open(tarPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		using MemoryStream memoryStream = new MemoryStream((int)((tarEntry.Size <= 2147483647L) ? tarEntry.Size : 0));
		fileStream.Position = tarEntry.DataStartOffset;
		while (num > 0L)
		{
			int count = (int)Math.Min(array.Length, num);
			int num3 = fileStream.Read(array, 0, count);
			if (num3 > 0)
			{
				memoryStream.Write(array, 0, num3);
				num -= num3;
				num2 += num3;
				UI.Progresbar1(num2, tarEntry.Size);
				continue;
			}
			throw new EndOfStreamException(message);
		}
		return memoryStream.ToArray();
	}

	private static IList<TarEntryInfo> ReadEntriesFromStream(Stream readStream)
	{
		string text = "Directory";
		List<TarEntryInfo> list = new List<TarEntryInfo>();
		byte[] array = new byte[512];
		while (readStream.Position + 512L <= readStream.Length)
		{
			ReadExactly(readStream, array, 512);
			if (IsZeroTarBlock(array))
			{
				break;
			}
			long num = readStream.Position - 512L;
			string text2 = ReadAsciiNullTerminated(array, 0, 100);
			if (string.IsNullOrEmpty(text2))
			{
				break;
			}
			long num2 = ParseTarSize(array, 124, 12);
			if (num2 < 0L)
			{
				num2 = 0L;
			}
			long position = readStream.Position;
			long dataEndOffset = ((num2 > 0L) ? (position + num2 - 1L) : position);
			long num3 = checked(num + 512L + AlignToTarBlock(num2));
			list.Add(new TarEntryInfo
			{
				Index = list.Count,
				Name = text2.Trim(),
				Type = (IsDirectoryEntry(array, text2) ? text : string.Empty),
				Size = num2,
				PacketStartOffset = (ulong)num,
				PacketEndOffset = (ulong)num3,
				DataStartOffset = position,
				DataEndOffset = dataEndOffset,
				DataOffset = 0uL
			});
			readStream.Position = num3;
		}
		return list;
	}

	private static bool IsZeroTarBlock(byte[] block)
	{
		for (int i = 0; i < block.Length; i++)
		{
			if (block[i] != 0)
			{
				return false;
			}
		}
		return true;
	}

	private static string ReadAsciiNullTerminated(byte[] headerBuffer, int startOffset, int maxLength)
	{
		int num = startOffset;
		int num2 = startOffset + maxLength;
		if (num < num2)
		{
			goto IL_0011;
		}
		bool flag = false;
		goto IL_0018;
		IL_0018:
		while (true)
		{
			if (flag)
			{
				num++;
				if (num < num2)
				{
					break;
				}
				flag = false;
				continue;
			}
			return Encoding.ASCII.GetString(headerBuffer, startOffset, num - startOffset);
		}
		goto IL_0011;
		IL_0011:
		flag = headerBuffer[num] != 0;
		goto IL_0018;
	}

	private static bool IsDirectoryEntry(byte[] headerBuffer, string fileName)
	{
		string value = "/";
		if (headerBuffer[156] != 53)
		{
			return fileName.EndsWith(value, StringComparison.Ordinal);
		}
		return true;
	}

	private static long ParseTarSize(byte[] headerBuffer, int startOffset, int maxLength)
	{
		if ((headerBuffer[startOffset] & 0x80) == 0)
		{
			return ParseOctalSize(headerBuffer, startOffset, maxLength);
		}
		return ParseBase256Size(headerBuffer, startOffset, maxLength);
	}

	private static long ParseOctalSize(byte[] headerBuffer, int startOffset, int maxLength)
	{
		long num = 0L;
		bool flag = false;
		int num2 = startOffset;
		if (num2 >= startOffset + maxLength)
		{
			return num;
		}
		byte b = headerBuffer[num2];
		int num4;
		bool flag2;
		if (flag)
		{
			flag2 = false;
			if (b < 48 || b > 55)
			{
				return num;
			}
			flag = true;
			num = checked((num << 3) + (b - 48));
			int num3 = 1708092898;
			while (true)
			{
				_ = num3 ^ 0x4D90B96E;
				num2++;
				if (num2 >= startOffset + maxLength)
				{
					return num;
				}
				b = headerBuffer[num2];
				if (!flag)
				{
					break;
				}
				flag2 = false;
				if (b < 48 || b > 55)
				{
					return num;
				}
				flag = true;
				num = checked((num << 3) + (b - 48));
				num3 = 1708092898;
			}
			num4 = 2144561975;
			goto IL_0133;
		}
		int num5 = 2144561975;
		while (true)
		{
			_ = num5 ^ 0x4D90B96E;
			flag2 = b == 0 || b == 32;
			while (flag2)
			{
				num2++;
				if (num2 >= startOffset + maxLength)
				{
					return num;
				}
				b = headerBuffer[num2];
				if (flag)
				{
					flag2 = false;
					continue;
				}
				goto IL_00f2;
			}
			break;
			IL_00f2:
			num5 = 2144561975;
		}
		if (b < 48 || b > 55)
		{
			return num;
		}
		flag = true;
		num = checked((num << 3) + (b - 48));
		int num6 = 1708092898;
		goto IL_00e4;
		IL_0133:
		_ = num4 ^ 0x4D90B96E;
		flag2 = b == 0 || b == 32;
		goto IL_0158;
		IL_0158:
		if (flag2)
		{
			num2++;
			goto IL_00fc;
		}
		if (b >= 48 && b <= 55)
		{
			flag = true;
			num = checked((num << 3) + (b - 48));
			num6 = 1708092898;
			goto IL_00e4;
		}
		return num;
		IL_00e4:
		_ = num6 ^ 0x4D90B96E;
		num2++;
		goto IL_00fc;
		IL_00fc:
		if (num2 >= startOffset + maxLength)
		{
			return num;
		}
		b = headerBuffer[num2];
		if (flag)
		{
			flag2 = false;
			goto IL_0158;
		}
		num4 = 2144561975;
		goto IL_0133;
	}

	private static long ParseBase256Size(byte[] headerBuffer, int startOffset, int maxLength)
	{
		string message = "Negative base-256 TAR size is not supported.";
		if ((headerBuffer[startOffset] & 0x40) == 0)
		{
			long num = headerBuffer[startOffset] & 0x3F;
			int num2 = startOffset + 1;
			if (num2 >= startOffset + maxLength)
			{
				return num;
			}
			num = (num << 8) | headerBuffer[num2];
			num2++;
			int num3 = 1123724686;
			while (true)
			{
				_ = num3 ^ 0x4B133E41;
				if (num2 >= startOffset + maxLength)
				{
					break;
				}
				num = (num << 8) | headerBuffer[num2];
				num2++;
				num3 = 1123724686;
			}
			return num;
		}
		throw new InvalidDataException(message);
	}

	private static long AlignToTarBlock(long dataLength)
	{
		if (dataLength > 0L)
		{
			long num = dataLength % 512L;
			if (num != 0L)
			{
				return dataLength + (512L - num);
			}
			return dataLength;
		}
		long num2 = 0L;
		return 0L;
	}

	private static void ReadExactly(Stream readStream, byte[] targetBuffer, int byteCount)
	{
		string message = "Unexpected End of Stream.";
		int num = 0;
		if (0 >= byteCount)
		{
			return;
		}
		int num2 = readStream.Read(targetBuffer, 0, byteCount);
		if (num2 > 0)
		{
			num = 0 + num2;
			int num3 = 441622682;
			while (true)
			{
				_ = num3 ^ -926768450;
				if (num < byteCount)
				{
					num2 = readStream.Read(targetBuffer, num, byteCount - num);
					if (num2 > 0)
					{
						num += num2;
						num3 = 441622682;
						continue;
					}
					throw new EndOfStreamException(message);
				}
				break;
			}
			return;
		}
		throw new EndOfStreamException(message);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
