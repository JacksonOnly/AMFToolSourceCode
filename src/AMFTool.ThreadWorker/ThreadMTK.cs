using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AMFTool.Bootinfo;
using AMFTool.Library;
using AMFTool.Library.Mediatek;
using AMFTool.Library.Mediatek.Config;
using AMFTool.Library.Mediatek.Download_Agent;
using AMFTool.Library.Mediatek.Download_Agent.xflash;
using AMFTool.Library.Mediatek.Download_Agent.xml;
using AMFTool.Library.Usb;
using AMFTool.PartitionManager;

namespace AMFTool.ThreadWorker;

public class ThreadMTK
{
	public MtkPreloader Preloader;

	public MtkConfig Config;

	public MtkClass Mtk;

	public MtkDaHandler DaHandler;

	public MtkDaLoader DaLoader;

	public DAConfig Dacfg;

	public DAXFlash DAXFlash;

	public DAXML DAXml;

	public (byte[], Gpt) partition;

	public static List<Partf> m_root = new List<Partf>();

	private Pgpt Pgpt;

	private Scatter Scatter;

	private string PartType;

	private byte[] Imei1;

	private byte[] Imei2;

	public ThreadMTK()
	{
		string partType = "user";
		object obj = null;
		PartType = partType;
		Imei1 = (byte[])obj;
		Imei2 = (byte[])obj;

		Config = new MtkConfig();
		Preloader = new MtkPreloader(Config);
		DaLoader = new MtkDaLoader();
		Mtk = new MtkClass(this);
		Dacfg = new DAConfig(Config);
		DaLoader.Dacfg = Dacfg;
		DaLoader.Mtk = Mtk;
		Config.SetDAConfig(Dacfg);
		DaHandler = new MtkDaHandler(Mtk);
		Pgpt = new Pgpt();
		Scatter = new Scatter();
	}

