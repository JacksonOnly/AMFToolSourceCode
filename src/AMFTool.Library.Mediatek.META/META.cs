using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using AMFTool.Library.Usb;

namespace AMFTool.Library.Mediatek.META;

public class META
{
	private ushort mSequence;

	public META()
	{
		mSequence = 1;
	}

	public bool SP_META_ConnectInMetaModeByUSB()
	{
		Thread.Sleep(1000);
		SerialMETA.write(new byte[4] { 172, 219, 207, 229 });
		SerialMETA.write(new byte[4] { 172, 219, 207, 229 });
		for (int i = 0; i < 5; i++)
		{
			Thread.Sleep(1000);
			byte[] array = new byte[8];
			int offset = 0;
			Binary.WriteU16LE(array, ref offset, mSequence);
			Binary.WriteU16LE(array, ref offset, 8);
			Binary.WriteU16LE(array, ref offset, 0);
			Binary.WriteU16LE(array, ref offset, 0);
			var (flag, array2) = SendApCommand(BuildApPacket(array), reply: true);
			if (flag && array2 != null && array2.Length >= 4)
			{
				int offset2 = 0;
				ushort num = Binary.ReadU16LE(array2, ref offset2);
				ushort num2 = Binary.ReadU16LE(array2, ref offset2);
				if (num2 == 9)
				{
					mSequence = (ushort)(num + 1);
					return true;
				}
			}
		}
		return false;
	}

	public METAApi.DEVICE_INFO_CNF SP_META_Get_DevInfo()
	{
		object result = null;
		byte[] array = new byte[4];
		int offset = 0;
		Binary.WriteU16LE(array, ref offset, mSequence);
		Binary.WriteU16LE(array, ref offset, 133);
		var (flag, array2) = SendApCommand(BuildApPacket(array), reply: true);
		if (flag && array2 != null && array2.Length >= 4)
		{
			int offset2 = 0;
			ushort num = Binary.ReadU16LE(array2, ref offset2);
			ushort num2 = Binary.ReadU16LE(array2, ref offset2);
			if (num2 != 134)
			{
				return (METAApi.DEVICE_INFO_CNF)result;
			}
			METAApi.DEVICE_INFO_CNF dEVICE_INFO_CNF = new METAApi.DEVICE_INFO_CNF();
			dEVICE_INFO_CNF.BbChip = array2.AsSpan(offset2, 64).ToArray();
			offset2 += 64;
			dEVICE_INFO_CNF.EcoVer = array2.AsSpan(offset2, 4).ToArray();
			offset2 += 4;
			dEVICE_INFO_CNF.SwTime = array2.AsSpan(offset2, 64).ToArray();
			offset2 += 64;
			dEVICE_INFO_CNF.DspFw = array2.AsSpan(offset2, 64).ToArray();
			offset2 += 64;
			dEVICE_INFO_CNF.DspPatch = array2.AsSpan(offset2, 64).ToArray();
			offset2 += 64;
			dEVICE_INFO_CNF.SwVer = array2.AsSpan(offset2, 64).ToArray();
			offset2 += 64;
			dEVICE_INFO_CNF.HwVer = array2.AsSpan(offset2, 64).ToArray();
			offset2 += 64;
			dEVICE_INFO_CNF.MelodyVer = array2.AsSpan(offset2, 64).ToArray();
			offset2 += 64;
			dEVICE_INFO_CNF.Status = array2[offset2];
			mSequence = (ushort)(num + 1);
			return dEVICE_INFO_CNF;
		}
		return (METAApi.DEVICE_INFO_CNF)result;
	}

	public string SP_META_GetTargetBuildProp(string prop)
	{
		string message = "[FAILED] FAILED TO GET BUILD PROP DATA";
		byte[] array = new byte[68];
		int offset = 0;
		Binary.WriteU16LE(array, ref offset, mSequence);
		Binary.WriteU16LE(array, ref offset, 137);
		if (!string.IsNullOrEmpty(prop))
		{
			byte[] bytes = Encoding.UTF8.GetBytes(prop);
			int length = Math.Min(bytes.Length, 64);
			bytes.AsSpan(0, length).CopyTo(array.AsSpan(4));
		}
		var (flag, array2) = SendApCommand(BuildApPacket(array), reply: true);
		if (flag && array2 != null && array2.Length >= 4)
		{
			int offset2 = 0;
			ushort num = Binary.ReadU16LE(array2, ref offset2);
			ushort num2 = Binary.ReadU16LE(array2, ref offset2);
			if (num2 != 138)
			{
				throw new Exception(message);
			}
			int num3 = Math.Min(128, array2.Length - offset2);
			int num4 = Array.IndexOf(array2, (byte)0, offset2, num3);
			if (num4 < 0)
			{
				num4 = offset2 + num3;
			}
			mSequence = (ushort)(num + 1);
			return Encoding.UTF8.GetString(array2, offset2, num4 - offset2);
		}
		return string.Empty;
	}

