using System;
using System.IO;
using AMFTool.Library.Qualcomm;

namespace AMFTool.PartitionManager;

public class FileStreamsQualcomm : Stream
{
	private long Sector;

	private long Sectors;

	private int SectorSize;

	private int PhysicalPartNum;

	private long LengthValue;

	private string Label;

	private string SizeInKb;

	private long PositionValue;

	private byte[] CacheBuffer;

	private long CacheOffset = -1L;

	private Firehose Firehose;

	private long TotalToRead;

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => false;

	public override long Length => LengthValue;

	public override long Position
	{
		get
		{
			return PositionValue;
		}
		set
		{
			string paramName = "value";
			if (value < 0L || value > LengthValue)
			{
				throw new ArgumentOutOfRangeException(paramName);
			}
			PositionValue = value;
		}
	}

	public FileStreamsQualcomm(long sector, long sectors, int sectorSize, int physicalPartNum, string label, string sizeInKb, Firehose firehose)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Sector = sector;
		Sectors = sectors;
		SectorSize = sectorSize;
		PhysicalPartNum = physicalPartNum;
		LengthValue = sectors * sectorSize;
		PositionValue = 0L;
		Label = label;
		SizeInKb = sizeInKb;
		Firehose = firehose;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		object obj = null;
		string paramName = "buffer";
		string message = "Buffer too small";
		if (buffer != obj)
		{
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentException(message);
			}
			if (PositionValue < LengthValue)
			{
				long num = LengthValue - PositionValue;
				if (count > num)
				{
					count = (int)num;
				}
				int num2 = 0;
				while (num2 < count)
				{
					if (CacheBuffer == null || PositionValue < CacheOffset || PositionValue >= CacheOffset + CacheBuffer.Length)
					{
						long positionValue = PositionValue;
						int num3 = Math.Min(1048576, count - num2);
						long num4 = positionValue / SectorSize;
						long num5 = (positionValue + num3 + SectorSize - 1L) / SectorSize;
						long sectorCount = num5 - num4;
						byte[] array = ReadFromDevice(num4, sectorCount);
						if (array == null || array.Length == 0)
						{
							break;
						}
						CacheBuffer = array;
						CacheOffset = num4 * SectorSize;
						int num6 = (int)(positionValue - CacheOffset);
						int val = array.Length - num6;
						int num7 = Math.Min(count - num2, val);
						Buffer.BlockCopy(array, num6, buffer, offset + num2, num7);
						PositionValue += num7;
						num2 += num7;
						TotalToRead += num7;
						UI.Progresbar1(TotalToRead, LengthValue);
					}
					else
					{
						int num8 = (int)(PositionValue - CacheOffset);
						int val2 = CacheBuffer.Length - num8;
						int num9 = Math.Min(count - num2, val2);
						Buffer.BlockCopy(CacheBuffer, num8, buffer, offset + num2, num9);
						PositionValue += num9;
						num2 += num9;
					}
				}
				return num2;
			}
			return 0;
		}
		throw new ArgumentNullException(paramName);
	}

	private byte[] ReadFromDevice(long sectorOffset, long sectorCount)
	{
		string format = "{0}";
		object result = null;
		try
		{
			long num = Sector + sectorOffset;
			long num2 = 1048576 / SectorSize;
			if (sectorCount > num2)
			{
				sectorCount = num2;
			}
			return Firehose.ReadFlashByte(PhysicalPartNum, string.Format(format, num), (int)sectorCount, Label, SizeInKb);
		}
		catch
		{
			return (byte[])result;
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		string message = "Invalid seek origin";
		string paramName = "offset";
		long num;
		bool flag;
		switch (origin)
		{
		default:
			throw new ArgumentException(message);
		case SeekOrigin.Begin:
			num = offset;
			if (num >= 0L)
			{
				goto IL_0077;
			}
			flag = true;
			break;
		case SeekOrigin.Current:
			num = PositionValue + offset;
			if (num >= 0L)
			{
				goto IL_0077;
			}
			flag = true;
			break;
		case SeekOrigin.End:
			{
				num = LengthValue + offset;
				if (num >= 0L)
				{
					goto IL_0077;
				}
				flag = true;
				break;
			}
			IL_0077:
			flag = num > LengthValue;
			break;
		}
		if (flag)
		{
			throw new ArgumentOutOfRangeException(paramName);
		}
		PositionValue = num;
		return PositionValue;
	}

	public override void Flush()
	{
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
		object cacheBuffer = null;
		if (!disposing)
		{
			base.Dispose(disposing);
			return;
		}
		CacheBuffer = (byte[])cacheBuffer;
		base.Dispose(disposing);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
