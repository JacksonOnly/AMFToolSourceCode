using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AMFTool.Library.Usb;

namespace AMFTool.Library.Qualcomm;

public class Sahara
{
	public enum Mode
	{
		None,
		NandPrg,
		ENandPrg,
		Sahara,
		Firehose,
		Error
	}

	public class SaharaResponse
	{
		public bool HasCmd;

		public SaharaDefs.SaharaCommand Cmd;

		public object Data;

		public Mode Mode;
	}

	public class SaharaConnectResult
	{
		public bool Success;

		public Mode Mode;

		public SaharaDefs.SaharaCommand Cmd;

		public object Data;
	}

	public class ChipIdV3
	{
		public uint ChipIdV3Value;

		public uint MsmId;

		public ushort OemId;

		public ushort ModelId;

		public byte[] RawData;
	}

	private uint Version;

	private Mode TransferMode;

	private bool Bit64;

	private uint PktSize;

	private uint Serial;

	private string Serials;

	public byte[] SblVersion;

	private ulong? Hwid;

	private string HwidStr;

	private string PkHash;

	private uint MsmId;

	private uint OemId;

	private string ModelId;

	private string OemStr;

	private string MsmStr;

	private string CpuStr;

	public byte[] LoaderAuto = new byte[0];

	private SaharaDefs.CommandHandler Ch;

	public byte[] loaderAuto = new byte[0];

	public Sahara()
	{
		Ch = new SaharaDefs.CommandHandler();
	}

	private string GetError(uint status)
	{
		string text = " (0x";
		string text2 = "X";
		string text3 = ")";
		string text4 = "Unknown: 0x";
		if (SaharaDefs.ErrorDesc.TryGetValue(status, out var value))
		{
			return value + text + status.ToString(text2) + text3;
		}
		return text4 + status.ToString(text2);
	}

