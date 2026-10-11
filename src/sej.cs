using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMFTool;

public class sej
{
	private bool encrypt = true;

	private int HACC_AES_DEC = 0;

	private int HACC_AES_ENC = 1;

	private int HACC_AES_MODE_MASK = 2;

	private int HACC_AES_ECB = 0;

	private int HACC_AES_CBC = 2;

	private int HACC_AES_TYPE_MASK = 48;

	private int HACC_AES_128 = 0;

	private int HACC_AES_192 = 16;

	private int HACC_AES_256 = 32;

	private int HACC_AES_CHG_BO_MASK = 4096;

	private int HACC_AES_CHG_BO_OFF = 0;

	private int HACC_AES_CHG_BO_ON = 4096;

	private int HACC_AES_START = 1;

	private int HACC_AES_CLR = 2;

	private uint HACC_AES_RDY = 32768u;

	private int HACC_AES_BK2C = 16;

	private int HACC_AES_R2K = 256;

	private uint HACC_SECINIT0_MAGIC = 2919943146u;

	private uint HACC_SECINIT1_MAGIC = 3449122840u;

	private int HACC_SECINIT2_MAGIC = 1177106705;

	private uint[] g_CFG_RANDOM_PATTERN = new uint[12]
	{
		759479152u, 2806305319u, 3500770891u, 2214576708u, 2127046251u, 1139281842u, 27669642u, 543685024u, 1414751237u, 3578177201u,
		482602042u, 3728177795u
	};

	private uint[] g_HACC_CFG_1 = new uint[8] { 2664694784u, 15238305u, 3824190397u, 793669002u, 4286811740u, 3913326819u, 2370694342u, 1174145417u };

	private uint[] g_HACC_CFG_2 = new uint[8] { 2857643226u, 1431445780u, 3824190397u, 1431445780u, 2857643226u, 2857643226u, 1431445780u, 2857643226u };

	private uint[] g_HACC_CFG_3 = new uint[8] { 646231696u, 3949439166u, 2702382156u, 393941525u, 378269286u, 310687364u, 3745238586u, 561999778u };

	private uint[] g_HACC_CFG_MTEE = new uint[4] { 2664694784u, 15238305u, 3824190397u, 793669002u };

	private int sej_base;

	private int[] CFG_1 = new int[8] { -1630272512, 15238305, -470776899, 793669002, -8155556, -381640477, -1924272954, 1174145417 };

	private int[] CFG_RANDOM_PATTERN = new int[12]
	{
		759479152, -1488661977, -794196405, -2080390588, 2127046251, 1139281842, 27669642, 543685024, 1414751237, -716790095,
		482602042, -566789501
	};

	private Func<int, int, List<int>> read32;

	private Func<int, List<int>, bool> write32;

	private HaccReg reg;

