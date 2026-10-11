using System;
using System.Buffers;
using AMFTool.Library.Usb;

namespace AMFTool.Library.Spreadtrum;

public class SprdMain
{
	public enum Checksum
	{
		CRC,
		FDL
	}

	public class Response
	{
		public byte Type { get; set; }

		public ushort Length { get; set; }

		public byte[] Payload { get; set; }

		public ushort Crc { get; set; }

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public Checksum Cksm = Checksum.CRC;

	private const int HdlcEscape = 125;

	public bool UseTranscode { get; set; } = true;

	public bool SkipRxCrcCheck { get; set; } = true;

	public void DisableTranscode()
	{
		UseTranscode = false;
	}

	public bool SendCommand(SprdProtocol.SCMD cmd, byte[] payload = null)
	{
		string format = "Sending CMD : {0}";
		Console.WriteLine(string.Format(format, cmd));
		return SerialSPD.Write(HdlcProtocol.BuildCommand(cmd, payload ?? Array.Empty<byte>(), Cksm, UseTranscode));
	}

	public Response ReadResponse()
	{
		object result = null;
		byte[] array = SerialSPD.ReadFrame(UseTranscode);
		if (array != null && array.Length != 0)
		{
			Response response = ParseFrame(array);
			if (response == null || response.Type == byte.MaxValue)
			{
				return (Response)result;
			}
			return response;
		}
		return (Response)result;
	}

	public Response ParseFrame(byte[] frame)
	{
		object result = null;
		int clearArray = 0;
		if (frame == null || frame.Length < 7)
		{
			return (Response)result;
		}
		int num = frame.Length - 2;
		if (num <= 0)
		{
			return (Response)result;
		}
		byte[] array = ArrayPool<byte>.Shared.Rent(num);
		try
		{
			if (ParseRawFrame(frame, array, out var decodedLength) && decodedLength > 0)
			{
				return ParseDecoded(array, decodedLength);
			}
			return (Response)result;
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(array, (byte)clearArray != 0);
		}
	}

	public bool ParseRawFrame(byte[] frame, byte[] decodedBuffer, out int decodedLength)
	{
		decodedLength = 0;
		if (frame != null && frame.Length >= 7)
		{
			if (frame[0] != 126 || frame[frame.Length - 1] != 126)
			{
				return false;
			}
			int num = frame.Length - 2;
			if (num <= 0)
			{
				return false;
			}
			if (UseTranscode)
			{
				int num2 = 0;
				bool flag = false;
				for (int i = 1; i < frame.Length - 1; i++)
				{
					byte b = frame[i];
					if (flag)
					{
						if (num2 >= decodedBuffer.Length)
						{
							return false;
						}
						decodedBuffer[num2++] = (byte)(b ^ 0x20);
						flag = false;
					}
					else if (b != 125)
					{
						if (num2 >= decodedBuffer.Length)
						{
							return false;
						}
						decodedBuffer[num2++] = b;
					}
					else
					{
						flag = true;
					}
				}
				decodedLength = num2;
				return num2 > 0;
			}
			if (decodedBuffer.Length >= num)
			{
				frame.AsSpan(1, num).CopyTo(decodedBuffer.AsSpan(0, num));
				decodedLength = num;
				return true;
			}
			return false;
		}
		return false;
	}

	public Response ParseDecoded(byte[] data, int dataLen)
	{
		object result = null;
		if (data != null && dataLen >= 6 && dataLen <= data.Length)
		{
			byte type = data[1];
			ushort num = (ushort)((data[2] << 8) | data[3]);
			if (dataLen >= 4 + num + 2)
			{
				byte[] array = new byte[num];
				int num2;
				ushort num3;
				byte[] array2;
				ushort num4;
				if (num <= 0)
				{
					num2 = 4 + num;
					num3 = (ushort)((data[num2] << 8) | data[num2 + 1]);
					if (SkipRxCrcCheck)
					{
						return new Response
						{
							Type = type,
							Length = num,
							Payload = array,
							Crc = num3
						};
					}
					array2 = new byte[num2];
					data.AsSpan(0, num2).CopyTo(array2.AsSpan(0, num2));
					num4 = ((Cksm != Checksum.CRC) ? SprdChecksum.CalculateSprdChecksum(array2) : SprdChecksum.CalculateCRC16Ccitt(array2));
					if (num3 != num4)
					{
						ushort num5 = ((Cksm != Checksum.CRC) ? SprdChecksum.CalculateCRC16Ccitt(array2) : SprdChecksum.CalculateSprdChecksum(array2));
						if (num3 != num5)
						{
							return (Response)result;
						}
						int num6;
						if (Cksm != Checksum.CRC)
						{
							Cksm = Checksum.CRC;
							num6 = 1576141757;
						}
						else
						{
							Cksm = Checksum.FDL;
							num6 = 1576141757;
						}
						_ = num6 ^ 0x570051DE;
					}
					return new Response
					{
						Type = type,
						Length = num,
						Payload = array,
						Crc = num3
					};
				}
				data.AsSpan(4, num).CopyTo(array.AsSpan(0, num));
				num2 = 4 + num;
				num3 = (ushort)((data[num2] << 8) | data[num2 + 1]);
				if (SkipRxCrcCheck)
				{
					return new Response
					{
						Type = type,
						Length = num,
						Payload = array,
						Crc = num3
					};
				}
				array2 = new byte[num2];
				data.AsSpan(0, num2).CopyTo(array2.AsSpan(0, num2));
				num4 = ((Cksm != Checksum.CRC) ? SprdChecksum.CalculateSprdChecksum(array2) : SprdChecksum.CalculateCRC16Ccitt(array2));
				if (num3 != num4)
				{
					ushort num5 = ((Cksm != Checksum.CRC) ? SprdChecksum.CalculateCRC16Ccitt(array2) : SprdChecksum.CalculateSprdChecksum(array2));
					if (num3 != num5)
					{
						return (Response)result;
					}
					int num7;
					if (Cksm != Checksum.CRC)
					{
						Cksm = Checksum.CRC;
						num7 = 1576141757;
					}
					else
					{
						Cksm = Checksum.FDL;
						num7 = 1576141757;
					}
					_ = num7 ^ 0x570051DE;
				}
				return new Response
				{
					Type = type,
					Length = num,
					Payload = array,
					Crc = num3
				};
			}
			return (Response)result;
		}
		return (Response)result;
	}

	public bool SendCommandAndACK(SprdProtocol.SCMD cmd, byte[] payload = null)
	{
		object obj = null;
		string format = "Response : {0}";
		if (SendCommand(cmd, payload))
		{
			Response response = ReadResponse();
			if (response == obj)
			{
				return false;
			}
			Console.WriteLine(string.Format(format, (SprdProtocol.SCMD)response.Type));
			return response.Type == 128;
		}
		return false;
	}

	public Response SendCommandAndGetResponse(SprdProtocol.SCMD cmd, byte[] payload = null)
	{
		object result = null;
		if (!SendCommand(cmd, payload))
		{
			return (Response)result;
		}
		return ReadResponse();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
