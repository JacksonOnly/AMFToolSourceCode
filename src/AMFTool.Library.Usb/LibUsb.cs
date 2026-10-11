using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AMFTool.Library.Usb;

public class LibUsb
{
	private struct usb_device_descriptor
	{
		public byte bLength;

		public byte bDescriptorType;

		public ushort bcdUSB;

		public byte bDeviceClass;

		public byte bDeviceSubClass;

		public byte bDeviceProtocol;

		public byte bMaxPacketSize0;

		public ushort idVendor;

		public ushort idProduct;

		public ushort bcdDevice;

		public byte iManufacturer;

		public byte iProduct;

		public byte iSerialNumber;

		public byte bNumConfigurations;
	}

	private struct usb_device
	{
		public IntPtr next;

		public IntPtr prev;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string filename;

		public IntPtr bus;

		public usb_device_descriptor descriptor;

		public IntPtr config;

		public IntPtr dev;

		public byte devnum;

		public byte num_children;

		public IntPtr children;
	}

	private struct usb_bus
	{
		public IntPtr next;

		public IntPtr prev;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string dirname;

		public IntPtr devices;

		public uint location;

		public IntPtr root_dev;
	}

	public struct ResponseCtrlTransfer
	{
		public bool usbFinded;

		public bool Transfered;
	}

	private const int MTK_PID_BROM = 3;

	private static IntPtr handle = IntPtr.Zero;

	private static bool disposed;

	public static string UtilsPathX64
	{
		get
		{
			string text = "\\usb\\x64\\install.exe";
			return utils.BaseDirectory + text;
		}
	}

	public static string UtilsPathX86
	{
		get
		{
			string text = "\\usb\\x32\\install.exe";
			return utils.BaseDirectory + text;
		}
	}

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void usb_init();

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int usb_find_busses();

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int usb_find_devices();

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr usb_get_busses();

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr usb_open(IntPtr dev);

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int usb_close(IntPtr dev);

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int usb_control_msg(IntPtr dev, int requesttype, int request, int value, int index, byte[] bytes, int size, int timeout);

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int usb_release_interface(IntPtr dev, int ifc_num);

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int usb_resetep(IntPtr dev, int ep);

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int usb_reset(IntPtr dev);

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int usb_claim_interface(IntPtr dev, int ifc_num);