	public METAApi.FILE_OPERATION_GETFILEINFO_CNF SP_META_File_Operation_GetFileInfo(uint index)
	{
		string message = "[FAILED] FAILED TO GET FILE INFO";
		byte[] array = new byte[328];
		int offset = 0;
		Binary.WriteU16LE(array, ref offset, mSequence);
		Binary.WriteU16LE(array, ref offset, 163);
		Binary.WriteU16LE(array, ref offset, 1);
		Binary.WriteU16LE(array, ref offset, 0);
		Binary.WriteU32LE(array, ref offset, index);
		offset += 316;
		var (flag, array2) = SendApCommand(BuildApPacket(array), reply: true);
		bool flag2;
		int offset2;
		ushort num;
		ushort num2;
		ushort num3;
		if (flag)
		{
			if (array2 != null)
			{
				if (flag2 = array2.Length < 272)
				{
					throw new Exception(message);
				}
				offset2 = 0;
				num = Binary.ReadU16LE(array2, ref offset2);
				num2 = Binary.ReadU16LE(array2, ref offset2);
				num3 = Binary.ReadU16LE(array2, ref offset2);
				Binary.ReadU16LE(array2, ref offset2);
				if (num2 != 164 || num3 != 1)
				{
					throw new Exception(message);
				}
				METAApi.FILE_OPERATION_GETFILEINFO_CNF fILE_OPERATION_GETFILEINFO_CNF = new METAApi.FILE_OPERATION_GETFILEINFO_CNF();
				fILE_OPERATION_GETFILEINFO_CNF.FileType = Binary.ReadU32LE(array2, ref offset2);
				fILE_OPERATION_GETFILEINFO_CNF.FileSize = Binary.ReadU32LE(array2, ref offset2);
				fILE_OPERATION_GETFILEINFO_CNF.FileName = new byte[256];
				Buffer.BlockCopy(array2, offset2, fILE_OPERATION_GETFILEINFO_CNF.FileName, 0, 256);
				mSequence = (ushort)(num + 1);
				return fILE_OPERATION_GETFILEINFO_CNF;
			}
			flag2 = true;
		}
		else
		{
			flag2 = true;
		}
		if (flag2)
		{
			throw new Exception(message);
		}
		offset2 = 0;
		num = Binary.ReadU16LE(array2, ref offset2);
		num2 = Binary.ReadU16LE(array2, ref offset2);
		num3 = Binary.ReadU16LE(array2, ref offset2);
		Binary.ReadU16LE(array2, ref offset2);
		if (num2 == 164 && num3 == 1)
		{
			METAApi.FILE_OPERATION_GETFILEINFO_CNF fILE_OPERATION_GETFILEINFO_CNF = new METAApi.FILE_OPERATION_GETFILEINFO_CNF();
			fILE_OPERATION_GETFILEINFO_CNF.FileType = Binary.ReadU32LE(array2, ref offset2);
			fILE_OPERATION_GETFILEINFO_CNF.FileSize = Binary.ReadU32LE(array2, ref offset2);
			fILE_OPERATION_GETFILEINFO_CNF.FileName = new byte[256];
			Buffer.BlockCopy(array2, offset2, fILE_OPERATION_GETFILEINFO_CNF.FileName, 0, 256);
			mSequence = (ushort)(num + 1);
			return fILE_OPERATION_GETFILEINFO_CNF;
		}
		throw new Exception(message);
	}

	public METAApi.FILE_OPERATION_PARSE_CNF SP_META_File_Operation_Parse(string path, string filter)
	{
		string text = "*";
		string message = "[FAILED] FAILED TO GET FILE COUNT";
		filter = ((!string.IsNullOrEmpty(filter)) ? filter : text);
		byte[] array = new byte[520];
		int offset = 0;
		Binary.WriteU16LE(array, ref offset, mSequence);
		Binary.WriteU16LE(array, ref offset, 163);
		Binary.WriteU32LE(array, ref offset, 0u);
		byte[] bytes = Encoding.UTF8.GetBytes(path);
		int length = Math.Min(bytes.Length, 255);
		bytes.AsSpan(0, length).CopyTo(array.AsSpan(offset));
		offset += 256;
		byte[] bytes2 = Encoding.UTF8.GetBytes(filter);
		int length2 = Math.Min(bytes2.Length, 255);
		bytes2.AsSpan(0, length2).CopyTo(array.AsSpan(offset));
		offset += 256;
		var (flag, array2) = SendApCommand(BuildApPacket(array), reply: true);
		if (flag && array2 != null && array2.Length >= 8)
		{
			int offset2 = 0;
			ushort num = Binary.ReadU16LE(array2, ref offset2);
			ushort num2 = Binary.ReadU16LE(array2, ref offset2);
			uint num3 = Binary.ReadU32LE(array2, ref offset2);
			if (num2 != 164 || num3 != 0)
			{
				throw new Exception(message);
			}
			METAApi.FILE_OPERATION_PARSE_CNF fILE_OPERATION_PARSE_CNF = new METAApi.FILE_OPERATION_PARSE_CNF();
			int num4 = array2.Length - offset2;
			byte[] array3 = new byte[num4];
			Buffer.BlockCopy(array2, offset2, array3, 0, num4);
			int offset3 = 0;
			fILE_OPERATION_PARSE_CNF.FileCount = Binary.ReadU32LE(array3, ref offset3);
			mSequence = (ushort)(num + 1);
			return fILE_OPERATION_PARSE_CNF;
		}
		throw new Exception(message);
	}

