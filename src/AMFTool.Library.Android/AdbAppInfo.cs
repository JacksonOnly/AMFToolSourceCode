namespace AMFTool.Library.Android;

public class AdbAppInfo
{
	public string Name { get; private set; }

	public string FileName { get; private set; }

	public AdbAppType Type { get; private set; }

	public AdbAppLocation Location { get; private set; }

	public AdbAppInfo(string name, string fileName, AdbAppType type, AdbAppLocation location)
	{
		Name = name;
		FileName = fileName;
		Type = type;
		Location = location;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
