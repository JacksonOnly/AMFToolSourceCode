using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using AMFTool.Library.Odin;
using AMFTool.Library.Odin.PIT;
using AMFTool.Library.Odin.Protocol;
using AMFTool.Library.Odin.Tar;
using AMFTool.Library.Usb;
using Downloader;

namespace AMFTool.ThreadWorker;

public class ThreadOdin
{
	private class FlashPlan
	{
		public OdinList Selection { get; set; }

		public TarEntryInfo TarEntry { get; set; }

		public long RawSize { get; set; }

		public string NormalizedPartitionName { get; set; }

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	private SerialOdin Serial;

	private Odin Odin;

	private string Model;

	public Downloader.Thread cmd;

	public bool Init()
	{
		string msg = "Initializing Odin Protocol... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "Rebooting... ";
		object serial = null;
		Serial = new SerialOdin();
		LokeCommandService loke = new LokeCommandService(Serial);
		try
		{
			if (!Initializing())
			{
				return false;
			}
			UI.RichLogs(msg, UI.log);
			if (Odin.IsOdinDevice(Serial))
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				switch (State.Job)
				{
				case (Job)86:
					ReadPIT(Serial);
					break;
				case Job.PitWrite:
					WritePIT(Serial);
					break;
				case (Job)88:
					FixSoftBrick(Serial);
					break;
				case Job.FRP2024:
					FrpResetConsol(Serial);
					break;
				case Job.Reset:
					FactoryReset(Serial);
					break;
				case Job.Flash:
					FlashFirmware(Serial, OdinFlashRequest.selection, OdinConfig.Repartition);
					break;
				case Job.Frp:
					FrpReset(Serial);
					break;
				}
				UI.RichLogs(msg4, UI.log);
				Odin.SwitchToNormalMode(loke);
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				return true;
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		finally
		{
			((SerialOdin)(object)Serial)?.Dispose();
			Serial = (SerialOdin)serial;
			Odin = new Odin();
		}
	}

	private bool Initializing()
	{
		string msg = "Waiting Download Mode Connection... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string text = "Connecting to device ";
		string text2 = "... ";
		string msg4 = "";
		string msg5 = "[Device Information] :";
		UI.RichLogs(msg, UI.log);
		if (UsbScan.FindMTP())
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			UI.RichLogs(text + Portinfo.COM + text2, UI.log);
			if (Serial.Open(Portinfo.COM))
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				byte[] array = new byte[4] { 68, 86, 73, 70 };
				Serial.Write(array, array.Length);
				System.Threading.Thread.Sleep(400);
				string text3 = Serial.ReadExisting();
				if (string.IsNullOrEmpty(text3))
				{
					return true;
				}
				UI.RichLogs(msg4, UI.log, nextline: true);
				UI.RichLogs(msg5, UI.log, nextline: true);
				ParseStringResponse(text3);
				return true;
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		return false;
	}

	private bool ParseStringResponse(string response)
	{
		string oldValue = "@#";
		string text = "";
		string text2 = "MODEL";
		string value = "Model\t\t";
		string key = "UN";
		string value2 = "Serial Number\t";
		string key2 = "CAPA";
		string value3 = "Storage (GB)\t";
		string key3 = "VENDOR";
		string value4 = "Vendor\t\t";
		string key4 = "FWVER";
		string value5 = "Firmware Version";
		string key5 = "PRODUCT";
		string value6 = "Product Code\t";
		string key6 = "SALES";
		string value7 = "Sales Code\t";
		string key7 = "VER";
		string value8 = "Software Version";
		string key8 = "DID";
		string value9 = "Device ID\t";
		string key9 = "TMU_TEMP";
		string value10 = "Temperature\t";
		string key10 = "RAND";
		string value11 = "Random ID";
		string key11 = "PROV";
		string value12 = "PROV\t\t";
		string text3 = "\t: ";
		response = response.Replace(oldValue, text);
		string[] array = response.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ text2, value },
			{ key, value2 },
			{ key2, value3 },
			{ key3, value4 },
			{ key4, value5 },
			{ key5, value6 },
			{ key6, value7 },
			{ key7, value8 },
			{ key8, value9 },
			{ key9, value10 },
			{ key10, value11 },
			{ key11, value12 }
		};
		string[] array2 = array;
		foreach (string text4 in array2)
		{
			string[] array3 = text4.Split('=');
			if (array3.Length != 2)
			{
				continue;
			}
			string text5 = array3[0].Trim();
			string text6 = array3[1].Trim();
			if (dictionary.ContainsKey(text5))
			{
				string text7 = dictionary[text5];
				if (text5 == text2)
				{
					Model = text6;
				}
				UI.RichLogs(text7 + text3, UI.log);
				UI.RichLogs(text6, UI.info, nextline: true);
			}
			else
			{
				UI.RichLogs(text5 + text3, UI.log);
				UI.RichLogs(text6, UI.info, nextline: true);
			}
		}
		UI.RichLogs(text, UI.log, nextline: true);
		return true;
	}

