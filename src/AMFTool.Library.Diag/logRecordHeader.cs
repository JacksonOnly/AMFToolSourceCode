namespace AMFTool.Library.Diag;

public struct logRecordHeader
{
	public ushort msgType;

	public ushort length;

	public ushort length2;

	public ushort log_code;

	public byte[] timestamp;

	public byte[] data;
}