	public byte[] SP_META_File_Operation_ReceiveFile(string filepath, uint size)
	{
		string oldValue = "//";
		string newValue = "/";
		string message = "read header failed";
		string message2 = "[FAILED] INVALID HEADER";
		string message3 = "read req failed";
		string message4 = "[FAILED] FAILED TO READ FILE DATA";
		string message5 = "[FAILED] Read cnf failed";
		string message6 = "[FAILED] INVALID FILE READ START STREAM STAGE";
		string message7 = "[FAILED] INVALID FILE READ END STREAM STAGE";
		string message8 = "[FAILED] Read crc failed";
		string message9 = "[FAILED] INVALID FILE READ CONTINUOUS STREAM STAGE";
		string message10 = "[FAILED] INVALID PEER MSG LEN";
		string message11 = "[FAILED] Read prefix failed";
		string message12 = "[FAILED] Read payload failed";
		string message13 = "[FAILED] Read CRC failed";
		filepath = (filepath ?? string.Empty).Replace(oldValue, newValue);
		byte[] array = new byte[520];
		int offset = 0;
		Binary.WriteU16LE(array, ref offset, mSequence);
		Binary.WriteU16LE(array, ref offset, 163);
		Binary.WriteU16LE(array, ref offset, 3);
		Binary.WriteU16LE(array, ref offset, 0);
		byte[] bytes = Encoding.UTF8.GetBytes(filepath);
		Array.Copy(bytes, 0, array, offset, Math.Min(bytes.Length, 512));
		SendApCommand(BuildApPacket(array), reply: false);
		int num = (int)((size != 0 && size <= int.MaxValue) ? size : 0);
		MemoryStream memoryStream = ((num <= 0) ? new MemoryStream() : new MemoryStream(num));
		while (true)
		{
			byte[] array2 = new byte[8];
			if (!ReadMetaPayload(array2, 8))
			{
				throw new Exception(message);
			}
			if (array2[0] != 85)
			{
				throw new Exception(message2);
			}
			int offset2 = 1;
			ushort num2 = Binary.ReadU16BE(array2, ref offset2);
			_ = array2[offset2++];
			Binary.ReadU16LE(array2, ref offset2);
			ushort num3 = Binary.ReadU16LE(array2, ref offset2);
			byte[] array3 = new byte[8];
			if (ReadMetaPayload(array3, 8))
			{
				int offset3 = 0;
				ushort num4 = Binary.ReadU16LE(array3, ref offset3);
				ushort num5 = Binary.ReadU16LE(array3, ref offset3);
				ushort num6 = Binary.ReadU16LE(array3, ref offset3);
				if (num5 == 164 && num6 == 3)
				{
					byte[] array4 = new byte[268];
					if (!ReadMetaPayload(array4, array4.Length))
					{
						throw new Exception(message5);
					}
					int offset4 = 0;
					uint num7 = Binary.ReadU32LE(array4, ref offset4);
					uint num8 = Binary.ReadU32LE(array4, ref offset4);
					Binary.ReadU32LE(array4, ref offset4);
					offset4 += 256;
					if (num7 != 0 || num8 == 3)
					{
						if (num8 != 6)
						{
							if (num7 != 0 && num8 != 2)
							{
								throw new Exception(message9);
							}
							int num9 = ((num3 >= 8) ? num3 : num2);
							if (num9 >= 8)
							{
								int num10 = num9 - 8;
								if (!ReadMetaPayload(new byte[8], 8))
								{
									throw new Exception(message11);
								}
								if (num10 > 0)
								{
									byte[] array5 = new byte[num10];
									if (!ReadMetaPayload(array5, num10))
									{
										break;
									}
									memoryStream.Write(array5, 0, array5.Length);
								}
								if (ReadMetaPayload(new byte[1], 1))
								{
									continue;
								}
								throw new Exception(message13);
							}
							throw new Exception(message10);
						}
						if (size != 0 && num7 != size)
						{
							throw new Exception(message7);
						}
						if (!ReadMetaPayload(new byte[1], 1))
						{
							throw new Exception(message8);
						}
						mSequence = (ushort)(num4 + 1);
						return memoryStream.ToArray();
					}
					throw new Exception(message6);
				}
				throw new Exception(message4);
			}
			throw new Exception(message3);
		}
		throw new Exception(message12);
	}

