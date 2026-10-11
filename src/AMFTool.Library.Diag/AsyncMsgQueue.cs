using System;
using System.Collections.Generic;

namespace AMFTool.Library.Diag;

public class AsyncMsgQueue
{
	private List<ushort> RegisteredMsgIDs;

	private List<Queue<AsyncMessage>> AsyncMsgQueues;

	private List<int> maxQueueSize;

	public AsyncMsgQueue()
	{
		RegisteredMsgIDs = new List<ushort>();
		AsyncMsgQueues = new List<Queue<AsyncMessage>>();
		maxQueueSize = new List<int>();
	}

	public void stopAllQueues()
	{
		RegisteredMsgIDs.Clear();
		AsyncMsgQueues.Clear();
		maxQueueSize.Clear();
	}

	public void RemoveQueue(ushort msgID)
	{
		int msgIDIndex = getMsgIDIndex(msgID);
		if (msgIDIndex >= 0)
		{
			RegisteredMsgIDs.RemoveAt(msgIDIndex);
			AsyncMsgQueues.RemoveAt(msgIDIndex);
			maxQueueSize.RemoveAt(msgIDIndex);
		}
	}

	public bool isMsgIDRegistered(ushort messageID)
	{
		if (getMsgIDIndex(messageID) >= 0)
		{
			return true;
		}
		return false;
	}

	protected int getMsgIDIndex(ushort messageID)
	{
		int result = -1;
		int num = 0;
		foreach (ushort registeredMsgID in RegisteredMsgIDs)
		{
			if (registeredMsgID != messageID)
			{
				num++;
				continue;
			}
			return num;
		}
		return result;
	}

	public bool registerAsyncMsgID(ushort messageID, int maxSize)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (!isMsgIDRegistered(messageID))
		{
			RegisteredMsgIDs.Add(messageID);
			AsyncMsgQueues.Add(new Queue<AsyncMessage>(maxSize));
			maxQueueSize.Add(maxSize);
			return true;
		}
		flushLogQueue(messageID);
		return false;
	}

	public AsyncMessage getAsyncMessage(ushort msgCode)
	{
		object result = null;
		Queue<AsyncMessage> asyncMsgQueue = getAsyncMsgQueue(msgCode);
		if (asyncMsgQueue != null)
		{
			if (asyncMsgQueue.Count <= 0)
			{
				return (AsyncMessage)result;
			}
			return asyncMsgQueue.Dequeue();
		}
		return (AsyncMessage)result;
	}

	public void flushLogQueue(ushort msgCode)
	{
		getAsyncMsgQueue(msgCode)?.Clear();
	}

	public int getMessageCount()
	{
		return AsyncMsgQueues.Count;
	}

	public int getMessageCount(ushort logCode)
	{
		Queue<AsyncMessage> asyncMsgQueue = getAsyncMsgQueue(logCode);
		return asyncMsgQueue.Count;
	}

	public void queueAsyncMessage(AsyncMessage msg)
	{
		try
		{
			int msgIDIndex = getMsgIDIndex(msg.msgID);
			if (msgIDIndex >= 0)
			{
				Queue<AsyncMessage> queue = AsyncMsgQueues[msgIDIndex];
				if (queue.Count < maxQueueSize[msgIDIndex])
				{
					queue.Enqueue(msg);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private Queue<AsyncMessage> getAsyncMsgQueue(ushort msgCode)
	{
		object result = null;
		try
		{
			int msgIDIndex = getMsgIDIndex(msgCode);
			if (msgIDIndex >= 0)
			{
				return AsyncMsgQueues[msgIDIndex];
			}
		}
		catch (Exception)
		{
		}
		return (Queue<AsyncMessage>)result;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
