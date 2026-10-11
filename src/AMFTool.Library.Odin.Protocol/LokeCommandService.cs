using System;
using AMFTool.Library.Usb;

namespace AMFTool.Library.Odin.Protocol;

public sealed class LokeCommandService
{
	private readonly SerialOdin SerialPort;

	public byte[] LastResponseBytes { get; set; } = new byte[8];

	public LokeCommandService(SerialOdin serialPort)
	{
		SerialPort = serialPort;
	}

	public bool SendCommand(SamsungLokeCommand commandPacket, bool shouldReadResponse = true)
	{
		string message = "Invalid LOKE response: 0xFF";
		byte[] array = BuildCommandBuffer(commandPacket);
		SerialPort.Write(array, array.Length);
		LastResponseBytes = new byte[8];
		if (shouldReadResponse)
		{
			LastResponseBytes = SerialPort.Read(8);
			if (LastResponseBytes.Length != 8 || LastResponseBytes[0] == byte.MaxValue)
			{
				throw new InvalidOperationException(message);
			}
			return true;
		}
		return true;
	}

	public bool InitializeSession(long totalPayloadBytes)
	{
		SamsungLokeCommand commandPacket = new SamsungLokeCommand(100, 0, 5L);
		if (SendCommand(commandPacket))
		{
			long protocolVariant = GetProtocolVariant(LastResponseBytes);
			if (protocolVariant == 5L)
			{
				commandPacket = new SamsungLokeCommand(100, 12);
				SendCommand(commandPacket, shouldReadResponse: false);
			}
			if (totalPayloadBytes != 0L)
			{
				if (protocolVariant == 2L)
				{
					commandPacket = new SamsungLokeCommand(100, 2);
					SendCommand(commandPacket);
				}
				if (protocolVariant == 3L || protocolVariant == 4L || protocolVariant == 5L)
				{
					commandPacket = new SamsungLokeCommand(100, 5, 1048576L);
					SendCommand(commandPacket);
				}
				commandPacket = new SamsungLokeCommand(100, 2, totalPayloadBytes);
				SendCommand(commandPacket);
				if (protocolVariant == 4L)
				{
					for (int i = 0; i < 3; i++)
					{
						commandPacket = new SamsungLokeCommand(105, i);
						SendCommand(commandPacket);
					}
				}
				return true;
			}
			return true;
		}
		return false;
	}

	public long GetProtocolVariant(byte[] responseBuffer)
	{
		return (Binary.ReadI32LE(responseBuffer, 4) & 0xFFFF0000L) >> 16;
	}

	public byte[] BuildCommandBuffer(SamsungLokeCommand commandPacket)
	{
		byte[] array = new byte[1024];
		Binary.WriteI32LE(array, 0, commandPacket.CommandId);
		Binary.WriteI32LE(array, 4, commandPacket.SequenceId);
		if (commandPacket.CommandId != 100)
		{
			Binary.WriteI32LE(array, 8, (int)commandPacket.BinaryTypeId);
			Binary.WriteI32LE(array, 12, commandPacket.PayloadSize);
			Binary.WriteI32LE(array, 16, commandPacket.UnknownCode);
			Binary.WriteI32LE(array, 20, commandPacket.DeviceId);
			Binary.WriteI32LE(array, 24, commandPacket.PartitionId);
			Binary.WriteI32LE(array, 28, commandPacket.SessionEndFlag);
			Binary.WriteI32LE(array, 32, commandPacket.EfsClearFlag);
			Binary.WriteI32LE(array, 36, commandPacket.BootUpdateFlag);
			return array;
		}
		Binary.WriteI64LE(array, 8, commandPacket.BinaryTypeId);
		Binary.WriteI32LE(array, 16, commandPacket.UnknownCode);
		Binary.WriteI32LE(array, 20, commandPacket.DeviceId);
		Binary.WriteI32LE(array, 24, commandPacket.PartitionId);
		Binary.WriteI32LE(array, 28, commandPacket.SessionEndFlag);
		Binary.WriteI32LE(array, 32, commandPacket.EfsClearFlag);
		Binary.WriteI32LE(array, 36, commandPacket.BootUpdateFlag);
		return array;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