	public METAApi.QUERY_APDBPATH_CNF SP_META_Query_APDBPath()
	{
		string message = "[FAILED] FAILED TO QUERY APDB PATH";
		byte[] array = new byte[4];
		int offset = 0;
		Binary.WriteU16LE(array, ref offset, mSequence);
		Binary.WriteU16LE(array, ref offset, 173);
		var (flag, array2) = SendApCommand(BuildApPacket(array), reply: true);
		if (!flag)
		{
			throw new Exception(message);
		}
		if (array2 != null)
		{
			if (array2.Length >= 132)
			{
				int offset2 = 0;
				ushort num = Binary.ReadU16LE(array2, ref offset2);
				ushort num2 = Binary.ReadU16LE(array2, ref offset2);
				if (num2 == 174)
				{
					METAApi.QUERY_APDBPATH_CNF qUERY_APDBPATH_CNF = new METAApi.QUERY_APDBPATH_CNF();
					qUERY_APDBPATH_CNF.ApdbPath = new byte[128];
					METAApi.QUERY_APDBPATH_CNF qUERY_APDBPATH_CNF2 = qUERY_APDBPATH_CNF;
					int val = array2.Length - offset2;
					int num3 = Math.Min(128, val);
					if (num3 <= 0)
					{
						mSequence = (ushort)(num + 1);
						return qUERY_APDBPATH_CNF2;
					}
					Buffer.BlockCopy(array2, offset2, qUERY_APDBPATH_CNF2.ApdbPath, 0, num3);
					mSequence = (ushort)(num + 1);
					return qUERY_APDBPATH_CNF2;
				}
				throw new Exception(message);
			}
			throw new Exception(message);
		}
		throw new Exception(message);
	}

	public METAApi.MODEM_QUERY_MDDBPATH_CNF SP_META_MODEM_Query_MDDBPath(uint modemId)
	{
		string message = "[FAILED] FAILED TO QUERY MODEM DB PATH";
		byte[] array = new byte[16];
		int offset = 0;
		Binary.WriteU16LE(array, ref offset, mSequence);
		Binary.WriteU16LE(array, ref offset, 147);
		Binary.WriteU32LE(array, ref offset, 9u);
		Binary.WriteU32LE(array, ref offset, modemId);
		Binary.WriteU32LE(array, ref offset, 0u);
		var (flag, array2) = SendApCommand(BuildApPacket(array), reply: true);
		bool flag2;
		if (flag)
		{
			if (array2 != null)
			{
				if (!(flag2 = array2.Length < 72))
				{
					int offset2 = 0;
					ushort num = Binary.ReadU16LE(array2, ref offset2);
					ushort num2 = Binary.ReadU16LE(array2, ref offset2);
					uint num3 = Binary.ReadU32LE(array2, ref offset2);
					if (num2 != 148 || num3 != 9)
					{
						throw new Exception(message);
					}
					METAApi.MODEM_QUERY_MDDBPATH_CNF mODEM_QUERY_MDDBPATH_CNF = new METAApi.MODEM_QUERY_MDDBPATH_CNF();
					mODEM_QUERY_MDDBPATH_CNF.MddbPath = new byte[64];
					METAApi.MODEM_QUERY_MDDBPATH_CNF mODEM_QUERY_MDDBPATH_CNF2 = mODEM_QUERY_MDDBPATH_CNF;
					Buffer.BlockCopy(array2, offset2, mODEM_QUERY_MDDBPATH_CNF2.MddbPath, 0, 64);
					mSequence = (ushort)(num + 1);
					return mODEM_QUERY_MDDBPATH_CNF2;
				}
				throw new Exception(message);
			}
			flag2 = true;
		}
		else
		{
			flag2 = true;
		}
		if (!flag2)
		{
			int offset2 = 0;
			ushort num = Binary.ReadU16LE(array2, ref offset2);
			ushort num2 = Binary.ReadU16LE(array2, ref offset2);
			uint num3 = Binary.ReadU32LE(array2, ref offset2);
			if (num2 == 148 && num3 == 9)
			{
				METAApi.MODEM_QUERY_MDDBPATH_CNF mODEM_QUERY_MDDBPATH_CNF = new METAApi.MODEM_QUERY_MDDBPATH_CNF();
				mODEM_QUERY_MDDBPATH_CNF.MddbPath = new byte[64];
				METAApi.MODEM_QUERY_MDDBPATH_CNF mODEM_QUERY_MDDBPATH_CNF2 = mODEM_QUERY_MDDBPATH_CNF;
				Buffer.BlockCopy(array2, offset2, mODEM_QUERY_MDDBPATH_CNF2.MddbPath, 0, 64);
				mSequence = (ushort)(num + 1);
				return mODEM_QUERY_MDDBPATH_CNF2;
			}
			throw new Exception(message);
		}
		throw new Exception(message);
	}

