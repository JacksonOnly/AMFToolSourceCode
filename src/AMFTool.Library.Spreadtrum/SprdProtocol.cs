using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AMFTool.Library.Spreadtrum.Config;
using AMFTool.Library.Usb;

namespace AMFTool.Library.Spreadtrum;

public class SprdProtocol
{
	public enum SCMD
	{
		CONNECT = 0,
		START_DATA = 1,
		MIDST_DATA = 2,
		END_DATA = 3,
		EXEC_DATA = 4,
		NORMAL_RESET = 5,
		READ_FLASH = 6,
		READ_CHIP_TYPE = 7,
		READ_NVITEM = 8,
		CHANGE_BAUD = 9,
		ERASE_FLASH = 10,
		REPARTITION = 11,
		READ_FLASH_TYPE = 12,
		READ_FLASH_INFO = 13,
		READ_SECTOR_SIZE = 15,
		READ_START = 16,
		READ_MIDST = 17,
		READ_END = 18,
		KEEP_CHARGE = 19,
		EXTTABLE = 20,
		READ_FLASH_UID = 21,
		READ_SOFTSIM_EID = 22,
		POWER_OFF = 23,
		CHECK_ROOT = 25,
		YCC_CMD_UNLOCK_BOOTLOADER = 25,
		READ_CHIP_UID = 26,
		YCC_CMD_LOCK_BOOTLOADER = 26,
		ENABLE_WRITE_FLASH = 27,
		ENABLE_SECUREBOOT = 28,
		IDENTIFY_START = 29,
		IDENTIFY_END = 30,
		READ_CU_REF = 31,
		READ_REFINFO = 32,
		DISABLE_TRANSCODE = 33,
		WRITE_DATETIME = 34,
		CUST_DUMMY = 35,
		READ_RF_TRANSCEIVER_TYPE = 36,
		SET_DEBUGINFO = 37,
		DDR_CHECK = 38,
		SELF_REFRESH = 39,
		WRITE_RAW_DATA_ENABLE = 40,
		READ_NAND_BLOCK_INFO = 41,
		SET_FIRST_MODE = 42,
		READ_PARTITION = 45,
		DLOAD_RAW_START = 49,
		WRITE_FLUSH_DATA = 50,
		DLOAD_RAW_START2 = 51,
		READ_LOG = 53,
		CHECK_BAUD = 126,
		END_PROCESS = 127,
		REP_ACK = 128,
		REP_VER = 129,
		REP_INVALID_CMD = 130,
		REP_UNKNOW_CMD = 131,
		REP_OPERATION_FAILED = 132,
		REP_NOT_SUPPORT_BAUDRATE = 133,
		REP_DOWN_NOT_START = 134,
		REP_DOWN_MULTI_START = 135,
		REP_DOWN_EARLY_END = 136,
		REP_DOWN_DEST_ERROR = 137,
		REP_DOWN_SIZE_ERROR = 138,
		REP_VERIFY_ERROR = 139,
		REP_NOT_VERIFY = 140,
		PHONE_NOT_ENOUGH_MEMORY = 141,
		PHONE_WAIT_INPUT_TIMEOUT = 142,
		PHONE_SUCCEED = 143,
		PHONE_VALID_BAUDRATE = 144,
		PHONE_REPEAT_CONTINUE = 145,
		PHONE_REPEAT_BREAK = 146,
		REP_READ_FLASH = 147,
		REP_READ_CHIP_TYPE = 148,
		REP_READ_NVITEM = 149,
		REP_INCOMPATIBLE_PARTITION = 150,
		REP_SIGN_VERIFY_ERROR = 166,
		REP_CHECK_ROOT_TRUE = 167,
		REP_READ_CHIP_UID = 171,
		REP_READ_PARTITION = 186,
		REP_READ_LOG = 187,
		YCC_REP_SET_BOOTLOADER_SUCCESS = 204,
		REP_UNSUPPORTED_COMMAND = 254,
		REP_LOG = 255,
		ERROR_CHECKSUM = 160,
		CHECKSUM_DIFF = 161,
		WRITE_ERROR = 162
	}

	public enum Stages
	{
		None,
		Brom,
		Payload,
		Preloader1,
		Preloader2,
		FDL1,
		FDL2
	}

	public enum SocType
	{
		None,
		New,
		Old
	}

	public class DAInfo
	{
		public uint Version;

		public uint DisableHDLC;

		public byte IsOldMemory;

		public byte SupportRawData;

		public byte[] bReserve;

		public uint FlushSize;

		public uint StorageType;

		public uint[] dwReserve;
	}

	public class StorageConfig
	{
		public string MemoryType;

		public int BlockSize;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public StorageConfig Storage;

	public Stages CurrentStage;

	public SocType SOCType;

	private SprdConfig Cfg;

	private SprdMain SprdMain;

	private Random Random;

	public string BromVersion;

	public int PerBlockSize;

	private (byte[], Gpt) partition;

	public byte[] Keys { get; set; }

