using System;
using System.IO;

namespace AMFTool.Library.Lz4;

public class Lz4DecodingStream : Stream
{
	private const uint SkipMagicStart = 407710288u;

	private const uint BlockLengthMask = 2147483647u;

	private const uint XxhashPrime1 = 2654435761u;

	private const uint XxhashPrime3 = 3266489917u;

	private const uint XxhashPrime5 = 374761393u;

	private Stream SourceStream;

	private bool LeaveSourceOpen;

	private byte[] HistoryWindow;

	private int HistoryLen;

	private byte[] OutputBuffer;

	private int OutputOffset;

	private int OutputAvailable;

	private byte[] WorkBuffer;

	private byte[] CompressedBuffer;

	private bool InFrame;

	private bool HasBlockChecksum;

	private bool HasContentChecksum;

	private bool IndependentBlocks;

	private int MaxBlockPayload;

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public Lz4DecodingStream(Stream input, bool keepOpen = false)
	{
		string paramName = "input";
		HistoryWindow = new byte[65536];
		OutputBuffer = Array.Empty<byte>();
		WorkBuffer = Array.Empty<byte>();
		CompressedBuffer = Array.Empty<byte>();

		SourceStream = input ?? throw new ArgumentNullException(paramName);
		LeaveSourceOpen = keepOpen;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num;
		if (count <= 0)
		{
			num = 0;
			return 0;
		}
		int num2 = 0;
		if (count <= 0)
		{
			num = 0;
			return 0;
		}
		int num5;
		int num3;
		if (OutputAvailable > 0)
		{
			num3 = Math.Min(count, OutputAvailable);
			OutputBuffer.AsSpan(OutputOffset, num3).CopyTo(buffer.AsSpan(offset, num3));
			OutputOffset += num3;
			OutputAvailable -= num3;
			offset += num3;
			count -= num3;
			num2 = 0 + num3;
			int num4 = 687527184;
			while (true)
			{
				_ = num4 ^ 0x5110B8FF;
				if (count <= 0)
				{
					return num2;
				}
				if (OutputAvailable <= 0)
				{
					break;
				}
				num3 = Math.Min(count, OutputAvailable);
				OutputBuffer.AsSpan(OutputOffset, num3).CopyTo(buffer.AsSpan(offset, num3));
				OutputOffset += num3;
				OutputAvailable -= num3;
				offset += num3;
				count -= num3;
				num2 += num3;
				num4 = 687527184;
			}
			num5 = 1583505084;
			goto IL_0191;
		}
		int num6;
		if (ReadNextChunk())
		{
			num3 = Math.Min(count, OutputAvailable);
			OutputBuffer.AsSpan(OutputOffset, num3).CopyTo(buffer.AsSpan(offset, num3));
			OutputOffset += num3;
			OutputAvailable -= num3;
			offset += num3;
			count -= num3;
			num2 = 0 + num3;
			num6 = 687527184;
			goto IL_017a;
		}
		num = 0;
		return 0;
		IL_017a:
		_ = num6 ^ 0x5110B8FF;
		if (count <= 0)
		{
			return num2;
		}
		if (OutputAvailable > 0)
		{
			num3 = Math.Min(count, OutputAvailable);
			goto IL_01da;
		}
		num5 = 1583505084;
		goto IL_0191;
		IL_0191:
		_ = num5 ^ 0x5110B8FF;
		if (ReadNextChunk())
		{
			num3 = Math.Min(count, OutputAvailable);
			goto IL_01da;
		}
		return num2;
		IL_01da:
		OutputBuffer.AsSpan(OutputOffset, num3).CopyTo(buffer.AsSpan(offset, num3));
		OutputOffset += num3;
		OutputAvailable -= num3;
		offset += num3;
		count -= num3;
		num2 += num3;
		num6 = 687527184;
		goto IL_017a;
	}

