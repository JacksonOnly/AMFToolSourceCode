using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace AMFTool.ThreadWorker;

public class DeviceManager
{
	public static Dictionary<string, List<Device>> RawData;

	public static List<FullDevice> Devices = new List<FullDevice>();

	private static void Parse(string json)
	{
		Device current2 = default(Device);
		while (true)
		{
			int num = 2045668045;
			while (true)
			{
				nint num2 = num ^ 0xB9AC897;
				nuint num3 = (nuint)num2;
				uint num5;
				int num4 = (int)((num5 = (uint)((int)num3 ^ 0xB9AC897 ^ 0x171E00DB ^ 0x171E00DB)) % 7);
				if (num4 == 0)
				{
					break;
				}
				if (num4 != 1 && num4 != 1)
				{
					if (num4 == 2 || num4 == 2)
					{
						RawData = JsonConvert.DeserializeObject<Dictionary<string, List<Device>>>(json);
						int num6 = (int)num5 ^ -1695429770;
						num = (num6 + 1003144049 - 1003144049) ^ -1695429770 ^ 0x3BE60558;
						continue;
					}
					if (num4 == 3 || num4 == 3)
					{
						if ((object)"" == "")
						{
							Devices.Clear();
						}
						num = (int)(((num5 << 2) | (num5 >> 30)) ^ 0x72026C24);
						continue;
					}
					if (num4 == 4 || num4 == 4)
					{
						int num7 = (int)num5 ^ -736697014;
						num = (num7 + 602002866 - 602002866) ^ -736697014 ^ 0x4D03246;
						continue;
					}
					if (num4 == 5 || num4 == 5)
					{
						if ((object)"" != "")
						{
						}
						num = (int)(0x7F57265 ^ num5);
						continue;
					}
					if (num4 == 6)
					{
						Dictionary<string, List<Device>>.Enumerator enumerator = RawData.GetEnumerator();
						try
						{
							while (true)
							{
								if (enumerator.MoveNext())
								{
									KeyValuePair<string, List<Device>> current;
									while (true)
									{
										IL_023e:
										current = enumerator.Current;
										while (true)
										{
											int num8 = 1721624712;
											int num11;
											while (true)
											{
												nint num9 = num8 ^ 0x7C8319B6;
												nuint num10 = (nuint)num9;
												num11 = (int)((num5 = (uint)((int)num10 ^ 0x7C8319B6 ^ 0x5E5850B8 ^ 0x5E5850B8)) % 5);
												if (num11 == 0)
												{
													num8 = 1075498116;
													continue;
												}
												if (num11 == 1)
												{
													break;
												}
												if (num11 != 2 && num11 != 2)
												{
													if (num11 != 3 && num11 != 3)
													{
														goto IL_021c;
													}
													if ((object)"" != "")
													{
													}
													int num12 = (int)num5 ^ -838192866;
													num8 = (num12 + -2127275021 - -2127275021) ^ -838192866 ^ 0x7E555BED;
													continue;
												}
												num8 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x6758870A);
											}
											goto IL_0221;
											IL_0221:
											if ("".Length != 0 || (object)"" != "")
											{
												continue;
											}
											goto IL_023e;
											IL_021c:
											if (num11 == 4)
											{
												break;
											}
											goto IL_0221;
										}
										break;
									}
									List<Device>.Enumerator enumerator2 = current.Value.GetEnumerator();
									try
									{
										while (true)
										{
											int num13;
											if (!enumerator2.MoveNext())
											{
												num13 = 2022436158;
												goto IL_0395;
											}
											goto IL_03eb;
											IL_03d6:
											if ((object)"" != "")
											{
												goto IL_03e4;
											}
											goto IL_03eb;
											IL_03eb:
											current2 = enumerator2.Current;
											goto IL_03e4;
											IL_03e4:
											num13 = 1478270482;
											goto IL_0395;
											IL_0395:
											int num16;
											while (true)
											{
												nint num14 = num13 ^ -1879593558;
												nuint num15 = (nuint)num14;
												num16 = (int)((num5 = (uint)((int)num15 ^ -1879593558 ^ 0x43A2D241 ^ 0x43A2D241)) % 8);
												if (num16 == 0)
												{
													num13 = 1000816465;
													continue;
												}
												if (num16 == 1)
												{
													break;
												}
												if (num16 != 2 && num16 != 2)
												{
													if (num16 != 3 && num16 != 3)
													{
														if (num16 != 4 && num16 != 4)
														{
															if (num16 != 5 && num16 != 5)
															{
																goto IL_03cc;
															}
															if ("".Length != 0)
															{
															}
															int num17 = (int)num5 ^ -1175653212;
															num13 = (num17 + 994465403 - 994465403) ^ -1175653212 ^ 0x4CBCBA2A;
															continue;
														}
														if ("".Length != 0)
														{
														}
														int num18 = (int)num5 ^ -1138711830;
														num13 = (num18 + -597564707 - -597564707) ^ -1138711830 ^ 0x52B920F9;
														continue;
													}
													Devices.Add(new FullDevice
													{
														Brand = current.Key,
														Model = current2.Model,
														SOC = current2.SOC,
														Platform = current2.Platform,
														Operation = current2.Operation
													});
													num13 = (int)(((num5 << 4) | (num5 >> 28)) ^ 0x1C09D19A);
													continue;
												}
												if ((object)"" != "")
												{
												}
												int num19 = (int)(num5 ^ 0x1878E151);
												num13 = (num19 + -246668464 - -246668464) ^ 0x1878E151 ^ 0x3E41F321;
											}
											goto IL_03d6;
											IL_03cc:
											if (num16 == 7)
											{
												continue;
											}
											if (num16 == 6)
											{
												break;
											}
											goto IL_03d6;
										}
									}
									finally
									{
										((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
										while (true)
										{
											IL_047f:
											int num20 = 1593986062;
											while (true)
											{
												nint num21 = num20 ^ -1275234241;
												nuint num22 = (nuint)num21;
												int num23 = (int)((num5 = (uint)((int)num22 ^ -1275234241 ^ 0x1630190A ^ 0x1630190A)) % 3);
												switch (num23)
												{
												default:
													if (num23 == 1 || num23 != 2)
													{
														goto IL_042a;
													}
													goto end_IL_044e;
												case 1:
													goto IL_042a;
												case 0:
													break;
												}
												goto IL_047f;
												IL_042a:
												int num24 = (int)num5 ^ -2112470119;
												num20 = (num24 + 294463928 - 294463928) ^ -2112470119 ^ 0x104A52F4;
												continue;
												end_IL_044e:
												break;
											}
											break;
										}
									}
									goto IL_04cf;
								}
								goto IL_0499;
								IL_0491:
								if ("".Length == 0)
								{
									continue;
								}
								goto IL_0499;
								IL_0499:
								int num25 = 899795546;
								goto IL_049e;
								IL_04cf:
								num25 = 602549776;
								goto IL_049e;
								IL_049e:
								nint num26 = num25 ^ -67424629;
								nuint num27 = (nuint)num26;
								switch ((int)((num5 = (uint)((int)num27 ^ -67424629 ^ -2045018850 ^ -2045018850)) % 3))
								{
								default:
									goto IL_0491;
								case 0:
									break;
								case 2:
									return;
								}
								goto IL_04cf;
							}
						}
						finally
						{
							((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
							while (true)
							{
								IL_053d:
								int num28 = 347343214;
								while (true)
								{
									nint num29 = num28 ^ 0x311B7757;
									nuint num30 = (nuint)num29;
									int num31 = (int)((num5 = (uint)((int)num30 ^ 0x311B7757 ^ -12676951 ^ -12676951)) % 3);
									switch (num31)
									{
									default:
										if (num31 == 1 || num31 != 2)
										{
											goto IL_0504;
										}
										goto end_IL_050c;
									case 1:
										goto IL_0504;
									case 0:
										break;
									}
									goto IL_053d;
									IL_0504:
									num28 = (int)(0x59949F2E ^ num5);
									continue;
									end_IL_050c:
									break;
								}
								break;
							}
						}
					}
				}
				if ((object)"" != "" || (object)"" != "")
				{
				}
				int num32 = (int)(num5 ^ 0x13CFED03);
				num = (num32 + -1640471017 - -1640471017) ^ 0x13CFED03 ^ 0x4B5321E1;
			}
		}
	}

	public static List<FullDevice> GetAll()
	{
		return Devices;
	}

	public static List<FullDevice> FilterByBrand(string brand)
	{
		return Devices.Where((FullDevice x) => x.Brand.ToLower() == brand.ToLower()).ToList();
	}

	public static List<FullDevice> Search(string keyword)
	{
		return Devices.Where((FullDevice x) => x.Model.ToLower().Contains(keyword.ToLower())).ToList();
	}

	public static List<string> GetBrands()
	{
		return Devices.Select((FullDevice x) => x.Brand).Distinct().ToList();
	}

	public static void GetDeviceListUnlock()
	{
		string text = "/models.json";
		string text2 = " - ";
		string json = new WebClient().DownloadString(Server.DataCenter + text);
		Parse(json);
		foreach (FullDevice item in GetAll())
		{
			Console.WriteLine(item.Brand + text2 + item.Model);
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
