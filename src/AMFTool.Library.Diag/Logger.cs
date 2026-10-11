using System;
using System.Collections.Generic;
using System.Threading;

namespace AMFTool.Library.Diag;

public static class Logger
{
	private static bool enabledField;

	private static int maxMessageSizeField;

	private static int maxQueueCountField;

	private static ManualResetEvent messagesAvailableField;

	private static Queue<Message> q;

	private static bool[] messageTypeEnableMap;

	private static uint sequenceNumber;

	private static object sequenceNumberLock;

	public static bool Enabled
	{
		get
		{
			return enabledField;
		}
		set
		{
			enabledField = value;
		}
	}

	public static int MaxMessageSize
	{
		get
		{
			return maxMessageSizeField;
		}
		set
		{
			string paramName = "MaxMessageSize";
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException(paramName);
			}
			maxMessageSizeField = value;
		}
	}

	public static int MaxQueueCount
	{
		get
		{
			return maxQueueCountField;
		}
		set
		{
			string paramName = "MaxQueueCount";
			if (value >= 0)
			{
				lock (q)
				{
					maxQueueCountField = value;
					if (maxQueueCountField > 0)
					{
						while (q.Count > maxQueueCountField)
						{
							q.Dequeue();
						}
					}
					return;
				}
			}
			throw new ArgumentOutOfRangeException(paramName);
		}
	}

	public static ManualResetEvent MessagesAvailable => messagesAvailableField;

	static Logger()
	{
		enabledField = false;
		maxMessageSizeField = 256;
		maxQueueCountField = 0;
		messagesAvailableField = new ManualResetEvent(initialState: false);
		q = new Queue<Message>();
		sequenceNumber = 0u;
		sequenceNumberLock = new object();
		MessageType[] array = (MessageType[])Enum.GetValues(typeof(MessageType));
		messageTypeEnableMap = new bool[array.GetLength(0)];
		int num = 0;
		MessageType[] array2 = array;
		int i = 0;
		for (int num2 = array2.Length - 1; i <= num2; i++)
		{
			messageTypeEnableMap[num] = false;
			num++;
		}
	}

	public static void Clear()
	{
		lock (q)
		{
			q.Clear();
			messagesAvailableField.Reset();
		}
	}

	public static List<Message> GetAllMessages()
	{
		List<Message> list = new List<Message>();
		lock (q)
		{
			while (q.Count > 0)
			{
				list.Add(q.Dequeue());
			}
			if (q.Count == 0)
			{
				messagesAvailableField.Reset();
			}
		}
		return list;
	}

	public static List<Message> GetUpToNMessages(uint n)
	{
		uint num = 0u;
		List<Message> list = new List<Message>();
		lock (q)
		{
			while (q.Count > 0 && num < n)
			{
				list.Add(q.Dequeue());
				num = (uint)((ulong)num + 1uL);
			}
			if (q.Count == 0)
			{
				messagesAvailableField.Reset();
			}
		}
		return list;
	}

	public static void EnableMessageType(MessageType messageType)
	{
		messageTypeEnableMap[(int)messageType] = true;
	}

	public static void DisableMessageType(MessageType messageType)
	{
		messageTypeEnableMap[(int)messageType] = false;
	}

	public static bool IsMessageTypeEnabled(MessageType messageType)
	{
		return messageTypeEnableMap[(int)messageType];
	}

	public static void Add(Message message)
	{
		if (!enabledField || !messageTypeEnableMap[(int)message.type])
		{
			return;
		}
		lock (q)
		{
			message.sequenceNumber = sequenceNumber;
			if (sequenceNumber != uint.MaxValue)
			{
				sequenceNumber = (uint)((ulong)sequenceNumber + 1uL);
			}
			else
			{
				sequenceNumber = 0u;
			}
			if (q.Count < maxQueueCountField || maxQueueCountField == 0)
			{
				q.Enqueue(message);
			}
			messagesAvailableField.Set();
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