	private bool ReadNextChunk()
	{
		string message = "Unsupported LZ4 frame.";
		string message2 = "LZ4 block size exceeds frame limit.";
		while (true)
		{
			if (InFrame)
			{
				uint num = ReadUInt32LE(SourceStream);
				if (num != 0)
				{
					bool flag = (num & 0x80000000u) != 0;
					int num2 = (int)(num & 0x7FFFFFFF);
					if (num2 <= MaxBlockPayload)
					{
						EnsureCompressedCapacity(num2);
						ReadExactly(SourceStream, CompressedBuffer, num2);
						if (HasBlockChecksum)
						{
							SkipBytes(SourceStream, 4L);
						}
						if (!flag)
						{
							int num3 = DecompressBlock(CompressedBuffer, num2, IndependentBlocks);
							OutputOffset = 0;
							OutputAvailable = num3;
							return num3 > 0;
						}
						EnsureOutputCapacity(num2);
						CompressedBuffer.AsSpan(0, num2).CopyTo(OutputBuffer.AsSpan(0, num2));
						OutputOffset = 0;
						OutputAvailable = num2;
						UpdateHistory(OutputBuffer, 0, num2, IndependentBlocks);
						return true;
					}
					throw new InvalidDataException(message2);
				}
				if (HasContentChecksum)
				{
					SkipBytes(SourceStream, 4L);
				}
				InFrame = false;
				continue;
			}
			if (!ReadUInt32LE(SourceStream, out var value))
			{
				return false;
			}
			if (value != 407708164)
			{
				if (value < 407710288 || value > 407710303)
				{
					break;
				}
				SkipBytes(SourceStream, ReadUInt32LE(SourceStream));
			}
			else
			{
				ReadFrameHeader();
				InFrame = true;
			}
		}
		throw new InvalidDataException(message);
	}

	private void ReadFrameHeader()
	{
		string message = "Unexpected End of stream while reading LZ4 frame header.";
		string message2 = "Unsupported LZ4 frame version.";
		string message3 = "LZ4 dictionary-id frames are not supported.";
		string message4 = "Unexpected End of stream while reading LZ4 header checksum.";
		string message5 = "Invalid LZ4 frame header checksum.";
		int num = SourceStream.ReadByte();
		int num2 = SourceStream.ReadByte();
		if (num >= 0 && num2 >= 0)
		{
			byte b = (byte)num;
			byte b2 = (byte)num2;
			if ((b & 0xC0) != 64)
			{
				throw new InvalidDataException(message2);
			}
			IndependentBlocks = (b & 0x20) != 0;
			HasBlockChecksum = (b & 0x10) != 0;
			bool flag = (b & 8) != 0;
			HasContentChecksum = (b & 4) != 0;
			bool flag2 = (b & 1) != 0;
			byte[] array = new byte[15]
			{
				b, b2, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0
			};
			int num3;
			if (!flag)
			{
				if (flag2)
				{
					uint value = ReadUInt32LE(SourceStream);
					Binary.WriteU32LE(array, 2, value);
					throw new NotSupportedException(message3);
				}
				num3 = SourceStream.ReadByte();
				if (num3 >= 0)
				{
					byte b3 = (byte)num3;
					byte b4 = ComputeHeaderChecksum(array, 2);
					if (b4 != b3)
					{
						throw new InvalidDataException(message5);
					}
					int blockSizeCode = (b2 >> 4) & 7;
					MaxBlockPayload = DecodeMaxBlockSize(blockSizeCode);
					if (IndependentBlocks)
					{
						HistoryLen = 0;
					}
					return;
				}
				throw new EndOfStreamException(message4);
			}
			ulong value2 = ReadUInt64LE(SourceStream);
			Binary.WriteU64LE(array, 2, value2);
			if (flag2)
			{
				uint value = ReadUInt32LE(SourceStream);
				Binary.WriteU32LE(array, 10, value);
				throw new NotSupportedException(message3);
			}
			num3 = SourceStream.ReadByte();
			if (num3 >= 0)
			{
				byte b3 = (byte)num3;
				byte b4 = ComputeHeaderChecksum(array, 10);
				if (b4 != b3)
				{
					throw new InvalidDataException(message5);
				}
				int blockSizeCode = (b2 >> 4) & 7;
				MaxBlockPayload = DecodeMaxBlockSize(blockSizeCode);
				if (IndependentBlocks)
				{
					HistoryLen = 0;
				}
				return;
			}
			throw new EndOfStreamException(message4);
		}
		throw new EndOfStreamException(message);
	}

