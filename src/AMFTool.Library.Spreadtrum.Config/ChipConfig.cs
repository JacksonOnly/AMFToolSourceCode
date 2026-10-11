using System.Collections.Generic;

namespace AMFTool.Library.Spreadtrum.Config;

public class ChipConfig
{
	public uint PayloadAddr;

	public uint RandomAddr;

	public uint RandomBound;

	public uint KeyAddr;

	public ulong KeyValue;

	public uint Preload1Addr;

	public uint Preload2Addr;

	public uint Fdl1Addr;

	public uint Fdl2Addr;

	public string Name;

	public string Description;

	public static readonly Dictionary<string, ChipConfig> HwConfig;

	public ChipConfig(string name = "", string description = "", uint payloadAddr = 0u, uint randomAddr = 0u, uint randomBound = 0u, uint keyAddr = 0u, ulong keyValue = 0uL, uint preload1Addr = 0u, uint preload2Addr = 0u, uint fdl1Addr = 0u, uint fdl2Addr = 0u, uint preload1AddrFdl2Addr = 0u)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Name = name;
		Description = description;
		PayloadAddr = payloadAddr;
		RandomAddr = randomAddr;
		RandomBound = randomBound;
		KeyAddr = keyAddr;
		KeyValue = keyValue;
		Preload1Addr = preload1Addr;
		Preload2Addr = preload2Addr;
		Fdl1Addr = fdl1Addr;
		Fdl2Addr = fdl2Addr;
	}

	static ChipConfig()
	{
		string text = "SC7731E";
		long field_ = Class38.field_3549;
		string text2 = "SC9832E";
		string text3 = "SC9863A";
		string description = "SC9863A_32Bit";
		string text4 = "SC9863A_64";
		string description2 = "SC9863A_64Bit";
		string text5 = "Tiger T310_64";
		string description3 = "Tiger T310_64Bit";
		string text6 = "Tiger T606_64";
		string description4 = "Tiger T606_64Bit";
		string text7 = "Tiger T610_64";
		string description5 = "Tiger T610_64Bit";
		string text8 = "Tiger T612_64";
		string description6 = "Tiger T612";
		string text9 = "Tiger T616_64";
		string description7 = "Tiger T616_64Bit";
		string text10 = "Tiger T700_64";
		string description8 = "Tiger T700_64Bit";
		HwConfig = new Dictionary<string, ChipConfig>
		{
			{
				text,
				new ChipConfig(text, text, 1342189056u, 1342185488u, 345u, 1342188464u, (ulong)field_, 1342185472u, 1342189056u, 2675965952u, 2667576832u)
			},
			{
				text2,
				new ChipConfig(text2, text2, 20480u, 12360u, 974u, 20200u, 20480uL, 12320u, 20480u, 2675965952u, 2667576832u)
			},
			{
				text3,
				new ChipConfig(text3, description, 20480u, 12288u, 973u, 20296u, 20480uL, 12320u, 20480u, 2675965952u, 2667576832u)
			},
			{
				text4,
				new ChipConfig(text4, description2, 20480u, 12288u, 973u, 20296u, 20480uL, 12320u, 20480u, 2675965952u, 2667576832u)
			},
			{
				text5,
				new ChipConfig(text5, description3, 21760u, 12352u, 481u, 16200u, 21760uL, 12320u, 21760u, 2675965952u, 2667576832u)
			},
			{
				text6,
				new ChipConfig(text6, description4, 1694500864u, 1694587976u, 88u, 1694588744u, 1694500864uL, 1694580640u, 1694500864u, 2675965952u, 2667576832u)
			},
			{
				text7,
				new ChipConfig(text7, description5, 21760u, 12288u, 488u, 16200u, 21760uL, 12320u, 21760u, 2675965952u, 2667576832u)
			},
			{
				text8,
				new ChipConfig(text8, description6, 1694500864u, 1694587976u, 88u, 1694588744u, 1694500864uL, 1694580640u, 1694500864u, 2675965952u, 2667576832u)
			},
			{
				text9,
				new ChipConfig(text9, description7, 1694500864u, 1694587984u, 87u, 1694588744u, 1694500864uL, 1694580640u, 1694500864u, 2675965952u, 2667576832u)
			},
			{
				text10,
				new ChipConfig(text10, description8, 21760u, 12352u, 481u, 16200u, 21760uL, 12320u, 21760u, 2675965952u, 2667576832u)
			}
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
