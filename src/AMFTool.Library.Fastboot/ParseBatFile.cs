using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AMFTool.Library.Fastboot;

public class ParseBatFile
{
	public class BatEntry
	{
		public string Command;

		public string Partition;

		public string FileName;

		public long FileSize;

		public string Location;

		private object method_01()
		{
			return new int[0];
		}
	}

	public static List<BatEntry> ParseFastbootBat(string batpath)
	{
		string value = "fastboot";
		string value2 = " erase ";
		string pattern = "erase\\s+(\\S+)";
		string command = "erase";
		string text = "";
		string value3 = " flash ";
		string pattern2 = "flash\\s+(\\S+)\\s+(.+?)(\\s+\\|\\||$)";
		string text2 = "%~dp0";
		string text3 = "\\";
		string command2 = "flash";
		List<BatEntry> list = new List<BatEntry>();
		string directoryName = Path.GetDirectoryName(batpath);
		string[] array = File.ReadAllLines(batpath);
		foreach (string text4 in array)
		{
			string text5 = text4.Trim();
			if (!text5.StartsWith(value))
			{
				continue;
			}
			if (!text5.Contains(value2))
			{
				if (!text5.Contains(value3))
				{
					continue;
				}
				Match match = Regex.Match(text5, pattern2);
				if (match.Success)
				{
					string value4 = match.Groups[1].Value;
					string text6 = match.Groups[2].Value.Trim().Trim('"');
					if (text6.Contains(text2))
					{
						text6 = text6.Replace(text2, directoryName + text3);
					}
					string fullPath = Path.GetFullPath(text6);
					string fileName = Path.GetFileName(fullPath);
					long fileSize = 0L;
					if (File.Exists(fullPath))
					{
						fileSize = new FileInfo(fullPath).Length;
					}
					list.Add(new BatEntry
					{
						Command = command2,
						Partition = value4,
						FileName = fileName,
						FileSize = fileSize,
						Location = fullPath
					});
				}
			}
			else
			{
				Match match2 = Regex.Match(text5, pattern);
				if (match2.Success)
				{
					list.Add(new BatEntry
					{
						Command = command,
						Partition = match2.Groups[1].Value,
						FileName = text,
						FileSize = 0L,
						Location = text
					});
				}
			}
		}
		return list;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
