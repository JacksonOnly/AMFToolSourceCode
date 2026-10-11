namespace AMFTool.Library.Diag;

public class QMSL_Event_Element_Struct
{
	public byte time_length;

	public byte[] time;

	public ushort event_id;

	public byte payload_len;

	public byte[] payload_data;

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
