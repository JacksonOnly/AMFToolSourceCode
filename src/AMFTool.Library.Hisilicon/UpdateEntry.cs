using System;
using System.Collections.Generic;
using System.IO;
using Restored.Namespace11;
using Restored.Namespace12;

namespace AMFTool.Library.Hisilicon;

public class UpdateEntry
{
	public const ushort DefaultBlockSize = 4096;

	private Struct3 _fileHeader;

	internal long DataOffset;

	internal ushort[] CheckSumTable;

	public string updateapp;

	public EntryType Type { get; internal set; }

	public uint HeaderId
	{
		get
		{
			return _fileHeader.field_01;
		}
		private set
		{
			_fileHeader.field_01 = value;
		}
	}

	public uint HeaderSize
	{
		get
		{
			return _fileHeader.field_02;
		}
		internal set
		{
			_fileHeader.field_02 = value;
		}
	}

	public string HardwareId
	{
		get
		{
			return Class35.method_04(_fileHeader.field_04);
		}
		set
		{
			Class35.method_03(value, _fileHeader.field_04);
		}
	}

	public uint FileSequence
	{
		get
		{
			return _fileHeader.field_05;
		}
		set
		{
			_fileHeader.field_05 = value;
		}
	}

	public uint FileSize
	{
		get
		{
			return _fileHeader.field_06;
		}
		internal set
		{
			_fileHeader.field_06 = value;
		}
	}

	public string FileDate
	{
		get
		{
			return Class35.method_04(_fileHeader.field_07);
		}
		set
		{
			Class35.method_03(value, _fileHeader.field_07);
		}
	}

	public string FileTime
	{
		get
		{
			return Class35.method_04(_fileHeader.field_08);
		}
		set
		{
			Class35.method_03(value, _fileHeader.field_08);
		}
	}

	public string FileType
	{
		get
		{
			return Class35.method_04(_fileHeader.field_09);
		}
		set
		{
			Class35.method_03(value, _fileHeader.field_09);
		}
	}

	public ushort HeaderChecksum
	{
		get
		{
			return _fileHeader.field_11;
		}
		private set
		{
			_fileHeader.field_11 = value;
		}
	}

	public ushort BlockSize
	{
		get
		{
			return _fileHeader.field_12;
		}
		private set
		{
			_fileHeader.field_12 = value;
		}
	}

	internal byte[] GetHeader()
	{
		string message = "TypeToByte() failed.";
		if (!Class35.method_02(_fileHeader, out var A178A9DF))
		{
			throw new Exception(message);
		}
		return A178A9DF;
	}

	internal void ComputeHeaderChecksum()
	{
		HeaderChecksum = 0;
		HeaderChecksum = Class35.field_02.ComputeSum(GetHeader());
	}

	private void ReadEntry(Stream stream, bool checksum)
	{
		string text = "ByteToType() failed @";
		string message = "Invalid file.";
		string format = "Checksum error @{0:X08}: {1:X04}<>{2:X04}";
		BinaryReader binaryReader = new BinaryReader(stream);
		if (!Class35.method_01<Struct3>(binaryReader, out _fileHeader))
		{
			throw new Exception(text + binaryReader.BaseStream.Position);
		}
		if (HeaderId != 2774182485u)
		{
			throw new Exception(message);
		}
		if (checksum)
		{
			ushort headerChecksum = HeaderChecksum;
			ComputeHeaderChecksum();
			if (HeaderChecksum != headerChecksum)
			{
				throw new Exception(string.Format(format, stream.Position, _fileHeader.field_11, headerChecksum));
			}
		}
		long num = HeaderSize - Struct3.field_14;
		CheckSumTable = new ushort[num / 2L];
		for (int i = 0; i < CheckSumTable.Length; i++)
		{
			CheckSumTable[i] = binaryReader.ReadUInt16();
		}
		DataOffset = stream.Position;
	}

