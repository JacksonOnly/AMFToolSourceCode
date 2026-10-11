using System;
using System.Collections.Generic;
using AMFTool;

public class hwcrypto
{
	public sej sej;

	public hwcrypto_dxcc.dxcc dxcc;

	public CryptoSetup setup;

	public Func<int, int, List<int>> read32;

	public Func<int, List<int>, bool> write32;

	public Func<int, byte[], bool> writemem;

	public hwcrypto(CryptoSetup setup)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		this.setup = setup;
		read32 = setup.read32;
		write32 = setup.write32;
		writemem = setup.writemem;
		sej = new sej(setup);
		dxcc = new hwcrypto_dxcc.dxcc(setup);
	}

	public byte[] aes_hwcrypt(byte[] data = null, object iv = null, bool encrypt = true, object otp = null, string mode = "cbc", string btype = "sej")
	{
		object obj = null;
		string text = "checking metod : ";
		string text2 = "checking mode : ";
		string text3 = "sej";
		string text4 = "cbc";
		string text5 = "rpmb";
		string text6 = "gcpu";
		string text7 = "dxcc";
		string text8 = "fde";
		string text9 = "rpmb2";
		string text10 = "mirpmb";
		string text11 = "itrustee";
		string text12 = "prov";
		string text13 = "sha256";
		if (data == obj)
		{
			data = new byte[0];
		}
		if (otp == obj)
		{
			otp = new byte[32];
			for (int i = 0; i <= 31; i++)
			{
				((byte[])otp)[i] = 0;
			}
		}
		else if (otp as string !=null)
		{
			otp = utils.HexStringToBytes((string)otp);
		}
		Console.WriteLine(text + btype);
		Console.WriteLine(text2 + mode);
		if (btype == text3)
		{
			if (encrypt)
			{
				if (mode == text4)
				{
					return sej.hw_aes128_cbc_encrypt(data, encrypt: true, (uint[])obj);
				}
			}
			else if (mode == text4)
			{
				return sej.hw_aes128_cbc_encrypt(data, encrypt: false, (uint[])obj);
			}
			if (mode == text5)
			{
				return sej.generate_rpmb(data, (byte[])otp);
			}
		}
		else if (!(btype == text6))
		{
			if (!(btype == text7))
			{
				return new byte[0];
			}
			if (mode == text8)
			{
				return dxcc.generate_rpmb(1);
			}
			if (mode == text9)
			{
				return dxcc.generate_rpmb(2);
			}
			if (mode == text5)
			{
				return dxcc.generate_rpmb();
			}
			if (mode == text10)
			{
				return dxcc.generate_rpmb_mitee();
			}
			if (mode == text11)
			{
				return dxcc.generate_itrustee_fbe();
			}
			if (mode == text12)
			{
				return dxcc.generate_provision_key();
			}
			if (mode == text13)
			{
				return dxcc.generate_sha256(data);
			}
		}
		return (byte[])obj;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
