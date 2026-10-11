using System;
using System.Security.Cryptography;

public class cryptutils
{
	public class aes
	{
		public byte[] aes_cbc(byte[] key, byte[] iv, byte[] data, bool decrypt = true)
		{
			string value = "proces aes";
			string value2 = "ok";
			Console.WriteLine(value);
			using Aes aes = Aes.Create();
			aes.Mode = CipherMode.CBC;
			aes.Key = key;
			aes.IV = iv;
			if (decrypt)
			{
				Console.WriteLine(value2);
				return aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
			}
			Console.WriteLine(value2);
			return aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
		}

		public byte[] encrypt(byte[] data, byte[] key, byte[] iv)
		{
			using Aes aes = Aes.Create();
			aes.Key = key;
			aes.IV = iv;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.None;
			ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);
			return cryptoTransform.TransformFinalBlock(data, 0, data.Length);
		}

		public byte[] decrypt(byte[] data, byte[] key, byte[] iv)
		{
			using Aes aes = Aes.Create();
			aes.Key = key;
			aes.IV = iv;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.None;
			ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
			return cryptoTransform.TransformFinalBlock(data, 0, data.Length);
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
