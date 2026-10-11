using System;
using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

public class LogCGPSMeasurement : LogMessage
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 28)]
	public struct LogCGPSMeasurementHdrType
	{
		public byte version;

		public uint fcount;

		public ushort gps_week;

		public uint gps_ms;

		public uint time_bias;

		public uint time_unc;

		public uint freq_bias;

		public uint freq_unc;

		public byte number_svs;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 59)]
	public struct LogCGPSMeasurementType
	{
		public byte sv_id;

		public byte obs_state;

		public byte obs_cnt;

		public byte good_obs_cnt;

		public byte filter_n;

		public short cno;

		public ushort latency;

		public byte pre_int;

		public ushort post_int;

		public int unfilt_ms;

		public int unfilt_sm;

		public int unfilt_time_unc;

		public int unfilt_speed;

		public int unfilt_speed_unc;

		public int meas_status;

		public byte misc_status;

		public int multipath_est;

		public int azi_rad;

		public int elev_rad;

		public int carrier_phase_cycles;

		public ushort carrier_phase_fract;

		public int reserved;
	}

	private const int recSize = 59;

	public LogCGPSMeasurementHdrType ds;

	public LogCGPSMeasurementType[] dsSv = new LogCGPSMeasurementType[32];

	private GCHandle pinnedPacket;

	private IntPtr p;

	private IntPtr pCGPSHdr;

	private IntPtr pCGPS;

	public LogCGPSMeasurement(LogMessage msgVal)
		: base(msgVal.msgID, msgVal.message, displayRawVal: false)
	{
		message = msgVal.message;
		pinnedPacket = GCHandle.Alloc(message, GCHandleType.Pinned);
		p = pinnedPacket.AddrOfPinnedObject();
		pCGPSHdr = new IntPtr(p.ToInt64() + 16L);
		pCGPS = new IntPtr(pCGPSHdr.ToInt64() + 28L);
		ds = (LogCGPSMeasurementHdrType)Marshal.PtrToStructure(pCGPSHdr, typeof(LogCGPSMeasurementHdrType));
		int i = 0;
		for (int num = ds.number_svs - 1; i <= num; i++)
		{
			IntPtr ptr = new IntPtr(pCGPS.ToInt64() + i * 59);
			LogCGPSMeasurementType logCGPSMeasurementType = (LogCGPSMeasurementType)Marshal.PtrToStructure(ptr, typeof(LogCGPSMeasurementType));
			dsSv[i] = logCGPSMeasurementType;
		}
	}

	public double getCNO()
	{
		return getCNO(0);
	}

	public double getCNO(int svNum)
	{
		double field_ = Class38.field_3686;
		double field_2 = Class38.field_543;
		if (ds.number_svs <= svNum)
		{
			return field_2;
		}
		double num = dsSv[svNum].cno;
		return num / field_;
	}

	public int getSVID(int svNum)
	{
		if (ds.number_svs <= svNum)
		{
			int num = 0;
			return 0;
		}
		return dsSv[svNum].sv_id;
	}

	public byte getGoodObsCnt()
	{
		return getGoodObsCnt(0);
	}

	public byte getGoodObsCnt(int svNum)
	{
		if (ds.number_svs <= svNum)
		{
			byte b = 0;
			return 0;
		}
		return dsSv[svNum].good_obs_cnt;
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
			text = text + Environment.NewLine + string.Format(format2, getSVID(i)) + string.Format(format3, getCNO(i));
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
