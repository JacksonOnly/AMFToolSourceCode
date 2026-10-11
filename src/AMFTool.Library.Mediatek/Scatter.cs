using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AMFTool.Library.Mediatek;

public class Scatter
{
	public List<string> UPDATE;

	public List<string> part_RESERVED;

	public List<string> part_PROTECTED;

	public List<string> part_INVISIBLE;

	public List<string> part_BINREGION;

	public static string Partmtk(string part)
	{
		string text = ".img";
		string text2 = "preloader";
		string text3 = "preloader.bin";
		string text4 = "expdb";
		string text5 = "expdb.bin";
		string text6 = "userarea";
		string text7 = "userarea.bin";
		string text8 = "frp";
		string text9 = "frp.bin";
		string text10 = "gz1";
		string text11 = "gz1.bin";
		string text12 = "gz2";
		string text13 = "gz2.bin";
		string text14 = "lk";
		string text15 = "lk.bin";
		string text16 = "k2";
		string text17 = "k2.bin";
		string text18 = "logo";
		string text19 = "logo.bin";
		string text20 = "md_udc";
		string text21 = "md_udc.bin";
		string text22 = "para";
		string text23 = "para.bin";
		string text24 = "pgpt";
		string text25 = "pgpt.bin";
		string text26 = "proinfo";
		string text27 = "proinfo.bin";
		string text28 = "sec1";
		string text29 = "sec1.bin";
		string text30 = "seccfg";
		string text31 = "seccfg.bin";
		string text32 = "up_param";
		string text33 = "up_param.bin";
		string text34 = "boot2";
		string text35 = "boot2.bin";
		string text36 = "boot1";
		string text37 = "boot1.bin";
		string result = part + text;
		if (part == text2)
		{
			result = text3;
		}
		if (part == text4)
		{
			result = text5;
		}
		if (part == text6)
		{
			result = text7;
		}
		if (part == text8)
		{
			result = text9;
		}
		if (part == text10)
		{
			result = text11;
		}
		if (part == text12)
		{
			result = text13;
		}
		if (part == text14)
		{
			result = text15;
		}
		if (part == text16)
		{
			result = text17;
		}
		if (part == text18)
		{
			result = text19;
		}
		if (part == text20)
		{
			result = text21;
		}
		if (part == text22)
		{
			result = text23;
		}
		if (part == text24)
		{
			result = text25;
		}
		if (part == text26)
		{
			result = text27;
		}
		if (part == text28)
		{
			result = text29;
		}
		if (part == text30)
		{
			result = text31;
		}
		if (part == text32)
		{
			result = text33;
		}
		if (part == text34)
		{
			result = text35;
		}
		if (part == text36)
		{
			result = text37;
		}
		return result;
	}

	public static bool FileSupport(string input)
	{
		string text = "preloader";
		string text2 = "recovery";
		string text3 = "vbmeta_system";
		string text4 = "vbmeta_vendor";
		string text5 = "md1img";
		string text6 = "vbmeta";
		string text7 = "spmfw";
		string text8 = "scp1";
		string text9 = "scp2";
		string text10 = "sspm_1";
		string text11 = "sspm_2";
		string text12 = "lk1";
		string text13 = "lk2";
		string text14 = "boot";
		string text15 = "logo";
		string text16 = "dtbo";
		string text17 = "tee1";
		string text18 = "tee2";
		string text19 = "super";
		string text20 = "system";
		string text21 = "cache";
		string text22 = "userdata";
		string text23 = "cust";
		string text24 = "exeid";
		if (!(input == text))
		{
			if (input == text2)
			{
				return true;
			}
			if (input == text3)
			{
				return true;
			}
			if (!(input == text4))
			{
				if (!(input == text5))
				{
					if (input == text6)
					{
						return true;
					}
					if (!(input == text7))
					{
						if (!(input == text8))
						{
							if (input == text9)
							{
								return true;
							}
							if (input == text10)
							{
								return true;
							}
							if (input == text11)
							{
								return true;
							}
							if (!(input == text12))
							{
								if (input == text13)
								{
									return true;
								}
								if (!(input == text14))
								{
									if (input == text15)
									{
										return true;
									}
									if (input == text16)
									{
										return true;
									}
									if (!(input == text17))
									{
										if (input == text18)
										{
											return true;
										}
										if (!(input == text19))
										{
											if (input == text20)
											{
												return true;
											}
											if (!(input == text21))
											{
												if (!(input == text22))
												{
													if (!(input == text23))
													{
														if (input == text24)
														{
															return true;
														}
														if (!(input == text7))
														{
															return false;
														}
														return true;
													}
													return true;
												}
												return true;
											}
											return true;
										}
										return true;
									}
									return true;
								}
								return true;
							}
							return true;
						}
						return true;
					}
					return true;
				}
				return true;
			}
			return true;
		}
		return true;
	}

