using System;

namespace AMFTool.Library.Spreadtrum;

public class HdlcProtocol
{
	private const byte HDLC_ESCAPE = 125;

	public static byte[] BuildCommand(SprdProtocol.SCMD cmd, byte[] payload, SprdMain.Checksum checksumMode, bool useTranscode = true)
	{
		if ((byte)cmd != 126)
		{
			if (payload == null)
			{
				payload = Array.Empty<byte>();
			}
			byte[] array = new byte[4]
			{
				(byte)((ushort)cmd >> 8),
				(byte)((ushort)cmd & 0xFF),
				(byte)(payload.Length >> 8),
				(byte)(payload.Length & 0xFF)
			};
			byte[] array2 = new byte[array.Length + payload.Length];
			array.AsSpan().CopyTo(array2.AsSpan(0, array.Length));
			payload.AsSpan().CopyTo(array2.AsSpan(array.Length, payload.Length));
			ushort num = ((checksumMode != SprdMain.Checksum.CRC) ? SprdChecksum.CalculateSprdChecksum(array2) : SprdChecksum.CalculateCRC16Ccitt(array2));
			byte[] array3 = new byte[2]
			{
				(byte)(num >> 8),
				(byte)(num & 0xFF)
			};
			byte[] array4 = new byte[array.Length + payload.Length + array3.Length];
			array.AsSpan().CopyTo(array4.AsSpan(0, array.Length));
			payload.AsSpan().CopyTo(array4.AsSpan(array.Length, payload.Length));
			array3.AsSpan().CopyTo(array4.AsSpan(array.Length + payload.Length, array3.Length));
			int num2 = 2 + array4.Length * 2;
			byte[] array5 = new byte[num2];
			int num3 = 0;
			num3 = 1;
			array5[0] = 126;
			if (useTranscode)
			{
				WriteEscaped(array5, ref num3, array4);
			}
			else
			{
				array4.AsSpan().CopyTo(array5.AsSpan(num3, array4.Length));
				num3 += array4.Length;
			}
			array5[num3++] = 126;
			byte[] array6 = new byte[num3];
			array5.AsSpan(0, num3).CopyTo(array6.AsSpan(0, num3));
			return array6;
		}
		return new byte[1] { 126 };
	}

	private static void WriteEscaped(byte[] dest, ref int pos, byte[] src)
	{
		foreach (byte b in src)
		{
			if (b == 126 || b == 125)
			{
				dest[pos++] = 125;
				dest[pos++] = (byte)(b ^ 0x20);
			}
			else
			{
				dest[pos++] = b;
			}
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
