using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualBasic;

namespace AMFTool;

public class utils
{
	public static readonly string BaseDirectory;

	public static bool Validloader(byte[] loader)
	{
		if (loader.Length >= 4)
		{
			if (loader[0] == 127)
			{
				if (loader[1] == 69)
				{
					if (loader[2] == 76)
					{
						return loader[3] == 70;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public static string FileSize(double byteCount)
	{
		string text = "0 Bytes";
		double field_ = Class38.field_531;
		string format = "{0:##.##}";
		string text2 = "TiB";
		double field_2 = Class38.field_534;
		double field_3 = Class38.field_535;
		string text3 = "GiB";
		double field_4 = Class38.field_537;
		double field_5 = Class38.field_538;
		string text4 = "MiB";
		double field_6 = Class38.field_540;
		double field_7 = Class38.field_541;
		string text5 = "KiB";
		double field_8 = Class38.field_543;
		double field_9 = Class38.field_544;
		string text6 = "b";
		string result = text;
		if (byteCount >= field_)
		{
			return string.Format(format, byteCount / field_) + text2;
		}
		if (byteCount >= field_2 && byteCount <= field_3)
		{
			return string.Format(format, byteCount / field_2) + text3;
		}
		if (byteCount >= field_4 && byteCount <= field_5)
		{
			return string.Format(format, byteCount / field_4) + text4;
		}
		if (!(byteCount >= field_6) || !(byteCount <= field_7))
		{
			if (byteCount >= field_8 && byteCount <= field_9)
			{
				return byteCount + text6;
			}
			return result;
		}
		return string.Format(format, byteCount / field_6) + text5;
	}

	public static string ExtractSubstring(string source, string startDelimiter, string endDelimiter)
	{
		string result = "";
		if (source.Contains(startDelimiter))
		{
			if (!source.Contains(endDelimiter))
			{
				return string.Empty;
			}
			int num = source.IndexOf(startDelimiter) + startDelimiter.Length;
			int num2 = source.IndexOf(endDelimiter, num);
			if (num2 != -1)
			{
				if (string.IsNullOrEmpty(endDelimiter))
				{
					return source.Substring(num);
				}
				return source.Substring(num, num2 - num);
			}
			return result;
		}
		return string.Empty;
	}

	public static string HexToString(byte[] Hex)
	{
		string oldValue = "-";
		string newValue = "";
		string value = "?";
		string pattern = "\\s+";
		string replacement = " ";
		string text = BitConverter.ToString(Hex);
		string text2 = text.Replace(oldValue, newValue);
		StringBuilder stringBuilder = new StringBuilder(Hex.Length / 2);
		int i = 0;
		for (int num = text2.Length - 2; i <= num; i += 2)
		{
			uint num2 = Convert.ToUInt32(text2.Substring(i, 2), 16);
			if ((long)num2 < 32L || (long)num2 > 126L)
			{
				stringBuilder.Append(value);
			}
			else
			{
				stringBuilder.Append(Convert.ToChar(num2));
			}
		}
		Regex regex = new Regex(pattern);
		string text3 = regex.Replace(stringBuilder.ToString(), replacement);
		return text3.ToString();
	}

	public static byte[] StringToByteArray(string hex)
	{
		string oldValue = " ";
		string oldValue2 = "-";
		string oldValue3 = "0x";
		string text = "0";
		string val = hex.Replace(oldValue, string.Empty).Replace(oldValue2, string.Empty).ToLower()
			.Replace(oldValue3, string.Empty);
		if (val.Length % 2 > 0)
		{
			string text2 = text + val.Substring(val.Length - 1);
			val = val.Substring(0, val.Length - 1) + text2;
			return (from x in Enumerable.Range(0, val.Length)
				where x % 2 == 0
				select Convert.ToByte(val.Substring(x, 2), 16)).ToArray();
		}
		return (from x in Enumerable.Range(0, val.Length)
			where x % 2 == 0
			select Convert.ToByte(val.Substring(x, 2), 16)).ToArray();
	}

	public static string BytesToHextring(byte[] input)
	{
		string oldValue = "-";
		string newValue = "";
		return BitConverter.ToString(input).Replace(oldValue, newValue).ToLower();
	}

	public static string BytesToHexString(byte[] byteInput)
	{
		string oldValue = "-";
		object newValue = null;
		return BitConverter.ToString(byteInput).Replace(oldValue, (string)newValue);
	}

	public static byte[] HexStringToBytes(string s)
	{
		string oldValue = " ";
		string newValue = "";
		int num = 0;
		object title = null;
		try
		{
			s = s.Replace(oldValue, newValue);
			int num2 = s.Length / 2;
			byte[] array = new byte[num2];
			int i = 0;
			for (int num3 = num2 - 1; i <= num3; i++)
			{
				array[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
			}
			return array;
		}
		catch (Exception ex)
		{
			Interaction.MsgBox(ex.ToString(), (MsgBoxStyle)num, title);
		}
		return new byte[num];
	}

	public static byte[] index(byte[] data, object idx, object len)
	{
		object obj = null;
		int num = 0;
		int num2 = data.Length;
		if (((idx is int) ? idx : null) ==null)
		{
			if (((idx is uint) ? idx : null) !=null)
			{
				num = Convert.ToInt32(idx);
			}
			else if (((idx is long) ? idx : null) !=null)
			{
				num = Convert.ToInt32(idx);
			}
			else if (((idx is ulong) ? idx : null) ==null)
			{
				if (((idx is short) ? idx : null) ==null)
				{
					if (((idx is ushort) ? idx : null) !=null)
					{
						num = Convert.ToInt32(idx);
					}
				}
				else
				{
					num = Convert.ToInt32(idx);
				}
			}
			else
			{
				num = Convert.ToInt32(idx);
			}
		}
		else
		{
			num = (int)idx;
		}
		if (((len is int) ? len : null) !=null)
		{
			num2 = (int)len;
		}
		else if (((len is uint) ? len : null) ==null)
		{
			if (((len is long) ? len : null) !=null)
			{
				num2 = Convert.ToInt32(len);
			}
			else if (((len is ulong) ? len : null) !=null)
			{
				num2 = Convert.ToInt32(len);
			}
			else if (((len is short) ? len : null) !=null)
			{
				num2 = Convert.ToInt32(len);
			}
			else if (((len is ushort) ? len : null) !=null)
			{
				num2 = Convert.ToInt32(len);
			}
		}
		else
		{
			num2 = Convert.ToInt32(len);
		}
		if (data != obj)
		{
			if (num2 > data.Length - num)
			{
				num2 = data.Length - num;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			byte[] array = new byte[num2];
			Array.Copy(data, num, array, 0, num2);
			return array;
		}
		return (byte[])obj;
	}

	public static object[] unpack(string fmt, byte[] bytes)
	{
		string message = "StringCannotEmpty";
		string value = ">";
		string value2 = "<";
		string message2 = "InvalidCharInString";
		string message3 = "FailedUnpack";
		if (fmt.Length < 1)
		{
			throw new ArgumentException(message);
		}
		bool littleEndian = false;
		int num = 0;
		if (fmt.StartsWith(value))
		{
			littleEndian = true;
			fmt = fmt.Substring(1);
		}
		else if (fmt.StartsWith(value2))
		{
			fmt = fmt.Substring(1);
		}
		char[] array = fmt.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			switch (array[i])
			{
			case 'Q':
			case 'q':
				num += 8;
				break;
			default:
				throw new ArgumentException(message2);
			case 'H':
			case 'h':
				num += 2;
				break;
			case 'B':
			case 'b':
			case 'x':
				num++;
				break;
			case 'I':
			case 'i':
				num += 4;
				break;
			}
		}
		if (bytes == null || bytes.Length != num)
		{
			throw new ArgumentException(message3);
		}
		int num2 = 0;
		List<object> list = new List<object>();
		char[] array2 = fmt.ToCharArray();
		for (int j = 0; j < array2.Length; j++)
		{
			switch (array2[j])
			{
			case 'x':
				num2++;
				break;
			case 'h':
			{
				byte[] array10 = new byte[2];
				Array.Copy(bytes, num2, array10, 0, 2);
				short num8 = FromBuffer<short>(array10, littleEndian);
				list.Add(num8);
				num2 += 2;
				break;
			}
			case 'i':
			case 'l':
			{
				byte[] array9 = new byte[4];
				Array.Copy(bytes, num2, array9, 0, 4);
				int num7 = FromBuffer<int>(array9, littleEndian);
				list.Add(num7);
				num2 += 4;
				break;
			}
			case 'b':
			{
				byte[] array8 = new byte[1];
				Array.Copy(bytes, num2, array8, 0, 1);
				sbyte b2 = FromBuffer<sbyte>(array8, littleEndian);
				list.Add(b2);
				num2++;
				break;
			}
			case 'Q':
			{
				byte[] array7 = new byte[8];
				Array.Copy(bytes, num2, array7, 0, 8);
				ulong num6 = FromBuffer<ulong>(array7, littleEndian);
				list.Add(num6);
				num2 += 8;
				break;
			}
			case 'H':
			{
				byte[] array6 = new byte[2];
				Array.Copy(bytes, num2, array6, 0, 2);
				ushort num5 = FromBuffer<ushort>(array6, littleEndian);
				list.Add(num5);
				num2 += 2;
				break;
			}
			default:
				throw new ArgumentException(message3);
			case 'I':
			case 'L':
			{
				byte[] array5 = new byte[4];
				Array.Copy(bytes, num2, array5, 0, 4);
				uint num4 = FromBuffer<uint>(array5, littleEndian);
				list.Add(num4);
				num2 += 4;
				break;
			}
			case 'B':
			{
				byte[] array4 = new byte[1];
				Array.Copy(bytes, num2, array4, 0, 1);
				byte b = FromBuffer<byte>(array4, littleEndian);
				list.Add(b);
				num2++;
				break;
			}
			case 'q':
			{
				byte[] array3 = new byte[8];
				Array.Copy(bytes, num2, array3, 0, 8);
				long num3 = FromBuffer<long>(array3, littleEndian);
				list.Add(num3);
				num2 += 8;
				break;
			}
			}
		}
		return list.ToArray();
	}

	public static T FromBuffer<T>(byte[] buffer, bool LittleEndian) where T : struct
	{
		T structure;
		int num;
		IntPtr intPtr;
		T result;
		if (!LittleEndian)
		{
			structure = new T();
			num = Marshal.SizeOf(structure);
			intPtr = Marshal.AllocHGlobal(num);
			Marshal.Copy(buffer, 0, intPtr, num);
			result = (T)Marshal.PtrToStructure(intPtr, structure.GetType());
			Marshal.FreeHGlobal(intPtr);
			return result;
		}
		Array.Reverse(buffer);
		structure = new T();
		num = Marshal.SizeOf(structure);
		intPtr = Marshal.AllocHGlobal(num);
		Marshal.Copy(buffer, 0, intPtr, num);
		result = (T)Marshal.PtrToStructure(intPtr, structure.GetType());
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public static string HexToDec(string hexstring)
	{
		string text = "none";
		string text2 = "0";
		if (hexstring == text)
		{
			hexstring = text2;
			return Convert.ToInt64(hexstring, 16).ToString();
		}
		return Convert.ToInt64(hexstring, 16).ToString();
	}

	public static string FileSize(string hex)
	{
		string text = "0x";
		object provider = null;
		string text2 = "0 Bytes";
		double field_ = Class38.field_540;
		double field_2 = Class38.field_534;
		string format = "{0:##.##}";
		string text3 = "TiB";
		double field_3 = Class38.field_537;
		string text4 = "GiB";
		string text5 = "MiB";
		double field_4 = Class38.field_543;
		string text6 = "KiB";
		long result;
		string result2;
		double num;
		if (!hex.StartsWith(text))
		{
			long.TryParse(hex, NumberStyles.HexNumber, (IFormatProvider)provider, out result);
			result2 = text2;
			num = result;
			num /= field_;
			if (num >= field_2)
			{
				return string.Format(format, num / field_2) + text3;
			}
			if (!(num >= field_3))
			{
				if (num >= field_)
				{
					return string.Format(format, num / field_) + text5;
				}
				if (num > field_4 && num < field_)
				{
					return num + text6;
				}
				return result2;
			}
			return string.Format(format, num / field_3) + text4;
		}
		hex = hex.Replace(text, string.Empty);
		long.TryParse(hex, NumberStyles.HexNumber, (IFormatProvider)provider, out result);
		result2 = text2;
		num = result;
		num /= field_;
		if (num >= field_2)
		{
			return string.Format(format, num / field_2) + text3;
		}
		if (!(num >= field_3))
		{
			if (num >= field_)
			{
				return string.Format(format, num / field_) + text5;
			}
			if (num > field_4 && num < field_)
			{
				return num + text6;
			}
			return result2;
		}
		return string.Format(format, num / field_3) + text4;
	}

	public static string FixSize(string sizeInKBStr)
	{
		string result = "Invalid size";
		double field_ = Class38.field_540;
		double field_2 = Class38.field_537;
		string format = "{0} MB";
		string format2 = "{0} KB";
		string format3 = "{0} bytes";
		if (double.TryParse(sizeInKBStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var result2))
		{
			double num = result2 * field_;
			if (!(num >= field_2))
			{
				if (!(num >= field_))
				{
					return string.Format(format3, Math.Round(num));
				}
				return string.Format(format2, Math.Round(num / field_));
			}
			return string.Format(format, Math.Round(num / field_2, 1));
		}
		return result;
	}

	public static bool IsNumber(object value)
	{
		object obj = null;
		if (value is sbyte)
		{
			return true;
		}
		if (!(value is byte))
		{
			if (!(value is short))
			{
				bool flag;
				if (!(value is ushort))
				{
					if (!(value is int))
					{
						if (!(value is uint))
						{
							if (!(value is long))
							{
								if (!(value is ulong))
								{
									if (!(value is float))
									{
										if (!(value is double))
										{
											return flag = ((value is decimal) ? value : null) !=null;
										}
										return true;
									}
									return true;
								}
								return true;
							}
							return true;
						}
						return true;
					}
					return true;
				}
				flag = true;
				return true;
			}
			return true;
		}
		return true;
	}

	public static void Pause(double seconds)
	{
		double field_ = Class38.field_557;
		Thread.Sleep((int)(seconds * field_));
	}

	public static string CheckBackupFolder(string name)
	{
		object obj = null;
		string text = "CPID";
		string text2 = "CRITICAL";
		string text3 = "QCN";
		string text4 = "PIT";
		string text5 = "EFS";
		string text6 = "NV";
		string text7 = "RPMB";
		string text8 = "TEMP";
		string text9 = "PRELOADER";
		string text10 = "SCREENSHOT";
		string text11 = "OTHER";
		string path = "Backup";
		string path2 = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		string text12 = (string)((name == null) ? obj : name.ToUpperInvariant());
		uint num = ImplementationDetails.method_01(text12);
		string text13;
		if (num <= 1889672913)
		{
			if (num <= 719742853)
			{
				if (num == 591418632)
				{
					if (text12 == text2)
					{
						text13 = text2;
						goto IL_009b;
					}
				}
				else if (num == 719742853 && text12 == text6)
				{
					text13 = text6;
					goto IL_009b;
				}
			}
			else if (num == 1465986161)
			{
				if (text12 == text10)
				{
					text13 = text10;
					goto IL_009b;
				}
			}
			else if (num == 1743900761)
			{
				if (text12 == text3)
				{
					text13 = text3;
					goto IL_009b;
				}
			}
			else if (num == 1889672913 && text12 == text9)
			{
				text13 = text9;
				goto IL_009b;
			}
		}
		else if (num > 2116406812)
		{
			if (num != 3273820103u)
			{
				if (num == 3724015634u)
				{
					if (text12 == text4)
					{
						text13 = text4;
						goto IL_009b;
					}
				}
				else if (num == 3929040691u && text12 == text5)
				{
					text13 = text5;
					goto IL_009b;
				}
			}
			else if (text12 == text8)
			{
				text13 = text8;
				goto IL_009b;
			}
		}
		else if (num == 2068768091)
		{
			if (text12 == text)
			{
				text13 = text;
				goto IL_009b;
			}
		}
		else if (num == 2116406812 && text12 == text7)
		{
			text13 = text7;
			goto IL_009b;
		}
		text13 = text11;
		goto IL_009b;
		IL_009b:
		string path3 = text13;
		string text14 = Path.Combine(path2, path, path3);
		if (!Directory.Exists(text14))
		{
			Directory.CreateDirectory(text14);
		}
		return text14;
	}

	public static int FindBinary(ReadOnlySpan<byte> data, ReadOnlySpan<byte> pattern, int pos = 0)
	{
		int num = ((pos < 0) ? (data.Length + pos) : pos);
		if (num < 0)
		{
			num = 0;
		}
		ReadOnlySpan<byte> readOnlySpan = ((num <= data.Length) ? data.Slice(num) : ReadOnlySpan<byte>.Empty);
		int num2 = pattern.IndexOf<byte>(46);
		int num3 = ((num2 >= 0) ? num2 : pattern.Length);
		ReadOnlySpan<byte> value = pattern.Slice(0, num3);
		int num4 = 0;
		int num6;
		while (true)
		{
			if (num4 == -1)
			{
				return -1;
			}
			if (value.Length != 0)
			{
				if (num4 <= readOnlySpan.Length)
				{
					int num5 = readOnlySpan.Slice(num4).IndexOf(value);
					num6 = ((num5 == -1) ? (-1) : (num4 + num5));
				}
				else
				{
					num6 = -1;
				}
			}
			else
			{
				num6 = ((num4 <= readOnlySpan.Length) ? num4 : (-1));
			}
			if (num6 != -1)
			{
				int num7 = num6 + value.Length;
				bool flag = true;
				int num8 = num3;
				while (num8 < pattern.Length && pattern[num8] == 46)
				{
					int num9 = num8 + 1;
					int i;
					for (i = num9; i < pattern.Length && pattern[i] != 46; i++)
					{
					}
					int num10 = i - num9;
					num7++;
					if (num10 != 0)
					{
						int num11 = num + num7;
						if (num11 < 0 || num11 + num10 > data.Length)
						{
							flag = false;
							break;
						}
						if (!data.Slice(num11, num10).SequenceEqual(pattern.Slice(num9, num10)))
						{
							flag = false;
							break;
						}
						num7 += num10;
					}
					num8 = i;
				}
				if (flag)
				{
					break;
				}
				num4 = num6 + 1;
				continue;
			}
			return -1;
		}
		return num + num6;
	}

	public static byte[] StructToBytes(object structObj, int length)
	{
		byte[] array = new byte[length];
		IntPtr intPtr = Marshal.AllocHGlobal(length);
		Marshal.StructureToPtr(structObj, intPtr, fDeleteOld: false);
		Marshal.Copy(intPtr, array, 0, length);
		Marshal.FreeHGlobal(intPtr);
		return array;
	}

	public static object BytesToStuct(byte[] bytes, Type type)
	{
		object result = null;
		int num = Marshal.SizeOf(type);
		if (num <= bytes.Length)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.Copy(bytes, 0, intPtr, num);
			object result2 = Marshal.PtrToStructure(intPtr, type);
			Marshal.FreeHGlobal(intPtr);
			return result2;
		}
		return result;
	}

	public static byte[] clearing(byte[] data, int header = 27, int tail = 1)
	{
		object result = null;
		try
		{
			int num = data.Length - header - tail;
			if (num > 0)
			{
				string text = Encoding.ASCII.GetString(data, header, num);
				StringBuilder stringBuilder = new StringBuilder();
				string text2 = text;
				foreach (char c in text2)
				{
					if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))
					{
						stringBuilder.Append(c);
					}
				}
				string text3 = stringBuilder.ToString();
				if (text3.Length % 2 != 0)
				{
					text3 = text3.Substring(0, text3.Length - 1);
				}
				byte[] array = new byte[text3.Length / 2];
				for (int j = 0; j < text3.Length; j += 2)
				{
					array[j / 2] = Convert.ToByte(text3.Substring(j, 2), 16);
				}
				return array;
			}
			return (byte[])result;
		}
		catch
		{
			return (byte[])result;
		}
	}

	public static byte[] GetFromIndex(byte[] data, int index, int len)
	{
		object obj = null;
		if (data != obj)
		{
			byte[] array;
			if (len > data.Length - index)
			{
				len = data.Length - index;
				if (len >= 0)
				{
					array = new byte[len];
					Array.Copy(data, index, array, 0, len);
					return array;
				}
				len = 0;
				array = new byte[0];
				Array.Copy(data, index, array, 0, 0);
				return array;
			}
			if (len >= 0)
			{
				array = new byte[len];
				Array.Copy(data, index, array, 0, len);
				return array;
			}
			len = 0;
			array = new byte[0];
			Array.Copy(data, index, array, 0, 0);
			return array;
		}
		return (byte[])obj;
	}

	public static string StorageId(string val)
	{
		string text = "0xa8";
		string text2 = "0x15";
		string text3 = "0x1ce";
		string text4 = "0xce";
		string text5 = "0x11";
		string text6 = "0x198";
		string text7 = "0x90";
		string text8 = "0x1ad";
		string text9 = "0x88";
		string text10 = "0x9d";
		string text11 = "0xfe";
		string text12 = "0x12c";
		string text13 = "0x02c";
		string text14 = "0x13";
		string text15 = "0x02";
		string text16 = "0x45";
		string text17 = "0x70";
		string text18 = "0x79";
		string result = "UNKNOWN";
		string result2 = "SAMSUNG";
		string result3 = "Toshiba";
		string result4 = "SkHynix";
		string result5 = "Foresee";
		string result6 = "ISSI";
		string result7 = "Micron";
		string result8 = "SanDisk";
		string result9 = "Kingston";
		string result10 = "Leahkinn";
		val = val.ToLowerInvariant();
		uint num = ImplementationDetails.method_01(val);
		if (num > 1039818084)
		{
			if (num <= 2389663245u)
			{
				if (num > 2238664674u)
				{
					if (num != 2298286604u)
					{
						if (num == 2389663245u && val == text18)
						{
							goto IL_0172;
						}
					}
					else if (val == text)
					{
						return result;
					}
				}
				else if (num == 2188184722u)
				{
					if (val == text16)
					{
						goto IL_01d2;
					}
				}
				else if (num == 2238664674u && val == text17)
				{
					goto IL_0172;
				}
			}
			else if (num <= 2578205691u)
			{
				if (num == 2434842761u)
				{
					if (val == text8)
					{
						goto IL_0225;
					}
				}
				else if (num == 2578205691u && val == text12)
				{
					goto IL_01e6;
				}
			}
			else if (num != 2619690760u)
			{
				if (num == 3432334069u)
				{
					if (val == text6)
					{
						goto IL_01fa;
					}
				}
				else if (num == 4265147356u && val == text13)
				{
					goto IL_01e6;
				}
			}
			else if (val == text3)
			{
				goto IL_0301;
			}
		}
		else if (num <= 309635847)
		{
			if (num <= 242525371)
			{
				if (num == 25696252)
				{
					if (val == text7)
					{
						goto IL_0225;
					}
				}
				else if (num == 242525371 && val == text2)
				{
					goto IL_0301;
				}
			}
			else if (num == 260730013)
			{
				if (val == text9)
				{
					return result5;
				}
			}
			else if (num == 309635847 && val == text5)
			{
				goto IL_01fa;
			}
		}
		else if (num > 360115799)
		{
			if (num != 763911488)
			{
				if (num == 839031109)
				{
					if (val == text4)
					{
						goto IL_0301;
					}
				}
				else if (num == 1039818084 && val == text11)
				{
					goto IL_01e6;
				}
			}
			else if (val == text10)
			{
				return result6;
			}
		}
		else if (num == 343191085)
		{
			if (val == text14)
			{
				goto IL_01e6;
			}
		}
		else if (num == 360115799 && val == text15)
		{
			goto IL_01d2;
		}
		return result10;
		IL_01e6:
		return result7;
		IL_0301:
		return result2;
		IL_01d2:
		return result8;
		IL_0172:
		return result9;
		IL_0225:
		return result4;
		IL_01fa:
		return result3;
	}

	public static string StorageSCL(string val)
	{
		string text = "00";
		string text2 = "01";
		string text3 = "02";
		string text4 = "03";
		string text5 = "04";
		string text6 = "05";
		string text7 = "06";
		string text8 = "07";
		string text9 = "08";
		string text10 = "09";
		string text11 = "0A";
		string text12 = "0B";
		string result = "0% - 10%";
		string result2 = "10% - 20%";
		string result3 = "20% - 30%";
		string result4 = "30% - 40%";
		string result5 = "40% - 50%";
		string result6 = "50% - 60%";
		string result7 = "60% - 70%";
		string result8 = "70% - 80%";
		string result9 = "80% - 90%";
		string result10 = "90% - 100%";
		string result11 = "Exceeded maximum device life time used!";
		string result12 = "Unknown";
		uint num = ImplementationDetails.method_01(val);
		if (num > 502098945)
		{
			if (num <= 552431802)
			{
				if (num != 518876564)
				{
					if (num != 535654183)
					{
						if (num == 552431802 && val == text2)
						{
							goto IL_020e;
						}
					}
					else if (val == text3)
					{
						return result2;
					}
				}
				else if (val == text4)
				{
					return result3;
				}
			}
			else if (num != 569209421)
			{
				switch (num)
				{
				case 1894641322u:
					if (!(val == text11))
					{
						break;
					}
					return result10;
				case 1877863703u:
					if (!(val == text12))
					{
						break;
					}
					return result11;
				}
			}
			else if (val == text)
			{
				goto IL_020e;
			}
		}
		else if (num > 451766088)
		{
			if (num != 468543707)
			{
				switch (num)
				{
				case 502098945u:
					if (!(val == text5))
					{
						break;
					}
					return result4;
				case 485321326u:
					if (!(val == text6))
					{
						break;
					}
					return result5;
				}
			}
			else if (val == text7)
			{
				return result6;
			}
		}
		else if (num == 418210850)
		{
			if (val == text10)
			{
				return result9;
			}
		}
		else if (num == 434988469)
		{
			if (val == text9)
			{
				return result8;
			}
		}
		else if (num == 451766088 && val == text8)
		{
			return result7;
		}
		return result12;
		IL_020e:
		return result;
	}

	public static string OldStorage(string val)
	{
		string text = "01";
		string text2 = "02";
		string text3 = "03";
		string result = "Normal";
		string result2 = "Urgent! Consumed 80% of reversed block please take backup";
		string result3 = "Dead! Please replace storage to continue";
		string result4 = "Unknown";
		if (!(val == text))
		{
			if (val == text2)
			{
				return result2;
			}
			if (val == text3)
			{
				return result3;
			}
			return result4;
		}
		return result;
	}

	public static string StorageCycle(string val)
	{
		string text = "00";
		string text2 = "01";
		string result = "0% - 10% device life time used";
		string text3 = "02";
		string result2 = "10% - 20% device life time used";
		string text4 = "03";
		string result3 = "20% - 30% device life time used";
		string text5 = "04";
		string result4 = "30% - 40% device life time used";
		string text6 = "05";
		string result5 = "40% - 50% device life time used";
		string text7 = "06";
		string result6 = "50% - 60% device life time used";
		string text8 = "07";
		string result7 = "60% - 70% device life time used";
		string text9 = "08";
		string result8 = "70% - 80% device life time used";
		string text10 = "09";
		string result9 = "80% - 90% device life time used";
		string text11 = "0A";
		string result10 = "90% - 100% device life time used";
		string text12 = "0B";
		string result11 = "Exceeded maximum device life time used!";
		string result12 = "Unknown";
		if (!(val == text) && !(val == text2))
		{
			if (!(val == text3))
			{
				if (!(val == text4))
				{
					if (!(val == text5))
					{
						if (!(val == text6))
						{
							if (val == text7)
							{
								return result6;
							}
							if (val == text8)
							{
								return result7;
							}
							if (val == text9)
							{
								return result8;
							}
							if (val == text10)
							{
								return result9;
							}
							if (!(val == text11))
							{
								if (!(val == text12))
								{
									return result12;
								}
								return result11;
							}
							return result10;
						}
						return result5;
					}
					return result4;
				}
				return result3;
			}
			return result2;
		}
		return result;
	}

	public static string StorageEOL(string val)
	{
		string text = "01";
		string result = "Normal";
		string text2 = "02";
		string result2 = "Urgent! Consumed 80% of reversed block please take backup";
		string text3 = "03";
		string result3 = "Dead! Please replace storage to continue";
		string result4 = "Unknown";
		if (!(val == text))
		{
			if (!(val == text2))
			{
				if (!(val == text3))
				{
					return result4;
				}
				return result3;
			}
			return result2;
		}
		return result;
	}

	static utils()
	{
		string path = "Data";
		BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
