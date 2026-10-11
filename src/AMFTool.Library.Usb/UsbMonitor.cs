using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMFTool.Library.Usb;

public class UsbMonitor
{
	public class ComInfo
	{
		public string Name { get; set; }

		public string Hwid { get; set; }

		public string ComPort { get; set; }

		public int Type { get; set; }

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public static List<ComInfo> ListDevices = new List<ComInfo>();

	public static SerialPort Ports = new SerialPort();

	public static readonly Stopwatch Watch = new Stopwatch();

	public static long Delta = 0L;

	public static bool Open = false;

	public static string Vid { get; set; }

	public static string Pid { get; set; }

	public static string Rev { get; set; }

	public static string State { get; set; }

	public static int Com { get; set; }

	public unsafe static void GetComInfo()
	{
		object obj = null;
		ManagementEventWatcher deviceWatcher = (ManagementEventWatcher)obj;
		Watch.Start();
		Task.Run(delegate
		{
			string queryOrEventClassName = "SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 3";
			object obj2 = null;
			try
			{
				WqlEventQuery query = new WqlEventQuery(queryOrEventClassName);
				deviceWatcher = new ManagementEventWatcher(query);
				deviceWatcher.EventArrived += OnDeviceEventArrived;
				deviceWatcher.Start();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		});
	}

	public static void OnDeviceEventArrived(object sender, EventArrivedEventArgs e)
	{
		if (Watch.ElapsedMilliseconds - Delta >= 100L)
		{
			Delta = Watch.ElapsedMilliseconds;
			UpdateList();
		}
	}

	public static void UpdateList()
	{
		Task.Run(delegate
		{
			string queryString = "Select * From Win32_POTSModem Where Status=\"OK\"";
			string text = "AttachedTo";
			string oldValue = "COM";
			string text2 = "Name";
			object obj = null;
			string text3 = " (";
			string text4 = ")";
			string text5 = "DeviceID";
			int num = 0;
			string value = "6860";
			string value2 = "PID_685D";
			string value3 = "A185D30";
			string value4 = "samsung mobile";
			string value5 = "usb modem";
			string queryString2 = "SELECT * FROM Win32_PnPEntity Where Status=\"OK\"";
			string propertyName = "Caption";
			string value6 = "Serial Port";
			string value7 = "QUALCOMM";
			string value8 = "PRELOADER";
			string value9 = "MEDIATEK";
			string value10 = "SPRD";
			string value11 = "LGE";
			string value12 = "1.0";
			string text6 = "(COM";
			string propertyName2 = "HardwareID";
			string value13 = "diagserd";
			string value14 = "0002";
			int type = 3;
			List<ComInfo> list = new List<ComInfo>();
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
				try
				{
					foreach (ManagementBaseObject item in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)item;
						ComInfo comInfo = new ComInfo
						{
							ComPort = managementObject.GetPropertyValue(Convert.ToString(text)).ToString().Replace(oldValue, string.Empty)
						};
						object propertyValue = managementObject.GetPropertyValue(text2);
						string text7 = (string)((propertyValue == null) ? obj : propertyValue.ToString());
						object propertyValue2 = managementObject.GetPropertyValue(text);
						comInfo.Name = text7 + text3 + (string)((propertyValue2 != null) ? propertyValue2.ToString() : obj) + text4;
						comInfo.Hwid = managementObject.GetPropertyValue(Convert.ToString(text5)).ToString();
						comInfo.Type = 0;
						if (comInfo.Hwid.Contains(value) || (comInfo.Hwid.Contains(value2) && !comInfo.Hwid.Contains(value3)) || (comInfo.Name.ToLower().Contains(value4) && comInfo.Name.ToLower().Contains(value5)))
						{
							comInfo.Type = 1;
						}
						else if (comInfo.Hwid.Contains(value2) && comInfo.Hwid.Contains(value3))
						{
							comInfo.Type = 2;
						}
						list.Add(comInfo);
					}
					managementObjectSearcher.Dispose();
				}
				catch (COMException value15)
				{
					Console.WriteLine(value15);
				}
				ManagementObjectSearcher managementObjectSearcher2 = new ManagementObjectSearcher(queryString2);
				try
				{
					foreach (ManagementObject item2 in managementObjectSearcher2.Get())
					{
						if (item2[propertyName] !=null)
						{
							string text8 = item2[Convert.ToString(text2)].ToString();
							if ((!text8.Contains(value6) && !text8.ToUpper().Contains(value7) && !text8.ToUpper().Contains(value8) && !text8.ToUpper().Contains(value9) && !text8.ToUpper().Contains(value10) && !text8.Contains(value11) && !text8.Contains(value12)) ? ((byte)num != 0) : text8.Contains(text6))
							{
								string text9 = item2[Convert.ToString(text5)].ToString();
								string[] array = (string[])item2[propertyName2];
								ComInfo comInfo2 = new ComInfo
								{
									Name = item2[Convert.ToString(text2)].ToString(),
									Hwid = (string)((array.Length == 0) ? obj : array[num]),
									ComPort = BetweenStrings(item2[Convert.ToString(text2)].ToString(), text6, text4)
								};
								if (text9.ToLower().Contains(value13) ? text9.ToLower().Contains(value14) : ((byte)num != 0))
								{
									comInfo2.Type = type;
								}
								list.Add(comInfo2);
							}
						}
					}
					managementObjectSearcher2.Dispose();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				if (((list.Count == ListDevices.Count) ? 1 : 0) == num)
				{
					ListDevices = list;
					try
					{
						UpdateComboPort(ListDevices);
						return;
					}
					catch (Exception ex2)
					{
						Console.WriteLine(ex2.Message);
						return;
					}
				}
			}
			catch (Exception value16)
			{
				Console.WriteLine(value16);
			}
		});
	}

