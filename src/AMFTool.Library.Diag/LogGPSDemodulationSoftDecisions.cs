using System;
using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

public class LogGPSDemodulationSoftDecisions : LogMessage
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 49)]
	public struct LogGpsDemodSoftDecisionsType
	{
		public byte versionId;

		public byte sv_id;

		public ushort w_DemodId;

		public uint bitId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public short[] softDecisions;

		public byte numBits;
	}

	private const int NUM_SOFT_DECISIONS = 20;

	public LogGpsDemodSoftDecisionsType ds;

	private GCHandle pinnedPacket;

	private IntPtr p;

	public LogGPSDemodulationSoftDecisions(LogMessage msgVal)
		: base(msgVal.msgID, msgVal.message, displayRawVal: false)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		message = msgVal.message;
		pinnedPacket = GCHandle.Alloc(message, GCHandleType.Pinned);
		p = pinnedPacket.AddrOfPinnedObject();
		p = new IntPtr(p.ToInt64() + 16L);
	}

	public byte getNumBits()
	{
		return ds.numBits;
	}

	public short[] getSoftDecisionArray()
	{
		short[] array = new short[20];
		int numBits = ds.numBits;
		int i = 0;
		for (int num = numBits - 1; i <= num; i++)
		{
			array[i] = ds.softDecisions[i];
		}
		return array;
	}

	public new static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
