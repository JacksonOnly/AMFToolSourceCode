using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AMFTool.Library.Usb;

public class Native2 : IDisposable
{
	private struct DCB
	{
		public int DCBlength;

		public int BaudRate;

		public uint flags;

		public ushort wReserved;

		public ushort XonLim;

		public ushort XoffLim;

		public byte ByteSize;

		public byte Parity;

		public byte StopBits;

		public sbyte XonChar;

		public sbyte XoffChar;

		public sbyte ErrorChar;

		public sbyte EofChar;

		public sbyte EvtChar;

		public ushort wReserved1;

		public bool fBinary
		{
			get
			{
				return (flags & 1) != 0;
			}
			set
			{
				flags = (value ? (flags | 1) : (flags & 0xFFFFFFFEu));
			}
		}

		public bool fOutxCtsFlow
		{
			get
			{
				return (flags & 4) != 0;
			}
			set
			{
				flags = (value ? (flags | 4) : (flags & 0xFFFFFFFBu));
			}
		}

		public bool fOutxDsrFlow
		{
			get
			{
				return (flags & 8) != 0;
			}
			set
			{
				flags = (value ? (flags | 8) : (flags & 0xFFFFFFF7u));
			}
		}

		public int fDtrControl
		{
			get
			{
				return (int)((flags >> 4) & 3);
			}
			set
			{
				flags = (flags & 0xFFFFFFCFu) | (uint)(value << 4);
			}
		}

		public bool fDsrSensitivity
		{
			get
			{
				return (flags & 0x40) != 0;
			}
			set
			{
				flags = (value ? (flags | 0x40) : (flags & 0xFFFFFFBFu));
			}
		}

		public bool fTXContinueOnXoff
		{
			get
			{
				return (flags & 0x80) != 0;
			}
			set
			{
				flags = (value ? (flags | 0x80) : (flags & 0xFFFFFF7Fu));
			}
		}

		public bool fOutX
		{
			get
			{
				return (flags & 0x100) != 0;
			}
			set
			{
				flags = (value ? (flags | 0x100) : (flags & 0xFFFFFEFFu));
			}
		}

		public bool fInX
		{
			get
			{
				return (flags & 0x200) != 0;
			}
			set
			{
				flags = (value ? (flags | 0x200) : (flags & 0xFFFFFDFFu));
			}
		}

		public int fRtsControl
		{
			get
			{
				return (int)((flags >> 12) & 3);
			}
			set
			{
				flags = (flags & 0xFFFFCFFFu) | (uint)(value << 12);
			}
		}

