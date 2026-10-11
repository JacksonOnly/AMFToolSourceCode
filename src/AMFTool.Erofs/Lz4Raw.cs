using System;
using System.IO;

namespace AMFTool.Erofs;

public static class Lz4Raw
{
	public static int Decompress(byte[] input, int offset, int length, byte[] output, int outoffset, int outmaxsize)
	{
		string message = "lz4: invalid match offset.";
		int num = offset;
		int num2 = offset + length;
		int num3 = outoffset;
		int num4 = outoffset + outmaxsize;
		while (num < num2 && num3 < num4)
		{
			byte b = input[num++];
			int num5 = b >> 4;
			if (num5 == 15)
			{
				while (num < num2)
				{
					byte b2 = input[num++];
					num5 += b2;
					if (b2 == byte.MaxValue)
					{
						continue;
					}
					break;
				}
			}
			if (num5 > 0)
			{
				int num6 = num2 - num;
				if (num5 > num6)
				{
					num5 = num6;
				}
				int num7 = num4 - num3;
				int num8 = ((num5 <= num7) ? num5 : num7);
				Buffer.BlockCopy(input, num, output, num3, num8);
				num += num5;
				num3 += num8;
				if (num3 >= num4)
				{
					break;
				}
			}
			if (num + 2 > num2)
			{
				break;
			}
			int num9 = input[num] | (input[num + 1] << 8);
			num += 2;
			if (num9 <= 0)
			{
				break;
			}
			int num10 = (b & 0xF) + 4;
			if ((b & 0xF) == 15)
			{
				while (num < num2)
				{
					byte b3 = input[num++];
					num10 += b3;
					if (b3 == byte.MaxValue)
					{
						continue;
					}
					break;
				}
			}
			int num11 = num3 - num9;
			if (num11 >= outoffset)
			{
				int num12 = num4 - num3;
				if (num10 > num12)
				{
					num10 = num12;
				}
				for (int i = 0; i < num10; i++)
				{
					output[num3++] = output[num11++];
				}
				continue;
			}
			throw new InvalidDataException(message);
		}
		return num3 - outoffset;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
