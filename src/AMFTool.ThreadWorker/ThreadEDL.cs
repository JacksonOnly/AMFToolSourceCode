using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using AMFTool.Library;
using AMFTool.Library.Qualcomm;
using AMFTool.Library.Usb;
using AMFTool.PartitionManager;

namespace AMFTool.ThreadWorker;

public class ThreadEDL
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_0
	{
		public string savepath;

		internal bool _003CReadWriteEFS_003Eb__1(string name)
		{
			return File.Exists(Path.Combine(savepath, name));
		}
	}

	private Sahara Sahara;

	private Firehose Firehose;

	private (byte[], Gpt) Partition;

	public ThreadEDL()
	{
		Sahara = new Sahara();
		Firehose = new Firehose();
	}

	private void ResetVariable()
	{
		Sahara = new Sahara();
		Firehose = new Firehose();
	}

	public bool Init()
	{
		int num = 0;
		int nextline = 1;
		string value = "<?xml version=\"1.0\" ?>\r\n";
		string value2 = "<data>\r\n";
		string value3 = "<!--NOTE: Genererate BY AMFTool **-->";
		string text = "<program SECTOR_SIZE_IN_BYTES=\"";
		string text2 = "\" file_sector_offset=\"0\" filename=\"";
		string text3 = "\" label=\"";
		string text4 = "\" num_partition_sectors=\"";
		string text5 = "\" physical_partition_number=\"";
		string text6 = "\" start_sector=\"";
		string text7 = "\"/>\r\n";
		string value4 = "</data>";
		string value5 = "userdata";
		string format = "{0}";
		string msg = "Failed!";
		try
		{
			if (!DeviceState.DeviceInit)
			{
				ResetVariable();
				if (!Initializing())
				{
					SerialEDL.Close();
					return false;
				}
			}
			switch (State.Job)
			{
			case (Job)2:
				ShowPartition();
				return true;
			case Job.Flash:
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(value);
				stringBuilder.Append(value2);
				stringBuilder.Append(value3);
				foreach (QCList item in QCList.Partition)
				{
					stringBuilder.Append(text + item.SectorSize + text2 + item.FileName + text3 + item.Label + text4 + item.NumPartitionSectors + text5 + item.PhysicalPartitionNumber + text6 + item.StartSector + text7);
				}
				stringBuilder.Append(value4);
				string xml = stringBuilder.ToString();
				WritePartitionXML(xml);
				break;
			}
			case Job.Readback:
			{
				using (List<GptList>.Enumerator enumerator = GptList.GptPartList.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						GptList current = enumerator.Current;
						foreach (GptEfi item2 in current.Partition)
						{
							if (!item2.Label.Equals(value5, StringComparison.OrdinalIgnoreCase))
							{
								QCList.Partition.Add(new QCList(string.Format(format, Firehose.cfg.SectorSize), item2.FileName, item2.Label, item2.NumPartitionSectors, item2.PhysicalPartitionNumber.ToString(), item2.StartSector, item2.SizeInKB));
							}
						}
					}
				}
				ReadPartitionXML(QCList.Partition);
				break;
			}
			case (Job)60:
				SetSlot(setA: false);
				break;
			case (Job)80:
				return ShutDown();
			case Job.SetSlotA:
				SetSlot(setA: true, setB: false);
				break;
			case Job.Xml:
				XMLOperation();
				break;
			case (Job)58:
				ResetFRP(PartitionSet.EDLFrp);
				MiCloudOperation(PartitionSet.EDLMicloud2);
				break;
			case (Job)56:
				ResetFRP(PartitionSet.EDLFrp);
				MiCloudOperation(PartitionSet.EDLMicloud1);
				break;
			case (Job)40:
				ReadWriteEFS(read: false);
				break;
			case Job.NVBackup:
				ReadWriteEFS(read: true);
				break;
			case (Job)42:
				ErasePartition(PartitionSet.EDLEfs);
				break;
			case Job.QCOppoID:
				ResetFRP(PartitionSet.EDLMicloud2);
				break;
			case Job.Frp:
				ResetFRP(PartitionSet.EDLFrp);
				break;
			case Job.Reset:
				ResetFRP(PartitionSet.EDLReset);
				break;
			case (Job)8:
				WritePartition(DeviceState.Partitiontarget[0], State.FilePath);
				break;
			case Job.Erase:
				ErasePartition(DeviceState.Partitiontarget);
				break;
			case Job.Read:
				ReadPartition(DeviceState.Partitiontarget, State.SavePath);
				break;
			}
			return ShutDown();
		}
		catch (Exception ex)
		{
			UI.RichLogs(msg, UI.error, (byte)nextline != 0, (byte)num != 0);
			Console.WriteLine(ex.Message);
			SerialEDL.Close();
			return (byte)num != 0;
		}
	}

	private bool Initializing()
	{
		string msg = "Waiting EDL 9008 USB Connection... ";
		string category = "QCOM";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "Name : ";
		string text = "Connecting to device ";
		string text2 = "... ";
		string msg5 = "Connecting to Firehose... ";
		string text3 = "OPLUS VIP";
		string text4 = "Snapdragon 680";
		string text5 = ":";
		string text6 = "Oppo A71 [CPH1801]";
		UI.RichLogs(msg, UI.log);
		if (UsbScan.FindUSB(category))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg4, UI.log);
			UI.RichLogs(Portinfo.Name, UI.info, nextline: true, bold: true);
			UI.RichLogs(text + Portinfo.COM + text2, UI.log);
			SerialEDL.Close();
			if (!SerialEDL.Open(Portinfo.COM, 1))
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			if (!Sahara.Init())
			{
				return false;
			}
			UI.RichLogs(msg5, UI.log);
			if (!Reconnect(10))
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			if (!Firehose.Init(EDLConfig.Storage, DeviceState.Brand == text3, DeviceState.Model == text4))
			{
				return false;
			}
			DeviceState.SetInit(Portinfo.VID + text5 + Portinfo.PID + text5 + Portinfo.COM + text5 + Portinfo.Name);
			if (!(DeviceState.Model == text6))
			{
				if (!ReadGPT())
				{
					return false;
				}
				bool flag = State.Job != Job.Flash && State.Job != Job.Xml && DeviceState.Brand != text3;
				if (flag && MTKConfig.Infirmware)
				{
					ReadDeviceInfo();
					return true;
				}
				return true;
			}
			return true;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		return false;
	}

	private bool Reconnect(int time)
	{
		SerialEDL.Close();
		if (!SerialEDL.Open(Portinfo.COM, time))
		{
			return false;
		}
		return true;
	}

	private bool ReadDeviceInfo()
	{
		string msg = "Reading Device Info : ";
		string msg2 = "[SUPER]";
		string msg3 = "[SYSTEM]";
		UI.RichLogs(msg, UI.log);
		long systemSector;
		long systemSectors;
		int systemLun;
		string systemName;
		string sizeinkb2;
		if (!DetectSuper(out var superSector, out var superSectors, out var superLun, out var superName, out var sizeinkb))
		{
			if (DetectSystem(out systemSector, out systemSectors, out systemLun, out systemName, out sizeinkb2))
			{
				UI.RichLogs(msg3, UI.info, nextline: true, bold: true);
				string data = DeviceInfoReader.ReadFromNormalPartition(systemSector, systemSectors, Firehose.cfg.SectorSize, systemLun, systemName, sizeinkb2, Firehose);
				Dictionary<string, string> dictionary = ParseBuildProp(data);
				if (dictionary.Count == 0)
				{
					return false;
				}
				ShowDeviceInfo(dictionary);
				return true;
			}
			return false;
		}
		UI.RichLogs(msg2, UI.info, nextline: true, bold: true);
		string data2 = DeviceInfoReader.ReadFromSuper(superSector, superSectors, Firehose.cfg.SectorSize, superLun, superName, sizeinkb, Firehose);
		Dictionary<string, string> dictionary2 = ParseBuildProp(data2);
		if (dictionary2.Count == 0)
		{
			if (DetectSystem(out systemSector, out systemSectors, out systemLun, out systemName, out sizeinkb2))
			{
				UI.RichLogs(msg3, UI.info, nextline: true, bold: true);
				string data = DeviceInfoReader.ReadFromNormalPartition(systemSector, systemSectors, Firehose.cfg.SectorSize, systemLun, systemName, sizeinkb2, Firehose);
				Dictionary<string, string> dictionary = ParseBuildProp(data);
				if (dictionary.Count == 0)
				{
					return false;
				}
				ShowDeviceInfo(dictionary);
				return true;
			}
			return false;
		}
		ShowDeviceInfo(dictionary2);
		return true;
	}

	private bool DetectSuper(out long superSector, out long superSectors, out int superLun, out string superName, out string sizeinkb)
	{
		string text = "";
		string text2 = "super";
		superSector = 0L;
		superSectors = 0L;
		superLun = 0;
		superName = text;
		sizeinkb = text;
		string[] array = new string[1] { text2 };
		foreach (string text3 in array)
		{
			var (flag, text4, num, num2, num3, text5) = DetectPartition(text3);
			if (flag && text3.IndexOf(text2, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				superSector = num;
				superSectors = num3;
				superLun = num2;
				superName = text4;
				sizeinkb = text5;
				return true;
			}
		}
		return false;
	}

	private bool DetectSystem(out long systemSector, out long systemSectors, out int systemLun, out string systemName, out string sizeinkb)
	{
		string text = "";
		string text2 = "system";
		string text3 = "system_a";
		string text4 = "system_b";
		string text5 = "android";
		systemSector = 0L;
		systemSectors = 0L;
		systemLun = 0;
		systemName = text;
		sizeinkb = text;
		string[] array = new string[4] { text2, text3, text4, text5 };
		int num = 0;
		string text6;
		long num2;
		int num3;
		long num4;
		string text7;
		while (true)
		{
			if (num >= array.Length)
			{
				return false;
			}
			string partitionName = array[num];
			bool flag;
			(flag, text6, num2, num3, num4, text7) = DetectPartition(partitionName);
			if (flag)
			{
				break;
			}
			num++;
		}
		systemSector = num2;
		systemSectors = num4;
		systemLun = num3;
		systemName = text6;
		sizeinkb = text7;
		return true;
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
				UI.RichLogs(text97.ToUpperInvariant(), UI.info, nextline: true, bold: true);
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

	private bool ResetFRP(List<string> partition, bool display = true)
	{
		string msg = "Erasing ";
		string text = "... ";
		int result = 1;
		string format = "{0}";
		string text2 = "Failed!";
		string text3 = "OK";
		foreach (string item in partition)
		{
			(bool, string, long, int, long, string) tuple = DetectPartition(item);
			var (flag, label, num, num2, num3, _) = tuple;
			_ = tuple.Item6;
			if (flag)
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item + text, UI.info, nextline: false, bold: true);
				}
				bool flag2 = Firehose.WriteFlash(Tools.format(item), string.Format(format, Firehose.cfg.SectorSize), string.Format(format, num2), string.Format(format, num), (int)num3, label);
				if (display)
				{
					UI.RichLogs((!flag2) ? text2 : text3, flag2 ? UI.ok : UI.error, nextline: true);
				}
			}
		}
		return (byte)result != 0;
	}

	private void ReadWriteEFS(bool read, bool openPath = true)
	{
		string item = default(string);
		List<string> eDLEfs = default(List<string>);
		int num6 = default(int);
		string path = default(string);
		string[] array = default(string[]);
		_003C_003Ec__DisplayClass14_0 _003C_003Ec__DisplayClass14_ = default(_003C_003Ec__DisplayClass14_0);
		int num7 = default(int);
		string msg = default(string);
		string path2 = default(string);
		string text = default(string);
		int num15 = default(int);
		int num17 = default(int);
		string[] array2 = default(string[]);
		string msg2 = default(string);
		bool flag = default(bool);
		string format = default(string);
		string msg3 = default(string);
		bool flag2 = default(bool);
		string name = default(string);
		bool flag3 = default(bool);
		int comparisonType = default(int);
		string value = default(string);
		string msg4 = default(string);
		string msg5 = default(string);
		string msg6 = default(string);
		bool flag4 = default(bool);
		bool flag5 = default(bool);
		string partitionName = default(string);
		bool flag6 = default(bool);
		bool flag7 = default(bool);
		while (true)
		{
			int num = 1392908977;
			while (true)
			{
				nint num2 = num ^ -930027891;
				nuint num3 = (nuint)num2;
				uint num5;
				int num4 = (int)((num5 = (uint)((int)num3 ^ -930027891 ^ -569738064 ^ -569738064)) % 43);
				if (num4 == 0)
				{
					break;
				}
				if (num4 == 1 || num4 == 1)
				{
					goto IL_0979;
				}
				if (num4 != 2 && num4 != 2)
				{
					if (num4 != 3 && num4 != 3)
					{
						if (num4 != 4 && num4 != 4)
						{
							if (num4 != 5 && num4 != 5)
							{
								if (num4 != 6 && num4 != 6)
								{
									if (num4 != 7 && num4 != 7)
									{
										if (num4 != 8 && num4 != 8)
										{
											if (num4 != 9 && num4 != 9)
											{
												if (num4 != 10 && num4 != 10)
												{
													if (num4 != 11 && num4 != 11)
													{
														if (num4 != 12 && num4 != 12)
														{
															if (num4 != 13 && num4 != 13)
															{
																if (num4 != 14 && num4 != 14)
																{
																	if (num4 != 15 && num4 != 15)
																	{
																		if (num4 != 16)
																		{
																			if (num4 != 17 && num4 != 17)
																			{
																				if (num4 != 18 && num4 != 18)
																				{
																					if (num4 != 19 && num4 != 19)
																					{
																						if (num4 != 20 && num4 != 20)
																						{
																							if (num4 != 21 && num4 != 21)
																							{
																								if (num4 != 22 && num4 != 22)
																								{
																									if (num4 != 23 && num4 != 23)
																									{
																										if (num4 != 24)
																										{
																											if (num4 == 25 || num4 == 25)
																											{
																												if ((object)"" == "" && (object)"" == "" && (object)"" == "")
																												{
																													item = eDLEfs[num6];
																												}
																												num = (int)((num5 << 4) | (num5 >> 28)) ^ -1491777429;
																												continue;
																											}
																											if (num4 == 26 || num4 == 26)
																											{
																												if ((object)"" == "")
																												{
																													path = array[num6];
																												}
																												num = (int)(0x2DC95AE2 ^ num5);
																												continue;
																											}
																											if (num4 == 27 || num4 == 27)
																											{
																												ReadPartition(new List<string> { item }, Path.Combine(_003C_003Ec__DisplayClass14_.savepath, path), (byte)num7 != 0);
																												int num8 = (int)(num5 ^ 0x406E5E5E);
																												num = (num8 + -1302393501 - -1302393501) ^ 0x406E5E5E ^ 0x51C677D2;
																												continue;
																											}
																											if (num4 == 28 || num4 == 28)
																											{
																												if ("".Length != 0)
																												{
																												}
																												int num9 = (int)num5 ^ -602933335;
																												num = (num9 + -1685578756 - -1685578756) ^ -602933335 ^ 0x76EA15B5;
																												continue;
																											}
																											if (num4 == 29 || num4 == 29)
																											{
																												if ((object)"" != "")
																												{
																												}
																												num = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x380CB7A6);
																												continue;
																											}
																											if (num4 == 30 || num4 == 30)
																											{
																												num6 += num7;
																												num = (int)(0x34D86251 ^ num5);
																												continue;
																											}
																											if (num4 == 31)
																											{
																												if ("".Length == 0 && (object)"" == "" && "".Length == 0 && "".Length == 0)
																												{
																													goto IL_0605;
																												}
																												goto IL_0611;
																											}
																											if (num4 != 32 && num4 != 32)
																											{
																												if (num4 != 33 && num4 != 33)
																												{
																													if (num4 != 34 && num4 != 34)
																													{
																														if (num4 != 35 && num4 != 35)
																														{
																															if (num4 != 36 && num4 != 36)
																															{
																																if (num4 == 37 || num4 == 37)
																																{
																																	if ((object)"" != "")
																																	{
																																	}
																																	num = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x2E459936);
																																	continue;
																																}
																																if (num4 == 38 || num4 == 38)
																																{
																																	if ("".Length == 0)
																																	{
																																		UI.RichLogs(msg, UI.error, (byte)num7 != 0, (byte)num7 != 0);
																																	}
																																	num = (int)(0x3230D79F ^ num5);
																																	continue;
																																}
																																if (num4 == 39 || num4 == 39)
																																{
																																	if ("".Length != 0 || (object)"" != "")
																																	{
																																	}
																																	num = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x6A42425D);
																																	continue;
																																}
																																if (num4 == 40 || num4 == 40)
																																{
																																	if ((object)"" != "")
																																	{
																																		int num10 = (int)(num5 ^ 0x425FBA);
																																		num = (num10 + -1241900155 - -1241900155) ^ 0x425FBA ^ 0x3C1780C7;
																																		continue;
																																	}
																																	return;
																																}
																																if (num4 != 41)
																																{
																																	if (num4 != 42)
																																	{
																																		goto IL_0979;
																																	}
																																	ZipArchive zipArchive = ZipFile.Open(Path.Combine(_003C_003Ec__DisplayClass14_.savepath, path2), (ZipArchiveMode)num7);
																																	try
																																	{
																																		while (true)
																																		{
																																			int num11 = 1521064051;
																																			while (true)
																																			{
																																				nint num12 = num11 ^ 0x471FBD97;
																																				nuint num13 = (nuint)num12;
																																				int num14 = (int)((num5 = (uint)((int)num13 ^ 0x471FBD97 ^ 0x55A28D04 ^ 0x55A28D04)) % 15);
																																				if (num14 == 0)
																																				{
																																					break;
																																				}
																																				if (num14 != 1 && num14 != 1)
																																				{
																																					if (num14 != 2 && num14 != 2)
																																					{
																																						if (num14 != 3 && num14 != 3)
																																						{
																																							if (num14 != 4 && num14 != 4)
																																							{
																																								if (num14 == 5)
																																								{
																																									goto IL_0be9;
																																								}
																																								if (num14 == 6 || num14 == 6)
																																								{
																																									if ("".Length != 0)
																																									{
																																									}
																																									num11 = (int)(0x3BD94DE2 ^ num5);
																																									continue;
																																								}
																																								if (num14 == 7 || num14 == 7)
																																								{
																																									if ((object)"" == "")
																																									{
																																										zipArchive.CreateEntryFromFile(Path.Combine(_003C_003Ec__DisplayClass14_.savepath, text), text, (CompressionLevel)num15);
																																									}
																																									int num16 = (int)num5 ^ -503988967;
																																									num11 = (num16 + -566864220 - -566864220) ^ -503988967 ^ 0x3E70FDC0;
																																									continue;
																																								}
																																								if (num14 == 8 || num14 == 8)
																																								{
																																									File.Delete(Path.Combine(_003C_003Ec__DisplayClass14_.savepath, text));
																																									num11 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x5420D688);
																																									continue;
																																								}
																																								if (num14 == 9 || num14 == 9)
																																								{
																																									if ("".Length != 0)
																																									{
																																									}
																																									num11 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x5337402C);
																																									continue;
																																								}
																																								if (num14 == 10 || num14 == 10)
																																								{
																																									if ("".Length != 0 || "".Length != 0 || "".Length != 0)
																																									{
																																									}
																																									num11 = (int)(0x15454462 ^ num5);
																																									continue;
																																								}
																																								if (num14 == 11 || num14 == 11)
																																								{
																																									if ((object)"" == "")
																																									{
																																										num17 += num7;
																																									}
																																									num11 = (int)((num5 << 4) | (num5 >> 28)) ^ -197918213;
																																									continue;
																																								}
																																								if (num14 != 12)
																																								{
																																									if (num14 != 13 && num14 != 13)
																																									{
																																										if (num14 != 14)
																																										{
																																											goto IL_0c22;
																																										}
																																										goto end_IL_0c67;
																																									}
																																									if ((object)"" != "")
																																									{
																																									}
																																									num11 = (int)(0x22B53AB1 ^ num5);
																																									continue;
																																								}
																																								if ((object)"" != "")
																																								{
																																									goto IL_0be2;
																																								}
																																							}
																																							if (num17 >= array2.Length)
																																							{
																																								goto IL_0be2;
																																							}
																																							goto IL_0be9;
																																						}
																																						if ("".Length == 0 && "".Length == 0)
																																						{
																																							num17 = num15;
																																						}
																																						num11 = (int)(0x6DE03561 ^ num5);
																																						continue;
																																					}
																																					array2 = array;
																																					num11 = (int)(0xE8A4CC8 ^ num5);
																																					continue;
																																				}
																																				goto IL_0c22;
																																				IL_0be2:
																																				num11 = 2015938078;
																																				continue;
																																				IL_0c22:
																																				num11 = (int)((num5 << 3) | (num5 >> 29)) ^ -210635081;
																																				continue;
																																				IL_0be9:
																																				text = array2[num17];
																																				num11 = 1431385926;
																																			}
																																			continue;
																																			end_IL_0c67:
																																			break;
																																		}
																																	}
																																	finally
																																	{
																																		if (zipArchive != null)
																																		{
																																			while (true)
																																			{
																																				int num18 = 411644261;
																																				while (true)
																																				{
																																					nint num19 = num18 ^ 0x4C36E1FD;
																																					nuint num20 = (nuint)num19;
																																					int num21 = (int)((num5 = (uint)((int)num20 ^ 0x4C36E1FD ^ -1601808690 ^ -1601808690)) % 4);
																																					if (num21 == 0)
																																					{
																																						break;
																																					}
																																					if (num21 != 1 && num21 != 1)
																																					{
																																						if (num21 == 2 || num21 == 2)
																																						{
																																							if ("".Length != 0)
																																							{
																																							}
																																							num18 = (int)(0x6E17A001 ^ num5);
																																							continue;
																																						}
																																						if (num21 == 3)
																																						{
																																							goto end_IL_0cee;
																																						}
																																					}
																																					((IDisposable)zipArchive).Dispose();
																																					num18 = (int)((num5 << 3) | (num5 >> 29)) ^ -537988774;
																																				}
																																				continue;
																																				end_IL_0cee:
																																				break;
																																			}
																																		}
																																	}
																																	UI.RichLogs(msg2, UI.ok, (byte)num7 != 0, (byte)num7 != 0);
																																	goto IL_0d0f;
																																}
																																if ("".Length != 0 || (object)"" != "")
																																{
																																	goto IL_03b1;
																																}
																															}
																															else if ("".Length != 0 || (object)"" != "" || flag)
																															{
																																num = (int)(0x23A591A9 ^ num5);
																																continue;
																															}
																															path2 = string.Format(format, DateTime.Now);
																															goto IL_03b1;
																														}
																														flag = (array.All(_003C_003Ec__DisplayClass14_._003CReadWriteEFS_003Eb__1) ? 1 : 0) == num15;
																														int num22 = (int)num5 ^ -309684553;
																														num = (num22 + 1456330877 - 1456330877) ^ -309684553 ^ 0x2F786C9D;
																														continue;
																													}
																													num = (int)((num5 << 3) | (num5 >> 29)) ^ -1189486456;
																													continue;
																												}
																												if ((object)"" == "")
																												{
																													UI.RichLogs(msg3, UI.log, (byte)num15 != 0, (byte)num15 != 0);
																												}
																												int num23 = (int)(num5 ^ 0x51E53FC4);
																												num = (num23 + 494454803 - 494454803) ^ 0x51E53FC4 ^ 0xDEA0CFF;
																												continue;
																											}
																											if ("".Length != 0 || !flag2)
																											{
																												num = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x162F59DA);
																												continue;
																											}
																										}
																										else if ("".Length == 0 && "".Length == 0)
																										{
																										}
																										num = 2025948637;
																										continue;
																									}
																									if ((object)"" != "")
																									{
																										num = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x7CF5596A);
																										continue;
																									}
																									goto IL_0605;
																								}
																								if ((object)"" == "")
																								{
																									num6 = num15;
																								}
																								int num24 = (int)num5 ^ -1046831229;
																								num = (num24 + -757041154 - -757041154) ^ -1046831229 ^ 0x5B2E2AA2;
																								continue;
																							}
																							if ((object)"" == "" && (object)"" == "")
																							{
																								_003C_003Ec__DisplayClass14_.savepath = utils.CheckBackupFolder(name);
																							}
																							num = (int)(0x3426022B ^ num5);
																							continue;
																						}
																						num = (int)(0x27737377 ^ num5);
																						continue;
																					}
																					if ((object)"" == "")
																					{
																						_003C_003Ec__DisplayClass14_ = new _003C_003Ec__DisplayClass14_0();
																					}
																					int num25 = (int)(num5 ^ 0x9CFCA75);
																					num = (num25 + 2099054352 - 2099054352) ^ 0x9CFCA75 ^ 0x23140EBD;
																					continue;
																				}
																				if ((object)"" != "" || flag3)
																				{
																					num = (int)(0x647077C0 ^ num5);
																					continue;
																				}
																				goto IL_10ab;
																			}
																			if ("".Length == 0)
																			{
																				flag3 = read;
																			}
																			num = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1B47D02D);
																			continue;
																		}
																		array = eDLEfs.Select(delegate(string p)
																		{
																			string text2 = ".img";
																			return p + text2;
																		}).ToArray();
																		num = 1919111861;
																		continue;
																	}
																	if ("".Length == 0 && (object)"" == "")
																	{
																		eDLEfs = PartitionSet.EDLEfs;
																	}
																	num = (int)((num5 << 4) | (num5 >> 28)) ^ -93323001;
																	continue;
																}
																if ((object)"" != "")
																{
																}
																num = (int)((num5 << 4) | (num5 >> 28)) ^ -1019890001;
																continue;
															}
															comparisonType = 5;
															int num26 = (int)(num5 ^ 0x56B3EBFF);
															num = (num26 + 1911499383 - 1911499383) ^ 0x56B3EBFF ^ 0x3A0D0DC3;
															continue;
														}
														value = Class38.field_691;
														num = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x68A0AB63);
														continue;
													}
													msg4 = Class38.field_2651;
													num = (int)(0xF688C53 ^ num5);
													continue;
												}
												if ((object)"" == "" && (object)"" == "")
												{
													msg5 = Class38.field_2650;
												}
												num = (int)(0x660051C9 ^ num5);
												continue;
											}
											msg6 = Class38.field_2649;
											int num27 = (int)num5 ^ -1621653839;
											num = (num27 + -995135583 - -995135583) ^ -1621653839 ^ 0x2DCB1826;
											continue;
										}
										msg2 = Class38.field_2080;
										num = (int)((num5 << 2) | (num5 >> 30)) ^ -2109889888;
										continue;
									}
									format = Class38.field_2648;
									num = (int)(0x4F138EC1 ^ num5);
									continue;
								}
								if ((object)"" == "" && (object)"" == "")
								{
									msg = Class38.field_2647;
								}
								num = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x355DC398);
								continue;
							}
							msg3 = Class38.field_2646;
							int num28 = (int)(num5 ^ 0x17A25135);
							num = (num28 + 865671882 - 865671882) ^ 0x17A25135 ^ 0x4446747F;
							continue;
						}
						num7 = 1;
						num = (int)(0xEF13EC8 ^ num5);
						continue;
					}
					if ((object)"" == "")
					{
						num15 = 0;
					}
					num = (int)(0x382B9D2B ^ num5);
					continue;
				}
				if ((object)"" == "")
				{
					name = Class38.field_572;
				}
				num = (int)(0x1ADBF608 ^ num5);
				continue;
				IL_0611:
				num = 1842700876;
				continue;
				IL_1052:
				int num30;
				while (true)
				{
					nint num29 = num30 ^ -1907326601;
					nuint num31 = (nuint)num29;
					int num32 = (int)((num5 = (uint)((int)num31 ^ -1907326601 ^ 0x37E427A4 ^ 0x37E427A4)) % 18);
					if (num32 == 0)
					{
						break;
					}
					if (num32 == 1 || num32 == 1)
					{
						goto IL_1026;
					}
					if (num32 != 2 && num32 != 2)
					{
						if (num32 != 3 && num32 != 3)
						{
							if (num32 != 4 && num32 != 4)
							{
								if (num32 != 5 && num32 != 5)
								{
									if (num32 != 6 && num32 != 6)
									{
										if (num32 != 7 && num32 != 7)
										{
											if (num32 != 8 && num32 != 8)
											{
												if (num32 != 9)
												{
													if (num32 == 10)
													{
														goto IL_0ee7;
													}
													if (num32 != 11)
													{
														if (num32 != 12 && num32 != 12)
														{
															if (num32 == 13 || num32 == 13)
															{
																if ("".Length != 0)
																{
																}
																num30 = (int)((num5 << 4) | (num5 >> 28)) ^ -197995595;
																continue;
															}
															if (num32 == 14 || num32 == 14)
															{
																UI.RichLogs(msg5, UI.error, (byte)num7 != 0, (byte)num7 != 0);
																num30 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x42DCD938);
																continue;
															}
															if (num32 == 15 || num32 == 15)
															{
																if ((object)"" != "" || (object)"" != "" || (object)"" != "")
																{
																}
																int num33 = (int)num5 ^ -1272266737;
																num30 = (num33 + -151242582 - -151242582) ^ -1272266737 ^ 0x365D0CDD;
																continue;
															}
															if (num32 == 16 || num32 == 16)
															{
																if ((object)"" != "")
																{
																	num30 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0xA20F5B);
																	continue;
																}
																return;
															}
															if (num32 != 17)
															{
																goto IL_1026;
															}
														}
														else if ("".Length != 0 || flag4)
														{
															num30 = (int)(0x48989649 ^ num5);
															continue;
														}
														ZipArchive zipArchive2 = ZipFile.OpenRead(State.FilePath);
														try
														{
															while (true)
															{
																int num34 = 471675853;
																while (true)
																{
																	nint num35 = num34 ^ 0xB0E502F;
																	nuint num36 = (nuint)num35;
																	int num37 = (int)((num5 = (uint)((int)num36 ^ 0xB0E502F ^ -1108748915 ^ -1108748915)) % 9);
																	if (num37 == 0)
																	{
																		break;
																	}
																	if (num37 != 1 && num37 != 1)
																	{
																		if (num37 != 2 && num37 != 2)
																		{
																			if (num37 == 3 || num37 == 3)
																			{
																				num34 = (int)((num5 << 2) | (num5 >> 30)) ^ -706894966;
																				continue;
																			}
																			if (num37 == 4 || num37 == 4)
																			{
																				if ((object)"" == "" && "".Length == 0 && (object)"" == "" && (object)"" == "")
																				{
																					UI.RichLogs(msg4, UI.error, (byte)num7 != 0, (byte)num7 != 0);
																				}
																				int num38 = (int)(num5 ^ 0x495D9103);
																				num34 = (num38 + -964584418 - -964584418) ^ 0x495D9103 ^ 0x7B01C618;
																				continue;
																			}
																			if (num37 == 5 || num37 == 5)
																			{
																				if ((object)"" != "")
																				{
																				}
																				int num39 = (int)num5 ^ -381635985;
																				num34 = (num39 + -1238342424 - -1238342424) ^ -381635985 ^ 0x18D01244;
																				continue;
																			}
																			if (num37 == 6 || num37 == 6)
																			{
																				if ("".Length != 0)
																				{
																					int num40 = (int)(num5 ^ 0x12196453);
																					num34 = (num40 + -745325637 - -745325637) ^ 0x12196453 ^ 0x3285B9C9;
																					continue;
																				}
																				return;
																			}
																			if (num37 != 7)
																			{
																				if (num37 != 8)
																				{
																					goto IL_1281;
																				}
																				IEnumerator<ZipArchiveEntry> enumerator = zipArchive2.Entries.GetEnumerator();
																				try
																				{
																					while (true)
																					{
																						ZipArchiveEntry current;
																						if (enumerator.MoveNext())
																						{
																							while (true)
																							{
																								IL_149f:
																								current = enumerator.Current;
																								while (true)
																								{
																									int num41 = 1348083954;
																									int num44;
																									while (true)
																									{
																										nint num42 = num41 ^ -127067543;
																										nuint num43 = (nuint)num42;
																										num44 = (int)((num5 = (uint)((int)num43 ^ -127067543 ^ 0x438B33B8 ^ 0x438B33B8)) % 8);
																										if (num44 == 0)
																										{
																											num41 = 555174553;
																											continue;
																										}
																										if (num44 == 1)
																										{
																											break;
																										}
																										if (num44 != 2 && num44 != 2)
																										{
																											if (num44 != 3 && num44 != 3)
																											{
																												if (num44 != 4 && num44 != 4)
																												{
																													if (num44 == 5 || num44 == 5)
																													{
																														goto end_IL_1498;
																													}
																													if (num44 != 6)
																													{
																														goto IL_148b;
																													}
																												}
																												else if ((object)"" != "" || flag5)
																												{
																													int num45 = (int)(num5 ^ 0x4FF701C6);
																													num41 = (num45 + -1225384423 - -1225384423) ^ 0x4FF701C6 ^ 0x525F76D9;
																													continue;
																												}
																												partitionName = Path.GetFileNameWithoutExtension(current.FullName).ToLower();
																												num41 = 1077381503;
																											}
																											else
																											{
																												if ((object)"" == "" && (object)"" == "")
																												{
																													flag5 = (current.FullName.EndsWith(value, (StringComparison)comparisonType) ? 1 : 0) == num15;
																												}
																												int num46 = (int)(num5 ^ 0x4FC8E3C6);
																												num41 = (num46 + 597967787 - 597967787) ^ 0x4FC8E3C6 ^ 0x309B587;
																											}
																										}
																										else
																										{
																											if ("".Length != 0 || "".Length != 0 || (object)"" != "" || (object)"" != "")
																											{
																											}
																											int num47 = (int)(num5 ^ 0x20C08D9B);
																											num41 = (num47 + 112127730 - 112127730) ^ 0x20C08D9B ^ 0x652F4319;
																										}
																									}
																									goto IL_1490;
																									IL_1490:
																									if ("".Length != 0)
																									{
																										continue;
																									}
																									goto IL_149f;
																									IL_148b:
																									if (num44 != 7)
																									{
																										goto IL_1490;
																									}
																									goto IL_14aa;
																									continue;
																									end_IL_1498:
																									break;
																								}
																								break;
																							}
																							continue;
																						}
																						goto IL_1699;
																						IL_1699:
																						int num48 = 1552368041;
																						goto IL_169e;
																						IL_14aa:
																						Stream stream = current.Open();
																						try
																						{
																							MemoryStream memoryStream = new MemoryStream();
																							try
																							{
																								stream.CopyTo(memoryStream);
																								while (true)
																								{
																									int num49 = 1619640741;
																									while (true)
																									{
																										nint num50 = num49 ^ 0x196C0EE2;
																										nuint num51 = (nuint)num50;
																										int num52 = (int)((num5 = (uint)((int)num51 ^ 0x196C0EE2 ^ -2049172959 ^ -2049172959)) % 4);
																										if (num52 == 0)
																										{
																											break;
																										}
																										if (num52 != 1 && num52 != 1)
																										{
																											if (num52 == 2 || num52 == 2)
																											{
																												if ("".Length == 0)
																												{
																													WritePartition(partitionName, memoryStream.ToArray(), (byte)num7 != 0);
																												}
																												num49 = (int)(0x3835ED19 ^ num5);
																												continue;
																											}
																											if (num52 == 3)
																											{
																												goto end_IL_1570;
																											}
																										}
																										if ((object)"" != "")
																										{
																										}
																										int num53 = (int)num5 ^ -818857736;
																										num49 = (num53 + 1199110005 - 1199110005) ^ -818857736 ^ 0xD699FD3;
																									}
																									continue;
																									end_IL_1570:
																									break;
																								}
																							}
																							finally
																							{
																								if (memoryStream != null)
																								{
																									while (true)
																									{
																										int num54 = 792867429;
																										while (true)
																										{
																											nint num55 = num54 ^ -1058720048;
																											nuint num56 = (nuint)num55;
																											int num57 = (int)((num5 = (uint)((int)num56 ^ -1058720048 ^ 0x5F1CBD55 ^ 0x5F1CBD55)) % 4);
																											if (num57 == 0)
																											{
																												break;
																											}
																											if (num57 != 1 && num57 != 1)
																											{
																												if (num57 == 2 || num57 == 2)
																												{
																													num54 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x46DCE823);
																													continue;
																												}
																												if (num57 == 3)
																												{
																													goto end_IL_15ec;
																												}
																											}
																											((IDisposable)memoryStream).Dispose();
																											num54 = (int)(0x25470D3F ^ num5);
																										}
																										continue;
																										end_IL_15ec:
																										break;
																									}
																								}
																							}
																						}
																						finally
																						{
																							if (stream != null)
																							{
																								while (true)
																								{
																									int num58 = 1310465753;
																									while (true)
																									{
																										nint num59 = num58 ^ 0x25D58E7D;
																										nuint num60 = (nuint)num59;
																										int num61 = (int)((num5 = (uint)((int)num60 ^ 0x25D58E7D ^ -1641115641 ^ -1641115641)) % 4);
																										if (num61 == 0)
																										{
																											break;
																										}
																										if (num61 != 1 && num61 != 1)
																										{
																											if (num61 == 2 || num61 == 2)
																											{
																												if ("".Length != 0)
																												{
																												}
																												num58 = (int)((num5 << 3) | (num5 >> 29)) ^ -1223052348;
																												continue;
																											}
																											if (num61 == 3)
																											{
																												goto end_IL_167f;
																											}
																										}
																										if ("".Length == 0)
																										{
																											((IDisposable)stream).Dispose();
																										}
																										num58 = (int)(0x3E0BBE4F ^ num5);
																									}
																									continue;
																									end_IL_167f:
																									break;
																								}
																							}
																						}
																						goto IL_16cf;
																						IL_16cf:
																						num48 = 1945378492;
																						goto IL_169e;
																						IL_169e:
																						nint num62 = num48 ^ -1646230477;
																						nuint num63 = (nuint)num62;
																						int num64 = (int)((num5 = (uint)((int)num63 ^ -1646230477 ^ 0x77CBC01F ^ 0x77CBC01F)) % 3);
																						if (num64 != 0)
																						{
																							if (num64 == 1 || num64 != 2)
																							{
																								if ("".Length == 0)
																								{
																									continue;
																								}
																								goto IL_1699;
																							}
																							break;
																						}
																						goto IL_16cf;
																					}
																					return;
																				}
																				finally
																				{
																					if (enumerator != null)
																					{
																						while (true)
																						{
																							int num65 = 2103032673;
																							while (true)
																							{
																								nint num66 = num65 ^ -62422356;
																								nuint num67 = (nuint)num66;
																								int num68 = (int)((num5 = (uint)((int)num67 ^ -62422356 ^ -135138957 ^ -135138957)) % 4);
																								if (num68 == 0)
																								{
																									break;
																								}
																								if (num68 != 1 && num68 != 1)
																								{
																									if (num68 == 2 || num68 == 2)
																									{
																										num65 = (int)((num5 << 4) | (num5 >> 28)) ^ -609341536;
																										continue;
																									}
																									if (num68 == 3)
																									{
																										goto end_IL_1783;
																									}
																								}
																								if ("".Length == 0 && "".Length == 0 && (object)"" == "")
																								{
																									enumerator.Dispose();
																								}
																								num65 = (int)(0x23D5C33 ^ num5);
																							}
																							continue;
																							end_IL_1783:
																							break;
																						}
																					}
																				}
																			}
																			if ((object)"" == "" && "".Length != 0)
																			{
																			}
																		}
																		else if ("".Length != 0 || "".Length != 0 || flag6)
																		{
																			int num69 = (int)(num5 ^ 0x426D093B);
																			num34 = (num69 + -1104523861 - -1104523861) ^ 0x426D093B ^ 0x5B4FA470;
																			continue;
																		}
																		num34 = 1954787111;
																		continue;
																	}
																	goto IL_1281;
																	IL_1281:
																	if ("".Length == 0 && (object)"" == "")
																	{
																		flag6 = zipArchive2.Entries.Count == num15;
																	}
																	num34 = (int)(0x62D1315B ^ num5);
																}
															}
														}
														finally
														{
															if (zipArchive2 != null)
															{
																while (true)
																{
																	int num70 = 1982802725;
																	while (true)
																	{
																		nint num71 = num70 ^ 0x40DE09AD;
																		nuint num72 = (nuint)num71;
																		int num73 = (int)((num5 = (uint)((int)num72 ^ 0x40DE09AD ^ 0x7257207C ^ 0x7257207C)) % 4);
																		if (num73 == 0)
																		{
																			break;
																		}
																		if (num73 != 1 && num73 != 1)
																		{
																			if (num73 == 2 || num73 == 2)
																			{
																				num70 = (int)((num5 << 4) | (num5 >> 28)) ^ -175466598;
																				continue;
																			}
																			if (num73 == 3)
																			{
																				goto end_IL_184c;
																			}
																		}
																		if ((object)"" == "" && (object)"" == "")
																		{
																			((IDisposable)zipArchive2).Dispose();
																		}
																		int num74 = (int)(num5 ^ 0x3EC34F69);
																		num70 = (num74 + -753345838 - -753345838) ^ 0x3EC34F69 ^ 0x2ECD75C3;
																	}
																	continue;
																	end_IL_184c:
																	break;
																}
															}
														}
													}
													goto IL_108a;
												}
												if ((object)"" != "")
												{
													num30 = 49175344;
													continue;
												}
												return;
											}
											if ("".Length == 0)
											{
												Process.Start(new ProcessStartInfo
												{
													FileName = _003C_003Ec__DisplayClass14_.savepath,
													UseShellExecute = ((byte)num7 != 0)
												});
											}
											num30 = (int)(0x5DCD5E39 ^ num5);
											continue;
										}
										if ("".Length != 0 || "".Length != 0 || flag7)
										{
											num30 = (int)(0x75C51275 ^ num5);
											continue;
										}
										return;
									}
									flag7 = openPath;
									int num75 = (int)(num5 ^ 0x7447DB3C);
									num30 = (num75 + 1071220192 - 1071220192) ^ 0x7447DB3C ^ 0x465469B1;
									continue;
								}
								if ((object)"" != "")
								{
								}
								num30 = (int)(0x3BC02465 ^ num5);
								continue;
							}
							UI.RichLogs(_003C_003Ec__DisplayClass14_.savepath, UI.info, (byte)num7 != 0, (byte)num7 != 0);
							num30 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x52386BC9);
							continue;
						}
						if ("".Length != 0)
						{
						}
						num30 = (int)(0x306A03ED ^ num5);
						continue;
					}
					UI.RichLogs(msg6, UI.log, (byte)num15 != 0, (byte)num15 != 0);
					num30 = (int)((num5 << 3) | (num5 >> 29)) ^ -1743640175;
					continue;
					IL_1026:
					if ("".Length != 0)
					{
					}
					int num76 = (int)num5 ^ -347003317;
					num30 = (num76 + 246244909 - 246244909) ^ -347003317 ^ 0x5CED4365;
				}
				goto IL_0d0f;
				IL_10ab:
				int num77;
				if (string.IsNullOrEmpty(State.FilePath))
				{
					num77 = num7;
					goto IL_10a7;
				}
				goto IL_0f08;
				IL_1092:
				num30 = 1894210734;
				goto IL_1052;
				IL_0ee7:
				if ((object)"" != "" || "".Length != 0 || "".Length != 0)
				{
					goto IL_0f08;
				}
				goto IL_10ab;
				IL_0f08:
				num30 = 192225863;
				goto IL_1052;
				IL_0d0f:
				num30 = 149535055;
				goto IL_1052;
				IL_108a:
				if ("".Length != 0)
				{
					goto IL_1092;
				}
				num77 = (((File.Exists(State.FilePath) ? 1 : 0) == num15) ? 1 : 0);
				goto IL_10a7;
				IL_0605:
				flag2 = num6 < eDLEfs.Count;
				goto IL_0611;
				IL_0979:
				if ((object)"" != "")
				{
				}
				num = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1BD6A664);
				continue;
				IL_10a7:
				flag4 = (byte)num77 != 0;
				goto IL_1092;
				IL_03b1:
				num = 1503373094;
			}
		}
	}

	private bool MiCloudOperation(List<string> partition)
	{
		string msg = "Reading ";
		string text = "... ";
		int result = 1;
		string format = "{0}";
		string text2 = "";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "Patching Data... ";
		object obj = null;
		string text3 = "modem";
		string text4 = "modem_a";
		string text5 = "modem_b";
		string item = "CardApp";
		string item2 = "marsAPP";
		string item3 = "CARDAPP";
		string item4 = "SLOTAPP";
		string item5 = "cardapp";
		string item6 = "FARDAPP";
		string item7 = "46T6GQ~2B0";
		string item8 = "FARDAPP B0";
		string item9 = "GGP2M5~2B0";
		string item10 = "CILBGY~2B0";
		string item11 = "FFHAND~2B0";
		string item12 = "4AZJED~2B0";
		string item13 = "RT4FMF~2B0";
		string item14 = "TMTCXU~2B0";
		string item15 = "GLN47I~2B0";
		string text6 = "persist";
		string item16 = "fdsd";
		string item17 = "ftst";
		string msg5 = "Writing ";
		string msg6 = "lock flag not found!";
		foreach (string item18 in partition)
		{
			(bool, string, long, int, long, string) tuple = DetectPartition(item18);
			var (flag, text7, num, num2, num3, _) = tuple;
			_ = tuple.Item6;
			if (!flag)
			{
				continue;
			}
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(text7 + text, UI.info, nextline: false, bold: true);
			byte[] array = Firehose.ReadFlashByte(num2, string.Format(format, num), (int)num3, text2, text2);
			if (array.Length >= 0)
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg4, UI.log);
				bool flag2 = false;
				byte[] data = (byte[])obj;
				if (text7 == text3 || text7 == text4 || text7 == text5)
				{
					(flag2, data) = FindMiCloudVuln(array, new List<(string, string)>
					{
						(item, item2),
						(item3, item4),
						(item5, item4),
						(item3, item6),
						(item3, item2),
						(item7, item8),
						(item9, item8),
						(item10, item8),
						(item11, item8),
						(item12, item8),
						(item13, item8),
						(item14, item8),
						(item15, item8)
					});
				}
				else if (text7 == text6)
				{
					(flag2, data) = FindMiCloudVuln(array, new List<(string, string)> { (item16, item17) });
				}
				if (!flag2)
				{
					UI.RichLogs(msg6, UI.error, nextline: true, bold: true);
					continue;
				}
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg5, UI.log);
				UI.RichLogs(text7 + text, UI.info, nextline: false, bold: true);
				if (Firehose.WriteFlash(data, string.Format(format, Firehose.cfg.SectorSize), string.Format(format, num2), string.Format(format, num), (int)num3, text7))
				{
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				}
			}
			else
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			}
		}
		return (byte)result != 0;
	}

	private (bool, byte[]) FindMiCloudVuln(byte[] input, List<(string, string)> patched)
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

	private bool XMLOperation()
	{
		string text = "Reset Lock";
		string text2 = "Reset FRP";
		string text3 = "Reset EFS";
		string text4 = "Reset EFS Method 2";
		string text5 = "Safe Format";
		string text6 = "Unlock Bootloader";
		string text7 = "reset.xml";
		string text8 = "frp.xml";
		string text9 = "efs.xml";
		string text10 = "efs2.xml";
		string text11 = "safe.xml";
		string text12 = "ubl.xml";
		object obj = null;
		string text13 = "Qualcomm/";
		string text14 = "/";
		string msg = "Downloading Resources... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string text15 = "xml get server = ";
		string text16 = "patch";
		string text17 = "erase";
		string text18 = "program";
		string name = "SECTOR_SIZE_IN_BYTES";
		string text19 = "512";
		string memory = "emmc";
		string text20 = "emmc = ";
		string text21 = "ufs = ";
		string memory2 = "ufs";
		string name2 = "filename";
		string text22 = "DISK";
		string name3 = "byte_offset";
		string name4 = "physical_partition_number";
		string name5 = "size_in_bytes";
		string name6 = "start_sector";
		string name7 = "value";
		string name8 = "what";
		string text23 = "... ";
		string name9 = "num_partition_sectors";
		string name10 = "label";
		string text24 = "";
		string path = "Data";
		string path2 = "img";
		string text25 = "misc_emmc";
		string text26 = "misc_ufs";
		string text27 = "misc_safe";
		string text28 = "ubl";
		string text29 = "emmc.img";
		string text30 = "ufs.img";
		string text31 = "safe.img";
		string text32 = "ubl.img";
		string operation = State.Operation;
		string text33 = (string)((operation == text) ? text7 : ((operation == text2) ? text8 : ((operation == text3) ? text9 : ((operation == text4) ? text10 : ((!(operation == text5)) ? ((!(operation == text6)) ? obj : text12) : text11)))));
		string text34 = text33;
		if (text34 != obj)
		{
			string filename = text13 + DeviceState.Brand + text14 + DeviceState.Model + text14 + text34;
			UI.RichLogs(msg, UI.log);
			string text35 = Server.DownloadXML(filename);
			text35 = text35.TrimStart('\ufeff', '\u200b', '\0', '\r', '\n', ' ');
			if (text35 != null && text35.Length < 0)
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(text35));
			xmlTextReader = new XmlTextReader(new StringReader(text35));
			Console.WriteLine(text15 + text35);
			while (xmlTextReader.Read())
			{
				if ((xmlTextReader.NodeType != XmlNodeType.Element || !(xmlTextReader.Name == text16)) && !(xmlTextReader.Name == text17) && !(xmlTextReader.Name == text18))
				{
					continue;
				}
				UI.SectorSize = xmlTextReader.GetAttribute(name);
				break;
			}
			if (!(UI.SectorSize == text19))
			{
				Console.WriteLine(text21 + UI.SectorSize.ToString());
				Firehose.Configure(memory2);
			}
			else
			{
				Firehose.Configure(memory);
				Console.WriteLine(text20 + UI.SectorSize.ToString());
			}
			int num = 0;
			int num2 = 0;
			XmlTextReader xmlTextReader2 = new XmlTextReader(new StringReader(text35));
			while (xmlTextReader2.Read())
			{
				if (xmlTextReader2.NodeType == XmlNodeType.Element && xmlTextReader2.Name == text16)
				{
					string attribute = xmlTextReader2.GetAttribute(name2);
					if (attribute.ToUpper() == text22)
					{
						num++;
					}
				}
			}
			xmlTextReader2 = new XmlTextReader(new StringReader(text35));
			while (true)
			{
				if (xmlTextReader2.Read())
				{
					if (xmlTextReader2.NodeType != XmlNodeType.Element || !(xmlTextReader2.Name == text16))
					{
						if (xmlTextReader2.NodeType == XmlNodeType.Element && xmlTextReader2.Name == text17)
						{
							string attribute2 = xmlTextReader2.GetAttribute(name);
							string attribute3 = xmlTextReader2.GetAttribute(name9);
							string attribute4 = xmlTextReader2.GetAttribute(name4);
							string attribute5 = xmlTextReader2.GetAttribute(name6);
							xmlTextReader2.GetAttribute(name10);
							string dataType = Pkts.PktErase(attribute2, attribute3, attribute4, attribute5);
							UI.RichLogs(State.Operation + text23, UI.log);
							if (Firehose.XmlSend(dataType))
							{
								UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
							}
							else
							{
								UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
							}
						}
						else
						{
							if (xmlTextReader2.NodeType != XmlNodeType.Element || !(xmlTextReader2.Name == text18))
							{
								continue;
							}
							string attribute6 = xmlTextReader2.GetAttribute(name);
							string attribute7 = xmlTextReader2.GetAttribute(name9);
							string attribute8 = xmlTextReader2.GetAttribute(name2);
							string attribute9 = xmlTextReader2.GetAttribute(name4);
							string attribute10 = xmlTextReader2.GetAttribute(name10);
							string attribute11 = xmlTextReader2.GetAttribute(name6);
							if (string.IsNullOrEmpty(attribute8))
							{
								UI.RichLogs(State.Operation + text23, UI.log);
								if (!Firehose.FormatFlash(attribute6, attribute9, attribute11 ?? text24, attribute7, text24, text24))
								{
									UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
								}
								else
								{
									UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
								}
								continue;
							}
							string path3 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path, path2);
							text33 = (string)((attribute8 == text25) ? text29 : ((attribute8 == text26) ? text30 : ((!(attribute8 == text27)) ? ((!(attribute8 == text28)) ? obj : text32) : text31)));
							string text36 = text33;
							if (text36 == obj)
							{
								break;
							}
							byte[] array = File.ReadAllBytes(Path.Combine(path3, text36));
							if (array.Length <= 1)
							{
								UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
								continue;
							}
							UI.RichLogs(State.Operation + text23, UI.log);
							if (!Firehose.WriteFlash(array, attribute6, attribute9, attribute11, int.Parse(attribute7), attribute10))
							{
								UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
							}
							else
							{
								UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
							}
						}
						continue;
					}
					string attribute12 = xmlTextReader2.GetAttribute(name);
					string attribute13 = xmlTextReader2.GetAttribute(name3);
					string attribute14 = xmlTextReader2.GetAttribute(name2);
					string attribute15 = xmlTextReader2.GetAttribute(name4);
					string attribute16 = xmlTextReader2.GetAttribute(name5);
					string attribute17 = xmlTextReader2.GetAttribute(name6);
					string attribute18 = xmlTextReader2.GetAttribute(name7);
					string attribute19 = xmlTextReader2.GetAttribute(name8);
					if (!(attribute14.ToUpper() == text22))
					{
						continue;
					}
					string dataType2 = Pkts.PktPatch(attribute12, attribute13, attribute14, attribute15, attribute16, attribute17, attribute18, attribute19);
					num2++;
					if (num2 == 1)
					{
						UI.RichLogs(State.Operation + text23, UI.log);
					}
					if (Firehose.XmlSend(dataType2))
					{
						UI.Progresbar1(num2, num);
						if (num2 == num)
						{
							UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						}
						continue;
					}
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				}
				return true;
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		return false;
	}

	private bool SetSlot(bool setA = true, bool setB = true)
	{
		string boot = "0";
		string msg = "Set Boot Slot ";
		string msg2 = "[A]";
		string msg3 = "... ";
		string msg4 = "Failed!";
		string msg5 = "OK";
		string boot2 = "1";
		string msg6 = "[B]";
		string dataType;
		if (!setA)
		{
			if (!setB)
			{
				return true;
			}
			dataType = Pkts.BootConSet(boot2);
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(msg6, UI.info, nextline: false, bold: true);
			UI.RichLogs(msg3, UI.log);
			if (Firehose.XmlSend(dataType))
			{
				UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
			}
			return true;
		}
		string dataType2 = Pkts.BootConSet(boot);
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(msg2, UI.info, nextline: false, bold: true);
		UI.RichLogs(msg3, UI.log);
		if (!Firehose.XmlSend(dataType2))
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
		}
		if (!setB)
		{
			return true;
		}
		dataType = Pkts.BootConSet(boot2);
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(msg6, UI.info, nextline: false, bold: true);
		UI.RichLogs(msg3, UI.log);
		if (Firehose.XmlSend(dataType))
		{
			UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
		return true;
	}

	private void ShowPartition()
	{
		using List<GptList>.Enumerator enumerator = GptList.GptPartList.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			return;
		}
		GptList current = enumerator.Current;
		foreach (GptEfi partition in current.Partition)
		{
			Main.Instance.EDLDataGrid.BeginInvoke((Action)delegate
			{
				string format = "{0}";
				Main.Instance.EDLDataGrid.Rows.Add(false, string.Format(format, partition.PhysicalPartitionNumber), partition.SectorSize, partition.Label, partition.FileName, utils.FixSize(partition.SizeInKB), partition.NumPartitionSectors, partition.StartSector);
			});
		}
	}

	private bool ReadGPT()
	{
		object obj = null;
		string msg = "Reading partition info... ";
		string text = "Failed";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string format = " [{0}]";
		if (State.Job != Job.Flash && State.Job != Job.Xml)
		{
			if (GptEfi.Uefi != obj)
			{
				GptEfi.Uefi.Clear();
			}
			UI.RichLogs(msg, UI.log);
			GptEfi.Uefi = new List<GptEfi>();
			GptList.GptPartList = new List<GptList>();
			byte[] array;
			Gpt gpt;
			for (int i = 0; i < Firehose.cfg.Maxlun; i++)
			{
				Partition = GetGPT(i, new GptSettings());
				(array, gpt) = Partition;
				if (array == null && gpt == obj)
				{
					Console.WriteLine(i + text);
				}
				else
				{
					GptList.GptPartList.Add(new GptList(GptEfi.Uefi));
				}
			}
			(array, gpt) = Partition;
			if (array == null && gpt == obj)
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg3, UI.ok);
			UI.RichLogs(string.Format(format, GptEfi.Uefi.Count), UI.log, nextline: true);
		}
		return Reconnect(100);
	}

	private (byte[], Gpt) GetGPT(int lun, GptSettings settings)
	{
		object obj = null;
		Partition = Firehose.GetGPT(lun, settings);
		var (array, gpt) = Partition;
		if (array != null)
		{
			return Partition;
		}
		if (gpt == null)
		{
			return ((byte[])obj, (Gpt)obj);
		}
		return Partition;
	}

	private (bool, string, long, int, long, string) DetectPartition(string partitionName)
	{
		object obj = null;
		int item = 0;
		int num = -1;
		int item2 = 1;
		var (array, gpt) = Partition;
		if (array == null && gpt == obj)
		{
			return (false, string.Empty, -1L, -1, -1L, string.Empty);
		}
		foreach (GptList gptPart in GptList.GptPartList)
		{
			foreach (GptEfi item3 in gptPart.Partition)
			{
				if (item3.Label == partitionName)
				{
					return ((byte)item2 != 0, item3.Label, long.Parse(item3.StartSector), item3.PhysicalPartitionNumber, long.Parse(item3.NumPartitionSectors), item3.SizeInKB);
				}
			}
		}
		return ((byte)item != 0, string.Empty, num, num, num, string.Empty);
	}

	private byte[] ReadPartitionBytes(string partitionName)
	{
		string format = "{0}";
		var (flag, label, num, physicalPartitionNumber, num2, sizeInKb) = DetectPartition(partitionName);
		if (!flag)
		{
			return Array.Empty<byte>();
		}
		return Firehose.ReadFlashByte(physicalPartitionNumber, string.Format(format, num), (int)num2, label, sizeInKb);
	}

	private void ReadPartition(List<string> partitionName, string savepath, bool display = true)
	{
		string msg = "Reading ";
		string text = "... ";
		string text2 = "";
		string format = "{0}";
		string text3 = "Failed!";
		string text4 = "OK";
		foreach (string item in partitionName)
		{
			var (flag, label, num, physicalPartitionNumber, num2, sizeInKb) = DetectPartition(item);
			if (flag)
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item + text, UI.info, nextline: false, bold: true);
				}
				bool flag2 = Firehose.ReadFlash(savepath ?? text2, physicalPartitionNumber, string.Format(format, num), (int)num2, label, sizeInKb);
				if (display)
				{
					UI.RichLogs((!flag2) ? text3 : text4, flag2 ? UI.ok : UI.error, nextline: true);
				}
			}
		}
	}

	private bool WritePartition(string partitionName, object file, bool display = true)
	{
		string msg = "Writing ";
		string text = "... ";
		string format = "{0}";
		string msg2 = "Failed!";
		string msg3 = "OK";
		(bool, string, long, int, long, string) tuple = DetectPartition(partitionName);
		var (flag, label, num, num2, num3, _) = tuple;
		_ = tuple.Item6;
		if (flag)
		{
			bool flag2;
			if (!display)
			{
				flag2 = Firehose.WriteFlash(file, string.Format(format, Firehose.cfg.SectorSize), string.Format(format, num2), string.Format(format, num), (int)num3, label);
				if (!display)
				{
					return flag2;
				}
				if (!flag2)
				{
					UI.RichLogs(msg2, flag2 ? UI.ok : UI.error, nextline: true);
				}
				else
				{
					UI.RichLogs(msg3, flag2 ? UI.ok : UI.error, nextline: true);
				}
				return flag2;
			}
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(partitionName + text, UI.info, nextline: false, bold: true);
			flag2 = Firehose.WriteFlash(file, string.Format(format, Firehose.cfg.SectorSize), string.Format(format, num2), string.Format(format, num), (int)num3, label);
			if (!display)
			{
				return flag2;
			}
			if (!flag2)
			{
				UI.RichLogs(msg2, flag2 ? UI.ok : UI.error, nextline: true);
			}
			else
			{
				UI.RichLogs(msg3, flag2 ? UI.ok : UI.error, nextline: true);
			}
			return flag2;
		}
		return false;
	}

	private void ErasePartition(List<string> partitionName, bool display = true)
	{
		string msg = "Erasing ";
		string text = "... ";
		string format = "{0}";
		string text2 = "Failed!";
		string text3 = "OK";
		foreach (string item in partitionName)
		{
			var (flag, label, num, num2, num3, sizeInKb) = DetectPartition(item);
			if (flag)
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item + text, UI.info, nextline: false, bold: true);
				}
				bool flag2 = Firehose.FormatFlash(string.Format(format, Firehose.cfg.SectorSize), string.Format(format, num2), string.Format(format, num), string.Format(format, num3), label, sizeInKb);
				if (display)
				{
					UI.RichLogs((!flag2) ? text2 : text3, flag2 ? UI.ok : UI.error, nextline: true);
				}
			}
		}
	}

	private bool WritePartitionXML(string xml)
	{
		string text = "program";
		string name = "SECTOR_SIZE_IN_BYTES";
		string name2 = "num_partition_sectors";
		string name3 = "filename";
		string name4 = "physical_partition_number";
		string name5 = "label";
		string name6 = "start_sector";
		string msg = "Writing ";
		string text2 = "[";
		string text3 = "]";
		string text4 = "->";
		string msg2 = "... ";
		string msg3 = "OK";
		string msg4 = "Failed";
		string separator = "\n";
		string text5 = "patch";
		string text6 = "DISK";
		string format = "Patch {0} : ";
		string name7 = "byte_offset";
		string name8 = "size_in_bytes";
		string name9 = "value";
		string name10 = "what";
		XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml));
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType != XmlNodeType.Element || !(xmlTextReader.Name == text))
			{
				continue;
			}
			string attribute = xmlTextReader.GetAttribute(name);
			string attribute2 = xmlTextReader.GetAttribute(name2);
			string attribute3 = xmlTextReader.GetAttribute(name3);
			string attribute4 = xmlTextReader.GetAttribute(name4);
			string text7 = xmlTextReader.GetAttribute(name5).ToLower();
			string attribute5 = xmlTextReader.GetAttribute(name6);
			if (JobStatus.Token.IsCancellationRequested)
			{
				return false;
			}
			if (File.Exists(attribute3))
			{
				UI.RichLogs(msg, UI.log);
				UI.RichLogs(text2 + Path.GetFileName(attribute3) + text3, UI.info, nextline: false, bold: true);
				UI.RichLogs(text4 + text7, UI.log);
				UI.RichLogs(msg2, UI.log);
				if (!Firehose.WriteFlash(attribute3, attribute, attribute4, attribute5, int.Parse(attribute2), text7))
				{
					UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				}
			}
		}
		if (EDLConfig.PatchXML.Count > 0 && !JobStatus.Token.IsCancellationRequested)
		{
			string text8 = string.Join(separator, EDLConfig.PatchXML.ToArray());
			string[] array = text8.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				int num = 0;
				int num2 = 0;
				xmlTextReader = new XmlTextReader(new StringReader(File.ReadAllText(array[i])));
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == text5 && xmlTextReader.GetAttribute(name3).ToUpper() == text6)
					{
						num++;
					}
				}
				UI.RichLogs(string.Format(format, i), UI.log);
				xmlTextReader = new XmlTextReader(new StringReader(File.ReadAllText(array[i])));
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == text5)
					{
						string attribute6 = xmlTextReader.GetAttribute(name);
						string attribute7 = xmlTextReader.GetAttribute(name7);
						string attribute8 = xmlTextReader.GetAttribute(name3);
						string attribute9 = xmlTextReader.GetAttribute(name4);
						string attribute10 = xmlTextReader.GetAttribute(name8);
						string attribute11 = xmlTextReader.GetAttribute(name6);
						string attribute12 = xmlTextReader.GetAttribute(name9);
						string attribute13 = xmlTextReader.GetAttribute(name10);
						if (attribute8.ToUpper() == text6)
						{
							string dataType = Pkts.PktPatch(attribute6, attribute7, attribute8, attribute9, attribute10, attribute11, attribute12, attribute13);
							Firehose.XmlSend(dataType);
							num2++;
							UI.Progresbar1(num2, num);
						}
					}
				}
				if (num != num2)
				{
					UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				}
			}
		}
		return true;
	}

	private bool ErasePartitionXML(string xml)
	{
		string text = "program";
		string name = "SECTOR_SIZE_IN_BYTES";
		string name2 = "num_partition_sectors";
		string name3 = "filename";
		string name4 = "physical_partition_number";
		string name5 = "label";
		string name6 = "start_sector";
		string text2 = "Erasing ";
		string text3 = " : ";
		string text4 = "";
		string msg = "OK";
		string msg2 = "Failed";
		XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml));
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == text)
			{
				string attribute = xmlTextReader.GetAttribute(name);
				string attribute2 = xmlTextReader.GetAttribute(name2);
				xmlTextReader.GetAttribute(name3);
				string attribute3 = xmlTextReader.GetAttribute(name4);
				string text5 = xmlTextReader.GetAttribute(name5).ToLower();
				string attribute4 = xmlTextReader.GetAttribute(name6);
				UI.RichLogs(text2 + text5.ToUpper() + text3, UI.log);
				if (Firehose.FormatFlash(attribute, attribute3, attribute4, attribute2, text4, text4))
				{
					UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				}
			}
		}
		return true;
	}

	private bool ReadPartitionXML(List<QCList> rowXml)
	{
		object obj = null;
		string text = "rawprogram";
		string text2 = ".xml";
		int num = 0;
		string format = "rawprogram{0}.xml";
		int num2 = 1;
		string value = "</data>";
		string value2 = "<?xml version=\"1.0\" ?>";
		string value3 = "<data>";
		string value4 = "\t<!--NOTE: genererate by AMFTool **-->";
		string text3 = "\t<!--NOTE: Sector size is ";
		string text4 = " bytes-->";
		string text5 = "Reading ";
		string text6 = "->";
		string text7 = "[";
		string text8 = "]... ";
		string text9 = "\\";
		string format2 = "{0}";
		string format3 = "\t<program SECTOR_SIZE_IN_BYTES=\"{0}\" file_sector_offset=\"0\" filename=\"{1}\" label=\"{2}\" num_partition_sectors=\"{3}\" partofsingleimage=\"{4}\" physical_partition_number=\"{5}\" readbackverify=\"{6}\" size_in_KB=\"{7}\" sparse=\"{8}\" start_byte_hex=\"{9}\" start_sector=\"{10}\" />";
		int num3 = 11;
		int num4 = 2;
		int num5 = 3;
		int num6 = 4;
		int num7 = 5;
		int num8 = 6;
		int num9 = 7;
		int num10 = 8;
		int num11 = 9;
		int num12 = 10;
		string msg = "OK";
		try
		{
			StreamWriter streamWriter = (StreamWriter)obj;
			foreach (QCList item in rowXml)
			{
				string path = Path.Combine(State.SavePath, text + item.PhysicalPartitionNumber + text2);
				if ((File.Exists(path) ? 1 : 0) == num)
				{
					if ((int.Parse(item.PhysicalPartitionNumber) > num) ? File.Exists(Path.Combine(State.SavePath, string.Format(format, int.Parse(item.PhysicalPartitionNumber) - num2))) : ((byte)num != 0))
					{
						streamWriter.WriteLine(value);
						streamWriter.Close();
					}
					streamWriter = new StreamWriter(Path.Combine(State.SavePath, text + item.PhysicalPartitionNumber + text2), (byte)num2 != 0, Encoding.UTF8);
					streamWriter.WriteLine(value2);
					streamWriter.WriteLine(value3);
					streamWriter.WriteLine(value4);
					streamWriter.WriteLine(text3 + item.SectorSize + text4);
				}
				UI.RichLogs(text5 + item.Label + text6, UI.log, (byte)num != 0, (byte)num != 0);
				UI.RichLogs(text7 + item.FileName + text8, UI.info, (byte)num != 0, (byte)num2 != 0);
				if (!JobStatus.Token.IsCancellationRequested)
				{
					if (Firehose.ReadFlash(State.SavePath + text9 + item.FileName, int.Parse(item.PhysicalPartitionNumber), item.StartSector, int.Parse(item.NumPartitionSectors), item.Label, item.SizeInKB))
					{
						foreach (GptEfi item2 in GptList.GptPartList[int.Parse(item.PhysicalPartitionNumber)].Partition)
						{
							if ((item.PhysicalPartitionNumber == string.Format(format2, item2.PhysicalPartitionNumber)) ? (item.Label.ToLower() == item2.Label.ToLower()) : ((byte)num != 0))
							{
								StreamWriter streamWriter2 = streamWriter;
								object[] array = new object[num3];
								array[num] = item2.SectorSize;
								array[num2] = item.FileName;
								array[num4] = item.Label;
								array[num5] = item2.NumPartitionSectors;
								array[num6] = item2.PartOfSingleImage;
								array[num7] = item2.PhysicalPartitionNumber;
								array[num8] = item2.ReadBackVerify;
								array[num9] = item2.SizeInKB;
								array[num10] = item2.Sparse;
								array[num11] = item2.StartByteHex;
								array[num12] = item2.StartSector;
								streamWriter2.WriteLine(string.Format(format3, array));
								break;
							}
						}
						UI.RichLogs(msg, UI.ok, (byte)num2 != 0, (byte)num2 != 0);
						continue;
					}
					streamWriter.WriteLine(value);
					return (byte)num != 0;
				}
				streamWriter.WriteLine(value);
				return (byte)num != 0;
			}
			streamWriter.WriteLine(value);
			streamWriter.Close();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message.ToString());
		}
		return (byte)num2 != 0;
	}

	private bool ShutDown()
	{
		int result = 1;
		if (!Firehose.Shutdown())
		{
			return false;
		}
		try
		{
			SerialEDL.Close();
		}
		catch
		{
		}
		return (byte)result != 0;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