	public SprdProtocol()
	{
		object obj = null;
		CurrentStage = Stages.None;
		SOCType = SocType.None;
		Random = new Random();
		PerBlockSize = 61440;
		Keys = (byte[])obj;

		Cfg = new SprdConfig();
		SprdMain = new SprdMain();
	}

	public bool InitDevices()
	{
		string msg = "Failed Init SPRD Config";
		string msg2 = "Handshaking... ";
		string msg3 = "Failed!";
		string msg4 = "OK";
		string msg5 = "Protocol : ";
		string fileName = "payload";
		string text = "Spreadtrum/";
		string text2 = "/payload.bin";
		string msg6 = "Sending Auth data... ";
		string msg7 = "Sending Pleloader 1... ";
		string msg8 = "SKIP";
		string msg9 = "Sending Pleloader 2...  ";
		string format = "Spreadtrum/{0}/preloader{1}.bin";
		string format2 = "Preloader {0}";
		string msg10 = "Booting Loader... ";
		string msg11 = "Boot Version : ";
		string format3 = "Spreadtrum/{0}/loader{1}.bin";
		string format4 = "FDL {0}";
		if (Cfg.InitCPUName(SPDConfig.CPUType))
		{
			UI.RichLogs(msg2, UI.log);
			if (!Handshake())
			{
				UI.RichLogs(msg3, UI.error, nextline: true);
				return false;
			}
			UI.RichLogs(msg4, UI.ok, nextline: true);
			if (!string.IsNullOrEmpty(BromVersion))
			{
				UI.RichLogs(msg5, UI.log);
				UI.RichLogs(BromVersion, UI.info, nextline: true);
			}
			if (DownloadUpload(fileName, text + SPDConfig.CPUType + text2, Cfg.ChipCfg.PayloadAddr))
			{
				CurrentStage = Stages.Payload;
				UI.RichLogs(msg6, UI.log);
				if (GetAndSendAuth())
				{
					UI.RichLogs(msg4, UI.ok, nextline: true);
					if (PSDConfig.Pactloader)
					{
						UI.RichLogs(msg7, UI.log);
						UI.RichLogs(msg8, UI.error, nextline: true);
						UI.RichLogs(msg9, UI.log);
						UI.RichLogs(msg8, UI.error, nextline: true);
					}
					else
					{
						for (int i = 1; i <= 2; i++)
						{
							string filePath = string.Format(format, SPDConfig.CPUType, i);
							uint addr = ((i == 1) ? Cfg.ChipCfg.Preload1Addr : Cfg.ChipCfg.Preload2Addr);
							if (!DownloadUpload(string.Format(format2, i), filePath, addr, decrypt: true))
							{
								return false;
							}
							CurrentStage = ((i == 1) ? Stages.Preloader1 : Stages.Preloader2);
						}
					}
					UI.RichLogs(msg10, UI.log);
					if (ReHandshake(out var bootVersion))
					{
						UI.RichLogs(msg4, UI.ok, nextline: true);
						UI.RichLogs(msg11, UI.log);
						UI.RichLogs(bootVersion, UI.info, nextline: true);
						int num = 1;
						while (true)
						{
							if (num > 2)
							{
								return true;
							}
							string filePath2 = string.Format(format3, SPDConfig.CPUType, num);
							uint addr2 = ((num == 1) ? Cfg.ChipCfg.Fdl1Addr : Cfg.ChipCfg.Fdl2Addr);
							if (!DownloadUpload(string.Format(format4, num), filePath2, addr2, decrypt: true))
							{
								break;
							}
							CurrentStage = ((num == 1) ? Stages.FDL1 : Stages.FDL2);
							num++;
						}
						return false;
					}
					UI.RichLogs(msg3, UI.error, nextline: true);
					return false;
				}
				UI.RichLogs(msg3, UI.error, nextline: true);
				return false;
			}
			return false;
		}
		UI.RichLogs(msg, UI.error, nextline: true);
		return false;
	}

	private bool Handshake()
	{
		int result = 0;
		object obj = null;
		try
		{
			SerialSPD.Handshake = true;
			SprdMain.Cksm = SprdMain.Checksum.CRC;
			for (int i = 0; i < 10; i++)
			{
				SprdMain.Response response = SprdMain.SendCommandAndGetResponse(SCMD.CHECK_BAUD, (byte[])obj);
				if (response != null && response.Type == 129 && response.Payload !=null)
				{
					BromVersion = Encoding.ASCII.GetString(response.Payload).TrimEnd(default(char));
				}
				if (!string.IsNullOrEmpty(BromVersion))
				{
					break;
				}
			}
			SerialSPD.Handshake = false;
			if (!SprdMain.SendCommandAndACK(SCMD.CONNECT, (byte[])obj))
			{
				return false;
			}
			CurrentStage = Stages.Brom;
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return (byte)result != 0;
		}
	}