	public ushort SP_META_Nvram_Get_LIDNumber(byte[] data, string lidName)
	{
		string message = "NVRAM blob empty";
		string message2 = "[FAILED] FAILED TO RETRIVE NV LID";
		string message3 = "[FAILED] INVALID LID OFFSET";
		string message4 = "[FAILED] INVALID LID DATA";
		if (data != null && data.Length != 0)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(lidName);
			int num = data.IndexOf(bytes);
			if (num < 0)
			{
				throw new Exception(message2);
			}
			if (num >= 4)
			{
				int offset = num - 4;
				ushort num2 = Binary.ReadU16LE(data, ref offset);
				if (num2 == 0 || num + num2 + 2 > data.Length)
				{
					throw new Exception(message4);
				}
				int num3 = num + num2;
				return (ushort)(data[num3] | (data[num3 + 1] << 8));
			}
			throw new Exception(message3);
		}
		throw new Exception(message);
	}

	public byte[] SP_META_Nvram_Read(ushort lid, ushort rid)
	{
		string message = "[FAILED] FAILED TO READ DATA";
		string message2 = "[PROTOCOL ERR] INVALID NVRAM DATA";
		byte[] arg = new byte[8];
		int offset = 0;
		Binary.WriteU16LE(arg, ref offset, mSequence);
		Binary.WriteU16LE(arg, ref offset, 22);
		Binary.WriteU16LE(arg, ref offset, lid);
		Binary.WriteU16LE(arg, ref offset, rid);
		byte[] data = Array.Empty<byte>();
		if (SendApCommandWithData(ref arg, ref data))
		{
			int offset2 = 0;
			ushort num = Binary.ReadU16LE(arg, ref offset2);
			ushort num2 = Binary.ReadU16LE(arg, ref offset2);
			ushort num3 = Binary.ReadU16LE(arg, ref offset2);
			ushort num4 = Binary.ReadU16LE(arg, ref offset2);
			if (num2 == 23 && num3 == lid && num4 == rid)
			{
				ushort num5 = Binary.ReadU16LE(arg, ref offset2);
				if (num5 != 1)
				{
					throw new Exception(message);
				}
				if (data != null && data.Length >= 8)
				{
					int offset3 = 0;
					ushort num6 = Binary.ReadU16LE(data, ref offset3);
					ushort num7 = Binary.ReadU16LE(data, ref offset3);
					ushort num8 = Binary.ReadU16LE(data, ref offset3);
					ushort num9 = Binary.ReadU16LE(data, ref offset3);
					if ((num6 | num7 | num8 | num9) != 0)
					{
						throw new Exception(message);
					}
					int num10 = data.Length - offset3;
					byte[] array = new byte[num10];
					Buffer.BlockCopy(data, offset3, array, 0, num10);
					mSequence = (ushort)(num + 1);
					return array;
				}
				throw new Exception(message2);
			}
			throw new Exception(message);
		}
		throw new Exception(message);
	}

	public void SP_META_Nvram_Write(ushort lid, ushort rid, byte[] payload)
	{
		string message = "[FAILED] FAILED TO WRITE DATA";
		byte[] arg = new byte[8];
		int offset = 0;
		Binary.WriteU16LE(arg, ref offset, mSequence);
		Binary.WriteU16LE(arg, ref offset, 24);
		Binary.WriteU16LE(arg, ref offset, lid);
		Binary.WriteU16LE(arg, ref offset, rid);
		byte[] data = new byte[8 + payload.Length];
		int offset2 = 0;
		Binary.WriteU16LE(data, ref offset2, 0);
		Binary.WriteU16LE(data, ref offset2, 0);
		Binary.WriteU16LE(data, ref offset2, 0);
		Binary.WriteU16LE(data, ref offset2, 0);
		Buffer.BlockCopy(payload, 0, data, offset2, payload.Length);
		if (SendApCommandWithData(ref arg, ref data))
		{
			int offset3 = 0;
			ushort num = Binary.ReadU16LE(arg, ref offset3);
			ushort num2 = Binary.ReadU16LE(arg, ref offset3);
			ushort num3 = Binary.ReadU16LE(arg, ref offset3);
			Binary.ReadU16LE(arg, ref offset3);
			if (num2 == 25 && num3 == lid)
			{
				ushort num4 = Binary.ReadU16LE(arg, ref offset3);
				if (num4 != 1)
				{
					throw new Exception(message);
				}
				mSequence = (ushort)(num + 1);
				return;
			}
			throw new Exception(message);
		}
		throw new Exception(message);
	}

	public bool SP_META_Customer_Operation_CMD_Format()
	{
		byte[] arg = new byte[12];
		int offset = 0;
		Binary.WriteU16LE(arg, ref offset, mSequence);
		Binary.WriteU16LE(arg, ref offset, 121);
		Binary.WriteU16LE(arg, ref offset, 0);
		Binary.WriteU16LE(arg, ref offset, 0);
		Binary.WriteU32LE(arg, ref offset, 1u);
		byte[] data = new byte[1032];
		int offset2 = 0;
		Binary.WriteU16LE(data, ref offset2, 1024);
		Binary.WriteU16LE(data, ref offset2, 1);
		Binary.WriteU16LE(data, ref offset2, 0);
		Binary.WriteU16LE(data, ref offset2, 1);
		if (!SendApCommandWithData(ref arg, ref data))
		{
			return false;
		}
		int offset3 = 0;
		ushort num = Binary.ReadU16LE(arg, ref offset3);
		ushort num2 = Binary.ReadU16LE(arg, ref offset3);
		if (num2 != 122)
		{
			return false;
		}
		mSequence = (ushort)(num + 1);
		return true;
	}

	public void SP_META_Customer_Operation_CMD_Shell(byte[] cmd)
	{
		string message = "[FAILED] SHELL DATA TOO LARGE";
		string message2 = "[FAILED] FAILED TO SEND SHELL CMD";
		string message3 = "[FAILED] FAILED TO SHELL CMD";
		byte[] arg = new byte[12];
		int offset = 0;
		Binary.WriteU16LE(arg, ref offset, mSequence);
		Binary.WriteU16LE(arg, ref offset, 121);
		Binary.WriteU16LE(arg, ref offset, 0);
		Binary.WriteU16LE(arg, ref offset, 0);
		Binary.WriteU32LE(arg, ref offset, 3u);
		if (cmd.Length <= 1024)
		{
			byte[] array = new byte[1024];
			Buffer.BlockCopy(cmd, 0, array, 0, cmd.Length);
			byte[] data = new byte[8 + array.Length];
			int offset2 = 0;
			Binary.WriteU16LE(data, ref offset2, (ushort)array.Length);
			Binary.WriteU16LE(data, ref offset2, 1);
			Binary.WriteU16LE(data, ref offset2, 0);
			Binary.WriteU16LE(data, ref offset2, 1);
			Buffer.BlockCopy(array, 0, data, offset2, array.Length);
			if (SendApCommandWithData(ref arg, ref data))
			{
				int offset3 = 0;
				ushort num = Binary.ReadU16LE(arg, ref offset3);
				ushort num2 = Binary.ReadU16LE(arg, ref offset3);
				if (num2 != 122)
				{
					throw new Exception(message3);
				}
				mSequence = (ushort)(num + 1);
				return;
			}
			throw new Exception(message2);
		}
		throw new Exception(message);
	}

	public byte[] SP_META_Customer_Operation_CMD_Shell2(string cmd, bool base64 = false)
	{
		string message = "[FAILED] FAILED TO SHELL CMD";
		byte[] bytes = Encoding.UTF8.GetBytes(cmd);
		byte[] arg = new byte[12];
		int offset = 0;
		Binary.WriteU16LE(arg, ref offset, mSequence);
		Binary.WriteU16LE(arg, ref offset, 121);
		Binary.WriteU16LE(arg, ref offset, 0);
		Binary.WriteU16LE(arg, ref offset, 0);
		Binary.WriteU32LE(arg, ref offset, 3u);
		byte[] data = new byte[8 + bytes.Length];
		int offset2 = 0;
		Binary.WriteU16LE(data, ref offset2, (ushort)bytes.Length);
		Binary.WriteU16LE(data, ref offset2, 1);
		Binary.WriteU16LE(data, ref offset2, 0);
		Binary.WriteU16LE(data, ref offset2, 1);
		bytes.AsSpan().CopyTo(data.AsSpan(offset2));
		if (SendApCommandWithData(ref arg, ref data))
		{
			int offset3 = 0;
			ushort num = Binary.ReadU16LE(arg, ref offset3);
			ushort num2 = Binary.ReadU16LE(arg, ref offset3);
			if (num2 != 122)
			{
				throw new Exception(message);
			}
			byte[] array = data;
			if (!base64)
			{
				mSequence = (ushort)(num + 1);
				return array;
			}
			if (array.Length != 0)
			{
				string text = Encoding.UTF8.GetString(array).Trim('\0', '\r', '\n', ' ');
				if (text.Length <= 0)
				{
					mSequence = (ushort)(num + 1);
					return array;
				}
				array = Convert.FromBase64String(text);
				mSequence = (ushort)(num + 1);
				return array;
			}
			mSequence = (ushort)(num + 1);
			return array;
		}
		return Array.Empty<byte>();
	}

	public byte[] SP_META_Nvram_Backup()
	{
		string message = "read header failed";
		string message2 = "read req failed";
		string message3 = "[FAILED] FAILED TO READ NVRAM DATA";
		string message4 = "[FAILED] Read cnf failed";
		string message5 = "[FAILED] Invalid payload length";
		string message6 = "[FAILED] Read prefix failed";
		string message7 = "[FAILED] Read payload failed";
		string message8 = "[FAILED] Read crc failed";
		byte[] array = new byte[1036];
		int offset = 0;
		Binary.WriteU16LE(array, ref offset, mSequence);
		Binary.WriteU16LE(array, ref offset, 94);
		Binary.WriteU16LE(array, ref offset, 0);
		Binary.WriteU16LE(array, ref offset, 0);
		Binary.WriteU32LE(array, ref offset, 0u);
		Array.Clear(array, 12, 1024);
		SendApCommand(BuildApPacket(array), reply: false);
		using MemoryStream memoryStream = new MemoryStream();
		bool flag = true;
		ushort num2;
		while (true)
		{
			byte[] array2 = new byte[8];
			if (ReadMetaPayload(array2, 8))
			{
				int offset2 = 0;
				ushort num = Binary.ReadU16LE(array2, ref offset2);
				offset2 += 6;
				byte[] array3 = new byte[8];
				if (!ReadMetaPayload(array3, 8))
				{
					throw new Exception(message2);
				}
				int offset3 = 0;
				num2 = Binary.ReadU16LE(array3, ref offset3);
				ushort num3 = Binary.ReadU16LE(array3, ref offset3);
				if (flag)
				{
					if (num3 != 95)
					{
						throw new Exception(message3);
					}
					flag = false;
				}
				byte[] data = new byte[8];
				if (ReadMetaPayload(data, 8))
				{
					if (num == 0)
					{
						break;
					}
					int num4 = num - 8;
					if (num4 < 0)
					{
						throw new Exception(message5);
					}
					if (!ReadMetaPayload(new byte[8], 8))
					{
						throw new Exception(message6);
					}
					if (num4 > 0)
					{
						byte[] array4 = new byte[num4];
						if (!ReadMetaPayload(array4, num4))
						{
							throw new Exception(message7);
						}
						memoryStream.Write(array4, 0, array4.Length);
					}
					if (!ReadMetaPayload(new byte[1], 1))
					{
						throw new Exception(message8);
					}
					continue;
				}
				throw new Exception(message4);
			}
			throw new Exception(message);
		}
		mSequence = (ushort)(num2 + 1);
		return memoryStream.ToArray();
	}

	public bool SP_META_RebootTarget()
	{
		byte[] array = new byte[8];
		int offset = 0;
		Binary.WriteU16LE(array, ref offset, mSequence);
		Binary.WriteU16LE(array, ref offset, 10);
		Binary.WriteU16LE(array, ref offset, 2);
		Binary.WriteU16LE(array, ref offset, 0);
		if (SendApCommand(BuildApPacket(array), reply: false).success)
		{
			byte[] array2 = new byte[4];
			int offset2 = 0;
			Binary.WriteU32LE(array2, ref offset2, 3855604652u);
			SerialMETA.write(array2);
			SerialMETA.write(array2);
			METAConfig.METAReady = false;
			return true;
		}
		return false;
	}

	public (bool success, byte[] response) SendApCommand(byte[] cmd, bool reply)
	{
		object item = null;
		if (cmd != null && cmd.Length != 0)
		{
			byte b = CRC(cmd, cmd.Length);
			byte[] array = new byte[cmd.Length + 1];
			Buffer.BlockCopy(cmd, 0, array, 0, cmd.Length);
			array[cmd.Length] = b;
			if (!SerialMETA.write(array))
			{
				return (success: false, response: (byte[])item);
			}
			if (!reply)
			{
				return (success: true, response: Array.Empty<byte>());
			}
			byte[] array2 = new byte[8];
			if (!ReadMetaPayload(array2, 8))
			{
				return (success: false, response: (byte[])item);
			}
			if (array2[0] == 85)
			{
				int num = array2[4] | (array2[5] << 8);
				int num2 = array2[6] | (array2[7] << 8);
				byte[] array3 = ((num <= 0) ? Array.Empty<byte>() : new byte[num]);
				if (num <= 0 || ReadMetaPayload(array3, num))
				{
					if (num2 > 0)
					{
						byte[] data = new byte[num2];
						if (!ReadMetaPayload(data, num2))
						{
							return (success: false, response: (byte[])item);
						}
					}
					byte[] data2 = new byte[1];
					if (ReadMetaPayload(data2, 1))
					{
						if (num >= 2)
						{
							mSequence = (ushort)(array3[0] | (array3[1] << 8));
							mSequence++;
						}
						return (success: true, response: array3);
					}
					return (success: false, response: (byte[])item);
				}
				return (success: false, response: (byte[])item);
			}
			return (success: false, response: (byte[])item);
		}
		return (success: false, response: (byte[])item);
	}

	public bool SendApCommandWithData(ref byte[] arg, ref byte[] data)
	{
		byte[] array = BuildApPacket(arg, data);
		byte b = CRC(array, array.Length);
		byte[] array2 = new byte[array.Length + 1];
		Buffer.BlockCopy(array, 0, array2, 0, array.Length);
		array2[array2.Length - 1] = b;
		byte[] array3 = UnescapeMetaBytes(array2.AsSpan(1, array2.Length - 1).ToArray());
		byte[] array4 = new byte[1 + array3.Length];
		array4[0] = 85;
		Buffer.BlockCopy(array3, 0, array4, 1, array3.Length);
		if (!SerialMETA.write(array4))
		{
			return false;
		}
		arg = Array.Empty<byte>();
		data = Array.Empty<byte>();
		byte[] array5 = new byte[8];
		if (!ReadMetaPayload(array5, 8))
		{
			return false;
		}
		if (array5[0] == 85)
		{
			int num = array5[4] | (array5[5] << 8);
			int num2 = array5[6] | (array5[7] << 8);
			if (num > 0)
			{
				arg = new byte[num];
				if (!ReadMetaPayload(arg, num))
				{
					return false;
				}
			}
			if (num2 > 0)
			{
				data = new byte[num2];
				if (!ReadMetaPayload(data, num2))
				{
					return false;
				}
			}
			byte[] data2 = new byte[1];
			if (ReadMetaPayload(data2, 1))
			{
				if (num >= 2)
				{
					mSequence = (ushort)(arg[0] | (arg[1] << 8));
					mSequence++;
				}
				return true;
			}
			return false;
		}
		return false;
	}

	private bool ReadMetaPayload(byte[] data, int len)
	{
		int num = 0;
		bool flag = false;
		List<byte> list = new List<byte>(len + 8);
		while (true)
		{
			if (num >= len)
			{
				return true;
			}
			byte[] array = SerialMETA.read(1);
			if (array == null || array.Length != 1)
			{
				break;
			}
			byte b = array[0];
			if (!flag)
			{
				if (b != 90)
				{
					list.Add(b);
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				list.Add(90);
				list.Add(b);
				flag = false;
			}
			byte[] array2 = EscapeMetaBytes(list.ToArray());
			list.Clear();
			int num2 = Math.Min(array2.Length, len - num);
			for (int i = 0; i < num2; i++)
			{
				data[num++] = array2[i];
			}
			if (array2.Length > num2)
			{
				for (int j = num2; j < array2.Length; j++)
				{
					list.Add(array2[j]);
				}
			}
		}
		return false;
	}

	public byte[] EscapeMetaBytes(byte[] buff)
	{
		string hex = "5a01";
		string hex2 = "a5";
		string hex3 = "5a5a";
		string hex4 = "5a";
		int index = 0;
		byte[] pattern = hex.HexToBytes();
		byte[] replacement = hex2.HexToBytes();
		byte[] pattern2 = hex3.HexToBytes();
		byte[] replacement2 = hex4.HexToBytes();
		while (true)
		{
			int num = Extentions.IndexOf(buff, pattern, index);
			if (num == -1)
			{
				break;
			}
			bool flag = num >= 2 && buff[num - 2] == 90 && buff[num - 1] == 90;
			bool flag2 = num == 0 || buff[num - 1] != 90;
			if (flag || flag2)
			{
				buff = buff.Replace(num, 2, replacement);
			}
			index = num + 1;
		}
		index = 0;
		while (true)
		{
			int num2 = Extentions.IndexOf(buff, pattern2, index);
			if (num2 != -1)
			{
				buff = buff.Replace(num2, 2, replacement2);
				index = num2 + 1;
				continue;
			}
			break;
		}
		return buff;
	}

	private byte[] UnescapeMetaBytes(byte[] buff)
	{
		string hex = "55";
		string hex2 = "5a01";
		string hex3 = "5a";
		string hex4 = "5a5a";
		int index = 0;
		byte[] pattern = hex.HexToBytes();
		byte[] replacement = hex2.HexToBytes();
		byte[] pattern2 = hex3.HexToBytes();
		byte[] replacement2 = hex4.HexToBytes();
		while (true)
		{
			int num = Extentions.IndexOf(buff, pattern, index);
			if (num == -1)
			{
				break;
			}
			buff = buff.Replace(num, 1, replacement);
			index = num + 2;
		}
		index = 0;
		while (true)
		{
			int num2 = Extentions.IndexOf(buff, pattern2, index);
			if (num2 != -1)
			{
				if (num2 < buff.Length - 1 && buff[num2 + 1] == 1)
				{
					index = num2 + 1;
					continue;
				}
				buff = buff.Replace(num2, 1, replacement2);
				index = num2 + 2;
				continue;
			}
			break;
		}
		return buff;
	}

	private byte CRC(byte[] buffer, int length)
	{
		byte b = 0;
		for (int i = 0; i < length; i++)
		{
			b ^= buffer[i];
		}
		return b;
	}

	private byte[] BuildApPacket(byte[] arg)
	{
		int num = arg.Length;
		int num2 = num + 4;
		byte[] array = new byte[8 + num];
		int num3 = 0;
		num3 = 1;
		array[0] = 85;
		Binary.WriteU16BE(array, ref num3, (ushort)num2);
		array[num3++] = 102;
		Binary.WriteU16LE(array, ref num3, (ushort)num);
		Binary.WriteU16LE(array, ref num3, 0);
		arg.AsSpan().CopyTo(array.AsSpan(num3));
		return array;
	}

	private byte[] BuildApPacket(byte[] arg, ReadOnlySpan<byte> data = default(ReadOnlySpan<byte>))
	{
		int num = arg.Length;
		int length = data.Length;
		int num2 = num + 4 + length;
		byte[] array = new byte[8 + num + length];
		int num3 = 0;
		num3 = 1;
		array[0] = 85;
		Binary.WriteU16BE(array, ref num3, (ushort)num2);
		array[num3++] = 102;
		Binary.WriteU16LE(array, ref num3, (ushort)num);
		Binary.WriteU16LE(array, ref num3, (ushort)length);
		if (num <= 0)
		{
			if (length > 0)
			{
				data.CopyTo(array.AsSpan(num3));
				return array;
			}
			return array;
		}
		Buffer.BlockCopy(arg, 0, array, num3, num);
		num3 += num;
		if (length > 0)
		{
			data.CopyTo(array.AsSpan(num3));
			return array;
		}
		return array;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
