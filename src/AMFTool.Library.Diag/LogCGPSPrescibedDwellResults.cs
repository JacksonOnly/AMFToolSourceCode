namespace AMFTool.Library.Diag;

public class LogCGPSPrescibedDwellResults : LogMessage
{
	private const int indexRespType = 1;

	private const int indexPacketsNum = 6;

	private const int indexFlags = 5;

	private long c684a;

	private long C684A
	{
		get
		{
			return c684a;
		}
		set
		{
			c684a = value;
		}
	}

	public LogCGPSPrescibedDwellResults(LogMessage msgVal)
		: base(msgVal.msgID, msgVal.message)
	{
	}

	public byte getRespType()
	{
		return message[27];
	}

	public byte getFlags()
	{
		if (message[17] == 0)
		{
			return message[31];
		}
		byte b = 0;
		return 0;
	}

	public double getCNO()
	{
		double field_ = Class38.field_3213;
		double field_2 = Class38.field_543;
		if (message[17] == 0 && (getFlags() & 1) == 1)
		{
			double num = (int)getWord(35);
			return num / field_;
		}
		return field_2;
	}

	public new static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
