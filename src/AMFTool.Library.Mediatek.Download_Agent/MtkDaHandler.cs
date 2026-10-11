using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AMFTool.Library.Mediatek.Config;

namespace AMFTool.Library.Mediatek.Download_Agent;

public class MtkDaHandler
{
	private MtkClass Mtk;

	public MtkDaHandler(MtkClass mtk)
	{
		Mtk = mtk;
	}

	public (byte[], string) DumpPreloaderRam()
	{
		object obj = null;
		string s = "MMM\u00018\0\0\0";
		string s2 = "MTK_BLOADER_INFO";
		string name = "PRELOADER";
		string text = "Successfully Extracted preloader for this device to ";
		List<int> list = Mtk.Pl.Read32(2097152, 16384);
		if (list == obj)
		{
			return ((byte[])obj, string.Empty);
		}
		byte[] array = list.SelectMany((int val) => BitConverter.GetBytes(val)).ToArray();
		int num = array.IndexOf(Encoding.ASCII.GetBytes(s));
		if (num != -1)
		{
			array = array.Skip(num).ToArray();
			int num2 = BitConverter.ToInt32(array, 32);
			array = new byte[0];
			int num3 = num;
			list = Mtk.Pl.Read32(2097152 + num, 3500);
			if (list == obj)
			{
				return ((byte[])obj, string.Empty);
			}
			array = array.Concat(list.SelectMany((int val) => BitConverter.GetBytes(val))).ToArray();
			num += 14000;
			UI.Progresbar1(num - num3, num2);
			bool flag = num - num3 > num2;
			int num4 = 1447530876;
			while (true)
			{
				_ = num4 ^ 0x714CB5A7;
				if (!flag)
				{
					list = Mtk.Pl.Read32(2097152 + num, 3500);
					if (list == obj)
					{
						break;
					}
					array = array.Concat(list.SelectMany((int val) => BitConverter.GetBytes(val))).ToArray();
					num += 14000;
					UI.Progresbar1(num - num3, num2);
					flag = num - num3 > num2;
					num4 = 1447530876;
					continue;
				}
				byte[] array2 = array.Take(num2).ToArray();
				num = array.IndexOf(Encoding.ASCII.GetBytes(s2));
				if (num != -1)
				{
					string text2 = Encoding.ASCII.GetString(array.Skip(num + 27).Take(48).ToArray()).TrimEnd(default(char));
					if (string.IsNullOrWhiteSpace(text2))
					{
						return (array2, text2);
					}
					string text3 = Path.Combine(utils.CheckBackupFolder(name), text2);
					File.WriteAllBytes(text3, array2);
					Console.WriteLine(text + text3);
					return (array2, text2);
				}
				return ((byte[])obj, string.Empty);
			}
			return ((byte[])obj, string.Empty);
		}
		return ((byte[])obj, string.Empty);
	}

	public bool ConfigureDA(byte[] preloader = null)
	{
		string value = "Custum EMI";
		string msg = "Using Selected EMI";
		string value2 = "SERVER EMI";
		string value3 = "DUMP EMI";
		if (Mtk.Pl.Init())
		{
			Mtk.DaLoader.Patch = false;
			if (!Mtk.BypassSecurity())
			{
				return false;
			}
			if (Mtk.Config.IsBROM && Mtk.Config.Chipcfg.Damode != (DAmodes)6)
			{
				if (MTKConfig.SerlEMI)
				{
					if (!MTKConfig.LocalEMI || !File.Exists(MTKConfig.PathPreloader))
					{
						Console.WriteLine(value2);
						preloader = Server.Downloadfile(Server.urlemi);
					}
					else
					{
						Console.WriteLine(value);
						preloader = File.ReadAllBytes(MTKConfig.PathPreloader);
						UI.RichLogs(msg, UI.error, nextline: true, bold: true);
					}
				}
				else
				{
					Console.WriteLine(value3);
					preloader = DumpPreloader();
				}
			}
			Mtk.Config.PreloaderData = preloader;
			State.Data = preloader;
			return Mtk.DaLoader.UploadDA(preloader);
		}
		return false;
	}

	private byte[] DumpPreloader()
	{
		string msg = "Dump Preloader... ";
		string text = "[";
		string text2 = "]";
		string msg2 = "Failed!";
		UI.RichLogs(msg, UI.log);
		var (array, text3) = DumpPreloaderRam();
		if (array == null)
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return Array.Empty<byte>();
		}
		if (array.Length == 0)
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return Array.Empty<byte>();
		}
		UI.RichLogs(text + text3 + text2, UI.info, nextline: true, bold: true);
		return array;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
