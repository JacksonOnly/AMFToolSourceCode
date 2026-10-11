using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AMFTool.Library.Mediatek.Config;

namespace AMFTool.Library.Mediatek.Download_Agent;

public class DAConfig
{
	public byte[] Emi;

	public int EmiVer;

	public MtkClass Mtk;

	public Storage Storage;

	public MtkConfig Config;

	public long FlashSize;

	public long rpmbsize;

	public long Boot1Size;

	public long Boot2Size;

	public string FlashType;

	public long UartLogLevl;

	public int PageSize;

	public DA Da;

	public byte[] da1;

	public byte[] da2;

	public byte[] da3;

	public Dictionary<int, List<DA>> dasetup;

	public string Loader;

	public byte[] DownloadAgent;

	public DaEntry DaEntry;

	public byte[][] da_pl;

	public DAConfig(MtkConfig cfg)
	{
		string flashType = "emmc";
		object obj = null;
		da_pl = new byte[3][];

		Config = cfg;
		FlashSize = 0L;
		rpmbsize = 0L;
		Boot1Size = 0L;
		Boot2Size = 0L;
		FlashType = flashType;
		PageSize = 512;
		Da = (DA)obj;
		da1 = (byte[])obj;
		da2 = (byte[])obj;
		da3 = (byte[])obj;
		dasetup = (Dictionary<int, List<DA>>)obj;
		EmiVer = 0;
		Emi = (byte[])obj;
		UartLogLevl = cfg.UartLogLevel;
		Loader = string.Empty;
		DownloadAgent = (byte[])obj;
		Storage = new Storage(Mtk, this);
	}

