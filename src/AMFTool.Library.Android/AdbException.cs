using System;

namespace AMFTool.Library.Android;

public class AdbException : Exception
{
	internal AdbException(string message)
		: base(message)
	{
	}
}
