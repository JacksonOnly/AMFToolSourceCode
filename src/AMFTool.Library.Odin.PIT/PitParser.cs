using System;
using System.Collections.Generic;
using System.Text;

namespace AMFTool.Library.Odin.PIT;

public class PitParser
{
	private const int HeaderSize = 28;

	private const int NameSize = 32;

	private double FF592;

	public static List<PitEntry> Parse(byte[] sourceBytes)
	{
		string message = "Invalid PIT data.";
		string message2 = "Invalid PIT Magic.";
		string message3 = "Invalid PIT Entry count.";
		string message4 = "PIT data is truncated.";
		if (sourceBytes == null || sourceBytes.Length < 28)
		{
			throw new ArgumentException(message);
		}
		uint num = Binary.ReadU32LE(sourceBytes);
		if (num != 305436790)
		{
			throw new InvalidOperationException(message2);
		}
		int num2 = Binary.ReadI32LE(sourceBytes, 4);
		if (num2 < 0)
		{
			throw new InvalidOperationException(message3);
		}
		List<PitEntry> list = new List<PitEntry>(num2);
		for (int i = 0; i < num2; i++)
		{
			int num3 = 28 + i * 132;
			if (num3 + 132 > sourceBytes.Length)
			{
				throw new InvalidOperationException(message4);
			}
			PitEntry item = new PitEntry
			{
				BinaryTypeId = Binary.ReadU32LE(sourceBytes, num3),
				DeviceTypeId = Binary.ReadU32LE(sourceBytes, num3 + 4),
				PartitionId = Binary.ReadU32LE(sourceBytes, num3 + 8),
				AttributeFlags = Binary.ReadU32LE(sourceBytes, num3 + 12),
				UpdateAttributeFlags = Binary.ReadU32LE(sourceBytes, num3 + 16),
				BlockSize = Binary.ReadU32LE(sourceBytes, num3 + 20),
				BlockCount = Binary.ReadU32LE(sourceBytes, num3 + 24),
				FileOffset = Binary.ReadU32LE(sourceBytes, num3 + 28),
				FileSize = Binary.ReadU32LE(sourceBytes, num3 + 32),
				PartitionName = ReadAsciiZ(sourceBytes, num3 + 36, 32),
				FlashFileName = ReadAsciiZ(sourceBytes, num3 + 68, 32),
				FotaFileName = ReadAsciiZ(sourceBytes, num3 + 100, 32)
			};
			list.Add(item);
		}
		list.Sort((PitEntry leftEntry, PitEntry rightEntry) => leftEntry.PartitionId.CompareTo(rightEntry.PartitionId));
		return list;
	}

	private static string ReadAsciiZ(byte[] sourceBytes, int startOffset, int maxLength)
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
			return Encoding.ASCII.GetString(sourceBytes, startOffset, num - startOffset);
		}
		goto IL_0011;
		IL_0011:
		flag = sourceBytes[num] != 0;
		goto IL_0018;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
