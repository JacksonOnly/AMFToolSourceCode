using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using SharpExt4;

namespace AMFTool.Library;

public class Tools
{
	private static readonly byte[] key;

	private static readonly byte[] sign;

	public static byte[] format(string target)
	{
		string text = "frp";
		string text2 = "config";
		string text3 = "persistent";
		string text4 = "userdata";
		string text5 = "misc";
		string text6 = "para";
		string s = "boot-recovery";
		string s2 = "recovery";
		string s3 = "--wipe_data";
		string s4 = "--reason=MasterClearConfirm";
		string s5 = "--locale=en-US";
		object result = null;
		bool flag;
		if (target == text)
		{
			flag = true;
		}
		else
		{
			if (!(target == text2))
			{
				if (!(flag = target == text3))
				{
					if (target == text4)
					{
						return Enumerable.Repeat((byte)0, 28672).ToArray();
					}
					if (!(target == text5) && !(target == text6))
					{
						return (byte[])result;
					}
					byte[] bytes = Encoding.UTF8.GetBytes(s);
					bytes = bytes.Concat(Enumerable.Repeat((byte)0, 51)).ToArray();
					bytes = bytes.Concat(Encoding.UTF8.GetBytes(s2)).Concat(new byte[1] { 10 }).ToArray();
					bytes = bytes.Concat(Encoding.UTF8.GetBytes(s3)).Concat(new byte[1] { 10 }).ToArray();
					bytes = bytes.Concat(Encoding.UTF8.GetBytes(s4)).Concat(new byte[1] { 10 }).ToArray();
					bytes = bytes.Concat(Encoding.UTF8.GetBytes(s5)).Concat(Enumerable.Repeat((byte)10, 2)).ToArray();
					return bytes.Concat(Enumerable.Repeat((byte)0, 524159).ToArray()).ToArray();
				}
				return Enumerable.Repeat((byte)0, 512).ToArray();
			}
			flag = true;
		}
		if (!flag)
		{
			if (target == text4)
			{
				return Enumerable.Repeat((byte)0, 28672).ToArray();
			}
			if (!(target == text5) && !(target == text6))
			{
				return (byte[])result;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			bytes = bytes.Concat(Enumerable.Repeat((byte)0, 51)).ToArray();
			bytes = bytes.Concat(Encoding.UTF8.GetBytes(s2)).Concat(new byte[1] { 10 }).ToArray();
			bytes = bytes.Concat(Encoding.UTF8.GetBytes(s3)).Concat(new byte[1] { 10 }).ToArray();
			bytes = bytes.Concat(Encoding.UTF8.GetBytes(s4)).Concat(new byte[1] { 10 }).ToArray();
			bytes = bytes.Concat(Encoding.UTF8.GetBytes(s5)).Concat(Enumerable.Repeat((byte)10, 2)).ToArray();
			return bytes.Concat(Enumerable.Repeat((byte)0, 524159).ToArray()).ToArray();
		}
		return Enumerable.Repeat((byte)0, 512).ToArray();
	}

	public static (bool, byte[]) FindMiCloudVuln(byte[] input, List<(string, string)> patched)
	{
		bool item = false;
		foreach (var item2 in patched)
		{
			int num = input.IndexOf(Encoding.UTF8.GetBytes(item2.Item1));
			if (num != -1)
			{
				(item, input) = input.patch(Encoding.UTF8.GetBytes(item2.Item1), Encoding.UTF8.GetBytes(item2.Item2));
			}
		}
		return (item, input);
	}

	public static byte[] PatchVBMeta(byte[] vbmeta, int vbmode)
	{
		vbmeta = vbmeta.ToArray();
		vbmeta[120] = (byte)((vbmode >> 24) & 0xFF);
		vbmeta[121] = (byte)((vbmode >> 16) & 0xFF);
		vbmeta[122] = (byte)((vbmode >> 8) & 0xFF);
		vbmeta[123] = (byte)(vbmode & 0xFF);
		return vbmeta;
	}

	public static byte[] PatchSimLock(byte[] protect1)
	{
		return null;
	}

	public static byte[] PatchSecurityPlugin(byte[] proinfo)
	{
		byte[] array = proinfo.ToArray();
		Array.Clear(array, 2752512, 1872);
		if (array[2754550] != 0)
		{
			return array;
		}
		array[2754550] = 58;
		return array;
	}

	public static byte[] PatchPayjoy(byte[] proinfo)
	{
		string hex = "25 55 7A 0D 06 00 00 00 58 01 00 00 75 71 39 6F 71 72 4A 77 7A 2F 43 34 46 38 56 67 30 4A 57 6A 4C 64 4A 4C 54 55 75 44 38 2F 4A 77 5A 4F 30 57 49 6F 31 36 30 4C 58 35 73 6E 48 58 78 31 30 59 35 5A 4D 65 34 50 68 4A 66 4B 56 58 37 47 4E 70 6C 63 36 70 55 55 4F 55 71 49 70 53 62 58 37 49 54 6E 4D 64 56 46 4B 7A 30 57 62 68 5A 4A 42 6A 36 69 6A 50 59 64 74 69 77 41 2F 4A 52 6C 78 57 4B 64 32 68 34 56 6B 6F 39 55 4D 65 75 52 30 36 41 30 75 4B 70 2F 4C 71 52 6B 65 6D 35 54 42 48 49 62 55 4D 5A 51 32 4B 4C 73 6B 71 57 50 6B 35 7A 72 50 70 42 77 64 4A 77 41 48 72 53 6B 4B 51 75 4B 46 53 52 66 30 37 37 68 4B 67 6D 50 4E 34 74 4F 38 41 63 32 4F 6B 62 67 57 4D 75 74 30 51 4D 42 6D 6B 44 37 4E 2B 6F 30 58 50 77 7A 53 64 36 61 64 79 6A 2B 42 72 75 79 73 64 33 49 6E 32 6A 56 44 78 4B 65 2F 6E 36 51 4A 66 61 2F 74 4A 77 71 54 74 43 75 69 71 71 62 63 6F 49 4C 54 32 6C 51 4F 4B 61 4D 4E 75 31 71 55 39 51 7A 52 6F 77 58 4E 37 79 73 76 56 65 68 58 62 69 4E 4E 48 6B 67 43 38 49 77 41 4A 65 4B 65 4F 78 76 78 56 48 34 35 5A 53 57 53 63 54 68 6B 47 75 47 62 66 33 41 3D 3D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 36 35 30 61 37 35 32 61 31 61 37 64 66 66 64 38 64 33 32 36 34 64 37 39 63 61 36 61 34 33 32 37 36 35 30 33 34 63 36 63 35 66 38 66 34 65 33 62 64 61 62 65 36 38 65 62 65 63 33 32 63 39 31 65 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
		proinfo.ToArray();
		byte[] sourceArray = hex.HexToBytes();
		Array.Copy(sourceArray, 0, proinfo, 2228736, 1040);
		return proinfo;
	}

	public static byte[] PatchMd1img(byte[] data)
	{
		int num = data.IndexOf(new byte[5] { 197, 100, 2, 106, 6 });
		byte[] array2;
		if (num != -1)
		{
			byte[] array = new byte[5] { 32, 232, 1, 106, 6 };
			Buffer.BlockCopy(array, 0, data, num, array.Length);
			num = data.IndexOf(new byte[7] { 255, 48, 145, 32, 232, 0, 106 });
			if (num == -1)
			{
				return data;
			}
			array2 = new byte[7] { 255, 48, 145, 32, 232, 1, 106 };
			Buffer.BlockCopy(array2, 0, data, num, array2.Length);
			return data;
		}
		num = data.IndexOf(new byte[7] { 255, 48, 145, 32, 232, 0, 106 });
		if (num == -1)
		{
			return data;
		}
		array2 = new byte[7] { 255, 48, 145, 32, 232, 1, 106 };
		Buffer.BlockCopy(array2, 0, data, num, array2.Length);
		return data;
	}

	public static byte[] patchlk(byte[] dadata)
	{
		string s = "dm-verity corruption";
		string s2 = "Your device is corrupt.";
		string s3 = "Your device download is not complete.";
		string s4 = "It can't be trusted and may not Or, device will power\u00a0off\u00a0in\u00a05s";
		string s5 = "Press power button to continue.";
		string s6 = "Or, device will\u00a0pover\u00a0off\u00a0in\u00a05s";
		string s7 = "It can't be trusted and may not";
		string s8 = "Press power button to continue";
		string s9 = "Or, device will power off in 5s";
		string s10 = "Or, device will power off\u00a0in\u00a05s";
		string s11 = "Or, device will power off\u00a0in";
		string s12 = "Or, device will power off in %ds";
		string s13 = "%ds";
		string s14 = "Download error";
		string s15 = "TOOL DL image Fail!";
		string s16 = "Please check the flashinfo partition for details.";
		string s17 = "Orange State";
		string s18 = "Your device has been unlocked and can't be trusted";
		string s19 = "Your device will boot in 5 seconds";
		string s20 = "Your device will shutdown in 5 seconds";
		string s21 = "Your device shutdown failed";
		string s22 = "Red State";
		string s23 = "Your device has failed verification and may not";
		string s24 = "work properly";
		string s25 = "No-Efuse State";
		string s26 = "Your software is efuse, but your device is no-efuse";
		string s27 = "Please efuse your device";
		string s28 = "Your device has been unlocked And can't be trusted";
		string s29 = "dm-verity error";
		string s30 = "Android system on your device is corrupted";
		string s31 = "Please confirm whether you want to boot in";
		string s32 = "EIO mode or RESTART mode";
		string s33 = "System will boot with";
		string s34 = "RESTART mode automatically after 30 seconds";
		string s35 = "(Volume UP)   : EIO mode";
		string s36 = "(Volume Down) : RESTART mode";
		int num = 0;
		byte[] array = dadata.ToArray();
		List<byte[]> list = new List<byte[]>
		{
			Encoding.UTF8.GetBytes(s),
			Encoding.UTF8.GetBytes(s2),
			Encoding.UTF8.GetBytes(s3),
			Encoding.UTF8.GetBytes(s4),
			Encoding.UTF8.GetBytes(s4),
			Encoding.UTF8.GetBytes(s5),
			Encoding.UTF8.GetBytes(s6),
			Encoding.UTF8.GetBytes(s7),
			Encoding.UTF8.GetBytes(s8),
			Encoding.UTF8.GetBytes(s9),
			Encoding.UTF8.GetBytes(s10),
			Encoding.UTF8.GetBytes(s11),
			Encoding.UTF8.GetBytes(s12),
			Encoding.UTF8.GetBytes(s13),
			Encoding.UTF8.GetBytes(s14),
			Encoding.UTF8.GetBytes(s15),
			Encoding.UTF8.GetBytes(s3),
			Encoding.UTF8.GetBytes(s16),
			Encoding.UTF8.GetBytes(s17),
			Encoding.UTF8.GetBytes(s18),
			Encoding.UTF8.GetBytes(s19),
			Encoding.UTF8.GetBytes(s20),
			Encoding.UTF8.GetBytes(s21),
			Encoding.UTF8.GetBytes(s22),
			Encoding.UTF8.GetBytes(s23),
			Encoding.UTF8.GetBytes(s24),
			Encoding.UTF8.GetBytes(s25),
			Encoding.UTF8.GetBytes(s26),
			Encoding.UTF8.GetBytes(s27),
			Encoding.UTF8.GetBytes(s22),
			Encoding.UTF8.GetBytes(s23),
			Encoding.UTF8.GetBytes(s24),
			Encoding.UTF8.GetBytes(s25),
			Encoding.UTF8.GetBytes(s26),
			Encoding.UTF8.GetBytes(s27),
			Encoding.UTF8.GetBytes(s17),
			Encoding.UTF8.GetBytes(s28),
			Encoding.UTF8.GetBytes(s18),
			Encoding.UTF8.GetBytes(s29),
			Encoding.UTF8.GetBytes(s30),
			Encoding.UTF8.GetBytes(s31),
			Encoding.UTF8.GetBytes(s32),
			Encoding.UTF8.GetBytes(s33),
			Encoding.UTF8.GetBytes(s34),
			Encoding.UTF8.GetBytes(s35),
			Encoding.UTF8.GetBytes(s36),
			Encoding.UTF8.GetBytes(s20),
			Encoding.UTF8.GetBytes(s21),
			Encoding.UTF8.GetBytes(s22),
			Encoding.UTF8.GetBytes(s23),
			Encoding.UTF8.GetBytes(s24),
			Encoding.UTF8.GetBytes(s25),
			Encoding.UTF8.GetBytes(s26),
			Encoding.UTF8.GetBytes(s27),
			Encoding.UTF8.GetBytes(s),
			Encoding.UTF8.GetBytes(s2),
			Encoding.UTF8.GetBytes(s3),
			Encoding.UTF8.GetBytes(s4),
			Encoding.UTF8.GetBytes(s4),
			Encoding.UTF8.GetBytes(s6),
			Encoding.UTF8.GetBytes(s7),
			Encoding.UTF8.GetBytes(s8),
			Encoding.UTF8.GetBytes(s9),
			Encoding.UTF8.GetBytes(s10),
			Encoding.UTF8.GetBytes(s11),
			Encoding.UTF8.GetBytes(s12),
			Encoding.UTF8.GetBytes(s13),
			Encoding.UTF8.GetBytes(s14),
			Encoding.UTF8.GetBytes(s15),
			Encoding.UTF8.GetBytes(s3),
			Encoding.UTF8.GetBytes(s16)
		};
		int num2 = 0;
		foreach (byte[] item in list)
		{
			int num3 = IndexOf(array, item.ToArray());
			if (num3 != -1)
			{
				byte[] array2 = Enumerable.Repeat((byte)0, item.Length).ToArray();
				Array.Copy(array2, 0, array, num3, array2.Length);
				num2++;
			}
		}
		if (num2 != num)
		{
			return array;
		}
		return dadata;
	}

	public static int IndexOf(byte[] data, byte[] pattern, int index = 0)
	{
		int i = index;
		for (int num = data.Length - pattern.Length + 1 - 1; i <= num; i++)
		{
			bool flag = true;
			int j = 0;
			for (int num2 = pattern.Length - 1; j <= num2; j++)
			{
				if (data[i + j] != pattern[j])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return i;
			}
		}
		return -1;
	}

	public static bool InsertLD0BOppo(string path, byte[] data)
	{
		string text = "/md/NVRAM/NVD_IMEI/FILELIST";
		string text2 = "/md/NVRAM/NVD_IMEI/LD0B_001";
		string text3 = "/md/NVRAM/CALIBRAT/OEM1_000";
		string text4 = "/md/NVRAM/CALIBRAT/ULT1_000";
		string text5 = "/md/NVRAM/INFO_FILE";
		ExtDisk val = ExtDisk.Open(path);
		try
		{
			ExtFileSystem val2 = ExtFileSystem.Open(val);
			try
			{
				string text6 = text;
				string text7 = text2;
				if (val2.FileExists(text6))
				{
					if (val2.FileExists(text7))
					{
						val2.DeleteFile(text7);
					}
					val2.RenameFile(text6, text7);
				}
				string text8 = text3;
				string text9 = text4;
				if (val2.FileExists(text8))
				{
					if (val2.FileExists(text9))
					{
						val2.DeleteFile(text9);
					}
					val2.RenameFile(text8, text9);
				}
				string text10 = text5;
				if (val2.FileExists(text10))
				{
					val2.DeleteFile(text10);
				}
				ExtFileStream val3 = val2.OpenFile(text7, FileMode.Create, FileAccess.Write);
				try
				{
					((Stream)(object)val3).Write(data, 0, data.Length);
					return true;
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static bool InsertLD0BXiaomi(string path, byte[] data)
	{
		string text = "/md/NVRAM/INFO_FILE";
		int num = 3;
		int num2 = 2;
		int num3 = 0;
		int num4 = 8;
		string text2 = "/md/NVRAM/NVD_IMEI/CSSD_000";
		int num5 = 72;
		int num6 = 64;
		int num7 = 1;
		int num8 = 239;
		int num9 = 205;
		string text3 = "/md/NVRAM/NVD_IMEI/LD0B_001";
		string msg = "failed";
		try
		{
			ExtDisk val = ExtDisk.Open(path);
			try
			{
				ExtFileSystem val2 = ExtFileSystem.Open(val);
				try
				{
					if (val2.FileExists(text))
					{
						ExtFileStream val3 = val2.OpenFile(text, FileMode.Open, FileAccess.Write);
						try
						{
							((Stream)(object)val3).Seek(0L, SeekOrigin.Begin);
							((Stream)(object)val3).Write(new byte[8], 0, 8);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					string text4 = text2;
					if (val2.FileExists(text4))
					{
						ExtFileStream val4 = val2.OpenFile(text4, (FileMode)num, (FileAccess)num2);
						try
						{
							int num10 = (int)((Stream)(object)val4).Length;
							if (num10 > num5)
							{
								((Stream)(object)val4).Seek((long)num6, (SeekOrigin)num3);
								byte[] array = new byte[num10 - num5 + num7];
								((Stream)(object)val4).Write(array, num3, array.Length);
								((Stream)(object)val4).Seek((long)(num10 - num4), (SeekOrigin)num3);
								byte[] array2 = new byte[num4];
								array2[num3] = (byte)num8;
								array2[num7] = (byte)num9;
								((Stream)(object)val4).Write(array2, num3, num4);
							}
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					string text5 = text3;
					ExtFileStream val5 = val2.OpenFile(text5, (FileMode)num2, (FileAccess)num2);
					try
					{
						((Stream)(object)val5).Write(data, num3, data.Length);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
					return (byte)num7 != 0;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
			UI.RichLogs(msg, Color.Red, (byte)num7 != 0, (byte)num7 != 0);
			return (byte)num3 != 0;
		}
	}

	public static byte[] BuildLD0B(byte[] imei1, byte[] imei2)
	{
		string imei3 = "000000000000000";
		object result = null;
		string hex = "4c44490010ef0a000a0000000a40000020000000000000000000a2440000000000000000000001000000000000000000000000000000";
		string hex2 = "4ab96d2f1a7925c700db7e7f217756793f086badd56d9fa38ebf8856d4aea0a7";
		try
		{
			if (imei1 == null)
			{
				imei1 = IMEIEncrypt(imei3);
			}
			if (imei2 == null)
			{
				imei2 = IMEIEncrypt(imei3);
			}
			if (imei1.Length == 32 && imei2.Length == 32)
			{
				byte[] array = hex.HexToBytes();
				byte[] array2 = hex2.HexToBytes();
				using MemoryStream memoryStream = new MemoryStream();
				memoryStream.Write(array, 0, array.Length);
				memoryStream.Write(imei2, 0, 32);
				memoryStream.Write(imei2, 0, 32);
				for (int i = 0; i < 8; i++)
				{
					memoryStream.Write(array2, 0, array2.Length);
				}
				return memoryStream.ToArray();
			}
			return (byte[])result;
		}
		catch
		{
			return (byte[])result;
		}
	}

	public static byte[] BuildNVRAM(byte[] nvram, string filepath)
	{
		int num = 0;
		string text = "/";
		string text2 = "*";
		int num2 = 1;
		object obj = null;
		string text3 = "/mnt/vendor/nvdata";
		string text4 = "/vendor/nvdata";
		string text5 = "/APCFG/APRDEB/WIFI";
		string text6 = "00";
		int num3 = 128;
		int count = nvram.IndexOf(Enumerable.Repeat(byte.MaxValue, 16).ToArray());
		byte[] array = nvram.Take(count).ToArray();
		ExtDisk val = ExtDisk.Open(filepath);
		string[] files;
		try
		{
			ExtFileSystem val2 = ExtFileSystem.Open(val);
			try
			{
				files = val2.GetFiles(text, text2, SearchOption.AllDirectories);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (files == obj)
		{
			return (byte[])obj;
		}
		string text7 = ((Extentions.IndexOf(array, Encoding.UTF8.GetBytes(text3), num) > num) ? text3 : text4);
		using BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
		for (int i = num; i < files.Length; i += num2)
		{
			UI.Progresbar1(i, files.Length);
			string text8 = text7 + files[i];
			Console.Write(text8);
			byte[] array2 = ((!(text8 == text7 + text5)) ? Encoding.UTF8.GetBytes(text8) : (Encoding.UTF8.GetBytes(text8).ToHexString() + text6).HexToBytes());
			int num4 = Extentions.IndexOf(array, array2, num);
			if (num4 > num)
			{
				int num5 = Math.Max(num, num3 - array2.Length);
				binaryWriter.BaseStream.Seek(num4, (SeekOrigin)num);
				binaryWriter.Write(array2);
				binaryWriter.Write(new byte[num5]);
			}
		}
		return array;
	}

	public static bool RebuildAllMap(string path)
	{
		string text = "AllMap";
		int num = 0;
		int num2 = 3;
		int result = 1;
		object obj = null;
		int num3 = 2;
		try
		{
			string text2 = text;
			ExtDisk val = ExtDisk.Open(path);
			try
			{
				ExtFileSystem val2 = ExtFileSystem.Open(val);
				try
				{
					if (!val2.FileExists(text2))
					{
						return false;
					}
					ExtFileStream val3 = val2.OpenFile(text2, FileMode.Open, FileAccess.Read);
					byte[] array;
					try
					{
						array = new byte[((Stream)(object)val3).Length];
						((Stream)(object)val3).Read(array, 0, array.Length);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
					array = GenerateMap(array, path);
					if (array == obj)
					{
						return (byte)num != 0;
					}
					ExtFileStream val4 = val2.OpenFile(text2, (FileMode)num2, (FileAccess)num3);
					try
					{
						((Stream)(object)val4).Write(array, num, array.Length);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
					return (byte)result != 0;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
			return (byte)num != 0;
		}
	}

	public static byte[] GenerateMap(byte[] map, string path)
	{
		string text = "/";
		string text2 = "*";
		object result = null;
		string text3 = "/mnt/vendor/nvdata";
		string text4 = "/vendor/nvdata";
		string text5 = "/APCFG/APRDEB/WIFI";
		string text6 = "00";
		ExtDisk val = ExtDisk.Open(path);
		try
		{
			ExtFileSystem val2 = ExtFileSystem.Open(val);
			try
			{
				string[] files = val2.GetFiles(text, text2, SearchOption.AllDirectories);
				if (files != null && files.Length != 0)
				{
					string text7 = ((map.IndexOf(Encoding.UTF8.GetBytes(text3)) <= 0) ? text4 : text3);
					using BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(map));
					int num = files.Length;
					int num2 = 0;
					string[] array = files;
					foreach (string text8 in array)
					{
						UI.Progresbar1(num2++, num);
						string text9 = text7 + text8;
						byte[] array2 = ((!(text9 == text7 + text5)) ? Encoding.UTF8.GetBytes(text9) : (Encoding.UTF8.GetBytes(text9).ToString() + text6).HexToBytes());
						int num3 = map.IndexOf(array2);
						if (num3 > 0)
						{
							int num4 = Math.Max(0, 128 - array2.Length);
							binaryWriter.BaseStream.Seek(num3, SeekOrigin.Begin);
							binaryWriter.Write(array2);
							binaryWriter.Write(new byte[num4]);
						}
					}
					return map;
				}
				return (byte[])result;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static bool ResetCert(string path, byte[] data)
	{
		string text = "AllFile";
		int num = 3;
		int num2 = 1;
		int num3 = 0;
		int num4 = 8;
		int num5 = -1;
		int num6 = 2;
		int num7 = 5;
		string text2 = "/md/NVRAM/NVD_DATA/LD32_000";
		string text3 = "/md/NVRAM/NVD_DATA/DH1M_000";
		string text4 = "/md/NVRAM/NVD_DATA/DH0Y_003";
		string text5 = "/md/NVRAM/NVD_DATA/DH1K_000";
		int num8 = 4;
		string text6 = "/md/NVRAM/INFO_FILE";
		string text7 = "/md/NVRAM/NVD_IMEI/LD0B_001";
		string msg = "failed";
		try
		{
			ExtDisk val = ExtDisk.Open(path);
			try
			{
				ExtFileSystem val2 = ExtFileSystem.Open(val);
				try
				{
					ExtFileStream val3 = val2.OpenFile(text, FileMode.Open, FileAccess.Read);
					byte[] array;
					try
					{
						array = new byte[((Stream)(object)val3).Length];
						((Stream)(object)val3).Read(array, 0, array.Length);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
					byte[] array2 = new byte[num4];
					Buffer.BlockCopy(Convert.FromBase64String("TERJABDvCgA="), 0, array2, 0, Buffer.ByteLength(array2));
					int num9 = Extentions.IndexOf(array, array2, num3);
					if (((num9 == num5) ? 1 : 0) == num3)
					{
						Buffer.BlockCopy(data, num3, array, num9, data.Length);
					}
					ExtFileStream val4 = val2.OpenFile(text, (FileMode)num, (FileAccess)num6);
					try
					{
						((Stream)(object)val4).Write(array, num3, array.Length);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
					string[] array3 = new string[num7];
					array3[num3] = text2;
					array3[num2] = text3;
					array3[num6] = text4;
					array3[num] = text5;
					array3[num8] = text6;
					string[] array4 = array3;
					string[] array5 = array4;
					for (int i = num3; i < array5.Length; i += num2)
					{
						string text8 = array5[i];
						if (val2.FileExists(text8))
						{
							val2.DeleteFile(text8);
						}
					}
					ExtFileStream val5 = val2.OpenFile(text7, (FileMode)num6, (FileAccess)num6);
					try
					{
						((Stream)(object)val5).Write(data, num3, data.Length);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
					return (byte)num2 != 0;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
			UI.RichLogs(msg, Color.Red, (byte)num2 != 0, (byte)num2 != 0);
			return (byte)num3 != 0;
		}
	}

	public static int CheckCertOPPO(byte[] data)
	{
		int num = data.IndexOf(new byte[5] { 197, 100, 2, 106, 6 });
		if (num == -1)
		{
			num = data.IndexOf(new byte[7] { 255, 48, 145, 32, 232, 0, 106 });
			if (num == -1)
			{
				return 0;
			}
			return 1;
		}
		num = data.IndexOf(new byte[7] { 255, 48, 145, 32, 232, 0, 106 });
		if (num == -1)
		{
			return 1;
		}
		return 2;
	}

	public static bool PatchIMEI(byte[] nv, byte[] imei1, byte[] imei2)
	{
		object obj = null;
		if (nv != obj)
		{
			int num = nv.IndexOf(sign);
			if (num != -1)
			{
				if (nv.Length >= num + 64 + 64)
				{
					if (!DeviceState.CheckedIMEI1)
					{
						if (!DeviceState.CheckedIMEI2)
						{
							return true;
						}
						if (imei2 != null && imei2.Length == 32)
						{
							Buffer.BlockCopy(imei2, 0, nv, num + 64 + 32, 32);
							return true;
						}
						return false;
					}
					if (imei1 != null && imei1.Length == 32)
					{
						Buffer.BlockCopy(imei1, 0, nv, num + 64, 32);
						if (!DeviceState.CheckedIMEI2)
						{
							return true;
						}
						if (imei2 != null && imei2.Length == 32)
						{
							Buffer.BlockCopy(imei2, 0, nv, num + 64 + 32, 32);
							return true;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public static byte[] IMEIEncrypt(string imei)
	{
		object result = null;
		if (string.IsNullOrEmpty(imei))
		{
			return (byte[])result;
		}
		string hex = imei_crypto(imei, encrypt: true);
		return hex.HexToBytes();
	}

	public static string[] IMEIDecrypt(byte[] data)
	{
		object obj = null;
		string text = "corrupt";
		if (data != obj)
		{
			int num = data.IndexOf(sign);
			if (num >= 0 && data.Length >= num + 64 + 64)
			{
				byte[] array = new byte[32];
				byte[] array2 = new byte[32];
				Buffer.BlockCopy(data, num + 64, array, 0, 32);
				Buffer.BlockCopy(data, num + 96, array2, 0, 32);
				return new string[2]
				{
					imei_crypto(array.ToHexString(), encrypt: false),
					imei_crypto(array2.ToHexString(), encrypt: false)
				};
			}
			return new string[2] { text, text };
		}
		return new string[2] { text, text };
	}

	public static string imei_crypto(string rawimei, bool encrypt)
	{
		int num = 0;
		int mode = 2;
		int num2 = 1;
		int count = 10;
		int num3 = 15;
		int num4 = 240;
		int num5 = 4;
		if (!encrypt)
		{
			byte[] array = rawimei.HexToBytes();
			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.Mode = CipherMode.ECB;
				aes.Padding = PaddingMode.None;
				ICryptoTransform cryptoTransform = aes.CreateDecryptor();
				array = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
			}
			array = array.Take(count).ToArray();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = num; (i >= array.Length) ? ((byte)num != 0) : (stringBuilder.Length < num3); i += num2)
			{
				byte b = array[i];
				stringBuilder.Append(b & num3);
				if (stringBuilder.Length < num3)
				{
					stringBuilder.Append((b & num4) >> num5);
				}
			}
			return stringBuilder.ToString().Substring(num, num3);
		}
		byte[] array2 = new byte[10];
		for (int j = 0; j < 15; j += 2)
		{
			byte b2 = (byte)(rawimei[j] - 48);
			byte b3 = (byte)((j + 1 >= 15) ? 15u : ((uint)(rawimei[j + 1] - 48)));
			array2[j / 2] = (byte)((b3 << 4) | b2);
		}
		using (MD5 mD = MD5.Create())
		{
			byte[] array3 = mD.ComputeHash(array2);
			byte[] array4 = new byte[8];
			for (int k = 0; k < 8; k++)
			{
				array4[k] = (byte)(array3[k] ^ array3[k + 8]);
			}
			array2 = array2.Concat(array4).ToArray();
			Array.Resize(ref array2, 32);
		}
		using (Aes aes2 = Aes.Create())
		{
			aes2.Key = key;
			aes2.Mode = (CipherMode)mode;
			aes2.Padding = (PaddingMode)num2;
			ICryptoTransform cryptoTransform2 = aes2.CreateEncryptor();
			array2 = cryptoTransform2.TransformFinalBlock(array2, num, array2.Length);
		}
		return array2.ToHexString();
	}

	static Tools()
	{
		string hex = "3f06bd14d45fa985dd027410f0214d22";
		string hex2 = "4C44490010EF0A00";
		key = hex.HexToBytes();
		sign = hex2.HexToBytes();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
