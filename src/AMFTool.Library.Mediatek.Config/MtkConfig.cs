using System;
using System.Collections.Generic;
using System.Linq;
using AMFTool.Library.Mediatek.Download_Agent;

namespace AMFTool.Library.Mediatek.Config;

public class MtkConfig
{
	public class TargetConfig
	{
		public bool Sbc;

		public bool Sla;

		public bool Daa;

		public bool SwJtag;

		public bool Epp;

		public bool Cert;

		public bool MemRead;

		public bool MemWrite;

		public bool CmdC8;

		public uint RawValue;
	}

	public Func<long, int, byte[]> Peek;

	public byte[] AuthData;

	public byte[] CertData;

	public bool IsBROM;

	public bool SkipWDT;

	public int interfaces;

	public bool ReadSocID;

	public byte[] PreloaderData;

	public DAConfig Dacfg;

	public int BlVer;

	public int PageSize;

	public string Cpu;

	public int HWCode;

	public byte[] Meid;

	public byte[] SocID;

	public TargetConfig Targetconfig;

	public ChipConfig Chipcfg;

	public byte[] Otp;

	public long UartLogLevel;

	public int HWVer;

	public int SWVer;

	public int HWSubCode;

	public int BromVersion;

	public MtkConfig()
	{
		string hex = "4D4D4D013800000046494C455F494E464F000000010000000500050220082000E008000000000400E00700000001000000000000000060C24D4D4D01980705004D544B0000000000000000000000000000000000000000000000000000000000006062C20000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000003000000000100000000000001000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000BF41BF60D3B614AE9D4224EAD18131A8E7F36920F1E274FCAA25B075203D40AFB4D8306AE4DCC290778CBF5AF29A6BA125691209E6BCBF9463B93B4A0FCEF2738CD379449C3AA2BED68A4B3E6BE751DC6F715959A5FF3B2D0747C82BF6322133981D3C1F880C0B796B449C6573168BC211D8E8181B107489B6B98047356FB1A66641702B47808E7000002F919843BD6D6C735CCB797FE338F32F31C4D89D2DA55654E4C48CEED084DBC8CEF498EEF203110721DAE4C957B5DFBA70B5BB16EB0541E4126FA38848F491F55D05FE12F0D5A00CBA54FCE399986AC2527CE7AC7EB2513B567AD83DC7286F8305602EAD7ED64E119C0AF38F255EEC4484E1F87E45FB03000000000100000000000001000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000BC205D716F54045DA37DFEB0D1D76CE20D86479E755BDCD484898522505768B48F8F2355B14CEF45BE3DE324E4D64DC448C8B9AD7236E9E445A75AF71A9329F3DC4857D751A99964DC9D5F0C85B1E88BA088838E2311CB58766B5312BD05FBA8CEA5341F515C47E8F3428C17B85372E81B4937BF17FFB95DBFC5CE297A547F59BF28B16F62D2EC99542B536F7E35E7130923EED515BAC9A43F5EB424587F01C7C425DAFB65241D2CD22D51A426F02DCC40E1B833FF0AFB7EF9460A9D3CB86A60B5C4890ACCB0AF05B801A0F7363243974DE8CDFE42F4D48F6D030C63DB7F313B4415B6C8300A9420B0A60923C6D6C238BF92444B453FB778CCBF4D86D1C5251503000000000100000000000001000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000BC205D716F54045DA37DFEB0D1D76CE20D86479E755BDCD484898522505768B48F8F2355B14CEF45BE3DE324E4D64DC448C8B9AD7236E9E445A75AF71A9329F3DC4857D751A99964DC9D5F0C85B1E88BA088838E2311CB58766B5312BD05FBA8CEA5341F515C47E8F3428C17B85372E81B4937BF17FFB95DBFC5CE297A547F59BF28B16F62D2EC99542B536F7E35E7130923EED515BAC9A43F5EB424587F01C7C425DAFB65241D2CD22D51A426F02DCC40E1B833FF0AFB7EF9460A9D3CB86A60B5C4890ACCB0AF05B801A0F7363243974DE8CDFE42F4D48F6D030C63DB7F313B4415B6C8300A9420B0A60923C6D6C238BF92444B453FB778CCBF4D86D1C525154D4D4D011000600000000000000000009BFE84F2D5EBF880225E1F87AEE1A436F90934B0ECF90B9BB16524ABE0AFA7DD9623E54C7636C81E8D2DC8E8FB400D990AE4B16F05CC364B99E30FA8CBF500EEB49D2FA28C8C04706BAB1A296065DDE85BFF5AFE7F2C388CDA8CD7AB4C493183B868B9FF55D38DDC5287E3DADF52ACE032FE53820E58EAFCF2528C494C0CEECC417E6E1E827F3017B476923342DC63B574FF9295D171A1E61236FB50DF2BD79BB856EC2287065AE31804DD4C48FE1E1F86C4D01E0F92F45B6BA24E30F7AD5FF0B534BF1495F7A464E3AECAAA89AE980CD7EA0D1D528FCB9CC6E34EDF23FC5D9F05AFAC0720846679A3339145898B0AD1D79D1338D334F55168B6D9D6335A3358";
		object obj = null;
		string text = "";
		AuthData = hex.HexToBytes();
		CertData = (byte[])obj;
		IsBROM = false;
		SkipWDT = false;
		interfaces = -1;
		ReadSocID = false;
		BlVer = -2;
		PageSize = 512;
		Cpu = text;
		HWCode = 0;
		Targetconfig = new TargetConfig();
		UartLogLevel = 2L;
		HWVer = 51712;
		SWVer = 0;

		Chipcfg = (ChipConfig)method_01(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, text, text, obj, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, DAmodes.None, obj, obj, obj, null, false);
	}

