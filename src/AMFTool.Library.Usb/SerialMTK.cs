using System;

namespace AMFTool.Library.Usb;

public class SerialMTK
{
	public static Native usb = new Native();

	public static bool Open(string port, int timeout = 1000)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		usb = new Native
		{
			PortName = port,
			BaudRate = 921600u,
			Timeout = timeout,
			max_to_read = 2097152,
			max_to_write = 2097152,
			readTimeout = 1000,
			writeTimeout = 1000
		};
		usb.Open();
		return usb.IsOpen;
	}

	public static bool Write(byte[] data)
	{
		if (JobStatus.Token.IsCancellationRequested)
		{
			return false;
		}
		return usb.writeusb(data);
	}

	public static byte[] Read(int len)
	{
		if (!JobStatus.Token.IsCancellationRequested)
		{
			return usb.readusb(len);
		}
		return Array.Empty<byte>();
	}

	public static void TimeoutSet()
	{
		usb.writeTimeout = 10;
		usb.readTimeout = 10;
	}

	public static void Close()
	{
		usb.Dispose();
	}

	public static void Flush()
	{
		usb.Flush();
	}

	public static bool Handshake(byte[] status = null)
	{
		int num = 0;
		int num2 = 1;
		int num3 = 255;
		int num4 = 10;
		int num5 = 0;
		byte[] array = new byte[4] { 160, 10, 80, 5 };
		int num6 = array.Length;
		int num7 = 0;
		while (num5 < num6)
		{
			try
			{
				byte[] array2 = new byte[num2];
				array2[num] = array[num5];
				Write(array2);
				status = Read(num2);
				if (((status == null) ? num : ((status[num] == (byte)(~array[num5] & num3)) ? 1 : 0)) == 0)
				{
					if (JobStatus.Token.IsCancellationRequested)
					{
						return (byte)num != 0;
					}
					usb.DiscardInBuffer();
					usb.DiscardOutBuffer();
					if (num7 == num4)
					{
						Close();
						return (byte)num != 0;
					}
					num7 += num2;
				}
				else
				{
					num5 += num2;
				}
			}
			catch
			{
				return (byte)num != 0;
			}
		}
		return (byte)num2 != 0;
	}

	public static bool Echo(object data, int size = 1)
	{
		byte[] array;
		if (size >= 4)
		{
			array = new byte[4];
			Binary.WriteI32BE(array, 0, (int)data);
			Write(array);
		}
		else
		{
			array = new byte[1] { (byte)(int)data };
			Write(array);
		}
		byte[] array2 = Read(size);
		if (array2 == null)
		{
			return false;
		}
		return ((ReadOnlySpan<byte>)array).SequenceEqual((ReadOnlySpan<byte>)array2);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
