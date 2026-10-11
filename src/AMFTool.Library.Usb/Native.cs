using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace AMFTool.Library.Usb;

public class Native : IDisposable
{
	public string PortName;

	public uint BaudRate = 9600u;

	public int readTimeout;

	public int writeTimeout;

	public int Timeout;

	public int max_to_read;

	public int max_to_write;

	private bool isconnected = false;

	private ComPort com_port;

	private FileIO.COMSTAT comstart;

	private const uint PURGE_RXABORT = 2u;

	private const uint PURGE_RXCLEAR = 8u;

	private int bytesToread
	{
		get
		{
			int lpErrors = 0;
			if (!FileIO.ClearCommError(com_port.port_fd, ref lpErrors, ref comstart))
			{
				int num = 0;
				return 0;
			}
			return (int)comstart.cbInQue;
		}
	}

	public bool IsOpen
	{
		get
		{
			if (com_port.port_fd == null || !isconnected)
			{
				return false;
			}
			return true;
		}
	}

	public bool Open()
	{
		string text = "\\\\.\\";
		string format = "CreateFile failed on {0}, error={1}";
		int num = 0;
		string format2 = "GetCommState failed, error={0}";
		string format3 = "SetCommState failed, error = {0}";
		string format4 = "GetCommTimeouts failed, error = {0}";
		string format5 = "SetCommTimeouts failed, error = {0}";
		string format6 = "SetupComm failed, error = {0}";
		string text2 = "Exception: ";
		try
		{
			com_port = default(ComPort);
			FileIO.DCB lpDCB = default(FileIO.DCB);
			FileIO.COMSTAT cOMSTAT = default(FileIO.COMSTAT);
			FileIO.COMMTIMEOUTS lpCommTimeouts = default(FileIO.COMMTIMEOUTS);
			com_port.port_name = PortName;
			com_port.port_fd = new SafeFileHandle(IntPtr.Zero, ownsHandle: true);
			SafeFileHandle safeFileHandle = FileIO.CreateFile(text + PortName, (FileIO.FileAccess)3221225472u, (FileIO.FileShareMode)3u, IntPtr.Zero, FileIO.FilePosition.OPEN_EXISTING, FileIO.FileAttributes.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
			bool flag;
			if (safeFileHandle.IsInvalid)
			{
				Console.WriteLine(string.Format(format, PortName, Marshal.GetLastWin32Error()));
				flag = false;
				isconnected = false;
				return false;
			}
			com_port.port_fd = safeFileHandle;
			com_port.BaudRate = BaudRate;
			com_port.Timeout = Timeout;
			com_port.writeTimeout = writeTimeout;
			com_port.readTimeout = readTimeout;
			com_port.max_to_write = max_to_write;
			com_port.max_to_read = max_to_read;
			lpDCB.DCBlength = (uint)Marshal.SizeOf(typeof(FileIO.DCB));
			if (FileIO.GetCommState(com_port.port_fd, ref lpDCB))
			{
				lpDCB.BaudRate = com_port.BaudRate;
				lpDCB.ByteSize = 8;
				lpDCB.StopBits = 1;
				lpDCB.Parity = 0;
				if (!FileIO.SetCommState(com_port.port_fd, ref lpDCB))
				{
					Console.WriteLine(string.Format(format3, Marshal.GetLastWin32Error()));
					flag = false;
					isconnected = false;
					return false;
				}
				if (FileIO.GetCommTimeouts(com_port.port_fd, ref lpCommTimeouts))
				{
					lpCommTimeouts.ReadIntervalTimeout = com_port.Timeout;
					lpCommTimeouts.ReadTotalTimeoutMultiplier = com_port.Timeout;
					lpCommTimeouts.ReadTotalTimeoutConstant = com_port.readTimeout;
					lpCommTimeouts.WriteTotalTimeoutMultiplier = com_port.Timeout;
					lpCommTimeouts.WriteTotalTimeoutConstant = com_port.writeTimeout;
					if (!FileIO.SetCommTimeouts(com_port.port_fd, ref lpCommTimeouts))
					{
						Console.WriteLine(string.Format(format5, Marshal.GetLastWin32Error()));
						flag = false;
						isconnected = false;
						return false;
					}
					if (!FileIO.SetupComm(com_port.port_fd, com_port.max_to_read, com_port.max_to_write))
					{
						Console.WriteLine(string.Format(format6, Marshal.GetLastWin32Error()));
						flag = false;
						isconnected = false;
						return false;
					}
					isconnected = !com_port.port_fd.IsInvalid && !com_port.port_fd.IsClosed;
					comstart = cOMSTAT;
					return isconnected;
				}
				Console.WriteLine(string.Format(format4, Marshal.GetLastWin32Error()));
				flag = false;
				isconnected = false;
				return false;
			}
			Console.WriteLine(string.Format(format2, Marshal.GetLastWin32Error()));
			flag = false;
			isconnected = false;
			return false;
		}
		catch (Exception ex)
		{
			Console.WriteLine(text2 + ex.Message);
			bool flag = (byte)num != 0;
			isconnected = (byte)num != 0;
			return flag;
		}
	}

	public bool Flush()
	{
		if (com_port.port_fd == null)
		{
			return false;
		}
		if (!com_port.port_fd.IsInvalid)
		{
			return FileIO.PurgeComm(com_port.port_fd, 15u);
		}
		return false;
	}

	private bool WriteAll(byte[] buffer)
	{
		object obj = null;
		if (com_port.port_fd != null && isconnected && buffer != obj)
		{
			int num = 0;
			int num2 = buffer.Length;
			while (num2 > 0)
			{
				int val = ((com_port.max_to_write <= 0) ? num2 : com_port.max_to_write);
				int num3 = Math.Min(num2, val);
				byte[] array = new byte[num3];
				Buffer.BlockCopy(buffer, num, array, 0, num3);
				if (!FileIO.WriteFile(com_port.port_fd, array, num3, out var lpNumberOfBytesWritten, IntPtr.Zero))
				{
					return false;
				}
				if (lpNumberOfBytesWritten <= 0)
				{
					return false;
				}
				num += lpNumberOfBytesWritten;
				num2 -= lpNumberOfBytesWritten;
			}
			return true;
		}
		return false;
	}

	private byte[] ReadExact(int length)
	{
		byte[] array;
		int num2;
		int lpNumberOfBytesRead;
		int num;
		int num4;
		int num6;
		int val;
		int num3;
		byte[] array2;
		if (com_port.port_fd != null && isconnected && length > 0)
		{
			array = new byte[length];
			num = 0;
			num2 = length;
			if (num2 <= 0)
			{
				return array;
			}
			if (com_port.max_to_read <= 0)
			{
				val = num2;
				num3 = Math.Min(num2, val);
				array2 = new byte[num3];
				if (FileIO.ReadFile(com_port.port_fd, array2, num3, out lpNumberOfBytesRead, IntPtr.Zero))
				{
					if (lpNumberOfBytesRead > 0)
					{
						Buffer.BlockCopy(array2, 0, array, 0, lpNumberOfBytesRead);
						num = 0 + lpNumberOfBytesRead;
						num4 = 302880627;
						goto IL_015c;
					}
					return Array.Empty<byte>();
				}
				return Array.Empty<byte>();
			}
			val = com_port.max_to_read;
			num3 = Math.Min(num2, val);
			array2 = new byte[num3];
			if (FileIO.ReadFile(com_port.port_fd, array2, num3, out lpNumberOfBytesRead, IntPtr.Zero))
			{
				if (lpNumberOfBytesRead > 0)
				{
					Buffer.BlockCopy(array2, 0, array, 0, lpNumberOfBytesRead);
					num = 0 + lpNumberOfBytesRead;
					int num5 = 302880627;
					while (true)
					{
						_ = num5 ^ 0x216ADED9;
						num2 -= lpNumberOfBytesRead;
						if (num2 <= 0)
						{
							return array;
						}
						if (com_port.max_to_read <= 0)
						{
							break;
						}
						val = com_port.max_to_read;
						num3 = Math.Min(num2, val);
						array2 = new byte[num3];
						if (FileIO.ReadFile(com_port.port_fd, array2, num3, out lpNumberOfBytesRead, IntPtr.Zero))
						{
							if (lpNumberOfBytesRead > 0)
							{
								Buffer.BlockCopy(array2, 0, array, num, lpNumberOfBytesRead);
								num += lpNumberOfBytesRead;
								num5 = 302880627;
								continue;
							}
							return Array.Empty<byte>();
						}
						return Array.Empty<byte>();
					}
					num6 = 1186468203;
					goto IL_020d;
				}
				return Array.Empty<byte>();
			}
			return Array.Empty<byte>();
		}
		return Array.Empty<byte>();
		IL_020d:
		_ = num6 ^ 0x216ADED9;
		val = num2;
		goto IL_0227;
		IL_015c:
		_ = num4 ^ 0x216ADED9;
		num2 -= lpNumberOfBytesRead;
		if (num2 <= 0)
		{
			return array;
		}
		if (com_port.max_to_read <= 0)
		{
			num6 = 1186468203;
			goto IL_020d;
		}
		val = com_port.max_to_read;
		goto IL_0227;
		IL_0227:
		num3 = Math.Min(num2, val);
		array2 = new byte[num3];
		if (FileIO.ReadFile(com_port.port_fd, array2, num3, out lpNumberOfBytesRead, IntPtr.Zero))
		{
			if (lpNumberOfBytesRead > 0)
			{
				Buffer.BlockCopy(array2, 0, array, num, lpNumberOfBytesRead);
				num += lpNumberOfBytesRead;
				num4 = 302880627;
				goto IL_015c;
			}
			return Array.Empty<byte>();
		}
		return Array.Empty<byte>();
	}

	public bool write(byte[] buffer)
	{
		object obj = null;
		int num = 0;
		int num2 = 1;
		string format = "error: {0}";
		if ((com_port.port_fd == obj) | !isconnected)
		{
			return false;
		}
		int num3 = 0;
		int num4 = buffer.Length;
		ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
		NativeOverlapped lpOverlapped = new NativeOverlapped
		{
			OffsetLow = 0,
			OffsetHigh = 0,
			EventHandle = manualResetEvent.SafeWaitHandle.DangerousGetHandle()
		};
		while (num3 < num4)
		{
			try
			{
				if ((isconnected ? 1 : 0) != num)
				{
					if (!FileIO.WriteFile(com_port.port_fd, buffer, Math.Min(num4 - num3, com_port.max_to_write), out var lpNumberOfBytesWritten, ref lpOverlapped))
					{
						return (byte)num != 0;
					}
					if (FileIO.GetOverlappedResult(com_port.port_fd, ref lpOverlapped, ref lpNumberOfBytesWritten, (byte)num2 != 0))
					{
						num3 += lpNumberOfBytesWritten;
						continue;
					}
					return (byte)num != 0;
				}
				return (byte)num != 0;
			}
			catch (Exception arg)
			{
				Console.WriteLine(string.Format(format, arg));
				return (byte)num != 0;
			}
		}
		return (byte)num2 != 0;
	}

	public byte[] read(bool setting = false)
	{
		object obj = null;
		int num = 0;
		int bWait = 1;
		string format = "error: {0}";
		if (!((com_port.port_fd == obj) | !isconnected))
		{
			int lpNumberOfBytesRead = 0;
			int num2 = (setting ? bytesToread : com_port.max_to_read);
			int num3 = 0;
			byte[] array = new byte[num2];
			ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
			NativeOverlapped lpOverlapped = new NativeOverlapped
			{
				OffsetLow = 0,
				OffsetHigh = 0,
				EventHandle = manualResetEvent.SafeWaitHandle.DangerousGetHandle()
			};
			while (num3 < num2)
			{
				try
				{
					if ((isconnected ? 1 : 0) != num)
					{
						if (!FileIO.ReadFile(com_port.port_fd, array, Math.Min(num2, com_port.max_to_read), out lpNumberOfBytesRead, ref lpOverlapped))
						{
							return new byte[num];
						}
						if (FileIO.GetOverlappedResult(com_port.port_fd, ref lpOverlapped, ref lpNumberOfBytesRead, (byte)bWait != 0))
						{
							num3 += array.Length;
							continue;
						}
						return new byte[num];
					}
					return new byte[num];
				}
				catch (Exception arg)
				{
					Console.WriteLine(string.Format(format, arg));
					return new byte[num];
				}
			}
			Array.Resize(ref array, lpNumberOfBytesRead);
			return array;
		}
		return new byte[0];
	}

	public bool writeusb(byte[] buffer)
	{
		string format = "error: {0}";
		int result = 0;
		try
		{
			return WriteAll(buffer);
		}
		catch (Exception arg)
		{
			Console.WriteLine(string.Format(format, arg));
			return (byte)result != 0;
		}
	}

	public byte[] readusb(int? resplen)
	{
		object obj = null;
		string format = "error: {0}";
		if (!((com_port.port_fd == obj) | !isconnected))
		{
			if (!resplen.HasValue || resplen.Value <= 0)
			{
				return Array.Empty<byte>();
			}
			try
			{
				return ReadExact(resplen.Value);
			}
			catch (Exception arg)
			{
				Console.WriteLine(string.Format(format, arg));
				return Array.Empty<byte>();
			}
		}
		return Array.Empty<byte>();
	}

	public bool DiscardInBuffer()
	{
		return FileIO.PurgeComm(com_port.port_fd, 10u);
	}

	public bool DiscardOutBuffer()
	{
		return FileIO.PurgeComm(com_port.port_fd, 5u);
	}

	public bool IsClose()
	{
		object obj = null;
		int num = 0;
		try
		{
			if (com_port.port_fd !=null)
			{
				Thread.Sleep(500);
				com_port.port_fd?.Close();
				((SafeHandle)(object)com_port.port_fd)?.Dispose();
				com_port.port_fd = (SafeFileHandle)obj;
				isconnected = false;
			}
		}
		catch
		{
		}
		bool result = (byte)num != 0;
		isconnected = (byte)num != 0;
		return result;
	}

	void IDisposable.Dispose()
	{
		Dispose();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		object obj = null;
		if (disposing && com_port.port_fd !=null)
		{
			SafeFileHandle port_fd = com_port.port_fd;
			if (port_fd == null)
			{
				SafeFileHandle port_fd2 = com_port.port_fd;
				if (port_fd2 == null)
				{
					com_port.port_fd = (SafeFileHandle)obj;
				}
				else
				{
					port_fd2.Close();
					com_port.port_fd = (SafeFileHandle)obj;
				}
				isconnected = false;
				return;
			}
			((SafeHandle)(object)port_fd).Dispose();
			SafeFileHandle port_fd3 = com_port.port_fd;
			if (port_fd3 == null)
			{
				com_port.port_fd = (SafeFileHandle)obj;
			}
			else
			{
				port_fd3.Close();
				com_port.port_fd = (SafeFileHandle)obj;
			}
			isconnected = false;
		}
		else
		{
			isconnected = false;
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_03()
	{
		return new int[0];
	}
}
