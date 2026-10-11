namespace AMFTool.Library.Usb;

public class SerialMETA
{
	public static Native2 usb = new Native2();

	public static bool Open(string port)
	{
		usb.Open(port);
		return usb.IsOpen;
	}

	public static bool write(byte[] data)
	{
		return usb.write(data);
	}

	public static byte[] read(int len)
	{
		return usb.read(len);
	}

	public static void Close()
	{
		usb.Dispose();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