	public sej(CryptoSetup setup)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		read32 = setup.read32;
		write32 = setup.write32;
		reg = new HaccReg(setup);
		sej_base = setup.sej_base.Value;
	}

	public byte[] sej_sec_cfg_sw(byte[] data, bool encrypt = true)
	{
		string value = "disini";
		string s = "25A1763A21BC854CD569DC23B4782B63";
		string hex = "57325A5A125497661254976657325A5A";
		Console.WriteLine(value);
		cryptutils.aes aes = new cryptutils.aes();
		return aes.aes_cbc(Encoding.ASCII.GetBytes(s), ToByteArray(hex), data, encrypt);
	}

	public static byte[] ToByteArray(string hex)
	{
		double field_ = Class38.field_202;
		double field_2 = Class38.field_203;
		int length = hex.Length;
		byte[] array = new byte[(int)Math.Round((double)length / field_ - field_2 + field_2)];
		int i = 0;
		for (int num = length - 1; i <= num; i += 2)
		{
			array[(int)Math.Round((double)i / field_)] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return array;
	}

	public byte[] sej_sec_cfg_hw_V3(byte[] data, bool encrypt = true)
	{
		string value = "aes 128";
		object iv = null;
		Console.WriteLine(value);
		return hw_aes128_cbc_encrypt(data, encrypt, (uint[])iv);
	}

	public byte[] xor_data(byte[] data)
	{
		string value = "xor_data";
		string text = "val:";
		string value2 = "valBytes";
		string text2 = "j:";
		Console.WriteLine(value);
		int num = 0;
		int[] cFG_ = CFG_1;
		for (int i = 0; i < cFG_.Length; i++)
		{
			int value3 = cFG_[i];
			Console.WriteLine(text + value3);
			byte[] bytes = BitConverter.GetBytes(value3);
			Console.WriteLine(value2);
			for (int j = 0; j <= 3; j++)
			{
				Console.WriteLine(text2 + j);
				data[num + j] = (byte)(data[num + j] ^ bytes[j]);
			}
			num += 4;
			if (num == 16)
			{
				break;
			}
		}
		Console.WriteLine(utils.BytesToHexString(data));
		return data;
	}

	public byte[] sej_sec_cfg_hw(byte[] data, bool encrypt = true)
	{
		string value = "sej_sec_cfg_hw";
		string value2 = "SEJ_V3_Init";
		string value3 = "sej_run";
		string value4 = "sej_terminate";
		Console.WriteLine(value);
		byte[] array;
		if (!encrypt)
		{
			Console.WriteLine(value2);
			SEJ_V3_Init(encrypt, g_HACC_CFG_1, legacy: true);
			Console.WriteLine(value3);
			array = sej_run(data);
			Console.WriteLine(value4);
			sej_terminate();
			if (!encrypt)
			{
				return xor_data(array);
			}
			return array;
		}
		data = xor_data(data);
		Console.WriteLine(value2);
		SEJ_V3_Init(encrypt, g_HACC_CFG_1, legacy: true);
		Console.WriteLine(value3);
		array = sej_run(data);
		Console.WriteLine(value4);
		sej_terminate();
		if (!encrypt)
		{
			return xor_data(array);
		}
		return array;
	}

	public byte[] SecCfgHwV3(byte[] data, bool encrypt = true)
	{
		object iv = null;
		return hw_aes128_cbc_encrypt(data, encrypt, (uint[])iv);
	}

	private int[] bytes_to_dwords(byte[] buf)
	{
		double field_ = Class38.field_213;
		double field_2 = Class38.field_203;
		List<int> list = new List<int>();
		int i = 0;
		for (int num = (int)Math.Round((double)buf.Length / field_ - field_2); i <= num; i++)
		{
			list.Add(BitConverter.ToInt32(buf, i * 4));
		}
		return list.ToArray();
	}

	public void tz_pre_init()
	{
	}

	public byte[] generate_rpmb(byte[] meid, byte[] otp, int derivedlen = 32)
	{
		sej_set_otp(otp);
		byte[] array = new byte[0];
		int i = 0;
		for (int num = derivedlen - 1; i <= num; i++)
		{
			array = array.Concat(new byte[1] { meid[i % meid.Length] }).ToArray();
		}
		return hw_aes128_cbc_encrypt(array, encrypt: true, g_HACC_CFG_1);
	}

	public void sej_set_otp(byte[] data)
	{
		string key = "HACC_SW_OTP0";
		string key2 = "HACC_SW_OTP1";
		string key3 = "HACC_SW_OTP2";
		string key4 = "HACC_SW_OTP3";
		string key5 = "HACC_SW_OTP4";
		string key6 = "HACC_SW_OTP5";
		string key7 = "HACC_SW_OTP6";
		string key8 = "HACC_SW_OTP7";
		int[] array = bytes_to_dwords(data);
		reg[key] = array[0];
		reg[key2] = array[1];
		reg[key3] = array[2];
		reg[key4] = array[3];
		reg[key5] = array[4];
		reg[key6] = array[5];
		reg[key7] = array[6];
		reg[key8] = array[7];
	}

	public int SEJ_V3_Init(bool ben = true, uint[] iv = null, bool legacy = false)
	{
		object obj = null;
		string text = "acon_setting:";
		string key = "HACC_AKEY0";
		string key2 = "HACC_AKEY1";
		string key3 = "HACC_AKEY2";
		string key4 = "HACC_AKEY3";
		string key5 = "HACC_AKEY4";
		string key6 = "HACC_AKEY5";
		string key7 = "HACC_AKEY6";
		string key8 = "HACC_AKEY7";
		string key9 = "HACC_ACON";
		string key10 = "HACC_ACONK";
		string key11 = "HACC_ACON2";
		string key12 = "HACC_ACFG0";
		string key13 = "HACC_ACFG1";
		string key14 = "HACC_ACFG2";
		string key15 = "HACC_ACFG3";
		string text2 = "legacy:";
		string key16 = "HACC_UNK";
		string text3 = "ss:";
		string text4 = "mm:";
		string text5 = "l:";
		string value = "SEJ_V3_Init results may be wrong";
		string key17 = "HACC_ASRC0";
		string key18 = "HACC_ASRC1";
		string key19 = "HACC_ASRC2";
		string key20 = "HACC_ASRC3";
		string value2 = "SEJ_V3_Init results may be wrong.";
		int num = HACC_AES_CHG_BO_OFF | HACC_AES_128;
		if (iv !=null)
		{
			num |= HACC_AES_CBC;
		}
		num = ((!ben) ? (num | HACC_AES_DEC) : (num | HACC_AES_ENC));
		Console.WriteLine(text + num);
		reg[key] = 0;
		reg[key2] = 0;
		reg[key3] = 0;
		reg[key4] = 0;
		reg[key5] = 0;
		reg[key6] = 0;
		reg[key7] = 0;
		reg[key8] = 0;
		reg[key9] = HACC_AES_CHG_BO_OFF | HACC_AES_CBC | HACC_AES_128 | HACC_AES_DEC;
		reg[key10] = HACC_AES_BK2C | HACC_AES_R2K;
		reg[key11] = HACC_AES_CLR;
		reg[key12] = -1630272512;
		reg[key13] = 15238305;
		reg[key14] = -470776899;
		reg[key15] = 793669002;
		Console.WriteLine(text2 + legacy);
		if (legacy)
		{
			int num2 = reg[key16];
			Console.WriteLine(text3 + num2);
			reg[key16] = num2 | 2;
			reg[key11] = 0x40000000 | HACC_AES_CLR;
			int i;
			for (i = 0; i < 20; i++)
			{
				int num3 = reg[key11];
				Console.WriteLine(text4 + num3);
				Console.WriteLine(text5 + i);
				if (num3 > 2147483648L)
				{
					break;
				}
			}
			if (i == 20)
			{
				Console.WriteLine(value);
			}
			num2 = reg[key16];
			Console.WriteLine(text3 + num2);
			reg[key16] = (int)(num2 & 0xFFFFFFFEL);
			reg[key10] = HACC_AES_BK2C;
			reg[key9] = num;
		}
		else
		{
			reg[key16] = 1;
			for (int j = 0; j <= 2; j++)
			{
				int num4 = j * 4;
				reg[key17] = CFG_RANDOM_PATTERN[num4];
				reg[key18] = CFG_RANDOM_PATTERN[num4 + 1];
				reg[key19] = CFG_RANDOM_PATTERN[num4 + 2];
				reg[key20] = CFG_RANDOM_PATTERN[num4 + 3];
				reg[key11] = HACC_AES_START;
				int k;
				for (k = 0; k < 20; k++)
				{
					if ((ulong)(reg[key11] & HACC_AES_RDY) > 0uL)
					{
						break;
					}
				}
				if (k == 20)
				{
					Console.WriteLine(value2);
					break;
				}
			}
			reg[key11] = HACC_AES_CLR;
			reg[key12] = -1630272512;
			reg[key13] = 15238305;
			reg[key14] = -470776899;
			reg[key15] = 793669002;
			reg[key9] = num;
			reg[key10] = 0;
		}
		return num;
	}

	public byte[] sej_run(byte[] data)
	{
		double field_ = Class38.field_213;
		double field_2 = Class38.field_203;
		string key = "HACC_ASRC0";
		string key2 = "HACC_ASRC1";
		string key3 = "HACC_ASRC2";
		string key4 = "HACC_ASRC3";
		string key5 = "HACC_ACON2";
		string value = "SEJ Hardware seems not to be configured correctly. Results may be wrong.";
		string key6 = "HACC_AOUT0";
		string key7 = "HACC_AOUT1";
		string key8 = "HACC_AOUT2";
		string key9 = "HACC_AOUT3";
		List<byte> list = new List<byte>();
		int[] array = bytes_to_dwords(data);
		int num = array.Length;
		int num2 = 0;
		int i = 0;
		for (int num3 = (int)Math.Round((double)num / field_ - field_2); i <= num3; i++)
		{
			reg[key] = array[num2];
			reg[key2] = array[num2 + 1];
			reg[key3] = array[num2 + 2];
			reg[key4] = array[num2 + 3];
			reg[key5] = HACC_AES_START;
			int j;
			for (j = 0; j < 20; j++)
			{
				if ((ulong)(reg[key5] & HACC_AES_RDY) > 0uL)
				{
					break;
				}
			}
			if (j == 20)
			{
				Console.WriteLine(value);
			}
			list.AddRange(BitConverter.GetBytes(reg[key6]));
			list.AddRange(BitConverter.GetBytes(reg[key7]));
			list.AddRange(BitConverter.GetBytes(reg[key8]));
			list.AddRange(BitConverter.GetBytes(reg[key9]));
			num2 += 4;
		}
		return list.ToArray();
	}

	public void sej_terminate()
	{
		string key = "HACC_ACON2";
		string key2 = "HACC_AKEY0";
		string key3 = "HACC_AKEY1";
		string key4 = "HACC_AKEY2";
		string key5 = "HACC_AKEY3";
		string key6 = "HACC_AKEY4";
		string key7 = "HACC_AKEY5";
		string key8 = "HACC_AKEY6";
		string key9 = "HACC_AKEY7";
		reg[key] = HACC_AES_CLR;
		reg[key2] = 0;
		reg[key3] = 0;
		reg[key4] = 0;
		reg[key5] = 0;
		reg[key6] = 0;
		reg[key7] = 0;
		reg[key8] = 0;
		reg[key9] = 0;
	}

	public byte[] hw_aes128_cbc_encrypt(byte[] buf, bool encrypt = true, uint[] iv = null)
	{
		object obj = null;
		byte[] result;
		if (iv != obj)
		{
			SEJ_V3_Init(encrypt, iv);
			result = sej_run(buf);
			sej_terminate();
			return result;
		}
		iv = g_HACC_CFG_1;
		SEJ_V3_Init(encrypt, iv);
		result = sej_run(buf);
		sej_terminate();
		return result;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
