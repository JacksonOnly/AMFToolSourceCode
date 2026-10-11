using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AMFTool;

public static class Extentions
{
	public static string GetString(this byte[] input)
	{
		try
		{
			return Encoding.UTF8.GetString(input, 0, input.Length);
		}
		catch
		{
			return string.Empty;
		}
	}

	public static string UTF8String(this byte[] buff)
	{
		if (buff == null)
		{
			return string.Empty;
		}
		if (buff.Length == 0)
		{
			return string.Empty;
		}
		int num = Array.IndexOf(buff, (byte)0);
		if (num < 0)
		{
			num = buff.Length;
			return Encoding.UTF8.GetString(buff, 0, num).Trim().ToUpperInvariant();
		}
		return Encoding.UTF8.GetString(buff, 0, num).Trim().ToUpperInvariant();
	}

	public static string ToHexString(this byte[] input)
	{
		string oldValue = "-";
		try
		{
			return BitConverter.ToString(input).Replace(oldValue, string.Empty).ToLower();
		}
		catch
		{
			return string.Empty;
		}
	}

	public static int IndexOf(this byte[] data, byte[] pattern, int index = 0)
	{
		for (int i = index; i < data.Length - pattern.Length + 1; i++)
		{
			bool flag = true;
			for (int j = 0; j < pattern.Length; j++)
			{
				if (data[i + j] != pattern[j])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return i;
			}
		}
		return -1;
	}

	public static int LastIndexOf(this byte[] data, byte[] pattern)
	{
		if (data != null && pattern != null && data.Length >= pattern.Length)
		{
			int num = data.Length - pattern.Length;
			while (num >= 0)
			{
				bool flag = true;
				for (int i = 0; i < pattern.Length; i++)
				{
					if (data[num + i] != pattern[i])
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					num--;
					continue;
				}
				return num;
			}
			return -1;
		}
		return -1;
	}

	public static (bool, byte[]) patch(this byte[] source, byte[] search, byte[] newbytes)
	{
		bool item = false;
		for (int i = 0; i < source.Length - search.Length + 1; i++)
		{
			bool flag = true;
			for (int j = 0; j < search.Length; j++)
			{
				if (source[i + j] != search[j])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				Array.Copy(newbytes, 0, source, i, newbytes.Length);
				item = flag;
			}
		}
		return (item, source);
	}

	public static byte[] HexToBytes(this string hex)
	{
		string oldValue = " ";
		hex = hex.Replace(oldValue, string.Empty);
		byte[] array = new byte[hex.Length / 2];
		for (int i = 0; i < hex.Length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return array;
	}

	public static long ToHex(this string value)
	{
		string oldValue = "0x";
		return long.Parse(value.ToLower().Replace(oldValue, string.Empty), NumberStyles.HexNumber);
	}

	public static byte[] Replace(this byte[] source, int start, int length, byte[] replacement)
	{
		if (start < 0)
		{
			bool flag = true;
			return source;
		}
		if (start < source.Length && length > 0)
		{
			int num = Math.Min(length, source.Length - start);
			List<byte> list = new List<byte>(source.Length - num + replacement.Length);
			list.AddRange(source.Take(start));
			list.AddRange(replacement);
			list.AddRange(source.Skip(start + num));
			return list.ToArray();
		}
		return source;
	}

	public static byte[] Take(this byte[] source, ref int offset, int length)
	{
		byte[] array = new byte[length];
		source.AsSpan(offset, length).CopyTo(array);
		offset += length;
		return array;
	}

	public static byte SwapNibbles(this byte x)
	{
		return (byte)(((x & 0xF) << 4) | ((x & 0xF0) >> 4));
	}

	public static string FixedLength(this string text, int length)
	{
		if (text.Length > length)
		{
			return text.Substring(0, length);
		}
		return text.PadRight(length, 'f');
	}

	public static byte[] Pack(this uint[] values)
	{
		return MemoryMarshal.AsBytes(values.AsSpan()).ToArray();
	}

	public static byte[] Pack(this ulong[] values)
	{
		return MemoryMarshal.AsBytes(values.AsSpan()).ToArray();
	}

	public static ushort[] Unpack16(this byte[] data)
	{
		return MemoryMarshal.Cast<byte, ushort>((ReadOnlySpan<byte>)data).ToArray();
	}

	public static uint[] Unpack32(this byte[] data)
	{
		return MemoryMarshal.Cast<byte, uint>((ReadOnlySpan<byte>)data).ToArray();
	}

	public static ulong[] Unpack64(this byte[] data)
	{
		return MemoryMarshal.Cast<byte, ulong>((ReadOnlySpan<byte>)data).ToArray();
	}

	public static ushort ReadU16LE(this byte[] buffer, ref int offset)
	{
		ushort result = BinaryPrimitives.ReadUInt16LittleEndian(buffer.AsSpan(offset));
		offset += 2;
		return result;
	}

	public static ushort ReadU16BE(this byte[] buffer, ref int offset)
	{
		ushort result = BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan(offset));
		offset += 2;
		return result;
	}

	public static uint ReadU32LE(this byte[] buffer, ref int offset)
	{
		uint result = BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan(offset));
		offset += 4;
		return result;
	}

	public static ulong ReadU64LE(this byte[] buffer, ref int offset)
	{
		ulong result = BinaryPrimitives.ReadUInt64LittleEndian(buffer.AsSpan(offset));
		offset += 8;
		return result;
	}

	public static void WriteU16LE(this byte[] buffer, ref int offset, ushort value)
	{
		BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan(offset), value);
		offset += 2;
	}

	public static void WriteU16BE(this byte[] buffer, ref int offset, ushort value)
	{
		BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(offset), value);
		offset += 2;
	}

	public static void WriteU32LE(this byte[] buffer, ref int offset, uint value)
	{
		BinaryPrimitives.WriteUInt32LittleEndian(buffer.AsSpan(offset), value);
		offset += 4;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
