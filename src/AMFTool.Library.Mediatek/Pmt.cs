namespace AMFTool.Library.Mediatek;

public class Pmt
{
	public class PtResident
	{
		public byte[] Name;

		public ulong Size;

		public ulong PartId;

		public ulong Offset;

		public ulong MaskFlags;

		public PtResident(byte[] data)
		{
			StructHelper structHelper = new StructHelper(data);
			Name = structHelper.Bytes(64);
			Size = structHelper.Qword();
			PartId = structHelper.Qword();
			Offset = structHelper.Qword();
			MaskFlags = structHelper.Qword();
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class PtInfo
	{
		public byte SequenceNumber;

		public byte ToolOrSdUpdate;

		public byte MirrorPtDl;

		public byte MirrorPtHasSpace;

		public byte PtChanged;

		public byte PtHasSpace;

		public PtInfo(byte[] data)
		{
			StructHelper structHelper = new StructHelper(data);
			SequenceNumber = structHelper.Bytes()[0];
			ToolOrSdUpdate = structHelper.Bytes()[0];
			byte b = structHelper.Bytes()[0];
			MirrorPtDl = (byte)((b >> 4) & 0xF);
			MirrorPtHasSpace = (byte)(b & 0xF);
			b = structHelper.Bytes()[0];
			PtChanged = (byte)((b >> 4) & 0xF);
			PtHasSpace = (byte)(b & 0xF);
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}

		private object method_02()
		{
			return new int[0];
		}
	}

	private object method_01()
	{
		return new int[0];
	}

	private object method_02()
	{
		return new int[0];
	}
}
