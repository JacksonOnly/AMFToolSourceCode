using System;
using System.Text;

namespace AMFTool.Library.Diag;

public struct Message
{
	public MessageType type;

	public string source;

	public string data;

	public DateTime timestamp;

	public uint sequenceNumber;

	public bool truncated;

	public Message(MessageType type, string source, bool data)
		: this(type, source, data.ToString())
	{
	}

	public Message(MessageType type, string source, char data)
		: this(type, source, new string(data, 1))
	{
	}

	public Message(MessageType type, string source, short data)
		: this(type, source, data.ToString())
	{
	}

	public Message(MessageType type, string source, ushort data)
		: this(type, source, data.ToString())
	{
	}

	public Message(MessageType type, string source, int data)
		: this(type, source, data.ToString())
	{
	}

	public Message(MessageType type, string source, uint data)
		: this(type, source, data.ToString())
	{
	}

	public Message(MessageType type, string source, long data)
		: this(type, source, data.ToString())
	{
	}

	public Message(MessageType type, string source, ulong data)
		: this(type, source, data.ToString())
	{
	}

	public Message(MessageType type, string source, float data)
		: this(type, source, data.ToString())
	{
	}

	public Message(MessageType type, string source, double data)
		: this(type, source, data.ToString())
	{
	}

	public Message(MessageType type, string source, Exception ex)
	{
		string text = "Source = ";
		string text2 = ", Message = ";
		this = new Message(type, source, text + ex.Source + text2 + ex.Message);
	}

	public Message(MessageType type, string source, string data)
	{
		int maxMessageSize = Logger.MaxMessageSize;
		timestamp = DateTime.Now;
		sequenceNumber = 0u;
		this.type = type;
		this.source = source;
		if (data.Length > maxMessageSize)
		{
			this.data = data.Substring(0, maxMessageSize);
			truncated = true;
		}
		else
		{
			this.data = data;
			truncated = false;
		}
	}

	public Message(MessageType type, string source, byte data)
		: this(type, source, new byte[1] { data }, 1)
	{
	}

	public Message(MessageType type, string source, byte[] data)
		: this(type, source, data, data.Length)
	{
	}

	public Message(MessageType type, string source, byte[] data, int dataLength)
	{
		string text = "";
		string text2 = "0x";
		string text3 = "X2";
		string text4 = " 0x";
		int num = 0;
		int maxMessageSize = Logger.MaxMessageSize;
		timestamp = DateTime.Now;
		sequenceNumber = 0u;
		this.type = type;
		this.source = source;
		this.data = text;
		for (int i = 0; i < data.Length; i++)
		{
			byte b = data[i];
			num++;
			if (num <= dataLength)
			{
				if (this.data.Length != 0)
				{
					this.data = this.data + text4 + b.ToString(text3);
				}
				else
				{
					this.data = text2 + b.ToString(text3);
				}
				if (this.data.Length > maxMessageSize)
				{
					this.data.Remove(maxMessageSize);
					truncated = true;
					return;
				}
			}
		}
		truncated = false;
	}

	public override string ToString()
	{
		string value = "TI Library Log Message";
		string text = "Timestamp: ";
		string text2 = "Sequence Number: ";
		string text3 = "Truncated: ";
		string text4 = "Type: ";
		string text5 = "Source: ";
		string text6 = "Data: ";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(value);
		stringBuilder.AppendLine(text + timestamp.ToString());
		stringBuilder.AppendLine(text2 + sequenceNumber);
		stringBuilder.AppendLine(text3 + truncated);
		stringBuilder.AppendLine(text4 + type);
		stringBuilder.AppendLine(text5 + source.ToString());
		stringBuilder.AppendLine(text6 + data.ToString());
		return stringBuilder.ToString();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
