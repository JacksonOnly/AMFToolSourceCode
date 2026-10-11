using System.Collections.Generic;
using System.IO;

namespace AMFTool.Library.Qualcomm;

public static class SaharaDefs
{
	public enum SaharaCommand : uint
	{
		SAHARA_HELLO_REQ = 1u,
		SAHARA_HELLO_RSP,
		SAHARA_READ_DATA,
		SAHARA_END_TRANSFER,
		SAHARA_DONE_REQ,
		SAHARA_DONE_RSP,
		SAHARA_RESET_REQ,
		SAHARA_RESET_RSP,
		SAHARA_MEMORY_DEBUG,
		SAHARA_MEMORY_READ,
		SAHARA_CMD_READY,
		SAHARA_SWITCH_MODE,
		SAHARA_EXECUTE_REQ,
		SAHARA_EXECUTE_RSP,
		SAHARA_EXECUTE_DATA,
		SAHARA_64BIT_MEMORY_DEBUG,
		SAHARA_64BIT_MEMORY_READ,
		SAHARA_64BIT_MEMORY_READ_DATA,
		SAHARA_RESET_STATE_MACHINE_ID
	}

	public enum SaharaExecCommand : uint
	{
		NOP,
		SERIAL_NUM_READ,
		MSM_HW_ID_READ,
		OEM_PK_HASH_READ,
		SWITCH_TO_DMSS_DLOAD,
		SWITCH_TO_STREAM_DLOAD,
		READ_DEBUG_DATA,
		GET_SOFTWARE_VERSION_SBL,
		GET_COMMAND_ID_LIST,
		GET_TRAINING_DATA,
		READ_CHIP_ID_V3
	}

	public enum SaharaMode : uint
	{
		SAHARA_MODE_IMAGE_TX_PENDING,
		SAHARA_MODE_IMAGE_TX_COMPLETE,
		SAHARA_MODE_MEMORY_DEBUG,
		SAHARA_MODE_COMMAND
	}

	public enum SaharaStatus : uint
	{
		SAHARA_STATUS_SUCCESS = 0u,
		SAHARA_NAK_INVALID_CMD = 1u,
		SAHARA_NAK_PROTOCOL_MISMATCH = 2u,
		SAHARA_NAK_INVALID_TARGET_PROTOCOL = 3u,
		SAHARA_NAK_INVALID_HOST_PROTOCOL = 4u,
		SAHARA_NAK_INVALID_PACKET_SIZE = 5u,
		SAHARA_NAK_UNEXPECTED_IMAGE_ID = 6u,
		SAHARA_NAK_INVALID_HEADER_SIZE = 7u,
		SAHARA_NAK_INVALID_DATA_SIZE = 8u,
		SAHARA_NAK_INVALID_IMAGE_TYPE = 9u,
		SAHARA_NAK_INVALID_TX_LENGTH = 10u,
		SAHARA_NAK_INVALID_RX_LENGTH = 11u,
		SAHARA_NAK_GENERAL_TX_RX_ERROR = 12u,
		SAHARA_NAK_READ_DATA_ERROR = 13u,
		SAHARA_NAK_UNSUPPORTED_NUM_PHDRS = 14u,
		SAHARA_NAK_INVALID_PDHR_SIZE = 15u,
		SAHARA_NAK_MULTIPLE_SHARED_SEG = 16u,
		SAHARA_NAK_UNINIT_PHDR_LOC = 17u,
		SAHARA_NAK_INVALID_DEST_ADDR = 18u,
		SAHARA_NAK_INVALID_IMG_HDR_DATA_SIZE = 19u,
		SAHARA_NAK_INVALID_ELF_HDR = 20u,
		SAHARA_NAK_UNKNOWN_HOST_ERROR = 21u,
		SAHARA_NAK_TIMEOUT_RX = 22u,
		SAHARA_NAK_TIMEOUT_TX = 23u,
		SAHARA_NAK_INVALID_HOST_MODE = 24u,
		SAHARA_NAK_INVALID_MEMORY_READ = 25u,
		SAHARA_NAK_INVALID_DATA_SIZE_REQUEST = 26u,
		SAHARA_NAK_MEMORY_DEBUG_NOT_SUPPORTED = 27u,
		SAHARA_NAK_INVALID_MODE_SWITCH = 28u,
		SAHARA_NAK_CMD_EXEC_FAILURE = 29u,
		SAHARA_NAK_EXEC_CMD_INVALID_PARAM = 30u,
		SAHARA_NAK_EXEC_CMD_UNSUPPORTED = 31u,
		SAHARA_NAK_EXEC_DATA_INVALID_CLIENT_CMD = 32u,
		SAHARA_NAK_HASH_TABLE_AUTH_FAILURE = 33u,
		SAHARA_NAK_HASH_VERIFICATION_FAILURE = 34u,
		SAHARA_NAK_HASH_TABLE_NOT_FOUND = 35u,
		SAHARA_NAK_TARGET_INIT_FAILURE = 36u,
		SAHARA_NAK_IMAGE_AUTH_FAILURE = 37u,
		SAHARA_NAK_INVALID_IMG_HASH_TABLE_SIZE = 38u,
		SAHARA_NAK_ENUMERATION_FAILURE = 39u,
		SAHARA_NAK_HW_BULK_TRANSFER_ERROR = 40u,
		SAHARA_NAK_MAX_CODE = 2147483647u
	}

