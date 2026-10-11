using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AMFTool.Library.Mediatek.Config;
using AMFTool.Library.Mediatek.Download_Agent.xflash;
using AMFTool.Library.Mediatek.Download_Agent.xflash.extention;
using AMFTool.Library.Mediatek.Download_Agent.xml;
using AMFTool.Library.Mediatek.Download_Agent.xml.extension;
using AMFTool.Library.Usb;

namespace AMFTool.Library.Mediatek.Download_Agent;

public class MtkDaLoader
{
	public DAConfig Dacfg;

	public bool Patch = false;

	public MtkClass Mtk;

	public string PreloaderName;

	public string PreloaderSize;

	public (byte[], Gpt) Partition;

	public DAmodes FlashMode;

	public DAXFlash DAXFlash;

	public DAXML DAXml;

	public XFlashExt XFlashExt;

	public XmlFlashExt XmlExt;

	public static List<byte[]> DownloadAgent;

	public MtkDaLoader()
	{
		PreloaderName = string.Empty;
		PreloaderSize = string.Empty;
		FlashMode = (DAmodes)3;
		DownloadAgent = new List<byte[]>();
	}

	public (int idx, int HashMode, int HashLen) ComputeHashPos(byte[] da1, byte[] da2, int da1SigLen, int da2SigLen, bool v6)
	{
		string value = "Error: Hash computation failed.";
		int num = da2.Length - da2SigLen;
		int num2;
		int item;
		(num2, item) = CalcDAHash(da1, da2.AsSpan(0, num).ToArray());
		if (num2 == -1)
		{
			num = da2.Length;
			(int, int) tuple2 = CalcDAHash(da1, da2);
			num2 = tuple2.Item2;
			(item, _) = tuple2;
			if (num2 != -1)
			{
				return (idx: num2, HashMode: item, HashLen: num);
			}
			if (!v6)
			{
				num = da2.Length - da2SigLen;
				(num2, item) = FindDAHashV5(da1);
				return (idx: num2, HashMode: item, HashLen: num);
			}
			num = da2.Length - da2SigLen;
			(num2, item) = FindDAHashV6(da1, da1SigLen);
			if (num2 != -1)
			{
				return (idx: num2, HashMode: item, HashLen: num);
			}
			Console.WriteLine(value);
			return (idx: -1, HashMode: -1, HashLen: -1);
		}
		return (idx: num2, HashMode: item, HashLen: num);
	}

	public (int Pos, int Mode) FindDAHashV6(byte[] da1, int sigLen)
	{
		int num = da1.Length - sigLen - 48;
		if (num >= 0)
		{
			if (!((ReadOnlySpan<byte>)da1.AsSpan(num, 48)).Slice(44, 4).SequenceEqual(new byte[4] { 0, 0, 0, 0 }))
			{
				return (Pos: -1, Mode: -1);
			}
			return (Pos: num, Mode: 2);
		}
		return (Pos: -1, Mode: -1);
	}

	public (int Pos, int Mode) FindDAHashV5(byte[] da1)
	{
		string s = "MMU MAP: VA";
		string value = "Error: No hash found";
		ReadOnlySpan<byte> span = da1.AsSpan();
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		int num = span.IndexOf(bytes);
		if (num == -1 || num < 48)
		{
			Console.WriteLine(value);
			return (Pos: -1, Mode: -1);
		}
		if (span.Slice(num - 48, 48).Slice(44, 4).SequenceEqual(new byte[4] { 0, 0, 0, 0 }))
		{
			return (Pos: num - 48, Mode: 2);
		}
		return (Pos: num - 20, Mode: 1);
	}

	public (int HashMode, int Idx) CalcDAHash(byte[] da1, byte[] da2)
	{
		int num = 0;
		int num2 = -1;
		int num3 = 1;
		int num4 = 2;
		ReadOnlySpan<byte> span = da1.AsSpan();
		_ = (ReadOnlySpan<byte>)da2.AsSpan();
		byte[] array;
		using (MD5 mD = MD5.Create())
		{
			array = mD.ComputeHash(da2);
		}
		byte[] array2;
		using (SHA1 sHA = SHA1.Create())
		{
			array2 = sHA.ComputeHash(da2);
		}
		byte[] array3;
		using (SHA256 sHA2 = SHA256.Create())
		{
			array3 = sHA2.ComputeHash(da2);
		}
		int num5 = span.IndexOf(array);
		int item = num;
		if (num5 == num2)
		{
			num5 = span.IndexOf(array2);
			item = num3;
			if (num5 == num2)
			{
				num5 = span.IndexOf(array3);
				item = num4;
				if (num5 == num2)
				{
					item = num2;
				}
			}
		}
		return (HashMode: item, Idx: num5);
	}

