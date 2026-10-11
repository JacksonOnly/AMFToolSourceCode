using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AMFTool.Library.Mediatek;
using AMFTool.Library.Mediatek.Config;
using AMFTool.Library.Mediatek.META;
using AMFTool.Library.Usb;

namespace AMFTool.ThreadWorker;

public class ThreadMETA
{
	private MtkPreloader Preloader;

	private MtkConfig Config;

	private META meta;

	private (byte[], Gpt) Partition;

	private int BlockSize;

	private string Block;

	private string MetaPath
	{
		get
		{
			string path = "meta";
			return Path.Combine(utils.BaseDirectory, path);
		}
	}

	public ThreadMETA()
	{
		meta = new META();
		Config = new MtkConfig();
		Preloader = new MtkPreloader(Config);
	}

	public bool Init()
	{
		int num = 0;
		string msg = "Failed!";
		int nextline = 1;
		string objB = "Andproid";
		string msg2 = "Factory Reset... ";
		string msg3 = "OK";
		string objB2 = "Transsion";
		string msg4 = "Reseting FRP... ";
		string partitionName = "frp";
		string msg5 = "Erasing FRP... ";
		string msg6 = "Enable ADB... ";
		string msg7 = "Disable ADB... ";
		string msg8 = "Removing Security Plugin... ";
		string msg9 = "Factory reset... ";
		string msg10 = "Removing Payjoy... ";
		string msg11 = "Rebooting... ";
		double field_ = Class38.field_409;
		try
		{
			if (!DeviceState.DeviceInit)
			{
				meta = new META();
				if (!Initializing())
				{
					UI.RichLogs(msg, UI.error, nextline: true, bold: true);
					SerialMETA.Close();
					return false;
				}
			}
			if (object.Equals(DeviceState.MetaMode, objB))
			{
				Job job = State.Job;
				Job job2 = job;
				if (job2 == Job.Erase)
				{
					UI.RichLogs(msg2, UI.log);
					factoryReset();
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				}
				else if (job2 == (Job)14)
				{
					MetaFormat();
				}
			}
			else if (object.Equals(DeviceState.MetaMode, objB2))
			{
				switch (State.Job)
				{
				case (Job)80:
					UI.RichLogs(msg11, UI.log);
					if (!meta.SP_META_RebootTarget())
					{
						UI.RichLogs(msg, UI.error, nextline: true);
						return false;
					}
					utils.Pause(field_);
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					break;
				case Job.Payjoy:
					UI.RichLogs(msg10, UI.log);
					if (SecurityPluginOperation())
					{
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						UI.RichLogs(msg9, UI.log);
						if (meta.SP_META_Customer_Operation_CMD_Format())
						{
							UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
							break;
						}
						UI.RichLogs(msg, UI.error, nextline: true);
						return false;
					}
					UI.RichLogs(msg, UI.error, nextline: true);
					return false;
				case (Job)62:
					UI.RichLogs(msg7, UI.log);
					ADBEnableDisableOperation();
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					break;
				case Job.AdbEnable:
					UI.RichLogs(msg6, UI.log);
					ADBEnableDisableOperation();
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					break;
				case Job.NVBackup:
					BackupNVRAMOperation();
					break;
				case Job.Erase:
					factoryReset();
					break;
				case Job.Reset:
					UI.RichLogs(msg4, UI.log);
					if (ErasePartition(partitionName))
					{
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						UI.RichLogs(msg2, UI.log);
						if (!meta.SP_META_Customer_Operation_CMD_Format())
						{
							UI.RichLogs(msg, UI.error, nextline: true);
							return false;
						}
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						break;
					}
					UI.RichLogs(msg, UI.error, nextline: true);
					return false;
				case Job.Frp:
					UI.RichLogs(msg4, UI.log);
					if (!ErasePartition(partitionName))
					{
						UI.RichLogs(msg, UI.error, nextline: true);
						return false;
					}
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					UI.RichLogs(msg5, UI.log);
					if (WritePartition(partitionName))
					{
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						break;
					}
					UI.RichLogs(msg, UI.error, nextline: true);
					return false;
				case Job.ImeiWrite:
					WriteIMEIOperation();
					break;
				case (Job)70:
					UI.RichLogs(msg8, UI.log);
					if (!SecurityPluginOperation())
					{
						UI.RichLogs(msg, UI.error, nextline: true);
						return false;
					}
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					UI.RichLogs(msg9, UI.log);
					if (!meta.SP_META_Customer_Operation_CMD_Format())
					{
						UI.RichLogs(msg, UI.error, nextline: true);
						return false;
					}
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					break;
				}
			}
			if (!meta.SP_META_RebootTarget())
			{
				return false;
			}
			utils.Pause(field_);
			return true;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)nextline != 0, (byte)num != 0);
			return (byte)num != 0;
		}
	}

	private bool Initializing()
	{
		int num = 0;
		string msg = "Waiting Mediatek USB Connection... ";
		string category = "MTK";
		int timeout = 360000;
		string msg2 = "OK";
		int num2 = 1;
		string msg3 = "Name : ";
		string value = "PreLoader";
		string text = "Connecting to device ";
		string text2 = "...";
		string msg4 = "Failed!";
		string msg5 = "Rebooting to META Mode... ";
		string text3 = "... ";
		string msg6 = "Handshaking... ";
		object status = null;
		string msg7 = "Waiting META USB Connection... ";
		double field_ = Class38.field_409;
		string msg8 = "Connecting to device... ";
		double field_2 = Class38.field_2144;
		int num3 = 7;
		string text4 = ":";
		int num4 = 2;
		int num5 = 3;
		int num6 = 4;
		int num7 = 5;
		int num8 = 6;
		string msg9 = "";
		string text5 = "MODE META ";
		string objB = "Andproid";
		string objB2 = "Transsion";
		if (!METAConfig.METAReady)
		{
			UI.RichLogs(msg, UI.log);
			if (!UsbScan.FindUSB(category))
			{
				return false;
			}
			UI.RichLogs(msg2, UI.ok, nextline: true);
			UI.RichLogs(msg3, UI.log);
			UI.RichLogs(Portinfo.Name, UI.info, nextline: true);
			try
			{
				if (!Portinfo.Name.Contains(value))
				{
					UI.RichLogs(text + Portinfo.COM + text3, UI.log);
					if (!SerialMTK.Open(Portinfo.COM))
					{
						UI.RichLogs(msg4, UI.error, nextline: true);
						return false;
					}
					UI.RichLogs(msg2, UI.ok, nextline: true);
					UI.RichLogs(msg6, UI.log);
					if (!SerialMTK.Handshake((byte[])status))
					{
						UI.RichLogs(msg4, UI.error, nextline: true);
						return false;
					}
					UI.RichLogs(msg2, UI.ok, nextline: true);
					if (!Preloader.Init(display: true, meta: true))
					{
						return false;
					}
				}
				else
				{
					UI.RichLogs(text + Portinfo.COM + text2, UI.log);
					if (!SerialMETA.Open(Portinfo.COM))
					{
						UI.RichLogs(msg4, UI.error, nextline: true);
						return false;
					}
					UI.RichLogs(msg2, UI.ok, nextline: true);
					UI.RichLogs(msg5, UI.log);
					RebootToMetaFromPreloader();
					UI.RichLogs(msg2, UI.ok, nextline: true);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				UI.RichLogs(msg4, UI.error, (byte)num2 != 0, (byte)num != 0);
				return (byte)num != 0;
			}
		}
		UI.RichLogs(msg7, UI.log, (byte)num != 0, (byte)num != 0);
		utils.Pause(field_);
		if ((UsbScan.FindUSB(category, timeout) ? 1 : 0) == num)
		{
			return (byte)num != 0;
		}
		UI.RichLogs(msg2, UI.ok, (byte)num2 != 0, (byte)num2 != 0);
		UI.RichLogs(msg3, UI.log, (byte)num != 0, (byte)num != 0);
		UI.RichLogs(Portinfo.Name, UI.info, (byte)num2 != 0, (byte)num2 != 0);
		UI.RichLogs(msg8, UI.log, (byte)num != 0, (byte)num != 0);
		if ((SerialMETA.Open(Portinfo.COM) ? 1 : 0) != num)
		{
			UI.RichLogs(msg2, UI.ok, (byte)num2 != 0, (byte)num2 != 0);
			utils.Pause(field_2);
			if ((Handshake() ? 1 : 0) != num)
			{
				ReadDeviceInfo();
				string[] array = new string[num3];
				array[num] = Portinfo.VID;
				array[num2] = text4;
				array[num4] = Portinfo.PID;
				array[num5] = text4;
				array[num6] = Portinfo.COM;
				array[num7] = text4;
				array[num8] = Portinfo.Name;
				DeviceState.SetInit(string.Concat(array));
				UI.RichLogs(msg9, UI.ok, (byte)num2 != 0, (byte)num2 != 0);
				Console.WriteLine(text5 + DeviceState.MetaMode);
				if (object.Equals(DeviceState.MetaMode, objB))
				{
					return (byte)num2 != 0;
				}
				if (!object.Equals(DeviceState.MetaMode, objB2))
				{
					return (byte)num2 != 0;
				}
				return ReadGPT();
			}
			return (byte)num != 0;
		}
		return (byte)num != 0;
	}

	private bool Handshake()
	{
		string msg = "Handshaking... ";
		double field_ = Class38.field_203;
		string msg2 = "OK";
		for (int i = 0; i < 3; i++)
		{
			UI.RichLogs(msg, UI.log);
			utils.Pause(field_);
			if (meta.SP_META_ConnectInMetaModeByUSB())
			{
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
				return true;
			}
			utils.Pause(field_);
		}
		return false;
	}

	private void RebootToMetaFromPreloader()
	{
		string text = "READY";
		string s = "METAMETA";
		int num = 4;
		int num2 = 0;
		string text2 = "METASLA";
		string text3 = "READYMETASLA";
		string s2 = "SLASTART\0";
		string value = "EXT";
		int len = 8;
		string s3 = "DISCONNECT";
		byte[] bytes = SerialMETA.read(5);
		string text4 = Encoding.ASCII.GetString(bytes);
		if (text4 != text)
		{
			return;
		}
		SerialMETA.write(Encoding.ASCII.GetBytes(s));
		uint[] array = new uint[3] { 4u, 1u, 3u };
		byte[] array2 = new byte[array.Length * 4];
		Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
		SerialMETA.write(array2);
		bytes = SerialMETA.read(13);
		text4 = Encoding.ASCII.GetString(bytes);
		if (text4 == text2 || text4 == text3)
		{
			SerialMETA.write(Encoding.ASCII.GetBytes(s2));
			byte[] array3 = SerialMETA.read(22);
			List<byte> list = new List<byte>();
			list.AddRange(array3.Skip(6).Take(4));
			uint[] array4 = (Encoding.ASCII.GetString(array3, 0, 16).Contains(value) ? new uint[3] { 483126860u, 723366324u, 1065621571u } : new uint[3] { 984453828u, 3285121377u, 3416882963u });
			uint[] array5 = array4;
			foreach (uint value2 in array5)
			{
				list.AddRange(BitConverter.GetBytes(value2));
			}
			byte[] data;
			using (MD5 mD = MD5.Create())
			{
				data = mD.ComputeHash(list.ToArray());
			}
			SerialMETA.write(data);
			SerialMETA.read(len);
		}
		if (!JobStatus.Token.IsCancellationRequested)
		{
			SerialMETA.write(array2);
			bytes = SerialMETA.read(len);
			Console.WriteLine(Encoding.ASCII.GetString(bytes));
			uint[] array6 = new uint[num];
			Buffer.BlockCopy(Convert.FromBase64String("BgAAAAEAAAADAAAAAQAAAA=="), 0, array6, 0, Buffer.ByteLength(array6));
			uint[] array7 = array6;
			byte[] array8 = new byte[array7.Length * num];
			Buffer.BlockCopy(array7, num2, array8, num2, array8.Length);
			SerialMETA.write(array8);
			SerialMETA.write(Encoding.ASCII.GetBytes(s3));
		}
	}

	private void RebootToMetaFromPreloadernew()
	{
		string msg = "OK";
		string msg2 = "Send BootMode Switch Request... ";
		string s = "METAMETA";
		string text = "status 1 : ";
		string value = "READY";
		string text2 = "status 2 : ";
		string text3 = "status 3 : ";
		string value2 = "METASLA";
		string s2 = "SLASTART.";
		string text4 = "status 4 : ";
		string value3 = "RANDOM";
		string text5 = "";
		string value4 = "EXT";
		string text6 = "tecno";
		string text7 = "infinix";
		string value5 = "SHA";
		string text8 = "itel";
		string s3 = "DISCONNECT";
		byte[] array = SerialMETA.read(5);
		UI.RichLogs(msg, Color.FromArgb(124, 235, 60), nextline: true, bold: true);
		UI.RichLogs(msg2, Color.FromArgb(245, 245, 245));
		SerialMETA.write(Encoding.ASCII.GetBytes(s));
		string text9 = Encoding.UTF8.GetString(SerialMETA.read(0));
		byte[] data = new byte[text9.Length * 4];
		Console.WriteLine(text + text9);
		if (text9.Contains(value))
		{
			text9 = Encoding.UTF8.GetString(SerialMETA.read(0));
			Console.WriteLine(text2 + text9);
		}
		Console.WriteLine(text3 + text9);
		if (!text9.Contains(value2))
		{
			return;
		}
		SerialMETA.write(Encoding.ASCII.GetBytes(s2));
		byte[] array2 = SerialMETA.read(0);
		text9 = Encoding.UTF8.GetString(array2);
		Console.WriteLine(text4 + text9);
		byte[] array3 = Array.Empty<byte>();
		if (!text9.Contains(value3))
		{
			return;
		}
		string text10 = text5;
		text10 = ((!text9.Contains(value4)) ? text7 : text6);
		if (!text9.Contains(value5))
		{
			byte[] first = array2.Skip(6).Take(4).ToArray();
			if (!(text10 == text7))
			{
				if (text10 == text6 || text10 == text8)
				{
					array3 = new byte[12]
					{
						76, 238, 203, 28, 180, 177, 29, 43, 67, 24,
						132, 63
					};
				}
			}
			else
			{
				array3 = new byte[12]
				{
					196, 146, 173, 58, 97, 249, 206, 195, 19, 127,
					169, 203
				};
			}
			byte[] data2 = MD5.Create().ComputeHash(first.Concat(array3).ToArray());
			SerialMETA.write(data2);
		}
		else
		{
			byte[] first2 = array2.Skip(6).Take(4).ToArray();
			int num = BitConverter.ToInt32(array2, 13);
			if (text10 == text7)
			{
				array3 = new byte[120]
				{
					124, 52, 225, 137, 18, 225, 205, 61, 86, 49,
					173, 178, 36, 118, 211, 18, 52, 226, 202, 253,
					19, 18, 61, 43, 59, 19, 225, 87, 34, 173,
					193, 29, 61, 52, 253, 61, 26, 87, 70, 26,
					53, 19, 196, 175, 90, 134, 34, 69, 157, 61,
					209, 70, 114, 65, 79, 173, 70, 173, 83, 17,
					194, 59, 61, 45, 26, 47, 61, 250, 223, 53,
					87, 36, 167, 77, 94, 79, 52, 211, 79, 45,
					223, 31, 19, 211, 178, 145, 65, 61, 79, 209,
					93, 145, 253, 46, 77, 111, 61, 65, 52, 127,
					69, 243, 138, 38, 26, 51, 79, 62, 94, 100,
					54, 138, 209, 246, 159, 53, 106, 150, 42, 93
				};
			}
			byte[] second = array3.Skip(12 * num).Take(12).ToArray();
			byte[] data3 = SHA256.Create().ComputeHash(first2.Concat(second).ToArray());
			SerialMETA.write(data3);
		}
		if (!JobStatus.Token.IsCancellationRequested)
		{
			SerialMETA.write(data);
			array = SerialMETA.read(8);
			Console.WriteLine(Encoding.ASCII.GetString(array));
			uint[] array4 = new uint[4] { 6u, 1u, 3u, 1u };
			byte[] array5 = new byte[array4.Length * 4];
			Buffer.BlockCopy(array4, 0, array5, 0, array5.Length);
			SerialMETA.write(array5);
			SerialMETA.write(Encoding.ASCII.GetBytes(s3));
		}
	}

	private void RebootToMetaFromBrom()
	{
		string msg = "Handshaking...";
		object status = null;
		string msg2 = "Failed!";
		string msg3 = "OK";
		UI.RichLogs(msg, UI.log);
		if (SerialMTK.Handshake((byte[])status))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true);
			if (Preloader.Init(display: true, meta: true))
			{
				LibUsb.Close();
			}
		}
		else
		{
			UI.RichLogs(msg2, UI.error, nextline: true);
		}
	}

	private void ReadDeviceInfo()
	{
		string msg = "";
		string msg2 = "[Version Information]";
		string msg3 = "Chipset : ";
		string msg4 = "Build Date : ";
		string msg5 = "Software Version : ";
		string text = "Brand#ro.product.brand";
		string text2 = "Model#ro.product.model";
		string text3 = "Product#ro.product.device";
		string text4 = "Product Name#ro.product.name";
		string text5 = "Android Version#ro.build.version.release";
		string text6 = "Security Patch#ro.build.version.security_patch";
		string text7 = "Display ID#ro.build.display.id";
		string text8 = "Build Date#ro.build.date";
		string text9 = "Build#ro.build.version.incremental";
		string text10 = "Build ID#ro.build.id";
		string msg6 = "[Devices Information]";
		string text11 = "UNKNOWN";
		string text12 = " : ";
		METAApi.DEVICE_INFO_CNF dEVICE_INFO_CNF = meta.SP_META_Get_DevInfo();
		string text13 = dEVICE_INFO_CNF.BbChip.UTF8String();
		dEVICE_INFO_CNF.EcoVer.UTF8String();
		string text14 = dEVICE_INFO_CNF.SwTime.UTF8String();
		dEVICE_INFO_CNF.DspFw.UTF8String();
		dEVICE_INFO_CNF.DspPatch.UTF8String();
		string text15 = dEVICE_INFO_CNF.SwVer.UTF8String();
		dEVICE_INFO_CNF.HwVer.UTF8String();
		dEVICE_INFO_CNF.MelodyVer.UTF8String();
		UI.RichLogs(msg, UI.log, nextline: true);
		UI.RichLogs(msg2, UI.log, nextline: true);
		if (text13.Length > 0)
		{
			UI.RichLogs(msg3, UI.log);
			UI.RichLogs(text13, UI.info, nextline: true, bold: true);
		}
		if (text14.Length > 0)
		{
			UI.RichLogs(msg4, UI.log);
			UI.RichLogs(text14, UI.info, nextline: true, bold: true);
		}
		if (text15.Length > 0)
		{
			UI.RichLogs(msg5, UI.log);
			UI.RichLogs(text15, UI.info, nextline: true, bold: true);
		}
		string[] array = new string[10] { text, text2, text3, text4, text5, text6, text7, text8, text9, text10 };
		UI.RichLogs(msg, UI.log, nextline: true);
		UI.RichLogs(msg6, UI.log, nextline: true);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('#');
			string text16 = meta.SP_META_GetTargetBuildProp(array2[1]);
			if (!string.IsNullOrEmpty(text16) && text16 != text11)
			{
				UI.RichLogs(array2[0] + text12, UI.log);
				UI.RichLogs(text16.ToUpper(), UI.info, nextline: true, bold: true);
			}
		}
	}

	private (bool, int, string, string) DetectStorage()
	{
		string cmd = "cat /sys/block/mmcblk0/queue/physical_block_size 2>/dev/null || cat /sys/block/sdc/queue/physical_block_size 2>/dev/null";
		string value = "512";
		string block = "mmcblk0";
		string text = "emmc";
		string value2 = "4096";
		string block2 = "sdc";
		string text2 = "ufs";
		object obj = null;
		string text3 = SendCustomCommand(cmd);
		string item;
		if (!text3.Contains(value))
		{
			if (text3.Contains(value2))
			{
				BlockSize = 4096;
				Block = block2;
				item = text2;
				return (true, BlockSize, item, Block);
			}
			return (false, 0, (string)obj, (string)obj);
		}
		BlockSize = 512;
		Block = block;
		item = text;
		return (true, BlockSize, item, Block);
	}

	private string SendCustomCommand(string cmd, bool base64 = false)
	{
		string text = " 2>&1; echo STATUS:$?";
		string value = "STATUS:";
		string text2 = "Shell Response: ";
		string cmd2 = cmd + text;
		byte[] array = meta.SP_META_Customer_Operation_CMD_Shell2(cmd2, base64);
		if (array == null || array.Length == 0)
		{
			return string.Empty;
		}
		string text3 = Encoding.ASCII.GetString(array).Trim('\0', '\r', '\n');
		if (text3.Contains(value))
		{
			Console.WriteLine(text2 + text3);
			return text3;
		}
		return text3;
	}

	private bool ReadGPT()
	{
		string msg = "Reading partition info... ";
		string format = "dd if=/dev/block/{0} bs=512 skip={1} count=1 2>/dev/null | base64 -w 0";
		object obj = null;
		string msg2 = "Failed!";
		string msg3 = "OK";
		string format2 = " [{0}]";
		if (State.Job != Job.AdbEnable && State.Job != (Job)62 && State.Job != Job.ImeiWrite)
		{
			UI.RichLogs(msg, UI.log);
			(bool, int, string, string) tuple = DetectStorage();
			var (flag, num, _, _) = tuple;
			_ = tuple.Item3;
			string item = tuple.Item4;
			if (flag)
			{
				MemoryStream memoryStream = new MemoryStream();
				for (int i = 0; i < 48; i++)
				{
					byte[] array = meta.SP_META_Customer_Operation_CMD_Shell2(string.Format(format, item, i), base64: true);
					if (array == null || array.Length == 0)
					{
						break;
					}
					memoryStream.Write(array, 0, array.Length);
				}
				byte[] array2 = memoryStream.ToArray();
				if (array2.Length < num * 2)
				{
					return false;
				}
				Gpt gpt = new Gpt();
				if (!gpt.Parse(array2, num))
				{
					return false;
				}
				Partition = (array2, gpt);
				var (array3, gpt2) = Partition;
				if (array3 == null && gpt2 == obj)
				{
					UI.RichLogs(msg2, UI.error, nextline: true);
					return false;
				}
				UI.RichLogs(msg3, UI.ok, nextline: false, bold: true);
				UI.RichLogs(string.Format(format2, gpt.PartEntries.Count), UI.log, nextline: true, bold: true);
				METAConfig.METAReady = true;
			}
			return true;
		}
		return true;
	}

	private (bool, string, long, long) DetectPartition(string partitionName)
	{
		object obj = null;
		int item = 0;
		int num = -1;
		var (array, gpt) = Partition;
		if (array == null && gpt == obj)
		{
			return (false, string.Empty, -1L, -1L);
		}
		foreach (Partf partEntry in Partition.Item2.PartEntries)
		{
			if (partEntry.Name == partitionName)
			{
				return (true, partEntry.Name, (long)partEntry.Sector, (long)partEntry.Sectors);
			}
		}
		return ((byte)item != 0, string.Empty, num, num);
	}

	private bool ErasePartition(string partitionName, bool display = false)
	{
		string format = "dd if=/dev/zero of=/dev/block/{0} bs={1} count=1 seek={2} conv=notrunc 2>/dev/null";
		string text = "echo '";
		string text2 = "' | base64 -d | sh";
		string text3 = "ERASE ";
		string text4 = " OK";
		(bool, string, long, long) tuple = DetectPartition(partitionName);
		var (flag, text5, num, _) = tuple;
		_ = tuple.Item4;
		if (!flag)
		{
			return false;
		}
		string s = string.Format(format, Block, BlockSize, num);
		string text6 = Convert.ToBase64String(Encoding.ASCII.GetBytes(s));
		string cmd = text + text6 + text2;
		meta.SP_META_Customer_Operation_CMD_Shell2(cmd);
		Console.WriteLine(text3 + text5 + text4);
		return true;
	}

	private bool ReadPartition(string partitionName, string savePath, bool display = false)
	{
		string text = "Reading ";
		string format = "dd if=/dev/block/{0} bs=1 skip={1} count={2} status=none 2>/dev/null | xxd -p -l {3}";
		UI.RichLogs(text + partitionName, UI.log);
		(bool, string, long, long) tuple = DetectPartition(partitionName);
		var (flag, _, _, _) = tuple;
		_ = tuple.Item2;
		long item = tuple.Item3;
		long item2 = tuple.Item4;
		if (flag)
		{
			long num = item * BlockSize;
			long num2 = item2 * BlockSize;
			long num3 = 0L;
			int num4 = 0;
			using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
			{
				while (num3 < num2)
				{
					int num5 = (int)Math.Min(128L, num2 - num3);
					long num6 = num + num3;
					string cmd = string.Format(format, Block, num6, num5, num5);
					byte[] data = meta.SP_META_Customer_Operation_CMD_Shell2(cmd);
					byte[] array = utils.clearing(data);
					if (array != null && array.Length != 0)
					{
						num4 = 0;
						if (array.Length <= num5)
						{
							fileStream.Write(array, 0, array.Length);
							num3 += array.Length;
						}
						else
						{
							fileStream.Write(array, 0, num5);
							num3 += num5;
						}
						UI.Progresbar1(num3, num2);
					}
					else
					{
						if (++num4 >= 3)
						{
							break;
						}
						Thread.Sleep(50);
					}
				}
				fileStream.Flush();
			}
			return new FileInfo(savePath).Length == num2;
		}
		return false;
	}

	private bool WritePartition(string partitionName, bool display = false)
	{
		string path = "Data";
		string path2 = "img";
		string path3 = "frp.img";
		string oldValue = "-";
		string newValue = "";
		string format = "echo \"{0}\" | xxd -r -p | dd of=/dev/block/{1} bs=1 seek={2} conv=notrunc 2>/dev/null";
		byte[] array = File.ReadAllBytes(Path.Combine(Application.StartupPath, path, path2, path3));
		(bool, string, long, long) tuple = DetectPartition(partitionName);
		var (flag, _, _, _) = tuple;
		_ = tuple.Item2;
		long item = tuple.Item3;
		_ = tuple.Item4;
		if (!flag)
		{
			return false;
		}
		long num = 0L;
		if (0L >= array.Length)
		{
			return true;
		}
		int num2 = (int)Math.Min(384L, array.Length - num);
		byte[] array2 = new byte[num2];
		Buffer.BlockCopy(array, (int)num, array2, 0, num2);
		string arg = BitConverter.ToString(array2).Replace(oldValue, newValue).ToLower();
		ulong num3 = (ulong)(item * BlockSize + num);
		string cmd = string.Format(format, arg, Block, num3);
		meta.SP_META_Customer_Operation_CMD_Shell2(cmd);
		num += num2;
		int num4 = 2090654323;
		while (true)
		{
			_ = num4 ^ -1985438634;
			if (num >= array.Length)
			{
				break;
			}
			num2 = (int)Math.Min(384L, array.Length - num);
			array2 = new byte[num2];
			Buffer.BlockCopy(array, (int)num, array2, 0, num2);
			arg = BitConverter.ToString(array2).Replace(oldValue, newValue).ToLower();
			num3 = (ulong)(item * BlockSize + num);
			cmd = string.Format(format, arg, Block, num3);
			meta.SP_META_Customer_Operation_CMD_Shell2(cmd);
			num += num2;
			num4 = 2090654323;
		}
		return true;
	}

	private bool ADBEnableDisableOperation(bool disable = false, bool newpsn = false)
	{
		int num = 0;
		string text = "/";
		string filter = "*";
		object obj = null;
		int num2 = 1;
		string value = "_ENUM";
		string value2 = "y_mode_flag";
		string message = "No valid APDB file found";
		string lidName = "AP_CFG_REEB_PRODUCT_INFO_LID";
		string s = "123456789ABCDEFG";
		string text2 = "yyyyMMdd";
		int num3 = 4;
		string value3 = "\\x";
		string text3 = "X2";
		string format = "setprop  persist.sys.usb.config mtp,adb key {0}";
		string format2 = "echo '{0}' > /dev/block/by-name/para";
		string text4 = meta.SP_META_Query_APDBPath().GetPath();
		if (!string.IsNullOrEmpty(text4))
		{
			if (!text4.EndsWith(text))
			{
				text4 += text;
			}
			METAApi.FILE_OPERATION_PARSE_CNF fILE_OPERATION_PARSE_CNF = meta.SP_META_File_Operation_Parse(text4, filter);
			if (fILE_OPERATION_PARSE_CNF.FileCount == 0)
			{
				return false;
			}
			METAApi.FILE_OPERATION_GETFILEINFO_CNF fILE_OPERATION_GETFILEINFO_CNF = (METAApi.FILE_OPERATION_GETFILEINFO_CNF)obj;
			bool flag = false;
			for (uint num4 = 0u; num4 < fILE_OPERATION_PARSE_CNF.FileCount; num4++)
			{
				METAApi.FILE_OPERATION_GETFILEINFO_CNF fILE_OPERATION_GETFILEINFO_CNF2 = meta.SP_META_File_Operation_GetFileInfo(num4);
				if (fILE_OPERATION_GETFILEINFO_CNF2.FileType == 1 && fILE_OPERATION_GETFILEINFO_CNF2.FileSize != 0)
				{
					string text5 = MetaBytesToString(fILE_OPERATION_GETFILEINFO_CNF2.FileName);
					if (text5.IndexOf(value, StringComparison.OrdinalIgnoreCase) < 0 && text5.IndexOf(value2, StringComparison.OrdinalIgnoreCase) < 0)
					{
						fILE_OPERATION_GETFILEINFO_CNF = fILE_OPERATION_GETFILEINFO_CNF2;
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				string filepath = text4 + MetaBytesToString(fILE_OPERATION_GETFILEINFO_CNF.FileName);
				byte[] data = meta.SP_META_File_Operation_ReceiveFile(filepath, fILE_OPERATION_GETFILEINFO_CNF.FileSize);
				ushort num5 = meta.SP_META_Nvram_Get_LIDNumber(data, lidName);
				if (num5 != 0 && num5 < ushort.MaxValue)
				{
					byte[] array = meta.SP_META_Nvram_Read(num5, 1);
					if (array == null || array.Length < 1024)
					{
						return false;
					}
					array[816] = 0;
					array[818] = ((!disable) ? ((byte)1) : ((byte)0));
					array[949] = 0;
					array[951] = ((!disable) ? ((byte)1) : ((byte)0));
					if (newpsn)
					{
						byte[] bytes = Encoding.ASCII.GetBytes(s);
						Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
					}
					meta.SP_META_Nvram_Write(num5, 1, array);
					string s2 = DateTime.Now.ToString(text2);
					byte[] b;
					using (MD5 mD = MD5.Create())
					{
						b = mD.ComputeHash(Encoding.UTF8.GetBytes(s2));
					}
					Guid guid = new Guid(b);
					byte[] array2 = guid.ToByteArray();
					StringBuilder stringBuilder = new StringBuilder(array2.Length * num3);
					for (int i = num; i < array2.Length; i += num2)
					{
						stringBuilder.Append(value3).Append(array2[i].ToString(text3));
					}
					meta.SP_META_Customer_Operation_CMD_Shell(Encoding.UTF8.GetBytes(string.Format(format, guid)));
					meta.SP_META_Customer_Operation_CMD_Shell(Encoding.UTF8.GetBytes(string.Format(format2, stringBuilder)));
					return (byte)num2 != 0;
				}
				return false;
			}
			throw new Exception(message);
		}
		return false;
	}

	private void BackupNVRAMOperation()
	{
		string value = "Backing up nvram";
		string message = "[PROTOCOL ERR] NVRAM BACKUP EMPTY";
		string name = "KOSONG";
		string path = "NVRAM.bin";
		string text = "[ARG ERR] ";
		string text2 = "Nv file is saved to ";
		Console.WriteLine(value);
		byte[] array = meta.SP_META_Nvram_Backup();
		if (array == null || array.Length == 0)
		{
			throw new Exception(message);
		}
		string path2 = Path.Combine(utils.CheckBackupFolder(name), path);
		try
		{
			using FileStream fileStream = new FileStream(path2, FileMode.Create, FileAccess.Write, FileShare.None);
			fileStream.Write(array, 0, array.Length);
			fileStream.Flush();
		}
		catch (Exception ex)
		{
			throw new Exception(text + ex.Message);
		}
		Console.WriteLine(text2 + Path.GetFullPath(path2));
	}

	private void factoryReset()
	{
		string text = "550010660C00000015008F00000000003000000085";
		string hex = text;
		WriteData(hex.HexToBytes());
	}

	public bool MetaFormat()
	{
		string msg = "Formating ";
		int result = 0;
		string msg2 = "USERDATA";
		string msg3 = " ... ";
		string text = "Data\\img\\para.img";
		string filename = "para";
		string msg4 = "OK";
		int num = 1;
		string msg5 = "Failed";
		string text2 = "Info : ";
		try
		{
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(msg2, UI.log);
			UI.RichLogs(msg3, UI.log);
			string filepath = text;
			if (MetaWriteFile(filepath, filename))
			{
				UI.RichLogs(msg4, UI.log, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg5, Color.Tomato, nextline: true, bold: true);
			}
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(msg5, Color.Tomato, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(text2 + ex.Message, Color.Tomato, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	public string CalcChecksumXOR(byte[] data)
	{
		string text = "X2";
		byte b = 0;
		for (int i = 0; i < data.Length; i++)
		{
			b ^= data[i];
		}
		return b.ToString(text);
	}

	public bool MetaWriteFile(string filepath, string filename)
	{
		int num = 1;
		string msg = "Failed";
		string text = "Info : ";
		int result = 0;
		try
		{
			byte[] input = GenerateWrite(filepath, filename);
			return WriteData(input);
		}
		catch (Exception ex)
		{
			UI.RichLogs(msg, Color.Tomato, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(text + ex.Message, Color.Tomato, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	public string GenerateSize3ByteLE(long sizeBytes)
	{
		string text = "X8";
		string text2 = sizeBytes.ToString(text);
		string text3 = text2.Substring(2, 6);
		return text3.Substring(4, 2) + text3.Substring(2, 2) + text3.Substring(0, 2);
	}

	public byte[] UnpackProinfo(byte[] input)
	{
		object result = null;
		byte[] array = new byte[0];
		try
		{
			return input.Skip(26).Take(input.Length - 26 - 1).ToArray();
		}
		catch (Exception)
		{
			return (byte[])result;
		}
	}

	public string StringToHex(string text)
	{
		string oldValue = "-";
		string newValue = "";
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		return BitConverter.ToString(bytes).Replace(oldValue, newValue);
	}

	public byte[] GenerateWrite(string sourcePath, string filename)
	{
		string text = "55621466080208601200A30002000000";
		string text2 = "";
		string text3 = "0003";
		string text4 = "60010000000000";
		object result = null;
		try
		{
			byte[] array = File.ReadAllBytes(sourcePath);
			string text5 = text;
			text5 += PadHexString(StringToHex(filename ?? text2), 256);
			string text6 = GenerateSize3ByteLE(array.Length);
			text5 += PadHexString(text6 + text3, 257);
			text5 += text4;
			text5 += utils.BytesToHextring(array);
			string text7 = ((text5.Length < 50224) ? text5.PadRight(50224, '0') : text5.Substring(0, 50224));
			text5 = text7 + CalcChecksumXOR(text7.HexToBytes());
			return text5.HexToBytes();
		}
		catch (Exception)
		{
			return (byte[])result;
		}
	}

	public string PadHexString(string hex, int totalBytes)
	{
		string oldValue = " ";
		string newValue = "";
		double field_ = Class38.field_202;
		hex = hex.Replace(oldValue, newValue);
		int num = (int)Math.Round((double)hex.Length / field_);
		int num2 = totalBytes - num;
		if (num2 <= 0)
		{
			return hex;
		}
		hex += new string('0', num2 * 2);
		return hex;
	}

	public bool WriteData(byte[] input, bool ack = true, int ackoffset = 7)
	{
		string value = "ACK";
		if (ack)
		{
			for (int i = 0; i != 5; i++)
			{
				SerialMETA.write(input);
				byte[] array = SerialMETA.read(0);
				if (array.Length >= 7)
				{
					byte b = array[ackoffset];
					if (b == 0 || b == 1 || b == 4)
					{
						Console.WriteLine(value);
						return true;
					}
				}
			}
			return false;
		}
		SerialMETA.write(input);
		return true;
	}

	private bool SecurityPluginOperation()
	{
		string cmd = "dd if=/dev/zero of=/dev/block/by-name/proinfo bs=1 count=1552 seek=2752512 conv=notrunc && echo -n ':' | dd of=/dev/block/by-name/proinfo bs=1 seek=2754550 conv=notrunc";
		meta.SP_META_Customer_Operation_CMD_Shell2(cmd);
		return true;
	}

	private string MetaBytesToString(byte[] buff)
	{
		if (buff == null || buff.Length == 0)
		{
			return string.Empty;
		}
		byte[] array = meta.EscapeMetaBytes(buff);
		int num = Array.IndexOf(array, (byte)0);
		if (num < 0)
		{
			return Encoding.UTF8.GetString(array);
		}
		return Encoding.UTF8.GetString(array, 0, num);
	}

	public bool WriteIMEIOperation()
	{
		string cmd = "-i alpha.inf";
		if (!DeviceState.CheckedIMEI1 && !DeviceState.CheckedIMEI2)
		{
			return false;
		}
		if (DeviceState.CheckedIMEI1 && DeviceState.CheckedIMEI2)
		{
			CreateINF_Dual(DeviceState.IMEI1, DeviceState.IMEI2);
			IMEIDUALINF(DeviceState.IMEI1, DeviceState.IMEI2);
			SerialMETA.Close();
			METAIMEIRepair(cmd);
			return true;
		}
		if (!DeviceState.CheckedIMEI1)
		{
			CreateINF_TWO(DeviceState.IMEI2);
			IMEI2INF(DeviceState.IMEI2);
			SerialMETA.Close();
			METAIMEIRepair(cmd);
			return true;
		}
		CreateINF_ONE(DeviceState.IMEI1);
		IMEI2INF(DeviceState.IMEI1);
		SerialMETA.Close();
		METAIMEIRepair(cmd);
		return true;
	}

	private void METAIMEIRepair(string cmd)
	{
		string path = "meta.exe";
		string verb = "runas";
		string text = "META exited with code ";
		Console.WriteLine(cmd);
		bool wroteImei1 = false;
		bool wroteImei2 = false;
		bool connected = false;
		bool dbLoaded = false;
		ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(MetaPath, path), cmd)
		{
			CreateNoWindow = true,
			WindowStyle = ProcessWindowStyle.Hidden,
			UseShellExecute = false,
			Verb = verb,
			WorkingDirectory = MetaPath,
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};
		using Process process = new Process
		{
			StartInfo = startInfo
		};
		process.OutputDataReceived += delegate(object s, DataReceivedEventArgs e)
		{
			string value = "connect target start";
			string msg = "Waiting Connect server... ";
			string value2 = "connect target success";
			string msg2 = "OK";
			string value3 = "load db from target start";
			string msg3 = "Load DB file from target... ";
			string value4 = "load db from target done";
			string value5 = "import nvram file start";
			string msg4 = "Write IMEI1 : ";
			string value6 = "write nvram done";
			string msg5 = "Write IMEI2 : ";
			string value7 = "write nvram failed";
			string msg6 = "Failed!";
			string value8 = "disconnect target start";
			string msg7 = "Rebooting device... ";
			string value9 = "disconnect target success";
			string data = e.Data;
			if (!string.IsNullOrEmpty(data))
			{
				Console.WriteLine(data);
				if (!connected)
				{
					if (data.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						UI.RichLogs(msg, UI.log);
					}
					if (data.IndexOf(value2, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
						UI.Progresbar1(20L, 20L);
						connected = true;
					}
				}
				if (!dbLoaded)
				{
					if (data.IndexOf(value3, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						UI.RichLogs(msg3, UI.log);
					}
					if (data.IndexOf(value4, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
						UI.Progresbar1(40L, 40L);
						dbLoaded = true;
					}
				}
				if (DeviceState.CheckedIMEI1 && !wroteImei1)
				{
					if (data.IndexOf(value5, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						UI.RichLogs(msg4, UI.log);
					}
					if (data.IndexOf(value6, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
						UI.Progresbar1(70L, 70L);
						wroteImei1 = true;
					}
				}
				else if (DeviceState.CheckedIMEI2 && !wroteImei2)
				{
					if (data.IndexOf(value5, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						UI.RichLogs(msg5, UI.log);
					}
					if (data.IndexOf(value6, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
						UI.Progresbar1(85L, 85L);
						wroteImei2 = true;
					}
				}
				if (data.IndexOf(value7, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					UI.RichLogs(msg6, UI.error, nextline: true);
				}
				if (data.IndexOf(value8, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					UI.RichLogs(msg7, UI.log);
				}
				if (data.IndexOf(value9, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
					UI.Progresbar1(100L, 100L);
				}
			}
		};
		process.ErrorDataReceived += delegate(object s, DataReceivedEventArgs e)
		{
			string text2 = "ERR: ";
			if (!string.IsNullOrEmpty(e.Data))
			{
				Console.WriteLine(text2 + e.Data);
				UI.RichLogs(e.Data, UI.error, nextline: true);
			}
		};
		Thread.Sleep(200);
		process.Start();
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();
		process.WaitForExit();
		if (process.ExitCode != 0)
		{
			UI.RichLogs(text + process.ExitCode, UI.error, nextline: true);
		}
		Thread.Sleep(300);
		Cleanup();
	}

	private void Cleanup()
	{
		string path = "alpha.inf";
		string path2 = "imei.nvData";
		string path3 = "imei2.nvData";
		string path4 = Path.Combine(MetaPath, path);
		if (!File.Exists(path4))
		{
			path4 = Path.Combine(MetaPath, path2);
			if (File.Exists(path4))
			{
				File.Delete(path4);
				path4 = Path.Combine(MetaPath, path3);
				if (File.Exists(path4))
				{
					File.Delete(path4);
				}
			}
			else
			{
				path4 = Path.Combine(MetaPath, path3);
				if (File.Exists(path4))
				{
					File.Delete(path4);
				}
			}
			return;
		}
		File.Delete(path4);
		path4 = Path.Combine(MetaPath, path2);
		if (File.Exists(path4))
		{
			File.Delete(path4);
			path4 = Path.Combine(MetaPath, path3);
			if (File.Exists(path4))
			{
				File.Delete(path4);
			}
		}
		else
		{
			path4 = Path.Combine(MetaPath, path3);
			if (File.Exists(path4))
			{
				File.Delete(path4);
			}
		}
	}

	private byte[] EncodeImei(string imei)
	{
		byte[] array = imei.FixedLength(16).HexToBytes();
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].SwapNibbles();
		}
		return array;
	}

	private void WriteNvData(byte[] imei, int recId, string fileName)
	{
		string value = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
		string value2 = "<NVRamEditor Version=\"1.0\">";
		string format = "\t<NVRamData LID=\"NVRAM_EF_IMEI_IMEISV_LID\">\r\n\t\t<Record RecID=\"{0}\">";
		string format2 = "\t\t\t<Field Name=\"imei[{0}]\" Type=\"unsigned char\" Size=\"1\" Val=\"0x{1:x2}\"/>";
		string value3 = "\t\t\t<Field Name=\"svn\" Type=\"unsigned char\" Size=\"1\" Val=\"0\"/>";
		string value4 = "\t\t\t<Field Name=\"pad\" Type=\"unsigned char\" Size=\"1\" Val=\"0\"/>";
		string value5 = "\t\t</Record>\r\n\t</NVRamData>\r\n</NVRamEditor>";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(value);
		stringBuilder.AppendLine(value2);
		stringBuilder.AppendLine(string.Format(format, recId));
		for (int i = 0; i < imei.Length; i++)
		{
			stringBuilder.AppendLine(string.Format(format2, i, imei[i]));
		}
		stringBuilder.AppendLine(value3);
		stringBuilder.AppendLine(value4);
		stringBuilder.AppendLine(value5);
		File.WriteAllText(Path.Combine(MetaPath, fileName), stringBuilder.ToString());
	}

	private void WriteInf(bool dual)
	{
		string path = "alpha.inf";
		string value = "[Connection]";
		string value2 = "PlatformType=DUT in META Mode";
		string value3 = "ComPortType=USB";
		string text = "ComPort=";
		string value4 = "EnableADBService=1";
		string value5 = "EnableSwitchModemMode=0";
		string value6 = "[Modem1Action1]";
		string value7 = "SoftWareType=";
		string value8 = "DB=Load from target";
		string text2 = "ImportNvramFilePath=";
		string path2 = "imei.nvData";
		string value9 = "WriteNvram=1";
		string value10 = "[Modem1Action2]";
		string path3 = "imei2.nvData";
		string value11 = "[Disconnection]";
		string value12 = "AfterDisconnect=Reboot";
		using StreamWriter streamWriter = new StreamWriter(Path.Combine(MetaPath, path), append: false);
		streamWriter.WriteLine(value);
		streamWriter.WriteLine(value2);
		streamWriter.WriteLine(value3);
		streamWriter.WriteLine(text + Portinfo.COM);
		streamWriter.WriteLine(value4);
		streamWriter.WriteLine(value5);
		streamWriter.WriteLine(value6);
		streamWriter.WriteLine(value7);
		streamWriter.WriteLine(value8);
		streamWriter.WriteLine(text2 + Path.Combine(MetaPath, path2));
		streamWriter.WriteLine(value9);
		if (dual)
		{
			streamWriter.WriteLine(value10);
			streamWriter.WriteLine(value7);
			streamWriter.WriteLine(value8);
			streamWriter.WriteLine(text2 + Path.Combine(MetaPath, path3));
			streamWriter.WriteLine(value9);
		}
		streamWriter.WriteLine(value11);
		streamWriter.WriteLine(value12);
	}

	private void CreateINF_ONE(string imei)
	{
		string fileName = "imei.nvData";
		WriteNvData(EncodeImei(imei), 1, fileName);
	}

	private void CreateINF_TWO(string imei)
	{
		string fileName = "imei2.nvData";
		WriteNvData(EncodeImei(imei), 2, fileName);
	}

	private void CreateINF_Dual(string imei1, string imei2)
	{
		string fileName = "imei.nvData";
		string fileName2 = "imei2.nvData";
		WriteNvData(EncodeImei(imei1), 1, fileName);
		WriteNvData(EncodeImei(imei2), 2, fileName2);
	}

	private void IMEI2INF(string imei)
	{
		CreateINF_ONE(imei);
		WriteInf(dual: false);
	}

	private void IMEIDUALINF(string imei1, string imei2)
	{
		CreateINF_Dual(imei1, imei2);
		WriteInf(dual: true);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
