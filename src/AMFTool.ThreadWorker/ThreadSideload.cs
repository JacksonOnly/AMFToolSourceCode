using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using AMFTool.Library.Android;

namespace AMFTool.ThreadWorker;

public class ThreadSideload
{
	private AdbClient ADB;

	public ThreadSideload()
	{
		object adbServerHost = null;

		ADB = new AdbClient(-1, (string)adbServerHost);
	}

	public bool Init()
	{
		string msg = "";
		string string_ = "reboot";
		object string_2 = null;
		if (Initializing())
		{
			switch (State.Job)
			{
			default:
				UI.RichLogs(msg, UI.log);
				goto IL_00cd;
			case Job.Reset:
				MiassistResetlock();
				UI.RichLogs(msg, UI.log);
				ADB.method_24(string_, 20000, (string)string_2);
				return true;
			case (Job)14:
			case (Job)16:
				UI.RichLogs(msg, UI.log);
				goto IL_00cd;
			case Job.Frp:
				MiassistFRP();
				UI.RichLogs(msg, UI.log);
				ADB.method_24(string_, 20000, (string)string_2);
				return true;
			case Job.Micloud:
				MiassistMIdisable();
				UI.RichLogs(msg, UI.log);
				goto IL_00cd;
			case (Job)58:
				{
					MiassistMIreset();
					UI.RichLogs(msg, UI.log);
					ADB.method_24(string_, 20000, (string)string_2);
					return true;
				}
				IL_00cd:
				ADB.method_24(string_, 20000, (string)string_2);
				return true;
			}
		}
		return false;
	}