	public static void UpdateComboPort(List<ComInfo> list)
	{
		object text = null;
		string text2 = " (COM";
		string text3 = ")";
		string value = "SAMSUNG";
		string regex = string.Empty;
		if (Main.Instance.ComboPort.InvokeRequired)
		{
			Main.Instance.ComboPort.Invoke((MethodInvoker)delegate
			{
				object text4 = null;
				int num = 0;
				string text5 = " Waiting for Device Automatic";
				int num2 = 1;
				string text6 = "[MTP] ";
				int num3 = 2;
				string text7 = "[DLM] ";
				string text8 = "[DIAG] ";
				string value2 = "SAMSUNG";
				string pattern = "\\((COM\\d+)\\)";
				string oldValue = "COM";
				string text9 = "";
				if (list.Count < Main.Instance.ComboPort.Properties.Items.Count)
				{
					Main.Instance.ComboPort.Text = (string)text4;
					Main.Instance.ComboPort.AllowDrop = false;
				}
				Main.Instance.ComboPort.Text = text5;
				Main.Instance.ComboPort.Properties.Items.Clear();
				foreach (ComInfo item in list)
				{
					string text10 = string.Empty;
					if (item.Type != 1)
					{
						if (item.Type != 2)
						{
							if (item.Type == 3)
							{
								text10 = text8;
							}
						}
						else
						{
							text10 = text7;
						}
					}
					else
					{
						text10 = text6;
					}
					string text11 = text10 + item.Name;
					Main.Instance.ComboPort.Properties.Items.Add(text11);
					if (item.Name.Contains(value2) && string.IsNullOrEmpty(regex))
					{
						regex = text11;
					}
				}
				if ((string.IsNullOrEmpty(regex) ? 1 : 0) != num)
				{
					if (list.Count > num)
					{
						Main.Instance.ComboPort.SelectedIndex = num;
					}
				}
				else
				{
					Main.Instance.ComboPort.SelectedItem = regex;
				}
				if ((string.IsNullOrEmpty(Main.Instance.ComboPort.Text) ? 1 : 0) == num)
				{
					Match match = Regex.Match(Main.Instance.ComboPort.Text, pattern);
					if (match.Success)
					{
						Com = Convert.ToInt32(match.Groups[num2].Value.Replace(oldValue, string.Empty));
					}
					foreach (ComInfo item2 in list)
					{
						string[] array = VidPid(item2.Hwid);
						Vid = array[num];
						Pid = array[num2];
						Rev = array[num3];
					}
					if ((string.IsNullOrEmpty(Vid) ? 1 : 0) == num)
					{
						Open = (byte)num2 != 0;
					}
				}
				else
				{
					Open = (byte)num != 0;
					Vid = text9;
					Pid = text9;
					Com = num;
				}
			});
			return;
		}
		if (list.Count < Main.Instance.ComboPort.Properties.Items.Count)
		{
			Main.Instance.ComboPort.Text = (string)text;
			Main.Instance.ComboPort.AllowDrop = false;
		}
		Main.Instance.ComboPort.Properties.Items.Clear();
		foreach (ComInfo item3 in list)
		{
			Console.WriteLine(item3.Name);
			Main.Instance.ComboPort.Properties.Items.Add(item3.Name + text2 + item3.ComPort + text3);
			if (item3.Name.Contains(value) && string.IsNullOrEmpty(regex))
			{
				regex = item3.Name + text2 + item3.ComPort + text3;
			}
		}
		Main.Instance.ComboPort.SelectedItem = regex;
	}

	public static string BetweenStrings(string text, string start, string end)
	{
		int num = text.IndexOf(start) + start.Length;
		int num2 = text.IndexOf(end, num);
		if (!object.Equals(end, string.Empty))
		{
			return text.Substring(num, num2 - num);
		}
		return text.Substring(num);
	}

	public static string[] VidPid(string stream)
	{
		string value = "VID_";
		string value2 = "PID_";
		string value3 = "REV_";
		string[] array = new string[3];
		int num = stream.IndexOf(value);
		string text = stream.Substring(num + 4);
		array[0] = text.Substring(0, 4);
		int num2 = stream.IndexOf(value2);
		string text2 = stream.Substring(num2 + 4);
		array[1] = text2.Substring(0, 4);
		int num3 = stream.IndexOf(value3);
		string text3 = stream.Substring(num3 + 4);
		array[2] = text3.Substring(0, 4);
		return array;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
