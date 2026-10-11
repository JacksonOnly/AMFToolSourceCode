using System;
using System.Collections.Generic;
using System.Text;

namespace AMFTool.Library.Mediatek.Download_Agent.xml;

public class XmlCmd
{
	private MtkClass mtk;

	public XmlCmd(MtkClass mtk)
	{
		this.mtk = mtk;
	}

	public string CreateCmd(string cmd, Dictionary<string, List<string>> content = null, string version = "1.0")
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<da>\n  <version>";
		string text2 = "</version>\n<command>CMD:";
		string text3 = "</command>\n    ";
		object obj = null;
		string text4 = "<";
		string text5 = ">\n";
		string text6 = "</";
		string value = "</da>";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(text + version + text2 + cmd + text3);
		if (content !=null)
		{
			foreach (KeyValuePair<string, List<string>> item in content)
			{
				stringBuilder.Append(text4 + item.Key + text5);
				foreach (string item2 in item.Value)
				{
					stringBuilder.Append(item2);
				}
				stringBuilder.Append(text6 + item.Key + text5);
			}
		}
		stringBuilder.Append(value);
		return stringBuilder.ToString();
	}

	public string CmdNotifyInitHw()
	{
		string cmd = "NOTIFY-INIT-HW";
		object content = null;
		string version = "1.0";
		return CreateCmd(cmd, (Dictionary<string, List<string>>)content, version);
	}

	public string CmdSetHostInfo(string hostInfo = "")
	{
		string text = "";
		string text2 = "yyyyMMddTHHmmss";
		string key = "arg";
		string text3 = "<info>";
		string text4 = "</info>";
		string cmd = "SET-HOST-INFO";
		string version = "1.0";
		Dictionary<string, List<string>> content;
		if (hostInfo == text)
		{
			hostInfo = DateTime.Now.ToString(text2);
			content = new Dictionary<string, List<string>> { 
			{
				key,
				new List<string> { text3 + hostInfo + text4 }
			} };
			return CreateCmd(cmd, content, version);
		}
		content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string> { text3 + hostInfo + text4 }
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdSecuritySetFlashPolicy(int hostOffset = 134217728, int length = 1048576)
	{
		string key = "arg";
		string format = "<source_file>MEM://0x{0:x}:0x{1:x}</source_file>";
		string cmd = "SECURITY-SET-FLASH-POLICY";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string> { string.Format(format, hostOffset, length) }
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdExpCallFunc(uint addr)
	{
		string key = "arg";
		string format = "<address>0x{0:x}</address>";
		string cmd = "EXP-CALL-FUNC";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string> { string.Format(format, addr) }
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdExpPatchMem(uint addr, uint length)
	{
		string key = "arg";
		string format = "<address>0x{0:x}</address>";
		string format2 = "<length>0x{0:x}</length>";
		string cmd = "EXP-PATCH-MEM";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				string.Format(format, addr),
				string.Format(format2, length)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdSecuritySetAllinoneSignature(long hostOffset = 134217728L, long length = 1048576L, string fileName = null)
	{
		object obj = null;
		string format = "MEM://0x{0:X}:{1:X}";
		string key = "arg";
		string text = "<source_file>";
		string text2 = "</source_file>";
		string cmd = "SECURITY-SET-ALLINONE-SIGNATURE";
		string version = "1.0";
		Dictionary<string, List<string>> content;
		if (fileName == obj)
		{
			fileName = string.Format(format, hostOffset, length);
			content = new Dictionary<string, List<string>> { 
			{
				key,
				new List<string> { text + fileName + text2 }
			} };
		}
		else
		{
			content = new Dictionary<string, List<string>> { 
			{
				key,
				new List<string> { text + fileName + text2 }
			} };
		}
		return CreateCmd(cmd, content, version);
	}

	public string CmdBootTo(long atAddr = 1073741824L, long jmpAddr = 1073741824L, long hostOffset = 140635416404044L, int length = 330872)
	{
		string key = "arg";
		string format = "<at_address>0x{0:x}</at_address>";
		string format2 = "<jmp_address>0x{0:x}</jmp_address>";
		string format3 = "<source_file>MEM://0x{0:x}:0x{1:x}</source_file>";
		string cmd = "BOOT-TO";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				string.Format(format, atAddr),
				string.Format(format2, jmpAddr),
				string.Format(format3, hostOffset, length)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdSetRuntimeParameter(string checksumLevel = "NONE", string batteryExist = "AUTO-DETECT", LogLevel daLogLevel = (LogLevel)2, string logChannel = "UART", FtSystemOSE systemOS = FtSystemOSE.OS_WINDOWS, string version = "1.1", bool initializeDram = true)
	{
		string text = "NO";
		string text2 = "YES";
		string key = "arg";
		string text3 = "<checksum_level>";
		string text4 = "</checksum_level>";
		string text5 = "<battery_exist>";
		string text6 = "</battery_exist>";
		string format = "<da_log_level>{0}</da_log_level>";
		string text7 = "<log_channel>";
		string text8 = "</log_channel>";
		string text9 = "<system_os>";
		string oldValue = "OS_";
		string newValue = "";
		string text10 = "</system_os>";
		string key2 = "adv";
		string text11 = "<initialize_dram>";
		string text12 = "</initialize_dram>";
		string cmd = "SET-RUNTIME-PARAMETER";
		string text13;
		Dictionary<string, List<string>> content;
		if (!initializeDram)
		{
			text13 = text;
			content = new Dictionary<string, List<string>>
			{
				{
					key,
					new List<string>
					{
						text3 + checksumLevel + text4,
						text5 + batteryExist + text6,
						string.Format(format, daLogLevel),
						text7 + logChannel + text8,
						text9 + systemOS.ToString().Replace(oldValue, newValue) + text10
					}
				},
				{
					key2,
					new List<string> { text11 + text13 + text12 }
				}
			};
			return CreateCmd(cmd, content, version);
		}
		text13 = text2;
		content = new Dictionary<string, List<string>>
		{
			{
				key,
				new List<string>
				{
					text3 + checksumLevel + text4,
					text5 + batteryExist + text6,
					string.Format(format, daLogLevel),
					text7 + logChannel + text8,
					text9 + systemOS.ToString().Replace(oldValue, newValue) + text10
				}
			},
			{
				key2,
				new List<string> { text11 + text13 + text12 }
			}
		};
		return CreateCmd(cmd, content, version);
	}

	public string CmdHostSupportedCommands(string hostCapability = "CMD:DOWNLOAD-FILE^1@CMD:FILE-SYS-OPERATION^1@CMD:PROGRESS-REPORT^1@CMD:UPLOAD-FILE^1@")
	{
		string key = "arg";
		string text = "<host_capability>";
		string text2 = "</host_capability>";
		string cmd = "HOST-SUPPORTED-COMMANDS";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string> { text + hostCapability + text2 }
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdCanHigherUsbSpeed(long hostMemOffset = 140635587662400L, int length = 64)
	{
		string key = "arg";
		string format = "<target_file>MEM://0x{0:x}:0x{1:x}</target_file>";
		string cmd = "CAN-HIGHER-USB-SPEED";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string> { string.Format(format, hostMemOffset, length) }
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdGetDevInfo(int hostMemOffset = 134217728, int length = 1048576)
	{
		string key = "arg";
		string format = "<target_file>MEM://0x{0:x}:0x{1:x}</target_file>";
		string cmd = "SECURITY-GET-DEV-FW-INFO";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string> { string.Format(format, hostMemOffset, length) }
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdGetHwInfo(long hostMemOffset = 140635417052928L, long length = 2097152L)
	{
		string key = "arg";
		string format = "<target_file>MEM://0x{0:x}:0x{1:x}</target_file>";
		string cmd = "GET-HW-INFO";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string> { string.Format(format, hostMemOffset, length) }
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdEmmcControl(string function = "GET-RPMB-STATUS", long memOffset = 140635419150096L, long memLength = 2097152L)
	{
		string key = "arg";
		string text = "<function>";
		string text2 = "</function>";
		string format = "<target_file>MEM://0x{0:x}:0x{1:x}</target_file>";
		string cmd = "EMMC-CONTROL";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + function + text2,
				string.Format(format, memOffset, memLength)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdReboot(bool disconnect = false)
	{
		string text = "IMMEDIATE";
		string text2 = "DISCONNECT";
		string key = "arg";
		string text3 = "<action>";
		string text4 = "</action>";
		string cmd = "REBOOT";
		string version = "1.0";
		string text5 = ((!disconnect) ? text : text2);
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string> { text3 + text5 + text4 }
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdWriteFlash(string partition = "system", long memOffset = 134217728L, long memLength = 1048576L)
	{
		string key = "arg";
		string text = "<partition>";
		string text2 = "</partition>";
		string format = "<source_file>MEM://0x{0}:0x{1}</source_file>";
		string cmd = "WRITE-PARTITION";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + partition + text2,
				string.Format(format, memOffset, memLength)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdWriteFlashOffset(string partition = "EMMC-USER", long offset = 0L, long memOffset = 134217728L, long memLength = 1048576L)
	{
		string key = "arg";
		string text = "<partition>";
		string text2 = "</partition>";
		string format = "<offset>0x{0:x}</offset>";
		string format2 = "<source_file>MEM://0x{0:x}:0x{1:x}</source_file>";
		string cmd = "WRITE-FLASH";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + partition + text2,
				string.Format(format, offset),
				string.Format(format2, memOffset, memLength)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdWritePartitions(string partition = "system", long memOffset = 134217728L, long memLength = 1048576L)
	{
		string key = "arg";
		string text = "<partition>";
		string text2 = "</partition>";
		string format = "<offset>0x{0}</offset>";
		string format2 = "<source_file>MEM://0x{0:x}:0x{1:x}</source_file>";
		string cmd = "WRITE-PARTITION";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + partition + text2,
				string.Format(format, 0),
				string.Format(format2, memOffset, memLength)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdFlashUpdate(string xml)
	{
		string key = "arg";
		string item = "<path_separator>\\</path_separator>";
		string text = "<source_file>";
		string text2 = "</source_file>";
		string item2 = "<backup_folder>D:/backup</backup_folder>";
		string cmd = "FLASH-UPDATE";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				item,
				text + xml + text2,
				item2
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdFlashUpdate(List<MTKList> partitions, string xml, string backup)
	{
		string text = "<pt name=\"";
		string text2 = "\">";
		string text3 = "</pt>";
		string key = "arg";
		string item = "<path_separator>\\</path_separator>\n";
		string text4 = "<source_file>";
		string text5 = "</source_file>\n";
		string text6 = "<backup_folder>";
		string text7 = "</backup_folder>\n";
		string format = "<flash_list>\n{0}</flash_list>\n";
		string cmd = "FLASH-UPDATE";
		string version = "1.0";
		StringBuilder stringBuilder = new StringBuilder();
		foreach (MTKList partition in partitions)
		{
			stringBuilder.AppendLine(text + partition.PartName + text2 + partition.FilePath + text3);
		}
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				item,
				text4 + xml + text5,
				text6 + backup + text7,
				string.Format(format, stringBuilder)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdFlashAll(List<MTKList> partitions, string xml)
	{
		string text = "<pt name=";
		string text2 = ">";
		string text3 = "</pt>";
		string key = "arg";
		string item = "<path_separator>\\</path_separator>\n";
		string text4 = "<source_file>";
		string text5 = "</source_file>\n";
		string format = "<flash_list>\n{0}</flash_list>\n";
		string cmd = "FLASH-ALL";
		string version = "1.0";
		StringBuilder stringBuilder = new StringBuilder();
		foreach (MTKList partition in partitions)
		{
			stringBuilder.AppendLine(text + partition.PartName + text2 + partition.FilePath + text3);
		}
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				item,
				text4 + xml + text5,
				string.Format(format, stringBuilder)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdWritePartition(string partition = "SOS")
	{
		string key = "arg";
		string text = "<partition>";
		string text2 = "</partition>";
		string item = "<source_file>C:\\test.bin</source_file>";
		string cmd = "WRITE-PARTITION";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + partition + text2,
				item
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string WritePartitions(List<string> partitions, string sourceFile = "")
	{
		string text = "<pt name=";
		string text2 = ">";
		string text3 = ".img</pt>";
		string key = "arg";
		string text4 = "<source_file>";
		string text5 = "</source_file>";
		string format = "<flash_list>{0}</flash_list>";
		string cmd = "WRITE-PARTITIONS";
		string version = "1.0";
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string partition in partitions)
		{
			stringBuilder.AppendLine(text + partition + text2 + partition + text3);
		}
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text4 + sourceFile + text5,
				string.Format(format, stringBuilder)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdReadPartition(string partition = "system", string location = "C:/file.bin")
	{
		string key = "arg";
		string text = "<partition>";
		string text2 = "</partition>";
		string text3 = "<target_file>\"";
		string text4 = "\"</target_file>";
		string cmd = "READ-PARTITION";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + partition + text2,
				text3 + location + text4
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdReadPartitions(string partition = "system", long memOffset = 134217728L, long memLength = 1048576L)
	{
		string key = "arg";
		string text = "<partition>";
		string text2 = "</partition>";
		string format = "<source_file>MEM://0x{0:x}:0x{1:x}</source_file>";
		string cmd = "READ-FLASH";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + partition + text2,
				string.Format(format, memOffset, memLength)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdReadFlash(string partition = "EMMC-USER", long offset = 0L, long length = 1048576L)
	{
		string key = "arg";
		string text = "<partition>";
		string text2 = "</partition>";
		string format = "<offset>0x{0:x}</offset>";
		string format2 = "<length>0x{0:x}</length>";
		string item = "<target_file>ROM_0</target_file>";
		string cmd = "READ-FLASH";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + partition + text2,
				string.Format(format, offset),
				string.Format(format2, length),
				item
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdEraseFlash(string partition = "EMMC-USER", long offset = 0L, long length = 1048576L)
	{
		string key = "arg";
		string text = "<partition>";
		string text2 = "</partition>";
		string format = "<offset>0x{0:x}</offset>";
		string format2 = "<length>0x{0:x}</length>";
		string cmd = "ERASE-FLASH";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + partition + text2,
				string.Format(format, offset),
				string.Format(format2, length)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdEraseAll(string partition = "ALL", long offset = 0L)
	{
		string key = "arg";
		string text = "<partition>";
		string text2 = "</partition>";
		string format = "<offset>0x{0:x}</offset>";
		string item = "<length>MAX</length>";
		string cmd = "ERASE-FLASH";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key,
			new List<string>
			{
				text + partition + text2,
				string.Format(format, offset),
				item
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public string CmdGetSysProperty(string key = "DA.SLA", long hostMemOffset = 140635417052928L, long length = 2097152L)
	{
		string key2 = "arg";
		string text = "<key>";
		string text2 = "</key>";
		string format = "<target_file>MEM://0x{0:x}:0x{1:x}</target_file>";
		string cmd = "GET-SYS-PROPERTY";
		string version = "1.0";
		Dictionary<string, List<string>> content = new Dictionary<string, List<string>> { 
		{
			key2,
			new List<string>
			{
				text + key + text2,
				string.Format(format, hostMemOffset, length)
			}
		} };
		return CreateCmd(cmd, content, version);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