	private UpdateEntry(Stream stream, bool checksum)
	{
		ReadEntry(stream, checksum);
	}

	private void CreateEntry(ushort blockSize)
	{
		string text = "yyyy.MM.dd";
		string text2 = "HH.mm.ss";
		_fileHeader = Struct3.method_01();
		DateTime now = DateTime.Now;
		FileDate = now.ToString(text);
		FileTime = now.ToString(text2);
		HeaderId = 2774182485u;
		BlockSize = blockSize;
	}

	private UpdateEntry(ushort blockSize)
	{
		CreateEntry(blockSize);
	}

	public static UpdateEntry Read(Stream stream, bool checksum = true)
	{
		return new UpdateEntry(stream, checksum);
	}

	public static UpdateEntry Create(ushort blockSize = 4096)
	{
		return new UpdateEntry(blockSize);
	}

	public Stream GetDataStream(Stream stream)
	{
		stream.Seek(DataOffset, SeekOrigin.Begin);
		return new PartialStream(stream, FileSize);
	}

	public Stream GetDataStream(string fileName)
	{
		FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
		fileStream.Seek(DataOffset, SeekOrigin.Begin);
		return new PartialStream(fileStream, FileSize);
	}

	public void Extract(Stream input, Stream output, bool checksum = true)
	{
		string format = "Checksum error in block {0}@{1:X08}: {2:X04}<>{3:X04}";
		Stream dataStream = GetDataStream(input);
		byte[] buffer = new byte[BlockSize];
		int num = 0;
		int num2;
		while ((num2 = dataStream.Read(buffer, 0, BlockSize)) > 0)
		{
			if (checksum)
			{
				ushort num3 = Class35.field_02.ComputeSum(buffer, 0, num2);
				if (num3 != CheckSumTable[num])
				{
					throw new Exception(string.Format(format, num, dataStream.Position - num2, CheckSumTable[num], num3));
				}
			}
			output.Write(buffer, 0, num2);
			UI.Progresbar1(Convert.ToInt64(dataStream.Length), Convert.ToInt64(output.Length));
			num++;
		}
	}

