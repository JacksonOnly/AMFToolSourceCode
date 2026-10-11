using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AMFTool.boot;

public class cpio_stream
{
	private MemoryStream cpioStream;

	private const int S_IFDIR = 16384;

	private const int S_IFMT = 61440;

	public cpio_stream(byte[] cpio)
	{
		cpioStream = new MemoryStream(cpio);
	}

	public List<string> parsecpio()
	{
		int num = 0;
		string text = "invalid";
		string value = "build.prop";
		string value2 = "prop.default";
		string value3 = "default.prop";
		int num2 = 1;
		int num3 = 10;
		string text2 = "TRAILER!!!";
		int num4 = 61440;
		int num5 = 40960;
		int num6 = 32768;
		MemoryStream memoryStream = cpioStream;
		long num7 = 0L;
		List<string> list = new List<string>();
		Func<int, int> func = (int x) => (-x + 1 - 1) & 3;
		while (memoryStream.Length > num7)
		{
			try
			{
				(string, int, int) tuple = read_cpio_header(memoryStream);
				string item = tuple.Item1;
				int item2 = tuple.Item2;
				int item3 = tuple.Item3;
				num7 += item3;
				if ((!(item != text)) ? ((byte)num != 0) : ((item.Contains(value) || item.Contains(value2)) ? ((byte)num2 != 0) : item.Contains(value3)))
				{
					byte[] array = new byte[item3];
					memoryStream.Read(array, num, array.Length);
					string text3 = Encoding.UTF8.GetString(array);
					char[] array2 = new char[num2];
					array2[num] = (char)num3;
					list.AddRange(text3.Split(array2));
					memoryStream.Seek(func(item3), (SeekOrigin)num2);
				}
				if (item == text2)
				{
					break;
				}
				if ((item2 & num4) != num5)
				{
					if ((item2 & num4) == num6)
					{
						byte[] buffer = new byte[item3];
						memoryStream.Read(buffer, num, item3);
						memoryStream.Seek(func(item3), (SeekOrigin)num2);
					}
					else
					{
						memoryStream.Seek(item3, (SeekOrigin)num2);
						memoryStream.Seek(func(item3), (SeekOrigin)num2);
					}
				}
				else
				{
					byte[] buffer2 = new byte[item3];
					memoryStream.Read(buffer2, num, item3);
					memoryStream.Seek(func(item3), (SeekOrigin)num2);
				}
				continue;
			}
			catch (Exception)
			{
				return new List<string>();
			}
		}
		return list;
	}

	public (string name, int mode, int filesize) read_cpio_header(Stream cpio)
	{
		string text = "070701";
		string item = "invalid";
		Func<int, int> func = (int x) => (-x + 1 - 1) & 3;
		byte[] array = new byte[6];
		cpio.Read(array, 0, 6);
		string text2 = Encoding.ASCII.GetString(array);
		if (!(text2 != text))
		{
			cpio.Seek(8L, SeekOrigin.Current);
			array = new byte[8];
			cpio.Read(array, 0, 8);
			int item2 = Convert.ToInt32(Encoding.ASCII.GetString(array), 16);
			cpio.Seek(32L, SeekOrigin.Current);
			cpio.Read(array, 0, 8);
			int item3 = Convert.ToInt32(Encoding.ASCII.GetString(array), 16);
			cpio.Seek(32L, SeekOrigin.Current);
			cpio.Read(array, 0, 8);
			int num = Convert.ToInt32(Encoding.ASCII.GetString(array), 16);
			cpio.Seek(8L, SeekOrigin.Current);
			array = new byte[num - 2 + 1];
			cpio.Read(array, 0, num - 1);
			string item4 = Encoding.UTF8.GetString(array);
			cpio.Seek(1L, SeekOrigin.Current);
			cpio.Seek(func(num + 110), SeekOrigin.Current);
			return (name: item4, mode: item2, filesize: item3);
		}
		return (name: item, mode: 0, filesize: array.Length);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