	public class HelloReq
	{
		public uint Cmd;

		public uint Len;

		public uint Version;

		public uint VersionSupported;

		public uint CmdPacketLength;

		public SaharaMode Mode;

		public uint Reserved1;

		public uint Reserved2;

		public uint Reserved3;

		public uint Reserved4;

		public uint Reserved5;

		public uint Reserved6;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class CmdHdr
	{
		public uint Cmd;

		public uint Len;
	}

	public class ReadData
	{
		public uint Cmd;

		public uint Len;

		public ulong ImageId;

		public ulong DataOffset;

		public ulong DataLen;
	}

	public class MemoryDebug
	{
		public uint Cmd;

		public uint Len;

		public uint MemoryTableAddr;

		public uint MemoryTableLength;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class MemoryDebug64
	{
		public uint Cmd;

		public uint Len;

		public ulong MemoryTableAddr;

		public ulong MemoryTableLength;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}

		private object method_02()
		{
			return new int[0];
		}
	}

	public class ExecuteRsp
	{
		public uint Cmd;

		public uint Len;

		public uint ClientCmd;

		public uint DataLen;
	}

	public class ImageEnd
	{
		public uint Cmd;

		public uint Len;

		public uint ImageId;

		public SaharaStatus ImageTxStatus;
	}

	public class Done
	{
		public uint Cmd;

		public uint Len;

		public SaharaStatus ImageTxStatus;
	}

	public class CommandHandler
	{
		public HelloReq PktHelloReq(byte[] data)
		{
			string direction = "little";
			if (data.Length < 48)
			{
				throw new InvalidDataException();
			}
			StructHelperIo structHelperIo = new StructHelperIo(new MemoryStream(data, writable: false), direction);
			return new HelloReq
			{
				Cmd = structHelperIo.Dword(),
				Len = structHelperIo.Dword(),
				Version = structHelperIo.Dword(),
				VersionSupported = structHelperIo.Dword(),
				CmdPacketLength = structHelperIo.Dword(),
				Mode = (SaharaMode)structHelperIo.Dword(),
				Reserved1 = structHelperIo.Dword(),
				Reserved2 = structHelperIo.Dword(),
				Reserved3 = structHelperIo.Dword(),
				Reserved4 = structHelperIo.Dword(),
				Reserved5 = structHelperIo.Dword(),
				Reserved6 = structHelperIo.Dword()
			};
		}

		public CmdHdr PktCmdHdr(byte[] data)
		{
			string direction = "little";
			if (data.Length < 8)
			{
				throw new InvalidDataException();
			}
			StructHelperIo structHelperIo = new StructHelperIo(new MemoryStream(data, writable: false), direction);
			return new CmdHdr
			{
				Cmd = structHelperIo.Dword(),
				Len = structHelperIo.Dword()
			};
		}

		public ReadData PktReadData(byte[] data)
		{
			string direction = "little";
			if (data.Length < 20)
			{
				throw new InvalidDataException();
			}
			StructHelperIo structHelperIo = new StructHelperIo(new MemoryStream(data, writable: false), direction);
			return new ReadData
			{
				Cmd = structHelperIo.Dword(),
				Len = structHelperIo.Dword(),
				ImageId = structHelperIo.Dword(),
				DataOffset = structHelperIo.Dword(),
				DataLen = structHelperIo.Dword()
			};
		}

		public ReadData PktReadData64(byte[] data)
		{
			string direction = "little";
			if (data.Length < 32)
			{
				throw new InvalidDataException();
			}
			StructHelperIo structHelperIo = new StructHelperIo(new MemoryStream(data, writable: false), direction);
			return new ReadData
			{
				Cmd = structHelperIo.Dword(),
				Len = structHelperIo.Dword(),
				ImageId = structHelperIo.Qword(),
				DataOffset = structHelperIo.Qword(),
				DataLen = structHelperIo.Qword()
			};
		}

		public MemoryDebug PktMemoryDebug(byte[] data)
		{
			string direction = "little";
			if (data.Length < 16)
			{
				throw new InvalidDataException();
			}
			StructHelperIo structHelperIo = new StructHelperIo(new MemoryStream(data, writable: false), direction);
			return new MemoryDebug
			{
				Cmd = structHelperIo.Dword(),
				Len = structHelperIo.Dword(),
				MemoryTableAddr = structHelperIo.Dword(),
				MemoryTableLength = structHelperIo.Dword()
			};
		}

		public MemoryDebug64 PktMemoryDebug64(byte[] data)
		{
			string direction = "little";
			if (data.Length < 24)
			{
				throw new InvalidDataException();
			}
			StructHelperIo structHelperIo = new StructHelperIo(new MemoryStream(data, writable: false), direction);
			return new MemoryDebug64
			{
				Cmd = structHelperIo.Dword(),
				Len = structHelperIo.Dword(),
				MemoryTableAddr = structHelperIo.Qword(),
				MemoryTableLength = structHelperIo.Qword()
			};
		}