	private void ResetVariable()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Config = new MtkConfig();
		Preloader = new MtkPreloader(Config);
		DaLoader = new MtkDaLoader();
		Mtk = new MtkClass(this);
		Dacfg = new DAConfig(Config);
		DaLoader.Dacfg = Dacfg;
		DaLoader.Mtk = Mtk;
		Config.SetDAConfig(Dacfg);
		DaHandler = new MtkDaHandler(Mtk);
	}

	public bool Init()
	{
		int result = 0;
		int num = 1;
		string text = "preloader";
		string text2 = "";
		string text3 = "pgpt";
		string value = "userdata";
		string text4 = "V5 2025";
		string msg = "Failed!";
		string text5 = "Error : ";
		try
		{
			if (!DeviceState.DeviceInit)
			{
				ResetVariable();
				if (!Initializing())
				{
					SerialMTK.Flush();
					SerialMTK.Close();
					return false;
				}
			}
			switch (State.Job)
			{
			case (Job)2:
				ShowPartition();
				return true;
			case Job.Readback:
			{
				int num3 = 2;
				List<Partf> partEntries2 = partition.Item2.PartEntries;
				ListPartition.List.Add(new ListPartition(0, text, text, 0L, Dacfg.Boot1Size, text2));
				ListPartition.List.Add(new ListPartition(1, text3, text3, 0L, Dacfg.Boot1Size / 128L, text2));
				foreach (Partf item in partEntries2)
				{
					num3++;
					if (!item.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						ListPartition.List.Add(new ListPartition(num3, item.Name, Scatter.Partmtk(item.Name) ?? text2, (long)item.Sector * (long)Dacfg.PageSize, (long)item.Sectors * (long)Dacfg.PageSize, text2));
					}
				}
				ReadFirmware(ListPartition.List);
				break;
			}
			case Job.Flash:
				WriteFlashMode(MTKList.Partition);
				break;
			case (Job)6:
			{
				int num2 = 2;
				List<Partf> partEntries = partition.Item2.PartEntries;
				ListPartition.List.Add(new ListPartition(0, text, text, 0L, Dacfg.Boot1Size, text2));
				foreach (Partf item2 in partEntries)
				{
					if (Scatter.FileSupport(item2.Name))
					{
						num2++;
						if (!item2.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							ListPartition.List.Add(new ListPartition(num2, item2.Name, Scatter.Partmtk(item2.Name) ?? text2, (long)item2.Sector * (long)Dacfg.PageSize, (long)item2.Sectors * (long)Dacfg.PageSize, text2));
						}
					}
				}
				ReadFirmware(ListPartition.List);
				break;
			}
			case Job.Frp:
				ResetFRPnew(PartitionSet.MTKFrp);
				break;
			case (Job)80:
				return DaLoader.Shutdown();
			case Job.RebuildUserDATA:
				RebuildUserDATA(PartitionSet.RebuildUserDATA);
				break;
			case (Job)72:
				ReadDumo(State.SavePath);
				break;
			case Job.Network:
				UnlockNetworkOppoOperation();
				break;
			case Job.ImeiWrite:
				if (State.ImeiType != IMEIType.Vivo)
				{
					ReadWriteNVOperation(read: true, openpath: false);
				}
				UI.RichLogs(text2, UI.log, nextline: true);
				IMEIOperation();
				break;
			case Job.BLLock:
				BootloaderOperation(unlock: false);
				break;
			case (Job)54:
				BootloaderOperation();
				break;
			case Job.Payjoy:
			case (Job)70:
				ProinfoOperation();
				break;
			case Job.RemoveDemo:
				RemoveDemo();
				break;
			case Job.Vbmeta:
				VBMetaOperation();
				break;
			case (Job)68:
			case (Job)78:
				LKOperation();
				break;
			case (Job)58:
			case Job.ID:
				ErasePartition(PartitionSet.MTKID);
				break;
			case (Job)56:
				DisableMiCloudOperation();
				break;
			case Job.RPMBRestore:
			case (Job)44:
			case Job.RPMBErase:
				RPMBOperation();
				break;
			case (Job)40:
				ReadWriteNVOperation(read: false);
				break;
			case Job.NVBackup:
				ReadWriteNVOperation(read: true);
				break;
			case (Job)42:
				ErasePartition(PartitionSet.MTKNVErase);
				break;
			case (Job)14:
				ResetFRPOperation(PartitionSet.MTKSafeFormat);
				break;
			case Job.Reset:
				if (DeviceState.Model == text4)
				{
					ResetFRPOperation(PartitionSet.MTKSafeFormat);
				}
				else
				{
					ErasePartition(PartitionSet.MTKReset);
				}
				break;
			case Job.WiteEMI:
				WritePartition(DeviceState.Partitiontarget[0], State.Data);
				break;
			case (Job)8:
				WritePartition(DeviceState.Partitiontarget[0], State.FilePath);
				return true;
			case Job.Erase:
				ErasePartition(DeviceState.Partitiontarget);
				return true;
			case (Job)10:
				ReadPartitionBulk(DeviceState.Partitiontarget, State.SavePath, display: true, isBulk: true);
				break;
			case Job.Read:
				ReadPartition(DeviceState.Partitiontarget, State.SavePath);
				return true;
			}
			return DaLoader.Shutdown();
		}
		catch (Exception ex)
		{
			UI.RichLogs(msg, UI.error, (byte)num != 0, (byte)num != 0);
			Console.WriteLine(text5 + ex.Message);
			SerialMTK.Flush();
			SerialMTK.Close();
			return (byte)result != 0;
		}
	}

	private bool Initializing()
	{
		string msg = "Waiting Mediatek USB Connection... ";
		string category = "MTK";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "Name : ";
		string msg5 = "Device : ";
		string text = "[VID:";
		string text2 = " PID:";
		string text3 = "]";
		string text4 = "Connecting to device ";
		string text5 = "... ";
		string msg6 = "Handshaking... ";
		object obj = null;
		string text6 = ":";
		string text7 = "Boot Slot : ";
		string msg7 = "Active Boot Slot : ";
		UI.RichLogs(msg, UI.log);
		if (UsbScan.FindUSB(category))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg4, UI.log);
			UI.RichLogs(Portinfo.Name, UI.info, nextline: true, bold: true);
			UI.RichLogs(msg5, UI.log);
			UI.RichLogs(Portinfo.COM, UI.info, nextline: false, bold: true);
			UI.RichLogs(text + Portinfo.VID + text2 + Portinfo.PID + text3, UI.info, nextline: true, bold: true);
			UI.RichLogs(text4 + Portinfo.COM + text5, UI.log);
			if (SerialMTK.Open(Portinfo.COM))
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg6, UI.log);
				if (SerialMTK.Handshake((byte[])obj))
				{
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					if (!DaHandler.ConfigureDA((byte[])obj))
					{
						return false;
					}
					DeviceState.SetInit(Portinfo.VID + text6 + Portinfo.PID + text6 + Portinfo.COM + text6 + Portinfo.Name);
					if (State.Job == Job.Flash)
					{
						return true;
					}
					if (!ReadGPT())
					{
						return false;
					}
					string currentSlot = GetCurrentSlot();
					Console.WriteLine(text7 + currentSlot);
					if (!string.IsNullOrEmpty(currentSlot))
					{
						UI.RichLogs(msg7, UI.log);
						UI.RichLogs(currentSlot, UI.info, nextline: true, bold: true);
						return ReadDeviceInfo();
					}
					return ReadDeviceInfo();
				}
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		return false;
	}

	private bool ReadDeviceInfo()
	{
		string msg = "Reading Device Info : ";
		string partitionName = "recovery";
		string msg2 = "[RECOVERY]";
		string format = "SETATUS READINFO RECOVERY {0}";
		string msg3 = "[SUPER]";
		string text = "corrupt";
		string msg4 = " IMEI 1 : ";
		string msg5 = " IMEI 2 : ";
		string msg6 = "[SYSTEM]";
		if (MTKConfig.Infirmware)
		{
			UI.RichLogs(msg, UI.log);
			byte[] array = ReadPartitionBytes(partitionName);
			if (array != null && array.Length != 0)
			{
				UI.RichLogs(msg2, UI.info, nextline: true, bold: true);
				AMFTool.Bootinfo.Bootinfo.extract(array);
			}
			else
			{
				MTKConfig.Infodevice = true;
			}
			Console.WriteLine(string.Format(format, MTKConfig.Infodevice));
			if (MTKConfig.Infodevice)
			{
				if (DetectSuper(out var superSector, out var superSectors))
				{
					UI.RichLogs(msg3, UI.info, nextline: true, bold: true);
					string data = DeviceInfoReader.ReadFromSuper(superSector, superSectors, Dacfg.PageSize, DaLoader);
					Dictionary<string, string> dictionary = ParseBuildProp(data);
					if (dictionary.Count != 0)
					{
						ShowDeviceInfo(dictionary);
						if (DetectNvdata(out var nvdatasector, out var nvdatasectors))
						{
							string[] array2 = DeviceInfoReader.ReadImeiFromNvdata(nvdatasector, nvdatasectors, Dacfg.PageSize, DaLoader);
							if (array2 != null && array2[0] != text)
							{
								UI.RichLogs(msg4, UI.log);
								UI.RichLogs(array2[0], UI.info, nextline: true, bold: true);
								UI.RichLogs(msg5, UI.log);
								UI.RichLogs(array2[1], UI.info, nextline: true, bold: true);
							}
						}
						return true;
					}
				}
				if (DetectSystem(out var systemSector, out var systemSectors))
				{
					UI.RichLogs(msg6, UI.info, nextline: true, bold: true);
					string data2 = DeviceInfoReader.ReadFromNormalPartition(systemSector, systemSectors, Dacfg.PageSize, DaLoader);
					Dictionary<string, string> dictionary2 = ParseBuildProp(data2);
					if (dictionary2.Count != 0)
					{
						ShowDeviceInfo(dictionary2);
						if (DetectNvdata(out var nvdatasector2, out var nvdatasectors2))
						{
							string[] array3 = DeviceInfoReader.ReadImeiFromNvdata(nvdatasector2, nvdatasectors2, Dacfg.PageSize, DaLoader);
							if (array3 != null && array3[0] != text)
							{
								UI.RichLogs(msg4, UI.log);
								UI.RichLogs(array3[0], UI.info, nextline: true, bold: true);
								UI.RichLogs(msg5, UI.log);
								UI.RichLogs(array3[1], UI.info, nextline: true, bold: true);
							}
						}
						return true;
					}
				}
			}
		}
		return true;
	}

	private bool DetectSuper(out long superSector, out long superSectors)
	{
		string text = "super";
		superSector = 0L;
		superSectors = 0L;
		string[] array = new string[1] { text };
		foreach (string text2 in array)
		{
			(bool, string, long, long) tuple = DetectPartition(text2);
			var (flag, _, _, _) = tuple;
			_ = tuple.Item2;
			long item = tuple.Item3;
			long item2 = tuple.Item4;
			if (flag && text2.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				superSector = item;
				superSectors = item2;
				return true;
			}
		}
		return false;
	}

	private bool DetectSystem(out long systemSector, out long systemSectors)
	{
		string text = "system";
		string text2 = "system_a";
		string text3 = "system_b";
		string text4 = "android";
		systemSector = 0L;
		systemSectors = 0L;
		string[] array = new string[4] { text, text2, text3, text4 };
		int num = 0;
		long item;
		long item2;
		while (true)
		{
			if (num >= array.Length)
			{
				return false;
			}
			string partitionName = array[num];
			(bool, string, long, long) tuple = DetectPartition(partitionName);
			var (flag, _, _, _) = tuple;
			_ = tuple.Item2;
			item = tuple.Item3;
			item2 = tuple.Item4;
			if (flag)
			{
				break;
			}
			num++;
		}
		systemSector = item;
		systemSectors = item2;
		return true;
	}

	private bool DetectNvdata(out long nvdatasector, out long nvdatasectors)
	{
		string text = "nvdata";
		nvdatasector = 0L;
		nvdatasectors = 0L;
		string[] array = new string[1] { text };
		foreach (string text2 in array)
		{
			(bool, string, long, long) tuple = DetectPartition(text2);
			var (flag, _, _, _) = tuple;
			_ = tuple.Item2;
			long item = tuple.Item3;
			long item2 = tuple.Item4;
			if (flag && text2.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				nvdatasector = item;
				nvdatasectors = item2;
				return true;
			}
		}
		return false;
	}

	public Dictionary<string, string> ParseBuildProp(string data)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		if (!string.IsNullOrWhiteSpace(data))
		{
			string[] array = data.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text.Length != 0 && text[0] != '#')
				{
					int num = text.IndexOf('=');
					if (num > 0)
					{
						dictionary[text.Substring(0, num)] = text.Substring(num + 1);
					}
				}
			}
			return dictionary;
		}
		return dictionary;
	}

	private void ShowDeviceInfo(Dictionary<string, string> props)
	{
		string item = "Model Name";
		string text = "ro.product.brand";
		string text2 = "ro.product.bootimage.model";
		string text3 = "ro.product.product.brand";
		string text4 = "ro.product.odm.brand";
		string text5 = "ro.product.vendor.brand";
		string text6 = "ro.product.system_ext.brand";
		string text7 = "ro.dolby.brand";
		string text8 = "ro.product.bootimage.brand";
		string item2 = "Product Name";
		string text9 = "ro.product.device";
		string text10 = "ro.product.product.device";
		string text11 = "ro.product.system_ext.device";
		string text12 = "ro.product.system.device";
		string text13 = "ro.product.mod_device";
		string text14 = "ro.miui.cust_device";
		string text15 = "ro.product.vendor.device";
		string text16 = "ro.vivo.product.version";
		string text17 = "ro.product.board";
		string text18 = "ro.product.bootimage.device";
		string item3 = "Manufacturer";
		string text19 = "ro.product.manufacturer";
		string text20 = "ro.product.vendor.manufacturer";
		string text21 = "ro.product.odm.manufacturer";
		string text22 = "ro.product.product.manufacturer";
		string text23 = "ro.product.system_ext.manufacturer";
		string text24 = "ro.product.system.manufacturer";
		string text25 = "ro.dolby.manufacturer";
		string text26 = "ro.product.bootimage.manufacturer";
		string item4 = "Platform";
		string text27 = "ro.board.platform";
		string text28 = "ro.vivo.product.platform";
		string text29 = "ro.mediatek.platform";
		string item5 = "Model";
		string text30 = "ro.product.model";
		string text31 = "ro.product.product.model";
		string text32 = "ro.product.system_ext.model";
		string text33 = "ro.product.system.model";
		string text34 = "ro.product.vendor.model";
		string text35 = "ro.product.odm.model";
		string text36 = "ro.vivo.product.release.model";
		string text37 = "ro.vivo.product.model";
		string item6 = "Name";
		string text38 = "ro.product.product.name";
		string text39 = "ro.product.system_ext.name";
		string text40 = "ro.product.system.name";
		string text41 = "ro.product.name";
		string text42 = "ro.vivo.product.release.name";
		string text43 = "ro.product.bootimage.tran.device.name.default";
		string text44 = "ro.product.bootimage.name";
		string item7 = "Market name";
		string text45 = "ro.oppo.market.name";
		string text46 = "ro.vendor.oplus.market.name";
		string text47 = "ro.product.product.marketname";
		string text48 = "ro.product.system_ext.marketname";
		string text49 = "ro.product.system.marketname";
		string item8 = "SDK ver";
		string text50 = "ro.build.version.sdk";
		string text51 = "ro.vendor.build.version.sdk";
		string text52 = "ro.product.build.version.sdk";
		string text53 = "ro.bootimage.build.version.sdk";
		string item9 = "Android Version";
		string text54 = "ro.odm.build.version.release";
		string text55 = "ro.product.build.version.release";
		string text56 = "ro.build.version.release";
		string text57 = "ro.vivo.os.version";
		string text58 = "ro.bootimage.build.version.release";
		string text59 = "ro.bootimage.build.version.release_or_codename";
		string item10 = "Security Patch";
		string text60 = "ro.build.version.security_patch";
		string text61 = "ro.vendor.build.security_patch";
		string text62 = "ro.bootimage.build.security_patch";
		string item11 = "Build ID";
		string text63 = "ro.system.build.version.incremental";
		string text64 = "ro.build.version.incremental";
		string text65 = "ro.vendor.build.version.incremental";
		string item12 = "MIUI Region";
		string item13 = "MIUI Version";
		string text66 = "ro.miui.ui.version.name";
		string item14 = "Vivo_ver";
		string text67 = "ro.vivo.system.product.version";
		string item15 = "Version viv";
		string text68 = "ro.vivo.default.version";
		string item16 = "Hardware";
		string text69 = "ro.build.expect.hardware";
		string item17 = "Versionn";
		string text70 = "ro.build.display.ota";
		string text71 = "ro.build.ota.versionnam";
		string item18 = "MY_manifest";
		string text72 = "ro.oplus.version.my_manifest";
		string text73 = "ro.oplus.image.my_manifest.version";
		string item19 = "oppover";
		string text74 = "ro.build.display.id_ims";
		string text75 = "ro.build.display.id";
		string text76 = "ro.build.display.full_id";
		string item20 = "Full_id";
		string text77 = "ro.vendor.vivo.product.version";
		string text78 = "ro.vro.vendor.vivo.product.versionendor.build.id";
		string item21 = "Release_or";
		string text79 = "ro.system.build.version.release_or_codename";
		string text80 = "ro.hardware.bbk";
		string text81 = "ro.build.PDA";
		string text82 = "ro.vivo.hardware.version";
		string text83 = "ro.product.model.bbk";
		string item22 = "Market.name";
		string text84 = "ro.build.version.ota";
		string item23 = "Build";
		string text85 = "ro.build.date";
		string text86 = "ro.product.build.date";
		string text87 = "ro.system.build.date";
		string text88 = "ro.odm.build.date";
		string text89 = "ro.bootimage.build.date";
		string item24 = "Print";
		string text90 = "ro.build.fingerprint";
		string text91 = "ro.build.version.base_os";
		string text92 = "ro.build.description";
		string text93 = "ro.build.representative.fingerprint";
		string text94 = " ";
		string text95 = " : ";
		string oldValue = ":user/release-keys";
		string newValue = "";
		string oldValue2 = "release-keys";
		Dictionary<string, string> props2 = props;
		if (props2 == null || props2.Count == 0)
		{
			return;
		}
		(string, string[])[] array = new(string, string[])[24]
		{
			(item, new string[8] { text, text2, text3, text4, text5, text6, text7, text8 }),
			(item2, new string[11]
			{
				text9, text10, text11, text12, text13, text14, text15, text16, text17, text18,
				text16
			}),
			(item3, new string[8] { text19, text20, text21, text22, text23, text24, text25, text26 }),
			(item4, new string[3] { text27, text28, text29 }),
			(item5, new string[9] { text30, text31, text32, text33, text34, text35, text36, text2, text37 }),
			(item6, new string[7] { text38, text39, text40, text41, text42, text43, text44 }),
			(item7, new string[5] { text45, text46, text47, text48, text49 }),
			(item8, new string[4] { text50, text51, text52, text53 }),
			(item9, new string[6] { text54, text55, text56, text57, text58, text59 }),
			(item10, new string[3] { text60, text61, text62 }),
			(item11, new string[3] { text63, text64, text65 }),
			(item12, new string[1] { text13 }),
			(item13, new string[1] { text66 }),
			(item14, new string[1] { text67 }),
			(item15, new string[1] { text68 }),
			(item16, new string[1] { text69 }),
			(item17, new string[2] { text70, text71 }),
			(item18, new string[2] { text72, text73 }),
			(item19, new string[3] { text74, text75, text76 }),
			(item20, new string[2] { text77, text78 }),
			(item21, new string[6] { text79, text44, text80, text81, text82, text83 }),
			(item22, new string[3] { text84, text45, text83 }),
			(item23, new string[5] { text85, text86, text87, text88, text89 }),
			(item24, new string[4] { text90, text91, text92, text93 })
		};
		for (int i = 0; i < array.Length; i++)
		{
			string text96 = pick(array[i].Item2);
			if (!string.IsNullOrEmpty(text96))
			{
				UI.RichLogs(text94 + array[i].Item1.PadRight(0) + text95, UI.log);
				string text97 = text96.Replace(oldValue, newValue);
				string text98 = text97.Replace(oldValue2, newValue);
				UI.RichLogs(text98.ToUpperInvariant(), UI.info, nextline: true, bold: true);
			}
		}
		string pick(params string[] keys)
		{
			object result = null;
			for (int j = 0; j < keys.Length; j++)
			{
				if (props2.TryGetValue(keys[j], out var value) && !string.IsNullOrEmpty(value))
				{
					return value;
				}
			}
			return (string)result;
		}
	}

	private void ResetFRPOperation(List<string> partition)
	{
		object obj = null;
		string text = "para";
		string text2 = "misc";
		string msg = "Reset Lock... ";
		string text3 = "frp";
		string text4 = "";
		string msg2 = "Reset Frp Lock... ";
		string msg3 = "Failed!";
		string msg4 = "OK";
		foreach (string item in partition)
		{
			(bool, string, long, long) tuple = DetectPartition(item);
			var (flag, text5, _, _) = tuple;
			_ = tuple.Item3;
			_ = tuple.Item4;
			if (!flag)
			{
				continue;
			}
			byte[] data = (byte[])obj;
			if (!(text5 == text) && !(text5 == text2))
			{
				if (text5 == text3 || text5 == text4)
				{
					UI.RichLogs(msg2, UI.log);
					data = Tools.format(text5);
				}
			}
			else
			{
				UI.RichLogs(msg, UI.log);
				data = Tools.format(text5);
			}
			if (DaLoader.OtherWriteFlash(text5, string.Empty, data))
			{
				UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			}
		}
	}

	private void ResetFRPnew(List<string> partition)
	{
		object obj = null;
		string text = "frp";
		string text2 = "config";
		string text3 = "persistent";
		string msg = "Reset Frp... ";
		string msg2 = "OK";
		string msg3 = "Erase Frp... ";
		string msg4 = "Failed!";
		foreach (string item in partition)
		{
			(bool, string, long, long) tuple = DetectPartition(item);
			var (flag, text4, _, _) = tuple;
			_ = tuple.Item3;
			_ = tuple.Item4;
			if (!flag)
			{
				continue;
			}
			byte[] array = (byte[])obj;
			if (text4 == text || text4 == text2 || text4 == text3)
			{
				UI.RichLogs(msg, UI.log);
				array = Tools.format(text4);
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg3, UI.log);
				if (!DaLoader.OtherWriteFlash(text4, string.Empty, array))
				{
					UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
				}
			}
		}
	}

	private void ReadWriteNVOperation(bool read, bool openpath = true)
	{
		int num = 0;
		int num2 = 1;
		string msg = "Save NV... ";
		string format = "NV BACKUP_{0}_{1}_{2:ddMMyyyy_HHmmss}_AMFTool.rar";
		string msg2 = "OK";
		string msg3 = "Saved to : ";
		string msg4 = "Invalid NV backup file";
		string msg5 = "Empty backup";
		int comparisonType = 5;
		List<string> mTKNVBackup = PartitionSet.MTKNVBackup;
		string[] array = mTKNVBackup.Select((string p) => Scatter.Partmtk(p)).ToArray();
		if (read)
		{
			string savePath = State.SavePath;
			for (int num3 = 0; num3 < mTKNVBackup.Count; num3++)
			{
				string item = mTKNVBackup[num3];
				string path = array[num3];
				string text = Path.Combine(savePath, path);
				Console.WriteLine(text);
				ReadPartition(new List<string> { item }, text);
			}
			UI.RichLogs(msg, UI.log);
			string path2 = string.Format(format, Config.Cpu, Config.Meid.ToHexString().ToUpper(), DateTime.Now);
			using (ZipArchive destination = ZipFile.Open(Path.Combine(savePath, path2), ZipArchiveMode.Create))
			{
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (File.Exists(Path.Combine(savePath, text2)))
					{
						destination.CreateEntryFromFile(Path.Combine(savePath, text2), text2, CompressionLevel.Optimal);
						File.Delete(Path.Combine(savePath, text2));
					}
				}
			}
			UI.RichLogs(msg2, UI.ok, (byte)num2 != 0, (byte)num2 != 0);
			UI.RichLogs(msg3, UI.log, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(savePath, UI.info, (byte)num2 != 0, (byte)num2 != 0);
			if (openpath)
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = savePath,
					UseShellExecute = ((byte)num2 != 0)
				});
			}
			return;
		}
		if (string.IsNullOrEmpty(State.FilePath) || !File.Exists(State.FilePath))
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
			return;
		}
		using ZipArchive zipArchive = ZipFile.OpenRead(State.FilePath);
		if (zipArchive.Entries.Count != 0)
		{
			foreach (ZipArchiveEntry entry in zipArchive.Entries)
			{
				if ((entry.FullName.EndsWith(entry.FullName, (StringComparison)comparisonType) ? 1 : 0) != num)
				{
					string partitionName = Path.GetFileNameWithoutExtension(entry.FullName).ToLower();
					using Stream stream = entry.Open();
					using MemoryStream memoryStream = new MemoryStream();
					stream.CopyTo(memoryStream);
					WritePartition(partitionName, memoryStream.ToArray(), (byte)num2 != 0);
				}
			}
			return;
		}
		UI.RichLogs(msg5, UI.error, nextline: true, bold: true);
	}

	private bool IMEIOperation()
	{
		return State.ImeiType switch
		{
			(IMEIType)0 => IMEIRepairOppo(), 
			IMEIType.Xiaomi => IMEIRepairXiaomi(), 
			(IMEIType)2 => IMEIRepairInfinix(), 
			IMEIType.Vivo => false, 
			(IMEIType)4 => IMEIRepairGeneric(), 
			_ => false, 
		};
	}

	private void LKOperation()
	{
		string msg = "Patch DM-Verity & Orange msg... ";
		int num = 1;
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		foreach (string item in PartitionSet.MTKLK)
		{
			byte[] array = ReadPartitionBytes(item);
			if (array != null && array.Length != 0)
			{
				byte[] file = Tools.patchlk(array);
				WritePartition(item, file, display: false);
			}
		}
		UI.RichLogs(msg2, UI.ok, (byte)num != 0, (byte)num != 0);
	}

	private void VBMetaOperation()
	{
		string msg = "Patching vbmeta... ";
		string keyword = "vbmeta";
		int num = 1;
		string msg2 = "0x02";
		UI.RichLogs(msg, UI.log);
		List<(string, long, long)> list = FindPartition(keyword);
		if (list.Count == 0)
		{
			return;
		}
		foreach (var item4 in list)
		{
			string item = item4.Item1;
			long item2 = item4.Item2;
			long item3 = item4.Item3;
			byte[] array = DaLoader.ReadFlashByte(item2 * Dacfg.PageSize, item3 * Dacfg.PageSize, PartType);
			if (array != null && array.Length != 0)
			{
				byte[] data = Tools.PatchVBMeta(array, 2);
				DaLoader.WriteFlash(item2 * Dacfg.PageSize, item3 * Dacfg.PageSize, item, data, PartType);
			}
		}
		UI.RichLogs(msg2, UI.ok, (byte)num != 0, (byte)num != 0);
	}

	private void RemoveDemo()
	{
		string msg = "Removing Demo... ";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ErasePartition(PartitionSet.MTKDemo, display: false);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void UnlockNetworkOppoOperation()
	{
		string msg = "Reading ";
		string msg2 = "Simlock ";
		string msg3 = "... ";
		string partitionName = "protect1";
		string msg4 = "Failed!";
		string msg5 = "OK";
		string msg6 = "Writing data... ";
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(msg2, UI.info, nextline: false, bold: true);
		UI.RichLogs(msg3, UI.info, nextline: false, bold: true);
		byte[] array = ReadPartitionBytes(partitionName);
		if (array != null && array.Length != 0)
		{
			UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
			byte[] array2 = Tools.PatchSimLock(array);
			if (array2 == null || array2.Length == 0)
			{
				UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
				return;
			}
			UI.RichLogs(msg6, UI.log);
			bool flag;
			if (flag = WritePartition(partitionName, array2, display: false))
			{
				UI.RichLogs(msg5, flag ? UI.ok : UI.error, nextline: true);
			}
			else
			{
				UI.RichLogs(msg4, flag ? UI.ok : UI.error, nextline: true);
			}
		}
		else
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
	}

	private void RebuildUserDATA(List<string> partitionName, bool display = true)
	{
		string value = "para";
		string msg = "Erasing ";
		string text = "Data";
		string msg2 = "... ";
		string text2 = "Failed!";
		string text3 = "OK";
		string text4 = "userdata";
		string msg3 = "Writing ";
		string path = "img";
		string path2 = "userdata.img";
		foreach (string item3 in partitionName)
		{
			if (item3.Contains(value))
			{
				(bool, string, long, long) tuple = DetectPartition(item3);
				var (flag, _, _, _) = tuple;
				_ = tuple.Item2;
				long item = tuple.Item3;
				long item2 = tuple.Item4;
				if (flag)
				{
					if (display)
					{
						UI.RichLogs(msg, UI.log);
						UI.RichLogs(text, UI.info, nextline: false, bold: true);
						UI.RichLogs(msg2, UI.log);
					}
					bool flag2 = DaLoader.FormatFlash(item * Dacfg.PageSize, item2 * Dacfg.PageSize, PartType);
					if (display)
					{
						UI.RichLogs(flag2 ? text3 : text2, flag2 ? UI.ok : UI.error, nextline: true);
					}
				}
			}
			else if (item3.Contains(text4))
			{
				UI.RichLogs(msg3, UI.log);
				UI.RichLogs(text, UI.info, nextline: false, bold: true);
				UI.RichLogs(msg2, UI.log);
				byte[] file = File.ReadAllBytes(Path.Combine(Application.StartupPath, text, path, path2));
				bool num = WritePartition(text4, file, display: false);
				UI.RichLogs(color: num ? UI.ok : UI.error, msg: (!num) ? text2 : text3, nextline: true);
			}
		}
	}

	private void DisableMiCloudOperation()
	{
		string msg = "Reading ";
		string msg2 = "MiCloud ";
		string msg3 = "data... ";
		string partitionName = "persist";
		string msg4 = "Failed!";
		string msg5 = "OK";
		string msg6 = "Patching MiCloud... ";
		string item = "fdsd";
		string item2 = "ftst";
		string msg7 = "lock flag not found!";
		string msg8 = "Writing data... ";
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(msg2, UI.info, nextline: false, bold: true);
		UI.RichLogs(msg3, UI.log);
		byte[] array = ReadPartitionBytes(partitionName);
		if (array != null && array.Length != 0)
		{
			UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg6, UI.log);
			var (flag, file) = Tools.FindMiCloudVuln(array, new List<(string, string)> { (item, item2) });
			if (flag)
			{
				UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg8, UI.log);
				bool flag2;
				if (!(flag2 = WritePartition(partitionName, file, display: false)))
				{
					UI.RichLogs(msg4, flag2 ? UI.ok : UI.error, nextline: true);
				}
				else
				{
					UI.RichLogs(msg5, flag2 ? UI.ok : UI.error, nextline: true);
				}
			}
			else
			{
				UI.RichLogs(msg7, UI.error, nextline: true, bold: true);
			}
		}
		else
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
	}

	private bool BootloaderOperation(bool unlock = true)
	{
		string msg = "Initializing seccfg... ";
		string partitionName = "seccfg";
		string msg2 = "Couldn't detect existing seccfg partition. Aborting.";
		string text = "OK";
		string text2 = "AND_SECCFG_v";
		string msg3 = "Unknown seccfg format";
		string text3 = "Relocking Bootloader... ";
		string text4 = "Unlocking Bootloader... ";
		string text5 = "lock";
		string text6 = "unlock";
		string text7 = "Failed!";
		ErasePartition(PartitionSet.MTKUBL);
		UI.RichLogs(msg, UI.log);
		byte[] array = ReadPartitionBytes(partitionName);
		if (array != null && array.Length >= 16)
		{
			UI.RichLogs(text, UI.ok, nextline: true, bold: true);
			bool flag = array.Length >= 12 && Encoding.ASCII.GetString(array, 0, 12) == text2;
			bool flag2 = array.Length >= 4 && array[0] == 77 && array[1] == 77 && array[2] == 77 && array[3] == 77;
			if (!flag && !flag2)
			{
				UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(unlock ? text4 : text3, UI.log);
			var (flag3, array2) = DaLoader.XFlashExt.Seccfg(array, unlock ? text6 : text5);
			if (!flag3 || array2 == null || array2.Length == 0)
			{
				return false;
			}
			bool flag4;
			UI.RichLogs((!(flag4 = WritePartition(partitionName, array2, display: false))) ? text7 : text, flag4 ? UI.ok : UI.error, nextline: true, bold: true);
			return flag4;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		return false;
	}

	private void RPMBOperation()
	{
		string text = "Erasing RPMB... ";
		string text2 = "Reading RPMB... ";
		string text3 = "Writing RPMB... ";
		object obj = null;
		string msg = "Failed!";
		string msg2 = "OK";
		switch (State.Job)
		{
		default:
		{
			string msg3 = (string)obj;
			UI.RichLogs(msg3, UI.log);
			break;
		}
		case Job.RPMBRestore:
		{
			string msg3 = text3;
			UI.RichLogs(msg3, UI.log);
			break;
		}
		case (Job)44:
		{
			string msg3 = text2;
			UI.RichLogs(msg3, UI.log);
			break;
		}
		case Job.RPMBErase:
		{
			string msg3 = text;
			UI.RichLogs(msg3, UI.log);
			break;
		}
		}
		bool flag = State.Job switch
		{
			Job.RPMBRestore => DaLoader.WriteRPBM(State.FilePath), 
			(Job)44 => DaLoader.ReadRPMB(State.SavePath), 
			Job.RPMBErase => DaLoader.EraseRPMB(), 
			_ => false, 
		};
		if (!flag)
		{
			UI.RichLogs(msg, flag ? UI.ok : UI.error, nextline: true);
		}
		else
		{
			UI.RichLogs(msg2, flag ? UI.ok : UI.error, nextline: true);
		}
	}

	private void ProinfoOperation()
	{
		string msg = "Reading ";
		string msg2 = "Lock ";
		string msg3 = "data... ";
		string partitionName = "proinfo";
		string msg4 = "Failed!";
		string msg5 = "OK";
		object obj = null;
		string msg6 = "Removing Payjoy Lock... ";
		string msg7 = "Removing Security Plugin Lock... ";
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(msg2, UI.info, nextline: false, bold: true);
		UI.RichLogs(msg3, UI.info, nextline: false, bold: true);
		byte[] array = ReadPartitionBytes(partitionName);
		if (array != null && array.Length != 0)
		{
			UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
			byte[] array2 = (byte[])obj;
			if (State.Job != Job.Payjoy)
			{
				bool flag;
				if (State.Job == (Job)70)
				{
					UI.RichLogs(msg7, UI.log);
					array2 = Tools.PatchSecurityPlugin(array);
					if (array2 == null || array2.Length == 0)
					{
						UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
					}
					else if (flag = WritePartition(partitionName, array2, display: false))
					{
						UI.RichLogs(msg5, flag ? UI.ok : UI.error, nextline: true);
					}
					else
					{
						UI.RichLogs(msg4, flag ? UI.ok : UI.error, nextline: true);
					}
				}
				else if (array2 == null || array2.Length == 0)
				{
					UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
				}
				else if (flag = WritePartition(partitionName, array2, display: false))
				{
					UI.RichLogs(msg5, flag ? UI.ok : UI.error, nextline: true);
				}
				else
				{
					UI.RichLogs(msg4, flag ? UI.ok : UI.error, nextline: true);
				}
			}
			else
			{
				UI.RichLogs(msg6, UI.log);
				array2 = Tools.PatchPayjoy(array);
				bool flag;
				if (array2 == null || array2.Length == 0)
				{
					UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
				}
				else if (flag = WritePartition(partitionName, array2, display: false))
				{
					UI.RichLogs(msg5, flag ? UI.ok : UI.error, nextline: true);
				}
				else
				{
					UI.RichLogs(msg4, flag ? UI.ok : UI.error, nextline: true);
				}
			}
		}
		else
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
	}

	private bool IMEIRepairInfinix()
	{
		string msg = "Reading Device IMEI... ";
		string partitionName = "nvdata";
		string partitionName2 = "nvram";
		string text = "corrupt";
		string msg2 = "Device NV is abnormal";
		string text2 = "OK";
		string msg3 = "[Device IMEI Information]";
		string msg4 = "IMEI [1] : ";
		string msg5 = "IMEI [2] : ";
		string msg6 = "NEW IMEI : ";
		string msg7 = "IMEI 1 : ";
		string msg8 = "IMEI 2 : ";
		string msg9 = "Encrypt IMEI... ";
		string text3 = "Failed!";
		string msg10 = "Writing IMEI to device... ";
		UI.RichLogs(msg, UI.log);
		byte[] array = ReadPartitionBytes(partitionName);
		byte[] array2 = ReadPartitionBytes(partitionName2);
		string[] array3 = Tools.IMEIDecrypt(array);
		if (!(array3[0] == text))
		{
			UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg3, UI.log, nextline: true);
			UI.RichLogs(msg4, UI.log);
			UI.RichLogs(array3[0], UI.info, nextline: true, bold: true);
			UI.RichLogs(msg5, UI.log);
			UI.RichLogs(array3[1], UI.info, nextline: true, bold: true);
			UI.RichLogs(msg6, UI.log, nextline: true);
			if (DeviceState.CheckedIMEI1)
			{
				UI.RichLogs(msg7, UI.log);
				UI.RichLogs(DeviceState.IMEI1, UI.info, nextline: true, bold: true);
				Imei1 = Tools.IMEIEncrypt(DeviceState.IMEI1);
			}
			if (DeviceState.CheckedIMEI2)
			{
				UI.RichLogs(msg8, UI.log);
				UI.RichLogs(DeviceState.IMEI2, UI.info, nextline: true, bold: true);
				Imei2 = Tools.IMEIEncrypt(DeviceState.IMEI2);
			}
			UI.RichLogs(msg9, UI.log);
			if (Tools.PatchIMEI(array, Imei1, Imei2) && Tools.PatchIMEI(array2, Imei1, Imei2))
			{
				UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg10, UI.log);
				bool num = WritePartition(partitionName, array, display: false) && WritePartition(partitionName2, array2, display: false);
				UI.RichLogs(color: num ? UI.ok : UI.error, msg: num ? text2 : text3, nextline: true);
				return true;
			}
			UI.RichLogs(text3, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		return false;
	}

	private bool IMEIRepairXiaomi()
	{
		string msg = "Reading Device IMEI... ";
		string partitionName = "nvdata";
		string text = "corrupt";
		string msg2 = "Device NV is abnormal";
		string msg3 = "OK";
		string msg4 = "[Device IMEI Information]";
		string msg5 = "IMEI [1] : ";
		string msg6 = "IMEI [2] : ";
		string msg7 = "NEW IMEI : ";
		string msg8 = "IMEI 1 : ";
		string msg9 = "IMEI 2 : ";
		string msg10 = "Encrypt IMEI... ";
		string name = "TEMP";
		string path = "AMFTool";
		object obj = null;
		string msg11 = "Failed!";
		string msg12 = "Writing IMEI to device... ";
		string item = "nvram";
		UI.RichLogs(msg, UI.log);
		byte[] array = ReadPartitionBytes(partitionName);
		string[] array2 = Tools.IMEIDecrypt(array);
		if (!(array2[0] == text))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg4, UI.log, nextline: true);
			UI.RichLogs(msg5, UI.log);
			UI.RichLogs(array2[0], UI.info, nextline: true, bold: true);
			UI.RichLogs(msg6, UI.log);
			UI.RichLogs(array2[1], UI.info, nextline: true, bold: true);
			UI.RichLogs(msg7, UI.log, nextline: true);
			if (DeviceState.CheckedIMEI1)
			{
				UI.RichLogs(msg8, UI.log);
				UI.RichLogs(DeviceState.IMEI1, UI.info, nextline: true, bold: true);
				Imei1 = Tools.IMEIEncrypt(DeviceState.IMEI1);
			}
			if (DeviceState.CheckedIMEI2)
			{
				UI.RichLogs(msg9, UI.log);
				UI.RichLogs(DeviceState.IMEI2, UI.info, nextline: true, bold: true);
				Imei2 = Tools.IMEIEncrypt(DeviceState.IMEI2);
			}
			UI.RichLogs(msg10, UI.log);
			string path2 = Path.Combine(utils.CheckBackupFolder(name), path);
			File.WriteAllBytes(path2, array);
			byte[] array3 = Tools.BuildLD0B(Imei1, Imei2);
			if (array3 != obj)
			{
				if (!Tools.InsertLD0BXiaomi(path2, array3))
				{
					UI.RichLogs(msg11, UI.error, nextline: true, bold: true);
					return false;
				}
				array = File.ReadAllBytes(path2);
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg12, UI.log);
				if (WritePartition(partitionName, array, display: false))
				{
					ErasePartition(new List<string> { item }, display: false);
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					if (File.Exists(path2))
					{
						File.Delete(path2);
					}
					return true;
				}
				UI.RichLogs(msg11, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg11, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		return false;
	}

	private bool IMEIRepairGeneric()
	{
		string msg = "Reading Device IMEI... ";
		string partitionName = "nvdata";
		string partitionName2 = "nvram";
		string text = "corrupt";
		string msg2 = "Device NV is abnormal";
		string text2 = "OK";
		string msg3 = "[Device IMEI Information]";
		string msg4 = "IMEI [1] : ";
		string msg5 = "IMEI [2] : ";
		string msg6 = "NEW IMEI : ";
		string msg7 = "IMEI 1 : ";
		string msg8 = "IMEI 2 : ";
		string msg9 = "Encrypt IMEI... ";
		string name = "TEMP";
		string path = "AMFTool";
		object obj = null;
		string text3 = "Failed!";
		string msg10 = "Writing IMEI to device... ";
		UI.RichLogs(msg, UI.log);
		byte[] array = ReadPartitionBytes(partitionName);
		byte[] nvram = ReadPartitionBytes(partitionName2);
		string[] array2 = Tools.IMEIDecrypt(array);
		if (array2[0] == text)
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg3, UI.log, nextline: true);
		UI.RichLogs(msg4, UI.log);
		UI.RichLogs(array2[0], UI.info, nextline: true, bold: true);
		UI.RichLogs(msg5, UI.log);
		UI.RichLogs(array2[1], UI.info, nextline: true, bold: true);
		UI.RichLogs(msg6, UI.log, nextline: true);
		if (DeviceState.CheckedIMEI1)
		{
			UI.RichLogs(msg7, UI.log);
			UI.RichLogs(DeviceState.IMEI1, UI.info, nextline: true, bold: true);
			Imei1 = Tools.IMEIEncrypt(DeviceState.IMEI1);
		}
		if (DeviceState.CheckedIMEI2)
		{
			UI.RichLogs(msg8, UI.log);
			UI.RichLogs(DeviceState.IMEI2, UI.info, nextline: true, bold: true);
			Imei2 = Tools.IMEIEncrypt(DeviceState.IMEI2);
		}
		UI.RichLogs(msg9, UI.log);
		string text4 = Path.Combine(utils.CheckBackupFolder(name), path);
		File.WriteAllBytes(text4, array);
		nvram = Tools.BuildNVRAM(nvram, text4);
		if (nvram == obj)
		{
			UI.RichLogs(text3, UI.error, nextline: true, bold: true);
			return false;
		}
		byte[] array3 = Tools.BuildLD0B(Imei1, Imei2);
		if (array3 != obj)
		{
			if (!Tools.RebuildAllMap(text4))
			{
				UI.RichLogs(text3, UI.error, nextline: true, bold: true);
				return false;
			}
			if (Tools.ResetCert(text4, array3))
			{
				array = File.ReadAllBytes(text4);
				UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg10, UI.log);
				bool num = WritePartition(partitionName, array, display: false) && WritePartition(partitionName2, nvram, display: false);
				UI.RichLogs(color: num ? UI.ok : UI.error, msg: (!num) ? text3 : text2, nextline: true);
				if (File.Exists(text4))
				{
					File.Delete(text4);
				}
				return true;
			}
			UI.RichLogs(text3, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(text3, UI.error, nextline: true, bold: true);
		return false;
	}

	private bool IMEIRepairOppo()
	{
		string msg = "Reading Device IMEI... ";
		string partitionName = "nvdata";
		string partitionName2 = "nvram";
		string text = "corrupt";
		string text2 = "OK";
		string msg2 = "[Device IMEI Information]";
		string msg3 = "IMEI [1] : ";
		string msg4 = "IMEI [2] : ";
		string msg5 = "Not Found!, Maybe NV is Abnormal!, Try Continue";
		string msg6 = "NEW IMEI : ";
		string msg7 = "IMEI 1 : ";
		string msg8 = "IMEI 2 : ";
		string text3 = "IMEI1 ";
		string text4 = "IMEI2 ";
		string msg9 = "Encrypt IMEI... ";
		string name = "TEMP";
		string path = "AMFTool";
		object obj = null;
		string text5 = "Failed!";
		string msg10 = "Writing IMEI to device... ";
		string msg11 = "Checking cert... ";
		string partitionName3 = "md1img";
		string partitionName4 = "md1img_a";
		string msg12 = "Patch type: ";
		string msg13 = "Patch Certificate... ";
		UI.RichLogs(msg, UI.log);
		byte[] array = ReadPartitionBytes(partitionName);
		byte[] nvram = ReadPartitionBytes(partitionName2);
		bool flag = true;
		int num = array.IndexOf(new byte[8] { 76, 68, 73, 0, 16, 239, 10, 0 });
		if (num == -1)
		{
			UI.RichLogs(msg5, UI.error, nextline: true, bold: true);
		}
		else
		{
			string[] array2 = Tools.IMEIDecrypt(array);
			if (!(array2[0] != text))
			{
				UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg2, UI.log, nextline: true);
				UI.RichLogs(msg3, UI.log);
				UI.RichLogs(array2[0], UI.info, nextline: true, bold: true);
				UI.RichLogs(msg4, UI.log);
				UI.RichLogs(array2[1], UI.info, nextline: true, bold: true);
				flag = false;
			}
		}
		UI.RichLogs(msg6, UI.log, nextline: true);
		if (DeviceState.CheckedIMEI1)
		{
			UI.RichLogs(msg7, UI.log);
			UI.RichLogs(DeviceState.IMEI1, UI.info, nextline: true, bold: true);
			Imei1 = Tools.IMEIEncrypt(DeviceState.IMEI1);
		}
		if (DeviceState.CheckedIMEI2)
		{
			UI.RichLogs(msg8, UI.log);
			UI.RichLogs(DeviceState.IMEI2, UI.info, nextline: true, bold: true);
			Imei2 = Tools.IMEIEncrypt(DeviceState.IMEI2);
		}
		Console.WriteLine(text3 + Imei1.ToHexString());
		Console.WriteLine(text4 + Imei2.ToHexString());
		UI.RichLogs(msg9, UI.log);
		if (!flag)
		{
			if (!Tools.PatchIMEI(array, Imei1, Imei2))
			{
				UI.RichLogs(text5, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg10, UI.log);
			bool num2 = WritePartition(partitionName, array, display: false);
			UI.RichLogs(color: num2 ? UI.ok : UI.error, msg: num2 ? text2 : text5, nextline: true);
		}
		else
		{
			string text6 = Path.Combine(utils.CheckBackupFolder(name), path);
			File.WriteAllBytes(text6, array);
			nvram = Tools.BuildNVRAM(nvram, text6);
			if (nvram == obj)
			{
				UI.RichLogs(text5, UI.error, nextline: true, bold: true);
				return false;
			}
			byte[] array3 = Tools.BuildLD0B(Imei1, Imei2);
			if (array3 == null || array3.Length == 0)
			{
				UI.RichLogs(text5, UI.error, nextline: true, bold: true);
				return false;
			}
			if (!Tools.InsertLD0BOppo(text6, array3))
			{
				UI.RichLogs(text5, UI.error, nextline: true, bold: true);
				return false;
			}
			array = File.ReadAllBytes(text6);
			UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg10, UI.log);
			bool num3 = WritePartition(partitionName, array, display: false) && WritePartition(partitionName2, nvram, display: false);
			UI.RichLogs(color: num3 ? UI.ok : UI.error, msg: (!num3) ? text5 : text2, nextline: true);
			if (File.Exists(text6))
			{
				File.Delete(text6);
			}
		}
		UI.RichLogs(msg11, UI.log);
		byte[] array4 = ReadPartitionBytes(partitionName3);
		if (array4 == null || array4.Length == 0)
		{
			array4 = ReadPartitionBytes(partitionName4);
		}
		if (array4 == null || array4.Length == 0)
		{
			UI.RichLogs(text5, UI.error, nextline: true, bold: true);
			DaLoader.Shutdown();
			return false;
		}
		int num4 = Tools.CheckCertOPPO(array4);
		UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg12, UI.log);
		UI.RichLogs(num4.ToString(), UI.info, nextline: true, bold: true);
		UI.RichLogs(msg13, UI.log);
		byte[] file = Tools.PatchMd1img(array4);
		bool num5 = WritePartition(partitionName3, file, display: false) || WritePartition(partitionName4, file, display: false);
		UI.RichLogs(color: num5 ? UI.ok : UI.error, msg: num5 ? text2 : text5, nextline: true);
		BootloaderOperation();
		return true;
	}

	private void ShowPartition()
	{
		int num = 0;
		List<Partf> partEntries = partition.Item2.PartEntries;
		foreach (Partf part in partEntries)
		{
			int i = num;
			Main.Instance.MTKDataGrid.BeginInvoke((Action)delegate
			{
				string text = "preloader";
				string text2 = "preloader.bin";
				string text3 = "512KB";
				string text4 = "0x0";
				string format = "0x{0:x}";
				string text5 = "";
				string format2 = "0x{0:X}";
				if (i != 0)
				{
					Main.Instance.MTKDataGrid.Rows.Add(false, part.Name, Scatter.Partmtk(part.Name), utils.FileSize((long)part.Sectors * (long)Dacfg.PageSize), string.Format(format2, part.Sector * (uint)Config.PageSize), string.Format(format2, part.Sectors * (uint)Config.PageSize));
				}
				else
				{
					if (!string.IsNullOrEmpty(DaLoader.PreloaderName))
					{
						Main.Instance.MTKDataGrid.Rows.Add(false, text, DaLoader.PreloaderName ?? text5, DaLoader.PreloaderSize, text4, string.Format(format, Dacfg.Boot1Size));
					}
					else
					{
						Main.Instance.MTKDataGrid.Rows.Add(false, text, text2, text3, text4, string.Format(format, Dacfg.Boot1Size));
					}
					Main.Instance.MTKDataGrid.Rows.Add(false, part.Name, Scatter.Partmtk(part.Name), utils.FileSize((long)part.Sectors * (long)Dacfg.PageSize), string.Format(format2, part.Sector * (uint)Config.PageSize), string.Format(format2, part.Sectors * (uint)Config.PageSize));
				}
			});
			UI.Progresbar1(num, partEntries.Count);
			num++;
		}
	}

	private bool ReadGPT()
	{
		object obj = null;
		string msg = "Reading partition info... ";
		string text = "0";
		string partType = "user";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string text2 = " [";
		string text3 = "]";
		byte[] array;
		Gpt gpt;
		if (m_root == obj)
		{
			UI.RichLogs(msg, UI.log);
			partition = DaLoader.GetGPT(new GptSettings(text, text, text), partType);
			m_root = partition.Item2.PartEntries;
			(array, gpt) = partition;
			if (array != null)
			{
				UI.RichLogs(msg3, UI.ok);
				UI.RichLogs(text2 + m_root.Count + text3, UI.log, nextline: true);
				return true;
			}
			if (gpt == obj)
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg3, UI.ok);
			UI.RichLogs(text2 + m_root.Count + text3, UI.log, nextline: true);
			return true;
		}
		m_root.Clear();
		UI.RichLogs(msg, UI.log);
		partition = DaLoader.GetGPT(new GptSettings(text, text, text), partType);
		m_root = partition.Item2.PartEntries;
		(array, gpt) = partition;
		if (array != null)
		{
			UI.RichLogs(msg3, UI.ok);
			UI.RichLogs(text2 + m_root.Count + text3, UI.log, nextline: true);
			return true;
		}
		if (gpt == obj)
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg3, UI.ok);
		UI.RichLogs(text2 + m_root.Count + text3, UI.log, nextline: true);
		return true;
	}

	private (bool, string, long, long) DetectPartition(string partitionName)
	{
		object obj = null;
		int item = 0;
		int num = -1;
		var (array, gpt) = partition;
		if (array != null || gpt != obj)
		{
			foreach (Partf partEntry in partition.Item2.PartEntries)
			{
				if (partEntry.Name == partitionName)
				{
					return (true, partEntry.Name, (long)partEntry.Sector, (long)partEntry.Sectors);
				}
			}
			return ((byte)item != 0, string.Empty, num, num);
		}
		return (false, string.Empty, -1L, -1L);
	}

	private List<(string name, long sector, long sectors)> FindPartition(string keyword)
	{
		object obj = null;
		List<(string, long, long)> list = new List<(string, long, long)>();
		var (array, gpt) = partition;
		if (array == null && gpt == obj)
		{
			return list;
		}
		foreach (Partf partEntry in partition.Item2.PartEntries)
		{
			if (partEntry.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				list.Add((partEntry.Name, (long)partEntry.Sector, (long)partEntry.Sectors));
			}
		}
		return list;
	}

	private byte[] ReadPartitionBytes(string partitionName)
	{
		(bool, string, long, long) tuple = DetectPartition(partitionName);
		var (flag, _, _, _) = tuple;
		_ = tuple.Item2;
		long item = tuple.Item3;
		long item2 = tuple.Item4;
		if (!flag)
		{
			return Array.Empty<byte>();
		}
		return DaLoader.ReadFlashByte(item * Dacfg.PageSize, item2 * Dacfg.PageSize, PartType);
	}

	private void ReadPartition(List<string> partitionName, string savePath, bool display = true, bool isBulk = false)
	{
		string text = "preloader";
		string msg = "Reading ";
		string text2 = "... ";
		string partType = "boot1";
		string emiName = "";
		string text3 = "Failed!";
		string text4 = "OK";
		foreach (string item in partitionName)
		{
			if (item.Contains(text))
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item + text2, UI.info, nextline: false, bold: true);
				}
				byte[] dataBytes = DaLoader.ReadFlashByName(0L, Dacfg.Boot1Size, text, partType);
				(string, byte[]) tuple = Dacfg.MExtractLoader(dataBytes, emiName);
				if (tuple.Item2 == null || tuple.Item2.Length == 0)
				{
					UI.RichLogs(text3, UI.error, nextline: true, bold: true);
					break;
				}
				File.WriteAllBytes(savePath, tuple.Item2);
				UI.RichLogs(text4, UI.ok, nextline: true, bold: true);
				break;
			}
			var (flag, fileName, num, num2) = DetectPartition(item);
			if (flag)
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item + text2, UI.info, nextline: false, bold: true);
				}
				bool flag2 = DaLoader.ReadFlash(num * Dacfg.PageSize, num2 * Dacfg.PageSize, fileName, savePath, PartType);
				if (display)
				{
					UI.RichLogs(flag2 ? text4 : text3, flag2 ? UI.ok : UI.error, nextline: true);
				}
			}
		}
	}

	private void ReadPartitionBulk(List<string> partitionName, string savePath, bool display = true, bool isBulk = false)
	{
		string text = "preloader";
		string msg = "Reading ";
		string text2 = "... ";
		string partType = "boot1";
		string emiName = "";
		string text3 = "Failed!";
		string text4 = "OK";
		foreach (string item in partitionName)
		{
			if (!item.Contains(text))
			{
				var (flag, text5, num, num2) = DetectPartition(item);
				if (flag)
				{
					if (display)
					{
						UI.RichLogs(msg, UI.log);
						UI.RichLogs(item + text2, UI.info, nextline: false, bold: true);
					}
					string destination = Path.Combine(savePath, Scatter.Partmtk(text5));
					bool flag2 = DaLoader.ReadFlash(num * Dacfg.PageSize, num2 * Dacfg.PageSize, text5, destination, PartType);
					if (display)
					{
						UI.RichLogs((!flag2) ? text3 : text4, flag2 ? UI.ok : UI.error, nextline: true);
					}
				}
			}
			else
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item + text2, UI.info, nextline: false, bold: true);
				}
				byte[] dataBytes = DaLoader.ReadFlashByName(0L, Dacfg.Boot1Size, text, partType);
				(string, byte[]) tuple2 = Dacfg.MExtractLoader(dataBytes, emiName);
				if (tuple2.Item2 != null && tuple2.Item2.Length != 0)
				{
					File.WriteAllBytes(Path.Combine(savePath, Scatter.Partmtk(item)), tuple2.Item2);
					UI.RichLogs(text4, UI.ok, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(text3, UI.error, nextline: true, bold: true);
				}
			}
		}
	}

	private bool WritePartition(string partitionName, object file, bool display = true)
	{
		string msg = "Writing ";
		string text = "... ";
		string value = "preloader";
		string partType = "boot1";
		string text2 = "Failed!";
		string text3 = "OK";
		string text4 = "_backup... ";
		string partType2 = "boot2";
		string msg2 = "SKIP!";
		if (display)
		{
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(partitionName + text, UI.info, nextline: false, bold: true);
		}
		if (!partitionName.Contains(value))
		{
			var (flag, text5, num, num2) = DetectPartition(partitionName);
			if (!flag)
			{
				if (display)
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				}
				return false;
			}
			bool flag2 = DaLoader.WriteFlash(num * Dacfg.PageSize, num2 * Dacfg.PageSize, text5, file, PartType);
			if (display)
			{
				UI.RichLogs((!flag2) ? text2 : text3, flag2 ? UI.ok : UI.error, nextline: true);
			}
			return flag2;
		}
		if (!DaLoader.WriteFlash(0L, Dacfg.Boot1Size, partitionName, file, partType))
		{
			UI.RichLogs(text2, UI.error, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(text3, UI.ok, nextline: true, bold: true);
		}
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(partitionName + text4, UI.info, nextline: false, bold: true);
		bool result;
		if (!(result = DaLoader.WriteFlash(0L, Dacfg.Boot2Size, partitionName, file, partType2)))
		{
			UI.RichLogs(text2, UI.error, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(text3, UI.ok, nextline: true, bold: true);
		}
		return result;
	}

	private bool WritePartitionDump(string partition, object file, bool display = true)
	{
		string msg = "Writing ";
		string text = "... ";
		string value = "boot";
		string partType = "boot1";
		string text2 = "Failed!";
		string text3 = "OK";
		string text4 = "_backup... ";
		string partType2 = "boot2";
		string msg2 = "SKIP!";
		string partType3 = "user";
		if (display)
		{
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(partition + text, UI.info, nextline: false, bold: true);
		}
		if (partition.Contains(value))
		{
			if (DaLoader.WriteFlash(0L, Dacfg.Boot1Size, partition, file, partType))
			{
				UI.RichLogs(text3, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(text2, UI.error, nextline: true, bold: true);
			}
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(partition + text4, UI.info, nextline: false, bold: true);
			bool result;
			if (!(result = DaLoader.WriteFlash(0L, Dacfg.Boot2Size, partition, file, partType2)))
			{
				UI.RichLogs(text2, UI.error, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(text3, UI.ok, nextline: true, bold: true);
			}
			return result;
		}
		var (flag, text5, num, num2) = DetectPartition(partition);
		if (flag)
		{
			bool flag2 = DaLoader.WriteFlash(num * Dacfg.PageSize, num2 * Dacfg.PageSize, text5, file, partType3);
			if (display)
			{
				UI.RichLogs((!flag2) ? text2 : text3, flag2 ? UI.ok : UI.error, nextline: true);
			}
			return flag2;
		}
		if (display)
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		}
		return false;
	}

	private void ErasePartition(List<string> partitionName, bool display = true)
	{
		string value = "preloader";
		string msg = "Erasing ";
		string text = "... ";
		string partType = "boot1";
		string text2 = "Failed!";
		string text3 = "OK";
		foreach (string item3 in partitionName)
		{
			if (!item3.Contains(value))
			{
				(bool, string, long, long) tuple = DetectPartition(item3);
				var (flag, _, _, _) = tuple;
				_ = tuple.Item2;
				long item = tuple.Item3;
				long item2 = tuple.Item4;
				if (flag)
				{
					if (display)
					{
						UI.RichLogs(msg, UI.log);
						UI.RichLogs(item3 + text, UI.info, nextline: false, bold: true);
					}
					bool flag2 = DaLoader.FormatFlash(item * Dacfg.PageSize, item2 * Dacfg.PageSize, PartType);
					if (display)
					{
						UI.RichLogs((!flag2) ? text2 : text3, flag2 ? UI.ok : UI.error, nextline: true);
					}
				}
				continue;
			}
			if (display)
			{
				UI.RichLogs(msg, UI.log);
				UI.RichLogs(item3 + text, UI.info, nextline: false, bold: true);
			}
			if (!DaLoader.FormatFlash(0L, Dacfg.Boot1Size, partType))
			{
				UI.RichLogs(text2, UI.error, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(text3, UI.ok, nextline: true, bold: true);
			}
			break;
		}
	}

	private bool ReadFirmware(List<ListPartition> addr)
	{
		string text = "_Android_scatter";
		string text2 = "";
		string text3 = ".txt";
		string text4 = "ufs";
		string text5 = ".xml";
		string text6 = "preloader";
		string partType = "boot1";
		string text7 = "Reading ";
		string text8 = "... ";
		string text9 = "/";
		string format = "{0}";
		string msg = "OK";
		string text10 = " [";
		string text11 = ".bin]: ";
		string msg2 = "Failed!";
		string text12 = "pgpt";
		string text13 = "0";
		object obj = null;
		string format2 = "0x{0:X}";
		string contents = "</root>";
		string text14 = Path.Combine(State.SavePath, Config.Chipcfg.Name + text);
		string directoryName = Path.GetDirectoryName(text14);
		for (int i = 0; i < addr.Count; i++)
		{
			if (i == 0)
			{
				Scatter.BuildScatterTxt(Config.Chipcfg.Name ?? text2, Dacfg.FlashType.ToUpper(), text14 + text3);
				if (Dacfg.FlashType == text4)
				{
					Scatter.BuildScatterXml(Config.Chipcfg.Name ?? text2, Dacfg.FlashType.ToUpper(), text14 + text5);
				}
			}
			if (addr[i].PartName == text6)
			{
				byte[] dataBytes = DaLoader.ReadFlashByName(addr[i].Start, addr[i].Size, addr[i].PartName, partType);
				(string, byte[]) tuple = Dacfg.MExtractLoader(dataBytes, text2);
				if (tuple.Item2.Length != 0)
				{
					var (fileName, _) = tuple;
					UI.RichLogs(text7, UI.log);
					UI.RichLogs(addr[i].PartName + text8, UI.log);
					File.WriteAllBytes(directoryName + text9 + tuple.Item1, tuple.Item2);
					Scatter.BuildPreloaderTxt(Dacfg.FlashType.ToUpper(), string.Format(format, addr[i].Value), addr[i].PartName, fileName, (int)Dacfg.Boot1Size, text14 + text3);
					if (Dacfg.FlashType == text4)
					{
						Scatter.BuildPreloaderXml(Dacfg.FlashType.ToUpper(), string.Format(format, addr[i].Value), addr[i].PartName, fileName, (int)Dacfg.Boot1Size, text14 + text5);
					}
					UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(text7 + addr[i].PartName + text10 + addr[i].PartName + text11, UI.log);
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				}
			}
			else if (!(addr[i].PartName == text12) || Config.Chipcfg.Damode == (DAmodes)6)
			{
				UI.RichLogs(text7, UI.log);
				UI.RichLogs(addr[i].PartName + text8, UI.log);
				if (!DaLoader.ReadFlash(addr[i].Start, addr[i].Size, addr[i].PartName, directoryName + text9 + addr[i].FileName, PartType))
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				}
				else
				{
					Scatter.GetPartitionTxt(Dacfg.FlashType.ToUpper(), string.Format(format, addr[i].Value), addr[i].PartName, string.Format(format2, addr[i].Start), string.Format(format2, addr[i].Size), text14 + text3);
					if (Dacfg.FlashType == text4)
					{
						Scatter.GetPartitionXml(Dacfg.FlashType.ToUpper(), string.Format(format, addr[i].Value), addr[i].PartName, string.Format(format2, addr[i].Start), string.Format(format2, addr[i].Size), text14 + text5);
					}
					UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
				}
			}
			else
			{
				UI.RichLogs(text7, UI.log);
				UI.RichLogs(addr[i].PartName + text8, UI.log);
				partition = DaLoader.GetGPT(new GptSettings(text13, text13, text13), PartType);
				var (array, gpt) = partition;
				if (array == null && gpt == obj)
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				}
				else
				{
					File.WriteAllBytes(directoryName + text9 + addr[i].FileName, partition.Item1);
					Scatter.BuildGptTxt(Dacfg.FlashType.ToUpper(), string.Format(format, addr[i].Value), addr[i].PartName, addr[i].FileName, (int)Dacfg.Boot1Size, text14 + text3);
					if (Dacfg.FlashType == text4)
					{
						Scatter.BuildGptXml(Dacfg.FlashType.ToUpper(), string.Format(format, addr[i].Value), addr[i].PartName, addr[i].FileName, (int)Dacfg.Boot1Size, text14 + text5);
					}
					UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
				}
			}
			if (JobStatus.Token.IsCancellationRequested)
			{
				break;
			}
		}
		if (Dacfg.FlashType == text4 && File.Exists(text14 + text5))
		{
			File.AppendAllText(text14 + text5, contents);
		}
		return true;
	}

	private bool RepairGPT(bool repairGpt)
	{
		string text = "0";
		string partType = "user";
		object obj = null;
		string fileName = "pgpt";
		string msg = "Rebuild Partition table... ";
		string msg2 = "OK";
		string msg3 = "Failed!";
		string fileName2 = "sgpt";
		string msg4 = "Formatting partition... ";
		string text2 = "emmc";
		(byte[], Gpt) gPT = DaLoader.GetGPT(new GptSettings(text, text, text), partType);
		if (repairGpt)
		{
			var (array, gpt) = gPT;
			if (array != null || gpt !=null)
			{
				byte[] array2 = Array.Empty<byte>();
				if (Config.Chipcfg.Damode == (DAmodes)6)
				{
					array2 = DaLoader.ReadFlashByName(0L, Dacfg.Boot1Size / 128L, fileName, PartType);
				}
				else if (Config.Chipcfg.Damode == DAmodes.XFLASH)
				{
					(array2, _) = gPT;
				}
				if (array2.Length != 0)
				{
					UI.RichLogs(msg, UI.log);
					if (DaLoader.WriteFlash(0L, array2.Length, fileName, array2, PartType))
					{
						UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
					}
					else
					{
						UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
					}
					byte[] array3 = Array.Empty<byte>();
					if (Config.Chipcfg.Damode == (DAmodes)6)
					{
						array3 = DaLoader.ReadFlashByName(0L, Dacfg.Boot1Size / 128L, fileName2, PartType);
					}
					else if (Config.Chipcfg.Damode == DAmodes.XFLASH)
					{
						array3 = array2.Skip(Dacfg.PageSize).ToArray();
					}
					if (array3.Length != 0)
					{
						UI.RichLogs(msg4, UI.log);
						if (!DaLoader.WriteFlash(0L, array3.Length, fileName2, array3, PartType))
						{
							UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
						}
						else
						{
							UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
						}
						goto IL_0159;
					}
					return false;
				}
				return false;
			}
		}
		if (repairGpt)
		{
			var (array, gpt) = gPT;
			if (array == null && gpt == null && Dacfg.FlashType == text2 && Config.Chipcfg.Damode != (DAmodes)6)
			{
				UI.RichLogs(msg, UI.log);
				byte[] array4 = Pgpt.PgptManual(Dacfg.FlashSize, repairGpt);
				if (array4.Length == 0)
				{
					return false;
				}
				if (DaLoader.WriteFlash(0L, array4.Length, fileName, array4, PartType))
				{
					UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
				}
				byte[] array5 = array4.Skip(Dacfg.PageSize).ToArray();
				UI.RichLogs(msg4, UI.log);
				if (DaLoader.WriteFlash(0L, array5.Length, fileName2, array5, PartType))
				{
					UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
				}
			}
		}
		goto IL_0159;
		IL_0159:
		return true;
	}

	private bool FlashDownload(List<MTKList> partList, bool repairGpt = false, bool formmat = false)
	{
		string text = "preloader_backup";
		string msg = "Formating All Partition";
		string text2 = " [";
		string text3 = "]";
		string msg2 = "... ";
		string partType = "user";
		string msg3 = "Failed!";
		string msg4 = "OK";
		string text4 = "preloader";
		string msg5 = "Writing ";
		string text5 = "[";
		string text6 = "->[";
		string partType2 = "boot1";
		string msg6 = "Failed";
		string text7 = "_backup]";
		string text8 = "_backup";
		string partType3 = "boot2";
		bool flag = false;
		for (int i = 0; i < partList.Count; i++)
		{
			if (partList[i].PartName == text)
			{
				flag = true;
				break;
			}
		}
		for (int j = 0; j < partList.Count; j++)
		{
			if (j == 0 && formmat)
			{
				UI.RichLogs(msg, UI.log);
				UI.RichLogs(text2 + utils.FileSize(Dacfg.FlashSize) + text3, UI.info, nextline: false, bold: true);
				UI.RichLogs(msg2, UI.log);
				if (!DaLoader.FormatFlash(0L, Dacfg.FlashSize, partType))
				{
					UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				}
				repairGpt = true;
			}
			if (j == 0 && repairGpt)
			{
				RepairGPT(repairGpt);
			}
			if (partList[j].PartName == text4 && File.Exists(partList[j].FilePath))
			{
				byte[] array = File.ReadAllBytes(partList[j].FilePath);
				UI.RichLogs(msg5, UI.log);
				UI.RichLogs(text5 + partList[j].FileName + text3, UI.info, nextline: false, bold: true);
				UI.RichLogs(text6 + partList[j].PartName + text3, UI.log);
				UI.RichLogs(msg2, UI.log);
				Console.WriteLine(partList[j].StartLba);
				if (!DaLoader.WriteFlash(partList[j].StartLba.ToHex(), array.Length, partList[j].PartName, array, partType2))
				{
					UI.RichLogs(msg6, UI.error, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				}
				if (!flag)
				{
					UI.RichLogs(msg5, UI.log);
					UI.RichLogs(text5 + partList[j].FileName + text3, UI.info, nextline: false, bold: true);
					UI.RichLogs(text6 + partList[j].PartName + text7, UI.log);
					UI.RichLogs(msg2, UI.log);
					Console.WriteLine(partList[j].StartLba);
					if (DaLoader.WriteFlash(partList[j].StartLba.ToHex(), array.Length, partList[j].PartName + text8, array, partType3))
					{
						UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
					}
					else
					{
						UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
					}
				}
			}
			else if (partList[j].PartName == text && File.Exists(partList[j].FilePath))
			{
				byte[] array2 = File.ReadAllBytes(partList[j].FilePath);
				UI.RichLogs(msg5, UI.log);
				UI.RichLogs(text5 + partList[j].FileName + text3, UI.info, nextline: false, bold: true);
				UI.RichLogs(text6 + partList[j].PartName + text7, UI.log);
				UI.RichLogs(msg2, UI.log);
				if (DaLoader.WriteFlash(partList[j].StartLba.ToHex(), array2.Length, partList[j].PartName, array2, partType3))
				{
					UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
				}
			}
			else
			{
				UI.RichLogs(msg5, UI.log);
				UI.RichLogs(text5 + partList[j].FileName + text3, UI.info, nextline: false, bold: true);
				UI.RichLogs(text6 + partList[j].PartName + text3, UI.log);
				UI.RichLogs(msg2, UI.log);
				if (DaLoader.WriteFlash(partList[j].StartLba.ToHex(), partList[j].LastLba.ToHex(), partList[j].PartName, partList[j].FilePath, PartType))
				{
					UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
				}
			}
		}
		return true;
	}

	private bool WriteFlashMode(List<MTKList> partList)
	{
		bool flag;
		if (Config.Chipcfg.Damode != (DAmodes)6)
		{
			if (Config.Chipcfg.Damode == DAmodes.XFLASH)
			{
				if (FlashDownload(partList, MTKConfig.Repartition, MTKConfig.FormatALL))
				{
					return DaLoader.Shutdown();
				}
				flag = false;
				return false;
			}
			return DaLoader.Shutdown();
		}
		if (State.FlashingMode != 0)
		{
			if (State.FlashingMode == Downloadmode.FormatDownload)
			{
				if (!DaLoader.FormatFlashXml(partList))
				{
					flag = false;
					return false;
				}
				return flag = DaLoader.Shutdown();
			}
			if (State.FlashingMode == (Downloadmode)2)
			{
				if (DaLoader.FlashUpgradeXml(partList))
				{
					return flag = DaLoader.Shutdown();
				}
				flag = false;
				return false;
			}
			return flag = DaLoader.Shutdown();
		}
		if (FlashDownload(partList, MTKConfig.Repartition))
		{
			return flag = DaLoader.Shutdown();
		}
		flag = false;
		return false;
	}

	private bool ReadDumo(string savePath)
	{
		string text = "8MB";
		string text2 = "16MB";
		string text3 = "32MB";
		string text4 = "64MB";
		string text5 = "128MB";
		string text6 = "256MB";
		string text7 = "512MB";
		string text8 = "1GB";
		string text9 = "2GB";
		string text10 = "4GB";
		long field_ = Class38.field_3037;
		string text11 = "Auto";
		string text12 = "Reading ";
		string msg = " boot1";
		string msg2 = "... ";
		string text13 = "boot1";
		string text14 = "/";
		string msg3 = "OK";
		string msg4 = "Failed!";
		string msg5 = " boot2 ";
		string text15 = "boot2";
		string msg6 = " userarea ";
		string text16 = "userarea";
		string partType = "user";
		if (!(UI.ReaddumpSize == text))
		{
			if (UI.ReaddumpSize == text2)
			{
				UI.ReadUserareaSize = 16777216L;
			}
			else if (UI.ReaddumpSize == text3)
			{
				UI.ReadUserareaSize = 33554432L;
			}
			else if (!(UI.ReaddumpSize == text4))
			{
				if (UI.ReaddumpSize == text5)
				{
					UI.ReadUserareaSize = 134217728L;
				}
				else if (UI.ReaddumpSize == text6)
				{
					UI.ReadUserareaSize = 268435456L;
				}
				else if (UI.ReaddumpSize == text7)
				{
					UI.ReadUserareaSize = 536870912L;
				}
				else if (UI.ReaddumpSize == text8)
				{
					UI.ReadUserareaSize = 1073741824L;
				}
				else if (UI.ReaddumpSize == text9)
				{
					UI.ReadUserareaSize = 2147483648L;
				}
				else if (UI.ReaddumpSize == text10)
				{
					UI.ReadUserareaSize = field_;
				}
				else if (UI.ReaddumpSize == text11)
				{
					UI.ReadUserareaSize = UI.ReadUserareaSize;
				}
			}
			else
			{
				UI.ReadUserareaSize = 67108864L;
			}
		}
		else
		{
			UI.ReadUserareaSize = 8388608L;
		}
		UI.RichLogs(text12 + UI.ReaddumpSize, UI.log);
		UI.RichLogs(msg, UI.info, nextline: false, bold: true);
		UI.RichLogs(msg2, UI.log);
		if (!DaLoader.ReadFlash(0L, Dacfg.Boot2Size, text13, savePath + text14 + Scatter.Partmtk(text13), text13))
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
		}
		UI.RichLogs(text12 + UI.ReaddumpSize, UI.log);
		UI.RichLogs(msg5, UI.info, nextline: false, bold: true);
		UI.RichLogs(msg2, UI.log);
		if (DaLoader.ReadFlash(0L, Dacfg.Boot2Size, text15, savePath + text14 + Scatter.Partmtk(text15), text15))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
		UI.RichLogs(text12 + UI.ReaddumpSize, UI.log);
		UI.RichLogs(msg6, UI.info, nextline: false, bold: true);
		UI.RichLogs(msg2, UI.log);
		if (DaLoader.ReadFlash(0L, UI.ReadUserareaSize, text16, savePath + text14 + Scatter.Partmtk(text16), partType))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
		return DaLoader.Shutdown();
	}

	private string GetCurrentSlot()
	{
		string partitionName = "misc";
		string partitionName2 = "para";
		string text = "\0\0";
		string oldValue = "_";
		string newValue = "";
		byte[] array = ReadPartitionBytes(partitionName);
		string text2;
		if (array == null)
		{
			array = ReadPartitionBytes(partitionName2);
			if (array == null || array.Length < 2050)
			{
				return string.Empty;
			}
			text2 = Encoding.UTF8.GetString(array, 2048, 2);
			if (text2 == text)
			{
				return string.Empty;
			}
			return text2.Replace(oldValue, newValue);
		}
		bool flag;
		if (array.Length == 0)
		{
			array = ReadPartitionBytes(partitionName2);
			if (array != null)
			{
				goto IL_0075;
			}
			flag = false;
		}
		else
		{
			if (array != null)
			{
				goto IL_0075;
			}
			flag = false;
		}
		goto IL_00a9;
		IL_0075:
		flag = array.Length >= 2050;
		goto IL_00a9;
		IL_00a9:
		if (!flag)
		{
			return string.Empty;
		}
		text2 = Encoding.UTF8.GetString(array, 2048, 2);
		if (text2 == text)
		{
			return string.Empty;
		}
		return text2.Replace(oldValue, newValue);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
