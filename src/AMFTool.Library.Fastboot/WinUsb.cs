using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Restored.Namespace13;

namespace AMFTool.Library.Fastboot;

public class WinUsb : IDisposable
{
	private IntPtr deviceHandle = IntPtr.Zero;

	private IntPtr winusbHandle = IntPtr.Zero;

	private byte bulkInPipe;

	private byte bulkOutPipe;

	private int defaultTimeout;

	private int defaultCommandTimeout;

	private static Guid[] GUID;

	public bool IsOpen => winusbHandle != IntPtr.Zero;

	public List<(string DevicePath, string InstanceId, string Serial)> EnumerateDevicePaths(int vid, int pid)
	{
		int num = 0;
		int b03061AE = 18;
		int num2 = 8;
		int num3 = 6;
		int offset = 4;
		int num4 = 1;
		List<(string, string, string)> list = new List<(string, string, string)>();
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		Guid[] gUID = GUID;
		for (int i = 0; i < gUID.Length; i += num4)
		{
			Guid guid = gUID[i];
			Guid C1D3F98A = guid;
			IntPtr intPtr = Class36.SetupDiGetClassDevs(ref C1D3F98A, IntPtr.Zero, IntPtr.Zero, (uint)b03061AE);
			if (intPtr == Class36.field_04)
			{
				continue;
			}
			try
			{
				Class36.SP_DEVICE_INTERFACE_DATA F4C8B78E = default(Class36.SP_DEVICE_INTERFACE_DATA);
				F4C8B78E.cbSize = Marshal.SizeOf(F4C8B78E);
				for (uint num5 = (uint)num; Class36.SetupDiEnumDeviceInterfaces(intPtr, IntPtr.Zero, ref C1D3F98A, num5, ref F4C8B78E); num5 += (uint)num4)
				{
					int D9B71DEF = num;
					Class36.SetupDiGetDeviceInterfaceDetail(intPtr, ref F4C8B78E, IntPtr.Zero, num, ref D9B71DEF, IntPtr.Zero);
					if (((D9B71DEF > num) ? 1 : 0) == num)
					{
						continue;
					}
					IntPtr intPtr2 = Marshal.AllocHGlobal(D9B71DEF);
					try
					{
						Marshal.WriteInt32(intPtr2, (IntPtr.Size == num2) ? num2 : num3);
						if ((Class36.SetupDiGetDeviceInterfaceDetail(intPtr, ref F4C8B78E, intPtr2, D9B71DEF, ref D9B71DEF, IntPtr.Zero) ? 1 : 0) == num)
						{
							continue;
						}
						string text = Marshal.PtrToStringAuto(IntPtr.Add(intPtr2, offset));
						if (!string.IsNullOrWhiteSpace(text))
						{
							(int, int) tuple = ParseVidPid(string.Empty, text);
							if (((vid <= num || pid <= num) ? num : ((tuple.Item1 == vid) ? ((((tuple.Item2 == pid) ? 1 : 0) == num) ? 1 : 0) : num4)) == 0 && (hashSet.Add(text) ? 1 : 0) != num)
							{
								string item = ExtractSerialFromPath(text);
								list.Add((text, text, item));
							}
						}
					}
					finally
					{
						Marshal.FreeHGlobal(intPtr2);
					}
				}
			}
			finally
			{
				Class36.SetupDiDestroyDeviceInfoList(intPtr);
			}
		}
		return list;
	}

	private string ExtractSerialFromPath(string devicePath)
	{
		string value = "{";
		if (!string.IsNullOrWhiteSpace(devicePath))
		{
			string[] array = devicePath.Split('#');
			if (array.Length >= 3)
			{
				string text = array[2];
				if (string.IsNullOrWhiteSpace(text))
				{
					return string.Empty;
				}
				if (text.StartsWith(value, StringComparison.OrdinalIgnoreCase))
				{
					return string.Empty;
				}
				return text;
			}
			return string.Empty;
		}
		return string.Empty;
	}

