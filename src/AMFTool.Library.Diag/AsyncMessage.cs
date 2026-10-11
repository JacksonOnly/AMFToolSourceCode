namespace AMFTool.Library.Diag;

public class AsyncMessage
{
	public ushort msgID;

	public byte[] message;

	public AsyncMessage(ushort messageID, byte[] msg)
	{
		msgID = messageID;
		message = new byte[msg.Length];
		int i = 0;
		for (int num = msg.Length - 1; i <= num; i++)
		{
			message[i] = msg[i];
		}
	}

	public short getShort(int stIndex)
	{
		return (short)((message[stIndex + 1] << 8) + message[stIndex]);
	}

	public ushort getWord(int stIndex)
	{
		return (ushort)((message[stIndex + 1] << 8) + message[stIndex]);
	}

	public uint method_0(int stIndex)
	{
		uint num = 0u;
		int num2 = stIndex + 3;
		if (num2 < stIndex)
		{
			uint num3 = 0u;
			return 0u;
		}
		num = 0u;
		num = (uint)(0 + message[num2]);
		int num4 = 97319723;
		while (true)
		{
			_ = num4 ^ 0x7BD7A2C8;
			num2--;
			if (num2 < stIndex)
			{
				break;
			}
			num <<= 8;
			num += message[num2];
			num4 = 97319723;
		}
		return num;
	}

	public ulong method_1(int stIndex)
	{
		ulong num = 0uL;
		int num2 = stIndex + 7;
		if (num2 < stIndex)
		{
			return num;
		}
		num <<= 8;
		num += message[num2];
		int num3 = 851520254;
		while (true)
		{
			_ = num3 ^ -1814973894;
			num2--;
			if (num2 < stIndex)
			{
				break;
			}
			num <<= 8;
			num += message[num2];
			num3 = 851520254;
		}
		return num;
	}

	public override string ToString()
	{
		string text = "";
		string format = "{0:x2} ";
		string text2 = text;
		byte[] array = message;
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			text2 += string.Format(format, b);
		}
		return text2;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
