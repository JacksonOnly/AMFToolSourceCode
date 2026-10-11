using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using AMFTool.Library.Mediatek.Auth;
using AMFTool.Library.Mediatek.Config;
using AMFTool.Library.Mediatek.Download_Agent.xflash.extention;
using AMFTool.Library.Mediatek.Exploit;
using AMFTool.Library.Usb;
using AMFTool.Properties;
using SharpMTKPlExploit.Class;

namespace AMFTool.Library.Mediatek.Download_Agent.xflash;

public class DAXFlash
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct DaArg
	{
		public ulong addr;

		public ulong len;
	}

	public MtkClass Mtk;

	public DAConfig Dacfg;

	public MtkConfig Config;

	public XFlashExt XFlashExt;

	public Partition Partition;

	public Kamakiripl KamakiriPl;

	public EmmcInfo Emmc;

	public UfsInfo Ufs;

	public NorInfo Nor;

	public NandInfo Nand;

	public ChipId ChipId;

	public RamInfo Raminfo;

	public byte[] Randomid;

	public byte[] DaVersion;

	public PacketLen PacketLen;

	public DAXFlash(MtkClass mtk, DAConfig dacfg)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Mtk = mtk;
		Dacfg = dacfg;
		Config = mtk.Config;
		XFlashExt = new XFlashExt(mtk, this);
		Partition = new Partition(mtk, ReadGPT, ReadPMT);
		KamakiriPl = new Kamakiripl(mtk);
	}

	private Tuple<int, int, long> GetStorage(string partType, long length)
	{
		string text = "nor";
		string text2 = "nand";
		string text3 = "ufs";
		string text4 = "sdc";
		string text5 = "lu3";
		string text6 = Dacfg.FlashType.ToLower();
		int num = ((text6 == text) ? 32 : ((text6 == text2) ? 16 : ((text6 == text3) ? 48 : ((!(text6 == text4)) ? 1 : 2))));
		int num2 = num;
		if (num2 == 48 && int.TryParse(partType, out var result) && result == 8)
		{
			partType = text5;
		}
		return PartitionTypeAndSize(num2, partType, length);
	}

	private Tuple<int, int, long> PartitionTypeAndSize(int storage = 0, string partType = null, long length = 0L)
	{
		string text = "user";
		string text2 = "boot1";
		string text3 = "emmc";
		string text4 = "boot2";
		string text5 = "gp1";
		string text6 = "gp2";
		string text7 = "gp3";
		string text8 = "gp4";
		string text9 = "rpmb";
		object result = null;
		string text10 = "lu3";
		string text11 = "lu1";
		string text12 = "lu2";
		string text13 = "lu4";
		int item = 0;
		if (storage != 1 && storage != 2)
		{
			if (storage != 48)
			{
				if (!new int[6] { 16, 18, 17, 19, 21, 20 }.Contains(storage))
				{
					if (new int[3] { 32, 34, 33 }.Contains(storage))
					{
						item = 8;
						length = Math.Min(length, Nor.AvailableSize);
					}
				}
				else
				{
					item = 8;
					length = Math.Min(length, Nand.TotalSize);
				}
			}
			else if (partType != null && !(partType == text10) && !(partType == text))
			{
				if (!(partType == text11) && !(partType == text2))
				{
					if (!(partType == text12) && !(partType == text4))
					{
						if (!(partType == text13) && !(partType == text9))
						{
							return (Tuple<int, int, long>)result;
						}
						item = 4;
						length = Math.Min(length, Ufs.Lu2Size);
					}
					else
					{
						item = 2;
						length = Math.Min(length, Ufs.Lu2Size);
					}
				}
				else
				{
					item = 1;
					length = Math.Min(length, Ufs.Lu1Size);
				}
			}
			else
			{
				item = 3;
				length = Math.Min(length, Ufs.Lu0Size);
			}
		}
		else if (partType != null && !(partType == text))
		{
			if (!(partType == text2))
			{
				if (partType == text4)
				{
					item = 2;
					if (Dacfg.FlashType == text3)
					{
						length = Math.Min(length, Emmc.Boot2Size);
					}
				}
				else if (!(partType == text5))
				{
					if (partType == text6)
					{
						item = 5;
						if (Dacfg.FlashType == text3)
						{
							length = Math.Min(length, Emmc.Gp2Size);
						}
					}
					else if (!(partType == text7))
					{
						if (!(partType == text8))
						{
							if (!(partType == text9))
							{
								return (Tuple<int, int, long>)result;
							}
							item = 3;
							if (Dacfg.FlashType == text3)
							{
								length = Math.Min(length, Emmc.RpmbSize);
							}
						}
						else
						{
							item = 7;
							if (Dacfg.FlashType == text3)
							{
								length = Math.Min(length, Emmc.Gp4Size);
							}
						}
					}
					else
					{
						item = 6;
						if (Dacfg.FlashType == text3)
						{
							length = Math.Min(length, Emmc.Gp3Size);
						}
					}
				}
				else
				{
					item = 4;
					if (Dacfg.FlashType == text3)
					{
						length = Math.Min(length, Emmc.Gp1Size);
					}
				}
			}
			else
			{
				item = 1;
				if (Dacfg.FlashType == text3)
				{
					length = Math.Min(length, Emmc.Boot1Size);
				}
			}
		}
		else
		{
			item = 8;
		}
		return new Tuple<int, int, long>(storage, item, length);
	}

	public (byte[], List<Partf>) ReadPMT(bool arg_01 = false, string arg_02 = "user")
	{
		return Partition.GetPmt(arg_01, arg_02);
	}

	private byte[] ReadGPT(long addr, long length, string partType, bool display = false)
	{
		object obj = null;
		Tuple<int, int, long> storage = GetStorage(partType, length);
		if (storage == obj)
		{
			return Array.Empty<byte>();
		}
		int item = storage.Item1;
		int item2 = storage.Item2;
		length = storage.Item3;
		PacketLen = GetPacketLength();
		if (PacketLen == obj)
		{
			return Array.Empty<byte>();
		}
		if (!CmdReadData(addr, length, item, item2))
		{
			return Array.Empty<byte>();
		}
		int num = 0;
		long num2 = length;
		byte[] array = new byte[(int)length];
		bool flag;
		if (num2 <= 0L)
		{
			if (flag = array.Length == length)
			{
				return array;
			}
			return Array.Empty<byte>();
		}
		byte[] array2 = XRead();
		array2.AsSpan().CopyTo(array.AsSpan(0));
		num2 -= array2.Length;
		num = 0 + array2.Length;
		int num3 = 625205444;
		while (true)
		{
			_ = num3 ^ 0x6E114DFE;
			if (Ack() != 0)
			{
				flag = array.Length == length;
				break;
			}
			if (num2 <= 0L)
			{
				flag = array.Length == length;
				break;
			}
			array2 = XRead();
			array2.AsSpan().CopyTo(array.AsSpan(num));
			num2 -= array2.Length;
			num += array2.Length;
			num3 = 625205444;
		}
		if (flag)
		{
			return array;
		}
		return Array.Empty<byte>();
	}

	public bool FormatFlash(long addr, long length, string partType = null)
	{
		object obj = null;
		string text = "Error on format: ";
		string format = "Successsfully formatted Addr 0x{0:X} with length 0x{1:X}.";
		Tuple<int, int, long> storage = GetStorage(partType, length);
		if (storage != obj)
		{
			int item = storage.Item1;
			int item2 = storage.Item2;
			length = storage.Item3;
			if (XSend(65539u))
			{
				int status = GetStatus();
				if (status == 0)
				{
					NandExtension nandExtension = new NandExtension();
					byte[] array = new uint[2]
					{
						(uint)item,
						Convert.ToUInt32(item2)
					}.Pack();
					byte[] array2 = new ulong[2]
					{
						(ulong)addr,
						(ulong)length
					}.Pack();
					byte[] array3 = new uint[8]
					{
						(uint)nandExtension.CellUsage,
						(uint)nandExtension.AddrType,
						(uint)nandExtension.BinType,
						(uint)nandExtension.OperationType,
						(uint)nandExtension.SysSlcPercent,
						(uint)nandExtension.UsrSlcPercent,
						(uint)nandExtension.PhyMaxSize,
						0u
					}.Pack();
					byte[] array4 = new byte[array.Length + array2.Length + array3.Length];
					int num = 0;
					array.AsSpan().CopyTo(array4.AsSpan(0, array.Length));
					num = 0 + array.Length;
					array2.AsSpan().CopyTo(array4.AsSpan(num, array2.Length));
					num += array2.Length;
					array3.AsSpan().CopyTo(array4.AsSpan(num, array3.Length));
					if (!SendParam(array4))
					{
						return false;
					}
					switch (GetStatus())
					{
					case 1074003973:
						Console.WriteLine(string.Format(format, addr, length));
						return true;
					case 1074003972:
					{
						int status2 = GetStatus();
						Thread.Sleep(status2);
						status = Ack();
						int num2 = 885120198;
						while (true)
						{
							_ = num2 ^ -2047080013;
							switch (status)
							{
							case 1074003973:
								Console.WriteLine(string.Format(format, addr, length));
								return true;
							case 1074003972:
								break;
							default:
								return false;
							}
							status2 = GetStatus();
							Thread.Sleep(status2);
							status = Ack();
							num2 = 885120198;
						}
					}
					default:
						return false;
					}
				}
				Console.WriteLine(text + Error.status((uint)status));
				return false;
			}
			return false;
		}
		return false;
	}

	public bool WriteFlash(long addr, long length, string fileName, object filePath, string partType = null)
	{
		object obj = null;
		int result = 0;
		double field_ = Class38.field_543;
		Stream stream = (Stream)((!(filePath is string path)) ? ((!(filePath is byte[] buffer)) ? obj : new MemoryStream(buffer)) : new FileStream(path, FileMode.Open, FileAccess.Read));
		Stream stream2 = stream;
		if (stream2 == obj)
		{
			return false;
		}
		string text = filePath as string;
		if (text ==null)
		{
			byte[] array = filePath as byte[];
			if (array !=null)
			{
				length = array.Length;
			}
		}
		else
		{
			length = new FileInfo(text).Length;
		}
		stream2.Seek(0L, SeekOrigin.Begin);
		Tuple<int, int, long> storage = GetStorage(partType, length);
		if (storage != obj)
		{
			_ = storage.Item1;
			_ = storage.Item2;
			length = storage.Item3;
			PacketLen = GetPacketLength();
			if (PacketLen == obj)
			{
				return false;
			}
			if (CmdWriteData(length, fileName))
			{
				long num = length;
				long writePacketLength = PacketLen.WritePacketLength;
				Stopwatch stopwatch = Stopwatch.StartNew();
				using (stream2)
				{
					long num2 = 0L;
					while (num > num2)
					{
						int num3 = (int)Math.Min(writePacketLength, num - num2);
						byte[] array2 = new byte[num3];
						int num4 = stream2.Read(array2, 0, num3);
						if (num4 == num3)
						{
							ushort num5 = 0;
							byte[] array3 = array2;
							foreach (byte b in array3)
							{
								num5 += b;
							}
							byte[] bytes = BitConverter.GetBytes(0);
							if (SerialMTK.Write(CmdPktParam(bytes.Length)))
							{
								if (!SerialMTK.Write(bytes))
								{
									return false;
								}
								bytes = BitConverter.GetBytes(num5);
								if (!SerialMTK.Write(CmdPktParam(bytes.Length)))
								{
									return false;
								}
								if (SerialMTK.Write(bytes))
								{
									if (SerialMTK.Write(CmdPktParam(num3)))
									{
										if (SerialMTK.Write(array2))
										{
											num2 += num3;
											UI.Progresbar1(num2, length);
											double totalSeconds = stopwatch.Elapsed.TotalSeconds;
											if (totalSeconds > field_)
											{
												UI.LabelWrite(utils.FileSize((long)((double)num2 / totalSeconds)), utils.FileSize(num2), utils.FileSize(length));
											}
											uint status = (uint)GetStatus();
											if (status == 0 || status == 3221291009u)
											{
												continue;
											}
											return false;
										}
										return false;
									}
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					stopwatch.Stop();
					uint status2 = (uint)GetStatus();
					if (status2 == 0 || status2 == 3221291009u)
					{
						SendDevctrl(Cmd.CC_OPTIONAL_DOWNLOAD_ACT, (byte[])obj);
						UI.Progresbar1(length, length);
						return true;
					}
				}
				return (byte)result != 0;
			}
			return false;
		}
		return false;
	}

	public bool ReadFlash(long addr, long length, string fileName = "", string destination = "", string partType = null)
	{
		object obj = null;
		double field_ = Class38.field_543;
		Tuple<int, int, long> storage = GetStorage(partType, length);
		if (storage != obj)
		{
			int item = storage.Item1;
			int item2 = storage.Item2;
			length = storage.Item3;
			if (!CmdReadData(addr, length, item, item2))
			{
				return false;
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			using FileStream fileStream = new FileStream(destination, FileMode.Create, FileAccess.Write);
			long num = 0L;
			long num2 = length;
			while (num2 > 0L)
			{
				byte[] array = SerialMTK.Read(12);
				if (array == null || array.Length != 12)
				{
					break;
				}
				uint[] array2 = array.Unpack32();
				uint num3 = array2[0];
				uint num4 = array2[1];
				int num5 = (int)array2[2];
				uint num6 = num4;
				uint num7 = num3;
				if (num7 != 4277071599u || num6 != 1)
				{
					break;
				}
				byte[] array3 = SerialMTK.Read(num5);
				if (array3 == null || array3.Length != num5)
				{
					break;
				}
				if (num5 <= 4)
				{
					if (num5 != 4 || Binary.ReadU32LE(array3) == 0)
					{
						continue;
					}
					break;
				}
				fileStream.Write(array3, 0, array3.Length);
				SerialMTK.Write(new uint[3] { 4277071599u, 1u, 4u }.Pack());
				SerialMTK.Write(new uint[1].Pack());
				num += array3.Length;
				num2 -= array3.Length;
				UI.Progresbar1(fileStream.Length, length);
				double totalSeconds = stopwatch.Elapsed.TotalSeconds;
				if (totalSeconds > field_)
				{
					UI.LabelWrite(utils.FileSize((long)((double)fileStream.Length / totalSeconds)), utils.FileSize(fileStream.Length), utils.FileSize(length));
				}
			}
			stopwatch.Stop();
			byte[] array4 = XRead();
			return Binary.ReadU32LE(array4) == 0;
		}
		return false;
	}

	public byte[] ReadFlashByte(long Addr, long length, string partType = null)
	{
		object obj = null;
		double field_ = Class38.field_543;
		Tuple<int, int, long> storage = GetStorage(partType, length);
		if (storage == obj)
		{
			return Array.Empty<byte>();
		}
		int item = storage.Item1;
		int item2 = storage.Item2;
		length = storage.Item3;
		if (CmdReadData(Addr, length, item, item2))
		{
			int num = 0;
			long num2 = length;
			byte[] array = new byte[(int)length];
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (num2 > 0L)
			{
				byte[] array2 = SerialMTK.Read(12);
				if (array2 == null || array2.Length != 12)
				{
					break;
				}
				uint[] array3 = array2.Unpack32();
				uint num3 = array3[0];
				uint num4 = array3[1];
				int num5 = (int)array3[2];
				uint num6 = num4;
				uint num7 = num3;
				if (num7 != 4277071599u || num6 != 1)
				{
					break;
				}
				byte[] array4 = SerialMTK.Read(num5);
				if (array4 == null || array4.Length != num5)
				{
					break;
				}
				if (num5 > 4)
				{
					array4.AsSpan().CopyTo(array.AsSpan(num));
					SerialMTK.Write(new uint[3] { 4277071599u, 1u, 4u }.Pack());
					SerialMTK.Write(new uint[1].Pack());
					num += array4.Length;
					num2 -= array4.Length;
					UI.Progresbar1(num, length);
					double totalSeconds = stopwatch.Elapsed.TotalSeconds;
					if (totalSeconds > field_)
					{
						UI.LabelWrite(utils.FileSize((long)((double)num / totalSeconds)), utils.FileSize(num), utils.FileSize(length));
					}
				}
				else if (num5 == 4 && Binary.ReadU32LE(array4) != 0)
				{
					break;
				}
			}
			stopwatch.Stop();
			byte[] array5 = XRead();
			return (Binary.ReadU32LE(array5) != 0) ? Array.Empty<byte>() : array;
		}
		return Array.Empty<byte>();
	}

	private (RamInfo sram, RamInfo dram) GetRamInfo(bool display = true)
	{
		object obj = null;
		string text = "Error on getting ram info: ";
		string msg = "";
		string msg2 = "[Memory Information]";
		string msg3 = "SRAM Size : ";
		string msg4 = "DRAM Size : ";
		byte[] item = SendDevctrl((Cmd)262156u, (byte[])obj).data;
		if (item == obj)
		{
			return (sram: (RamInfo)obj, dram: (RamInfo)obj);
		}
		int status = GetStatus();
		if (status == 0)
		{
			RamInfo ramInfo = new RamInfo();
			RamInfo ramInfo2 = new RamInfo();
			if (item.Length != 24)
			{
				if (item.Length != 48)
				{
					return (sram: (RamInfo)obj, dram: (RamInfo)obj);
				}
				ulong[] array = item.Unpack64();
				RamInfo ramInfo3 = ramInfo;
				RamInfo ramInfo4 = ramInfo;
				RamInfo ramInfo5 = ramInfo2;
				RamInfo ramInfo6 = ramInfo2;
				RamInfo ramInfo7 = ramInfo2;
				ulong type = array[0];
				ulong baseAddress = array[1];
				ulong size = array[2];
				ulong type2 = array[3];
				ulong baseAddress2 = array[4];
				ulong size2 = array[5];
				ramInfo.Type = type;
				ramInfo3.BaseAddress = baseAddress;
				ramInfo4.Size = size;
				ramInfo5.Type = type2;
				ramInfo6.BaseAddress = baseAddress2;
				ramInfo7.Size = size2;
			}
			else
			{
				uint[] array2 = item.Unpack32();
				RamInfo ramInfo7 = ramInfo;
				RamInfo ramInfo6 = ramInfo;
				RamInfo ramInfo5 = ramInfo2;
				RamInfo ramInfo4 = ramInfo2;
				RamInfo ramInfo3 = ramInfo2;
				ulong size2 = array2[0];
				ulong baseAddress2 = array2[1];
				ulong type2 = array2[2];
				ulong size = array2[3];
				ulong baseAddress = array2[4];
				ulong type = array2[5];
				ramInfo.Type = size2;
				ramInfo7.BaseAddress = baseAddress2;
				ramInfo6.Size = type2;
				ramInfo5.Type = size;
				ramInfo4.BaseAddress = baseAddress;
				ramInfo3.Size = type;
			}
			if (ramInfo.Type != 0L && ramInfo2.Type > 0L && display)
			{
				UI.RichLogs(msg, UI.log, nextline: true);
				UI.RichLogs(msg2, UI.log, nextline: true);
				UI.RichLogs(msg3, UI.log);
				UI.RichLogs(ramInfo.SizeFormat, UI.info, nextline: true, bold: true);
				UI.RichLogs(msg4, UI.log);
				UI.RichLogs(ramInfo2.SizeFormat, UI.info, nextline: true, bold: true);
			}
			Dacfg.Storage.Sram = ramInfo;
			Dacfg.Storage.Dram = ramInfo2;
			return (sram: ramInfo, dram: ramInfo2);
		}
		Console.WriteLine(text + Error.status((uint)status));
		return (sram: (RamInfo)obj, dram: (RamInfo)obj);
	}

	private EmmcInfo GetEmmcInfo(bool display = true)
	{
		object obj = null;
		string text = "Error on getting emmc info: ";
		string format = "EMMC FWVer        : 0x{0:X}";
		string text2 = "EMMC ID           : ";
		string text3 = "EMMC CID          : ";
		string format2 = "EMMC BOOT1 Size   : 0x{0:X}";
		string format3 = "EMMC BOOT2 Size   : 0x{0:X}";
		string format4 = "EMMC GP1 Size     : 0x{0:X}";
		string format5 = "EMMC GP2 Size     : 0x{0:X}";
		string format6 = "EMMC GP3 Size     : 0x{0:X}";
		string format7 = "EMMC GP4 Size     : 0x{0:X}";
		string format8 = "EMMC RPMB Size    : 0x{0:X}";
		string format9 = "EMMC USER Size    : 0x{0:X}";
		string msg = "";
		string msg2 = "[EMMC Information]";
		string msg3 = "EMMC FWVer   : ";
		string format10 = "0x{0:x}";
		string msg4 = "EMMC ID      : ";
		string msg5 = "EMMC CID     : ";
		string msg6 = "EMMC VENDOR  : ";
		string text4 = "0x";
		string msg7 = "MRF ID       : ";
		string oldValue = "0";
		string newValue = "0.";
		string msg8 = "REVISION     : ";
		string msg9 = "BOOT1 Size   : ";
		string format11 = "0x0000000000000000{0:x} {1}";
		string format12 = "{0:x}";
		string msg10 = "BOOT2 Size   : ";
		string msg11 = "RPMB  Size   : ";
		string msg12 = "USERAREA     : ";
		string msg13 = "Pre-EOL Info : ";
		string msg14 = "Lifetime     : ";
		string msg15 = "SLC ";
		string msg16 = " | ";
		string msg17 = "(MLC) ";
		byte[] item = SendDevctrl(Cmd.GET_EMMC_INFO, (byte[])obj).data;
		if (item != null && item.Length != 0)
		{
			int status = GetStatus();
			if (status == 0)
			{
				EmmcInfo emmcInfo = new EmmcInfo();
				uint[] array = item.Skip(0).Take(8).ToArray()
					.Unpack32();
				EmmcInfo emmcInfo2 = emmcInfo;
				long type = array[0];
				long blockSize = array[1];
				emmcInfo.Type = type;
				emmcInfo2.BlockSize = blockSize;
				ulong[] array2 = item.Skip(8).Take(64).ToArray()
					.Unpack64();
				emmcInfo2 = emmcInfo;
				EmmcInfo emmcInfo3 = emmcInfo;
				EmmcInfo emmcInfo4 = emmcInfo;
				EmmcInfo emmcInfo5 = emmcInfo;
				EmmcInfo emmcInfo6 = emmcInfo;
				EmmcInfo emmcInfo7 = emmcInfo;
				EmmcInfo emmcInfo8 = emmcInfo;
				blockSize = (long)array2[0];
				type = (long)array2[1];
				long rpmbSize = (long)array2[2];
				long gp1Size = (long)array2[3];
				long gp2Size = (long)array2[4];
				long gp3Size = (long)array2[5];
				long gp4Size = (long)array2[6];
				long userSize = (long)array2[7];
				emmcInfo.Boot1Size = blockSize;
				emmcInfo2.Boot2Size = type;
				emmcInfo3.RpmbSize = rpmbSize;
				emmcInfo4.Gp1Size = gp1Size;
				emmcInfo5.Gp2Size = gp2Size;
				emmcInfo6.Gp3Size = gp3Size;
				emmcInfo7.Gp4Size = gp4Size;
				emmcInfo8.UserSize = userSize;
				emmcInfo.Cid = new byte[16];
				item.AsSpan(72, 16).CopyTo(emmcInfo.Cid.AsSpan(0, 16));
				emmcInfo.FwVer = (long)item.Skip(88).Take(8).ToArray()
					.Unpack64()[0];
				emmcInfo.Vid = new byte[1];
				emmcInfo.Rev = new byte[1];
				emmcInfo.LfTime = new byte[1];
				emmcInfo.Slc = new byte[1];
				emmcInfo.Mlc = new byte[1];
				ref byte reference = ref emmcInfo.Vid[0];
				ref byte reference2 = ref emmcInfo.Rev[0];
				ref byte reference3 = ref emmcInfo.LfTime[0];
				ref byte reference4 = ref emmcInfo.Slc[0];
				ref byte reference5 = ref emmcInfo.Mlc[0];
				byte b = item[72];
				byte b2 = item[73];
				byte b3 = item[96];
				byte b4 = item[97];
				byte b5 = item[99];
				reference = b;
				reference2 = b2;
				reference3 = b3;
				reference4 = b4;
				reference5 = b5;
				if (!((ulong)emmcInfo.Type > 0uL && display))
				{
					Mtk.Config.PageSize = 512;
					Mtk.DaLoader.Dacfg.PageSize = 512;
					Mtk.DaLoader.Dacfg.FlashSize = emmcInfo.UserSize;
					return emmcInfo;
				}
				Console.WriteLine(string.Format(format, emmcInfo.FwVer));
				Console.WriteLine(text2 + Encoding.UTF8.GetString(emmcInfo.Cid, 3, 6));
				Console.WriteLine(text3 + emmcInfo.Cid.ToHexString());
				Console.WriteLine(string.Format(format2, emmcInfo.Boot1Size));
				Console.WriteLine(string.Format(format3, emmcInfo.Boot2Size));
				Console.WriteLine(string.Format(format4, emmcInfo.Gp1Size));
				Console.WriteLine(string.Format(format5, emmcInfo.Gp2Size));
				Console.WriteLine(string.Format(format6, emmcInfo.Gp3Size));
				Console.WriteLine(string.Format(format7, emmcInfo.Gp4Size));
				Console.WriteLine(string.Format(format8, emmcInfo.RpmbSize));
				Console.WriteLine(string.Format(format9, emmcInfo.UserSize));
				UI.RichLogs(msg, UI.log, nextline: true);
				UI.RichLogs(msg2, UI.log, nextline: true);
				UI.RichLogs(msg3, UI.log);
				UI.RichLogs(string.Format(format10, emmcInfo.FwVer), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg4, UI.log);
				UI.RichLogs(Encoding.UTF8.GetString(emmcInfo.Cid, 3, 6).ToUpper(), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg5, UI.log);
				UI.RichLogs(emmcInfo.Cid.ToHexString().ToUpper(), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg6, UI.log);
				UI.RichLogs(utils.StorageId(text4 + emmcInfo.Vid.ToHexString()).ToUpper(), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg7, UI.log);
				UI.RichLogs(emmcInfo.Vid.ToHexString().Replace(oldValue, newValue), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg8, UI.log);
				UI.RichLogs(emmcInfo.Rev.ToHexString(), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg9, UI.log);
				UI.RichLogs(string.Format(format11, emmcInfo.Boot1Size, utils.FileSize(string.Format(format12, emmcInfo.Boot1Size))), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg10, UI.log);
				UI.RichLogs(string.Format(format11, emmcInfo.Boot2Size, utils.FileSize(string.Format(format12, emmcInfo.Boot2Size))), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg11, UI.log);
				UI.RichLogs(string.Format(format11, emmcInfo.RpmbSize, utils.FileSize(string.Format(format12, emmcInfo.RpmbSize))), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg12, UI.log);
				UI.RichLogs(string.Format(format11, emmcInfo.UserSize, utils.FileSize(string.Format(format12, emmcInfo.UserSize))), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg13, UI.log);
				UI.RichLogs(utils.OldStorage(emmcInfo.LfTime.ToHexString()), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg14, UI.log);
				UI.RichLogs(msg15, UI.info);
				UI.RichLogs(utils.StorageSCL(emmcInfo.Slc.ToHexString()), UI.info, nextline: false, bold: true);
				UI.RichLogs(msg16, UI.info, nextline: false, bold: true);
				UI.RichLogs(msg17, UI.info, nextline: false, bold: true);
				UI.RichLogs(utils.StorageSCL(emmcInfo.Mlc.ToHexString()), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg, UI.log, nextline: true);
				Mtk.Config.PageSize = 512;
				Mtk.DaLoader.Dacfg.PageSize = 512;
				Mtk.DaLoader.Dacfg.FlashSize = emmcInfo.UserSize;
				return emmcInfo;
			}
			Console.WriteLine(text + Error.status((uint)status));
			return (EmmcInfo)obj;
		}
		return (EmmcInfo)obj;
	}

	private NandInfo GetNandInfo(bool display = true)
	{
		object obj = null;
		string text = "Error on getting nand info: ";
		string format = "NAND Pagesize:   0x{0:X}";
		string format2 = "NAND Blocksize:  0x{0:X}";
		string format3 = "NAND Sparesize:  0x{0:X}";
		string format4 = "NAND Total size: 0x{0:X}";
		string format5 = "NAND Avail:      0x{0:X}";
		string text2 = "NAND ID:         ";
		string oldValue = "-";
		string newValue = "";
		byte[] item = SendDevctrl((Cmd)262146u, (byte[])obj).data;
		if (item == null || item.Length == 0)
		{
			return (NandInfo)obj;
		}
		int status = GetStatus();
		if (status == 0)
		{
			NandInfo nandInfo = new NandInfo();
			uint[] array = item.Skip(0).Take(16).ToArray()
				.Unpack32();
			NandInfo nandInfo2 = nandInfo;
			NandInfo nandInfo3 = nandInfo;
			NandInfo nandInfo4 = nandInfo;
			int type = (int)array[0];
			int pageSize = (int)array[1];
			int blockSize = (int)array[2];
			int spareSize = (int)array[3];
			nandInfo.Type = type;
			nandInfo2.PageSize = pageSize;
			nandInfo3.BlockSize = blockSize;
			nandInfo4.SpareSize = spareSize;
			ulong[] array2 = item.Skip(16).Take(16).ToArray()
				.Unpack64();
			nandInfo4 = nandInfo;
			long totalSize = (long)array2[0];
			long availableSize = (long)array2[1];
			nandInfo.TotalSize = totalSize;
			nandInfo4.AvailableSize = availableSize;
			nandInfo.NandBmtExist = item[32];
			if (item.Length >= 45)
			{
				nandInfo.NandId = new byte[12];
				item.AsSpan(33, 12).CopyTo(nandInfo.NandId.AsSpan(0, 12));
			}
			else
			{
				nandInfo.NandId = Array.Empty<byte>();
			}
			if (nandInfo.Type != 0)
			{
				Mtk.Config.PageSize = nandInfo.PageSize;
				Mtk.DaLoader.Dacfg.PageSize = nandInfo.PageSize;
				if (display)
				{
					Console.WriteLine(string.Format(format, nandInfo.PageSize));
					Console.WriteLine(string.Format(format2, nandInfo.BlockSize));
					Console.WriteLine(string.Format(format3, nandInfo.SpareSize));
					Console.WriteLine(string.Format(format4, nandInfo.TotalSize));
					Console.WriteLine(string.Format(format5, nandInfo.AvailableSize));
					Console.WriteLine(text2 + nandInfo.NandId.ToHexString().Replace(oldValue, newValue).ToLower());
				}
			}
			return nandInfo;
		}
		Console.WriteLine(text + Error.status((uint)status));
		return (NandInfo)obj;
	}

	private NorInfo GetNorInfo(bool display = true)
	{
		object obj = null;
		string text = "Error on getting nor info: ";
		string format = "NOR Pagesize: 0x{0:X}";
		string format2 = "NOR Size:     0x{0:X}";
		byte[] item = SendDevctrl(Cmd.GET_NOR_INFO, (byte[])obj).data;
		if (item != null && item.Length != 0)
		{
			int status = GetStatus();
			if (status != 0)
			{
				Console.WriteLine(text + Error.status((uint)status));
				return (NorInfo)obj;
			}
			NorInfo norInfo = new NorInfo();
			uint[] array = item.Skip(0).Take(8).ToArray()
				.Unpack32();
			NorInfo norInfo2 = norInfo;
			int type = (int)array[0];
			int pageSize = (int)array[1];
			norInfo.Type = type;
			norInfo2.PageSize = pageSize;
			ulong[] array2 = item.Skip(8).Take(8).ToArray()
				.Unpack64();
			norInfo.AvailableSize = (long)array2[0];
			if (norInfo.Type != 0 && display)
			{
				Console.WriteLine(string.Format(format, norInfo.PageSize));
				Console.WriteLine(string.Format(format2, norInfo.AvailableSize));
				return norInfo;
			}
			return norInfo;
		}
		return (NorInfo)obj;
	}

	private UfsInfo GetUFSInfo(bool display = true)
	{
		object obj = null;
		string text = "Error on getting ufs info: ";
		string value = "Error on getting ufs info: response too short";
		string format = "UFS MID    : 0x{0:X}";
		string text2 = "UFS CID    : ";
		string text3 = "UFS FWVer  : 0x";
		string format2 = "UFS BLOCK  : 0x{0:X}";
		string text4 = "UFS ID     : ";
		string text5 = "UFS SERIAL : 0x";
		string format3 = "LUN0       : 0x{0:X}";
		string format4 = "LUN1       : 0x{0:X}";
		string format5 = "LUN2       : 0x{0:X}";
		string msg = "";
		string msg2 = "[UFS Information]";
		string msg3 = "UFS BlockSize : ";
		string format6 = "0x{0:x}";
		string msg4 = "UFS ID        : ";
		string msg5 = "UFS MID       : ";
		string msg6 = "UFS CID       : ";
		string msg7 = "UFS FWVer     : ";
		string text6 = "0x";
		string msg8 = "UFS Serial    : ";
		string msg9 = "UFS Vendor    : ";
		string msg10 = "UFS LU0 Size  : ";
		string format7 = "0x0000000000000000{0:x} {1}";
		string format8 = "{0:x}";
		string msg11 = "UFS LU1 Size  : ";
		string msg12 = "UFS LU2 Size  : ";
		string msg13 = "UFS LU3 Size  : ";
		string msg14 = "Pre-EOL Info  : ";
		string msg15 = "Lifetime      : ";
		string msg16 = "SLC ";
		string msg17 = " | ";
		string msg18 = "(MLC) ";
		byte[] item = SendDevctrl((Cmd)262148u, (byte[])obj).data;
		if (item.Length != 0)
		{
			int status = GetStatus();
			if (status == 0)
			{
				if (item.Length < 32)
				{
					Console.WriteLine(value);
					return (UfsInfo)obj;
				}
				UfsInfo ufsInfo = new UfsInfo();
				uint[] array = item.Skip(0).Take(8).ToArray()
					.Unpack32();
				UfsInfo ufsInfo2 = ufsInfo;
				int type = (int)array[0];
				long blockSize = array[1];
				ufsInfo.Type = type;
				ufsInfo2.BlockSize = blockSize;
				ulong[] array2 = item.Skip(8).Take(24).ToArray()
					.Unpack64();
				ufsInfo2 = ufsInfo;
				UfsInfo ufsInfo3 = ufsInfo;
				blockSize = (long)array2[0];
				long lu2Size = (long)array2[1];
				long lu0Size = (long)array2[2];
				ufsInfo.Lu2Size = blockSize;
				ufsInfo2.Lu2Size = lu2Size;
				ufsInfo3.Lu0Size = lu0Size;
				int num = item.Length - 32;
				byte[] array3 = ((num <= 0) ? Array.Empty<byte>() : item.Skip(32).Take(num).ToArray());
				ufsInfo.Cid = ((array3.Length >= 16) ? array3.AsSpan(0, 16).ToArray() : Array.Empty<byte>());
				ufsInfo.FwVer = ((array3.Length >= 26) ? array3.AsSpan(22, 4).ToArray() : Array.Empty<byte>());
				ufsInfo.Serial = ((array3.Length >= 42) ? array3.AsSpan(30, 12).ToArray() : Array.Empty<byte>());
				ufsInfo.LfTime = new byte[1];
				ufsInfo.Slc = new byte[1];
				ufsInfo.Mlc = new byte[1];
				if (!(ufsInfo.Type != 0 && display))
				{
					Mtk.Config.PageSize = 4096;
					Mtk.DaLoader.Dacfg.PageSize = (int)ufsInfo.BlockSize;
					Mtk.DaLoader.Dacfg.FlashSize = ufsInfo.Lu0Size;
					return ufsInfo;
				}
				Console.WriteLine(string.Format(format, ufsInfo.Cid[0]));
				Console.WriteLine(text2 + ufsInfo.Cid.ToHexString());
				Console.WriteLine(text3 + ufsInfo.FwVer.ToHexString());
				Console.WriteLine(string.Format(format2, ufsInfo.BlockSize));
				Console.WriteLine(text4 + Encoding.UTF8.GetString(ufsInfo.Cid, 2, 14));
				Console.WriteLine(text5 + ufsInfo.Serial.ToHexString());
				Console.WriteLine(string.Format(format3, ufsInfo.Lu0Size));
				Console.WriteLine(string.Format(format4, ufsInfo.Lu1Size));
				Console.WriteLine(string.Format(format5, ufsInfo.Lu2Size));
				UI.RichLogs(msg, UI.log, nextline: true);
				UI.RichLogs(msg2, UI.log, nextline: true);
				UI.RichLogs(msg3, UI.log);
				UI.RichLogs(string.Format(format6, ufsInfo.BlockSize), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg4, UI.log);
				UI.RichLogs(Encoding.UTF8.GetString(ufsInfo.Cid.Skip(2).ToArray()), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg5, UI.log);
				UI.RichLogs(string.Format(format6, ufsInfo.Cid[0]), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg6, UI.log);
				UI.RichLogs(ufsInfo.Cid.ToHexString(), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg7, UI.log);
				UI.RichLogs(text6 + ufsInfo.FwVer.ToHexString(), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg8, UI.log);
				UI.RichLogs(text6 + ufsInfo.Serial.ToHexString(), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg9, UI.log);
				UI.RichLogs(utils.StorageId(string.Format(format6, ufsInfo.Cid[0])), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg10, UI.log);
				UI.RichLogs(string.Format(format7, ufsInfo.Lu0Size, utils.FileSize(string.Format(format8, ufsInfo.Lu0Size))), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg11, UI.log);
				UI.RichLogs(string.Format(format7, ufsInfo.Lu1Size, utils.FileSize(string.Format(format8, ufsInfo.Lu1Size))), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg12, UI.log);
				UI.RichLogs(string.Format(format7, ufsInfo.Lu2Size, utils.FileSize(string.Format(format8, ufsInfo.Lu2Size))), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg13, UI.log);
				UI.RichLogs(string.Format(format7, ufsInfo.Lu3Size, utils.FileSize(string.Format(format8, ufsInfo.Lu3Size))), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg14, UI.log);
				UI.RichLogs(utils.OldStorage(ufsInfo.LfTime.ToHexString()), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg15, UI.log);
				UI.RichLogs(msg16, UI.info);
				UI.RichLogs(utils.StorageSCL(ufsInfo.Slc.ToHexString()), UI.info, nextline: false, bold: true);
				UI.RichLogs(msg17, UI.info, nextline: false, bold: true);
				UI.RichLogs(msg18, UI.info, nextline: false, bold: true);
				UI.RichLogs(utils.StorageSCL(ufsInfo.Mlc.ToHexString()), UI.info, nextline: true, bold: true);
				UI.RichLogs(msg, UI.log, nextline: true);
				Mtk.Config.PageSize = 4096;
				Mtk.DaLoader.Dacfg.PageSize = (int)ufsInfo.BlockSize;
				Mtk.DaLoader.Dacfg.FlashSize = ufsInfo.Lu0Size;
				return ufsInfo;
			}
			Console.WriteLine(text + Error.status((uint)status));
			return (UfsInfo)obj;
		}
		return (UfsInfo)obj;
	}

	public int Ack(bool rStatus = true)
	{
		int result = -1;
		try
		{
			if (Convert.ToUInt32(Mtk.Config.Chipcfg.Dacode) != 26497)
			{
				SerialMTK.Write(new uint[3] { 4277071599u, 1u, 4u }.Pack());
				SerialMTK.Write(new uint[1].Pack());
			}
			else
			{
				SerialMTK.Write(new uint[4] { 4277071599u, 1u, 4u, 0u }.Pack());
			}
			if (rStatus)
			{
				return GetStatus();
			}
			return 1;
		}
		catch (Exception)
		{
			return result;
		}
	}

	public bool XSend(object data, DataType dataType = (DataType)1, bool is64bit = false)
	{
		byte[] bytes;
		int num;
		if (utils.IsNumber(data))
		{
			if (!is64bit)
			{
				bytes = BitConverter.GetBytes(Convert.ToUInt32(data));
				num = 4;
				if (SerialMTK.Write(new uint[3]
				{
					4277071599u,
					(uint)dataType,
					4u
				}.Pack()))
				{
					return SerialMTK.Write(bytes);
				}
				return false;
			}
			bytes = BitConverter.GetBytes(Convert.ToUInt64(data));
			num = 8;
			if (SerialMTK.Write(new uint[3]
			{
				4277071599u,
				(uint)dataType,
				8u
			}.Pack()))
			{
				return SerialMTK.Write(bytes);
			}
			return false;
		}
		bytes = (byte[])data;
		num = bytes.Length;
		if (SerialMTK.Write(new uint[3]
		{
			4277071599u,
			(uint)dataType,
			(uint)num
		}.Pack()))
		{
			return SerialMTK.Write(bytes);
		}
		return false;
	}

	public byte[] XRead()
	{
		string text = "XRead error: ";
		try
		{
			byte[] array = SerialMTK.Read(12);
			if (12 == array.Length)
			{
				bool flag = BitConverter.ToUInt32(array, 0) == 4277071599u;
				bool flag2 = BitConverter.ToInt32(array, 4) == 1;
				if (flag && flag2)
				{
					int num = BitConverter.ToInt32(array, 8);
					array = SerialMTK.Read(num);
					if (array.Length == num)
					{
						return array;
					}
				}
			}
			return new byte[0];
		}
		catch (Exception ex)
		{
			Console.WriteLine(text + ex.Message);
			return Array.Empty<byte>();
		}
	}

	public int GetStatus()
	{
		string value = "readstatus : Wrong magic";
		string text = "Status length error: Too few data 0x";
		string text2 = "X";
		byte[] array = SerialMTK.Read(12);
		int num;
		if (array == null || array.Length != 12)
		{
			num = -1;
			return -1;
		}
		uint[] array2 = array.Unpack32();
		uint num2 = array2[0];
		uint num3 = array2[2];
		uint num4 = num2;
		if (num4 == 4277071599u)
		{
			byte[] array3 = SerialMTK.Read((int)num3);
			if (array3 != null && array3.Length >= num3)
			{
				switch (num3)
				{
				case 4u:
				{
					uint num6 = Binary.ReadU32LE(array3);
					switch (num6)
					{
					case 4277071599u:
						num = 0;
						return 0;
					default:
						return (int)num6;
					case 3221684228u:
						num = 0;
						return 0;
					}
				}
				case 2u:
				{
					ushort num5 = Binary.ReadU16LE(array3);
					if (num5 == 0)
					{
						num = 0;
						return 0;
					}
					return num5;
				}
				default:
					return (int)Binary.ReadU32LE(array3);
				}
			}
			Console.WriteLine(text + array3.Length.ToString(text2));
			num = -1;
			return -1;
		}
		Console.WriteLine(value);
		num = -1;
		return -1;
	}

	public bool SendParam(object param, DataType dataType = (DataType)1)
	{
		object obj = null;
		int num = 0;
		int num2 = 1;
		int num3 = -1073676287;
		int num4 = -1073479600;
		string text = "Error on sending parameter: ";
		byte[] array = param as byte[];
		param = ((array ==null) ? (param as List<byte[]>) : new List<byte[]> { array });
		foreach (byte[] item in (List<byte[]>)param)
		{
			if (!SerialMTK.Write(new uint[3]
			{
				4277071599u,
				1u,
				(uint)item.Length
			}.Pack()))
			{
				continue;
			}
			int num5 = item.Length;
			int num6 = 0;
			while (num5 > 0)
			{
				int num7 = Math.Min(num5, SerialMTK.usb.max_to_write);
				byte[] array2 = new byte[num7];
				Buffer.BlockCopy(item, num6, array2, 0, num7);
				if (!SerialMTK.Write(array2))
				{
					break;
				}
				num6 += num7;
				num5 -= num7;
				UI.Progresbar1(num6, item.Length);
			}
		}
		uint status = (uint)GetStatus();
		if (((status != 0) ? ((status == (uint)num3) ? 1 : 0) : num2) == 0)
		{
			if (((status == (uint)num4) ? 1 : 0) == num)
			{
				Console.WriteLine(text + Error.status(status));
			}
			return (byte)num != 0;
		}
		return (byte)num2 != 0;
	}

	public (bool status, byte[] data) SendDevctrl(Cmd cmd, byte[] param = null)
	{
		object obj = null;
		string format = "Error on sending dev ctrl {0}: {1}";
		uint num = 0u;
		if (!XSend(65545u))
		{
			Console.WriteLine(string.Format(format, cmd, Error.status(0u)));
			return (status: false, data: (byte[])obj);
		}
		num = (uint)GetStatus();
		if (num != 0 || !XSend((uint)cmd))
		{
			if (num != 3221291012u)
			{
				Console.WriteLine(string.Format(format, cmd, Error.status(num)));
				return (status: false, data: (byte[])obj);
			}
			return (status: false, data: (byte[])obj);
		}
		num = (uint)GetStatus();
		switch (num)
		{
		default:
			Console.WriteLine(string.Format(format, cmd, Error.status(num)));
			return (status: false, data: (byte[])obj);
		case 0u:
			if (param == obj)
			{
				return (status: true, data: XRead());
			}
			return (status: SendParam(param), data: (byte[])obj);
		case 3221291012u:
			return (status: false, data: (byte[])obj);
		}
	}

	private bool SetResetKey(uint resetKey = 104u)
	{
		return SendDevctrl(Cmd.SET_RESET_KEY, BitConverter.GetBytes(resetKey)).status;
	}

	private bool SetChecksumLevel(uint level = 0u)
	{
		return SendDevctrl((Cmd)131075u, BitConverter.GetBytes(level)).status;
	}

	private bool SendEmi(byte[] emi)
	{
		int result = 0;
		string text = "Error on sending emi: ";
		string value = "Error on sending emi: failed to send length";
		string value2 = "DRAM setup failed";
		string value3 = "DRAM setup passed";
		if (!XSend(65546u))
		{
			return false;
		}
		int status = GetStatus();
		if (status != 0)
		{
			Console.WriteLine(text + Error.status((uint)status));
			return false;
		}
		try
		{
			Thread.Sleep(10);
			if (!XSend(emi.Length))
			{
				Console.WriteLine(value);
				return false;
			}
			try
			{
				if (SendParam(emi))
				{
					Console.WriteLine(value3);
					return true;
				}
				Console.WriteLine(value2);
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine(text + ex.Message);
			}
		}
		catch (Exception ex2)
		{
			Console.WriteLine(text + ex2.Message);
		}
		return (byte)result != 0;
	}

	private bool EmiExtra()
	{
		int num = 1;
		string path = "Preloader";
		int result = 0;
		object obj = null;
		string value = "No EMMC Info";
		string msg = "Sending EMI... ";
		string value2 = "No  Emi Detected Try to Find Correct Emi... ";
		string searchPattern = "*";
		string text = "Preloader : ";
		string text2 = "Emi ";
		string text3 = " not accepted";
		string text4 = " Accepted";
		string msg2 = "Failed Correct Preloader Found!";
		string value3 = "No Correct Preloader Found!";
		EmmcInfo emmcInfo = GetEmmcInfo();
		string path2 = Path.Combine(utils.BaseDirectory, path);
		if (!Directory.Exists(path2))
		{
			return false;
		}
		if (emmcInfo.Cid == obj)
		{
			Console.WriteLine(value);
			return false;
		}
		UI.RichLogs(msg, UI.log);
		byte[] fromIndex = utils.GetFromIndex(emmcInfo.Cid, 0, 8);
		Console.WriteLine(value2);
		foreach (string item in Directory.EnumerateFiles(path2, searchPattern, SearchOption.AllDirectories))
		{
			Console.WriteLine(text + item);
			byte[] array = File.ReadAllBytes(item);
			if (array.IndexOf(fromIndex) != -1)
			{
				if (SendEmi(array))
				{
					Console.WriteLine(text2 + item + text4);
					return true;
				}
				Console.WriteLine(text2 + item + text3);
			}
		}
		UI.RichLogs(msg2, UI.error, (byte)num != 0, (byte)num != 0);
		Console.WriteLine(value3);
		return (byte)result != 0;
	}

	private bool SendData(byte[] data)
	{
		string text = "Error on sending data: ";
		int num;
		int max_to_write;
		int num2;
		int num5;
		int num6;
		int num3;
		if (SerialMTK.Write(new uint[3]
		{
			4277071599u,
			1u,
			(uint)data.Length
		}.Pack()))
		{
			num = data.Length;
			num2 = 0;
			max_to_write = SerialMTK.usb.max_to_write;
			if (0 >= num)
			{
				int status = GetStatus();
				if (status != 0)
				{
					Console.WriteLine(text + Error.status((uint)status));
					return false;
				}
				return true;
			}
			if (num > max_to_write)
			{
				num3 = max_to_write;
				if (SerialMTK.Write(data.AsSpan(0, num3).ToArray()))
				{
					num2 = 0 + num3;
					int num4 = 1907026508;
					while (true)
					{
						_ = num4 ^ -1396249852;
						UI.Progresbar1(num2, num);
						if (num2 >= num)
						{
							int status = GetStatus();
							if (status != 0)
							{
								Console.WriteLine(text + Error.status((uint)status));
								return false;
							}
							return true;
						}
						if (num - num2 <= max_to_write)
						{
							break;
						}
						num3 = max_to_write;
						if (SerialMTK.Write(data.AsSpan(num2, num3).ToArray()))
						{
							num2 += num3;
							num4 = 1907026508;
							continue;
						}
						return false;
					}
					num5 = 1066233341;
					goto IL_0183;
				}
				return false;
			}
			num3 = num;
			if (SerialMTK.Write(data.AsSpan(0, num3).ToArray()))
			{
				num2 = 0 + num3;
				num6 = 1907026508;
				goto IL_0104;
			}
			return false;
		}
		return false;
		IL_0104:
		_ = num6 ^ -1396249852;
		UI.Progresbar1(num2, num);
		if (num2 >= num)
		{
			int status = GetStatus();
			if (status != 0)
			{
				Console.WriteLine(text + Error.status((uint)status));
				return false;
			}
			return true;
		}
		if (num - num2 > max_to_write)
		{
			num3 = max_to_write;
			goto IL_0199;
		}
		num5 = 1066233341;
		goto IL_0183;
		IL_0183:
		_ = num5 ^ -1396249852;
		num3 = num - num2;
		goto IL_0199;
		IL_0199:
		if (SerialMTK.Write(data.AsSpan(num2, num3).ToArray()))
		{
			num2 += num3;
			num6 = 1907026508;
			goto IL_0104;
		}
		return false;
	}

	public bool BootTo(long Addr, byte[] da, double timeout = 0.5)
	{
		string format = "Error on boot to, Addr: 0x{0:X}";
		string format2 = "Error on boot usbwrite, Addr: 0x{0:X}";
		string format3 = "Error on boot to send_data, Addr: 0x{0:X}";
		string value = "Extensions were accepted. Jumping to extensions... ";
		string value2 = "Upload data was accepted. Jumping to stage 2... ";
		string value3 = "Boot to succeeded.";
		string text = "Error on boot to : ";
		if (XSend(65544u))
		{
			if (GetStatus() != 0)
			{
				Console.WriteLine(string.Format(format, Addr));
				return false;
			}
			byte[] array = new ulong[2]
			{
				(ulong)Addr,
				(ulong)da.Length
			}.Pack();
			byte[] data = new uint[3]
			{
				4277071599u,
				1u,
				(uint)array.Length
			}.Pack();
			if (SerialMTK.Write(data))
			{
				if (!SerialMTK.Write(array))
				{
					Console.WriteLine(string.Format(format2, Addr));
					return false;
				}
				if (!SendData(da))
				{
					Console.WriteLine(string.Format(format3, Addr));
					return false;
				}
				if (Addr != 1342111744L && Addr != 1744830464L)
				{
					Console.WriteLine(value2);
					utils.Pause(timeout);
				}
				else
				{
					Console.WriteLine(value);
					utils.Pause(timeout);
				}
				uint status = (uint)GetStatus();
				if (status == 1129208147 || status == 0)
				{
					Console.WriteLine(value3);
					return true;
				}
				Console.WriteLine(text + Error.status(status));
				return false;
			}
			Console.WriteLine(string.Format(format2, Addr));
			return false;
		}
		return false;
	}

	private byte[] GetConnectionAgent()
	{
		object param = null;
		string text = "Error on getting connection agent: ";
		(bool, byte[]) tuple = SendDevctrl((Cmd)262154u, (byte[])param);
		bool flag;
		uint status;
		if (!tuple.Item1)
		{
			flag = true;
		}
		else
		{
			if (tuple.Item2 != null)
			{
				if (flag = tuple.Item2.Length == 0)
				{
					return Array.Empty<byte>();
				}
				status = (uint)GetStatus();
				if (status == 0)
				{
					return tuple.Item2;
				}
				Console.WriteLine(text + Error.status(status));
				return Array.Empty<byte>();
			}
			flag = true;
		}
		if (flag)
		{
			return Array.Empty<byte>();
		}
		status = (uint)GetStatus();
		if (status == 0)
		{
			return tuple.Item2;
		}
		Console.WriteLine(text + Error.status(status));
		return Array.Empty<byte>();
	}

	private byte[] GetDAVersion()
	{
		object param = null;
		string text = "Error on getting DA version: ";
		string text2 = "DA-VERSION : ";
		(bool, byte[]) tuple = SendDevctrl(Cmd.GET_DA_VERSION, (byte[])param);
		bool flag;
		if (tuple.Item1)
		{
			if (tuple.Item2 != null)
			{
				if (!(flag = tuple.Item2.Length == 0))
				{
					uint status = (uint)GetStatus();
					if (status != 0)
					{
						Console.WriteLine(text + Error.status(status));
						return Array.Empty<byte>();
					}
					string text3 = Encoding.UTF8.GetString(tuple.Item2);
					Console.WriteLine(text2 + text3);
					return tuple.Item2;
				}
				return Array.Empty<byte>();
			}
			flag = true;
		}
		else
		{
			flag = true;
		}
		if (!flag)
		{
			uint status = (uint)GetStatus();
			if (status != 0)
			{
				Console.WriteLine(text + Error.status(status));
				return Array.Empty<byte>();
			}
			string text3 = Encoding.UTF8.GetString(tuple.Item2);
			Console.WriteLine(text2 + text3);
			return tuple.Item2;
		}
		return Array.Empty<byte>();
	}

	public ChipId GetChipID()
	{
		object obj = null;
		string format = "HW-CODE         : 0x{0:X}";
		string format2 = "HWSUB-CODE      : 0x{0:X}";
		string format3 = "HW-VERSION      : 0x{0:X}";
		string format4 = "SW-VERSION      : 0x{0:X}";
		string format5 = "CHIP-EVOLUTION  : 0x{0:X}";
		string text = "Error on getting chip id: ";
		byte[] item = SendDevctrl(Cmd.GET_CHIP_ID, (byte[])obj).data;
		int status;
		ChipId chipId;
		ushort[] array;
		ChipId chipId3;
		ChipId chipId4;
		ChipId chipId5;
		ChipId chipId6;
		int hwCode;
		int hwSubCode;
		int hwVersion;
		int swVersion;
		int chipEvolution;
		if (item == null)
		{
			if (item.Length < 10)
			{
				return (ChipId)obj;
			}
			status = GetStatus();
			if (status != 0)
			{
				Console.WriteLine(text + Error.status((uint)status));
				return (ChipId)obj;
			}
			chipId = new ChipId();
			array = item.Take(10).ToArray().Unpack16();
			ChipId chipId2 = chipId;
			chipId3 = chipId;
			chipId4 = chipId;
			chipId5 = chipId;
			chipId6 = chipId;
			hwCode = array[0];
			hwSubCode = array[1];
			hwVersion = array[2];
			swVersion = array[3];
			chipEvolution = array[4];
			chipId2.HwCode = hwCode;
			chipId3.HwSubCode = hwSubCode;
			chipId4.HwVersion = hwVersion;
			chipId5.SwVersion = swVersion;
			chipId6.ChipEvolution = chipEvolution;
			Console.WriteLine(string.Format(format, chipId.HwCode));
			Console.WriteLine(string.Format(format2, chipId.HwSubCode));
			Console.WriteLine(string.Format(format3, chipId.HwVersion));
			Console.WriteLine(string.Format(format4, chipId.SwVersion));
			Console.WriteLine(string.Format(format5, chipId.ChipEvolution));
			return chipId;
		}
		status = GetStatus();
		if (status != 0)
		{
			Console.WriteLine(text + Error.status((uint)status));
			return (ChipId)obj;
		}
		chipId = new ChipId();
		array = item.Take(10).ToArray().Unpack16();
		ChipId chipId7 = chipId;
		chipId3 = chipId;
		chipId4 = chipId;
		chipId5 = chipId;
		chipId6 = chipId;
		hwCode = array[0];
		hwSubCode = array[1];
		hwVersion = array[2];
		swVersion = array[3];
		chipEvolution = array[4];
		chipId7.HwCode = hwCode;
		chipId3.HwSubCode = hwSubCode;
		chipId4.HwVersion = hwVersion;
		chipId5.SwVersion = swVersion;
		chipId6.ChipEvolution = chipEvolution;
		Console.WriteLine(string.Format(format, chipId.HwCode));
		Console.WriteLine(string.Format(format2, chipId.HwSubCode));
		Console.WriteLine(string.Format(format3, chipId.HwVersion));
		Console.WriteLine(string.Format(format4, chipId.SwVersion));
		Console.WriteLine(string.Format(format5, chipId.ChipEvolution));
		return chipId;
	}

	private byte[] GetExpiredDate()
	{
		object param = null;
		string text = "Error on getting expire date: ";
		(bool, byte[]) tuple = SendDevctrl(Cmd.GET_EXPIRE_DATE, (byte[])param);
		if (!tuple.Item1 || tuple.Item2 == null || tuple.Item2.Length == 0)
		{
			return Array.Empty<byte>();
		}
		uint status = (uint)GetStatus();
		if (status != 0)
		{
			Console.WriteLine(text + Error.status(status));
			return Array.Empty<byte>();
		}
		return tuple.Item2;
	}

	private byte[] GetRandomID()
	{
		object obj = null;
		string text = "Error on getting random id: ";
		(bool, byte[]) tuple = SendDevctrl((Cmd)262152u, (byte[])obj);
		if (tuple.Item2 == obj)
		{
			return Array.Empty<byte>();
		}
		uint status = (uint)GetStatus();
		if (status == 0)
		{
			return tuple.Item2;
		}
		Console.WriteLine(text + Error.status(status));
		return Array.Empty<byte>();
	}

	private byte[] GetDevFwInfo()
	{
		object param = null;
		string text = "Error on getting dev fw info: ";
		(bool, byte[]) tuple = SendDevctrl(Cmd.GET_DEV_FW_INFO, (byte[])param);
		if (tuple.Item1 && tuple.Item2 != null && tuple.Item2.Length != 0)
		{
			uint status = (uint)GetStatus();
			if (status == 0)
			{
				return tuple.Item2;
			}
			Console.WriteLine(text + Error.status(status));
			return Array.Empty<byte>();
		}
		return Array.Empty<byte>();
	}

	private PacketLen GetPacketLength()
	{
		object obj = null;
		string text = "Error on getting packet length: ";
		(bool, byte[]) tuple = SendDevctrl(Cmd.GET_PACKET_LENGTH, (byte[])obj);
		if (tuple.Item1 && tuple.Item2 != null && tuple.Item2.Length != 0)
		{
			uint status = (uint)GetStatus();
			if (status == 0)
			{
				uint[] array = tuple.Item2.Take(8).ToArray().Unpack32();
				return new PacketLen
				{
					WritePacketLength = array[0],
					ReadPacketLength = array[1]
				};
			}
			Console.WriteLine(text + Error.status(status));
			return (PacketLen)obj;
		}
		return (PacketLen)obj;
	}

	private bool GetSLAStatus()
	{
		object param = null;
		string text = "Error on getting sla enabled status: ";
		(bool, byte[]) tuple = SendDevctrl((Cmd)262166u, (byte[])param);
		if (!tuple.Item1)
		{
			bool flag = true;
			return false;
		}
		if (tuple.Item2 == null || tuple.Item2.Length == 0)
		{
			return false;
		}
		uint status = (uint)GetStatus();
		if (status == 0)
		{
			return tuple.Item2[0] != 0;
		}
		Console.WriteLine(text + Error.status(status));
		return false;
	}

	private byte[] GetUSBSpeed()
	{
		object param = null;
		string text = "Error on getting usb speed: ";
		(bool, byte[]) tuple = SendDevctrl(Cmd.GET_USB_SPEED, (byte[])param);
		if (!tuple.Item1)
		{
			bool flag = true;
			return Array.Empty<byte>();
		}
		if (tuple.Item2 != null && tuple.Item2.Length != 0)
		{
			uint status = (uint)GetStatus();
			if (status == 0)
			{
				return tuple.Item2;
			}
			Console.WriteLine(text + Error.status(status));
			return Array.Empty<byte>();
		}
		return Array.Empty<byte>();
	}

	private bool SetUSBSpeed()
	{
		string text = "Error on switching usb speed: ";
		if (XSend(65547u))
		{
			int status = GetStatus();
			if (status == 0)
			{
				if (XSend(new uint[1] { 244129793u }.Pack()))
				{
					return GetStatus() == 0;
				}
				return false;
			}
			Console.WriteLine(text + Error.status((uint)status));
			return false;
		}
		return false;
	}

	private bool CmdWriteData(long length, string fileName)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (XSend(65537u))
		{
			if (GetStatus() != 0)
			{
				return false;
			}
			if (SendParam(new List<byte[]>
			{
				Encoding.UTF8.GetBytes(fileName),
				BitConverter.GetBytes(length)
			}))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private bool CmdReadData(long Addr, long size, int storage, int partType)
	{
		string text = "Error on reading data: ";
		if (XSend(65541u))
		{
			uint status = (uint)GetStatus();
			if (status == 0)
			{
				NandExtension nandExtension = new NandExtension();
				byte[] array = new uint[2]
				{
					(uint)storage,
					(uint)partType
				}.Pack();
				byte[] array2 = new ulong[2]
				{
					(ulong)Addr,
					(ulong)size
				}.Pack();
				byte[] array3 = new uint[8]
				{
					(uint)nandExtension.CellUsage,
					(uint)nandExtension.AddrType,
					(uint)nandExtension.BinType,
					(uint)nandExtension.Region,
					(uint)nandExtension.OperationType,
					(uint)nandExtension.SysSlcPercent,
					(uint)nandExtension.UsrSlcPercent,
					(uint)nandExtension.PhyMaxSize
				}.Pack();
				byte[] array4 = new byte[array.Length + array2.Length + array3.Length];
				int num = 0;
				array.AsSpan(0, array.Length).CopyTo(array4.AsSpan(0, array.Length));
				num = 0 + array.Length;
				array2.AsSpan(0, array2.Length).CopyTo(array4.AsSpan(num, array2.Length));
				num += array2.Length;
				array3.AsSpan(0, array3.Length).CopyTo(array4.AsSpan(num, array3.Length));
				if (!SendParam(array4))
				{
					return false;
				}
				status = (uint)GetStatus();
				if (status == 0)
				{
					return true;
				}
				Console.WriteLine(text + Error.status(status));
				return false;
			}
			Console.WriteLine(text + Error.status(status));
			return false;
		}
		return false;
	}

	private byte[] CmdPktParam(int length)
	{
		return new uint[3]
		{
			4277071599u,
			1u,
			(uint)length
		}.Pack();
	}

	public bool Shutdown(int asyncMode = 1)
	{
		int result = 0;
		if (XSend(65543u) && GetStatus() == 0)
		{
			try
			{
				if (XSend(new uint[6]
				{
					(uint)asyncMode,
					3000u,
					0u,
					0u,
					0u,
					0u
				}.Pack()))
				{
					return GetStatus() == 0;
				}
			}
			catch
			{
			}
		}
		return (byte)result != 0;
	}

	public bool Sync()
	{
		return XSend(1129208147u);
	}

	public bool SetupEnvironment()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (XSend(65792u))
		{
			return SendParam(new uint[5]
			{
				(uint)Dacfg.UartLogLevl,
				1u,
				1u,
				0u,
				0u
			}.Pack());
		}
		return false;
	}

	public bool SetupHwInit()
	{
		if (XSend(65793u))
		{
			return SendParam(BitConverter.GetBytes(0));
		}
		return false;
	}

	private (byte[] Da1, byte[] Da2) PatchDA(byte[] da1, byte[] da2)
	{
		uint signLen = Dacfg.Da.Region[1].SignLen;
		uint signLen2 = Dacfg.Da.Region[2].SignLen;
		var (num, hashMode, num2) = Mtk.DaLoader.ComputeHashPos(da1, da2, (int)signLen, (int)signLen2, Dacfg.Da.v6);
		if (num != 0)
		{
			da1 = XFlashExt.PatchDA1(da1);
			da2 = XFlashExt.PatchDA2(da2);
			da1 = Mtk.DaLoader.FixHash(da1, da2, num, hashMode, num2);
			Mtk.DaLoader.Patch = true;
			Dacfg.da2 = da2.AsSpan(0, num2).ToArray();
			return (Da1: da1, Da2: da2);
		}
		Mtk.DaLoader.Patch = false;
		int length = da2.Length - (int)signLen2;
		Dacfg.da2 = da2.AsSpan(0, length).ToArray();
		return (Da1: da1, Da2: da2);
	}

	private bool UploadDA1(int daparse)
	{
		string text = "Cek DA Pacth : ";
		int result = 0;
		object obj = null;
		string value = "No valid da loader found... aborting.";
		string value2 = "Uploading xflash stage 1";
		int num = 1;
		string text2 = "Preloader Mode V2";
		string value3 = "Successfully uploaded stage 1, jumping ..";
		string msg = "OK";
		string format = "Jumping to 0x{0:x}... ";
		string msg2 = "Send DA Sync Signal... ";
		string msg3 = "Setup Environment... ";
		string msg4 = "Setup hw init... ";
		string msg5 = "Syncing with DA... ";
		string value4 = "stat";
		try
		{
			Console.WriteLine(text + daparse);
			if (daparse == 0)
			{
				Dacfg.da1 = (byte[])obj;
				Dacfg.da2 = (byte[])obj;
				Dacfg.da3 = (byte[])obj;
				if (Dacfg.DownloadAgent != obj)
				{
					Console.WriteLine(value2);
					using MemoryStream memoryStream = new MemoryStream(Dacfg.DownloadAgent, writable: false);
					EntryRegion entryRegion = Dacfg.Da.Region[1];
					memoryStream.Seek(0L, SeekOrigin.Begin);
					memoryStream.Seek(entryRegion.Buff, SeekOrigin.Begin);
					byte[] array = new byte[entryRegion.Len];
					memoryStream.Read(array, 0, (int)entryRegion.Len);
					EntryRegion entryRegion2 = Dacfg.Da.Region[2];
					memoryStream.Seek(entryRegion2.Buff, SeekOrigin.Begin);
					byte[] array2 = new byte[entryRegion2.Len];
					memoryStream.Read(array2, 0, (int)entryRegion2.Len);
					if (DeviceState.Model == text2 && Dacfg.Da.Region[3].Buff != 0)
					{
						EntryRegion entryRegion3 = Dacfg.Da.Region[3];
						memoryStream.Seek(entryRegion3.Buff, SeekOrigin.Begin);
						byte[] array3 = new byte[entryRegion3.Len];
						memoryStream.Read(array3, 0, (int)entryRegion3.Len);
						Dacfg.da3 = array3.AsSpan(0, (int)(entryRegion3.Len - entryRegion3.SignLen)).ToArray();
					}
					if (!MTKConfig.Customda && (Mtk.DaLoader.Patch || !Mtk.Config.Targetconfig.Sbc))
					{
						(array, array2) = PatchDA(array, array2);
					}
					else
					{
						Dacfg.da2 = array2.AsSpan(0, (int)(entryRegion2.Len - entryRegion2.SignLen)).ToArray();
						Mtk.DaLoader.Patch = false;
					}
					if (Mtk.Pl.SendDA((int)entryRegion.StartAddr, (int)entryRegion.Len, (int)entryRegion.SignLen, array))
					{
						Console.WriteLine(value3);
						UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
						UI.RichLogs(string.Format(format, (int)entryRegion.StartAddr), UI.log);
						if (Mtk.Pl.JumpDA((int)entryRegion.StartAddr))
						{
							byte[] array4 = SerialMTK.Read(1);
							if (array4 == null || array4.Length != 1)
							{
								return false;
							}
							if (array4[0] == 192)
							{
								UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
								UI.RichLogs(msg2, UI.log);
								if (Sync())
								{
									UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
									UI.RichLogs(msg3, UI.log);
									if (SetupEnvironment())
									{
										UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
										UI.RichLogs(msg4, UI.log);
										SetupHwInit();
										UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
										UI.RichLogs(msg5, UI.log);
										byte[] array5 = XRead();
										if (array5 == obj)
										{
											return false;
										}
										if (BitConverter.ToInt32(array5, 0) != 1129208147L)
										{
											return false;
										}
										UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
										return true;
									}
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				Console.WriteLine(value);
				return false;
			}
			Console.WriteLine(value4);
			Console.WriteLine((int)Config.Chipcfg.PlPayloadAddr);
			if (!Mtk.Pl.SendDA((int)Config.Chipcfg.PlPayloadAddr, Dacfg.da1.Length, 256, Dacfg.da1))
			{
				return false;
			}
			Console.WriteLine(value3);
			UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
			UI.RichLogs(string.Format(format, (int)Config.Chipcfg.PlPayloadAddr), UI.log);
			if (Mtk.Pl.JumpDA((int)Config.Chipcfg.PlPayloadAddr))
			{
				byte[] array6 = SerialMTK.Read(1);
				if (array6 != null && array6.Length == 1)
				{
					if (array6[0] == 192)
					{
						UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
						UI.RichLogs(msg2, UI.log);
						if (Sync())
						{
							UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
							UI.RichLogs(msg3, UI.log);
							if (SetupEnvironment())
							{
								UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
								UI.RichLogs(msg4, UI.log);
								SetupHwInit();
								UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
								UI.RichLogs(msg5, UI.log);
								byte[] array7 = XRead();
								if (array7 == obj)
								{
									return false;
								}
								if (BitConverter.ToInt32(array7, 0) == 1129208147L)
								{
									UI.RichLogs(msg, UI.ok, nextline: true, bold: true);
									return true;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)num != 0, (byte)num != 0);
			return (byte)result != 0;
		}
	}

	private bool Reinit(bool show = false)
	{
		string flashType = "emmc";
		string flashType2 = "nand";
		string flashType3 = "nor";
		string flashType4 = "ufs";
		ChipId = GetChipID();
		DaVersion = GetDAVersion();
		Randomid = GetRandomID();
		(Raminfo, Raminfo) = GetRamInfo(show);
		NandInfo nandInfo;
		if ((Emmc = GetEmmcInfo(show)) == null)
		{
			nandInfo = (Nand = GetNandInfo());
		}
		else
		{
			if (Emmc.UserSize != 0L)
			{
				Dacfg.FlashType = flashType;
				Dacfg.FlashSize = Emmc.UserSize;
				Dacfg.rpmbsize = Emmc.RpmbSize;
				Dacfg.Boot1Size = Emmc.Boot1Size;
				Dacfg.Boot2Size = Emmc.Boot2Size;
				return true;
			}
			nandInfo = (Nand = GetNandInfo());
		}
		NorInfo norInfo;
		if (nandInfo == null)
		{
			norInfo = (Nor = GetNorInfo(show));
		}
		else
		{
			if (Nand.Type != 0)
			{
				Dacfg.FlashType = flashType2;
				Dacfg.FlashSize = Nand.TotalSize;
				Dacfg.rpmbsize = 0L;
				Dacfg.Boot1Size = 4194304L;
				Dacfg.Boot2Size = 4194304L;
				return true;
			}
			norInfo = (Nor = GetNorInfo(show));
		}
		if (norInfo != null)
		{
			if (Nor.Type != 0)
			{
				Dacfg.FlashType = flashType3;
				Dacfg.FlashSize = Nor.AvailableSize;
				Dacfg.rpmbsize = 0L;
				Dacfg.Boot1Size = 4194304L;
				Dacfg.Boot2Size = 4194304L;
				return true;
			}
			if ((Ufs = GetUFSInfo(show)) == null)
			{
				return true;
			}
			if (Ufs.BlockSize != 0L)
			{
				Dacfg.FlashType = flashType4;
				Dacfg.FlashSize = Ufs.Lu0Size;
				Dacfg.rpmbsize = Ufs.Lu1Size;
				Dacfg.Boot1Size = Ufs.Lu1Size;
				Dacfg.Boot2Size = Ufs.Lu2Size;
				return true;
			}
			return true;
		}
		if ((Ufs = GetUFSInfo(show)) == null)
		{
			return true;
		}
		if (Ufs.BlockSize != 0L)
		{
			Dacfg.FlashType = flashType4;
			Dacfg.FlashSize = Ufs.Lu0Size;
			Dacfg.rpmbsize = Ufs.Lu1Size;
			Dacfg.Boot1Size = Ufs.Lu1Size;
			Dacfg.Boot2Size = Ufs.Lu2Size;
			return true;
		}
		return true;
	}

	private bool SetRemoteSecPolicy(byte[] key)
	{
		return SendDevctrl((Cmd)131083u, key).status;
	}

	private bool HandleSLA()
	{
		object obj = null;
		int num = 0;
		string value = "No valid SLA key found, trying dummy auth... ";
		int num2 = 256;
		string value2 = "SLA Signature was accepted.";
		int result = 1;
		int start = 4;
		int length = 16;
		try
		{
			SlaKeys slaKeys = (SlaKeys)obj;
			foreach (SlaKeys da_sla_key in SlaKeys.da_sla_keys)
			{
				RSAParameters rSAParameters = da_sla_key.key.ExportParameters(includePrivateParameters: false);
				if (Dacfg.da2.IndexOf(rSAParameters.Modulus) == -1)
				{
					continue;
				}
				slaKeys = da_sla_key;
				break;
			}
			if (slaKeys != obj)
			{
				byte[] devFwInfo = GetDevFwInfo();
				if ((devFwInfo != null) ? ((nuint)devFwInfo.LongLength > (nuint)num) : ((byte)num != 0))
				{
					byte[] data = devFwInfo.AsSpan(start, length).ToArray();
					byte[] remoteSecPolicy = Sla.GenerateDaSlaSignature(data, slaKeys.key);
					if (SetRemoteSecPolicy(remoteSecPolicy))
					{
						Console.WriteLine(value2);
						return (byte)result != 0;
					}
				}
			}
			else
			{
				Console.WriteLine(value);
				byte[] remoteSecPolicy2 = new byte[num2];
				if (SetRemoteSecPolicy(remoteSecPolicy2))
				{
					Console.WriteLine(value2);
					return (byte)result != 0;
				}
			}
			return (byte)num != 0;
		}
		catch
		{
			return (byte)num != 0;
		}
	}

	private bool SetAllSign(byte[] data = null)
	{
		byte[] param = new uint[3] { 4277071599u, 1u, 0u }.Pack();
		return SendDevctrl(Cmd.SET_ALL_IN_ONE_SIG, param).status;
	}

	public bool UploadDA(int stage = 0)
	{
		string msg = "Failed!";
		string msg2 = "Booting device";
		string text = "(";
		string text2 = " auth)... ";
		string msg3 = "OK";
		string msg4 = "Sending EMI... ";
		string value = "Sending emi data failed.";
		string value2 = "Sending emi data ... ";
		string value3 = "Sending emi data succeeded.";
		string msg5 = "Sending Download-Agent [2]... ";
		string value4 = "Uploading stage 2... ";
		double field_ = Class38.field_2783;
		string value5 = "Successfully uploaded stage 2";
		string msg6 = "Sending DA Extention... ";
		object obj = null;
		string msg7 = "Failed! Patch EXT DA";
		string value6 = "DA Extensions failed to enable";
		string msg8 = "Failed! Sending DAEXT";
		string msg9 = "Failed! No Response From EXTDA";
		string format = "Failed! Response {0}";
		string value7 = "DA Extensions successfully added";
		string msg10 = "If Send DA2 Failed Try Check Carbonara Exploit!";
		string format2 = "Sending Download-Agent [{0}]... ";
		string text3 = "Preloader Mode V2";
		string value8 = "Uploading Stage 3... ";
		string value9 = "Successfully uploaded stage 3";
		int daparse = (MTKConfig.Danofarse ? 1 : 0);
		if (UploadDA1(daparse))
		{
			GetExpiredDate();
			SetResetKey();
			SetChecksumLevel();
			UI.RichLogs(msg2, UI.log);
			byte[] connectionAgent = GetConnectionAgent();
			UI.RichLogs(text + Encoding.UTF8.GetString(connectionAgent) + text2, UI.log);
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			if (!Config.IsBROM)
			{
				stage++;
			}
			else
			{
				stage = 1;
				UI.RichLogs(msg4, UI.log);
				if (Dacfg.Emi == null || Dacfg.Emi.Length == 0)
				{
					if (!EmiExtra())
					{
						Console.WriteLine(value);
						UI.RichLogs(msg, UI.error, nextline: true, bold: true);
						return false;
					}
				}
				else
				{
					Console.WriteLine(value2);
					if (!SendEmi(Dacfg.Emi))
					{
						Console.WriteLine(value);
						UI.RichLogs(msg, UI.error, nextline: true, bold: true);
						return false;
					}
				}
				Console.WriteLine(value3);
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			}
			if (MTKConfig.Danofarse)
			{
				if (!SendParam())
				{
					return false;
				}
				UI.RichLogs(msg5, UI.log);
				Console.WriteLine(value4);
				if (!BootTo(1073741824L, Dacfg.da2, field_))
				{
					UI.RichLogs(msg, UI.error, nextline: true, bold: true);
					return false;
				}
				Console.WriteLine(value5);
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				UI.RichLogs(msg6, UI.log);
				byte[] array = XFlashExt.Xtpatch(Dacfg.da2, 1073741824L);
				if (array == obj)
				{
					UI.RichLogs(msg7, UI.error, nextline: true, bold: true);
					return false;
				}
				if (!BootTo(1744830464L, array, field_))
				{
					Console.WriteLine(value6);
					UI.RichLogs(msg8, UI.error, nextline: true, bold: true);
					return true;
				}
				byte[] item = SendDevctrl((Cmd)983040u, (byte[])obj).data;
				if (GetStatus() != 0)
				{
					Console.WriteLine(value6);
					UI.RichLogs(msg9, UI.error, nextline: true, bold: true);
					return false;
				}
				if (Binary.ReadU32LE(item) != 2711790500u)
				{
					UI.RichLogs(string.Format(format, Binary.ReadI32LE(item)), UI.error, nextline: true, bold: true);
					return false;
				}
				Console.WriteLine(value7);
				UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			}
			else if (stage == 1)
			{
				stage++;
				bool flag = false;
				if (!Config.IsBROM && Mtk.Config.Targetconfig.Sbc && MTKConfig.UsingCarbonara && (flag = RunCarbonara()))
				{
					Mtk.DaLoader.Patch = true;
				}
				if (!flag)
				{
					if (SendParameterDAgent())
					{
						Mtk.DaLoader.Patch = true;
					}
					else
					{
						Mtk.DaLoader.Patch = false;
						UI.RichLogs(msg, UI.error, nextline: true, bold: true);
						UI.RichLogs(msg10, UI.error, nextline: true, bold: true);
					}
					UI.RichLogs(string.Format(format2, stage), UI.log);
					Console.WriteLine(value4);
					if (!BootTo(Dacfg.Da.Region[stage].StartAddr, Dacfg.da2, field_))
					{
						UI.RichLogs(msg, UI.error, nextline: true, bold: true);
						return false;
					}
					Console.WriteLine(value5);
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					if (DeviceState.Model == text3 && Dacfg.da3 !=null)
					{
						stage++;
						Console.WriteLine(value8);
						UI.RichLogs(string.Format(format2, stage), UI.log);
						if (!BootTo(Dacfg.Da.Region[stage].StartAddr, Dacfg.da3, field_))
						{
							UI.RichLogs(msg, UI.error, nextline: true, bold: true);
							return false;
						}
						if (!BootDACrypt())
						{
							UI.RichLogs(msg, UI.error, nextline: true, bold: true);
							return false;
						}
						Console.WriteLine(value9);
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					}
				}
				if (!BypassSLA(Config.IsBROM))
				{
					return false;
				}
				if (Mtk.DaLoader.Patch)
				{
					UI.RichLogs(msg6, UI.log);
					byte[] array2 = XFlashExt.Xtpatch(Dacfg.da2, 1073741824L);
					if (array2 == obj)
					{
						UI.RichLogs(msg7, UI.error, nextline: true, bold: true);
						return false;
					}
					if (!BootTo(1744830464L, array2, field_))
					{
						Console.WriteLine(value6);
						UI.RichLogs(msg8, UI.error, nextline: true, bold: true);
						return true;
					}
					byte[] item2 = SendDevctrl((Cmd)983040u, (byte[])obj).data;
					if (GetStatus() != 0)
					{
						Console.WriteLine(value6);
						UI.RichLogs(msg9, UI.error, nextline: true, bold: true);
						return false;
					}
					if (Binary.ReadU32LE(item2) != 2711790500u)
					{
						UI.RichLogs(string.Format(format, Binary.ReadI32LE(item2)), UI.error, nextline: true, bold: true);
						return false;
					}
					Console.WriteLine(value7);
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
				}
			}
			if (Reinit(show: true))
			{
				return SwitchUSB();
			}
			return false;
		}
		UI.RichLogs(msg, UI.error, nextline: true, bold: true);
		return false;
	}

	private bool RunCarbonara()
	{
		string msg = "Running Carbonara Exploit... ";
		string msg2 = "Not Support!";
		string msg3 = "Failed!";
		string msg4 = "OK";
		Carbonara carbonara = new Carbonara(Mtk);
		UI.RichLogs(msg, UI.log);
		if (carbonara.SupportChecking())
		{
			bool flag;
			if (flag = carbonara.Run())
			{
				UI.RichLogs(msg4, flag ? UI.ok : UI.error, nextline: true);
				return flag;
			}
			UI.RichLogs(msg3, flag ? UI.ok : UI.error, nextline: true);
			return flag;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		return false;
	}

	private bool SwitchUSB()
	{
		string msg = "Switching to highspeed usb... ";
		object obj = null;
		string msg2 = "Switch to highspeed mode failed!";
		string msg3 = "OK";
		string msg4 = "Waiting Connection Usb HighSpeed... ";
		double field_ = Class38.field_2783;
		string category = "HIGHSPEED";
		string msg5 = "Not found!";
		string msg6 = "Failed Connect to HighSpeed USB!";
		if (MTKConfig.Highspeed)
		{
			UI.RichLogs(msg, UI.log);
			byte[] uSBSpeed = GetUSBSpeed();
			if (uSBSpeed == obj)
			{
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg4, UI.log);
			if (SetUSBSpeed())
			{
				SerialMTK.Close();
				utils.Pause(field_);
				if (!UsbScan.FindUSB(category))
				{
					UI.RichLogs(msg5, UI.error, nextline: true, bold: true);
					return false;
				}
				if (SerialMTK.Open(Portinfo.COM))
				{
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					return true;
				}
				UI.RichLogs(msg6, UI.error, nextline: true, bold: true);
				return false;
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		return true;
	}

	private bool BypassSLA(bool brom)
	{
		string text = "Vivo";
		string msg = "Sending Authorized... ";
		string msg2 = "Failed!";
		string msg3 = "OK";
		string value = "DA SLA is Disable";
		string msg4 = "DA SLA Status : ";
		string msg5 = "Disable";
		string value2 = "DA SLA is enabled";
		string msg6 = "Enable";
		string msg7 = "DA SLA Bypassing... ";
		string value3 = "lake";
		string value4 = "tides";
		string value5 = "moon";
		string hex = "6D45077B9160CF31F943D4C91F9D26C5EA3094FB5273F31415BBF22DA47828D0B1D277D6A0F9DBE9199A3F0147C9EDBFA3B9C4BAD40042C6DE5C2E58FCA0F3E46F4F50559F0BD4BFAEBCB7BF2875E39CAECE5874D3204D23B09C20BFE4EDAE052D346B60E8797596F4DAC0D9877A0F1E4DC737258255398B2600F262AA8DA3911CB23B5D921AED5B566AC82008BFA53E03EB5C0E70D6253BE584233D17EA1AAB302EC73FE208BF5CF2640D3604AC44CB5D53CA05096AD2189658474428A0B94531E2A76042E2E2510E127139D622571DAEA1E4887E432E9E0A7430F216A1EAD3929F0AD5831854C58D9B367120FCBD393C8C59BC8DFDA4D4F007127A77ECE92A";
		string value6 = "Can't bypass DA SLA";
		if (brom || !(DeviceState.Brand == text))
		{
			if (GetSLAStatus())
			{
				Console.WriteLine(value2);
				UI.RichLogs(msg4, UI.log);
				UI.RichLogs(msg6, UI.error, nextline: true, bold: true);
				UI.RichLogs(msg7, UI.log);
				if (DeviceState.Model.ToLower().Contains(value3) || DeviceState.Model.ToLower().Contains(value4) || DeviceState.Model.ToLower().Contains(value5))
				{
					GetDevFwInfo();
					byte[] remoteSecPolicy = hex.HexToBytes();
					if (SetRemoteSecPolicy(remoteSecPolicy))
					{
						UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						return true;
					}
				}
				if (HandleSLA())
				{
					UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
					return true;
				}
				Console.WriteLine(value6);
				UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
				return false;
			}
			Console.WriteLine(value);
			UI.RichLogs(msg4, UI.log);
			UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
			return true;
		}
		UI.RichLogs(msg, UI.log);
		if (!SetAllSign(Resources.allin_one_signature))
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
		return true;
	}

	public static byte[] StringToByteArray(string hex)
	{
		string text = "0";
		if (hex.Length % 2 > 0)
		{
			string text2 = text + hex.Substring(hex.Length - 1);
			hex = hex.Substring(0, hex.Length - 1) + text2;
			return (from x in Enumerable.Range(0, hex.Length)
				where x % 2 == 0
				select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
		}
		return (from x in Enumerable.Range(0, hex.Length)
			where x % 2 == 0
			select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
	}

	private bool SendParam()
	{
		string msg = "Sending Param... ";
		string text = "MT";
		string text2 = "X";
		object obj = null;
		string text3 = "MT6765";
		string text4 = "MT6761";
		string text5 = "MT6768";
		string text6 = "MT6877";
		string text7 = "MT6781";
		string text8 = "MT6771";
		string text9 = "MT6833";
		string text10 = "MT6763";
		string text11 = "MT6853";
		string text12 = "Vivo";
		string text13 = "Oppo-Realme";
		string text14 = "Samsung";
		string text15 = "Preloader Patch V1";
		string text16 = "Preloader Patch V2";
		string text17 = "Samsung Galaxy A04e SM-A042F";
		string hex = "A762C73355ABC5D980F3D2C8BCF916580AB0DA166B0D70C4600BF35C88E44BFE";
		string hex2 = "9F5FEAC0301AD3F0277B28EFE558407436E1365869F169F7ABE14495449E386B";
		string hex3 = "ED18BDC79EC5B4FE1CDFF339DC576558CE5FA698D28A4E99CF41B7D294AF11E7";
		string hex4 = "156F419F6317C5102054DEF698935227C4B46E0BF3C33B2B83870C52FA18D6F7";
		string hex5 = "D457E8005489FB23D70BFC90FD62CB6A62CF581A17093D5197280577A684571E";
		string hex6 = "8D2381A2AE60FC526BEA1835765EF9CCE7BBE0EA";
		string hex7 = "DCA3DD65BBDF5248DD9C35F59117D15B54D72299D7969EAB179950BAAD46C926";
		string hex8 = "301150DF6FEAEB9F2B034C602CE6C57A550C0B3B2A26A45965954CB05BB1FC94";
		string hex9 = "BDA08F0275A653B4292CCA2680732CF22D6D9A65";
		string hex10 = "04F7C127AA4F7B521BE127F50914FE721BDE084852170D9FAEED42E902C631BE";
		string hex11 = "884457B964CE88E1AA7BCB00A95EAD3DA154E4A1FE2C1D8F698F3916F93E7625";
		string hex12 = "517B6AE475162D55A31A7DEBF58EF6D0CC875473BC02E0D9725985D12E359705";
		string msg2 = "OK";
		if (!Mtk.Config.IsBROM && MTKConfig.MTKServer && !MTKConfig.Customda)
		{
			UI.RichLogs(msg, UI.log);
			string item = text + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text2);
			uint num = 0u;
			byte[] data = (byte[])obj;
			(string, string, string) tuple = (item, DeviceState.Brand, DeviceState.Model);
			(string, string, string) tuple2 = tuple;
			string item2 = tuple2.Item1;
			uint num2 = ImplementationDetails.method_01(item2);
			if (num2 <= 72442538)
			{
				if (num2 <= 5332062)
				{
					if (num2 == 5184967)
					{
						if (item2 == text8)
						{
							string item3 = tuple2.Item2;
							if (item3 == text12)
							{
								string item4 = tuple2.Item3;
								if (item4 == text15)
								{
									num = 2295568u;
									data = hex6.HexToBytes();
								}
							}
							else if (item3 == text13)
							{
								string item4 = tuple2.Item3;
								if (item4 == text15)
								{
									num = 2257793u;
									data = hex6.HexToBytes();
								}
							}
						}
					}
					else if (num2 == 5332062 && item2 == text3)
					{
						string item3 = tuple2.Item2;
						if (!(item3 == text12))
						{
							if (!(item3 == text13))
							{
								if (item3 == text14)
								{
									string item4 = tuple2.Item3;
									if (item4 == text17)
									{
										num = 2285276u;
										data = hex12.HexToBytes();
									}
								}
							}
							else
							{
								string item4 = tuple2.Item3;
								if (!(item4 == text15))
								{
									if (item4 == text16)
									{
										num = 2273844u;
										data = hex8.HexToBytes();
									}
								}
								else
								{
									num = 2273908u;
									data = hex8.HexToBytes();
								}
							}
						}
						else
						{
							string item4 = tuple2.Item3;
							if (!(item4 == text15))
							{
								if (item4 == text16)
								{
									num = 2273844u;
									data = hex8.HexToBytes();
								}
							}
							else
							{
								num = 2273844u;
								data = hex.HexToBytes();
							}
						}
					}
				}
				else if (num2 == 38887300)
				{
					if (item2 == text10)
					{
						string item3 = tuple2.Item2;
						if (item3 == text13)
						{
							string item4 = tuple2.Item3;
							if (item4 == text15)
							{
								num = 2271329u;
								data = hex9.HexToBytes();
							}
						}
					}
				}
				else if (num2 == 72442538 && item2 == text4)
				{
					string item3 = tuple2.Item2;
					if (item3 == text12)
					{
						string item4 = tuple2.Item3;
						if (item4 == text15)
						{
							num = 2272268u;
							data = hex2.HexToBytes();
						}
					}
				}
			}
			else if (num2 > 2321510234u)
			{
				if (num2 != 3898622240u)
				{
					switch (num2)
					{
					case 4249966501u:
					{
						if (!(item2 == text5))
						{
							break;
						}
						string item3 = tuple2.Item2;
						if (item3 == text12)
						{
							string item4 = tuple2.Item3;
							if (item4 == text15)
							{
								num = 2285220u;
								data = hex3.HexToBytes();
							}
						}
						else if (item3 == text13)
						{
							string item4 = tuple2.Item3;
							if (item4 == text15)
							{
								num = 2285276u;
								data = hex11.HexToBytes();
							}
							else if (item4 == text16)
							{
								num = 2285276u;
								data = hex8.HexToBytes();
							}
						}
						break;
					}
					case 4133095728u:
					{
						if (!(item2 == text6))
						{
							break;
						}
						string item3 = tuple2.Item2;
						if (item3 == text12)
						{
							string item4 = tuple2.Item3;
							if (item4 == text15)
							{
								num = 2400552u;
								data = hex4.HexToBytes();
							}
						}
						break;
					}
					}
				}
				else if (item2 == text7)
				{
					string item3 = tuple2.Item2;
					if (item3 == text12)
					{
						string item4 = tuple2.Item3;
						if (item4 == text15)
						{
							num = 2265464u;
							data = hex5.HexToBytes();
						}
					}
				}
			}
			else
			{
				switch (num2)
				{
				case 2321510234u:
				{
					if (!(item2 == text11))
					{
						break;
					}
					string item3 = tuple2.Item2;
					if (item3 == text13)
					{
						string item4 = tuple2.Item3;
						if (item4 == text15)
						{
							num = 2330045u;
							data = hex10.HexToBytes();
						}
					}
					break;
				}
				case 2186303544u:
				{
					if (!(item2 == text9))
					{
						break;
					}
					string item3 = tuple2.Item2;
					if (item3 == text12)
					{
						string item4 = tuple2.Item3;
						if (item4 == text15)
						{
							num = 2340296u;
							data = hex7.HexToBytes();
						}
					}
					break;
				}
				}
			}
			DaArg daArg = new DaArg
			{
				addr = num,
				len = Convert.ToUInt64(data.Length)
			};
			byte[] result = new byte[15];
			byte[] array = StructToBytes(daArg, Marshal.SizeOf(daArg));
			xflashCmd(65544u, array, array.Length, ref data, data.Length, ref result, skipDataStatus: false, skipArgStatus: true);
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		}
		return true;
	}

	public static byte[] StructToBytes(object structObj, int length)
	{
		byte[] array = new byte[length];
		IntPtr intPtr = Marshal.AllocHGlobal(length);
		Marshal.StructureToPtr(structObj, intPtr, fDeleteOld: false);
		Marshal.Copy(intPtr, array, 0, length);
		Marshal.FreeHGlobal(intPtr);
		return array;
	}

	public bool xflashCmd(uint cmd, byte[] arg, int arglen, ref byte[] result, bool skipArgStatus = false, bool skipCmdStatus = false)
	{
		object obj = null;
		string fmt = "<III";
		byte[] result2 = (byte[])obj;
		if (!xflashCmd(cmd, ref result2, skipCmdStatus))
		{
			return false;
		}
		byte[] data = StructConverter.Pack(fmt, new object[3] { 4277071599u, 1u, arglen });
		if (!SerialMTK.Write(data))
		{
			return false;
		}
		if (arg == null || SerialMTK.Write(arg))
		{
			if (skipArgStatus)
			{
				return true;
			}
			return xflashReadData(cmd, ref result, mainCmd: false);
		}
		return false;
	}

	public bool xflashCmd(uint cmd, byte[] arg, int arglen, ref byte[] data, int datalen, ref byte[] result, bool skipDataStatus = false, bool skipArgStatus = false, bool skipCmdStatus = false)
	{
		object obj = null;
		string fmt = "<III";
		byte[] result2 = (byte[])obj;
		if (xflashCmd(cmd, arg, arglen, ref result2, skipArgStatus, skipCmdStatus))
		{
			byte[] data2 = StructConverter.Pack(fmt, new object[3] { 4277071599u, 1u, datalen });
			if (!SerialMTK.Write(data2))
			{
				return false;
			}
			if (!SerialMTK.Write(data))
			{
				return false;
			}
			if (!skipDataStatus)
			{
				return xflashReadData(cmd, ref result, mainCmd: false);
			}
			return true;
		}
		return false;
	}

	public bool xflashCmd(uint cmd, ref byte[] result, bool skipStatus = false)
	{
		string fmt = "<I";
		string fmt2 = "<III";
		byte[] data = StructConverter.Pack(fmt, new object[1] { cmd });
		byte[] data2 = StructConverter.Pack(fmt2, new object[3] { 4277071599u, 1u, 4 });
		SerialMTK.Write(data2);
		SerialMTK.Write(data);
		if (!skipStatus)
		{
			return xflashReadData(cmd, ref result, mainCmd: false);
		}
		return true;
	}

	public bool xflashReadData(uint cmd, ref byte[] result, bool mainCmd)
	{
		string fmt = "<III";
		string fmt2 = "<I";
		object obj = null;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		while (true)
		{
			byte[] bytes = SerialMTK.Read(12);
			object[] array = StructConverter.Unpack(fmt, bytes);
			Convert.ToUInt32(array[0]);
			uint num = Convert.ToUInt32(array[1]);
			uint num2 = Convert.ToUInt32(array[2]);
			if (num2 != 0)
			{
				if (num != 1)
				{
					byte[] array2 = SerialMTK.Read(Convert.ToInt32(num2));
					if (array2.Length == 0)
					{
						return false;
					}
					continue;
				}
				if (flag || (long)num2 != 4L)
				{
					if (!flag3)
					{
						byte[] array3 = SerialMTK.Read(Convert.ToInt32(num2));
						if (array3.Length == 0)
						{
							return false;
						}
						result = array3;
						if (cmd >= 262145 && cmd <= 262165)
						{
							flag3 = true;
							continue;
						}
						if (cmd == 983040)
						{
							flag3 = true;
							continue;
						}
					}
					else
					{
						bytes = SerialMTK.Read(Convert.ToInt32(num2));
						if (bytes.Length == 0)
						{
							return false;
						}
					}
				}
				else
				{
					if (flag2)
					{
						bytes = SerialMTK.Read(Convert.ToInt32(4));
						if (bytes.Length == 0)
						{
							return false;
						}
						continue;
					}
					bytes = SerialMTK.Read(Convert.ToInt32(4));
					if (Convert.ToUInt32(StructConverter.Unpack(fmt2, bytes)[0]) == 0)
					{
						if (!mainCmd && cmd == 65541 && !flag2)
						{
							flag2 = true;
							continue;
						}
						flag = true;
						if (result != obj)
						{
							continue;
						}
					}
					else if (cmd != 524293)
					{
						break;
					}
				}
			}
			else if (!flag)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	private bool SendParameterDAgent()
	{
		string msg = "Sending DAHash... ";
		string text = "MT";
		string text2 = "X";
		object obj = null;
		string text3 = "MT0907";
		string text4 = "MT1129";
		string text5 = "MT1172";
		string text6 = "MT1208";
		string text7 = "MT6761";
		string text8 = "MT6763";
		string text9 = "MT6765";
		string text10 = "MT6768";
		string text11 = "MT6771";
		string text12 = "MT6781";
		string text13 = "MT6833";
		string text14 = "MT6853";
		string text15 = "MT6877";
		string text16 = "MT6885";
		string text17 = "MT6893";
		string text18 = "Oppo-Realme";
		string text19 = "Preloader Mode V3";
		string text20 = "Motorola";
		string text21 = "Preloader Mode";
		string text22 = "Huawei-Honor";
		string text23 = "Preloader Mode V2";
		string text24 = "Infinix-Itel-Tecno";
		string text25 = "Vivo";
		string text26 = "Preloader Mode V1";
		string text27 = "Samsung";
		string text28 = "SM-A037F";
		string text29 = "SM-A042F";
		string text30 = "SM-A045F";
		string text31 = "SM-A226B";
		string hex = "6F85D83BB72BA8A7DB4AFA286BB26ED74145B83F1C07F45D3D11385308ED65E6";
		string hex2 = "383D8EDD782C2BB43B73FAAA58F1ED6C14B42A6E9B84C82CBDE5ADD0E1CD3FF9";
		string hex3 = "D102F42C90EC77D2F0912F36B9C42EFD000E865D2AF32399185F55AE67E543BC";
		string hex4 = "FC89007F25C867AE17546F2D4EEEE72ECF168BCE17673384FFF0A045FB3EDFC7";
		string hex5 = "3808B432F9BB3C4C27CAE9BBB0564EB86DA48139";
		string hex6 = "061317F0B64337270B64FA06B98335FFC4919DDFBA0B09C4484B5CD74F02D917";
		string hex7 = "9F5FEAC0301AD3F0277B28EFE558407436E1365869F169F7ABE14495449E386B";
		string hex8 = "BDA08F0275A653B4292CCA2680732CF22D6D9A65";
		string hex9 = "301150DF6FEAEB9F2B034C602CE6C57A550C0B3B2A26A45965954CB05BB1FC94";
		string hex10 = "83D3BE9BA49EC4EFB32B380EAB61DD1B80731672F069207E2F6FCE772E4FCD8F";
		string hex11 = "B37B1AD46FD6517AF42A9933B125EFB5131D4F60CCD9D9B59156EA5E26E3DC42";
		string hex12 = "A7A366A213FA23BC23F166E9359E3BC95954C1BF39FADA5F09CD3A2B35AAD153";
		string hex13 = "A762C73355ABC5D980F3D2C8BCF916580AB0DA166B0D70C4600BF35C88E44BFE";
		string hex14 = "FC17E71AF5CCE561712A4525BD64F94200F5E97164666DDA9C835E4A2D379C64";
		string hex15 = "279B46360ABE64113071EA1CCA88D59850B6723847898D7CC2329A54C7C5E724";
		string hex16 = "884457B964CE88E1AA7BCB00A95EAD3DA154E4A1FE2C1D8F698F3916F93E7625";
		string hex17 = "882E20BD4457BAC22A7FBBC7818EED7F0DE32B8B6D49B736E3FFF08220A481BA";
		string hex18 = "A2D6E3678EBF4C6E6DE695E0B7ECBA21C84FAA27934FB0D7284BFF4CD889AE88";
		string hex19 = "ED18BDC79EC5B4FE1CDFF339DC576558CE5FA698D28A4E99CF41B7D294AF11E7";
		string hex20 = "8D2381A2AE60FC526BEA1835765EF9CCE7BBE0EA";
		string hex21 = "D457E8005489FB23D70BFC90FD62CB6A62CF581A17093D5197280577A684571E";
		string hex22 = "2E4E8BAB706D65C1F61127F401089CB7A1B8FBAB38E5ADB3A1013D393B7DF25D";
		string hex23 = "C3A3EB7C2A7CF542979F3AE2394CD02CC82545083EC6B74963ACB86FD33666D1";
		string hex24 = "C1591CDEDB858C9640D64F27935254A521BC8EB9EECBC071676018994B0392C6";
		string hex25 = "DCA3DD65BBDF5248DD9C35F59117D15B54D72299D7969EAB179950BAAD46C926";
		string hex26 = "04F7C127AA4F7B521BE127F50914FE721BDE084852170D9FAEED42E902C631BE";
		string hex27 = "156F419F6317C5102054DEF698935227C4B46E0BF3C33B2B83870C52FA18D6F7";
		string hex28 = "D53CF9726CDD545915F51B9FDB7AD7945DAB6EB5483FE52FC57C090079FA9621";
		string hex29 = "69A46ADAD957095CC7E4CEB77D8A977DC476503E2AE0A2D27BD84FB7B002CE09";
		string hex30 = "73AB62A0BFA1841E0F58406EAED024FAA41D76C22DEEA289860A8197EA1109D1";
		string hex31 = "CD0AAB864C858CFD75CF1882B8447A92EECE7DAD32CEA0E6457E4C1EA44CB891";
		string msg2 = "OK";
		if (!Mtk.Config.IsBROM && MTKConfig.MTKServer && !MTKConfig.Customda)
		{
			UI.RichLogs(msg, UI.log);
			string item = text + Convert.ToUInt16(Mtk.Config.Chipcfg.Dacode).ToString(text2);
			uint num = 0u;
			byte[] array = (byte[])obj;
			(string, string, string) tuple = (item, DeviceState.Brand, DeviceState.Model);
			(string, string, string) tuple2 = tuple;
			string item2 = tuple2.Item1;
			uint num2 = ImplementationDetails.method_01(item2);
			if (num2 <= 1927833983)
			{
				if (num2 <= 38887300)
				{
					if (num2 == 5184967)
					{
						if (item2 == text11)
						{
							string item3 = tuple2.Item2;
							if (!(item3 == text18))
							{
								if (item3 == text25)
								{
									string item4 = tuple2.Item3;
									if (!(item4 == text26))
									{
										if (item4 == text23)
										{
											num = 2272268u;
											array = hex20.HexToBytes();
										}
									}
									else
									{
										num = 2295568u;
										array = hex20.HexToBytes();
									}
								}
							}
							else
							{
								string item4 = tuple2.Item3;
								if (item4 == text26 || item4 == text19)
								{
									num = 2257793u;
									array = hex20.HexToBytes();
								}
							}
						}
					}
					else if (num2 == 5332062)
					{
						if (item2 == text9)
						{
							string item3 = tuple2.Item2;
							if (!(item3 == text22))
							{
								if (!(item3 == text24))
								{
									if (!(item3 == text20))
									{
										if (item3 == text18)
										{
											string item4 = tuple2.Item3;
											if (item4 == text26 || item4 == text23 || item4 == text19)
											{
												num = 2273908u;
												array = hex9.HexToBytes();
											}
										}
										else if (!(item3 == text27))
										{
											if (item3 == text25)
											{
												string item4 = tuple2.Item3;
												if (!(item4 == text26))
												{
													if (item4 == text23)
													{
														num = 2273844u;
														array = hex9.HexToBytes();
													}
												}
												else
												{
													num = 2273844u;
													array = hex13.HexToBytes();
												}
											}
										}
										else
										{
											string item4 = tuple2.Item3;
											if (item4 == text28)
											{
												num = 2272588u;
												array = hex9.HexToBytes();
											}
											else if (!(item4 == text29))
											{
												if (item4 == text30)
												{
													num = 2273812u;
													array = hex9.HexToBytes();
												}
											}
											else
											{
												num = 2272588u;
												array = hex12.HexToBytes();
											}
										}
									}
									else
									{
										string item4 = tuple2.Item3;
										if (item4 == text21)
										{
											num = 2273812u;
											array = hex11.HexToBytes();
										}
									}
								}
								else
								{
									string item4 = tuple2.Item3;
									if (item4 == text26)
									{
										num = 2272268u;
										array = hex10.HexToBytes();
									}
									else if (item4 == text23 || item4 == text19)
									{
										num = 2273812u;
										array = hex9.HexToBytes();
									}
								}
							}
							else
							{
								string item4 = tuple2.Item3;
								if (item4 == text21)
								{
									num = 2273812u;
									array = hex9.HexToBytes();
								}
							}
						}
					}
					else if (num2 == 38887300 && item2 == text8)
					{
						string item3 = tuple2.Item2;
						if (item3 == text18)
						{
							string item4 = tuple2.Item3;
							if (item4 == text26 || item4 == text19)
							{
								num = 2271329u;
								array = hex8.HexToBytes();
							}
						}
					}
				}
				else if (num2 > 1607001342)
				{
					if (num2 != 1876662483)
					{
						if (num2 == 1927833983 && item2 == text5)
						{
							string item3 = tuple2.Item2;
							if (item3 == text22)
							{
								string item4 = tuple2.Item3;
								if (item4 == text21)
								{
									num = 33972768u;
									array = hex3.HexToBytes();
								}
							}
							else if (item3 == text18)
							{
								string item4 = tuple2.Item3;
								if (item4 == text19)
								{
									num = 34005408u;
									array = hex4.HexToBytes();
								}
							}
						}
					}
					else if (item2 == text4)
					{
						string item3 = tuple2.Item2;
						if (item3 == text20)
						{
							string item4 = tuple2.Item3;
							if (item4 == text21)
							{
								num = 34141880u;
								array = hex2.HexToBytes();
							}
						}
					}
				}
				else
				{
					switch (num2)
					{
					case 1607001342u:
					{
						if (!(item2 == text3))
						{
							break;
						}
						string item3 = tuple2.Item2;
						if (item3 == text18)
						{
							string item4 = tuple2.Item3;
							if (item4 == text19)
							{
								num = 17356464u;
								array = hex.HexToBytes();
							}
						}
						break;
					}
					case 72442538u:
					{
						if (!(item2 == text7))
						{
							break;
						}
						string item3 = tuple2.Item2;
						if (item3 == text24)
						{
							string item4 = tuple2.Item3;
							if (item4 == text26)
							{
								num = 2272268u;
								array = hex6.HexToBytes();
							}
						}
						else if (item3 == text25)
						{
							string item4 = tuple2.Item3;
							if (item4 == text26 || item4 == text23)
							{
								num = 2272268u;
								array = hex7.HexToBytes();
							}
						}
						break;
					}
					}
				}
			}
			else if (num2 > 2321510234u)
			{
				if (num2 <= 3898622240u)
				{
					switch (num2)
					{
					case 3898622240u:
					{
						if (!(item2 == text12))
						{
							break;
						}
						string item3 = tuple2.Item2;
						if (item3 == text25)
						{
							string item4 = tuple2.Item3;
							if (item4 == text26)
							{
								num = 2265464u;
								array = hex21.HexToBytes();
							}
						}
						break;
					}
					case 2353636311u:
					{
						if (!(item2 == text6))
						{
							break;
						}
						string item3 = tuple2.Item2;
						if (item3 == text18)
						{
							string item4 = tuple2.Item3;
							if (item4 == text23 || item4 == text19)
							{
								num = 2434636u;
								array = hex5.HexToBytes();
							}
						}
						break;
					}
					}
				}
				else if (num2 != 4133095728u)
				{
					if (num2 == 4249966501u && item2 == text10)
					{
						string item3 = tuple2.Item2;
						if (!(item3 == text22))
						{
							if (!(item3 == text24))
							{
								if (!(item3 == text20))
								{
									if (item3 == text18)
									{
										string item4 = tuple2.Item3;
										if (!(item4 == text26) && !(item4 == text23))
										{
											if (item4 == text19)
											{
												num = 2284749u;
												array = hex18.HexToBytes();
											}
										}
										else
										{
											num = 2285276u;
											array = hex16.HexToBytes();
										}
									}
									else if (item3 == text25)
									{
										string item4 = tuple2.Item3;
										if (item4 == text26)
										{
											num = 2285220u;
											array = hex19.HexToBytes();
										}
										else if (item4 == text23)
										{
											num = 2285220u;
											array = hex16.HexToBytes();
										}
									}
								}
								else
								{
									string item4 = tuple2.Item3;
									if (item4 == text21)
									{
										num = 2285220u;
										array = hex17.HexToBytes();
									}
								}
							}
							else
							{
								string item4 = tuple2.Item3;
								if (item4 == text26)
								{
									num = 2285164u;
									array = hex15.HexToBytes();
								}
								else if (item4 == text23)
								{
									num = 2285164u;
									array = hex16.HexToBytes();
								}
								else if (item4 == text19)
								{
									num = 2285220u;
									array = hex16.HexToBytes();
								}
							}
						}
						else
						{
							string item4 = tuple2.Item3;
							if (item4 == text21)
							{
								num = 2285228u;
								array = hex14.HexToBytes();
							}
						}
					}
				}
				else if (item2 == text15)
				{
					string item3 = tuple2.Item2;
					if (item3 == text18)
					{
						string item4 = tuple2.Item3;
						if (!(item4 == text23))
						{
							if (item4 == text19)
							{
								num = 2400328u;
								array = hex28.HexToBytes();
							}
						}
						else
						{
							num = 2400416u;
							array = hex27.HexToBytes();
						}
					}
					else if (item3 == text25)
					{
						string item4 = tuple2.Item3;
						if (!(item4 == text26))
						{
							if (item4 == text23)
							{
								num = 2339000u;
								array = hex29.HexToBytes();
							}
						}
						else
						{
							num = 2400552u;
							array = hex27.HexToBytes();
						}
					}
				}
			}
			else
			{
				switch (num2)
				{
				case 2321510234u:
				{
					if (!(item2 == text14))
					{
						break;
					}
					string item3 = tuple2.Item2;
					if (!(item3 == text18))
					{
						if (item3 == text25)
						{
							string item4 = tuple2.Item3;
							if (item4 == text23)
							{
								num = 2345788u;
								array = hex26.HexToBytes();
							}
						}
					}
					else
					{
						string item4 = tuple2.Item3;
						if (item4 == text26 || item4 == text23 || item4 == text19)
						{
							num = 2330045u;
							array = hex26.HexToBytes();
						}
					}
					break;
				}
				case 2185317806u:
				{
					if (!(item2 == text17))
					{
						break;
					}
					string item3 = tuple2.Item2;
					if (item3 == text22)
					{
						string item4 = tuple2.Item3;
						if (item4 == text21)
						{
							num = 2342744u;
							array = hex31.HexToBytes();
						}
					}
					else if (item3 == text18)
					{
						string item4 = tuple2.Item3;
						if (item4 == text23 || item4 == text19)
						{
							num = 2342384u;
							array = hex31.HexToBytes();
						}
					}
					break;
				}
				case 2017394521u:
				{
					if (!(item2 == text16))
					{
						break;
					}
					string item3 = tuple2.Item2;
					if (item3 == text18)
					{
						string item4 = tuple2.Item3;
						if (item4 == text19)
						{
							num = 2323189u;
							array = hex30.HexToBytes();
						}
					}
					break;
				}
				case 2186303544u:
				{
					if (!(item2 == text13))
					{
						break;
					}
					string item3 = tuple2.Item2;
					if (!(item3 == text22))
					{
						if (!(item3 == text24))
						{
							if (!(item3 == text18))
							{
								if (item3 == text27)
								{
									string item4 = tuple2.Item3;
									if (item4 == text31)
									{
										num = 2336884u;
										array = hex23.HexToBytes();
									}
								}
								else if (item3 == text25)
								{
									string item4 = tuple2.Item3;
									if (item4 == text26)
									{
										num = 2340296u;
										array = hex25.HexToBytes();
									}
									else if (item4 == text23)
									{
										num = 2340296u;
										array = hex23.HexToBytes();
									}
								}
							}
							else
							{
								string item4 = tuple2.Item3;
								if (item4 == text23)
								{
									num = 2340200u;
									array = hex23.HexToBytes();
								}
								else if (item4 == text19)
								{
									num = 2339897u;
									array = hex24.HexToBytes();
								}
							}
						}
						else
						{
							string item4 = tuple2.Item3;
							if (item4 == text23)
							{
								num = 2340136u;
								array = hex23.HexToBytes();
							}
						}
					}
					else
					{
						string item4 = tuple2.Item3;
						if (item4 == text21)
						{
							num = 2340192u;
							array = hex22.HexToBytes();
						}
					}
					break;
				}
				}
			}
			if (num == 0 || array == null || array.Length == 0)
			{
				return false;
			}
			byte[] array2 = new byte[16];
			Span<byte> buffer = array2.AsSpan();
			Binary.WriteU64LE(buffer, 0, num);
			Binary.WriteU64LE(buffer, 8, (ulong)array.Length);
			if (!XSend(65544))
			{
				return false;
			}
			if (GetStatus() != 0)
			{
				return false;
			}
			if (!XSend(array2))
			{
				return false;
			}
			if (!XSend(array))
			{
				return false;
			}
			if (GetStatus() != 0)
			{
				return false;
			}
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		}
		return true;
	}

	private bool BootDACrypt()
	{
		string hex = "0004d49ea184e800bd83f0e38a6d4e2f";
		byte[] meid = Config.Meid;
		byte[] iv = hex.HexToBytes();
		SecurityDA securityDA = new SecurityDA
		{
			is_security_enabled = 0u,
			sec_enc_seccfg = 0u,
			mmc_get_card = 0u,
			mmc_set_part_config = 0u,
			mmc_rpmb_send_command = 0u,
			tzclk_on_addr = 268439692u,
			tzclk_on_value = 402653184u,
			tzclk_off_addr = 268439688u,
			tzclk_off_value = 402653184u,
			dxcc_base = 270598144u,
			g_ufs_hba = 0u,
			ufshcd_get_free_tag = 0u,
			ufshcd_queuecommand = 0u,
			ufshcd_put_tag = 0u,
			mmc_permanent_cmd = 0u,
			mmc_permanent_patch = 0u
		};
		int hWCode = Config.HWCode;
		int num = hWCode;
		if (num > 1928)
		{
			if (num <= 2384)
			{
				switch (num)
				{
				case 2384:
					securityDA.is_security_enabled = 1073810956u;
					securityDA.sec_enc_seccfg = 1074034633u;
					securityDA.mmc_get_card = 1073832173u;
					securityDA.mmc_set_part_config = 1073834769u;
					securityDA.mmc_rpmb_send_command = 1073848221u;
					securityDA.g_ufs_hba = 1074184520u;
					securityDA.ufshcd_get_free_tag = 1073789597u;
					securityDA.ufshcd_queuecommand = 1073792089u;
					securityDA.ufshcd_put_tag = 1073789649u;
					securityDA.mmc_permanent_cmd = 1073834807u;
					securityDA.mmc_permanent_patch = 1073833265u;
					break;
				case 2070:
					securityDA.is_security_enabled = 1073808044u;
					securityDA.sec_enc_seccfg = 1074015473u;
					securityDA.mmc_get_card = 1073829133u;
					securityDA.mmc_set_part_config = 1073831729u;
					securityDA.mmc_rpmb_send_command = 1073845181u;
					securityDA.g_ufs_hba = 1074157252u;
					securityDA.ufshcd_get_free_tag = 1073787225u;
					securityDA.ufshcd_queuecommand = 1073789449u;
					securityDA.ufshcd_put_tag = 1073787277u;
					securityDA.mmc_permanent_cmd = 1073831767u;
					securityDA.mmc_permanent_patch = 1073830225u;
					break;
				case 2067:
					securityDA.is_security_enabled = 1073807752u;
					securityDA.sec_enc_seccfg = 1073965257u;
					securityDA.mmc_get_card = 1073828669u;
					securityDA.mmc_set_part_config = 1073831261u;
					securityDA.mmc_rpmb_send_command = 1073845009u;
					securityDA.g_ufs_hba = 1074104192u;
					securityDA.ufshcd_get_free_tag = 1073786997u;
					securityDA.ufshcd_queuecommand = 1073789221u;
					securityDA.ufshcd_put_tag = 1073787049u;
					securityDA.mmc_permanent_cmd = 1073831299u;
					securityDA.mmc_permanent_patch = 1073829757u;
					break;
				case 2182:
					securityDA.is_security_enabled = 1073808136u;
					securityDA.sec_enc_seccfg = 1074019117u;
					securityDA.mmc_get_card = 1073829229u;
					securityDA.mmc_set_part_config = 1073831825u;
					securityDA.mmc_rpmb_send_command = 1073845261u;
					securityDA.g_ufs_hba = 1074164036u;
					securityDA.ufshcd_get_free_tag = 1073787289u;
					securityDA.ufshcd_queuecommand = 1073789513u;
					securityDA.ufshcd_put_tag = 1073787341u;
					securityDA.mmc_permanent_cmd = 1073831863u;
					securityDA.mmc_permanent_patch = 1073830321u;
					break;
				}
			}
			else if (num <= 2441)
			{
				switch (num)
				{
				case 2441:
					securityDA.is_security_enabled = 1073810972u;
					securityDA.sec_enc_seccfg = 1074024213u;
					securityDA.mmc_get_card = 1073820777u;
					securityDA.mmc_set_part_config = 1073823373u;
					securityDA.mmc_rpmb_send_command = 1073836853u;
					securityDA.g_ufs_hba = 1074162112u;
					securityDA.ufshcd_get_free_tag = 1073789589u;
					securityDA.ufshcd_queuecommand = 1073792073u;
					securityDA.ufshcd_put_tag = 1073789641u;
					securityDA.mmc_permanent_cmd = 1073823411u;
					securityDA.mmc_permanent_patch = 1073821869u;
					break;
				case 2393:
					securityDA.is_security_enabled = 1073810968u;
					securityDA.sec_enc_seccfg = 1074086913u;
					securityDA.mmc_get_card = 1073832137u;
					securityDA.mmc_set_part_config = 1073834733u;
					securityDA.mmc_rpmb_send_command = 1073848257u;
					securityDA.g_ufs_hba = 1074286536u;
					securityDA.ufshcd_get_free_tag = 1073789585u;
					securityDA.ufshcd_queuecommand = 1073792077u;
					securityDA.ufshcd_put_tag = 1073789637u;
					securityDA.mmc_permanent_cmd = 1073834771u;
					securityDA.mmc_permanent_patch = 1073833229u;
					break;
				}
			}
			else if (num != 2454)
			{
				if (num == 4198)
				{
					securityDA.is_security_enabled = 1073810928u;
					securityDA.sec_enc_seccfg = 1073960997u;
					securityDA.mmc_get_card = 1073830937u;
					securityDA.mmc_set_part_config = 1073833533u;
					securityDA.mmc_rpmb_send_command = 1073847009u;
					securityDA.g_ufs_hba = 1074090960u;
					securityDA.ufshcd_get_free_tag = 1073789613u;
					securityDA.ufshcd_queuecommand = 1073792097u;
					securityDA.ufshcd_put_tag = 1073789665u;
					securityDA.mmc_permanent_cmd = 1073833571u;
					securityDA.mmc_permanent_patch = 1073832029u;
				}
			}
			else
			{
				securityDA.is_security_enabled = 1073808204u;
				securityDA.sec_enc_seccfg = 1074027753u;
				securityDA.mmc_get_card = 1073829437u;
				securityDA.mmc_set_part_config = 1073832097u;
				securityDA.mmc_rpmb_send_command = 1073849109u;
				securityDA.g_ufs_hba = 1074179788u;
				securityDA.ufshcd_get_free_tag = 1073787417u;
				securityDA.ufshcd_queuecommand = 1073789641u;
				securityDA.ufshcd_put_tag = 1073787469u;
				securityDA.mmc_permanent_cmd = 1073830537u;
			}
		}
		else if (num <= 1799)
		{
			if (num > 1680)
			{
				if (num != 1689)
				{
					if (num == 1799)
					{
						securityDA.is_security_enabled = 1073789408u;
						securityDA.sec_enc_seccfg = 1073977061u;
						securityDA.mmc_get_card = 1073808393u;
						securityDA.mmc_set_part_config = 1073810989u;
						securityDA.mmc_rpmb_send_command = 1073824341u;
						securityDA.mmc_permanent_cmd = 1073811027u;
						securityDA.mmc_permanent_patch = 1073809485u;
					}
				}
				else
				{
					securityDA.is_security_enabled = 1073786484u;
					securityDA.sec_enc_seccfg = 1073893009u;
					securityDA.mmc_get_card = 1073803853u;
					securityDA.mmc_set_part_config = 1073805531u;
					securityDA.mmc_rpmb_send_command = 1073800333u;
					securityDA.mmc_permanent_cmd = 1073805569u;
				}
			}
			else if (num == 806)
			{
				securityDA.is_security_enabled = 1073795112u;
				securityDA.sec_enc_seccfg = 1073900625u;
				securityDA.mmc_get_card = 1073819181u;
				securityDA.mmc_set_part_config = 1073822073u;
				securityDA.mmc_rpmb_send_command = 1073812389u;
			}
			else if (num == 1680)
			{
				securityDA.is_security_enabled = 1073803660u;
				securityDA.sec_enc_seccfg = 1073959045u;
				securityDA.mmc_get_card = 1073822453u;
				securityDA.mmc_set_part_config = 1073825045u;
				securityDA.mmc_rpmb_send_command = 1073838813u;
				securityDA.g_ufs_hba = 1074067408u;
				securityDA.ufshcd_get_free_tag = 1073782993u;
				securityDA.ufshcd_queuecommand = 1073785217u;
				securityDA.ufshcd_put_tag = 1073783045u;
				securityDA.mmc_permanent_cmd = 1073825083u;
				securityDA.mmc_permanent_patch = 1073823541u;
			}
		}
		else if (num > 1829)
		{
			if (num != 1894)
			{
				if (num == 1928)
				{
					securityDA.is_security_enabled = 1073803676u;
					securityDA.sec_enc_seccfg = 1073954729u;
					securityDA.mmc_get_card = 1073824177u;
					securityDA.mmc_set_part_config = 1073826769u;
					securityDA.mmc_rpmb_send_command = 1073840497u;
					securityDA.g_ufs_hba = 1074059452u;
					securityDA.ufshcd_get_free_tag = 1073782993u;
					securityDA.ufshcd_queuecommand = 1073785217u;
					securityDA.ufshcd_put_tag = 1073783045u;
					securityDA.mmc_permanent_cmd = 1073826807u;
					securityDA.mmc_permanent_patch = 1073825265u;
				}
			}
			else
			{
				securityDA.is_security_enabled = 1073786024u;
				securityDA.sec_enc_seccfg = 1073950189u;
				securityDA.mmc_get_card = 1073805573u;
				securityDA.mmc_set_part_config = 1073808181u;
				securityDA.mmc_rpmb_send_command = 1073822101u;
				securityDA.mmc_permanent_cmd = 1073808219u;
				securityDA.mmc_permanent_patch = 1073806665u;
			}
		}
		else
		{
			switch (num)
			{
			case 1829:
				securityDA.is_security_enabled = 1073803948u;
				securityDA.sec_enc_seccfg = 1073958709u;
				securityDA.mmc_get_card = 1073823633u;
				securityDA.mmc_set_part_config = 1073826225u;
				securityDA.mmc_rpmb_send_command = 1073840021u;
				securityDA.g_ufs_hba = 1074087804u;
				securityDA.ufshcd_get_free_tag = 1073783213u;
				securityDA.ufshcd_queuecommand = 1073785437u;
				securityDA.ufshcd_put_tag = 1073783265u;
				securityDA.mmc_permanent_cmd = 1073826263u;
				securityDA.mmc_permanent_patch = 1073824721u;
				break;
			case 1815:
				securityDA.is_security_enabled = 1073785868u;
				securityDA.sec_enc_seccfg = 1073953277u;
				securityDA.mmc_get_card = 1073805413u;
				securityDA.mmc_set_part_config = 1073808081u;
				securityDA.mmc_rpmb_send_command = 1073825245u;
				securityDA.mmc_permanent_patch = 1073806509u;
				break;
			}
		}
		byte[] array = new byte[64];
		int offset = 0;
		Binary.WriteU32LE(array, ref offset, securityDA.is_security_enabled);
		Binary.WriteU32LE(array, ref offset, securityDA.sec_enc_seccfg);
		Binary.WriteU32LE(array, ref offset, securityDA.mmc_get_card);
		Binary.WriteU32LE(array, ref offset, securityDA.mmc_set_part_config);
		Binary.WriteU32LE(array, ref offset, securityDA.mmc_rpmb_send_command);
		Binary.WriteU32LE(array, ref offset, securityDA.tzclk_on_addr);
		Binary.WriteU32LE(array, ref offset, securityDA.tzclk_on_value);
		Binary.WriteU32LE(array, ref offset, securityDA.tzclk_off_addr);
		Binary.WriteU32LE(array, ref offset, securityDA.tzclk_off_value);
		Binary.WriteU32LE(array, ref offset, securityDA.dxcc_base);
		Binary.WriteU32LE(array, ref offset, securityDA.g_ufs_hba);
		Binary.WriteU32LE(array, ref offset, securityDA.ufshcd_get_free_tag);
		Binary.WriteU32LE(array, ref offset, securityDA.ufshcd_queuecommand);
		Binary.WriteU32LE(array, ref offset, securityDA.ufshcd_put_tag);
		Binary.WriteU32LE(array, ref offset, securityDA.mmc_permanent_cmd);
		Binary.WriteU32LE(array, ref offset, securityDA.mmc_permanent_patch);
		array = Crypt(meid, iv, array);
		if (!XSend(786432))
		{
			return false;
		}
		if (!XSend(array))
		{
			return true;
		}
		if (GetStatus() != 0)
		{
			return false;
		}
		return true;
	}

	private byte[] Crypt(byte[] key, byte[] iv, byte[] data)
	{
		using Aes aes = Aes.Create();
		aes.KeySize = 128;
		aes.Key = key;
		aes.IV = iv;
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.Zeros;
		ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
		using MemoryStream memoryStream = new MemoryStream();
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
		{
			cryptoStream.Write(data, 0, data.Length);
			cryptoStream.FlushFinalBlock();
		}
		return memoryStream.ToArray();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
