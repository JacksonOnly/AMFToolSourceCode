using System;
using System.IO;

namespace AMFTool;

public class StructHelperIo
{
	private Stream Data;

	private bool Little;

	

	public StructHelperIo(Stream data = null, string direction = "little")
	{
		string text = "little";
		Little = true;

		this.Data = data;
		Little = direction == text;
	}

	public void SetData(Stream data, long offset = 0L)
	{
		this.Data = data;
		data.Seek(offset, SeekOrigin.Begin);
	}

	public ulong Qword()
	{
		byte[] buffer = ReadExact(8);
		return ToUInt64(buffer);
	}

	public uint Dword()
	{
		byte[] buffer = ReadExact(4);
		return ToUInt32(buffer);
	}

	public uint[] Dwords(int dwordCount = 1)
	{
		uint[] array = new uint[dwordCount];
		for (int i = 0; i < dwordCount; i++)
		{
			array[i] = Dword();
		}
		return array;
	}

	public ushort Short()
	{
		byte[] buffer = ReadExact(2);
		return ToUInt16(buffer);
	}

	public ushort[] Shorts(int shortCount)
	{
		ushort[] array = new ushort[shortCount];
		for (int i = 0; i < shortCount; i++)
		{
			array[i] = Short();
		}
		return array;
	}

	public byte[] Bytes(int readLength = 1)
	{
		if (readLength > 0)
		{
			byte[] array = new byte[readLength];
			int num = this.Data.Read(array, 0, readLength);
			if (num != 0)
			{
				if (num != readLength)
				{
					Array.Resize(ref array, num);
					return array;
				}
				return array;
			}
			return Array.Empty<byte>();
		}
		return Array.Empty<byte>();
	}

	public byte[] StringBytes(int readLength = 1)
	{
		return Bytes(readLength);
	}

	public long GetPos()
	{
		return this.Data.Position;
	}

	public void Seek(long pos)
	{
		this.Data.Seek(pos, SeekOrigin.Begin);
	}

	private byte[] ReadExact(int length)
	{
		byte[] array = new byte[length];
		int num = this.Data.Read(array, 0, length);
		if (num != length)
		{
			throw new EndOfStreamException();
		}
		return array;
	}

	private ushort ToUInt16(byte[] buffer)
	{
		if (BitConverter.IsLittleEndian != Little)
		{
			Array.Reverse(buffer);
			return BitConverter.ToUInt16(buffer, 0);
		}
		return BitConverter.ToUInt16(buffer, 0);
	}

	private uint ToUInt32(byte[] buffer)
	{
		if (BitConverter.IsLittleEndian == Little)
		{
			return BitConverter.ToUInt32(buffer, 0);
		}
		Array.Reverse(buffer);
		return BitConverter.ToUInt32(buffer, 0);
	}

	private ulong ToUInt64(byte[] buffer)
	{
		if (BitConverter.IsLittleEndian != Little)
		{
			Array.Reverse(buffer);
			return BitConverter.ToUInt64(buffer, 0);
		}
		return BitConverter.ToUInt64(buffer, 0);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