		public ExecuteRsp PktExecuteRspCmd(byte[] data)
		{
			string direction = "little";
			if (data.Length < 16)
			{
				throw new InvalidDataException();
			}
			StructHelperIo structHelperIo = new StructHelperIo(new MemoryStream(data, writable: false), direction);
			return new ExecuteRsp
			{
				Cmd = structHelperIo.Dword(),
				Len = structHelperIo.Dword(),
				ClientCmd = structHelperIo.Dword(),
				DataLen = structHelperIo.Dword()
			};
		}

		public ImageEnd PktImageEnd(byte[] data)
		{
			string direction = "little";
			if (data.Length < 16)
			{
				throw new InvalidDataException();
			}
			StructHelperIo structHelperIo = new StructHelperIo(new MemoryStream(data, writable: false), direction);
			return new ImageEnd
			{
				Cmd = structHelperIo.Dword(),
				Len = structHelperIo.Dword(),
				ImageId = structHelperIo.Dword(),
				ImageTxStatus = (SaharaStatus)structHelperIo.Dword()
			};
		}

		public Done PktDone(byte[] data)
		{
			string direction = "little";
			if (data.Length < 12)
			{
				throw new InvalidDataException();
			}
			StructHelperIo structHelperIo = new StructHelperIo(new MemoryStream(data, writable: false), direction);
			return new Done
			{
				Cmd = structHelperIo.Dword(),
				Len = structHelperIo.Dword(),
				ImageTxStatus = (SaharaStatus)structHelperIo.Dword()
			};
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public const uint SAHARA_MIN_VERSION = 1u;

	public static Dictionary<uint, string> ErrorDesc;

	static SaharaDefs()
	{
		string value = "Invalid command received in current state";
		string value2 = "Protocol mismatch between host and target";
		string value3 = "Invalid target protocol version";
		string value4 = "Invalid host protocol version";
		string value5 = "Invalid packet size received";
		string value6 = "Unexpected image ID received";
		string value7 = "Invalid image header size received";
		string value8 = "Invalid image data size received";
		string value9 = "Invalid image type received";
		string value10 = "Invalid tranmission length";
		string value11 = "Invalid reception length";
		string value12 = "General transmission or reception error";
		string value13 = "Error while transmitting READ_DATA packet";
		string value14 = "Cannot receive specified number of program headers";
		string value15 = "Invalid data length received for program headers";
		string value16 = "Multiple shared segments found in ELF image";
		string value17 = "Uninitialized program header location";
		string value18 = "Invalid destination address";
		string value19 = "Invalid data size received in image header";
		string value20 = "Invalid ELF header received";
		string value21 = "Unknown host error received in HELLO_RESP";
		string value22 = "Timeout while receiving data";
		string value23 = "Timeout while transmitting data";
		string value24 = "Invalid Mode received from host";
		string value25 = "Invalid memory read access";
		string value26 = "Host cannot handle read data size requested";
		string value27 = "Memory debug not supported";
		string value28 = "Invalid Mode switch";
		string value29 = "Failed to execute command";
		string value30 = "Invalid parameter passed to command execution";
		string value31 = "Unsupported client command received";
		string value32 = "Invalid client command received for data response";
		string value33 = "Failed to authenticate hash table";
		string value34 = "Failed to verify hash for a given segment of ELF image";
		string value35 = "Failed to find hash table in ELF image";
		string value36 = "Target failed to initialize";
		string value37 = "Failed to authenticate generic image";
		string value38 = "Invalid ELF hash table size.  Too bit or small.";
		string value39 = "Invalid IMG Hash Table Size";
		string value40 = "Enumeration failed";
		string value41 = "Hardware Bulk transfer error";
		ErrorDesc = new Dictionary<uint, string>
		{
			{ 0u, value },
			{ 1u, value2 },
			{ 2u, value3 },
			{ 3u, value4 },
			{ 4u, value5 },
			{ 5u, value6 },
			{ 6u, value7 },
			{ 7u, value8 },
			{ 8u, value9 },
			{ 9u, value10 },
			{ 10u, value11 },
			{ 11u, value12 },
			{ 12u, value13 },
			{ 13u, value14 },
			{ 14u, value15 },
			{ 15u, value16 },
			{ 16u, value17 },
			{ 17u, value18 },
			{ 18u, value19 },
			{ 19u, value20 },
			{ 20u, value21 },
			{ 21u, value22 },
			{ 22u, value23 },
			{ 23u, value24 },
			{ 24u, value25 },
			{ 25u, value26 },
			{ 26u, value27 },
			{ 27u, value28 },
			{ 28u, value29 },
			{ 29u, value30 },
			{ 30u, value31 },
			{ 31u, value32 },
			{ 32u, value33 },
			{ 33u, value34 },
			{ 34u, value35 },
			{ 35u, value36 },
			{ 36u, value37 },
			{ 37u, value38 },
			{ 38u, value39 },
			{ 39u, value40 },
			{ 40u, value41 }
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
