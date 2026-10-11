#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AMFTool.Library.Mediatek.Auth;
using AMFTool.Library.Mediatek.Config;
using AMFTool.Library.Usb;

namespace AMFTool.Library.Mediatek;

public class MtkPreloader
{
	public enum Cmd
	{
		SEND_PARTITION_DATA = 112,
		JUMP_TO_PARTITION = 113,
		CHECK_USB_CMD = 114,
		STAY_STILL = 128,
		CMD_88 = 136,
		CMD_READ16_A2 = 162,
		I2C_INIT = 176,
		I2C_DEINIT = 177,
		I2C_WRITE8 = 178,
		I2C_READ8 = 179,
		I2C_SET_SPEED = 180,
		I2C_INIT_EX = 182,
		I2C_DEINIT_EX = 183,
		I2C_WRITE8_EX = 184,
		I2C_READ8_EX = 185,
		I2C_SET_SPEED_EX = 186,
		GET_MAUI_FW_VER = 191,
		OLD_SLA_SEND_AUTH = 193,
		OLD_SLA_GET_RN = 194,
		OLD_SLA_VERIFY_RN = 195,
		PWR_INIT = 196,
		PWR_DEINIT = 197,
		PWR_READ16 = 198,
		PWR_WRITE16 = 199,
		CMD_C8 = 200,
		READ16 = 208,
		READ32 = 209,
		WRITE16 = 210,
		WRITE16_NO_ECHO = 211,
		WRITE32 = 212,
		JUMP_DA = 213,
		JUMP_BL = 214,
		SEND_DA = 215,
		GET_TARGET_CONFIG = 216,
		SEND_ENV_PREPARE = 217,
		brom_register_access = 218,
		UART1_LOG_EN = 219,
		UART1_SET_BAUDRATE = 220,
		BROM_DEBUGLOG = 221,
		JUMP_DA64 = 222,
		GET_BROM_LOG_NEW = 223,
		SEND_CERT = 224,
		GET_ME_ID = 225,
		SEND_AUTH = 226,
		SLA = 227,
		CMD_E4 = 228,
		CMD_E5 = 229,
		CMD_E6 = 230,
		GET_SOC_ID = 231,
		CMD_E8 = 232,
		ZEROIZATION = 240,
		GET_PL_CAP = 251,
		CMD_FA = 250,
		GET_HW_SW_VER = 252,
		GET_HW_CODE = 253,
		GET_BL_VER = 254,
		GET_VERSION = 255,
		ASYNC_DA = 192
	}

	private const uint UsbdlBrom = 2u;

	private const uint UsbdlTimeoutMax = 16383u;

	private const int MiscLockKeyMagic = 44440;

	public MtkConfig cfg;

	public MtkPreloader(MtkConfig cfg)
	{
		this.cfg = cfg;
	}

	public bool Init(bool display = true, bool meta = false)
	{
		string msg = "Sync error. Please power off the device and retry.";
		string message = "Disabling Watchdog... ";
		object obj = null;
		string msg2 = "Rebooting to META Mode...";
		string text = "X4";
		double field_ = Class38.field_7761;
		double field_2 = Class38.field_203;
		string msg3 = "OK";
		string msg4 = "Need Authorized!";
		string msg5 = "Requesting auth data... ";
		string text2 = "Mediatek/";
		string text3 = "/";
		string text4 = ".auth";
		string msg6 = "Failed!";
		string value = "Auth File is required.";
		if (SerialMTK.Echo(253) || SerialMTK.Echo(253))
		{
			byte[] array = SerialMTK.Read(4);
			cfg.HWCode = Binary.ReadU16BE(array);
			cfg.HWVer = Binary.ReadU16BE(array, 2);
			if (cfg.HWCode != 0)
			{
				cfg.InitHwCode(cfg.HWCode);
				cfg.Cpu = cfg.Chipcfg.Name;
				PrintBromConfig();
				Debug.WriteLine(message);
				DisableWatchDogTimer((int)cfg.Chipcfg.Watchdog);
				cfg.Targetconfig = GetTargetConfig(display);
				GetBLVersion();
				GetBromVersion();
				cfg.Meid = (byte[])obj;
				if (GetHWSWVersion())
				{
					GetMeid();
					GetSocId();
					PrintHwInfo();
					if (!meta)
					{
						if (MTKConfig.Needauthfile && (cfg.Targetconfig.Sla || cfg.Targetconfig.Daa))
						{
							UI.RichLogs(msg4, UI.error, nextline: true);
							UI.RichLogs(msg5, UI.log);
							cfg.AuthData = Server.Downloadbyte(text2 + DeviceState.Brand + text3 + DeviceState.Model + text4);
							if (cfg.AuthData == null || cfg.AuthData.Length == 0)
							{
								UI.RichLogs(msg6, UI.error, nextline: true);
								return false;
							}
							UI.RichLogs(msg3, UI.ok, nextline: true, bold: true);
						}
						if (cfg.AuthData != null && cfg.IsBROM && cfg.Targetconfig.Daa)
						{
							SendAuth(cfg.AuthData);
						}
						else if (cfg.IsBROM && cfg.Targetconfig.Daa)
						{
							Console.WriteLine(value);
						}
						if (cfg.CertData != null && cfg.IsBROM && cfg.Targetconfig.Daa)
						{
							SendRootCert(cfg.CertData);
						}
						if (cfg.Targetconfig.Sla && cfg.Chipcfg.Damode == (DAmodes)6)
						{
							HandleSLA(cfg.IsBROM);
						}
						return true;
					}
					UI.RichLogs(msg2, UI.log);
					(uint, uint) watchdogAddr = cfg.GetWatchdogAddr();
					var (num, _) = watchdogAddr;
					_ = watchdogAddr.Item2;
					Console.WriteLine(num.ToString(text));
					BromRegisterAccess(0, 1, new byte[1] { 1 }, checkStatus: true, 3);
					BromRegisterAccess(0, 1, (byte[])obj, checkStatus: true, 2);
					utils.Pause(field_);
					Write32((int)(num + 20), new List<int> { 4617 });
					utils.Pause(field_2);
					UI.RichLogs(msg3, UI.ok, nextline: true);
					return true;
				}
				return false;
			}
			return false;
		}
		UI.RichLogs(msg, UI.error, nextline: true);
		return false;
	}