	public byte[] FixHash(byte[] da1, byte[] da2, int hashPos, int hashMode, int hashLen)
	{
		int num = 0;
		byte[] array = (byte[])da1.Clone();
		int length = Math.Min(hashLen, da2.Length);
		ReadOnlySpan<byte> readOnlySpan = da2.AsSpan(0, length);
		byte[] array2;
		switch (hashMode)
		{
		default:
			return array;
		case 0:
		{
			using (MD5 mD = MD5.Create())
			{
				array2 = mD.ComputeHash(readOnlySpan.ToArray());
			}
			break;
		}
		case 1:
		{
			using (SHA1 sHA2 = SHA1.Create())
			{
				array2 = sHA2.ComputeHash(readOnlySpan.ToArray());
			}
			break;
		}
		case 2:
		{
			using (SHA256 sHA = SHA256.Create())
			{
				array2 = sHA.ComputeHash(readOnlySpan.ToArray());
			}
			break;
		}
		}
		if (((hashPos + array2.Length > array.Length) ? 1 : 0) == num)
		{
			Span<byte> destination = array.AsSpan(hashPos, array2.Length);
			array2.AsSpan().CopyTo(destination);
		}
		return array;
	}

	public (byte[], Gpt) GetGPT(GptSettings settings, string partType = "user")
	{
		object obj = null;
		byte[] array;
		Gpt gpt2;
		if (FlashMode == (DAmodes)6)
		{
			(byte[], object) gpt = DAXml.Partition.GetGpt(settings, partType);
			Partition = (gpt.Item1, (Gpt)gpt.Item2);
			(array, gpt2) = Partition;
			if (array == null && gpt2 == obj)
			{
				return ((byte[])obj, (Gpt)obj);
			}
			return Partition;
		}
		if (FlashMode != DAmodes.XFLASH)
		{
			(array, gpt2) = Partition;
		}
		else
		{
			(byte[], object) gpt = DAXFlash.Partition.GetGpt(settings, partType);
			Partition = (gpt.Item1, (Gpt)gpt.Item2);
			(array, gpt2) = Partition;
		}
		if (array == null && gpt2 == obj)
		{
			return ((byte[])obj, (Gpt)obj);
		}
		return Partition;
	}

