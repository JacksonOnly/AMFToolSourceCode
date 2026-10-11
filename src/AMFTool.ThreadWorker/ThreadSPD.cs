using System;
using System.Collections.Generic;
using System.IO;
using AMFTool.Library.Spreadtrum;
using AMFTool.Library.Usb;

namespace AMFTool.ThreadWorker;

public class ThreadSPD
{
	private SprdProtocol Spd;

	public (byte[], Gpt) partition;

	public ThreadSPD()
	{
		Spd = new SprdProtocol();
	}

	public bool Init()
	{
		int result = 0;
		string msg = "Failed!";
		int num = 1;
		try
		{
			if (DeviceState.DeviceInit || Initializing())
			{
				switch (State.Job)
				{
				case (Job)80:
					ShutDown();
					goto default;
				case (Job)8:
					WritePartition(DeviceState.Partitiontarget[0], State.FilePath);
					return true;
				case Job.Read:
					ReadPartition(DeviceState.Partitiontarget, State.SavePath);
					goto default;
				case Job.Erase:
					ErasePartition(DeviceState.Partitiontarget);
					return true;
				case (Job)12:
					WritePartition(DeviceState.Partitiontarget[0], State.FilePath);
					goto default;
				case Job.Reset:
					ResetLock(PartitionSet.SPDReset);
					goto default;
				default:
					return Spd.Shutdown();
				case Job.Frp:
					EraseFRP(PartitionSet.SPDfrp);
					goto default;
				case (Job)2:
					ShowPartition();
					return true;
				}
			}
			SerialSPD.Close();
			UI.RichLogs(msg, UI.error, nextline: true, bold: true);
			return false;
		}
		catch (Exception ex)
		{
			SerialSPD.Close();
			UI.RichLogs(msg, UI.error, (byte)num != 0, (byte)num != 0);
			Console.WriteLine(ex.Message);
			return (byte)result != 0;
		}
	}

