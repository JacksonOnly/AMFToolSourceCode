using System;
using System.IO.Ports;
using System.Threading;

namespace AMFTool.Library.Usb;

public class SerialOdin : IDisposable
{
	public SerialPort Port;

	public bool IsOpen
	{
		get
		{
			if (Port != null)
			{
				return Port.IsOpen;
			}
			return false;
		}
	}

	public bool Open(string portname)
	{
		int result = 0;
		Close();
		Port = new SerialPort
		{
			PortName = portname,
			BaudRate = 115200,
			Parity = Parity.None,
			DataBits = 8,
			StopBits = StopBits.One,
			Handshake = Handshake.RequestToSend,
			DtrEnable = false,
			RtsEnable = false,
			ReadTimeout = 10000,
			WriteTimeout = 10000
		};
		try
		{
			Port.Open();
			Port.DiscardInBuffer();
			Port.DiscardOutBuffer();
			return true;
		}
		catch
		{
			Close();
			return (byte)result != 0;
		}
	}

	public void Write(byte[] data, int len)
	{
		bool flag = Port == null || !Port.IsOpen;
		if (!flag && !JobStatus.Token.IsCancellationRequested)
		{
			Port.Write(data, 0, len);
		}
	}

	public byte[] Read(int len = 0)
	{
		if (Port != null)
		{
			if (!Port.IsOpen)
			{
				return Array.Empty<byte>();
			}
			if (!JobStatus.Token.IsCancellationRequested)
			{
				byte[] array = new byte[len];
				int num = 0;
				if (0 >= len)
				{
					return array;
				}
				int num2 = Port.Read(array, 0, len);
				if (num2 > 0)
				{
					num = 0 + num2;
					int num3 = 665639828;
					while (true)
					{
						_ = num3 ^ -883577717;
						if (num >= len)
						{
							return array;
						}
						num2 = Port.Read(array, num, len - num);
						if (num2 <= 0)
						{
							break;
						}
						num += num2;
						num3 = 665639828;
					}
					return Array.Empty<byte>();
				}
				return Array.Empty<byte>();
			}
			return Array.Empty<byte>();
		}
		return Array.Empty<byte>();
	}

	public string ReadExisting()
	{
		return Port.ReadExisting();
	}

	public void Close()
	{
		object obj = null;
		int millisecondsTimeout = 300;
		if (Port == obj)
		{
			return;
		}
		try
		{
			if (Port.IsOpen)
			{
				Port.Close();
			}
		}
		catch
		{
		}
		finally
		{
			try
			{
				Port.Dispose();
			}
			catch
			{
			}
			Port = (SerialPort)obj;
			Thread.Sleep(millisecondsTimeout);
		}
	}

	public void Dispose()
	{
		if (!Port.IsOpen)
		{
			Port.Dispose();
			return;
		}
		Port.Close();
		Port.Dispose();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
