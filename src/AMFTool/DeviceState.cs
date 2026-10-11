using System;
using System.Collections.Generic;

namespace AMFTool;

public class DeviceState
{
	public static string LastFingerprint;

	public static List<string> Partitiontarget;

	public static string MetaMode { get; set; }

	public static bool CheckedIMEI1 { get; set; }

	public static bool CheckedIMEI2 { get; set; }

	public static bool Unlock { get; set; }

	public static bool Reboot { get; set; }

	public static string Brand { get; set; }

	public static string ModelDa { get; set; }

	public static string Model { get; set; }

	public static string IMEI1 { get; set; }

	public static string IMEI2 { get; set; }

	public static string Serialnumber { get; set; }

	public static bool DeviceInit { get; set; }

	public static void SetInit(string fingerprint)
	{
		DeviceInit = true;
		LastFingerprint = fingerprint;
	}

	public static void ResetInit(string message)
	{
		string lastFingerprint = "";
		Console.WriteLine(message);
		DeviceInit = false;
		LastFingerprint = lastFingerprint;
	}

	static DeviceState()
	{
		string lastFingerprint = "";
		MetaMode = string.Empty;
		CheckedIMEI1 = false;
		CheckedIMEI2 = false;
		Unlock = false;
		Reboot = false;
		Brand = string.Empty;
		ModelDa = string.Empty;
		Model = string.Empty;
		IMEI1 = string.Empty;
		IMEI2 = string.Empty;
		Serialnumber = string.Empty;
		DeviceInit = false;
		LastFingerprint = lastFingerprint;
		Partitiontarget = new List<string>();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
