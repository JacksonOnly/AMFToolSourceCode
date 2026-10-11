using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AMFTool;
using Microsoft.VisualBasic.CompilerServices;

public class hwcrypto_dxcc
{
	public class dxcc_reg
	{
		private long sej_base;

		private Func<int, int, List<int>> read32;

		private Func<int, List<int>, bool> write32;

		private Func<int, byte[], bool> writemem;

		public dxcc_reg(CryptoSetup setup)
		{
			sej_base = (uint)setup.dxcc_base.Value;
			read32 = setup.read32;
			write32 = setup.write32;
			writemem = setup.writemem;
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class dxcc
	{
		public const int DX_HOST_IRR = 2560;

		public const int DX_HOST_ICR = 2568;

		public const int DX_DSCRPTR_QUEUE0_WORD0 = 3712;

		public const int DX_DSCRPTR_QUEUE0_WORD1 = 3716;

		public const int DX_DSCRPTR_QUEUE0_WORD2 = 3720;

		public const int DX_DSCRPTR_QUEUE0_WORD3 = 3724;

		public const int DX_DSCRPTR_QUEUE0_WORD4 = 3728;

		public const int DX_DSCRPTR_QUEUE0_WORD5 = 3732;

		public const int DX_DSCRPTR_QUEUE0_CONTENT = 3740;

		public const int DX_HOST_SEP_HOST_GPR0 = 2688;

		public const int DX_HOST_SEP_HOST_GPR1 = 2696;

		public const int DX_HOST_SEP_HOST_GPR2 = 2704;

		public const int DX_HOST_SEP_HOST_GPR3 = 2716;

		public const int DX_HOST_SEP_HOST_GPR4 = 2720;

		public string oem_pubk = "DACD8B5FDA8A766FB7BCAA43F0B16915CE7B47714F1395FDEBCF12A2D41155B0FB587A51FECCCB4DDA1C8E5EB9EB69B86DAF2C620F6C2735215A5F22C0B6CE377AA0D07EB38ED340B5629FC2890494B078A63D6D07FDEACDBE3E7F27FDE4B143F49DB4971437E6D00D9E18B56F02DABEB0000B6E79516D0C8074B5A42569FD0D9196655D2A4030D42DFE05E9F64883E6D5F79A5BFA3E7014C9A62853DC1F21D5D626F4D0846DB16452187DD776E8886B48C210C9E208059E7CAFC997FD2CA210775C1A5D9AA261252FB975268D970C62733871D57814098A453DF92BC6CA19025CD9D430F02EE46F80DE6C63EA802BEF90673AAC4C6667F2883FB4501FA77455";

		private uint hwcode;

		private long dxcc_base;

		private long sej_base;

		private Func<int, int, List<int>> read32;

		private Func<int, List<int>, bool> write32;

		private Func<int, byte[], bool> writemem;

		private long da_payload_addr;

		public dxcc_reg reg;

		public dxcc(CryptoSetup setup)
		{
			hwcode = (uint)setup.hwcode.Value;
			dxcc_base = (uint)setup.dxcc_base.Value;
			read32 = setup.read32;
			write32 = setup.write32;
			writemem = setup.writemem;
			da_payload_addr = (uint)setup.da_payload_addr.Value;
			reg = new dxcc_reg(setup);
		}

		public bool SB_HalClearInterruptBit()
		{
			return write32((int)(dxcc_base + 2568L), new List<int> { 4 });
		}

		public int SB_CryptoWait()
		{
			List<int> list;
			do
			{
				list = read32((int)(dxcc_base + 2560L), 1);
			}
			while (Convert.ToInt32(list[0]) == 0);
			return list[0];
		}

		public void sasi_paldmaunmap(object value1)
		{
		}

		public object sasi_paldmamap(object value1)
		{
			return value1;
		}

		public void sasi_sb_adddescsequence(int[] data)
		{
			string value = "sasi_sb_adddescsequence:waiting";
			string text = "reading : ";
			string text2 = "write 1 :";
			string text3 = "write 2 :";
			string text4 = "write 3 :";
			string text5 = "write 4 :";
			string text6 = "write 5 :";
			string text7 = "write 6 :";
			Console.WriteLine(value);
			List<int> list = read32((int)(dxcc_base + 3740L), 1);
			Console.WriteLine(text + list[0]);
			bool flag = (ulong)(Convert.ToInt64(list[0]) << 28) > 0uL;
			int num = 1987161408;
			while (true)
			{
				_ = num ^ 0x506A5943;
				if (flag)
				{
					break;
				}
				list = read32((int)(dxcc_base + 3740L), 1);
				Console.WriteLine(text + list[0]);
				flag = (ulong)(Convert.ToInt64(list[0]) << 28) > 0uL;
				num = 1987161408;
			}
			Console.WriteLine(text2 + (dxcc_base + 3712L), data[0]);
			write32((int)(dxcc_base + 3712L), new List<int> { data[0] });
			Console.WriteLine(text3 + (dxcc_base + 3712L), data[1]);
			write32((int)(dxcc_base + 3716L), new List<int> { data[1] });
			Console.WriteLine(text4 + (dxcc_base + 3712L), data[2]);
			write32((int)(dxcc_base + 3720L), new List<int> { data[2] });
			Console.WriteLine(text5 + (dxcc_base + 3712L), data[3]);
			write32((int)(dxcc_base + 3724L), new List<int> { data[3] });
			Console.WriteLine(text6 + (dxcc_base + 3712L), data[4]);
			write32((int)(dxcc_base + 3728L), new List<int> { data[4] });
			Console.WriteLine(text7 + (dxcc_base + 3712L), data[5]);
			write32((int)(dxcc_base + 3732L), new List<int> { data[5] });
		}

		private int tzcc_clk(bool value)
		{
			if (value)
			{
				return Conversions.ToInteger(write32(268439692, new List<int> { 402653184 }));
			}
			return Conversions.ToInteger(write32(268439688, new List<int> { 134217728 }));
		}

		public byte[] generate_itrustee_fbe(int key_sz = 32)
		{
			int num = 0;
			int num2 = 1;
			int count = 16;
			string text = "TrustedCorekeymaster";
			int c = 7;
			int arg = 4;
			byte[] array = new byte[0];
			int destaddr = (int)(da_payload_addr - 768L);
			tzcc_clk(value: true);
			int i = 0;
			for (int num3 = key_sz / 16 - 1; ((i > num3) ? 1 : 0) == num; i += num2)
			{
				byte[] bytes = Encoding.ASCII.GetBytes(text + new string((char)c, count));
				byte[] array2 = bytes.Concat(BitConverter.GetBytes((byte)i)).ToArray();
				int arg2 = SBROM_AesCmac(num2, num, array2, num, array2.Length, destaddr);
				foreach (int item in read32(arg2, arg))
				{
					array = array.Concat(BitConverter.GetBytes(item)).ToArray();
				}
			}
			tzcc_clk((byte)num != 0);
			return array;
		}

		public byte[] generate_rpmb(int level = 0)
		{
			string s = "RPMB KEY";
			string s2 = "SASI";
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			byte[] bytes2 = Encoding.ASCII.GetBytes(s2);
			int i = 0;
			for (int num = bytes.Length - 1; i <= num; i++)
			{
				bytes[i] = (byte)(bytes[i] + level);
			}
			int j = 0;
			for (int num2 = bytes2.Length - 1; j <= num2; j++)
			{
				bytes2[j] = (byte)(bytes2[j] + level);
			}
			int requestedlen = 32;
			if (level > 0)
			{
				requestedlen = 16;
			}
			tzcc_clk(value: true);
			int destaddr = (int)(da_payload_addr - 768L);
			byte[] result = SBROM_KeyDerivation(1, bytes, bytes2, requestedlen, destaddr);
			tzcc_clk(value: false);
			return result;
		}

		public byte[] generate_rpmb_mitee()
		{
			string hex = "AD1AC6B4BDF4EDB7";
			string hex2 = "69EF6584";
			string text = "rpmb_ikey:";
			string text2 = "rpmb_salt:";
			string text3 = "keylength:";
			string value = "write clk... ";
			string text4 = "da payload:";
			string value2 = "SBROM_KeyDerivation... ";
			string text5 = "rpmbkey:";
			byte[] array = utils.StringToByteArray(hex);
			byte[] array2 = utils.StringToByteArray(hex2);
			int requestedlen = 16;
			Console.WriteLine(text + utils.BytesToHexString(array));
			Console.WriteLine(text2 + utils.BytesToHexString(array2));
			Console.WriteLine(text3 + requestedlen);
			Console.WriteLine(value);
			tzcc_clk(value: true);
			int destaddr = (int)(da_payload_addr - 768L);
			Console.WriteLine(text4 + (da_payload_addr - 768L));
			Console.WriteLine(value2);
			byte[] array3 = SBROM_KeyDerivation(1, array, array2, requestedlen, destaddr);
			Console.WriteLine(value);
			Console.WriteLine(text5 + utils.BytesToHextring(array3));
			tzcc_clk(value: false);
			return array3;
		}

		private int salt_func(int value)
		{
			for (int num = read32((int)(dxcc_base + 2748L), 1)[0] & 1; num == 0; num = read32((int)(dxcc_base + 2748L), 1)[0] & 1)
			{
			}
			write32((int)(dxcc_base + 2724L), new List<int> { (4 * value) | 0x10000 });
			for (int num = read32((int)(dxcc_base + 2740L), 1)[0] << 31; num == 0; num = read32((int)(dxcc_base + 2740L), 1)[0] << 31)
			{
			}
			return read32((int)(dxcc_base + 2732L), 1)[0];
		}

		public byte[] generate_provision_key()
		{
			string s = "KEY PLAT";
			string s2 = "PROVISION KEY";
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			byte[] bytes2 = Encoding.ASCII.GetBytes(s2);
			tzcc_clk(value: true);
			int destaddr = (int)(da_payload_addr - 768L);
			byte[] salt = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(oem_pubk));
			byte[] result = SBROM_KeyDerivation(2, bytes, salt, 16, destaddr);
			for (int num = read32((int)(dxcc_base + 2804L), 1)[0] & 1; num == 0; num = read32((int)(dxcc_base + 2804L), 1)[0] & 1)
			{
			}
			SBROM_KeyDerivation(5, bytes2, salt, 16, destaddr);
			write32((int)(dxcc_base + 2752L), new List<int> { 0 });
			write32((int)(dxcc_base + 2756L), new List<int> { 0 });
			write32((int)(dxcc_base + 2760L), new List<int> { 0 });
			write32((int)(dxcc_base + 2764L), new List<int> { 0 });
			int[] array = hw_desc_init();
			array[0] = 0;
			array[1] = 134217857;
			array[2] = 0;
			array[3] = 0;
			array[4] = 75504672;
			array[5] = 0;
			sasi_sb_adddescsequence(array);
			destaddr = (int)(da_payload_addr - 768L);
			SB_HalWaitDescCompletion();
			tzcc_clk(value: false);
			return result;
		}

		public byte[] generate_sha256(byte[] data)
		{
			int num = (int)(da_payload_addr - 768L);
			sbrom_sha256(data, num);
			List<byte> list = new List<byte>();
			foreach (int item in read32(num, 8))
			{
				list.AddRange(BitConverter.GetBytes(item));
			}
			return list.ToArray();
		}

		public byte[] SBROM_KeyDerivation(int aeskeytype, byte[] label, byte[] salt, int requestedlen, int destaddr)
		{
			int num = 1;
			int arg = 4;
			int num2 = 0;
			int num3 = 255;
			int num4 = 8;
			List<byte> list = new List<byte>();
			if (aeskeytype - 1 <= 4 && ((1 << aeskeytype - 1) & 0x17) != 0)
			{
				if (requestedlen <= 255 && !Conversions.ToBoolean(requestedlen << 28))
				{
					if (label.Length != 0 && label.Length <= 32)
					{
						int num5 = salt.Length + 3 + label.Length;
						int num6 = requestedlen + 15 >> 4;
						int i = 0;
						for (int num7 = num6 - 1; ((i > num7) ? 1 : 0) == num2; i += num)
						{
							byte[] array = new byte[num];
							array[num2] = (byte)(i + num);
							IEnumerable<byte> first = array.Concat(label).Concat(new byte[num]).Concat(salt);
							byte[] array2 = new byte[num];
							array2[num2] = (byte)((num4 * requestedlen) & num3);
							byte[] source = first.Concat(array2).ToArray();
							int num8 = SBROM_AesCmac(aeskeytype, num2, source.Take(num5).ToArray(), num2, num5, destaddr);
							if ((uint)num8 <= (uint)num2)
							{
								continue;
							}
							foreach (int item in read32(num8, arg))
							{
								list.AddRange(BitConverter.GetBytes(item));
							}
						}
						return list.ToArray();
					}
					return BitConverter.GetBytes(-234881021);
				}
				return BitConverter.GetBytes(-234881021);
			}
			return BitConverter.GetBytes(-234881022);
		}

		private int SBROM_AesCmac(int aesKeyType, int InternalKey, byte[] DataIn, int dmaMode, int bufferlen, int destaddr)
		{
			int num = destaddr + 16;
			int num2 = DataIn.Length / 32 * 32;
			int num3 = num + num2;
			int num4 = num3 + num2;
			int num5 = num4;
			int num6;
			if (InternalKey == 0)
			{
				if (dmaMode == 0)
				{
					writemem(num, DataIn.Take(bufferlen).ToArray());
				}
				else
				{
					dmaMode = dmaMode;
					writemem(num, DataIn.Take(bufferlen).ToArray());
				}
				if (SBROM_AesCmacDriver(aesKeyType, num5, num, dmaMode, bufferlen, destaddr))
				{
					return num5;
				}
				num6 = 0;
				return 0;
			}
			writemem(num, DataIn.Take(bufferlen).ToArray());
			if (dmaMode == 0)
			{
				writemem(num, DataIn.Take(bufferlen).ToArray());
			}
			else
			{
				dmaMode = dmaMode;
				writemem(num, DataIn.Take(bufferlen).ToArray());
			}
			if (SBROM_AesCmacDriver(aesKeyType, num5, num, dmaMode, bufferlen, destaddr))
			{
				return num5;
			}
			num6 = 0;
			return 0;
		}

		public bool SB_HalInit()
		{
			return SB_HalClearInterruptBit();
		}

		public int SB_HalWaitDescCompletion(int destptr = 0)
		{
			List<int> list = new List<int>();
			SB_HalClearInterruptBit();
			int num = Conversions.ToInteger(sasi_paldmamap(0));
			list.Add(0);
			list.Add(134217745);
			list.Add(destptr);
			list.Add(134217746);
			list.Add(256);
			list.Add(destptr << 16);
			sasi_sb_adddescsequence(list.ToArray());
			while ((SB_CryptoWait() & 4) == 0)
			{
			}
			int num2 = read32((int)(dxcc_base + 2976L), 1)[0];
			bool flag = num2 != 0;
			int num3 = 1830589903;
			while (true)
			{
				_ = num3 ^ -1610875139;
				if (flag)
				{
					break;
				}
				flag = num2 != 0;
				num3 = 1830589903;
			}
			if (num2 != 1)
			{
				return -167772159;
			}
			SB_HalClearInterruptBit();
			sasi_paldmaunmap(num);
			return 0;
		}

		public bool SBROM_AesCmacDriver(object aesKeyType, long pInternalKey, long pDataIn, int dmaMod, int blockSize, long pCMacResult)
		{
			int keySize = ((!Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(aesKeyType, 1, TextCompare: false))) ? 16 : (((read32((int)(dxcc_base + 2720L), 1)[0] & 2) != 0) ? 32 : 16));
			SB_HalInit();
			int[] pDesc = hw_desc_init();
			pDesc = hw_desc_set_cipher_mode(pDesc, 7u);
			pDesc = hw_desc_set_cipher_config0(pDesc, 0u);
			pDesc = hw_desc_set_key_size_aes(pDesc, (uint)keySize);
			pDesc = hw_desc_set_din_sram(pDesc, 0u, 16u);
			pDesc = hw_desc_set_din_const(pDesc, 0u, 16u);
			pDesc = hw_desc_set_flow_mode(pDesc, 32u);
			pDesc = hw_desc_set_setup_mode(pDesc, 1u);
			pDesc[1] |= 134217728;
			sasi_sb_adddescsequence(pDesc);
			int[] pDesc2 = hw_desc_init();
			if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(aesKeyType, 0, TextCompare: false)))
			{
				int dinAdr = (int)pInternalKey;
				pDesc2 = hw_desc_set_din_sram(pDesc2, (uint)dinAdr, 16u);
			}
			pDesc2 = hw_desc_set_cipher_do(pDesc2, Conversions.ToUInteger(aesKeyType));
			pDesc2 = hw_desc_set_cipher_mode(pDesc2, 7u);
			pDesc2 = hw_desc_set_cipher_config0(pDesc2, 0u);
			pDesc2 = hw_desc_set_key_size_aes(pDesc2, (uint)keySize);
			pDesc2 = hw_desc_set_flow_mode(pDesc2, 32u);
			pDesc2 = hw_desc_set_setup_mode(pDesc2, 4u);
			pDesc2[4] = Conversions.ToInteger(Operators.OrObject(pDesc2[4], Operators.LeftShiftObject(Operators.AndObject(Operators.RightShiftObject(aesKeyType, 2), 3), 20)));
			sasi_sb_adddescsequence(pDesc2);
			int[] pDesc3 = hw_desc_init();
			pDesc3 = ((dmaMod != 1) ? hw_desc_set_din_type(pDesc3, 2u, (ulong)pDataIn, (uint)blockSize, 0u, 0u) : hw_desc_set_din_sram(pDesc3, (uint)pDataIn, (uint)blockSize));
			pDesc3 = hw_desc_set_flow_mode(pDesc3, 1u);
			sasi_sb_adddescsequence(pDesc3);
			if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(aesKeyType, 2, TextCompare: false)))
			{
				int[] pDesc4 = hw_desc_init();
				pDesc4 = hw_desc_set_cipher_mode(pDesc4, 7u);
				pDesc4 = hw_desc_set_cipher_config0(pDesc4, 0u);
				pDesc4 = hw_desc_set_setup_mode(pDesc4, 8u);
				pDesc4 = hw_desc_set_flow_mode(pDesc4, 38u);
				pDesc4 = ((dmaMod == 1) ? hw_desc_set_dout_sram(pDesc4, (ulong)pInternalKey, 16u) : hw_desc_set_dout_dlli(pDesc4, (ulong)pInternalKey, 16u, 0u, 0u));
				pDesc4 = hw_desc_set_din_nodma(pDesc4, 0u, 0u);
				sasi_sb_adddescsequence(pDesc4);
			}
			return SB_HalWaitDescCompletion((int)pCMacResult) == 0;
		}

		public byte[] mtee_decrypt(byte[] data)
		{
			string s = "B936C14D95A99585073E5607784A51F7444B60D6BFD6110F76D004CCB7E1950E";
			byte[] buffer = utils.HexStringToBytes(s);
			using SHA256 sHA = SHA256.Create();
			byte[] source = sHA.ComputeHash(buffer);
			using Aes aes = Aes.Create();
			aes.Key = source.Take(16).ToArray();
			aes.IV = source.Skip(16).ToArray();
			aes.Mode = CipherMode.CBC;
			using ICryptoTransform cryptoTransform = aes.CreateDecryptor();
			return cryptoTransform.TransformFinalBlock(data, 0, data.Length);
		}

		public byte[] descramble(byte[] data)
		{
			string s = "5C0E349A27DC46034C7B6744A378BD17";
			string s2 = "A0B0924686447109F2D51DCDDC93458A";
			byte[] key = utils.HexStringToBytes(s);
			byte[] iV = utils.HexStringToBytes(s2);
			using Aes aes = Aes.Create();
			aes.Key = key;
			aes.IV = iV;
			aes.Mode = CipherMode.ECB;
			using ICryptoTransform cryptoTransform = aes.CreateDecryptor();
			return cryptoTransform.TransformFinalBlock(data, 0, data.Length);
		}

		public int sbrom_sha256(byte[] buffer, long destaddr)
		{
			string s = "19CDE05BABD9831F8C68059B7F520E513AF54FA572F36E3C85AE67BB67E6096A";
			long num = destaddr + 64L;
			long num2 = destaddr + 32L;
			writemem(268439692, BitConverter.GetBytes(402653184));
			byte[] arg = utils.HexStringToBytes(s);
			writemem((int)num2, arg);
			writemem((int)num, buffer);
			sbrom_cryptoinitdriver(num2, 0);
			sbrom_cryptoupdate(num, destaddr, buffer.Length, 1, 0, 3);
			sbrom_cryptofinishdriver(destaddr);
			writemem(268439688, BitConverter.GetBytes(134217728));
			return 0;
		}

		public void sbrom_cryptoinitdriver(long aesivptr, int cryptodrivermode)
		{
			long field_ = Class38.field_120;
			if ((cryptodrivermode & field_) == 0L)
			{
				int[] pDesc = hw_desc_init();
				pDesc = hw_desc_set_din_type(pDesc, 2u, (ulong)aesivptr, 32u, 0u, 0u);
				pDesc = hw_desc_set_flow_mode(pDesc, 37u);
				pDesc = hw_desc_set_cipher_mode(pDesc, 2u);
				pDesc = hw_desc_set_setup_mode(pDesc, 1u);
				sasi_sb_adddescsequence(pDesc);
				int[] pDesc2 = hw_desc_init();
				pDesc2 = hw_desc_set_flow_mode(pDesc2, 37u);
				pDesc2 = hw_desc_set_cipher_mode(pDesc2, 2u);
				pDesc2 = hw_desc_set_setup_mode(pDesc2, 4u);
				pDesc2 = hw_desc_set_din_const(pDesc2, 0u, 16u);
				sasi_sb_adddescsequence(pDesc2);
			}
		}

		public object sbrom_cryptoupdate(long inputptr, long outputptr, int blockSize, int islastblock, int cryptodrivermode, int waitforcrypto)
		{
			long field_ = Class38.field_120;
			object result = null;
			if (waitforcrypto != 2 || SB_HalWaitDescCompletion((int)dxcc_base) == 0)
			{
				if (islastblock == 1 && (cryptodrivermode & field_) == 0L)
				{
					int[] pDesc = hw_desc_init();
					pDesc = hw_desc_set_dout_dlli(pDesc, (ulong)outputptr, 16u, 0u, 0u);
					pDesc = hw_desc_set_flow_mode(pDesc, 43u);
					pDesc = hw_desc_set_cipher_mode(pDesc, 2u);
					pDesc = hw_desc_set_cipher_config1(pDesc, 1u);
					pDesc = hw_desc_set_setup_mode(pDesc, 9u);
					sasi_sb_adddescsequence(pDesc);
				}
				int[] pDesc2 = hw_desc_init();
				pDesc2 = hw_desc_set_din_type(pDesc2, 2u, (ulong)inputptr, (uint)blockSize, 0u, 0u);
				if (cryptodrivermode == 0)
				{
					pDesc2 = hw_desc_set_flow_mode(pDesc2, 7u);
				}
				sasi_sb_adddescsequence(pDesc2);
				if ((waitforcrypto != 2 || islastblock != 0) && waitforcrypto != 3)
				{
					if (waitforcrypto == 0)
					{
						return 0;
					}
					return -234881023;
				}
				SB_HalWaitDescCompletion((int)dxcc_base);
			}
			return result;
		}

		public object sbrom_cryptofinishdriver(long outputptr)
		{
			int[] pDesc = hw_desc_init();
			pDesc = hw_desc_set_dout_dlli(pDesc, (ulong)outputptr, 16u, 0u, 0u);
			pDesc = hw_desc_set_flow_mode(pDesc, 43u);
			pDesc = hw_desc_set_cipher_mode(pDesc, 2u);
			pDesc = hw_desc_set_cipher_config0(pDesc, 2u);
			pDesc = hw_desc_set_cipher_config1(pDesc, 1u);
			pDesc = hw_desc_set_setup_mode(pDesc, 8u);
			sasi_sb_adddescsequence(pDesc);
			return SB_HalWaitDescCompletion((int)dxcc_base);
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class DescDirection
	{
		public const uint DESC_DIRECTION_ILLEGAL = uint.MaxValue;

		public const int DESC_DIRECTION_ENCRYPT_ENCRYPT = 0;

		public const int DESC_DIRECTION_DECRYPT_DECRYPT = 1;

		public const int DESC_DIRECTION_DECRYPT_ENCRYPT = 3;

		public const int DESC_DIRECTION_END = int.MaxValue;
	}

	public class sep_engine_type
	{
		public const int SEP_ENGINE_NULL = 0;

		public const int SEP_ENGINE_AES = 1;

		public const int SEP_ENGINE_DES = 2;

		public const int SEP_ENGINE_HASH = 3;

		public const int SEP_ENGINE_RC4 = 4;

		public const int SEP_ENGINE_DOUT = 5;
	}

	public class sep_crypto_alg
	{
		public const uint SEP_CRYPTO_ALG_NULL = uint.MaxValue;

		public const int SEP_CRYPTO_ALG_AES = 0;

		public const int SEP_CRYPTO_ALG_DES = 1;

		public const int SEP_CRYPTO_ALG_HASH = 2;

		public const int SEP_CRYPTO_ALG_RC4 = 3;

		public const int SEP_CRYPTO_ALG_C2 = 4;

		public const int SEP_CRYPTO_ALG_HMAC = 5;

		public const int SEP_CRYPTO_ALG_AEAD = 6;

		public const int SEP_CRYPTO_ALG_BYPASS = 7;

		public const int SEP_CRYPTO_ALG_COMBINED = 8;

		public const int SEP_CRYPTO_ALG_NUM = 9;

		public const int SEP_CRYPTO_ALG_RESERVE32B = int.MaxValue;
	}

	public class sep_crypto_direction
	{
		public const uint SEP_CRYPTO_DIRECTION_NULL = uint.MaxValue;

		public const int SEP_CRYPTO_DIRECTION_ENCRYPT = 0;

		public const int SEP_CRYPTO_DIRECTION_DECRYPT = 1;

		public const int SEP_CRYPTO_DIRECTION_DECRYPT_ENCRYPT = 3;

		public const int SEP_CRYPTO_DIRECTION_RESERVE32B = int.MaxValue;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class sep_cipher_mode
	{
		public const uint SEP_CIPHER_NULL_MODE = uint.MaxValue;

		public const int SEP_CIPHER_ECB = 0;

		public const int SEP_CIPHER_CBC = 1;

		public const int SEP_CIPHER_CTR = 2;

		public const int SEP_CIPHER_CBC_MAC = 3;

		public const int SEP_CIPHER_XTS = 4;

		public const int SEP_CIPHER_XCBC_MAC = 5;

		public const int SEP_CIPHER_OFB = 6;

		public const int SEP_CIPHER_CMAC = 7;

		public const int SEP_CIPHER_CCM = 8;

		public const int SEP_CIPHER_CBC_CTS = 11;

		public const int SEP_CIPHER_GCTR = 12;

		public const int SEP_CIPHER_RESERVE32B = int.MaxValue;
	}

	public class sep_hash_mode
	{
		public const uint SEP_HASH_NULL = uint.MaxValue;

		public const int SEP_HASH_SHA1 = 0;

		public const int SEP_HASH_SHA256 = 1;

		public const int SEP_HASH_SHA224 = 2;

		public const int SEP_HASH_SHA512 = 3;

		public const int SEP_HASH_SHA384 = 4;

		public const int SEP_HASH_MD5 = 5;

		public const int SEP_HASH_CBC_MAC = 6;

		public const int SEP_HASH_XCBC_MAC = 7;

		public const int SEP_HASH_CMAC = 8;

		public const int SEP_HASH_MODE_NUM = 9;

		public const int SEP_HASH_RESERVE32B = int.MaxValue;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class sep_hash_hw_mode
	{
		public const int SEP_HASH_HW_MD5 = 0;

		public const int SEP_HASH_HW_SHA1 = 1;

		public const int SEP_HASH_HW_SHA256 = 2;

		public const int SEP_HASH_HW_SHA224 = 10;

		public const int SEP_HASH_HW_SHA512 = 4;

		public const int SEP_HASH_HW_SHA384 = 12;

		public const int SEP_HASH_HW_GHASH = 6;

		public const int SEP_HASH_HW_RESERVE32B = int.MaxValue;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class sep_c2_mode
	{
		public const uint SEP_C2_NULL = uint.MaxValue;

		public const int SEP_C2_ECB = 0;

		public const int SEP_C2_CBC = 1;

		public const int SEP_C2_RESERVE32B = int.MaxValue;
	}

	public class sep_multi2_mode
	{
		public const uint SEP_MULTI2_NULL = uint.MaxValue;

		public const int SEP_MULTI2_ECB = 0;

		public const int SEP_MULTI2_CBC = 1;

		public const int SEP_MULTI2_OFB = 2;

		public const int SEP_MULTI2_RESERVE32B = int.MaxValue;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class sep_crypto_key_type
	{
		public const int SEP_USER_KEY = 0;

		public const int SEP_ROOT_KEY = 1;

		public const int SEP_PROVISIONING_KEY = 2;

		public const int SEP_SESSION_KEY = 3;

		public const int SEP_APPLET_KEY = 4;

		public const int SEP_END_OF_KEYS = int.MaxValue;

		private object method_01()
		{
			return new int[0];
		}
	}

	public class DmaMode
	{
		public const uint DMA_MODE_NULL = uint.MaxValue;

		public const int NO_DMA = 0;

		public const int DMA_SRAM = 1;

		public const int DMA_DLLI = 2;

		public const int DMA_MLLI = 3;
	}

	public class FlowMode
	{
		public const int FLOW_MODE_NULL = -1;

		public const int BYPASS = 0;

		public const int DIN_AES_DOUT = 1;

		public const int AES_to_HASH = 2;

		public const int AES_and_HASH = 3;

		public const int DIN_DES_DOUT = 4;

		public const int DES_to_HASH = 5;

		public const int DES_and_HASH = 6;

		public const int DIN_HASH = 7;

		public const int DIN_HASH_and_BYPASS = 8;

		public const int AESMAC_and_BYPASS = 9;

		public const int AES_to_HASH_and_DOUT = 10;

		public const int DIN_RC4_DOUT = 11;

		public const int DES_to_HASH_and_DOUT = 12;

		public const int AES_to_AES_to_HASH_and_DOUT = 13;

		public const int AES_to_AES_to_HASH = 14;

		public const int AES_to_HASH_and_AES = 15;

		public const int DIN_MULTI2_DOUT = 16;

		public const int DIN_AES_AESMAC = 17;

		public const int HASH_to_DOUT = 18;

		public const int S_DIN_to_AES = 32;

		public const int S_DIN_to_AES2 = 33;

		public const int S_DIN_to_DES = 34;

		public const int S_DIN_to_RC4 = 35;

		public const int S_DIN_to_MULTI2 = 36;

		public const int S_DIN_to_HASH = 37;

		public const int S_AES_to_DOUT = 38;

		public const int S_AES2_to_DOUT = 39;

		public const int S_RC4_to_DOUT = 41;

		public const int S_DES_to_DOUT = 42;

		public const int S_HASH_to_DOUT = 43;

		public const int SET_FLOW_ID = 44;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class SetupOp
	{
		public const int SETUP_LOAD_NOP = 0;

		public const int SETUP_LOAD_STATE0 = 1;

		public const int SETUP_LOAD_STATE1 = 2;

		public const int SETUP_LOAD_STATE2 = 3;

		public const int SETUP_LOAD_KEY0 = 4;

		public const int SETUP_LOAD_XEX_KEY = 5;

		public const int SETUP_WRITE_STATE0 = 8;

		public const int SETUP_WRITE_STATE1 = 9;

		public const int SETUP_WRITE_STATE2 = 10;

		public const int SETUP_WRITE_STATE3 = 11;
	}

	public class AesMacSelector
	{
		public const int AES_SK = 1;

		public const int AES_CMAC_INIT = 2;

		public const int AES_CMAC_SIZE0 = 3;
	}

	public class HwCryptoKey
	{
		public const int USER_KEY = 0;

		public const int ROOT_KEY = 1;

		public const int PROVISIONING_KEY = 2;

		public const int SESSION_KEY = 3;

		public const int RESERVED_KEY = 4;

		public const int PLATFORM_KEY = 5;

		public const int CUSTOMER_KEY = 6;

		public const int KFDE0_KEY = 7;

		public const int KFDE1_KEY = 9;

		public const int KFDE2_KEY = 10;

		public const int KFDE3_KEY = 11;
	}

	public class HwAesKeySize
	{
		public const int AES_128_KEY = 0;

		public const int AES_192_KEY = 1;

		public const int AES_256_KEY = 2;
	}

	public class HwDesKeySize
	{
		public const int DES_ONE_KEY = 0;

		public const int DES_TWO_KEYS = 1;

		public const int DES_THREE_KEYS = 2;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public static int[] hw_desc_init()
	{
		return new int[6];
	}

	public static uint bitmask(int maskSize)
	{
		if (maskSize >= 32)
		{
			uint num = uint.MaxValue;
			return uint.MaxValue;
		}
		return (uint)((1 << maskSize) - 1);
	}

	public static uint tovalue(uint value, int bitsize, int shift)
	{
		uint num = value & (uint)((1 << bitsize) - 1);
		return num << shift;
	}

	public static int[] hw_desc_set_cipher_mode(int[] pDesc, uint cipherMode)
	{
		string text = "hw_desc_set_cipher_mode:";
		pDesc[4] |= (int)tovalue(cipherMode, 4, 10);
		Console.WriteLine(text + pDesc[4]);
		return pDesc;
	}

	public static int[] hw_desc_set_cipher_config0(int[] pDesc, uint cipherConfig)
	{
		string text = "hw_desc_set_cipher_config0:";
		pDesc[4] |= (int)tovalue(cipherConfig, 2, 17);
		Console.WriteLine(text + pDesc[4]);
		return pDesc;
	}

	public static int[] hw_desc_set_cipher_config1(int[] pDesc, uint cipherConfig)
	{
		string text = "hw_desc_set_cipher_config1:";
		pDesc[4] |= (int)tovalue(cipherConfig, 1, 19);
		Console.WriteLine(text + pDesc[4]);
		return pDesc;
	}

	public static int[] hw_desc_set_setup_mode(int[] pDesc, uint setupMode)
	{
		string text = "hw_desc_set_setup_mode:";
		pDesc[4] |= (int)tovalue(setupMode, 2, 20);
		Console.WriteLine(text + pDesc[4]);
		return pDesc;
	}

	public static int[] hw_desc_set_flow_mode(int[] pDesc, uint flowMode)
	{
		string text = "hw_desc_set_flow_mode:";
		pDesc[4] |= (int)tovalue(flowMode, 6, 0);
		Console.WriteLine(text + pDesc[4]);
		return pDesc;
	}

	public static int[] hw_desc_set_dout_sram(int[] pDesc, ulong doutAdr, uint doutSize)
	{
		string text = "hw_desc_set_dout_sram:";
		object[] array = new object[3] { 3720, 0, 32 };
		int shift = Conversions.ToInteger(array[1]);
		int bitsize = Conversions.ToInteger(array[2]);
		pDesc[2] |= (int)tovalue((uint)(doutAdr & 0xFFFFFFFFL), bitsize, shift);
		Console.WriteLine(text + pDesc[2]);
		shift = 0;
		bitsize = 2;
		pDesc[3] |= (int)tovalue(1u, 2, 0);
		Console.WriteLine(text + pDesc[3]);
		shift = 1;
		bitsize = 24;
		pDesc[3] |= (int)tovalue(doutSize, 24, 1);
		Console.WriteLine(text + pDesc[3]);
		return pDesc;
	}

	public static int[] hw_desc_set_dout_dlli(int[] pDesc, ulong doutAdr, uint doutSize, uint axiNs, uint lastind)
	{
		string text = "hw_desc_set_dout_dlli:";
		object[] array = new object[3] { 3720, 0, 32 };
		int shift = Conversions.ToInteger(array[1]);
		int bitsize = Conversions.ToInteger(array[2]);
		pDesc[2] |= (int)tovalue((uint)(doutAdr & 0xFFFFFFFFL), bitsize, shift);
		Console.WriteLine(text + pDesc[2]);
		shift = 16;
		bitsize = 16;
		pDesc[5] |= (int)tovalue((uint)(((doutAdr >> 32) & 0xFFFFFFFFL) << 16), 16, 16);
		Console.WriteLine(text + pDesc[5]);
		shift = 0;
		bitsize = 2;
		pDesc[3] |= (int)tovalue(2u, 2, 0);
		Console.WriteLine(text + pDesc[3]);
		shift = 2;
		bitsize = 24;
		pDesc[3] |= (int)tovalue(doutSize, 24, 2);
		Console.WriteLine(text + pDesc[3]);
		shift = 27;
		bitsize = 1;
		pDesc[3] |= (int)tovalue(lastind, 1, 27);
		Console.WriteLine(text + pDesc[3]);
		shift = 26;
		bitsize = 1;
		pDesc[3] |= (int)tovalue(lastind, 1, 26);
		Console.WriteLine(text + pDesc[3]);
		return pDesc;
	}

	public static int[] hw_desc_set_key_size_aes(int[] pDesc, uint keySize)
	{
		string text = "hw_desc_set_key_size_aes:";
		pDesc[4] |= (int)tovalue((uint)((ulong)(keySize >> 3) - 2uL), 2, 22);
		Console.WriteLine(text + pDesc[4]);
		return pDesc;
	}

	public static int[] hw_desc_set_din_sram(int[] pDesc, uint dinAdr, uint dinSize)
	{
		object[] array = new object[3] { 3712, 0, 32 };
		int shift = Conversions.ToInteger(array[1]);
		int bitsize = Conversions.ToInteger(array[2]);
		pDesc[0] |= (int)tovalue(dinAdr, bitsize, shift);
		shift = 0;
		bitsize = 2;
		pDesc[1] |= (int)tovalue(1u, 2, 0);
		shift = 2;
		bitsize = 24;
		pDesc[1] |= (int)tovalue(dinSize, 24, 2);
		return pDesc;
	}

	public static int[] hw_desc_set_din_const(int[] pDesc, uint val, uint dinSize)
	{
		object[] array = new object[3] { 3712, 0, 32 };
		int shift = Conversions.ToInteger(array[1]);
		int bitsize = Conversions.ToInteger(array[2]);
		pDesc[0] |= (int)tovalue(val, bitsize, shift);
		shift = 27;
		bitsize = 1;
		pDesc[1] |= (int)tovalue(1u, 1, 27);
		shift = 0;
		bitsize = 2;
		pDesc[1] |= (int)tovalue(1u, 2, 0);
		shift = 2;
		bitsize = 24;
		pDesc[1] |= (int)tovalue(dinSize, 24, 2);
		return pDesc;
	}

	public static int[] hw_desc_set_cipher_do(int[] pDesc, uint cipherDo)
	{
		pDesc[4] |= (int)tovalue(cipherDo, 2, 15);
		return pDesc;
	}

	public static int[] hw_desc_set_din_nodma(int[] pDesc, uint dinAdr, uint dinSize)
	{
		object[] array = new object[3] { 3712, 0, 32 };
		int shift = Conversions.ToInteger(array[1]);
		int bitsize = Conversions.ToInteger(array[2]);
		pDesc[0] |= (int)tovalue(dinAdr, bitsize, shift);
		shift = 0;
		bitsize = 2;
		pDesc[1] |= (int)tovalue(0u, 2, 0);
		shift = 2;
		bitsize = 24;
		pDesc[1] |= (int)tovalue(dinSize, 24, 2);
		return pDesc;
	}

	public static int[] hw_desc_set_din_type(int[] pDesc, uint dmaMode, ulong dinAdr, uint dinSize, uint axiId, uint axiNs)
	{
		object[] array = new object[3] { 3712, 0, 32 };
		int shift = Conversions.ToInteger(array[1]);
		int bitsize = Conversions.ToInteger(array[2]);
		pDesc[0] |= (int)tovalue((uint)(dinAdr & 0xFFFFFFFFL), bitsize, shift);
		shift = 0;
		bitsize = 16;
		pDesc[5] |= (int)tovalue((uint)((dinAdr >> 32) & 0xFFFFL), 16, 0);
		shift = 0;
		bitsize = 2;
		pDesc[1] |= (int)tovalue(dmaMode, 2, 0);
		shift = 2;
		bitsize = 24;
		pDesc[1] |= (int)tovalue(dinSize, 24, 2);
		shift = 30;
		bitsize = 2;
		pDesc[1] |= (int)tovalue(axiId, 2, 30);
		shift = 26;
		bitsize = 1;
		pDesc[1] |= (int)tovalue(axiNs, 1, 26);
		return pDesc;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