	private bool ReHandshake(out string bootVersion)
	{
		int num = 1;
		int num2 = 0;
		int cmd = 126;
		object obj = null;
		int num3 = 129;
		double field_ = Class38.field_2783;
		int num4 = 20;
		bootVersion = string.Empty;
		SprdMain.Cksm = SprdMain.Checksum.FDL;
		SerialSPD.Handshake = true;
		for (int i = 0; ((i > num4) ? 1 : 0) == num2; i += num)
		{
			SprdMain.Response response = SprdMain.SendCommandAndGetResponse((SCMD)cmd, (byte[])obj);
			if ((response != null) ? ((uint)response.Length > (uint)num2) : ((byte)num2 != 0))
			{
				try
				{
					if ((response.Type != num3) ? ((byte)num2 != 0) : (response.Payload !=null))
					{
						bootVersion = Encoding.ASCII.GetString(response.Payload).TrimEnd(new char[num]);
						break;
					}
				}
				catch
				{
				}
			}
			utils.Pause(field_);
		}
		if (string.IsNullOrEmpty(bootVersion))
		{
			return (byte)num2 != 0;
		}
		SerialSPD.Handshake = (byte)num2 != 0;
		if ((SprdMain.SendCommandAndACK((SCMD)num2, (byte[])obj) ? 1 : 0) == num2)
		{
			return (byte)num2 != 0;
		}
		return (byte)num != 0;
	}

	private bool DownloadUpload(string fileName, string filePath, uint addr, bool decrypt = false)
	{
		string text = "Sending ";
		string text2 = "... ";
		string msg = "Failed! File not found";
		string msg2 = "Failed!";
		string msg3 = "OK";
		UI.RichLogs(text + fileName + text2, UI.log);
		byte[] array = Server.LoadAndDecrypt(filePath);
		if (array == null || array.Length == 0)
		{
			UI.RichLogs(msg, UI.error, nextline: true);
			return false;
		}
		bool flag;
		if (!decrypt)
		{
			flag = !UploadFile(addr, array);
		}
		else
		{
			RC4Transform(Keys, 0, array);
			flag = !UploadFile(addr, array);
		}
		if (!flag)
		{
			UI.RichLogs(msg3, UI.ok, nextline: true);
			return true;
		}
		UI.RichLogs(msg2, UI.error, nextline: true);
		return false;
	}

	private bool UploadFile(uint address, byte[] data)
	{
		object payload = null;
		bool flag;
		if (!SendFile(address, data))
		{
			flag = false;
			return false;
		}
		if (CurrentStage == Stages.Brom || CurrentStage == Stages.Preloader2)
		{
			flag = true;
			return true;
		}
		if (CurrentStage == Stages.Payload || CurrentStage == Stages.Preloader1)
		{
			return flag = SprdMain.SendCommandAndACK(SCMD.EXEC_DATA, (byte[])payload);
		}
		if (CurrentStage != Stages.FDL1)
		{
			flag = false;
			return false;
		}
		SprdMain.Response response = SprdMain.SendCommandAndGetResponse(SCMD.EXEC_DATA, (byte[])payload);
		if (response.Type != 150)
		{
			return flag = response.Type == 128;
		}
		DAInfo daInfo = GetDaInfo(response.Payload);
		bool flag2;
		if (daInfo.DisableHDLC != 0)
		{
			if (!SprdMain.SendCommandAndACK(SCMD.DISABLE_TRANSCODE, (byte[])payload))
			{
				flag = false;
				return false;
			}
			SprdMain.DisableTranscode();
			flag2 = daInfo.SupportRawData > 0;
		}
		else
		{
			flag2 = daInfo.SupportRawData > 0;
		}
		if (flag2)
		{
			if (SprdMain.SendCommandAndACK(SCMD.WRITE_RAW_DATA_ENABLE, (byte[])payload))
			{
				return true;
			}
			flag = false;
			return false;
		}
		return true;
	}

