using System;
using System.Collections.Generic;
using System.IO;

namespace AMFTool.Ext4;

public class Ext4BlockReader : Stream
{
	private Ext4FileSystem FileSystem;

	private Ext4Inode Inode;

	private List<Ext4Extent> Extents;

	private long PositionValue;

	private byte[] BlockBuffer;

	private byte[] ZeroBuffer;

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => false;

	public override long Length => Inode.Size;

	public override long Position
	{
		get
		{
			return PositionValue;
		}
		set
		{
			string paramName = "value";
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException(paramName);
			}
			if (value > Length)
			{
				throw new ArgumentOutOfRangeException(paramName);
			}
			PositionValue = value;
		}
	}

	public Ext4BlockReader(Ext4FileSystem fs, Ext4Inode inode)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		FileSystem = fs;
		Inode = inode;
		Extents = FileSystem.BuildBlockMap(Inode);
		BlockBuffer = new byte[FileSystem.BlockSize];
		ZeroBuffer = new byte[FileSystem.BlockSize];
	}

	public byte[] ReadAll()
	{
		string message = "File too large.";
		if (Length > 0L)
		{
			if (Length > 2147483647L)
			{
				throw new NotSupportedException(message);
			}
			byte[] array = new byte[(int)Length];
			int num = 0;
			if (0 >= array.Length)
			{
				if (0 != array.Length)
				{
					byte[] array2 = new byte[0];
					Buffer.BlockCopy(array, 0, array2, 0, 0);
					return array2;
				}
				return array;
			}
			int num2 = Read(array, 0, array.Length);
			if (num2 > 0)
			{
				num = 0 + num2;
				int num3 = 592735993;
				while (true)
				{
					_ = num3 ^ 0x59B52A0;
					if (num >= array.Length)
					{
						if (num != array.Length)
						{
							byte[] array2 = new byte[num];
							Buffer.BlockCopy(array, 0, array2, 0, num);
							return array2;
						}
						return array;
					}
					num2 = Read(array, num, array.Length - num);
					if (num2 <= 0)
					{
						break;
					}
					num += num2;
					num3 = 592735993;
				}
				if (num != array.Length)
				{
					byte[] array2 = new byte[num];
					Buffer.BlockCopy(array, 0, array2, 0, num);
					return array2;
				}
				return array;
			}
			if (0 != array.Length)
			{
				byte[] array2 = new byte[0];
				Buffer.BlockCopy(array, 0, array2, 0, 0);
				return array2;
			}
			return array;
		}
		return Array.Empty<byte>();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		object obj = null;
		string paramName = "buffer";
		if (buffer != obj)
		{
			if (offset >= 0 && count >= 0 && offset + count <= buffer.Length)
			{
				if (PositionValue < Length)
				{
					long num = Length - PositionValue;
					if (count > num)
					{
						count = (int)num;
					}
					if (!Inode.HasInlineData || Inode.InlineData == null || Inode.InlineDataLength <= 0)
					{
						int i;
						int num4;
						for (i = 0; i < count; i += num4)
						{
							long positionValue = PositionValue;
							long num2 = positionValue / FileSystem.BlockSize;
							int num3 = (int)(positionValue % FileSystem.BlockSize);
							num4 = (int)Math.Min(count - i, FileSystem.BlockSize - num3);
							long num5 = FileSystem.ResolvePhysicalBlockForLogicalBlock(Extents, (uint)num2);
							if (num5 >= 0L)
							{
								long offset2 = num5 * FileSystem.BlockSize;
								FileSystem.ReadAt(offset2, BlockBuffer, BlockBuffer.Length);
								Buffer.BlockCopy(BlockBuffer, num3, buffer, offset + i, num4);
							}
							else
							{
								Buffer.BlockCopy(ZeroBuffer, num3, buffer, offset + i, num4);
							}
							PositionValue += num4;
						}
						return i;
					}
					int num6 = (int)PositionValue;
					int num7 = Math.Min(Inode.InlineDataLength, (int)Math.Min(2147483647L, Inode.Size));
					int num8 = Math.Min(count, Math.Max(0, num7 - num6));
					if (num8 > 0)
					{
						Buffer.BlockCopy(Inode.InlineData, num6, buffer, offset, num8);
						PositionValue += num8;
						return num8;
					}
					return 0;
				}
				return 0;
			}
			throw new ArgumentOutOfRangeException();
		}
		throw new ArgumentNullException(paramName);
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
			num = PositionValue + offset;
			if (num >= 0L)
			{
				goto IL_0077;
			}
			flag = true;
			break;
		case SeekOrigin.End:
			{
				num = Length + offset;
				if (num >= 0L)
				{
					goto IL_0077;
				}
				flag = true;
				break;
			}
			IL_0077:
			flag = num > Length;
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

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