	private FlashPlan BuildAutoPitFlashPlan(List<OdinList> selection, Dictionary<string, Dictionary<string, TarEntryInfo>> tarEntryLookup)
	{
		object obj = null;
		string text = "pit";
		List<string> list = (from x in selection
			where !string.IsNullOrWhiteSpace(x.TarPath)
			orderby GetFlashTypeOrder(x.Type)
			select x.TarPath).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
		foreach (string tarPath in list)
		{
			if (!tarEntryLookup.TryGetValue(tarPath, out var value))
			{
				continue;
			}
			TarEntryInfo tarEntryInfo = value.Values.FirstOrDefault((TarEntryInfo x) => IsPitTarEntryName(x.Name));
			if (tarEntryInfo == obj)
			{
				continue;
			}
			OdinList odinList = selection.FirstOrDefault((OdinList x) => string.Equals(x.TarPath, tarPath, StringComparison.OrdinalIgnoreCase));
			object obj2;
			if (odinList == null)
			{
				obj2 = obj;
				if (obj2 != null)
				{
					goto IL_00fd;
				}
			}
			else
			{
				obj2 = odinList.Type;
				if (obj2 != null)
				{
					goto IL_00fd;
				}
			}
			obj2 = string.Empty;
			goto IL_00fd;
			IL_00fd:
			string type = (string)obj2;
			string text2 = tarEntryInfo.Name ?? text;
			return new FlashPlan
			{
				Selection = new OdinList
				{
					Type = type,
					PartitionName = NormalizePartitionName(text2),
					EntryName = text2,
					TarPath = tarPath,
					BeginAddress = tarEntryInfo.DataStartOffset,
					EndAddress = tarEntryInfo.DataEndOffset
				},
				TarEntry = tarEntryInfo,
				RawSize = tarEntryInfo.Size,
				NormalizedPartitionName = NormalizePartitionName(text2)
			};
		}
		return (FlashPlan)obj;
	}