	private bool SendFile(uint addr, byte[] data)
	{
		double field_ = Class38.field_543;
		object payload = null;
		if (SprdMain.SendCommandAndACK(SCMD.START_DATA, BuildPacket(addr, data)))
		{
			int chunk = GetChunk();
			Stopwatch stopwatch = Stopwatch.StartNew();
			int num = 0;
			while (num < data.Length)
			{
				int num2 = Math.Min(chunk, data.Length - num);
				byte[] array = new byte[num2];
				data.AsSpan(num, num2).CopyTo(array.AsSpan(0, num2));
				if (SprdMain.SendCommandAndACK(SCMD.MIDST_DATA, array))
				{
					num += num2;
					UI.Progresbar1(num, data.Length);
					double totalSeconds = stopwatch.Elapsed.TotalSeconds;
					if (totalSeconds > field_)
					{
						UI.LabelWrite(utils.FileSize((long)((double)num / totalSeconds)), utils.FileSize(num), utils.FileSize(data.Length));
					}
					continue;
				}
				return false;
			}
			if (!SprdMain.SendCommandAndACK(SCMD.END_DATA, (byte[])payload))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	private void RC4Transform(byte[] key, int skipLen, byte[] buffer)
	{
		byte[] array = new byte[256];
		for (int i = 0; i < 256; i++)
		{
			array[i] = (byte)i;
		}
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < 256; j++)
		{
			num = (num + array[j] + key[num2]) & 0xFF;
			byte b = array[j];
			array[j] = array[num];
			array[num] = b;
			num2++;
			if (num2 >= key.Length)
			{
				num2 = 0;
			}
		}
		int num3 = 0;
		num = 0;
		for (int k = 0; k < skipLen; k++)
		{
			num3 = (num3 + 1) & 0xFF;
			num = (num + array[num3]) & 0xFF;
			byte b2 = array[num3];
			array[num3] = array[num];
			array[num] = b2;
		}
		for (int l = 0; l < buffer.Length; l++)
		{
			num3 = (num3 + 1) & 0xFF;
			num = (num + array[num3]) & 0xFF;
			byte b3 = array[num3];
			array[num3] = array[num];
			array[num] = b3;
			int num4 = (array[num3] + array[num]) & 0xFF;
			buffer[l] ^= array[num4];
		}
	}

	private DAInfo GetDaInfo(byte[] data)
	{
		string memoryType = "ufs";
		string memoryType2 = "emmc";
		DAInfo dAInfo = new DAInfo();
		int num = data.Length;
		if (Binary.ReadU32LE(data) != 1953981806)
		{
			int num2 = 0;
			dAInfo.Version = Binary.ReadU32LE(data);
			num2 = 4;
			dAInfo.DisableHDLC = Binary.ReadU32LE(data, 4);
			num2 = 8;
			num2 = 9;
			dAInfo.IsOldMemory = data[8];
			num2 = 10;
			dAInfo.SupportRawData = data[9];
			dAInfo.bReserve = data.AsSpan(10, 2).ToArray();
			num2 = 12;
			dAInfo.FlushSize = Binary.ReadU32LE(data, 12);
			num2 = 16;
			dAInfo.StorageType = Binary.ReadU32LE(data, 16);
			num2 = 20;
			dAInfo.dwReserve = new uint[59];
			for (int i = 0; i < 59; i++)
			{
				if (num2 + 4 > num)
				{
					break;
				}
				dAInfo.dwReserve[i] = Binary.ReadU32LE(data, num2);
				num2 += 4;
			}
		}
		else
		{
			int num3 = 4;
			while (num3 + 4 <= num)
			{
				ushort num4 = Binary.ReadU16LE(data, num3);
				ushort num5 = Binary.ReadU16LE(data, num3 + 2);
				num3 += 4;
				if (num3 + num5 > num)
				{
					break;
				}
				switch (num4)
				{
				case 0:
					if (num5 >= 4)
					{
						dAInfo.DisableHDLC = Binary.ReadU32LE(data, num3);
					}
					break;
				case 2:
					dAInfo.SupportRawData = data[num3];
					break;
				case 3:
					if (num5 >= 4)
					{
						dAInfo.FlushSize = Binary.ReadU32LE(data, num3);
					}
					break;
				case 6:
					if (num5 >= 4)
					{
						dAInfo.StorageType = Binary.ReadU32LE(data, num3);
					}
					break;
				}
				num3 += num5;
			}
		}
		Storage = new StorageConfig();
		if (dAInfo.StorageType == 259)
		{
			Storage.MemoryType = memoryType;
			Storage.BlockSize = 4096;
		}
		else
		{
			Storage.MemoryType = memoryType2;
			Storage.BlockSize = 512;
		}
		return dAInfo;
	}

	private byte[] GetBootKey(byte[] data)
	{
		uint[] array = new uint[4];
		data.AsSpan(0, Math.Min(data.Length, 16)).CopyTo(MemoryMarshal.AsBytes(array.AsSpan()).Slice(0, Math.Min(data.Length, 16)));
		byte[] array2 = new byte[16];
		uint key = GetKey(0, array[1], array[2]);
		uint key2 = GetKey(1, array[2]);
		uint key3 = GetKey(2, array[0], key);
		uint key4 = GetKey(3, array[3]);
		uint key5 = GetKey(4, key2, array[2]);
		array2[0] = GetByte(key, 0);
		array2[1] = GetByte(key2, 0);
		array2[2] = GetByte(key3, 3);
		array2[3] = GetByte(key, 1);
		array2[4] = GetByte(key4, 1);
		array2[5] = GetByte(key3, 2);
		array2[6] = GetByte(key5, 3);
		array2[7] = GetByte(key4, 3);
		array2[8] = GetByte(key, 3);
		array2[9] = GetByte(key5, 2);
		array2[10] = GetByte(key5, 1);
		array2[11] = GetByte(key3, 1);
		array2[12] = GetByte(key, 2);
		array2[13] = GetByte(key4, 0);
		array2[14] = GetByte(key3, 0);
		array2[15] = GetByte(key4, 2);
		return array2;
	}

	private bool GetAndSendAuth()
	{
		int result = 0;
		int result2 = 1;
		string text = "Raw key: ";
		try
		{
			for (int i = 0; i < 200; i++)
			{
				uint addr = Cfg.ChipCfg.RandomAddr + (uint)(Random.Next(1, (int)Cfg.ChipCfg.RandomBound) * 8);
				uint value = (uint)Random.Next(1, 65535);
				if (SprdMain.SendCommandAndACK(SCMD.START_DATA, BuildPacket(addr, 4u)))
				{
					byte[] bytes = BitConverter.GetBytes(value);
					if (!SprdMain.SendCommandAndACK(SCMD.MIDST_DATA, bytes))
					{
						return false;
					}
					continue;
				}
				return false;
			}
			List<uint> list = new List<uint>
			{
				Cfg.ChipCfg.KeyAddr,
				Cfg.ChipCfg.KeyAddr - 64,
				Cfg.ChipCfg.KeyAddr - 96,
				Cfg.ChipCfg.KeyAddr - 128
			};
			byte[] array = new byte[16];
			foreach (uint item in list)
			{
				byte[] payload = ((Cfg.ChipCfg.KeyValue <= 4294967295L) ? BitConverter.GetBytes((uint)Cfg.ChipCfg.KeyValue) : BitConverter.GetBytes(Cfg.ChipCfg.KeyValue));
				if (SprdMain.SendCommandAndACK(SCMD.START_DATA, BuildPacket(item, 4u)))
				{
					SprdMain.Response response = SprdMain.SendCommandAndGetResponse(SCMD.MIDST_DATA, payload);
					if (response.Payload != null && response.Payload.Length != 0)
					{
						response.Payload.AsSpan(0, Math.Min(16, response.Payload.Length)).CopyTo(array.AsSpan(0, Math.Min(16, response.Payload.Length)));
						Console.WriteLine(text + array.ToHexString());
						bool flag = true;
						for (int j = 0; j < 16; j++)
						{
							if (array[j] != 0)
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							break;
						}
						continue;
					}
					return false;
				}
				return false;
			}
			Keys = GetBootKey(array);
		}
		catch
		{
			return (byte)result != 0;
		}
		return (byte)result2 != 0;
	}

	public byte[] BuildPacket(uint addr, byte[] payload)
	{
		if (payload == null)
		{
			return BuildPacket(addr, 0u);
		}
		return BuildPacket(addr, (uint)payload.Length);
	}

	public byte[] BuildPacket(uint addr, uint size)
	{
		byte[] array = new byte[8];
		Binary.WriteU32BE(array, 0, addr);
		Binary.WriteU32BE(array, 4, size);
		return array;
	}

	private int GetChunk()
	{
		if (CurrentStage != Stages.FDL1)
		{
			return 528;
		}
		return 2112;
	}

	public bool Shutdown()
	{
		object payload = null;
		return SprdMain.SendCommandAndACK(SCMD.NORMAL_RESET, (byte[])payload);
	}

	public (byte[], Gpt) GetGPT()
	{
		string partName = "user_partition";
		object obj = null;
		if (CheckPartitionExist(partName))
		{
			byte[] array = ReadFlashBytes(partName, 32768L);
			if (array == null || array.Length == 0)
			{
				return partition;
			}
			Gpt gpt = new Gpt();
			if (gpt.Parse(array, Storage.BlockSize))
			{
				partition = (array, gpt);
				var (array2, gpt2) = partition;
				if (array2 != null)
				{
					return partition;
				}
				if (gpt2 != obj)
				{
					return partition;
				}
				return ((byte[])obj, (Gpt)obj);
			}
			return ((byte[])obj, (Gpt)obj);
		}
		return ((byte[])obj, (Gpt)obj);
	}

	public bool CheckPartitionExist(string partName)
	{
		object payload = null;
		if (!SprdMain.SendCommandAndACK(SCMD.READ_START, CreateSelectPartitionRequest(partName, 8L)))
		{
			SprdMain.SendCommandAndGetResponse(SCMD.READ_END, (byte[])payload);
			return false;
		}
		uint[] array = new uint[2] { 8u, 0u };
		byte[] array2 = new byte[8];
		MemoryMarshal.AsBytes(array.AsSpan()).Slice(0, 8).CopyTo(array2.AsSpan(0, 8));
		SprdMain.Response response = SprdMain.SendCommandAndGetResponse(SCMD.READ_MIDST, array2);
		SprdMain.SendCommandAndACK(SCMD.READ_END, (byte[])payload);
		return response.Type == 147;
	}

	private byte[] CreateSelectPartitionRequest(string partName, long size)
	{
		bool flag;
		byte[] array;
		if (!(flag = size > 4294967295L))
		{
			array = new byte[76];
			Encoding.Unicode.GetBytes(partName).CopyTo(array, 0);
			Binary.WriteU32LE(array, 72, (uint)size);
			if (flag)
			{
				Binary.WriteU32LE(array, 76, (uint)((size >> 32) & 0xFFFFFFFFL));
				return array;
			}
			return array;
		}
		array = new byte[80];
		Encoding.Unicode.GetBytes(partName).CopyTo(array, 0);
		Binary.WriteU32LE(array, 72, (uint)size);
		if (flag)
		{
			Binary.WriteU32LE(array, 76, (uint)((size >> 32) & 0xFFFFFFFFL));
			return array;
		}
		return array;
	}

	private byte[] CreateReadPartitionRequest(uint readSize, long readOffset, bool useMode64)
	{
		byte[] array = ((!useMode64) ? new byte[8] : new byte[12]);
		Binary.WriteU32LE(array, 0, readSize);
		Binary.WriteU32LE(array, 4, (uint)readOffset);
		if (!useMode64)
		{
			return array;
		}
		Binary.WriteU32LE(array, 8, (uint)((readOffset >> 32) & 0xFFFFFFFFL));
		return array;
	}

	public bool ReadFlash(string partName, string filePath, long size, long offset = 0L)
	{
		string text = "Reading ";
		string text2 = "splloader";
		string text3 = "ubipac";
		string text4 = "uboot";
		string text5 = "sml";
		string text6 = "trustos";
		int result = 1;
		string value = "fixnv1";
		double field_ = Class38.field_543;
		int cmd = 18;
		object payload = null;
		Console.WriteLine(text + partName);
		bool useMode = size > 4294967295L;
		int perBlockSize = PerBlockSize;
		if (partName == text2 || partName == text3 || partName == text4 || partName == text5 || partName == text6)
		{
			PerBlockSize = 4096;
		}
		if (partName.Contains(value))
		{
			partName = partName.Replace('1', '2');
		}
		int timeout = SerialSPD.GetTimeout();
		SerialSPD.TimeoutSet(150000);
		if (!SprdMain.SendCommandAndACK(SCMD.READ_START, CreateSelectPartitionRequest(partName, size)))
		{
			return false;
		}
		uint perBlockSize2 = (uint)PerBlockSize;
		long num = 0L;
		long num2 = 0L;
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			using FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
			while (num < size)
			{
				uint num3 = (uint)Math.Min(perBlockSize2, size - num);
				byte[] payload2 = CreateReadPartitionRequest(num3, offset + num, useMode);
				SprdMain.Response response = SprdMain.SendCommandAndGetResponse(SCMD.READ_MIDST, payload2);
				if (response != null && response.Payload != null && response.Type == 147)
				{
					int num4 = (int)Math.Min((uint)response.Payload.Length, num3);
					if (num4 > 0)
					{
						fileStream.Write(response.Payload, 0, num4);
					}
					num += num3;
					num2 += num4;
					UI.Progresbar1(num2, size);
					double totalSeconds = stopwatch.Elapsed.TotalSeconds;
					if (totalSeconds > field_)
					{
						UI.LabelWrite(utils.FileSize((long)((double)num2 / totalSeconds)), utils.FileSize(num2), utils.FileSize(size));
					}
					continue;
				}
				return false;
			}
		}
		finally
		{
			SprdMain.SendCommandAndACK((SCMD)cmd, (byte[])payload);
			PerBlockSize = perBlockSize;
			SerialSPD.TimeoutSet(timeout);
		}
		return (byte)result != 0;
	}

