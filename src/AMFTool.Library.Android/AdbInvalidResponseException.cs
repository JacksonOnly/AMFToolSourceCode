namespace AMFTool.Library.Android;

public class AdbInvalidResponseException : AdbException
{
	internal AdbInvalidResponseException(string response)
		: base("The server returned an invalid response '" + response + "'")
	{
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
