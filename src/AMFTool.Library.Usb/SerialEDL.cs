namespace AMFTool.Library.Usb;

public class SerialEDL
{
	public static Native usb = new Native();

	public static bool Open(string PortName, int Timeout)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		usb = new Native
		{
			PortName = PortName,
			BaudRate = 921600u,
			Timeout = Timeout,
			max_to_read = 1048576,
			max_to_write = 1048576,
			readTimeout = 1000,
			writeTimeout = 1000
		};
		usb.Open();
		return usb.IsOpen;
	}

	public static bool Write(byte[] send)
	{
		return usb.write(send);
	}

	public static byte[] Read(bool setting = false)
	{
		return usb.read(setting);
	}

	public static void Close()
	{
		usb.Dispose();
	}

	public static void Purgebuffer()
	{
		usb.DiscardInBuffer();
		usb.DiscardOutBuffer();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
