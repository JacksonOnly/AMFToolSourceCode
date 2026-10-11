using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMFTool.Library.Mediatek.Config;

namespace AMFTool.Library.Mediatek;

public class Partition
{
	private MtkClass Mtk;

	private MtkConfig Config;

	private Func<long, long, string, bool, byte[]> ReadFlash;

	private Func<bool, string, (byte[], List<Partf>)> ReadPMT;

	public Partition(MtkClass mtk, Func<long, long, string, bool, byte[]> readFlash, Func<bool, string, (byte[], List<Partf>)> readPmt)
	{
		Mtk = mtk;
		Config = mtk.Config;
		ReadFlash = readFlash;
		ReadPMT = readPmt;
	}

	public (byte[], List<Partf>) GetPmt(bool backup = false, string parttype = "user")
	{
		string s = "PTv3";
		string s2 = "MPT3";
		object obj = null;
		int pageSize = Mtk.DaLoader.Dacfg.PageSize;
		int num = (int)(backup ? (Mtk.DaLoader.Dacfg.FlashSize - 2 * pageSize + pageSize) : (Mtk.DaLoader.Dacfg.FlashSize - 2 * pageSize));
		byte[] array = ReadFlash(num, 2 * Config.PageSize, parttype, arg4: false);
		uint num2 = Binary.ReadU32LE(array);
		if (num2 == Binary.ReadI32LE(Encoding.ASCII.GetBytes(s)) || num2 == Binary.ReadI32LE(Encoding.ASCII.GetBytes(s2)))
		{
			byte[] array2 = new byte[array.Length - 8];
			Array.Copy(array, 8, array2, 0, array2.Length);
			List<Partf> list = new List<Partf>();
			for (int i = 0; i < 128; i++)
			{
				Pmt.PtResident ptResident = new Pmt.PtResident(array2.Skip(i * 96).Take(96).ToArray());
				if (ptResident.Name.Take(4).SequenceEqual(new byte[4]))
				{
					break;
				}
				Partf partf = new Partf();
				partf.Name = Encoding.UTF8.GetString(ptResident.Name).TrimEnd(default(char));
				partf.Sector = ptResident.Offset / (ulong)Config.PageSize;
				partf.Sectors = ptResident.Size / (ulong)Config.PageSize;
				partf.Type = new byte[1] { 1 };
				partf.Flags = ptResident.MaskFlags;
				Partf item = partf;
				list.Add(item);
			}
			return (array, list);
		}
		return ((byte[])obj, (List<Partf>)obj);
	}

	public (byte[], object) GetGpt(GptSettings gptSettings, string parttype = "user")
	{
		object obj = null;
		string s = "BPI\0";
		string value = "BPI";
		string text = "EMMC_BOOT";
		string text2 = "UFS_BOOT";
		string text3 = "ufs";
		Gpt gpt = (Gpt)obj;
		byte[] array = ReadFlash(0L, 2 * Config.PageSize, parttype, arg4: false);
		if (array.Length == 0)
		{
			return ((byte[])obj, obj);
		}
		if (array.Take(4).SequenceEqual(Encoding.ASCII.GetBytes(s)))
		{
			Console.WriteLine(value);
			gpt = new Gpt(gptSettings.GptNumPartEntries, gptSettings.GptPartEntrySize, gptSettings.GptPartEntryStartLba);
			array = ReadFlash(0L, 32 * Config.PageSize, parttype, arg4: false);
			if (array.Length == 0)
			{
				return ((byte[])obj, obj);
			}
			gpt.ParseBpi(array, Config.PageSize);
			return (array, gpt);
		}
		if (array.Take(9).SequenceEqual(Encoding.ASCII.GetBytes(text)) && ReadPMT !=null)
		{
			Console.WriteLine(text);
			(byte[], List<Partf>) tuple = ReadPMT(arg1: false, parttype);
			if (tuple.Item1 == obj)
			{
				return ((byte[])obj, obj);
			}
			return (tuple.Item1, tuple.Item2);
		}
		if (!array.Take(8).SequenceEqual(Encoding.ASCII.GetBytes(text2)) || ReadPMT ==null)
		{
			gpt = new Gpt(gptSettings.GptNumPartEntries, gptSettings.GptPartEntrySize, gptSettings.GptPartEntryStartLba);
			Gpt.GptHeader gptHeader = gpt.ParseHeader(array, Config.PageSize);
			ulong firstUsableLba;
			if (((ReadOnlySpan<byte>)gptHeader.Signature).SequenceEqual((ReadOnlySpan<byte>)new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 }))
			{
				array = ReadFlash(Mtk.DaLoader.Dacfg.FlashSize - 16384L, 2 * Config.PageSize, parttype, arg4: false);
				if (array.Length != 0)
				{
					gptHeader = gpt.ParseHeader(array, Config.PageSize);
					if (!((ReadOnlySpan<byte>)gptHeader.Signature).SequenceEqual((ReadOnlySpan<byte>)new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 }))
					{
						firstUsableLba = gptHeader.FirstUsableLba;
						if (firstUsableLba != 0L)
						{
							long arg = (long)firstUsableLba * (long)Config.PageSize;
							if (Mtk.DaLoader.Dacfg.FlashType == text3)
							{
								arg = 32768L;
								array = ReadFlash(0L, 32768L, parttype, arg4: false);
							}
							else
							{
								array = ReadFlash(0L, arg, parttype, arg4: false);
							}
							if (array.Length != 0)
							{
								if (gpt.Parse(array, Config.PageSize))
								{
									return (array, gpt);
								}
								return ((byte[])obj, obj);
							}
							return ((byte[])obj, obj);
						}
						return ((byte[])obj, obj);
					}
					return ((byte[])obj, obj);
				}
				return ((byte[])obj, obj);
			}
			firstUsableLba = gptHeader.FirstUsableLba;
			if (firstUsableLba != 0L)
			{
				long arg = (long)firstUsableLba * (long)Config.PageSize;
				if (Mtk.DaLoader.Dacfg.FlashType == text3)
				{
					arg = 32768L;
					array = ReadFlash(0L, 32768L, parttype, arg4: false);
				}
				else
				{
					array = ReadFlash(0L, arg, parttype, arg4: false);
				}
				if (array.Length != 0)
				{
					if (gpt.Parse(array, Config.PageSize))
					{
						return (array, gpt);
					}
					return ((byte[])obj, obj);
				}
				return ((byte[])obj, obj);
			}
			return ((byte[])obj, obj);
		}
		Console.WriteLine(text2);
		(byte[], List<Partf>) tuple2 = ReadPMT(arg1: false, parttype);
		if (tuple2.Item1 != obj)
		{
			return (tuple2.Item1, tuple2.Item2);
		}
		return ((byte[])obj, obj);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
