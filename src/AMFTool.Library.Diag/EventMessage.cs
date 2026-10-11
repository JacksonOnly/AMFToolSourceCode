namespace AMFTool.Library.Diag;

public class EventMessage : AsyncMessage
{
	public EventMessage(ushort eventID, byte[] msg)
		: base((ushort)(eventID & 0xFFF), msg)
	{
	}

	public new static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
