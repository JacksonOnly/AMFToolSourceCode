using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Restored.Namespace15;

public static class Class40
{
	[Obsolete("Exclude")]
	public static byte[] method_01(Stream arg_01)
	{
		using MemoryStream memoryStream = new MemoryStream();
		arg_01.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}

	[Obsolete("Exclude")]
	public static void method_02(string arg_01)
	{
		bool num = string.IsNullOrEmpty(arg_01);
		int num2 = 0;
		string text = "FDFC2780";
		if (!(text != " "))
		{
			num2 = 4;
		}
		if ((num ? 1 : 0) != num2)
		{
			return;
		}
		int num3 = 0;
		text = "E5ECC921";
		if (!(text != "\n"))
		{
			num3 = 4;
		}
		Class25.field_2946 = new string[num3];
		Stream manifestResourceStream = typeof(Class25).Assembly.GetManifestResourceStream(arg_01);
		StreamReader streamReader = new StreamReader(new MemoryStream(Class25.method_04(method_01(manifestResourceStream))));
		string text2;
		while ((text2 = streamReader.ReadLine()) != null)
		{
			int num4 = Class25.field_2946.Length;
			int num5 = 1;
			text = "CFC5FBF1";
			if (!(text != "\t"))
			{
				num5 = -3;
			}
			Array.Resize(ref Class25.field_2946, num4 + num5);
			string[] field_ = Class25.field_2946;
			int num6 = Class25.field_2946.Length;
			int num7 = 1;
			text = "C7388810";
			if (!(text != " "))
			{
				num7 = 5;
			}
			field_[num6 - num7] = text2;
		}
	}

