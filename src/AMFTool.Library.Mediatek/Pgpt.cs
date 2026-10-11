using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AMFTool.Library.Mediatek;

public class Pgpt
{
	public class Guid
	{
		public string PatName;

		public string Uuid;

		public Guid(string patName, string uuid)
		{
			PatName = patName;
			Uuid = uuid;
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public List<Guid> GuidList;

	public List<string> GuidManual;

	public byte[] PgptManual(long discSize, bool repairPgpt)
	{
		int num = 1024;
		string hex = "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000ee00000001000000ff7f8f0e00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000055aa";
		int num2 = 0;
		string s = "EFI PART";
		int num3 = 92;
		int num4 = 1;
		int num5 = 512;
		string text = "A2A0D0EBE5B9334487C068B6B72699C7";
		string text2 = "";
		string text3 = "0000000000000000";
		string text4 = "preloader";
		string text5 = "pgpt";
		string text6 = "sgpt";
		string text7 = "userdata";
		string text8 = "otp";
		string text9 = "flashinfo";
		string oldValue = " ";
		string pattern = "^0+";
		int num6 = 33;
		int num7 = 32;
		int num8 = 40;
		int num9 = 16384;
		object result = null;
		try
		{
			int num10 = 528;
			int num11 = 600;
			byte[] array = new byte[17408];
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				byte[] array2 = hex.HexToBytes();
				memoryStream.Write(array2, 0, array2.Length);
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				memoryStream.Write(bytes, 0, bytes.Length);
				byte[] bytes2 = BitConverter.GetBytes(65536);
				memoryStream.Write(bytes2, 0, bytes2.Length);
				byte[] bytes3 = BitConverter.GetBytes(92);
				memoryStream.Write(bytes3, 0, bytes3.Length);
				byte[] bytes4 = BitConverter.GetBytes(0);
				memoryStream.Write(bytes4, 0, bytes4.Length);
				byte[] bytes5 = BitConverter.GetBytes(0);
				memoryStream.Write(bytes5, 0, bytes5.Length);
				byte[] bytes6 = BitConverter.GetBytes(1L);
				memoryStream.Write(bytes6, 0, bytes6.Length);
				byte[] bytes7 = BitConverter.GetBytes((discSize - 512L) / 512L);
				memoryStream.Write(bytes7, 0, bytes7.Length);
				byte[] bytes8 = BitConverter.GetBytes(34L);
				memoryStream.Write(bytes8, 0, bytes8.Length);
				byte[] bytes9 = BitConverter.GetBytes((discSize - 17408L) / 512L);
				memoryStream.Write(bytes9, 0, bytes9.Length);
				byte[] array3 = Enumerable.Repeat((byte)0, 16).ToArray();
				memoryStream.Write(array3, 0, array3.Length);
				byte[] bytes10 = BitConverter.GetBytes(2L);
				memoryStream.Write(bytes10, 0, bytes10.Length);
				byte[] bytes11 = BitConverter.GetBytes(128);
				memoryStream.Write(bytes11, 0, bytes11.Length);
				byte[] bytes12 = BitConverter.GetBytes(128);
				memoryStream.Write(bytes12, 0, bytes12.Length);
				byte[] bytes13 = BitConverter.GetBytes(3863163032u);
				memoryStream.Write(bytes13, 0, bytes13.Length);
				GetCRC32(bytes.Concat(bytes2).Concat(bytes3).Concat(bytes4)
					.Concat(bytes5)
					.Concat(bytes6)
					.Concat(bytes7)
					.Concat(bytes8)
					.Concat(bytes9)
					.Concat(array3)
					.Concat(bytes11)
					.Concat(bytes12)
					.Concat(bytes13)
					.ToArray());
				string text10 = text;
				string text11 = text2;
				string text12 = text2;
				string text13 = text2;
				string text14 = text3;
				string text15 = text2;
				int num12 = 0;
				long num13 = 0L;
				long num14 = 0L;
				long num15 = 0L;
				long num16 = 0L;
				long num17 = 0L;
				long num18 = 0L;
				memoryStream.Seek(1024L, SeekOrigin.Begin);
				if (GptAddr.Addr.Count > 0)
				{
					foreach (GptAddr item in GptAddr.Addr)
					{
						if (item.Name == text4 || item.Name == text5 || item.Name == text6)
						{
							continue;
						}
						if (repairPgpt)
						{
							if (item.Name == text7)
							{
								num14 = 1024L + num13;
							}
							if (item.Name == text8)
							{
								num15 = 1024L + num13;
								num16 = item.Size;
							}
							if (item.Name == text9)
							{
								num17 = 1024L + num13;
								num18 = item.Size;
							}
						}
						text11 = GuidManual[num12];
						text12 = BitConverter.GetBytes(item.Start / 512L).ToHexString();
						text13 = BitConverter.GetBytes((item.Start + item.Size - 512L) / 512L).ToHexString();
						text15 = FixHash(Encoding.ASCII.GetBytes(item.Name.Replace(oldValue, text2)).ToHexString());
						string input = (text10 + text11 + text12 + text13 + text14 + text15).Replace(oldValue, text2);
						input = Regex.Replace(input, pattern, text2);
						int lenAdd = 256 - input.Length;
						byte[] array4 = (input + AddZero(lenAdd)).HexToBytes();
						num12++;
						memoryStream.Write(array4, 0, array4.Length);
						num13 += array4.Length;
					}
				}
				if (repairPgpt)
				{
					int num19 = num6;
					long num20 = discSize / num5 - num19 - num4;
					long num21 = num20 - num18 / num5 + num4;
					byte[] bytes14 = BitConverter.GetBytes(num21);
					byte[] bytes15 = BitConverter.GetBytes(num20);
					memoryStream.Seek(num17 + num7, (SeekOrigin)num2);
					memoryStream.Write(bytes14, num2, bytes14.Length);
					memoryStream.Write(bytes15, num2, bytes15.Length);
					long num22 = num16 / num5;
					long num23 = num21 - num4;
					long num24 = num23 - num22 + num4;
					byte[] bytes16 = BitConverter.GetBytes(num24);
					byte[] bytes17 = BitConverter.GetBytes(num23);
					memoryStream.Seek(num15 + num7, (SeekOrigin)num2);
					memoryStream.Write(bytes16, num2, bytes16.Length);
					memoryStream.Write(bytes17, num2, bytes17.Length);
					byte[] bytes18 = BitConverter.GetBytes(num24 - num4);
					memoryStream.Seek(num14 + num8, (SeekOrigin)num2);
					memoryStream.Write(bytes18, num2, bytes18.Length);
				}
				int num25 = num9;
				byte[] array5 = new byte[num25];
				memoryStream.Seek(num, (SeekOrigin)num2);
				memoryStream.Read(array5, num2, array5.Length);
				byte[] cRC = GetCRC32(array5);
				memoryStream.Seek(num11, (SeekOrigin)num2);
				memoryStream.Write(cRC, num2, cRC.Length);
				byte[] array6 = new byte[num3];
				memoryStream.Seek(num5, (SeekOrigin)num2);
				memoryStream.Read(array6, num2, array6.Length);
				byte[] cRC2 = GetCRC32(array6);
				memoryStream.Seek(num10, (SeekOrigin)num2);
				memoryStream.Write(cRC2, num2, cRC2.Length);
			}
			return array;
		}
		catch
		{
			return (byte[])result;
		}
	}