	public void PrintHwInfo()
	{
		string msg = "CPU Model   : ";
		int num = 0;
		string text = "";
		string oldValue = "/";
		string newValue = "_";
		int num2 = 1;
		string format = "Unknown 0x{0:X}";
		string msg2 = "Unknown";
		string msg3 = "Chipset     : ";
		int num3 = 5;
		string format2 = "{0}";
		int num4 = 6;
		string format3 = "0x{0}";
		string cpuType = "5";
		string cpuType2 = "6";
		string cpuType3 = "3";
		string text2 = "Chipset Type = ";
		string msg4 = "HW Code     : ";
		string format4 = "0x{0:x}";
		string msg5 = "HW Version  : ";
		string msg6 = "SW Version  : ";
		string format5 = "0x{0:x2}";
		object obj = null;
		string msg7 = "MEID        : ";
		string msg8 = "SOCID[0]    : ";
		int num5 = 32;
		string msg9 = "SOCID[1]    : ";
		try
		{
			UI.RichLogs(msg, UI.log);
			if (!(cfg.Chipcfg.Description == text) || !(cfg.Chipcfg.Name != text))
			{
				if (!(cfg.Chipcfg.Description != text) || !(cfg.Chipcfg.Name != text))
				{
					UI.RichLogs(string.Format(format, cfg.HWCode), UI.info, nextline: true);
				}
				else
				{
					UI.RichLogs(cfg.Chipcfg.Name.Replace(oldValue, newValue) ?? text, UI.info, nextline: true);
				}
			}
			else
			{
				UI.RichLogs(cfg.Chipcfg.Name.Replace(oldValue, newValue) ?? text, UI.info, nextline: true);
			}
		}
		catch
		{
			UI.RichLogs(msg2, UI.info, (byte)num2 != 0, (byte)num != 0);
		}
		UI.RichLogs(msg3, UI.log, (byte)num != 0, (byte)num != 0);
		if (cfg.Chipcfg.Damode != (DAmodes)num3)
		{
			if (cfg.Chipcfg.Damode != (DAmodes)num4)
			{
				UI.RichLogs(string.Format(format2, cfg.Chipcfg.Damode), UI.info, (byte)num2 != 0, (byte)num2 != 0);
			}
			else
			{
				UI.RichLogs(string.Format(format3, cfg.Chipcfg.Damode), UI.info, (byte)num2 != 0, (byte)num2 != 0);
			}
		}
		else
		{
			UI.RichLogs(string.Format(format2, cfg.Chipcfg.Damode), UI.info, (byte)num2 != 0, (byte)num2 != 0);
		}
		if (cfg.Chipcfg.Damode == (DAmodes)num3)
		{
			MTKConfig.CpuType = cpuType;
		}
		else if (cfg.Chipcfg.Damode != (DAmodes)num4)
		{
			MTKConfig.CpuType = cpuType3;
		}
		else
		{
			MTKConfig.CpuType = cpuType2;
		}
		Console.WriteLine(text2 + MTKConfig.CpuType);
		UI.RichLogs(msg4, UI.log, (byte)num != 0, (byte)num != 0);
		UI.RichLogs(string.Format(format4, cfg.HWCode), UI.info, (byte)num2 != 0, (byte)num != 0);
		UI.RichLogs(msg5, UI.log, (byte)num != 0, (byte)num != 0);
		UI.RichLogs(string.Format(format4, cfg.HWVer), UI.info, (byte)num2 != 0, (byte)num != 0);
		UI.RichLogs(msg6, UI.log, (byte)num != 0, (byte)num != 0);
		UI.RichLogs(string.Format(format5, cfg.SWVer), UI.info, (byte)num2 != 0, (byte)num != 0);
		if (cfg.Meid !=null)
		{
			UI.RichLogs(msg7, UI.log, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(cfg.Meid.ToHexString().ToUpper(), UI.info, (byte)num2 != 0, (byte)num != 0);
		}
		if (cfg.SocID !=null)
		{
			UI.RichLogs(msg8, UI.log, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(cfg.SocID.ToHexString().Substring(num, num5).ToUpper(), UI.info, (byte)num2 != 0, (byte)num != 0);
			UI.RichLogs(msg9, UI.log, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(cfg.SocID.ToHexString().Substring(cfg.SocID.ToHexString().Length - num5, num5).ToUpper(), UI.info, (byte)num2 != 0, (byte)num != 0);
		}
		UI.RichLogs(text, UI.log, (byte)num2 != 0, (byte)num != 0);
	}

	public void PrintBromConfig()
	{
		string text = "CPU: ";
		string text2 = " (";
		string text3 = ")";
		string format = "HW version: 0x{0:X}";
		string format2 = "WDT: 0x{0:X}";
		string format3 = "Uart: 0x{0:X}";
		string format4 = "Brom payload addr: 0x{0:X}";
		string format5 = "DA payload addr: 0x{0:X}";
		object obj = null;
		string format6 = "CQ_DMA addr: 0x{0:X}";
		string format7 = "Var1: 0x{0:X}";
		Console.WriteLine(text + cfg.Cpu + text2 + cfg.Chipcfg.Description + text3);
		Console.WriteLine(string.Format(format, cfg.HWVer));
		Console.WriteLine(string.Format(format2, cfg.Chipcfg.Watchdog));
		Console.WriteLine(string.Format(format3, cfg.Chipcfg.Uart));
		Console.WriteLine(string.Format(format4, cfg.Chipcfg.BromPayloadAddr));
		Console.WriteLine(string.Format(format5, cfg.Chipcfg.DaPayloadAddr));
		if (cfg.Chipcfg.CqdmaBase !=null)
		{
			Console.WriteLine(string.Format(format6, cfg.Chipcfg.CqdmaBase));
			Console.WriteLine(string.Format(format7, cfg.Chipcfg.Var1));
		}
		else
		{
			Console.WriteLine(string.Format(format7, cfg.Chipcfg.Var1));
		}
	}

	public List<int> Read(int addr, int dwords = 1, int length = 32)
	{
		object obj = null;
		List<int> list = new List<int>();
		Cmd cmd = ((length == 16) ? Cmd.READ16 : Cmd.READ32);
		if (SerialMTK.Echo((int)cmd))
		{
			if (SerialMTK.Echo(addr, 4))
			{
				if (!SerialMTK.Echo(dwords, 4))
				{
					return (List<int>)obj;
				}
				byte[] array = SerialMTK.Read(2);
				if (array != null && array.Length == 2)
				{
					if (Binary.ReadU16BE(array) > 255)
					{
						return (List<int>)obj;
					}
					int num = 0;
					while (true)
					{
						if (num >= dwords)
						{
							array = SerialMTK.Read(2);
							if (array != null && array.Length == 2)
							{
								if (Binary.ReadU16BE(array) <= 255)
								{
									break;
								}
								return (List<int>)obj;
							}
							return (List<int>)obj;
						}
						byte[] array2 = SerialMTK.Read((length == 32) ? 4 : 2);
						if (array2 != obj)
						{
							if (length != 32)
							{
								list.Add(Binary.ReadU16BE(array2));
							}
							else
							{
								list.Add(Binary.ReadI32BE(array2));
							}
							num++;
							continue;
						}
						return (List<int>)obj;
					}
					return list;
				}
				return (List<int>)obj;
			}
			return (List<int>)obj;
		}
		return (List<int>)obj;
	}

	public List<int> Read32(object addr, int dwords = 1)
	{
		return Read((int)addr, dwords);
	}

	public List<int> Read16(object addr, int dwords = 1)
	{
		return Read((int)addr, dwords, 16);
	}

	private bool Write(int addr, List<int> values, int length = 32)
	{
		int num = 1;
		int num2 = 0;
		string format = "Error on da_write{0}, addr 0x{1:X}, send cmd";
		string format2 = "Error on da_write{0}, addr {1:X}, write address";
		int num3 = 2;
		int num4 = 255;
		string format3 = "Error on da_write {0}, addr=0x{1:X}, {2}";
		string format4 = "Error on da_write{0}, addr {1:X}, {2}";
		Cmd cmd = ((length == 16) ? Cmd.WRITE16 : Cmd.WRITE32);
		if (SerialMTK.Echo((int)cmd))
		{
			if (SerialMTK.Echo(addr, 4))
			{
				bool flag = SerialMTK.Echo(values.Count, 4);
				byte[] array = SerialMTK.Read(2);
				if (array == null || array.Length != 2)
				{
					return false;
				}
				ushort num5 = Binary.ReadU16BE(array);
				if (num5 <= 255)
				{
					if (flag && num5 <= 3)
					{
						foreach (int value in values)
						{
							int size = ((length == 16) ? 2 : 4);
							if (SerialMTK.Echo(value, size))
							{
								continue;
							}
							break;
						}
						array = SerialMTK.Read(num3);
						if ((array != null) ? (((array.Length == num3) ? 1 : 0) == num2) : ((byte)num != 0))
						{
							return (byte)num2 != 0;
						}
						num5 = Binary.ReadU16BE(array, num2);
						if (((num5 > num4) ? 1 : 0) == num2)
						{
							return (byte)num != 0;
						}
						UI.RichLogs(string.Format(format4, length, addr, Error.status(num5)), UI.error, (byte)num != 0, (byte)num2 != 0);
					}
					return (byte)num2 != 0;
				}
				UI.RichLogs(string.Format(format3, length, addr, Error.status(num5)), UI.error, nextline: true);
				return false;
			}
			UI.RichLogs(string.Format(format2, length, addr), UI.error, nextline: true);
			return false;
		}
		UI.RichLogs(string.Format(format, length, addr), UI.error, nextline: true);
		return false;
	}

	public bool Write32(object addr, List<int> dwords)
	{
		return Write((int)addr, dwords);
	}

	public bool Write16(object addr, List<int> dwords)
	{
		return Write((int)addr, dwords, 16);
	}

	public void ResetToBrom(bool en = true, int timeout = 0)
	{
		object obj = null;
		if (cfg.Chipcfg.MiscLock == obj)
		{
			return;
		}
		uint num = 0u;
		if (timeout == 0)
		{
			uint num2 = 16383u;
			num2 = 65532u;
			num2 = 65532u;
			num = 65532u;
			if (en)
			{
				num = 65533u;
				num = 65533u;
				num = 1145896957u;
				int num3 = Convert.ToInt32(cfg.Chipcfg.MiscLock);
				int num4 = num3 + 8;
				int num5 = num3 - 32;
				Write32(num3, new List<int> { 44440 });
				Write32(num4, new List<int> { 1 });
				Write32(num3, new List<int> { 0 });
				Write32(num5, new List<int> { 1145896957 });
			}
			else
			{
				num = 65532u;
				num = 65532u;
				num = 1145896956u;
				int num3 = Convert.ToInt32(cfg.Chipcfg.MiscLock);
				int num4 = num3 + 8;
				int num5 = num3 - 32;
				Write32(num3, new List<int> { 44440 });
				Write32(num4, new List<int> { 1 });
				Write32(num3, new List<int> { 0 });
				Write32(num5, new List<int> { 1145896956 });
			}
		}
		else
		{
			uint num2 = (uint)Math.Max(timeout / 1000, 0);
			num2 <<= 2;
			num2 &= 0xFFFC;
			num = 0 | num2;
			if (en)
			{
				num |= 1;
				num &= 0xFFFFFFFDu;
				num |= 0x444C0000;
				int num3 = Convert.ToInt32(cfg.Chipcfg.MiscLock);
				int num4 = num3 + 8;
				int num5 = num3 - 32;
				Write32(num3, new List<int> { 44440 });
				Write32(num4, new List<int> { 1 });
				Write32(num3, new List<int> { 0 });
				Write32(num5, new List<int> { (int)num });
			}
			else
			{
				num &= 0xFFFFFFFEu;
				num &= 0xFFFFFFFDu;
				num |= 0x444C0000;
				int num3 = Convert.ToInt32(cfg.Chipcfg.MiscLock);
				int num4 = num3 + 8;
				int num5 = num3 - 32;
				Write32(num3, new List<int> { 44440 });
				Write32(num4, new List<int> { 1 });
				Write32(num3, new List<int> { 0 });
				Write32(num5, new List<int> { (int)num });
			}
		}
	}

	public bool DisableWatchDogTimer(int hwcode)
	{
		var (obj, obj2) = cfg.GetWatchDogAddr(hwcode);
		return Write32((int)obj, new List<int> { (int)obj2 });
	}

	public void GetBromVersion()
	{
		if (!SerialMTK.Write(new byte[1] { 255 }))
		{
			cfg.BromVersion = -1;
			return;
		}
		byte[] array = SerialMTK.Read(1);
		if (array == null || array.Length == 0)
		{
			cfg.BromVersion = -1;
		}
		else
		{
			cfg.BromVersion = array[0];
		}
	}

	public void GetBLVersion()
	{
		string value = "BROM mode detected";
		if (!SerialMTK.Write(new byte[1] { 254 }))
		{
			cfg.IsBROM = false;
			cfg.BlVer = -1;
			return;
		}
		byte[] array = SerialMTK.Read(1);
		bool flag;
		if (array != null)
		{
			if (array.Length != 0)
			{
				if (!(flag = array[0] == 254))
				{
					cfg.IsBROM = false;
					cfg.BlVer = -1;
				}
				else
				{
					Console.WriteLine(value);
					cfg.IsBROM = true;
					cfg.BlVer = array[0];
				}
				return;
			}
			flag = false;
		}
		else
		{
			flag = false;
		}
		if (!flag)
		{
			cfg.IsBROM = false;
			cfg.BlVer = -1;
		}
		else
		{
			Console.WriteLine(value);
			cfg.IsBROM = true;
			cfg.BlVer = array[0];
		}
	}

	public MtkConfig.TargetConfig GetTargetConfig(bool display = true)
	{
		string msg = "CMD Get_Target_Config not supported.";
		string message = "Get Target Config Error";
		string msg2 = "";
		string msg3 = "[Target Config Information]";
		string msg4 = "SBC Enabled : ";
		string format = "{0}";
		string msg5 = "SLA Enabled : ";
		string msg6 = "DAA Enabled : ";
		string format2 = "Target config: 0x{0:X}";
		string format3 = "SBC enabled: {0}";
		string format4 = "SLA enabled: {0}";
		string format5 = "DAA enabled: {0}";
		string format6 = "SWJTAG enabled: {0}";
		string format7 = "EPP_PARAM at 0x600 after EMMC_BOOT/SDMMC_BOOT: {0}";
		string format8 = "Root cert required: {0}";
		string format9 = "Mem read auth: {0}";
		string format10 = "Mem write auth: {0}";
		string format11 = "Cmd 0xC8 blocked: {0}";
		MtkConfig.TargetConfig result = new MtkConfig.TargetConfig
		{
			Sbc = false,
			Sla = false,
			Daa = false,
			SwJtag = false,
			Epp = false,
			Cert = false,
			MemRead = false,
			MemWrite = false,
			CmdC8 = false,
			RawValue = 0u
		};
		if (!SerialMTK.Echo(216))
		{
			UI.RichLogs(msg, UI.warning, nextline: true);
			return result;
		}
		byte[] array = SerialMTK.Read(4);
		if (array != null && array.Length == 4)
		{
			uint num = Binary.ReadU32BE(array);
			int num2 = Binary.ReadU16BE(SerialMTK.Read(2));
			if (num2 <= 255)
			{
				MtkConfig.TargetConfig targetConfig = new MtkConfig.TargetConfig
				{
					Sbc = ((num & 1) != 0),
					Sla = ((num & 2) != 0),
					Daa = ((num & 4) != 0),
					SwJtag = ((num & 6) != 0),
					Epp = ((num & 8) != 0),
					Cert = ((num & 0x10) != 0),
					MemRead = ((num & 0x20) != 0),
					MemWrite = ((num & 0x40) != 0),
					CmdC8 = ((num & 0x80) != 0),
					RawValue = num
				};
				if (display)
				{
					UI.RichLogs(msg2, UI.log, nextline: true);
					UI.RichLogs(msg3, UI.log, nextline: true);
					UI.RichLogs(msg4, UI.log);
					UI.RichLogs(string.Format(format, targetConfig.Sbc), targetConfig.Sbc ? UI.error : UI.ok, nextline: true, bold: true);
					UI.RichLogs(msg5, UI.log);
					UI.RichLogs(string.Format(format, targetConfig.Sla), targetConfig.Sla ? UI.error : UI.ok, nextline: true, bold: true);
					UI.RichLogs(msg6, UI.log);
					UI.RichLogs(string.Format(format, targetConfig.Daa), targetConfig.Daa ? UI.error : UI.ok, nextline: true, bold: true);
					Console.WriteLine(string.Format(format2, num));
					Console.WriteLine(string.Format(format3, targetConfig.Sbc));
					Console.WriteLine(string.Format(format4, targetConfig.Sla));
					Console.WriteLine(string.Format(format5, targetConfig.Daa));
					Console.WriteLine(string.Format(format6, targetConfig.SwJtag));
					Console.WriteLine(string.Format(format7, targetConfig.Epp));
					Console.WriteLine(string.Format(format8, targetConfig.Cert));
					Console.WriteLine(string.Format(format9, targetConfig.MemRead));
					Console.WriteLine(string.Format(format10, targetConfig.MemWrite));
					Console.WriteLine(string.Format(format11, targetConfig.CmdC8));
				}
				return targetConfig;
			}
			throw new Exception(message);
		}
		return result;
	}

	public bool JumpDA(int addr)
	{
		string format = "Jumping to 0x{0:X}";
		string format2 = "Jumping to 0x{0:X}: ok.";
		string text = "Jump_DA status error:";
		string value = "Jump_DA response address mismatch.";
		Console.WriteLine(string.Format(format, addr));
		if (SerialMTK.Echo(213))
		{
			byte[] array = new byte[4];
			Binary.WriteI32BE(array, 0, addr);
			if (SerialMTK.Write(array))
			{
				byte[] array2 = SerialMTK.Read(4);
				if (array2 != null && array2.Length == 4)
				{
					int num = (int)Binary.ReadU32BE(array2);
					if (num == addr)
					{
						array2 = SerialMTK.Read(2);
						if (array2 != null && array2.Length == 2)
						{
							ushort num2 = Binary.ReadU16BE(array2);
							Thread.Sleep(100);
							if (num2 != 0)
							{
								Console.WriteLine(text + Error.status(num2));
								return false;
							}
							Console.WriteLine(string.Format(format2, addr));
							return true;
						}
						return false;
					}
					Console.WriteLine(value);
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool SendRootCert(byte[] cert)
	{
		string value = "Error on uploading certificate.";
		string text = "Send cert error:";
		var (genChecksum, array) = PrepareData(Array.Empty<byte>(), cert, 0);
		if (!SerialMTK.Echo(224))
		{
			return false;
		}
		if (!SerialMTK.Echo(array.Length, 4))
		{
			return false;
		}
		byte[] array2 = SerialMTK.Read(2);
		if (array2 == null)
		{
			return false;
		}
		if (array2.Length == 2)
		{
			ushort num = Binary.ReadU16BE(array2);
			if (num > 255)
			{
				Console.WriteLine(text + Error.status(num));
				return false;
			}
			if (UploadData(cert, genChecksum, 0))
			{
				return true;
			}
			Console.WriteLine(value);
			return false;
		}
		return false;
	}

	public bool SendAuth(byte[] auth)
	{
		string value = "Uploading data.";
		string value2 = "Checksum of upload doesn't match !";
		string value3 = "Sending Auth Success";
		string value4 = "No auth needed.";
		string text = "Send auth error:";
		var (num, array) = PrepareData(auth, Array.Empty<byte>(), auth.Length);
		if (!SerialMTK.Echo(226))
		{
			return false;
		}
		int num2 = array.Length;
		byte[] array2 = new byte[4];
		Binary.WriteI32BE(array2, 0, num2);
		if (SerialMTK.Write(array2))
		{
			byte[] array3 = SerialMTK.Read(4);
			if (array3 != null && array3.Length == 4)
			{
				int num3 = (int)Binary.ReadU32BE(array3);
				if (num3 == num2)
				{
					Console.WriteLine(value);
					array3 = SerialMTK.Read(2);
					if (array3 != null && array3.Length == 2)
					{
						ushort num4 = Binary.ReadU16BE(array3);
						if (num4 >= 255)
						{
							if (num4 != 7436)
							{
								Console.WriteLine(text + Error.status(num4));
							}
							else
							{
								Console.WriteLine(value4);
							}
							return false;
						}
						int num5 = array.Length;
						int num6 = 0;
						byte[] array4 = new byte[SerialMTK.usb.max_to_write];
						while (true)
						{
							if (num5 <= 0)
							{
								SerialMTK.Write(Array.Empty<byte>());
								Thread.Sleep(35);
								array3 = SerialMTK.Read(2);
								if (array3 != null && array3.Length == 2)
								{
									ushort num7 = Binary.ReadU16BE(array3);
									if (num != num7 && num7 != 0)
									{
										Console.WriteLine(value2);
									}
									break;
								}
								return false;
							}
							int num8 = Math.Min(num5, SerialMTK.usb.max_to_write);
							array.AsSpan(num6, num8).CopyTo(array4.AsSpan(0, num8));
							if (SerialMTK.Write(array4.Take(num8).ToArray()))
							{
								num6 += num8;
								num5 -= num8;
								UI.Progresbar1(num6, array.Length);
								continue;
							}
							return false;
						}
						array3 = SerialMTK.Read(2);
						if (array3 != null && array3.Length == 2)
						{
							if (Binary.ReadU16BE(array3) != 0)
							{
								return false;
							}
							Console.WriteLine(value3);
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

	public bool HandleSLA(bool isBrom = true)
	{
		int result = 0;
		string text = "Send auth error:";
		string text2 = "Send sla challenge response len error:";
		try
		{
			if (isBrom)
			{
				foreach (SlaKeys brom_sla_key in SlaKeys.brom_sla_keys)
				{
					if (SerialMTK.Echo(227))
					{
						byte[] array = SerialMTK.Read(2);
						if (array == null || array.Length != 2)
						{
							return false;
						}
						ushort num = Binary.ReadU16BE(array);
						if (num != 28695)
						{
							if (num <= 255)
							{
								array = SerialMTK.Read(4);
								if (array != null && array.Length == 4)
								{
									uint num2 = Binary.ReadU32BE(array);
									byte[] array2 = SerialMTK.Read((int)num2);
									if (array2 == null || array2.Length != (int)num2)
									{
										return false;
									}
									byte[] array3 = Sla.GenerateBromSlaChallenge(array2, brom_sla_key.N, brom_sla_key.D);
									int num3 = array3.Length;
									byte[] array4 = new byte[4];
									Binary.WriteU32LE(array4, 0, (uint)num3);
									if (!SerialMTK.Write(array4))
									{
										return false;
									}
									array = SerialMTK.Read(4);
									if (array != null && array.Length == 4)
									{
										uint num4 = Binary.ReadU32BE(array);
										if (num3 == (int)num4)
										{
											array = SerialMTK.Read(2);
											if (array == null || array.Length != 2)
											{
												return false;
											}
											num = Binary.ReadU16BE(array);
											if (num > 255)
											{
												UI.RichLogs(text2 + Error.status(num), UI.error, nextline: true);
												return false;
											}
											SerialMTK.Write(array3);
											array = SerialMTK.Read(4);
											if (array == null || array.Length != 4)
											{
												return false;
											}
											uint num5 = Binary.ReadU32BE(array);
											if (num5 < 255)
											{
												return true;
											}
											Console.WriteLine(text + Error.status(num5));
										}
										continue;
									}
									return false;
								}
								return false;
							}
							Console.WriteLine(text + Error.status(num));
							return false;
						}
						return true;
					}
					return false;
				}
				return (byte)result != 0;
			}
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return (byte)result != 0;
		}
	}

	public byte[] BromRegisterAccess(int address, int length, byte[] data = null, bool checkStatus = true, int mode = -1)
	{
		object obj = null;
		string message = "Kamakiri2 failed, cache issue :(";
		bool flag;
		if (mode != -1)
		{
			flag = !SerialMTK.Echo(218);
		}
		else
		{
			mode = ((data != null) ? 1 : 0);
			flag = !SerialMTK.Echo(218);
		}
		if (!flag)
		{
			if (SerialMTK.Echo(mode, 4))
			{
				if (SerialMTK.Echo(address, 4))
				{
					if (!SerialMTK.Echo(length, 4))
					{
						return (byte[])obj;
					}
					byte[] array = SerialMTK.Read(2);
					if (array == null || array.Length != 2)
					{
						return (byte[])obj;
					}
					ushort num = Binary.ReadU16LE(array);
					switch (num)
					{
					case 6685:
						throw new Exception(message);
					case 0:
					{
						byte[] result = (byte[])obj;
						if (mode == 0 || mode == 2)
						{
							result = SerialMTK.Read(length);
							if (!checkStatus)
							{
								return result;
							}
							array = SerialMTK.Read(2);
							if (array != null && array.Length == 2)
							{
								num = Binary.ReadU16LE(array);
								if (num == 0)
								{
									return result;
								}
								throw new Exception(Error.status(num));
							}
							return (byte[])obj;
						}
						SerialMTK.Write(data.Take(length).ToArray());
						if (!checkStatus)
						{
							return result;
						}
						array = SerialMTK.Read(2);
						if (array != null && array.Length == 2)
						{
							num = Binary.ReadU16LE(array);
							if (num == 0)
							{
								return result;
							}
							throw new Exception(Error.status(num));
						}
						return (byte[])obj;
					}
					default:
						throw new Exception(Error.status(num));
					}
				}
				return (byte[])obj;
			}
			return (byte[])obj;
		}
		return (byte[])obj;
	}

	public bool GetHWSWVersion()
	{
		string msg = "Invalid HW/SW version response";
		SerialMTK.Echo(252);
		byte[] array = SerialMTK.Read(8);
		if (array != null && array.Length == 8)
		{
			cfg.HWSubCode = Binary.ReadU16BE(array);
			cfg.HWVer = Binary.ReadU16BE(array, 2);
			cfg.SWVer = Binary.ReadU16BE(array, 4);
			Binary.ReadU16BE(array, 6);
			return true;
		}
		UI.RichLogs(msg, UI.error, nextline: true);
		return false;
	}

	public void GetMeid()
	{
		string text = "Error on get_meid: ";
		if (!SerialMTK.Write(new byte[1] { 254 }))
		{
			return;
		}
		byte[] array = SerialMTK.Read(1);
		bool isBROM;
		if (array == null || array.Length != 1 || (!(isBROM = array[0] == 254) && array[0] <= 2))
		{
			return;
		}
		SerialMTK.Write(new byte[1] { 225 });
		byte[] array2 = SerialMTK.Read(1);
		if (array2 == null || array2.Length != 1 || array2[0] != 225)
		{
			return;
		}
		byte[] array3 = SerialMTK.Read(4);
		if (array3 == null || array3.Length != 4)
		{
			return;
		}
		int len = (int)Binary.ReadU32BE(array3);
		cfg.Meid = SerialMTK.Read(len);
		array3 = SerialMTK.Read(2);
		if (array3 != null && array3.Length == 2)
		{
			int num = Binary.ReadU16LE(array3);
			cfg.IsBROM = isBROM;
			if (num != 0)
			{
				Console.WriteLine(text + num);
			}
		}
	}

	public void GetSocId()
	{
		string text = "Error on get_socid: ";
		if (!SerialMTK.Write(new byte[1] { 254 }))
		{
			return;
		}
		byte[] array = SerialMTK.Read(1);
		if (array == null || array.Length != 1)
		{
			return;
		}
		if (array[0] == 254)
		{
			cfg.IsBROM = true;
			SerialMTK.Write(new byte[1] { 231 });
			byte[] array2 = SerialMTK.Read(1);
			if (array2 == null || array2.Length == 0 || array2[0] != 231)
			{
				return;
			}
			byte[] array3 = SerialMTK.Read(4);
			if (array3 == null || array3.Length != 4)
			{
				return;
			}
			int len = (int)Binary.ReadU32BE(array3);
			cfg.SocID = SerialMTK.Read(len);
			array3 = SerialMTK.Read(2);
			if (array3 != null && array3.Length == 2)
			{
				int num = Binary.ReadU16LE(array3);
				if (num != 0)
				{
					Console.WriteLine(text + num);
				}
			}
		}
		else
		{
			if (array[0] <= 2)
			{
				return;
			}
			cfg.IsBROM = false;
			SerialMTK.Write(new byte[1] { 231 });
			byte[] array2 = SerialMTK.Read(1);
			if (array2 == null || array2.Length == 0 || array2[0] != 231)
			{
				return;
			}
			byte[] array3 = SerialMTK.Read(4);
			if (array3 == null || array3.Length != 4)
			{
				return;
			}
			int len = (int)Binary.ReadU32BE(array3);
			cfg.SocID = SerialMTK.Read(len);
			array3 = SerialMTK.Read(2);
			if (array3 != null && array3.Length == 2)
			{
				int num = Binary.ReadU16LE(array3);
				if (num != 0)
				{
					Console.WriteLine(text + num);
				}
			}
		}
	}

	public static (int Checksum, byte[] Data) PrepareData(Span<byte> data, Span<byte> sigData, int maxSize)
	{
		int num = ((maxSize > 0 && maxSize < data.Length) ? maxSize : data.Length);
		int num2 = num + sigData.Length;
		byte[] array = new byte[(((num2 + sigData.Length) & 1) == 0) ? num2 : (num2 + 1)];
		Span<byte> span = array.AsSpan();
		data.Slice(0, num).CopyTo(span);
		sigData.CopyTo(span.Slice(num));
		ushort num3 = 0;
		for (int i = 0; i < span.Length; i += 2)
		{
			num3 ^= Binary.ReadU16LE(span, i);
		}
		return (Checksum: num3, Data: array);
	}

	private bool UploadData(byte[] data, int genChecksum, int genChecksumXor16, bool crash = false)
	{
		string value = "Uploading data.";
		int result = 0;
		string value2 = "No reply from da loader.";
		string value3 = "Checksum of upload doesn't match !";
		string text = "upload_data failed with error: ";
		string text2 = "upload_data resp error : ";
		Console.WriteLine(value);
		int num = data.Length;
		int num2 = 0;
		byte[] array = new byte[SerialMTK.usb.max_to_write];
		while (true)
		{
			if (num <= 0)
			{
				if (cfg.HWCode != 9521)
				{
					SerialMTK.Write(Array.Empty<byte>());
					Thread.Sleep(120);
				}
				if (crash)
				{
					return true;
				}
				break;
			}
			int num3 = Math.Min(num, array.Length);
			data.AsSpan(num2, num3).CopyTo(array);
			if (SerialMTK.Write(array.Take(num3).ToArray()))
			{
				num -= num3;
				num2 += num3;
				UI.Progresbar1(num2, data.Length);
				if ((num2 & 0x1FFF) == 0)
				{
					SerialMTK.Write(Array.Empty<byte>());
				}
				continue;
			}
			return false;
		}
		try
		{
			byte[] array2 = SerialMTK.Read(2);
			if (array2 != null && array2.Length == 2)
			{
				ushort num4 = Binary.ReadU16BE(array2);
				if (num4 != 0)
				{
					if (genChecksum != num4 && genChecksumXor16 != num4 && num4 != 0)
					{
						Console.WriteLine(value3);
					}
					array2 = SerialMTK.Read(2);
					if (array2 == null || array2.Length != 2)
					{
						return false;
					}
					ushort num5 = Binary.ReadU16BE(array2);
					if (num5 > 255)
					{
						Console.WriteLine(text + Error.status(num5));
						return false;
					}
					return true;
				}
				Console.WriteLine(value2);
				return false;
			}
			return false;
		}
		catch (Exception ex)
		{
			Console.WriteLine(text2 + ex.Message);
			return (byte)result != 0;
		}
	}

	private ushort Checksum16Xor(Span<byte> data, int len)
	{
		ushort num = 0;
		int num2 = len >> 1;
		for (int i = 0; i < num2; i++)
		{
			num ^= Binary.ReadU16LE(data, i << 1);
		}
		return num;
	}

	public bool SendDA(int addr, int size, int signLen, byte[] daData)
	{
		string value = "Error on DA_Send cmd";
		string value2 = "Error on DA_Send address";
		string value3 = "Error on DA_Send size";
		string value4 = "Error on DA_Send sign_len";
		string value5 = "SLA required ... ";
		string value6 = "Bad sla challange :(";
		string format = "Checksum 1 = 0x{0:X4}";
		string format2 = "Checksum 2 = 0x{0:X4}";
		string value7 = "Error on uploading da data";
		string text = "DA_Send status error:";
		(int Checksum, byte[] Data) tuple = PrepareData(daData.AsSpan(0, daData.Length - signLen), daData.AsSpan(daData.Length - signLen), size);
		int item = tuple.Checksum;
		byte[] item2 = tuple.Data;
		ushort num = Checksum16Xor(daData, size);
		if (SerialMTK.Echo(215))
		{
			if (!SerialMTK.Echo(addr, 4))
			{
				Console.WriteLine(value2);
				return false;
			}
			if (SerialMTK.Echo(item2.Length, 4))
			{
				if (SerialMTK.Echo(signLen, 4))
				{
					byte[] array = SerialMTK.Read(2);
					if (array != null && array.Length == 2)
					{
						ushort num2 = Binary.ReadU16BE(array);
						if (num2 == 7437)
						{
							Console.WriteLine(value5);
							if (!HandleSLA(cfg.IsBROM))
							{
								Console.WriteLine(value6);
								return false;
							}
							num2 = 0;
						}
						if (num2 <= 255)
						{
							Console.WriteLine(string.Format(format, item));
							Console.WriteLine(string.Format(format2, num));
							if (!UploadData(item2, item, num))
							{
								Console.WriteLine(value7);
								return false;
							}
							return true;
						}
						Console.WriteLine(text + Error.status(num2));
						return false;
					}
					return false;
				}
				Console.WriteLine(value4);
				return false;
			}
			Console.WriteLine(value3);
			return false;
		}
		Console.WriteLine(value);
		return false;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
