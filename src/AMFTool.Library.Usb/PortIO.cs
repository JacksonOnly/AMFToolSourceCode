using System;
using System.IO.Ports;
using System.Threading;

namespace AMFTool.Library.Usb;

public class PortIO
{
	public static SerialPort serialPort;

	public static byte[] resp = new byte[1];

	public static byte[] DataReadFlash = new byte[1];

	public static void Open(string PortCOMDiag)
	{
		string text = "Open Unisoc Diag Port at : ";
		Console.WriteLine(text + PortCOMDiag);
		serialPort = new SerialPort(PortCOMDiag);
		serialPort.BaudRate = 115200;
		serialPort.Parity = Parity.None;
		serialPort.Handshake = Handshake.None;
		serialPort.DataBits = 8;
		serialPort.StopBits = StopBits.One;
		serialPort.Open();
	}

	public static void Close()
	{
		if (serialPort.IsOpen)
		{
			serialPort.Close();
			serialPort.Dispose();
		}
	}

	public static byte[] Read()
	{
		if (serialPort.IsOpen)
		{
			int bytesToRead = serialPort.BytesToRead;
			byte[] array = new byte[bytesToRead];
			serialPort.BaseStream.Read(array, 0, bytesToRead);
			return array;
		}
		return new byte[1];
	}

	public static void Write(byte[] request)
	{
		if (serialPort.IsOpen)
		{
			serialPort.Write(request, 0, request.Length);
			Thread.Sleep(15);
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
