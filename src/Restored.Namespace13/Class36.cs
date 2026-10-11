using System;
using System.Runtime.InteropServices;

namespace Restored.Namespace13;

internal static class Class36
{
	public enum USBD_PIPE_TYPE
	{
		UsbdPipeTypeControl,
		UsbdPipeTypeIsochronous,
		UsbdPipeTypeBulk,
		UsbdPipeTypeInterrupt
	}

	public struct USB_INTERFACE_DESCRIPTOR
	{
		public byte bLength;

		public byte bDescriptorType;

		public byte bInterfaceNumber;

		public byte bAlternateSetting;

		public byte bNumEndpoints;

		public byte bInterfaceClass;

		public byte bInterfaceSubClass;

		public byte bInterfaceProtocol;

		public byte iInterface;
	}

	public struct WINUSB_PIPE_INFORMATION
	{
		public USBD_PIPE_TYPE PipeType;

		public byte PipeId;

		public ushort MaximumPacketSize;

		public byte Interval;
	}

	public struct SP_DEVICE_INTERFACE_DATA
	{
		public int cbSize;

		public Guid InterfaceClassGuid;

		public int Flags;

		public IntPtr Reserved;
	}

	public const uint field_01 = 1073741824u;

	public const uint field_02 = 2u;

	public const uint field_03 = 1073741824u;

	public static readonly IntPtr field_04 = new IntPtr(-1);

	public const uint field_05 = 16u;

	public const int field_06 = 121;

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr CreateFile(string CBF98754, uint FE230489, uint FCFFA752, IntPtr ECA4B6A4, uint E2387C59, uint CE722D13, IntPtr D4789415);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool CloseHandle(IntPtr D32E37D7);

	[DllImport("winusb.dll", SetLastError = true)]
	public static extern bool WinUsb_Initialize(IntPtr EB89949C, out IntPtr FDF9C62C);

	[DllImport("winusb.dll", SetLastError = true)]
	public static extern bool WinUsb_Free(IntPtr D414B507);

	[DllImport("winusb.dll", SetLastError = true)]
	public static extern bool WinUsb_QueryInterfaceSettings(IntPtr AE675F38, byte B2992A1A, out USB_INTERFACE_DESCRIPTOR A7DDBA7C);

	[DllImport("winusb.dll", SetLastError = true)]
	public static extern bool WinUsb_QueryPipe(IntPtr ED8C459E, byte E5C943D3, byte D80BFACD, out WINUSB_PIPE_INFORMATION CD21C2A8);

	[DllImport("winusb.dll", SetLastError = true)]
	public static extern bool WinUsb_SetPipePolicy(IntPtr D7AAE249, byte AE4ED55A, uint F0AC09E4, uint FCDE97C6, ref uint B207E659);

	[DllImport("winusb.dll", SetLastError = true)]
	public static extern bool WinUsb_WritePipe(IntPtr FC40E392, byte C20239E8, byte[] AC6839B3, uint E4D6F783, out uint C1ED8BC2, IntPtr D0DF9465);

	[DllImport("winusb.dll", SetLastError = true)]
	public static extern bool WinUsb_ReadPipe(IntPtr A7D7AF02, byte E096C769, byte[] FEB20F0D, uint BBB03CF1, out uint BBA39D66, IntPtr F39BD463);

	[DllImport("setupapi.dll", SetLastError = true)]
	public static extern IntPtr SetupDiGetClassDevs(ref Guid C1D3F98A, IntPtr E21ACADE, IntPtr CAFD52B4, uint B03061AE);

	[DllImport("setupapi.dll", SetLastError = true)]
	public static extern bool SetupDiEnumDeviceInterfaces(IntPtr A5B22573, IntPtr AB1DC0A2, ref Guid D3B5E5F9, uint FEA479ED, ref SP_DEVICE_INTERFACE_DATA F4C8B78E);

	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr A5BA57D6, ref SP_DEVICE_INTERFACE_DATA E012AE78, IntPtr BB8681D1, int DDE798F, ref int D9B71DEF, IntPtr D05A3A61);

	[DllImport("setupapi.dll", SetLastError = true)]
	public static extern bool SetupDiDestroyDeviceInfoList(IntPtr CA28AC9C);
}
