using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AMFTool;
using AMFTool.Library.Usb;

namespace Restored.Namespace2;

internal class Class27
{
	public static byte[] field_01;

	public bool method_01()
	{
		int result = 0;
		string msg = "Failed";
		int num = 1;
		string msg2 = "Failed!";
		try
		{
			if (DeviceState.DeviceInit || method_06())
			{
				Job job = State.Job;
				Job job2 = job;
				if ((uint)(job2 - 3) <= 1u)
				{
					method_02();
				}
				return method_03();
			}
			UI.RichLogs(msg, Color.Tomato, nextline: true, bold: true);
			return false;
		}
		catch (Exception ex)
		{
			PortIO.Close();
			UI.RichLogs(msg2, UI.error, (byte)num != 0, (byte)num != 0);
			Console.WriteLine(ex.Message);
			return (byte)result != 0;
		}
	}

	public static bool method_02()
	{
		string msg = "";
		string msg2 = "Factory Reset... ";
		string hex = "7E 0F 00 00 00 21 00 68 00 41 54 2B 53 50 44 49 41 47 3D 22 41 54 2B 45 54 53 52 45 53 45 54 22 0D 0A 7E";
		string text = "read factory Reset : ";
		string value = "~";
		string text2 = "OK";
		string msg3 = "Failed";
		UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg2, UI.log);
		Thread.Sleep(1000);
		byte[] request = utils.StringToByteArray(hex);
		PortIO.Write(request);
		byte[] array = PortIO.Read();
		string text3 = utils.BytesToHexString(array);
		Console.WriteLine(text + text3);
		if (!(!Encoding.UTF8.GetString(array).Contains(value) | !Encoding.UTF8.GetString(array).Contains(text2)))
		{
			UI.RichLogs(text2, UI.ok, nextline: true, bold: true);
			return true;
		}
		UI.RichLogs(msg3, Color.Tomato, nextline: true, bold: true);
		return true;
	}

	public static bool method_03()
	{
		string hex = "7E 00 00 00 00 08 00 0C 08 7E";
		Thread.Sleep(10);
		byte[] request = utils.StringToByteArray(hex);
		PortIO.Write(request);
		PortIO.Close();
		return true;
	}

	public static bool method_04()
	{
		string msg = "Read information...  ";
		string s = "7e 01 00 00 00 08 00 00 00 7e";
		string text = "response:";
		string text2 = "Platform\t\t";
		string text3 = "Platform Version";
		string text4 = "Project\t\t";
		string text5 = "Project Version";
		string text6 = "BASE\t\t";
		string text7 = "BASE Version";
		string text8 = "Hardware\t\t";
		string text9 = "HW Version";
		string text10 = ": ";
		string endDelimiter = "?";
		string text11 = " ";
		string text12 = " : ";
		string s2 = "7e 03 00 00 00 11 00 68 00 41 54 2b 43 47 53 4e 0d 0a 7e";
		string text13 = "\\?(.*?)\\?";
		string text14 = "value:";
		string value = "release";
		string msg2 = " Description : ";
		UI.RichLogs(msg, Color.FromArgb(245, 245, 245), nextline: true, bold: true);
		byte[] request = utils.HexStringToBytes(s);
		PortIO.Write(request);
		byte[] hex = PortIO.Read();
		string text15 = utils.HexToString(hex);
		Console.WriteLine(text + text15);
		string[,] array = new string[4, 2]
		{
			{ text2, text3 },
			{ text4, text5 },
			{ text6, text7 },
			{ text8, text9 }
		};
		int i = 0;
		for (int num = array.GetLength(0) - 1; i <= num; i++)
		{
			string text16 = utils.ExtractSubstring(text15, array[i, 1] + text10, endDelimiter);
			if (!string.IsNullOrEmpty(text16))
			{
				UI.RichLogs(text11 + array[i, 0] + text12, Color.FromArgb(245, 245, 245));
				UI.RichLogs(text16, Color.FromArgb(124, 235, 60), nextline: true, bold: true);
			}
		}
		request = utils.HexStringToBytes(s2);
		PortIO.Write(request);
		byte[] hex2 = PortIO.Read();
		string input = utils.HexToString(hex2);
		string pattern = text13;
		MatchCollection matchCollection = Regex.Matches(input, pattern);
		foreach (Match item in matchCollection)
		{
			string value2 = item.Groups[1].Value;
			Console.WriteLine(text14 + value2);
			if ((!string.IsNullOrWhiteSpace(value2) & (value2.Length >= 5)) && value2.Contains(value))
			{
				UI.RichLogs(msg2, Color.FromArgb(245, 245, 245));
				UI.RichLogs(value2, Color.FromArgb(124, 235, 60), nextline: true, bold: true);
			}
		}
		return method_03();
	}

	public static bool method_05()
	{
		string s = "7e 01 00 00 00 08 00 00 00 7e";
		string s2 = "7e 03 00 00 00 11 00 68 00 41 54 2b 43 47 53 4e 0d 0a 7e";
		byte[] request = utils.HexStringToBytes(s);
		PortIO.Write(request);
		byte[] hex = PortIO.Read();
		utils.HexToString(hex);
		request = utils.HexStringToBytes(s2);
		PortIO.Write(request);
		PortIO.Read();
		return true;
	}

	private bool method_06()
	{
		string msg = "Waiting SPD USB Connection... ";
		string category = "SPRD";
		string msg2 = "OK";
		string msg3 = "Name : ";
		string text = "Connecting to device ";
		string text2 = "... ";
		string msg4 = "";
		string msg5 = "Switch phone to DIAG mode...  ";
		string objB = "Factory Reset [1]";
		string hex = "7E 00 00 00 00 08 00 FE 81 7E";
		string hex2 = "7E 00 00 00 00 08 00 FE 95 7E";
		string msg6 = "Sending Pleloader... ";
		string value = "SPRD3";
		string text3 = "00";
		string msg7 = "Failed";
		string msg8 = "Info : Do not press any button to enter diag mode.insert the USB cable into the device.";
		string msg9 = "Info : Port Not Open";
		string msg10 = "Reconnecting... ";
		string text4 = ":";
		UI.RichLogs(msg, UI.log);
		if (!UsbScan.FindUSB(category))
		{
			return false;
		}
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg3, UI.log);
		UI.RichLogs(Portinfo.Name, UI.info, nextline: true, bold: true);
		UI.RichLogs(text + Portinfo.COM + text2, UI.log);
		UI.RichLogs(msg4, UI.ok, nextline: false, bold: true);
		Marshal.AllocHGlobal(1024);
		PortIO.Open(Portinfo.COM);
		if (!PortIO.serialPort.IsOpen)
		{
			UI.RichLogs(msg7, Color.Tomato, nextline: true, bold: true);
			UI.RichLogs(msg9, Color.Tomato, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg5, UI.log);
		if (object.Equals(State.Operation, objB))
		{
			field_01 = utils.StringToByteArray(hex);
		}
		else
		{
			field_01 = utils.StringToByteArray(hex2);
		}
		byte[] request = field_01;
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg6, UI.log);
		PortIO.Write(request);
		byte[] array = PortIO.Read();
		string text5 = Encoding.ASCII.GetString(array);
		Console.WriteLine(utils.BytesToHextring(array));
		if (!text5.Contains(value) && utils.BytesToHextring(array).Substring(0, 2) == text3)
		{
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg10, UI.log);
			PortIO.Close();
			Thread.Sleep(3000);
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			if (!method_07())
			{
				return false;
			}
			DeviceState.SetInit(Portinfo.VID + text4 + Portinfo.PID + text4 + Portinfo.COM + text4 + Portinfo.Name);
			return true;
		}
		UI.RichLogs(msg7, Color.Tomato, nextline: true, bold: true);
		UI.RichLogs(msg8, Color.Tomato, nextline: true, bold: true);
		PortIO.Close();
		return false;
	}

	public bool method_07()
	{
		string msg = "";
		string msg2 = "Waiting for DIAG device... ";
		string category = "SPRD";
		string msg3 = "OK";
		string msg4 = "Handshaking... ";
		string msg5 = "Failed";
		string msg6 = "Info : Port Not Open";
		Thread.Sleep(500);
		UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg2, UI.log);
		if (UsbScan.FindUSB(category))
		{
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg4, UI.log);
			PortIO.Open(Portinfo.COM);
			if (!PortIO.serialPort.IsOpen)
			{
				UI.RichLogs(msg5, Color.Tomato, nextline: true, bold: true);
				UI.RichLogs(msg6, Color.Tomato, nextline: true, bold: true);
				PortIO.Close();
				return false;
			}
			Thread.Sleep(200);
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			return method_05();
		}
		return false;
	}

	static Class27()
	{
		string hex = "7E 00 00 00 00 08 00 FE 81 7E";
		field_01 = utils.StringToByteArray(hex);
	}

	public static int method_08(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
