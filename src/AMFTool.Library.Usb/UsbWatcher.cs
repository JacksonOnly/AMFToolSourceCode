using System;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AMFTool.Library.Usb;

public class UsbWatcher : NativeWindow
{
	public struct DEV_BROADCAST_DEVICEINTERFACE
	{
		public int dbcc_size;

		public int dbcc_devicetype;

		public int dbcc_reserved;

		public Guid dbcc_classguid;

		public short dbcc_name;
	}

	public struct SP_DEVINFO_DATA
	{
		public int cbSize;

		public Guid ClassGuid;

		public uint DevInst;

		public IntPtr Reserved;
	}

	private IntPtr notifyHandle;

	private static readonly Guid[] usbGUID;

	public const int DBT_DEVICEREMOVECOMPLETE = 32772;

	public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;

	public const int DIGCF_ALLCLASSES = 4;

	public UsbWatcher(IntPtr handle)
	{
		AssignHandle(handle);
		Guid[] array = usbGUID;
		foreach (Guid g in array)
		{
			RegisterInterface(handle, g);
		}
	}

	private void RegisterInterface(IntPtr h, Guid g)
	{
		DEV_BROADCAST_DEVICEINTERFACE filter = new DEV_BROADCAST_DEVICEINTERFACE
		{
			dbcc_size = Marshal.SizeOf(typeof(DEV_BROADCAST_DEVICEINTERFACE)),
			dbcc_devicetype = 5,
			dbcc_classguid = g
		};
		notifyHandle = RegisterDeviceNotification(h, ref filter, 0);
	}

	protected override void WndProc(ref Message m)
	{
		base.WndProc(ref m);
		if (m.Msg == 537)
		{
			int num = m.WParam.ToInt32();
			if (num == 32772)
			{
				OnDeviceRemoved();
			}
		}
	}

	private void OnDeviceRemoved()
	{
		string message = "Init Reset : Device Removed";
		if (string.IsNullOrEmpty(DeviceState.LastFingerprint))
		{
			return;
		}
		string[] array = DeviceState.LastFingerprint.Split(':');
		if (array.Length >= 3)
		{
			string vid = array[0];
			string pid = array[1];
			string text = array[2];
			string[] portNames = SerialPort.GetPortNames();
			if (!portNames.Contains(text, StringComparer.OrdinalIgnoreCase))
			{
				DeviceState.ResetInit(message);
				SerialMTK.Close();
			}
			else if (!CheckVidPid(vid, pid, text))
			{
				DeviceState.ResetInit(message);
				SerialMTK.Close();
			}
		}
	}

	private bool CheckVidPid(string vid, string pid, string com)
	{
		string enumerator = "USB";
		object deviceInstanceId = null;
		string text = "VID_";
		string text2 = "PID_";
		string text3 = "(";
		string text4 = ")";
		IntPtr intPtr = SetupDiGetClassDevs(IntPtr.Zero, enumerator, IntPtr.Zero, 6);
		if (intPtr == IntPtr.Zero)
		{
			return true;
		}
		SP_DEVINFO_DATA DeviceInfoData = new SP_DEVINFO_DATA
		{
			cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA))
		};
		for (uint num = 0u; SetupDiEnumDeviceInfo(intPtr, num, ref DeviceInfoData); num++)
		{
			uint RequiredSize = 0u;
			SetupDiGetDeviceInstanceId(intPtr, ref DeviceInfoData, (StringBuilder)deviceInstanceId, 0u, ref RequiredSize);
			if (RequiredSize == 0)
			{
				continue;
			}
			StringBuilder stringBuilder = new StringBuilder((int)RequiredSize);
			SetupDiGetDeviceInstanceId(intPtr, ref DeviceInfoData, stringBuilder, RequiredSize, ref RequiredSize);
			string text5 = stringBuilder.ToString().ToUpperInvariant();
			if (text5.Contains(text + vid) && text5.Contains(text2 + pid))
			{
				string friendlyName = GetFriendlyName(intPtr, DeviceInfoData);
				if (friendlyName.Contains(text3 + com + text4))
				{
					SetupDiDestroyDeviceInfoList(intPtr);
					return true;
				}
			}
		}
		SetupDiDestroyDeviceInfoList(intPtr);
		return false;
	}

	private string GetFriendlyName(IntPtr h, SP_DEVINFO_DATA info)
	{
		object propertyBuffer = null;
		string result = "";
		SetupDiGetDeviceRegistryProperty(h, ref info, 12u, out var PropertyRegDataType, (byte[])propertyBuffer, 0u, out var RequiredSize);
		if (RequiredSize != 0)
		{
			byte[] array = new byte[RequiredSize];
			SetupDiGetDeviceRegistryProperty(h, ref info, 12u, out PropertyRegDataType, array, RequiredSize, out RequiredSize);
			return Encoding.Unicode.GetString(array).TrimEnd(default(char));
		}
		return result;
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, ref DEV_BROADCAST_DEVICEINTERFACE filter, int flags);

	[DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
	private static extern IntPtr SetupDiGetClassDevs(IntPtr ClassGuid, string Enumerator, IntPtr hwndParent, int Flags);

	[DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
	private static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

	[DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
	private static extern bool SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, StringBuilder DeviceInstanceId, uint DeviceInstanceIdSize, ref uint RequiredSize);

	[DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
	private static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, uint Property, out uint PropertyRegDataType, byte[] PropertyBuffer, uint PropertyBufferSize, out uint RequiredSize);

	[DllImport("setupapi.dll")]
	private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

	static UsbWatcher()
	{
		string g = "A5DCBF10-6530-11D2-901F-00C04FB951ED";
		string g2 = "53F56307-B6BF-11D0-94F2-00A0C91EFB8B";
		string g3 = "4D1E55B2-F16F-11CF-88CB-001111000030";
		string g4 = "AD498944-762F-11D0-8DCB-00C04FC3358C";
		string g5 = "219D0508-57A8-4FF5-97A1-BD86587C6C7E";
		string g6 = "4D36E967-E325-11CE-BFC1-08002BE10318";
		string g7 = "86E0D1E0-8089-11D0-9CE4-08003E301F73";
		usbGUID = new Guid[7]
		{
			new Guid(g),
			new Guid(g2),
			new Guid(g3),
			new Guid(g4),
			new Guid(g5),
			new Guid(g6),
			new Guid(g7)
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
