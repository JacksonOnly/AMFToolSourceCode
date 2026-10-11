using System;
using System.Runtime.InteropServices;

namespace AMFTool.Library.Diag;

public class LogMessage : AsyncMessage
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
	public struct LogMeasurementHdrType
	{
		public byte cmdCode;

		public byte more;

		public ushort length;

		public ushort length2;

		public ushort logCode;

		public ulong timeStamp;
	}

	private const int HeaderLen = 16;

	private GCHandle pinnedPacket;

	private IntPtr p;

	public LogMeasurementHdrType logHdr;

	private bool displayRaw;

	public LogMessage(ushort logCodeVal, byte[] messageVal)
		: base(logCodeVal, messageVal)
	{
		displayRaw = true;
		overlayStructure(logCodeVal, messageVal);
	}

	public LogMessage(ushort logCodeVal, byte[] messageVal, bool displayRawVal)
		: base(logCodeVal, messageVal)
	{
		displayRaw = displayRawVal;
		overlayStructure(logCodeVal, messageVal);
	}

	private void overlayStructure(ushort logCodeVal, byte[] messageVal)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		pinnedPacket = GCHandle.Alloc(messageVal, GCHandleType.Pinned);
		p = pinnedPacket.AddrOfPinnedObject();
		logHdr = (LogMeasurementHdrType)Marshal.PtrToStructure(p, typeof(LogMeasurementHdrType));
	}

	public byte getPayloadByte(int stIndex)
	{
		return message[stIndex + 16];
	}

	public short getPayloadShort(int stIndex)
	{
		return getShort(stIndex + 16);
	}

	public ushort getPayloadWord(int stIndex)
	{
		return getWord(stIndex + 16);
	}

	public uint getPayloadUint32(int stIndex)
	{
		return method_0(stIndex + 16);
	}

	public ulong getPayloadUint64(int stIndex)
	{
		return method_1(stIndex + 16);
	}

	public string getTimeStamp()
	{
		double field_ = Class38.field_4504;
		double field_2 = Class38.field_4505;
		double field_3 = Class38.field_4506;
		double field_4 = Class38.field_4507;
		string format = "{0:00}:{1:00}:{2:00.000}";
		double num = logHdr.timeStamp >> 16;
		num *= field_;
		int num2 = (int)Math.Round(num / field_2);
		num -= (double)num2 * field_2;
		int num3 = (int)Math.Round(num / field_3);
		num -= (double)num3 * field_3;
		int num4 = (int)Math.Round(num / field_4);
		num -= (double)num4 * field_4;
		return string.Format(format, num3, num4, num);
	}

	public override string ToString()
	{
		string format = "0x{0:X} {1} ";
		string text = string.Format(format, logHdr.logCode, getTimeStamp());
		if (!displayRaw)
		{
			return text;
		}
		return text + method_02();
	}

	public new static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	public string method_02()
	{
		return base.ToString();
	}
}