	private bool Initializing()
	{
		string msg = "Initializing protocol.... ";
		int result = 0;
		string msg2 = "Failed!";
		int num = 1;
		string msg3 = "adb.exe not found!";
		string msg4 = "OK";
		string msg5 = "Waiting ADB Connection... ";
		object obj = null;
		string msg6 = "Stopped";
		double field_ = Class38.field_2144;
		string msg7 = "No device found!";
		string text = "";
		string msg8 = "Product       : ";
		string msg9 = "Model         : ";
		string msg10 = "Device        : ";
		string msg11 = "Connecting to : ";
		string msg12 = "Reading System info  : ";
		string @string = "getinfo";
		string text2 = "d:";
		string string_ = "b:";
		string string_2 = "\r\n";
		string string_3 = "c:";
		string string_4 = "daemon_version:";
		string string_5 = "l:";
		string string_6 = "m:";
		string string_7 = "r:";
		string string_8 = "sn:";
		string string_9 = "v:";
		string msg13 = "MIUI version  : ";
		string msg14 = "OS version    : ";
		string text3 = "F";
		string msg15 = "Stable";
		string text4 = "X";
		string msg16 = "Developer (Weekly)";
		string text5 = "S";
		string msg17 = "Monthly build";
		string text6 = "D";
		string msg18 = "Daily build";
		string text7 = "E";
		string msg19 = "Emegency build";
		string text8 = "A";
		string msg20 = "Experimental build";
		string text9 = "I";
		string msg21 = "Internal build";
		string text10 = "T";
		string msg22 = "Top secret build";
		string text11 = "U";
		string msg23 = "Unstable build";
		string msg24 = "Language       : ";
		string msg25 = "Region         : ";
		string msg26 = "Recovery versi : ";
		string msg27 = "SN             : ";
		string msg28 = "Skip eRRoR";
		string value = "not found";
		string msg29 = "Device Unplugged";
		string msg30 = "Please check USB Connection";
		string msg31 = "Please allow usb debugging";
		UI.RichLogs(msg, UI.log);
		if (ADB.Check() || ADB.StartServer())
		{
			UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg5, UI.log);
			AdbDevice[] array = (AdbDevice[])obj;
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (stopwatch.Elapsed < TimeSpan.FromMinutes(field_))
			{
				array = ADB.GetDevices();
				if (array.Length != 0)
				{
					break;
				}
				for (int i = 0; i < 10; i++)
				{
					if (!JobStatus.Token.IsCancellationRequested)
					{
						Thread.Sleep(100);
						continue;
					}
					UI.RichLogs(msg6, UI.error, nextline: true, bold: true);
					return false;
				}
			}
			if (array != null && array.Length != 0)
			{
				AdbDevice[] array2 = array;
				foreach (AdbDevice adbDevice in array2)
				{
					UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
					UI.RichLogs(text, UI.ok, nextline: true, bold: true);
					ADB.SetDevice(adbDevice.SerialNumber);
					UI.RichLogs(msg8, UI.log);
					UI.RichLogs(adbDevice.Product.ToUpper() ?? text, UI.info, nextline: true, bold: true);
					UI.RichLogs(msg9, UI.log);
					UI.RichLogs(adbDevice.Model.ToUpper() ?? text, UI.info, nextline: true, bold: true);
					UI.RichLogs(msg10, UI.log);
					UI.RichLogs(adbDevice.Device.ToUpper() ?? text, UI.info, nextline: true, bold: true);
					UI.RichLogs(msg11, UI.log);
					UI.RichLogs(adbDevice.SerialNumber.ToUpper() ?? text, UI.info, nextline: true, bold: true);
					UI.RichLogs(text, UI.log);
				}
				try
				{
					UI.RichLogs(msg12, UI.log);
					string text12 = ADB.Method23(@string);
					if (text12.Contains(text2))
					{
						UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
						string text13 = smethod_0(text12, string_, string_2);
						string text14 = smethod_0(text12, string_3, string_2);
						string text15 = smethod_0(text12, text2, string_2);
						string text16 = smethod_0(text12, string_4, string_2);
						string text17 = smethod_0(text12, string_5, string_2);
						string text18 = smethod_0(text12, string_6, string_2);
						string text19 = smethod_0(text12, string_7, string_2);
						string text20 = smethod_0(text12, string_8, string_2);
						string text21 = smethod_0(text12, string_9, string_2);
						UI.RichLogs(msg9, UI.log);
						UI.RichLogs(text15.ToUpper() ?? text, UI.info, nextline: true, bold: true);
						UI.RichLogs(msg8, UI.log);
						UI.RichLogs(text18.ToUpper() ?? text, UI.info, nextline: true, bold: true);
						UI.RichLogs(msg13, UI.log);
						UI.RichLogs(text21.ToUpper() ?? text, UI.info, nextline: true, bold: true);
						UI.RichLogs(msg14, UI.log);
						UI.RichLogs(text14.ToUpper() ?? text, UI.info, nextline: true, bold: true);
						if (text13 == text3)
						{
							UI.RichLogs(msg15, UI.log);
						}
						else if (!(text13 == text4))
						{
							if (text13 == text5)
							{
								UI.RichLogs(msg17, UI.log);
							}
							else if (!(text13 == text6))
							{
								if (!(text13 == text7))
								{
									if (text13 == text8)
									{
										UI.RichLogs(msg20, UI.log);
									}
									else if (text13 == text9)
									{
										UI.RichLogs(msg21, UI.log);
									}
									else if (text13 == text10)
									{
										UI.RichLogs(msg22, UI.log);
									}
									else if (!(text13 == text11))
									{
										UI.RichLogs(text13, UI.log);
									}
									else
									{
										UI.RichLogs(msg23, UI.log);
									}
								}
								else
								{
									UI.RichLogs(msg19, UI.log);
								}
							}
							else
							{
								UI.RichLogs(msg18, UI.log);
							}
						}
						else
						{
							UI.RichLogs(msg16, UI.log);
						}
						UI.RichLogs(msg24, UI.log);
						UI.RichLogs(text17.ToUpper() ?? text, UI.info, nextline: true, bold: true);
						UI.RichLogs(msg25, UI.log);
						UI.RichLogs(text19.ToUpper() ?? text, UI.info, nextline: true, bold: true);
						UI.RichLogs(msg26, UI.log);
						UI.RichLogs(text16.ToUpper() ?? text, UI.info, nextline: true, bold: true);
						UI.RichLogs(msg27, UI.log);
						UI.RichLogs(text20.ToUpper() ?? text, UI.info, nextline: true, bold: true);
					}
					else
					{
						UI.RichLogs(msg28, UI.error, nextline: true, bold: true);
					}
				}
				catch (AdbException ex)
				{
					UI.RichLogs(msg2, UI.error, (byte)num != 0, (byte)num != 0);
					if (!ex.Message.ToLower().Contains(value))
					{
						UI.RichLogs(msg31, UI.error, (byte)num != 0, (byte)num != 0);
					}
					else
					{
						UI.RichLogs(msg29, UI.error, (byte)num != 0, (byte)num != 0);
						UI.RichLogs(msg30, UI.error, (byte)num != 0, (byte)num != 0);
					}
					return (byte)result != 0;
				}
				return (byte)num != 0;
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			UI.RichLogs(msg7, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
		return false;
	}

	private void MiassistFRP()
	{
		string msg = "Checking Partition... ";
		string string_ = "ls /dev/block/by-name";
		object string_2 = null;
		string value = " frp";
		string msg2 = "OK";
		string text = "NV31YRL6PK";
		string text2 = "pull /proc/partitions \"";
		string text3 = "\"";
		string text4 = "\\partitions";
		string msg3 = "CAN NOT COMPARE";
		string msg4 = "NOT FOUND";
		UI.RichLogs(msg, UI.log);
		string text5 = ADB.method_24(string_, 20000, (string)string_2);
		if (!text5.Contains(value))
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
			return;
		}
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		string text6 = Path.GetTempPath() + text;
		if (Directory.Exists(text6))
		{
			ADB.method_24(text2 + text6 + text3, 20000, (string)string_2);
			if (!File.Exists(text6 + text4))
			{
				UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
				Directory.Delete(text6, recursive: true);
			}
			else
			{
				string[] string_3 = File.ReadAllLines(text6 + text4);
				method_3_miassist(text6, string_3);
				Directory.Delete(text6, recursive: true);
			}
			return;
		}
		Directory.CreateDirectory(text6);
		ADB.method_24(text2 + text6 + text3, 20000, (string)string_2);
		if (!File.Exists(text6 + text4))
		{
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			Directory.Delete(text6, recursive: true);
		}
		else
		{
			string[] string_3 = File.ReadAllLines(text6 + text4);
			method_3_miassist(text6, string_3);
			Directory.Delete(text6, recursive: true);
		}
	}

	private void MiassistMIreset()
	{
		string msg = "Checking Partition... ";
		string string_ = "ls /dev/block/by-name";
		object string_2 = null;
		string value = " persist";
		string msg2 = "OK";
		string text = "TC824GWLRH";
		string text2 = "pull /proc/partitions \"";
		string text3 = "\"";
		string text4 = "\\partitions";
		string msg3 = "CAN NOT COMPARE";
		string msg4 = "NOT FOUND";
		if (!method_2_miassist())
		{
			return;
		}
		UI.RichLogs(msg, UI.log);
		string text5 = ADB.method_24(string_, 20000, (string)string_2);
		if (text5.Contains(value))
		{
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			string text6 = Path.GetTempPath() + text;
			if (!Directory.Exists(text6))
			{
				Directory.CreateDirectory(text6);
			}
			ADB.method_24(text2 + text6 + text3, 20000, (string)string_2);
			if (File.Exists(text6 + text4))
			{
				string[] string_3 = File.ReadAllLines(text6 + text4);
				method_4_miassist(text6, text5, string_3);
			}
			else
			{
				UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			}
			Directory.Delete(text6, recursive: true);
		}
		else
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
	}

	private void MiassistResetlock()
	{
		string msg = "Factory Resetting... ";
		string @string = "format-data";
		string value = "success";
		string msg2 = "OK";
		string msg3 = "FAIL";
		UI.RichLogs(msg, UI.log);
		string text = ADB.Method23(@string);
		if (text.Contains(value))
		{
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		}
		else
		{
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
		}
	}

	private void Reboot()
	{
		string command = "reboot";
		ADB.ExecuteRemoteCommand(command);
	}

	public static string smethod_0(string string_0, string string_1, string string_2)
	{
		string result = "";
		if (string_0.Contains(string_1))
		{
			if (string_0.Contains(string_2))
			{
				int num = string_0.IndexOf(string_1) + string_1.Length;
				int num2 = string_0.IndexOf(string_2, num);
				if (num2 != -1)
				{
					if (string.IsNullOrEmpty(string_2))
					{
						return string_0.Substring(num);
					}
					return string_0.Substring(num, num2 - num);
				}
				return result;
			}
			return string.Empty;
		}
		return string.Empty;
	}

	private void MiassistMIdisable()
	{
		string msg = "Checking Partition... ";
		string string_ = "ls /dev/block/by-name";
		object string_2 = null;
		string value = " modem_a";
		string value2 = " modem";
		string value3 = " modem_b";
		string msg2 = "OK";
		string text = "EU379OZ78F";
		string text2 = "pull /proc/partitions \"";
		string text3 = "\"";
		string text4 = "\\partitions";
		string msg3 = "CAN NOT COMPARE";
		if (!method_2_miassist())
		{
			return;
		}
		UI.RichLogs(msg, UI.log);
		string text5 = ADB.method_24(string_, 20000, (string)string_2);
		if (!text5.Contains(value) && !text5.Contains(value2) && !text5.Contains(value3))
		{
			return;
		}
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		string text6 = Path.GetTempPath() + text;
		if (Directory.Exists(text6))
		{
			ADB.method_24(text2 + text6 + text3, 20000, (string)string_2);
			if (!File.Exists(text6 + text4))
			{
				UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
				Directory.Delete(text6, recursive: true);
			}
			else
			{
				string[] string_3 = File.ReadAllLines(text6 + text4);
				method_7_miassist(text6, text5, string_3);
				Directory.Delete(text6, recursive: true);
			}
			return;
		}
		Directory.CreateDirectory(text6);
		ADB.method_24(text2 + text6 + text3, 20000, (string)string_2);
		if (!File.Exists(text6 + text4))
		{
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			Directory.Delete(text6, recursive: true);
		}
		else
		{
			string[] string_3 = File.ReadAllLines(text6 + text4);
			method_7_miassist(text6, text5, string_3);
			Directory.Delete(text6, recursive: true);
		}
	}

	public void method_7_miassist(string string_0, string string_1, string[] string_2)
	{
		string path = "2PCFYPL9DS";
		string path2 = "ASX7U6MU1T";
		string msg = "Removing bypass Micloud lock...";
		string item = "modem";
		string item2 = "modem_a";
		string item3 = "modem_b";
		string text = " ";
		string text2 = "pull /dev/block/by-name/";
		int num = 1;
		string text3 = " \"";
		string text4 = "\"";
		object obj = null;
		string value = "1 file pulled";
		string text5 = "push \"";
		string text6 = "\" /dev/block/";
		string value2 = "1 file pushed";
		string msg2 = "✓";
		string msg3 = "✗";
		string msg4 = "OK";
		string msg5 = "FAIL";
		string text7 = Path.Combine(string_0, path);
		string text8 = Path.Combine(string_0, path2);
		UI.RichLogs(msg, UI.log);
		bool flag = false;
		List<string> list = new List<string> { item, item2, item3 };
		List<string> list2 = new List<string>();
		foreach (string item4 in list)
		{
			if (!string_1.Contains(text + item4))
			{
				continue;
			}
			string text9 = ADB.method_24(text2 + item4 + text3 + text7 + text4, 20000, (string)obj);
			if (text9.Contains(value))
			{
				byte[] byte_ = new byte[16]
				{
					230, 0, 97, 0, 114, 0, 100, 0, 97, 0,
					15, 0, 235, 112, 0, 112
				};
				byte[] byte_2 = new byte[16]
				{
					99, 0, 97, 0, 114, 0, 100, 0, 97, 0,
					15, 0, 235, 112, 0, 112
				};
				byte[] byte_3 = File.ReadAllBytes(text7);
				byte[] array = smethod_7(byte_3, byte_, byte_2);
				if (array == obj)
				{
					continue;
				}
				File.WriteAllBytes(text8, array);
				string text10 = method_5_miassist(string_0, text7, string_2, list2);
				if (text10 !=null)
				{
					list2.Add(text10);
					string text11 = ADB.method_24(text5 + text8 + text6 + text10, 20000, (string)obj);
					if (text11.Contains(value2))
					{
						UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
						flag = true;
					}
					else
					{
						UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
					}
				}
			}
			else
			{
				UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			}
		}
		if (!flag)
		{
			UI.RichLogs(msg5, UI.error, (byte)num != 0, (byte)num != 0);
		}
		else
		{
			UI.RichLogs(msg4, UI.ok, (byte)num != 0, (byte)num != 0);
		}
	}

	public void method_3_miassist(string string_0, string[] string_1)
	{
		string path = "SS1FI0XKSL";
		string path2 = "4QAFAEV00UOW";
		string msg = "Resetting FRP.... ";
		string text = "pull /dev/block/by-name/frp \"";
		string text2 = "\"";
		object obj = null;
		string value = "1 file pulled";
		string text3 = "push \"";
		string text4 = "\" /dev/block/";
		string value2 = "1 file pushed";
		string msg2 = "OK";
		string msg3 = "FAIL";
		string msg4 = "NO ANY MATCH";
		string text5 = Path.Combine(string_0, path);
		string text6 = Path.Combine(string_0, path2);
		UI.RichLogs(msg, UI.log);
		string text7 = ADB.method_24(text + text5 + text2, 20000, (string)obj);
		if (!text7.Contains(value))
		{
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			return;
		}
		smethod_11(text6);
		string text8 = method_5_miassist(string_0, text5, string_1, (List<string>)obj);
		if (text8 !=null)
		{
			string text9 = ADB.method_24(text3 + text6 + text4 + text8, 20000, (string)obj);
			if (text9.Contains(value2))
			{
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			}
			else
			{
				UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			}
		}
		else
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		}
	}