	public void SetDAConfig(DAConfig daconfig)
	{
		Dacfg = daconfig;
	}

	public void SetPeek(Func<long, int, byte[]> peek)
	{
		Peek = peek;
	}

	public void SetOtp(byte[] value)
	{
		Otp = value;
	}

	public byte[] GetOtp()
	{
		object obj = null;
		if (PreloaderData ==null)
		{
			if (Otp == null)
			{
				Otp = new byte[32];
				return Otp;
			}
			return Otp;
		}
		int num = PreloaderData.IndexOf(new byte[5] { 77, 77, 77, 1, 48 });
		if (num == -1)
		{
			if (Otp == null)
			{
				Otp = new byte[32];
				return Otp;
			}
			return Otp;
		}
		Otp = PreloaderData.Skip(num + 12).Take(32).ToArray();
		if (Otp == null)
		{
			Otp = new byte[32];
			return Otp;
		}
		return Otp;
	}

	public byte[] GetMeid()
	{
		return Meid;
	}

	private MtkConfig DefaultValue(int hwcode)
	{
		object obj = null;
		ChipConfig chipcfg = Chipcfg;
		if (chipcfg.Var1 == null)
		{
			chipcfg.Var1 = 10;
		}
		chipcfg = Chipcfg;
		if (chipcfg.Watchdog == null)
		{
			chipcfg.Watchdog = 268464128;
		}
		chipcfg = Chipcfg;
		if (chipcfg.Uart == null)
		{
			chipcfg.Uart = 285220864;
		}
		chipcfg = Chipcfg;
		if (chipcfg.BromPayloadAddr == null)
		{
			chipcfg.BromPayloadAddr = 1051136;
		}
		chipcfg = Chipcfg;
		if (chipcfg.DaPayloadAddr == null)
		{
			chipcfg.DaPayloadAddr = 2097152;
		}
		chipcfg = Chipcfg;
		if (chipcfg.CqdmaBase == null)
		{
			chipcfg.CqdmaBase = obj;
		}
		chipcfg = Chipcfg;
		if (chipcfg.GcpuBase == null)
		{
			chipcfg.GcpuBase = obj;
		}
		chipcfg = Chipcfg;
		if (chipcfg.SejBase == null)
		{
			chipcfg.SejBase = obj;
		}
		chipcfg = Chipcfg;
		if (chipcfg.Dacode == null)
		{
			chipcfg.Dacode = hwcode;
		}
		chipcfg = Chipcfg;
		if (chipcfg.ApDmaMem == null)
		{
			chipcfg.ApDmaMem = 285213088;
		}
		Chipcfg.Damode = ((Chipcfg.Damode == DAmodes.None) ? ((DAmodes)3) : Chipcfg.Damode);
		chipcfg = Chipcfg;
		if (chipcfg.DxccBase == null)
		{
			chipcfg.DxccBase = obj;
		}
		chipcfg = Chipcfg;
		if (chipcfg.MeidAddr == null)
		{
			chipcfg.MeidAddr = obj;
		}
		chipcfg = Chipcfg;
		if (chipcfg.SocidAddr == null)
		{
			chipcfg.SocidAddr = obj;
		}
		chipcfg = Chipcfg;
		if (chipcfg.ProvAddr == null)
		{
			chipcfg.ProvAddr = obj;
		}
		return this;
	}

