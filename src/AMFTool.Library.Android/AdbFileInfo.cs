using System;

namespace AMFTool.Library.Android;

public class AdbFileInfo
{
	public string FullName { get; private set; }

	public string Name { get; private set; }

	public int Size { get; private set; }

	public int Mode { get; private set; }

	public DateTime Modified { get; private set; }

	public bool IsFile => (Mode & 0x8000) > 0;

	public bool IsDirectory => (Mode & 0x4000) > 0;

	public bool IsSymbolicLink => (Mode & 0xA000) > 0;

	internal AdbFileInfo(string fullName, string name, int size, int mode, DateTime modified)
	{
		FullName = fullName;
		Name = name;
		Size = size;
		Mode = mode;
		Modified = modified;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
