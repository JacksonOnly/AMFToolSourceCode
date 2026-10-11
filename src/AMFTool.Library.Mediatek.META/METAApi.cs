using System;
using System.Text;

namespace AMFTool.Library.Mediatek.META;

public class METAApi
{
	public enum META_CMD : uint
	{
		META_INIT = 3855604652u,
		META_SUCCESS = 0u,
		AP_HEADER = 85u,
		AP_FT_READ = 96u,
		AP_FT_WRITE = 102u,
		NV_FT_WRITE = 208u,
		NV_FT_READ = 216u,
		FT_IS_ALIVE_REQ = 8u,
		FT_IS_ALIVE_CNF = 9u,
		FT_VER_INFO_REQ = 51u,
		FT_VER_INFO_CNF = 52u,
		FT_VER_INFO_V2_REQ = 133u,
		FT_VER_INFO_V2_CNF = 134u,
		FT_GET_CHIPID_REQ = 123u,
		FT_GET_CHIPID_CNF = 124u,
		FT_BUILD_PROP_REQ = 137u,
		FT_BUILD_PROP_CNF = 138u,
		FT_MISC_REQ = 73u,
		FT_MISC_CNF = 74u,
		FT_MISC_OP_GET_IMEI0 = 0u,
		FT_MISC_OP_GET_IMEI1 = 1u,
		FT_POWER_OFF_REQ = 10u,
		FT_UTILITY_COMMAND_REQ = 14u,
		FT_UTILITY_COMMAND_CNF = 15u,
		FT_CRYPTFS_REQ = 145u,
		FT_CRYPTFS_CNF = 146u,
		FT_MODEM_REQ = 147u,
		FT_MODEM_CNF = 148u,
		FT_FILE_OPERATION_REQ = 163u,
		FT_FILE_OPERATION_CNF = 164u,
		NVRAM_BLUETOOTH_OFFSET = 1u,
		NVRAM_WIFI_OFFSET = 53u,
		NVRAM_ALL_ADDR_OFFSET = 59u,
		FT_RF_TEST_REQ_ID = 0u,
		FT_RF_TEST_CNF_ID = 1u,
		FT_REG_READ_ID = 2u,
		FT_REG_READ_CNF_ID = 3u,
		FT_REG_WRITE_ID = 4u,
		FT_REG_WRITE_CNF_ID = 5u,
		FT_ADC_GETMEADATA_ID = 6u,
		FT_ADC_GETMEADATA_CNF_ID = 7u,
		FT_IS_ALIVE_REQ_ID = 8u,
		FT_IS_ALIVE_CNF_ID = 9u,
		FT_POWER_OFF_REQ_ID = 10u,
		FT_RESERVED04_ID = 11u,
		FT_CHECK_META_VER_REQ_ID = 12u,
		FT_CHECK_META_VER_CNF_ID = 13u,
		FT_UTILITY_COMMAND_REQ_ID = 14u,
		FT_UTILITY_COMMAND_CNF_ID = 15u,
		FT_NVRAM_GET_DISK_INFO_REQ_ID = 16u,
		FT_NVRAM_GET_DISK_INFO_CNF_ID = 17u,
		FT_NVRAM_RESET_REQ_ID = 18u,
		FT_NVRAM_RESET_CNF_ID = 19u,
		FT_NVRAM_LOCK_CNF_ID = 20u,
		FT_NVRAM_LOCK_REQ_ID = 21u,
		FT_NVRAM_READ_REQ_ID = 22u,
		FT_NVRAM_READ_CNF_ID = 23u,
		FT_NVRAM_WRITE_REQ_ID = 24u,
		FT_NVRAM_WRITE_CNF_ID = 25u,
		FT_FAT_OPERATION_ID = 26u,
		FT_L4AUD_REQ_ID = 41u,
		FT_L4AUD_CNF_ID = 42u,
		FT_VER_INFO_REQ_ID = 51u,
		FT_VER_INFO_CNF_ID = 52u,
		FT_CCT_REQ_ID = 53u,
		FT_CCT_CNF_ID = 54u,
		FT_WIFI_WNDRV_SET_REQ_ID = 55u,
		FT_WIFI_WNDRV_SET_CNF_ID = 56u,
		FT_WIFI_WNDRV_QUERY_REQ_ID = 57u,
		FT_WIFI_WNDRV_QUERY_CNF_ID = 58u,
		FT_WIFI_REQ_ID = 59u,
		FT_WIFI_CNF_ID = 60u,
		FT_BT_REQ_ID = 61u,
		FT_BT_CNF_ID = 62u,
		FT_PMIC_REG_READ_ID = 63u,
		FT_PMIC_REG_READ_CNF_ID = 64u,
		FT_PMIC_REG_WRITE_ID = 65u,
		FT_PMIC_REG_WRITE_CNF_ID = 66u,
		FT_URF_TEST_REQ_ID = 67u,
		FT_URF_TEST_CNF_ID = 68u,
		FT_FM_REQ_ID = 69u,
		FT_FM_CNF_ID = 70u,
		FT_TDMB_REQ_ID = 71u,
		FT_TDMB_CNF_ID = 72u,
		FT_DISPATCH_REPORT_ID = 73u,
		FT_WM_METATEST_REQ_ID = 74u,
		FT_WM_METATEST_CNF_ID = 75u,
		FT_WM_BAT_REQ_ID = 76u,
		FT_WM_BAT_CNF_ID = 77u,
		FT_WM_DVB_REQ_ID = 78u,
		FT_WM_DVB_CNF_ID = 79u,
		FT_BATT_READ_INFO_REQ_ID = 80u,
		FT_BATT_READ_INFO_CNF_ID = 81u,
		FT_GPS_REQ_ID = 82u,
		FT_GPS_CNF_ID = 83u,
		FT_BAT_CHIPUPDATE_REQ_ID = 84u,
		FT_BAT_CHIPUPDATE_CNF_ID = 85u,
		FT_SDCARD_REQ_ID = 86u,
		FT_SDCARD_CNF_ID = 87u,
		FT_LOW_POWER_REQ_ID = 88u,
		FT_LOW_POWER_CNF_ID = 89u,
		FT_GPIO_REQ_ID = 90u,
		FT_GPIO_CNF_ID = 91u,
		FT_NVRAM_BACKUP_REQ_ID = 94u,
		FT_NVRAM_BACKUP_CNF_ID = 95u,
		FT_NVRAM_RESTORE_REQ_ID = 96u,
		FT_NVRAM_RESTORE_CNF_ID = 97u,
		FT_GSENSOR_REQ_ID = 114u,
		FT_GSENSOR_CNF_ID = 115u,
		FT_META_MODE_LOCK_REQ_ID = 116u,
		FT_META_MODE_LOCK_CNF_ID = 117u,
		FT_REBOOT_REQ_ID = 118u,
		FT_MATV_CMD_REQ_ID = 119u,
		FT_MATV_CMD_CNF_ID = 120u,
		FT_CUSTOMER_REQ_ID = 121u,
		FT_CUSTOMER_CNF_ID = 122u,
		FT_GET_CHIPID_REQ_ID = 123u,
		FT_GET_CHIPID_CNF_ID = 124u,
		FT_MSENSOR_REQ_ID = 125u,
		FT_MSENSOR_CNF_ID = 126u,
		FT_CTP_REQ_ID = 127u,
		FT_CTP_CNF_ID = 128u,
		FT_ALSPS_REQ_ID = 129u,
		FT_ALSPS_CNF_ID = 130u,
		FT_GYROSCOPE_REQ_ID = 131u,
		FT_GYROSCOPE_CNF_ID = 132u,
		FT_VER_INFO_V2_REQ_ID = 133u,
		FT_VER_INFO_V2_CNF_ID = 134u,
		FT_CMMB_REQ_ID = 135u,
		FT_CMMB_CNF_ID = 136u,
		FT_BUILD_PROP_REQ_ID = 137u,
		FT_BUILD_PROP_CNF_ID = 138u,
		FT_NFC_REQ_ID = 139u,
		FT_NFC_CNF_ID = 140u,
		FT_ADC_REQ_ID = 141u,
		FT_ADC_CNF_ID = 142u,
		FT_EMMC_REQ_ID = 143u,
		FT_EMMC_CNF_ID = 144u,
		FT_CRYPTFS_REQ_ID = 145u,
		FT_CRYPTFS_CNF_ID = 146u,
		FT_MODEM_REQ_ID = 147u,
		FT_MODEM_CNF_ID = 148u,
		FT_SIM_NUM_REQ_ID = 149u,
		FT_SIM_NUM_CNF_ID = 150u,
		FT_DFO_REQ_ID = 151u,
		FT_DFO_CNF_ID = 152u,
		FT_QUERY_APDB_PATH_ID = 173u,
		FT_QUERY_APDB_PATH_CNF_ID = 174u,
		FT_QUERY_MDDB_PATH_ID = 147u,
		FT_QUERY_MDDB_PATH_CNF_ID = 148u,
		FT_MSG_LAST_ID = 149u
	}