	private bool Initializing()
	{
		string msg = "Waiting SPD USB Connection... ";
		string category = "SPD";
		string msg2 = "OK";
		string msg3 = "Name : ";
		string text = "Connecting to device ";
		string text2 = "... ";
		string msg4 = "Failed!";
		string text3 = ":";
		UI.RichLogs(msg, UI.log);
		if (UsbScan.FindUSB(category))
		{
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg3, UI.log);
			UI.RichLogs(Portinfo.Name, UI.info, nextline: true, bold: true);
			UI.RichLogs(text + Portinfo.COM + text2, UI.log);
			if (SerialSPD.Open(Portinfo.COM))
			{
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
				if (Spd.InitDevices())
				{
					DeviceState.SetInit(Portinfo.VID + text3 + Portinfo.PID + text3 + Portinfo.COM + text3 + Portinfo.Name);
					return ReadGPT();
				}
				return false;
			}
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
			return false;
		}
		return false;
	}

	private void ShowPartition()
	{
		int num = 0;
		List<Partf> partEntries = partition.Item2.PartEntries;
		foreach (Partf part in partEntries)
		{
			Main.Instance.SPDDataGrid.BeginInvoke((Action)delegate
			{
				string text = ".img";
				string format = "0x{0:X}";
				Main.Instance.SPDDataGrid.Rows.Add(false, part.Name, part.Name + text, utils.FileSize((long)part.Sectors * (long)Spd.Storage.BlockSize), string.Format(format, part.Sector * (uint)Spd.Storage.BlockSize), string.Format(format, part.Sectors * (uint)Spd.Storage.BlockSize));
			});
			UI.Progresbar1(num, partEntries.Count);
			num++;
		}
	}

	private bool ReadGPT()
	{
		string msg = "Reading partition info... ";
		object obj = null;
		string msg2 = "Failed!";
		string msg3 = "OK";
		UI.RichLogs(msg, UI.log);
		partition = Spd.GetGPT();
		var (array, gpt) = partition;
		if (array != null || gpt != obj)
		{
			UI.RichLogs(msg3, UI.ok, nextline: true);
			return true;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		return false;
	}

	private void ReadPartition(List<string> partitionName, string savePath, bool display = true)
	{
		string msg = "Reading ";
		string text = "... ";
		string text2 = "Failed!";
		string text3 = "OK";
		foreach (string item2 in partitionName)
		{
			(bool, string, long, long) tuple = DetectPartition(item2);
			var (flag, partName, _, _) = tuple;
			_ = tuple.Item3;
			long item = tuple.Item4;
			if (flag)
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item2 + text, UI.info, nextline: false, bold: true);
				}
				bool flag2 = Spd.ReadFlash(partName, savePath, item * Spd.Storage.BlockSize);
				if (display)
				{
					UI.RichLogs((!flag2) ? text2 : text3, flag2 ? UI.ok : UI.error, nextline: true);
				}
			}
		}
	}

	private bool WritePartition(string partitionName, object file, bool display = true)
	{
		string msg = "Writing ";
		string text = "... ";
		string msg2 = "SKIP!";
		string msg3 = "Failed!";
		string msg4 = "OK";
		bool flag;
		string partName;
		long item;
		bool flag2;
		if (!display)
		{
			(bool, string, long, long) tuple = DetectPartition(partitionName);
			(flag, partName, _, _) = tuple;
			_ = tuple.Item3;
			item = tuple.Item4;
			if (!flag)
			{
				if (!display)
				{
					return false;
				}
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			flag2 = Spd.WriteFlash(partName, file, item * Spd.Storage.BlockSize);
			if (!display)
			{
				return true;
			}
			if (flag2)
			{
				UI.RichLogs(msg4, flag2 ? UI.ok : UI.error, nextline: true);
				return true;
			}
			UI.RichLogs(msg3, flag2 ? UI.ok : UI.error, nextline: true);
			return true;
		}
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(partitionName + text, UI.info, nextline: false, bold: true);
		(bool, string, long, long) tuple3 = DetectPartition(partitionName);
		(flag, partName, _, _) = tuple3;
		_ = tuple3.Item3;
		item = tuple3.Item4;
		if (!flag)
		{
			if (!display)
			{
				return false;
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		flag2 = Spd.WriteFlash(partName, file, item * Spd.Storage.BlockSize);
		if (!display)
		{
			return true;
		}
		if (flag2)
		{
			UI.RichLogs(msg4, flag2 ? UI.ok : UI.error, nextline: true);
			return true;
		}
		UI.RichLogs(msg3, flag2 ? UI.ok : UI.error, nextline: true);
		return true;
	}

	private void ErasePartition(List<string> partitionName, bool display = true)
	{
		string msg = "Erasing ";
		string text = "... ";
		string text2 = "Failed!";
		string text3 = "OK";
		foreach (string item in partitionName)
		{
			(bool, string, long, long) tuple = DetectPartition(item);
			var (flag, partName, _, _) = tuple;
			_ = tuple.Item3;
			_ = tuple.Item4;
			if (flag)
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item + text, UI.info, nextline: false, bold: true);
				}
				bool flag2 = Spd.FormatFlash(partName);
				if (display)
				{
					UI.RichLogs((!flag2) ? text2 : text3, flag2 ? UI.ok : UI.error, nextline: true);
				}
			}
		}
	}

	private void ResetLock(List<string> partitionName, bool display = true)
	{
		string msg = "Erasing ";
		string text = "Failed!";
		string text2 = "OK";
		foreach (string item in partitionName)
		{
			(bool, string, long, long) tuple = DetectPartition(item);
			var (flag, partName, _, _) = tuple;
			_ = tuple.Item3;
			_ = tuple.Item4;
			if (flag)
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item.ToUpper(), UI.info, nextline: false, bold: true);
					UI.RichLogs(msg, UI.log);
				}
				bool flag2 = Spd.FormatFlash(partName);
				if (display)
				{
					UI.RichLogs((!flag2) ? text : text2, flag2 ? UI.ok : UI.error, nextline: true);
				}
			}
		}
	}

	private void EraseFRP(List<string> partitionName, bool display = true)
	{
		string msg = "Erasing ";
		string msg2 = "...  ";
		string text = "Failed!";
		string text2 = "OK";
		string path = "Data";
		string path2 = "img";
		string path3 = "frp.img";
		foreach (string item2 in partitionName)
		{
			(bool, string, long, long) tuple = DetectPartition(item2);
			var (flag, partName, _, _) = tuple;
			_ = tuple.Item3;
			long item = tuple.Item4;
			if (flag)
			{
				if (display)
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(item2.ToUpper(), UI.info, nextline: false, bold: true);
					UI.RichLogs(msg2, UI.log);
				}
				bool flag2 = Spd.FormatFlash(partName);
				if (display)
				{
					UI.RichLogs((!flag2) ? text : text2, flag2 ? UI.ok : UI.error, nextline: true);
				}
				byte[] data = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, path, path2, path3));
				Spd.WriteFlash(partName, data, item * Spd.Storage.BlockSize);
			}
		}
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

	private void ShutDown()
	{
		string msg = "Rebooting... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		UI.RichLogs(msg, UI.log);
		bool flag;
		if (flag = Spd.Shutdown())
		{
			UI.RichLogs(msg3, flag ? UI.ok : UI.error, nextline: true);
		}
		else
		{
			UI.RichLogs(msg2, flag ? UI.ok : UI.error, nextline: true);
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