		public bool fAbortOnError
		{
			get
			{
				return (flags & 0x4000) != 0;
			}
			set
			{
				flags = (value ? (flags | 0x4000) : (flags & 0xFFFFBFFFu));
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

	private struct COMMTIMEOUTS
	{
		public uint ReadIntervalTimeout;

		public uint ReadTotalTimeoutMultiplier;

		public uint ReadTotalTimeoutConstant;

		public uint WriteTotalTimeoutMultiplier;

		public uint WriteTotalTimeoutConstant;
	}

	private const uint PURGE_TXCLEAR = 4u;

	private const uint PURGE_TXABORT = 1u;

	private const uint GENERIC_WRITE = 1073741824u;

	private const uint FILE_ATTRIBUTE_NORMAL = 128u;

	private IntPtr handle = IntPtr.Zero;

	private bool isOpen = false;

	public string PortName { get; private set; }

	public bool IsOpen => isOpen;

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

	[DllImport("kernel32.dll")]
	private static extern bool CloseHandle(IntPtr hObject);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool GetCommState(IntPtr hFile, ref DCB lpDCB);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool SetCommState(IntPtr hFile, ref DCB lpDCB);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool SetupComm(IntPtr hFile, int dwInQueue, int dwOutQueue);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool SetCommTimeouts(IntPtr hFile, ref COMMTIMEOUTS lpCommTimeouts);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool PurgeComm(IntPtr hFile, uint dwFlags);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWriteк, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

	public bool Open(string portName)
	{
		string text = "\\\\.\\";
		string lpFileName;
		if (isOpen)
		{
			Close();
			lpFileName = text + portName;
		}
		else
		{
			lpFileName = text + portName;
		}
		handle = CreateFile(lpFileName, 3221225472u, 0u, IntPtr.Zero, 3u, 128u, IntPtr.Zero);
		if (!(handle == IntPtr.Zero) && !(handle == new IntPtr(-1)))
		{
			SetupComm(handle, 4096, 4096);
			ConfigurePort();
			PurgeComm(handle, 12u);
			isOpen = true;
			return true;
		}
		return false;
	}

	private void ConfigurePort()
	{
		DCB lpDCB = default(DCB);
		lpDCB.DCBlength = Marshal.SizeOf(lpDCB);
		GetCommState(handle, ref lpDCB);
		lpDCB.BaudRate = 115200;
		lpDCB.ByteSize = 8;
		lpDCB.Parity = 0;
		lpDCB.StopBits = 0;
		lpDCB.fBinary = true;
		lpDCB.fOutxCtsFlow = false;
		lpDCB.fOutxDsrFlow = false;
		lpDCB.fDtrControl = 0;
		lpDCB.fDsrSensitivity = false;
		lpDCB.fTXContinueOnXoff = false;
		lpDCB.fOutX = false;
		lpDCB.fInX = false;
		lpDCB.fRtsControl = 0;
		lpDCB.fAbortOnError = false;
		SetCommState(handle, ref lpDCB);
		COMMTIMEOUTS lpCommTimeouts = new COMMTIMEOUTS
		{
			ReadIntervalTimeout = 50u,
			ReadTotalTimeoutConstant = 1000u,
			ReadTotalTimeoutMultiplier = 0u,
			WriteTotalTimeoutConstant = 1000u,
			WriteTotalTimeoutMultiplier = 0u
		};
		SetCommTimeouts(handle, ref lpCommTimeouts);
	}

	public bool write(byte[] data)
	{
		string message = "Port not open";
		object obj = null;
		string paramName = "data";
		string format = "Error: {0}";
		if (isOpen)
		{
			if (data != obj)
			{
				if (!JobStatus.Token.IsCancellationRequested)
				{
					Purge();
					if (!WriteFile(handle, data, (uint)data.Length, out var lpNumberOfBytesWritten, IntPtr.Zero))
					{
						throw new IOException(string.Format(format, Marshal.GetLastWin32Error()));
					}
					if (!JobStatus.Token.IsCancellationRequested)
					{
						return lpNumberOfBytesWritten == data.Length;
					}
					return false;
				}
				return false;
			}
			throw new ArgumentNullException(paramName);
		}
		throw new InvalidOperationException(message);
	}

	public void Purge()
	{
		if (isOpen && !PurgeComm(handle, 15u))
		{
			Marshal.GetLastWin32Error();
		}
	}

	public byte[] read(int length)
	{
		string message = "Port not open";
		string format = "Error: {0}";
		if (!isOpen)
		{
			throw new InvalidOperationException(message);
		}
		if (!JobStatus.Token.IsCancellationRequested)
		{
			byte[] array = new byte[length];
			byte[] array2;
			if (!ReadFile(handle, array, (uint)length, out var lpNumberOfBytesRead, IntPtr.Zero))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 258)
				{
					lpNumberOfBytesRead = 0u;
					if (JobStatus.Token.IsCancellationRequested)
					{
						return new byte[0];
					}
					if (lpNumberOfBytesRead >= length)
					{
						return array;
					}
					array2 = new byte[lpNumberOfBytesRead];
					Array.Copy(array, array2, lpNumberOfBytesRead);
					return array2;
				}
				throw new IOException(string.Format(format, lastWin32Error));
			}
			if (JobStatus.Token.IsCancellationRequested)
			{
				return new byte[0];
			}
			if (lpNumberOfBytesRead >= length)
			{
				return array;
			}
			array2 = new byte[lpNumberOfBytesRead];
			Array.Copy(array, array2, lpNumberOfBytesRead);
			return array2;
		}
		return new byte[0];
	}

	public int read(byte[] buffer, int maxLength)
	{
		object obj = null;
		string paramName = "buffer";
		string format = "Error: {0}";
		if (buffer != obj)
		{
			int nNumberOfBytesToRead = Math.Min(buffer.Length, maxLength);
			if (!JobStatus.Token.IsCancellationRequested)
			{
				if (ReadFile(handle, buffer, (uint)nNumberOfBytesToRead, out var lpNumberOfBytesRead, IntPtr.Zero))
				{
					return (int)lpNumberOfBytesRead;
				}
				if (Marshal.GetLastWin32Error() != 258)
				{
					throw new IOException(string.Format(format, Marshal.GetLastWin32Error()));
				}
				return (int)lpNumberOfBytesRead;
			}
			int num = 0;
			return 0;
		}
		throw new ArgumentNullException(paramName);
	}

	public void Close()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (!(handle != IntPtr.Zero))
		{
			isOpen = false;
			return;
		}
		if (!(handle != new IntPtr(-1)))
		{
			isOpen = false;
			return;
		}
		CloseHandle(handle);
		handle = IntPtr.Zero;
		isOpen = false;
	}

	public void Dispose()
	{
		Close();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
