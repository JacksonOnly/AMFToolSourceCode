using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FTM_RFFE_Info_Type
{
	public ushort mfg_id;

	public ushort prd_id;

	public ushort prd_rev;
}
