namespace AMFTool;

public class User
{
	public static string Email { get; set; }

	public static string Password { get; set; }

	public static string HWID { get; set; }

	public static string Expired { get; set; }

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