	public void Extract(Stream input, string output, bool checksum = true)
	{
		using FileStream output2 = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.None);
		Extract(input, output2, checksum);
	}

	public void Extract(string input, Stream output, bool checksum = true)
	{
		using FileStream input2 = new FileStream(input, FileMode.Open, FileAccess.Read, FileShare.Read);
		Extract(input2, output, checksum);
	}

	public void ExtractToOneFile(List<UpdateEntry> input, string output, bool checksum = true)
	{
		int num6 = default(int);
		int num8 = default(int);
		int num9 = default(int);
		int num11 = default(int);
		string text = default(string);
		int num12 = default(int);
		UpdateEntry current = default(UpdateEntry);
		uint num23 = default(uint);
		int num44 = default(int);
		Stream dataStream = default(Stream);
		int num45 = default(int);
		ushort num46 = default(ushort);
		bool flag = default(bool);
		bool flag2 = default(bool);
		byte[] buffer = default(byte[]);
		bool flag3 = default(bool);
		while (true)
		{
			int num = 717735611;
			while (true)
			{
				nint num2 = num ^ 0x522DF6F1;
				nuint num3 = (nuint)num2;
				uint num5;
				int num4 = (int)((num5 = (uint)((int)num3 ^ 0x522DF6F1 ^ -1319681952 ^ -1319681952)) % 10);
				if (num4 == 0)
				{
					break;
				}
				if (num4 != 1 && num4 != 1)
				{
					if (num4 == 2 || num4 == 2)
					{
						if ("".Length == 0)
						{
							num6 = 2;
						}
						int num7 = (int)num5 ^ -1350716076;
						num = (num7 + 1394675255 - 1394675255) ^ -1350716076 ^ 0x184A5E41;
						continue;
					}
					if (num4 == 3 || num4 == 3)
					{
						if ((object)"" == "")
						{
							num8 = 0;
						}
						num = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x7B5F74D);
						continue;
					}
					if (num4 == 4 || num4 == 4)
					{
						num9 = 3;
						int num10 = (int)(num5 ^ 0x63DDBA03);
						num = (num10 + 487835181 - 487835181) ^ 0x63DDBA03 ^ 0x18AB05FF;
						continue;
					}
					if (num4 == 5 || num4 == 5)
					{
						if ("".Length == 0 && (object)"" == "")
						{
							num11 = 1;
						}
						num = (int)(0x35448BB ^ num5);
						continue;
					}
					if (num4 == 6 || num4 == 6)
					{
						if ("".Length == 0)
						{
							text = Class38.field_10321;
						}
						num = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x2B3F4076);
						continue;
					}
					if (num4 == 7 || num4 == 7)
					{
						if ((object)"" == "" && (object)"" == "")
						{
							num12 = 4;
						}
						int num13 = (int)num5 ^ -628422607;
						num = (num13 + -1977115375 - -1977115375) ^ -628422607 ^ 0x29181BA3;
						continue;
					}
					if (num4 == 8 || num4 == 8)
					{
						if ("".Length != 0 || "".Length != 0)
						{
						}
						num = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x294E6832);
						continue;
					}
					if (num4 == 9)
					{
						FileStream fileStream = new FileStream(output, (FileMode)num6, (FileAccess)num6, (FileShare)num8);
						try
						{
							while (true)
							{
								int num14 = 101617409;
								while (true)
								{
									nint num15 = num14 ^ -822601730;
									nuint num16 = (nuint)num15;
									int num17 = (int)((num5 = (uint)((int)num16 ^ -822601730 ^ -1879821809 ^ -1879821809)) % 4);
									if (num17 == 0)
									{
										break;
									}
									if (num17 != 1 && num17 != 1)
									{
										if (num17 == 2 || num17 == 2)
										{
											if ("".Length != 0)
											{
											}
											num14 = (int)(0x5357B2D ^ num5);
											continue;
										}
										if (num17 == 3)
										{
											List<UpdateEntry>.Enumerator enumerator = input.GetEnumerator();
											try
											{
												while (true)
												{
													int num18;
													if (!enumerator.MoveNext())
													{
														num18 = 251495291;
														goto IL_0354;
													}
													goto IL_0395;
													IL_038b:
													int num19;
													if (num19 == 4)
													{
														continue;
													}
													if (num19 == 5)
													{
														break;
													}
													goto IL_0395;
													IL_0395:
													current = enumerator.Current;
													num18 = 1881848635;
													goto IL_0354;
													IL_0354:
													while (true)
													{
														nint num20 = num18 ^ 0x3FACFE36;
														nuint num21 = (nuint)num20;
														num19 = (int)((num5 = (uint)((int)num21 ^ 0x3FACFE36 ^ -1338982435 ^ -1338982435)) % 7);
														if (num19 == 0)
														{
															num18 = 1482213146;
															continue;
														}
														if (num19 == 1)
														{
															break;
														}
														if (num19 != 2 && num19 != 2)
														{
															if (num19 != 3 && num19 != 3)
															{
																if (num19 != 6 && num19 != 6)
																{
																	goto IL_038b;
																}
																int num22 = (int)num5 ^ -1859873967;
																num18 = (num22 + -1749052421 - -1749052421) ^ -1859873967 ^ 0x75A8F0B5;
																continue;
															}
															num23 += current.FileSize;
															num18 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x6F80F44B);
															continue;
														}
														if ("".Length != 0)
														{
														}
														num18 = (int)((num5 << 3) | (num5 >> 29)) ^ -410586847;
													}
													goto IL_0395;
												}
											}
											finally
											{
												((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
												while (true)
												{
													int num24 = 1912188847;
													while (true)
													{
														nint num25 = num24 ^ -488538743;
														nuint num26 = (nuint)num25;
														int num27 = (int)((num5 = (uint)((int)num26 ^ -488538743 ^ -624549154 ^ -624549154)) % 3);
														if (num27 == 0)
														{
															break;
														}
														if (num27 == 1 || num27 == 1 || num27 != 2)
														{
															if ("".Length != 0)
															{
															}
															int num28 = (int)num5 ^ -2103235649;
															num24 = (num28 + 536350958 - 536350958) ^ -2103235649 ^ 0x24EE302F;
															continue;
														}
														goto end_IL_0436;
													}
													continue;
													end_IL_0436:
													break;
												}
											}
											List<UpdateEntry>.Enumerator enumerator2 = input.GetEnumerator();
											try
											{
												while (true)
												{
													if (enumerator2.MoveNext())
													{
														UpdateEntry current2;
														int num32;
														do
														{
															IL_09dc:
															current2 = enumerator2.Current;
															int num29 = 750933094;
															while (true)
															{
																nint num30 = num29 ^ 0x455D90CD;
																nuint num31 = (nuint)num30;
																num32 = (int)((num5 = (uint)((int)num31 ^ 0x455D90CD ^ 0x21EED30E ^ 0x21EED30E)) % 26);
																if (num32 == 0)
																{
																	num29 = 2126835361;
																	continue;
																}
																if (num32 == 1)
																{
																	break;
																}
																if (num32 != 2 && num32 != 2)
																{
																	if (num32 != 3 && num32 != 3)
																	{
																		if (num32 != 22 && num32 != 22)
																		{
																			if (num32 != 4 && num32 != 4)
																			{
																				if (num32 != 5 && num32 != 5)
																				{
																					if (num32 != 6 && num32 != 6)
																					{
																						if (num32 != 7 && num32 != 7)
																						{
																							if (num32 != 24 && num32 != 24)
																							{
																								if (num32 != 8 && num32 != 8)
																								{
																									if (num32 != 9 && num32 != 9)
																									{
																										if (num32 != 10 && num32 != 10)
																										{
																											if (num32 != 11 && num32 != 11)
																											{
																												if (num32 != 12 && num32 != 12)
																												{
																													if (num32 != 23 && num32 != 23)
																													{
																														if (num32 != 13 && num32 != 13)
																														{
																															if (num32 != 14 && num32 != 14)
																															{
																																if (num32 != 15 && num32 != 15)
																																{
																																	if (num32 != 16 && num32 != 16)
																																	{
																																		if (num32 != 25 && num32 != 25)
																																		{
																																			if (num32 != 17 && num32 != 17)
																																			{
																																				if (num32 != 18 && num32 != 18)
																																				{
																																					if (num32 != 19 && num32 != 19)
																																					{
																																						if (num32 != 20 && num32 != 20)
																																						{
																																							goto IL_09d6;
																																						}
																																						if ((object)"" == "")
																																						{
																																							CheckSumTable = current2.CheckSumTable;
																																						}
																																						int num33 = (int)num5 ^ -1198981836;
																																						num29 = (num33 + -1941487018 - -1941487018) ^ -1198981836 ^ 0x28D303A9;
																																						continue;
																																					}
																																					if ("".Length == 0)
																																					{
																																						DataOffset = current2.DataOffset;
																																					}
																																					num29 = (int)(0x762A3373 ^ num5);
																																					continue;
																																				}
																																				if ("".Length != 0)
																																				{
																																				}
																																				num29 = (int)(0x7C904809 ^ num5);
																																				continue;
																																			}
																																			if ("".Length == 0 && "".Length == 0)
																																			{
																																				Type = current2.Type;
																																			}
																																			num29 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0xFBBE9E8);
																																			continue;
																																		}
																																		if ((object)"" != "")
																																		{
																																		}
																																		num29 = (int)((num5 << 3) | (num5 >> 29)) ^ -1397948720;
																																		continue;
																																	}
																																	HeaderSize = current2.HeaderSize;
																																	int num34 = (int)(num5 ^ 0x19904D09);
																																	num29 = (num34 + -1888013041 - -1888013041) ^ 0x19904D09 ^ 0x28A74B3;
																																	continue;
																																}
																																if ((object)"" != "" || "".Length != 0)
																																{
																																}
																																num29 = (int)((num5 << 2) | (num5 >> 30)) ^ -964353106;
																																continue;
																															}
																															if ("".Length == 0)
																															{
																																HardwareId = current2.HardwareId;
																															}
																															num29 = (int)(0xEDD8B5 ^ num5);
																															continue;
																														}
																														if ("".Length != 0 || "".Length != 0 || "".Length != 0)
																														{
																														}
																														int num35 = (int)(num5 ^ 0x26840154);
																														num29 = (num35 + 713578217 - 713578217) ^ 0x26840154 ^ 0x6D817E55;
																														continue;
																													}
																													FileTime = current2.FileTime;
																													num29 = (int)(0x60FFBAC6 ^ num5);
																													continue;
																												}
																												if ("".Length != 0 || "".Length != 0)
																												{
																												}
																												num29 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x815CBE8);
																												continue;
																											}
																											if ("".Length == 0)
																											{
																												FileType = current2.FileType;
																											}
																											num29 = (int)(0x1CDF2CD9 ^ num5);
																											continue;
																										}
																										num29 = (int)(0x32375CE7 ^ num5);
																										continue;
																									}
																									if ((object)"" == "")
																									{
																										FileDate = current2.FileDate;
																									}
																									int num36 = (int)num5 ^ -1146817085;
																									num29 = (num36 + -2144215081 - -2144215081) ^ -1146817085 ^ 0x65E77DF7;
																									continue;
																								}
																								if ("".Length != 0)
																								{
																								}
																								num29 = (int)((num5 << 3) | (num5 >> 29)) ^ -1508041886;
																								continue;
																							}
																							if ((object)"" == "" && (object)"" == "")
																							{
																								HeaderId = current2.HeaderId;
																							}
																							num29 = (int)(0x4F8DC280 ^ num5);
																							continue;
																						}
																						num29 = (int)((num5 << 2) | (num5 >> 30)) ^ -2139612620;
																						continue;
																					}
																					if ((object)"" == "" && "".Length == 0)
																					{
																						HeaderChecksum = current2.HeaderChecksum;
																					}
																					num29 = (int)((num5 << 2) | (num5 >> 30)) ^ -380123828;
																					continue;
																				}
																				if ((object)"" != "")
																				{
																				}
																				int num37 = (int)num5 ^ -446663173;
																				num29 = (num37 + -516200163 - -516200163) ^ -446663173 ^ 0x3F91C103;
																				continue;
																			}
																			if ("".Length == 0)
																			{
																				FileSequence = current2.FileSequence;
																			}
																			num29 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x6B3DB0C0);
																			continue;
																		}
																		if ("".Length != 0 || (object)"" != "")
																		{
																		}
																		int num38 = (int)(num5 ^ 0x4D849BA0);
																		num29 = (num38 + -1791903592 - -1791903592) ^ 0x4D849BA0 ^ 0x24D35E22;
																		continue;
																	}
																	FileSize = current2.FileSize;
																	num29 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x5B79F39C);
																	continue;
																}
																if ("".Length != 0)
																{
																}
																num29 = (int)(((num5 << 3) | (num5 >> 29)) ^ 0x60F5C610);
															}
															goto IL_09dc;
															IL_09d6:;
														}
														while (num32 != 21);
														FileStream fileStream2 = new FileStream(current2.updateapp, (FileMode)num9, (FileAccess)num11, (FileShare)num11);
														try
														{
															while (true)
															{
																int num39 = 240100076;
																while (true)
																{
																	nint num40 = num39 ^ 0xC442281;
																	nuint num41 = (nuint)num40;
																	int num42 = (int)((num5 = (uint)((int)num41 ^ 0xC442281 ^ 0x5964AB9C ^ 0x5964AB9C)) % 25);
																	if (num42 == 0)
																	{
																		break;
																	}
																	if (num42 != 1 && num42 != 1)
																	{
																		if (num42 != 2 && num42 != 2)
																		{
																			if (num42 != 3 && num42 != 3)
																			{
																				if (num42 != 22 && num42 != 22)
																				{
																					if (num42 != 4)
																					{
																						if (num42 != 5 && num42 != 5)
																						{
																							if (num42 != 6 && num42 != 6)
																							{
																								if (num42 != 7 && num42 != 7)
																								{
																									if (num42 != 24 && num42 != 24)
																									{
																										if (num42 != 8 && num42 != 8)
																										{
																											if (num42 != 9 && num42 != 9)
																											{
																												if (num42 == 10 || num42 == 10)
																												{
																													int num43 = (int)(num5 ^ 0x6263DA80);
																													num39 = (num43 + -1650350520 - -1650350520) ^ 0x6263DA80 ^ 0x5CB59F6A;
																													continue;
																												}
																												if (num42 == 11 || num42 == 11)
																												{
																													if ((object)"" != "")
																													{
																														num39 = (int)(0x7C220BF ^ num5);
																														continue;
																													}
																													string format = text;
																													object[] array = new object[num12];
																													array[num8] = num44;
																													array[num11] = dataStream.Position - num45;
																													array[num6] = CheckSumTable[num44];
																													array[num9] = num46;
																													throw new Exception(string.Format(format, array));
																												}
																												if (num42 != 12)
																												{
																													if (num42 != 23)
																													{
																														if (num42 != 13 && num42 != 13)
																														{
																															if (num42 != 14 && num42 != 14)
																															{
																																if (num42 != 15 && num42 != 15)
																																{
																																	if (num42 != 16 && num42 != 16)
																																	{
																																		if (num42 != 21 && num42 != 21)
																																		{
																																			if (num42 != 17)
																																			{
																																				if (num42 != 18 && num42 != 18)
																																				{
																																					if (num42 != 19 && num42 != 19)
																																					{
																																						if (num42 != 20)
																																						{
																																							goto IL_0ed9;
																																						}
																																						goto end_IL_0f30;
																																					}
																																					num39 = (int)(0x6EA44022 ^ num5);
																																					continue;
																																				}
																																				if ("".Length != 0 || !flag)
																																				{
																																					int num47 = (int)(num5 ^ 0x4D4303B9);
																																					num39 = (num47 + 965495425 - 965495425) ^ 0x4D4303B9 ^ 0x45D56658;
																																					continue;
																																				}
																																				goto IL_0e15;
																																			}
																																			if ((object)"" == "")
																																			{
																																				goto IL_0e27;
																																			}
																																			goto IL_0e41;
																																		}
																																		if ("".Length != 0)
																																		{
																																		}
																																		num39 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x6E089682);
																																		continue;
																																	}
																																	if ("".Length == 0)
																																	{
																																		num44 += num11;
																																	}
																																	num39 = (int)(0xAAD9A4C ^ num5);
																																	continue;
																																}
																																if ("".Length != 0)
																																{
																																}
																																num39 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1F554B86);
																																continue;
																															}
																															if ("".Length == 0)
																															{
																																UI.Progresbar1(num23, Convert.ToInt64(output.Length));
																															}
																															num39 = (int)(0x5FF4DE25 ^ num5);
																															continue;
																														}
																														if ((object)"" != "" || (object)"" != "")
																														{
																														}
																														num39 = (int)((num5 << 3) | (num5 >> 29)) ^ -2081277260;
																														continue;
																													}
																													if ((object)"" == "" && "".Length == 0)
																													{
																														goto IL_0da0;
																													}
																													goto IL_0dac;
																												}
																												if ((object)"" != "")
																												{
																												}
																											}
																											else if (flag2)
																											{
																												num39 = (int)((num5 << 2) | (num5 >> 30)) ^ -113662126;
																												continue;
																											}
																											num39 = 1312803198;
																										}
																										else
																										{
																											flag2 = ((num46 == CheckSumTable[num44]) ? 1 : 0) == num8;
																											num39 = (int)(0x76D6F6C0 ^ num5);
																										}
																									}
																									else
																									{
																										if ("".Length == 0)
																										{
																											num46 = Class35.field_02.ComputeSum(buffer, num8, num45);
																										}
																										num39 = (int)(0x665C90F3 ^ num5);
																									}
																								}
																								else
																								{
																									int num48 = (int)num5 ^ -312830464;
																									num39 = (num48 + -1714830148 - -1714830148) ^ -312830464 ^ 0x3B9F3EC7;
																								}
																								continue;
																							}
																							if ("".Length != 0 || "".Length != 0 || "".Length != 0 || flag3)
																							{
																								num39 = (int)((num5 << 2) | (num5 >> 30)) ^ -1916582949;
																								continue;
																							}
																							goto IL_0da0;
																						}
																						if ("".Length == 0 && (object)"" == "")
																						{
																							flag3 = checksum;
																						}
																						num39 = (int)((num5 << 2) | (num5 >> 30)) ^ -333574509;
																						continue;
																					}
																					if ((object)"" == "" && (object)"" == "")
																					{
																					}
																					goto IL_0e15;
																				}
																				if ("".Length != 0)
																				{
																					num39 = (int)(0x59F1AA14 ^ num5);
																					continue;
																				}
																				goto IL_0e27;
																			}
																			if ((object)"" == "" && (object)"" == "")
																			{
																				num44 = num8;
																			}
																			int num49 = (int)num5 ^ -1384231934;
																			num39 = (num49 + -642356602 - -642356602) ^ -1384231934 ^ 0x2D0EFF10;
																			continue;
																		}
																		if ("".Length == 0)
																		{
																			buffer = new byte[BlockSize];
																		}
																		int num50 = (int)num5 ^ -1649083157;
																		num39 = (num50 + 504696801 - 504696801) ^ -1649083157 ^ 0x635B2EFF;
																		continue;
																	}
																	goto IL_0ed9;
																	IL_0da0:
																	fileStream.Write(buffer, num8, num45);
																	goto IL_0dac;
																	IL_0dac:
																	num39 = 492854288;
																	continue;
																	IL_0e15:
																	num39 = 1991498305;
																	continue;
																	IL_0e27:
																	flag = (num45 = dataStream.Read(buffer, num8, BlockSize)) > num8;
																	goto IL_0e41;
																	IL_0e41:
																	num39 = 1499687668;
																	continue;
																	IL_0ed9:
																	if ("".Length == 0)
																	{
																		dataStream = GetDataStream(fileStream2);
																	}
																	num39 = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x72E7997F);
																}
																continue;
																end_IL_0f30:
																break;
															}
														}
														finally
														{
															if (fileStream2 != null)
															{
																while (true)
																{
																	int num51 = 1532843305;
																	while (true)
																	{
																		nint num52 = num51 ^ -1318103041;
																		nuint num53 = (nuint)num52;
																		int num54 = (int)((num5 = (uint)((int)num53 ^ -1318103041 ^ -775189308 ^ -775189308)) % 4);
																		if (num54 == 0)
																		{
																			break;
																		}
																		if (num54 != 1 && num54 != 1)
																		{
																			if (num54 == 2 || num54 == 2)
																			{
																				num51 = (int)(0x1CB365D9 ^ num5);
																				continue;
																			}
																			if (num54 == 3)
																			{
																				goto end_IL_102d;
																			}
																		}
																		if ((object)"" == "" && (object)"" == "" && (object)"" == "")
																		{
																			((IDisposable)fileStream2).Dispose();
																		}
																		num51 = (int)(0x1F8499B ^ num5);
																	}
																	continue;
																	end_IL_102d:
																	break;
																}
															}
														}
														goto IL_107d;
													}
													goto IL_1047;
													IL_103f:
													if ("".Length == 0)
													{
														continue;
													}
													goto IL_1047;
													IL_1047:
													int num55 = 1149107030;
													goto IL_104c;
													IL_107d:
													num55 = 1035423808;
													goto IL_104c;
													IL_104c:
													nint num56 = num55 ^ -826086915;
													nuint num57 = (nuint)num56;
													switch ((int)((num5 = (uint)((int)num57 ^ -826086915 ^ -863709280 ^ -863709280)) % 3))
													{
													default:
														goto IL_103f;
													case 0:
														break;
													case 2:
														return;
													}
													goto IL_107d;
												}
											}
											finally
											{
												((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
												while (true)
												{
													IL_1107:
													int num58 = 1242356494;
													while (true)
													{
														nint num59 = num58 ^ 0x57DBBEE4;
														nuint num60 = (nuint)num59;
														int num61 = (int)((num5 = (uint)((int)num60 ^ 0x57DBBEE4 ^ 0x6C3D8DE6 ^ 0x6C3D8DE6)) % 3);
														switch (num61)
														{
														default:
															if (num61 == 1 || num61 != 2)
															{
																goto IL_10b2;
															}
															goto end_IL_10d6;
														case 1:
															goto IL_10b2;
														case 0:
															break;
														}
														goto IL_1107;
														IL_10b2:
														int num62 = (int)(num5 ^ 0x5FCFB020);
														num58 = (num62 + 966345918 - 966345918) ^ 0x5FCFB020 ^ 0x7BF8476D;
														continue;
														end_IL_10d6:
														break;
													}
													break;
												}
											}
										}
									}
									if ((object)"" == "")
									{
										num23 = (uint)num8;
									}
									num14 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x7356716E);
								}
							}
						}
						finally
						{
							if (fileStream != null)
							{
								while (true)
								{
									int num63 = 2069488777;
									while (true)
									{
										nint num64 = num63 ^ 0x15EC8201;
										nuint num65 = (nuint)num64;
										int num66 = (int)((num5 = (uint)((int)num65 ^ 0x15EC8201 ^ -1157694023 ^ -1157694023)) % 4);
										if (num66 == 0)
										{
											break;
										}
										if (num66 != 1 && num66 != 1)
										{
											if (num66 == 2 || num66 == 2)
											{
												if ("".Length != 0 || (object)"" != "")
												{
												}
												num63 = (int)(0x2943974D ^ num5);
												continue;
											}
											if (num66 == 3)
											{
												goto end_IL_11d1;
											}
										}
										if ("".Length == 0 && (object)"" == "")
										{
											((IDisposable)fileStream).Dispose();
										}
										int num67 = (int)num5 ^ -1965857719;
										num63 = (num67 + -104240186 - -104240186) ^ -1965857719 ^ 0x5810E2D3;
									}
									continue;
									end_IL_11d1:
									break;
								}
							}
						}
					}
				}
				if ("".Length != 0 || (object)"" != "")
				{
				}
				num = (int)((num5 << 2) | (num5 >> 30)) ^ -409435476;
			}
		}
	}

	public void Extract(string input, string output, bool checksum = true)
	{
		using FileStream input2 = new FileStream(input, FileMode.Open, FileAccess.Read, FileShare.Read);
		using FileStream output2 = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.None);
		Extract(input2, output2, checksum);
	}

	public void ExtractHeader(Stream stream)
	{
		byte[] header = GetHeader();
		stream.Write(header, 0, header.Length);
	}

	public void ExtractHeader(string output)
	{
		using FileStream stream = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.None);
		ExtractHeader(stream);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