	public enum META_MODEM_OPERATION_INFO : uint
	{
		SP_META_MODEM_Query_Info = 0u,
		SP_META_MODEM_Capability = 1u,
		SP_META_MODEM_Get_CurrentModemType = 2u,
		SP_META_MODEM_Query_MDIMGType = 3u,
		SP_META_MODEM_Query_MDDBPath = 9u
	}

	public enum META_FILE_OPERATION_INFO : uint
	{
		FT_FILE_OP_PARSE = 0u,
		FT_FILE_OP_GETFILEINFO = 1u,
		FT_FILE_OP_SENDFILE = 2u,
		FT_FILE_OP_RECEIVEFILE = 3u,
		FT_FILE_OP_END = 268435455u
	}

	public enum FT_FILE_TYPE : uint
	{
		FT_FILE_TYPE_INVALID = 0u,
		FT_FILE_TYPE_FILE = 1u,
		FT_FILE_TYPE_DIR = 2u,
		FT_FILE_TYPE_END = 268435455u
	}

	public enum FT_CUSTOMER_CMD_TYPE
	{
		FT_CUSTOMER_OP_BASIC = 0,
		FT_CUSTOMER_OP_CMD_FORMAT = 1,
		FT_CUSTOMER_OP_BELLI_DEGIL = 2,
		FT_CUSTOMER_OP_CMD_SHELL = 3,
		FT_CUSTOMER_OP_GET_RANDOM = 120,
		FT_CUSTOMER_OP_SET_FACTORY = 121,
		FT_CUSTOMER_OP_GET_BUILD = 122,
		FT_CUSTOMER_OP_SET_FACTORY_CERT = 123,
		FT_CUSTOMER_OP_END = 255
	}

