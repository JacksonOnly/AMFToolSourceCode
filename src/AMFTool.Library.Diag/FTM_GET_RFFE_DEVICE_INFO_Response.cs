using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FTM_GET_RFFE_DEVICE_INFO_Response
{
	public byte cmd;

	public byte subsys;

	public ushort mode;

	public ushort ftm_cmd;

	public ushort status;

	public ushort mfg_id0;

	public ushort mfg_id1;

	public ushort mfg_id2;

	public ushort mfg_id3;

	public ushort mfg_id4;

	public ushort prd_id0;

	public ushort prd_id1;

	public ushort prd_id2;

	public ushort prd_id3;

	public ushort prd_id4;

	public ushort rev_id0;

	public ushort rev_id1;

	public ushort rev_id2;

	public ushort rev_id3;

	public ushort rev_id4;
}
