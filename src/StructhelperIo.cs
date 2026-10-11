using System;
using System.IO;
using System.Text;

public class StructhelperIo
{
	private int pos = 0;

	private MemoryStream data;

	private string direction;

	public StructhelperIo(MemoryStream data = null, string direction = "little")
	{
		this.data = data;
		this.direction = direction;
	}

	public void setdata(MemoryStream data, int pos = 0)
	{
		this.pos = pos;
		this.data = data;
	}

	public long qword()
	{
		byte[] array = new byte[8];
		data.Read(array, 0, 8);
		return BitConverter.ToInt64(array, 0);
	}

	public int dword()
	{
		byte[] array = new byte[4];
		data.Read(array, 0, 4);
		return BitConverter.ToInt32(array, 0);
	}

	public int[] dwords(int dwordss = 1)
	{
		int[] array = new int[dwordss];
		int i = 0;
		for (int num = dwordss - 1; i <= num; i++)
		{
			byte[] array2 = new byte[4];
			data.Read(array2, 0, 4);
			array[i] = BitConverter.ToInt32(array2, 0);
		}
		return array;
	}

	public short _short()
	{
		byte[] array = new byte[2];
		data.Read(array, 0, 2);
		return BitConverter.ToInt16(array, 0);
	}

	public short[] shorts(int shortss)
	{
		short[] array = new short[shortss];
		int i = 0;
		for (int num = shortss - 1; i <= num; i++)
		{
			byte[] array2 = new byte[2];
			data.Read(array2, 0, 2);
			array[i] = BitConverter.ToInt16(array2, 0);
		}
		return array;
	}

	public byte[] bytes(int rlen = 1)
	{
		byte[] array = new byte[rlen];
		data.Read(array, 0, rlen);
		return array;
	}

	public string _string(int rlen = 1)
	{
		byte[] buffer = new byte[rlen];
		data.Read(buffer, 0, rlen);
		return Encoding.ASCII.GetString(buffer);
	}

	public int getpos()
	{
		return (int)data.Position;
	}

	public void seek(int pos)
	{
		data.Seek(pos, SeekOrigin.Begin);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