	public static void smethod_11(string string_0)
	{
		byte[] array = new byte[524288];
		int i = 0;
		for (int num = array.Length - 1; i <= num; i++)
		{
			array[i] = 0;
		}
		FileStream fileStream = File.Create(string_0);
		using (fileStream)
		{
			fileStream.Write(array, 0, array.Length);
		}
	}

	public bool method_4_miassist(string string_0, string string_1, string[] string_2)
	{
		string path = "Y4Z96YKER3";
		string path2 = "71Q0J9AOMVGV";
		string msg = "esetting Micloud.... ";
		string text = "pull /dev/block/by-name/persist \"";
		string text2 = "\"";
		object obj = null;
		string value = "1 file pulled";
		string text3 = "push \"";
		string text4 = "\" /dev/block/";
		string value2 = "1 file pushed";
		string msg2 = "OK";
		string msg3 = "FAIL";
		string msg4 = "NO ANY MATCH";
		string msg5 = "LOCK DATA NOT FOUND";
		string text5 = Path.Combine(string_0, path);
		string text6 = Path.Combine(string_0, path2);
		UI.RichLogs(msg, UI.log);
		string text7 = ADB.method_24(text + text5 + text2, 20000, (string)obj);
		if (!text7.Contains(value))
		{
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			return false;
		}
		byte[] byte_ = new byte[6] { 4, 2, 102, 100, 115, 100 };
		byte[] byte_2 = new byte[6] { 4, 2, 84, 85, 78, 71 };
		byte[] byte_3 = File.ReadAllBytes(text5);
		byte[] array = smethod_7(byte_3, byte_, byte_2);
		if (array ==null)
		{
			UI.RichLogs(msg5, UI.error, nextline: true, bold: true);
			return false;
		}
		File.WriteAllBytes(text6, array);
		string text8 = method_5_miassist(string_0, text5, string_2, (List<string>)obj);
		if (text8 !=null)
		{
			string text9 = ADB.method_24(text3 + text6 + text4 + text8, 20000, (string)obj);
			if (!text9.Contains(value2))
			{
				UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			return true;
		}
		UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
		return false;
	}

	public static byte[] smethod_7(byte[] byte_0, byte[] byte_1, byte[] byte_2)
	{
		object obj = null;
		byte[] result = (byte[])obj;
		int num = smethod_8(byte_0, byte_1);
		if (num >= 0)
		{
			result = new byte[byte_0.Length - byte_1.Length + byte_2.Length];
			Buffer.BlockCopy(byte_0, 0, result, 0, num);
			Buffer.BlockCopy(byte_2, 0, result, num, byte_2.Length);
			Buffer.BlockCopy(byte_0, num + byte_1.Length, result, num + byte_2.Length, byte_0.Length - (num + byte_1.Length));
			return result;
		}
		return result;
	}

	public static int smethod_8(byte[] byte_0, byte[] byte_1)
	{
		int result = -1;
		int num = 0;
		int i = 0;
		for (int num2 = byte_0.Length - 1; i <= num2; i++)
		{
			if (byte_0[i] != byte_1[num])
			{
				num = ((byte_0[i] == byte_1[0]) ? 1 : 0);
				continue;
			}
			if (num != byte_1.Length - 1)
			{
				num++;
				continue;
			}
			result = i - num;
			break;
		}
		return result;
	}

	public string method_5_miassist(string string_0, string string_1, string[] string_2, List<string> list_0 = null)
	{
		object obj = null;
		string path = "ZAUUYGWQKJ3";
		string pattern = "\\s+";
		string text = "";
		string text2 = "pull /dev/block/";
		string text3 = " \"";
		string text4 = "\"";
		if (list_0 == obj)
		{
			list_0 = new List<string>();
		}
		string text5 = Path.Combine(string_0, path);
		long length = new FileInfo(string_1).Length;
		long num = length / 1024L;
		int num2 = 0;
		string text8;
		while (true)
		{
			if (num2 >= string_2.Length)
			{
				return (string)obj;
			}
			string text6 = string_2[num2];
			string[] array = Regex.Split(text6.Trim(), pattern);
			if (array.Length >= 4)
			{
				string text7 = array[2];
				text8 = array[3];
				if (!((num.ToString() ?? text) != (text7 ?? text)) && !list_0.Contains(text8))
				{
					ADB.method_24(text2 + text8 + text3 + text5 + text4, 20000, (string)obj);
					if (smethod_9(text5, string_1))
					{
						break;
					}
				}
			}
			num2++;
		}
		return text8;
	}

	public static bool smethod_9(string string_0, string string_1)
	{
		using FileStream stream_ = new FileStream(string_0, FileMode.Open);
		using FileStream stream_2 = new FileStream(string_1, FileMode.Open);
		return smethod_10(stream_, stream_2);
	}

	private static bool smethod_10(Stream stream_0, Stream stream_1)
	{
		byte[] array = new byte[2048];
		byte[] array2 = new byte[2048];
		int num = stream_0.Read(array, 0, 2048);
		int num2 = 1435841567;
		while (true)
		{
			_ = num2 ^ 0x4A49A385;
			int num3 = stream_1.Read(array2, 0, 2048);
			if (num != num3)
			{
				return false;
			}
			if (num != 0)
			{
				if (!array.Take(num).SequenceEqual(array2.Take(num3)))
				{
					break;
				}
				num = stream_0.Read(array, 0, 2048);
				num2 = 1435841567;
				continue;
			}
			return true;
		}
		return false;
	}

	public bool method_2_miassist()
	{
		string msg = "Checking Device... ";
		string string_ = "ls /";
		object string_2 = null;
		string value = " dev";
		string value2 = " data";
		string msg2 = "OK";
		string msg3 = "NOT SUPPORTED";
		UI.RichLogs(msg, UI.log);
		string text = ADB.method_24(string_, 20000, (string)string_2);
		if (!text.Contains(value) && !text.Contains(value2))
		{
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		return true;
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
