using System;
using System.Collections.Generic;
using System.IO;

namespace AMFTool;

public sealed class LogicalPartitionStream : Stream
{
	private Stream Stream;

	private List<LogicalPartitionSuper.LogicalExtent> Extents;

	private long LogicalSize;

	private long LogicalPos;

	private byte[] CacheBuffer;

	private long CacheBase;

	private long CacheTail;

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => false;

	public override long Length => LogicalSize;

	public override long Position
	{
		get
		{
			return LogicalPos;
		}
		set
		{
			string paramName = "value";
			if (value < 0L || value > LogicalSize)
			{
				throw new ArgumentOutOfRangeException(paramName);
			}
			LogicalPos = value;
		}
	}

	public LogicalPartitionStream(Stream streamSource, LogicalPartitionSuper.LogicalPartition partition)
	{
		object obj = null;
		string paramName = "streamSource";
		string paramName2 = "partition";

		if (streamSource != obj)
		{
			if (partition == obj)
			{
				throw new ArgumentNullException(paramName2);
			}
			Stream = streamSource;
			Extents = partition.Extents;
			LogicalSize = partition.TotalSize;
			CacheBuffer = (byte[])obj;
			CacheTail = -1L;
			CacheBase = -1L;
			LogicalPos = 0L;
			return;
		}
		throw new ArgumentNullException(paramName);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		object obj = null;
		string paramName = "buffer";
		string message = "Unsupported logical partition extent type.";
		string message2 = "Invalid logical partition extent offset.";
		if (buffer == obj)
		{
			throw new ArgumentNullException(paramName);
		}
		if (offset < 0 || count < 0 || offset + count > buffer.Length)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (LogicalPos < LogicalSize)
		{
			long num = LogicalSize - LogicalPos;
			if (count > num)
			{
				count = (int)num;
			}
			int num2 = 0;
			while (num2 < count)
			{
				LogicalPartitionSuper.LogicalExtent? logicalExtent = FindExtentForLogicalPosition(LogicalPos);
				if (!logicalExtent.HasValue)
				{
					break;
				}
				long num3 = LogicalPos - logicalExtent.Value.LogicalOffset;
				long num4 = logicalExtent.Value.Size - num3;
				int num5 = (int)Math.Min(count - num2, num4);
				if (logicalExtent.Value.TargetType != 1)
				{
					if (logicalExtent.Value.TargetType != 0 || logicalExtent.Value.PhysicalOffset < 0L)
					{
						throw new InvalidDataException(message);
					}
					long num6 = logicalExtent.Value.PhysicalOffset + num3;
					if (num6 < 0L)
					{
						throw new InvalidDataException(message2);
					}
					int maxWindow = (int)((num4 > 2147483647L) ? int.MaxValue : num4);
					if (CacheBuffer != null && num6 >= CacheBase && num6 < CacheTail)
					{
						long num7 = CacheTail - num6;
						if (num7 <= 0L)
						{
							CacheBuffer = (byte[])obj;
							CacheTail = -1L;
							CacheBase = -1L;
						}
						else
						{
							int num8 = (int)Math.Min(num5, num7);
							int srcOffset = (int)(num6 - CacheBase);
							Buffer.BlockCopy(CacheBuffer, srcOffset, buffer, offset + num2, num8);
							LogicalPos += num8;
							num2 += num8;
						}
					}
					else
					{
						RefillCacheWindow(num6, num5, maxWindow);
						long num9 = CacheTail - num6;
						if (num9 <= 0L)
						{
							break;
						}
						int num10 = (int)Math.Min(num5, num9);
						int srcOffset2 = (int)(num6 - CacheBase);
						Buffer.BlockCopy(CacheBuffer, srcOffset2, buffer, offset + num2, num10);
						LogicalPos += num10;
						num2 += num10;
					}
				}
				else
				{
					Array.Clear(buffer, offset + num2, num5);
					LogicalPos += num5;
					num2 += num5;
				}
			}
			return num2;
		}
		return 0;
	}

	private void RefillCacheWindow(long physicalOffset, int minNeed, int maxWindow)
	{
		int num = Math.Min(1048576, maxWindow);
		int num2;
		int num3;
		int num4;
		if (num >= minNeed)
		{
			CacheBuffer = new byte[num];
			Stream.Seek(physicalOffset, SeekOrigin.Begin);
			num2 = 0;
			num3 = num;
			if (num3 <= 0)
			{
				CacheBase = physicalOffset;
				CacheTail = physicalOffset + 0L;
				return;
			}
			num4 = Stream.Read(CacheBuffer, 0, num3);
			if (num4 <= 0)
			{
				CacheBase = physicalOffset;
				CacheTail = physicalOffset + 0L;
				return;
			}
			num2 = 0 + num4;
			num3 -= num4;
			int num5 = 267919816;
			while (true)
			{
				_ = num5 ^ -747559920;
				if (num3 <= 0)
				{
					CacheBase = physicalOffset;
					CacheTail = physicalOffset + num2;
					return;
				}
				num4 = Stream.Read(CacheBuffer, num2, num3);
				if (num4 <= 0)
				{
					break;
				}
				num2 += num4;
				num3 -= num4;
				num5 = 267919816;
			}
			CacheBase = physicalOffset;
			CacheTail = physicalOffset + num2;
			return;
		}
		num = minNeed;
		CacheBuffer = new byte[num];
		Stream.Seek(physicalOffset, SeekOrigin.Begin);
		num2 = 0;
		num3 = num;
		if (num3 <= 0)
		{
			CacheBase = physicalOffset;
			CacheTail = physicalOffset + 0L;
			return;
		}
		num4 = Stream.Read(CacheBuffer, 0, num3);
		if (num4 <= 0)
		{
			CacheBase = physicalOffset;
			CacheTail = physicalOffset + 0L;
			return;
		}
		num2 = 0 + num4;
		num3 -= num4;
		int num6 = 267919816;
		while (true)
		{
			_ = num6 ^ -747559920;
			if (num3 <= 0)
			{
				CacheBase = physicalOffset;
				CacheTail = physicalOffset + num2;
				return;
			}
			num4 = Stream.Read(CacheBuffer, num2, num3);
			if (num4 <= 0)
			{
				break;
			}
			num2 += num4;
			num3 -= num4;
			num6 = 267919816;
		}
		CacheBase = physicalOffset;
		CacheTail = physicalOffset + num2;
	}

	private LogicalPartitionSuper.LogicalExtent? FindExtentForLogicalPosition(long logicalOffset)
	{
		for (int i = 0; i < Extents.Count; i++)
		{
			LogicalPartitionSuper.LogicalExtent value = Extents[i];
			if (logicalOffset >= value.LogicalOffset && logicalOffset < value.LogicalOffset + value.Size)
			{
				return value;
			}
		}
		return null;
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
			if (num >= 0L)
			{
				goto IL_0077;
			}
			flag = true;
			break;
		case SeekOrigin.Current:
			num = LogicalPos + offset;
			if (num >= 0L)
			{
				goto IL_0077;
			}
			flag = true;
			break;
		case SeekOrigin.End:
			{
				num = LogicalSize + offset;
				if (num >= 0L)
				{
					goto IL_0077;
				}
				flag = true;
				break;
			}
			IL_0077:
			flag = num > LogicalSize;
			break;
		}
		if (flag)
		{
			throw new ArgumentOutOfRangeException(paramName2);
		}
		LogicalPos = num;
		return LogicalPos;
	}

	public override void Flush()
	{
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] source, int sourceOffset, int count)
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