	[DllImport("libusb0.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int usb_set_configuration(IntPtr dev, int configuration);

	public static bool InstallLibusb(string VID, string PID)
	{
		string text = "install --device=\"USB\\Vid_";
		string text2 = "&Pid_";
		string text3 = "\"";
		string value = "installer failed";
		string value2 = "err:";
		string text4;
		bool flag;
		if (!Environment.Is64BitOperatingSystem)
		{
			text4 = Excute(UtilsPathX86, text + VID + text2 + PID + text3);
			if (!string.IsNullOrEmpty(text4))
			{
				if (!text4.Contains(value))
				{
					return !text4.Contains(value2);
				}
				return false;
			}
			flag = false;
			return false;
		}
		text4 = Excute(UtilsPathX64, text + VID + text2 + PID + text3);
		if (!string.IsNullOrEmpty(text4))
		{
			if (!text4.Contains(value))
			{
				return !text4.Contains(value2);
			}
			return false;
		}
		flag = false;
		return false;
	}

	public static string Execute(string exe, string args, string workdir = "", int timeout = 0)
	{
		string verb = "runas";
		string result = "";
		Process process = new Process
		{
			EnableRaisingEvents = true,
			StartInfo = new ProcessStartInfo
			{
				FileName = exe,
				Verb = verb,
				Arguments = args,
				CreateNoWindow = true,
				LoadUserProfile = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true
			}
		};
		if (!string.IsNullOrEmpty(workdir))
		{
			process.StartInfo.WorkingDirectory = workdir;
		}
		if (process.Start())
		{
			StringBuilder so = new StringBuilder();
			process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					so.AppendLine(e.Data);
				}
			};
			StringBuilder se = new StringBuilder();
			process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					se.AppendLine(e.Data);
				}
			};
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			if (timeout <= 0)
			{
				process.WaitForExit();
			}
			else
			{
				process.WaitForExit(timeout);
			}
			string text = so.ToString();
			string text2 = se.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
		}
		return result;
	}

	private static string Excute(string fn, string args)
	{
		string workdir = "";
		return Execute(fn, args, workdir, 5000);
	}

	public static bool Open()
	{
		int result = 0;
		string format = "[USB] MTK device connected: VID=0x{0:X4}, PID=0x{1:X4}";
		string value = "No MTK USB device found";
		string value2 = "Error: libusb0.dll not found - please install libusb-win32 driver";
		string text = "Connection failed: ";
		try
		{
			InstallLibusb(Portinfo.VID, Portinfo.PID);
			usb_init();
			usb_find_busses();
			usb_find_devices();
			IntPtr intPtr = usb_get_busses();
			while (intPtr != IntPtr.Zero)
			{
				usb_bus usb_bus = Marshal.PtrToStructure<usb_bus>(intPtr);
				IntPtr intPtr2 = usb_bus.devices;
				while (intPtr2 != IntPtr.Zero)
				{
					usb_device usb_device = Marshal.PtrToStructure<usb_device>(intPtr2);
					if (usb_device.descriptor.idVendor == 3725 && (usb_device.descriptor.idProduct == 3 || usb_device.descriptor.idProduct == 8192))
					{
						handle = usb_open(intPtr2);
						if (handle != IntPtr.Zero)
						{
							Console.WriteLine(string.Format(format, usb_device.descriptor.idVendor, usb_device.descriptor.idProduct));
							return true;
						}
					}
					intPtr2 = usb_device.next;
				}
				intPtr = usb_bus.next;
			}
			Console.WriteLine(value);
			return false;
		}
		catch (DllNotFoundException)
		{
			Console.WriteLine(value2);
			return (byte)result != 0;
		}
		catch (Exception ex2)
		{
			Console.WriteLine(text + ex2.Message);
			return (byte)result != 0;
		}
	}

	public static bool ControlTransfer(byte bRequestType, byte bRequest, int wValue, int wIndex, object data_or_wLength, int len)
	{
		int result = 0;
		object obj = null;
		if (!EnsureOpen())
		{
			return false;
		}
		try
		{
			byte[] array = (byte[])obj;
			int num = 0;
			if (data_or_wLength !=null)
			{
				if (!utils.IsNumber(data_or_wLength))
				{
					byte[] array2 = data_or_wLength as byte[];
					if (array2 !=null)
					{
						array = array2;
						num = array2.Length;
					}
				}
				else
				{
					array = new byte[Convert.ToInt32(data_or_wLength)];
					num = array.Length;
				}
			}
			if (len > 0 && array != null && len <= array.Length)
			{
				num = len;
			}
			int num2 = usb_control_msg(handle, bRequestType, bRequest, wValue, wIndex, array, num, 1000);
			usb_release_interface(handle, 0);
			usb_resetep(handle, 0);
			return num2 >= 0 && (num2 == num || num2 == num + 8);
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public static ResponseCtrlTransfer ControlTransfer(byte bRequestType, byte bRequest, int wValue, int wIndex, object data_or_wLength)
	{
		int num = 0;
		object obj = null;
		string message = "Unsupported ControlTransfer data type";
		ResponseCtrlTransfer result = new ResponseCtrlTransfer
		{
			usbFinded = false,
			Transfered = false
		};
		if (!EnsureOpen())
		{
			return result;
		}
		try
		{
			result.usbFinded = true;
			byte[] bytes = (byte[])obj;
			int num2 = 0;
			if (data_or_wLength !=null)
			{
				if (!utils.IsNumber(data_or_wLength))
				{
					byte[] array = data_or_wLength as byte[];
					if (array ==null)
					{
						throw new Exception(message);
					}
					bytes = array;
					num2 = array.Length;
				}
				else
				{
					num2 = Convert.ToInt32(data_or_wLength);
					bytes = new byte[num2];
				}
			}
			int num3 = usb_control_msg(handle, bRequestType, bRequest, wValue, wIndex, bytes, num2, 1000);
			if (num3 >= 0 && (num3 == num2 || num3 == num2 + 8))
			{
				result.Transfered = true;
			}
		}
		catch
		{
			result.Transfered = (byte)num != 0;
		}
		finally
		{
			try
			{
				usb_release_interface(handle, num);
			}
			catch
			{
			}
			try
			{
				usb_resetep(handle, num);
			}
			catch
			{
			}
		}
		return result;
	}

	private static bool EnsureOpen()
	{
		if (handle != IntPtr.Zero)
		{
			return true;
		}
		return Open();
	}

	public static void Close(bool reset = false)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (handle == IntPtr.Zero)
		{
			return;
		}
		try
		{
			if (reset)
			{
				usb_reset(handle);
			}
			usb_close(handle);
		}
		catch
		{
		}
		finally
		{
			handle = IntPtr.Zero;
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