	private int DecompressBlock(byte[] compressedInput, int inputLen, bool independent)
	{
		string message = "Invalid LZ4 block. Literal Length exceeds input.";
		string message2 = "Invalid LZ4 block. Literal bytes out of range.";
		string message3 = "Invalid LZ4 block. Missing match Offset.";
		string message4 = "Invalid LZ4 block. Offset cannot be zero.";
		string message5 = "Invalid LZ4 block. Match Length exceeds input.";
		string message6 = "Invalid LZ4 block. Match points before available history.";
		int num = ((!independent) ? HistoryLen : 0);
		EnsureWorkCapacity(num + MaxBlockPayload + 8);
		if (num > 0)
		{
			HistoryWindow.AsSpan(0, num).CopyTo(WorkBuffer.AsSpan(0, num));
		}
		int num2 = 0;
		int num3 = num;
		while (num2 < inputLen)
		{
			byte b = compressedInput[num2++];
			int num4 = b >> 4;
			if (num4 == 15)
			{
				int num5;
				do
				{
					if (num2 >= inputLen)
					{
						throw new InvalidDataException(message);
					}
					num5 = compressedInput[num2++];
					num4 += num5;
				}
				while (num5 == 255);
			}
			if (num2 + num4 > inputLen)
			{
				throw new InvalidDataException(message2);
			}
			compressedInput.AsSpan(num2, num4).CopyTo(WorkBuffer.AsSpan(num3, num4));
			num2 += num4;
			num3 += num4;
			if (num2 >= inputLen)
			{
				break;
			}
			if (num2 + 2 > inputLen)
			{
				throw new InvalidDataException(message3);
			}
			int num6 = compressedInput[num2] | (compressedInput[num2 + 1] << 8);
			num2 += 2;
			if (num6 <= 0)
			{
				throw new InvalidDataException(message4);
			}
			int num7 = b & 0xF;
			if (num7 == 15)
			{
				int num8;
				do
				{
					if (num2 >= inputLen)
					{
						throw new InvalidDataException(message5);
					}
					num8 = compressedInput[num2++];
					num7 += num8;
				}
				while (num8 == 255);
			}
			num7 += 4;
			int num9 = num3 - num6;
			if (num9 >= 0)
			{
				for (int i = 0; i < num7; i++)
				{
					WorkBuffer[num3 + i] = WorkBuffer[num9 + i];
				}
				num3 += num7;
				continue;
			}
			throw new InvalidDataException(message6);
		}
		int num10 = num3 - num;
		EnsureOutputCapacity(num10);
		WorkBuffer.AsSpan(num, num10).CopyTo(OutputBuffer.AsSpan(0, num10));
		UpdateHistory(OutputBuffer, 0, num10, independent);
		return num10;
	}

	private void UpdateHistory(byte[] decodedBuffer, int decodedOff, int decodedLen, bool independent)
	{
		if (!independent)
		{
			if (decodedLen < HistoryWindow.Length)
			{
				if (HistoryLen + decodedLen > HistoryWindow.Length)
				{
					int num = HistoryLen + decodedLen - HistoryWindow.Length;
					HistoryWindow.AsSpan(num, HistoryLen - num).CopyTo(HistoryWindow.AsSpan(0, HistoryLen - num));
					HistoryLen -= num;
					decodedBuffer.AsSpan(decodedOff, decodedLen).CopyTo(HistoryWindow.AsSpan(HistoryLen, decodedLen));
					HistoryLen += decodedLen;
				}
				else
				{
					decodedBuffer.AsSpan(decodedOff, decodedLen).CopyTo(HistoryWindow.AsSpan(HistoryLen, decodedLen));
					HistoryLen += decodedLen;
				}
			}
			else
			{
				decodedBuffer.AsSpan(decodedOff + decodedLen - HistoryWindow.Length, HistoryWindow.Length).CopyTo(HistoryWindow.AsSpan(0, HistoryWindow.Length));
				HistoryLen = HistoryWindow.Length;
			}
		}
		else
		{
			HistoryLen = 0;
		}
	}

	private int DecodeMaxBlockSize(int blockSizeCode)
	{
		return blockSizeCode switch
		{
			5 => 262144, 
			6 => 1048576, 
			7 => 4194304, 
			_ => 65536, 
		};
	}

