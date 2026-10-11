using System;
using System.IO;

namespace AMFTool.Library.Android;

public static class AdbHelpers
{
	public static string CombinePath(string path1, string path2)
	{
		return Path.Combine(path1, path2).Replace('\\', '/');
	}

	private static DateTime GetUnixEpoch()
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	}

	public static DateTime FromUnixTime(int unixTime)
	{
		return GetUnixEpoch().AddSeconds(unixTime).ToLocalTime();
	}

	public static int ToUnixTime(DateTime dateTime)
	{
		return Convert.ToInt32((dateTime.ToUniversalTime() - GetUnixEpoch()).TotalSeconds);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
