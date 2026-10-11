using System;
using System.IO;

namespace AMFTool.Library.Hisilicon;

public class PartialStream : Stream
{
	private readonly Stream _baseStream;

	private readonly long _basePosition;

	private readonly long _size;

	public override bool CanRead => _baseStream.CanRead;

	public override bool CanSeek => _baseStream.CanSeek;

	public override bool CanWrite => _baseStream.CanWrite;

	public override long Length => _size;

	public override long Position
	{
		get
		{
			return _baseStream.Position - _basePosition;
		}
		set
		{
			string message = "Position failed";
			if (value > _size || value < 0L)
			{
				throw new EndOfStreamException(message);
			}
			_baseStream.Position = _basePosition + value;
		}
	}

	public PartialStream(Stream baseStream, long size)
	{
		string paramName = "size";
		string message = "Invalid size";

		_baseStream = baseStream;
		_basePosition = baseStream.Position;
		if (_basePosition + _size > _baseStream.Length)
		{
			throw new ArgumentOutOfRangeException(paramName, _basePosition + _size, message);
		}
		_size = size;
	}

	public override void Flush()
	{
		_baseStream.Flush();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		string message = "Seek failed";
		switch (origin)
		{
		default:
			return _baseStream.Seek(_basePosition + _size + offset, origin);
		case SeekOrigin.Begin:
			if (offset <= _size && offset >= 0L)
			{
				return _baseStream.Seek(_basePosition + offset, origin);
			}
			throw new EndOfStreamException(message);
		case SeekOrigin.Current:
		{
			long num = _baseStream.Position - _basePosition;
			if (num + offset > _size || num + offset < 0L)
			{
				throw new EndOfStreamException(message);
			}
			return _baseStream.Seek(num + offset, origin);
		}
		case SeekOrigin.End:
			if (offset > 0L)
			{
				throw new EndOfStreamException(message);
			}
			return _baseStream.Seek(_basePosition + _size + offset, origin);
		}
	}

	public override void SetLength(long value)
	{
		_baseStream.SetLength(value);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		long num = _baseStream.Position - _basePosition;
		if (num != _size)
		{
			if (num + count <= _size)
			{
				return _baseStream.Read(buffer, offset, count);
			}
			long num2 = _size - num;
			count = (int)((num2 > 2147483647L) ? int.MaxValue : num2);
			return _baseStream.Read(buffer, offset, count);
		}
		int num3 = 0;
		return 0;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		string message = "Write failed";
		long num = _baseStream.Position - _basePosition;
		if (num + count > _size)
		{
			throw new EndOfStreamException(message);
		}
		_baseStream.Write(buffer, offset, count);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