	private byte ComputeHeaderChecksum(byte[] descriptorBuffer, int descriptorLen)
	{
		uint num = XxHash32(descriptorBuffer, 0, descriptorLen, 0u);
		return (byte)(num >> 8);
	}

	private uint XxHash32(byte[] data, int startOff, int byteLen, uint seed)
	{
		int num = startOff;
		int num2 = startOff + byteLen;
		uint num3 = seed + 374761393 + (uint)byteLen;
		bool flag = num + 4 <= num2;
		while (flag)
		{
			num3 += (uint)((int)Binary.ReadU32LE(data, num) * -1028477379);
			num3 = RotateLeft(num3, 17) * 668265263;
			num += 4;
			flag = num + 4 <= num2;
		}
		if (num >= num2)
		{
			num3 ^= num3 >> 15;
			num3 *= 2246822519u;
			num3 ^= num3 >> 13;
			num3 *= 3266489917u;
			return num3 ^ (num3 >> 16);
		}
		num3 += (uint)(data[num++] * 374761393);
		num3 = RotateLeft(num3, 11) * 2654435761u;
		int num4 = 699376335;
		while (true)
		{
			_ = num4 ^ -163195383;
			if (num >= num2)
			{
				break;
			}
			num3 += (uint)(data[num++] * 374761393);
			num3 = RotateLeft(num3, 11) * 2654435761u;
			num4 = 699376335;
		}
		num3 ^= num3 >> 15;
		num3 *= 2246822519u;
		num3 ^= num3 >> 13;
		num3 *= 3266489917u;
		return num3 ^ (num3 >> 16);
	}

	private uint RotateLeft(uint value, int bitCount)
	{
		return (value << bitCount) | (value >> 32 - bitCount);
	}

	private bool ReadUInt32LE(Stream stream, out uint value)
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

	private uint ReadUInt32LE(Stream stream)
	{
		byte[] array = new byte[4];
		ReadExactly(stream, array, 4);
		return Binary.ReadU32LE(array);
	}

	private ulong ReadUInt64LE(Stream stream)
	{
		byte[] array = new byte[8];
		ReadExactly(stream, array, 8);
		return Binary.ReadU64LE(array);
	}

	private int ReadUpTo(Stream stream, byte[] buffer, int byteCount)
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
		int num4 = 178329428;
		while (true)
		{
			_ = num4 ^ 0x3D7DE64A;
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
			num4 = 178329428;
		}
		return num;
	}

	private void ReadExactly(Stream stream, byte[] buffer, int byteCount)
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
			int num3 = 471171497;
			while (true)
			{
				_ = num3 ^ 0x3ECBC76B;
				if (num < byteCount)
				{
					num2 = stream.Read(buffer, num, byteCount - num);
					if (num2 > 0)
					{
						num += num2;
						num3 = 471171497;
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

	private void SkipBytes(Stream stream, long byteCount)
	{
		string message = "Unexpected End of stream while skipping bytes.";
		byte[] array = new byte[8192];
		long num = byteCount;
		if (num <= 0L)
		{
			return;
		}
		int count = (int)Math.Min(array.Length, num);
		int num2 = stream.Read(array, 0, count);
		if (num2 > 0)
		{
			num -= num2;
			int num3 = 966876918;
			while (true)
			{
				_ = num3 ^ 0x57D07EAB;
				if (num > 0L)
				{
					count = (int)Math.Min(array.Length, num);
					num2 = stream.Read(array, 0, count);
					if (num2 > 0)
					{
						num -= num2;
						num3 = 966876918;
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

	private void EnsureOutputCapacity(int bufferLen)
	{
		if (OutputBuffer.Length < bufferLen)
		{
			OutputBuffer = new byte[bufferLen];
		}
	}

	private void EnsureCompressedCapacity(int bufferLen)
	{
		if (CompressedBuffer.Length < bufferLen)
		{
			CompressedBuffer = new byte[bufferLen];
		}
	}

	private void EnsureWorkCapacity(int bufferLen)
	{
		if (WorkBuffer.Length < bufferLen)
		{
			WorkBuffer = new byte[bufferLen];
		}
	}

	public override void Flush()
	{
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !LeaveSourceOpen)
		{
			SourceStream.Dispose();
			base.Dispose(disposing);
		}
		else
		{
			base.Dispose(disposing);
		}
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
