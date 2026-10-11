using System;
using System.IO;

namespace AMFTool.Library.Lz4;

public static class SizeCalculator
{
	public static long CalculateDecodedSize(Stream source)
	{
		string message = "Unsupported LZ4 data. Expected a valid LZ4 frame.";
		long num = 0L;
		int num3;
		int num5;
		bool flag;
		if (flag = ReadUInt32LE(source, out var value))
		{
			if (value == 407708164)
			{
				int num2 = 75670614;
				while (true)
				{
					_ = num2 ^ 0x50A565EF;
					num = checked(num + ParseFrame(source));
					if (flag = ReadUInt32LE(source, out value))
					{
						if (value != 407708164)
						{
							break;
						}
						num2 = 75670614;
						continue;
					}
					return num;
				}
				if (IsSkippableFrame(value))
				{
					num3 = 100419884;
					goto IL_00d8;
				}
				throw new InvalidDataException(message);
			}
			if (IsSkippableFrame(value))
			{
				int num4 = 100419884;
				while (true)
				{
					_ = num4 ^ 0x50A565EF;
					SkipBytes(source, ReadUInt32LE(source));
					if (flag = ReadUInt32LE(source, out value))
					{
						if (value == 407708164)
						{
							break;
						}
						if (IsSkippableFrame(value))
						{
							num4 = 100419884;
							continue;
						}
						throw new InvalidDataException(message);
					}
					return num;
				}
				num5 = 75670614;
				goto IL_0100;
			}
			throw new InvalidDataException(message);
		}
		return num;
		IL_0100:
		_ = num5 ^ 0x50A565EF;
		num = checked(num + ParseFrame(source));
		flag = ReadUInt32LE(source, out value);
		goto IL_011a;
		IL_00d8:
		_ = num3 ^ 0x50A565EF;
		SkipBytes(source, ReadUInt32LE(source));
		flag = ReadUInt32LE(source, out value);
		goto IL_011a;
		IL_011a:
		if (flag)
		{
			if (value == 407708164)
			{
				num5 = 75670614;
				goto IL_0100;
			}
			if (IsSkippableFrame(value))
			{
				num3 = 100419884;
				goto IL_00d8;
			}
			throw new InvalidDataException(message);
		}
		return num;
	}

	private static bool IsSkippableFrame(uint frameMagicValue)
	{
		if (frameMagicValue >= 407710288)
		{
			return frameMagicValue <= 407710303;
		}
		return false;
	}

	private static long ParseFrame(Stream source)
	{
		string message = "Unexpected End of stream while reading LZ4 frame header.";
		string message2 = "Unsupported LZ4 frame version.";
		long field_ = Class38.field_3544;
		string message3 = "LZ4 content size exceeds Int64 range.";
		string message4 = "Unexpected End of stream while reading LZ4 header checksum.";
		string message5 = "Invalid LZ4 frame header checksum.";
		string message6 = "LZ4 dictionary-id frames are not supported by this internal parser.";
		int num = source.ReadByte();
		int num2 = source.ReadByte();
		if (num >= 0 && num2 >= 0)
		{
			byte b = (byte)num;
			byte b2 = (byte)num2;
			if ((b & 0xC0) == 64)
			{
				bool hasBlockChecksum = (b & 0x10) != 0;
				bool flag = (b & 8) != 0;
				bool flag2 = (b & 4) != 0;
				bool flag3 = (b & 1) != 0;
				int blockSizeCode = (b2 >> 4) & 7;
				int maxBlockSize = DecodeMaxBlockSize(blockSizeCode);
				byte[] array = new byte[15];
				int num3 = 0;
				num3 = 1;
				array[0] = b;
				num3 = 2;
				array[1] = b2;
				long num4 = -1L;
				if (flag)
				{
					ulong num5 = ReadUInt64LE(source);
					if (num5 > (ulong)field_)
					{
						throw new InvalidDataException(message3);
					}
					num4 = (long)num5;
					AppendUInt64LE(array, ref num3, num5);
				}
				if (flag3)
				{
					uint value = ReadUInt32LE(source);
					AppendUInt32LE(array, ref num3, value);
				}
				int num6 = source.ReadByte();
				if (num6 < 0)
				{
					throw new EndOfStreamException(message4);
				}
				byte b3 = (byte)num6;
				byte b4 = ComputeHeaderChecksum(array, num3);
				if (b4 == b3)
				{
					if (!flag3)
					{
						long result;
						if (flag)
						{
							result = num4;
							if (source.CanSeek)
							{
								source.Position = source.Length;
								return result;
							}
							SkipBlocks(source, hasBlockChecksum, maxBlockSize);
						}
						else
						{
							result = CountBlocks(source, hasBlockChecksum, maxBlockSize);
						}
						if (flag2)
						{
							SkipBytes(source, 4L);
						}
						return result;
					}
					throw new NotSupportedException(message6);
				}
				throw new InvalidDataException(message5);
			}
			throw new InvalidDataException(message2);
		}
		throw new EndOfStreamException(message);
	}

