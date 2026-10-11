using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace AMFTool.Library.Usb;

public class FileIO
{
	public enum FileAccess : uint
	{
		GENERIC_READ = 2147483648u,
		GENERIC_WRITE = 1073741824u
	}

	public enum FileShareMode : uint
	{
		FILE_SHARE_READ = 1u,
		FILE_SHARE_WRITE
	}

	public enum FilePosition : uint
	{
		CREATE_NEW = 1u,
		CREATE_ALWAYS,
		OPEN_EXISTING,
		OPEN_ALWAYS,
		TRUNCATE_EXISTING
	}

	public enum FileAttributes : uint
	{
		FILE_ATTRIBUTE_NORMAL = 0x80u
	}

	public struct COMMTIMEOUTS
	{
		public int ReadIntervalTimeout;

		public int ReadTotalTimeoutMultiplier;

		public int ReadTotalTimeoutConstant;

		public int WriteTotalTimeoutMultiplier;

		public int WriteTotalTimeoutConstant;
	}

	public struct COMSTAT
	{
		public uint Flags;

		public uint cbInQue;

		public uint cbOutQue;
	}

	public struct COMMPROP
	{
		public ushort wPacketLength;

		public ushort wPacketVersion;

		public int dwServiceMask;

		public int dwReserved1;

		public int dwMaxTxQueue;

		public int dwMaxRxQueue;

		public int dwMaxBaud;

		public int dwProvSubType;

		public int dwProvCapabilities;

		public int dwSettableParams;

		public int dwSettableBaud;

		public ushort wSettableData;

		public ushort wSettableStopParity;

		public int dwCurrentTxQueue;

		public int dwCurrentRxQueue;

		public int dwProvSpec1;

		public int dwProvSpec2;

		public char wcProvChar;
	}

	public struct DCB
	{
		public uint DCBlength;

		public uint BaudRate;

		public uint Flags;

		public ushort wReserved;

		public ushort XonLim;

		public ushort XoffLim;

		public byte ByteSize;

		public byte Parity;

		public byte StopBits;

		public byte XonChar;

		public byte XoffChar;

		public byte ErrorChar;

		public byte EofChar;

		public byte EvtChar;

		public ushort wReserved1;
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShareMode dwShareMode, IntPtr lpSecurityAttributes, FilePosition dwCreationDisposition, FileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool SetCommTimeouts(SafeFileHandle hCommDev, ref COMMTIMEOUTS lpCommTimeouts);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool GetCommTimeouts(SafeFileHandle hCommDev, ref COMMTIMEOUTS lpCommTimeouts);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, out int lpNumberOfBytesWritten, [In] ref NativeOverlapped lpOverlapped);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, out int lpNumberOfBytesWritten, IntPtr lpOverlapped);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool ReadFile(SafeFileHandle hFile, [In][Out] byte[] lpBuffer, int nNumberOfBytesToRead, out int lpNumberOfBytesRead, [In] ref NativeOverlapped lpOverlapped);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool ReadFile(SafeFileHandle hFile, [In][Out] byte[] lpBuffer, int nNumberOfBytesToRead, out int lpNumberOfBytesRead, IntPtr lpOverlapped);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool CloseHandle(SafeFileHandle hCommDev);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool GetOverlappedResult(SafeFileHandle hFile, ref NativeOverlapped lpOverlapped, ref int nNumberOfBytesTransferred, bool bWait);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool ClearCommError(SafeFileHandle hFile, ref int lpErrors, ref COMSTAT lpStat);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool SetupComm(SafeFileHandle hFile, int dwInQueue, int dwOutQueue);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool GetCommProperties(SafeFileHandle hFile, ref COMMPROP lpCommProp);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool GetCommModemStatus(SafeFileHandle hFile, ref int lpModemStat);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern int GetFileType(SafeFileHandle hFile);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool PurgeComm(SafeFileHandle hFile, uint dwFlags);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool GetCommState(SafeFileHandle hFile, ref DCB lpDCB);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool SetCommState(SafeFileHandle hFile, ref DCB lpDCB);

	[DllImport("kernel32.dll")]
	internal static extern bool EscapeCommFunction(SafeFileHandle hFile, int dwFunc);
}