	public byte[] ReadFlashBytes(string partName, long size, long offset = 0L)
	{
		string text = "splloader";
		string text2 = "ubipac";
		string text3 = "uboot";
		string text4 = "sml";
		string text5 = "trustos";
		string value = "fixnv1";
		object obj = null;
		double field_ = Class38.field_543;
		int cmd = 18;
		bool useMode = size > 4294967295L;
		int perBlockSize = PerBlockSize;
		if (partName == text || partName == text2 || partName == text3 || partName == text4 || partName == text5)
		{
			PerBlockSize = 4096;
		}
		if (partName.Contains(value))
		{
			partName = partName.Replace('1', '2');
		}
		int timeout = SerialSPD.GetTimeout();
		SerialSPD.TimeoutSet(150000);
		if (SprdMain.SendCommandAndACK(SCMD.READ_START, CreateSelectPartitionRequest(partName, size)))
		{
			uint perBlockSize2 = (uint)PerBlockSize;
			long num = 0L;
			using MemoryStream memoryStream = new MemoryStream((int)Math.Min(size, 2147483647L));
			long num2 = 0L;
			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				while (num < size)
				{
					uint num3 = (uint)Math.Min(perBlockSize2, size - num);
					byte[] payload = CreateReadPartitionRequest(num3, offset + num, useMode);
					SprdMain.Response response = SprdMain.SendCommandAndGetResponse(SCMD.READ_MIDST, payload);
					if (response != null && response.Payload != null && response.Type == 147)
					{
						int num4 = (int)Math.Min((uint)response.Payload.Length, num3);
						if (num4 > 0)
						{
							memoryStream.Write(response.Payload, 0, num4);
						}
						num += num3;
						num2 += num4;
						UI.Progresbar1(num2, size);
						double totalSeconds = stopwatch.Elapsed.TotalSeconds;
						if (totalSeconds > field_)
						{
							UI.LabelWrite(utils.FileSize((long)((double)num2 / totalSeconds)), utils.FileSize(num2), utils.FileSize(size));
						}
						continue;
					}
					return (byte[])obj;
				}
			}
			finally
			{
				SprdMain.SendCommandAndACK((SCMD)cmd, (byte[])obj);
				PerBlockSize = perBlockSize;
				SerialSPD.TimeoutSet(timeout);
			}
			stopwatch.Stop();
			return memoryStream.ToArray();
		}
		return (byte[])obj;
	}

