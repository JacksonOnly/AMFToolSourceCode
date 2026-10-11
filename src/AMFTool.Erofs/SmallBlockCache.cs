using System;
using System.IO;

namespace AMFTool.Erofs;

public class SmallBlockCache
{
	private Stream io;

	private long baseoff;

	private int blocksz;

	private long idx0 = -1L;

	private long idx1 = -1L;

	private byte[] buf0;

	private byte[] buf1;

	public SmallBlockCache(Stream io, long baseoff, int blocksz)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		this.io = io;
		this.baseoff = baseoff;
		this.blocksz = blocksz;
		buf0 = new byte[blocksz];
		buf1 = new byte[blocksz];
	}

	public void ReadBlock(long blockidx, byte[] destination)
	{
		string message = "destination too small.";
		if (destination == null)
		{
			throw new ArgumentException(message);
		}
		if (destination.Length >= blocksz)
		{
			if (blockidx == idx0)
			{
				Buffer.BlockCopy(buf0, 0, destination, 0, blocksz);
			}
			else if (blockidx != idx1)
			{
				long absoff = baseoff + blockidx * blocksz;
				ReadAt(io, absoff, destination, 0, blocksz);
				idx1 = idx0;
				byte[] array = buf1;
				buf1 = buf0;
				buf0 = array;
				idx0 = blockidx;
				Buffer.BlockCopy(destination, 0, buf0, 0, blocksz);
			}
			else
			{
				Buffer.BlockCopy(buf1, 0, destination, 0, blocksz);
			}
			return;
		}
		throw new ArgumentException(message);
	}

	private static void ReadAt(Stream source, long absoff, byte[] buffer, int offset, int count)
	{
		source.Seek(absoff, SeekOrigin.Begin);
		int num = 0;
		if (0 >= count)
		{
			return;
		}
		int num2 = source.Read(buffer, offset, count);
		if (num2 <= 0)
		{
			throw new EndOfStreamException();
		}
		num = 0 + num2;
		int num3 = 1776851116;
		while (true)
		{
			_ = num3 ^ 0x2FA6A8A2;
			if (num < count)
			{
				num2 = source.Read(buffer, offset + num, count - num);
				if (num2 <= 0)
				{
					throw new EndOfStreamException();
				}
				num += num2;
				num3 = 1776851116;
				continue;
			}
			break;
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