	private static bool IsFirehose(byte[] data)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (data != null)
		{
			if (data.AsSpan().IndexOf("<?xml"u8) < 0)
			{
				if (data.AsSpan().IndexOf("<response"u8) < 0)
				{
					return data.AsSpan().IndexOf("<log"u8) >= 0;
				}
				return true;
			}
			return true;
		}
		return false;
	}

	private SaharaResponse GetResponse()
	{
		object obj = null;
		try
		{
			byte[] array = SerialEDL.Read();
			if (array != null && array.Length != 0)
			{
				if (IsFirehose(array))
				{
					return new SaharaResponse
					{
						Mode = Mode.Firehose
					};
				}
				SaharaDefs.CmdHdr cmdHdr = Ch.PktCmdHdr(array);
				SaharaDefs.SaharaCommand cmd = (SaharaDefs.SaharaCommand)cmdHdr.Cmd;
				if (cmd != SaharaDefs.SaharaCommand.SAHARA_HELLO_REQ)
				{
					if (cmd == SaharaDefs.SaharaCommand.SAHARA_DONE_RSP)
					{
						return new SaharaResponse
						{
							HasCmd = true,
							Cmd = cmd,
							Data = Ch.PktDone(array),
							Mode = Mode.Sahara
						};
					}
					if (cmd == SaharaDefs.SaharaCommand.SAHARA_END_TRANSFER)
					{
						return new SaharaResponse
						{
							HasCmd = true,
							Cmd = cmd,
							Data = Ch.PktImageEnd(array),
							Mode = Mode.Sahara
						};
					}
					if (cmd != SaharaDefs.SaharaCommand.SAHARA_64BIT_MEMORY_READ_DATA)
					{
						if (cmd != SaharaDefs.SaharaCommand.SAHARA_READ_DATA)
						{
							if (cmd == SaharaDefs.SaharaCommand.SAHARA_64BIT_MEMORY_DEBUG)
							{
								Bit64 = true;
								return new SaharaResponse
								{
									HasCmd = true,
									Cmd = cmd,
									Data = Ch.PktMemoryDebug64(array),
									Mode = Mode.Sahara
								};
							}
							if (cmd == SaharaDefs.SaharaCommand.SAHARA_MEMORY_DEBUG)
							{
								Bit64 = false;
								return new SaharaResponse
								{
									HasCmd = true,
									Cmd = cmd,
									Data = Ch.PktMemoryDebug(array),
									Mode = Mode.Sahara
								};
							}
							if (cmd != SaharaDefs.SaharaCommand.SAHARA_EXECUTE_RSP)
							{
								if (cmd != SaharaDefs.SaharaCommand.SAHARA_CMD_READY && cmd != SaharaDefs.SaharaCommand.SAHARA_RESET_RSP)
								{
									return (SaharaResponse)obj;
								}
								return new SaharaResponse
								{
									HasCmd = true,
									Cmd = cmd,
									Data = obj,
									Mode = Mode.Sahara
								};
							}
							return new SaharaResponse
							{
								HasCmd = true,
								Cmd = cmd,
								Data = Ch.PktExecuteRspCmd(array),
								Mode = Mode.Sahara
							};
						}
						Bit64 = false;
						return new SaharaResponse
						{
							HasCmd = true,
							Cmd = cmd,
							Data = Ch.PktReadData(array),
							Mode = Mode.Sahara
						};
					}
					Bit64 = true;
					return new SaharaResponse
					{
						HasCmd = true,
						Cmd = cmd,
						Data = Ch.PktReadData64(array),
						Mode = Mode.Sahara
					};
				}
				return new SaharaResponse
				{
					HasCmd = true,
					Cmd = cmd,
					Data = Ch.PktHelloReq(array),
					Mode = Mode.Sahara
				};
			}
			return (SaharaResponse)obj;
		}
		catch
		{
			return (SaharaResponse)obj;
		}
	}

	private bool SendHello(SaharaDefs.SaharaMode TransferMode, uint version_min = 1u, uint max_cmd_len = 0u, uint Version = 2u)
	{
		int nextline = 1;
		int num = 0;
		try
		{
			uint[] obj = new uint[12]
			{
				2u, 48u, 0u, 0u, 0u, 0u, 1u, 2u, 3u, 4u,
				5u, 6u
			};
			obj[2] = Version;
			obj[3] = version_min;
			obj[4] = max_cmd_len;
			obj[5] = (uint)TransferMode;
			byte[] send = obj.Pack();
			return SerialEDL.Write(send);
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)nextline != 0, (byte)num != 0);
			return (byte)num != 0;
		}
	}

	private SaharaConnectResult Connect(bool reconnect = false)
	{
		int num = 0;
		int nextline = 1;
		string text = "Handshake ";
		int mode = 5;
		try
		{
			byte[] array = ((!reconnect) ? SerialEDL.Read(setting: true) : SerialEDL.Read());
			Console.WriteLine(text + array.ToHexString());
			if (array != null && array.Length > 1)
			{
				if (array[0] == 1)
				{
					SaharaDefs.CmdHdr cmdHdr = Ch.PktCmdHdr(array);
					if (cmdHdr.Cmd == 1)
					{
						SaharaDefs.HelloReq helloReq = Ch.PktHelloReq(array);
						PktSize = helloReq.CmdPacketLength;
						Version = helloReq.Version;
						return new SaharaConnectResult
						{
							Success = true,
							Mode = Mode.Sahara,
							Cmd = SaharaDefs.SaharaCommand.SAHARA_HELLO_REQ,
							Data = helloReq
						};
					}
					if (cmdHdr.Cmd == 4)
					{
						return new SaharaConnectResult
						{
							Success = true,
							Mode = Mode.Sahara,
							Cmd = SaharaDefs.SaharaCommand.SAHARA_END_TRANSFER,
							Data = Ch.PktImageEnd(array)
						};
					}
				}
				if (IsFirehose(array))
				{
					return new SaharaConnectResult
					{
						Success = true,
						Mode = Mode.Firehose
					};
				}
			}
			if (HangHack())
			{
				return new SaharaConnectResult
				{
					Success = true,
					Mode = Mode.Firehose
				};
			}
			return new SaharaConnectResult
			{
				Success = false,
				Mode = Mode.Error
			};
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)nextline != 0, (byte)num != 0);
		}
		return new SaharaConnectResult
		{
			Success = ((byte)num != 0),
			Mode = (Mode)mode
		};
	}

	private bool HangHack()
	{
		int num = 0;
		int nextline = 1;
		try
		{
			SwitchMode(SaharaDefs.SaharaMode.SAHARA_MODE_IMAGE_TX_PENDING);
			for (int i = 0; i < 3; i++)
			{
				byte[] data = SerialEDL.Read(setting: true);
				if (!IsFirehose(data))
				{
					Thread.Sleep(1000);
					continue;
				}
				return true;
			}
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)nextline != 0, (byte)num != 0);
		}
		return (byte)num != 0;
	}

	private bool EnterCMDMode(uint Version = 2u)
	{
		object obj = null;
		if (!SendHello(SaharaDefs.SaharaMode.SAHARA_MODE_COMMAND, 1u, 0u, Version))
		{
			return false;
		}
		SaharaResponse response = GetResponse();
		if (response == null)
		{
			return false;
		}
		if (!response.HasCmd)
		{
			return false;
		}
		if (response.Cmd != SaharaDefs.SaharaCommand.SAHARA_END_TRANSFER)
		{
			return response.Cmd == SaharaDefs.SaharaCommand.SAHARA_CMD_READY;
		}
		SaharaDefs.ImageEnd imageEnd = response.Data as SaharaDefs.ImageEnd;
		if (imageEnd ==null)
		{
			return false;
		}
		UI.RichLogs(GetError((uint)imageEnd.ImageTxStatus), UI.error, nextline: true);
		return false;
	}

	private uint GetSerialNum()
	{
		byte[] array = SendExecCmd(SaharaDefs.SaharaExecCommand.SERIAL_NUM_READ);
		if (array == null || array.Length < 4)
		{
			uint num = 0u;
			return 0u;
		}
		return Binary.ReadU32LE(array);
	}

	private ulong? GetMSMHwid()
	{
		byte[] array = SendExecCmd(SaharaDefs.SaharaExecCommand.MSM_HW_ID_READ);
		if (array != null && array.Length >= 8)
		{
			return Binary.ReadU64LE(array);
		}
		return null;
	}

	private string GetPKHash()
	{
		int nextline = 1;
		object result = null;
		int bold = 0;
		string oldValue = "-";
		string newValue = "";
		try
		{
			byte[] array = SendExecCmd(SaharaDefs.SaharaExecCommand.OEM_PK_HASH_READ);
			if (array != null && array.Length >= 4)
			{
				byte[] array2 = new byte[4];
				array.AsSpan(0, 4).CopyTo(array2);
				int num = Extentions.IndexOf(array, array2, 4);
				if (num != -1)
				{
					byte[] array3 = new byte[num];
					array.AsSpan(0, num).CopyTo(array3);
					array = array3;
				}
				return BitConverter.ToString(array).Replace(oldValue, newValue).ToLower();
			}
			return (string)result;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)nextline != 0, (byte)bold != 0);
			return (string)result;
		}
	}

	private ChipIdV3 GetChipIDV3()
	{
		object result = null;
		string text = "cmdexec_get_chip_id_v3 error: ";
		try
		{
			byte[] array = SendExecCmd(SaharaDefs.SaharaExecCommand.READ_CHIP_ID_V3);
			if (array != null && array.Length >= 44)
			{
				ChipIdV3 chipIdV = new ChipIdV3
				{
					ChipIdV3Value = Binary.ReadU32LE(array),
					MsmId = Binary.ReadU32LE(array, 36),
					OemId = Binary.ReadU16LE(array, 40),
					ModelId = Binary.ReadU16LE(array, 42),
					RawData = array
				};
				if (chipIdV.OemId == 0 && array.Length >= 46)
				{
					chipIdV.OemId = Binary.ReadU16LE(array, 44);
				}
				return chipIdV;
			}
			return (ChipIdV3)result;
		}
		catch (Exception ex)
		{
			Console.WriteLine(text + ex.Message);
			return (ChipIdV3)result;
		}
	}

	private bool ReadInfo(uint Version)
	{
		string text = "x8";
		string text2 = "x16";
		string text3 = "x4";
		string msg = "CPU Detected      : ";
		string msg2 = "Sahara Version    : ";
		string format = "{0}";
		string msg3 = "HW ID             : ";
		string text4 = "0x";
		string msg4 = "MSM ID            : ";
		string msg5 = "OEM ID            : ";
		string text5 = " ";
		string text6 = "[";
		string text7 = "]";
		string msg6 = "Model ID          : ";
		string text8 = " [";
		string msg7 = "PK_HASH[0]        : ";
		string text9 = "";
		string msg8 = "PK_HASH[1]        : ";
		string msg9 = "PK_HASH[2]        : ";
		object obj = null;
		string text10 = "X4";
		string text11 = "x6";
		string text12 = "00";
		string msg10 = "Chip Identifier V3 : ";
		string format2 = "0x{0:X8}";
		string msg11 = "HW ID (V3)        : ";
		string msg12 = "Serial            : ";
		if (EnterCMDMode(Version))
		{
			Serial = GetSerialNum();
			Serials = Serial.ToString(text);
			if (Version >= 3)
			{
				PkHash = GetPKHash();
				ChipIdV3 chipIDV = GetChipIDV3();
				if (chipIDV ==null)
				{
					UI.RichLogs(msg2, UI.log);
					UI.RichLogs(string.Format(format, Version), UI.info, nextline: true);
					UI.RichLogs(msg12, UI.log);
					UI.RichLogs(text4 + Serials, UI.info, nextline: true);
					if (!string.IsNullOrEmpty(PkHash))
					{
						if (PkHash.Length >= 32)
						{
							UI.RichLogs(msg7, UI.log);
							UI.RichLogs(PkHash.Substring(0, 32).ToUpper() ?? text9, UI.info, nextline: true);
						}
						if (PkHash.Length >= 64)
						{
							UI.RichLogs(msg8, UI.log);
							UI.RichLogs(PkHash.Substring(PkHash.Length - 32, 32).ToUpper() ?? text9, UI.info, nextline: true);
						}
						UI.RichLogs(msg9, UI.log);
						UI.RichLogs(HwidStr + PkHash.Substring(0, 16).ToUpper(), UI.info, nextline: true);
					}
				}
				else
				{
					MsmId = chipIDV.MsmId;
					OemId = chipIDV.OemId;
					ModelId = chipIDV.ModelId.ToString(text10);
					MsmStr = MsmId.ToString(text11);
					OemStr = OemId.ToString(text3);
					HwidStr = text12 + MsmStr + OemStr + ModelId;
					Hwid = Convert.ToUInt64(HwidStr, 16);
					string vendor = QualcommCfg.GetVendor(OemId);
					CpuStr = QualcommCfg.GetSOC(MsmId);
					UI.RichLogs(msg10, UI.log);
					UI.RichLogs(string.Format(format2, chipIDV.ChipIdV3Value), UI.info, nextline: true);
					UI.RichLogs(msg, UI.log);
					UI.RichLogs(CpuStr, UI.info, nextline: true);
					UI.RichLogs(msg2, UI.log);
					UI.RichLogs(string.Format(format, Version), UI.info, nextline: true);
					UI.RichLogs(msg11, UI.log);
					UI.RichLogs(text4 + HwidStr, UI.info, nextline: true);
					UI.RichLogs(msg4, UI.log);
					UI.RichLogs(text4 + MsmStr, UI.info, nextline: true);
					UI.RichLogs(msg5, UI.log);
					UI.RichLogs(text4 + OemStr + text8 + vendor + text7, UI.info, nextline: true);
					UI.RichLogs(msg6, UI.log);
					UI.RichLogs(text4 + ModelId + text8 + CpuStr + text7, UI.info, nextline: true);
				}
			}
			else
			{
				Hwid = GetMSMHwid();
				PkHash = GetPKHash();
				if (Hwid.HasValue)
				{
					HwidStr = Hwid.Value.ToString(text2);
					MsmId = Convert.ToUInt32(HwidStr.Substring(2, 6), 16);
					OemId = Convert.ToUInt32(HwidStr.Substring(HwidStr.Length - 8, 4), 16);
					uint num = Convert.ToUInt32(HwidStr.Substring(HwidStr.Length - 4), 16);
					OemStr = OemId.ToString(text3);
					ModelId = num.ToString(text3);
					MsmStr = MsmId.ToString(text);
					string vendor2 = QualcommCfg.GetVendor(OemId);
					UI.RichLogs(msg, UI.log);
					CpuStr = QualcommCfg.GetSOC(MsmId);
					UI.RichLogs(CpuStr, UI.info, nextline: true);
					UI.RichLogs(msg2, UI.log);
					UI.RichLogs(string.Format(format, Version), UI.info, nextline: true);
					UI.RichLogs(msg3, UI.log);
					UI.RichLogs(text4 + HwidStr, UI.info, nextline: true);
					UI.RichLogs(msg4, UI.log);
					UI.RichLogs(text4 + MsmStr, UI.info, nextline: true);
					UI.RichLogs(msg5, UI.log);
					UI.RichLogs(text4 + OemStr + text5, UI.info);
					UI.RichLogs(text6 + vendor2 + text7, UI.info, nextline: true);
					UI.RichLogs(msg6, UI.log);
					UI.RichLogs(text4 + ModelId + text8 + CpuStr + text7, UI.info, nextline: true);
					if (!string.IsNullOrEmpty(PkHash))
					{
						if (PkHash.Length >= 32)
						{
							UI.RichLogs(msg7, UI.log);
							UI.RichLogs(PkHash.Substring(0, 32).ToUpper() ?? text9, UI.info, nextline: true);
						}
						if (PkHash.Length >= 64)
						{
							UI.RichLogs(msg8, UI.log);
							UI.RichLogs(PkHash.Substring(PkHash.Length - 32, 32).ToUpper() ?? text9, UI.info, nextline: true);
						}
					}
					UI.RichLogs(msg9, UI.log);
					UI.RichLogs(HwidStr + PkHash.Substring(0, 16).ToUpper(), UI.info, nextline: true);
				}
			}
			SwitchMode(SaharaDefs.SaharaMode.SAHARA_MODE_IMAGE_TX_PENDING);
			return true;
		}
		return false;
	}

	private bool SendDone()
	{
		int num = 0;
		int nextline = 1;
		object obj = null;
		string msg = "Invalid Transfer command received.";
		string format = "Received invalid response {0}.";
		try
		{
			if (!SerialEDL.Write(new uint[2] { 5u, 8u }.Pack()))
			{
				return false;
			}
			SaharaResponse response = GetResponse();
			Thread.Sleep(300);
			if (response != null && response.HasCmd)
			{
				if (response.Cmd != SaharaDefs.SaharaCommand.SAHARA_DONE_RSP)
				{
					if (response.Cmd == SaharaDefs.SaharaCommand.SAHARA_END_TRANSFER)
					{
						SaharaDefs.ImageEnd imageEnd = response.Data as SaharaDefs.ImageEnd;
						if (imageEnd !=null && imageEnd.ImageTxStatus == SaharaDefs.SaharaStatus.SAHARA_NAK_INVALID_CMD)
						{
							UI.RichLogs(msg, UI.error, nextline: true);
							return false;
						}
					}
					UI.RichLogs(string.Format(format, response.Cmd), UI.error, nextline: true);
					return false;
				}
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)nextline != 0, (byte)num != 0);
			return (byte)num != 0;
		}
	}

	private bool SendReset()
	{
		int num = 0;
		int nextline = 1;
		object obj = null;
		try
		{
			SerialEDL.Write(new uint[2] { 7u, 8u }.Pack());
			SaharaResponse response = GetResponse();
			if (response != null && response.HasCmd)
			{
				if (response.Cmd != SaharaDefs.SaharaCommand.SAHARA_RESET_RSP)
				{
					if (response.Cmd == SaharaDefs.SaharaCommand.SAHARA_END_TRANSFER)
					{
						SaharaDefs.ImageEnd imageEnd = response.Data as SaharaDefs.ImageEnd;
						if (imageEnd !=null)
						{
							UI.RichLogs(GetError((uint)imageEnd.ImageTxStatus), UI.error, nextline: true);
						}
					}
					return false;
				}
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)nextline != 0, (byte)num != 0);
			return (byte)num != 0;
		}
	}

	private void SwitchMode(SaharaDefs.SaharaMode TransferMode)
	{
		SerialEDL.Write(new uint[3]
		{
			12u,
			12u,
			(uint)TransferMode
		}.Pack());
	}

	private byte[] SendExecCmd(SaharaDefs.SaharaExecCommand mcmd)
	{
		int bold = 0;
		int nextline = 1;
		object obj = null;
		try
		{
			SerialEDL.Write(new uint[3]
			{
				13u,
				12u,
				(uint)mcmd
			}.Pack());
			SaharaResponse response = GetResponse();
			if (response != null && response.HasCmd)
			{
				if (response.Cmd != SaharaDefs.SaharaCommand.SAHARA_EXECUTE_RSP)
				{
					if (response.Cmd == SaharaDefs.SaharaCommand.SAHARA_END_TRANSFER)
					{
						SaharaDefs.ImageEnd imageEnd = response.Data as SaharaDefs.ImageEnd;
						if (imageEnd !=null)
						{
							UI.RichLogs(GetError((uint)imageEnd.ImageTxStatus), UI.error, nextline: true);
						}
					}
					return (byte[])obj;
				}
				SaharaDefs.ExecuteRsp executeRsp = response.Data as SaharaDefs.ExecuteRsp;
				if (executeRsp !=null)
				{
					SerialEDL.Write(new uint[3]
					{
						15u,
						12u,
						(uint)mcmd
					}.Pack());
					if (executeRsp.DataLen != 0 && executeRsp.DataLen <= int.MaxValue)
					{
						byte[] array = SerialEDL.usb.readusb((int)executeRsp.DataLen);
						if (array != null && array.Length != 0)
						{
							return array;
						}
					}
					return SerialEDL.Read();
				}
				return (byte[])obj;
			}
			return (byte[])obj;
		}
		catch (Exception ex)
		{
			UI.RichLogs(ex.Message, UI.error, (byte)nextline != 0, (byte)bold != 0);
			return (byte[])obj;
		}
	}

	private (bool, byte[]) FirehoseSelect()
	{
		string msg = "";
		string text = "Auto";
		string msg2 = "Downloading flash programmer... ";
		string text2 = "get loader ";
		string text3 = "_";
		string text4 = "Qualcomm/";
		string text5 = "/";
		string msg3 = "Failed!";
		string msg4 = "Invalid Loader!, Try Decrypt Loader first";
		string msg5 = "OK";
		string text6 = "/loader.bin";
		string msg6 = "Local firehose not found!";
		string msg7 = "Using Selected firehose";
		string msg8 = "Reading local flash programmer... ";
		string msg9 = "Invalid firehose";
		UI.RichLogs(msg, UI.log, nextline: true);
		byte[] array = Array.Empty<byte>();
		if (DeviceState.Brand == text && DeviceState.Model == text)
		{
			UI.RichLogs(msg2, UI.log);
			Console.WriteLine(text2 + HwidStr + text3 + PkHash.Substring(0, 16));
			string text7 = Server.FindLoader(HwidStr + text3 + PkHash.Substring(0, 16));
			array = Server.LoadAndDecrypt(text4 + DeviceState.Brand + text5 + text7);
			if (array == null || array.Length == 0)
			{
				UI.RichLogs(msg3, UI.error, nextline: true);
				return (false, Array.Empty<byte>());
			}
			if (utils.Validloader(array))
			{
				UI.RichLogs(msg5, UI.ok, nextline: true);
				return (true, array);
			}
			UI.RichLogs(msg4, UI.error, nextline: true);
			return (false, Array.Empty<byte>());
		}
		if (!EDLConfig.EDLServer && !DeviceState.Unlock)
		{
			if (!File.Exists(EDLConfig.PathLoader))
			{
				UI.RichLogs(msg6, UI.error, nextline: true);
				return (false, Array.Empty<byte>());
			}
			UI.RichLogs(msg7, UI.error, nextline: true);
			UI.RichLogs(msg8, UI.log);
			array = File.ReadAllBytes(EDLConfig.PathLoader);
			if (array != null && array.Length != 0)
			{
				if (utils.Validloader(array))
				{
					UI.RichLogs(msg5, UI.ok, nextline: true);
					return (true, array);
				}
				UI.RichLogs(msg4, UI.error, nextline: true);
				return (false, Array.Empty<byte>());
			}
			UI.RichLogs(msg9, UI.error, nextline: true);
			return (false, Array.Empty<byte>());
		}
		UI.RichLogs(msg2, UI.log);
		string path = text4 + DeviceState.Brand + text5 + DeviceState.Model + text6;
		array = Server.LoadAndDecrypt(path);
		if (array != null && array.Length != 0)
		{
			if (utils.Validloader(array))
			{
				UI.RichLogs(msg5, UI.ok, nextline: true);
				return (true, array);
			}
			UI.RichLogs(msg4, UI.error, nextline: true);
			return (false, Array.Empty<byte>());
		}
		UI.RichLogs(msg3, UI.error, nextline: true);
		return (false, Array.Empty<byte>());
	}

	private Mode UploadLoader(uint Version)
	{
		int bold = 0;
		int result = 5;
		int nextline = 1;
		string msg = "Invalid loader data.";
		string msg2 = "Sending Exploit... ";
		string msg3 = "Failed!";
		string msg4 = "OK";
		string msg5 = "Loader Not Support!";
		string msg6 = "Timeout while uploading loader. Wrong loader ?";
		string text = "32-Bit mode detected.";
		string text2 = "64-Bit mode detected.";
		string msg7 = "NAND mode detected.";
		string msg8 = "eNAND mode detected.";
		string format = "Unknown sahara Id: {0:X}";
		string msg9 = "Sending programmer file... ";
		string msg10 = "Failed to send loader chunk to EDL port.";
		double field_ = Class38.field_543;
		string msg11 = "Error on uploading Loader.";
		string msg12 = "Unknown response received on uploading loader.";
		string text3 = "Error on uploading loader: ";
		object obj = null;
		var (flag, array) = FirehoseSelect();
		if (flag)
		{
			if (array != null && array.Length != 0)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				long num = 0L;
				if (IsSDM665())
				{
					UI.RichLogs(msg2, UI.log);
					if (!SDM665Exploit(new OppoExploit()))
					{
						UI.RichLogs(msg3, UI.error, nextline: true);
						return Mode.Error;
					}
					UI.RichLogs(msg4, UI.ok, nextline: true);
				}
				if (!SendHello(SaharaDefs.SaharaMode.SAHARA_MODE_IMAGE_TX_PENDING, 1u, 0u, Version))
				{
					return Mode.Error;
				}
				try
				{
					long num2 = array.Length;
					int num3 = 0;
					UI.Progresbar1(0L, num2);
					while (num2 >= 0L)
					{
						SaharaResponse response = GetResponse();
						if (response == null || !response.HasCmd)
						{
							UI.RichLogs(msg5, UI.error, nextline: true);
							return Mode.Error;
						}
						if (response.Cmd != SaharaDefs.SaharaCommand.SAHARA_DONE_REQ)
						{
							if (response.Cmd == SaharaDefs.SaharaCommand.SAHARA_READ_DATA || response.Cmd == SaharaDefs.SaharaCommand.SAHARA_64BIT_MEMORY_READ_DATA)
							{
								Bit64 = response.Cmd == SaharaDefs.SaharaCommand.SAHARA_64BIT_MEMORY_READ_DATA;
								if (num3 == 0)
								{
									UI.RichLogs((!Bit64) ? text : text2, UI.log, nextline: true);
								}
								SaharaDefs.ReadData readData = (SaharaDefs.ReadData)response.Data;
								ulong imageId = readData.ImageId;
								if (imageId != 7L)
								{
									switch (imageId)
									{
									case 11uL:
										TransferMode = Mode.ENandPrg;
										if (num3 == 0)
										{
											UI.RichLogs(msg8, UI.log, nextline: true);
										}
										break;
									case 0uL:
									case 1uL:
									case 2uL:
									case 3uL:
									case 4uL:
									case 5uL:
									case 6uL:
									case 7uL:
									case 8uL:
									case 9uL:
									case 10uL:
										UI.RichLogs(string.Format(format, imageId), UI.error, nextline: true);
										return Mode.Error;
									default:
										TransferMode = Mode.Firehose;
										break;
									}
								}
								else
								{
									TransferMode = Mode.NandPrg;
									if (num3 == 0)
									{
										UI.RichLogs(msg7, UI.log, nextline: true);
									}
								}
								if (num3 == 0)
								{
									UI.RichLogs(msg9, UI.log);
								}
								num3++;
								int num4 = (int)readData.DataOffset;
								int num5 = (int)readData.DataLen;
								if (num4 + num5 > array.Length)
								{
									int num6 = array.Length;
									Array.Resize(ref array, num4 + num5);
									for (int i = num6; i < array.Length; i++)
									{
										array[i] = byte.MaxValue;
									}
								}
								if (SerialEDL.Write(array.AsSpan(num4, num5).ToArray()))
								{
									num += num5;
									UI.Progresbar1(Math.Min(num, num2), num2);
									double totalSeconds = stopwatch.Elapsed.TotalSeconds;
									if (totalSeconds > field_)
									{
										UI.LabelWrite(utils.FileSize((long)((double)num / totalSeconds)), utils.FileSize(num), utils.FileSize(num2));
									}
									num2 -= num5;
									continue;
								}
								UI.RichLogs(msg10, UI.error, nextline: true);
								return Mode.Error;
							}
							if (response.Cmd != SaharaDefs.SaharaCommand.SAHARA_END_TRANSFER)
							{
								UI.RichLogs(msg12, UI.error, nextline: true);
								return Mode.Error;
							}
							SaharaDefs.ImageEnd imageEnd = (SaharaDefs.ImageEnd)response.Data;
							if (imageEnd.ImageTxStatus != SaharaDefs.SaharaStatus.SAHARA_STATUS_SUCCESS)
							{
								UI.RichLogs(GetError((uint)imageEnd.ImageTxStatus), UI.error, nextline: true);
								return Mode.Error;
							}
							UI.Progresbar1(num2, num2);
							UI.LabelWrite(utils.FileSize(field_), utils.FileSize(num2), utils.FileSize(num2));
							if (!SendDone())
							{
								UI.RichLogs(msg11, UI.error, nextline: true);
								return Mode.Error;
							}
							UI.RichLogs(msg4, UI.ok, nextline: true);
							return TransferMode;
						}
						if (SendDone())
						{
							return TransferMode;
						}
						UI.RichLogs(msg6, UI.error, nextline: true);
						return Mode.Error;
					}
				}
				catch (Exception ex)
				{
					UI.RichLogs(text3 + (string)((ex != null) ? ex.ToString() : obj), UI.error, (byte)nextline != 0, (byte)bold != 0);
					return (Mode)result;
				}
				return (Mode)result;
			}
			UI.RichLogs(msg, UI.error, nextline: true);
			return Mode.Error;
		}
		return Mode.Error;
	}

	public bool Init()
	{
		string msg = "Device is in Sahara error state, please reboot the device.";
		string msg2 = "Mode detected     : ";
		string msg3 = "Please Reconnect device to edl mode!";
		object obj = null;
		string msg4 = "Currently Not Support this Mode";
		string msg5 = "Error on sahara handshake, resetting.";
		SaharaConnectResult saharaConnectResult = Connect();
		if (!saharaConnectResult.Success)
		{
			UI.RichLogs(msg, UI.error, nextline: true);
			return false;
		}
		UI.RichLogs(msg2, UI.log);
		UI.RichLogs(saharaConnectResult.Mode.ToString().ToLower(), UI.info, nextline: true);
		if (saharaConnectResult.Cmd != SaharaDefs.SaharaCommand.SAHARA_END_TRANSFER)
		{
			if (saharaConnectResult.Mode != Mode.Firehose)
			{
				if (saharaConnectResult.Mode == Mode.Sahara)
				{
					SaharaDefs.HelloReq helloReq = default(SaharaDefs.HelloReq);
					bool flag;
					if (saharaConnectResult.Cmd == SaharaDefs.SaharaCommand.SAHARA_HELLO_REQ)
					{
						helloReq = saharaConnectResult.Data as SaharaDefs.HelloReq;
						flag = helloReq ==null;
					}
					else
					{
						flag = true;
					}
					if (!flag)
					{
						if (helloReq.Mode != SaharaDefs.SaharaMode.SAHARA_MODE_MEMORY_DEBUG)
						{
							uint version = helloReq.Version;
							if (ReadInfo(version))
							{
								SaharaConnectResult saharaConnectResult2 = Connect(reconnect: true);
								if (!saharaConnectResult2.Success)
								{
									return false;
								}
								if (saharaConnectResult2.Mode == Mode.Sahara)
								{
									Mode mode = UploadLoader(version);
									if (mode != Mode.Error)
									{
										return true;
									}
									return false;
								}
								return true;
							}
							UI.RichLogs(msg5, UI.error, nextline: true);
							SendReset();
							return false;
						}
						UI.RichLogs(msg4, UI.error, nextline: true);
						return false;
					}
					return false;
				}
				return false;
			}
			return true;
		}
		UI.RichLogs(msg3, UI.error, nextline: true);
		return false;
	}

	private bool SDM665Exploit(OppoExploit exp)
	{
		string hex = "139a9a9a";
		int result = 0;
		try
		{
			byte[] send = hex.HexToBytes();
			for (int i = 0; i < 95; i++)
			{
				SerialEDL.Write(send);
			}
			if (SerialEDL.Write(exp.Sdm665Payload1))
			{
				if (SerialEDL.Write(exp.Sdm665Payload2))
				{
					if (SerialEDL.Write(exp.Sdm665Payload3))
					{
						if (!SerialEDL.Write(exp.Sdm665Payload4))
						{
							return false;
						}
						if (SerialEDL.Write(exp.Sdm665Payload5))
						{
							if (!SerialEDL.Write(exp.Sdm665Payload6))
							{
								return false;
							}
							if (!SerialEDL.Write(exp.Sdm665Payload7))
							{
								return false;
							}
							byte[] array = SerialEDL.Read();
							if (array.Length == 0)
							{
								int num = 0;
								do
								{
									array = SerialEDL.Read();
									if (array.Length == 0)
									{
										num++;
										continue;
									}
									return true;
								}
								while (num < 25);
							}
							return array.Length != 0;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	private bool IsSDM665()
	{
		string text = "Realme";
		string text2 = "Realme 5 [RMX1911, RMX1919, RMX1927]";
		string text3 = "Realme 5i [RMX2030, RMX2032]";
		string text4 = "Realme 5R [RMX1911, RMX1919, RMX1927]";
		string text5 = "Realme 5S [RMX1925]";
		string text6 = "Oppo";
		string text7 = "Oppo A5 2020 [CPH1931, CPH1933, CPH1943]";
		string text8 = "Oppo A9 2020 [CPH1937, CPH1939, CPH1941]";
		string text9 = "Oppo A52 [CPH2061, CPH2069]";
		string text10 = "Oppo A72 [CPH2067]";
		string text11 = "Oppo A92 [CPH2059]";
		bool flag2;
		if (!(DeviceState.Brand == text))
		{
			if (DeviceState.Brand == text6)
			{
				bool flag;
				if (!(DeviceState.Model == text7))
				{
					if (!(DeviceState.Model == text8))
					{
						if (!(DeviceState.Model == text9))
						{
							if (!(DeviceState.Model == text10))
							{
								if (flag = DeviceState.Model == text11)
								{
									flag2 = true;
									return true;
								}
								flag2 = false;
								return false;
							}
							flag = true;
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					flag2 = true;
					return true;
				}
				return false;
			}
			return false;
		}
		if (!(DeviceState.Model == text2) && !(DeviceState.Model == text3) && !(DeviceState.Model == text4) && !(DeviceState.Model == text5))
		{
			flag2 = false;
			return false;
		}
		flag2 = true;
		return true;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
