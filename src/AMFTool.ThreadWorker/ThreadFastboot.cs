using System;
using System.Collections.Generic;
using AMFTool.Library.Fastboot;

namespace AMFTool.ThreadWorker;

public class ThreadFastboot
{
	private Fastboot Fastboot;

	public ThreadFastboot()
	{
		Fastboot = new Fastboot();
	}

	public bool Init()
	{
		string item = "frp";
		string item2 = "config";
		string item3 = "userdata";
		string item4 = "modemst1";
		string item5 = "modemst2";
		string item6 = "fsg";
		if (DeviceState.Reboot || Initializing())
		{
			switch (State.Job)
			{
			case Job.SetBootSlotA:
			case (Job)96:
				ChangeBootSlotOperation();
				break;
			case (Job)78:
				FixDMVerityOperation();
				break;
			case (Job)80:
				Rebootbl();
				break;
			case Job.RebootEDL:
			case (Job)82:
			case (Job)84:
				Reboot();
				break;
			case (Job)42:
				ErasePartition(new List<string> { item4, item5, item6 });
				break;
			case Job.Flash:
				FlashFirmware();
				break;
			case (Job)8:
				WritePartition(DeviceState.Partitiontarget[0], State.FilePath);
				break;
			case Job.Erase:
				ErasePartition(DeviceState.Partitiontarget);
				break;
			case Job.Reset:
				ErasePartition(new List<string> { item, item2, item3 });
				break;
			case Job.Frp:
				ErasePartition(new List<string> { item, item2, item3 });
				break;
			case (Job)2:
				ReadGPT();
				break;
			case (Job)54:
			case Job.BLLock:
				BootloaderOperation();
				break;
			}
			Fastboot.Disconnect();
			return true;
		}
		return false;
	}