	public static string GetQcFiles(string label)
	{
		string text = "aboot";
		string result = "emmc_appsboot.mbn";
		string text2 = "abootbak";
		string result2 = "emmc_appsbootbak.mbn";
		string text3 = "apdp";
		string result3 = "dpAP.mbn";
		string text4 = "BackupGPT";
		string result4 = "gpt_backup0.bin";
		string text5 = "boot";
		string result5 = "boot.img";
		string text6 = "cache";
		string result6 = "cache.img";
		string text7 = "cmnlib";
		string result7 = "cmnlib.mbn";
		string text8 = "cmnlibbak";
		string result8 = "cmnlibbak.mbn";
		string text9 = "cmnlib64";
		string result9 = "cmnlib64.mbn";
		string text10 = "cmnlib64bak";
		string result10 = "cmnlib64bak.mbn";
		string text11 = "devcfg";
		string result11 = "devcfg.mbn";
		string text12 = "devcfgbak";
		string result12 = "devcfgbak.mbn";
		string text13 = "DRIVER";
		string result13 = "DRIVER.ISO";
		string text14 = "dsp";
		string result14 = "adspso.bin";
		string text15 = "dtbo";
		string result15 = "dtbo.img";
		string text16 = "keymaster";
		string result16 = "keymaster64.mbn";
		string text17 = "keymasterbak";
		string result17 = "keymasterbak64.mbn";
		string result18 = "keymaster.mbn";
		string result19 = "keymasterbak.mbn";
		string text18 = "lksecapp";
		string result20 = "lksecapp.mbn";
		string text19 = "lksecappbak";
		string result21 = "lksecappbak.mbn";
		string text20 = "LOGO";
		string result22 = "logo.bin";
		string text21 = "mdtp";
		string result23 = "mdtp.img";
		string text22 = "misc";
		string result24 = "misc.img";
		string text23 = "modem";
		string result25 = "NON-HLOS.bin";
		string text24 = "oppodycnvbk";
		string result26 = "dynamic_nvbk.bin";
		string text25 = "opporeserve1";
		string result27 = "emmc_fw.bin";
		string text26 = "opporeserve2";
		string result28 = "opporeserve2.img";
		string text27 = "oppostanvbk";
		string result29 = "static_nvbk.bin";
		string text28 = "persist";
		string result30 = "persist.img";
		string text29 = "PrimaryGPT";
		string result31 = "gpt_main0.bin";
		string text30 = "recovery";
		string result32 = "recovery.img";
		string text31 = "rpm";
		string result33 = "rpm.mbn";
		string text32 = "rpmbak";
		string result34 = "rpmbak.mbn";
		string text33 = "sbl1";
		string result35 = "sbl1.mbn";
		string text34 = "sbl1bak";
		string result36 = "sbl1bak.mbn";
		string text35 = "sec";
		string result37 = "sec.dat";
		string text36 = "system";
		string result38 = "system.img";
		string text37 = "tz";
		string result39 = "tz.mbn";
		string text38 = "tzbak";
		string result40 = "tzbak.mbn";
		string text39 = "userdata";
		string result41 = "userdata.img";
		string text40 = "vbmeta";
		string result42 = "vbmeta.img";
		string text41 = "vendor";
		string result43 = "vendor.img";
		string text42 = ".bin";
		if (!(label == text))
		{
			if (!(label == text2))
			{
				if (label == text3)
				{
					return result3;
				}
				if (!(label == text4))
				{
					if (label == text5)
					{
						return result5;
					}
					if (!(label == text6))
					{
						if (!(label == text7))
						{
							if (!(label == text8))
							{
								if (label == text9)
								{
									return result9;
								}
								if (!(label == text10))
								{
									if (label == text11)
									{
										return result11;
									}
									if (label == text12)
									{
										return result12;
									}
									if (!(label == text13))
									{
										if (label == text14)
										{
											return result14;
										}
										if (label == text15)
										{
											return result15;
										}
										if (label == text16)
										{
											return result16;
										}
										if (!(label == text17))
										{
											if (!(label == text16))
											{
												if (!(label == text17))
												{
													if (!(label == text18))
													{
														if (!(label == text19))
														{
															if (!(label == text20))
															{
																if (!(label == text21))
																{
																	if (!(label == text22))
																	{
																		if (!(label == text23))
																		{
																			if (!(label == text24))
																			{
																				if (!(label == text25))
																				{
																					if (!(label == text26))
																					{
																						if (label == text27)
																						{
																							return result29;
																						}
																						if (label == text28)
																						{
																							return result30;
																						}
																						if (!(label == text29))
																						{
																							if (!(label == text30))
																							{
																								if (label == text31)
																								{
																									return result33;
																								}
																								if (!(label == text32))
																								{
																									if (label == text33)
																									{
																										return result35;
																									}
																									if (!(label == text34))
																									{
																										if (!(label == text35))
																										{
																											if (label == text36)
																											{
																												return result38;
																											}
																											if (label == text37)
																											{
																												return result39;
																											}
																											if (!(label == text38))
																											{
																												if (label == text39)
																												{
																													return result41;
																												}
																												if (label == text40)
																												{
																													return result42;
																												}
																												if (!(label == text41))
																												{
																													return label + text42;
																												}
																												return result43;
																											}
																											return result40;
																										}
																										return result37;
																									}
																									return result36;
																								}
																								return result34;
																							}
																							return result32;
																						}
																						return result31;
																					}
																					return result28;
																				}
																				return result27;
																			}
																			return result26;
																		}
																		return result25;
																	}
																	return result24;
																}
																return result23;
															}
															return result22;
														}
														return result21;
													}
													return result20;
												}
												return result19;
											}
											return result18;
										}
										return result17;
									}
									return result13;
								}
								return result10;
							}
							return result8;
						}
						return result7;
					}
					return result6;
				}
				return result4;
			}
			return result2;
		}
		return result;
	}

