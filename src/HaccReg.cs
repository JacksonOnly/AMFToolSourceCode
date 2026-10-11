using System;
using System.Collections.Generic;

public class HaccReg
{
	private int sej_base;

	private Func<int, int, List<int>> read32;

	private Func<int, List<int>, bool> write32;

	private Dictionary<string, int> regval;

	public int this[string key]
	{
		get
		{
			string text = "key:";
			string text2 = "read32:";
			string text3 = "addr:";
			Console.WriteLine(text + key);
			if (!ConstantsAES.regval.ContainsKey(key))
			{
				throw new KeyNotFoundException();
			}
			Console.WriteLine(text2 + key);
			int arg = ConstantsAES.regval[key] + sej_base;
			Console.WriteLine(text3 + arg);
			return read32(arg, 1)[0];
		}
		set
		{
			string text = "write32:";
			if (!ConstantsAES.regval.ContainsKey(key))
			{
				throw new KeyNotFoundException();
			}
			Console.WriteLine(text + key);
			int arg = ConstantsAES.regval[key] + sej_base;
			write32(arg, new List<int> { value });
		}
	}

	public HaccReg(CryptoSetup setup)
	{
		sej_base = setup.sej_base.Value;
		read32 = setup.read32;
		write32 = setup.write32;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
