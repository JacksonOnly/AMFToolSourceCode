using System;
using System.Collections.Generic;

namespace AMFTool.Library.Odin.PIT;

public class PitResult
{
	public bool IsSuccess { get; set; }

	public string ErrorMessage { get; set; } = string.Empty;

	public byte[] PitData { get; set; } = Array.Empty<byte>();

	public List<PitEntry> PitEntries { get; set; } = new List<PitEntry>();

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
