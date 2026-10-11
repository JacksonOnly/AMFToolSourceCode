using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AMFTool.Library.Qualcomm;

public class Xml
{
	public static byte[] FindData(byte[] input)
	{
		string s = "<response value=\"ACK\" rawmode=\"true\" />";
		string s2 = "</data>";
		int num = 0;
		List<byte[]> list = new List<byte[]>
		{
			Encoding.UTF8.GetBytes(s),
			Encoding.UTF8.GetBytes(s2)
		};
		int num2 = 0;
		foreach (byte[] item in list)
		{
			int num3 = Extentions.IndexOf(input, item, num2);
			if (num3 != -1)
			{
				num2 = num3 + item.Length;
			}
		}
		if (num2 != num)
		{
			int num4 = input.Length - num2;
			byte[] array = new byte[num4];
			Buffer.BlockCopy(input, num2, array, num, num4);
			return array;
		}
		return input;
	}

	public static byte[] FindGPT(byte[] input)
	{
		string item = "<response value=\"ACK\" rawmode=\"true\" />";
		string item2 = "</data>";
		string s = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
		input = input.ToArray();
		List<string> list = new List<string> { item, item2 };
		for (int i = 0; i < list.Count; i++)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(list[i]);
			int num = input.IndexOf(bytes);
			if (num != -1)
			{
				input = input.Skip(num + bytes.Length).ToArray();
			}
			byte[] bytes2 = Encoding.UTF8.GetBytes(s);
			int num2 = input.IndexOf(bytes2);
			if (num2 != -1)
			{
				input = input.Take(num2).ToArray();
			}
		}
		return input;
	}

	public static byte[] Parse(byte[] input)
	{
		string s = "rawmode=\"true\"";
		string s2 = "</data>";
		string s3 = "<?xml";
		if (input != null && input.Length != 0)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			int num = input.IndexOf(bytes);
			if (num != -1)
			{
				byte[] bytes2 = Encoding.ASCII.GetBytes(s2);
				int num2 = Extentions.IndexOf(input, bytes2, num);
				if (num2 == -1)
				{
					return input;
				}
				int num3 = num2 + bytes2.Length;
				byte[] bytes3 = Encoding.UTF8.GetBytes(s3);
				int num4 = input.LastIndexOf(bytes3);
				int num5;
				if (num4 <= num3 || num4 == -1)
				{
					num4 = input.Length;
					num5 = num4 - num3;
					if (num5 > 0)
					{
						byte[] array = new byte[num5];
						Buffer.BlockCopy(input, num3, array, 0, num5);
						return array;
					}
					return Array.Empty<byte>();
				}
				num5 = num4 - num3;
				if (num5 > 0)
				{
					byte[] array = new byte[num5];
					Buffer.BlockCopy(input, num3, array, 0, num5);
					return array;
				}
				return Array.Empty<byte>();
			}
			return input;
		}
		return Array.Empty<byte>();
	}

	public static List<string> GetLog(byte[] input)
	{
		string value = "value=\"";
		string value2 = "INFO: ";
		string value3 = "<log";
		List<string> list = new List<string>();
		string text = Encoding.UTF8.GetString(input);
		int startIndex = 0;
		while ((startIndex = text.IndexOf(value3, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
		{
			int num = text.IndexOf(value, startIndex, StringComparison.OrdinalIgnoreCase);
			if (num < 0)
			{
				startIndex += 4;
				continue;
			}
			num += 7;
			int num2 = text.IndexOf('"', num);
			if (num2 >= 0)
			{
				string value4 = text.Substring(num, num2 - num);
				string text2 = WebUtility.HtmlDecode(value4);
				text2 = ((!text2.StartsWith(value2)) ? text2 : text2.Substring(6));
				list.Add(text2);
				startIndex = num2 + 1;
			}
			else
			{
				startIndex = num;
			}
		}
		return list;
	}

	public static Dictionary<string, string> GetResponse(byte[] input)
	{
		string value = "=\"";
		string value2 = "<response";
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		string text = Encoding.UTF8.GetString(input);
		int startIndex = 0;
		while ((startIndex = text.IndexOf(value2, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
		{
			int num = text.IndexOf('>', startIndex);
			if (num < 0)
			{
				break;
			}
			string text2 = text.Substring(startIndex, num - startIndex);
			int startIndex2 = 0;
			while ((startIndex2 = text2.IndexOf(value, startIndex2, StringComparison.Ordinal)) != -1)
			{
				int num2 = text2.LastIndexOf(' ', startIndex2);
				if (num2 != -1)
				{
					string key = text2.Substring(num2 + 1, startIndex2 - num2 - 1).Trim();
					int num3 = startIndex2 + 2;
					int num4 = text2.IndexOf('"', num3);
					if (num4 != -1)
					{
						dictionary[key] = text2.Substring(num3, num4 - num3);
						startIndex2 = num4 + 1;
						continue;
					}
				}
				startIndex2 += 2;
			}
			startIndex = num + 1;
		}
		return dictionary;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
