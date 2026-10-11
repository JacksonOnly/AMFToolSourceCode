using System;
using System.Text;

namespace AMFTool;

public class StructHelper
{
	private int Pos = 0;

	private byte[] Data;

	public StructHelper(byte[] data, int pos = 0)
	{
		Pos = pos;
		Data = data;
	}

	public ulong Qword(bool big = false)
	{
		ulong result = BitConverter.ToUInt64(Data, Pos);
		Pos += 8;
		return result;
	}

	public uint Dword(bool big = false)
	{
		uint result = BitConverter.ToUInt32(Data, Pos);
		Pos += 4;
		return result;
	}

	public uint[] Dwords(int dwordCount = 1, bool big = false)
	{
		uint[] array = new uint[dwordCount];
		for (int i = 0; i < dwordCount; i++)
		{
			array[i] = BitConverter.ToUInt32(Data, Pos);
			Pos += 4;
		}
		return array;
	}

	public ulong[] Qwords(int qwordCount = 1, bool big = false)
	{
		ulong[] array = new ulong[qwordCount];
		for (int i = 0; i < qwordCount; i++)
		{
			array[i] = BitConverter.ToUInt64(Data, Pos);
			Pos += 8;
		}
		return array;
	}

	public ushort Short(bool big = false)
	{
		ushort result = BitConverter.ToUInt16(Data, Pos);
		Pos += 2;
		return result;
	}

	public ushort[] UShorts(int shortCount, bool big = false)
	{
		ushort[] array = new ushort[shortCount];
		for (int i = 0; i < shortCount; i++)
		{
			array[i] = BitConverter.ToUInt16(Data, Pos);
			Pos += 2;
		}
		return array;
	}

	public byte[] Bytes(int readLength = 1)
	{
		byte[] array = new byte[readLength];
		Array.Copy(Data, Pos, array, 0, readLength);
		Pos += readLength;
		if (readLength == 1)
		{
			return new byte[1] { array[0] };
		}
		return array;
	}

	public string Strings(int readLength = 1)
	{
		string result = Encoding.UTF8.GetString(Data, Pos, readLength);
		Pos += readLength;
		return result;
	}

	public int GetPos()
	{
		return Pos;
	}

	public void Seek(int pos)
	{
		Pos = pos;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