	private bool IsPitTarEntryName(string entryName)
	{
		string value = ".pit";
		string b = "pit";
		if (!string.IsNullOrWhiteSpace(entryName))
		{
			if (!entryName.EndsWith(value, StringComparison.OrdinalIgnoreCase))
			{
				string fileName = Path.GetFileName(entryName);
				return string.Equals(fileName, b, StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}
		return false;
	}

	private int GetFlashTypeOrder(string type)
	{
		object obj = null;
		string text = "BL";
		string text2 = "AP";
		string text3 = "CP";
		string text4 = "CSC";
		string text5 = "HOME";
		if (type != obj)
		{
			string text6 = type.Trim().ToUpperInvariant();
			if (text6 == text)
			{
				return 0;
			}
			if (!(text6 == text2))
			{
				if (!(text6 == text3))
				{
					if (text6 == text4)
					{
						return 3;
					}
					if (text6 == text5)
					{
						return 4;
					}
					return 100;
				}
				return 2;
			}
			return 1;
		}
		return 100;
	}

	private Dictionary<string, Dictionary<string, TarEntryInfo>> BuildTarEntryLookup(List<OdinList> selection, Tar tar)
	{
		int num = 0;
		Dictionary<string, Dictionary<string, TarEntryInfo>> dictionary = new Dictionary<string, Dictionary<string, TarEntryInfo>>(StringComparer.OrdinalIgnoreCase);
		List<string> list = (from x in selection
			where !string.IsNullOrWhiteSpace(x.TarPath)
			select x.TarPath).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
		foreach (string item in list)
		{
			if ((File.Exists(item) ? 1 : 0) == num)
			{
				continue;
			}
			Dictionary<string, TarEntryInfo> dictionary2 = new Dictionary<string, TarEntryInfo>(StringComparer.Ordinal);
			IList<TarEntryInfo> entries = tar.GetEntries(item);
			foreach (TarEntryInfo item2 in entries)
			{
				if ((dictionary2.ContainsKey(item2.Name) ? 1 : 0) == num)
				{
					dictionary2.Add(item2.Name, item2);
				}
			}
			dictionary[item] = dictionary2;
		}
		return dictionary;
	}

	private List<FlashPlan> BuildPlans(List<OdinList> selection, Dictionary<string, Dictionary<string, TarEntryInfo>> tarEntryLookup, Tar tar)
	{
		List<FlashPlan> list = new List<FlashPlan>();
		foreach (OdinList item in selection)
		{
			if (!string.IsNullOrWhiteSpace(item.TarPath) && !string.IsNullOrWhiteSpace(item.EntryName) && tarEntryLookup.TryGetValue(item.TarPath, out var value) && value.TryGetValue(item.EntryName, out var value2))
			{
				string normalizedPartitionName = NormalizePartitionName(string.IsNullOrWhiteSpace(item.PartitionName) ? item.EntryName : item.PartitionName);
				long rawSize = (item.IsPit ? value2.Size : (item.IsLz4 ? tar.GetLz4DecodedSizeFromEntry(item.TarPath, item.EntryName) : value2.Size));
				list.Add(new FlashPlan
				{
					Selection = item,
					TarEntry = value2,
					RawSize = rawSize,
					NormalizedPartitionName = normalizedPartitionName
				});
			}
		}
		return list;
	}

	private string NormalizePartitionName(string filename)
	{
		string value = ".lz4";
		if (!string.IsNullOrWhiteSpace(filename))
		{
			string text = filename.Replace('\\', '/');
			int num = text.LastIndexOf('/');
			bool flag;
			if (num < 0)
			{
				if (!(flag = text.EndsWith(value, StringComparison.OrdinalIgnoreCase)))
				{
					return text;
				}
				return text.Substring(0, text.Length - 4);
			}
			if (num < text.Length - 1)
			{
				text = text.Substring(num + 1);
				flag = text.EndsWith(value, StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				flag = text.EndsWith(value, StringComparison.OrdinalIgnoreCase);
			}
			if (!flag)
			{
				return text;
			}
			return text.Substring(0, text.Length - 4);
		}
		return string.Empty;
	}

	private bool FlashFirmware(SerialOdin serial, List<OdinList> selection, bool repartition)
	{
		int result = 0;
		string msg = "No valid selected entries to flash.";
		int num = 1;
		string text = "Total flash size : ";
		string msg2 = "Initializing Device... ";
		string msg3 = "Failed!";
		string msg4 = "OK";
		object obj = null;
		string text2 = "Auto selected PIT : ";
		string text3 = "Writing PIT : ";
		string msg5 = "Repartition enabled but PIT file not found in selected firmware.";
		string msg6 = "Reading device PIT... ";
		string text4 = "Skip ";
		string text5 = " (partition not in device PIT)";
		string text6 = "Flashing ";
		string text7 = " ... ";
		Tar tar = new Tar();
		try
		{
			LokeCommandService lokeCommandService = new LokeCommandService(serial);
			Dictionary<string, Dictionary<string, TarEntryInfo>> tarEntryLookup = BuildTarEntryLookup(selection, tar);
			List<FlashPlan> list = BuildPlans(selection, tarEntryLookup, tar);
			if (list.Count == 0)
			{
				UI.RichLogs(msg, UI.error, nextline: true, bold: true);
				return false;
			}
			long num2 = list.Where((FlashPlan x) => !x.Selection.IsPit).Sum((FlashPlan x) => x.RawSize);
			UI.RichLogs(text + utils.FileSize(num2), UI.log, nextline: true);
			UI.RichLogs(msg2, UI.log);
			if (lokeCommandService.InitializeSession(num2))
			{
				UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				if (repartition)
				{
					FlashPlan flashPlan = list.FirstOrDefault((FlashPlan x) => x.Selection.IsPit || IsPitTarEntryName(x.Selection.EntryName));
					if (flashPlan == obj)
					{
						flashPlan = BuildAutoPitFlashPlan(selection, tarEntryLookup);
						if (flashPlan !=null)
						{
							UI.RichLogs(text2 + flashPlan.Selection.EntryName, UI.warning, nextline: true);
						}
					}
					if (flashPlan ==null)
					{
						UI.RichLogs(msg5, UI.error, nextline: true, bold: true);
					}
					else
					{
						UI.RichLogs(text3 + flashPlan.Selection.EntryName, UI.log);
						byte[] pitData = tar.ExtractEntry(flashPlan.Selection.TarPath, flashPlan.Selection.EntryName);
						if (!Odin.WritePitData(serial, lokeCommandService, pitData))
						{
							UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
							return false;
						}
						UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
					}
				}
				UI.RichLogs(msg6, UI.log);
				PitResult pitResult = Odin.ReadPitTable(serial, lokeCommandService);
				if (!pitResult.IsSuccess)
				{
					UI.RichLogs(pitResult.ErrorMessage, UI.error, nextline: true, bold: true);
					return false;
				}
				UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				Dictionary<string, PitEntry> dictionary = pitResult.PitEntries.GroupBy((PitEntry x) => NormalizePartitionName(x.FlashFileName), StringComparer.OrdinalIgnoreCase).ToDictionary((IGrouping<string, PitEntry> x) => x.Key, (IGrouping<string, PitEntry> x) => x.First(), StringComparer.OrdinalIgnoreCase);
				foreach (FlashPlan item in list)
				{
					if (item.Selection.IsPit)
					{
						continue;
					}
					if (dictionary.TryGetValue(item.NormalizedPartitionName, out var value))
					{
						UI.RichLogs(text6 + item.Selection.EntryName + text7, UI.log);
						if (!Odin.FlashTarEntryToPartition(serial, lokeCommandService, item.Selection.TarPath, item.TarEntry, value, item.RawSize))
						{
							UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
							return false;
						}
						UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
					}
					else
					{
						UI.RichLogs(text4 + item.Selection.EntryName + text5, UI.warning, nextline: true);
					}
				}
				return (byte)num != 0;
			}
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	public bool FactoryReset(SerialOdin serial)
	{
		string msg = "Initializing Device... ";
		int result = 0;
		string msg2 = "Failed!";
		int num = 1;
		string msg3 = "OK";
		string msg4 = "Factory reset... ";
		try
		{
			LokeCommandService lokeCommandService = new LokeCommandService(serial);
			UI.RichLogs(msg, UI.log);
			if (lokeCommandService.InitializeSession(0L))
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg4, UI.log);
				if (!Odin.FactoryResetUserdata(serial, lokeCommandService))
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
					return false;
				}
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				return true;
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	private bool FrpReset(SerialOdin serial)
	{
		string msg = "Initializing Device... ";
		int result = 0;
		string msg2 = "Failed!";
		int num = 1;
		string msg3 = "OK";
		string msg4 = "Reading pit from devices... ";
		string name = "PIT";
		string format = "{0}_{1:ddMMyyyy_HHmmss}.pit";
		string msg5 = "Waiting Download Mode Connection... ";
		string msg6 = "Removing Frp... ";
		try
		{
			LokeCommandService lokeCommandService = new LokeCommandService(serial);
			UI.RichLogs(msg, UI.log);
			if (lokeCommandService.InitializeSession(0L))
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg4, UI.log);
				string path = utils.CheckBackupFolder(name);
				string path2 = string.Format(format, Model, DateTime.Now);
				if (Odin.ReadPitAndPatchPersistent(serial, lokeCommandService, Path.Combine(path, path2)))
				{
					System.Threading.Thread.Sleep(1000);
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					UI.RichLogs(msg5, UI.log);
					if (UsbScan.FindMTP())
					{
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						UI.RichLogs(msg6, UI.log);
						if (Odin.RestorePitAndClearPersistent(serial, lokeCommandService))
						{
							UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
							return true;
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
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	private bool FrpResetConsol(SerialOdin serial)
	{
		string msg = "Initializing Device... ";
		int result = 0;
		string msg2 = "Failed!";
		int num = 1;
		string msg3 = "OK";
		string msg4 = "Reading pit from devices... ";
		string text = "backup\\";
		string text2 = ".pit";
		string data = "111111111333";
		string msg5 = "Waiting Download Mode Connection... ";
		string msg6 = "Removing Frp... ";
		string data2 = "111111111313";
		try
		{
			LokeCommandService lokeCommandService = new LokeCommandService(serial);
			UI.RichLogs(msg, UI.log);
			if (lokeCommandService.InitializeSession(0L))
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg4, UI.log);
				string pit_back = text + Model + text2;
				if (cmd.ReadBytes(data, pit_back).GetAwaiter().GetResult())
				{
					System.Threading.Thread.Sleep(1000);
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					UI.RichLogs(msg5, UI.log);
					if (UsbScan.FindMTP())
					{
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						UI.RichLogs(msg6, UI.log);
						if (!cmd.WriteBytes(data2).GetAwaiter().GetResult())
						{
							UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
							return false;
						}
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						return true;
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
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	public bool FixSoftBrick(SerialOdin serial)
	{
		string msg = "Initializing Device... ";
		int result = 0;
		string msg2 = "Failed!";
		int num = 1;
		string msg3 = "OK";
		string msg4 = "Fixing softbrick... ";
		try
		{
			LokeCommandService lokeCommandService = new LokeCommandService(serial);
			UI.RichLogs(msg, UI.log);
			if (lokeCommandService.InitializeSession(0L))
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg4, UI.log);
				if (!Odin.SendSoftbrickCommands(lokeCommandService))
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
					return false;
				}
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				return true;
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	private bool WritePIT(SerialOdin serial)
	{
		int result = 0;
		int num = 1;
		string msg = "PIT file is empty.";
		string msg2 = "Initializing Device... ";
		string msg3 = "Failed!";
		string msg4 = "OK";
		string text = "Writing PIT : ";
		string msg5 = "Rebooting... ";
		try
		{
			byte[] array = File.ReadAllBytes(OdinConfig.PitPath);
			if (array != null && array.Length != 0)
			{
				LokeCommandService lokeCommandService = new LokeCommandService(serial);
				UI.RichLogs(msg2, UI.log);
				if (!lokeCommandService.InitializeSession(0L))
				{
					UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
					return false;
				}
				UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				UI.RichLogs(text + Path.GetFileName(OdinConfig.PitPath), UI.log);
				if (!Odin.WritePitData(serial, lokeCommandService, array))
				{
					UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
					return false;
				}
				UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg5, UI.log);
				Odin.SwitchToNormalMode(lokeCommandService);
				UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				return true;
			}
			UI.RichLogs(msg, UI.error, nextline: true, bold: true);
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	private bool ReadPIT(SerialOdin serial)
	{
		string msg = "Initializing Device... ";
		int result = 0;
		string msg2 = "Failed!";
		int num = 1;
		string msg3 = "OK";
		string msg4 = "Reading device PIT... ";
		string format = "PIT entries : {0}";
		string name = "PIT";
		string format2 = "{0}_{1:ddMMyyyy_HHmmss}.pit";
		string msg5 = "Saved to : ";
		try
		{
			LokeCommandService lokeCommandService = new LokeCommandService(serial);
			UI.RichLogs(msg, UI.log);
			if (!lokeCommandService.InitializeSession(0L))
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg4, UI.log);
			PitResult pitResult = Odin.ReadPitTable(serial, lokeCommandService);
			if (pitResult.IsSuccess)
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(string.Format(format, pitResult.PitEntries.Count), UI.info, nextline: true);
				string text = utils.CheckBackupFolder(name);
				string path = string.Format(format2, Model, DateTime.Now);
				File.WriteAllBytes(Path.Combine(text, path), pitResult.PitData);
				UI.RichLogs(msg5, UI.log);
				UI.RichLogs(text, UI.info, nextline: true);
				Process.Start(new ProcessStartInfo
				{
					FileName = text,
					UseShellExecute = true
				});
				return true;
			}
			UI.RichLogs(pitResult.ErrorMessage, UI.error, nextline: true, bold: true);
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	public ThreadOdin()
	{
		string model = "";
		Odin = new Odin();
		Model = model;
		cmd = new Downloader.Thread();

	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
