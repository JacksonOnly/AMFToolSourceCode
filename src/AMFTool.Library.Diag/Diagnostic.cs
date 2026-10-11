using System;
using System.Diagnostics;
using System.Text;

namespace AMFTool.Library.Diag;

public class Diagnostic
{
	private int ipro = 0;

	public Phone phone;

	private uint timeout = 1000u;

	private Imei_Info Imei1 = default(Imei_Info);

	private Imei_Info Imei2 = default(Imei_Info);

	public Diagnostic(Phone phone)
	{
		this.phone = phone;
	}

	public bool ReadWriteCert(DiagInfo diaginfo, string pathcritical, bool read)
	{
		string msg = "Reading Critical data... ";
		string name = "CRITICAL";
		string text = "/";
		string text2 = "_";
		string text3 = "yyyyMMdd_HHmmss";
		string text4 = ".qcn";
		string msg2 = "OK";
		string msg3 = "Saved to : ";
		string msg4 = "Failed!";
		string msg5 = "Writing Critical data... ";
		string directory_name = "oem";
		if (!ConnectDevice(diaginfo))
		{
			return false;
		}
		try
		{
			if (read)
			{
				UI.RichLogs(msg, UI.log);
				string text5 = utils.CheckBackupFolder(name);
				string text6 = text + Imei1.imei + text2 + Imei2.imei + text2 + DateTime.Now.ToString(text3) + text4;
				Console.WriteLine(text5);
				string text7 = text5 + text6;
				if (Readcritical_info(text7))
				{
					UI.RichLogs(msg2, UI.ok, nextline: true);
					UI.RichLogs(msg3, UI.log);
					UI.RichLogs(text7, UI.info, nextline: true);
					Process.Start(new ProcessStartInfo
					{
						FileName = text5,
						UseShellExecute = true
					});
					return true;
				}
				UI.RichLogs(msg4, UI.error, nextline: true);
				return false;
			}
			UI.RichLogs(msg5, UI.log);
			if (!phone.EFSCreateDirectory(directory_name))
			{
				UI.RichLogs(msg4, UI.error, nextline: true);
				DisconnectServer();
				return false;
			}
			if (writecritical_info(pathcritical))
			{
				UI.RichLogs(msg2, UI.ok, nextline: true);
				return true;
			}
			UI.RichLogs(msg4, UI.error, nextline: true);
			return false;
		}
		finally
		{
			try
			{
				phone.OnNvEvent -= PhoneOnNvEvent;
			}
			catch
			{
			}
			try
			{
				DisconnectServer();
			}
			catch
			{
			}
		}
	}

	public bool WriteIMEItodevice(DiagInfo info, string imei, int index)
	{
		int result = 0;
		string s = "000000";
		try
		{
			phone.SetLibraryMode((LibraryModeEnum)0);
			phone.ConnectToServer(info.portnum, timeout);
			phone.SendSPC(Encoding.ASCII.GetBytes(s));
			if (!WriteIMEI_Dual(imei, index))
			{
				DisconnectServer();
				return false;
			}
			phone.EFS_SyncWithWait(2000);
			return true;
		}
		catch
		{
			DisconnectServer();
			return (byte)result != 0;
		}
	}

