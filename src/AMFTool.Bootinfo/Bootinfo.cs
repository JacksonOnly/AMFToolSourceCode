using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AMFTool.boot;
using Microsoft.VisualBasic.CompilerServices;

namespace AMFTool.Bootinfo;

public class Bootinfo
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct bootheader
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] magic;

		public int kernel_size;

		public uint kernel_addr;

		public int ramdisk_size;

		public uint ramdisk_addr;

		public uint second_size;

		public uint second_addr;

		public uint tags_addr;

		public int page_size;

		public uint dt_size;

		public int os_version;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] name;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		public byte[] cmdline;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] id4x8;
	}

	public static Dictionary<string, List<string>> build;

	public static void extract(byte[] bootBytes)
	{
		int num6 = default(int);
		string hex = default(string);
		int num7 = default(int);
		bool flag = default(bool);
		MemoryStream memoryStream = default(MemoryStream);
		string value = default(string);
		int num18 = default(int);
		bootheader bootheader = default(bootheader);
		int page_size = default(int);
		cpio_stream cpio_stream = default(cpio_stream);
		byte[] dst = default(byte[]);
		List<string> list = default(List<string>);
		byte[] array = default(byte[]);
		byte[] array2 = default(byte[]);
		cpio_stream cpio_stream2 = default(cpio_stream);
		bool flag2 = default(bool);
		int num33 = default(int);
		bool flag3 = default(bool);
		int num46 = default(int);
		int num47 = default(int);
		bool flag4 = default(bool);
		int num49 = default(int);
		int num50 = default(int);
		int num51 = default(int);
		int num52 = default(int);
		int count = default(int);
		long offset = default(long);
		long num55 = default(long);
		long num56 = default(long);
		bool flag5 = default(bool);
		BinaryReader binaryReader = default(BinaryReader);
		byte[] array3 = default(byte[]);
		bool flag6 = default(bool);
		string msg = default(string);
		string msg2 = default(string);
		string msg3 = default(string);
		string msg4 = default(string);
		string msg5 = default(string);
		string msg6 = default(string);
		bool flag7 = default(bool);
		string text = default(string);
		string text2 = default(string);
		byte[] array4 = default(byte[]);
		bool flag8 = default(bool);
		string key = default(string);
		string current2 = default(string);
		string msg7 = default(string);
		string text3 = default(string);
		string msg8 = default(string);
		string msg9 = default(string);
		string msg10 = default(string);
		string msg11 = default(string);
		string msg12 = default(string);
		string msg13 = default(string);
		string msg14 = default(string);
		string msg15 = default(string);
		string msg16 = default(string);
		string msg17 = default(string);
		string msg18 = default(string);
		string text4 = default(string);
		string oldValue = default(string);
		string newValue = default(string);
		string msg19 = default(string);
		string msg20 = default(string);
		bool flag9 = default(bool);
		bool flag10 = default(bool);
		bool flag11 = default(bool);
		bool flag12 = default(bool);
		bool flag13 = default(bool);
		bool flag14 = default(bool);
		bool flag15 = default(bool);
		bool flag16 = default(bool);
		bool flag17 = default(bool);
		bool flag18 = default(bool);
		bool flag19 = default(bool);
		bool flag20 = default(bool);
		bool flag21 = default(bool);
		bool flag22 = default(bool);
		bool flag23 = default(bool);
		bool flag24 = default(bool);
		bool flag25 = default(bool);
		bool flag26 = default(bool);
		bool flag27 = default(bool);
		bool flag28 = default(bool);
		bool flag29 = default(bool);
		bool flag30 = default(bool);
		bool flag31 = default(bool);
		bool flag32 = default(bool);
		bool flag33 = default(bool);
		bool flag34 = default(bool);
		bool flag35 = default(bool);
		bool flag36 = default(bool);
		bool flag37 = default(bool);
		bool flag38 = default(bool);
		bool flag39 = default(bool);
		bool flag40 = default(bool);
		bool flag41 = default(bool);
		bool flag42 = default(bool);
		bool flag43 = default(bool);
		bool flag44 = default(bool);
		bool flag45 = default(bool);
		bool flag46 = default(bool);
		bool flag47 = default(bool);
		bool flag48 = default(bool);
		bool flag49 = default(bool);
		bool flag50 = default(bool);
		bool flag51 = default(bool);
		bool flag52 = default(bool);
		bool flag53 = default(bool);
		bool flag54 = default(bool);
		string value2 = default(string);
		bool flag55 = default(bool);
		string value3 = default(string);
		string value4 = default(string);
		string value5 = default(string);
		string value6 = default(string);
		string value7 = default(string);
		string value8 = default(string);
		string value9 = default(string);
		string value10 = default(string);
		string value11 = default(string);
		string value12 = default(string);
		string value13 = default(string);
		string value14 = default(string);
		string value15 = default(string);
		string value16 = default(string);
		string value17 = default(string);
		string value18 = default(string);
		string value19 = default(string);
		string value20 = default(string);
		string value21 = default(string);
		string value22 = default(string);
		string value23 = default(string);
		string value24 = default(string);
		bool flag56 = default(bool);
		string text5 = default(string);
		string value25 = default(string);
		int count2 = default(int);
		while (true)
		{
			int num = 1437590553;
			while (true)
			{
				nint num2 = num ^ 0x6C3E46B7;
				nuint num3 = (nuint)num2;
				uint num5;
				int num4 = (int)((num5 = (uint)((int)num3 ^ 0x6C3E46B7 ^ 0x989D69B ^ 0x989D69B)) % 72);
				if (num4 == 67)
				{
					break;
				}
				if (num4 != 33 && num4 != 33)
				{
					if (num4 != 3 && num4 != 3)
					{
						if (num4 != 4 && num4 != 4)
						{
							if (num4 != 5 && num4 != 5)
							{
								if (num4 != 6 && num4 != 6)
								{
									if (num4 != 7 && num4 != 7)
									{
										if (num4 != 8 && num4 != 8)
										{
											if (num4 != 9 && num4 != 9)
											{
												if (num4 != 10 && num4 != 10)
												{
													if (num4 != 11 && num4 != 11)
													{
														if (num4 != 12 && num4 != 12)
														{
															if (num4 != 13 && num4 != 13)
															{
																if (num4 != 14 && num4 != 14)
																{
																	if (num4 != 15 && num4 != 15)
																	{
																		if (num4 != 16 && num4 != 16)
																		{
																			if (num4 != 17 && num4 != 17)
																			{
																				if (num4 != 18 && num4 != 18)
																				{
																					if (num4 != 19 && num4 != 19)
																					{
																						if (num4 != 20 && num4 != 20)
																						{
																							if (num4 != 21 && num4 != 21)
																							{
																								if (num4 != 22 && num4 != 22)
																								{
																									if (num4 != 23 && num4 != 23)
																									{
																										if (num4 != 24 && num4 != 24)
																										{
																											if (num4 != 25 && num4 != 25)
																											{
																												if (num4 != 26 && num4 != 26)
																												{
																													if (num4 != 27 && num4 != 27)
																													{
																														if (num4 != 28 && num4 != 28)
																														{
																															if (num4 != 29 && num4 != 29)
																															{
																																if (num4 != 30 && num4 != 30)
																																{
																																	if (num4 != 31 && num4 != 31)
																																	{
																																		if (num4 != 32 && num4 != 32)
																																		{
																																			if (num4 != 0 && num4 != 0)
																																			{
																																				if (num4 != 34 && num4 != 34)
																																				{
																																					if (num4 != 35 && num4 != 35)
																																					{
																																						if (num4 != 36 && num4 != 36)
																																						{
																																							if (num4 != 37 && num4 != 37)
																																							{
																																								if (num4 != 38 && num4 != 38)
																																								{
																																									if (num4 != 39 && num4 != 39)
																																									{
																																										if (num4 != 40 && num4 != 40)
																																										{
																																											if (num4 != 41 && num4 != 41)
																																											{
																																												if (num4 != 42 && num4 != 42)
																																												{
																																													if (num4 != 43 && num4 != 43)
																																													{
																																														if (num4 != 44 && num4 != 44)
																																														{
																																															if (num4 != 45 && num4 != 45)
																																															{
																																																if (num4 != 46 && num4 != 46)
																																																{
																																																	if (num4 != 47 && num4 != 47)
																																																	{
																																																		if (num4 != 48 && num4 != 48)
																																																		{
																																																			if (num4 != 49 && num4 != 49)
																																																			{
																																																				if (num4 != 50 && num4 != 50)
																																																				{
																																																					if (num4 != 51 && num4 != 51)
																																																					{
																																																						if (num4 != 52 && num4 != 52)
																																																						{
																																																							if (num4 != 53 && num4 != 53)
																																																							{
																																																								if (num4 != 54 && num4 != 54)
																																																								{
																																																									if (num4 != 55 && num4 != 55)
																																																									{
																																																										if (num4 != 56 && num4 != 56)
																																																										{
																																																											if (num4 != 57 && num4 != 57)
																																																											{
																																																												if (num4 != 58 && num4 != 58)
																																																												{
																																																													if (num4 != 59 && num4 != 59)
																																																													{
																																																														if (num4 != 60 && num4 != 60)
																																																														{
																																																															if (num4 != 61 && num4 != 61)
																																																															{
																																																																if (num4 != 62 && num4 != 62)
																																																																{
																																																																	if (num4 != 63 && num4 != 63)
																																																																	{
																																																																		if (num4 != 64 && num4 != 64)
																																																																		{
																																																																			if (num4 != 65 && num4 != 65)
																																																																			{
																																																																				if (num4 != 66 && num4 != 66)
																																																																				{
																																																																					if (num4 != 1 && num4 != 1)
																																																																					{
																																																																						if (num4 == 68 || num4 == 68)
																																																																						{
																																																																							num = (int)((num5 << 2) | (num5 >> 30)) ^ -1208303724;
																																																																							continue;
																																																																						}
																																																																						if (num4 == 69 || num4 == 69)
																																																																						{
																																																																							if ("".Length == 0)
																																																																							{
																																																																								num6 = find_binary(bootBytes, StringToByteArray(hex), num7);
																																																																							}
																																																																							num = (int)(0x172EF03 ^ num5);
																																																																							continue;
																																																																						}
																																																																						if (num4 == 70 || num4 == 70)
																																																																						{
																																																																							if ("".Length == 0 && (object)"" == "" && (object)"" == "" && (object)"" == "")
																																																																							{
																																																																								bootBytes = bootBytes.Skip(num6).Take(bootBytes.Length - num6).ToArray();
																																																																							}
																																																																							int num8 = (int)(num5 ^ 0x283C088B);
																																																																							num = (num8 + 1822391558 - 1822391558) ^ 0x283C088B ^ 0x40265171;
																																																																							continue;
																																																																						}
																																																																						if (num4 == 71 || num4 == 71)
																																																																						{
																																																																							if ((object)"" != "")
																																																																							{
																																																																							}
																																																																							num = (int)(0x6A22A0C5 ^ num5);
																																																																							continue;
																																																																						}
																																																																						if (num4 != 2)
																																																																						{
																																																																							goto IL_1175;
																																																																						}
																																																																					}
																																																																					else if ("".Length != 0 || flag)
																																																																					{
																																																																						int num9 = (int)num5 ^ -1520309599;
																																																																						num = (num9 + -1669375091 - -1669375091) ^ -1520309599 ^ 0x3818A95D;
																																																																						continue;
																																																																					}
																																																																					try
																																																																					{
																																																																						while (true)
																																																																						{
																																																																							int num10 = 579749266;
																																																																							while (true)
																																																																							{
																																																																								nint num11 = num10 ^ 0x7FD9A550;
																																																																								nuint num12 = (nuint)num11;
																																																																								int num13 = (int)((num5 = (uint)((int)num12 ^ 0x7FD9A550 ^ 0x49235488 ^ 0x49235488)) % 3);
																																																																								if (num13 == 2)
																																																																								{
																																																																									break;
																																																																								}
																																																																								if (num13 == 1 || num13 == 1 || num13 != 0)
																																																																								{
																																																																									if ("".Length == 0)
																																																																									{
																																																																										memoryStream = new MemoryStream(bootBytes);
																																																																									}
																																																																									num10 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x14827E6F);
																																																																									continue;
																																																																								}
																																																																								MemoryStream memoryStream2 = memoryStream;
																																																																								try
																																																																								{
																																																																									while (true)
																																																																									{
																																																																										int num14 = 2065175883;
																																																																										while (true)
																																																																										{
																																																																											nint num15 = num14 ^ -2014933628;
																																																																											nuint num16 = (nuint)num15;
																																																																											int num17 = (int)((num5 = (uint)((int)num16 ^ -2014933628 ^ 0x8B3C600 ^ 0x8B3C600)) % 75);
																																																																											if (num17 == 67)
																																																																											{
																																																																												break;
																																																																											}
																																																																											int num48;
																																																																											if (num17 != 33 && num17 != 33)
																																																																											{
																																																																												if (num17 != 3 && num17 != 3)
																																																																												{
																																																																													if (num17 != 4 && num17 != 4)
																																																																													{
																																																																														if (num17 != 5 && num17 != 5)
																																																																														{
																																																																															if (num17 != 6 && num17 != 6)
																																																																															{
																																																																																if (num17 != 7 && num17 != 7)
																																																																																{
																																																																																	if (num17 != 8 && num17 != 8)
																																																																																	{
																																																																																		if (num17 != 9 && num17 != 9)
																																																																																		{
																																																																																			if (num17 != 10 && num17 != 10)
																																																																																			{
																																																																																				if (num17 != 11 && num17 != 11)
																																																																																				{
																																																																																					if (num17 != 12 && num17 != 12)
																																																																																					{
																																																																																						if (num17 != 13 && num17 != 13)
																																																																																						{
																																																																																							if (num17 != 14 && num17 != 14)
																																																																																							{
																																																																																								if (num17 != 15 && num17 != 15)
																																																																																								{
																																																																																									if (num17 != 16 && num17 != 16)
																																																																																									{
																																																																																										if (num17 != 17 && num17 != 17)
																																																																																										{
																																																																																											if (num17 != 18 && num17 != 18)
																																																																																											{
																																																																																												if (num17 != 19 && num17 != 19)
																																																																																												{
																																																																																													if (num17 != 20 && num17 != 20)
																																																																																													{
																																																																																														if (num17 != 21 && num17 != 21)
																																																																																														{
																																																																																															if (num17 != 22 && num17 != 22)
																																																																																															{
																																																																																																if (num17 != 23 && num17 != 23)
																																																																																																{
																																																																																																	if (num17 == 24)
																																																																																																	{
																																																																																																		if ((object)"" != "")
																																																																																																		{
																																																																																																		}
																																																																																																		goto IL_2210;
																																																																																																	}
																																																																																																	if (num17 == 25 || num17 == 25)
																																																																																																	{
																																																																																																		value = Conversions.ToString((byte)num18 != 0);
																																																																																																		int num19 = (int)num5 ^ -24696749;
																																																																																																		num14 = (num19 + 1109639374 - 1109639374) ^ -24696749 ^ 0x40395CCB;
																																																																																																		continue;
																																																																																																	}
																																																																																																	if (num17 == 26 || num17 == 26)
																																																																																																	{
																																																																																																		num14 = (int)(0x158FDB45 ^ num5);
																																																																																																		continue;
																																																																																																	}
																																																																																																	if (num17 != 27)
																																																																																																	{
																																																																																																		if (num17 != 28 && num17 != 28)
																																																																																																		{
																																																																																																			if (num17 != 29 && num17 != 29)
																																																																																																			{
																																																																																																				if (num17 != 30 && num17 != 30)
																																																																																																				{
																																																																																																					if (num17 == 31 || num17 == 31)
																																																																																																					{
																																																																																																						if ((object)"" != "" || (object)"" != "")
																																																																																																						{
																																																																																																						}
																																																																																																						num14 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x5D4BA889);
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num17 == 32 || num17 == 32)
																																																																																																					{
																																																																																																						bootheader.page_size = page_size;
																																																																																																						num14 = (int)((num5 << 3) | (num5 >> 29)) ^ -110876833;
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num17 == 0 || num17 == 0)
																																																																																																					{
																																																																																																						if ((object)"" == "")
																																																																																																						{
																																																																																																							bootheader.ramdisk_size = (int)bootheader.kernel_addr;
																																																																																																						}
																																																																																																						num14 = (int)((num5 << 3) | (num5 >> 29)) ^ -1232528769;
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num17 == 34 || num17 == 34)
																																																																																																					{
																																																																																																						if ((object)"" != "" || "".Length != 0)
																																																																																																						{
																																																																																																						}
																																																																																																						num14 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x71622E07);
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num17 != 35)
																																																																																																					{
																																																																																																						if (num17 != 36 && num17 != 36)
																																																																																																						{
																																																																																																							if (num17 != 37 && num17 != 37)
																																																																																																							{
																																																																																																								if (num17 != 38 && num17 != 38)
																																																																																																								{
																																																																																																									if (num17 != 39)
																																																																																																									{
																																																																																																										if (num17 != 40 && num17 != 40)
																																																																																																										{
																																																																																																											if (num17 != 41 && num17 != 41)
																																																																																																											{
																																																																																																												if (num17 != 42 && num17 != 42)
																																																																																																												{
																																																																																																													if (num17 != 43 && num17 != 43)
																																																																																																													{
																																																																																																														if (num17 == 44 || num17 == 44)
																																																																																																														{
																																																																																																															if ((object)"" != "")
																																																																																																															{
																																																																																																															}
																																																																																																															num14 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x3C18D9EF);
																																																																																																															continue;
																																																																																																														}
																																																																																																														if (num17 == 45 || num17 == 45)
																																																																																																														{
																																																																																																															if ("".Length != 0 || (object)"" != "")
																																																																																																															{
																																																																																																																num14 = (int)((num5 << 3) | (num5 >> 29)) ^ -1062329293;
																																																																																																																continue;
																																																																																																															}
																																																																																																															goto IL_1d05;
																																																																																																														}
																																																																																																														if (num17 != 46)
																																																																																																														{
																																																																																																															if (num17 != 47 && num17 != 47)
																																																																																																															{
																																																																																																																if (num17 != 48 && num17 != 48)
																																																																																																																{
																																																																																																																	if (num17 == 49)
																																																																																																																	{
																																																																																																																		if ("".Length == 0 && (object)"" != "")
																																																																																																																		{
																																																																																																																		}
																																																																																																																		goto IL_1d05;
																																																																																																																	}
																																																																																																																	if (num17 == 50 || num17 == 50)
																																																																																																																	{
																																																																																																																		if ("".Length != 0 || "".Length != 0)
																																																																																																																		{
																																																																																																																			int num20 = (int)(num5 ^ 0x394F411);
																																																																																																																			num14 = (num20 + -364270032 - -364270032) ^ 0x394F411 ^ 0x70D63EB5;
																																																																																																																			continue;
																																																																																																																		}
																																																																																																																		goto IL_1cdd;
																																																																																																																	}
																																																																																																																	if (num17 != 51)
																																																																																																																	{
																																																																																																																		if (num17 != 52 && num17 != 52)
																																																																																																																		{
																																																																																																																			if (num17 != 53 && num17 != 53)
																																																																																																																			{
																																																																																																																				if (num17 != 54 && num17 != 54)
																																																																																																																				{
																																																																																																																					if (num17 != 55 && num17 != 55)
																																																																																																																					{
																																																																																																																						if (num17 != 56 && num17 != 56)
																																																																																																																						{
																																																																																																																							if (num17 != 57 && num17 != 57)
																																																																																																																							{
																																																																																																																								if (num17 != 58 && num17 != 58)
																																																																																																																								{
																																																																																																																									if (num17 != 59 && num17 != 59)
																																																																																																																									{
																																																																																																																										if (num17 != 60 && num17 != 60)
																																																																																																																										{
																																																																																																																											if (num17 != 61)
																																																																																																																											{
																																																																																																																												if (num17 != 62 && num17 != 62)
																																																																																																																												{
																																																																																																																													if (num17 != 63 && num17 != 63)
																																																																																																																													{
																																																																																																																														if (num17 != 64 && num17 != 64)
																																																																																																																														{
																																																																																																																															if (num17 != 65 && num17 != 65)
																																																																																																																															{
																																																																																																																																if (num17 != 66 && num17 != 66)
																																																																																																																																{
																																																																																																																																	if (num17 != 1 && num17 != 1)
																																																																																																																																	{
																																																																																																																																		if (num17 == 68 || num17 == 68)
																																																																																																																																		{
																																																																																																																																			if ("".Length != 0 || (object)"" != "")
																																																																																																																																			{
																																																																																																																																			}
																																																																																																																																			int num21 = (int)num5 ^ -682982902;
																																																																																																																																			num14 = (num21 + -117722732 - -117722732) ^ -682982902 ^ 0x373233B4;
																																																																																																																																			continue;
																																																																																																																																		}
																																																																																																																																		if (num17 == 69 || num17 == 69)
																																																																																																																																		{
																																																																																																																																			cpio_stream = new cpio_stream(dst);
																																																																																																																																			num14 = (int)(0x51E8D7AD ^ num5);
																																																																																																																																			continue;
																																																																																																																																		}
																																																																																																																																		if (num17 == 70 || num17 == 70)
																																																																																																																																		{
																																																																																																																																			list = cpio_stream.parsecpio();
																																																																																																																																			num14 = (int)((num5 << 3) | (num5 >> 29)) ^ -1507722578;
																																																																																																																																			continue;
																																																																																																																																		}
																																																																																																																																		if (num17 == 71 || num17 == 71)
																																																																																																																																		{
																																																																																																																																			if ((object)"" != "" || "".Length != 0)
																																																																																																																																			{
																																																																																																																																			}
																																																																																																																																			num14 = (int)((num5 << 2) | (num5 >> 30)) ^ -800140556;
																																																																																																																																			continue;
																																																																																																																																		}
																																																																																																																																		if (num17 != 72)
																																																																																																																																		{
																																																																																																																																			if (num17 != 73 && num17 != 73)
																																																																																																																																			{
																																																																																																																																				if (num17 != 74)
																																																																																																																																				{
																																																																																																																																					if (num17 != 2)
																																																																																																																																					{
																																																																																																																																						goto IL_22ae;
																																																																																																																																					}
																																																																																																																																					GZipStream gZipStream = new GZipStream(new MemoryStream(array), (CompressionMode)num7);
																																																																																																																																					try
																																																																																																																																					{
																																																																																																																																						while (true)
																																																																																																																																						{
																																																																																																																																							int num22 = 284922146;
																																																																																																																																							while (true)
																																																																																																																																							{
																																																																																																																																								nint num23 = num22 ^ -1028771665;
																																																																																																																																								nuint num24 = (nuint)num23;
																																																																																																																																								int num25 = (int)((num5 = (uint)((int)num24 ^ -1028771665 ^ 0x5A0B5D77 ^ 0x5A0B5D77)) % 4);
																																																																																																																																								if (num25 == 0)
																																																																																																																																								{
																																																																																																																																									break;
																																																																																																																																								}
																																																																																																																																								if (num25 != 2 && num25 != 2)
																																																																																																																																								{
																																																																																																																																									if (num25 == 3 || num25 == 3)
																																																																																																																																									{
																																																																																																																																										if ((object)"" == "" && "".Length == 0)
																																																																																																																																										{
																																																																																																																																											array2 = new byte[array.Length];
																																																																																																																																										}
																																																																																																																																										num22 = (int)(0x2AAE15BE ^ num5);
																																																																																																																																										continue;
																																																																																																																																									}
																																																																																																																																									if (num25 == 1)
																																																																																																																																									{
																																																																																																																																										MemoryStream memoryStream3 = new MemoryStream();
																																																																																																																																										try
																																																																																																																																										{
																																																																																																																																											while (true)
																																																																																																																																											{
																																																																																																																																												int num26 = 1268829845;
																																																																																																																																												while (true)
																																																																																																																																												{
																																																																																																																																													nint num27 = num26 ^ -971930594;
																																																																																																																																													nuint num28 = (nuint)num27;
																																																																																																																																													int num29 = (int)((num5 = (uint)((int)num28 ^ -971930594 ^ 0x9035138 ^ 0x9035138)) % 17);
																																																																																																																																													if (num29 == 0)
																																																																																																																																													{
																																																																																																																																														break;
																																																																																																																																													}
																																																																																																																																													if (num29 != 12 && num29 != 12)
																																																																																																																																													{
																																																																																																																																														if (num29 != 7)
																																																																																																																																														{
																																																																																																																																															if (num29 != 10 && num29 != 10)
																																																																																																																																															{
																																																																																																																																																if (num29 != 1 && num29 != 1)
																																																																																																																																																{
																																																																																																																																																	if (num29 != 15 && num29 != 15)
																																																																																																																																																	{
																																																																																																																																																		if (num29 != 2 && num29 != 2)
																																																																																																																																																		{
																																																																																																																																																			if (num29 != 8 && num29 != 8)
																																																																																																																																																			{
																																																																																																																																																				if (num29 == 9)
																																																																																																																																																				{
																																																																																																																																																					if ("".Length == 0 && "".Length == 0)
																																																																																																																																																					{
																																																																																																																																																						goto IL_25d5;
																																																																																																																																																					}
																																																																																																																																																					goto IL_25e2;
																																																																																																																																																				}
																																																																																																																																																				if (num29 == 3 || num29 == 3)
																																																																																																																																																				{
																																																																																																																																																					num26 = (int)((num5 << 3) | (num5 >> 29)) ^ -1824263352;
																																																																																																																																																					continue;
																																																																																																																																																				}
																																																																																																																																																				if (num29 == 11 || num29 == 11)
																																																																																																																																																				{
																																																																																																																																																					if ((object)"" != "" || "".Length != 0)
																																																																																																																																																					{
																																																																																																																																																					}
																																																																																																																																																					num26 = (int)(0xB77BE64 ^ num5);
																																																																																																																																																					continue;
																																																																																																																																																				}
																																																																																																																																																				if (num29 == 4)
																																																																																																																																																				{
																																																																																																																																																					goto IL_26b3;
																																																																																																																																																				}
																																																																																																																																																				if (num29 == 13 || num29 == 13)
																																																																																																																																																				{
																																																																																																																																																					if ((object)"" != "")
																																																																																																																																																					{
																																																																																																																																																						num26 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x6301A8B9);
																																																																																																																																																						continue;
																																																																																																																																																					}
																																																																																																																																																					goto IL_269e;
																																																																																																																																																				}
																																																																																																																																																				if (num29 != 14)
																																																																																																																																																				{
																																																																																																																																																					if (num29 != 5 && num29 != 5)
																																																																																																																																																					{
																																																																																																																																																						if (num29 != 16 && num29 != 16)
																																																																																																																																																						{
																																																																																																																																																							if (num29 != 6)
																																																																																																																																																							{
																																																																																																																																																								goto IL_26a5;
																																																																																																																																																							}
																																																																																																																																																							goto end_IL_26ff;
																																																																																																																																																						}
																																																																																																																																																						num26 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x66B1311E);
																																																																																																																																																						continue;
																																																																																																																																																					}
																																																																																																																																																					list = cpio_stream2.parsecpio();
																																																																																																																																																					int num30 = (int)num5 ^ -852338047;
																																																																																																																																																					num26 = (num30 + -2061429975 - -2061429975) ^ -852338047 ^ 0x539D840A;
																																																																																																																																																					continue;
																																																																																																																																																				}
																																																																																																																																																			}
																																																																																																																																																			else if ("".Length != 0)
																																																																																																																																																			{
																																																																																																																																																				num26 = (int)((num5 << 3) | (num5 >> 29)) ^ -942555103;
																																																																																																																																																				continue;
																																																																																																																																																			}
																																																																																																																																																			cpio_stream2 = new cpio_stream(memoryStream3.ToArray());
																																																																																																																																																			num26 = 280281045;
																																																																																																																																																		}
																																																																																																																																																		else
																																																																																																																																																		{
																																																																																																																																																			if ("".Length != 0)
																																																																																																																																																			{
																																																																																																																																																			}
																																																																																																																																																			int num31 = (int)num5 ^ -464710923;
																																																																																																																																																			num26 = (num31 + -420668215 - -420668215) ^ -464710923 ^ 0x7F509750;
																																																																																																																																																		}
																																																																																																																																																		continue;
																																																																																																																																																	}
																																																																																																																																																	if ((object)"" != "" || flag2)
																																																																																																																																																	{
																																																																																																																																																		int num32 = (int)num5 ^ -734891549;
																																																																																																																																																		num26 = (num32 + 369216745 - 369216745) ^ -734891549 ^ 0x33D11C5B;
																																																																																																																																																		continue;
																																																																																																																																																	}
																																																																																																																																																	goto IL_25d5;
																																																																																																																																																}
																																																																																																																																																if ((object)"" == "" && (object)"" == "")
																																																																																																																																																{
																																																																																																																																																	flag2 = ((num33 > num7) ? 1 : 0) == num7;
																																																																																																																																																}
																																																																																																																																																int num34 = (int)num5 ^ -1505494181;
																																																																																																																																																num26 = (num34 + 173289373 - 173289373) ^ -1505494181 ^ 0x663C9C4D;
																																																																																																																																																continue;
																																																																																																																																															}
																																																																																																																																															if ("".Length == 0)
																																																																																																																																															{
																																																																																																																																																num33 = gZipStream.Read(array2, num7, array2.Length);
																																																																																																																																															}
																																																																																																																																															num26 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1F0FE3FB);
																																																																																																																																															continue;
																																																																																																																																														}
																																																																																																																																														if ("".Length == 0 && "".Length == 0)
																																																																																																																																														{
																																																																																																																																														}
																																																																																																																																														goto IL_269e;
																																																																																																																																													}
																																																																																																																																													goto IL_26a5;
																																																																																																																																													IL_269e:
																																																																																																																																													num26 = 47748029;
																																																																																																																																													continue;
																																																																																																																																													IL_25d5:
																																																																																																																																													memoryStream3.Write(array2, num7, num33);
																																																																																																																																													goto IL_25e2;
																																																																																																																																													IL_25e2:
																																																																																																																																													num26 = 1410298090;
																																																																																																																																													continue;
																																																																																																																																													IL_26a5:
																																																																																																																																													if ((object)"" != "")
																																																																																																																																													{
																																																																																																																																														num26 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x3E3CEF48);
																																																																																																																																														continue;
																																																																																																																																													}
																																																																																																																																													goto IL_26b3;
																																																																																																																																													IL_26b3:
																																																																																																																																													num26 = 1865749639;
																																																																																																																																												}
																																																																																																																																												continue;
																																																																																																																																												end_IL_26ff:
																																																																																																																																												break;
																																																																																																																																											}
																																																																																																																																										}
																																																																																																																																										finally
																																																																																																																																										{
																																																																																																																																											if (memoryStream3 != null)
																																																																																																																																											{
																																																																																																																																												while (true)
																																																																																																																																												{
																																																																																																																																													int num35 = 2031122442;
																																																																																																																																													while (true)
																																																																																																																																													{
																																																																																																																																														nint num36 = num35 ^ 0x5A349C48;
																																																																																																																																														nuint num37 = (nuint)num36;
																																																																																																																																														int num38 = (int)((num5 = (uint)((int)num37 ^ 0x5A349C48 ^ -853779395 ^ -853779395)) % 4);
																																																																																																																																														if (num38 == 0)
																																																																																																																																														{
																																																																																																																																															break;
																																																																																																																																														}
																																																																																																																																														if (num38 != 2 && num38 != 2)
																																																																																																																																														{
																																																																																																																																															if (num38 == 3 || num38 == 3)
																																																																																																																																															{
																																																																																																																																																if ((object)"" != "" || (object)"" != "")
																																																																																																																																																{
																																																																																																																																																}
																																																																																																																																																int num39 = (int)(num5 ^ 0x774F22A9);
																																																																																																																																																num35 = (num39 + -1623175643 - -1623175643) ^ 0x774F22A9 ^ 0x7F6C012A;
																																																																																																																																																continue;
																																																																																																																																															}
																																																																																																																																															if (num38 == 1)
																																																																																																																																															{
																																																																																																																																																goto end_IL_27ec;
																																																																																																																																															}
																																																																																																																																														}
																																																																																																																																														if ((object)"" == "" && (object)"" == "" && (object)"" == "")
																																																																																																																																														{
																																																																																																																																															((IDisposable)memoryStream3).Dispose();
																																																																																																																																														}
																																																																																																																																														num35 = (int)((num5 << 2) | (num5 >> 30)) ^ -220303502;
																																																																																																																																													}
																																																																																																																																													continue;
																																																																																																																																													end_IL_27ec:
																																																																																																																																													break;
																																																																																																																																												}
																																																																																																																																											}
																																																																																																																																										}
																																																																																																																																										goto end_IL_23b3;
																																																																																																																																									}
																																																																																																																																								}
																																																																																																																																								if ("".Length == 0 && "".Length == 0)
																																																																																																																																								{
																																																																																																																																									num33 = num7;
																																																																																																																																								}
																																																																																																																																								num22 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x3D5482E3);
																																																																																																																																							}
																																																																																																																																							continue;
																																																																																																																																							end_IL_23b3:
																																																																																																																																							break;
																																																																																																																																						}
																																																																																																																																					}
																																																																																																																																					finally
																																																																																																																																					{
																																																																																																																																						if (gZipStream != null)
																																																																																																																																						{
																																																																																																																																							while (true)
																																																																																																																																							{
																																																																																																																																								int num40 = 854483482;
																																																																																																																																								while (true)
																																																																																																																																								{
																																																																																																																																									nint num41 = num40 ^ -982210742;
																																																																																																																																									nuint num42 = (nuint)num41;
																																																																																																																																									int num43 = (int)((num5 = (uint)((int)num42 ^ -982210742 ^ -1343265288 ^ -1343265288)) % 4);
																																																																																																																																									if (num43 == 0)
																																																																																																																																									{
																																																																																																																																										break;
																																																																																																																																									}
																																																																																																																																									if (num43 != 2 && num43 != 2)
																																																																																																																																									{
																																																																																																																																										if (num43 == 3 || num43 == 3)
																																																																																																																																										{
																																																																																																																																											if ((object)"" != "" || "".Length != 0)
																																																																																																																																											{
																																																																																																																																											}
																																																																																																																																											int num44 = (int)num5 ^ -492354541;
																																																																																																																																											num40 = (num44 + 916218220 - 916218220) ^ -492354541 ^ 0x1C80F03E;
																																																																																																																																											continue;
																																																																																																																																										}
																																																																																																																																										if (num43 == 1)
																																																																																																																																										{
																																																																																																																																											goto end_IL_28aa;
																																																																																																																																										}
																																																																																																																																									}
																																																																																																																																									((IDisposable)gZipStream).Dispose();
																																																																																																																																									num40 = (int)((num5 << 3) | (num5 >> 29)) ^ -1591203778;
																																																																																																																																								}
																																																																																																																																								continue;
																																																																																																																																								end_IL_28aa:
																																																																																																																																								break;
																																																																																																																																							}
																																																																																																																																						}
																																																																																																																																					}
																																																																																																																																					goto IL_28b2;
																																																																																																																																				}
																																																																																																																																				if ((object)"" == "" && (object)"" != "")
																																																																																																																																				{
																																																																																																																																				}
																																																																																																																																				goto IL_18a0;
																																																																																																																																			}
																																																																																																																																			goto IL_29ad;
																																																																																																																																		}
																																																																																																																																		if ("".Length != 0)
																																																																																																																																		{
																																																																																																																																		}
																																																																																																																																	}
																																																																																																																																	else if (flag3)
																																																																																																																																	{
																																																																																																																																		int num45 = (int)(num5 ^ 0x562BD119);
																																																																																																																																		num14 = (num45 + -1122425857 - -1122425857) ^ 0x562BD119 ^ 0x485EEF62;
																																																																																																																																		continue;
																																																																																																																																	}
																																																																																																																																	num14 = 1476135373;
																																																																																																																																}
																																																																																																																																else
																																																																																																																																{
																																																																																																																																	flag3 = num46 > num7;
																																																																																																																																	num14 = (int)((num5 << 3) | (num5 >> 29)) ^ -652455973;
																																																																																																																																}
																																																																																																																															}
																																																																																																																															else
																																																																																																																															{
																																																																																																																																num46 = Lz4Decompresso.lz4_hadikit_decompress(array, ref dst);
																																																																																																																																num14 = (int)(0x7FFAE759 ^ num5);
																																																																																																																															}
																																																																																																																														}
																																																																																																																														else
																																																																																																																														{
																																																																																																																															if ("".Length == 0)
																																																																																																																															{
																																																																																																																																dst = new byte[num47];
																																																																																																																															}
																																																																																																																															num14 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x2AE65FF8);
																																																																																																																														}
																																																																																																																													}
																																																																																																																													else
																																																																																																																													{
																																																																																																																														if ("".Length != 0 || "".Length != 0)
																																																																																																																														{
																																																																																																																														}
																																																																																																																														num14 = (int)(0x21957A0E ^ num5);
																																																																																																																													}
																																																																																																																													continue;
																																																																																																																												}
																																																																																																																												if (flag4)
																																																																																																																												{
																																																																																																																													num14 = (int)((num5 << 2) | (num5 >> 30)) ^ -1827446492;
																																																																																																																													continue;
																																																																																																																												}
																																																																																																																												goto IL_18a0;
																																																																																																																											}
																																																																																																																											num48 = ((((array[num49] == num50) ? 1 : 0) == num7) ? 1 : 0);
																																																																																																																											goto IL_18ff;
																																																																																																																										}
																																																																																																																										if ("".Length != 0 || array[num18] != num51)
																																																																																																																										{
																																																																																																																											num14 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x4FE07B2);
																																																																																																																											continue;
																																																																																																																										}
																																																																																																																									}
																																																																																																																									else if (array[num7] != num52)
																																																																																																																									{
																																																																																																																										num14 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0xA0E1F1);
																																																																																																																										continue;
																																																																																																																									}
																																																																																																																									num48 = num7;
																																																																																																																									goto IL_18ff;
																																																																																																																								}
																																																																																																																								if ("".Length == 0 && "".Length == 0)
																																																																																																																								{
																																																																																																																									list = new List<string>();
																																																																																																																								}
																																																																																																																								num14 = (int)(0x88D4D52 ^ num5);
																																																																																																																								continue;
																																																																																																																							}
																																																																																																																							if ("".Length == 0)
																																																																																																																							{
																																																																																																																								array.Take(count).ToArray();
																																																																																																																							}
																																																																																																																							num14 = (int)(0x63B7FF ^ num5);
																																																																																																																							continue;
																																																																																																																						}
																																																																																																																						memoryStream.Read(array, num7, bootheader.ramdisk_size);
																																																																																																																						num14 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x468752DC);
																																																																																																																						continue;
																																																																																																																					}
																																																																																																																					if ("".Length == 0 && "".Length == 0)
																																																																																																																					{
																																																																																																																						array = new byte[bootheader.ramdisk_size];
																																																																																																																					}
																																																																																																																					int num53 = (int)num5 ^ -834317924;
																																																																																																																					num14 = (num53 + -1400028166 - -1400028166) ^ -834317924 ^ 0x1000112C;
																																																																																																																					continue;
																																																																																																																				}
																																																																																																																				if ((object)"" == "")
																																																																																																																				{
																																																																																																																					memoryStream.Seek(offset, (SeekOrigin)num7);
																																																																																																																				}
																																																																																																																				int num54 = (int)num5 ^ -1733784504;
																																																																																																																				num14 = (num54 + -1917617152 - -1917617152) ^ -1733784504 ^ 0x5929D06A;
																																																																																																																				continue;
																																																																																																																			}
																																																																																																																			if ((object)"" == "" && "".Length == 0 && (object)"" == "")
																																																																																																																			{
																																																																																																																				offset = bootheader.page_size * (num55 + num56);
																																																																																																																			}
																																																																																																																			int num57 = (int)num5 ^ -556879853;
																																																																																																																			num14 = (num57 + 523900421 - 523900421) ^ -556879853 ^ 0x548A2FCF;
																																																																																																																			continue;
																																																																																																																		}
																																																																																																																		num56 = get_number_of_pages(bootheader.kernel_size, bootheader.page_size);
																																																																																																																		int num58 = (int)num5 ^ -329356372;
																																																																																																																		num14 = (num58 + -2054006546 - -2054006546) ^ -329356372 ^ 0x6EF8EB54;
																																																																																																																		continue;
																																																																																																																	}
																																																																																																																	if ((object)"" != "")
																																																																																																																	{
																																																																																																																		goto IL_1b22;
																																																																																																																	}
																																																																																																																}
																																																																																																																else if ((object)"" != "")
																																																																																																																{
																																																																																																																	num14 = (int)(0x62145328 ^ num5);
																																																																																																																	continue;
																																																																																																																}
																																																																																																																num55 = num18;
																																																																																																																goto IL_1b22;
																																																																																																															}
																																																																																																															_ = memoryStream.Position;
																																																																																																															num14 = (int)((num5 << 2) | (num5 >> 30)) ^ -1324413736;
																																																																																																															continue;
																																																																																																														}
																																																																																																														if ("".Length != 0)
																																																																																																														{
																																																																																																															goto IL_1bec;
																																																																																																														}
																																																																																																													}
																																																																																																													else if ((object)"" != "" || (object)"" != "" || flag5)
																																																																																																													{
																																																																																																														int num59 = (int)num5 ^ -1492401787;
																																																																																																														num14 = (num59 + 1903778462 - 1903778462) ^ -1492401787 ^ 0xC64FE70;
																																																																																																														continue;
																																																																																																													}
																																																																																																													binaryReader.BaseStream.Seek(-bootheader.page_size, (SeekOrigin)num18);
																																																																																																													goto IL_1bec;
																																																																																																												}
																																																																																																												flag5 = ((ReadOnlySpan<byte>)array3).SequenceEqual((ReadOnlySpan<byte>)new byte[bootheader.page_size]);
																																																																																																												num14 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x8F2E540);
																																																																																																												continue;
																																																																																																											}
																																																																																																											memoryStream.Read(array3, num7, array3.Length);
																																																																																																											int num60 = (int)num5 ^ -1444691930;
																																																																																																											num14 = (num60 + 797761964 - 797761964) ^ -1444691930 ^ 0x7FD37FBE;
																																																																																																											continue;
																																																																																																										}
																																																																																																										if ("".Length == 0 && "".Length == 0)
																																																																																																										{
																																																																																																											array3 = binaryReader.ReadBytes(bootheader.page_size);
																																																																																																										}
																																																																																																										int num61 = (int)num5 ^ -1725674376;
																																																																																																										num14 = (num61 + -1130340880 - -1130340880) ^ -1725674376 ^ 0x49289AC9;
																																																																																																										continue;
																																																																																																									}
																																																																																																									if ("".Length == 0)
																																																																																																									{
																																																																																																									}
																																																																																																									goto IL_1cdd;
																																																																																																								}
																																																																																																								if ("".Length != 0 || (object)"" != "" || "".Length != 0)
																																																																																																								{
																																																																																																									num14 = (int)(0x293AC12D ^ num5);
																																																																																																									continue;
																																																																																																								}
																																																																																																								goto IL_1d05;
																																																																																																							}
																																																																																																							if ((object)"" != "" || "".Length != 0 || (object)"" != "")
																																																																																																							{
																																																																																																							}
																																																																																																							num14 = (int)(0x531CF17B ^ num5);
																																																																																																							continue;
																																																																																																						}
																																																																																																						memoryStream.Seek(bootheader.page_size - Marshal.SizeOf(typeof(bootheader)), (SeekOrigin)num18);
																																																																																																						num14 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x1774C91C);
																																																																																																						continue;
																																																																																																					}
																																																																																																				}
																																																																																																				else if (flag6)
																																																																																																				{
																																																																																																					num14 = (int)(0x7F087408 ^ num5);
																																																																																																					continue;
																																																																																																				}
																																																																																																				num14 = 288347811;
																																																																																																			}
																																																																																																			else
																																																																																																			{
																																																																																																				if ((object)"" == "" && (object)"" == "" && "".Length == 0)
																																																																																																				{
																																																																																																					flag6 = ((bootheader.page_size > num7) ? 1 : 0) == num7;
																																																																																																				}
																																																																																																				num14 = (int)(0x6534C7BB ^ num5);
																																																																																																			}
																																																																																																		}
																																																																																																		else
																																																																																																		{
																																																																																																			bootheader = unpack_reader<bootheader>(binaryReader);
																																																																																																			num14 = (int)(0x3471E72C ^ num5);
																																																																																																		}
																																																																																																		continue;
																																																																																																	}
																																																																																																	if ((object)"" != "" || (object)"" != "")
																																																																																																	{
																																																																																																		goto IL_1f3a;
																																																																																																	}
																																																																																																}
																																																																																																else if ("".Length != 0 || (object)"" != "")
																																																																																																{
																																																																																																	int num62 = (int)(num5 ^ 0xCC3523B);
																																																																																																	num14 = (num62 + 1885315065 - 1885315065) ^ 0xCC3523B ^ 0x23477B40;
																																																																																																	continue;
																																																																																																}
																																																																																																binaryReader = new BinaryReader(new MemoryStream(bootBytes));
																																																																																																goto IL_1f3a;
																																																																																															}
																																																																																															int num63 = (int)num5 ^ -1161429537;
																																																																																															num14 = (num63 + 1559610037 - 1559610037) ^ -1161429537 ^ 0x47CEF39C;
																																																																																															continue;
																																																																																														}
																																																																																														if ((object)"" != "")
																																																																																														{
																																																																																														}
																																																																																														int num64 = (int)num5 ^ -753090206;
																																																																																														num14 = (num64 + 2000490166 - 2000490166) ^ -753090206 ^ 0x54633127;
																																																																																														continue;
																																																																																													}
																																																																																													if ((object)"" == "")
																																																																																													{
																																																																																														MTKConfig.Infodevice = (byte)num18 != 0;
																																																																																													}
																																																																																													num14 = (int)(0x14E93467 ^ num5);
																																																																																													continue;
																																																																																												}
																																																																																												if ((object)"" != "")
																																																																																												{
																																																																																												}
																																																																																												int num65 = (int)(num5 ^ 0x2CEB0BBF);
																																																																																												num14 = (num65 + -2092381996 - -2092381996) ^ 0x2CEB0BBF ^ 0x3B70C931;
																																																																																												continue;
																																																																																											}
																																																																																											if ((object)"" == "")
																																																																																											{
																																																																																												UI.RichLogs(msg, UI.log, (byte)num18 != 0, (byte)num7 != 0);
																																																																																											}
																																																																																											num14 = (int)(0x3B306A30 ^ num5);
																																																																																											continue;
																																																																																										}
																																																																																										num14 = (int)(0x3AC8A295 ^ num5);
																																																																																										continue;
																																																																																									}
																																																																																									if ("".Length == 0)
																																																																																									{
																																																																																										UI.RichLogs(msg2, UI.error, (byte)num7 != 0, (byte)num18 != 0);
																																																																																									}
																																																																																									num14 = (int)((num5 << 2) | (num5 >> 30)) ^ -1240528387;
																																																																																									continue;
																																																																																								}
																																																																																								if ("".Length != 0 || (object)"" != "")
																																																																																								{
																																																																																								}
																																																																																								num14 = (int)((num5 << 3) | (num5 >> 29)) ^ -692065271;
																																																																																								continue;
																																																																																							}
																																																																																							UI.RichLogs(msg3, UI.error, (byte)num7 != 0, (byte)num18 != 0);
																																																																																							num14 = (int)(0xFBC2E54 ^ num5);
																																																																																							continue;
																																																																																						}
																																																																																						if ("".Length != 0)
																																																																																						{
																																																																																						}
																																																																																						num14 = (int)(0x33F52BA ^ num5);
																																																																																						continue;
																																																																																					}
																																																																																					if ((object)"" == "" && "".Length == 0 && (object)"" == "")
																																																																																					{
																																																																																						UI.RichLogs(msg4, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																					}
																																																																																					num14 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0xBF2451B);
																																																																																					continue;
																																																																																				}
																																																																																				if ((object)"" != "" || "".Length != 0)
																																																																																				{
																																																																																				}
																																																																																				num14 = (int)(0x661B4E8E ^ num5);
																																																																																				continue;
																																																																																			}
																																																																																			UI.RichLogs(msg5, UI.ok, (byte)num7 != 0, (byte)num7 != 0);
																																																																																			num14 = (int)((num5 << 2) | (num5 >> 30)) ^ -873381962;
																																																																																			continue;
																																																																																		}
																																																																																		num14 = (int)(0x67DED9D4 ^ num5);
																																																																																		continue;
																																																																																	}
																																																																																	UI.RichLogs(msg6, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																	int num66 = (int)(num5 ^ 0x2C8DE59B);
																																																																																	num14 = (num66 + -278522616 - -278522616) ^ 0x2C8DE59B ^ 0x7A94EDA5;
																																																																																	continue;
																																																																																}
																																																																																if ((object)"" != "")
																																																																																{
																																																																																}
																																																																																int num67 = (int)num5 ^ -1022481996;
																																																																																num14 = (num67 + 1387636104 - 1387636104) ^ -1022481996 ^ 0x2774F9A0;
																																																																																continue;
																																																																															}
																																																																															if ("".Length != 0 || (object)"" != "" || flag7)
																																																																															{
																																																																																num14 = (int)((num5 << 2) | (num5 >> 30)) ^ -1104713118;
																																																																																continue;
																																																																															}
																																																																															goto IL_2210;
																																																																														}
																																																																														if ((object)"" == "")
																																																																														{
																																																																															flag7 = text != text2;
																																																																														}
																																																																														num14 = (int)(0xFED2526 ^ num5);
																																																																														continue;
																																																																													}
																																																																													if ("".Length == 0 && "".Length == 0)
																																																																													{
																																																																														text = Encoding.ASCII.GetString(array4);
																																																																													}
																																																																													num14 = (int)(0x1C6134FA ^ num5);
																																																																													continue;
																																																																												}
																																																																												memoryStream.Read(array4, num7, num50);
																																																																												int num68 = (int)(num5 ^ 0x23EDD4F9);
																																																																												num14 = (num68 + -101298425 - -101298425) ^ 0x23EDD4F9 ^ 0x2243FB56;
																																																																												continue;
																																																																											}
																																																																											goto IL_22ae;
																																																																											IL_2210:
																																																																											num14 = 1926865750;
																																																																											continue;
																																																																											IL_22ae:
																																																																											array4 = new byte[num50];
																																																																											num14 = (int)(0x3043EEBD ^ num5);
																																																																											continue;
																																																																											IL_1b22:
																																																																											num14 = 1947601927;
																																																																											continue;
																																																																											IL_295c:
																																																																											int num70;
																																																																											while (true)
																																																																											{
																																																																												nint num69 = num70 ^ 0x4DC9D7AB;
																																																																												nuint num71 = (nuint)num69;
																																																																												int num72 = (int)((num5 = (uint)((int)num71 ^ 0x4DC9D7AB ^ -1289897216 ^ -1289897216)) % 6);
																																																																												if (num72 == 0)
																																																																												{
																																																																													break;
																																																																												}
																																																																												if (num72 != 5)
																																																																												{
																																																																													if (num72 == 3 || num72 == 3)
																																																																													{
																																																																														if ("".Length != 0 || flag8)
																																																																														{
																																																																															num70 = (int)((num5 << 3) | (num5 >> 29)) ^ -450844712;
																																																																															continue;
																																																																														}
																																																																														goto IL_75c0;
																																																																													}
																																																																													if (num72 == 4 || num72 == 4)
																																																																													{
																																																																														if ((object)"" != "")
																																																																														{
																																																																														}
																																																																														num70 = (int)(0x1F4EF73D ^ num5);
																																																																														continue;
																																																																													}
																																																																													if (num72 == 1 || num72 == 1)
																																																																													{
																																																																														if ("".Length != 0 || "".Length != 0 || "".Length != 0)
																																																																														{
																																																																														}
																																																																														int num73 = (int)(num5 ^ 0x72054A48);
																																																																														num70 = (num73 + -197780408 - -197780408) ^ 0x72054A48 ^ 0x7DE276A7;
																																																																														continue;
																																																																													}
																																																																													if (num72 == 2)
																																																																													{
																																																																														Dictionary<string, List<string>>.Enumerator enumerator = build.GetEnumerator();
																																																																														try
																																																																														{
																																																																															while (true)
																																																																															{
																																																																																if (enumerator.MoveNext())
																																																																																{
																																																																																	KeyValuePair<string, List<string>> current;
																																																																																	int num77;
																																																																																	do
																																																																																	{
																																																																																		IL_2ad3:
																																																																																		current = enumerator.Current;
																																																																																		int num74 = 1622942061;
																																																																																		while (true)
																																																																																		{
																																																																																			nint num75 = num74 ^ 0xC16C6B2;
																																																																																			nuint num76 = (nuint)num75;
																																																																																			num77 = (int)((num5 = (uint)((int)num76 ^ 0xC16C6B2 ^ 0x5D71F85B ^ 0x5D71F85B)) % 6);
																																																																																			if (num77 == 0)
																																																																																			{
																																																																																				num74 = 1251779351;
																																																																																				continue;
																																																																																			}
																																																																																			if (num77 == 5)
																																																																																			{
																																																																																				break;
																																																																																			}
																																																																																			if (num77 != 3 && num77 != 3)
																																																																																			{
																																																																																				if (num77 != 4 && num77 != 4)
																																																																																				{
																																																																																					if (num77 != 1 && num77 != 1)
																																																																																					{
																																																																																						goto IL_2ace;
																																																																																					}
																																																																																					if ((object)"" != "" || "".Length != 0 || (object)"" != "")
																																																																																					{
																																																																																					}
																																																																																					int num78 = (int)num5 ^ -1028975247;
																																																																																					num74 = (num78 + -724744239 - -724744239) ^ -1028975247 ^ 0xE633FC3;
																																																																																					continue;
																																																																																				}
																																																																																				if ("".Length == 0)
																																																																																				{
																																																																																					key = current.Key;
																																																																																				}
																																																																																				num74 = (int)(0x694F5297 ^ num5);
																																																																																				continue;
																																																																																			}
																																																																																			if ("".Length != 0)
																																																																																			{
																																																																																			}
																																																																																			int num79 = (int)(num5 ^ 0x206A53B5);
																																																																																			num74 = (num79 + -592107183 - -592107183) ^ 0x206A53B5 ^ 0x135ABFC7;
																																																																																		}
																																																																																		goto IL_2ad3;
																																																																																		IL_2ace:;
																																																																																	}
																																																																																	while (num77 != 2);
																																																																																	List<string>.Enumerator enumerator2 = current.Value.GetEnumerator();
																																																																																	try
																																																																																	{
																																																																																		while (true)
																																																																																		{
																																																																																			int num80;
																																																																																			if (!enumerator2.MoveNext())
																																																																																			{
																																																																																				num80 = 1035224576;
																																																																																				goto IL_7390;
																																																																																			}
																																																																																			goto IL_73e9;
																																																																																			IL_73da:
																																																																																			if ("".Length != 0)
																																																																																			{
																																																																																				goto IL_73e2;
																																																																																			}
																																																																																			goto IL_73e9;
																																																																																			IL_73e9:
																																																																																			current2 = enumerator2.Current;
																																																																																			goto IL_73e2;
																																																																																			IL_73e2:
																																																																																			num80 = 651647311;
																																																																																			goto IL_7390;
																																																																																			IL_7390:
																																																																																			int num83;
																																																																																			while (true)
																																																																																			{
																																																																																				nint num81 = num80 ^ -2005805571;
																																																																																				nuint num82 = (nuint)num81;
																																																																																				num83 = (int)((num5 = (uint)((int)num82 ^ -2005805571 ^ 0x2CA37CDF ^ 0x2CA37CDF)) % 311);
																																																																																				if (num83 == 0)
																																																																																				{
																																																																																					num80 = 1856647184;
																																																																																					continue;
																																																																																				}
																																																																																				if (num83 == 198)
																																																																																				{
																																																																																					break;
																																																																																				}
																																																																																				if (num83 != 303 && num83 != 303)
																																																																																				{
																																																																																					if (num83 != 164 && num83 != 164)
																																																																																					{
																																																																																						if (num83 != 1 && num83 != 1)
																																																																																						{
																																																																																							if (num83 != 249 && num83 != 249)
																																																																																							{
																																																																																								if (num83 != 2 && num83 != 2)
																																																																																								{
																																																																																									if (num83 != 131 && num83 != 131)
																																																																																									{
																																																																																										if (num83 != 151 && num83 != 151)
																																																																																										{
																																																																																											if (num83 != 3 && num83 != 3)
																																																																																											{
																																																																																												if (num83 != 185 && num83 != 185)
																																																																																												{
																																																																																													if (num83 != 4 && num83 != 4)
																																																																																													{
																																																																																														if (num83 == 216 || num83 == 216)
																																																																																														{
																																																																																															num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -980953412;
																																																																																															continue;
																																																																																														}
																																																																																														if (num83 == 236 || num83 == 236)
																																																																																														{
																																																																																															UI.RichLogs(msg7, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																															num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x6B84F537);
																																																																																															continue;
																																																																																														}
																																																																																														if (num83 == 5 || num83 == 5)
																																																																																														{
																																																																																															int num84 = (int)num5 ^ -50124629;
																																																																																															num80 = (num84 + 1888009507 - 1888009507) ^ -50124629 ^ 0x4A3061AA;
																																																																																															continue;
																																																																																														}
																																																																																														if (num83 == 270 || num83 == 270)
																																																																																														{
																																																																																															if ("".Length == 0)
																																																																																															{
																																																																																																UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																															}
																																																																																															num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x172A911);
																																																																																															continue;
																																																																																														}
																																																																																														if (num83 == 283 || num83 == 283)
																																																																																														{
																																																																																															if ((object)"" != "" || (object)"" != "")
																																																																																															{
																																																																																															}
																																																																																															num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x353DE6A);
																																																																																															continue;
																																																																																														}
																																																																																														if (num83 == 6 || num83 == 6)
																																																																																														{
																																																																																															if ("".Length != 0)
																																																																																															{
																																																																																															}
																																																																																															int num85 = (int)(num5 ^ 0x13F05491);
																																																																																															num80 = (num85 + 1296082866 - 1296082866) ^ 0x13F05491 ^ 0x46D0CFC2;
																																																																																															continue;
																																																																																														}
																																																																																														if (num83 != 123)
																																																																																														{
																																																																																															if (num83 != 7 && num83 != 7)
																																																																																															{
																																																																																																if (num83 != 138)
																																																																																																{
																																																																																																	if (num83 != 144 && num83 != 144)
																																																																																																	{
																																																																																																		if (num83 != 8 && num83 != 8)
																																																																																																		{
																																																																																																			if (num83 != 156 && num83 != 156)
																																																																																																			{
																																																																																																				if (num83 != 9 && num83 != 9)
																																																																																																				{
																																																																																																					if (num83 == 169 || num83 == 169)
																																																																																																					{
																																																																																																						num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x278B9AB1);
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num83 == 177 || num83 == 177)
																																																																																																					{
																																																																																																						if ("".Length == 0 && "".Length == 0 && (object)"" == "" && "".Length == 0)
																																																																																																						{
																																																																																																							UI.RichLogs(msg8, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																						}
																																																																																																						int num86 = (int)(num5 ^ 0x313DD25);
																																																																																																						num80 = (num86 + 2098577595 - 2098577595) ^ 0x313DD25 ^ 0x5513EDF2;
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num83 == 10 || num83 == 10)
																																																																																																					{
																																																																																																						int num87 = (int)num5 ^ -2092522232;
																																																																																																						num80 = (num87 + -524976484 - -524976484) ^ -2092522232 ^ 0x6DB36C6E;
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num83 == 190 || num83 == 190)
																																																																																																					{
																																																																																																						if ((object)"" == "")
																																																																																																						{
																																																																																																							UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																						}
																																																																																																						num80 = (int)(0x75A96E29 ^ num5);
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num83 == 11 || num83 == 11)
																																																																																																					{
																																																																																																						int num88 = (int)num5 ^ -873848136;
																																																																																																						num80 = (num88 + 638158148 - 638158148) ^ -873848136 ^ 0x584722D3;
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num83 == 203 || num83 == 203)
																																																																																																					{
																																																																																																						num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x4B281963);
																																																																																																						continue;
																																																																																																					}
																																																																																																					if (num83 != 211)
																																																																																																					{
																																																																																																						if (num83 != 12 && num83 != 12)
																																																																																																						{
																																																																																																							if (num83 != 223)
																																																																																																							{
																																																																																																								if (num83 != 231 && num83 != 231)
																																																																																																								{
																																																																																																									if (num83 != 13 && num83 != 13)
																																																																																																									{
																																																																																																										if (num83 != 244 && num83 != 244)
																																																																																																										{
																																																																																																											if (num83 != 14 && num83 != 14)
																																																																																																											{
																																																																																																												if (num83 == 257 || num83 == 257)
																																																																																																												{
																																																																																																													int num89 = (int)(num5 ^ 0x176BFC75);
																																																																																																													num80 = (num89 + -1935120131 - -1935120131) ^ 0x176BFC75 ^ 0x1D0E160B;
																																																																																																													continue;
																																																																																																												}
																																																																																																												if (num83 == 262 || num83 == 262)
																																																																																																												{
																																																																																																													if ("".Length == 0 && "".Length == 0)
																																																																																																													{
																																																																																																														UI.RichLogs(msg9, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																													}
																																																																																																													num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -1816342964;
																																																																																																													continue;
																																																																																																												}
																																																																																																												if (num83 == 15 || num83 == 15)
																																																																																																												{
																																																																																																													if ((object)"" != "")
																																																																																																													{
																																																																																																													}
																																																																																																													num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -934840794;
																																																																																																													continue;
																																																																																																												}
																																																																																																												if (num83 == 277 || num83 == 277)
																																																																																																												{
																																																																																																													if ((object)"" == "" && "".Length == 0)
																																																																																																													{
																																																																																																														UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																													}
																																																																																																													num80 = (int)(0x46B38B64 ^ num5);
																																																																																																													continue;
																																																																																																												}
																																																																																																												if (num83 == 16 || num83 == 16)
																																																																																																												{
																																																																																																													num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x5D6EB2CB);
																																																																																																													continue;
																																																																																																												}
																																																																																																												if (num83 == 290 || num83 == 290)
																																																																																																												{
																																																																																																													if ("".Length != 0 || (object)"" != "")
																																																																																																													{
																																																																																																													}
																																																																																																													num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x1E0133D);
																																																																																																													continue;
																																																																																																												}
																																																																																																												if (num83 != 295)
																																																																																																												{
																																																																																																													if (num83 != 17 && num83 != 17)
																																																																																																													{
																																																																																																														if (num83 != 308)
																																																																																																														{
																																																																																																															if (num83 != 18 && num83 != 18)
																																																																																																															{
																																																																																																																if (num83 != 126 && num83 != 126)
																																																																																																																{
																																																																																																																	if (num83 != 128 && num83 != 128)
																																																																																																																	{
																																																																																																																		if (num83 != 19 && num83 != 19)
																																																																																																																		{
																																																																																																																			if (num83 == 133 || num83 == 133)
																																																																																																																			{
																																																																																																																				int num90 = (int)num5 ^ -1564922082;
																																																																																																																				num80 = (num90 + 1640297986 - 1640297986) ^ -1564922082 ^ 0x1C2DEACA;
																																																																																																																				continue;
																																																																																																																			}
																																																																																																																			if (num83 == 136 || num83 == 136)
																																																																																																																			{
																																																																																																																				if ("".Length == 0 && "".Length == 0 && (object)"" == "")
																																																																																																																				{
																																																																																																																					UI.RichLogs(msg10, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																				}
																																																																																																																				int num91 = (int)num5 ^ -194880753;
																																																																																																																				num80 = (num91 + -1071851625 - -1071851625) ^ -194880753 ^ 0x4D66965;
																																																																																																																				continue;
																																																																																																																			}
																																																																																																																			if (num83 == 20 || num83 == 20)
																																																																																																																			{
																																																																																																																				num80 = (int)(0x5EA632BF ^ num5);
																																																																																																																				continue;
																																																																																																																			}
																																																																																																																			if (num83 == 141 || num83 == 141)
																																																																																																																			{
																																																																																																																				UI.RichLogs(text3.ToUpper(), UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																				num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -1516529670;
																																																																																																																				continue;
																																																																																																																			}
																																																																																																																			if (num83 == 21 || num83 == 21)
																																																																																																																			{
																																																																																																																				if ("".Length != 0 || (object)"" != "" || (object)"" != "")
																																																																																																																				{
																																																																																																																				}
																																																																																																																				num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x3670B867);
																																																																																																																				continue;
																																																																																																																			}
																																																																																																																			if (num83 == 146 || num83 == 146)
																																																																																																																			{
																																																																																																																				if ((object)"" != "")
																																																																																																																				{
																																																																																																																				}
																																																																																																																				num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x56E8797F);
																																																																																																																				continue;
																																																																																																																			}
																																																																																																																			if (num83 != 149)
																																																																																																																			{
																																																																																																																				if (num83 != 22 && num83 != 22)
																																																																																																																				{
																																																																																																																					if (num83 != 154)
																																																																																																																					{
																																																																																																																						if (num83 != 23 && num83 != 23)
																																																																																																																						{
																																																																																																																							if (num83 != 159 && num83 != 159)
																																																																																																																							{
																																																																																																																								if (num83 != 162 && num83 != 162)
																																																																																																																								{
																																																																																																																									if (num83 != 24 && num83 != 24)
																																																																																																																									{
																																																																																																																										if (num83 == 167 || num83 == 167)
																																																																																																																										{
																																																																																																																											if ((object)"" != "")
																																																																																																																											{
																																																																																																																											}
																																																																																																																											int num92 = (int)num5 ^ -258408356;
																																																																																																																											num80 = (num92 + 137531383 - 137531383) ^ -258408356 ^ 0x75C8E8FC;
																																																																																																																											continue;
																																																																																																																										}
																																																																																																																										if (num83 == 25 || num83 == 25)
																																																																																																																										{
																																																																																																																											if ((object)"" == "")
																																																																																																																											{
																																																																																																																												UI.RichLogs(msg11, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																											}
																																																																																																																											num80 = (int)(0x349309 ^ num5);
																																																																																																																											continue;
																																																																																																																										}
																																																																																																																										if (num83 == 172 || num83 == 172)
																																																																																																																										{
																																																																																																																											if ("".Length != 0)
																																																																																																																											{
																																																																																																																											}
																																																																																																																											num80 = (int)(0x25AB23DB ^ num5);
																																																																																																																											continue;
																																																																																																																										}
																																																																																																																										if (num83 == 174 || num83 == 174)
																																																																																																																										{
																																																																																																																											if ((object)"" == "")
																																																																																																																											{
																																																																																																																												UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																											}
																																																																																																																											num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x5C110C19);
																																																																																																																											continue;
																																																																																																																										}
																																																																																																																										if (num83 == 26 || num83 == 26)
																																																																																																																										{
																																																																																																																											num80 = (int)(0x73D5F8E9 ^ num5);
																																																																																																																											continue;
																																																																																																																										}
																																																																																																																										if (num83 == 180 || num83 == 180)
																																																																																																																										{
																																																																																																																											if ("".Length != 0 || (object)"" != "")
																																																																																																																											{
																																																																																																																											}
																																																																																																																											num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1B5D2DDB);
																																																																																																																											continue;
																																																																																																																										}
																																																																																																																										if (num83 != 182)
																																																																																																																										{
																																																																																																																											if (num83 != 27 && num83 != 27)
																																																																																																																											{
																																																																																																																												if (num83 != 187)
																																																																																																																												{
																																																																																																																													if (num83 != 28 && num83 != 28)
																																																																																																																													{
																																																																																																																														if (num83 != 193 && num83 != 193)
																																																																																																																														{
																																																																																																																															if (num83 != 195 && num83 != 195)
																																																																																																																															{
																																																																																																																																if (num83 != 29 && num83 != 29)
																																																																																																																																{
																																																																																																																																	if (num83 == 200 || num83 == 200)
																																																																																																																																	{
																																																																																																																																		if ((object)"" != "")
																																																																																																																																		{
																																																																																																																																		}
																																																																																																																																		num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x5D82FD0A);
																																																																																																																																		continue;
																																																																																																																																	}
																																																																																																																																	if (num83 == 30 || num83 == 30)
																																																																																																																																	{
																																																																																																																																		UI.RichLogs(msg12, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																		num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x9B8EF0B);
																																																																																																																																		continue;
																																																																																																																																	}
																																																																																																																																	if (num83 == 205 || num83 == 205)
																																																																																																																																	{
																																																																																																																																		if ((object)"" != "" || (object)"" != "")
																																																																																																																																		{
																																																																																																																																		}
																																																																																																																																		num80 = (int)(0x588D81F8 ^ num5);
																																																																																																																																		continue;
																																																																																																																																	}
																																																																																																																																	if (num83 == 208 || num83 == 208)
																																																																																																																																	{
																																																																																																																																		if ((object)"" == "")
																																																																																																																																		{
																																																																																																																																			UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																		}
																																																																																																																																		num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x58AB7290);
																																																																																																																																		continue;
																																																																																																																																	}
																																																																																																																																	if (num83 == 31 || num83 == 31)
																																																																																																																																	{
																																																																																																																																		if ((object)"" != "")
																																																																																																																																		{
																																																																																																																																		}
																																																																																																																																		num80 = (int)(0x347D1013 ^ num5);
																																																																																																																																		continue;
																																																																																																																																	}
																																																																																																																																	if (num83 == 213 || num83 == 213)
																																																																																																																																	{
																																																																																																																																		if ((object)"" != "")
																																																																																																																																		{
																																																																																																																																		}
																																																																																																																																		num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x4EDC9329);
																																																																																																																																		continue;
																																																																																																																																	}
																																																																																																																																	if (num83 != 32)
																																																																																																																																	{
																																																																																																																																		if (num83 != 218 && num83 != 218)
																																																																																																																																		{
																																																																																																																																			if (num83 != 221)
																																																																																																																																			{
																																																																																																																																				if (num83 != 33 && num83 != 33)
																																																																																																																																				{
																																																																																																																																					if (num83 != 226 && num83 != 226)
																																																																																																																																					{
																																																																																																																																						if (num83 != 229 && num83 != 229)
																																																																																																																																						{
																																																																																																																																							if (num83 != 34 && num83 != 34)
																																																																																																																																							{
																																																																																																																																								if (num83 == 234 || num83 == 234)
																																																																																																																																								{
																																																																																																																																									int num93 = (int)num5 ^ -1080622794;
																																																																																																																																									num80 = (num93 + -153350991 - -153350991) ^ -1080622794 ^ 0x4463C3C1;
																																																																																																																																									continue;
																																																																																																																																								}
																																																																																																																																								if (num83 == 35 || num83 == 35)
																																																																																																																																								{
																																																																																																																																									UI.RichLogs(msg13, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																									int num94 = (int)num5 ^ -1872667259;
																																																																																																																																									num80 = (num94 + 446273036 - 446273036) ^ -1872667259 ^ 0x18949AC6;
																																																																																																																																									continue;
																																																																																																																																								}
																																																																																																																																								if (num83 == 239 || num83 == 239)
																																																																																																																																								{
																																																																																																																																									if ("".Length != 0)
																																																																																																																																									{
																																																																																																																																									}
																																																																																																																																									num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -2095530423;
																																																																																																																																									continue;
																																																																																																																																								}
																																																																																																																																								if (num83 == 241 || num83 == 241)
																																																																																																																																								{
																																																																																																																																									if ("".Length == 0)
																																																																																																																																									{
																																																																																																																																										UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																									}
																																																																																																																																									num80 = (int)(0x51945293 ^ num5);
																																																																																																																																									continue;
																																																																																																																																								}
																																																																																																																																								if (num83 == 36 || num83 == 36)
																																																																																																																																								{
																																																																																																																																									if ("".Length != 0 || (object)"" != "" || (object)"" != "")
																																																																																																																																									{
																																																																																																																																									}
																																																																																																																																									num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -1087407987;
																																																																																																																																									continue;
																																																																																																																																								}
																																																																																																																																								if (num83 == 247 || num83 == 247)
																																																																																																																																								{
																																																																																																																																									if ((object)"" != "")
																																																																																																																																									{
																																																																																																																																									}
																																																																																																																																									int num95 = (int)num5 ^ -208774703;
																																																																																																																																									num80 = (num95 + 1142979250 - 1142979250) ^ -208774703 ^ 0x3C4F5B3D;
																																																																																																																																									continue;
																																																																																																																																								}
																																																																																																																																								if (num83 != 37)
																																																																																																																																								{
																																																																																																																																									if (num83 != 252 && num83 != 252)
																																																																																																																																									{
																																																																																																																																										if (num83 != 254)
																																																																																																																																										{
																																																																																																																																											if (num83 != 38 && num83 != 38)
																																																																																																																																											{
																																																																																																																																												if (num83 != 259 && num83 != 259)
																																																																																																																																												{
																																																																																																																																													if (num83 != 39 && num83 != 39)
																																																																																																																																													{
																																																																																																																																														if (num83 != 265 && num83 != 265)
																																																																																																																																														{
																																																																																																																																															if (num83 == 267 || num83 == 267)
																																																																																																																																															{
																																																																																																																																																if ((object)"" != "" || "".Length != 0)
																																																																																																																																																{
																																																																																																																																																}
																																																																																																																																																num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x2B1B23EA);
																																																																																																																																																continue;
																																																																																																																																															}
																																																																																																																																															if (num83 == 40 || num83 == 40)
																																																																																																																																															{
																																																																																																																																																if ("".Length == 0)
																																																																																																																																																{
																																																																																																																																																	UI.RichLogs(msg14, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																}
																																																																																																																																																num80 = (int)(0x2E79B33C ^ num5);
																																																																																																																																																continue;
																																																																																																																																															}
																																																																																																																																															if (num83 == 272 || num83 == 272)
																																																																																																																																															{
																																																																																																																																																num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x3AE869CC);
																																																																																																																																																continue;
																																																																																																																																															}
																																																																																																																																															if (num83 == 275 || num83 == 275)
																																																																																																																																															{
																																																																																																																																																if ((object)"" == "" && (object)"" == "")
																																																																																																																																																{
																																																																																																																																																	UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																}
																																																																																																																																																int num96 = (int)num5 ^ -1258504359;
																																																																																																																																																num80 = (num96 + -979793061 - -979793061) ^ -1258504359 ^ 0x5AA9321B;
																																																																																																																																																continue;
																																																																																																																																															}
																																																																																																																																															if (num83 == 41 || num83 == 41)
																																																																																																																																															{
																																																																																																																																																int num97 = (int)(num5 ^ 0x71C30E1F);
																																																																																																																																																num80 = (num97 + 1454027511 - 1454027511) ^ 0x71C30E1F ^ 0x436C9642;
																																																																																																																																																continue;
																																																																																																																																															}
																																																																																																																																															if (num83 == 280 || num83 == 280)
																																																																																																																																															{
																																																																																																																																																num80 = (int)(0x7A7CEEFC ^ num5);
																																																																																																																																																continue;
																																																																																																																																															}
																																																																																																																																															if (num83 != 42)
																																																																																																																																															{
																																																																																																																																																if (num83 != 285 && num83 != 285)
																																																																																																																																																{
																																																																																																																																																	if (num83 != 288)
																																																																																																																																																	{
																																																																																																																																																		if (num83 != 43 && num83 != 43)
																																																																																																																																																		{
																																																																																																																																																			if (num83 != 293 && num83 != 293)
																																																																																																																																																			{
																																																																																																																																																				if (num83 != 44 && num83 != 44)
																																																																																																																																																				{
																																																																																																																																																					if (num83 != 298 && num83 != 298)
																																																																																																																																																					{
																																																																																																																																																						if (num83 == 301 || num83 == 301)
																																																																																																																																																						{
																																																																																																																																																							int num98 = (int)(num5 ^ 0x2C96C9B);
																																																																																																																																																							num80 = (num98 + -2114508324 - -2114508324) ^ 0x2C96C9B ^ 0x4B9304E8;
																																																																																																																																																							continue;
																																																																																																																																																						}
																																																																																																																																																						if (num83 == 45 || num83 == 45)
																																																																																																																																																						{
																																																																																																																																																							if ("".Length == 0 && (object)"" == "")
																																																																																																																																																							{
																																																																																																																																																								UI.RichLogs(msg15, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																							}
																																																																																																																																																							num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x3056CA68);
																																																																																																																																																							continue;
																																																																																																																																																						}
																																																																																																																																																						if (num83 == 306 || num83 == 306)
																																																																																																																																																						{
																																																																																																																																																							num80 = (int)(0x4C2157C ^ num5);
																																																																																																																																																							continue;
																																																																																																																																																						}
																																																																																																																																																						if (num83 == 46 || num83 == 46)
																																																																																																																																																						{
																																																																																																																																																							if ((object)"" == "")
																																																																																																																																																							{
																																																																																																																																																								UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																							}
																																																																																																																																																							int num99 = (int)(num5 ^ 0x1AB0756C);
																																																																																																																																																							num80 = (num99 + -845198526 - -845198526) ^ 0x1AB0756C ^ 0x40567DC3;
																																																																																																																																																							continue;
																																																																																																																																																						}
																																																																																																																																																						if (num83 == 121 || num83 == 121)
																																																																																																																																																						{
																																																																																																																																																							if ((object)"" != "")
																																																																																																																																																							{
																																																																																																																																																							}
																																																																																																																																																							int num100 = (int)(num5 ^ 0x508125D9);
																																																																																																																																																							num80 = (num100 + -311206564 - -311206564) ^ 0x508125D9 ^ 0x1A872D02;
																																																																																																																																																							continue;
																																																																																																																																																						}
																																																																																																																																																						if (num83 == 122 || num83 == 122)
																																																																																																																																																						{
																																																																																																																																																							if ((object)"" != "" || (object)"" != "")
																																																																																																																																																							{
																																																																																																																																																							}
																																																																																																																																																							num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -874248544;
																																																																																																																																																							continue;
																																																																																																																																																						}
																																																																																																																																																						if (num83 != 47)
																																																																																																																																																						{
																																																																																																																																																							if (num83 != 124 && num83 != 124)
																																																																																																																																																							{
																																																																																																																																																								if (num83 != 125)
																																																																																																																																																								{
																																																																																																																																																									if (num83 != 48 && num83 != 48)
																																																																																																																																																									{
																																																																																																																																																										if (num83 != 127 && num83 != 127)
																																																																																																																																																										{
																																																																																																																																																											if (num83 != 49 && num83 != 49)
																																																																																																																																																											{
																																																																																																																																																												if (num83 != 129 && num83 != 129)
																																																																																																																																																												{
																																																																																																																																																													if (num83 == 130 || num83 == 130)
																																																																																																																																																													{
																																																																																																																																																														if ((object)"" != "" || "".Length != 0)
																																																																																																																																																														{
																																																																																																																																																														}
																																																																																																																																																														num80 = (int)(0x55D9A11D ^ num5);
																																																																																																																																																														continue;
																																																																																																																																																													}
																																																																																																																																																													if (num83 == 50 || num83 == 50)
																																																																																																																																																													{
																																																																																																																																																														UI.RichLogs(msg16, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																														num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1CB8603E);
																																																																																																																																																														continue;
																																																																																																																																																													}
																																																																																																																																																													if (num83 == 132 || num83 == 132)
																																																																																																																																																													{
																																																																																																																																																														if ("".Length != 0 || "".Length != 0)
																																																																																																																																																														{
																																																																																																																																																														}
																																																																																																																																																														num80 = (int)(0x24569ED ^ num5);
																																																																																																																																																														continue;
																																																																																																																																																													}
																																																																																																																																																													if (num83 == 51 || num83 == 51)
																																																																																																																																																													{
																																																																																																																																																														if ("".Length == 0)
																																																																																																																																																														{
																																																																																																																																																															UI.RichLogs(androidver(text3), UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																														}
																																																																																																																																																														num80 = (int)(0x289B5F96 ^ num5);
																																																																																																																																																														continue;
																																																																																																																																																													}
																																																																																																																																																													if (num83 == 134 || num83 == 134)
																																																																																																																																																													{
																																																																																																																																																														num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x21986D44);
																																																																																																																																																														continue;
																																																																																																																																																													}
																																																																																																																																																													if (num83 == 135 || num83 == 135)
																																																																																																																																																													{
																																																																																																																																																														num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -777287785;
																																																																																																																																																														continue;
																																																																																																																																																													}
																																																																																																																																																													if (num83 != 52)
																																																																																																																																																													{
																																																																																																																																																														if (num83 != 137 && num83 != 137)
																																																																																																																																																														{
																																																																																																																																																															if (num83 != 53)
																																																																																																																																																															{
																																																																																																																																																																if (num83 != 139 && num83 != 139)
																																																																																																																																																																{
																																																																																																																																																																	if (num83 != 140 && num83 != 140)
																																																																																																																																																																	{
																																																																																																																																																																		if (num83 != 54 && num83 != 54)
																																																																																																																																																																		{
																																																																																																																																																																			if (num83 != 142 && num83 != 142)
																																																																																																																																																																			{
																																																																																																																																																																				if (num83 == 143 || num83 == 143)
																																																																																																																																																																				{
																																																																																																																																																																					num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x24EFB4FE);
																																																																																																																																																																					continue;
																																																																																																																																																																				}
																																																																																																																																																																				if (num83 == 55 || num83 == 55)
																																																																																																																																																																				{
																																																																																																																																																																					if ((object)"" == "")
																																																																																																																																																																					{
																																																																																																																																																																						UI.RichLogs(msg17, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																					}
																																																																																																																																																																					num80 = (int)(0x57499537 ^ num5);
																																																																																																																																																																					continue;
																																																																																																																																																																				}
																																																																																																																																																																				if (num83 == 145 || num83 == 145)
																																																																																																																																																																				{
																																																																																																																																																																					if ((object)"" != "" || "".Length != 0)
																																																																																																																																																																					{
																																																																																																																																																																					}
																																																																																																																																																																					num80 = (int)(0x7B2AC152 ^ num5);
																																																																																																																																																																					continue;
																																																																																																																																																																				}
																																																																																																																																																																				if (num83 == 56 || num83 == 56)
																																																																																																																																																																				{
																																																																																																																																																																					if ((object)"" == "" && "".Length == 0)
																																																																																																																																																																					{
																																																																																																																																																																						UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																					}
																																																																																																																																																																					int num101 = (int)num5 ^ -2052142936;
																																																																																																																																																																					num80 = (num101 + -1461275828 - -1461275828) ^ -2052142936 ^ 0x409B7E07;
																																																																																																																																																																					continue;
																																																																																																																																																																				}
																																																																																																																																																																				if (num83 == 147 || num83 == 147)
																																																																																																																																																																				{
																																																																																																																																																																					num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -334325679;
																																																																																																																																																																					continue;
																																																																																																																																																																				}
																																																																																																																																																																				if (num83 == 148 || num83 == 148)
																																																																																																																																																																				{
																																																																																																																																																																					if ((object)"" != "" || "".Length != 0 || (object)"" != "")
																																																																																																																																																																					{
																																																																																																																																																																					}
																																																																																																																																																																					num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x5CB1A53);
																																																																																																																																																																					continue;
																																																																																																																																																																				}
																																																																																																																																																																				if (num83 != 57)
																																																																																																																																																																				{
																																																																																																																																																																					if (num83 != 150 && num83 != 150)
																																																																																																																																																																					{
																																																																																																																																																																						if (num83 != 58)
																																																																																																																																																																						{
																																																																																																																																																																							if (num83 != 152 && num83 != 152)
																																																																																																																																																																							{
																																																																																																																																																																								if (num83 != 153 && num83 != 153)
																																																																																																																																																																								{
																																																																																																																																																																									if (num83 != 59 && num83 != 59)
																																																																																																																																																																									{
																																																																																																																																																																										if (num83 != 155 && num83 != 155)
																																																																																																																																																																										{
																																																																																																																																																																											if (num83 == 60 || num83 == 60)
																																																																																																																																																																											{
																																																																																																																																																																												if ("".Length != 0)
																																																																																																																																																																												{
																																																																																																																																																																												}
																																																																																																																																																																												int num102 = (int)(num5 ^ 0x37C099E9);
																																																																																																																																																																												num80 = (num102 + 1658844109 - 1658844109) ^ 0x37C099E9 ^ 0xC4C8AD;
																																																																																																																																																																												continue;
																																																																																																																																																																											}
																																																																																																																																																																											if (num83 == 157 || num83 == 157)
																																																																																																																																																																											{
																																																																																																																																																																												if ("".Length == 0)
																																																																																																																																																																												{
																																																																																																																																																																													UI.RichLogs(msg18, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																												}
																																																																																																																																																																												int num103 = (int)num5 ^ -31915639;
																																																																																																																																																																												num80 = (num103 + 441843935 - 441843935) ^ -31915639 ^ 0x69C3917B;
																																																																																																																																																																												continue;
																																																																																																																																																																											}
																																																																																																																																																																											if (num83 == 158 || num83 == 158)
																																																																																																																																																																											{
																																																																																																																																																																												if ("".Length != 0)
																																																																																																																																																																												{
																																																																																																																																																																												}
																																																																																																																																																																												num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x4719C940);
																																																																																																																																																																												continue;
																																																																																																																																																																											}
																																																																																																																																																																											if (num83 == 61 || num83 == 61)
																																																																																																																																																																											{
																																																																																																																																																																												if ((object)"" == "" && (object)"" == "")
																																																																																																																																																																												{
																																																																																																																																																																													UI.RichLogs(text4 + text3.Replace(oldValue, newValue), UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																												}
																																																																																																																																																																												int num104 = (int)(num5 ^ 0x7AEE90CC);
																																																																																																																																																																												num80 = (num104 + -1671469055 - -1671469055) ^ 0x7AEE90CC ^ 0x3149D02D;
																																																																																																																																																																												continue;
																																																																																																																																																																											}
																																																																																																																																																																											if (num83 == 160 || num83 == 160)
																																																																																																																																																																											{
																																																																																																																																																																												if ((object)"" != "")
																																																																																																																																																																												{
																																																																																																																																																																												}
																																																																																																																																																																												num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x46764F74);
																																																																																																																																																																												continue;
																																																																																																																																																																											}
																																																																																																																																																																											if (num83 == 161 || num83 == 161)
																																																																																																																																																																											{
																																																																																																																																																																												if ("".Length != 0)
																																																																																																																																																																												{
																																																																																																																																																																												}
																																																																																																																																																																												num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0xF50EEAC);
																																																																																																																																																																												continue;
																																																																																																																																																																											}
																																																																																																																																																																											if (num83 != 62)
																																																																																																																																																																											{
																																																																																																																																																																												if (num83 != 163 && num83 != 163)
																																																																																																																																																																												{
																																																																																																																																																																													if (num83 != 63)
																																																																																																																																																																													{
																																																																																																																																																																														if (num83 != 165 && num83 != 165)
																																																																																																																																																																														{
																																																																																																																																																																															if (num83 != 166 && num83 != 166)
																																																																																																																																																																															{
																																																																																																																																																																																if (num83 != 64 && num83 != 64)
																																																																																																																																																																																{
																																																																																																																																																																																	if (num83 != 168 && num83 != 168)
																																																																																																																																																																																	{
																																																																																																																																																																																		if (num83 == 65 || num83 == 65)
																																																																																																																																																																																		{
																																																																																																																																																																																			if ("".Length != 0)
																																																																																																																																																																																			{
																																																																																																																																																																																			}
																																																																																																																																																																																			num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x76020B92);
																																																																																																																																																																																			continue;
																																																																																																																																																																																		}
																																																																																																																																																																																		if (num83 == 170 || num83 == 170)
																																																																																																																																																																																		{
																																																																																																																																																																																			UI.RichLogs(msg19, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																			num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x51142E51);
																																																																																																																																																																																			continue;
																																																																																																																																																																																		}
																																																																																																																																																																																		if (num83 == 171 || num83 == 171)
																																																																																																																																																																																		{
																																																																																																																																																																																			if ((object)"" != "")
																																																																																																																																																																																			{
																																																																																																																																																																																			}
																																																																																																																																																																																			num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x3E44D78F);
																																																																																																																																																																																			continue;
																																																																																																																																																																																		}
																																																																																																																																																																																		if (num83 == 66 || num83 == 66)
																																																																																																																																																																																		{
																																																																																																																																																																																			UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																			int num105 = (int)num5 ^ -648424566;
																																																																																																																																																																																			num80 = (num105 + -1000754599 - -1000754599) ^ -648424566 ^ 0x7E875DE;
																																																																																																																																																																																			continue;
																																																																																																																																																																																		}
																																																																																																																																																																																		if (num83 == 173 || num83 == 173)
																																																																																																																																																																																		{
																																																																																																																																																																																			if ((object)"" != "" || (object)"" != "" || (object)"" != "")
																																																																																																																																																																																			{
																																																																																																																																																																																			}
																																																																																																																																																																																			num80 = (int)(0x605F5367 ^ num5);
																																																																																																																																																																																			continue;
																																																																																																																																																																																		}
																																																																																																																																																																																		if (num83 == 67 || num83 == 67)
																																																																																																																																																																																		{
																																																																																																																																																																																			num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -1684444185;
																																																																																																																																																																																			continue;
																																																																																																																																																																																		}
																																																																																																																																																																																		if (num83 != 175)
																																																																																																																																																																																		{
																																																																																																																																																																																			if (num83 != 176 && num83 != 176)
																																																																																																																																																																																			{
																																																																																																																																																																																				if (num83 != 68)
																																																																																																																																																																																				{
																																																																																																																																																																																					if (num83 != 178 && num83 != 178)
																																																																																																																																																																																					{
																																																																																																																																																																																						if (num83 != 179 && num83 != 179)
																																																																																																																																																																																						{
																																																																																																																																																																																							if (num83 != 69 && num83 != 69)
																																																																																																																																																																																							{
																																																																																																																																																																																								if (num83 != 181 && num83 != 181)
																																																																																																																																																																																								{
																																																																																																																																																																																									if (num83 == 70 || num83 == 70)
																																																																																																																																																																																									{
																																																																																																																																																																																										if ((object)"" != "")
																																																																																																																																																																																										{
																																																																																																																																																																																										}
																																																																																																																																																																																										num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -831241742;
																																																																																																																																																																																										continue;
																																																																																																																																																																																									}
																																																																																																																																																																																									if (num83 == 183 || num83 == 183)
																																																																																																																																																																																									{
																																																																																																																																																																																										UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																										int num106 = (int)num5 ^ -622430069;
																																																																																																																																																																																										num80 = (num106 + -2022648730 - -2022648730) ^ -622430069 ^ 0x3EC52290;
																																																																																																																																																																																										continue;
																																																																																																																																																																																									}
																																																																																																																																																																																									if (num83 == 184 || num83 == 184)
																																																																																																																																																																																									{
																																																																																																																																																																																										if ((object)"" != "")
																																																																																																																																																																																										{
																																																																																																																																																																																										}
																																																																																																																																																																																										num80 = (int)(0x3265BCFA ^ num5);
																																																																																																																																																																																										continue;
																																																																																																																																																																																									}
																																																																																																																																																																																									if (num83 == 71 || num83 == 71)
																																																																																																																																																																																									{
																																																																																																																																																																																										if ("".Length == 0)
																																																																																																																																																																																										{
																																																																																																																																																																																											UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																										}
																																																																																																																																																																																										num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x71EC5DF9);
																																																																																																																																																																																										continue;
																																																																																																																																																																																									}
																																																																																																																																																																																									if (num83 == 186 || num83 == 186)
																																																																																																																																																																																									{
																																																																																																																																																																																										if ("".Length != 0 || (object)"" != "" || "".Length != 0)
																																																																																																																																																																																										{
																																																																																																																																																																																										}
																																																																																																																																																																																										int num107 = (int)(num5 ^ 0x4F789356);
																																																																																																																																																																																										num80 = (num107 + -1320105706 - -1320105706) ^ 0x4F789356 ^ 0x5706E91E;
																																																																																																																																																																																										continue;
																																																																																																																																																																																									}
																																																																																																																																																																																									if (num83 == 72 || num83 == 72)
																																																																																																																																																																																									{
																																																																																																																																																																																										num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x218CA95A);
																																																																																																																																																																																										continue;
																																																																																																																																																																																									}
																																																																																																																																																																																									if (num83 != 188)
																																																																																																																																																																																									{
																																																																																																																																																																																										if (num83 != 189 && num83 != 189)
																																																																																																																																																																																										{
																																																																																																																																																																																											if (num83 != 73)
																																																																																																																																																																																											{
																																																																																																																																																																																												if (num83 != 191 && num83 != 191)
																																																																																																																																																																																												{
																																																																																																																																																																																													if (num83 != 192 && num83 != 192)
																																																																																																																																																																																													{
																																																																																																																																																																																														if (num83 != 74 && num83 != 74)
																																																																																																																																																																																														{
																																																																																																																																																																																															if (num83 != 194 && num83 != 194)
																																																																																																																																																																																															{
																																																																																																																																																																																																if (num83 == 75 || num83 == 75)
																																																																																																																																																																																																{
																																																																																																																																																																																																	if ("".Length != 0 || (object)"" != "")
																																																																																																																																																																																																	{
																																																																																																																																																																																																	}
																																																																																																																																																																																																	int num108 = (int)(num5 ^ 0x13444DC);
																																																																																																																																																																																																	num80 = (num108 + -962358200 - -962358200) ^ 0x13444DC ^ 0x3696A788;
																																																																																																																																																																																																	continue;
																																																																																																																																																																																																}
																																																																																																																																																																																																if (num83 == 196 || num83 == 196)
																																																																																																																																																																																																{
																																																																																																																																																																																																	if ((object)"" == "")
																																																																																																																																																																																																	{
																																																																																																																																																																																																		UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																																	}
																																																																																																																																																																																																	int num109 = (int)num5 ^ -971586460;
																																																																																																																																																																																																	num80 = (num109 + 1443065007 - 1443065007) ^ -971586460 ^ 0x2B12DF13;
																																																																																																																																																																																																	continue;
																																																																																																																																																																																																}
																																																																																																																																																																																																if (num83 == 197 || num83 == 197)
																																																																																																																																																																																																{
																																																																																																																																																																																																	if ("".Length != 0 || "".Length != 0 || "".Length != 0)
																																																																																																																																																																																																	{
																																																																																																																																																																																																	}
																																																																																																																																																																																																	int num110 = (int)num5 ^ -1673317193;
																																																																																																																																																																																																	num80 = (num110 + -509440081 - -509440081) ^ -1673317193 ^ 0xFD05A6B;
																																																																																																																																																																																																	continue;
																																																																																																																																																																																																}
																																																																																																																																																																																																if (num83 == 76 || num83 == 76)
																																																																																																																																																																																																{
																																																																																																																																																																																																	UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																																	num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x5B0B2F0D);
																																																																																																																																																																																																	continue;
																																																																																																																																																																																																}
																																																																																																																																																																																																if (num83 == 199 || num83 == 199)
																																																																																																																																																																																																{
																																																																																																																																																																																																	num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -673809456;
																																																																																																																																																																																																	continue;
																																																																																																																																																																																																}
																																																																																																																																																																																																if (num83 == 77 || num83 == 77)
																																																																																																																																																																																																{
																																																																																																																																																																																																	if ((object)"" != "" || "".Length != 0)
																																																																																																																																																																																																	{
																																																																																																																																																																																																	}
																																																																																																																																																																																																	num80 = (int)(0x486A2582 ^ num5);
																																																																																																																																																																																																	continue;
																																																																																																																																																																																																}
																																																																																																																																																																																																if (num83 != 201)
																																																																																																																																																																																																{
																																																																																																																																																																																																	if (num83 != 202 && num83 != 202)
																																																																																																																																																																																																	{
																																																																																																																																																																																																		if (num83 != 78)
																																																																																																																																																																																																		{
																																																																																																																																																																																																			if (num83 != 204 && num83 != 204)
																																																																																																																																																																																																			{
																																																																																																																																																																																																				if (num83 != 79 && num83 != 79)
																																																																																																																																																																																																				{
																																																																																																																																																																																																					if (num83 != 206 && num83 != 206)
																																																																																																																																																																																																					{
																																																																																																																																																																																																						if (num83 != 207 && num83 != 207)
																																																																																																																																																																																																						{
																																																																																																																																																																																																							if (num83 == 80 || num83 == 80)
																																																																																																																																																																																																							{
																																																																																																																																																																																																								if ((object)"" != "" || (object)"" != "")
																																																																																																																																																																																																								{
																																																																																																																																																																																																								}
																																																																																																																																																																																																								num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x1A73B0B8);
																																																																																																																																																																																																								continue;
																																																																																																																																																																																																							}
																																																																																																																																																																																																							if (num83 == 209 || num83 == 209)
																																																																																																																																																																																																							{
																																																																																																																																																																																																								if ((object)"" == "")
																																																																																																																																																																																																								{
																																																																																																																																																																																																									UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																																								}
																																																																																																																																																																																																								num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -1615099151;
																																																																																																																																																																																																								continue;
																																																																																																																																																																																																							}
																																																																																																																																																																																																							if (num83 == 210 || num83 == 210)
																																																																																																																																																																																																							{
																																																																																																																																																																																																								int num111 = (int)(num5 ^ 0x11B1ADC9);
																																																																																																																																																																																																								num80 = (num111 + 1911029853 - 1911029853) ^ 0x11B1ADC9 ^ 0x5465E17B;
																																																																																																																																																																																																								continue;
																																																																																																																																																																																																							}
																																																																																																																																																																																																							if (num83 == 81 || num83 == 81)
																																																																																																																																																																																																							{
																																																																																																																																																																																																								if ("".Length == 0)
																																																																																																																																																																																																								{
																																																																																																																																																																																																									UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																																								}
																																																																																																																																																																																																								num80 = (int)(0x125AC74E ^ num5);
																																																																																																																																																																																																								continue;
																																																																																																																																																																																																							}
																																																																																																																																																																																																							if (num83 == 212 || num83 == 212)
																																																																																																																																																																																																							{
																																																																																																																																																																																																								if ((object)"" != "")
																																																																																																																																																																																																								{
																																																																																																																																																																																																								}
																																																																																																																																																																																																								int num112 = (int)num5 ^ -1306822891;
																																																																																																																																																																																																								num80 = (num112 + 533104543 - 533104543) ^ -1306822891 ^ 0x5DAC3331;
																																																																																																																																																																																																								continue;
																																																																																																																																																																																																							}
																																																																																																																																																																																																							if (num83 == 82 || num83 == 82)
																																																																																																																																																																																																							{
																																																																																																																																																																																																								if ("".Length != 0 || (object)"" != "")
																																																																																																																																																																																																								{
																																																																																																																																																																																																								}
																																																																																																																																																																																																								int num113 = (int)num5 ^ -1617350861;
																																																																																																																																																																																																								num80 = (num113 + 1897610192 - 1897610192) ^ -1617350861 ^ 0x69005108;
																																																																																																																																																																																																								continue;
																																																																																																																																																																																																							}
																																																																																																																																																																																																							if (num83 != 214)
																																																																																																																																																																																																							{
																																																																																																																																																																																																								if (num83 != 215 && num83 != 215)
																																																																																																																																																																																																								{
																																																																																																																																																																																																									if (num83 != 83)
																																																																																																																																																																																																									{
																																																																																																																																																																																																										if (num83 != 217 && num83 != 217)
																																																																																																																																																																																																										{
																																																																																																																																																																																																											if (num83 != 84 && num83 != 84)
																																																																																																																																																																																																											{
																																																																																																																																																																																																												if (num83 != 219 && num83 != 219)
																																																																																																																																																																																																												{
																																																																																																																																																																																																													if (num83 != 220 && num83 != 220)
																																																																																																																																																																																																													{
																																																																																																																																																																																																														if (num83 == 85 || num83 == 85)
																																																																																																																																																																																																														{
																																																																																																																																																																																																															if ((object)"" != "" || (object)"" != "" || (object)"" != "")
																																																																																																																																																																																																															{
																																																																																																																																																																																																															}
																																																																																																																																																																																																															num80 = (int)((num5 << 3) | (num5 >> 29)) ^ -1025507019;
																																																																																																																																																																																																															continue;
																																																																																																																																																																																																														}
																																																																																																																																																																																																														if (num83 == 222 || num83 == 222)
																																																																																																																																																																																																														{
																																																																																																																																																																																																															if ("".Length == 0)
																																																																																																																																																																																																															{
																																																																																																																																																																																																																UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																																															}
																																																																																																																																																																																																															num80 = (int)(0x76568099 ^ num5);
																																																																																																																																																																																																															continue;
																																																																																																																																																																																																														}
																																																																																																																																																																																																														if (num83 == 86 || num83 == 86)
																																																																																																																																																																																																														{
																																																																																																																																																																																																															num80 = (int)(0x1FDAC065 ^ num5);
																																																																																																																																																																																																															continue;
																																																																																																																																																																																																														}
																																																																																																																																																																																																														if (num83 == 224 || num83 == 224)
																																																																																																																																																																																																														{
																																																																																																																																																																																																															if ("".Length == 0 && "".Length == 0)
																																																																																																																																																																																																															{
																																																																																																																																																																																																																UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																																															}
																																																																																																																																																																																																															num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x45F15543);
																																																																																																																																																																																																															continue;
																																																																																																																																																																																																														}
																																																																																																																																																																																																														if (num83 == 225 || num83 == 225)
																																																																																																																																																																																																														{
																																																																																																																																																																																																															if ("".Length != 0)
																																																																																																																																																																																																															{
																																																																																																																																																																																																															}
																																																																																																																																																																																																															int num114 = (int)num5 ^ -183589035;
																																																																																																																																																																																																															num80 = (num114 + -558753581 - -558753581) ^ -183589035 ^ 0xC46A6AA;
																																																																																																																																																																																																															continue;
																																																																																																																																																																																																														}
																																																																																																																																																																																																														if (num83 == 87 || num83 == 87)
																																																																																																																																																																																																														{
																																																																																																																																																																																																															if ("".Length != 0)
																																																																																																																																																																																																															{
																																																																																																																																																																																																															}
																																																																																																																																																																																																															int num115 = (int)num5 ^ -248933818;
																																																																																																																																																																																																															num80 = (num115 + 2032453909 - 2032453909) ^ -248933818 ^ 0x11F67AF0;
																																																																																																																																																																																																															continue;
																																																																																																																																																																																																														}
																																																																																																																																																																																																														if (num83 != 227)
																																																																																																																																																																																																														{
																																																																																																																																																																																																															if (num83 != 228 && num83 != 228)
																																																																																																																																																																																																															{
																																																																																																																																																																																																																if (num83 != 88)
																																																																																																																																																																																																																{
																																																																																																																																																																																																																	if (num83 != 230 && num83 != 230)
																																																																																																																																																																																																																	{
																																																																																																																																																																																																																		if (num83 != 89 && num83 != 89)
																																																																																																																																																																																																																		{
																																																																																																																																																																																																																			if (num83 != 232 && num83 != 232)
																																																																																																																																																																																																																			{
																																																																																																																																																																																																																				if (num83 != 233 && num83 != 233)
																																																																																																																																																																																																																				{
																																																																																																																																																																																																																					if (num83 == 90 || num83 == 90)
																																																																																																																																																																																																																					{
																																																																																																																																																																																																																						if ((object)"" != "")
																																																																																																																																																																																																																						{
																																																																																																																																																																																																																						}
																																																																																																																																																																																																																						int num116 = (int)num5 ^ -2085644567;
																																																																																																																																																																																																																						num80 = (num116 + 1937514381 - 1937514381) ^ -2085644567 ^ 0x248E9D49;
																																																																																																																																																																																																																						continue;
																																																																																																																																																																																																																					}
																																																																																																																																																																																																																					if (num83 == 235 || num83 == 235)
																																																																																																																																																																																																																					{
																																																																																																																																																																																																																						if ("".Length == 0)
																																																																																																																																																																																																																						{
																																																																																																																																																																																																																							UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																																																						}
																																																																																																																																																																																																																						num80 = (int)(0x489E170 ^ num5);
																																																																																																																																																																																																																						continue;
																																																																																																																																																																																																																					}
																																																																																																																																																																																																																					if (num83 == 91 || num83 == 91)
																																																																																																																																																																																																																					{
																																																																																																																																																																																																																						if ((object)"" != "")
																																																																																																																																																																																																																						{
																																																																																																																																																																																																																						}
																																																																																																																																																																																																																						num80 = (int)(0x461F5F7 ^ num5);
																																																																																																																																																																																																																						continue;
																																																																																																																																																																																																																					}
																																																																																																																																																																																																																					if (num83 == 237 || num83 == 237)
																																																																																																																																																																																																																					{
																																																																																																																																																																																																																						UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																																																						num80 = (int)(0x5701B997 ^ num5);
																																																																																																																																																																																																																						continue;
																																																																																																																																																																																																																					}
																																																																																																																																																																																																																					if (num83 == 238 || num83 == 238)
																																																																																																																																																																																																																					{
																																																																																																																																																																																																																						if ((object)"" != "" || (object)"" != "")
																																																																																																																																																																																																																						{
																																																																																																																																																																																																																						}
																																																																																																																																																																																																																						num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -1948016510;
																																																																																																																																																																																																																						continue;
																																																																																																																																																																																																																					}
																																																																																																																																																																																																																					if (num83 == 92 || num83 == 92)
																																																																																																																																																																																																																					{
																																																																																																																																																																																																																						if ("".Length != 0)
																																																																																																																																																																																																																						{
																																																																																																																																																																																																																						}
																																																																																																																																																																																																																						num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x5C4B7385);
																																																																																																																																																																																																																						continue;
																																																																																																																																																																																																																					}
																																																																																																																																																																																																																					if (num83 != 240)
																																																																																																																																																																																																																					{
																																																																																																																																																																																																																						if (num83 != 93 && num83 != 93)
																																																																																																																																																																																																																						{
																																																																																																																																																																																																																							if (num83 != 242)
																																																																																																																																																																																																																							{
																																																																																																																																																																																																																								if (num83 != 243 && num83 != 243)
																																																																																																																																																																																																																								{
																																																																																																																																																																																																																									if (num83 != 94 && num83 != 94)
																																																																																																																																																																																																																									{
																																																																																																																																																																																																																										if (num83 != 245 && num83 != 245)
																																																																																																																																																																																																																										{
																																																																																																																																																																																																																											if (num83 != 246 && num83 != 246)
																																																																																																																																																																																																																											{
																																																																																																																																																																																																																												if (num83 == 95 || num83 == 95)
																																																																																																																																																																																																																												{
																																																																																																																																																																																																																													if ((object)"" != "" || (object)"" != "")
																																																																																																																																																																																																																													{
																																																																																																																																																																																																																													}
																																																																																																																																																																																																																													num80 = (int)(0x339C10D7 ^ num5);
																																																																																																																																																																																																																													continue;
																																																																																																																																																																																																																												}
																																																																																																																																																																																																																												if (num83 == 248 || num83 == 248)
																																																																																																																																																																																																																												{
																																																																																																																																																																																																																													if ("".Length == 0)
																																																																																																																																																																																																																													{
																																																																																																																																																																																																																														UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																																																													}
																																																																																																																																																																																																																													int num117 = (int)num5 ^ -1676083600;
																																																																																																																																																																																																																													num80 = (num117 + 733186496 - 733186496) ^ -1676083600 ^ 0xD92ADD5;
																																																																																																																																																																																																																													continue;
																																																																																																																																																																																																																												}
																																																																																																																																																																																																																												if (num83 == 96 || num83 == 96)
																																																																																																																																																																																																																												{
																																																																																																																																																																																																																													num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1ED413FA);
																																																																																																																																																																																																																													continue;
																																																																																																																																																																																																																												}
																																																																																																																																																																																																																												if (num83 == 250 || num83 == 250)
																																																																																																																																																																																																																												{
																																																																																																																																																																																																																													if ((object)"" == "")
																																																																																																																																																																																																																													{
																																																																																																																																																																																																																														UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																																																													}
																																																																																																																																																																																																																													int num118 = (int)num5 ^ -1812015495;
																																																																																																																																																																																																																													num80 = (num118 + -1489963690 - -1489963690) ^ -1812015495 ^ 0x11EBC45F;
																																																																																																																																																																																																																													continue;
																																																																																																																																																																																																																												}
																																																																																																																																																																																																																												if (num83 == 251 || num83 == 251)
																																																																																																																																																																																																																												{
																																																																																																																																																																																																																													if ("".Length != 0 || "".Length != 0)
																																																																																																																																																																																																																													{
																																																																																																																																																																																																																													}
																																																																																																																																																																																																																													int num119 = (int)num5 ^ -1636838668;
																																																																																																																																																																																																																													num80 = (num119 + -1857987802 - -1857987802) ^ -1636838668 ^ 0xD0D4889;
																																																																																																																																																																																																																													continue;
																																																																																																																																																																																																																												}
																																																																																																																																																																																																																												if (num83 == 97 || num83 == 97)
																																																																																																																																																																																																																												{
																																																																																																																																																																																																																													if ("".Length != 0)
																																																																																																																																																																																																																													{
																																																																																																																																																																																																																													}
																																																																																																																																																																																																																													int num120 = (int)num5 ^ -1060520764;
																																																																																																																																																																																																																													num80 = (num120 + 1686266039 - 1686266039) ^ -1060520764 ^ 0x591244FC;
																																																																																																																																																																																																																													continue;
																																																																																																																																																																																																																												}
																																																																																																																																																																																																																												if (num83 != 253)
																																																																																																																																																																																																																												{
																																																																																																																																																																																																																													if (num83 != 98 && num83 != 98)
																																																																																																																																																																																																																													{
																																																																																																																																																																																																																														if (num83 != 255)
																																																																																																																																																																																																																														{
																																																																																																																																																																																																																															if (num83 != 256 && num83 != 256)
																																																																																																																																																																																																																															{
																																																																																																																																																																																																																																if (num83 != 99 && num83 != 99)
																																																																																																																																																																																																																																{
																																																																																																																																																																																																																																	if (num83 != 258 && num83 != 258)
																																																																																																																																																																																																																																	{
																																																																																																																																																																																																																																		if (num83 != 100 && num83 != 100)
																																																																																																																																																																																																																																		{
																																																																																																																																																																																																																																			if (num83 == 260 || num83 == 260)
																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																				if ((object)"" != "")
																																																																																																																																																																																																																																				{
																																																																																																																																																																																																																																				}
																																																																																																																																																																																																																																				num80 = (int)(0x17C39486 ^ num5);
																																																																																																																																																																																																																																				continue;
																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																			if (num83 == 261 || num83 == 261)
																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																				if ((object)"" == "" && (object)"" == "")
																																																																																																																																																																																																																																				{
																																																																																																																																																																																																																																					UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																																																																				}
																																																																																																																																																																																																																																				num80 = (int)(0x6EEB012C ^ num5);
																																																																																																																																																																																																																																				continue;
																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																			if (num83 == 101 || num83 == 101)
																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																				num80 = (int)(0x3E98CE6A ^ num5);
																																																																																																																																																																																																																																				continue;
																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																			if (num83 == 263 || num83 == 263)
																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																				if ("".Length == 0)
																																																																																																																																																																																																																																				{
																																																																																																																																																																																																																																					UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																																																																				}
																																																																																																																																																																																																																																				int num121 = (int)num5 ^ -430357000;
																																																																																																																																																																																																																																				num80 = (num121 + 1586687240 - 1586687240) ^ -430357000 ^ 0x57D16418;
																																																																																																																																																																																																																																				continue;
																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																			if (num83 == 264 || num83 == 264)
																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																				if ((object)"" != "")
																																																																																																																																																																																																																																				{
																																																																																																																																																																																																																																				}
																																																																																																																																																																																																																																				num80 = (int)(0x4E5C8DB5 ^ num5);
																																																																																																																																																																																																																																				continue;
																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																			if (num83 == 102 || num83 == 102)
																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																				if ("".Length != 0 || (object)"" != "")
																																																																																																																																																																																																																																				{
																																																																																																																																																																																																																																				}
																																																																																																																																																																																																																																				num80 = (int)(0x45735541 ^ num5);
																																																																																																																																																																																																																																				continue;
																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																			if (num83 != 266)
																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																				if (num83 != 103 && num83 != 103)
																																																																																																																																																																																																																																				{
																																																																																																																																																																																																																																					if (num83 != 268)
																																																																																																																																																																																																																																					{
																																																																																																																																																																																																																																						if (num83 != 269 && num83 != 269)
																																																																																																																																																																																																																																						{
																																																																																																																																																																																																																																							if (num83 != 104 && num83 != 104)
																																																																																																																																																																																																																																							{
																																																																																																																																																																																																																																								if (num83 != 271 && num83 != 271)
																																																																																																																																																																																																																																								{
																																																																																																																																																																																																																																									if (num83 != 105 && num83 != 105)
																																																																																																																																																																																																																																									{
																																																																																																																																																																																																																																										if (num83 == 273 || num83 == 273)
																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																											int num122 = (int)num5 ^ -2082289597;
																																																																																																																																																																																																																																											num80 = (num122 + -1971524479 - -1971524479) ^ -2082289597 ^ 0x69C5BBA8;
																																																																																																																																																																																																																																											continue;
																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																										if (num83 == 274 || num83 == 274)
																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																											UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																																																																											num80 = (int)(0x55EBE47C ^ num5);
																																																																																																																																																																																																																																											continue;
																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																										if (num83 == 106 || num83 == 106)
																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																											int num123 = (int)(num5 ^ 0x27A9958B);
																																																																																																																																																																																																																																											num80 = (num123 + 630923580 - 630923580) ^ 0x27A9958B ^ 0x5F52CAF0;
																																																																																																																																																																																																																																											continue;
																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																										if (num83 == 276 || num83 == 276)
																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																											if ((object)"" == "")
																																																																																																																																																																																																																																											{
																																																																																																																																																																																																																																												UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																																																																											}
																																																																																																																																																																																																																																											int num124 = (int)num5 ^ -2053302611;
																																																																																																																																																																																																																																											num80 = (num124 + 1100624193 - 1100624193) ^ -2053302611 ^ 0x426D8A13;
																																																																																																																																																																																																																																											continue;
																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																										if (num83 == 107 || num83 == 107)
																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																											if ((object)"" != "" || (object)"" != "" || (object)"" != "")
																																																																																																																																																																																																																																											{
																																																																																																																																																																																																																																											}
																																																																																																																																																																																																																																											int num125 = (int)(num5 ^ 0x65D00777);
																																																																																																																																																																																																																																											num80 = (num125 + 1496369456 - 1496369456) ^ 0x65D00777 ^ 0x5A6B588;
																																																																																																																																																																																																																																											continue;
																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																										if (num83 == 278 || num83 == 278)
																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																											if ("".Length != 0 || (object)"" != "" || (object)"" != "")
																																																																																																																																																																																																																																											{
																																																																																																																																																																																																																																											}
																																																																																																																																																																																																																																											num80 = (int)(0x7FED1F03 ^ num5);
																																																																																																																																																																																																																																											continue;
																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																										if (num83 != 279)
																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																											if (num83 != 108 && num83 != 108)
																																																																																																																																																																																																																																											{
																																																																																																																																																																																																																																												if (num83 != 281)
																																																																																																																																																																																																																																												{
																																																																																																																																																																																																																																													if (num83 != 282 && num83 != 282)
																																																																																																																																																																																																																																													{
																																																																																																																																																																																																																																														if (num83 != 109 && num83 != 109)
																																																																																																																																																																																																																																														{
																																																																																																																																																																																																																																															if (num83 != 284 && num83 != 284)
																																																																																																																																																																																																																																															{
																																																																																																																																																																																																																																																if (num83 != 110 && num83 != 110)
																																																																																																																																																																																																																																																{
																																																																																																																																																																																																																																																	if (num83 == 286 || num83 == 286)
																																																																																																																																																																																																																																																	{
																																																																																																																																																																																																																																																		num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x6BF291F4);
																																																																																																																																																																																																																																																		continue;
																																																																																																																																																																																																																																																	}
																																																																																																																																																																																																																																																	if (num83 == 287 || num83 == 287)
																																																																																																																																																																																																																																																	{
																																																																																																																																																																																																																																																		if ("".Length == 0 && (object)"" == "" && "".Length == 0)
																																																																																																																																																																																																																																																		{
																																																																																																																																																																																																																																																			UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																																																																																		}
																																																																																																																																																																																																																																																		int num126 = (int)(num5 ^ 0x71F8486);
																																																																																																																																																																																																																																																		num80 = (num126 + 42899454 - 42899454) ^ 0x71F8486 ^ 0x358BAFDC;
																																																																																																																																																																																																																																																		continue;
																																																																																																																																																																																																																																																	}
																																																																																																																																																																																																																																																	if (num83 == 111 || num83 == 111)
																																																																																																																																																																																																																																																	{
																																																																																																																																																																																																																																																		if ((object)"" != "")
																																																																																																																																																																																																																																																		{
																																																																																																																																																																																																																																																		}
																																																																																																																																																																																																																																																		int num127 = (int)(num5 ^ 0x3A4A4B52);
																																																																																																																																																																																																																																																		num80 = (num127 + 1676708121 - 1676708121) ^ 0x3A4A4B52 ^ 0x528E44F0;
																																																																																																																																																																																																																																																		continue;
																																																																																																																																																																																																																																																	}
																																																																																																																																																																																																																																																	if (num83 == 289 || num83 == 289)
																																																																																																																																																																																																																																																	{
																																																																																																																																																																																																																																																		UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																																																																																		int num128 = (int)(num5 ^ 0x567C1D53);
																																																																																																																																																																																																																																																		num80 = (num128 + -1753267536 - -1753267536) ^ 0x567C1D53 ^ 0xBE630EB;
																																																																																																																																																																																																																																																		continue;
																																																																																																																																																																																																																																																	}
																																																																																																																																																																																																																																																	if (num83 == 112 || num83 == 112)
																																																																																																																																																																																																																																																	{
																																																																																																																																																																																																																																																		if ("".Length != 0)
																																																																																																																																																																																																																																																		{
																																																																																																																																																																																																																																																		}
																																																																																																																																																																																																																																																		int num129 = (int)(num5 ^ 0x5D3368F6);
																																																																																																																																																																																																																																																		num80 = (num129 + -1742135966 - -1742135966) ^ 0x5D3368F6 ^ 0x6A8440F8;
																																																																																																																																																																																																																																																		continue;
																																																																																																																																																																																																																																																	}
																																																																																																																																																																																																																																																	if (num83 == 291 || num83 == 291)
																																																																																																																																																																																																																																																	{
																																																																																																																																																																																																																																																		if ((object)"" != "")
																																																																																																																																																																																																																																																		{
																																																																																																																																																																																																																																																		}
																																																																																																																																																																																																																																																		int num130 = (int)(num5 ^ 0x29E1B0AE);
																																																																																																																																																																																																																																																		num80 = (num130 + 1025104482 - 1025104482) ^ 0x29E1B0AE ^ 0x683374D2;
																																																																																																																																																																																																																																																		continue;
																																																																																																																																																																																																																																																	}
																																																																																																																																																																																																																																																	if (num83 != 292)
																																																																																																																																																																																																																																																	{
																																																																																																																																																																																																																																																		if (num83 != 113 && num83 != 113)
																																																																																																																																																																																																																																																		{
																																																																																																																																																																																																																																																			if (num83 == 294)
																																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																																				goto IL_4768;
																																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																																			if (num83 != 114 && num83 != 114)
																																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																																				if (num83 != 296 && num83 != 296)
																																																																																																																																																																																																																																																				{
																																																																																																																																																																																																																																																					if (num83 != 297 && num83 != 297)
																																																																																																																																																																																																																																																					{
																																																																																																																																																																																																																																																						if (num83 != 115 && num83 != 115)
																																																																																																																																																																																																																																																						{
																																																																																																																																																																																																																																																							if (num83 == 299 || num83 == 299)
																																																																																																																																																																																																																																																							{
																																																																																																																																																																																																																																																								if ((object)"" != "")
																																																																																																																																																																																																																																																								{
																																																																																																																																																																																																																																																								}
																																																																																																																																																																																																																																																								int num131 = (int)(num5 ^ 0x3C700525);
																																																																																																																																																																																																																																																								num80 = (num131 + 220270578 - 220270578) ^ 0x3C700525 ^ 0x28795D98;
																																																																																																																																																																																																																																																								continue;
																																																																																																																																																																																																																																																							}
																																																																																																																																																																																																																																																							if (num83 == 300 || num83 == 300)
																																																																																																																																																																																																																																																							{
																																																																																																																																																																																																																																																								if ((object)"" == "" && "".Length == 0 && (object)"" == "")
																																																																																																																																																																																																																																																								{
																																																																																																																																																																																																																																																									UI.RichLogs(msg20, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																																																																																																																																																																																																								}
																																																																																																																																																																																																																																																								int num132 = (int)(num5 ^ 0x2F52C8C3);
																																																																																																																																																																																																																																																								num80 = (num132 + -530636895 - -530636895) ^ 0x2F52C8C3 ^ 0x509CDBF3;
																																																																																																																																																																																																																																																								continue;
																																																																																																																																																																																																																																																							}
																																																																																																																																																																																																																																																							if (num83 == 116 || num83 == 116)
																																																																																																																																																																																																																																																							{
																																																																																																																																																																																																																																																								num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -1848717975;
																																																																																																																																																																																																																																																								continue;
																																																																																																																																																																																																																																																							}
																																																																																																																																																																																																																																																							if (num83 == 302 || num83 == 302)
																																																																																																																																																																																																																																																							{
																																																																																																																																																																																																																																																								if ("".Length == 0)
																																																																																																																																																																																																																																																								{
																																																																																																																																																																																																																																																									UI.RichLogs(text3, UI.info, (byte)num18 != 0, (byte)num18 != 0);
																																																																																																																																																																																																																																																								}
																																																																																																																																																																																																																																																								num80 = (int)((num5 << 3) | (num5 >> 29)) ^ -1005145677;
																																																																																																																																																																																																																																																								continue;
																																																																																																																																																																																																																																																							}
																																																																																																																																																																																																																																																							if (num83 == 117 || num83 == 117)
																																																																																																																																																																																																																																																							{
																																																																																																																																																																																																																																																								if ((object)"" != "")
																																																																																																																																																																																																																																																								{
																																																																																																																																																																																																																																																								}
																																																																																																																																																																																																																																																								int num133 = (int)(num5 ^ 0x7C3439FB);
																																																																																																																																																																																																																																																								num80 = (num133 + 1094993506 - 1094993506) ^ 0x7C3439FB ^ 0x5803BDEE;
																																																																																																																																																																																																																																																								continue;
																																																																																																																																																																																																																																																							}
																																																																																																																																																																																																																																																							if (num83 == 304 || num83 == 304)
																																																																																																																																																																																																																																																							{
																																																																																																																																																																																																																																																								int num134 = (int)(num5 ^ 0x166FF698);
																																																																																																																																																																																																																																																								num80 = (num134 + -704592922 - -704592922) ^ 0x166FF698 ^ 0x6F1CD744;
																																																																																																																																																																																																																																																								continue;
																																																																																																																																																																																																																																																							}
																																																																																																																																																																																																																																																							if (num83 != 305)
																																																																																																																																																																																																																																																							{
																																																																																																																																																																																																																																																								if (num83 != 118)
																																																																																																																																																																																																																																																								{
																																																																																																																																																																																																																																																									if (num83 != 307 && num83 != 307)
																																																																																																																																																																																																																																																									{
																																																																																																																																																																																																																																																										if (num83 != 119 && num83 != 119)
																																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																																											if (num83 == 309)
																																																																																																																																																																																																																																																											{
																																																																																																																																																																																																																																																												goto IL_72f0;
																																																																																																																																																																																																																																																											}
																																																																																																																																																																																																																																																											goto IL_73cb;
																																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																																										if ((object)"" != "")
																																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																																											num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -1630455399;
																																																																																																																																																																																																																																																											continue;
																																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																																										goto end_IL_73fb;
																																																																																																																																																																																																																																																									}
																																																																																																																																																																																																																																																									if ((object)"" != "")
																																																																																																																																																																																																																																																									{
																																																																																																																																																																																																																																																									}
																																																																																																																																																																																																																																																									num80 = (int)(0x596BDC82 ^ num5);
																																																																																																																																																																																																																																																									continue;
																																																																																																																																																																																																																																																								}
																																																																																																																																																																																																																																																								if ("".Length == 0 && "".Length == 0)
																																																																																																																																																																																																																																																								{
																																																																																																																																																																																																																																																									goto IL_70a5;
																																																																																																																																																																																																																																																								}
																																																																																																																																																																																																																																																								goto IL_70ac;
																																																																																																																																																																																																																																																							}
																																																																																																																																																																																																																																																						}
																																																																																																																																																																																																																																																						else if ((object)"" != "" || flag9)
																																																																																																																																																																																																																																																						{
																																																																																																																																																																																																																																																							num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x1244372C);
																																																																																																																																																																																																																																																							continue;
																																																																																																																																																																																																																																																						}
																																																																																																																																																																																																																																																						num80 = 343883081;
																																																																																																																																																																																																																																																					}
																																																																																																																																																																																																																																																					else
																																																																																																																																																																																																																																																					{
																																																																																																																																																																																																																																																						flag9 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																																																																																						int num135 = (int)(num5 ^ 0x68A44BA9);
																																																																																																																																																																																																																																																						num80 = (num135 + 688037052 - 688037052) ^ 0x68A44BA9 ^ 0x28B502EE;
																																																																																																																																																																																																																																																					}
																																																																																																																																																																																																																																																				}
																																																																																																																																																																																																																																																				else
																																																																																																																																																																																																																																																				{
																																																																																																																																																																																																																																																					int num136 = (int)num5 ^ -467974159;
																																																																																																																																																																																																																																																					num80 = (num136 + -607966738 - -607966738) ^ -467974159 ^ 0x77018FFC;
																																																																																																																																																																																																																																																				}
																																																																																																																																																																																																																																																				continue;
																																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																																			if ("".Length != 0 || flag10)
																																																																																																																																																																																																																																																			{
																																																																																																																																																																																																																																																				num80 = (int)(0x6FD92B8B ^ num5);
																																																																																																																																																																																																																																																				continue;
																																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																																		}
																																																																																																																																																																																																																																																		else if ("".Length != 0)
																																																																																																																																																																																																																																																		{
																																																																																																																																																																																																																																																			int num137 = (int)(num5 ^ 0x42CAE554);
																																																																																																																																																																																																																																																			num80 = (num137 + -86533328 - -86533328) ^ 0x42CAE554 ^ 0x6CD241B1;
																																																																																																																																																																																																																																																			continue;
																																																																																																																																																																																																																																																		}
																																																																																																																																																																																																																																																		goto IL_70a5;
																																																																																																																																																																																																																																																	}
																																																																																																																																																																																																																																																}
																																																																																																																																																																																																																																																else if ("".Length != 0 || "".Length != 0 || flag11)
																																																																																																																																																																																																																																																{
																																																																																																																																																																																																																																																	int num138 = (int)num5 ^ -2012119766;
																																																																																																																																																																																																																																																	num80 = (num138 + -797655466 - -797655466) ^ -2012119766 ^ 0xCB1E852;
																																																																																																																																																																																																																																																	continue;
																																																																																																																																																																																																																																																}
																																																																																																																																																																																																																																																num80 = 457239466;
																																																																																																																																																																																																																																															}
																																																																																																																																																																																																																																															else
																																																																																																																																																																																																																																															{
																																																																																																																																																																																																																																																if ("".Length == 0)
																																																																																																																																																																																																																																																{
																																																																																																																																																																																																																																																	flag11 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																																																																																}
																																																																																																																																																																																																																																																num80 = (int)(0x161A6CD2 ^ num5);
																																																																																																																																																																																																																																															}
																																																																																																																																																																																																																																														}
																																																																																																																																																																																																																																														else
																																																																																																																																																																																																																																														{
																																																																																																																																																																																																																																															if ((object)"" != "")
																																																																																																																																																																																																																																															{
																																																																																																																																																																																																																																															}
																																																																																																																																																																																																																																															num80 = (int)(0x54B958D3 ^ num5);
																																																																																																																																																																																																																																														}
																																																																																																																																																																																																																																														continue;
																																																																																																																																																																																																																																													}
																																																																																																																																																																																																																																													if (flag12)
																																																																																																																																																																																																																																													{
																																																																																																																																																																																																																																														int num139 = (int)num5 ^ -1705493767;
																																																																																																																																																																																																																																														num80 = (num139 + 601429058 - 601429058) ^ -1705493767 ^ 0x4BC0CBFC;
																																																																																																																																																																																																																																														continue;
																																																																																																																																																																																																																																													}
																																																																																																																																																																																																																																													goto IL_4768;
																																																																																																																																																																																																																																												}
																																																																																																																																																																																																																																												goto IL_4970;
																																																																																																																																																																																																																																											}
																																																																																																																																																																																																																																											goto IL_70a5;
																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																										if ("".Length == 0 && (object)"" != "")
																																																																																																																																																																																																																																										{
																																																																																																																																																																																																																																										}
																																																																																																																																																																																																																																									}
																																																																																																																																																																																																																																									else if ("".Length != 0 || flag13)
																																																																																																																																																																																																																																									{
																																																																																																																																																																																																																																										num80 = (int)(0x37BD092C ^ num5);
																																																																																																																																																																																																																																										continue;
																																																																																																																																																																																																																																									}
																																																																																																																																																																																																																																									num80 = 1093694430;
																																																																																																																																																																																																																																								}
																																																																																																																																																																																																																																								else
																																																																																																																																																																																																																																								{
																																																																																																																																																																																																																																									if ("".Length == 0)
																																																																																																																																																																																																																																									{
																																																																																																																																																																																																																																										flag13 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																																																																									}
																																																																																																																																																																																																																																									num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -1230959355;
																																																																																																																																																																																																																																								}
																																																																																																																																																																																																																																							}
																																																																																																																																																																																																																																							else
																																																																																																																																																																																																																																							{
																																																																																																																																																																																																																																								if ((object)"" != "" || (object)"" != "")
																																																																																																																																																																																																																																								{
																																																																																																																																																																																																																																								}
																																																																																																																																																																																																																																								num80 = (int)(0x5D34C027 ^ num5);
																																																																																																																																																																																																																																							}
																																																																																																																																																																																																																																							continue;
																																																																																																																																																																																																																																						}
																																																																																																																																																																																																																																						if ((object)"" != "" || flag14)
																																																																																																																																																																																																																																						{
																																																																																																																																																																																																																																							int num140 = (int)(num5 ^ 0x7146A033);
																																																																																																																																																																																																																																							num80 = (num140 + 1772764284 - 1772764284) ^ 0x7146A033 ^ 0xFA559A9;
																																																																																																																																																																																																																																							continue;
																																																																																																																																																																																																																																						}
																																																																																																																																																																																																																																						goto IL_4970;
																																																																																																																																																																																																																																					}
																																																																																																																																																																																																																																					goto IL_4b2f;
																																																																																																																																																																																																																																				}
																																																																																																																																																																																																																																				goto IL_70a5;
																																																																																																																																																																																																																																			}
																																																																																																																																																																																																																																		}
																																																																																																																																																																																																																																		else if ((object)"" != "" || flag15)
																																																																																																																																																																																																																																		{
																																																																																																																																																																																																																																			int num141 = (int)num5 ^ -106097804;
																																																																																																																																																																																																																																			num80 = (num141 + -1871342878 - -1871342878) ^ -106097804 ^ 0x401EFE6B;
																																																																																																																																																																																																																																			continue;
																																																																																																																																																																																																																																		}
																																																																																																																																																																																																																																		num80 = 992831527;
																																																																																																																																																																																																																																	}
																																																																																																																																																																																																																																	else
																																																																																																																																																																																																																																	{
																																																																																																																																																																																																																																		if ("".Length == 0)
																																																																																																																																																																																																																																		{
																																																																																																																																																																																																																																			flag15 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																																																																		}
																																																																																																																																																																																																																																		num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x6C904F34);
																																																																																																																																																																																																																																	}
																																																																																																																																																																																																																																}
																																																																																																																																																																																																																																else
																																																																																																																																																																																																																																{
																																																																																																																																																																																																																																	num80 = (int)(0x1426B576 ^ num5);
																																																																																																																																																																																																																																}
																																																																																																																																																																																																																																continue;
																																																																																																																																																																																																																															}
																																																																																																																																																																																																																															if (flag16)
																																																																																																																																																																																																																															{
																																																																																																																																																																																																																																num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x34499DBC);
																																																																																																																																																																																																																																continue;
																																																																																																																																																																																																																															}
																																																																																																																																																																																																																															goto IL_4b2f;
																																																																																																																																																																																																																														}
																																																																																																																																																																																																																														if ((object)"" == "")
																																																																																																																																																																																																																														{
																																																																																																																																																																																																																															goto IL_4d56;
																																																																																																																																																																																																																														}
																																																																																																																																																																																																																														goto IL_4d61;
																																																																																																																																																																																																																													}
																																																																																																																																																																																																																													goto IL_70a5;
																																																																																																																																																																																																																												}
																																																																																																																																																																																																																											}
																																																																																																																																																																																																																											else if ("".Length != 0 || flag17)
																																																																																																																																																																																																																											{
																																																																																																																																																																																																																												int num142 = (int)(num5 ^ 0x7377EAC2);
																																																																																																																																																																																																																												num80 = (num142 + 1495911896 - 1495911896) ^ 0x7377EAC2 ^ 0xEEC8C02;
																																																																																																																																																																																																																												continue;
																																																																																																																																																																																																																											}
																																																																																																																																																																																																																											num80 = 2108170511;
																																																																																																																																																																																																																										}
																																																																																																																																																																																																																										else
																																																																																																																																																																																																																										{
																																																																																																																																																																																																																											if ("".Length == 0 && (object)"" == "")
																																																																																																																																																																																																																											{
																																																																																																																																																																																																																												flag17 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																																																											}
																																																																																																																																																																																																																											num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -2049791366;
																																																																																																																																																																																																																										}
																																																																																																																																																																																																																									}
																																																																																																																																																																																																																									else
																																																																																																																																																																																																																									{
																																																																																																																																																																																																																										int num143 = (int)num5 ^ -1253774779;
																																																																																																																																																																																																																										num80 = (num143 + -394721954 - -394721954) ^ -1253774779 ^ 0x3ABB24F5;
																																																																																																																																																																																																																									}
																																																																																																																																																																																																																									continue;
																																																																																																																																																																																																																								}
																																																																																																																																																																																																																								if ("".Length != 0 || "".Length != 0 || flag18)
																																																																																																																																																																																																																								{
																																																																																																																																																																																																																									int num144 = (int)num5 ^ -198143850;
																																																																																																																																																																																																																									num80 = (num144 + -1686920529 - -1686920529) ^ -198143850 ^ 0x16692622;
																																																																																																																																																																																																																									continue;
																																																																																																																																																																																																																								}
																																																																																																																																																																																																																								goto IL_4d56;
																																																																																																																																																																																																																							}
																																																																																																																																																																																																																							goto IL_4f3d;
																																																																																																																																																																																																																						}
																																																																																																																																																																																																																						goto IL_70a5;
																																																																																																																																																																																																																					}
																																																																																																																																																																																																																					if ("".Length == 0 && "".Length != 0)
																																																																																																																																																																																																																					{
																																																																																																																																																																																																																					}
																																																																																																																																																																																																																				}
																																																																																																																																																																																																																				else if ((object)"" != "" || flag19)
																																																																																																																																																																																																																				{
																																																																																																																																																																																																																					int num145 = (int)num5 ^ -1845807627;
																																																																																																																																																																																																																					num80 = (num145 + 1196130082 - 1196130082) ^ -1845807627 ^ 0x318935CD;
																																																																																																																																																																																																																					continue;
																																																																																																																																																																																																																				}
																																																																																																																																																																																																																				num80 = 1942165548;
																																																																																																																																																																																																																			}
																																																																																																																																																																																																																			else
																																																																																																																																																																																																																			{
																																																																																																																																																																																																																				flag19 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																																																				num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x69A2EC43);
																																																																																																																																																																																																																			}
																																																																																																																																																																																																																		}
																																																																																																																																																																																																																		else
																																																																																																																																																																																																																		{
																																																																																																																																																																																																																			int num146 = (int)num5 ^ -255716278;
																																																																																																																																																																																																																			num80 = (num146 + 1906019402 - 1906019402) ^ -255716278 ^ 0x796023C0;
																																																																																																																																																																																																																		}
																																																																																																																																																																																																																		continue;
																																																																																																																																																																																																																	}
																																																																																																																																																																																																																	if ("".Length != 0 || flag20)
																																																																																																																																																																																																																	{
																																																																																																																																																																																																																		num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x30C30EF5);
																																																																																																																																																																																																																		continue;
																																																																																																																																																																																																																	}
																																																																																																																																																																																																																	goto IL_4f3d;
																																																																																																																																																																																																																}
																																																																																																																																																																																																																if ("".Length == 0)
																																																																																																																																																																																																																{
																																																																																																																																																																																																																	goto IL_516a;
																																																																																																																																																																																																																}
																																																																																																																																																																																																																goto IL_5175;
																																																																																																																																																																																																															}
																																																																																																																																																																																																															goto IL_70a5;
																																																																																																																																																																																																														}
																																																																																																																																																																																																														if ((object)"" != "")
																																																																																																																																																																																																														{
																																																																																																																																																																																																														}
																																																																																																																																																																																																													}
																																																																																																																																																																																																													else if ((object)"" != "" || flag21)
																																																																																																																																																																																																													{
																																																																																																																																																																																																														int num147 = (int)num5 ^ -138843476;
																																																																																																																																																																																																														num80 = (num147 + -532055722 - -532055722) ^ -138843476 ^ 0x76127947;
																																																																																																																																																																																																														continue;
																																																																																																																																																																																																													}
																																																																																																																																																																																																													num80 = 1197491422;
																																																																																																																																																																																																												}
																																																																																																																																																																																																												else
																																																																																																																																																																																																												{
																																																																																																																																																																																																													if ((object)"" == "")
																																																																																																																																																																																																													{
																																																																																																																																																																																																														flag21 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																																													}
																																																																																																																																																																																																													num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -554054333;
																																																																																																																																																																																																												}
																																																																																																																																																																																																											}
																																																																																																																																																																																																											else
																																																																																																																																																																																																											{
																																																																																																																																																																																																												if ((object)"" != "")
																																																																																																																																																																																																												{
																																																																																																																																																																																																												}
																																																																																																																																																																																																												num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x7C3D5E4D);
																																																																																																																																																																																																											}
																																																																																																																																																																																																											continue;
																																																																																																																																																																																																										}
																																																																																																																																																																																																										if (flag22)
																																																																																																																																																																																																										{
																																																																																																																																																																																																											int num148 = (int)(num5 ^ 0x6974223E);
																																																																																																																																																																																																											num80 = (num148 + 1366063578 - 1366063578) ^ 0x6974223E ^ 0x2C352A70;
																																																																																																																																																																																																											continue;
																																																																																																																																																																																																										}
																																																																																																																																																																																																										goto IL_516a;
																																																																																																																																																																																																									}
																																																																																																																																																																																																									if ("".Length == 0)
																																																																																																																																																																																																									{
																																																																																																																																																																																																										goto IL_53a4;
																																																																																																																																																																																																									}
																																																																																																																																																																																																									goto IL_53af;
																																																																																																																																																																																																								}
																																																																																																																																																																																																								if ((object)"" != "")
																																																																																																																																																																																																								{
																																																																																																																																																																																																									num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x5C2C0BBA);
																																																																																																																																																																																																									continue;
																																																																																																																																																																																																								}
																																																																																																																																																																																																								goto IL_70a5;
																																																																																																																																																																																																							}
																																																																																																																																																																																																						}
																																																																																																																																																																																																						else if ((object)"" != "" || flag23)
																																																																																																																																																																																																						{
																																																																																																																																																																																																							num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x3A9791FC);
																																																																																																																																																																																																							continue;
																																																																																																																																																																																																						}
																																																																																																																																																																																																						num80 = 1345292293;
																																																																																																																																																																																																					}
																																																																																																																																																																																																					else
																																																																																																																																																																																																					{
																																																																																																																																																																																																						if ((object)"" == "" && (object)"" == "")
																																																																																																																																																																																																						{
																																																																																																																																																																																																							flag23 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																																						}
																																																																																																																																																																																																						num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -1387369689;
																																																																																																																																																																																																					}
																																																																																																																																																																																																				}
																																																																																																																																																																																																				else
																																																																																																																																																																																																				{
																																																																																																																																																																																																					if ((object)"" != "")
																																																																																																																																																																																																					{
																																																																																																																																																																																																					}
																																																																																																																																																																																																					num80 = (int)(0x3B258D7F ^ num5);
																																																																																																																																																																																																				}
																																																																																																																																																																																																				continue;
																																																																																																																																																																																																			}
																																																																																																																																																																																																			if (flag24)
																																																																																																																																																																																																			{
																																																																																																																																																																																																				int num149 = (int)num5 ^ -819285109;
																																																																																																																																																																																																				num80 = (num149 + -1178564597 - -1178564597) ^ -819285109 ^ 0x5D791706;
																																																																																																																																																																																																				continue;
																																																																																																																																																																																																			}
																																																																																																																																																																																																			goto IL_53a4;
																																																																																																																																																																																																		}
																																																																																																																																																																																																		if ((object)"" == "" && (object)"" == "")
																																																																																																																																																																																																		{
																																																																																																																																																																																																			goto IL_55b9;
																																																																																																																																																																																																		}
																																																																																																																																																																																																		goto IL_55c4;
																																																																																																																																																																																																	}
																																																																																																																																																																																																	goto IL_70a5;
																																																																																																																																																																																																}
																																																																																																																																																																																																if ((object)"" == "" && (object)"" != "")
																																																																																																																																																																																																{
																																																																																																																																																																																																}
																																																																																																																																																																																															}
																																																																																																																																																																																															else if (flag25)
																																																																																																																																																																																															{
																																																																																																																																																																																																num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x5C458E1D);
																																																																																																																																																																																																continue;
																																																																																																																																																																																															}
																																																																																																																																																																																															num80 = 2057009249;
																																																																																																																																																																																														}
																																																																																																																																																																																														else
																																																																																																																																																																																														{
																																																																																																																																																																																															flag25 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																															num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -1802016912;
																																																																																																																																																																																														}
																																																																																																																																																																																													}
																																																																																																																																																																																													else
																																																																																																																																																																																													{
																																																																																																																																																																																														if ((object)"" != "" || (object)"" != "")
																																																																																																																																																																																														{
																																																																																																																																																																																														}
																																																																																																																																																																																														num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x543CC97);
																																																																																																																																																																																													}
																																																																																																																																																																																													continue;
																																																																																																																																																																																												}
																																																																																																																																																																																												if ("".Length != 0 || flag26)
																																																																																																																																																																																												{
																																																																																																																																																																																													int num150 = (int)num5 ^ -342520028;
																																																																																																																																																																																													num80 = (num150 + 1265526210 - 1265526210) ^ -342520028 ^ 0x47307EB6;
																																																																																																																																																																																													continue;
																																																																																																																																																																																												}
																																																																																																																																																																																												goto IL_55b9;
																																																																																																																																																																																											}
																																																																																																																																																																																											goto IL_57b8;
																																																																																																																																																																																										}
																																																																																																																																																																																										goto IL_70a5;
																																																																																																																																																																																									}
																																																																																																																																																																																								}
																																																																																																																																																																																								else if ("".Length != 0 || flag27)
																																																																																																																																																																																								{
																																																																																																																																																																																									num80 = (int)(0x4AFBC9FB ^ num5);
																																																																																																																																																																																									continue;
																																																																																																																																																																																								}
																																																																																																																																																																																								num80 = 1365643485;
																																																																																																																																																																																							}
																																																																																																																																																																																							else
																																																																																																																																																																																							{
																																																																																																																																																																																								if ((object)"" == "" && (object)"" == "")
																																																																																																																																																																																								{
																																																																																																																																																																																									flag27 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																								}
																																																																																																																																																																																								int num151 = (int)num5 ^ -1728208207;
																																																																																																																																																																																								num80 = (num151 + -844428029 - -844428029) ^ -1728208207 ^ 0x7B82E457;
																																																																																																																																																																																							}
																																																																																																																																																																																						}
																																																																																																																																																																																						else
																																																																																																																																																																																						{
																																																																																																																																																																																							if ((object)"" != "")
																																																																																																																																																																																							{
																																																																																																																																																																																							}
																																																																																																																																																																																							num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1255652B);
																																																																																																																																																																																						}
																																																																																																																																																																																						continue;
																																																																																																																																																																																					}
																																																																																																																																																																																					if (flag28)
																																																																																																																																																																																					{
																																																																																																																																																																																						num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x7D5C75F5);
																																																																																																																																																																																						continue;
																																																																																																																																																																																					}
																																																																																																																																																																																					goto IL_57b8;
																																																																																																																																																																																				}
																																																																																																																																																																																				if ("".Length == 0)
																																																																																																																																																																																				{
																																																																																																																																																																																					goto IL_59cd;
																																																																																																																																																																																				}
																																																																																																																																																																																				goto IL_59d8;
																																																																																																																																																																																			}
																																																																																																																																																																																			if ((object)"" != "")
																																																																																																																																																																																			{
																																																																																																																																																																																				int num152 = (int)num5 ^ -296747825;
																																																																																																																																																																																				num80 = (num152 + 1932754310 - 1932754310) ^ -296747825 ^ 0xFDADA2E;
																																																																																																																																																																																				continue;
																																																																																																																																																																																			}
																																																																																																																																																																																			goto IL_70a5;
																																																																																																																																																																																		}
																																																																																																																																																																																	}
																																																																																																																																																																																	else if (flag29)
																																																																																																																																																																																	{
																																																																																																																																																																																		int num153 = (int)num5 ^ -1527895391;
																																																																																																																																																																																		num80 = (num153 + 1399030489 - 1399030489) ^ -1527895391 ^ 0x486933F7;
																																																																																																																																																																																		continue;
																																																																																																																																																																																	}
																																																																																																																																																																																	num80 = 2145393557;
																																																																																																																																																																																}
																																																																																																																																																																																else
																																																																																																																																																																																{
																																																																																																																																																																																	if ("".Length == 0)
																																																																																																																																																																																	{
																																																																																																																																																																																		flag29 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																																	}
																																																																																																																																																																																	int num154 = (int)num5 ^ -1202634395;
																																																																																																																																																																																	num80 = (num154 + 104177712 - 104177712) ^ -1202634395 ^ 0x1137B7F2;
																																																																																																																																																																																}
																																																																																																																																																																															}
																																																																																																																																																																															else
																																																																																																																																																																															{
																																																																																																																																																																																if ("".Length != 0)
																																																																																																																																																																																{
																																																																																																																																																																																}
																																																																																																																																																																																int num155 = (int)num5 ^ -1394983945;
																																																																																																																																																																																num80 = (num155 + -1978731987 - -1978731987) ^ -1394983945 ^ 0x711A94C2;
																																																																																																																																																																															}
																																																																																																																																																																															continue;
																																																																																																																																																																														}
																																																																																																																																																																														if ((object)"" != "" || flag30)
																																																																																																																																																																														{
																																																																																																																																																																															num80 = (int)(0x42A1BAC0 ^ num5);
																																																																																																																																																																															continue;
																																																																																																																																																																														}
																																																																																																																																																																														goto IL_59cd;
																																																																																																																																																																													}
																																																																																																																																																																													if ((object)"" == "")
																																																																																																																																																																													{
																																																																																																																																																																														goto IL_5c20;
																																																																																																																																																																													}
																																																																																																																																																																													goto IL_5c2b;
																																																																																																																																																																												}
																																																																																																																																																																												if ((object)"" != "")
																																																																																																																																																																												{
																																																																																																																																																																													num80 = (int)(0x68C4216F ^ num5);
																																																																																																																																																																													continue;
																																																																																																																																																																												}
																																																																																																																																																																												goto IL_70a5;
																																																																																																																																																																											}
																																																																																																																																																																											if ((object)"" == "" && (object)"" != "")
																																																																																																																																																																											{
																																																																																																																																																																											}
																																																																																																																																																																										}
																																																																																																																																																																										else if ((object)"" != "" || flag31)
																																																																																																																																																																										{
																																																																																																																																																																											num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -695288021;
																																																																																																																																																																											continue;
																																																																																																																																																																										}
																																																																																																																																																																										num80 = 1612077682;
																																																																																																																																																																									}
																																																																																																																																																																									else
																																																																																																																																																																									{
																																																																																																																																																																										if ("".Length == 0 && (object)"" == "")
																																																																																																																																																																										{
																																																																																																																																																																											flag31 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																										}
																																																																																																																																																																										num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x3F255227);
																																																																																																																																																																									}
																																																																																																																																																																								}
																																																																																																																																																																								else
																																																																																																																																																																								{
																																																																																																																																																																									num80 = (int)(0xEE450E1 ^ num5);
																																																																																																																																																																								}
																																																																																																																																																																								continue;
																																																																																																																																																																							}
																																																																																																																																																																							if ((object)"" != "" || (object)"" != "" || flag32)
																																																																																																																																																																							{
																																																																																																																																																																								num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x20E4B328);
																																																																																																																																																																								continue;
																																																																																																																																																																							}
																																																																																																																																																																							goto IL_5c20;
																																																																																																																																																																						}
																																																																																																																																																																						if ("".Length == 0)
																																																																																																																																																																						{
																																																																																																																																																																							goto IL_5e14;
																																																																																																																																																																						}
																																																																																																																																																																						goto IL_5e1f;
																																																																																																																																																																					}
																																																																																																																																																																					goto IL_70a5;
																																																																																																																																																																				}
																																																																																																																																																																				if ("".Length != 0)
																																																																																																																																																																				{
																																																																																																																																																																				}
																																																																																																																																																																			}
																																																																																																																																																																			else if ("".Length != 0 || flag33)
																																																																																																																																																																			{
																																																																																																																																																																				int num156 = (int)num5 ^ -424935663;
																																																																																																																																																																				num80 = (num156 + -1405765452 - -1405765452) ^ -424935663 ^ 0x2BEED123;
																																																																																																																																																																				continue;
																																																																																																																																																																			}
																																																																																																																																																																			num80 = 1369823908;
																																																																																																																																																																		}
																																																																																																																																																																		else
																																																																																																																																																																		{
																																																																																																																																																																			if ("".Length == 0)
																																																																																																																																																																			{
																																																																																																																																																																				flag33 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																																			}
																																																																																																																																																																			int num157 = (int)(num5 ^ 0x44D334C0);
																																																																																																																																																																			num80 = (num157 + -1084565276 - -1084565276) ^ 0x44D334C0 ^ 0x4D730AD;
																																																																																																																																																																		}
																																																																																																																																																																	}
																																																																																																																																																																	else
																																																																																																																																																																	{
																																																																																																																																																																		num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -1344857061;
																																																																																																																																																																	}
																																																																																																																																																																	continue;
																																																																																																																																																																}
																																																																																																																																																																if ("".Length != 0 || flag34)
																																																																																																																																																																{
																																																																																																																																																																	int num158 = (int)num5 ^ -1790374092;
																																																																																																																																																																	num80 = (num158 + -690357859 - -690357859) ^ -1790374092 ^ 0x7DF6C6D3;
																																																																																																																																																																	continue;
																																																																																																																																																																}
																																																																																																																																																																goto IL_5e14;
																																																																																																																																																															}
																																																																																																																																																															if ("".Length == 0 && "".Length == 0 && (object)"" == "")
																																																																																																																																																															{
																																																																																																																																																																goto IL_600d;
																																																																																																																																																															}
																																																																																																																																																															goto IL_6018;
																																																																																																																																																														}
																																																																																																																																																														if ("".Length != 0 || (object)"" != "")
																																																																																																																																																														{
																																																																																																																																																															int num159 = (int)(num5 ^ 0x2B6F697);
																																																																																																																																																															num80 = (num159 + -1811926494 - -1811926494) ^ 0x2B6F697 ^ 0x774BDC3E;
																																																																																																																																																															continue;
																																																																																																																																																														}
																																																																																																																																																														goto IL_70a5;
																																																																																																																																																													}
																																																																																																																																																												}
																																																																																																																																																												else if (flag35)
																																																																																																																																																												{
																																																																																																																																																													num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x654C2962);
																																																																																																																																																													continue;
																																																																																																																																																												}
																																																																																																																																																												num80 = 27505599;
																																																																																																																																																											}
																																																																																																																																																											else
																																																																																																																																																											{
																																																																																																																																																												if ((object)"" == "")
																																																																																																																																																												{
																																																																																																																																																													flag35 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																												}
																																																																																																																																																												int num160 = (int)num5 ^ -843137569;
																																																																																																																																																												num80 = (num160 + 455606998 - 455606998) ^ -843137569 ^ 0x78019242;
																																																																																																																																																											}
																																																																																																																																																										}
																																																																																																																																																										else
																																																																																																																																																										{
																																																																																																																																																											if ("".Length != 0)
																																																																																																																																																											{
																																																																																																																																																											}
																																																																																																																																																											num80 = (int)(0x5E263260 ^ num5);
																																																																																																																																																										}
																																																																																																																																																										continue;
																																																																																																																																																									}
																																																																																																																																																									if ("".Length != 0 || flag36)
																																																																																																																																																									{
																																																																																																																																																										num80 = (int)(0x724A8BE3 ^ num5);
																																																																																																																																																										continue;
																																																																																																																																																									}
																																																																																																																																																									goto IL_600d;
																																																																																																																																																								}
																																																																																																																																																								if ((object)"" == "")
																																																																																																																																																								{
																																																																																																																																																									goto IL_625a;
																																																																																																																																																								}
																																																																																																																																																								goto IL_6265;
																																																																																																																																																							}
																																																																																																																																																							if ("".Length != 0 || "".Length != 0)
																																																																																																																																																							{
																																																																																																																																																								num80 = (int)(0x6629BC8C ^ num5);
																																																																																																																																																								continue;
																																																																																																																																																							}
																																																																																																																																																							goto IL_70a5;
																																																																																																																																																						}
																																																																																																																																																						if ("".Length == 0 && "".Length != 0)
																																																																																																																																																						{
																																																																																																																																																						}
																																																																																																																																																					}
																																																																																																																																																					else if ("".Length != 0 || "".Length != 0 || (object)"" != "" || flag37)
																																																																																																																																																					{
																																																																																																																																																						int num161 = (int)num5 ^ -1288021811;
																																																																																																																																																						num80 = (num161 + -743854063 - -743854063) ^ -1288021811 ^ 0x22CD3AB;
																																																																																																																																																						continue;
																																																																																																																																																					}
																																																																																																																																																					num80 = 519580671;
																																																																																																																																																				}
																																																																																																																																																				else
																																																																																																																																																				{
																																																																																																																																																					if ((object)"" == "")
																																																																																																																																																					{
																																																																																																																																																						flag37 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																																					}
																																																																																																																																																					num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -729387898;
																																																																																																																																																				}
																																																																																																																																																			}
																																																																																																																																																			else
																																																																																																																																																			{
																																																																																																																																																				if ((object)"" != "")
																																																																																																																																																				{
																																																																																																																																																				}
																																																																																																																																																				num80 = (int)(0xC5A3544 ^ num5);
																																																																																																																																																			}
																																																																																																																																																			continue;
																																																																																																																																																		}
																																																																																																																																																		if ((object)"" != "" || "".Length != 0 || flag38)
																																																																																																																																																		{
																																																																																																																																																			num80 = (int)(0x4BAB260 ^ num5);
																																																																																																																																																			continue;
																																																																																																																																																		}
																																																																																																																																																		goto IL_625a;
																																																																																																																																																	}
																																																																																																																																																	if ("".Length == 0 && "".Length == 0)
																																																																																																																																																	{
																																																																																																																																																		goto IL_6489;
																																																																																																																																																	}
																																																																																																																																																	goto IL_6494;
																																																																																																																																																}
																																																																																																																																																if ((object)"" != "" || "".Length != 0)
																																																																																																																																																{
																																																																																																																																																	num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x2AE377F4);
																																																																																																																																																	continue;
																																																																																																																																																}
																																																																																																																																																goto IL_70a5;
																																																																																																																																															}
																																																																																																																																															if ("".Length == 0 && (object)"" == "" && (object)"" != "")
																																																																																																																																															{
																																																																																																																																															}
																																																																																																																																														}
																																																																																																																																														else if (flag39)
																																																																																																																																														{
																																																																																																																																															num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -374800092;
																																																																																																																																															continue;
																																																																																																																																														}
																																																																																																																																														num80 = 745192050;
																																																																																																																																													}
																																																																																																																																													else
																																																																																																																																													{
																																																																																																																																														if ((object)"" == "" && (object)"" == "")
																																																																																																																																														{
																																																																																																																																															flag39 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																														}
																																																																																																																																														num80 = (int)(0x28F69448 ^ num5);
																																																																																																																																													}
																																																																																																																																												}
																																																																																																																																												else
																																																																																																																																												{
																																																																																																																																													if ((object)"" != "")
																																																																																																																																													{
																																																																																																																																													}
																																																																																																																																													int num162 = (int)num5 ^ -2134395422;
																																																																																																																																													num80 = (num162 + 1825947071 - 1825947071) ^ -2134395422 ^ 0x326691BE;
																																																																																																																																												}
																																																																																																																																												continue;
																																																																																																																																											}
																																																																																																																																											if ((object)"" != "" || flag40)
																																																																																																																																											{
																																																																																																																																												num80 = (int)(0x3494E3D4 ^ num5);
																																																																																																																																												continue;
																																																																																																																																											}
																																																																																																																																											goto IL_6489;
																																																																																																																																										}
																																																																																																																																										goto IL_6673;
																																																																																																																																									}
																																																																																																																																									goto IL_70a5;
																																																																																																																																								}
																																																																																																																																								if ("".Length != 0)
																																																																																																																																								{
																																																																																																																																								}
																																																																																																																																							}
																																																																																																																																							else if ((object)"" != "" || "".Length != 0 || "".Length != 0 || flag41)
																																																																																																																																							{
																																																																																																																																								num80 = (int)(0x295F0BB4 ^ num5);
																																																																																																																																								continue;
																																																																																																																																							}
																																																																																																																																							num80 = 1958482011;
																																																																																																																																						}
																																																																																																																																						else
																																																																																																																																						{
																																																																																																																																							flag41 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																							int num163 = (int)num5 ^ -741596391;
																																																																																																																																							num80 = (num163 + -1424819640 - -1424819640) ^ -741596391 ^ 0x1F1AAFCF;
																																																																																																																																						}
																																																																																																																																					}
																																																																																																																																					else
																																																																																																																																					{
																																																																																																																																						if ((object)"" != "")
																																																																																																																																						{
																																																																																																																																						}
																																																																																																																																						num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -1200671246;
																																																																																																																																					}
																																																																																																																																					continue;
																																																																																																																																				}
																																																																																																																																				if ((object)"" != "" || flag42)
																																																																																																																																				{
																																																																																																																																					int num164 = (int)num5 ^ -436360751;
																																																																																																																																					num80 = (num164 + 764248505 - 764248505) ^ -436360751 ^ 0x79298051;
																																																																																																																																					continue;
																																																																																																																																				}
																																																																																																																																				goto IL_6673;
																																																																																																																																			}
																																																																																																																																			if ("".Length == 0 && (object)"" == "")
																																																																																																																																			{
																																																																																																																																				goto IL_684b;
																																																																																																																																			}
																																																																																																																																			goto IL_6856;
																																																																																																																																		}
																																																																																																																																		goto IL_70a5;
																																																																																																																																	}
																																																																																																																																}
																																																																																																																																else if ("".Length != 0 || (object)"" != "" || flag43)
																																																																																																																																{
																																																																																																																																	num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x3AA37A2B);
																																																																																																																																	continue;
																																																																																																																																}
																																																																																																																																num80 = 349772899;
																																																																																																																															}
																																																																																																																															else
																																																																																																																															{
																																																																																																																																flag43 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																																num80 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x3AE3AE0F);
																																																																																																																															}
																																																																																																																														}
																																																																																																																														else
																																																																																																																														{
																																																																																																																															if ("".Length != 0 || (object)"" != "")
																																																																																																																															{
																																																																																																																															}
																																																																																																																															num80 = (int)(0x7A3B9E78 ^ num5);
																																																																																																																														}
																																																																																																																														continue;
																																																																																																																													}
																																																																																																																													if (flag44)
																																																																																																																													{
																																																																																																																														num80 = (int)((num5 << 3) | (num5 >> 29)) ^ -898451026;
																																																																																																																														continue;
																																																																																																																													}
																																																																																																																													goto IL_684b;
																																																																																																																												}
																																																																																																																												goto IL_6a24;
																																																																																																																											}
																																																																																																																											if ((object)"" != "" || "".Length != 0 || "".Length != 0)
																																																																																																																											{
																																																																																																																												int num165 = (int)num5 ^ -534646924;
																																																																																																																												num80 = (num165 + 2095946009 - 2095946009) ^ -534646924 ^ 0x657BCFF8;
																																																																																																																												continue;
																																																																																																																											}
																																																																																																																											goto IL_70a5;
																																																																																																																										}
																																																																																																																									}
																																																																																																																									else if (flag45)
																																																																																																																									{
																																																																																																																										num80 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x4A728268);
																																																																																																																										continue;
																																																																																																																									}
																																																																																																																									num80 = 288834124;
																																																																																																																								}
																																																																																																																								else
																																																																																																																								{
																																																																																																																									if ("".Length == 0)
																																																																																																																									{
																																																																																																																										flag45 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																									}
																																																																																																																									num80 = (int)((num5 << 3) | (num5 >> 29)) ^ -2065885546;
																																																																																																																								}
																																																																																																																							}
																																																																																																																							else
																																																																																																																							{
																																																																																																																								if ((object)"" != "" || (object)"" != "")
																																																																																																																								{
																																																																																																																								}
																																																																																																																								num80 = (int)(0x7D53EC80 ^ num5);
																																																																																																																							}
																																																																																																																							continue;
																																																																																																																						}
																																																																																																																						if ("".Length != 0 || (object)"" != "" || "".Length != 0 || flag46)
																																																																																																																						{
																																																																																																																							num80 = (int)((num5 << 3) | (num5 >> 29)) ^ -1960936428;
																																																																																																																							continue;
																																																																																																																						}
																																																																																																																						goto IL_6a24;
																																																																																																																					}
																																																																																																																					if ("".Length == 0)
																																																																																																																					{
																																																																																																																						goto IL_6c75;
																																																																																																																					}
																																																																																																																					goto IL_6c80;
																																																																																																																				}
																																																																																																																				if ((object)"" != "" || "".Length != 0)
																																																																																																																				{
																																																																																																																					num80 = (int)(0x7372988F ^ num5);
																																																																																																																					continue;
																																																																																																																				}
																																																																																																																				goto IL_70a5;
																																																																																																																			}
																																																																																																																			if ((object)"" == "" && "".Length == 0 && "".Length != 0)
																																																																																																																			{
																																																																																																																			}
																																																																																																																		}
																																																																																																																		else if (flag47)
																																																																																																																		{
																																																																																																																			int num166 = (int)num5 ^ -113351218;
																																																																																																																			num80 = (num166 + -730140074 - -730140074) ^ -113351218 ^ 0x5CEA1452;
																																																																																																																			continue;
																																																																																																																		}
																																																																																																																		num80 = 1306747693;
																																																																																																																	}
																																																																																																																	else
																																																																																																																	{
																																																																																																																		if ("".Length == 0 && "".Length == 0)
																																																																																																																		{
																																																																																																																			flag47 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																																		}
																																																																																																																		int num167 = (int)num5 ^ -314807348;
																																																																																																																		num80 = (num167 + -256980558 - -256980558) ^ -314807348 ^ 0x7DD3AD74;
																																																																																																																	}
																																																																																																																}
																																																																																																																else
																																																																																																																{
																																																																																																																	int num168 = (int)num5 ^ -1727867597;
																																																																																																																	num80 = (num168 + -131178491 - -131178491) ^ -1727867597 ^ 0x10BC2CA2;
																																																																																																																}
																																																																																																																continue;
																																																																																																															}
																																																																																																															if ("".Length != 0 || flag48)
																																																																																																															{
																																																																																																																int num169 = (int)num5 ^ -1152675167;
																																																																																																																num80 = (num169 + -1338515372 - -1338515372) ^ -1152675167 ^ 0x6C1784EF;
																																																																																																																continue;
																																																																																																															}
																																																																																																															goto IL_6c75;
																																																																																																														}
																																																																																																														if ("".Length == 0)
																																																																																																														{
																																																																																																															goto IL_6e6a;
																																																																																																														}
																																																																																																														goto IL_6e75;
																																																																																																													}
																																																																																																													if ((object)"" != "")
																																																																																																													{
																																																																																																														num80 = (int)(0x3EDA4BB1 ^ num5);
																																																																																																														continue;
																																																																																																													}
																																																																																																													goto IL_70a5;
																																																																																																												}
																																																																																																											}
																																																																																																											else if (flag49)
																																																																																																											{
																																																																																																												num80 = (int)((num5 << 4) | (num5 >> 28)) ^ -105558247;
																																																																																																												continue;
																																																																																																											}
																																																																																																											num80 = 1435707570;
																																																																																																										}
																																																																																																										else
																																																																																																										{
																																																																																																											flag49 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																											int num170 = (int)(num5 ^ 0x327771BB);
																																																																																																											num80 = (num170 + -1927577905 - -1927577905) ^ 0x327771BB ^ 0x7332C705;
																																																																																																										}
																																																																																																									}
																																																																																																									else
																																																																																																									{
																																																																																																										if ((object)"" != "")
																																																																																																										{
																																																																																																										}
																																																																																																										num80 = (int)((num5 << 3) | (num5 >> 29)) ^ -956513554;
																																																																																																									}
																																																																																																									continue;
																																																																																																								}
																																																																																																								if (flag50)
																																																																																																								{
																																																																																																									num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -1263386769;
																																																																																																									continue;
																																																																																																								}
																																																																																																								goto IL_6e6a;
																																																																																																							}
																																																																																																							if ((object)"" == "")
																																																																																																							{
																																																																																																								goto IL_706d;
																																																																																																							}
																																																																																																							goto IL_7078;
																																																																																																						}
																																																																																																						if ((object)"" != "")
																																																																																																						{
																																																																																																							num80 = (int)(0x3BBD45F6 ^ num5);
																																																																																																							continue;
																																																																																																						}
																																																																																																						goto IL_70a5;
																																																																																																					}
																																																																																																					if ((object)"" != "")
																																																																																																					{
																																																																																																					}
																																																																																																				}
																																																																																																				else if ((object)"" != "" || "".Length != 0 || flag51)
																																																																																																				{
																																																																																																					num80 = (int)(0x7D9AB487 ^ num5);
																																																																																																					continue;
																																																																																																				}
																																																																																																				num80 = 2030446860;
																																																																																																			}
																																																																																																			else
																																																																																																			{
																																																																																																				if ("".Length == 0)
																																																																																																				{
																																																																																																					flag51 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																																				}
																																																																																																				int num171 = (int)num5 ^ -233787170;
																																																																																																				num80 = (num171 + 2120187546 - 2120187546) ^ -233787170 ^ 0x5D84B5F4;
																																																																																																			}
																																																																																																		}
																																																																																																		else
																																																																																																		{
																																																																																																			if ("".Length != 0)
																																																																																																			{
																																																																																																			}
																																																																																																			num80 = (int)(0x2C062A93 ^ num5);
																																																																																																		}
																																																																																																		continue;
																																																																																																	}
																																																																																																	if ((object)"" != "" || "".Length != 0 || flag52)
																																																																																																	{
																																																																																																		num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x3094371E);
																																																																																																		continue;
																																																																																																	}
																																																																																																	goto IL_706d;
																																																																																																}
																																																																																																goto IL_7271;
																																																																																															}
																																																																																															if ((object)"" != "")
																																																																																															{
																																																																																																num80 = (int)(0x37037B3E ^ num5);
																																																																																																continue;
																																																																																															}
																																																																																															goto IL_70a5;
																																																																																														}
																																																																																													}
																																																																																													else if ((object)"" != "" || flag53)
																																																																																													{
																																																																																														num80 = (int)(0x4BB86C7F ^ num5);
																																																																																														continue;
																																																																																													}
																																																																																													num80 = 803406618;
																																																																																												}
																																																																																												else
																																																																																												{
																																																																																													if ((object)"" == "" && "".Length == 0)
																																																																																													{
																																																																																														flag53 = (string.IsNullOrEmpty(key) ? 1 : 0) == num7;
																																																																																													}
																																																																																													num80 = (int)((num5 << 3) | (num5 >> 29)) ^ -553013929;
																																																																																												}
																																																																																											}
																																																																																											else
																																																																																											{
																																																																																												if ("".Length != 0 || "".Length != 0)
																																																																																												{
																																																																																												}
																																																																																												num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -238880136;
																																																																																											}
																																																																																											continue;
																																																																																										}
																																																																																										if ((object)"" != "" || (object)"" != "" || "".Length != 0 || (object)"" != "" || flag54)
																																																																																										{
																																																																																											num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -801874066;
																																																																																											continue;
																																																																																										}
																																																																																										goto IL_7271;
																																																																																									}
																																																																																									if ("".Length == 0)
																																																																																									{
																																																																																										flag54 = key.Contains(value2);
																																																																																									}
																																																																																									num80 = (int)(0x6A564E40 ^ num5);
																																																																																									continue;
																																																																																								}
																																																																																								if ((object)"" != "")
																																																																																								{
																																																																																								}
																																																																																								num80 = (int)((num5 << 2) | (num5 >> 30)) ^ -1822266729;
																																																																																								continue;
																																																																																							}
																																																																																							if ((object)"" != "" || flag55)
																																																																																							{
																																																																																								int num172 = (int)num5 ^ -1289896005;
																																																																																								num80 = (num172 + 1508270744 - 1508270744) ^ -1289896005 ^ 0x5932724E;
																																																																																								continue;
																																																																																							}
																																																																																							goto IL_72f0;
																																																																																						}
																																																																																						flag55 = (string.IsNullOrEmpty(text3) ? 1 : 0) == num7;
																																																																																						num80 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1E50FC7F);
																																																																																						continue;
																																																																																					}
																																																																																					text3 = infolist(list, current2);
																																																																																					num80 = (int)(0x5A230273 ^ num5);
																																																																																					continue;
																																																																																				}
																																																																																				if ("".Length != 0 || "".Length != 0 || "".Length != 0)
																																																																																				{
																																																																																				}
																																																																																				int num173 = (int)num5 ^ -1524793542;
																																																																																				num80 = (num173 + 356414912 - 356414912) ^ -1524793542 ^ 0x3E8DC106;
																																																																																				continue;
																																																																																				IL_55b9:
																																																																																				flag24 = key.Contains(value3);
																																																																																				goto IL_55c4;
																																																																																				IL_625a:
																																																																																				flag36 = key.Contains(value4);
																																																																																				goto IL_6265;
																																																																																				IL_6489:
																																																																																				flag38 = key.Contains(value5);
																																																																																				goto IL_6494;
																																																																																				IL_6494:
																																																																																				num80 = 609056223;
																																																																																				continue;
																																																																																				IL_600d:
																																																																																				flag34 = key.Contains(value6);
																																																																																				goto IL_6018;
																																																																																				IL_4d61:
																																																																																				num80 = 273167106;
																																																																																				continue;
																																																																																				IL_6c80:
																																																																																				num80 = 1037453727;
																																																																																				continue;
																																																																																				IL_7271:
																																																																																				flag52 = key.Contains(value7);
																																																																																				num80 = 1938086523;
																																																																																				continue;
																																																																																				IL_5e14:
																																																																																				flag32 = key.Contains(value8);
																																																																																				goto IL_5e1f;
																																																																																				IL_6018:
																																																																																				num80 = 1464760068;
																																																																																				continue;
																																																																																				IL_4768:
																																																																																				flag10 = key.Contains(value9);
																																																																																				num80 = 2066257679;
																																																																																				continue;
																																																																																				IL_6a24:
																																																																																				flag44 = key.Contains(value10);
																																																																																				num80 = 1524177751;
																																																																																				continue;
																																																																																				IL_6e75:
																																																																																				num80 = 1738409158;
																																																																																				continue;
																																																																																				IL_4970:
																																																																																				flag12 = key.Contains(value11);
																																																																																				num80 = 561024689;
																																																																																				continue;
																																																																																				IL_6856:
																																																																																				num80 = 112455456;
																																																																																				continue;
																																																																																				IL_70a5:
																																																																																				MTKConfig.Infodevice = (byte)num7 != 0;
																																																																																				goto IL_70ac;
																																																																																				IL_55c4:
																																																																																				num80 = 1500738479;
																																																																																				continue;
																																																																																				IL_59cd:
																																																																																				flag28 = key.Contains(value12);
																																																																																				goto IL_59d8;
																																																																																				IL_59d8:
																																																																																				num80 = 218442535;
																																																																																				continue;
																																																																																				IL_4f3d:
																																																																																				flag18 = key.Contains(value13);
																																																																																				num80 = 1563448869;
																																																																																				continue;
																																																																																				IL_53a4:
																																																																																				flag22 = key.Contains(value14);
																																																																																				goto IL_53af;
																																																																																				IL_57b8:
																																																																																				flag26 = key.Contains(value15);
																																																																																				num80 = 1169422080;
																																																																																				continue;
																																																																																				IL_5e1f:
																																																																																				num80 = 315332693;
																																																																																				continue;
																																																																																				IL_6e6a:
																																																																																				flag48 = key.Contains(value16);
																																																																																				goto IL_6e75;
																																																																																				IL_5175:
																																																																																				num80 = 38111103;
																																																																																				continue;
																																																																																				IL_6265:
																																																																																				num80 = 480036634;
																																																																																				continue;
																																																																																				IL_4b2f:
																																																																																				flag14 = key.Contains(value17);
																																																																																				num80 = 535869130;
																																																																																				continue;
																																																																																				IL_4d56:
																																																																																				flag16 = key.Contains(value18);
																																																																																				goto IL_4d61;
																																																																																				IL_53af:
																																																																																				num80 = 1013438812;
																																																																																				continue;
																																																																																				IL_7078:
																																																																																				num80 = 552413670;
																																																																																				continue;
																																																																																				IL_5c2b:
																																																																																				num80 = 2042877994;
																																																																																				continue;
																																																																																				IL_70ac:
																																																																																				num80 = 1790362930;
																																																																																				continue;
																																																																																				IL_706d:
																																																																																				flag50 = key.Contains(value19);
																																																																																				goto IL_7078;
																																																																																				IL_6c75:
																																																																																				flag46 = key.Contains(value20);
																																																																																				goto IL_6c80;
																																																																																				IL_6673:
																																																																																				flag40 = key.Contains(value21);
																																																																																				num80 = 77372173;
																																																																																				continue;
																																																																																				IL_5c20:
																																																																																				flag30 = key.Contains(value22);
																																																																																				goto IL_5c2b;
																																																																																				IL_72f0:
																																																																																				num80 = 831748040;
																																																																																				continue;
																																																																																				IL_516a:
																																																																																				flag20 = key.Contains(value23);
																																																																																				goto IL_5175;
																																																																																				IL_684b:
																																																																																				flag42 = key.Contains(value24);
																																																																																				goto IL_6856;
																																																																																			}
																																																																																			goto IL_73da;
																																																																																			IL_73cb:
																																																																																			if (num83 == 310)
																																																																																			{
																																																																																				continue;
																																																																																			}
																																																																																			if (num83 == 120)
																																																																																			{
																																																																																				break;
																																																																																			}
																																																																																			goto IL_73da;
																																																																																			continue;
																																																																																			end_IL_73fb:
																																																																																			break;
																																																																																		}
																																																																																	}
																																																																																	finally
																																																																																	{
																																																																																		((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
																																																																																		while (true)
																																																																																		{
																																																																																			int num174 = 78810002;
																																																																																			while (true)
																																																																																			{
																																																																																				nint num175 = num174 ^ 0x6827D370;
																																																																																				nuint num176 = (nuint)num175;
																																																																																				int num177 = (int)((num5 = (uint)((int)num176 ^ 0x6827D370 ^ 0x4AAFE7C5 ^ 0x4AAFE7C5)) % 3);
																																																																																				if (num177 == 0)
																																																																																				{
																																																																																					break;
																																																																																				}
																																																																																				if (num177 == 2 || num177 == 2 || num177 != 1)
																																																																																				{
																																																																																					if ("".Length != 0)
																																																																																					{
																																																																																					}
																																																																																					num174 = (int)(0x19CAC468 ^ num5);
																																																																																					continue;
																																																																																				}
																																																																																				goto end_IL_747c;
																																																																																			}
																																																																																			continue;
																																																																																			end_IL_747c:
																																																																																			break;
																																																																																		}
																																																																																	}
																																																																																	goto IL_74dc;
																																																																																}
																																																																																goto IL_749a;
																																																																																IL_7492:
																																																																																if ("".Length == 0)
																																																																																{
																																																																																	continue;
																																																																																}
																																																																																goto IL_749a;
																																																																																IL_749a:
																																																																																int num178 = 1100604139;
																																																																																goto IL_749f;
																																																																																IL_74dc:
																																																																																num178 = 1549030550;
																																																																																goto IL_749f;
																																																																																IL_749f:
																																																																																nint num179 = num178 ^ -17653322;
																																																																																nuint num180 = (nuint)num179;
																																																																																switch ((int)((num5 = (uint)((int)num180 ^ -17653322 ^ -1428065646 ^ -1428065646)) % 3))
																																																																																{
																																																																																default:
																																																																																	goto IL_7492;
																																																																																case 0:
																																																																																	break;
																																																																																case 1:
																																																																																	goto end_IL_74e3;
																																																																																}
																																																																																goto IL_74dc;
																																																																																continue;
																																																																																end_IL_74e3:
																																																																																break;
																																																																															}
																																																																														}
																																																																														finally
																																																																														{
																																																																															((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
																																																																															while (true)
																																																																															{
																																																																																int num181 = 917211296;
																																																																																while (true)
																																																																																{
																																																																																	nint num182 = num181 ^ 0x71F05A18;
																																																																																	nuint num183 = (nuint)num182;
																																																																																	int num184 = (int)((num5 = (uint)((int)num183 ^ 0x71F05A18 ^ 0x6CE0D33C ^ 0x6CE0D33C)) % 3);
																																																																																	if (num184 == 0)
																																																																																	{
																																																																																		break;
																																																																																	}
																																																																																	if (num184 == 2 || num184 == 2 || num184 != 1)
																																																																																	{
																																																																																		if ((object)"" != "")
																																																																																		{
																																																																																		}
																																																																																		num181 = (int)(0x6D0A9DEC ^ num5);
																																																																																		continue;
																																																																																	}
																																																																																	goto end_IL_756a;
																																																																																}
																																																																																continue;
																																																																																end_IL_756a:
																																																																																break;
																																																																															}
																																																																														}
																																																																														goto IL_7572;
																																																																													}
																																																																												}
																																																																												goto IL_2998;
																																																																												IL_7577:
																																																																												int num186;
																																																																												nint num185 = num186 ^ 0x4462BE4F;
																																																																												nuint num187 = (nuint)num185;
																																																																												int num188 = (int)((num5 = (uint)((int)num187 ^ 0x4462BE4F ^ 0x35C12C57 ^ 0x35C12C57)) % 3);
																																																																												if (num188 == 0)
																																																																												{
																																																																													goto IL_7572;
																																																																												}
																																																																												if (num188 != 2 && num188 == 1)
																																																																												{
																																																																													return;
																																																																												}
																																																																												goto IL_75c0;
																																																																												IL_75c0:
																																																																												num186 = 610055629;
																																																																												goto IL_7577;
																																																																												IL_7572:
																																																																												num186 = 569754326;
																																																																												goto IL_7577;
																																																																											}
																																																																											goto IL_28b2;
																																																																											IL_1cdd:
																																																																											num14 = 937384165;
																																																																											continue;
																																																																											IL_18ff:
																																																																											flag4 = (byte)num48 != 0;
																																																																											num14 = 2024748812;
																																																																											continue;
																																																																											IL_2998:
																																																																											if ((object)"" != "")
																																																																											{
																																																																												goto IL_29a6;
																																																																											}
																																																																											goto IL_29ad;
																																																																											IL_29a6:
																																																																											num70 = 1430253573;
																																																																											goto IL_295c;
																																																																											IL_28b2:
																																																																											num70 = 1984214213;
																																																																											goto IL_295c;
																																																																											IL_1bec:
																																																																											num14 = 881723522;
																																																																											continue;
																																																																											IL_1d05:
																																																																											num14 = 1398499700;
																																																																											continue;
																																																																											IL_29ad:
																																																																											flag8 = list.Count > num7;
																																																																											goto IL_29a6;
																																																																											IL_1f3a:
																																																																											num14 = 1998613903;
																																																																											continue;
																																																																											IL_18a0:
																																																																											num14 = 1386126002;
																																																																										}
																																																																									}
																																																																								}
																																																																								finally
																																																																								{
																																																																									if (memoryStream2 != null)
																																																																									{
																																																																										while (true)
																																																																										{
																																																																											int num189 = 1109879587;
																																																																											while (true)
																																																																											{
																																																																												nint num190 = num189 ^ -2061324271;
																																																																												nuint num191 = (nuint)num190;
																																																																												int num192 = (int)((num5 = (uint)((int)num191 ^ -2061324271 ^ 0xE2035B3 ^ 0xE2035B3)) % 4);
																																																																												if (num192 == 0)
																																																																												{
																																																																													break;
																																																																												}
																																																																												if (num192 != 3 && num192 != 3)
																																																																												{
																																																																													if (num192 == 1 || num192 == 1)
																																																																													{
																																																																														if ("".Length != 0)
																																																																														{
																																																																														}
																																																																														num189 = (int)(0x4E0BF303 ^ num5);
																																																																														continue;
																																																																													}
																																																																													if (num192 == 2)
																																																																													{
																																																																														goto end_IL_7681;
																																																																													}
																																																																												}
																																																																												((IDisposable)memoryStream2).Dispose();
																																																																												int num193 = (int)(num5 ^ 0x43E6D324);
																																																																												num189 = (num193 + 550245787 - 550245787) ^ 0x43E6D324 ^ 0x31AE32C6;
																																																																											}
																																																																											continue;
																																																																											end_IL_7681:
																																																																											break;
																																																																										}
																																																																									}
																																																																								}
																																																																							}
																																																																						}
																																																																					}
																																																																					catch (Exception)
																																																																					{
																																																																						while (true)
																																																																						{
																																																																							int num194 = 27804053;
																																																																							while (true)
																																																																							{
																																																																								nint num195 = num194 ^ -833111169;
																																																																								nuint num196 = (nuint)num195;
																																																																								int num197 = (int)((num5 = (uint)((int)num196 ^ -833111169 ^ -14081660 ^ -14081660)) % 22);
																																																																								if (num197 == 0)
																																																																								{
																																																																									break;
																																																																								}
																																																																								if (num197 != 13 && num197 != 13)
																																																																								{
																																																																									if (num197 != 1 && num197 != 1)
																																																																									{
																																																																										if (num197 != 17 && num197 != 17)
																																																																										{
																																																																											if (num197 == 2 || num197 == 2)
																																																																											{
																																																																												if ("".Length != 0)
																																																																												{
																																																																												}
																																																																												num194 = (int)(0x7A4D02CD ^ num5);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 3 || num197 == 3)
																																																																											{
																																																																												UI.RichLogs(msg6, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																												num194 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x644B9988);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 18 || num197 == 18)
																																																																											{
																																																																												if ((object)"" != "")
																																																																												{
																																																																												}
																																																																												num194 = (int)(0x54B7A68E ^ num5);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 4 || num197 == 4)
																																																																											{
																																																																												UI.RichLogs(msg5, UI.ok, (byte)num7 != 0, (byte)num7 != 0);
																																																																												int num198 = (int)num5 ^ -894391533;
																																																																												num194 = (num198 + -1243917364 - -1243917364) ^ -894391533 ^ 0x27045F77;
																																																																												continue;
																																																																											}
																																																																											if (num197 == 15 || num197 == 15)
																																																																											{
																																																																												if ("".Length != 0)
																																																																												{
																																																																												}
																																																																												num194 = (int)(0x56D8A064 ^ num5);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 5 || num197 == 5)
																																																																											{
																																																																												UI.RichLogs(msg4, UI.log, (byte)num7 != 0, (byte)num7 != 0);
																																																																												int num199 = (int)num5 ^ -1839682481;
																																																																												num194 = (num199 + -1940427204 - -1940427204) ^ -1839682481 ^ 0x74CAE2B;
																																																																												continue;
																																																																											}
																																																																											if (num197 == 6 || num197 == 6)
																																																																											{
																																																																												num194 = (int)((num5 << 3) | (num5 >> 29)) ^ -1616920200;
																																																																												continue;
																																																																											}
																																																																											if (num197 == 20 || num197 == 20)
																																																																											{
																																																																												if ((object)"" == "")
																																																																												{
																																																																													UI.RichLogs(msg3, UI.error, (byte)num7 != 0, (byte)num18 != 0);
																																																																												}
																																																																												num194 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x1B0E4CE);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 7 || num197 == 7)
																																																																											{
																																																																												int num200 = (int)num5 ^ -996436888;
																																																																												num194 = (num200 + -1764899668 - -1764899668) ^ -996436888 ^ 0x456F46AB;
																																																																												continue;
																																																																											}
																																																																											if (num197 == 14 || num197 == 14)
																																																																											{
																																																																												UI.RichLogs(msg2, UI.error, (byte)num7 != 0, (byte)num18 != 0);
																																																																												num194 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x5EF303A2);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 8 || num197 == 8)
																																																																											{
																																																																												if ((object)"" != "" || "".Length != 0)
																																																																												{
																																																																												}
																																																																												num194 = (int)(0x2CC3E638 ^ num5);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 16 || num197 == 16)
																																																																											{
																																																																												UI.RichLogs(msg, UI.log, (byte)num18 != 0, (byte)num7 != 0);
																																																																												num194 = (int)(0x28925497 ^ num5);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 9 || num197 == 9)
																																																																											{
																																																																												if ("".Length != 0)
																																																																												{
																																																																												}
																																																																												num194 = (int)(0x5FE83721 ^ num5);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 10 || num197 == 10)
																																																																											{
																																																																												if ((object)"" == "" && "".Length == 0)
																																																																												{
																																																																													MTKConfig.Infodevice = (byte)num18 != 0;
																																																																												}
																																																																												num194 = (int)(0x6FBC334B ^ num5);
																																																																												continue;
																																																																											}
																																																																											if (num197 == 19 || num197 == 19)
																																																																											{
																																																																												if ((object)"" != "")
																																																																												{
																																																																												}
																																																																												num194 = (int)((num5 << 4) | (num5 >> 28)) ^ -794368534;
																																																																												continue;
																																																																											}
																																																																											if (num197 == 11 || num197 == 11)
																																																																											{
																																																																												if ((object)"" != "")
																																																																												{
																																																																												}
																																																																												int num201 = (int)num5 ^ -856347626;
																																																																												num194 = (num201 + 156032887 - 156032887) ^ -856347626 ^ 0x3094D1CE;
																																																																												continue;
																																																																											}
																																																																											if (num197 != 21)
																																																																											{
																																																																												if (num197 != 12)
																																																																												{
																																																																													goto IL_7b0b;
																																																																												}
																																																																												return;
																																																																											}
																																																																											if ((object)"" != "")
																																																																											{
																																																																											}
																																																																										}
																																																																										else if ((object)"" != "" || "".Length != 0 || flag56)
																																																																										{
																																																																											int num202 = (int)(num5 ^ 0x5F72E885);
																																																																											num194 = (num202 + 1459068606 - 1459068606) ^ 0x5F72E885 ^ 0x6FA4D585;
																																																																											continue;
																																																																										}
																																																																										num194 = 691258228;
																																																																									}
																																																																									else
																																																																									{
																																																																										flag56 = Conversions.ToBoolean(value);
																																																																										num194 = (int)((num5 << 2) | (num5 >> 30)) ^ -1279420821;
																																																																									}
																																																																									continue;
																																																																								}
																																																																								goto IL_7b0b;
																																																																								IL_7b0b:
																																																																								if ("".Length != 0 || "".Length != 0 || "".Length != 0)
																																																																								{
																																																																								}
																																																																								num194 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x2E761085);
																																																																							}
																																																																						}
																																																																					}
																																																																				}
																																																																				if ("".Length == 0)
																																																																				{
																																																																					flag = text5.Contains(value25);
																																																																				}
																																																																				int num203 = (int)(num5 ^ 0x49E8F4DB);
																																																																				num = (num203 + 187517692 - 187517692) ^ 0x49E8F4DB ^ 0x500D9B1B;
																																																																			}
																																																																			else
																																																																			{
																																																																				if ((object)"" == "" && (object)"" == "")
																																																																				{
																																																																					text5 = Encoding.UTF8.GetString(bootBytes.Take(count2).ToArray());
																																																																				}
																																																																				num = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x4D05B0D0);
																																																																			}
																																																																		}
																																																																		else
																																																																		{
																																																																			if ((object)"" == "")
																																																																			{
																																																																				value = Conversions.ToString((byte)num7 != 0);
																																																																			}
																																																																			num = (int)(0x79240F79 ^ num5);
																																																																		}
																																																																	}
																																																																	else
																																																																	{
																																																																		num = (int)(0x7F22FB3F ^ num5);
																																																																	}
																																																																}
																																																																else
																																																																{
																																																																	value9 = Class38.field_10557;
																																																																	num = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x1040393E);
																																																																}
																																																															}
																																																															else
																																																															{
																																																																if ((object)"" == "" && "".Length == 0)
																																																																{
																																																																	value11 = Class38.field_10556;
																																																																}
																																																																int num204 = (int)num5 ^ -563439495;
																																																																num = (num204 + -136054797 - -136054797) ^ -563439495 ^ 0x2BEAC6C3;
																																																															}
																																																														}
																																																														else
																																																														{
																																																															if ((object)"" == "" && "".Length == 0)
																																																															{
																																																																value17 = Class38.field_10555;
																																																															}
																																																															num = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x41128B94);
																																																														}
																																																													}
																																																													else
																																																													{
																																																														value18 = Class38.field_10554;
																																																														num = (int)(0x2A75B73F ^ num5);
																																																													}
																																																												}
																																																												else
																																																												{
																																																													value13 = Class38.field_10553;
																																																													num = (int)(0x566A8D89 ^ num5);
																																																												}
																																																											}
																																																											else
																																																											{
																																																												if ((object)"" == "")
																																																												{
																																																													value23 = Class38.field_10552;
																																																												}
																																																												int num205 = (int)(num5 ^ 0x4962C9FC);
																																																												num = (num205 + 1671086315 - 1671086315) ^ 0x4962C9FC ^ 0x2458838B;
																																																											}
																																																										}
																																																										else
																																																										{
																																																											value14 = Class38.field_10551;
																																																											num = (int)(0x7901B109 ^ num5);
																																																										}
																																																									}
																																																									else
																																																									{
																																																										if ((object)"" == "" && (object)"" == "")
																																																										{
																																																											value3 = Class38.field_3291;
																																																										}
																																																										num = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x4564CBA);
																																																									}
																																																								}
																																																								else
																																																								{
																																																									if ("".Length == 0 && (object)"" == "")
																																																									{
																																																										value15 = Class38.field_2619;
																																																									}
																																																									num = (int)(0x6074C9D1 ^ num5);
																																																								}
																																																							}
																																																							else
																																																							{
																																																								if ("".Length == 0)
																																																								{
																																																									msg20 = Class38.field_10550;
																																																								}
																																																								int num206 = (int)(num5 ^ 0x4E429010);
																																																								num = (num206 + -95546070 - -95546070) ^ 0x4E429010 ^ 0x3CDA5B4B;
																																																							}
																																																						}
																																																						else
																																																						{
																																																							if ("".Length == 0 && (object)"" == "")
																																																							{
																																																								value12 = Class38.field_10549;
																																																							}
																																																							num = (int)((num5 << 4) | (num5 >> 28)) ^ -1043465585;
																																																						}
																																																					}
																																																					else
																																																					{
																																																						if ((object)"" == "" && (object)"" == "")
																																																						{
																																																							msg19 = Class38.field_10548;
																																																						}
																																																						int num207 = (int)(num5 ^ 0x1C24EC18);
																																																						num = (num207 + 851818769 - 851818769) ^ 0x1C24EC18 ^ 0x25802A3F;
																																																					}
																																																				}
																																																				else
																																																				{
																																																					value22 = Class38.field_2600;
																																																					num = (int)(0x73087AF1 ^ num5);
																																																				}
																																																			}
																																																			else
																																																			{
																																																				if ((object)"" == "")
																																																				{
																																																					newValue = Class38.field_292;
																																																				}
																																																				num = (int)(0x612965BB ^ num5);
																																																			}
																																																		}
																																																		else
																																																		{
																																																			if ((object)"" == "" && (object)"" == "")
																																																			{
																																																				oldValue = Class38.field_10547;
																																																			}
																																																			int num208 = (int)(num5 ^ 0x21539A4E);
																																																			num = (num208 + 1240758395 - 1240758395) ^ 0x21539A4E ^ 0x243CCF69;
																																																		}
																																																	}
																																																	else
																																																	{
																																																		if ("".Length == 0 && (object)"" == "" && "".Length == 0 && "".Length == 0)
																																																		{
																																																			text4 = Class38.field_10546;
																																																		}
																																																		num = (int)((num5 << 3) | (num5 >> 29)) ^ -91391767;
																																																	}
																																																}
																																																else
																																																{
																																																	if ((object)"" == "" && "".Length == 0)
																																																	{
																																																		msg18 = Class38.field_10545;
																																																	}
																																																	int num209 = (int)(num5 ^ 0x77E4848A);
																																																	num = (num209 + 1240303516 - 1240303516) ^ 0x77E4848A ^ 0x6A331CD9;
																																																}
																																															}
																																															else
																																															{
																																																value8 = Class38.field_2605;
																																																int num210 = (int)(num5 ^ 0x55248D8E);
																																																num = (num210 + 295502650 - 295502650) ^ 0x55248D8E ^ 0x16882303;
																																															}
																																														}
																																														else
																																														{
																																															if ("".Length == 0)
																																															{
																																																msg17 = Class38.field_10544;
																																															}
																																															num = (int)(0x29275081 ^ num5);
																																														}
																																													}
																																													else
																																													{
																																														value6 = Class38.field_2604;
																																														num = (int)(0x7082098F ^ num5);
																																													}
																																												}
																																												else
																																												{
																																													msg16 = Class38.field_10543;
																																													num = (int)((num5 << 3) | (num5 >> 29)) ^ -1407378111;
																																												}
																																											}
																																											else
																																											{
																																												value4 = Class38.field_2591;
																																												num = (int)(0x3C3A15FB ^ num5);
																																											}
																																										}
																																										else
																																										{
																																											msg15 = Class38.field_10542;
																																											num = (int)(0x66809941 ^ num5);
																																										}
																																									}
																																									else
																																									{
																																										if ((object)"" == "")
																																										{
																																											value5 = Class38.field_2586;
																																										}
																																										num = (int)(0x223C6EBF ^ num5);
																																									}
																																								}
																																								else
																																								{
																																									if ("".Length == 0 && "".Length == 0)
																																									{
																																										msg14 = Class38.field_10541;
																																									}
																																									num = (int)((num5 << 4) | (num5 >> 28)) ^ -322803761;
																																								}
																																							}
																																							else
																																							{
																																								value21 = Class38.field_2580;
																																								num = (int)(0xEF22CA3 ^ num5);
																																							}
																																						}
																																						else
																																						{
																																							if ("".Length == 0)
																																							{
																																								msg13 = Class38.field_10540;
																																							}
																																							num = (int)(0x17FEC5F1 ^ num5);
																																						}
																																					}
																																					else
																																					{
																																						value24 = Class38.field_2572;
																																						num = (int)((num5 << 3) | (num5 >> 29)) ^ -1059085298;
																																					}
																																				}
																																				else
																																				{
																																					msg12 = Class38.field_8301;
																																					num = (int)(0x1FD680F9 ^ num5);
																																				}
																																			}
																																			else
																																			{
																																				if ("".Length == 0 && "".Length == 0)
																																				{
																																					value10 = Class38.field_1461;
																																				}
																																				num = (int)(0x158398F2 ^ num5);
																																			}
																																		}
																																		else
																																		{
																																			if ("".Length == 0)
																																			{
																																				msg11 = Class38.field_10539;
																																			}
																																			num = (int)((num5 << 2) | (num5 >> 30)) ^ -712875447;
																																		}
																																	}
																																	else
																																	{
																																		value20 = Class38.field_2597;
																																		num = (int)(0xE8E018F ^ num5);
																																	}
																																}
																																else
																																{
																																	msg10 = Class38.field_10538;
																																	int num211 = (int)(num5 ^ 0x67A205B4);
																																	num = (num211 + -1512453885 - -1512453885) ^ 0x67A205B4 ^ 0x320F2A1;
																																}
																															}
																															else
																															{
																																if ((object)"" == "")
																																{
																																	value16 = Class38.field_2561;
																																}
																																num = (int)(0x7A14C5E3 ^ num5);
																															}
																														}
																														else
																														{
																															msg9 = Class38.field_10537;
																															num = (int)(0x5B54CF49 ^ num5);
																														}
																													}
																													else
																													{
																														if ("".Length == 0)
																														{
																															value19 = Class38.field_2552;
																														}
																														num = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x6F42DAD8);
																													}
																												}
																												else
																												{
																													if ("".Length == 0)
																													{
																														msg8 = Class38.field_10536;
																													}
																													int num212 = (int)(num5 ^ 0x26E51887);
																													num = (num212 + -1651207987 - -1651207987) ^ 0x26E51887 ^ 0x5CCECE61;
																												}
																											}
																											else
																											{
																												if ("".Length == 0 && "".Length == 0 && "".Length == 0)
																												{
																													value7 = Class38.field_2542;
																												}
																												int num213 = (int)(num5 ^ 0x214762E0);
																												num = (num213 + -790243174 - -790243174) ^ 0x214762E0 ^ 0x2A0343EB;
																											}
																										}
																										else
																										{
																											if ("".Length == 0)
																											{
																												msg7 = Class38.field_10535;
																											}
																											int num214 = (int)num5 ^ -1877006225;
																											num = (num214 + 830070304 - 830070304) ^ -1877006225 ^ 0x37008209;
																										}
																									}
																									else
																									{
																										value2 = Class38.field_2534;
																										int num215 = (int)num5 ^ -714688551;
																										num = (num215 + 638504276 - 638504276) ^ -714688551 ^ 0x778E4687;
																									}
																								}
																								else
																								{
																									if ("".Length == 0)
																									{
																										num47 = 50000000;
																									}
																									num = (int)(0x7E924261 ^ num5);
																								}
																							}
																							else
																							{
																								if ("".Length == 0 && (object)"" == "")
																								{
																									num49 = 2;
																								}
																								int num216 = (int)num5 ^ -58868668;
																								num = (num216 + -1969136423 - -1969136423) ^ -58868668 ^ 0x2C93ABEB;
																							}
																						}
																						else
																						{
																							if ("".Length == 0)
																							{
																								num51 = 139;
																							}
																							num = (int)((num5 << 2) | (num5 >> 30)) ^ -1179285756;
																						}
																					}
																					else
																					{
																						num52 = 31;
																						num = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x40AE4ABE);
																					}
																				}
																				else
																				{
																					count = 3;
																					num = (int)((num5 << 3) | (num5 >> 29)) ^ -340953214;
																				}
																			}
																			else
																			{
																				if ((object)"" != "" || "".Length != 0 || "".Length != 0)
																				{
																				}
																				int num217 = (int)(num5 ^ 0x29093100);
																				num = (num217 + -40301179 - -40301179) ^ 0x29093100 ^ 0x5ED4FAF3;
																			}
																		}
																		else
																		{
																			if ("".Length == 0)
																			{
																				page_size = 4096;
																			}
																			num = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x8ADE9F8);
																		}
																	}
																	else
																	{
																		if ((object)"" == "")
																		{
																			msg = Class38.field_1219;
																		}
																		num = (int)((num5 << 3) | (num5 >> 29)) ^ -282738656;
																	}
																}
																else
																{
																	msg2 = Class38.field_10533;
																	num = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x3468746E);
																}
															}
															else
															{
																if ((object)"" == "" && (object)"" == "")
																{
																	num18 = 1;
																}
																num = (int)((num5 << 2) | (num5 >> 30)) ^ -2030860910;
															}
														}
														else
														{
															if ((object)"" == "")
															{
																msg3 = Class38.field_3517;
															}
															int num218 = (int)(num5 ^ 0x6A79BD12);
															num = (num218 + 345259145 - 345259145) ^ 0x6A79BD12 ^ 0x53EA13A9;
														}
													}
													else
													{
														if ("".Length == 0)
														{
															msg4 = Class38.field_3003;
														}
														num = (int)(0x2D9CD26F ^ num5);
													}
												}
												else
												{
													if ((object)"" == "")
													{
														msg5 = Class38.field_2080;
													}
													int num219 = (int)num5 ^ -147292373;
													num = (num219 + 1659224813 - 1659224813) ^ -147292373 ^ 0x5068F8C9;
												}
											}
											else
											{
												msg6 = Class38.field_10532;
												num = (int)((num5 << 3) | (num5 >> 29)) ^ -884381632;
											}
										}
										else
										{
											if ("".Length == 0)
											{
												text2 = Class38.field_10531;
											}
											int num220 = (int)num5 ^ -213819138;
											num = (num220 + 2084939917 - 2084939917) ^ -213819138 ^ 0x5B8917A1;
										}
									}
									else
									{
										if ("".Length == 0)
										{
											num50 = 8;
										}
										num = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x2CBCCD78);
									}
								}
								else
								{
									if ((object)"" == "" && "".Length == 0 && (object)"" == "")
									{
										hex = Class38.field_10530;
									}
									int num221 = (int)num5 ^ -909283531;
									num = (num221 + -1876229985 - -1876229985) ^ -909283531 ^ 0x421D7669;
								}
							}
							else
							{
								if ((object)"" == "" && "".Length == 0)
								{
									value25 = Class38.field_10529;
								}
								int num222 = (int)num5 ^ -301595142;
								num = (num222 + -2024422916 - -2024422916) ^ -301595142 ^ 0x4E0D645B;
							}
						}
						else
						{
							if ((object)"" == "" && "".Length == 0 && "".Length == 0)
							{
								count2 = 15;
							}
							int num223 = (int)num5 ^ -703209062;
							num = (num223 + 289349329 - 289349329) ^ -703209062 ^ 0x425CA741;
						}
					}
					else
					{
						num7 = 0;
						num = (int)((num5 << 2) | (num5 >> 30)) ^ -630457391;
					}
					continue;
				}
				goto IL_1175;
				IL_1175:
				int num224 = (int)(num5 ^ 0x7B7C8C9E);
				num = (num224 + 1313399213 - 1313399213) ^ 0x7B7C8C9E ^ 0x30118192;
			}
		}
	}

	public static string androidver(string os)
	{
		string value = "1.5";
		string text = "(";
		string text2 = ") Cupcake";
		string value2 = "1.6";
		string text3 = ") Donut";
		string value3 = "2";
		string text4 = ") Eclair";
		string value4 = "2.2";
		string value5 = "2.2.3";
		string text5 = ") Froyo";
		string value6 = "2.3";
		string text6 = ") Gingerbread";
		string value7 = "3.0";
		string value8 = "3.1";
		string value9 = "3.2";
		string text7 = " (";
		string text8 = ") Honeycomb";
		string value10 = "4.0";
		string text9 = ") ICE Cream Sandwich";
		string value11 = "4.1";
		string value12 = "4.2";
		string value13 = "4.3";
		string text10 = ") Jelly Bean";
		string value14 = "4.4";
		string text11 = ") KitKat";
		string value15 = "5.0";
		string value16 = "5.1";
		string text12 = ") Lollipop";
		string value17 = "6.0";
		string text13 = ") Marshmallow";
		string value18 = "7.0";
		string value19 = "7.1";
		string text14 = ") Nougat";
		string value20 = "8.0";
		string value21 = "8.1";
		string text15 = ") Oreo";
		string value22 = "9";
		string text16 = ") Pie";
		string value23 = "10";
		string text17 = ")";
		string value24 = "11";
		string value25 = "12";
		string value26 = "13";
		string value27 = "14";
		string value28 = "15";
		string value29 = "16";
		string value30 = "17";
		string value31 = "18";
		string value32 = "19";
		string value33 = "20";
		os = os.Trim();
		string result = string.Empty;
		if (os.Contains(value))
		{
			result = text + os + text2;
		}
		if (os.Contains(value2))
		{
			result = text + os + text3;
		}
		if (os.Contains(value3))
		{
			result = text + os + text4;
		}
		if (os.Contains(value4) || os.Contains(value5))
		{
			result = text + os + text5;
		}
		if (os.Contains(value6))
		{
			result = text + os + text6;
		}
		if (os.Contains(value7) || os.Contains(value8) || os.Contains(value9))
		{
			result = text7 + os + text8;
		}
		if (os.Contains(value10))
		{
			result = text + os + text9;
		}
		if (os.Contains(value11) || os.Contains(value12) || os.Contains(value13))
		{
			result = text + os + text10;
		}
		if (os.Contains(value14))
		{
			result = text + os + text11;
		}
		if (os.Contains(value15) || os.Contains(value16))
		{
			result = text + os + text12;
		}
		if (os.Contains(value17))
		{
			result = text + os + text13;
		}
		if (os.Contains(value18) || os.Contains(value19))
		{
			result = text + os + text14;
		}
		if (os.Contains(value20) || os.Contains(value21))
		{
			result = text + os + text15;
		}
		if (os.Contains(value22))
		{
			result = text + os + text16;
		}
		if (os.Contains(value23))
		{
			result = text + os + text17;
		}
		if (os.Contains(value24))
		{
			result = text + os + text17;
		}
		if (os.Contains(value25))
		{
			result = text + os + text17;
		}
		if (os.Contains(value26))
		{
			result = text + os + text17;
		}
		if (os.Contains(value27))
		{
			result = text + os + text17;
		}
		if (os.Contains(value28))
		{
			result = text + os + text17;
		}
		if (os.Contains(value29))
		{
			result = text + os + text17;
		}
		if (os.Contains(value30))
		{
			result = text + os + text17;
		}
		if (os.Contains(value31))
		{
			result = text + os + text17;
		}
		if (os.Contains(value32))
		{
			result = text + os + text17;
		}
		if (os.Contains(value33))
		{
			result = text + os + text17;
		}
		return result;
	}

	public static long get_number_of_pages(long imageSize, long pageSize)
	{
		return (imageSize + pageSize - 1L) / pageSize;
	}

	public static T unpack_reader<T>(BinaryReader reader)
	{
		byte[] value = reader.ReadBytes(Marshal.SizeOf(typeof(T)));
		GCHandle gCHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
		T result = Conversions.ToGenericParameter<T>(Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(T)));
		gCHandle.Free();
		return result;
	}

	public static string infolist(List<string> allinfolist, string search)
	{
		string value = "=";
		object result = null;
		foreach (string item in allinfolist)
		{
			if (!item.Contains(value) || !item.StartsWith(search))
			{
				continue;
			}
			return item.Split('=')[1];
		}
		return (string)result;
	}

	public static int find_binary(byte[] array, object sequence, int start = 0)
	{
		object obj = null;
		int num = 0;
		int result = -1;
		int num2 = 1;
		int num3 = 5;
		string text = "\r\nfindSequence report : ";
		int num4 = 2;
		string text2 = "\r\nfindSequence data : ";
		int num5 = 3;
		string oldValue = "-";
		string newValue = " ";
		int num6 = 4;
		string text3 = "\r\n";
		byte[] array2 = Array.Empty<byte>();
		try
		{
			if (sequence as string !=null)
			{
				array2 = StringToByteArray(Conversions.ToString(sequence));
			}
			else if (sequence as byte[] !=null)
			{
				array2 = (byte[])sequence;
			}
			if (array2.Length != 0)
			{
				int num7 = array.Length - array2.Length;
				byte b = array2[0];
				while (start <= num7)
				{
					if (array[start] == b)
					{
						int num8 = 1;
						while (true)
						{
							if (num8 == array2.Length)
							{
								return start;
							}
							if (array[start + num8] != array2[num8])
							{
								break;
							}
							num8++;
						}
					}
					start++;
				}
				return -1;
			}
			return -1;
		}
		catch (Exception ex)
		{
			string[] array3 = new string[num3];
			array3[num] = text;
			array3[num2] = ex.Message;
			array3[num4] = text2;
			array3[num5] = BitConverter.ToString(array2).Replace(oldValue, newValue);
			array3[num6] = text3;
			Console.WriteLine(string.Concat(array3));
			return result;
		}
	}

	public static byte[] StringToByteArray(string hex)
	{
		string oldValue = " ";
		string oldValue2 = "-";
		string oldValue3 = "0x";
		string text = "0";
		string val = hex.Replace(oldValue, string.Empty).Replace(oldValue2, string.Empty).ToLower()
			.Replace(oldValue3, string.Empty);
		if (val.Length % 2 > 0)
		{
			string text2 = text + val.Substring(val.Length - 1);
			val = val.Substring(0, val.Length - 1) + text2;
			return (from x in Enumerable.Range(0, val.Length)
				where x % 2 == 0
				select Convert.ToByte(val.Substring(x, 2), 16)).ToArray();
		}
		return (from x in Enumerable.Range(0, val.Length)
			where x % 2 == 0
			select Convert.ToByte(val.Substring(x, 2), 16)).ToArray();
	}

	static Bootinfo()
	{
		string key = "Model Name";
		string item = "ro.product.brand";
		string item2 = "ro.product.bootimage.model";
		string item3 = "ro.product.product.brand";
		string item4 = "ro.product.odm.brand";
		string item5 = "ro.product.vendor.brand";
		string item6 = "ro.product.system_ext.brand";
		string item7 = "ro.dolby.brand";
		string item8 = "ro.product.bootimage.brand";
		string key2 = "Product Name";
		string item9 = "ro.product.device";
		string item10 = "ro.product.product.device";
		string item11 = "ro.product.system_ext.device";
		string item12 = "ro.product.system.device";
		string item13 = "ro.product.mod_device";
		string item14 = "ro.miui.cust_device";
		string item15 = "ro.product.vendor.device";
		string item16 = "ro.vivo.product.version";
		string item17 = "ro.product.board";
		string item18 = "ro.product.bootimage.device";
		string key3 = "Manufacturer";
		string item19 = "ro.product.manufacturer";
		string item20 = "ro.product.vendor.manufacturer";
		string item21 = "ro.product.odm.manufacturer";
		string item22 = "ro.product.product.manufacturer";
		string item23 = "ro.product.system_ext.manufacturer";
		string item24 = "ro.product.system.manufacturer";
		string item25 = "ro.dolby.manufacturer";
		string item26 = "ro.product.bootimage.manufacturer";
		string key4 = "Platform";
		string item27 = "ro.board.platform";
		string item28 = "ro.vivo.product.platform";
		string item29 = "ro.mediatek.platform";
		string key5 = "Model";
		string item30 = "ro.product.model";
		string item31 = "ro.product.product.model";
		string item32 = "ro.product.system_ext.model";
		string item33 = "ro.product.system.model";
		string item34 = "ro.product.vendor.model";
		string item35 = "ro.product.odm.model";
		string item36 = "ro.vivo.product.release.model";
		string item37 = "ro.vivo.product.model";
		string key6 = "Name";
		string item38 = "ro.product.product.name";
		string item39 = "ro.product.system_ext.name";
		string item40 = "ro.product.system.name";
		string item41 = "ro.product.name";
		string item42 = "ro.vivo.product.release.name";
		string item43 = "ro.product.bootimage.tran.device.name.default";
		string item44 = "ro.product.bootimage.name";
		string key7 = "Market name";
		string item45 = "ro.oppo.market.name";
		string item46 = "ro.vendor.oplus.market.name";
		string item47 = "ro.product.product.marketname";
		string item48 = "ro.product.system_ext.marketname";
		string item49 = "ro.product.system.marketname";
		string key8 = "SDK ver";
		string item50 = "ro.build.version.sdk";
		string item51 = "ro.vendor.build.version.sdk";
		string item52 = "ro.product.build.version.sdk";
		string item53 = "ro.bootimage.build.version.sdk";
		string key9 = "Android Version";
		string item54 = "ro.odm.build.version.release";
		string item55 = "ro.product.build.version.release";
		string item56 = "ro.build.version.release";
		string item57 = "ro.vivo.os.version";
		string item58 = "ro.bootimage.build.version.release";
		string item59 = "ro.bootimage.build.version.release_or_codename";
		string key10 = "Security Patch";
		string item60 = "ro.build.version.security_patch";
		string item61 = "ro.vendor.build.security_patch";
		string item62 = "ro.bootimage.build.security_patch";
		string key11 = "Build ID";
		string item63 = "ro.system.build.version.incremental";
		string item64 = "ro.build.version.incremental";
		string item65 = "ro.vendor.build.version.incremental";
		string key12 = "MIUI Region";
		string key13 = "MIUI Version";
		string item66 = "ro.miui.ui.version.name";
		string key14 = "vivo_ver";
		string item67 = "ro.vivo.system.product.version";
		string key15 = "6754";
		string item68 = "ro.vivo.default.version";
		string key16 = "5235";
		string item69 = "ro.build.expect.hardware";
		string key17 = "854";
		string item70 = "ro.build.display.ota";
		string item71 = "ro.build.ota.versionnam";
		string key18 = "my_manifest";
		string item72 = "ro.oplus.version.my_manifest";
		string item73 = "ro.oplus.image.my_manifest.version";
		string key19 = "oppover";
		string item74 = "ro.build.display.id_ims";
		string item75 = "ro.build.display.id";
		string item76 = "ro.build.display.full_id";
		string key20 = "23246";
		string item77 = "ro.vendor.vivo.product.version";
		string item78 = "ro.vro.vendor.vivo.product.versionendor.build.id";
		string key21 = "8445373";
		string item79 = "ro.system.build.version.release_or_codename";
		string item80 = "ro.hardware.bbk";
		string item81 = "ro.build.PDA";
		string item82 = "ro.vivo.hardware.version";
		string item83 = "ro.product.model.bbk";
		string key22 = "962R";
		string item84 = "ro.build.version.ota";
		string key23 = "Build Date";
		string item85 = "ro.build.date";
		string item86 = "ro.product.build.date";
		string item87 = "ro.system.build.date";
		string item88 = "ro.odm.build.date";
		string item89 = "ro.bootimage.build.date";
		string key24 = "5953463";
		string item90 = "ro.build.fingerprint";
		string item91 = "ro.build.version.base_os";
		string item92 = "ro.build.description";
		string item93 = "ro.build.representative.fingerprint";
		build = new Dictionary<string, List<string>>
		{
			{
				key,
				new List<string> { item, item2, item3, item4, item5, item6, item7, item8 }
			},
			{
				key2,
				new List<string>
				{
					item9, item10, item11, item12, item13, item14, item15, item16, item17, item18,
					item16
				}
			},
			{
				key3,
				new List<string> { item19, item20, item21, item22, item23, item24, item25, item26 }
			},
			{
				key4,
				new List<string> { item27, item28, item29 }
			},
			{
				key5,
				new List<string> { item30, item31, item32, item33, item34, item35, item36, item2, item37 }
			},
			{
				key6,
				new List<string> { item38, item39, item40, item41, item42, item43, item44 }
			},
			{
				key7,
				new List<string> { item45, item46, item47, item48, item49 }
			},
			{
				key8,
				new List<string> { item50, item51, item52, item53 }
			},
			{
				key9,
				new List<string> { item54, item55, item56, item57, item58, item59 }
			},
			{
				key10,
				new List<string> { item60, item61, item62 }
			},
			{
				key11,
				new List<string> { item63, item64, item65 }
			},
			{
				key12,
				new List<string> { item13 }
			},
			{
				key13,
				new List<string> { item66 }
			},
			{
				key14,
				new List<string> { item67 }
			},
			{
				key15,
				new List<string> { item68 }
			},
			{
				key16,
				new List<string> { item69 }
			},
			{
				key17,
				new List<string> { item70, item71 }
			},
			{
				key18,
				new List<string> { item72, item73 }
			},
			{
				key19,
				new List<string> { item74, item75, item76 }
			},
			{
				key20,
				new List<string> { item77, item78 }
			},
			{
				key21,
				new List<string> { item79, item44, item80, item81, item82, item83 }
			},
			{
				key22,
				new List<string> { item84, item45, item83 }
			},
			{
				key23,
				new List<string> { item85, item86, item87, item88, item89 }
			},
			{
				key24,
				new List<string> { item90, item91, item92, item93 }
			}
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