	public void InitHwCode(int hwcode)
	{
		string text = " (";
		string text2 = ")";
		object obj = null;
		string text3 = "";
		HWCode = hwcode;
		if (!ChipConfig.HwConfig.ContainsKey(hwcode))
		{
			Chipcfg = (ChipConfig)method_01(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, text3, text3, obj, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, DAmodes.None, obj, obj, obj, null, false);
			DefaultValue(hwcode);
			return;
		}
		Chipcfg = ChipConfig.HwConfig[hwcode];
		Chipcfg.HwCode = hwcode;
		if (GetMTKChipINFO(hwcode, out var info))
		{
			Chipcfg.Description = info.Key + text + info.Value + text2;
			Chipcfg.Name = info.Key;
		}
		DefaultValue(hwcode);
	}

	public (uint, uint) GetWatchdogAddr()
	{
		uint num = Convert.ToUInt32(Chipcfg.Watchdog);
		(uint, uint) result;
		if (num != 0)
		{
			if (num != 8704)
			{
				if (num <= 270602240)
				{
					switch (num)
					{
					case 268464128u:
						return (num, 570425444u);
					case 268465152u:
						return (num, 570425344u);
					case 270602240u:
						return (num, 570425444u);
					}
					result = (num, 570425444u);
				}
				else
				{
					switch (num)
					{
					case 270606336u:
						return (num, 570425344u);
					case 2684551168u:
						return (num, 8704u);
					case 3221225472u:
						return (num, 8804u);
					}
					result = (num, 570425444u);
				}
				return result;
			}
			int hWCode = HWCode;
			if (hWCode <= 25173)
			{
				if (hWCode != 25169)
				{
					if (hWCode != 25173)
					{
						return (num, 1879199744u);
					}
					return (num, 1881014272u);
				}
				result = (num, 2147680256u);
				goto IL_0183;
			}
			if (hWCode != 25206)
			{
				if (hWCode == 25878)
				{
					result = (num, 2147680256u);
					goto IL_0183;
				}
				if (hWCode != 33123)
				{
					return (num, 1879199744u);
				}
				result = (num, 1628176384u);
			}
			else
			{
				result = (num, 1628176384u);
			}
			return result;
		}
		return (0u, 0u);
		IL_0183:
		return result;
	}

	public (object, object) GetWatchDogAddr(int wdt)
	{
		object obj = null;
		if (wdt != 0)
		{
			(object, object) result;
			(object, object) tuple;
			if (wdt <= 268464128)
			{
				switch (wdt)
				{
				default:
					return (wdt, 570425444);
				case -1073741824:
					return (wdt, 8804);
				case 8704:
				{
					int hWCode = HWCode;
					if (hWCode <= 25173)
					{
						if (hWCode == 25169)
						{
							tuple = (wdt, 2147680256u);
							goto IL_01d4;
						}
						if (hWCode != 25173)
						{
							tuple = (wdt, 1879199744);
							goto IL_020c;
						}
						tuple = (wdt, 1881014272);
						result = tuple;
					}
					else
					{
						if (hWCode != 25206)
						{
							if (hWCode == 25878)
							{
								tuple = (wdt, 2147680256u);
								goto IL_01d4;
							}
							if (hWCode != 33123)
							{
								tuple = (wdt, 1879199744);
								goto IL_020c;
							}
							tuple = (wdt, 1628176384);
						}
						else
						{
							tuple = (wdt, 1628176384);
						}
						result = tuple;
					}
					goto IL_01d6;
				}
				case 268464128:
					{
						return (wdt, 570425444);
					}
					IL_01d4:
					result = tuple;
					goto IL_01d6;
					IL_01d6:
					return result;
					IL_020c:
					result = tuple;
					goto IL_01d6;
				}
			}
			return wdt switch
			{
				268465152 => (wdt, 570425344), 
				270602240 => (wdt, 570425444), 
				270606336 => (wdt, 570425344), 
				_ => (wdt, 570425444), 
			};
		}
		return (obj, obj);
	}

