using System;
using System.Numerics;
using System.Security.Cryptography;
using AMFTool.Library.Utils;

namespace AMFTool.Library.Mediatek.Auth;

public class Sla
{
	public static byte[] CustomizedSign(BigInteger n, BigInteger e, byte[] msg)
	{
		int num = CryptoNumber.Size(n);
		int num2 = (int)CryptoNumber.CeilDiv(num, 8L);
		byte[] array = new byte[num2 - msg.Length - 3];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = byte.MaxValue;
		}
		byte[] array2 = new byte[2 + array.Length + 1 + msg.Length];
		array2[0] = 0;
		array2[1] = 1;
		Buffer.BlockCopy(array, 0, array2, 2, array.Length);
		array2[2 + array.Length] = 0;
		Buffer.BlockCopy(msg, 0, array2, 3 + array.Length, msg.Length);
		BigInteger value = CryptoNumber.BytesToLong(array2);
		BigInteger value2 = BigInteger.ModPow(value, e, n);
		return CryptoNumber.LongToBytes(value2, num2);
	}

	public static byte[] GenerateBromSlaChallenge(byte[] data, BigInteger d, BigInteger e)
	{
		for (int i = 0; i < data.Length; i += 2)
		{
			byte b = data[i];
			data[i] = data[i + 1];
			data[i + 1] = b;
		}
		byte[] array = CustomizedSign(d, e, data);
		for (int j = 0; j < array.Length; j += 2)
		{
			byte b2 = array[j];
			array[j] = array[j + 1];
			array[j + 1] = b2;
		}
		return array;
	}

	public static byte[] GenerateDaSlaSignature(byte[] data, RSA rsa)
	{
		return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
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