	public bool BuildScatterXml(string cpu, string storage, string filePath)
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		string text2 = "<root>\n";
		string text3 = "  <general name=\"MTK_PLATFORM_CFG\">\n";
		string text4 = "    <config_version name=\"V2.2.0\">\n";
		string text5 = "      <platform>";
		string text6 = "</platform>\n";
		string text7 = "      <project>AMFTool</project>\n";
		string text8 = "      <storage>";
		string text9 = "</storage>\n";
		string text10 = "      <boot_channel>MSDC_0</boot_channel>\n";
		string text11 = "      <block_size>0x20000</block_size>\n";
		string text12 = "    </config_version>\n";
		string text13 = "  </general>\n";
		string text14 = text;
		text14 += text2;
		text14 += text3;
		text14 += text4;
		text14 = text14 + text5 + cpu + text6;
		text14 += text7;
		text14 = text14 + text8 + storage + text9;
		text14 += text10;
		text14 += text11;
		text14 += text12;
		text14 += text13;
		File.WriteAllText(filePath, text14);
		return true;
	}

	public bool BuildScatterTxt(string cpu, string storage, string filePath)
	{
		string text = "############################################################################################################\n";
		string text2 = "#\n";
		string text3 = "#  General Setting\n";
		string text4 = "- general: MTK_PLATFORM_CFG\n";
		string text5 = "  info:\n";
		string text6 = "    - config_version: V1.1.2\n";
		string text7 = "      platform: ";
		string text8 = "\n";
		string text9 = "      project: AMFTool\n";
		string text10 = "      storage: ";
		string text11 = "      boot_channel: MSDC_0\n";
		string text12 = "      block_size: 0x20000\n";
		string text13 = "#  Layout Setting\n";
		string text14 = "############################################################################################################\n\n";
		string text15 = text;
		text15 += text2;
		text15 += text3;
		text15 += text2;
		text15 += text;
		text15 += text4;
		text15 += text5;
		text15 += text6;
		text15 = text15 + text7 + cpu + text8;
		text15 += text9;
		text15 = text15 + text10 + storage + text8;
		text15 += text11;
		text15 += text12;
		text15 += text;
		text15 += text2;
		text15 += text13;
		text15 += text2;
		text15 += text14;
		File.WriteAllText(filePath, text15);
		return true;
	}

	public bool BuildPreloaderTxt(string storage, string sysIndex, string partitionName, string fileName, int bootSize, string filePath)
	{
		string text = "- partition_index: SYS";
		string text2 = "\n";
		string text3 = "  partition_name: ";
		string text4 = "  file_name: ";
		string text5 = "  is_download: true\n";
		string text6 = "  type: SV5_BL_BIN\n";
		string text7 = "  linear_start_addr: 0x0\n";
		string text8 = "  physical_start_addr: 0x0\n";
		string format = "  partition_size: 0x{0:x}\n";
		string text9 = "EMMC";
		string text10 = "  region: EMMC_BOOT1_BOOT2\n";
		string text11 = "  storage: HW_STORAGE_";
		string text12 = "UFS";
		string text13 = "  region: UFS_LU0_LU1\n";
		string text14 = "  boundary_check: true\n";
		string text15 = "  is_reserved: false\n";
		string text16 = "  operation_type: BOOTLOADERS\n";
		string text17 = "  is_upgradable: true\n";
		string text18 = "  empty_boot_needed: false\n";
		string text19 = "  combo_partsize_check: false\n";
		string text20 = "  reserve: 0x00\n\n";
		string text21 = text + sysIndex + text2;
		text21 = text21 + text3 + partitionName + text2;
		text21 = text21 + text4 + fileName + text2;
		text21 += text5;
		text21 += text6;
		text21 += text7;
		text21 += text8;
		text21 += string.Format(format, bootSize);
		if (!(storage == text9))
		{
			if (storage == text12)
			{
				text21 += text13;
				text21 = text21 + text11 + storage + text2;
				text21 += text14;
				text21 += text15;
				text21 += text16;
				text21 += text17;
				text21 += text18;
				text21 += text19;
				text21 += text20;
				File.AppendAllText(filePath, text21);
				return true;
			}
			text21 += text14;
			text21 += text15;
			text21 += text16;
			text21 += text17;
			text21 += text18;
			text21 += text19;
			text21 += text20;
			File.AppendAllText(filePath, text21);
			return true;
		}
		text21 += text10;
		text21 = text21 + text11 + storage + text2;
		text21 += text14;
		text21 += text15;
		text21 += text16;
		text21 += text17;
		text21 += text18;
		text21 += text19;
		text21 += text20;
		File.AppendAllText(filePath, text21);
		return true;
	}

	public bool BuildPreloaderXml(string storage, string sysIndex, string partitionName, string fileName, int bootSize, string filePath)
	{
		string text = "  <partition_index name=\"SYS";
		string text2 = "\">\n";
		string text3 = "    <partition_name>";
		string text4 = "</partition_name>\n";
		string text5 = "    <file_name>";
		string text6 = "</file_name>\n";
		string text7 = "    <is_download>true</is_download>\n";
		string text8 = "    <type>SV5_BL_BIN</type>\n";
		string text9 = "    <linear_start_addr>0x0</linear_start_addr>\n";
		string text10 = "    <physical_start_addr>0x0</physical_start_addr>\n";
		string format = "    <partition_size>0x{0:x}</partition_size>\n";
		string text11 = "EMMC";
		string text12 = "    <region>EMMC_BOOT1_BOOT2</region>\n";
		string text13 = "    <storage>HW_STORAGE_";
		string text14 = "</storage>\n";
		string text15 = "UFS";
		string text16 = "    <region>UFS_LU0_LU1</region>\n";
		string text17 = "    <boundary_check>true</boundary_check>\n";
		string text18 = "    <is_reserved>false</is_reserved>\n";
		string text19 = "    <operation_type>BOOTLOADERS</operation_type>\n";
		string text20 = "    <is_upgradable>true</is_upgradable>";
		string text21 = "    <empty_boot_needed>false</empty_boot_needed>\n";
		string text22 = "    <combo_partsize_check>false</combo_partsize_check>\n";
		string text23 = "    <reserve>0x00</reserve>\n";
		string text24 = "  </partition_index>\n";
		string text25 = text + sysIndex + text2;
		text25 = text25 + text3 + partitionName + text4;
		text25 = text25 + text5 + fileName + text6;
		text25 += text7;
		text25 += text8;
		text25 += text9;
		text25 += text10;
		text25 += string.Format(format, bootSize / 128);
		if (storage == text11)
		{
			text25 += text12;
			text25 = text25 + text13 + storage + text14;
		}
		else if (storage == text15)
		{
			text25 += text16;
			text25 = text25 + text13 + storage + text14;
		}
		text25 += text17;
		text25 += text18;
		text25 += text19;
		text25 += text20;
		text25 += text21;
		text25 += text22;
		text25 += text23;
		text25 += text24;
		File.AppendAllText(filePath, text25);
		return true;
	}

	public bool BuildGptTxt(string storage, string sysIndex, string partitionName, string fileName, int bootSize, string filePath)
	{
		string text = "- partition_index: SYS";
		string text2 = "\n";
		string text3 = "  partition_name: ";
		string text4 = "  file_name: ";
		string text5 = "  is_download: false\n";
		string text6 = "  type: NORMAL_ROM\n";
		string text7 = "  linear_start_addr: 0x0\n";
		string text8 = "  physical_start_addr: 0x0\n";
		string format = "  partition_size: 0x{0:x}\n";
		string text9 = "EMMC";
		string text10 = "  region: EMMC_USER\n";
		string text11 = "  storage: HW_STORAGE_";
		string text12 = "UFS";
		string text13 = "  region: UFS_LU2\n";
		string text14 = "  boundary_check: true\n";
		string text15 = "  is_reserved: false\n";
		string text16 = "  operation_type: INVISIBLE\n";
		string text17 = "  is_upgradable: false\n";
		string text18 = "  empty_boot_needed: false\n";
		string text19 = "  combo_partsize_check: false\n";
		string text20 = "  reserve: 0x00\n\n";
		string text21 = text + sysIndex + text2;
		text21 = text21 + text3 + partitionName + text2;
		text21 = text21 + text4 + Partmtk(fileName) + text2;
		text21 += text5;
		text21 += text6;
		text21 += text7;
		text21 += text8;
		text21 += string.Format(format, bootSize / 128);
		if (!(storage == text9))
		{
			if (storage == text12)
			{
				text21 += text13;
				text21 = text21 + text11 + storage + text2;
				text21 += text14;
				text21 += text15;
				text21 += text16;
				text21 += text17;
				text21 += text18;
				text21 += text19;
				text21 += text20;
				File.AppendAllText(filePath, text21);
				return true;
			}
			text21 += text14;
			text21 += text15;
			text21 += text16;
			text21 += text17;
			text21 += text18;
			text21 += text19;
			text21 += text20;
			File.AppendAllText(filePath, text21);
			return true;
		}
		text21 += text10;
		text21 = text21 + text11 + storage + text2;
		text21 += text14;
		text21 += text15;
		text21 += text16;
		text21 += text17;
		text21 += text18;
		text21 += text19;
		text21 += text20;
		File.AppendAllText(filePath, text21);
		return true;
	}

	public bool BuildGptXml(string storage, string sysIndex, string partitionName, string fileName, int bootSize, string filePath)
	{
		string text = "  <partition_index name=\"SYS";
		string text2 = "\">\n";
		string text3 = "    <partition_name>";
		string text4 = "</partition_name>\n";
		string text5 = "    <file_name>";
		string text6 = "</file_name>\n";
		string text7 = "    <is_download>false</is_download>\n";
		string text8 = "    <type>NORMAL_ROM</type>\n";
		string text9 = "    <linear_start_addr>0x0</linear_start_addr>\n";
		string text10 = "    <physical_start_addr>0x0</physical_start_addr>\n";
		string format = "    <partition_size>0x{0:x}</partition_size>\n";
		string text11 = "EMMC";
		string text12 = "    <region>EMMC_USER</region>\n";
		string text13 = "    <storage>HW_STORAGE_";
		string text14 = "</storage>\n";
		string text15 = "UFS";
		string text16 = "    <region>UFS_LU2</region>\n";
		string text17 = "    <boundary_check>true</boundary_check>\n";
		string text18 = "    <is_reserved>false</is_reserved>\n";
		string text19 = "    <operation_type>INVISIBLE</operation_type>\n";
		string text20 = "    <is_upgradable>false</is_upgradable>\n";
		string text21 = "    <empty_boot_needed>false</empty_boot_needed>\n";
		string text22 = "    <combo_partsize_check>false</combo_partsize_check>\n";
		string text23 = "    <reserve>0x00</reserve>\n";
		string text24 = "  </partition_index>\n";
		string text25 = text + sysIndex + text2;
		text25 = text25 + text3 + partitionName + text4;
		text25 = text25 + text5 + fileName + text6;
		text25 += text7;
		text25 += text8;
		text25 += text9;
		text25 += text10;
		text25 += string.Format(format, bootSize);
		if (!(storage == text11))
		{
			if (storage == text15)
			{
				text25 += text16;
				text25 = text25 + text13 + storage + text14;
				text25 += text17;
				text25 += text18;
				text25 += text19;
				text25 += text20;
				text25 += text21;
				text25 += text22;
				text25 += text23;
				text25 += text24;
				File.AppendAllText(filePath, text25);
				return true;
			}
			text25 += text17;
		}
		else
		{
			text25 += text12;
			text25 = text25 + text13 + storage + text14;
			text25 += text17;
		}
		text25 += text18;
		text25 += text19;
		text25 += text20;
		text25 += text21;
		text25 += text22;
		text25 += text23;
		text25 += text24;
		File.AppendAllText(filePath, text25);
		return true;
	}

	private bool BuildPartitionScatter(string storage, string sysIndex, string partitionName, string fileName, string isDownload, string startAddr, string partitionSize, string operationType, string filePath)
	{
		string text = "- partition_index: SYS";
		string text2 = "\n";
		string text3 = "  partition_name: ";
		string text4 = "  file_name: ";
		string text5 = "  is_download: ";
		string text6 = "  type: NORMAL_ROM\n";
		string text7 = "  linear_start_addr: ";
		string text8 = "  physical_start_addr: ";
		string text9 = "  partition_size: ";
		string text10 = "EMMC";
		string text11 = "  region: EMMC_BOOT2\n";
		string text12 = "  storage: HW_STORAGE_";
		string text13 = "UFS";
		string text14 = "  region: UFS_LU2\n";
		string text15 = "  boundary_check: true\n";
		string text16 = "  is_reserved: false\n";
		string text17 = "  operation_type: ";
		string text18 = "  is_upgradable: false\n";
		string text19 = "  empty_boot_needed: false\n";
		string text20 = "  combo_partsize_check: false\n";
		string text21 = "  reserve: 0x00\n\n";
		string text22 = text + sysIndex + text2;
		text22 = text22 + text3 + partitionName + text2;
		text22 = text22 + text4 + Partmtk(fileName) + text2;
		text22 = text22 + text5 + isDownload + text2;
		text22 += text6;
		text22 = text22 + text7 + startAddr + text2;
		text22 = text22 + text8 + startAddr + text2;
		text22 = text22 + text9 + partitionSize + text2;
		if (storage == text10)
		{
			text22 += text11;
			text22 = text22 + text12 + storage + text2;
			text22 += text15;
			text22 += text16;
			text22 = text22 + text17 + operationType + text2;
			text22 += text18;
			text22 += text19;
			text22 += text20;
			text22 += text21;
			File.AppendAllText(filePath, text22);
			return true;
		}
		if (storage == text13)
		{
			text22 += text14;
			text22 = text22 + text12 + storage + text2;
			text22 += text15;
			text22 += text16;
			text22 = text22 + text17 + operationType + text2;
			text22 += text18;
			text22 += text19;
			text22 += text20;
			text22 += text21;
			File.AppendAllText(filePath, text22);
			return true;
		}
		text22 += text15;
		text22 += text16;
		text22 = text22 + text17 + operationType + text2;
		text22 += text18;
		text22 += text19;
		text22 += text20;
		text22 += text21;
		File.AppendAllText(filePath, text22);
		return true;
	}

	private bool BuildPartitionXml(string storage, string sysIndex, string partitionName, string fileName, string isDownload, string startAddr, string partitionSize, string operationType, string filePath)
	{
		string text = "  <partition_index name=\"SYS";
		string text2 = "\">\n";
		string text3 = "    <partition_name>";
		string text4 = "</partition_name>\n";
		string text5 = "    <file_name>";
		string text6 = "</file_name>\n";
		string text7 = "    <is_download>";
		string text8 = "</is_download>\n";
		string text9 = "    <type>NORMAL_ROM</type>\n";
		string text10 = "    <linear_start_addr>";
		string text11 = "</linear_start_addr>\n";
		string text12 = "    <physical_start_addr>";
		string text13 = "</physical_start_addr>\n";
		string text14 = "    <partition_size>";
		string text15 = "</partition_size>\n";
		string text16 = "EMMC";
		string text17 = "    <region>EMMC_USER</region>\n";
		string text18 = "    <storage>HW_STORAGE_";
		string text19 = "</storage>\n";
		string text20 = "UFS";
		string text21 = "    <region>UFS_LU2</region>\n";
		string text22 = "    <boundary_check>true</boundary_check>\n";
		string text23 = "    <is_reserved>false</is_reserved>\n";
		string text24 = "    <operation_type>";
		string text25 = "</operation_type>\n";
		string text26 = "    <is_upgradable>false</is_upgradable>\n";
		string text27 = "    <empty_boot_needed>false</empty_boot_needed>\n";
		string text28 = "    <combo_partsize_check>false</combo_partsize_check>\n";
		string text29 = "    <reserve>0x00</reserve>\n";
		string text30 = "  </partition_index>\n";
		string text31 = text + sysIndex + text2;
		text31 = text31 + text3 + partitionName + text4;
		text31 = text31 + text5 + fileName + text6;
		text31 = text31 + text7 + isDownload + text8;
		text31 += text9;
		text31 = text31 + text10 + startAddr + text11;
		text31 = text31 + text12 + startAddr + text13;
		text31 = text31 + text14 + partitionSize + text15;
		if (storage == text16)
		{
			text31 += text17;
			text31 = text31 + text18 + storage + text19;
		}
		else if (storage == text20)
		{
			text31 += text21;
			text31 = text31 + text18 + storage + text19;
		}
		text31 += text22;
		text31 += text23;
		text31 = text31 + text24 + operationType + text25;
		text31 += text26;
		text31 += text27;
		text31 += text28;
		text31 += text29;
		text31 += text30;
		File.AppendAllText(filePath, text31);
		return true;
	}

	public bool GetPartitionTxt(string storage, string sysIndex, string partitionName, string startAddr, string partitionSize, string filePath)
	{
		string isDownload = "true";
		string operationType = "UPDATE";
		string operationType2 = "RESERVED";
		string operationType3 = "PROTECTED";
		string operationType4 = "INVISIBLE";
		string operationType5 = "BINREGION";
		bool flag = false;
		for (int i = 0; i < UPDATE.Count(); i++)
		{
			if (UPDATE[i] == Partmtk(partitionName))
			{
				BuildPartitionScatter(storage, sysIndex, UPDATE[i], UPDATE[i], isDownload, startAddr, partitionSize, operationType, filePath);
				flag = true;
				return true;
			}
		}
		for (int j = 0; j < part_RESERVED.Count(); j++)
		{
			if (part_RESERVED[j] == Partmtk(partitionName))
			{
				BuildPartitionScatter(storage, sysIndex, part_RESERVED[j], part_RESERVED[j], isDownload, startAddr, partitionSize, operationType2, filePath);
				flag = true;
				return true;
			}
		}
		for (int k = 0; k < part_PROTECTED.Count(); k++)
		{
			if (part_PROTECTED[k] == Partmtk(partitionName))
			{
				BuildPartitionScatter(storage, sysIndex, part_PROTECTED[k], part_PROTECTED[k], isDownload, startAddr, partitionSize, operationType3, filePath);
				flag = true;
				return true;
			}
		}
		for (int l = 0; l < part_INVISIBLE.Count(); l++)
		{
			if (part_INVISIBLE[l] == Partmtk(partitionName))
			{
				BuildPartitionScatter(storage, sysIndex, part_INVISIBLE[l], part_INVISIBLE[l], isDownload, startAddr, partitionSize, operationType4, filePath);
				flag = true;
				return true;
			}
		}
		for (int m = 0; m < part_BINREGION.Count(); m++)
		{
			if (part_BINREGION[m] == Partmtk(partitionName))
			{
				BuildPartitionScatter(storage, sysIndex, part_BINREGION[m], part_BINREGION[m], isDownload, startAddr, partitionSize, operationType5, filePath);
				flag = true;
				return true;
			}
		}
		if (!flag)
		{
			BuildPartitionScatter(storage, sysIndex, partitionName, partitionName, isDownload, startAddr, partitionSize, operationType4, filePath);
			flag = true;
		}
		return flag;
	}

	public bool GetPartitionXml(string storage, string sysIndex, string partitionName, string startAddr, string partitionSize, string filePath)
	{
		string isDownload = "true";
		string operationType = "UPDATE";
		string operationType2 = "RESERVED";
		string operationType3 = "PROTECTED";
		string operationType4 = "INVISIBLE";
		string operationType5 = "BINREGION";
		bool flag = false;
		for (int i = 0; i < UPDATE.Count(); i++)
		{
			if (UPDATE[i] == Partmtk(partitionName))
			{
				BuildPartitionXml(storage, sysIndex, UPDATE[i], UPDATE[i], isDownload, startAddr, partitionSize, operationType, filePath);
				flag = true;
				return true;
			}
		}
		for (int j = 0; j < part_RESERVED.Count(); j++)
		{
			if (part_RESERVED[j] == Partmtk(partitionName))
			{
				BuildPartitionXml(storage, sysIndex, part_RESERVED[j], part_RESERVED[j], isDownload, startAddr, partitionSize, operationType2, filePath);
				flag = true;
				return true;
			}
		}
		for (int k = 0; k < part_PROTECTED.Count(); k++)
		{
			if (part_PROTECTED[k] == Partmtk(partitionName))
			{
				BuildPartitionXml(storage, sysIndex, part_PROTECTED[k], part_PROTECTED[k], isDownload, startAddr, partitionSize, operationType3, filePath);
				flag = true;
				return true;
			}
		}
		for (int l = 0; l < part_INVISIBLE.Count(); l++)
		{
			if (part_INVISIBLE[l] == Partmtk(partitionName))
			{
				BuildPartitionXml(storage, sysIndex, part_INVISIBLE[l], part_INVISIBLE[l], isDownload, startAddr, partitionSize, operationType4, filePath);
				flag = true;
				return true;
			}
		}
		for (int m = 0; m < part_BINREGION.Count(); m++)
		{
			if (part_BINREGION[m] == Partmtk(partitionName))
			{
				BuildPartitionXml(storage, sysIndex, part_BINREGION[m], part_BINREGION[m], isDownload, startAddr, partitionSize, operationType5, filePath);
				flag = true;
				return true;
			}
		}
		if (!flag)
		{
			BuildPartitionXml(storage, sysIndex, partitionName, partitionName, isDownload, startAddr, partitionSize, operationType4, filePath);
			flag = true;
		}
		return flag;
	}

	public Scatter()
	{
		string item = "system";
		string item2 = "recovery";
		string item3 = "vbmeta_system";
		string item4 = "vbmeta_vendor";
		string item5 = "md1img";
		string item6 = "spmfw";
		string item7 = "efuse";
		string item8 = "prism";
		string item9 = "optics";
		string item10 = "scp1";
		string item11 = "scp2";
		string item12 = "sspm_1";
		string item13 = "sspm_2";
		string item14 = "lk";
		string item15 = "lk2";
		string item16 = "boot";
		string item17 = "dtbo";
		string item18 = "param";
		string item19 = "up_param";
		string item20 = "vendor";
		string item21 = "exaid";
		string item22 = "logo";
		string item23 = "odm";
		string item24 = "secro";
		string item25 = "omr";
		string item26 = "tee1";
		string item27 = "odmdtbo";
		string item28 = "cust";
		string item29 = "tee2";
		string item30 = "tzar";
		string item31 = "super";
		string item32 = "vbmeta";
		string item33 = "cache";
		string item34 = "userdata";
		string item35 = "otp";
		string item36 = "flashinfo";
		string item37 = "sgpt";
		string item38 = "persist";
		string item39 = "persistent";
		string item40 = "proinfo";
		string item41 = "nvcfg";
		string item42 = "protect1";
		string item43 = "persistbak";
		string item44 = "protect2";
		string item45 = "efs";
		string item46 = "sec_efs";
		string item47 = "steady";
		string item48 = "btd";
		string item49 = "bota";
		string item50 = "rsv1";
		string item51 = "spu";
		string item52 = "opporeserve1";
		string item53 = "opporeserve2";
		string item54 = "opporeserve3";
		string item55 = "oppo_custom";
		string item56 = "gz1";
		string item57 = "gz2";
		string item58 = "boot_para";
		string item59 = "para";
		string item60 = "misc";
		string item61 = "expdb";
		string item62 = "frp";
		string item63 = "nvdata";
		string item64 = "md_udc";
		string item65 = "gsort";
		string item66 = "minidump";
		string item67 = "ffu";
		string item68 = "metadata";
		string item69 = "seccfg";
		string item70 = "sec1";
		string item71 = "sec2";
		string item72 = "keyrefuge";
		string item73 = "keydata";
		string item74 = "pad";
		string item75 = "devinfo";
		string item76 = "product";
		string item77 = "oemkeystore";
		string item78 = "keystore";
		string item79 = "box";
		string item80 = "systeminfo";
		string item81 = "sutinfo";
		string item82 = "cda";
		string item83 = "deviceinfo";
		string item84 = "elabel";
		string item85 = "nvram";
		UPDATE = new List<string>
		{
			item, item2, item3, item4, item5, item6, item7, item8, item9, item10,
			item11, item12, item13, item14, item15, item16, item17, item18, item19, item20,
			item21, item22, item23, item24, item25, item26, item27, item28, item29, item30,
			item31, item32, item33, item34
		};
		part_RESERVED = new List<string> { item35, item36, item37 };
		part_PROTECTED = new List<string>
		{
			item38, item39, item40, item41, item42, item43, item44, item45, item46, item47,
			item48, item49, item50, item51, item52, item53, item54, item55
		};
		part_INVISIBLE = new List<string>
		{
			item56, item57, item58, item59, item60, item61, item62, item63, item64, item65,
			item66, item67, item68, item69, item70, item71, item72, item73, item74, item75,
			item76, item77, item78, item79, item80, item81, item82, item83, item84
		};
		part_BINREGION = new List<string> { item85 };

	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
