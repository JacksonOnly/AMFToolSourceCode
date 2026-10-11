using System;
using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

public class LogMfloRssiValueDynamicParams : LogMessage
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
	public struct MfloRssiValueDynamicParamsType
	{
		public byte Interrupt_type;

		public ushort reserved1;

		public byte reserved2;

		public ushort rssi_int_part;

		public byte rssi_fract_part;

		public byte rssi_value_positive;
	}

	public MfloRssiValueDynamicParamsType ds;

	private GCHandle pinnedPacket;

	private IntPtr p;

	public LogMfloRssiValueDynamicParams(LogMessage msgVal)
		: base(msgVal.msgID, msgVal.message, displayRawVal: false)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		pinnedPacket = GCHandle.Alloc(message, GCHandleType.Pinned);
		p = pinnedPacket.AddrOfPinnedObject();
		p = new IntPtr(p.ToInt64() + 16L);
		ds = (MfloRssiValueDynamicParamsType)Marshal.PtrToStructure(p, typeof(MfloRssiValueDynamicParamsType));
	}

	public override string ToString()
	{
		string format = " RSSI: {0}{1}.{2}";
		string text = "-";
		string text2 = "";
		return method_02() + string.Format(format, (ds.rssi_value_positive != 0) ? text2 : text, ds.rssi_int_part, ds.rssi_fract_part);
	}

	public new static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	public new string method_02()
	{
		return base.ToString();
	}
}
