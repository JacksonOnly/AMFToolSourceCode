using System;
using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

public class LogLTEAntCorrelation : LogMessage
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 20)]
	public struct LogLTEAntCorrelationType
	{
		public uint verServSubframeFrameRes;

		public int rssiRx0;

		public int rssiRx1;

		public int rssiCombined;

		public uint resNumRecords;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
	public struct LogLTEAntCorrelationRecordType
	{
		public uint txAntRes;

		public int antCorrReal;

		public int antCorrImag;
	}

	private const int lteAntCorrRecordSize = 12;

	public LogLTEAntCorrelationType logLTEAntCorrelationType_0;

	public LogLTEAntCorrelationRecordType[] LTEAntCorrRecord = new LogLTEAntCorrelationRecordType[4];

	private GCHandle pinnedPacket;

	private IntPtr p;

	private IntPtr intptr_0;

	private IntPtr intptr_1;

	public LogLTEAntCorrelation(LogMessage msgVal)
		: base(msgVal.msgID, msgVal.message, displayRawVal: false)
	{
		message = msgVal.message;
		pinnedPacket = GCHandle.Alloc(message, GCHandleType.Pinned);
		p = pinnedPacket.AddrOfPinnedObject();
		intptr_0 = new IntPtr(p.ToInt64() + 16L);
		intptr_1 = new IntPtr(intptr_0.ToInt64() + 20L);
		logLTEAntCorrelationType_0 = (LogLTEAntCorrelationType)Marshal.PtrToStructure(intptr_0, typeof(LogLTEAntCorrelationType));
		int numRecords = getNumRecords();
		LTEAntCorrRecord = new LogLTEAntCorrelationRecordType[numRecords];
		int i = 0;
		for (int num = numRecords - 1; i <= num; i++)
		{
			IntPtr ptr = new IntPtr(intptr_1.ToInt64() + i * 12);
			LogLTEAntCorrelationRecordType logLTEAntCorrelationRecordType = (LogLTEAntCorrelationRecordType)Marshal.PtrToStructure(ptr, typeof(LogLTEAntCorrelationRecordType));
			LTEAntCorrRecord[i] = logLTEAntCorrelationRecordType;
		}
	}

	public int getNumRecords()
	{
		return Convert.ToInt16(logLTEAntCorrelationType_0.resNumRecords >> 30);
	}

	public double getRssi(int antenna)
	{
		double field_ = Class38.field_202;
		double field_2 = Class38.field_4499;
		double value;
		if (antenna != 0)
		{
			value = Convert.ToDouble(logLTEAntCorrelationType_0.rssiRx1) / Convert.ToDouble(Math.Pow(field_, field_2));
			return Math.Round(value, 2);
		}
		value = Convert.ToDouble(logLTEAntCorrelationType_0.rssiRx0) / Convert.ToDouble(Math.Pow(field_, field_2));
		return Math.Round(value, 2);
	}

	public double getPhase()
	{
		double field_ = Class38.field_543;
		double field_2 = Class38.field_4500;
		double field_3 = Class38.field_4501;
		double field_4 = Class38.field_4502;
		double field_5 = Class38.field_4503;
		double num = field_;
		double num2 = Convert.ToDouble(LTEAntCorrRecord[0].antCorrReal);
		double num3 = Convert.ToDouble(LTEAntCorrRecord[0].antCorrImag);
		if (num2 != field_)
		{
			num = Math.Atan(Math.Abs(num3) / Math.Abs(num2));
			if (!(num2 >= field_) && num3 >= field_)
			{
				return field_4 - num;
			}
			if (num2 < field_ && num3 < field_)
			{
				return field_4 + num;
			}
			if (!(num2 > field_) || !(num3 < field_))
			{
				return num;
			}
			return field_5 - num;
		}
		return (!(num3 < field_)) ? field_2 : field_3;
	}

	public int getAntCorrReal()
	{
		return LTEAntCorrRecord[0].antCorrReal;
	}

	public int getAntCorrImag()
	{
		return LTEAntCorrRecord[0].antCorrImag;
	}

	public new static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
