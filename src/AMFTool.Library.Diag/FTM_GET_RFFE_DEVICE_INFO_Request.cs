using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FTM_GET_RFFE_DEVICE_INFO_Request
{
	public byte rf_mode;

	public byte rfm_device;

	public byte rffe_device_type;
}