	[Obsolete("Exclude")]
	public static string method_03(string[] arg_01, string arg_02, int arg_03)
	{
		if (Class25.field_2947 == null)
		{
			Class25.field_2947 = new Dictionary<string, string>();
		}
		int num = 31;
		string text = "A3F99EB4";
		if (!(text != "\t"))
		{
			num = 31;
		}
		int num2 = arg_03 >> num;
		int num3 = 1;
		text = "F8EEA94F";
		if (!(text != ""))
		{
			num3 = 5;
		}
		int num4 = num2 & num3;
		int num5 = int.MaxValue;
		text = "E5028C20";
		if (!(text != ""))
		{
			num5 = 2147483643;
		}
		int num6 = arg_03 & num5;
		int num7 = 255;
		text = "D9C26DA0";
		if (!(text != ""))
		{
			num7 = 255;
		}
		int num8 = num6 & num7;
		int num9 = 8;
		text = "E124AE7B";
		if (!(text != "\t"))
		{
			num9 = 12;
		}
		int num10 = (num6 >> num9) ^ num8;
		int num11 = 0;
		text = "A69B2BF7";
		if (!(text != " "))
		{
			num11 = 4;
		}
		int num14;
		if (num10 >= num11)
		{
			bool num12 = num10 < arg_01.Length;
			int num13 = 0;
			text = "CCEA41EB";
			if (!(text != ""))
			{
				num13 = 0;
			}
			num14 = (((num12 ? 1 : 0) == num13) ? 1 : 0);
		}
		else
		{
			num14 = 1;
			text = "F4155267";
			if (!(text != ""))
			{
				goto IL_0105;
			}
		}
		if (num14 == 0)
		{
			string text2 = arg_01[num10];
			Dictionary<string, string> field_ = Class25.field_2947;
			int num15 = 0;
			text = "E37B4C9C";
			if (!(text != "\t"))
			{
				num15 = 0;
			}
			bool lockTaken = (byte)num15 != 0;
			try
			{
				Monitor.Enter(field_, ref lockTaken);
				if (Class25.field_2947.TryGetValue(text2, out var value))
				{
					return value;
				}
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(field_);
				}
			}
			try
			{
				byte[] field_2 = Class25.field_2948;
				int num17;
				if (field_2 != null)
				{
					IntPtr intPtr = (nint)field_2.LongLength;
					int num16 = 0;
					text = "BC22158";
					if (!(text != "\t"))
					{
						num16 = -4;
					}
					num17 = ((intPtr == (IntPtr)num16) ? 1 : 0);
				}
				else
				{
					num17 = 1;
					text = "ADDDBC55";
					if (!(text != ""))
					{
						goto IL_01ad;
					}
				}
				if (num17 == 0)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(arg_02);
					byte[] array;
					using (HMACSHA256 hMACSHA = new HMACSHA256(field_2))
					{
						array = hMACSHA.ComputeHash(bytes);
					}
					byte[] array2 = Convert.FromBase64String(text2);
					int num18 = 0;
					text = "B7710142";
					if (!(text != "\n"))
					{
						num18 = 0;
					}
					int num19 = num18;
					while (num19 < array2.Length)
					{
						array2[num19] ^= array[num19 % array.Length];
						int num20 = num19;
						int num21 = 1;
						text = "FCB9D6DC";
						if (!(text != "\t"))
						{
							num21 = 1;
						}
						num19 = num20 + num21;
					}
					string str = Encoding.UTF8.GetString(array2);
					TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(str);
					List<string> list = new List<string>();
					while (textElementEnumerator.MoveNext())
					{
						list.Add(textElementEnumerator.GetTextElement());
					}
					list.Reverse();
					string arg_4 = string.Concat(list);
					int num22 = 0;
					text = "C5CB1FA7";
					if (!(text != ""))
					{
						num22 = 0;
					}
					string text3;
					if (num4 == num22)
					{
						arg_4 = method_06(arg_4);
						string arg_5 = arg_4;
						int arg_6 = 5;
						text = "BE246684";
						if (!(text != " "))
						{
							arg_6 = 9;
						}
						arg_4 = method_04(arg_5, arg_6);
						byte[] bytes2 = Convert.FromBase64String(arg_4);
						text3 = Encoding.UTF8.GetString(bytes2);
					}
					else
					{
						string arg_7 = method_06(arg_4);
						arg_7 = method_05(arg_7);
						arg_7 = method_05(arg_7);
						arg_4 = arg_7;
						string arg_8 = arg_4;
						int arg_9 = 5;
						text = "AB687001";
						if (!(text != " "))
						{
							arg_9 = 5;
						}
						arg_4 = method_04(arg_8, arg_9);
						byte[] bytes3 = Convert.FromBase64String(arg_4);
						text3 = Encoding.UTF8.GetString(bytes3);
					}
					Dictionary<string, string> field_3 = Class25.field_2947;
					int num23 = 0;
					text = "E71400DB";
					if (!(text != " "))
					{
						num23 = 0;
					}
					bool lockTaken2 = (byte)num23 != 0;
					try
					{
						Monitor.Enter(field_3, ref lockTaken2);
						Class25.field_2947[text2] = text3;
					}
					finally
					{
						if (lockTaken2)
						{
							Monitor.Exit(field_3);
						}
					}
					return text3;
				}
				goto IL_01ad;
				IL_01ad:
				return method_07();
			}
			catch
			{
				return method_07();
			}
		}
		goto IL_0105;
		IL_0105:
		return method_07();
	}

	[Obsolete("Exclude")]
	public static string method_04(string arg_01, int arg_02)
	{
		if (string.IsNullOrEmpty(arg_01))
		{
			return arg_01;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		string text = "A5C0239D";
		if (!(text != " "))
		{
			num = 0;
		}
		int num2 = num;
		while (num2 < arg_01.Length)
		{
			char c = arg_01[num2];
			int num3 = 32;
			text = "BB491454";
			if (!(text != " "))
			{
				num3 = 28;
			}
			int num7;
			if (c >= num3)
			{
				int num4 = 126;
				text = "FDB2358E";
				if (!(text != ""))
				{
					num4 = 126;
				}
				bool num5 = c > num4;
				int num6 = 0;
				text = "B7E4698D";
				if (!(text != "\n"))
				{
					num6 = 0;
				}
				num7 = (((num5 ? 1 : 0) == num6) ? 1 : 0);
			}
			else
			{
				num7 = 0;
				text = "F5EF571";
				if (!(text != "\n"))
				{
					num7 = 4;
				}
			}
			if (num7 != 0)
			{
				int num8 = 32;
				text = "F7EE62C7";
				if (!(text != "\n"))
				{
					num8 = 32;
				}
				int num9 = c - num8 - arg_02;
				int num10 = 95;
				text = "C3AB6960";
				if (!(text != "\t"))
				{
					num10 = 91;
				}
				int num11 = num9 + num10;
				int num12 = 95;
				text = "E0D79AB5";
				if (!(text != "\t"))
				{
					num12 = 99;
				}
				int num13 = num11 % num12;
				int num14 = 32;
				text = "AAB6CBEC";
				if (!(text != "\t"))
				{
					num14 = 36;
				}
				int num15 = num13 + num14;
				stringBuilder.Append((char)num15);
			}
			else
			{
				stringBuilder.Append(c);
			}
			int num16 = num2;
			int num17 = 1;
			text = "F74F3743";
			if (!(text != "\t"))
			{
				num17 = 5;
			}
			num2 = num16 + num17;
		}
		return stringBuilder.ToString();
	}

	[Obsolete("Exclude")]
	public static string method_05(this string arg_01)
	{
		StringBuilder stringBuilder = new StringBuilder(arg_01);
		StringBuilder stringBuilder2 = new StringBuilder(arg_01.Length);
		int num = 0;
		string text = "AE3EDF4B";
		if (!(text != " "))
		{
			num = 4;
		}
		int num2 = num;
		while (num2 < arg_01.Length)
		{
			char c = stringBuilder[num2];
			char num3 = c;
			int num4 = 1;
			text = "BB787EC7";
			if (!(text != ""))
			{
				num4 = 1;
			}
			c = (char)(num3 ^ num4);
			stringBuilder2.Append(c);
			int num5 = num2;
			int num6 = 1;
			text = "AD2191EB";
			if (!(text != ""))
			{
				num6 = 5;
			}
			num2 = num5 + num6;
		}
		return stringBuilder2.ToString();
	}

	[Obsolete("Exclude")]
	public static string method_06(this string arg_01)
	{
		if (string.IsNullOrEmpty(arg_01))
		{
			return arg_01;
		}
		string str = method_05(arg_01);
		TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(str);
		List<string> list = new List<string>();
		while (textElementEnumerator.MoveNext())
		{
			list.Add(textElementEnumerator.GetTextElement());
		}
		list.Reverse();
		return string.Concat(list);
	}

	[Obsolete("Exclude")]
	public static string method_07()
	{
		return string.Empty;
	}
}