	public string AddZero(int lenAdd)
	{
		string text = "";
		string text2 = "0";
		string text3 = text;
		for (int i = 0; i < lenAdd; i++)
		{
			text3 += text2;
		}
		return text3;
	}

	public string FixHash(string partNameHex)
	{
		string text = "00";
		string text2 = string.Empty;
		int num;
		for (num = 0; num < partNameHex.Length; num++)
		{
			text2 = text2 + partNameHex.Substring(num, 2) + text;
			num++;
		}
		return text2;
	}

	public byte[] GetCRC32(byte[] sourceBytes)
	{
		MemoryStream memoryStream = new MemoryStream(sourceBytes);
		long num = 4294967295L;
		byte[] array = new byte[4096];
		int num2 = memoryStream.Read(array, 0, 4096);
		long[] array2 = new long[256];
		long num3 = 3988292384L;
		for (int i = 0; i <= 255; i++)
		{
			long num4 = i;
			for (int num5 = 8; num5 >= 1; num5--)
			{
				if ((ulong)(num4 & 1L) <= 0uL)
				{
					num4 = ((num4 & 0xFFFFFFFEL) / 2L) & 0x7FFFFFFFL;
				}
				else
				{
					num4 = ((num4 & 0xFFFFFFFEL) / 2L) & 0x7FFFFFFFL;
					num4 ^= num3;
				}
			}
			array2[i] = num4;
		}
		while (num2 > 0)
		{
			for (int j = 0; j < num2; j++)
			{
				long num6 = (num & 0xFFL) ^ array[j];
				num = ((num & 0xFFFFFF00L) / 256L) & 0xFFFFFFL;
				num ^= array2[num6];
			}
			num2 = memoryStream.Read(array, 0, 4096);
		}
		return BitConverter.GetBytes((int)(~num));
	}