	private static void SkipBlocks(Stream source, bool hasBlockChecksum, int maxBlockSize)
	{
		string message = "LZ4 block size exceeds frame block size setting.";
		uint num = ReadUInt32LE(source);
		if (num == 0)
		{
			return;
		}
		int num2 = (int)(num & 0x7FFFFFFF);
		if (num2 <= maxBlockSize)
		{
			SkipBytes(source, num2);
			int num3 = 659240541;
			while (true)
			{
				_ = num3 ^ -711087884;
				if (hasBlockChecksum)
				{
					SkipBytes(source, 4L);
				}
				num = ReadUInt32LE(source);
				if (num != 0)
				{
					num2 = (int)(num & 0x7FFFFFFF);
					if (num2 <= maxBlockSize)
					{
						SkipBytes(source, num2);
						num3 = 659240541;
						continue;
					}
					throw new InvalidDataException(message);
				}
				break;
			}
			return;
		}
		throw new InvalidDataException(message);
	}

	private static long CountBlocks(Stream source, bool hasBlockChecksum, int maxBlockSize)
	{
		string message = "LZ4 block size exceeds frame block size setting.";
		long num = 0L;
		byte[] array = Array.Empty<byte>();
		while (true)
		{
			uint num2 = ReadUInt32LE(source);
			if (num2 == 0)
			{
				break;
			}
			bool flag = (num2 & 0x80000000u) != 0;
			int num3 = (int)(num2 & 0x7FFFFFFF);
			checked
			{
				if (num3 <= maxBlockSize)
				{
					if (!flag)
					{
						if (array.Length < num3)
						{
							array = new byte[num3];
						}
						ReadExactly(source, array, num3);
						num += CalculateBlockDecodedSize(array, num3);
					}
					else
					{
						SkipBytes(source, num3);
						num += num3;
					}
					if (hasBlockChecksum)
					{
						SkipBytes(source, 4L);
					}
					continue;
				}
				throw new InvalidDataException(message);
			}
		}
		return num;
	}

	private static int CalculateBlockDecodedSize(byte[] compressedBlock, int blockLen)
	{
		string message = "Invalid LZ4 block. Literal Length exceeds input Length.";
		string message2 = "Invalid LZ4 block. Literal section exceeds block size.";
		string message3 = "Invalid LZ4 block. Match Offset is incomplete.";
		string message4 = "Invalid LZ4 block. Match Offset cannot be zero.";
		string message5 = "Invalid LZ4 block. Match Length exceeds input Length.";
		int num = 0;
		int num2 = 0;
		while (num < blockLen)
		{
			byte b = compressedBlock[num++];
			int num3 = b >> 4;
			if (num3 == 15)
			{
				int num4;
				do
				{
					if (num < blockLen)
					{
						num4 = compressedBlock[num++];
						num3 += num4;
						continue;
					}
					throw new InvalidDataException(message);
				}
				while (num4 == 255);
			}
			if (num + num3 > blockLen)
			{
				throw new InvalidDataException(message2);
			}
			num += num3;
			num2 = checked(num2 + num3);
			if (num >= blockLen)
			{
				break;
			}
			if (num + 2 <= blockLen)
			{
				int num5 = compressedBlock[num] | (compressedBlock[num + 1] << 8);
				num += 2;
				if (num5 != 0)
				{
					int num6 = b & 0xF;
					if (num6 == 15)
					{
						int num7;
						do
						{
							if (num >= blockLen)
							{
								throw new InvalidDataException(message5);
							}
							num7 = compressedBlock[num++];
							num6 += num7;
						}
						while (num7 == 255);
					}
					num6 += 4;
					num2 = checked(num2 + num6);
					continue;
				}
				throw new InvalidDataException(message4);
			}
			throw new InvalidDataException(message3);
		}
		return num2;
	}

	private static int DecodeMaxBlockSize(int blockSizeCode)
	{
		return blockSizeCode switch
		{
			4 => 65536, 
			5 => 262144, 
			6 => 1048576, 
			7 => 4194304, 
			_ => 65536, 
		};
	}

	private static byte ComputeHeaderChecksum(byte[] descriptorBuffer, int descriptorLen)
	{
		uint num = XxHash32(descriptorBuffer, 0, descriptorLen, 0u);
		return (byte)(num >> 8);
	}