	public bool GetMTKChipINFO(int hwcode, out KeyValuePair<string, string> info)
	{
		string key = "MT6261";
		string value = "MTK_SV3";
		string key2 = "MT6572";
		string key3 = "MT6592";
		string key4 = "MT6580";
		string key5 = "MT6582";
		string key6 = "MT6735";
		string key7 = "MT6753";
		string key8 = "MT6737";
		string key9 = "MT6739";
		string value2 = "MTK_SV5";
		string key10 = "MT6755";
		string value3 = "Helio P10/P10M/P15/P18";
		string key11 = "MT6750";
		string key12 = "MT6757";
		string value4 = "Helio P20/P25";
		string key13 = "MT6761";
		string value5 = "Helio A20/A22";
		string key14 = "MT6763";
		string value6 = "Helio P23";
		string key15 = "MT6765";
		string value7 = "Helio A25/G25/P22/G35/P35";
		string key16 = "MT6768";
		string value8 = "Helio G70/G80/G85";
		string key17 = "MT6771";
		string value9 = "Helio P60/P70";
		string key18 = "MT6779";
		string value10 = "Helio P90";
		string key19 = "MT6785";
		string value11 = "Helio G90/G90T/G95";
		string key20 = "MT6885";
		string value12 = "D1000_+_L_C";
		string key21 = "MT6797";
		string value13 = "Helio X20/X20M/X25/X27";
		string key22 = "MT6799";
		string value14 = "Helio X30";
		string key23 = "MT6758";
		string value15 = "Helio P30";
		string key24 = "MT6873";
		string value16 = "D800";
		string key25 = "MT8127";
		string key26 = "MT8163";
		string key27 = "MT8173";
		string key28 = "MT8695";
		string value17 = "Dimensity_5G";
		string key29 = "MT8168";
		string key30 = "MT6795";
		string value18 = "Helio X10/X10T";
		string key31 = "MT6893";
		string value19 = "D1100/1200 5G";
		string key32 = "MT6877";
		string value20 = "D900/920 5G";
		string key33 = "MT6853";
		string value21 = "D720/800U";
		string key34 = "MT6833";
		string value22 = "D700";
		string key35 = "MT6781";
		string value23 = "Helio G96";
		string key36 = "MT6983";
		string key37 = "MT6897";
		string value24 = "D8300 Ultra";
		string key38 = "MT6789";
		string value25 = "Helio G99";
		string key39 = "MT6835";
		string value26 = "D6100+";
		string key40 = "MT6886";
		string value27 = "D200 Ultra";
		string key41 = "MT6855";
		string value28 = "D8100";
		string key42 = "MT6895";
		string key43 = "MT6985";
		string value29 = "D9200";
		if (hwcode == 25185)
		{
			info = new KeyValuePair<string, string>(key, value);
		}
		else if (hwcode == 25970)
		{
			info = new KeyValuePair<string, string>(key2, value);
		}
		else if (hwcode == 26002)
		{
			info = new KeyValuePair<string, string>(key3, value);
		}
		else if (hwcode == 25984)
		{
			info = new KeyValuePair<string, string>(key4, value);
		}
		else if (hwcode != 25986)
		{
			switch (hwcode)
			{
			default:
				info = default(KeyValuePair<string, string>);
				break;
			case 4466:
				info = new KeyValuePair<string, string>(key42, value28);
				break;
			case 4393:
				info = new KeyValuePair<string, string>(key41, value28);
				break;
			case 4649:
				info = new KeyValuePair<string, string>(key40, value27);
				break;
			case 4758:
				info = new KeyValuePair<string, string>(key43, value29);
				break;
			case 4616:
				info = new KeyValuePair<string, string>(key38, value25);
				break;
			case 4617:
				info = new KeyValuePair<string, string>(key39, value26);
				break;
			case 2311:
				info = new KeyValuePair<string, string>(key36, value19);
				break;
			case 4198:
				info = new KeyValuePair<string, string>(key35, value23);
				break;
			case 2441:
				info = new KeyValuePair<string, string>(key34, value22);
				break;
			case 4611:
				info = new KeyValuePair<string, string>(key37, value24);
				break;
			case 2454:
				info = new KeyValuePair<string, string>(key33, value21);
				break;
			case 2384:
				info = new KeyValuePair<string, string>(key31, value19);
				break;
			case 2393:
				info = new KeyValuePair<string, string>(key32, value20);
				break;
			case 26517:
				info = new KeyValuePair<string, string>(key30, value18);
				break;
			case 34453:
				info = new KeyValuePair<string, string>(key28, value17);
				break;
			case 33139:
				info = new KeyValuePair<string, string>(key27, value2);
				break;
			case 33128:
				info = new KeyValuePair<string, string>(key29, value2);
				break;
			case 33063:
				info = new KeyValuePair<string, string>(key25, value2);
				break;
			case 2182:
				info = new KeyValuePair<string, string>(key24, value16);
				break;
			case 1672:
				info = new KeyValuePair<string, string>(key23, value15);
				break;
			case 33123:
				info = new KeyValuePair<string, string>(key26, value2);
				break;
			case 1378:
				info = new KeyValuePair<string, string>(key22, value14);
				break;
			case 2070:
				info = new KeyValuePair<string, string>(key20, value12);
				break;
			case 2067:
				info = new KeyValuePair<string, string>(key19, value11);
				break;
			case 633:
				info = new KeyValuePair<string, string>(key21, value13);
				break;
			case 1928:
				info = new KeyValuePair<string, string>(key17, value9);
				break;
			case 1799:
				info = new KeyValuePair<string, string>(key16, value8);
				break;
			case 1894:
				info = new KeyValuePair<string, string>(key15, value7);
				break;
			case 1829:
				info = new KeyValuePair<string, string>(key18, value10);
				break;
			case 1680:
				info = new KeyValuePair<string, string>(key14, value6);
				break;
			case 1361:
				info = new KeyValuePair<string, string>(key12, value4);
				break;
			case 1815:
				info = new KeyValuePair<string, string>(key13, value5);
				break;
			case 806:
				info = new KeyValuePair<string, string>(key10, value3);
				break;
			case 1537:
				info = new KeyValuePair<string, string>(key11, value2);
				break;
			case 821:
				info = new KeyValuePair<string, string>(key8, value);
				break;
			case 823:
				info = new KeyValuePair<string, string>(key7, value);
				break;
			case 801:
				info = new KeyValuePair<string, string>(key6, value);
				break;
			case 1689:
				info = new KeyValuePair<string, string>(key9, value2);
				break;
			}
		}
		else
		{
			info = new KeyValuePair<string, string>(key5, value);
		}
		string key44 = info.Key;
		return key44 != null && key44.Length > 0;
	}

	public static object method_01(object arg_01, object arg_02, object arg_03, object arg_04, object arg_05, object arg_06, object arg_07, object arg_08, object arg_09, object arg_10, object arg_11, string arg_12, string arg_13, object arg_14, object arg_15, object arg_16, object arg_17, object arg_18, List<uint[]> arg_19, object arg_20, object arg_21, List<uint[]> arg_22, DAmodes arg_23, object arg_24, object arg_25, object arg_26, int? arg_27, bool? arg_28)
	{
		return new ChipConfig(arg_01, arg_02, arg_03, arg_04, arg_05, arg_06, arg_07, arg_08, arg_09, arg_10, arg_11, arg_12, arg_13, arg_14, arg_15, arg_16, arg_17, arg_18, arg_19, arg_20, arg_21, arg_22, arg_23, arg_24, arg_25, arg_26, arg_27, arg_28);
	}

	public static int method_02(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	public static bool E8F36(object arg_01, int arg_02, ref KeyValuePair<string, string> arg_03)
	{
		return ((MtkConfig)arg_01).GetMTKChipINFO(arg_02, out arg_03);
	}
}