	public bool FormatFlash(string partName, int timeout = 40000)
	{
		string value = "fixnv1";
		int timeout2 = SerialSPD.GetTimeout();
		SerialSPD.TimeoutSet(timeout);
		try
		{
			if (partName.Contains(value))
			{
				partName = partName.Replace('1', '2');
			}
			return SprdMain.SendCommandAndACK(SCMD.ERASE_FLASH, CreateSelectPartitionRequest(partName, 0L));
		}
		finally
		{
			SerialSPD.TimeoutSet(timeout2);
		}
	}

	public bool WriteFlash(string partName, object data, long size)
	{
		string text = "Write ";
		string value = "runtimenv";
		string value2 = "calinv";
		int result = 1;
		string value3 = "fixnv1";
		object obj = null;
		string text2 = "Start Write ";
		long field_ = Class38.field_3544;
		double field_2 = Class38.field_543;
		Console.WriteLine(text + partName);
		if (CheckPartitionExist(partName))
		{
			if (!partName.Contains(value))
			{
				if (!partName.Contains(value2))
				{
					if (partName.Contains(value3))
					{
						using (Stream stream = OpenDataStream(data))
						{
							if (stream != obj)
							{
								return WriteFixNv(partName, stream);
							}
							return false;
						}
					}
					Console.WriteLine(text2 + partName);
					using Stream stream2 = OpenDataStream(data);
					if (stream2 == obj)
					{
						return false;
					}
					long num = ((size > 0L) ? size : stream2.Length);
					if (num == 0L)
					{
						return false;
					}
					if (stream2.CanSeek)
					{
						stream2.Position = 0L;
					}
					byte[] array = new byte[PerBlockSize];
					long num2 = 0L;
					long num3 = 0L;
					Stopwatch stopwatch = Stopwatch.StartNew();
					int timeout = SerialSPD.GetTimeout();
					SerialSPD.TimeoutSet(150000);
					try
					{
						if (SprdMain.UseTranscode && num > 1048576L)
						{
							num = num / 1024L * 1024L;
						}
						if (!SprdMain.SendCommandAndACK(SCMD.START_DATA, CreateSelectPartitionRequest(partName, num)))
						{
							return false;
						}
						while (num2 < num)
						{
							int length = (int)Math.Min(array.Length, num - num2);
							int num4 = ReadExactStream(stream2, array, length);
							if (num4 <= 0)
							{
								return false;
							}
							byte[] array2 = new byte[num4];
							array.AsSpan(0, num4).CopyTo(array2.AsSpan(0, num4));
							if (SprdMain.SendCommandAndACK(SCMD.MIDST_DATA, array2))
							{
								num2 += num4;
								num3 += num4;
								UI.Progresbar1(num3, Math.Min(num, field_));
								double totalSeconds = stopwatch.Elapsed.TotalSeconds;
								if (totalSeconds > field_2)
								{
									UI.LabelWrite(utils.FileSize((long)((double)num3 / totalSeconds)), utils.FileSize(num3), utils.FileSize(num));
								}
								continue;
							}
							return false;
						}
						if (!SprdMain.SendCommandAndACK(SCMD.END_DATA, (byte[])obj))
						{
							return false;
						}
					}
					finally
					{
						SerialSPD.TimeoutSet(timeout);
					}
					return (byte)result != 0;
				}
				return true;
			}
			return FormatFlash(partName);
		}
		return false;
	}

