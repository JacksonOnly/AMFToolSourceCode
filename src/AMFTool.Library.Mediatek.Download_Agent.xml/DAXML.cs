using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AMFTool.Library.Mediatek.Auth;
using AMFTool.Library.Mediatek.Config;
using AMFTool.Library.Mediatek.Download_Agent.xml.extension;
using AMFTool.Library.Mediatek.Exploit;
using AMFTool.Library.Usb;
using Restored.Namespace8;
using Restored.Namespace9;

namespace AMFTool.Library.Mediatek.Download_Agent.xml;

public class DAXML
{
	public MtkClass Mtk;

	public DAConfig Dacfg;

	public MtkConfig Config;

	public XmlFlashExt XmlExt;

	public XmlCmd XmlCmd;

	public Partition Partition;

	private StorageInfo Storage;

	private EmmcInfo Emmc;

	private UfsInfo Ufs;

	private NandInfo Nand;

	private NorInfo Nor;

	public DAXML(MtkClass mtk, DAConfig dacfg)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Mtk = mtk;
		Dacfg = dacfg;
		Config = mtk.Config;
		XmlExt = new XmlFlashExt(mtk, this);
		XmlCmd = new XmlCmd(mtk);
		Partition = new Partition(mtk, ReadGPT, ReadPMT);
	}

	private (int, object, long) GetStorage(object partType, long length)
	{
		string text = "nor";
		string text2 = "nand";
		string text3 = "ufs";
		string text4 = "sdc";
		if (Dacfg.FlashType == text)
		{
			return PartitionTypeAndSize(32, partType, length);
		}
		if (!(Dacfg.FlashType == text2))
		{
			if (!(Dacfg.FlashType == text3))
			{
				if (!(Dacfg.FlashType == text4))
				{
					return PartitionTypeAndSize(1, partType, length);
				}
				return PartitionTypeAndSize(2, partType, length);
			}
			if (partType is int num && num == 8)
			{
				partType = 3;
				return PartitionTypeAndSize(48, partType, length);
			}
			return PartitionTypeAndSize(48, partType, length);
		}
		return PartitionTypeAndSize(16, partType, length);
	}

	private (int, object, long) PartitionTypeAndSize(int storage = 0, object partType = null, long length = 0L)
	{
		object obj = null;
		string format = "{0}";
		string text = "user";
		string text2 = "EMMC-USER";
		string text3 = "boot1";
		string text4 = "EMMC-BOOT1";
		string text5 = "emmc";
		string text6 = "boot2";
		string text7 = "EMMC-BOOT2";
		string text8 = "gp1";
		string text9 = "EMMC-GP1";
		string text10 = "lu3";
		string text11 = "UFS-LUA2";
		string text12 = "lu1";
		string text13 = "UFS-LUA0";
		string text14 = "lu2";
		string text15 = "UFS-LUA1";
		string text16 = "lu4";
		string text17 = "rpmb";
		string text18 = "UFS-LUA3";
		string text19 = "NAND-WHOLE";
		string text20 = "NOR-WHOLE";
		if (!(storage == 1 || storage == 2))
		{
			if (storage == 48)
			{
				if (partType != null && !(string.Format(format, partType) == text10) && !(string.Format(format, partType) == text))
				{
					if (!(string.Format(format, partType) == text12) && !(string.Format(format, partType) == text3))
					{
						if (!(string.Format(format, partType) == text14) && !(string.Format(format, partType) == text6))
						{
							if (string.Format(format, partType) == text16 || string.Format(format, partType) == text17)
							{
								partType = text18;
								length = Math.Min(length, Ufs.Lu4Size);
							}
						}
						else
						{
							partType = text15;
							length = Math.Min(length, Ufs.Lu2Size);
						}
					}
					else
					{
						partType = text13;
						length = Math.Min(length, Ufs.Lu1Size);
					}
				}
				else
				{
					partType = text11;
					length = Math.Min(length, Ufs.Lu3Size);
				}
			}
			else if (new int[6] { 16, 18, 17, 19, 21, 20 }.Contains(storage))
			{
				partType = text19;
				length = Math.Min(length, Nand.TotalSize);
			}
			else if (new int[3] { 32, 34, 33 }.Contains(storage))
			{
				partType = text20;
				length = Math.Min(length, Nor.AvailableSize);
			}
		}
		else
		{
			storage = 1;
			if ((partType == obj) | (string.Format(format, partType) == text))
			{
				partType = text2;
			}
			else if (string.Format(format, partType) == text3)
			{
				partType = text4;
				if (Dacfg.FlashType == text5)
				{
					length = Math.Min(length, Emmc.Boot1Size);
				}
			}
			else if (string.Format(format, partType) == text6)
			{
				partType = text7;
				if (Dacfg.FlashType == text5)
				{
					length = Math.Min(length, Emmc.Boot2Size);
				}
			}
			else if (string.Format(format, partType) == text8)
			{
				partType = text9;
				if (Dacfg.FlashType == text5)
				{
					length = Math.Min(length, Emmc.Gp1Size);
				}
			}
		}
		return (storage, partType, length);
	}

	public (byte[], List<Partf>) ReadPMT(bool arg_01 = false, string arg_02 = "user")
	{
		return Partition.GetPmt(arg_01, arg_02);
	}

	public byte[] ReadGPT(long addr, long length, object partType, bool display = false)
	{
		object obj = null;
		string text = "emmc";
		string text2 = "lu3";
		string text3 = "user";
		string format = "{0}";
		string text4 = "CMD:UPLOAD-FILE";
		string text5 = "START";
		int? num = null;
		if (partType == obj)
		{
			partType = ((!(Dacfg.FlashType == text)) ? text2 : text3);
		}
		(int, object, long) storage = GetStorage(partType, length);
		(int, object, long) tuple = storage;
		if (tuple.Item1 != 0 || tuple.Item2 != null || tuple.Item3 != 0L)
		{
			tuple = storage;
			num = tuple.Item1;
			partType = tuple.Item2;
			length = tuple.Item3;
			if ((bool)SendCommand(XmlCmd.CmdReadFlash(string.Format(format, partType), (int)addr, (int)length), noAck: true))
			{
				string text6;
				object obj2;
				(text6, obj2) = GetCommandResult();
				if (text6 == text4)
				{
					Class33 @class = obj2 as Class33;
					if (@class !=null)
					{
						byte[] result = DownloadRaw(@class);
						(text6, obj2) = GetCommandResult();
						if ((string)obj2 == text5)
						{
							return result;
						}
					}
				}
				Ack();
				GetCommandResult();
				return Array.Empty<byte>();
			}
			return Array.Empty<byte>();
		}
		return Array.Empty<byte>();
	}

	public bool ReadFlash(long addr, long length, string fileName, string destination)
	{
		string location = "C:/file.bin";
		object obj = null;
		string text = "START";
		string text2 = "CMD:START";
		if (!(bool)SendCommand(XmlCmd.CmdReadPartition(fileName, location), noAck: true))
		{
			return false;
		}
		(string, object) commandResult = GetCommandResult();
		string item = commandResult.Item1;
		object item2 = commandResult.Item2;
		Class33 @class = item2 as Class33;
		if (@class !=null)
		{
			if (!File.Exists(destination))
			{
				if (!DownloadFile(@class, destination))
				{
					return false;
				}
				(item, item2) = GetCommandResult();
				if (!((string)item2 == text))
				{
					return false;
				}
				return item == text2;
			}
			File.Delete(destination);
			if (!DownloadFile(@class, destination))
			{
				return false;
			}
			(item, item2) = GetCommandResult();
			if (!((string)item2 == text))
			{
				return false;
			}
			return item == text2;
		}
		return false;
	}

	public byte[] ReadFlashByte(long addr, long length, object partType)
	{
		string format = "{0}";
		string text = "CMD:UPLOAD-FILE";
		object obj = null;
		string text2 = "START";
		int? num = null;
		(int, object, long) storage = GetStorage(partType, length);
		(int, object, long) tuple = storage;
		if (tuple.Item1 != 0 || tuple.Item2 != null || tuple.Item3 != 0L)
		{
			tuple = storage;
			num = tuple.Item1;
			partType = tuple.Item2;
			length = tuple.Item3;
			if (!(bool)SendCommand(XmlCmd.CmdReadFlash(string.Format(format, partType), addr, length), noAck: true))
			{
				return Array.Empty<byte>();
			}
			string text3;
			object obj2;
			(text3, obj2) = GetCommandResult();
			if (!(text3 == text))
			{
				return Array.Empty<byte>();
			}
			Class33 @class = obj2 as Class33;
			if (@class ==null)
			{
				return Array.Empty<byte>();
			}
			byte[] result = DownloadRaw(@class);
			(text3, obj2) = GetCommandResult();
			if (!((string)obj2 == text2))
			{
				return Array.Empty<byte>();
			}
			return result;
		}
		return Array.Empty<byte>();
	}

	public byte[] ReadFlashByName(string fileName)
	{
		string location = "C:/file.bin";
		object obj = null;
		string text = "START";
		if (!(bool)SendCommand(XmlCmd.CmdReadPartition(fileName, location), noAck: true))
		{
			return Array.Empty<byte>();
		}
		(string, object) commandResult = GetCommandResult();
		_ = commandResult.Item1;
		object item = commandResult.Item2;
		Class33 @class = item as Class33;
		if (@class !=null)
		{
			byte[] result = DownloadRaw(@class);
			(string, object) commandResult2 = GetCommandResult();
			_ = commandResult2.Item1;
			item = commandResult2.Item2;
			if (!((string)item == text))
			{
				return Array.Empty<byte>();
			}
			return result;
		}
		return Array.Empty<byte>();
	}

	public bool FormatFlash(long addr, long length, object partType = null)
	{
		object obj = null;
		string text = "emmc";
		string text2 = "lu3";
		string text3 = "user";
		int? num = null;
		(int, object, long) storage;
		(int?, object, long) tuple;
		(int?, object, long) tuple2;
		int? num2;
		if (partType != obj)
		{
			storage = GetStorage(partType, length);
			int? item = (num = storage.Item1);
			partType = storage.Item2;
			length = storage.Item3;
			tuple = (item, storage.Item2, storage.Item3);
			tuple2 = tuple;
			(num2, _, _) = tuple2;
			if (!((num2.GetValueOrDefault() == 0) & num2.HasValue) || tuple2.Item2 != null || tuple2.Item3 != 0L)
			{
				if ((bool)SendCommand(XmlCmd.CmdEraseFlash((string)partType, addr, length)))
				{
					return true;
				}
				return false;
			}
			return false;
		}
		if (Dacfg.FlashType == text)
		{
			partType = text3;
			storage = GetStorage(partType, length);
			int? item2 = (num = storage.Item1);
			partType = storage.Item2;
			length = storage.Item3;
			tuple = (item2, storage.Item2, storage.Item3);
			tuple2 = tuple;
			(num2, _, _) = tuple2;
			if (!((num2.GetValueOrDefault() == 0) & num2.HasValue) || tuple2.Item2 != null || tuple2.Item3 != 0L)
			{
				if ((bool)SendCommand(XmlCmd.CmdEraseFlash((string)partType, addr, length)))
				{
					return true;
				}
				return false;
			}
			return false;
		}
		partType = text2;
		storage = GetStorage(partType, length);
		int? item3 = (num = storage.Item1);
		partType = storage.Item2;
		length = storage.Item3;
		tuple = (item3, storage.Item2, storage.Item3);
		tuple2 = tuple;
		(num2, _, _) = tuple2;
		if (!((num2.GetValueOrDefault() == 0) & num2.HasValue) || tuple2.Item2 != null || tuple2.Item3 != 0L)
		{
			if ((bool)SendCommand(XmlCmd.CmdEraseFlash((string)partType, addr, length)))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public bool WriteFlash(string partition, object filePath)
	{
		string text = "CMD:DOWNLOAD-FILE";
		object obj = null;
		if ((bool)SendCommand(XmlCmd.CmdWritePartition(partition), noAck: true))
		{
			GetCommandResult();
			SendExist();
			GetCommandResult();
			SendExist();
			var (text2, obj2) = GetCommandResult();
			if (!(text2 == text))
			{
				return false;
			}
			DwnFile dwnFile = obj2 as DwnFile;
			if (dwnFile ==null)
			{
				return false;
			}
			if (!UploadSparse(dwnFile, filePath))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public bool FlashUpgrade(List<MTKList> listpartition, string pathXml)
	{
		int nextline = 1;
		int num = 0;
		string msg = "Failed!";
		string msg2 = "Writing ";
		string text = "[";
		string text2 = "]";
		string text3 = "->[";
		string msg3 = "... ";
		object obj = null;
		string msg4 = "OK";
		try
		{
			string text4 = File.ReadAllText(pathXml);
			if (!(bool)SendCommand(XmlCmd.CmdFlashUpdate(listpartition, pathXml, AppDomain.CurrentDomain.BaseDirectory), noAck: true))
			{
				UI.RichLogs(msg, UI.error, nextline: true);
				return false;
			}
			GetCommandResult();
			AckValue(text4.Length);
			GetResponse();
			AckValue(0L);
			GetResponse();
			XSend(Encoding.UTF8.GetBytes(text4));
			GetResponse();
			SendExist();
			GetCommandResult();
			SendExist();
			GetCommandResult();
			GetCommandResult();
			int num2 = 0;
			foreach (MTKList item in listpartition)
			{
				UI.RichLogs(msg2, UI.log);
				UI.RichLogs(text + item.FileName + text2, UI.info);
				UI.RichLogs(text3 + item.PartName + text2, UI.log);
				UI.RichLogs(msg3, UI.log);
				SendExist();
				GetCommandResult();
				SendExist();
				var (format, obj2) = GetCommandResult();
				Console.WriteLine(format, obj2);
				DwnFile dwnFile = obj2 as DwnFile;
				if (dwnFile ==null)
				{
					UI.RichLogs(msg, UI.error, nextline: true);
					break;
				}
				if (!UploadSparse(dwnFile, dwnFile.SourceFile))
				{
					UI.RichLogs(msg, UI.error, nextline: true);
					break;
				}
				UI.RichLogs(msg4, UI.ok, nextline: true);
				num2++;
			}
			return num2 == listpartition.Count;
		}
		catch
		{
			UI.RichLogs(msg, UI.error, (byte)nextline != 0, (byte)num != 0);
			return (byte)num != 0;
		}
	}

	public bool FormatAllFlash(List<MTKList> listpartition, string pathXml)
	{
		string msg = "Formating Partition... ";
		int num = 0;
		string partition = "ALL";
		string msg2 = "Failed!";
		int nextline = 1;
		string msg3 = "OK";
		string msg4 = "Analyzing Scatter ";
		string text = "";
		string msg5 = "... ";
		string msg6 = "Writing ";
		string text2 = "[";
		string text3 = "]";
		string text4 = "->[";
		object obj = null;
		try
		{
			UI.RichLogs(msg, UI.log);
			if ((bool)SendCommand(XmlCmd.CmdEraseAll(partition)))
			{
				string text5 = File.ReadAllText(pathXml);
				UI.RichLogs(msg3, UI.ok, nextline: true);
				UI.RichLogs(msg4, UI.log);
				UI.RichLogs(Path.GetFileName(pathXml) ?? text, UI.info, nextline: true);
				UI.RichLogs(msg5, UI.log);
				if ((bool)SendCommand(XmlCmd.CmdFlashAll(listpartition, pathXml), noAck: true))
				{
					GetCommandResult();
					AckValue(text5.Length);
					GetResponse();
					AckValue(0L);
					GetResponse();
					XSend(Encoding.UTF8.GetBytes(text5));
					GetResponse();
					GetCommandResult();
					UI.RichLogs(msg3, UI.ok, nextline: true);
					int num2 = 0;
					foreach (MTKList item2 in listpartition)
					{
						UI.RichLogs(msg6, UI.log);
						UI.RichLogs(text2 + item2.FileName + text3, UI.info);
						UI.RichLogs(text4 + item2.PartName + text3, UI.log);
						UI.RichLogs(msg5, UI.log);
						SendExist();
						GetCommandResult();
						SendExist();
						(string, object) commandResult = GetCommandResult();
						_ = commandResult.Item1;
						object item = commandResult.Item2;
						DwnFile dwnFile = item as DwnFile;
						if (dwnFile ==null)
						{
							UI.RichLogs(msg2, UI.error, nextline: true);
							break;
						}
						if (!UploadSparse(dwnFile, dwnFile.SourceFile))
						{
							UI.RichLogs(msg2, UI.error, nextline: true);
							break;
						}
						UI.RichLogs(msg3, UI.ok, nextline: true);
						num2++;
					}
					return num2 == listpartition.Count;
				}
				UI.RichLogs(msg2, UI.error, nextline: true);
				return false;
			}
			UI.RichLogs(msg2, UI.error, nextline: true);
			return false;
		}
		catch
		{
			UI.RichLogs(msg2, UI.error, (byte)nextline != 0, (byte)num != 0);
			return (byte)num != 0;
		}
	}

	private string GetField(object data, string fieldName)
	{
		object obj = null;
		string text = "<";
		string text2 = ">";
		string text3 = "</";
		byte[] array = data as byte[];
		string text4;
		int num;
		int num2;
		if (array !=null)
		{
			data = Encoding.UTF8.GetString(array);
			text4 = (string)data;
			num = text4.IndexOf(text + fieldName + text2, StringComparison.Ordinal);
			if (num == -1)
			{
				return string.Empty;
			}
			num += fieldName.Length + 2;
			num2 = text4.IndexOf(text3 + fieldName + text2, num, StringComparison.Ordinal);
			if (num2 == -1)
			{
				return string.Empty;
			}
			return text4.Substring(num, num2 - num);
		}
		text4 = (string)data;
		num = text4.IndexOf(text + fieldName + text2, StringComparison.Ordinal);
		if (num == -1)
		{
			return string.Empty;
		}
		num += fieldName.Length + 2;
		num2 = text4.IndexOf(text3 + fieldName + text2, num, StringComparison.Ordinal);
		if (num2 == -1)
		{
			return string.Empty;
		}
		return text4.Substring(num, num2 - num);
	}

	public byte[] XRead()
	{
		string value = "XRead error: Wrong magic";
		object result = null;
		string text = "XRead error: ";
		try
		{
			byte[] data = SerialMTK.Read(12);
			uint[] array = data.Unpack32();
			uint num = array[0];
			uint len = array[2];
			uint num2 = num;
			if (num2 != 4277071599u)
			{
				Console.WriteLine(value);
				return (byte[])result;
			}
			return SerialMTK.Read((int)len).ToArray();
		}
		catch (Exception ex)
		{
			Console.WriteLine(text + ex.Message);
			return (byte[])result;
		}
	}

	public bool XSend(object data, DataType dataType = (DataType)1, bool is64bit = false)
	{
		object obj = null;
		int num;
		byte[] data2;
		if (!utils.IsNumber(data))
		{
			if (data as string !=null)
			{
				data = Encoding.UTF8.GetBytes((string)data).Concat(new byte[1]).ToArray();
				num = ((byte[])data).Length;
				data2 = data as byte[];
				if (SerialMTK.Write(new uint[3]
				{
					4277071599u,
					(uint)dataType,
					(uint)num
				}.Pack()))
				{
					return SerialMTK.Write(data2);
				}
				return false;
			}
			data = (byte[])data;
			num = ((byte[])data).Length;
			data2 = data as byte[];
			if (SerialMTK.Write(new uint[3]
			{
				4277071599u,
				(uint)dataType,
				(uint)num
			}.Pack()))
			{
				return SerialMTK.Write(data2);
			}
			return false;
		}
		if (!is64bit)
		{
			data = BitConverter.GetBytes((int)data);
			num = 4;
			data2 = data as byte[];
			if (SerialMTK.Write(new uint[3]
			{
				4277071599u,
				(uint)dataType,
				4u
			}.Pack()))
			{
				return SerialMTK.Write(data2);
			}
			return false;
		}
		data = BitConverter.GetBytes((long)data);
		num = 8;
		data2 = data as byte[];
		if (SerialMTK.Write(new uint[3]
		{
			4277071599u,
			(uint)dataType,
			8u
		}.Pack()))
		{
			return SerialMTK.Write(data2);
		}
		return false;
	}

	public bool Ack()
	{
		string data = "OK\0";
		return XSend(data);
	}

	public bool AckValue(long length)
	{
		string format = "OK@0x{0:X}\0";
		return XSend(string.Format(format, length));
	}

	private bool AckText(string text)
	{
		string text2 = "OK@";
		string text3 = "\0";
		return XSend(text2 + text + text3);
	}

	private bool SendExist()
	{
		string data = "OK@NOT-EXISTS";
		return XSend(data);
	}

	private bool SetupEnvironment()
	{
		string checksumLevel = "NONE";
		string batteryExist = "AUTO-DETECT";
		string logChannel = "UART";
		string version = "1.1";
		return (bool)SendCommand(XmlCmd.CmdSetRuntimeParameter(checksumLevel, batteryExist, (LogLevel)2, logChannel, FtSystemOSE.OS_WINDOWS, version));
	}

	public object SendCommand(string xml, bool noAck = false)
	{
		string text = "Sending Command : ";
		string text2 = "OK";
		string text3 = "CMD:END";
		string text4 = "2nd DA address is invalid. reset.\r\n";
		string text5 = "CMD:START";
		string text6 = "ERR!UNSUPPORTED";
		string value = "ERR!";
		Console.WriteLine(text + xml);
		if (XSend(xml))
		{
			string response = GetResponse();
			if (!(response == text2))
			{
				if (!(response == text6))
				{
					if (response.Contains(value))
					{
						return response;
					}
					return false;
				}
				(string, object) commandResult = GetCommandResult();
				string text7;
				(text7, _) = commandResult;
				_ = commandResult.Item2;
				Ack();
				(string, object) commandResult2 = GetCommandResult();
				(text7, _) = commandResult2;
				_ = commandResult2.Item2;
				if (text7 == text5)
				{
					return false;
				}
				return false;
			}
			if (noAck)
			{
				return true;
			}
			string text8;
			object obj;
			(text8, obj) = GetCommandResult();
			if (!(text8 == text3))
			{
				return obj;
			}
			Ack();
			if ((string)obj == text4)
			{
				return false;
			}
			(text8, obj) = GetCommandResult();
			if (!(text8 == text5))
			{
				return false;
			}
			if (!(response == text2))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public string GetResponse(bool raw = false)
	{
		byte[] array = SerialMTK.Read(12);
		if (array.Length == 12)
		{
			uint[] array2 = array.Unpack32();
			uint num = array2[0];
			uint num2 = array2[1];
			uint num3 = array2[2];
			uint num4 = num2;
			uint num5 = num;
			if (num5 == 4277071599u && num4 == 1)
			{
				byte[] array3 = SerialMTK.Read((int)num3);
				if (array3.Length == num3)
				{
					if (!raw)
					{
						return Encoding.UTF8.GetString(array3).TrimEnd(default(char));
					}
					return Encoding.UTF8.GetString(array3);
				}
			}
			return string.Empty;
		}
		return string.Empty;
	}

	private byte[] GetResponseData()
	{
		byte[] array = SerialMTK.Read(12);
		if (array.Length != 12)
		{
			return Array.Empty<byte>();
		}
		uint[] array2 = array.Unpack32();
		uint num = array2[0];
		uint num2 = array2[1];
		uint num3 = array2[2];
		uint num4 = num2;
		uint num5 = num;
		if (num5 != 4277071599u || num4 != 1)
		{
			return Array.Empty<byte>();
		}
		byte[] array3 = new byte[num3];
		int num6 = 0;
		if (0L >= (long)num3)
		{
			if (0L != (long)num3)
			{
				return Array.Empty<byte>();
			}
			return array3;
		}
		byte[] array4 = SerialMTK.Read((int)num3);
		if (array4.Length == 0)
		{
			if (0L != (long)num3)
			{
				return Array.Empty<byte>();
			}
			return array3;
		}
		array4.CopyTo(array3.AsSpan(0));
		num6 = 0 + array4.Length;
		int num7 = 290781993;
		while (true)
		{
			_ = num7 ^ -760332054;
			if (num6 >= num3)
			{
				if (num6 == num3)
				{
					break;
				}
				return Array.Empty<byte>();
			}
			array4 = SerialMTK.Read((int)num3 - num6);
			if (array4.Length == 0)
			{
				if (num6 != num3)
				{
					return Array.Empty<byte>();
				}
				return array3;
			}
			array4.CopyTo(array3.AsSpan(num6));
			num6 += array4.Length;
			num7 = 290781993;
		}
		return array3;
	}

	private (byte[] Da1, byte[] Da2) PatchDA(byte[] da1, byte[] da2)
	{
		string value = "Error: da2 patched length mismatches. Using unpatched version.";
		uint signLen = Dacfg.Da.Region[1].SignLen;
		uint signLen2 = Dacfg.Da.Region[2].SignLen;
		var (num, hashMode, num2) = Mtk.DaLoader.ComputeHashPos(da1, da2, (int)signLen, (int)signLen2, Dacfg.Da.v6);
		if (num != 0)
		{
			da1 = XmlExt.PatchDA1(da1);
			byte[] array = XmlExt.PatchDA2(da2);
			if (array.Length == da2.Length)
			{
				da2 = array;
				da1 = Mtk.DaLoader.FixHash(da1, da2, num, hashMode, num2);
			}
			else
			{
				Console.WriteLine(value);
				da1 = Mtk.DaLoader.FixHash(da1, da2, num, hashMode, num2);
			}
			Mtk.DaLoader.Patch = true;
			Dacfg.da2 = da2.AsSpan(0, num2).ToArray();
			return (Da1: da1, Da2: da2);
		}
		Mtk.DaLoader.Patch = false;
		int length = da2.Length - (int)signLen2;
		Dacfg.da2 = da2.AsSpan(0, length).ToArray();
		return (Da1: da1, Da2: da2);
	}

	public bool UploadDA1()
	{
		object obj = null;
		string msg = "OK";
		string format = "Jumping to 0x{0:x}... ";
		string msg2 = "Send DA Sync Signal... ";
		string text = "CMD:START";
		string msg3 = "Setup Environment... ";
		string msg4 = "Setup hw init... ";
		string msg5 = "Receive DA Sync Signal... ";
		string hostinfo = "";
		Dacfg.da1 = (byte[])obj;
		Dacfg.da2 = (byte[])obj;
		if (Dacfg.DownloadAgent == obj)
		{
			return false;
		}
		using MemoryStream memoryStream = new MemoryStream(Dacfg.DownloadAgent);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		uint buff = Dacfg.Da.Region[1].Buff;
		uint len = Dacfg.Da.Region[1].Len;
		uint startAddr = Dacfg.Da.Region[1].StartAddr;
		_ = Dacfg.Da.Region[1].StartAddr;
		uint signLen = Dacfg.Da.Region[1].SignLen;
		memoryStream.Seek(buff, SeekOrigin.Begin);
		byte[] array = new byte[len];
		memoryStream.Read(array, 0, (int)len);
		uint buff2 = Dacfg.Da.Region[2].Buff;
		uint signLen2 = Dacfg.Da.Region[2].SignLen;
		memoryStream.Seek(buff2, SeekOrigin.Begin);
		byte[] array2 = new byte[Dacfg.Da.Region[2].Len];
		memoryStream.Read(array2, 0, (int)Dacfg.Da.Region[2].Len);
		if (Mtk.DaLoader.Patch || !Mtk.Config.Targetconfig.Sbc)
		{
			(array, array2) = PatchDA(array, array2);
			Mtk.DaLoader.Patch = true;
			Dacfg.da2 = array2;
		}
		else
		{
			Mtk.DaLoader.Patch = false;
		}
		Dacfg.da2 = array2.AsSpan(0, array2.Length - (int)signLen2).ToArray();
		if (Mtk.Pl.SendDA((int)startAddr, (int)len, (int)signLen, array))
		{
			UI.RichLogs(msg, UI.ok, nextline: true);
			UI.RichLogs(string.Format(format, (int)startAddr), UI.log);
			if (Mtk.Pl.JumpDA((int)startAddr))
			{
				UI.RichLogs(msg, UI.ok, nextline: true);
				UI.RichLogs(msg2, UI.log);
				(string, object) commandResult = GetCommandResult();
				var (text2, _) = commandResult;
				_ = commandResult.Item2;
				if (text2 != text)
				{
					return false;
				}
				UI.RichLogs(msg, UI.ok, nextline: true);
				UI.RichLogs(msg3, UI.log);
				if (!SetupEnvironment())
				{
					return false;
				}
				UI.RichLogs(msg, UI.ok, nextline: true);
				UI.RichLogs(msg4, UI.log);
				if (SetupHwInit())
				{
					UI.RichLogs(msg, UI.ok, nextline: true);
					UI.RichLogs(msg5, UI.log);
					if (!SetupHostInfo(hostinfo))
					{
						return false;
					}
					UI.RichLogs(msg, UI.ok, nextline: true);
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	private bool SetupHwInit()
	{
		string hostCapability = "CMD:DOWNLOAD-FILE^1@CMD:FILE-SYS-OPERATION^1@CMD:PROGRESS-REPORT^1@CMD:UPLOAD-FILE^1@";
		SendCommand(XmlCmd.CmdHostSupportedCommands(hostCapability));
		return (bool)SendCommand(XmlCmd.CmdNotifyInitHw());
	}

	private bool SetupHostInfo(string hostinfo = "")
	{
		return (bool)SendCommand(XmlCmd.CmdSetHostInfo(hostinfo));
	}

	public (string, object) GetCommandResult()
	{
		string fieldName = "command";
		string text = "";
		string value = "OK@";
		string text2 = "OK";
		string text3 = "CMD:PROGRESS-REPORT";
		string text4 = "OK!EOT";
		string text5 = "CMD:START";
		string item = "START";
		string text6 = "CMD:DOWNLOAD-FILE";
		string fieldName2 = "checksum";
		string fieldName3 = "info";
		string fieldName4 = "source_file";
		string fieldName5 = "packet_length";
		string text7 = "CMD:UPLOAD-FILE";
		string fieldName6 = "target_file";
		string text8 = "CMD:FILE-SYS-OPERATION";
		string fieldName7 = "key";
		string fieldName8 = "file_path";
		string text9 = "CMD:END";
		string fieldName9 = "result";
		string text10 = "message";
		string response = GetResponse();
		string field = GetField(response, fieldName);
		string text11 = text;
		if (field == text && response.Contains(value))
		{
			long num = Convert.ToInt64(response.Split('@')[1].Substring(2), 16);
			Ack();
			text11 = GetResponse();
			if (text11 == text2)
			{
				Ack();
				byte[] array = new byte[num];
				byte[] responseData;
				for (int i = 0; i < num; i += responseData.Length)
				{
					responseData = GetResponseData();
					responseData.CopyTo(array.AsSpan(i));
				}
				Ack();
				return (field, Encoding.UTF8.GetString(array));
			}
		}
		if (field == text3)
		{
			Ack();
			string text12 = text;
			while (text12 != text4)
			{
				text12 = GetResponse();
				Ack();
			}
			response = GetResponse();
			field = GetField(response, fieldName);
		}
		if (!(field == text5))
		{
			if (field == text6)
			{
				string field2 = GetField(response, fieldName2);
				string field3 = GetField(response, fieldName3);
				string field4 = GetField(response, fieldName4);
				long packetLength = Convert.ToInt64(GetField(response, fieldName5), 16);
				Ack();
				return (field, new DwnFile(field2, field3, field4, packetLength));
			}
			if (!(field == text7))
			{
				if (!(field == text8))
				{
					if (field == text9)
					{
						text11 = GetField(response, fieldName9);
						if (response.Contains(text10) && text11 != text2)
						{
							string field5 = GetField(response, text10);
							return (field, field5);
						}
					}
					return (field, text11);
				}
				string field6 = GetField(response, fieldName7);
				string field7 = GetField(response, fieldName8);
				Ack();
				return (field, new Class32(field6, field7));
			}
			string field8 = GetField(response, fieldName2);
			string field9 = GetField(response, fieldName3);
			string field10 = GetField(response, fieldName6);
			long c7C1E2A = Convert.ToInt64(GetField(response, fieldName5), 16);
			Ack();
			return (field, new Class33(field8, field9, field10, c7C1E2A));
		}
		Ack();
		return (field, item);
	}

	public bool Upload(DwnFile result, byte[] data, bool raw = false)
	{
		object obj = null;
		string value = "No upload data received. Aborting.";
		string value2 = ":";
		string text = "OK";
		string fieldName = "message";
		string text2 = "Error : ";
		string format = "Error on writing stage2 at pos {0:X}";
		double field_ = Class38.field_543;
		string text3 = "CMD:END";
		string text4 = "CMD:START";
		if (result == obj)
		{
			Console.WriteLine(value);
			return false;
		}
		string sourceFile = result.SourceFile;
		long packetLength = result.PacketLength;
		long num = (sourceFile.Contains(value2) ? Convert.ToInt64(sourceFile.Split(':')[2].Substring(2), 16) : data.Length);
		AckValue(num);
		string response = GetResponse();
		int num2 = 0;
		long num3 = num;
		Stopwatch stopwatch = Stopwatch.StartNew();
		if (!(response == text))
		{
			return false;
		}
		while (true)
		{
			if (num3 <= 0L)
			{
				if (raw)
				{
					Ack();
				}
				string text5;
				object obj2;
				(text5, obj2) = GetCommandResult();
				Ack();
				if (!(text5 == text3) || !((string)obj2 == text))
				{
					GetCommandResult();
					return false;
				}
				(text5, obj2) = GetCommandResult();
				if (text5 == text4)
				{
					return true;
				}
				return false;
			}
			AckValue(0L);
			response = GetResponse();
			if (response.Contains(text))
			{
				int num4 = Math.Min((int)packetLength, data.Length - num2);
				byte[] array = new byte[num4];
				data.AsSpan(num2, num4).CopyTo(array);
				XSend(array);
				response = GetResponse();
				if (!response.Contains(text))
				{
					break;
				}
				num2 += num4;
				num3 -= packetLength;
				UI.Progresbar1(num2, num3);
				double totalSeconds = stopwatch.Elapsed.TotalSeconds;
				if (totalSeconds > field_)
				{
					UI.LabelWrite(utils.FileSize((double)num2 / totalSeconds), utils.FileSize(num2), utils.FileSize(num));
				}
				continue;
			}
			string field = GetField(response, fieldName);
			Console.WriteLine(text2 + field);
			return false;
		}
		Console.WriteLine(string.Format(format, num2));
		return false;
	}

	public bool UploadSparse(DwnFile result, object data = null, long? memLen = null)
	{
		object obj = null;
		int result2 = 0;
		string text = "OK";
		double field_ = Class38.field_543;
		string text2 = "CMD:FILE-SYS-OPERATION";
		string text3 = "CMD:END";
		string format = "{0}";
		string text4 = "CMD:START";
		Stream stream = (Stream)((!(data is string path)) ? ((!(data is byte[] buffer)) ? obj : new MemoryStream(buffer)) : new FileStream(path, FileMode.Open, FileAccess.Read));
		Stream stream2 = stream;
		if (stream2 == obj)
		{
			return false;
		}
		long? num = memLen;
		long num2;
		if (num.HasValue)
		{
			num2 = num.GetValueOrDefault();
		}
		else if (!(data is string fileName))
		{
			byte[] obj2 = data as byte[];
			num2 = ((obj2 != null) ? obj2.Length : 0);
		}
		else
		{
			num2 = new FileInfo(fileName).Length;
		}
		long num3 = num2;
		AckValue(num3);
		string response = GetResponse();
		if (!(response != text))
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			long packetLength = result.PacketLength;
			long num4 = 0L;
			using (stream2)
			{
				while (num4 < num3)
				{
					AckValue(0L);
					response = GetResponse();
					if (response.Contains(text))
					{
						int num5 = (int)Math.Min(packetLength, num3 - num4);
						byte[] array = new byte[num5];
						int num6 = stream2.Read(array, 0, num5);
						if (num6 == num5)
						{
							XSend(array);
							num4 += num5;
							response = GetResponse();
							if (!response.Contains(text))
							{
								return false;
							}
							UI.Progresbar1(num4, num3);
							double totalSeconds = stopwatch.Elapsed.TotalSeconds;
							if (totalSeconds > field_)
							{
								UI.LabelWrite(utils.FileSize((long)((double)num4 / totalSeconds)), utils.FileSize(num4), utils.FileSize(num3));
							}
							continue;
						}
						return false;
					}
					return false;
				}
				stopwatch.Stop();
				string text5;
				object arg;
				(text5, arg) = GetCommandResult();
				if (text5 == text2)
				{
					return true;
				}
				if (text5 == text3 && string.Format(format, arg) == text)
				{
					Ack();
					(text5, arg) = GetCommandResult();
					return text5 == text4;
				}
				Ack();
				GetCommandResult();
			}
			return (byte)result2 != 0;
		}
		return false;
	}

	private byte[] DownloadRaw(Class33 result)
	{
		object obj = null;
		string value = "No download data received. Aborting.";
		string value2 = "OK@";
		string text = "Error on downloading data: ";
		string value3 = "OK";
		double field_ = Class38.field_543;
		if (result != obj)
		{
			string response = GetResponse();
			if (response.Contains(value2))
			{
				long num = Convert.ToInt64(response.Split('@')[1].Substring(2), 16);
				Ack();
				response = GetResponse();
				if (!response.Contains(value3))
				{
					return Array.Empty<byte>();
				}
				Ack();
				byte[] array = new byte[num];
				int num2 = 0;
				long num3 = num;
				Stopwatch stopwatch = Stopwatch.StartNew();
				while (num3 > 0L)
				{
					byte[] responseData = GetResponseData();
					responseData.CopyTo(array.AsSpan(num2));
					num3 -= responseData.Length;
					num2 += responseData.Length;
					UI.Progresbar1(num2, num);
					Ack();
					response = GetResponse();
					if (!response.Contains(value3))
					{
						break;
					}
					Ack();
					double totalSeconds = stopwatch.Elapsed.TotalSeconds;
					if (totalSeconds > field_)
					{
						UI.LabelWrite(utils.FileSize((double)num2 / totalSeconds), utils.FileSize(num2), utils.FileSize(num));
					}
				}
				stopwatch.Stop();
				if (num2 == num)
				{
					return array;
				}
				return Array.Empty<byte>();
			}
			Console.WriteLine(text + response);
			return Array.Empty<byte>();
		}
		Console.WriteLine(value);
		return Array.Empty<byte>();
	}

	private bool DownloadFile(Class33 result, string fileName)
	{
		object obj = null;
		string value = "No download data received. Aborting.";
		string value2 = "OK@";
		string text = "Error on downloading data: ";
		string value3 = "OK";
		double field_ = Class38.field_543;
		if (result != obj)
		{
			string response = GetResponse();
			if (response.Contains(value2))
			{
				long num = Convert.ToInt64(response.Split('@')[1].Substring(2), 16);
				Ack();
				response = GetResponse();
				if (response.Contains(value3))
				{
					Ack();
					long num2 = num;
					using FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
					Stopwatch stopwatch = Stopwatch.StartNew();
					while (true)
					{
						if (num2 > 0L)
						{
							byte[] responseData = GetResponseData();
							num2 -= responseData.Length;
							fileStream.Write(responseData, 0, responseData.Length);
							UI.Progresbar1(fileStream.Length, num);
							Ack();
							response = GetResponse();
							if (!response.Contains(value3))
							{
								break;
							}
							Ack();
							double totalSeconds = stopwatch.Elapsed.TotalSeconds;
							if (totalSeconds > field_)
							{
								UI.LabelWrite(utils.FileSize((double)fileStream.Length / totalSeconds), utils.FileSize(fileStream.Length), utils.FileSize(num));
							}
							continue;
						}
						stopwatch.Stop();
						return fileStream.Length == num;
					}
					return false;
				}
				return false;
			}
			Console.WriteLine(text + response);
			return false;
		}
		Console.WriteLine(value);
		return false;
	}

	private byte[] Download(Class33 result)
	{
		object obj = null;
		string value = "No download data received. Aborting.";
		string value2 = "OK@";
		string text = "Error on downloading data: ";
		string value3 = "OK";
		if (result != obj)
		{
			string response = GetResponse();
			if (response.Contains(value2))
			{
				long num = Convert.ToInt64(response.Split('@')[1].Substring(2), 16);
				Ack();
				response = GetResponse();
				if (!response.Contains(value3))
				{
					return Array.Empty<byte>();
				}
				Ack();
				byte[] array = new byte[num];
				int num2 = 0;
				long num3 = num;
				if (num3 <= 0L)
				{
					Ack();
					if (0L == num)
					{
						return array;
					}
					return Array.Empty<byte>();
				}
				byte[] responseData = GetResponseData();
				int num4 = responseData.Length;
				responseData.CopyTo(array.AsSpan(0));
				num2 = 0 + num4;
				int num5 = 1434368971;
				while (true)
				{
					_ = num5 ^ 0x56B51DB0;
					num3 -= num4;
					if (num3 <= 0L)
					{
						break;
					}
					responseData = GetResponseData();
					num4 = responseData.Length;
					responseData.CopyTo(array.AsSpan(num2));
					num2 += num4;
					num5 = 1434368971;
				}
				Ack();
				if (num2 == num)
				{
					return array;
				}
				return Array.Empty<byte>();
			}
			Console.WriteLine(text + response);
			return Array.Empty<byte>();
		}
		Console.WriteLine(value);
		return Array.Empty<byte>();
	}

	public bool BootTo(long addr, byte[] data)
	{
		long field_ = Class38.field_8459;
		object obj = null;
		string value = "Uploading stage 2... ";
		string value2 = "Successfully uploaded stage 2.";
		string value3 = "Wrong boot_to response :(";
		object obj2 = SendCommand(XmlCmd.CmdBootTo(addr, addr, field_, data.Length));
		DwnFile dwnFile = obj2 as DwnFile;
		if (dwnFile !=null)
		{
			Console.WriteLine(value);
			if (!Upload(dwnFile, data))
			{
				return false;
			}
			Console.WriteLine(value2);
			return true;
		}
		Console.WriteLine(value3);
		return false;
	}

	private bool HandleSLA(byte[] data, int hostOffset = 134217728)
	{
		object obj = null;
		string value = "Running sla auth... ";
		string value2 = "Successfully uploaded sla auth.";
		object obj2 = SendCommand(XmlCmd.CmdSecuritySetFlashPolicy(hostOffset, data.Length));
		DwnFile dwnFile = obj2 as DwnFile;
		if (dwnFile !=null)
		{
			Console.WriteLine(value);
			if (Upload(dwnFile, data))
			{
				Console.WriteLine(value2);
				bool flag = true;
				return true;
			}
			return false;
		}
		return false;
	}

	public bool HandleAllinoneSignature(byte[] data = null, string fileName = null)
	{
		object obj = null;
		string value = "Uploading allinone signature... ";
		string value2 = "Successfully uploaded allinone signature.";
		DwnFile dwnFile;
		if (data == obj)
		{
			data = new byte[256];
			if (fileName != obj)
			{
				object obj2 = SendCommand(XmlCmd.CmdSecuritySetAllinoneSignature(134217728L, 1048576L, fileName));
				dwnFile = obj2 as DwnFile;
			}
			else
			{
				object obj2 = SendCommand(XmlCmd.CmdSecuritySetAllinoneSignature(134217728L, data.Length, fileName));
				dwnFile = obj2 as DwnFile;
			}
			if (dwnFile !=null)
			{
				Console.WriteLine(value);
				if (Upload(dwnFile, data))
				{
					Console.WriteLine(value2);
					bool flag = true;
					return true;
				}
				return false;
			}
			return false;
		}
		if (fileName != obj)
		{
			object obj2 = SendCommand(XmlCmd.CmdSecuritySetAllinoneSignature(134217728L, 1048576L, fileName));
			dwnFile = obj2 as DwnFile;
		}
		else
		{
			object obj2 = SendCommand(XmlCmd.CmdSecuritySetAllinoneSignature(134217728L, data.Length, fileName));
			dwnFile = obj2 as DwnFile;
		}
		if (dwnFile !=null)
		{
			Console.WriteLine(value);
			if (Upload(dwnFile, data))
			{
				Console.WriteLine(value2);
				bool flag = true;
				return true;
			}
			return false;
		}
		return false;
	}

	public bool UploadDA()
	{
		string msg = "Failed!";
		string value = "DA Stage 1 successfully loaded.";
		string msg2 = "Sending Download-Agent [2]... ";
		string msg3 = "OK";
		object sla = null;
		if (UploadDA1())
		{
			Console.WriteLine(value);
			byte[] da = Dacfg.da2;
			uint startAddr = Dacfg.Da.Region[2].StartAddr;
			if (Mtk.DaLoader.Patch || !Config.Targetconfig.Sbc)
			{
				UI.RichLogs(msg2, UI.log);
				if (!BootTo(startAddr, da))
				{
					UI.RichLogs(msg, UI.error, nextline: true);
					return false;
				}
				UI.RichLogs(msg3, UI.ok, nextline: true);
			}
			else if (!RunCarbonara())
			{
				UI.RichLogs(msg2, UI.log);
				if (!BootTo(startAddr, da))
				{
					UI.RichLogs(msg, UI.error, nextline: true);
					return false;
				}
				UI.RichLogs(msg3, UI.ok, nextline: true);
				if (RunHeapBait())
				{
					Mtk.DaLoader.Patch = true;
				}
			}
			else
			{
				Mtk.DaLoader.Patch = true;
			}
			SetupHwInit();
			ChangeUsbSpeed();
			if (!BypassSLA((byte[])sla))
			{
				UI.RichLogs(msg, UI.error, nextline: true);
				return false;
			}
			Reinit(display: true);
			CheckLifecycle();
			if (Mtk.DaLoader.Patch)
			{
				return BootExtDANormal();
			}
			return true;
		}
		UI.RichLogs(msg, UI.error, nextline: true);
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
		UI.RichLogs(msg2, UI.error, nextline: true);
		return false;
	}

	private bool RunHeapBait()
	{
		string msg = "Running HeapBait Exploit... ";
		string msg2 = "Not Support!";
		string msg3 = "Failed!";
		string msg4 = "OK";
		HeapBait heapBait = new HeapBait(Dacfg, this);
		UI.RichLogs(msg, UI.log);
		if (heapBait.SupportChecking())
		{
			bool flag;
			if (flag = heapBait.Run())
			{
				UI.RichLogs(msg4, flag ? UI.ok : UI.error, nextline: true);
				if (!flag)
				{
					return flag;
				}
				Dacfg.da3 = heapBait.PrepareExt();
				return flag;
			}
			UI.RichLogs(msg3, flag ? UI.ok : UI.error, nextline: true);
			if (!flag)
			{
				return flag;
			}
			Dacfg.da3 = heapBait.PrepareExt();
			return flag;
		}
		UI.RichLogs(msg2, UI.error, nextline: true);
		bool flag2 = false;
		return false;
	}

	private bool BootExtDAHeapBait()
	{
		string msg = "Sending DA Extention... ";
		string msg2 = "Failed!";
		string cmd = "EXT-ACK";
		object obj = null;
		string version = "1.0";
		string value = "Wrong EXT-ACK response.";
		string value2 = "Failed to read EXT-ACK data.";
		string fieldName = "status";
		string text = "OK";
		string text2 = "DA extension status error: ";
		string text3 = "CMD:END";
		string text4 = "CMD:START";
		UI.RichLogs(msg, UI.log);
		if (BootTo(1744830464L, Dacfg.da3))
		{
			object obj2 = SendCommand(XmlCmd.CreateCmd(cmd, (Dictionary<string, List<string>>)obj, version));
			Class33 @class = obj2 as Class33;
			if (@class ==null)
			{
				Console.WriteLine(value);
				UI.RichLogs(msg2, UI.error, nextline: true);
				return true;
			}
			byte[] array = Download(@class);
			if (array.Length == 0)
			{
				Console.WriteLine(value2);
				UI.RichLogs(msg2, UI.error, nextline: true);
				return true;
			}
			string field = GetField(Encoding.UTF8.GetString(array), fieldName);
			if (!(field != text))
			{
				(string, object) commandResult = GetCommandResult();
				string text5;
				(text5, _) = commandResult;
				_ = commandResult.Item2;
				if (text5 != text3)
				{
					UI.RichLogs(msg2, UI.error, nextline: true);
					return true;
				}
				Ack();
				(string, object) commandResult2 = GetCommandResult();
				(text5, _) = commandResult2;
				_ = commandResult2.Item2;
				if (!(text5 != text4))
				{
					UI.RichLogs(text, UI.ok, nextline: true);
					return true;
				}
				UI.RichLogs(msg2, UI.error, nextline: true);
				return true;
			}
			Console.WriteLine(text2 + field);
			UI.RichLogs(msg2, UI.error, nextline: true);
			return true;
		}
		UI.RichLogs(msg2, UI.error, nextline: true);
		return true;
	}

	private bool BootExtDANormal()
	{
		return true;
	}

	private bool BypassSLA(byte[] sla = null)
	{
		int num = 0;
		string msg = "DA SLA Status : ";
		string msg2 = "Disable";
		int num2 = 1;
		string msg3 = "Enable";
		string msg4 = "DA SLA Bypassing... ";
		string text = "Infinix-Itel-Tecno";
		string text2 = "Oppo-Realme";
		string hex = "46177D7539E98DF6D6FF46C2E5BB459A059F91E5100EA9AE5908804F009E339372BD94D937C366B0320CD58ED905997D2D529E6285E3358EDE178C33BDD067295F4B18E701C39B96A7B6F4C8B8CE3DAC115BAF30254561CC5A64FB9FDCD45A28F7058EBC0A707EC5146C296FF72E7C33C0CC254A2F2A78F6C7BD9D2692B2145C2334A10BEE6D8B9303D8F368E630B49AA2C783A170A5449F7E9DCFF8C8EA8690F05B6F3ED4D3DED5B1FAB30A82E93FBFC91D3B4F03728597AB27E3A4F06600D76FA0D50DE3D8BAD6E0CF45654DBA23121F601130C2AB61B3F4C867E5BC0FD6B25C3F9FEBCD1B3393EF6406AF8C440CE3454C3689AE88405F8A2B875EC0C32A9B";
		int hostOffset = 134217728;
		string value = "SLA Signature was accepted.";
		string msg5 = "OK";
		object obj = null;
		string key = "rnd";
		string msg6 = "Failed!";
		if (CheckSLA())
		{
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(msg3, UI.error, nextline: true);
			UI.RichLogs(msg4, UI.log);
			if (DeviceState.Brand == text || DeviceState.Brand == text2)
			{
				sla = hex.HexToBytes();
				if (HandleSLA(sla))
				{
					Console.WriteLine(value);
					UI.RichLogs(msg5, UI.ok, nextline: true);
					return true;
				}
			}
			sla = new byte[256];
			if (!HandleSLA(sla))
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
				if (slaKeys !=null)
				{
					Dictionary<string, byte[]> devInfo = GetDevInfo();
					byte[] value2 = default(byte[]);
					if ((devInfo != null) ? ((devInfo.TryGetValue(key, out value2) ? 1 : 0) == num) : ((byte)num2 != 0))
					{
						UI.RichLogs(msg6, UI.error, (byte)num2 != 0, (byte)num != 0);
						return (byte)num != 0;
					}
					byte[] data = Sla.GenerateDaSlaSignature(value2, slaKeys.key);
					if (HandleSLA(data, hostOffset))
					{
						UI.RichLogs(msg5, UI.error, (byte)num2 != 0, (byte)num != 0);
						return (byte)num2 != 0;
					}
				}
				return (byte)num != 0;
			}
			Console.WriteLine(value);
			UI.RichLogs(msg5, UI.ok, nextline: true);
			return true;
		}
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(msg2, UI.error, nextline: true);
		return true;
	}

	public Dictionary<string, byte[]> GetDevInfo()
	{
		object obj = null;
		string text = "OK";
		string text2 = "rnd";
		string text3 = "hrid";
		string text4 = "socid";
		string text5 = "START";
		Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
		if (!(bool)SendCommand(XmlCmd.CmdGetDevInfo(), noAck: true))
		{
			return dictionary;
		}
		(string, object) commandResult = GetCommandResult();
		_ = commandResult.Item1;
		object item = commandResult.Item2;
		Class33 @class = item as Class33;
		if (@class ==null)
		{
			return dictionary;
		}
		byte[] data = Download(@class);
		(string, object) commandResult2 = GetCommandResult();
		_ = commandResult2.Item1;
		item = commandResult2.Item2;
		Ack();
		if ((string)item == text)
		{
			bool flag;
			if (data.IndexOf(Encoding.ASCII.GetBytes(text2)) != -1)
			{
				dictionary[text2] = GetField(data, text2).HexToBytes();
				flag = data.IndexOf(Encoding.ASCII.GetBytes(text3)) != -1;
			}
			else
			{
				flag = data.IndexOf(Encoding.ASCII.GetBytes(text3)) != -1;
			}
			bool flag2;
			if (!flag)
			{
				flag2 = data.IndexOf(Encoding.ASCII.GetBytes(text4)) != -1;
			}
			else
			{
				dictionary[text3] = GetField(data, text3).HexToBytes();
				flag2 = data.IndexOf(Encoding.ASCII.GetBytes(text4)) != -1;
			}
			object item2;
			if (!flag2)
			{
				(string, object) commandResult3 = GetCommandResult();
				_ = commandResult3.Item1;
				item2 = commandResult3.Item2;
				if (!((string)item2 == text5))
				{
					return dictionary;
				}
				return dictionary;
			}
			dictionary[text4] = GetField(data, text4).HexToBytes();
			(string, object) commandResult4 = GetCommandResult();
			_ = commandResult4.Item1;
			item2 = commandResult4.Item2;
			if (!((string)item2 == text5))
			{
				return dictionary;
			}
			return dictionary;
		}
		return dictionary;
	}

	private StorageInfo GetHWInfo(bool display = false)
	{
		long field_ = Class38.field_8488;
		object obj = null;
		string text = "OK";
		string text2 = "START";
		string fieldName = "storage";
		string text3 = "UFS";
		string fieldName2 = "ram_size";
		string fieldName3 = "battery_voltage";
		string fieldName4 = "random_id";
		string fieldName5 = "block_size";
		string fieldName6 = "lua0_size";
		string fieldName7 = "lua1_size";
		string fieldName8 = "lua2_size";
		string fieldName9 = "lua3_size";
		string fieldName10 = "product_id";
		string fieldName11 = "ufs_vendor_id";
		string fieldName12 = "id";
		string fieldName13 = "ufs_cid";
		string fieldName14 = "ufs_fwver";
		string text4 = "";
		string msg = "[UFS Information]";
		string msg2 = "UFS CID           : ";
		string msg3 = "UFS Block Size    : ";
		string format = "{0}";
		string msg4 = "Vendor ID         : ";
		string msg5 = "Vendor            : ";
		string msg6 = "Random ID         : ";
		string msg7 = "UFS FWVer         : ";
		string msg8 = "UFS LU0 Size      : ";
		string msg9 = "UFS LU1 Size      : ";
		string msg10 = "UFS LU2 Size      : ";
		string text5 = "EMMC";
		string fieldName15 = "boot1_size";
		string fieldName16 = "boot2_size";
		string fieldName17 = "rpmb_size";
		string fieldName18 = "user_size";
		string fieldName19 = "gp1_size";
		string fieldName20 = "gp2_size";
		string fieldName21 = "gp3_size";
		string fieldName22 = "gp4_size";
		string fieldName23 = "emmc_cid";
		string text6 = "NAND";
		string fieldName24 = "page_size";
		string fieldName25 = "spare_size";
		string fieldName26 = "total_size";
		string fieldName27 = "page_parity_size";
		string fieldName28 = "sub_type";
		SendCommand(XmlCmd.CmdGetHwInfo(field_), noAck: true);
		(string, object) commandResult = GetCommandResult();
		_ = commandResult.Item1;
		object item = commandResult.Item2;
		Class33 @class = item as Class33;
		if (@class !=null)
		{
			byte[] data = Download(@class);
			(string, object) commandResult2 = GetCommandResult();
			_ = commandResult2.Item1;
			item = commandResult2.Item2;
			Ack();
			if (!((string)item != text))
			{
				(string, object) commandResult3 = GetCommandResult();
				_ = commandResult3.Item1;
				item = commandResult3.Item2;
				if (!((string)item != text2))
				{
					string field = GetField(data, fieldName);
					Storage = new StorageInfo();
					Storage.StorageType = field;
					if (!(field == text3))
					{
						if (!(field == text5))
						{
							if (field == text6)
							{
								Storage.BlockSize = Convert.ToUInt32(GetField(data, fieldName5), 16);
								Storage.PageSize = Convert.ToUInt32(GetField(data, fieldName24), 16);
								Storage.SpareSize = Convert.ToUInt32(GetField(data, fieldName25), 16);
								Storage.TotalSize = Convert.ToUInt32(GetField(data, fieldName26), 16);
								Storage.Cid = GetField(data, fieldName12);
								Storage.PageParitySize = Convert.ToUInt32(GetField(data, fieldName27), 16);
								Storage.SubType = GetField(data, fieldName28);
							}
						}
						else
						{
							Storage.BlockSize = Convert.ToUInt32(GetField(data, fieldName5), 16);
							Storage.Boot1Size = Convert.ToUInt32(GetField(data, fieldName15), 16);
							Storage.Boot2Size = Convert.ToUInt32(GetField(data, fieldName16), 16);
							Storage.RpmbSize = Convert.ToUInt32(GetField(data, fieldName17), 16);
							Storage.UserSize = Convert.ToUInt32(GetField(data, fieldName18), 16);
							Storage.Gp1Size = Convert.ToUInt32(GetField(data, fieldName19), 16);
							Storage.Gp2Size = Convert.ToUInt32(GetField(data, fieldName20), 16);
							Storage.Gp3Size = Convert.ToUInt32(GetField(data, fieldName21), 16);
							Storage.Gp4Size = Convert.ToUInt32(GetField(data, fieldName22), 16);
							Storage.Cid = GetField(data, fieldName12);
							if (string.IsNullOrEmpty(Storage.Cid))
							{
								Storage.Cid = GetField(data, fieldName23);
							}
						}
					}
					else
					{
						Storage.RamSize = GetField(data, fieldName2);
						Storage.BatteryVoltage = GetField(data, fieldName3);
						Storage.RandomId = GetField(data, fieldName4);
						Storage.BlockSize = Convert.ToInt64(GetField(data, fieldName5), 16);
						Storage.Lua0Size = Convert.ToInt64(GetField(data, fieldName6), 16);
						Storage.Lua1Size = Convert.ToInt64(GetField(data, fieldName7), 16);
						Storage.Lua2Size = Convert.ToInt64(GetField(data, fieldName8), 16);
						Storage.Lua3Size = Convert.ToInt64(GetField(data, fieldName9), 16);
						Storage.ProductId = GetField(data, fieldName10);
						Storage.VendorId = GetField(data, fieldName11);
						Storage.Cid = GetField(data, fieldName12);
						if (string.IsNullOrEmpty(Storage.Cid))
						{
							Storage.Cid = GetField(data, fieldName13);
						}
						Storage.FwVer = GetField(data, fieldName14);
						if (display)
						{
							UI.RichLogs(text4, UI.log, nextline: true);
							UI.RichLogs(msg, UI.log, nextline: true, bold: true);
							if (!string.IsNullOrEmpty(Storage.Cid))
							{
								UI.RichLogs(msg2, UI.log);
								UI.RichLogs(Storage.Cid, UI.info, nextline: true);
							}
							UI.RichLogs(msg3, UI.log);
							UI.RichLogs(string.Format(format, Storage.BlockSize), UI.info, nextline: true);
							if (!string.IsNullOrEmpty(Storage.VendorId))
							{
								UI.RichLogs(msg4, UI.log);
								UI.RichLogs(Storage.VendorId, UI.info, nextline: true);
								UI.RichLogs(msg5, UI.log);
								UI.RichLogs(utils.StorageId(Storage.VendorId), UI.info, nextline: true);
							}
							if (!string.IsNullOrEmpty(Storage.RandomId))
							{
								UI.RichLogs(msg6, UI.log);
								UI.RichLogs(Storage.RandomId, UI.info, nextline: true);
							}
							if (!string.IsNullOrEmpty(Storage.FwVer))
							{
								UI.RichLogs(msg7, UI.log);
								UI.RichLogs(Storage.FwVer, UI.info, nextline: true);
							}
							UI.RichLogs(msg8, UI.log);
							UI.RichLogs(utils.FileSize(Storage.Lua0Size) ?? text4, UI.info, nextline: true);
							UI.RichLogs(msg9, UI.log);
							UI.RichLogs(utils.FileSize(Storage.Lua1Size) ?? text4, UI.info, nextline: true);
							UI.RichLogs(msg10, UI.log);
							UI.RichLogs(utils.FileSize(Storage.Lua2Size) ?? text4, UI.info, nextline: true);
						}
					}
					return Storage;
				}
				return (StorageInfo)obj;
			}
			return (StorageInfo)obj;
		}
		return (StorageInfo)obj;
	}

	private bool CheckSLA()
	{
		string key = "DA.SLA";
		string value = "item key=";
		string value2 = "Couldn't find item key";
		string text = "DISABLED";
		string text2 = Encoding.UTF8.GetString(GetSysProperty(key));
		int num = text2.IndexOf(value);
		if (num != -1)
		{
			string text3 = text2.Substring(num + 8);
			int num2 = text3.IndexOf('>');
			if (num2 != -1)
			{
				int num3 = text3.IndexOf('<', num2);
				if (num3 != -1)
				{
					string text4 = text3.Substring(num2 + 1, num3 - num2 - 1);
					return text4 != text;
				}
				return false;
			}
			return false;
		}
		Console.WriteLine(value2);
		return false;
	}

	private byte[] GetSysProperty(string key = "DA.SLA", int length = 2097152)
	{
		long field_ = Class38.field_8488;
		object obj = null;
		string text = "OK";
		string text2 = "START";
		SendCommand(XmlCmd.CmdGetSysProperty(key, field_, length), noAck: true);
		(string, object) commandResult = GetCommandResult();
		_ = commandResult.Item1;
		object item = commandResult.Item2;
		Class33 @class = item as Class33;
		if (@class !=null)
		{
			byte[] result = Download(@class);
			(string, object) commandResult2 = GetCommandResult();
			_ = commandResult2.Item1;
			item = commandResult2.Item2;
			Ack();
			if ((string)item == text)
			{
				(string, object) commandResult3 = GetCommandResult();
				_ = commandResult3.Item1;
				item = commandResult3.Item2;
				if (!((string)item == text2))
				{
					return (byte[])obj;
				}
				return result;
			}
			return (byte[])obj;
		}
		return (byte[])obj;
	}

	private bool ChangeUsbSpeed()
	{
		long field_ = Class38.field_8532;
		return (bool)SendCommand(XmlCmd.CmdCanHigherUsbSpeed(field_));
	}

	private bool CheckLifecycle()
	{
		string function = "LIFE-CYCLE-STATUS";
		long field_ = Class38.field_8534;
		object obj = null;
		string text = "CMD:END";
		string text2 = "OK";
		string text3 = "START";
		string text4 = "OK\0";
		SendCommand(XmlCmd.CmdEmmcControl(function, field_), noAck: true);
		(string, object) commandResult = GetCommandResult();
		string item = commandResult.Item1;
		object item2 = commandResult.Item2;
		Class33 @class = item2 as Class33;
		if (@class !=null)
		{
			byte[] bytes = Download(@class);
			(item, item2) = GetCommandResult();
			Ack();
			if (!((string)item2 == text2))
			{
				return false;
			}
			(item, item2) = GetCommandResult();
			if (!((string)item2 == text3))
			{
				return false;
			}
			return Encoding.UTF8.GetString(bytes) == text4;
		}
		if (item == text)
		{
			Ack();
			GetCommandResult();
			return false;
		}
		return false;
	}

	private void Reinit(bool display = false)
	{
		string text = "emmc";
		string text2 = "nand";
		string text3 = "ufs";
		StorageInfo hWInfo = GetHWInfo(display);
		if (string.Equals(hWInfo.StorageType, text, StringComparison.OrdinalIgnoreCase))
		{
			Emmc = new EmmcInfo
			{
				Gp1Size = hWInfo.Gp1Size,
				Gp2Size = hWInfo.Gp2Size,
				Gp3Size = hWInfo.Gp3Size,
				Gp4Size = hWInfo.Gp4Size,
				RpmbSize = hWInfo.RpmbSize,
				Boot1Size = hWInfo.Boot1Size,
				Boot2Size = hWInfo.Boot2Size
			};
			Dacfg.FlashType = text;
			Dacfg.FlashSize = hWInfo.UserSize;
			Dacfg.rpmbsize = hWInfo.RpmbSize;
			Dacfg.Boot1Size = hWInfo.Boot1Size;
			Dacfg.Boot2Size = hWInfo.Boot2Size;
			Config.PageSize = 512;
			Mtk.DaLoader.Dacfg.PageSize = 512;
			Mtk.DaLoader.Dacfg.FlashSize = hWInfo.UserSize;
		}
		if (string.Equals(hWInfo.StorageType, text2, StringComparison.OrdinalIgnoreCase))
		{
			Dacfg.FlashType = text2;
			Dacfg.FlashSize = hWInfo.TotalSize;
			Dacfg.rpmbsize = 0L;
			Dacfg.Boot1Size = 4194304L;
			Dacfg.Boot2Size = 4194304L;
		}
		if (string.Equals(hWInfo.StorageType, text3, StringComparison.OrdinalIgnoreCase))
		{
			Ufs = new UfsInfo
			{
				Lu1Size = hWInfo.Lua0Size,
				Lu2Size = hWInfo.Lua1Size,
				Lu3Size = hWInfo.Lua2Size,
				Lu4Size = hWInfo.Lua3Size
			};
			Dacfg.FlashType = text3;
			Dacfg.FlashSize = hWInfo.Lua0Size;
			Dacfg.rpmbsize = hWInfo.Lua1Size;
			Dacfg.Boot1Size = hWInfo.Lua1Size;
			Dacfg.Boot2Size = hWInfo.Lua2Size;
			Config.PageSize = 4096;
			Mtk.DaLoader.Dacfg.PageSize = (int)hWInfo.BlockSize;
			Mtk.DaLoader.Dacfg.FlashSize = hWInfo.Lua2Size;
		}
	}

	public bool Shutdown(bool disconnect = false)
	{
		string text = "OK";
		string fieldName = "command";
		string text2 = "CMD:END";
		if (XSend(XmlCmd.CmdReboot(disconnect)))
		{
			string response = GetResponse();
			if (!(response == text))
			{
				return false;
			}
			response = GetResponse();
			if (GetField(response, fieldName) == text2)
			{
				return Ack();
			}
			return false;
		}
		return false;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
