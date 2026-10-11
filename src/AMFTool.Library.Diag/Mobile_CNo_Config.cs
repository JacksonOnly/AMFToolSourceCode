namespace AMFTool.Library.Diag;

public class Mobile_CNo_Config
{
	public ushort iCollect_Mode;

	public ushort iCapture_Size;

	public ushort iNumber_FFT_Integrations;

	public int GL0_FrequencyID;

	public int GL0_HW_Channel;

	public bool bCollectAllConstellations;

	public bool Exit_Sarf_On_Completion;

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