	public (int Vid, int Pid) ParseVidPid(string instanceId, string devicePath)
	{
		string text = " ";
		string value = "vid_";
		string value2 = "pid_";
		int result = 0;
		int result2 = 0;
		string text2 = (instanceId + text + devicePath).ToLowerInvariant();
		int num = text2.IndexOf(value, StringComparison.OrdinalIgnoreCase);
		int num2 = text2.IndexOf(value2, StringComparison.OrdinalIgnoreCase);
		if (num >= 0)
		{
			if (num + 8 <= text2.Length)
			{
				int.TryParse(text2.Substring(num + 4, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
				if (num2 < 0 || num2 + 8 > text2.Length)
				{
					return (Vid: result, Pid: result2);
				}
				int.TryParse(text2.Substring(num2 + 4, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result2);
				return (Vid: result, Pid: result2);
			}
			if (num2 < 0 || num2 + 8 > text2.Length)
			{
				return (Vid: result, Pid: result2);
			}
			int.TryParse(text2.Substring(num2 + 4, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result2);
			return (Vid: result, Pid: result2);
		}
		if (num2 < 0 || num2 + 8 > text2.Length)
		{
			return (Vid: result, Pid: result2);
		}
		int.TryParse(text2.Substring(num2 + 4, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result2);
		return (Vid: result, Pid: result2);
	}

	public bool Open(string devicePath, int timeout, int commandTimeout)
	{
		defaultTimeout = timeout;
		defaultCommandTimeout = commandTimeout;
		deviceHandle = Class36.CreateFile(devicePath, 3221225472u, 3u, IntPtr.Zero, 3u, 1073741824u, IntPtr.Zero);
		bool flag;
		if (!(deviceHandle == Class36.field_04))
		{
			flag = deviceHandle == Class36.field_04;
		}
		else
		{
			deviceHandle = Class36.CreateFile(devicePath, 3221225472u, 3u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
			flag = deviceHandle == Class36.field_04;
		}
		if (!flag)
		{
			if (Class36.WinUsb_Initialize(deviceHandle, out winusbHandle))
			{
				if (!ResolveBulkEndpoints())
				{
					Close();
					return false;
				}
				return true;
			}
			Close();
			return false;
		}
		return false;
	}

	private bool ResolveBulkEndpoints()
	{
		bulkInPipe = 0;
		bulkOutPipe = 0;
		if (Class36.WinUsb_QueryInterfaceSettings(winusbHandle, 0, out var A7DDBA7C))
		{
			byte b = 0;
			if (0 >= A7DDBA7C.bNumEndpoints)
			{
				if (bulkInPipe != 0)
				{
					return bulkOutPipe != 0;
				}
				return false;
			}
			Class36.WINUSB_PIPE_INFORMATION CD21C2A;
			bool flag = !Class36.WinUsb_QueryPipe(winusbHandle, 0, b, out CD21C2A);
			int num = 173251372;
			while (true)
			{
				_ = num ^ -869701191;
				if (flag)
				{
					b++;
				}
				else if (CD21C2A.PipeType == Class36.USBD_PIPE_TYPE.UsbdPipeTypeBulk)
				{
					if ((CD21C2A.PipeId & 0x80) != 0)
					{
						bulkInPipe = CD21C2A.PipeId;
					}
					else
					{
						bulkOutPipe = CD21C2A.PipeId;
					}
					b++;
				}
				else
				{
					b++;
				}
				if (b >= A7DDBA7C.bNumEndpoints)
				{
					break;
				}
				flag = !Class36.WinUsb_QueryPipe(winusbHandle, 0, b, out CD21C2A);
				num = 173251372;
			}
			if (bulkInPipe != 0)
			{
				return bulkOutPipe != 0;
			}
			return false;
		}
		bool flag2 = false;
		return false;
	}

	private void SetTimeout(byte pipeId, int timeoutMs)
	{
		if (timeoutMs > 0)
		{
			uint B207E = (uint)timeoutMs;
			Class36.WinUsb_SetPipePolicy(winusbHandle, pipeId, 3u, 4u, ref B207E);
		}
		else
		{
			uint B207E = (uint)defaultCommandTimeout;
			Class36.WinUsb_SetPipePolicy(winusbHandle, pipeId, 3u, 4u, ref B207E);
		}
	}

	public int Write(byte[] data, int timeout)
	{
		int num;
		if (!IsOpen)
		{
			num = -1;
			return -1;
		}
		SetTimeout(bulkOutPipe, (timeout > 0) ? timeout : defaultTimeout);
		if (Class36.WinUsb_WritePipe(winusbHandle, bulkOutPipe, data, (uint)data.Length, out var C1ED8BC, IntPtr.Zero))
		{
			return (int)C1ED8BC;
		}
		num = -1;
		return -1;
	}

	public int Read(byte[] buffer, int timeout)
	{
		int num;
		if (!IsOpen)
		{
			num = -1;
			return -1;
		}
		SetTimeout(bulkInPipe, (timeout > 0) ? timeout : defaultCommandTimeout);
		if (Class36.WinUsb_ReadPipe(winusbHandle, bulkInPipe, buffer, (uint)buffer.Length, out var BBA39D, IntPtr.Zero))
		{
			return (int)BBA39D;
		}
		int lastWin32Error = Marshal.GetLastWin32Error();
		if (lastWin32Error == 121)
		{
			num = 0;
			return 0;
		}
		num = -1;
		return -1;
	}

	public void Close()
	{
		if (winusbHandle != IntPtr.Zero)
		{
			Class36.WinUsb_Free(winusbHandle);
			winusbHandle = IntPtr.Zero;
			if (deviceHandle != IntPtr.Zero && deviceHandle != Class36.field_04)
			{
				Class36.CloseHandle(deviceHandle);
				deviceHandle = IntPtr.Zero;
			}
		}
		else if (deviceHandle != IntPtr.Zero && deviceHandle != Class36.field_04)
		{
			Class36.CloseHandle(deviceHandle);
			deviceHandle = IntPtr.Zero;
		}
	}

	public void Dispose()
	{
		Close();
	}

	static WinUsb()
	{
		string g = "DEE824EF-729B-4A0E-9C14-B7117D33A817";
		string g2 = "F72FE0D4-CBCB-407D-8814-9ED673D0DD6B";
		string g3 = "A5DCBF10-6530-11D2-901F-00C04FB951ED";
		GUID = new Guid[3]
		{
			new Guid(g),
			new Guid(g2),
			new Guid(g3)
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