	private bool Initializing()
	{
		string msg = "Waiting Fastboot Connection... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "";
		string msg5 = "Device Information";
		string msg6 = "Serial Number : ";
		string msg7 = "Product       : ";
		string name = "product";
		string msg8 = "Rollback      : ";
		string name2 = "rollback_ver";
		string msg9 = "unlocked      : ";
		string name3 = "unlocked";
		string msg10 = "Battery vol   : ";
		string name4 = "battery-voltage";
		string msg11 = "Serial no     : ";
		string name5 = "serialno";
		string msg12 = "Recovery Ver  : ";
		string name6 = "version";
		string msg13 = "Current slot  : ";
		string name7 = "current-slot";
		string msg14 = "Cpuid         : ";
		string name8 = "cpuid";
		string msg15 = "Version base  : ";
		string name9 = "version-baseband";
		string msg16 = "Bootloader    : ";
		string name10 = "version-bootloader";
		UI.RichLogs(msg, UI.log);
		if (!Fastboot.IsOpen(360))
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg5, UI.log, nextline: true);
		UI.RichLogs(msg6, UI.log);
		UI.RichLogs(Fastboot.GetSerialNumber(), UI.info, nextline: true, bold: true);
		UI.RichLogs(msg7, UI.log);
		UI.RichLogs(Fastboot.GetVar(name), UI.info, nextline: true, bold: true);
		UI.RichLogs(msg8, UI.log);
		UI.RichLogs(Fastboot.GetVar(name2), UI.info, nextline: true, bold: true);
		if (MTKConfig.Infirmware)
		{
			UI.RichLogs(msg9, UI.log);
			UI.RichLogs(Fastboot.GetVar(name3), UI.info, nextline: true, bold: true);
			UI.RichLogs(msg10, UI.log);
			UI.RichLogs(Fastboot.GetVar(name4), UI.info, nextline: true, bold: true);
			UI.RichLogs(msg11, UI.log);
			UI.RichLogs(Fastboot.GetVar(name5), UI.info, nextline: true, bold: true);
			UI.RichLogs(msg12, UI.log);
			UI.RichLogs(Fastboot.GetVar(name6), UI.info, nextline: true, bold: true);
			UI.RichLogs(msg13, UI.log);
			UI.RichLogs(Fastboot.GetVar(name7), UI.info, nextline: true, bold: true);
			UI.RichLogs(msg14, UI.log);
			UI.RichLogs(Fastboot.GetVar(name8), UI.info, nextline: true, bold: true);
			UI.RichLogs(msg15, UI.log);
			UI.RichLogs(Fastboot.GetVar(name9), UI.info, nextline: true, bold: true);
			UI.RichLogs(msg16, UI.log);
			UI.RichLogs(Fastboot.GetVar(name10), UI.info, nextline: true, bold: true);
			return true;
		}
		return true;
	}

	private void ChangeBootSlotOperation()
	{
		string msg = "Change Boot Slot to A... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "Change Boot Slot to B... ";
		if (State.Job == Job.SetBootSlotA)
		{
			UI.RichLogs(msg, UI.log);
			if (Fastboot.ChangeSlot(toA: true))
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			}
		}
		else if (State.Job == (Job)96)
		{
			UI.RichLogs(msg4, UI.log);
			if (Fastboot.ChangeSlot(toA: false))
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			}
		}
	}

	private void FixDMVerityOperation()
	{
		string msg = "DM Verity Fix... ";
		string command = "oem cdms fix";
		string msg2 = "Failed!";
		string msg3 = "OK";
		UI.RichLogs(msg, UI.log);
		if (Fastboot.SendCommand(command))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		}
	}

	private void Reboot()
	{
		string text = "reboot";
		string text2 = "Rebooting... ";
		string text3 = "reboot-bootloader";
		string text4 = "Reboot to Bootloader... ";
		string text5 = "reboot-recovery";
		string text6 = "Reboot to Recovery... ";
		string text7 = "reboot-edl";
		string text8 = "Reboot to EDL... ";
		string msg = "Failed!";
		string msg2 = "OK";
		switch (State.Job)
		{
		case (Job)80:
		{
			string command = text;
			string msg3 = text2;
			UI.RichLogs(msg3, UI.log);
			if (Fastboot.SendCommand(command))
			{
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg, UI.error, nextline: true, bold: true);
			}
			break;
		}
		case Job.RebootEDL:
		{
			string command = text7;
			string msg3 = text8;
			UI.RichLogs(msg3, UI.log);
			if (Fastboot.SendCommand(command))
			{
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg, UI.error, nextline: true, bold: true);
			}
			break;
		}
		case (Job)82:
		{
			string command = text5;
			string msg3 = text6;
			UI.RichLogs(msg3, UI.log);
			if (Fastboot.SendCommand(command))
			{
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg, UI.error, nextline: true, bold: true);
			}
			break;
		}
		case Job.RebootDownload:
			break;
		case (Job)84:
		{
			string command = text3;
			string msg3 = text4;
			UI.RichLogs(msg3, UI.log);
			if (Fastboot.SendCommand(command))
			{
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg, UI.error, nextline: true, bold: true);
			}
			break;
		}
		}
	}

	private void Rebootbl()
	{
		string text = "reboot";
		DeviceState.Reboot = false;
		Job job = State.Job;
		Job job2 = job;
		if (job2 == (Job)80)
		{
			string command = text;
			Fastboot.SendCommand(command);
		}
	}

	private void BootloaderOperation()
	{
		string msg = "Locking bootloader... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "Unlocking bootloader... ";
		if (State.Job != Job.BLLock)
		{
			if (State.Job == (Job)54)
			{
				UI.RichLogs(msg4, UI.log);
				if (Fastboot.SetBootloaderUnlock(unlock: true))
				{
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				}
				else
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				}
			}
		}
		else
		{
			UI.RichLogs(msg, UI.log);
			if (!Fastboot.SetBootloaderUnlock(unlock: false))
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			}
		}
	}

	private void FlashFirmware()
	{
		string text = "flash";
		string msg = "Writing ";
		int num = 0;
		string text2 = "[";
		string text3 = "]";
		int num2 = 1;
		string text4 = "->";
		string msg2 = "... ";
		string msg3 = "OK";
		string msg4 = "Failed!";
		string text5 = "erase";
		string msg5 = "Erasing ";
		string text6 = "";
		string msg6 = "Rebooting... ";
		string command = "reboot";
		try
		{
			foreach (FastbootList partition in FastbootList.Partitions)
			{
				if (!(partition.Command == text))
				{
					if (partition.Command == text5)
					{
						UI.RichLogs(msg5, UI.log);
						UI.RichLogs(partition.Partition ?? text6, UI.info, nextline: false, bold: true);
						UI.RichLogs(msg2, UI.log);
						if (!Fastboot.ErasePartition(partition.Partition))
						{
							UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
						}
						else
						{
							UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						}
					}
				}
				else
				{
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(text2 + partition.Partition + text3, UI.info, nextline: false, bold: true);
					UI.RichLogs(text4 + partition.FileName, UI.log);
					UI.RichLogs(msg2, UI.log);
					if (Fastboot.WritePartition(partition.Partition, partition.Location))
					{
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					}
					else
					{
						UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
					}
				}
			}
			UI.RichLogs(msg6, UI.log, (byte)num != 0, (byte)num != 0);
			if ((Fastboot.SendCommand(command) ? 1 : 0) == num)
			{
				UI.RichLogs(msg4, UI.error, (byte)num2 != 0, (byte)num2 != 0);
			}
			else
			{
				UI.RichLogs(msg3, UI.ok, (byte)num2 != 0, (byte)num2 != 0);
			}
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message ?? text6, UI.error, (byte)num2 != 0, (byte)num2 != 0);
		}
	}

	private void ReadGPT()
	{
		List<Fastboot.PartitionEntry> list = Fastboot.ReadGpt();
		foreach (Fastboot.PartitionEntry part in list)
		{
			Main.Instance.FastbootDataGrid.BeginInvoke((Action)delegate
			{
				string text = ".img";
				string text2 = "-";
				Main.Instance.FastbootDataGrid.Rows.Add(false, part.Partition, part.Partition + text, utils.FileSize(part.Size), text2);
			});
		}
	}

	private void ErasePartition(List<string> partitions)
	{
		string msg = "Erasing ";
		string msg2 = "... ";
		string msg3 = "Failed!";
		string msg4 = "OK";
		foreach (string partition in partitions)
		{
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(partition, UI.info, nextline: false, bold: true);
			UI.RichLogs(msg2, UI.log);
			if (Fastboot.ErasePartition(partition))
			{
				UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				continue;
			}
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			break;
		}
	}

	private void WritePartition(string partitioname, string filepath)
	{
		string msg = "Writing ";
		string msg2 = "... ";
		string msg3 = "Failed!";
		string msg4 = "OK";
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(partitioname, UI.info, nextline: false, bold: true);
		UI.RichLogs(msg2, UI.log);
		if (Fastboot.WritePartition(partitioname, filepath))
		{
			UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
