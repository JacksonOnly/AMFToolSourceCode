using System;
using System.IO;
using AMFTool.Library.Mediatek.Download_Agent;

namespace AMFTool.PartitionManager;

public class FileStreamsMediatek : Stream
{
	private long Sector;

	private long Sectors;

	private int SectorSize;

	private long LengthValue;

	private long PositionValue;

	private byte[] CacheBuffer;

	private long CacheOffset = -1L;

	private MtkDaLoader DaLoader;

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
			if ((ulong)value > (ulong)LengthValue)
			{
				throw new ArgumentOutOfRangeException(paramName);
			}
			PositionValue = value;
		}
	}

	public FileStreamsMediatek(long sector, long sectors, int sectorSize, MtkDaLoader loader)
	{
		Sector = sector;
		Sectors = sectors;
		SectorSize = sectorSize;
		LengthValue = sectors * sectorSize;
		DaLoader = loader;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		object obj = null;
		string paramName = "buffer";
		string message = "Buffer too small";
		if (buffer != obj)
		{
			if (offset >= 0 && count >= 0)
			{
				if (offset + count > buffer.Length)
				{
					throw new ArgumentException(message);
				}
				if (PositionValue >= LengthValue)
				{
					return 0;
				}
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
						long num3 = positionValue / SectorSize;
						long num4 = positionValue % SectorSize;
						long num5 = (Math.Min(1048576, count - num2) + num4 + SectorSize - 1L) / SectorSize;
						long num6 = num5 * SectorSize;
						byte[] array = ReadFromDevice(num3, (int)num6);
						if (array == null || array.Length == 0)
						{
							break;
						}
						CacheBuffer = array;
						CacheOffset = num3 * SectorSize;
					}
					else
					{
						int num7 = (int)(PositionValue - CacheOffset);
						int val = CacheBuffer.Length - num7;
						int num8 = Math.Min(count - num2, val);
						Buffer.BlockCopy(CacheBuffer, num7, buffer, offset + num2, num8);
						PositionValue += num8;
						num2 += num8;
					}
				}
				return num2;
			}
			throw new ArgumentOutOfRangeException();
		}
		throw new ArgumentNullException(paramName);
	}

	private byte[] ReadFromDevice(long lbaOff, int byteCount)
	{
		string partType = "user";
		object result = null;
		try
		{
			long addr = (Sector + lbaOff) * SectorSize;
			return DaLoader.ReadFlashByte(addr, byteCount, partType);
		}
		catch
		{
			return (byte[])result;
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		string paramName = "origin";
		string paramName2 = "offset";
		long num;
		bool flag;
		switch (origin)
		{
		default:
			throw new ArgumentOutOfRangeException(paramName);
		case SeekOrigin.Begin:
			num = offset;
			flag = (ulong)num > (ulong)LengthValue;
			break;
		case SeekOrigin.Current:
			num = PositionValue + offset;
			flag = (ulong)num > (ulong)LengthValue;
			break;
		case SeekOrigin.End:
			num = LengthValue + offset;
			flag = (ulong)num > (ulong)LengthValue;
			break;
		}
		if (flag)
		{
			throw new ArgumentOutOfRangeException(paramName2);
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
		if (disposing)
		{
			CacheBuffer = (byte[])cacheBuffer;
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
}
