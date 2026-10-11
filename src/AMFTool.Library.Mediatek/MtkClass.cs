using System;
using System.Collections.Generic;
using AMFTool.Library.Mediatek.Config;
using AMFTool.Library.Mediatek.Download_Agent;
using AMFTool.Library.Mediatek.Exploit;
using AMFTool.Library.Usb;
using AMFTool.ThreadWorker;

namespace AMFTool.Library.Mediatek;

public class MtkClass
{
	public MtkDaLoader DaLoader;

	public MtkPreloader Pl;

	public MtkConfig Config;

	public Kamakiri2 Kamakiri2;

	public Kamakiripl KamakiriPl;

	private ThreadMETA threadMETA;

	public MtkClass(ThreadMTK th)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		DaLoader = th.DaLoader;
		Config = th.Config;
		Kamakiri2 = new Kamakiri2(this);
		KamakiriPl = new Kamakiripl(this);
		Pl = th.Preloader;
		Config.Dacfg = th.Dacfg;
	}

	public MtkClass(ThreadMETA threadMETA)
	{
		this.threadMETA = threadMETA;
	}

	public byte[] PatchPreloaderSecurityDA1(byte[] input)
	{
		int num = 0;
		string item = "A3687BB12846";
		string item2 = "0123A3602846";
		string item3 = "oppo security";
		string item4 = "B3F5807F01D1";
		string item5 = "B3F5807F01D14FF000004FF000007047";
		string item6 = "mt6739 c30";
		string item7 = "B3F5807F04BF4FF4807305F011B84FF0FF307047";
		string item8 = "B3F5807F04BF4FF480734FF000004FF000007047";
		string item9 = "regular";
		string item10 = "10B50C680268";
		string item11 = "10B5012010BD";
		string item12 = "ram blacklist";
		string item13 = "08B5104B7B441B681B68";
		string item14 = "00207047000000000000";
		string item15 = "seclib_sec_usbdl_enabled";
		string item16 = "5072656C6F61646572205374617274";
		string item17 = "50617463686564204C205374617274";
		string item18 = "Patched loader msg";
		string item19 = "F0B58BB002AE20250C460746";
		string item20 = "002070470000000000205374617274";
		string item21 = "sec_img_auth";
		string item22 = "FFC0F3400008BD";
		string item23 = "FF4FF0000008BD";
		string item24 = "get_vfy_policy";
		string item25 = "040007C0";
		string item26 = "00000000";
		string item27 = "hash_check";
		string item28 = "CCF20709";
		string item29 = "4FF00009";
		string item30 = "hash_check2";
		string item31 = "hash_check3";
		string text = "Patched \"";
		string text2 = "\" in preloader";
		string value = "Failed to patch preloader security";
		bool flag = false;
		byte[] array = new byte[input.Length];
		input.AsSpan(0, input.Length).CopyTo(array.AsSpan(0, input.Length));
		List<(string, string, string)> list = new List<(string, string, string)>();
		list.Add((item, item2, item3));
		list.Add((item4, item5, item6));
		list.Add((item7, item8, item9));
		list.Add((item10, item11, item12));
		list.Add((item13, item14, item15));
		list.Add((item16, item17, item18));
		list.Add((item19, item20, item21));
		list.Add((item22, item23, item24));
		list.Add((item25, item26, item27));
		list.Add((item28, item29, item30));
		list.Add((new byte[6] { 20, 44, 246, 46, 254, 231 }.ToHexString(), new byte[6].ToHexString(), item31));
		List<(string, string, string)> list2 = list;
		foreach (var item32 in list2)
		{
			byte[] pattern = item32.Item1.HexToBytes();
			int num2 = array.IndexOf(pattern);
			if (num2 != -1)
			{
				byte[] array2 = item32.Item2.HexToBytes();
				array2.CopyTo(array, num2);
				Console.WriteLine(text + item32.Item3 + text2);
				flag = true;
			}
		}
		if ((flag ? 1 : 0) == num)
		{
			Console.WriteLine(value);
		}
		return array;
	}

	public byte[] PatchPreloaderSecurityDA2(byte[] input)
	{
		int num = 0;
		string item = "A3687BB12846";
		string item2 = "0123A3602846";
		string item3 = "oppo security";
		string item4 = "B3F5807F01D1";
		string item5 = "B3F5807F01D14FF000004FF000007047";
		string item6 = "mt6739 c30";
		string item7 = "B3F5807F04BF4FF4807305F011B84FF0FF307047";
		string item8 = "B3F5807F04BF4FF480734FF000004FF000007047";
		string item9 = "regular";
		string item10 = "10B50C680268";
		string item11 = "10B5012010BD";
		string item12 = "ram blacklist";
		string item13 = "08B5104B7B441B681B68";
		string item14 = "00207047000000000000";
		string item15 = "seclib_sec_usbdl_enabled";
		string item16 = "5072656C6F61646572205374617274";
		string item17 = "50617463686564204C205374617274";
		string item18 = "Patched loader msg";
		string item19 = "F0B58BB002AE20250C460746";
		string item20 = "002070470000000000205374617274";
		string item21 = "sec_img_auth";
		string item22 = "FFC0F3400008BD";
		string item23 = "FF4FF0000008BD";
		string item24 = "get_vfy_policy";
		string text = "Patched \"";
		string text2 = "\" in preloader";
		string value = "Failed to patch preloader security";
		bool flag = false;
		byte[] array = new byte[input.Length];
		input.AsSpan(0, input.Length).CopyTo(array.AsSpan(0, input.Length));
		List<(string, string, string)> list = new List<(string, string, string)>
		{
			(item, item2, item3),
			(item4, item5, item6),
			(item7, item8, item9),
			(item10, item11, item12),
			(item13, item14, item15),
			(item16, item17, item18),
			(item19, item20, item21),
			(item22, item23, item24)
		};
		foreach (var item25 in list)
		{
			byte[] pattern = item25.Item1.HexToBytes();
			int num2 = array.IndexOf(pattern);
			if (num2 != -1)
			{
				byte[] array2 = item25.Item2.HexToBytes();
				array2.CopyTo(array, num2);
				Console.WriteLine(text + item25.Item3 + text2);
				flag = true;
			}
		}
		if ((flag ? 1 : 0) == num)
		{
			Console.WriteLine(value);
		}
		return array;
	}

	public bool Crasher(int? mode = null)
	{
		string text = " usb port ";
		string value = "auto cResh OF ";
		string value2 = "PreLoader";
		string value3 = "auto cResh ON ";
		string msg = "Force Preloader to BOOTROM... ";
		string value4 = "We'are not in bootrom, trying to crash da";
		double field_ = Class38.field_203;
		string msg2 = "OK";
		string msg3 = "Waiting Mediatek USB Connection... ";
		string category = "MTK";
		string msg4 = "Failed!";
		string msg5 = "Name : ";
		string text2 = "Connecting to device ";
		string text3 = "... ";
		string msg6 = "Handshaking... ";
		object status = null;
		Console.WriteLine(text + Portinfo.Name);
		if (!MTKConfig.MTKServer)
		{
			if (!Portinfo.Name.Contains(value2))
			{
				return true;
			}
			Console.WriteLine(value3);
			UI.RichLogs(msg, UI.log);
			Console.WriteLine(value4);
			Crash(mode.Value);
			SerialMTK.Close();
			utils.Pause(field_);
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg3, UI.log);
			if (!UsbScan.FindUSB(category))
			{
				UI.RichLogs(msg4, UI.error, nextline: true);
				return false;
			}
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg5, UI.log);
			UI.RichLogs(Portinfo.Name, UI.info, nextline: true);
			UI.RichLogs(text2 + Portinfo.COM + text3, UI.log);
			if (SerialMTK.Open(Portinfo.COM))
			{
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg6, UI.log);
				if (SerialMTK.Handshake((byte[])status))
				{
					UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
					Pl.Init();
					return true;
				}
				UI.RichLogs(msg4, UI.error, nextline: true);
				return false;
			}
			UI.RichLogs(msg4, UI.error, nextline: true);
			return false;
		}
		Console.WriteLine(value);
		return true;
	}

	public void Crash(int mode = 0)
	{
		switch (mode)
		{
		case 0:
			Pl.SendDA(0, 256, 256, new byte[256]);
			break;
		case 3:
			Pl.ResetToBrom();
			break;
		case 1:
			Pl.Read32(0, 256);
			break;
		case 2:
		{
			List<byte> list = new List<byte>();
			list.AddRange(new byte[8] { 0, 1, 159, 229, 16, 255, 47, 225 });
			list.AddRange(new byte[272]);
			if (Pl.SendDA(0, list.Count, 0, list.ToArray()))
			{
				Pl.JumpDA(mode);
				byte[] array = SerialMTK.Read(1);
				if (Binary.ReadU16LE(array) != 192)
				{
				}
			}
			break;
		}
		}
	}

	public bool BypassSecurity()
	{
		int reset = 0;
		string msg = "Bypass Security... ";
		string msg2 = "Failed!";
		object status = null;
		string msg3 = "OK";
		try
		{
			if (Crasher(2))
			{
				if (Pl.cfg.IsBROM && Pl.cfg.Chipcfg.Damode != (DAmodes)6)
				{
					UI.RichLogs(msg, UI.log);
					if (!Kamakiri2.RunPayload((string)Pl.cfg.Chipcfg.Loader))
					{
						UI.RichLogs(msg2, UI.error, nextline: true);
						return false;
					}
					if (!SerialMTK.Handshake((byte[])status))
					{
						UI.RichLogs(msg2, UI.error, nextline: true);
						DaLoader.Patch = false;
						return false;
					}
					DaLoader.Patch = true;
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				}
				return true;
			}
			return false;
		}
		finally
		{
			LibUsb.Close((byte)reset != 0);
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
