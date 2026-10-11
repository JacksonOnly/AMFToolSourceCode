using System;
using System.IO;

namespace AMFTool.Library.Hisilicon;

public class EventStream : Stream
{
	public class ReadEventArgs : EventArgs
	{
		public object Param;

		public int Result;

		public byte[] Buffer;
	}

	public class WriteEventArgs : EventArgs
	{
		public object Param;
	}

	public delegate void OnReadEventHandler(object sender, ReadEventArgs e);

	public delegate void OnWriteEventHandler(object sender, WriteEventArgs e);

	private readonly Stream _baseStream;

	private readonly object _param;

	public OnReadEventHandler OnReadEvent;

	public OnWriteEventHandler OnWriteEvent;

	public override bool CanRead => _baseStream.CanRead;

	public override bool CanSeek => _baseStream.CanSeek;

	public override bool CanWrite => _baseStream.CanWrite;

	public override long Length => _baseStream.Length;

	public override long Position
	{
		get
		{
			return _baseStream.Position;
		}
		set
		{
			_baseStream.Position = value;
		}
	}

	public EventStream(Stream baseStream, object param = null)
	{
		_baseStream = baseStream;
		_param = param;
	}

	public override void Flush()
	{
		_baseStream.Flush();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return _baseStream.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		_baseStream.SetLength(value);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		object obj = null;
		int num = _baseStream.Read(buffer, offset, count);
		if (num == 0)
		{
			return num;
		}
		if (OnReadEvent ==null)
		{
			return num;
		}
		OnReadEvent(this, new ReadEventArgs
		{
			Buffer = buffer,
			Result = num,
			Param = _param
		});
		return num;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		object obj = null;
		_baseStream.Write(buffer, offset, count);
		if (OnWriteEvent !=null)
		{
			OnWriteEvent(this, new WriteEventArgs
			{
				Param = _param
			});
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
