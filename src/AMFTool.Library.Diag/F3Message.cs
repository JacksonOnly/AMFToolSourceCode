using System;
using System.Collections.Generic;

namespace AMFTool.Library.Diag;

public class F3Message : AsyncMessage
{
	private const int NUMARGSOFS = 2;

	public ushort ssMask;

	public string formatStr;

	public string fileNameStr;

	public List<int> args;

	public F3Message(ushort id, byte[] msg) : base(id, msg)
	{
		string text = "";
		args = new List<int>();
		
		msgID = id;
		int num = message[2];
		int num2 = 20 + 4 * num;
		ssMask = getWord(16);
		int i = 0;
		for (int num3 = num - 1; i <= num3; i++)
		{
			int item = (message[20 + i * 4 + 3] << 24) | (message[20 + i * 4 + 2] << 16) | (message[20 + i * 4 + 1] << 8) | message[20 + i * 4];
			args.Add(item);
		}
		int num4 = 0;
		formatStr = text;
		int j = num2;
		for (int num5 = message.Length - 1; j <= num5; j++)
		{
			if (message[j] != 0)
			{
				formatStr += Convert.ToString(Convert.ToChar(message[j]));
				continue;
			}
			num4 = j + 1;
			break;
		}
		fileNameStr = text;
		for (int k = num4; k < message.Length && message[k] != 0; k++)
		{
			fileNameStr += Convert.ToString(Convert.ToChar(message[k]));
		}
	}

	public new static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
