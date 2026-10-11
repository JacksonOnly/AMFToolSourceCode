#define DEBUG
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;

namespace AMFTool.Library.Usb;

public class UsbScan
{
	private static readonly (int vid, int pid)[] Mediatek;

	private static readonly (int vid, int pid)[] Qualcomm;

	private static readonly (int vid, int pid)[] Hisi;

	private static readonly (int vid, int pid)[] MTKHighspeed;

	private static readonly (int vid, int pid)[] SPD;

	private static readonly (int vid, int pid)[] SPRD;

	private static readonly Regex RX_VID;

	private static readonly Regex RX_PID;

	private static readonly Regex RX_COM;

	public static bool FindUSB(string category = "ALL", int timeout = 360000)
	{
		string text = "ALL";
		int num = 6;
		int num2 = 0;
		string item = "MTK";
		int num3 = 1;
		string item2 = "QCOM";
		int num4 = 2;
		string item3 = "HISI";
		int num5 = 3;
		string item4 = "HIGHSPEED";
		int num6 = 4;
		string item5 = "SPD";
		int num7 = 5;
		string item6 = "SPRD";
		string scope = "root\\CIMV2";
		string queryString = "SELECT Name,PNPDeviceID FROM Win32_PnPEntity WHERE Name LIKE '%(COM%'";
		string propertyName = "Name";
		object obj = null;
		string propertyName2 = "PNPDeviceID";
		int fromBase = 16;
		string text2 = "X4";
		string text3 = "N/A";
		int num8 = 8;
		string text4 = "USB Detected Name = ";
		string text5 = " COM = ";
		string text6 = " VID = ";
		string text7 = " PID = ";
		int num9 = 7;
		int millisecondsTimeout = 250;
		string cat = (category ?? text).Trim().ToUpperInvariant();
		(string, (int, int)[])[] array = new(string, (int, int)[])[6]
		{
			(item, Mediatek),
			(item2, Qualcomm),
			(item3, Hisi),
			(item4, MTKHighspeed),
			(item5, SPD),
			(item6, SPRD)
		};
		(string, (int, int)[])[] array2 = ((string, (int, int)[])[])((!(cat == text)) ? ((Array)array.Where(delegate((string tag, (int vid, int pid)[] list) x)
		{
			string text10 = "QUALCOMM";
			string text11 = "QCOM";
			string text12 = "HUAWEI";
			string text13 = "HISI";
			string text14 = "SPD";
			string text15 = "SPRD";
			if (x.tag == cat)
			{
				return true;
			}
			if (!(cat == text10))
			{
				if (!(cat == text12))
				{
					if (cat == text14)
					{
						if (x.tag == text14)
						{
							return true;
						}
						if (!(cat == text15))
						{
							return false;
						}
					}
					else if (!(cat == text15))
					{
						return false;
					}
					return x.tag == text15;
				}
				if (x.tag == text13)
				{
					return true;
				}
				if (cat == text14)
				{
					if (x.tag == text14)
					{
						return true;
					}
					if (!(cat == text15))
					{
						return false;
					}
				}
				else if (!(cat == text15))
				{
					return false;
				}
				return x.tag == text15;
			}
			if (x.tag == text11)
			{
				return true;
			}
			if (!(cat == text12))
			{
				if (cat == text14)
				{
					if (x.tag == text14)
					{
						return true;
					}
					if (!(cat == text15))
					{
						return false;
					}
				}
				else if (!(cat == text15))
				{
					return false;
				}
				return x.tag == text15;
			}
			if (x.tag == text13)
			{
				return true;
			}
			if (cat == text14)
			{
				if (x.tag == text14)
				{
					return true;
				}
				if (!(cat == text15))
				{
					return false;
				}
			}
			else if (!(cat == text15))
			{
				return false;
			}
			return x.tag == text15;
		}).ToArray()) : ((Array)array));
		if (array2.Length != 0)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (true)
			{
				if (stopwatch.ElapsedMilliseconds >= timeout)
				{
					return (byte)num2 != 0;
				}
				if (JobStatus.Token.IsCancellationRequested)
				{
					break;
				}
				try
				{
					using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, queryString);
					foreach (ManagementObject item8 in managementObjectSearcher.Get())
					{
						object obj2 = item8[propertyName];
						string text8 = (string)((obj2 != null) ? obj2.ToString() : obj);
						object obj3 = item8[propertyName2];
						string text9 = (string)((obj3 != null) ? obj3.ToString() : obj);
						if (string.IsNullOrEmpty(text8) ? ((byte)num3 != 0) : string.IsNullOrEmpty(text9))
						{
							continue;
						}
						Match match = RX_VID.Match(text9);
						Match match2 = RX_PID.Match(text9);
						if ((!match.Success) ? ((byte)num3 != 0) : ((match2.Success ? 1 : 0) == num2))
						{
							continue;
						}
						int vid = Convert.ToInt32(match.Groups[num3].Value, fromBase);
						int pid = Convert.ToInt32(match2.Groups[num3].Value, fromBase);
						(string, (int, int)[])[] array3 = array2;
						for (int num10 = num2; num10 < array3.Length; num10 += num3)
						{
							(string, (int, int)[]) tuple = array3[num10];
							_ = tuple.Item1;
							(int, int)[] item7 = tuple.Item2;
							if ((item7.Any(((int vid, int pid) x) => x.vid == vid && x.pid == pid) ? 1 : 0) != num2)
							{
								Match match3 = RX_COM.Match(text8);
								Portinfo.VID = vid.ToString(text2);
								Portinfo.PID = pid.ToString(text2);
								Portinfo.COM = ((!match3.Success) ? text3 : match3.Groups[num3].Value);
								Portinfo.Name = text8;
								string[] array4 = new string[num8];
								array4[num2] = text4;
								array4[num3] = Portinfo.Name;
								array4[num4] = text5;
								array4[num5] = Portinfo.COM;
								array4[num6] = text6;
								array4[num7] = Portinfo.VID;
								array4[num] = text7;
								array4[num9] = Portinfo.PID;
								Console.WriteLine(string.Concat(array4));
								return (byte)num3 != 0;
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
				}
				Thread.Sleep(millisecondsTimeout);
			}
			return (byte)num2 != 0;
		}
		return false;
	}

	public static bool FindMTP()
	{
		int num = 0;
		string scope = "root\\CIMV2";
		string queryString = "SELECT * FROM Win32_POTSModem WHERE Description LIKE '%SAMSUNG%' OR Name LIKE '%SAMSUNG%'";
		string propertyName = "Description";
		object obj = null;
		string text = "";
		string propertyName2 = "Name";
		string propertyName3 = "AttachedTo";
		string value = "SAMSUNG";
		int num2 = 1;
		int millisecondsTimeout = 1000;
		int num3 = 180000;
		bool flag = false;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		try
		{
			while (stopwatch.ElapsedMilliseconds < num3)
			{
				if (!JobStatus.Token.IsCancellationRequested)
				{
					try
					{
						ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, queryString);
						foreach (ManagementObject item in managementObjectSearcher.Get())
						{
							object obj2 = item[propertyName];
							object obj3;
							if (obj2 == null)
							{
								obj3 = obj;
								if (obj3 != null)
								{
									goto IL_00b1;
								}
							}
							else
							{
								obj3 = obj2.ToString();
								if (obj3 != null)
								{
									goto IL_00b1;
								}
							}
							obj3 = text;
							goto IL_00b1;
							IL_00b1:
							string text2 = (string)obj3;
							object obj4 = item[propertyName2];
							object obj5;
							if (obj4 == null)
							{
								obj5 = obj;
								if (obj5 != null)
								{
									goto IL_00d6;
								}
							}
							else
							{
								obj5 = obj4.ToString();
								if (obj5 != null)
								{
									goto IL_00d6;
								}
							}
							obj5 = text;
							goto IL_00d6;
							IL_00d6:
							string text3 = (string)obj5;
							object obj6 = item[propertyName3];
							object obj7;
							if (obj6 == null)
							{
								obj7 = obj;
								if (obj7 != null)
								{
									goto IL_00fb;
								}
							}
							else
							{
								obj7 = obj6.ToString();
								if (obj7 != null)
								{
									goto IL_00fb;
								}
							}
							obj7 = text;
							goto IL_00fb;
							IL_00fb:
							string text4 = (string)obj7;
							if ((text2.ToUpper().Contains(value) || text3.ToUpper().Contains(value)) ? ((string.IsNullOrEmpty(text4) ? 1 : 0) == num) : ((byte)num != 0))
							{
								Portinfo.COM = text4;
								Portinfo.Name = text2;
								flag = (byte)num2 != 0;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
					Thread.Sleep(millisecondsTimeout);
					continue;
				}
				return (byte)num != 0;
			}
		}
		catch (Exception ex2)
		{
			Console.WriteLine(ex2.Message);
			flag = (byte)num != 0;
		}
		finally
		{
			stopwatch.Stop();
		}
		return flag;
	}

	public static bool FindDiag()
	{
		int num = 0;
		object obj = null;
		string scope = "root\\CIMV2";
		string queryString = "SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%Diagnostic%' OR Name like '%DIAG%' ";
		string propertyName = "Name";
		string pattern = "\\((COM\\d+)\\)";
		int num2 = 1;
		int millisecondsTimeout = 1000;
		int num3 = 180000;
		bool flag = false;
		ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = (ManagementObjectCollection.ManagementObjectEnumerator)obj;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		try
		{
			for (; stopwatch.ElapsedMilliseconds < num3; Thread.Sleep(millisecondsTimeout))
			{
				if (!JobStatus.Token.IsCancellationRequested)
				{
					try
					{
						managementObjectEnumerator = new ManagementObjectSearcher(scope, queryString).Get().GetEnumerator();
						if (managementObjectEnumerator.MoveNext())
						{
							ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
							Match match = Regex.Match(managementObject.GetPropertyValue(propertyName).ToString(), pattern);
							if (match.Success)
							{
								Portinfo.COM = match.Groups[num2].Value;
							}
							string[] array = new string[num2];
							array[num] = managementObject.GetPropertyValue(propertyName).ToString();
							Portinfo.Name = string.Concat(array);
							flag = (byte)num2 != 0;
						}
						if (!flag)
						{
							continue;
						}
					}
					finally
					{
						if (managementObjectEnumerator !=null)
						{
							((IDisposable)managementObjectEnumerator).Dispose();
						}
					}
					break;
				}
				return (byte)num != 0;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			flag = (byte)num != 0;
		}
		finally
		{
			stopwatch.Stop();
		}
		return flag;
	}

	static UsbScan()
	{
		string pattern = "VID_([0-9A-F]{4})";
		string pattern2 = "PID_([0-9A-F]{4})";
		string pattern3 = "\\((COM\\d+)\\)";
		Mediatek = new(int, int)[15]
		{
			(3725, 3),
			(3725, 24576),
			(3725, 8192),
			(3725, 8193),
			(3725, 8447),
			(3725, 12288),
			(4100, 24576),
			(8921, 6),
			(4046, 61952),
			(4046, 53737),
			(4046, 53730),
			(4046, 53740),
			(4046, 53725),
			(3725, 8199),
			(3725, 8206)
		};
		Qualcomm = new(int, int)[1] { (1478, 36872) };
		Hisi = new(int, int)[1] { (4817, 13833) };
		MTKHighspeed = new(int, int)[1] { (3725, 8193) };
		SPD = new(int, int)[1] { (6018, 19712) };
		SPRD = new(int, int)[1] { (6018, 19712) };
		RX_VID = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		RX_PID = new Regex(pattern2, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		RX_COM = new Regex(pattern3, RegexOptions.IgnoreCase | RegexOptions.Compiled);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