	public bool Shutdown()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (FlashMode == (DAmodes)6)
		{
			if (DAXml.Shutdown())
			{
				MTKConfig.Infodevice = true;
				return true;
			}
			return false;
		}
		if (FlashMode == DAmodes.XFLASH)
		{
			if (DAXFlash.Shutdown())
			{
				MTKConfig.Infodevice = true;
				return true;
			}
			return false;
		}
		MTKConfig.Infodevice = true;
		return true;
	}

	private void SetDA()
	{
		FlashMode = (DAmodes)3;
		bool flag;
		if (Mtk.Config.Chipcfg.Damode == DAmodes.XFLASH)
		{
			FlashMode = DAmodes.XFLASH;
			if (!(flag = FlashMode == DAmodes.XFLASH))
			{
				if (FlashMode == (DAmodes)6)
				{
					DAXml = new DAXML(Mtk, Dacfg);
					XmlExt = new XmlFlashExt(Mtk, DAXml);
				}
			}
			else
			{
				DAXFlash = new DAXFlash(Mtk, Dacfg);
				XFlashExt = new XFlashExt(Mtk, DAXFlash);
			}
			return;
		}
		if (Mtk.Config.Chipcfg.Damode == (DAmodes)6)
		{
			FlashMode = (DAmodes)6;
			flag = FlashMode == DAmodes.XFLASH;
		}
		else
		{
			flag = FlashMode == DAmodes.XFLASH;
		}
		if (!flag)
		{
			if (FlashMode == (DAmodes)6)
			{
				DAXml = new DAXML(Mtk, Dacfg);
				XmlExt = new XmlFlashExt(Mtk, DAXml);
			}
		}
		else
		{
			DAXFlash = new DAXFlash(Mtk, Dacfg);
			XFlashExt = new XFlashExt(Mtk, DAXFlash);
		}
	}

	public bool BootTo(long addr, byte[] data)
	{
		object obj = null;
		double field_ = Class38.field_2783;
		if (data != null && data.Length != 0)
		{
			if (DAXFlash ==null)
			{
				if (DAXml !=null)
				{
					return DAXml.BootTo(addr, data);
				}
				return false;
			}
			return DAXFlash.BootTo(addr, data, field_);
		}
		return false;
	}

	public byte[] PatchDA2(byte[] da2)
	{
		if (FlashMode == DAmodes.XFLASH)
		{
			return XFlashExt.PatchDA2(da2);
		}
		if (FlashMode != (DAmodes)6)
		{
			return Array.Empty<byte>();
		}
		return XmlExt.PatchDA2(da2);
	}

	public void LoadLocalDagent()
	{
		string text = "\\download-agent";
		string[] files = Directory.GetFiles(utils.BaseDirectory + text);
		for (int i = 0; i < files.Length; i++)
		{
			byte[] item = File.ReadAllBytes(files[i]);
			DownloadAgent.Add(item);
		}
	}

	public bool UploadDA(byte[] preloader = null)
	{
		string objB = "5";
		string value = "PreLoader";
		string msg = "Downloading resources... ";
		object obj = null;
		string text = "Mediatek/";
		string text2 = "/";
		string text3 = "_MT";
		string text4 = "X";
		string text5 = "_1.bin";
		string msg2 = "Failed!";
		string text6 = "_2.bin";
		string msg3 = "OK";
		string text7 = "Samsung";
		string text8 = ".bin";
		string text9 = "MTK_AllInOne_DA_V1";
		string text10 = "\\download-agent";
		string text11 = "MTK_AllInOne_DA_V2";
		string text12 = "\\download-agent\\MTK_AllInOne_DA_2.bin";
		string msg4 = "DA Not Found!";
		string text13 = "MTK_AllInOne_DA_V3";
		string text14 = "\\download-agent\\MTK_AllInOne_DA.bin";
		string text15 = "MTK_AllInOne_DA_Oppo";
		string text16 = "\\download-agent\\DefaultOppo.bin";
		string text17 = "MTK_AllInOne_DA_Xioami";
		string text18 = "\\download-agent\\DefaultXiaomi.bin";
		string text19 = "MTK_AllInOne_DA_MT_V6";
		string text20 = "\\download-agent\\DefaultV6.bin";
		string objB2 = "6";
		string value2 = "Patch";
		string format = "DA Pacth = {0}";
		string msg5 = "Load DA Failed!";
		string emiName = "";
		string text21 = "Brand : ";
		string text22 = " Model : ";
		string text23 = " GetDA Brom : ";
		string msg6 = "Sending Download-Agent [1]... ";
		DownloadAgent.Clear();
		if (object.Equals(MTKConfig.CpuType, objB))
		{
			if (Portinfo.Name.Contains(value))
			{
				if (MTKConfig.MTKServer)
				{
					UI.RichLogs(msg, UI.log);
					if (!MTKConfig.Danofarse)
					{
						if (!(DeviceState.Brand == text7))
						{
							if (!MTKConfig.Customda || !MTKConfig.MTKServer)
							{
								byte[] array = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text4).ToLower() + text8);
								if (array == null || array.Length == 0)
								{
									UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
									return false;
								}
								DownloadAgent.Add(array);
								UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
							}
							else
							{
								byte[] array2 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text8);
								if (array2 == null || array2.Length == 0)
								{
									UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
									return false;
								}
								DownloadAgent.Add(array2);
								UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
							}
						}
						else
						{
							byte[] array3 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text8);
							if (array3 == null || array3.Length == 0)
							{
								UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
								return false;
							}
							DownloadAgent.Add(array3);
							UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						}
					}
					else
					{
						Dacfg.da1 = (byte[])obj;
						Dacfg.da2 = (byte[])obj;
						byte[] array4 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text4).ToLower() + text5);
						if (array4 == null || array4.Length == 0)
						{
							UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
							return false;
						}
						Dacfg.da1 = array4;
						byte[] array5 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text4).ToLower() + text6);
						if (array5 == null || array5.Length == 0)
						{
							UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
							return false;
						}
						Dacfg.da2 = array5;
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					}
				}
			}
			else if (!(DeviceState.ModelDa == text9))
			{
				if (DeviceState.ModelDa == text11)
				{
					string path = utils.BaseDirectory + text12;
					if (!File.Exists(path))
					{
						UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
						return false;
					}
					DownloadAgent.Add(File.ReadAllBytes(path));
				}
				else if (!(DeviceState.ModelDa == text13))
				{
					if (!(DeviceState.ModelDa == text15))
					{
						if (!(DeviceState.ModelDa == text17))
						{
							if (!(DeviceState.ModelDa == text19))
							{
								string[] files = Directory.GetFiles(utils.BaseDirectory + text10);
								for (int i = 0; i < files.Length; i++)
								{
									byte[] item = File.ReadAllBytes(files[i]);
									DownloadAgent.Add(item);
								}
							}
							else
							{
								string path2 = utils.BaseDirectory + text20;
								if (!File.Exists(path2))
								{
									UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
									return false;
								}
								DownloadAgent.Add(File.ReadAllBytes(path2));
							}
						}
						else
						{
							string path3 = utils.BaseDirectory + text18;
							if (!File.Exists(path3))
							{
								UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
								return false;
							}
							DownloadAgent.Add(File.ReadAllBytes(path3));
						}
					}
					else
					{
						string path4 = utils.BaseDirectory + text16;
						if (!File.Exists(path4))
						{
							UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
							return false;
						}
						DownloadAgent.Add(File.ReadAllBytes(path4));
					}
				}
				else
				{
					string path5 = utils.BaseDirectory + text14;
					if (!File.Exists(path5))
					{
						UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
						return false;
					}
					DownloadAgent.Add(File.ReadAllBytes(path5));
				}
			}
			else
			{
				string[] files2 = Directory.GetFiles(utils.BaseDirectory + text10);
				for (int j = 0; j < files2.Length; j++)
				{
					byte[] item2 = File.ReadAllBytes(files2[j]);
					DownloadAgent.Add(item2);
				}
			}
		}
		else if (!object.Equals(MTKConfig.CpuType, objB2))
		{
			if (Portinfo.Name.Contains(value))
			{
				if (MTKConfig.MTKServer)
				{
					UI.RichLogs(msg, UI.log);
					if (MTKConfig.Danofarse)
					{
						Dacfg.da1 = (byte[])obj;
						Dacfg.da2 = (byte[])obj;
						byte[] array6 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text4).ToLower() + text5);
						if (array6 == null || array6.Length == 0)
						{
							UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
							return false;
						}
						Dacfg.da1 = array6;
						byte[] array7 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text4).ToLower() + text6);
						if (array7 == null || array7.Length == 0)
						{
							UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
							return false;
						}
						Dacfg.da2 = array7;
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					}
					else if (!(DeviceState.Brand == text7))
					{
						if (MTKConfig.Customda && MTKConfig.MTKServer)
						{
							byte[] array8 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text8);
							if (array8 == null || array8.Length == 0)
							{
								UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
								return false;
							}
							DownloadAgent.Add(array8);
							UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						}
						else
						{
							byte[] array9 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text4).ToLower() + text8);
							if (array9 == null || array9.Length == 0)
							{
								UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
								return false;
							}
							DownloadAgent.Add(array9);
							UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						}
					}
					else
					{
						byte[] array10 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text8);
						if (array10 == null || array10.Length == 0)
						{
							UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
							return false;
						}
						DownloadAgent.Add(array10);
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					}
				}
			}
			else if (DeviceState.ModelDa == text9)
			{
				string[] files3 = Directory.GetFiles(utils.BaseDirectory + text10);
				for (int k = 0; k < files3.Length; k++)
				{
					byte[] item3 = File.ReadAllBytes(files3[k]);
					DownloadAgent.Add(item3);
				}
			}
			else if (DeviceState.ModelDa == text11)
			{
				string path6 = utils.BaseDirectory + text12;
				if (!File.Exists(path6))
				{
					UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
					return false;
				}
				DownloadAgent.Add(File.ReadAllBytes(path6));
			}
			else if (!(DeviceState.ModelDa == text13))
			{
				if (!(DeviceState.ModelDa == text15))
				{
					if (DeviceState.ModelDa == text17)
					{
						string path7 = utils.BaseDirectory + text18;
						if (!File.Exists(path7))
						{
							UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
							return false;
						}
						DownloadAgent.Add(File.ReadAllBytes(path7));
					}
					else if (DeviceState.ModelDa == text19)
					{
						string path8 = utils.BaseDirectory + text20;
						if (!File.Exists(path8))
						{
							UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
							return false;
						}
						DownloadAgent.Add(File.ReadAllBytes(path8));
					}
					else
					{
						string[] files4 = Directory.GetFiles(utils.BaseDirectory + text10);
						for (int l = 0; l < files4.Length; l++)
						{
							byte[] item4 = File.ReadAllBytes(files4[l]);
							DownloadAgent.Add(item4);
						}
					}
				}
				else
				{
					string path9 = utils.BaseDirectory + text16;
					if (!File.Exists(path9))
					{
						UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
						return false;
					}
					DownloadAgent.Add(File.ReadAllBytes(path9));
				}
			}
			else
			{
				string path10 = utils.BaseDirectory + text14;
				if (!File.Exists(path10))
				{
					UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
					return false;
				}
				DownloadAgent.Add(File.ReadAllBytes(path10));
			}
		}
		else if (MTKConfig.MTKServer)
		{
			UI.RichLogs(msg, UI.log);
			if (MTKConfig.Danofarse)
			{
				Dacfg.da1 = (byte[])obj;
				Dacfg.da2 = (byte[])obj;
				byte[] array11 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text4).ToLower() + text5);
				if (array11 == null || array11.Length == 0)
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
					return false;
				}
				Dacfg.da1 = array11;
				byte[] array12 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text4).ToLower() + text6);
				if (array12 == null || array12.Length == 0)
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
					return false;
				}
				Dacfg.da2 = array12;
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			}
			else if (!(DeviceState.Brand == text7))
			{
				if (MTKConfig.Customda && MTKConfig.MTKServer)
				{
					byte[] array13 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text8);
					if (array13 == null || array13.Length == 0)
					{
						UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
						return false;
					}
					DownloadAgent.Add(array13);
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				}
				else
				{
					byte[] array14 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text3 + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text4).ToLower() + text8);
					if (array14 == null || array14.Length == 0)
					{
						UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
						return false;
					}
					DownloadAgent.Add(array14);
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				}
			}
			else
			{
				byte[] array15 = Server.LoadAndDecrypt(text + DeviceState.Brand + text2 + DeviceState.Model + text8);
				if (array15 == null || array15.Length == 0)
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
					return false;
				}
				DownloadAgent.Add(array15);
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			}
		}
		else if (DeviceState.ModelDa == text9)
		{
			string[] files5 = Directory.GetFiles(utils.BaseDirectory + text10);
			for (int m = 0; m < files5.Length; m++)
			{
				byte[] item5 = File.ReadAllBytes(files5[m]);
				DownloadAgent.Add(item5);
			}
		}
		else if (!(DeviceState.ModelDa == text11))
		{
			if (DeviceState.ModelDa == text13)
			{
				string path11 = utils.BaseDirectory + text14;
				if (!File.Exists(path11))
				{
					UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
					return false;
				}
				DownloadAgent.Add(File.ReadAllBytes(path11));
			}
			else if (!(DeviceState.ModelDa == text15))
			{
				if (!(DeviceState.ModelDa == text17))
				{
					if (!(DeviceState.ModelDa == text19))
					{
						string[] files6 = Directory.GetFiles(utils.BaseDirectory + text10);
						for (int n = 0; n < files6.Length; n++)
						{
							byte[] item6 = File.ReadAllBytes(files6[n]);
							DownloadAgent.Add(item6);
						}
					}
					else
					{
						string path12 = utils.BaseDirectory + text20;
						if (!File.Exists(path12))
						{
							UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
							return false;
						}
						DownloadAgent.Add(File.ReadAllBytes(path12));
					}
				}
				else
				{
					string path13 = utils.BaseDirectory + text18;
					if (!File.Exists(path13))
					{
						UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
						return false;
					}
					DownloadAgent.Add(File.ReadAllBytes(path13));
				}
			}
			else
			{
				string path14 = utils.BaseDirectory + text16;
				if (!File.Exists(path14))
				{
					UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
					return false;
				}
				DownloadAgent.Add(File.ReadAllBytes(path14));
			}
		}
		else
		{
			string path15 = utils.BaseDirectory + text12;
			if (!File.Exists(path15))
			{
				UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
				return false;
			}
			DownloadAgent.Add(File.ReadAllBytes(path15));
		}
		if (!DeviceState.Model.Contains(value2))
		{
			MTKConfig.Danofarse = false;
			Console.WriteLine(string.Format(format, MTKConfig.Danofarse));
		}
		else if (!Portinfo.Name.Contains(value))
		{
			MTKConfig.Danofarse = false;
			Console.WriteLine(string.Format(format, MTKConfig.Danofarse));
		}
		else
		{
			MTKConfig.Danofarse = true;
			Console.WriteLine(string.Format(format, MTKConfig.Danofarse));
		}
		if (!MTKConfig.Danofarse)
		{
			DA dA = Dacfg.Setup(DownloadAgent);
			if (dA == obj)
			{
				UI.RichLogs(msg5, UI.error, nextline: true, bold: true);
				return false;
			}
		}
		if (Mtk.Config.IsBROM && Mtk.Config.Chipcfg.Damode != (DAmodes)6)
		{
			PreloaderName = Dacfg.MExtractLoader(preloader, emiName).Item1;
			PreloaderSize = utils.FileSize(preloader.Length);
			Dacfg.ExtractEMI(preloader);
		}
		SetDA();
		Console.WriteLine(text21 + DeviceState.Brand + text22 + DeviceState.Model + text23 + DeviceState.ModelDa);
		UI.RichLogs(msg6, UI.log);
		if (FlashMode != DAmodes.XFLASH)
		{
			if (FlashMode != (DAmodes)6)
			{
				return false;
			}
			return DAXml.UploadDA();
		}
		return DAXFlash.UploadDA();
	}

	public bool WriteFlash(long addr, long length, string partition, object data, string partType = "user")
	{
		if (JobStatus.Token.IsCancellationRequested)
		{
			return false;
		}
		if (FlashMode != (DAmodes)6)
		{
			if (FlashMode == DAmodes.XFLASH)
			{
				return DAXFlash.WriteFlash(addr, length, partition, data, partType);
			}
			return false;
		}
		return DAXml.WriteFlash(partition, data);
	}

	public bool FormatFlash(long addr, long length, string partType = "user")
	{
		string format = " formatting = addr :{0} length :{1} partType :{2}";
		Console.WriteLine(string.Format(format, addr, length, partType));
		if (!JobStatus.Token.IsCancellationRequested)
		{
			if (FlashMode == (DAmodes)6)
			{
				return DAXml.FormatFlash(addr, length, partType);
			}
			if (FlashMode != DAmodes.XFLASH)
			{
				return false;
			}
			return DAXFlash.FormatFlash(addr, length, partType);
		}
		return false;
	}

	public bool ReadFlash(long addr, long length, string fileName, string destination, string partType = "user")
	{
		if (!JobStatus.Token.IsCancellationRequested)
		{
			if (FlashMode != (DAmodes)6)
			{
				if (FlashMode != DAmodes.XFLASH)
				{
					return false;
				}
				return DAXFlash.ReadFlash(addr, length, fileName, destination, partType);
			}
			return DAXml.ReadFlash(addr, length, fileName, destination);
		}
		return false;
	}

	public byte[] ReadFlashByte(long addr, long length, string partType)
	{
		if (!JobStatus.Token.IsCancellationRequested)
		{
			if (FlashMode == (DAmodes)6)
			{
				return DAXml.ReadFlashByte(addr, length, partType);
			}
			if (FlashMode != DAmodes.XFLASH)
			{
				return Array.Empty<byte>();
			}
			return DAXFlash.ReadFlashByte(addr, length, partType);
		}
		return Array.Empty<byte>();
	}

	public byte[] ReadFlashByName(long addr, long length, string fileName, string partType)
	{
		if (JobStatus.Token.IsCancellationRequested)
		{
			return Array.Empty<byte>();
		}
		if (FlashMode != (DAmodes)6)
		{
			if (FlashMode == DAmodes.XFLASH)
			{
				return DAXFlash.ReadFlashByte(addr, length, partType);
			}
			return Array.Empty<byte>();
		}
		return DAXml.ReadFlashByName(fileName);
	}

	public bool OtherWriteFlash(string partition, string fileName, byte[] data = null)
	{
		if (!JobStatus.Token.IsCancellationRequested)
		{
			if (FlashMode != (DAmodes)6)
			{
				if (FlashMode == DAmodes.XFLASH)
				{
					return WriteFlashNew(partition, string.Empty, data);
				}
				return false;
			}
			return DAXml.WriteFlash(partition, data);
		}
		return false;
	}

	public bool WriteFlashNew(string partition, string fileName = "", byte[] data = null)
	{
		string text = "write new partition = ";
		string text2 = " fileName = ";
		int result = 0;
		object obj = null;
		int result2 = 1;
		try
		{
			Console.WriteLine(text + partition + text2 + fileName);
			SerialMTK.Write(new byte[12]
			{
				239, 238, 238, 254, 1, 0, 0, 0, 4, 0,
				0, 0
			});
			SerialMTK.Write(new byte[4]);
			SerialMTK.Read(16);
			long num = 0L;
			long num2 = 0L;
			long num3 = 256L;
			Stream stream;
			if (data !=null)
			{
				num = data.Length;
				stream = new MemoryStream(data);
			}
			else
			{
				stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				num = new FileInfo(fileName).Length;
			}
			using (stream)
			{
				stream.Seek(num2, SeekOrigin.Begin);
				while (num2 < num)
				{
					SerialMTK.Write(new byte[12]
					{
						239, 238, 238, 254, 1, 0, 0, 0, 4, 0,
						0, 0
					});
					SerialMTK.Write(new byte[4] { 9, 0, 1, 0 });
					SerialMTK.Read(16);
					SerialMTK.Write(new byte[12]
					{
						239, 238, 238, 254, 1, 0, 0, 0, 4, 0,
						0, 0
					});
					SerialMTK.Write(new byte[4] { 13, 0, 2, 0 });
					SerialMTK.Read(16);
					SerialMTK.Write(new byte[12]
					{
						239, 238, 238, 254, 1, 0, 0, 0, 72, 1,
						0, 0
					});
					byte[] array = new byte[328];
					Binary.WriteU32LE(array, 0, (uint)num2);
					byte[] bytes = Encoding.ASCII.GetBytes(partition);
					bytes.AsSpan().CopyTo(array.AsSpan(8));
					int num4 = (int)Math.Min(num3, num - num2);
					byte[] array2 = new byte[num4];
					stream.Read(array2, 0, num4);
					array2.AsSpan().CopyTo(array.AsSpan(328 - (int)num3));
					SerialMTK.Write(array);
					SerialMTK.Read(16);
					num2 += num4;
					UI.Progresbar1(num2, num);
				}
			}
			return (byte)result2 != 0;
		}
		catch (Exception ex)
		{
			Console.Write(ex.ToString());
		}
		return (byte)result != 0;
	}

	public bool WriteRPBM(string filePath)
	{
		return false;
	}

	public bool ReadRPMB(string savePath)
	{
		if (FlashMode != DAmodes.XFLASH)
		{
			return false;
		}
		return DAXFlash.XFlashExt.Read_RPMB(savePath);
	}

	public bool EraseRPMB()
	{
		return false;
	}

	public bool FormatFlashXml(List<MTKList> partList)
	{
		return DAXml.FormatAllFlash(partList, MTKConfig.PathFirmware);
	}

	public bool FlashUpgradeXml(List<MTKList> partList)
	{
		return DAXml.FlashUpgrade(partList, MTKConfig.PathFirmware);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