	public Pgpt()
	{
		string item = "30d37af5c23988449bb000cb43c9ccd4";
		string item2 = "976d68fe4435414abe21167e25b61b6f";
		string item3 = "a843b11ca8b1574bb251945c5119e8fe";
		string item4 = "3b349e3bc8cd7f4d9fa6b6812e50ab62";
		string item5 = "792c6a5f1766854bac02c2975a14d2d7";
		string item6 = "0b05e24ab55df74faad35730534be63d";
		string item7 = "39099b1f6be1c94ba5bcdc2ee969d801";
		string item8 = "21c722d7ee0db84c8a832c63cd1393c7";
		string item9 = "a87921e0b5cea94888314f1c9c5a8695";
		string item10 = "819ab084d2faac41890e407c24975e74";
		string item11 = "efa5f0e81b8dea429c2a835cd77de363";
		string item12 = "75e1f0d5e1a6b74d94c0f82ad032950b";
		string item13 = "e156901d39e1ca4f8c0bb75fd74d81c6";
		string item14 = "0b219277a8b6d545ad913361ed14c608";
		string item15 = "b96d8a1332101d4591e90fa38ff94fbb";
		string item16 = "4c936d75e350914caf4602d824169ca7";
		string item17 = "67c2f3a32155dd42a7243bdec20c7c6f";
		string item18 = "2acd688cc9cc5d4c8b5734ae9b2dd481";
		string item19 = "f8eb5c6aa754894b8d1dc5eb140b095b";
		string item20 = "f85bd6a0dee8074194341d318c843d37";
		string item21 = "bbc0f04627f2b64eb82f66408e13e36d";
		string item22 = "31c1c2fb92631742b51e548a6edb03d0";
		string item23 = "81a995e185e234478025ec323e9589d9";
		string item24 = "f85290e23a5d974eadb55f312ce6610a";
		string item25 = "d7ab3c9c5da3454b8c57b80775426b35";
		string item26 = "319709e7a695a645a1e51b6aba032cf1";
		string item27 = "abe173826f846844b999ee2ea8e50a16";
		string item28 = "f17264d2bc9e1d42ba14311296457c90";
		string item29 = "e9cb2cb75520f446a1674a069c201738";
		string item30 = "f320159cc5c2894b8242fe4c61208a9e";
		string item31 = "3f5f2d904a43e74d8988321e88c9b8aa";
		string item32 = "c874cebee2d863489bfe5b0b66bb920f";
		string item33 = "cf4213ffbeb7d544a25ea435addd2702";
		string item34 = "1b8fdaa407fe3b4395cb84a5f23e477b";
		string item35 = "155e63c2aa614e459c40ebe1bdf19b9b";
		string item36 = "90122d4da3365d4fafb4319f8ab6dcd8";
		string item37 = "f012cefdeba7f7408350960972e6cb57";
		string item38 = "a2afbb0fa94a904489835329328505fd";
		string item39 = "2f4b6ea7cb31ba40826ac0cb0b73c856";
		string item40 = "30c04af50470024d9481bbf982036807";
		string item41 = "e210c3c47e4ad377481861e2d8bb5e86";
		string item42 = "0f713437130fb91a734c12a08ec50837";
		string item43 = "2fb0a5857337b3181049718cde95107e";
		string item44 = "a683ce6f735248471145c205ebb4b8ad";
		string item45 = "a3e64536e3a7b2194941172c10190eef";
		string item46 = "b4920bf62f0eb691fb4a2d0b643bab4b";
		string item47 = "feed5cd0b720b36859f24fe25317f2fc";
		string item48 = "670b065474fa825c611644821e54dced";
		string item49 = "fb28a1c0d1599e0af0e8ea3d36a9d658";
		string item50 = "f8a7f5f5ecf8a13bcbc94e2a7cbc7d07";
		string item51 = "8a616d73795b0b539dac895339690613";
		string item52 = "c62a34a2404c2cb0514fe90d46f93454";
		string item53 = "746f9d33b8bc2bf0694aae0152bcadc0";
		string item54 = "a322d12c4261a9311288cb60fbead9be";
		string item55 = "d17b9bc0465e0c4597d5adf4072a8d6b";
		string item56 = "b8658a44d4a5b0bdddebbf4ea3ceafc6";
		string item57 = "74e3b1b17f8c8ab4d575ce0ca20f1183";
		string item58 = "d47dd324f8ca6e1a45be89e4b0705843";
		string item59 = "107b9537ef6740143b896200484311b6";
		string item60 = "8b971c440953aa1f913c32c5f087e9ae";
		string item61 = "26b55f57238dc461fed5e9e040eb0741";
		string item62 = "";
		GuidList = new List<Guid>();
		GuidManual = new List<string>
		{
			item, item2, item3, item4, item5, item6, item7, item8, item9, item10,
			item11, item12, item13, item14, item15, item16, item17, item18, item19, item20,
			item21, item22, item23, item24, item25, item26, item27, item28, item29, item30,
			item31, item32, item33, item34, item35, item36, item37, item38, item39, item40,
			item41, item42, item43, item44, item45, item46, item47, item48, item49, item50,
			item51, item52, item53, item54, item55, item56, item57, item58, item59, item60,
			item61, item62, item62, item62, item62, item62, item62, item62
		};

	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