	public class DEVICE_INFO_CNF
	{
		public byte[] BbChip;

		public byte[] EcoVer;

		public byte[] SwTime;

		public byte[] DspFw;

		public byte[] DspPatch;

		public byte[] SwVer;

		public byte[] HwVer;

		public byte[] MelodyVer;

		public byte Status;

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class FILE_OPERATION_PARSE_CNF
	{
		public uint FileCount;
	}

	public class QUERY_APDBPATH_CNF
	{
		public byte[] ApdbPath;

		public string GetPath()
		{
			int num = Array.IndexOf(ApdbPath, (byte)0);
			if (num < 0)
			{
				num = ApdbPath.Length;
				return Encoding.UTF8.GetString(ApdbPath, 0, num);
			}
			return Encoding.UTF8.GetString(ApdbPath, 0, num);
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class FILE_OPERATION_GETFILEINFO_CNF
	{
		public uint FileType;

		public uint FileSize;

		public byte[] FileName;

		public string GetFileName()
		{
			int num = Array.IndexOf(FileName, (byte)0);
			if (num >= 0)
			{
				return Encoding.UTF8.GetString(FileName, 0, num);
			}
			num = FileName.Length;
			return Encoding.UTF8.GetString(FileName, 0, num);
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class MODEM_QUERY_MDDBPATH_CNF
	{
		public byte[] MddbPath;

		public string GetPath()
		{
			int i;
			for (i = 0; i < MddbPath.Length && MddbPath[i] == 0; i++)
			{
			}
			int j;
			for (j = i; j < MddbPath.Length && MddbPath[j] != 0; j++)
			{
			}
			return (i >= j) ? string.Empty : Encoding.UTF8.GetString(MddbPath, i, j - i);
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}
}