	private Stream OpenDataStream(object data)
	{
		object obj = null;
		string text = data as string;
		if (text !=null)
		{
			return new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		byte[] array = data as byte[];
		if (array ==null)
		{
			Stream stream = data as Stream;
			if (stream ==null)
			{
				return (Stream)obj;
			}
			return stream;
		}
		return new MemoryStream(array, writable: false);
	}

	private bool WriteFixNv(string partName, Stream data)
	{
		object payload = null;
		double field_ = Class38.field_543;
		byte[] array = new byte[data.Length];
		ReadExactStream(data, array, array.Length);
		uint num = 0u;
		uint num2 = 4u;
		if (array.Length >= 4 && Binary.ReadU32LE(array) == 20054)
		{
			num = 512u;
		}
		ushort[] array2 = new ushort[2];
		while (true)
		{
			array2[0] = Binary.ReadU16LE(array, (int)(num + num2));
			array2[1] = Binary.ReadU16LE(array, (int)(num + num2 + 2));
			if (array2[1] == 0)
			{
				break;
			}
			num2 += (uint)(array2[1] + 4);
			uint num3 = ((num2 + 3) & 0xFFFFFFFCu) - num2;
			num2 += num3;
			array2[0] = Binary.ReadU16LE(array, (int)(num + num2));
			if (array2[0] != ushort.MaxValue)
			{
				continue;
			}
			num2 += 8;
			byte[] array3 = new byte[num2 - 2];
			array.AsSpan((int)(num + 2), array3.Length).CopyTo(array3);
			ushort value = SprdChecksum.CalculateCrc16Nv(array3);
			byte[] bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			bytes.AsSpan().CopyTo(array.AsSpan((int)num));
			byte[] array4 = new byte[80];
			Encoding.Unicode.GetBytes(partName).AsSpan().CopyTo(array4.AsSpan(0, partName.Length * 2));
			BitConverter.GetBytes(num2).AsSpan().CopyTo(array4.AsSpan(72));
			uint num4 = 0u;
			for (int i = (int)num; i < num + num2; i++)
			{
				num4 += array[i];
			}
			BitConverter.GetBytes(num4).AsSpan().CopyTo(array4.AsSpan(76));
			if (SprdMain.SendCommandAndACK(SCMD.START_DATA, array4))
			{
				uint num5 = 0u;
				Stopwatch stopwatch = Stopwatch.StartNew();
				while (num5 < num2)
				{
					uint num6 = (uint)Math.Min(PerBlockSize, (int)(num2 - num5));
					byte[] array5 = new byte[num6];
					array.AsSpan((int)num5, (int)num6).CopyTo(array5.AsSpan(0, (int)num6));
					if (!SprdMain.SendCommandAndACK(SCMD.MIDST_DATA, array5))
					{
						SprdMain.SendCommandAndACK(SCMD.END_DATA, (byte[])payload);
						UI.Progresbar1(1L, 1L);
						return false;
					}
					num5 += num6;
					UI.Progresbar1(num5, num2);
					double totalSeconds = stopwatch.Elapsed.TotalSeconds;
					if (totalSeconds > field_)
					{
						UI.LabelWrite(utils.FileSize((long)((double)num5 / totalSeconds)), utils.FileSize(num5), utils.FileSize(num2));
					}
				}
				if (!SprdMain.SendCommandAndACK(SCMD.END_DATA, (byte[])payload))
				{
					return false;
				}
				return true;
			}
			return false;
		}
		return false;
	}

	private bool ReadPartitionCustomize(Stream dest, string partName, long size)
	{
		int num = 0;
		int result = 1;
		string text = "splloader";
		string text2 = "ubipac";
		string text3 = "uboot";
		string text4 = "sml";
		string text5 = "trustos";
		string value = "fixnv1";
		int cmd = 18;
		object payload = null;
		if (dest != null && size != 0L)
		{
			bool useMode = size > 4294967295L;
			int perBlockSize = PerBlockSize;
			if (partName == text || partName == text2 || partName == text3 || partName == text4 || partName == text5)
			{
				PerBlockSize = 4096;
			}
			if (partName.Contains(value))
			{
				partName = partName.Replace('1', '2');
			}
			int timeout = SerialSPD.GetTimeout();
			SerialSPD.TimeoutSet(150000);
			if (!SprdMain.SendCommandAndACK(SCMD.READ_START, CreateSelectPartitionRequest(partName, size)))
			{
				return false;
			}
			uint perBlockSize2 = (uint)PerBlockSize;
			long num2 = 0L;
			try
			{
				uint num3;
				for (; num2 < size; num2 += num3)
				{
					num3 = (uint)Math.Min(perBlockSize2, size - num2);
					byte[] payload2 = CreateReadPartitionRequest(num3, num2, useMode);
					SprdMain.Response response = SprdMain.SendCommandAndGetResponse(SCMD.READ_MIDST, payload2);
					if (response != null && response.Payload != null && response.Type == 147)
					{
						int num4 = (int)Math.Min((uint)response.Payload.Length, num3);
						if (num4 > 0)
						{
							dest.Write(response.Payload, 0, num4);
						}
						continue;
					}
					return false;
				}
			}
			finally
			{
				SprdMain.SendCommandAndACK((SCMD)cmd, (byte[])payload);
				PerBlockSize = perBlockSize;
				SerialSPD.TimeoutSet(timeout);
			}
			dest.Position = num;
			return (byte)result != 0;
		}
		return false;
	}

	private static int ReadExactStream(Stream stream, byte[] buffer, int length)
	{
		int num = 0;
		int num2;
		if (0 >= length)
		{
			num2 = 0;
			return 0;
		}
		int num3 = stream.Read(buffer, 0, length);
		if (num3 > 0)
		{
			num = 0 + num3;
			int num4 = 582928464;
			while (true)
			{
				_ = num4 ^ 0x4D93122F;
				if (num >= length)
				{
					return num;
				}
				num3 = stream.Read(buffer, num, length - num);
				if (num3 <= 0)
				{
					break;
				}
				num += num3;
				num4 = 582928464;
			}
			return num;
		}
		num2 = 0;
		return 0;
	}

	private uint GetKey(int order, uint a, uint b = 0u)
	{
		uint[] array = new uint[6] { 2346725359u, 1948241936u, 3123532032u, 3245164016u, 1660447998u, 1171435263u };
		switch (order)
		{
		default:
		{
			uint num = 0u;
			return 0u;
		}
		case 0:
			return ~(~(a + b) & array[0]) & ~((a + b) & array[1]);
		case 1:
			return ~(~(~a) | array[2]);
		case 2:
			return ~((~(~a) | b) & (~a | ~b));
		case 3:
			return ~(~(~a + array[3]) & array[4]) & ~(~(~(~a + array[3]) | array[4]));
		case 4:
			return a | ~(~b | array[5]);
		}
	}

	private byte GetByte(uint value, int order)
	{
		return (byte)((value >> order * 8) & 0xFF);
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
