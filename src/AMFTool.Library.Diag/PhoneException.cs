using System;
using System.Runtime.Serialization;

namespace AMFTool.Library.Diag;

[Serializable]
public class PhoneException : Exception
{
	public PhoneException()
	{
	}

	public PhoneException(string ExceptionMessage)
		: base(ExceptionMessage)
	{
	}

	public PhoneException(string ExceptionMessage, Exception innerException)
		: base(ExceptionMessage, innerException)
	{
	}

	protected PhoneException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
