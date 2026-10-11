namespace AMFTool.Library.Mediatek.Download_Agent.xml;

public class DwnFile
{
	public string Checksum { get; }

	public string Info { get; }

	public string SourceFile { get; }

	public long PacketLength { get; }

	public DwnFile(string checksum, string info, string sourceFile, long packetLength)
	{
		Checksum = checksum;
		Info = info;
		SourceFile = sourceFile;
		PacketLength = packetLength;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
