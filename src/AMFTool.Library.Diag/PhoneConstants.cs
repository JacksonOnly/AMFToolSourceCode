namespace AMFTool.Library.Diag;

public sealed class PhoneConstants
{
	public const int PhoneConnectTimeOutInSec = 10;

	public const int DefaultRespSize = 2048;

	public const int NumberOfDemodBitsInOneLogPacket = 20;

	public const int LOG_NOTHING = 0;

	public const int LOG_C_HIGH_LEVEL_STOP = 16384;

	public const int LOG_FN = 2;

	public const int LOG_INF = 8;

	public const int LOG_ERR = 16;

	public const int LOG_FN_AHDLC = 64;

	public const int LOG_INF_AHDLC = 256;

	public const int LOG_IO_DEV = 1024;

	public const int LOG_RET_DEV = 4096;

	public const int LOG_ERR_DEV = 8192;

	public const int LOG_ALL = 65535;

	private PhoneConstants()
	{
	}
}
