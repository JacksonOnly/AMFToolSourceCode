using System;
using System.Globalization;
using System.Numerics;

namespace AMFTool.Library.Utils;

public class CryptoNumber
{
	public static bool IsDecimalString(string text)
	{
		if (!string.IsNullOrWhiteSpace(text))
		{
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] < '0' || text[i] > '9')
				{
					return false;
				}
			}
			if (text.Length > 1 && text[0] == '0')
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public static long CeilDiv(long numerator, long denominator)
	{
		string message = "Values must be non-negative";
		if (denominator == 0L)
		{
			throw new DivideByZeroException();
		}
		if (numerator < 0L || denominator < 0L)
		{
			throw new ArgumentException(message);
		}
		long num = numerator / denominator;
		long num2 = numerator % denominator;
		if (numerator == 0L || (ulong)num2 <= 0uL)
		{
			return num;
		}
		return num + 1L;
	}

	public static byte[] LongToBytes(BigInteger value, int blockSize = 0)
	{
		string message = "Values must be non-negative";
		if (value < 0L || blockSize < 0)
		{
			throw new ArgumentException(message);
		}
		byte[] array = value.ToByteArray();
		int num = array.Length;
		if (array[num - 1] == 0)
		{
			num--;
		}
		byte[] array2 = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = array[num - 1 - i];
		}
		if (blockSize > 0)
		{
			int num2 = ((array2.Length - 1) / blockSize + 1) * blockSize;
			if (array2.Length < num2)
			{
				byte[] array3 = new byte[num2];
				Buffer.BlockCopy(array2, 0, array3, num2 - array2.Length, array2.Length);
				return array3;
			}
		}
		return array2;
	}

	public static BigInteger BytesToLong(byte[] byteArray)
	{
		if (byteArray != null && byteArray.Length != 0)
		{
			byte[] array = new byte[byteArray.Length + 1];
			for (int i = 0; i < byteArray.Length; i++)
			{
				array[i] = byteArray[byteArray.Length - 1 - i];
			}
			array[array.Length - 1] = 0;
			return new BigInteger(array);
		}
		return BigInteger.Zero;
	}

	public static int Size(BigInteger number)
	{
		string message = "Size in bits only available for non-negative numbers";
		if (!(number < 0L))
		{
			if (!number.IsZero)
			{
				byte[] array = number.ToByteArray();
				int num = array.Length - 1;
				int num2 = array[num];
				int num3 = num * 8;
				if (num2 == 0)
				{
					return num3;
				}
				num2 >>= 1;
				num3++;
				int num4 = 316773852;
				while (true)
				{
					_ = num4 ^ 0x4FACE0A6;
					if (num2 == 0)
					{
						break;
					}
					num2 >>= 1;
					num3++;
					num4 = 316773852;
				}
				return num3;
			}
			int num5 = 0;
			return 0;
		}
		throw new ArgumentException(message);
	}

	public static BigInteger ParseBigInteger(string value)
	{
		string message = "Value is empty.";
		string paramName = "value";
		string value2 = "0x";
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentException(message, paramName);
		}
		string text = value.Trim();
		bool flag;
		bool flag2;
		if (!(flag = text.StartsWith(value2, StringComparison.OrdinalIgnoreCase)))
		{
			if (!flag)
			{
				goto IL_0052;
			}
			flag2 = false;
		}
		else
		{
			text = text.Substring(2);
			if (!flag)
			{
				goto IL_0052;
			}
			flag2 = false;
		}
		goto IL_0059;
		IL_0059:
		if (!flag2)
		{
			return BytesToLong(text.HexToBytes());
		}
		return BigInteger.Parse(text, CultureInfo.InvariantCulture);
		IL_0052:
		flag2 = IsDecimalString(text);
		goto IL_0059;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