	public string Daname(byte[] da)
	{
		string oldValue = "\0\0";
		string newValue = "";
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(da));
		byte[] array = new byte[79];
		binaryReader.Read(array, 0, array.Length);
		return Encoding.UTF8.GetString(array.Skip(32).ToArray()).Replace(oldValue, newValue).TrimEnd(default(char));
	}

	private (int, byte[]) MExtractEMI(byte[] data)
	{
		string s = "MMM\u00018\0\0\0";
		string s2 = "MTK_BLOADER_INFO_v";
		object item = null;
		string s3 = "MTK_BIN";
		int num = data.IndexOf(Encoding.ASCII.GetBytes(s));
		if (num != -1)
		{
			data = data.Skip(num).ToArray();
			int num2 = BitConverter.ToInt32(data.Skip(32).Take(4).ToArray(), 0);
			int num3 = BitConverter.ToInt32(data.Skip(44).Take(4).ToArray(), 0);
			data = data.Take(num2 - num3).ToArray();
			int num4 = BitConverter.ToInt32(data.Skip(data.Length - 4).ToArray(), 0);
			if (num4 == 0)
			{
				data = data.Take(data.Length - 2048).ToArray();
				num4 = BitConverter.ToInt32(data.Skip(data.Length - 4).ToArray(), 0);
			}
			data = data.Skip(data.Length - num4 - 4).Take(num4).ToArray();
		}
		byte[] bytes = Encoding.ASCII.GetBytes(s2);
		int num5 = bytes.Length;
		num = data.IndexOf(bytes);
		if (num != -1)
		{
			if (num == 0 && Config.Chipcfg.Damode == DAmodes.XFLASH)
			{
				int item2 = int.Parse(Encoding.ASCII.GetString(data.Skip(num + num5).Take(2).ToArray()).TrimEnd(default(char)));
				return (item2, data);
			}
			if (data.IndexOf(Encoding.ASCII.GetBytes(s3)) != -1)
			{
				byte[] item3 = data.Skip(data.IndexOf(Encoding.ASCII.GetBytes(s3)) + 12).ToArray();
				int item4 = int.Parse(Encoding.ASCII.GetString(data.Skip(num + num5).Take(2).ToArray()).TrimEnd(default(char)));
				return (item4, item3);
			}
			return (0, (byte[])item);
		}
		return (0, (byte[])item);
	}

	public byte[] ExtractEMI(byte[] preloader)
	{
		int emiVer = 0;
		object emi = null;
		try
		{
			(EmiVer, Emi) = MExtractEMI(preloader);
		}
		catch (Exception)
		{
			EmiVer = emiVer;
			Emi = (byte[])emi;
		}
		return preloader;
	}

	public (string, byte[]) MExtractLoader(byte[] dataBytes, string emiName = "")
	{
		object obj = null;
		string s = "MTK_BLOADER_INFO";
		if (dataBytes != obj)
		{
			byte[] array = new byte[dataBytes.Length];
			int num = dataBytes.IndexOf(new byte[8] { 77, 77, 77, 1, 56, 0, 0, 0 });
			if (num != -1)
			{
				dataBytes = dataBytes.Skip(num).ToArray();
				int count = Binary.ReadI32LE(dataBytes, 32);
				array = dataBytes.Take(count).ToArray();
				num = dataBytes.IndexOf(Encoding.ASCII.GetBytes(s));
				if (num != -1)
				{
					emiName = Encoding.ASCII.GetString(dataBytes.Skip(num + 27).Take(48).ToArray()).TrimEnd(default(char));
					return (emiName, array);
				}
				return (string.Empty, new byte[0]);
			}
			num = dataBytes.IndexOf(Encoding.ASCII.GetBytes(s));
			if (num != -1)
			{
				emiName = Encoding.ASCII.GetString(dataBytes.Skip(num + 27).Take(48).ToArray()).TrimEnd(default(char));
				return (string.Empty, new byte[0]);
			}
			return (string.Empty, new byte[0]);
		}
		return (string.Empty, new byte[0]);
	}

	public void SetLoader(List<byte[]> loaders)
	{
		string text = "Setup : ";
		dasetup = new Dictionary<int, List<DA>>();
		for (int i = 0; i < loaders.Count; i++)
		{
			if (ParseDALoader(loaders[i], dasetup))
			{
				Console.WriteLine(text + Daname(loaders[i]));
			}
		}
	}

	private bool ParseDALoader(byte[] loader, Dictionary<int, List<DA>> dasetup)
	{
		int num = 0;
		string value = "MTK_DA_v6";
		int num2 = 108;
		int num3 = 220;
		int num4 = 1;
		try
		{
			Stream input = new MemoryStream(loader);
			using BinaryReader binaryReader = new BinaryReader(input);
			byte[] array = new byte[104];
			binaryReader.Read(array, 0, array.Length);
			int num5 = Binary.ReadI32LE(binaryReader.ReadBytes(4));
			bool v = Encoding.ASCII.GetString(array).Contains(value);
			for (int i = 0; i < num5; i += num4)
			{
				binaryReader.BaseStream.Seek(num2 + i * num3, (SeekOrigin)num);
				DA dA = new DA(binaryReader.ReadBytes(num3), (byte)num != 0);
				dA.SetDownloadAgent(loader);
				dA.v6 = v;
				if ((dasetup.ContainsKey(dA.Hwcode) ? 1 : 0) == num)
				{
					if ((uint)dA.Hwcode > (uint)num)
					{
						dasetup[dA.Hwcode] = new List<DA> { dA };
					}
					continue;
				}
				bool flag = (byte)num != 0;
				foreach (DA item in dasetup[dA.Hwcode])
				{
					if ((dA.HwVersion == item.HwVersion && dA.SwVersion == item.SwVersion && dA.HwSubCode == item.HwSubCode) ? ((uint)dA.HwSubCode > (uint)num) : ((byte)num != 0))
					{
						flag = (byte)num4 != 0;
						break;
					}
				}
				if ((!flag) ? ((uint)dA.Hwcode > (uint)num) : ((byte)num != 0))
				{
					dasetup[dA.Hwcode].Add(dA);
				}
			}
			return (byte)num4 != 0;
		}
		catch
		{
			return (byte)num != 0;
		}
	}

	public bool ParseDAPreloader(string soc, byte[] data = null)
	{
		int result = 0;
		string text = "MTK/";
		string text2 = "/";
		string text3 = "_";
		string text4 = ".bin";
		string oldValue = "MT";
		string newValue = "";
		string oldValue2 = "mt";
		try
		{
			data = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + soc.ToLower() + text4);
			if (data != null && data.Length != 0)
			{
				byte[] array = data.ToArray();
				DaHeader daHeader = default(DaHeader);
				DaEntry daEntry = default(DaEntry);
				DaEntry = default(DaEntry);
				int num = Marshal.SizeOf(typeof(DaHeader));
				daHeader = (DaHeader)utils.BytesToStuct(utils.GetFromIndex(array, 0, num), typeof(DaHeader));
				array = array.Skip(num).ToArray();
				for (uint num2 = 0u; num2 < daHeader.Count; num2++)
				{
					num = Marshal.SizeOf(typeof(DaEntry));
					daEntry = (DaEntry)utils.BytesToStuct(utils.GetFromIndex(array, 0, num), typeof(DaEntry));
					array = array.Skip(num).ToArray();
					uint num3 = Convert.ToUInt32(soc.Replace(oldValue, newValue).Replace(oldValue2, newValue), 16);
					if (daEntry.HwCode == num3)
					{
						DaEntry = daEntry;
						for (int i = 1; i < daEntry.RegionCount; i++)
						{
							da_pl[i - 1] = data.Skip(daEntry.MGeometries[i].Offset).Take(daEntry.MGeometries[i].Length).ToArray();
						}
						return true;
					}
				}
				return true;
			}
			return false;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public DA Setup(List<byte[]> da)
	{
		int num = 0;
		object obj = null;
		int num2 = 25185;
		string value = "No da_loader config set up";
		SetLoader(da);
		int num3 = (int)Config.Chipcfg.Dacode;
		if (dasetup.ContainsKey(num3))
		{
			List<DA> list = dasetup[num3];
			foreach (DA item in list)
			{
				if (item.HwVersion <= Config.HWVer && item.SwVersion <= Config.SWVer && Da == obj)
				{
					if (item.v6)
					{
						Config.Chipcfg.Damode = (DAmodes)6;
					}
					Da = item;
					DownloadAgent = item.DownloadAgent;
				}
			}
		}
		if ((Da != null) ? ((byte)num != 0) : (((num3 == num2) ? 1 : 0) == num))
		{
			Console.WriteLine(value);
		}
		return Da;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