	private bool WriteIMEI_Dual(string imei, int index)
	{
		int result = 0;
		try
		{
			phone.WriteIMEI_DualSIM(imei, index);
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public bool ReadWriteQCN(DiagInfo dinfo, string qcnfile, bool dual, bool read)
	{
		string msg = "Reading QCN data... ";
		string text = "/";
		string text2 = "_";
		string text3 = "yyyyMMdd_HHmmss";
		string text4 = ".qcn";
		string msg2 = "OK";
		string msg3 = "Saved to : ";
		string msg4 = "Failed!";
		string msg5 = "Write QCN data... ";
		if (!ConnectDeviceQCN(dinfo, dual))
		{
			return false;
		}
		if (read)
		{
			UI.RichLogs(msg, UI.log);
			string text5 = text + Imei1.imei + text2 + Imei2.imei + text2 + DateTime.Now.ToString(text3) + text4;
			Console.WriteLine(qcnfile);
			if (ReadQCNfromdevice(qcnfile + text5))
			{
				UI.RichLogs(msg2, UI.ok, nextline: true);
				UI.RichLogs(msg3, UI.log);
				UI.RichLogs(qcnfile, UI.info, nextline: true);
				Process.Start(new ProcessStartInfo
				{
					FileName = qcnfile,
					UseShellExecute = true
				});
				return true;
			}
			UI.RichLogs(msg4, UI.error, nextline: true);
			DisconnectServer();
			return false;
		}
		UI.RichLogs(msg5, UI.log);
		if (!WriteQCNtodevice(qcnfile))
		{
			UI.RichLogs(msg4, UI.error, nextline: true);
			DisconnectServer();
			return false;
		}
		UI.RichLogs(msg2, UI.ok, nextline: true);
		return true;
	}

	private bool Setlibrarymode()
	{
		int result = 0;
		try
		{
			phone.SetLibraryMode((LibraryModeEnum)0);
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	private bool ConnectPort(DiagInfo info)
	{
		int result = 0;
		try
		{
			phone.ConnectToServer(info.portnum, timeout);
			return phone.IsPhoneConnected();
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public bool ConnectDevice(DiagInfo info)
	{
		string msg = "Initializing Protocol... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "Connecting to device... ";
		string s = "000000";
		ipro = 0;
		UI.RichLogs(msg, UI.log);
		if (!Setlibrarymode())
		{
			UI.RichLogs(msg2, UI.error, nextline: true);
			return false;
		}
		UI.RichLogs(msg3, UI.ok, nextline: true);
		UI.RichLogs(msg4, UI.log);
		if (!ConnectPort(info))
		{
			UI.RichLogs(msg2, UI.error, nextline: true);
			DisconnectServer();
			return false;
		}
		UI.RichLogs(msg3, UI.ok, nextline: true);
		Getinfodevice();
		if (!SendSPC(Encoding.UTF8.GetBytes(s)))
		{
			DisconnectServer();
			return false;
		}
		Getimeifromdevice();
		phone.EnableQcnNvItemCallBacks();
		phone.OnNvEvent += PhoneOnNvEvent;
		return true;
	}

	public bool ConnectDeviceQCN(DiagInfo info, bool dual)
	{
		string msg = "Connecting to device... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string s = "000000";
		string msg4 = "Failed";
		ipro = 0;
		UI.RichLogs(msg, UI.log);
		if (!Setlibrarymode())
		{
			UI.RichLogs(msg2, UI.error, nextline: true);
			return false;
		}
		if (ConnectPort(info))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true);
			Getinfodevice();
			if (SendSPC(Encoding.UTF8.GetBytes(s)))
			{
				Getimeifromdevice();
				if (SetSIMDual(dual))
				{
					phone.EnableQcnNvItemCallBacks();
					phone.OnNvEvent += PhoneOnNvEvent;
					return true;
				}
				UI.RichLogs(msg4, UI.error, nextline: true);
				DisconnectServer();
				return false;
			}
			UI.RichLogs(msg2, UI.error, nextline: true);
			DisconnectServer();
			return false;
		}
		UI.RichLogs(msg2, UI.error, nextline: true);
		DisconnectServer();
		return false;
	}

	private void PhoneOnNvEvent(object obj, string status)
	{
		string value = ":";
		string value2 = "%";
		string newValue = "";
		string[] array = status.Split(',');
		string s = array[2].Substring(array[2].LastIndexOf(value) + 2).Replace(array[2].Substring(array[2].IndexOf(value2)), newValue);
		UI.Progresbar1(int.Parse(s));
		if (!string.IsNullOrEmpty(status))
		{
			if (ipro == 100)
			{
				ipro = 0;
				ipro++;
			}
			else
			{
				ipro++;
			}
		}
	}

	private bool SetSIMDual(bool dual)
	{
		int result = 0;
		try
		{
			if (!dual)
			{
				phone.NV_SetTargetSupportMultiSIM(gTargetSupportMultiSim: true);
				return true;
			}
			phone.NV_SetTargetSupportMultiSIM(gTargetSupportMultiSim: true);
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	private bool Readcritical_info(string path)
	{
		string text = "./oem/critical_info";
		int result = 0;
		string eFSPathFileName = text;
		try
		{
			phone.EFSReadFile(path, eFSPathFileName);
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	private bool writecritical_info(string path)
	{
		string text = "./oem/critical_info";
		int result = 0;
		string eFSPathFileName = text;
		try
		{
			phone.EFSWriteFile(path, eFSPathFileName);
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	private bool ReadQCNfromdevice(string path)
	{
		int result = 0;
		int ResultCode = -1;
		try
		{
			phone.BackupNVFromMobileToQCN(path, ref ResultCode);
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	private bool WriteQCNtodevice(string path)
	{
		int num = 0;
		int num2 = 1;
		int syncTimeout = 10000;
		int nvItemsCount = -1;
		try
		{
			phone.LoadNVsFromQCN(path, ref nvItemsCount, out var resultCode);
			int num3 = 10;
			bool flag = false;
			while ((!flag) ? (num3 > num) : ((byte)num != 0))
			{
				num3 -= num2;
				try
				{
					phone.NV_WriteNVsToMobile(ref resultCode);
					flag = (byte)num2 != 0;
				}
				catch (Exception ex)
				{
					UI.RichLogs(ex.Message, UI.error, (byte)num2 != 0, (byte)num != 0);
				}
			}
			if (flag)
			{
				if (!phone.EFS_SyncWithWait(syncTimeout))
				{
					return (byte)num != 0;
				}
				return (byte)num2 != 0;
			}
		}
		catch
		{
		}
		return (byte)num != 0;
	}

	private void Getinfodevice()
	{
		string msg = "MSM HW Version : ";
		string msg2 = "Model : ";
		string msg3 = "Device SW REV : ";
		phone.DIAG_EXT_BUILD_ID_F(out var string_, out var string_2, out var Model);
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(Model, UI.info, nextline: true);
		UI.RichLogs(msg2, UI.log);
		UI.RichLogs(string_2, UI.info, nextline: true);
		UI.RichLogs(msg3, UI.log);
		UI.RichLogs(string_, UI.info, nextline: true);
	}

	private bool SendSPC(byte[] SPC)
	{
		int nextline = 1;
		string msg = "Failed!, Something Wrong";
		int num = 0;
		try
		{
			phone.SendSPC(SPC);
			return true;
		}
		catch
		{
			UI.RichLogs(msg, UI.error, (byte)nextline != 0, (byte)num != 0);
			return (byte)num != 0;
		}
	}

	private void Getimeifromdevice()
	{
		string msg = "IMEI 1 : ";
		string msg2 = "IMEI 2 : ";
		Imei1 = default(Imei_Info);
		Imei2 = default(Imei_Info);
		UI.RichLogs(msg, UI.log);
		phone.ReadIMEI_DualSIM(out Imei1, 0);
		UI.RichLogs(Imei1.imei, UI.info, nextline: true);
		UI.RichLogs(msg2, UI.log);
		phone.ReadIMEI_DualSIM(out Imei2, 1);
		UI.RichLogs(Imei2.imei, UI.info, nextline: true);
	}

	private void DisconnectServer()
	{
		phone.DisconnectServer();
		phone.DisconnectAllServers();
	}

	public bool RebootNormal(int delay)
	{
		int result = 0;
		try
		{
			phone.ResetPhone(delay);
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
