using System;
using System.IO;

namespace AMFTool.Library.Odin.Tar;

public sealed class LimitedReadStream : Stream
{
	private readonly Stream innerStream;

	private readonly bool leaveInnerOpen;

	private readonly long startOffset;

	private readonly long maxLength;

	public override bool CanRead => innerStream.CanRead;

	public override bool CanSeek => innerStream.CanSeek;

	public override bool CanWrite => false;

	public override long Length => maxLength;

	public override long Position
	{
		get
		{
			return innerStream.Position - startOffset;
		}
		set
		{
			string paramName = "value";
			if (value < 0L || value > maxLength)
			{
				throw new ArgumentOutOfRangeException(paramName);
			}
			innerStream.Position = startOffset + value;
		}
	}

	public LimitedReadStream(Stream innerStream, long startOffset, long maxLength, bool leaveInnerOpen = false)
	{
		string paramName = "innerStream";
		string message = "Stream must be readable.";
		string message2 = "Stream must be seekable.";
		string paramName2 = "maxLength";
		string message3 = "Start position and MaxLength must be non-negative.";

		this.innerStream = innerStream ?? throw new ArgumentNullException(paramName);
		this.leaveInnerOpen = leaveInnerOpen;
		this.startOffset = startOffset;
		this.maxLength = maxLength;
		if (!this.innerStream.CanRead)
		{
			throw new ArgumentException(message, paramName);
		}
		if (!this.innerStream.CanSeek)
		{
			throw new ArgumentException(message2, paramName);
		}
		if (startOffset < 0L || maxLength < 0L)
		{
			throw new ArgumentOutOfRangeException(paramName2, message3);
		}
		this.innerStream.Position = startOffset;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		long num = maxLength - Position;
		if (num > 0L)
		{
			int count2 = (int)Math.Min(count, num);
			return innerStream.Read(buffer, offset, count2);
		}
		int num2 = 0;
		return 0;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		string paramName = "origin";
		string message = "Attempted to seek outside the limited Stream range.";
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
			num = Position + offset;
			if (num >= 0L)
			{
				goto IL_0077;
			}
			flag = true;
			break;
		case SeekOrigin.End:
			{
				num = maxLength + offset;
				if (num >= 0L)
				{
					goto IL_0077;
				}
				flag = true;
				break;
			}
			IL_0077:
			flag = num > maxLength;
			break;
		}
		if (flag)
		{
			throw new IOException(message);
		}
		Position = num;
		return Position;
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override void Flush()
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !leaveInnerOpen)
		{
			innerStream.Dispose();
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
