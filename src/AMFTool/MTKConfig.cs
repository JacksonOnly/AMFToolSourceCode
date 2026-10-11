namespace AMFTool;

public class MTKConfig
{
	public static string CpuType { get; set; } = string.Empty;

	public static bool MTKServer { get; set; } = false;

	public static bool UsingCarbonara { get; set; } = false;

	public static bool Highspeed { get; set; } = true;

	public static bool Repartition { get; set; } = false;

	public static bool FormatALL { get; set; } = false;

	public static bool LocalDA { get; set; } = false;

	public static bool LocalEMI { get; set; } = false;

	public static bool Infodevice { get; set; } = false;

	public static bool Infirmware { get; set; } = true;

	public static bool SerlEMI { get; set; } = false;

	public static bool Auth { get; set; } = false;

	public static bool CarbonaraEX { get; set; } = false;

	public static string PathPreloader { get; set; } = string.Empty;

	public static string PathDownloadAgent { get; set; } = string.Empty;

	public static string PathFirmware { get; set; } = string.Empty;

	public static bool Customda { get; set; } = false;

	public static bool Danofarse { get; set; } = false;

	public static bool Needauthfile { get; set; } = false;

	public static int PreloaderMethod { get; set; } = 0;

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
