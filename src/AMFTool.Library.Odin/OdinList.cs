using System;
using System.IO;

namespace AMFTool.Library.Odin;

public class OdinList
{
	public string Type { get; set; } = string.Empty;

	public string PartitionName { get; set; } = string.Empty;

	public string EntryName { get; set; } = string.Empty;

	public string TarPath { get; set; } = string.Empty;

	public long BeginAddress { get; set; }

	public long EndAddress { get; set; }

	public bool IsLz4
	{
		get
		{
			string value = ".lz4";
			if (EntryName != null)
			{
				return EntryName.EndsWith(value, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}
	}

	public bool IsPit
	{
		get
		{
			string value = ".pit";
			string b = "pit";
			if (string.IsNullOrWhiteSpace(EntryName))
			{
				return false;
			}
			if (EntryName.EndsWith(value, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			string fileName = Path.GetFileName(EntryName);
			return string.Equals(fileName, b, StringComparison.OrdinalIgnoreCase);
		}
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
