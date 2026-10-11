using System;
using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

public class LogAAGPSType3Measurement : LogMessage
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
	public struct LogAAGPSMeasurementHdrType3
	{
		public uint fcount;

		public ushort length;

		public byte version;

		public ulong reserved;

		public byte number_svs;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 38)]
	public struct LogAAGPSMeasurementType3
	{
		public byte sv_id;

		public byte health;

		public byte elevation;

		public byte azimuth;

		public short snr;

		public short cno;

		public ushort latency_ms;

		public byte pre_int;

		public ushort post_int;

		public uint millisecond;

		public uint sub_millisecond;

		public ushort sv_time_unc;

		public int sv_speed;

		public uint sv_speed_unc;

		public uint meas_status;

		public byte channel_state;

		public byte good_observations;

		public byte total_observations;

		public int reserved;
	}

	private const int recSize = 38;

	public LogAAGPSMeasurementHdrType3 ds;

	public LogAAGPSMeasurementType3[] dsSv = new LogAAGPSMeasurementType3[32];

	private GCHandle pinnedPacket;

	private IntPtr p;

	private IntPtr intptr_0;

	private IntPtr pAAGPS;

	public LogAAGPSType3Measurement(LogMessage msgVal)
		: base(msgVal.msgID, msgVal.message, displayRawVal: false)
	{
		message = msgVal.message;
		pinnedPacket = GCHandle.Alloc(message, GCHandleType.Pinned);
		p = pinnedPacket.AddrOfPinnedObject();
		intptr_0 = new IntPtr(p.ToInt64() + 16L);
		pAAGPS = new IntPtr(intptr_0.ToInt64() + 16L);
		ds = (LogAAGPSMeasurementHdrType3)Marshal.PtrToStructure(intptr_0, typeof(LogAAGPSMeasurementHdrType3));
		int i = 0;
		for (int num = ds.number_svs - 1; i <= num; i++)
		{
			IntPtr ptr = new IntPtr(pAAGPS.ToInt64() + i * 38);
			LogAAGPSMeasurementType3 logAAGPSMeasurementType = (LogAAGPSMeasurementType3)Marshal.PtrToStructure(ptr, typeof(LogAAGPSMeasurementType3));
			dsSv[i] = logAAGPSMeasurementType;
		}
	}

	public double getCNO()
	{
		return getCNO(0);
	}

	public double getCNO(int numSV)
	{
		double field_ = Class38.field_3213;
		double field_2 = Class38.field_543;
		if (ds.number_svs <= 0)
		{
			return field_2;
		}
		return (double)dsSv[numSV].cno / field_;
	}

	public byte getGoodObsCnt()
	{
		return getGoodObsCnt(0);
	}

	public byte getGoodObsCnt(int numSV)
	{
		if (ds.number_svs > 0)
		{
			return dsSv[numSV].good_observations;
		}
		byte b = 0;
		return 0;
	}

	public override string ToString()
	{
		string format = "FCount: {0}";
		string format2 = " SV: {0:00}";
		string format3 = " CNO: {0:0.00} ";
		string text = method_02() + string.Format(format, ds.fcount);
		int i = 0;
		for (int num = ds.number_svs - 1; i <= num; i++)
		{
			text = text + Environment.NewLine + string.Format(format2, dsSv[i].sv_id) + string.Format(format3, getCNO(i));
		}
		return text;
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
