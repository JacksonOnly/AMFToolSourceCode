using System;
using AMFTool.Library.Android;
using AMFTool.Library.Diag;
using AMFTool.Library.Usb;

namespace AMFTool.ThreadWorker;

public class ThreadDiag
{
	private DiagInfo Diaginfo;

	private AdbClient ADB;

	public ThreadDiag()
	{
		object adbServerHost = null;

		ADB = new AdbClient(-1, (string)adbServerHost);
	}

	public bool Init()
	{
		string msg = "Waiting Qualcomm Diag USB Connection... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "Port Name : ";
		string oldValue = "COM";
		string newValue = "";
		object obj = null;
		Diaginfo = new DiagInfo();
		UI.RichLogs(msg, UI.log);
		if (!UsbScan.FindDiag())
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			bool flag = false;
			return false;
		}
		UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg4, UI.log);
		UI.RichLogs(Portinfo.Name, UI.info, nextline: true, bold: true);
		Diaginfo.portnum = Convert.ToInt32(Portinfo.COM.Replace(oldValue, newValue));
		Diagnostic diagnostic = new Diagnostic(new Phone());
		switch (State.Job)
		{
		default:
			return diagnostic.RebootNormal(1000);
		case (Job)46:
			if (!diagnostic.ReadWriteCert(Diaginfo, (string)obj, read: true))
			{
				bool flag = true;
				return true;
			}
			return diagnostic.RebootNormal(1000);
		case Job.RebuildUserDATA:
			return diagnostic.RebootNormal(1000);
		case (Job)48:
			if (!diagnostic.ReadWriteCert(Diaginfo, State.FilePath, read: false))
			{
				bool flag = false;
				return false;
			}
			return diagnostic.RebootNormal(1000);
		case Job.QCNRead:
			if (!diagnostic.ReadWriteQCN(Diaginfo, State.FilePath, dual: true, read: true))
			{
				bool flag = true;
				return true;
			}
			return diagnostic.RebootNormal(1000);
		case (Job)50:
			if (!diagnostic.ReadWriteQCN(Diaginfo, State.FilePath, dual: true, read: false))
			{
				bool flag = false;
				return false;
			}
			return diagnostic.RebootNormal(1000);
		case Job.ImeiWrite:
		{
			bool flag;
			if (IMEIValidation())
			{
				if (diagnostic.ConnectDevice(Diaginfo))
				{
					if (!WriteIMEIOperation(diagnostic))
					{
						flag = false;
						return false;
					}
					return diagnostic.RebootNormal(1000);
				}
				flag = false;
				return false;
			}
			flag = false;
			return false;
		}
		case (Job)52:
			if (!diagnostic.ReadWriteQCN(Diaginfo, (string)obj, dual: true, read: true))
			{
				bool flag = false;
				return false;
			}
			return diagnostic.RebootNormal(1000);
		}
	}

	private bool IMEIValidation()
	{
		string msg = "IMEI 1 Invalid";
		string msg2 = "IMEI 2 Invalid";
		if (DeviceState.CheckedIMEI1)
		{
			if (DeviceState.IMEI1.Length < 15)
			{
				UI.RichLogs(msg, UI.error, nextline: true, bold: true);
				return false;
			}
			if (DeviceState.CheckedIMEI2 && DeviceState.IMEI2.Length < 15)
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			return true;
		}
		if (DeviceState.CheckedIMEI2 && DeviceState.IMEI2.Length < 15)
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		return true;
	}

	private bool WriteIMEIOperation(Diagnostic diag)
	{
		string msg = "Writing IMEI 1... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string msg4 = "Writing IMEI 2... ";
		if (!DeviceState.CheckedIMEI1)
		{
			if (DeviceState.CheckedIMEI2)
			{
				UI.RichLogs(msg4, UI.log);
				if (!diag.WriteIMEItodevice(Diaginfo, DeviceState.IMEI2, 0))
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
					return false;
				}
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				return true;
			}
			return true;
		}
		UI.RichLogs(msg, UI.log);
		if (diag.WriteIMEItodevice(Diaginfo, DeviceState.IMEI1, 0))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			if (DeviceState.CheckedIMEI2)
			{
				UI.RichLogs(msg4, UI.log);
				if (!diag.WriteIMEItodevice(Diaginfo, DeviceState.IMEI2, 0))
				{
					UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
					return false;
				}
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				return true;
			}
			return true;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		return false;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
