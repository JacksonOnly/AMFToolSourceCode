using Microsoft.Win32.SafeHandles;

namespace AMFTool.Library.Usb;

public struct ComPort
{
	public SafeFileHandle port_fd;

	public string port_name;

	public uint BaudRate;

	public int Timeout;

	public int readTimeout;

	public int writeTimeout;

	public int max_to_write;

	public int max_to_read;
}
