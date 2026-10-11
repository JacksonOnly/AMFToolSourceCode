using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FTM_GET_RFFE_DEVICE_IDS_Response
{
	public ushort status;

	public ushort num_device_instances;

	public FTM_RFFE_Info_Type[] device_info_ids;
}
