using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Restored.Namespace10;

namespace AMFTool.Library.Usb;

public class SerialHisi
{
	private const int MAX_DATA_LEN = 1024;

	private static byte[] HeadFrame = new byte[4] { 254, 0, 255, 1 };

	private static byte[] DataFrame = new byte[1] { 218 };

	private static byte[] TailFrame = new byte[1] { 237 };

	private SerialPort Port;

	public bool Open(string portname)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Close();
		Port = new SerialPort
		{
			PortName = portname,
			BaudRate = 115200,
			DtrEnable = true,
			RtsEnable = true,
			ReadTimeout = 1000,
			WriteTimeout = 1000
		};
		try
		{
			Port.Open();
		}
		catch
		{
			Close();
		}
		return Port.IsOpen;
	}

	public void Close()
	{
		object port = null;
		if (Port != null && Port.IsOpen)
		{
			Port.Close();
			Port.Dispose();
			Port = (SerialPort)port;
		}
	}

	public void WriteData(byte[] data)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		try
		{
			Port.Write(data, 0, data.Length);
		}
		catch
		{
		}
	}

	public bool Write(byte[] data, uint address)
	{
		float field_ = Class38.field_3411;
		int num = data.Length;
		int num2 = num / (1024 + ((num % 1024 > 0) ? 1 : 0));
		int num3 = 0;
		int num4 = 0;
		SendHeadFrame(num, address);
		while (num > 1024)
		{
			byte[] array = new byte[1024];
			Buffer.BlockCopy(data, num4, array, 0, 1024);
			SendDataFrame(num3 + 1, array);
			num4 += 1024;
			num -= 1024;
			num3++;
			if (num3 % ((num2 > 250) ? 10 : 3) == 0)
			{
				UI.Progresbar1((long)(field_ * (float)num3 / (float)num2), 100L);
			}
		}
		if (num > 0)
		{
			byte[] array2 = new byte[num];
			Buffer.BlockCopy(data, num4, array2, 0, num);
			SendDataFrame(num3 + 1, array2);
		}
		UI.Progresbar1(100L, 100L);
		SendTailFrame(num3 + 2);
		return true;
	}

	private void SendHeadFrame(int length, uint address)
	{
		List<byte> list = new List<byte>(HeadFrame);
		byte[] bytes = BitConverter.GetBytes(length);
		Array.Reverse(bytes);
		list.AddRange(bytes);
		byte[] bytes2 = BitConverter.GetBytes(address);
		Array.Reverse(bytes2);
		list.AddRange(bytes2);
		SendFrame(list.ToArray());
	}

	private void SendDataFrame(int n, byte[] data)
	{
		List<byte> list = new List<byte>(DataFrame)
		{
			(byte)(n & 0xFF),
			(byte)(~n & 0xFF)
		};
		list.AddRange(data);
		SendFrame(list.ToArray());
	}

	private void SendTailFrame(int n)
	{
		List<byte> list = new List<byte>(TailFrame)
		{
			(byte)(n & 0xFF),
			(byte)(~n & 0xFF)
		};
		SendFrame(list.ToArray());
	}

	private void SendFrame(byte[] data)
	{
		ushort num = Class34.method_01(data);
		List<byte> list = new List<byte>(data)
		{
			(byte)((num >> 8) & 0xFF),
			(byte)(num & 0xFF)
		};
		int count = list.Count();
		byte[] buffer = list.ToArray();
		Port.Write(buffer, 0, count);
		Port.DiscardInBuffer();
		Port.DiscardOutBuffer();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
