using System;
using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

public class LogLTEAGC : LogMessage
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
	public struct LogLTEAGCType
	{
		public uint uint_0;

		public uint second32bits;
	}

	public LogLTEAGCType logLTEAGCType_0;

	private GCHandle pinnedPacket;

	private IntPtr p;

	private IntPtr intptr_0;

	public LogLTEAGC(LogMessage msgVal)
		: base(msgVal.msgID, msgVal.message, displayRawVal: false)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		message = msgVal.message;
		pinnedPacket = GCHandle.Alloc(message, GCHandleType.Pinned);
		p = pinnedPacket.AddrOfPinnedObject();
		intptr_0 = new IntPtr(p.ToInt64() + 16L);
		logLTEAGCType_0 = (LogLTEAGCType)Marshal.PtrToStructure(intptr_0, typeof(LogLTEAGCType));
	}

	public int getRxPath()
	{
		ulong num = logLTEAGCType_0.uint_0 << 9;
		num >>= 31;
		Convert.ToInt16(num);
		byte[] bytes = BitConverter.GetBytes(logLTEAGCType_0.uint_0);
		if (!BitConverter.IsLittleEndian)
		{
			if ((bytes[1] & 0x40) <= 0)
			{
				return 0;
			}
			return 1;
		}
		bytes = ReverseBytes(bytes);
		if ((bytes[1] & 0x40) <= 0)
		{
			return 0;
		}
		return 1;
	}

	public double getRssi()
	{
		double field_ = Class38.field_543;
		byte[] bytes = BitConverter.GetBytes(logLTEAGCType_0.second32bits);
		if (BitConverter.IsLittleEndian)
		{
			bytes = ReverseBytes(bytes);
			return field_;
		}
		return field_;
	}

	private byte[] ReverseBytes(byte[] inArray)
	{
		double field_ = Class38.field_202;
		double field_2 = Class38.field_203;
		int num = inArray.Length - 1;
		int i = 0;
		for (int num2 = (int)Math.Round((double)inArray.Length / field_ - field_2); i <= num2; i++)
		{
			byte b = inArray[i];
			inArray[i] = inArray[num];
			inArray[num] = b;
			num--;
		}
		return inArray;
	}

	public new static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