	private static uint XxHash32(byte[] sourceBytes, int startOff, int byteLen, uint seed)
	{
		int num = startOff;
		int num2 = startOff + byteLen;
		uint num3 = seed + 374761393 + (uint)byteLen;
		bool flag;
		if (num + 4 > num2)
		{
			flag = num < num2;
			while (flag)
			{
				num3 += (uint)(sourceBytes[num++] * 374761393);
				num3 = RotateLeft(num3, 11) * 2654435761u;
				flag = num < num2;
			}
			num3 ^= num3 >> 15;
			num3 *= 2246822519u;
			num3 ^= num3 >> 13;
			num3 *= 3266489917u;
			return num3 ^ (num3 >> 16);
		}
		num3 += (uint)((int)ReadUInt32LE(sourceBytes, num) * -1028477379);
		num3 = RotateLeft(num3, 17) * 668265263;
		int num4 = 685069794;
		while (true)
		{
			_ = num4 ^ -259010140;
			num += 4;
			if (num + 4 > num2)
			{
				break;
			}
			num3 += (uint)((int)ReadUInt32LE(sourceBytes, num) * -1028477379);
			num3 = RotateLeft(num3, 17) * 668265263;
			num4 = 685069794;
		}
		flag = num < num2;
		while (flag)
		{
			num3 += (uint)(sourceBytes[num++] * 374761393);
			num3 = RotateLeft(num3, 11) * 2654435761u;
			flag = num < num2;
		}
		num3 ^= num3 >> 15;
		num3 *= 2246822519u;
		num3 ^= num3 >> 13;
		num3 *= 3266489917u;
		return num3 ^ (num3 >> 16);
	}

	private static uint ReadUInt32LE(byte[] sourceBytes, int startOff)
	{
		return Binary.ReadU32LE(sourceBytes, startOff);
	}

	private static uint RotateLeft(uint value, int bitCount)
	{
		return (value << bitCount) | (value >> 32 - bitCount);
	}

	private static void AppendUInt64LE(byte[] destinationBytes, ref int offset, ulong value)
	{
		Binary.WriteU64LE(destinationBytes, offset, value);
		offset += 8;
	}

	private static void AppendUInt32LE(byte[] destinationBytes, ref int offset, uint value)
	{
		Binary.WriteU32LE(destinationBytes, offset, value);
		offset += 4;
	}

	private static bool ReadUInt32LE(Stream stream, out uint value)
	{
		string message = "Unexpected End of stream while reading UInt32.";
		byte[] array = new byte[4];
		switch (ReadUpTo(stream, array, 4))
		{
		case 4:
			value = Binary.ReadU32LE(array);
			return true;
		default:
			throw new EndOfStreamException(message);
		case 0:
			value = 0u;
			return false;
		}
	}

	private static uint ReadUInt32LE(Stream stream)
	{
		byte[] array = new byte[4];
		ReadExactly(stream, array, 4);
		return Binary.ReadU32LE(array);
	}

	private static ulong ReadUInt64LE(Stream stream)
	{
		byte[] array = new byte[8];
		ReadExactly(stream, array, 8);
		return Binary.ReadU64LE(array);
	}

	private static int ReadUpTo(Stream stream, byte[] buffer, int byteCount)
	{
		int num = 0;
		if (0 >= byteCount)
		{
			int num2 = 0;
			return 0;
		}
		int num3 = stream.Read(buffer, 0, byteCount);
		if (num3 <= 0)
		{
			int num2 = 0;
			return 0;
		}
		num = 0 + num3;
		int num4 = 1631358682;
		while (true)
		{
			_ = num4 ^ 0x44AAD003;
			if (num >= byteCount)
			{
				return num;
			}
			num3 = stream.Read(buffer, num, byteCount - num);
			if (num3 <= 0)
			{
				break;
			}
			num += num3;
			num4 = 1631358682;
		}
		return num;
	}

	private static void ReadExactly(Stream stream, byte[] buffer, int byteCount)
	{
		string message = "Unexpected End of stream.";
		int num = 0;
		if (0 >= byteCount)
		{
			return;
		}
		int num2 = stream.Read(buffer, 0, byteCount);
		if (num2 > 0)
		{
			num = 0 + num2;
			int num3 = 1674800169;
			while (true)
			{
				_ = num3 ^ -748989100;
				if (num < byteCount)
				{
					num2 = stream.Read(buffer, num, byteCount - num);
					if (num2 > 0)
					{
						num += num2;
						num3 = 1674800169;
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

	private static void SkipBytes(Stream stream, long byteCount)
	{
		string message = "Unexpected End of stream while skipping bytes.";
		byte[] array = new byte[8192];
		long num = byteCount;
		bool flag = num > 0L;
		while (flag)
		{
			int count = (int)Math.Min(array.Length, num);
			int num2 = stream.Read(array, 0, count);
			if (num2 > 0)
			{
				num -= num2;
				flag = num > 0L;
				continue;
			}
			throw new EndOfStreamException(message);
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
