using System.Diagnostics;
using System.IO;
using AMFTool.Library;

namespace AMFTool.ThreadWorker;

public class ThreadExtra
{
	public bool Init()
	{
		string msg = "File Selected not found!";
		object obj = null;
		if (File.Exists(State.FilePath))
		{
			byte[] nv = File.ReadAllBytes(State.FilePath);
			return NVEditor(nv, (byte[])obj, (byte[])obj);
		}
		UI.RichLogs(msg, UI.error, nextline: true, bold: true);
		return false;
	}

	private bool NVEditor(byte[] nv, byte[] imei1 = null, byte[] imei2 = null)
	{
		string msg = "Reading NV File... ";
		string text = "corrupt";
		string msg2 = "NV Corrupt | Abnormal";
		string msg3 = "OK";
		string msg4 = "Current IMEI : ";
		string msg5 = "IMEI 1 : ";
		string msg6 = "IMEI 2 : ";
		string msg7 = "NEW IMEI : ";
		string msg8 = "Writing IMEI to NV... ";
		string msg9 = "Failed!";
		string msg10 = "Saving file... ";
		string msg11 = "File Saved : ";
		UI.RichLogs(msg, UI.log);
		string[] array = Tools.IMEIDecrypt(nv);
		if (array[0] == text)
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg4, UI.log, nextline: true);
		UI.RichLogs(msg5, UI.log);
		UI.RichLogs(array[0], UI.info, nextline: true, bold: true);
		UI.RichLogs(msg6, UI.log);
		UI.RichLogs(array[1], UI.info, nextline: true, bold: true);
		UI.RichLogs(msg7, UI.log, nextline: true);
		if (DeviceState.CheckedIMEI1)
		{
			UI.RichLogs(msg5, UI.log);
			UI.RichLogs(DeviceState.IMEI1, UI.info, nextline: true, bold: true);
			imei1 = Tools.IMEIEncrypt(DeviceState.IMEI1);
		}
		if (DeviceState.CheckedIMEI2)
		{
			UI.RichLogs(msg6, UI.log);
			UI.RichLogs(DeviceState.IMEI2, UI.info, nextline: true, bold: true);
			imei2 = Tools.IMEIEncrypt(DeviceState.IMEI2);
		}
		UI.RichLogs(msg8, UI.log);
		if (Tools.PatchIMEI(nv, imei1, imei2))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg10, UI.log);
			File.WriteAllBytes(State.SavePath, nv);
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg11, UI.log);
			UI.RichLogs(State.SavePath, UI.info, nextline: true, bold: true);
			string directoryName = Path.GetDirectoryName(State.SavePath);
			Process.Start(new ProcessStartInfo
			{
				FileName = directoryName,
				UseShellExecute = true
			});
			return true;
		}
		UI.RichLogs(msg9, UI.error, nextline: true, bold: true);
		return false;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
