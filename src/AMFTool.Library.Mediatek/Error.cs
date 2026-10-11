using System.Collections.Generic;

namespace AMFTool.Library.Mediatek;

public static class Error
{
	public static Dictionary<uint, string> ErrorCodes;

	public static Dictionary<uint, string> ErrorCodes_XFlash;

	public static Dictionary<uint, string> ErrorCodes_Legacy;

	public static string status(uint status)
	{
		string text = " (0x";
		string text2 = "X";
		string text3 = ")";
		string text4 = "Unknown: 0x";
		if (!ErrorCodes.ContainsKey(status))
		{
			if (ErrorCodes_XFlash.ContainsKey(status))
			{
				return ErrorCodes_XFlash[status] + text + status.ToString(text2) + text3;
			}
			if (ErrorCodes_Legacy.ContainsKey(status))
			{
				return ErrorCodes_Legacy[status] + text + status.ToString(text2) + text3;
			}
			return text4 + status.ToString(text2);
		}
		return ErrorCodes[status] + text + status.ToString(text2) + text3;
	}

	static Error()
	{
		string value = "OK";
		string value2 = "STOP";
		string value3 = "UNDEFINED_ERROR";
		string value4 = "INVALID_ARGUMENTS";
		string value5 = "INVALID_BBCHIP_TYPE";
		string value6 = "INVALID_EXT_CLOCK";
		string value7 = "INVALID_BMTSIZE";
		string value8 = "GET_DLL_VER_FAIL";
		string value9 = "INVALID_BUF";
		string value10 = "BUF_IS_NULL";
		string value11 = "BUF_LEN_IS_ZERO";
		string value12 = "BUF_SIZE_TOO_SMALL";
		string value13 = "NOT_ENOUGH_STORAGE_SPACE";
		string value14 = "NOT_ENOUGH_MEMORY";
		string value15 = "COM_PORT_OPEN_FAIL";
		string value16 = "COM_PORT_SET_TIMEOUT_FAIL";
		string value17 = "COM_PORT_SET_STATE_FAIL";
		string value18 = "COM_PORT_PURGE_FAIL";
		string value19 = "FILEPATH_NOT_SPECIFIED_YET";
		string value20 = "UNKNOWN_TARGET_BBCHIP";
		string value21 = "SKIP_BBCHIP_HW_VER_CHECK";
		string value22 = "UNSUPPORTED_VER_OF_BOOT_ROM";
		string value23 = "UNSUPPORTED_VER_OF_BOOTLOADER";
		string value24 = "UNSUPPORTED_VER_OF_DA";
		string value25 = "UNSUPPORTED_VER_OF_SEC_INFO";
		string value26 = "UNSUPPORTED_VER_OF_ROM_INFO";
		string value27 = "SEC_INFO_NOT_FOUND";
		string value28 = "ROM_INFO_NOT_FOUND";
		string value29 = "CUST_PARA_NOT_SUPPORTED";
		string value30 = "CUST_PARA_WRITE_LEN_INCONSISTENT";
		string value31 = "SEC_RO_NOT_SUPPORTED";
		string value32 = "SEC_RO_WRITE_LEN_INCONSISTENT";
		string value33 = "ADDR_N_LEN_NOT_32BITS_ALIGNMENT";
		string value34 = "UART_CHKSUM_ERROR";
		string value35 = "EMMC_FLASH_BOOT";
		string value36 = "NOR_FLASH_BOOT";
		string value37 = "NAND_FLASH_BOOT";
		string value38 = "UNSUPPORTED_VER_OF_EMI_INFO";
		string value39 = "PART_NO_VALID_TABLE";
		string value40 = "PART_NO_SPACE_FOUND";
		string value41 = "UNSUPPORTED_VER_OF_SEC_CFG";
		string value42 = "UNSUPPORTED_OPERATION";
		string value43 = "CHKSUM_ERROR";
		string value44 = "TIMEOUT";
		string value45 = "SET_META_REG_FAIL";
		string value46 = "SET_FLASHTOOL_REG_FAIL";
		string value47 = "SET_REMAP_REG_FAIL";
		string value48 = "SET_EMI_FAIL";
		string value49 = "DOWNLOAD_DA_FAIL";
		string value50 = "CMD_STARTCMD_FAIL";
		string value51 = "CMD_STARTCMD_TIMEOUT";
		string value52 = "CMD_JUMP_FAIL";
		string value53 = "CMD_WRITE16_MEM_FAIL";
		string value54 = "CMD_READ16_MEM_FAIL";
		string value55 = "CMD_WRITE16_REG_FAIL";
		string value56 = "CMD_READ16_REG_FAIL";
		string value57 = "CMD_CHKSUM16_MEM_FAIL";
		string value58 = "CMD_WRITE32_MEM_FAIL";
		string value59 = "CMD_READ32_MEM_FAIL";
		string value60 = "CMD_WRITE32_REG_FAIL";
		string value61 = "CMD_READ32_REG_FAIL";
		string value62 = "CMD_CHKSUM32_MEM_FAIL";
		string value63 = "JUMP_TO_META_MODE_FAIL";
		string value64 = "WR16_RD16_MEM_RESULT_DIFF";
		string value65 = "CHKSUM16_MEM_RESULT_DIFF";
		string value66 = "BBCHIP_HW_VER_INCORRECT";
		string value67 = "FAIL_TO_GET_BBCHIP_HW_VER";
		string value68 = "AUTOBAUD_FAIL";
		string value69 = "SPEEDUP_BAUDRATE_FAIL";
		string value70 = "LOCK_POWERKEY_FAIL";
		string value71 = "WM_APP_MSG_OUT_OF_RANGE";
		string value72 = "NOT_SUPPORT_MT6205B";
		string value73 = "EXCEED_MAX_DATA_BLOCKS";
		string value74 = "EXTERNAL_SRAM_DETECTION_FAIL";
		string value75 = "EXTERNAL_DRAM_DETECTION_FAIL";
		string value76 = "GET_FW_VER_FAIL";
		string value77 = "CONNECT_TO_BOOTLOADER_FAIL";
		string value78 = "CMD_SEND_DA_FAIL";
		string value79 = "CMD_SEND_DA_CHKSUM_DIFF";
		string value80 = "CMD_JUMP_DA_FAIL";
		string value81 = "CMD_JUMP_BL_FAIL";
		string value82 = "EFUSE_REG_NO_MATCH_WITH_TARGET";
		string value83 = "EFUSE_WRITE_TIMEOUT";
		string value84 = "EFUSE_DATA_PROCESS_ERROR";
		string value85 = "EFUSE_BLOW_ERROR";
		string value86 = "EFUSE_ALREADY_BROKEN";
		string value87 = "EFUSE_BLOW_PARTIAL";
		string value88 = "SEC_VER_FAIL";
		string value89 = "PL_SEC_VER_FAIL";
		string value90 = "SET_WATCHDOG_FAIL";
		string value91 = "EFUSE_VALUE_IS_NOT_ZERO";
		string value92 = "EFUSE_WRITE_TIMEOUT_WITHOUT_EFUSE_VERIFY";
		string value93 = "EFUSE_UNKNOW_EXCEPTION_WITHOUT_EFUSE_VERIFY";
		string value94 = "INT_RAM_ERROR";
		string value95 = "EXT_RAM_ERROR";
		string value96 = "SETUP_DRAM_FAIL";
		string value97 = "SETUP_PLL_ERR";
		string value98 = "SETUP_EMI_PLL_ERR";
		string value99 = "DRAM_ABNORMAL_TYPE_SETTING";
		string value100 = "DRAMC_RANK0_CALIBRATION_FAILED";
		string value101 = "DRAMC_RANK1_CALIBRATION_FAILED";
		string value102 = "DRAM_NOT_SUPPORT";
		string value103 = "RAM_FLOARTING";
		string value104 = "RAM_UNACCESSABLE";
		string value105 = "RAM_ERROR";
		string value106 = "DEVICE_NOT_FOUND";
		string value107 = "NOR_UNSUPPORTED_DEV_ID";
		string value108 = "NAND_UNSUPPORTED_DEV_ID";
		string value109 = "NOR_FLASH_NOT_FOUND";
		string value110 = "NAND_FLASH_NOT_FOUND";
		string value111 = "SOC_CHECK_FAIL";
		string value112 = "NOR_PROGRAM_FAILED";
		string value113 = "NOR_ERASE_FAILED";
		string value114 = "NAND_PAGE_PROGRAM_FAILED";
		string value115 = "NAND_SPARE_PROGRAM_FAILED";
		string value116 = "NAND_HW_COPYBACK_FAILED";
		string value117 = "NAND_ERASE_FAILED";
		string value118 = "IN_PROGRESS";
		string value119 = "SUPERAND_ONLY_SUPPORT_PAGE_READ";
		string value120 = "SUPERAND_PAGE_PRGRAM_NOT_SUPPORT";
		string value121 = "SUPERAND_SPARE_PRGRAM_NOT_SUPPORT";
		string value122 = "SUPERAND_COPYBACK_NOT_SUPPORT";
		string value123 = "NOR_CMD_SEQUENCE_ERR";
		string value124 = "NOR_BLOCK_IS_LOCKED";
		string value125 = "NAND_BLOCK_IS_LOCKED";
		string value126 = "NAND_BLOCK_DATA_UNSTABLE";
		string value127 = "NOR_BLOCK_DATA_UNSTABLE";
		string value128 = "NOR_VPP_RANGE_ERR";
		string value129 = "INVALID_BEGIN_ADDR";
		string value130 = "NOR_INVALID_ERASE_BEGIN_ADDR";
		string value131 = "NOR_INVALID_READ_BEGIN_ADDR";
		string value132 = "NOR_INVALID_PROGRAM_BEGIN_ADDR";
		string value133 = "INVALID_RANGE";
		string value134 = "NOR_PROGRAM_AT_ODD_ADDR";
		string value135 = "NOR_PROGRAM_WITH_ODD_LENGTH";
		string value136 = "NOR_BUFPGM_NO_SUPPORT";
		string value137 = "NAND_UNKNOWN_ERR";
		string value138 = "NAND_BAD_BLOCK";
		string value139 = "NAND_ECC_1BIT_CORRECT";
		string value140 = "NAND_ECC_2BITS_ERR";
		string value141 = "NAND_SPARE_CHKSUM_ERR";
		string value142 = "NAND_HW_COPYBACK_DATA_INCONSISTENT";
		string value143 = "NAND_INVALID_PAGE_INDEX";
		string value144 = "NFI_NOT_SUPPORT";
		string value145 = "NFI_CS1_NOT_SUPPORT";
		string value146 = "NFI_16BITS_IO_NOT_SUPPORT";
		string value147 = "NFB_BOOTLOADER_NOT_EXIST";
		string value148 = "NAND_NO_GOOD_BLOCK";
		string value149 = "NAND_UBIIMG_NOT_SPARSEIMAGE";
		string value150 = "BOOTLOADER_IS_TOO_LARGE";
		string value151 = "SIBLEY_REWRITE_OBJ_MODE_REGION";
		string value152 = "SIBLEY_WRITE_B_HALF_IN_CTRL_MODE_REGION";
		string value153 = "SIBLEY_ILLEGAL_CMD";
		string value154 = "SIBLEY_PROGRAM_AT_THE_SAME_REGIONS";
		string value155 = "UART_GET_DATA_TIMEOUT";
		string value156 = "UART_GET_CHKSUM_LSB_TIMEOUT";
		string value157 = "UART_GET_CHKSUM_MSB_TIMEOUT";
		string value158 = "UART_DATA_CKSUM_ERROR";
		string value159 = "UART_RX_BUF_FULL";
		string value160 = "UART_RX_BUF_NOT_ENOUGH";
		string value161 = "FLASH_RECOVERY_BUF_NOT_ENOUGH";
		string value162 = "HANDSET_SEC_INFO_NOT_FOUND";
		string value163 = "HANDSET_SEC_INFO_MAC_VERIFY_FAIL";
		string value164 = "HANDSET_ROM_INFO_NOT_FOUND";
		string value165 = "HANDSET_FAT_INFO_NOT_FOUND";
		string value166 = "OPERATION_UNSUPPORT_FOR_NFB";
		string value167 = "BYPASS_POST_PROCESS";
		string value168 = "NOR_OTP_NOT_SUPPORT";
		string value169 = "NOR_OTP_EXIST";
		string value170 = "NOR_OTP_LOCKED";
		string value171 = "NOR_OTP_GETSIZE_FAIL";
		string value172 = "NOR_OTP_READ_FAIL";
		string value173 = "NOR_OTP_PROGRAM_FAIL";
		string value174 = "NOR_OTP_LOCK_FAIL";
		string value175 = "NOR_OTP_LOCK_CHECK_STATUS_FAIL";
		string value176 = "BLANK_FLASH";
		string value177 = "CODE_AREA_IS_BLANK";
		string value178 = "SEC_RO_AREA_IS_BLANK";
		string value179 = "NOR_OTP_UNLOCKED";
		string value180 = "UNSUPPORTED_BBCHIP";
		string value181 = "FAT_NOT_EXIST";
		string value182 = "EXT_SRAM_NOT_FOUND";
		string value183 = "EXT_DRAM_NOT_FOUND";
		string value184 = "MT_PIN_LOW";
		string value185 = "MT_PIN_HIGH";
		string value186 = "MT_PIN_SHORT";
		string value187 = "MT_BUS_ERROR";
		string value188 = "MT_ADDR_NOT_2BYTE_ALIGNMENT";
		string value189 = "MT_ADDR_NOT_4BYTE_ALIGNMENT";
		string value190 = "MT_SIZE_NOT_2BYTE_ALIGNMENT";
		string value191 = "MT_SIZE_NOT_4BYTE_ALIGNMENT";
		string value192 = "MT_DEDICATED_PATTERN_ERROR";
		string value193 = "MT_INC_PATTERN_ERROR";
		string value194 = "MT_DEC_PATTERN_ERROR";
		string value195 = "NFB_BLOCK_0_IS_BAD";
		string value196 = "CUST_PARA_AREA_IS_BLANK";
		string value197 = "ENTER_RELAY_MODE_FAIL";
		string value198 = "ENTER_RELAY_MODE_IS_FORBIDDEN_AFTER_META";
		string value199 = "NAND_PAGE_READ_FAILED";
		string value200 = "NAND_IMAGE_BLOCK_NO_EXIST";
		string value201 = "NAND_IMAGE_LIST_NOT_EXIST";
		string value202 = "MBA_RESOURCE_NO_EXIST_IN_TARGET";
		string value203 = "MBA_PROJECT_VERSION_NO_MATCH_WITH_TARGET";
		string value204 = "MBA_UPDATING_RESOURCE_NO_EXIST_IN_TARGET";
		string value205 = "MBA_UPDATING_RESOURCE_SIZE_EXCEED_IN_TARGET";
		string value206 = "NAND_BIN_SIZE_EXCEED_MAX_SIZE";
		string value207 = "NAND_EXCEED_CONTAINER_LIMIT";
		string value208 = "NAND_REACH_END_OF_FLASH";
		string value209 = "NAND_OTP_NOT_SUPPORT";
		string value210 = "NAND_OTP_EXIST";
		string value211 = "NAND_OTP_LOCKED";
		string value212 = "NAND_OTP_LOCK_FAIL";
		string value213 = "NAND_OTP_UNLOCKED";
		string value214 = "OTP_NOT_SUPPORT";
		string value215 = "OTP_EXIST";
		string value216 = "OTP_LOCKED";
		string value217 = "OTP_GETSIZE_FAIL";
		string value218 = "OTP_READ_FAIL";
		string value219 = "OTP_PROGRAM_FAIL";
		string value220 = "OTP_LOCK_FAIL";
		string value221 = "OTP_LOCK_CHECK_STATUS_FAIL";
		string value222 = "OTP_UNLOCKED";
		string value223 = "SEC_RO_ILLEGAL_MAGIC_TAIL";
		string value224 = "HANDSET_FOTA_INFO_NOT_FOUND";
		string value225 = "HANDSET_UA_INFO_NOT_FOUND";
		string value226 = "SB_FSM_INVALID_INFO";
		string value227 = "NFB_TARGET_DUAL_BL_PAIRED_VERSION_NOT_MATCHED_WITH_MAUI";
		string value228 = "NFB_TARGET_DUAL_BL_FEATURE_COMBINATION_NOT_MATCHED_WITH_MAUI";
		string value229 = "NFB_TARGET_IS_SINGLE_BL_BUT_PC_NOT";
		string value230 = "NFB_TARGET_IS_DUAL_BL_BUT_PC_NOT";
		string value231 = "NOR_TARGET_BL_PAIRED_VERSION_NOT_MATCHED_WITH_MAUI";
		string value232 = "NOR_TARGET_BL_FEATURE_COMBINATION_NOT_MATCHED_WITH_MAUI";
		string value233 = "NOR_TARGET_IS_NOT_NEW_BL_BUT_PC_IS";
		string value234 = "NOR_TARGET_IS_NEW_BL_BUT_PC_NOT";
		string value235 = "DOWNLOAD_BOOTLOADER_FLASH_DEV_IS_NONE";
		string value236 = "DOWNLOAD_BOOTLOADER_FLASH_DEV_IS_NOT_SUPPORTED";
		string value237 = "DOWNLOAD_BOOTLOADER_BEGIN_ADDR_OVERLAPS_WITH_PREVIOUS_BOUNDARY";
		string value238 = "UPDATE_BOOTLOADER_EXIST_MAGIC_NOT_MATCHED";
		string value239 = "UPDATE_BOOTLOADER_FILE_TYPE_NOT_MATCHED";
		string value240 = "UPDATE_BOOTLOADER_FILE_SIZE_EXCEEDS_BOUNDARY_ADDR";
		string value241 = "UPDATE_BOOTLOADER_BEGIN_ADDR_NOT_MATCHED";
		string value242 = "EMMC_FLASH_NOT_FOUND";
		string value243 = "EMMC_FW_VER_CHECK_FAIL";
		string value244 = "SDMMC_FLASH_NOT_FOUND";
		string value245 = "SDMMC_CONFIG_FAILED";
		string value246 = "SDMMC_READ_FAILED";
		string value247 = "SDMMC_WRITE_FAILED";
		string value248 = "SDMMC_ERR_CRC";
		string value249 = "SDMMC_ERR_TIMEOUT";
		string value250 = "SDMMC_UNSUPPORTED";
		string value251 = "DSPBL_CHECK_PLATFORM_FAILED";
		string value252 = "UFS_FLASH_NOT_FOUND";
		string value253 = "UFS_CONFIG_FAILED";
		string value254 = "UFS_READ_FAILED";
		string value255 = "UFS_WRITE_FAILED";
		string value256 = "UFS_ERR_TIMEOUT";
		string value257 = "UFS_UNSUPPORTED";
		string value258 = "UFS_OTP_NOT_SUPPORT";
		string value259 = "UFS_OTP_EXIST";
		string value260 = "UFS_OTP_LOCKED";
		string value261 = "UFS_OTP_LOCK_FAIL";
		string value262 = "UFS_OTP_UNLOCKED";
		string value263 = "HANDSET_SEC_CFG_NOT_FOUND";
		string value264 = "EMMC_OTP_NOT_SUPPORT";
		string value265 = "EMMC_OTP_EXIST";
		string value266 = "EMMC_OTP_LOCKED";
		string value267 = "EMMC_OTP_LOCK_FAIL";
		string value268 = "EMMC_OTP_UNLOCKED";
		string value269 = "READ_IMEI_PID_SWV_NOT_SUPPORT";
		string value270 = "NFI_EMPTY_PAGE";
		string value271 = "INVALID_STORAGE_TYPE";
		string value272 = "SEND_CMD_FAIL";
		string value273 = "READ_CMD_ACK_FAIL";
		string value274 = "READ_FLASH_STATUS_INFO_FAIL";
		string value275 = "PL_VALIDATION_FAIL";
		string value276 = "STORAGE_NOT_MATCH";
		string value277 = "CHIP_TYPE_NOT_MATCH";
		string value278 = "EXCEED_MAX_PARTITION_COUNT";
		string value279 = "CALLBACK_SLA_CHALLENGE_FAIL";
		string value280 = "SLA_WRONG_AUTH_FILE";
		string value281 = "SLA_INVALID_AUTH_FILE";
		string value282 = "SLA_CHALLENGE_FAIL";
		string value283 = "SLA_FAIL";
		string value284 = "DAA_FAIL";
		string value285 = "SBC_FAIL";
		string value286 = "SF_SECURE_VER_CHECK_FAIL";
		string value287 = "SF_HANDSET_SECURE_CUSTOM_NAME_NOT_MATCH";
		string value288 = "SF_FTCFG_LOCKDOWN";
		string value289 = "SF_CODE_DOWNLOAD_FORBIDDEN";
		string value290 = "SF_CODE_READBACK_FORBIDDEN";
		string value291 = "SF_CODE_FORMAT_FORBIDDEN";
		string value292 = "SF_SEC_RO_DOWNLOAD_FORBIDDEN";
		string value293 = "SF_SEC_RO_READBACK_FORBIDDEN";
		string value294 = "SF_SEC_RO_FORMAT_FORBIDDEN";
		string value295 = "SF_FAT_DOWNLOAD_FORBIDDEN";
		string value296 = "SF_FAT_READBACK_FORBIDDEN";
		string value297 = "SF_FAT_FORMAT_FORBIDDEN";
		string value298 = "SF_RESTRICTED_AREA_ACCESS_FORBIDDEN";
		string value299 = "SECURE_CUSTOM_NAME_NOT_MATCH_BETWEEN_AUTH_AND_DL_HANDLE";
		string value300 = "DOWNLOAD_FILE_IS_CORRUPTED";
		string value301 = "NOT_SUPPORT";
		string value302 = "BOOTLOADER_IMAGE_SIGNATURE_FAIL";
		string value303 = "BOOTLOADER_ELDER_SW_VERSION_CANNOT_BE_DOWNLOADED";
		string value304 = "BOOTLOADER_IMAGE_NO_SIGNATURE";
		string value305 = "BOOTLOADER_CORRUPTED_SCATTER_FILE";
		string value306 = "SECURE_USB_DL_NO_MAUI_IN_SCATTER_FILE";
		string value307 = "SEND_CERT_FAIL";
		string value308 = "SEND_AUTH_FAIL";
		string value309 = "GET_SEC_CONFIG_FAIL";
		string value310 = "GET_ME_ID_FAIL";
		string value311 = "GET_HW_SW_VER_FAIL";
		string value312 = "GET_HW_CODE_FAIL";
		string value313 = "ROM_INFO_ID_MISMATCH";
		string value314 = "SEC_CTRL_ID_MISMATCH";
		string value315 = "SEC_KEY_ID_MISMATCH";
		string value316 = "SECURE_USB_DL_FAIL";
		string value317 = "SECURE_USB_DL_CHECK_TARGET_STATUS_FAIL";
		string value318 = "SECURE_USB_DL_SEND_CHIP_STATUS_FAIL";
		string value319 = "SECURE_USB_DL_DISABLED";
		string value320 = "SECURE_USB_DL_ENABLED";
		string value321 = "SECURE_USB_DL_IMAGE_PUBLIC_N_KEY_READ_FAIL";
		string value322 = "SECURE_USB_DL_IMAGE_PUBLIC_E_KEY_READ_FAIL";
		string value323 = "SECURE_USB_DL_IMAGE_SIGN_HEADER_NOT_FOUND";
		string value324 = "SECURE_USB_DL_IMGAE_SIGNATURE_VERIFY_FAIL";
		string value325 = "SECURE_USB_DL_IMAGE_HASH_FAIL";
		string value326 = "SECURE_USB_DL_IMAGE_NOT_FOUND";
		string value327 = "SECURE_USB_DL_INVALID_IMAGE_ARGUMENT";
		string value328 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_INIT_FAIL";
		string value329 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_WRITE_IMAGE_NAME_FAIL";
		string value330 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_WRITE_IMAGE_NAME_LEN_FAIL";
		string value331 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_WRITE_TYPE_FAIL";
		string value332 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_WRITE_HEADER_FAIL";
		string value333 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_WRITE_IMAGE_OFFSET_FAIL";
		string value334 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_WRITE_SIGNATURE_HASH_FAIL";
		string value335 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_GET_CHECK_RESULT_FAIL";
		string value336 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_DOWNLOAD_IMAGE_INVALID";
		string value337 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_UNKNOWN_CHECK_RESULT";
		string value338 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_WRONG_OPERATION";
		string value339 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_INVALID_HEADER_LENGTH";
		string value340 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_INVALID_IMAGE_OFFSET";
		string value341 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_INVALID_SIGNATURE_LENGTH";
		string value342 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_SIGNATURE_LENGTH_TOO_LARGE";
		string value343 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_IMAGE_NAME_LENGTH_TOO_LONG";
		string value344 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_EXT_HEADER_TOO_LARGE";
		string value345 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_EXT_HEADER_OFFSET_INVALID";
		string value346 = "SECURE_USB_DL_IMAGE_INFO_CHECK_CMD_EXT_HEADER_SELF_COPY_FAIL";
		string value347 = "SEC_CFG_NOT_EXIST";
		string value348 = "SEC_CFG_WRITE_CMD_INIT_FAIL";
		string value349 = "SEC_CFG_WRONG_MAGIC_NUMBER";
		string value350 = "SEC_CFG_IS_FULL_CANNOT_ADD_NEW_IMAGE";
		string value351 = "SEC_CFG_IMAGE_NOT_FOUND_SO_CANNOT_UPDATE";
		string value352 = "SEC_CFG_IMAGE_CUST_NAME_MISMATCH";
		string value353 = "SEC_CFG_IMAGE_CANNOT_ROLL_BACK_SW_LOAD";
		string value354 = "SEC_CFG_IMAGE_EXIST_CANNOT_CREATE_MEW_FILE";
		string value355 = "SEC_CFG_WRITE_FAIL_CFG_NOT_EXIST";
		string value356 = "SEC_CFG_WRITE_FAIL_MAGIC_INCORRECT";
		string value357 = "SEC_CFG_WRITE_FAIL_CANNOT_WRITE_TO_FIRST_BLOCK";
		string value358 = "SEC_CFG_WRITE_FAIL_YAFFS2_POST_PROCESS_FAIL";
		string value359 = "SEC_CFG_WRITE_FAIL_NAND_DEVICE";
		string value360 = "SEC_CFG_WRITE_FAIL_CANNOT_READ_BACK";
		string value361 = "SEC_CFG_WRITE_FAIL_READ_BACK_MAGIC_INCORRECT";
		string value362 = "SEC_CFG_WRITE_FAIL_READ_BACK_ID_INCORRECT";
		string value363 = "SEC_CFG_WRITE_FAIL_READ_BACK_STATUS_INCORRECT";
		string value364 = "SEC_CFG_WRITE_FAIL_READ_BACK_END_PATTERN_INCORRECT";
		string value365 = "SEC_CFG_WRITE_FAIL_SEC_CFG_CANNOT_OVERWRITE_NEXT_PARTITION";
		string value366 = "SEC_CFG_WRITE_FAIL_NAND_NOT_DETECTED";
		string value367 = "SEC_CFG_WRITE_FAIL_EMMC_NOT_DETECTED";
		string value368 = "SEC_CFG_WRITE_FAIL_EMMC_DEVICE";
		string value369 = "SEC_CFG_WRITE_FAIL_NAND_PAGE_SIZE_NOT_SUPPORT";
		string value370 = "SEC_CFG_WRITE_FAIL_NAND_FIND_GOOD_BLK_FAIL";
		string value371 = "SEC_CFG_WRITE_FAIL_NAND_ERASE_FAIL";
		string value372 = "SEC_CFG_WRITE_FAIL_UNKNOWN_DEVIE_TYPE";
		string value373 = "SEC_CFG_READ_FAIL_NAND_NOT_DETECTED";
		string value374 = "SEC_CFG_READ_FAIL_EMMC_NOT_DETECTED";
		string value375 = "SEC_CFG_READ_FAIL_EMMC_DEVICE";
		string value376 = "SEC_CFG_READ_FAIL_NAND_PAGE_SIZE_NOT_SUPPORT";
		string value377 = "SEC_CFG_READ_FAIL_NAND_FIND_GOOD_BLK_FAIL";
		string value378 = "SEC_CFG_READ_FAIL_UNKNOWN_DEVIE_TYPE";
		string value379 = "SEC_CFG_READ_FAIL_NAND_LOGICAL_READ_FAIL";
		string value380 = "SEC_CFG_EXT_REGION_SPACE_OVERFLOW";
		string value381 = "SECURE_USB_DL_ROM_INFO_UPDATE_REQUEST_FAIL";
		string value382 = "SECURE_USB_DL_DA_RETURN_INVALID_TYPE";
		string value383 = "SECURE_USB_DL_MOVE_IMAGE_HEADER_TO_END_FAIL";
		string value384 = "SECURE_USB_DL_NO_NEED_TO_MOVE_IMAGE_HEADER";
		string value385 = "SECURE_USB_DL_NO_NEED_TO_REMOVE_IMAGE_HEADER_AND_SIG";
		string value386 = "CIPHER_DATA_UNALIGNED";
		string value387 = "CIPHER_MODE_INVALID";
		string value388 = "CIPHER_KEY_INVALID";
		string value389 = "CIPHER_INIT_FAIL";
		string value390 = "CIPHER_ROM_NOT_LOADED";
		string value391 = "CIPHER_DEC_FAIL";
		string value392 = "CIPHER_ENC_TEST_FAIL";
		string value393 = "AES_VER_INVALID";
		string value394 = "INVALID_IMGDEC_CFG";
		string value395 = "IMGDEC_INVALID_FORCE_DEC_PARAM";
		string value396 = "IMGDEC_INVALID_AES_KEY_SIZE";
		string value397 = "IMGDEC_FAIL_IMAGE_NOT_ENCRYPTED";
		string value398 = "IMGDEC_INIT_FAIL_FTH_IS_NULL";
		string value399 = "IMGDEC_INIT_FAIL_DECH_IS_NULL";
		string value400 = "INIDEC_FAIL_INI_NOT_ENCRYPTED";
		string value401 = "INIDEC_INVALID_AES_KEY_SIZE";
		string value402 = "INVALID_PROJECT";
		string value403 = "SECRO_ANTICLONE_LENGTH_INVALID";
		string value404 = "SECRO_HASH_INCORRECT";
		string value405 = "SECRO_ENCRYPT_FAIL";
		string value406 = "AC_REGION_NOT_FOUND_IN_SECROIMG";
		string value407 = "EPP_FAIL";
		string value408 = "EPP_EXT_DRAM_NOT_FOUND";
		string value409 = "EPP_EXT_DRAM_INIT_FAIL";
		string value410 = "EPP_NO_EMI_CONFIG_PARAM_FAIL";
		string value411 = "PL_MODE_UNSUPPORTED";
		string value412 = "PL_MODE_FORBIDDEN";
		string value413 = "PL_MODE_INVALID_ARGUMETS";
		string value414 = "PL_READ_FAIL";
		string value415 = "PL_WRITE_FAIL";
		string value416 = "PL_READ_TIMEOUT";
		string value417 = "PL_WRITE_TIMEOUT";
		string value418 = "PL_DISC_CMD_NEEDED";
		string value419 = "CRYPTO_INIT_FAIL";
		string value420 = "CRYPTO_DEINIT_FAIL";
		string value421 = "CRYPTO_MODE_INVALID";
		string value422 = "CRYPTO_KEY_INVALID";
		string value423 = "CRYPTO_DATA_UNALIGNED";
		string value424 = "CRYPTO_SEEK_LEN_ERROR";
		string value425 = "ADDRESS_TOO_HIGH_80000000";
		string value426 = "ADDRESS_TOO_HIGH_40000000";
		string value427 = "INVALID_ADDRESS";
		string value428 = "NO_AUTH_NEEDED";
		string value429 = "SLA_ERROR";
		string value430 = "DA_OVERLAP";
		string value431 = "DA_INVALID_JUMP_ADDR";
		string value432 = "DA_EXCEED_MAX_DA_BLOCKS";
		string value433 = "DA_ADDR_NOT_16BITS_ALIGNED";
		string value434 = "DA_INVALID_ADDR_AND_LEN";
		string value435 = "DA_LEN_IS_ZERO";
		string value436 = "DA_SIG_LEN_EXCEED_DA_LEN";
		string value437 = "DA_SIG_LEN_IS_ZERO_AND_DAA_ACTIVE";
		string value438 = "DA_NOT_EXIST";
		string value439 = "DA_BLOCKS_INCONSISTENT_WITH_TOOL_AUTH";
		string value440 = "DA_VERSION_NOT_SUPPORTED";
		string value441 = "AUTH_IMAGE_VERIFY_FAIL";
		string value442 = "DA_IMAGE_SIG_VERIFY_FAIL";
		string value443 = "DA_IMAGE_NO_MEM_FAIL";
		string value444 = "DA_INIT_KEY_FAIL";
		string value445 = "IMG_INIT_KEY_FAIL";
		string value446 = "HASH_IMAGE_FAIL";
		string value447 = "DA_RELOCATE_SIZE_NOT_ENOUGH";
		string value448 = "LIB_SEC_CFG_NOT_EXIST";
		string value449 = "LIB_VER_INVALID";
		string value450 = "LIB_SEC_CFG_ERASE_FAIL";
		string value451 = "LIB_SEC_CFG_CANNOT_WRITE";
		string value452 = "LIB_SEC_CFG_END_PATTERN_NOT_EXIST";
		string value453 = "LIB_SEC_CFG_STATUS_INVALID";
		string value454 = "LIB_SEC_CFG_READ_SIZE_NOT_ENOUGH";
		string value455 = "LIB_SEC_CFG_RSA_KEY_INIT_FAIL";
		string value456 = "IMG_VERIFY_THIS_IMG_INFO_NOT_EXIST";
		string value457 = "IMG_VERIFY_HASH_COMPARE_FAIL";
		string value458 = "IMG_VERIFY_NO_SPACE_ADD_IMG_INFO";
		string value459 = "SEC_DL_TOKEN_NOT_FOUND_IN_IMG";
		string value460 = "SEC_DL_FLOW_ERROR";
		string value461 = "IMG_VERIFY_INVALID_IMG_INFO_ATTR";
		string value462 = "IMG_SECROIMG_NOT_FOUND";
		string value463 = "IMG_READ_FAIL";
		string value464 = "IMG_VERIFY_SIGNATURE_FAIL";
		string value465 = "IMG_SIGN_FORMAT_NOT_MATCH";
		string value466 = "IMG_EXTENSION_HDR_NOT_FOUND";
		string value467 = "IMG_EXTENSION_MAGIC_WRONG";
		string value468 = "IMG_EXTENSION_TYPE_NOT_SUPPORT";
		string value469 = "IMG_EXTENSION_HASH_CAL_FAIL";
		string value470 = "IMG_LOCK_TABLE_NOT_EXIST";
		string value471 = "IMG_LOCK_ALL_LOCK";
		string value472 = "IMG_LOCK_NO_SPACE_ADD_LOCK_INFO";
		string value473 = "IMG_LOCK_THIS_IMG_INFO_NOT_EXIST";
		string value474 = "IMG_LOCK_MAGIC_ERROR";
		string value475 = "SBC_KEY_NOT_FOUND";
		string value476 = "BR_SEC_CFG_NOT_FOUND";
		string value477 = "ERR_PUBK_NOT_INITIALIZED";
		string value478 = "SHA256_OP_FAIL";
		string value479 = "ERR_PSS_CHK_FAIL";
		string value480 = "ERR_SIG_CHK_FAIL";
		string value481 = "REGION_INVALID_INCLUDE";
		string value482 = "REGION_INVALID_OVERLAP";
		string value483 = "REGION_INVALID_OVERFLOW";
		string value484 = "DA_INVALID_LOCATION";
		string value485 = "DA_INVALID_LENGTH";
		string value486 = "DAA_Security_Error_Signature";
		string value487 = "DAA_Security_Error";
		string value488 = "DAA_SIG_VERIFY_FAILED";
		string value489 = "INVALID_SIG_SZ";
		string value490 = "INVALID_PADDING_SZ";
		string value491 = "SW_ID_MISMATCH";
		string value492 = "INVALID_IMG_TYPE";
		string value493 = "BUF_NOT_ENOUGH_FOR_CERT";
		string value494 = "IMG_NOT_FOUND";
		string value495 = "IMG_HDR_HASH_VFY_FAIL";
		string value496 = "IMG_HASH_VFY_FAIL";
		string value497 = "INVALID_OID_SZ";
		string value498 = "INVALID_OID_BUF_SZ";
		string value499 = "END_OF_CERT";
		string value500 = "CERT_OBJ_ID_NOT_MATCH";
		string value501 = "CERT_OBJ_SZ_NOT_MATCH";
		string value502 = "CERT_TRAVERSE_UNKNOWN_MODE";
		string value503 = "INVALID_OID_IDX";
		string value504 = "OID_NOT_FOUND";
		string value505 = "OBJ_NOT_FOUND";
		string value506 = "E_KEY_LEN_NOT_MATCH";
		string value507 = "E_KEY_NOT_MATCH";
		string value508 = "N_KEY_LEN_NOT_MATCH";
		string value509 = "N_KEY_NOT_MATCH";
		string value510 = "PUBK_AUTH_FAIL";
		string value511 = "INVALID_IMG_HDR";
		string value512 = "NOT_EXPECTED_IMG";
		string value513 = "CERT_IMG_VER_NOT_SYNC";
		string value514 = "IMAGE_IMG_VER_NOT_SYNC";
		string value515 = "IMG_VER_ROLLBACK";
		string value516 = "OTP_FIELD_NOT_ENOUGH";
		string value517 = "OTP_REG_BIT_CONFIG_WRONG";
		string value518 = "OTP_WRITE_FAIL";
		string value519 = "Error";
		string value520 = "Abort";
		string value521 = "Unsupported command";
		string value522 = "Unsupported ctrl code";
		string value523 = "Protocol error";
		string value524 = "Protocol buffer overflow";
		string value525 = "Insufficient buffer";
		string value526 = "USB SCAN error";
		string value527 = "Invalid hsession";
		string value528 = "Invalid session";
		string value529 = "Invalid stage";
		string value530 = "Not implemented";
		string value531 = "File not found";
		string value532 = "Open file error";
		string value533 = "Write file error";
		string value534 = "Read file error";
		string value535 = "Create File error / Unsupported Version";
		string value536 = "Rom info not found";
		string value537 = "Cust name not found";
		string value538 = "Device not supported";
		string value539 = "DL forbidden";
		string value540 = "Img too large";
		string value541 = "PL verify fail";
		string value542 = "Image verify fail";
		string value543 = "Hash operation fail";
		string value544 = "Hash binding check fail";
		string value545 = "Invalid buf";
		string value546 = "Binding hash not available";
		string value547 = "Write data not allowed";
		string value548 = "Format not allowed";
		string value549 = "SV5 public key auth failed";
		string value550 = "SV5 hash verify failed";
		string value551 = "SV5 RSA OP failed";
		string value552 = "SV5 RSA verify failed";
		string value553 = "SV5 GFH not found";
		string value554 = "Cert1 invalid";
		string value555 = "Cert2 invalid";
		string value556 = "Imghdr invalid";
		string value557 = "Sig size invalid";
		string value558 = "RSA pss op fail";
		string value559 = "Cert auth failed";
		string value560 = "Public key auth mismatch n size";
		string value561 = "Public key auth mismatch e size";
		string value562 = "Public key auth mismatch n";
		string value563 = "Public key auth mismatch e";
		string value564 = "Public key auth mismatch hash";
		string value565 = "Cert obj not found";
		string value566 = "Cert oid not found";
		string value567 = "Cert out of range";
		string value568 = "Oid doesn't match";
		string value569 = "Length doesn't match";
		string value570 = "ASN1 unknown op";
		string value571 = "OID index out of range";
		string value572 = "OID too large";
		string value573 = "Public key size mismatch";
		string value574 = "SWID mismatch";
		string value575 = "Hash size mismatch";
		string value576 = "IMGHDR type mismatch";
		string value577 = "IMG type mismatch";
		string value578 = "IMGHDR hash verify failed";
		string value579 = "IMG has verify failed";
		string value580 = "Anti rollback violation";
		string value581 = "SECCFG not found";
		string value582 = "SECCFG magic incorrect";
		string value583 = "SECCFG invalid";
		string value584 = "Cipher mode invalid";
		string value585 = "Cipher key invalid";
		string value586 = "Cipher data unaligned";
		string value587 = "GFH file info not found";
		string value588 = "GFH anti clone not found";
		string value589 = "GFH sec cfg not found";
		string value590 = "Unsupported source type";
		string value591 = "Cust name mismatch";
		string value592 = "Invalid address";
		string value593 = "Certificate version not synced";
		string value594 = "Signature not synced";
		string value595 = "Ext AllInOne Signature rejected";
		string value596 = "Ext AllInOne Signature missing";
		string value597 = "Comm Key is not set";
		string value598 = "DevInfo Check failed";
		string value599 = "Bootimg count overflow";
		string value600 = "Signature not found";
		string value601 = "Bootimg special handle";
		string value602 = "Remote Seucrity policy disabled";
		string value603 = "RSA OAEP fail";
		string value604 = "DA Anti-Rollback error";
		string value605 = "Get OTP value fail";
		string value606 = "Invalid unit size";
		string value607 = "Invalid group idx";
		string value608 = "Img version overflow";
		string value609 = "Otp table not initialized";
		string value610 = "Invalid Partition name";
		string value611 = "DA version Anti-Rollback error";
		string value612 = "Invalid msg size";
		string value613 = "Security level unsupported";
		string value614 = "Security level mismatch";
		string value615 = "Fault injection error";
		string value616 = "Public Key hash group invalid - too many root keys";
		string value617 = "Security level too large";
		string value618 = "Security config is formatted";
		string value619 = "Security config unknown error";
		string value620 = "Lockstate seccfg fail";
		string value621 = "Lockstate custom fail";
		string value622 = "Lockstate inconsistent";
		string value623 = "Scatter file invalid";
		string value624 = "DA file invalid";
		string value625 = "DA selection error";
		string value626 = "Preloader invalid";
		string value627 = "EMI hdr invalid";
		string value628 = "Storage mismatch";
		string value629 = "Invalid parameters";
		string value630 = "Invalid GPT";
		string value631 = "Invalid PMT";
		string value632 = "Layout changed";
		string value633 = "Invalid format param";
		string value634 = "Unknown storage section type";
		string value635 = "Unknown scatter field";
		string value636 = "Partition tbl doesn't exist";
		string value637 = "Scatter hw chip id mismatch";
		string value638 = "SEC cert file not found";
		string value639 = "SEC auth file not found";
		string value640 = "SEC auth file needed";
		string value641 = "EMI containter file not found";
		string value642 = "Scatter file not found";
		string value643 = "Xml file op error";
		string value644 = "Unsupported page size";
		string value645 = "EMI info length offset invalid";
		string value646 = "EMI info length invalid";
		string value647 = "Unsupported operation";
		string value648 = "Thread error";
		string value649 = "Checksum error";
		string value650 = "Unknown sparse";
		string value651 = "Unknown sparse chunk type";
		string value652 = "Partition not found";
		string value653 = "Read parttbl failed";
		string value654 = "Exceeded max partition number";
		string value655 = "Unknown storage type";
		string value656 = "Dram Test failed";
		string value657 = "Exceed available range";
		string value658 = "Write sparse image failed";
		string value659 = "MMC error";
		string value660 = "Nand error";
		string value661 = "Nand in progress";
		string value662 = "Nand timeout";
		string value663 = "Nand bad block";
		string value664 = "Nand erase failed";
		string value665 = "Nand page program failed";
		string value666 = "EMI setting version error";
		string value667 = "UFS error";
		string value668 = "DA OTP not supported";
		string value669 = "DA OTP lock failed";
		string value670 = "EFUSE unknown error";
		string value671 = "EFUSE write timeout without verify";
		string value672 = "EFUSE blown";
		string value673 = "EFUSE revert bit";
		string value674 = "EFUSE blown partly";
		string value675 = "EFUSE invalid argument";
		string value676 = "EFUSE value is not zero";
		string value677 = "EFUSE blown incorrect data";
		string value678 = "EFUSE broken";
		string value679 = "EFUSE blow error";
		string value680 = "EFUSE data process error";
		string value681 = "EFUSE unlock bpkey error";
		string value682 = "EFUSE create list error";
		string value683 = "EFUSE write register error";
		string value684 = "EFUSE padding type mismatch";
		string value685 = "Device ctrl exception";
		string value686 = "Shutdown Cmd exception";
		string value687 = "Download exception";
		string value688 = "Upload exception";
		string value689 = "Ext Ram exception";
		string value690 = "Notify Switch Usb Speed exception";
		string value691 = "Read data exception";
		string value692 = "Write data exception";
		string value693 = "Format exception";
		string value694 = "OTP operation exception";
		string value695 = "Switch usb exception";
		string value696 = "Write efuse exception";
		string value697 = "Read efuse exception";
		string value698 = "Brom start cmd/connect not preloader failed";
		string value699 = "Brom get bbchip hw ver failed";
		string value700 = "Brom cmd send da failed";
		string value701 = "Brom cmd jump da failed";
		string value702 = "Brom cmd failed";
		string value703 = "Brom stage callback failed";
		string value704 = "DA Version mismatch";
		string value705 = "DA not found";
		string value706 = "DA section not found";
		string value707 = "DA hash mismatch";
		string value708 = "DA exceed max num";
		string value709 = "S_DA_INT_RAM_ERROR";
		string value710 = "S_DA_EXT_RAM_ERROR";
		string value711 = "S_DA_SETUP_DRAM_FAIL";
		string value712 = "S_DA_SETUP_PLL_ERR";
		string value713 = "S_DA_DRAM_NOT_SUPPORT";
		string value714 = "S_DA_RAM_FLOARTING";
		string value715 = "S_DA_RAM_UNACCESSABLE";
		string value716 = "S_DA_RAM_ERROR";
		string value717 = "S_DA_DEVICE_NOT_FOUND";
		string value718 = "S_DA_NOR_UNSUPPORTED_DEV_ID";
		string value719 = "S_DA_NAND_UNSUPPORTED_DEV_ID";
		string value720 = "S_DA_NOR_FLASH_NOT_FOUND";
		string value721 = "S_DA_NAND_FLASH_NOT_FOUND";
		string value722 = "S_DA_SOC_CHECK_FAIL";
		string value723 = "S_DA_NOR_PROGRAM_FAILED";
		string value724 = "S_DA_NOR_ERASE_FAILED";
		string value725 = "S_DA_NAND_PAGE_PROGRAM_FAILED";
		string value726 = "S_DA_NAND_SPARE_PROGRAM_FAILED";
		string value727 = "S_DA_NAND_HW_COPYBACK_FAILED";
		string value728 = "S_DA_NAND_ERASE_FAILED";
		string value729 = "S_DA_TIMEOUT";
		string value730 = "S_DA_IN_PROGRESS";
		string value731 = "S_DA_SUPERAND_ONLY_SUPPORT_PAGE_READ";
		string value732 = "S_DA_SUPERAND_PAGE_READ_NOT_SUPPORT";
		string value733 = "S_DA_SUPERAND_PAGE_PRGRAM_NOT_SUPPORT";
		string value734 = "S_DA_SUPERAND_SPARE_PRGRAM_NOT_SUPPORT";
		string value735 = "S_DA_SUPERAND_SPARE_READ_NOT_SUPPORT";
		string value736 = "S_DA_SUPERAND_PAGE_SPARE_PRGRAM_NOT_SUPPORT";
		string value737 = "S_DA_SUPERAND_COPYBACK_NOT_SUPPORT";
		string value738 = "S_DA_NOR_CMD_SEQUENCE_ERR";
		string value739 = "S_DA_NOR_BLOCK_IS_LOCKED";
		string value740 = "S_DA_NAND_BLOCK_IS_LOCKED";
		string value741 = "S_DA_NAND_BLOCK_DATA_UNSTABLE";
		string value742 = "S_DA_NOR_BLOCK_DATA_UNSTABLE";
		string value743 = "S_DA_NOR_VPP_RANGE_ERR";
		string value744 = "S_DA_INVALID_BEGIN_ADDR";
		string value745 = "S_DA_NOR_INVALID_ERASE_BEGIN_ADDR";
		string value746 = "S_DA_NOR_INVALID_READ_BEGIN_ADDR";
		string value747 = "S_DA_NOR_INVALID_PROGRAM_BEGIN_ADDR";
		string value748 = "S_DA_INVALID_RANGE";
		string value749 = "S_DA_NOR_PROGRAM_AT_ODD_ADDR";
		string value750 = "S_DA_NOR_PROGRAM_WITH_ODD_LENGTH";
		string value751 = "S_DA_NOR_BUFPGM_NO_SUPPORT";
		string value752 = "S_DA_NAND_UNKNOWN_ERR";
		string value753 = "S_DA_NAND_BAD_BLOCK";
		string value754 = "S_DA_NAND_ECC_1BIT_CORRECT";
		string value755 = "S_DA_NAND_ECC_2BITS_ERR";
		string value756 = "S_DA_NAND_ECC_UNCORRECTABLE_ERROR";
		string value757 = "S_DA_NAND_SPARE_CHKSUM_ERR";
		string value758 = "S_DA_NAND_HW_COPYBACK_DATA_INCONSISTENT";
		string value759 = "S_DA_NAND_INVALID_PAGE_INDEX";
		string value760 = "S_DA_NFI_NOT_SUPPORT";
		string value761 = "S_DA_NFI_CS1_NOT_SUPPORT";
		string value762 = "S_DA_NFI_16BITS_IO_NOT_SUPPORT";
		string value763 = "S_DA_NFB_BOOTLOADER_NOT_EXIST";
		string value764 = "S_DA_NAND_NO_GOOD_BLOCK";
		string value765 = "S_DA_BOOTLOADER_IS_TOO_LARGE";
		string value766 = "S_DA_SIBLEY_REWRITE_OBJ_MODE_REGION";
		string value767 = "S_DA_SIBLEY_WRITE_B_HALF_IN_CTRL_MODE_REGION";
		string value768 = "S_DA_SIBLEY_ILLEGAL_CMD";
		string value769 = "S_DA_SIBLEY_PROGRAM_AT_THE_SAME_REGIONS";
		string value770 = "S_DA_UART_GET_DATA_TIMEOUT";
		string value771 = "S_DA_UART_GET_CHKSUM_LSB_TIMEOUT";
		string value772 = "S_DA_UART_GET_CHKSUM_MSB_TIMEOUT";
		string value773 = "S_DA_UART_DATA_CKSUM_ERROR";
		string value774 = "S_DA_UART_RX_BUF_FULL";
		string value775 = "S_DA_FLASH_RECOVERY_BUF_NOT_ENOUGH";
		string value776 = "S_DA_HANDSET_SEC_INFO_NOT_FOUND";
		string value777 = "S_DA_HANDSET_SEC_INFO_MAC_VERIFY_FAIL";
		string value778 = "S_DA_HANDSET_ROM_INFO_NOT_FOUND";
		string value779 = "S_DA_HANDSET_FAT_INFO_NOT_FOUND";
		string value780 = "S_DA_OPERATION_UNSUPPORT_FOR_NFB";
		string value781 = "S_DA_BYPASS_POST_PROCESS";
		string value782 = "S_DA_NOR_OTP_NOT_SUPPORT";
		string value783 = "S_DA_NOR_OTP_EXIST";
		string value784 = "S_DA_NOR_OTP_LOCKED";
		string value785 = "S_DA_NOR_OTP_GETSIZE_FAIL";
		string value786 = "S_DA_NOR_OTP_READ_FAIL";
		string value787 = "S_DA_NOR_OTP_PROGRAM_FAIL";
		string value788 = "S_DA_NOR_OTP_LOCK_FAIL";
		string value789 = "S_DA_NOR_OTP_LOCK_CHECK_STATUS_FAIL";
		string value790 = "S_DA_BLANK_FLASH";
		string value791 = "S_DA_CODE_AREA_IS_BLANK";
		string value792 = "S_DA_SEC_RO_AREA_IS_BLANK";
		string value793 = "S_DA_NOR_OTP_UNLOCKED";
		string value794 = "S_DA_UNSUPPORTED_BBCHIP";
		string value795 = "S_DA_FAT_NOT_EXIST";
		string value796 = "S_DA_EXT_SRAM_NOT_FOUND";
		string value797 = "S_DA_EXT_DRAM_NOT_FOUND";
		string value798 = "S_DA_MT_PIN_LOW";
		string value799 = "S_DA_MT_PIN_HIGH";
		string value800 = "S_DA_MT_PIN_SHORT";
		string value801 = "S_DA_MT_BUS_ERROR";
		string value802 = "S_DA_MT_ADDR_NOT_2BYTE_ALIGNMENT";
		string value803 = "S_DA_MT_ADDR_NOT_4BYTE_ALIGNMENT";
		string value804 = "S_DA_MT_SIZE_NOT_2BYTE_ALIGNMENT";
		string value805 = "S_DA_MT_SIZE_NOT_4BYTE_ALIGNMENT";
		string value806 = "S_DA_MT_DEDICATED_PATTERN_ERROR";
		string value807 = "S_DA_MT_INC_PATTERN_ERROR";
		string value808 = "S_DA_MT_DEC_PATTERN_ERROR";
		string value809 = "S_DA_NFB_BLOCK_0_IS_BAD";
		string value810 = "S_DA_CUST_PARA_AREA_IS_BLANK";
		string value811 = "S_DA_ENTER_RELAY_MODE_FAIL";
		string value812 = "S_DA_ENTER_RELAY_MODE_IS_FORBIDDEN_AFTER_META";
		string value813 = "S_DA_NAND_PAGE_READ_FAILED";
		string value814 = "S_DA_NAND_IMAGE_BLOCK_NO_EXIST";
		string value815 = "S_DA_NAND_IMAGE_LIST_NOT_EXIST";
		string value816 = "S_DA_MBA_RESOURCE_NO_EXIST_IN_TARGET";
		string value817 = "S_DA_MBA_PROJECT_VERSION_NO_MATCH_WITH_TARGET";
		string value818 = "S_DA_MBA_UPDATING_RESOURCE_NO_EXIST_IN_TARGET";
		string value819 = "S_DA_MBA_UPDATING_RESOURCE_SIZE_EXCEED_IN_TARGET";
		string value820 = "S_DA_NAND_BIN_SIZE_EXCEED_MAX_SIZE";
		string value821 = "S_DA_NAND_EXCEED_CONTAINER_LIMIT";
		string value822 = "S_DA_NAND_REACH_END_OF_FLASH";
		string value823 = "S_DA_NAND_OTP_NOT_SUPPORT";
		string value824 = "S_DA_NAND_OTP_EXIST";
		string value825 = "S_DA_NAND_OTP_LOCKED";
		string value826 = "S_DA_NAND_OTP_LOCK_FAIL";
		string value827 = "S_DA_NAND_OTP_UNLOCKED";
		string value828 = "S_DA_OTP_NOT_SUPPORT";
		string value829 = "S_DA_OTP_EXIST";
		string value830 = "S_DA_OTP_LOCKED";
		string value831 = "S_DA_OTP_GETSIZE_FAIL";
		string value832 = "S_DA_OTP_READ_FAIL";
		string value833 = "S_DA_OTP_PROGRAM_FAIL";
		string value834 = "S_DA_OTP_LOCK_FAIL";
		string value835 = "S_DA_OTP_LOCK_CHECK_STATUS_FAIL";
		string value836 = "S_DA_OTP_UNLOCKED";
		string value837 = "S_DA_SEC_RO_ILLEGAL_MAGIC_TAIL";
		string value838 = "S_DA_HANDSET_FOTA_INFO_NOT_FOUND";
		string value839 = "S_DA_HANDSET_UA_INFO_NOT_FOUND";
		string value840 = "S_DA_SB_FSM_INVALID_INFO";
		string value841 = "S_DA_NFB_TARGET_DUAL_BL_PAIRED_VERSION_NOT_MATCHED_WITH_MAUI";
		string value842 = "S_DA_NFB_TARGET_DUAL_BL_FEATURE_COMBINATION_NOT_MATCHED_WITH_MAUI";
		string value843 = "S_DA_NFB_TARGET_IS_SINGLE_BL_BUT_PC_NOT";
		string value844 = "S_DA_NFB_TARGET_IS_DUAL_BL_BUT_PC_NOT";
		string value845 = "S_DA_NOR_TARGET_BL_PAIRED_VERSION_NOT_MATCHED_WITH_MAUI";
		string value846 = "S_DA_NOR_TARGET_BL_FEATURE_COMBINATION_NOT_MATCHED_WITH_MAUI";
		string value847 = "S_DA_NOR_TARGET_IS_NOT_NEW_BL_BUT_PC_IS";
		string value848 = "S_DA_NOR_TARGET_IS_NEW_BL_BUT_PC_NOT";
		string value849 = "S_DA_GEN_DA_VERSION_INFO_TEMP_ILB_FAIL";
		string value850 = "S_DA_FLASH_NOT_FOUND";
		string value851 = "S_DA_BOOT_CERT_NOT_EXIST";
		string value852 = "S_DA_NAND_CODE_IMAGE_OVERLAP_FAT_REGION";
		string value853 = "S_DA_DOWNLOAD_BOOTLOADER_FLASH_DEV_IS_NONE";
		string value854 = "S_DA_DOWNLOAD_BOOTLOADER_FLASH_DEV_IS_NOT_SUPPORTED";
		string value855 = "S_DA_DOWNLOAD_BOOTLOADER_BEGIN_ADDR_OVERLAPS_WITH_PREVIOUS_BOUNDARY";
		string value856 = "S_DA_UPDATE_BOOTLOADER_EXIST_MAGIC_NOT_MATCHED";
		string value857 = "S_DA_UPDATE_BOOTLOADER_FILE_TYPE_NOT_MATCHED";
		string value858 = "S_DA_UPDATE_BOOTLOADER_FILE_SIZE_EXCEEDS_BOUNDARY_ADDR";
		string value859 = "S_DA_UPDATE_BOOTLOADER_BEGIN_ADDR_NOT_MATCHED";
		string value860 = "S_DA_CBR_SET_BUF_AND_API_FAIL";
		string value861 = "S_DA_CBR_NOT_FOUND";
		string value862 = "S_DA_CBR_FLASH_LAYOUT_NOT_FOUND";
		string value863 = "S_DA_CBR_FLASH_SPACE_INFO_NOT_FOUND";
		string value864 = "S_DA_CBR_FLASH_CONFIG_NOT_FOUND";
		string value865 = "S_DA_CBR_SET_ENVRIONMENT_FAILED";
		string value866 = "S_DA_CBR_CREAT_FAILED";
		string value867 = "S_DA_CBR_COMPARE_FAILED";
		string value868 = "S_DA_CBR_WRONG_VERSION";
		string value869 = "S_DA_CBR_ALREADY_EXIST";
		string value870 = "S_DA_CBR_RECORD_BUF_TOO_SMALL";
		string value871 = "S_DA_CBR_RECORD_NOT_EXIST";
		string value872 = "S_DA_CBR_RECORD_ALREADY_EXIST";
		string value873 = "S_DA_CBR_FULL";
		string value874 = "S_DA_CBR_RECORD_WRITE_LEN_INCONSISTENT";
		string value875 = "S_DA_CBR_VERSION_NOT_MATCHED";
		string value876 = "S_DA_CBR_NOT_SUPPORT_PCT_FLASH";
		string value877 = "S_DA_CBR_UNKNOWN_ERROR";
		string value878 = "S_DA_SEC_RO_ACC_PARSE_ERROR";
		string value879 = "S_DA_HEADER_BLOCK_NOT_EXIST";
		string value880 = "S_DA_S_PRE_PARSE_CUSTOMER_NAME_FAIL";
		string value881 = "S_DA_S_RETRIEVE_SEC_RO_FAIL_IN_SECURE_INIT";
		string value882 = "S_DA_S_FLASH_INFO_NOT_EXIST";
		string value883 = "S_DA_S_MAUI_INFO_NOT_EXIST";
		string value884 = "S_DA_S_BOOTLOADER_SHARE_DATA_NOT_EXIST";
		string value885 = "S_DA_GFH_FILE_INFO_RETREIVAL_FAIL";
		string value886 = "S_DA_NAND_REMARK_FAIL";
		string value887 = "S_DA_TARGET_IS_NOT_NEW_BL_BUT_PC_IS";
		string value888 = "S_DA_EMMC_FLASH_NOT_FOUND";
		string value889 = "S_DA_EMMC_ENABLE_BOOT_FAILED";
		string value890 = "S_DA_HB_FOUND_IN_OTHER_FLASH_DEV";
		string value891 = "S_DA_USB_2_0_NOT_SUPPORT";
		string value892 = "S_DA_CBR_INIT_FAILED";
		string value893 = "S_DA_CBR_MAUI_INFO_SIZE_TOO_BIG";
		string value894 = "S_DA_CBR_WRITE_MAUI_INFO_FAILED";
		string value895 = "S_DA_CBR_READ_MAUI_INFO_FAILED";
		string value896 = "S_DA_UNSUPPORTED_OPERATION";
		string value897 = "S_DA_MBA_RESOURCE_BIN_NUMBER_NOT_MATCH_WITH_TARGET";
		string value898 = "S_DA_MBA_HEADER_NOT_EXIST";
		string value899 = "S_DA_MBA_RESOURCE_VERSION_NO_MATCH_WITH_TARGET";
		string value900 = "S_DA_BOOTLOADER_SELF_UPDATE_FAIL";
		string value901 = "S_DA_SEARCH_BL_SELF_UPDATE_INFO_FAIL";
		string value902 = "S_DA_SPACE_NOT_ENOUGH_FOR_EXT_BL_MARKER";
		string value903 = "S_DA_FIND_EXT_BL_MARKER_FAIL";
		string value904 = "S_DA_TOO_MANY_BAD_BLOCKS_FOR_EXT_BL_MARKER";
		string value905 = "S_DA_TOO_MANY_BAD_BLOCKS_FOR_EXT_BL_BACKUP";
		string value906 = "S_DA_EXT_BL_VER_MISMATCHED";
		string value907 = "S_DA_EXT_BL_VER_NOT_FOUND";
		string value908 = "S_DA_BL_SELF_UPDATE_FEATURE_CHECK_FAILED";
		string value909 = "S_DA_BL_ROM_INFO_NOT_FOUND";
		string value910 = "S_DA_EXT_BL_MAX_SIZE_MISMATCHED";
		string value911 = "S_DA_INVALID_PARAMETER_FROM_PC";
		string value912 = "S_DA_BL_SELF_UPDATE_NOT_SUPPORTED";
		string value913 = "S_DA_EXT_BL_HDR_NOT_FOUND";
		string value914 = "S_DA_S_FLASH_LAYOUT_NOT_EXIST";
		string value915 = "S_DA_S_FLASH_ID_NOT_EXIST";
		string value916 = "S_DA_MAUI_GFH_FLASH_ID_NOT_MATCH_WITH_TARGET";
		string value917 = "S_DA_FLASH_ERASE_SIZE_NOT_SUPPORT";
		string value918 = "S_DA_SRD_NOT_FOUND";
		string value919 = "S_DA_SRD_UPDATE_FAILED";
		string value920 = "S_DA_NAND_DATA_ADDR_NOT_PAGE_ALIGNMENT";
		string value921 = "S_DA_BL_GFH_BROM_SEC_CFG_NOT_FOUND";
		string value922 = "S_DA_BL_CUSTOMER_NAME_BUFFER_INSUFFICIENT";
		string value923 = "S_DA_COM_BUSY";
		string value924 = "S_DA_INITIAL_BMT_FAILED_CAUSED_FROM_POOL_SIZE_ERROR";
		string value925 = "S_DA_LOAD_ORIGINAL_BMT_FAILED";
		string value926 = "S_DA_INVALID_NAND_PAGE_BUFFER";
		string value927 = "S_DA_DL_BOOT_REGION_IS_OVERLAP_CONTROL_BLOCK_REGION";
		string value928 = "S_DA_PRE_DL_HB_INIT_FAIL";
		string value929 = "S_DA_POST_DL_HB_WRITE_FAIL";
		string value930 = "S_DA_LOAD_IMG_PARA_FAIL";
		string value931 = "S_DA_WRITE_IMG_PARA_FAIL";
		string value932 = "S_DA_UPDATE_HB_FAIL";
		string value933 = "S_DA_BIN_SIZE_EXCEED_MAX_ERR";
		string value934 = "S_DA_PARTIAL_BIN_TYPE_ERR";
		string value935 = "S_DA_IMAGE_PARA_QUERY_ERR";
		string value936 = "S_DA_IMAGE_PARA_UPDATE_ERR";
		string value937 = "S_DA_FLASH_LAYOUT_BIN_NOT_FOUND";
		string value938 = "S_DA_FLASH_LAYOUT_GET_ELEMENT_FAIL";
		string value939 = "S_DA_FLASH_LAYOUT_ADD_ELEMENT_FAIL";
		string value940 = "S_DA_CBR_FOUND_BUT_MAUI_NOT_EXIST";
		string value941 = "S_DA_UPDATE_BOOTLOADER_NOT_CONTAIN_CRITICAL_DATA";
		string value942 = "S_DA_DUMP_FLASH_LAYOUT_FAIL";
		string value943 = "S_DA_BMT_NO_INIT";
		string value944 = "S_DA_NOR_PROGRAM_REGION_IS_OVERLAP";
		ErrorCodes = new Dictionary<uint, string>
		{
			{ 0u, value },
			{ 1000u, value2 },
			{ 1001u, value3 },
			{ 1002u, value4 },
			{ 1003u, value5 },
			{ 1004u, value6 },
			{ 1005u, value7 },
			{ 1006u, value8 },
			{ 1007u, value9 },
			{ 1008u, value10 },
			{ 1009u, value11 },
			{ 1010u, value12 },
			{ 1011u, value13 },
			{ 1012u, value14 },
			{ 1013u, value15 },
			{ 1014u, value16 },
			{ 1015u, value17 },
			{ 1016u, value18 },
			{ 1017u, value19 },
			{ 1018u, value20 },
			{ 1019u, value21 },
			{ 1020u, value22 },
			{ 1021u, value23 },
			{ 1022u, value24 },
			{ 1023u, value25 },
			{ 1024u, value26 },
			{ 1025u, value27 },
			{ 1026u, value28 },
			{ 1027u, value29 },
			{ 1028u, value30 },
			{ 1029u, value31 },
			{ 1030u, value32 },
			{ 1031u, value33 },
			{ 1032u, value34 },
			{ 1033u, value35 },
			{ 1034u, value36 },
			{ 1035u, value37 },
			{ 1036u, value38 },
			{ 1037u, value39 },
			{ 1038u, value40 },
			{ 1039u, value41 },
			{ 1040u, value42 },
			{ 1041u, value43 },
			{ 1042u, value44 },
			{ 2000u, value45 },
			{ 2001u, value46 },
			{ 2002u, value47 },
			{ 2003u, value48 },
			{ 2004u, value49 },
			{ 2005u, value50 },
			{ 2006u, value51 },
			{ 2007u, value52 },
			{ 2008u, value53 },
			{ 2009u, value54 },
			{ 2010u, value55 },
			{ 2011u, value56 },
			{ 2012u, value57 },
			{ 2013u, value58 },
			{ 2014u, value59 },
			{ 2015u, value60 },
			{ 2016u, value61 },
			{ 2017u, value62 },
			{ 2018u, value63 },
			{ 2019u, value64 },
			{ 2020u, value65 },
			{ 2021u, value66 },
			{ 2022u, value67 },
			{ 2023u, value68 },
			{ 2024u, value69 },
			{ 2025u, value70 },
			{ 2026u, value71 },
			{ 2027u, value72 },
			{ 2028u, value73 },
			{ 2029u, value74 },
			{ 2030u, value75 },
			{ 2031u, value76 },
			{ 2032u, value77 },
			{ 2033u, value78 },
			{ 2034u, value79 },
			{ 2035u, value80 },
			{ 2036u, value81 },
			{ 2037u, value82 },
			{ 2038u, value83 },
			{ 2039u, value84 },
			{ 2040u, value85 },
			{ 2041u, value86 },
			{ 2042u, value87 },
			{ 2043u, value88 },
			{ 2044u, value89 },
			{ 2045u, value90 },
			{ 2046u, value91 },
			{ 2047u, value92 },
			{ 2048u, value93 },
			{ 3000u, value94 },
			{ 3001u, value95 },
			{ 3002u, value96 },
			{ 3003u, value97 },
			{ 3004u, value98 },
			{ 3005u, value99 },
			{ 3006u, value100 },
			{ 3007u, value101 },
			{ 3008u, value102 },
			{ 3009u, value103 },
			{ 3010u, value104 },
			{ 3011u, value105 },
			{ 3012u, value106 },
			{ 3013u, value107 },
			{ 3014u, value108 },
			{ 3015u, value109 },
			{ 3016u, value110 },
			{ 3017u, value111 },
			{ 3018u, value112 },
			{ 3019u, value113 },
			{ 3020u, value114 },
			{ 3021u, value115 },
			{ 3022u, value116 },
			{ 3023u, value117 },
			{ 3024u, value44 },
			{ 3025u, value118 },
			{ 3026u, value119 },
			{ 3027u, value120 },
			{ 3028u, value121 },
			{ 3029u, value122 },
			{ 3030u, value123 },
			{ 3031u, value124 },
			{ 3032u, value125 },
			{ 3033u, value126 },
			{ 3034u, value127 },
			{ 3035u, value128 },
			{ 3036u, value129 },
			{ 3037u, value130 },
			{ 3038u, value131 },
			{ 3039u, value132 },
			{ 3040u, value133 },
			{ 3041u, value134 },
			{ 3042u, value135 },
			{ 3043u, value136 },
			{ 3044u, value137 },
			{ 3045u, value138 },
			{ 3046u, value139 },
			{ 3047u, value140 },
			{ 3048u, value141 },
			{ 3049u, value142 },
			{ 3050u, value143 },
			{ 3051u, value144 },
			{ 3052u, value145 },
			{ 3053u, value146 },
			{ 3054u, value147 },
			{ 3055u, value148 },
			{ 3056u, value149 },
			{ 3057u, value150 },
			{ 3058u, value151 },
			{ 3059u, value152 },
			{ 3060u, value153 },
			{ 3061u, value154 },
			{ 3062u, value155 },
			{ 3063u, value156 },
			{ 3064u, value157 },
			{ 3065u, value158 },
			{ 3066u, value159 },
			{ 3067u, value160 },
			{ 3068u, value161 },
			{ 3069u, value162 },
			{ 3070u, value163 },
			{ 3071u, value164 },
			{ 3072u, value165 },
			{ 3073u, value166 },
			{ 3074u, value167 },
			{ 3075u, value168 },
			{ 3076u, value169 },
			{ 3077u, value170 },
			{ 3078u, value171 },
			{ 3079u, value172 },
			{ 3080u, value173 },
			{ 3081u, value174 },
			{ 3082u, value175 },
			{ 3083u, value176 },
			{ 3084u, value177 },
			{ 3085u, value178 },
			{ 3086u, value179 },
			{ 3087u, value180 },
			{ 3088u, value181 },
			{ 3089u, value182 },
			{ 3090u, value183 },
			{ 3091u, value184 },
			{ 3092u, value185 },
			{ 3093u, value186 },
			{ 3094u, value187 },
			{ 3095u, value188 },
			{ 3096u, value189 },
			{ 3097u, value190 },
			{ 3098u, value191 },
			{ 3099u, value192 },
			{ 3100u, value193 },
			{ 3101u, value194 },
			{ 3102u, value195 },
			{ 3103u, value196 },
			{ 3104u, value197 },
			{ 3105u, value198 },
			{ 3106u, value199 },
			{ 3107u, value200 },
			{ 3108u, value201 },
			{ 3109u, value202 },
			{ 3110u, value203 },
			{ 3111u, value204 },
			{ 3112u, value205 },
			{ 3113u, value206 },
			{ 3114u, value207 },
			{ 3115u, value208 },
			{ 3116u, value209 },
			{ 3117u, value210 },
			{ 3118u, value211 },
			{ 3119u, value212 },
			{ 3120u, value213 },
			{ 3121u, value214 },
			{ 3122u, value215 },
			{ 3123u, value216 },
			{ 3124u, value217 },
			{ 3125u, value218 },
			{ 3126u, value219 },
			{ 3127u, value220 },
			{ 3128u, value221 },
			{ 3129u, value222 },
			{ 3130u, value223 },
			{ 3131u, value224 },
			{ 3132u, value225 },
			{ 3133u, value226 },
			{ 3134u, value227 },
			{ 3135u, value228 },
			{ 3136u, value229 },
			{ 3137u, value230 },
			{ 3138u, value231 },
			{ 3139u, value232 },
			{ 3140u, value233 },
			{ 3141u, value234 },
			{ 3142u, value235 },
			{ 3143u, value236 },
			{ 3144u, value237 },
			{ 3145u, value238 },
			{ 3146u, value239 },
			{ 3147u, value240 },
			{ 3148u, value241 },
			{ 3149u, value242 },
			{ 3150u, value243 },
			{ 3151u, value244 },
			{ 3152u, value245 },
			{ 3153u, value246 },
			{ 3154u, value247 },
			{ 3155u, value248 },
			{ 3156u, value249 },
			{ 3157u, value250 },
			{ 3158u, value251 },
			{ 3159u, value252 },
			{ 3160u, value253 },
			{ 3161u, value254 },
			{ 3162u, value255 },
			{ 3163u, value256 },
			{ 3164u, value257 },
			{ 3165u, value258 },
			{ 3166u, value259 },
			{ 3167u, value260 },
			{ 3168u, value261 },
			{ 3169u, value262 },
			{ 3170u, value263 },
			{ 3171u, value264 },
			{ 3172u, value265 },
			{ 3173u, value266 },
			{ 3174u, value267 },
			{ 3175u, value268 },
			{ 3176u, value269 },
			{ 3177u, value270 },
			{ 3178u, value271 },
			{ 3179u, value272 },
			{ 3180u, value273 },
			{ 3181u, value274 },
			{ 3182u, value275 },
			{ 3183u, value276 },
			{ 3184u, value277 },
			{ 3185u, value278 },
			{ 6000u, value279 },
			{ 6001u, value280 },
			{ 6002u, value281 },
			{ 6003u, value282 },
			{ 6004u, value283 },
			{ 6005u, value284 },
			{ 6006u, value285 },
			{ 6007u, value286 },
			{ 6008u, value287 },
			{ 6009u, value288 },
			{ 6010u, value289 },
			{ 6011u, value290 },
			{ 6012u, value291 },
			{ 6013u, value292 },
			{ 6014u, value293 },
			{ 6015u, value294 },
			{ 6016u, value295 },
			{ 6017u, value296 },
			{ 6018u, value297 },
			{ 6019u, value298 },
			{ 6020u, value299 },
			{ 6021u, value300 },
			{ 6022u, value301 },
			{ 6023u, value302 },
			{ 6024u, value303 },
			{ 6025u, value304 },
			{ 6026u, value305 },
			{ 6027u, value306 },
			{ 6028u, value307 },
			{ 6029u, value308 },
			{ 6030u, value309 },
			{ 6031u, value310 },
			{ 6032u, value311 },
			{ 6033u, value312 },
			{ 6034u, value28 },
			{ 6035u, value313 },
			{ 6036u, value314 },
			{ 6037u, value315 },
			{ 6038u, value316 },
			{ 6039u, value317 },
			{ 6040u, value318 },
			{ 6041u, value319 },
			{ 6042u, value320 },
			{ 6043u, value321 },
			{ 6044u, value322 },
			{ 6045u, value323 },
			{ 6046u, value324 },
			{ 6047u, value325 },
			{ 6048u, value326 },
			{ 6049u, value327 },
			{ 6050u, value328 },
			{ 6051u, value329 },
			{ 6052u, value330 },
			{ 6053u, value331 },
			{ 6054u, value332 },
			{ 6055u, value333 },
			{ 6056u, value334 },
			{ 6057u, value335 },
			{ 6058u, value336 },
			{ 6059u, value337 },
			{ 6060u, value338 },
			{ 6061u, value339 },
			{ 6062u, value340 },
			{ 6063u, value341 },
			{ 6128u, value342 },
			{ 6129u, value343 },
			{ 6130u, value344 },
			{ 6131u, value345 },
			{ 6132u, value346 },
			{ 6133u, value347 },
			{ 6134u, value348 },
			{ 6135u, value349 },
			{ 6136u, value350 },
			{ 6137u, value351 },
			{ 6138u, value352 },
			{ 6139u, value353 },
			{ 6140u, value354 },
			{ 6141u, value355 },
			{ 6142u, value356 },
			{ 6143u, value357 },
			{ 6144u, value358 },
			{ 6145u, value359 },
			{ 6146u, value360 },
			{ 6147u, value361 },
			{ 6148u, value362 },
			{ 6149u, value363 },
			{ 6150u, value364 },
			{ 6151u, value365 },
			{ 6152u, value366 },
			{ 6153u, value367 },
			{ 6154u, value368 },
			{ 6155u, value369 },
			{ 6156u, value370 },
			{ 6157u, value371 },
			{ 6158u, value372 },
			{ 6159u, value373 },
			{ 6160u, value374 },
			{ 6161u, value375 },
			{ 6162u, value376 },
			{ 6163u, value377 },
			{ 6164u, value378 },
			{ 6165u, value379 },
			{ 6166u, value380 },
			{ 6167u, value381 },
			{ 6168u, value382 },
			{ 6169u, value383 },
			{ 6170u, value384 },
			{ 6171u, value385 },
			{ 6172u, value386 },
			{ 6173u, value387 },
			{ 6174u, value388 },
			{ 6175u, value389 },
			{ 6176u, value390 },
			{ 6177u, value391 },
			{ 6178u, value392 },
			{ 6179u, value393 },
			{ 6180u, value394 },
			{ 6181u, value395 },
			{ 6182u, value396 },
			{ 6183u, value397 },
			{ 6184u, value398 },
			{ 6185u, value399 },
			{ 6186u, value400 },
			{ 6187u, value401 },
			{ 6188u, value402 },
			{ 6189u, value403 },
			{ 6190u, value404 },
			{ 6191u, value405 },
			{ 6192u, value406 },
			{ 7000u, value407 },
			{ 7001u, value408 },
			{ 7002u, value409 },
			{ 7003u, value410 },
			{ 10001u, value411 },
			{ 10002u, value412 },
			{ 10003u, value413 },
			{ 10004u, value414 },
			{ 10005u, value415 },
			{ 10006u, value416 },
			{ 10007u, value417 },
			{ 10008u, value418 },
			{ 4096u, value419 },
			{ 4097u, value420 },
			{ 4098u, value421 },
			{ 4099u, value422 },
			{ 4100u, value423 },
			{ 4101u, value424 },
			{ 7424u, value425 },
			{ 7425u, value426 },
			{ 7426u, value427 },
			{ 7436u, value428 },
			{ 7437u, value429 },
			{ 7438u, value430 },
			{ 7439u, value431 },
			{ 7440u, value432 },
			{ 7441u, value433 },
			{ 7442u, value434 },
			{ 7443u, value435 },
			{ 7444u, value436 },
			{ 7445u, value437 },
			{ 7446u, value438 },
			{ 7447u, value439 },
			{ 7457u, value440 },
			{ 8192u, value441 },
			{ 8193u, value442 },
			{ 8194u, value443 },
			{ 8195u, value444 },
			{ 8196u, value445 },
			{ 8197u, value446 },
			{ 8198u, value447 },
			{ 12288u, value448 },
			{ 12289u, value449 },
			{ 12290u, value450 },
			{ 12291u, value451 },
			{ 12292u, value452 },
			{ 12293u, value453 },
			{ 12294u, value454 },
			{ 12295u, value455 },
			{ 16384u, value456 },
			{ 16385u, value457 },
			{ 16386u, value458 },
			{ 16387u, value459 },
			{ 16388u, value460 },
			{ 16389u, value461 },
			{ 16390u, value462 },
			{ 16391u, value463 },
			{ 16392u, value464 },
			{ 16393u, value465 },
			{ 16394u, value466 },
			{ 16395u, value467 },
			{ 16396u, value468 },
			{ 16397u, value469 },
			{ 20480u, value470 },
			{ 20481u, value471 },
			{ 20482u, value472 },
			{ 20483u, value473 },
			{ 20484u, value474 },
			{ 24576u, value475 },
			{ 24577u, value476 },
			{ 24578u, value477 },
			{ 24579u, value478 },
			{ 24580u, value479 },
			{ 24581u, value480 },
			{ 28672u, value481 },
			{ 28673u, value482 },
			{ 28674u, value483 },
			{ 28675u, value484 },
			{ 28676u, value485 },
			{ 28693u, value486 },
			{ 28695u, value487 },
			{ 28708u, value488 },
			{ 32768u, value489 },
			{ 32769u, value490 },
			{ 36864u, value491 },
			{ 36865u, value492 },
			{ 36866u, value493 },
			{ 36867u, value494 },
			{ 36868u, value495 },
			{ 36869u, value496 },
			{ 36870u, value497 },
			{ 36871u, value498 },
			{ 36872u, value499 },
			{ 36873u, value500 },
			{ 36874u, value501 },
			{ 36875u, value502 },
			{ 36876u, value503 },
			{ 36877u, value504 },
			{ 36878u, value505 },
			{ 36879u, value506 },
			{ 36880u, value507 },
			{ 36881u, value508 },
			{ 36882u, value509 },
			{ 36883u, value510 },
			{ 36884u, value511 },
			{ 36885u, value512 },
			{ 40960u, value513 },
			{ 40961u, value514 },
			{ 40962u, value515 },
			{ 40963u, value516 },
			{ 40964u, value517 },
			{ 40965u, value518 }
		};
		ErrorCodes_XFlash = new Dictionary<uint, string>
		{
			{ 0u, value },
			{ 3221291009u, value519 },
			{ 3221291010u, value520 },
			{ 3221291011u, value521 },
			{ 3221291012u, value522 },
			{ 3221291013u, value523 },
			{ 3221291014u, value524 },
			{ 3221291015u, value525 },
			{ 3221291016u, value526 },
			{ 3221291017u, value527 },
			{ 3221291018u, value528 },
			{ 3221291019u, value529 },
			{ 3221291020u, value530 },
			{ 3221291021u, value531 },
			{ 3221291022u, value532 },
			{ 3221291023u, value533 },
			{ 3221291024u, value534 },
			{ 3221291025u, value535 },
			{ 3221356545u, value536 },
			{ 3221356546u, value537 },
			{ 3221356547u, value538 },
			{ 3221356548u, value539 },
			{ 3221356549u, value540 },
			{ 3221356550u, value541 },
			{ 3221356551u, value542 },
			{ 3221356552u, value543 },
			{ 3221356553u, value544 },
			{ 3221356554u, value545 },
			{ 3221356555u, value546 },
			{ 3221356556u, value547 },
			{ 3221356557u, value548 },
			{ 3221356558u, value549 },
			{ 3221356559u, value550 },
			{ 3221356560u, value551 },
			{ 3221356561u, value552 },
			{ 3221356562u, value553 },
			{ 3221356563u, value554 },
			{ 3221356564u, value555 },
			{ 3221356565u, value556 },
			{ 3221356566u, value557 },
			{ 3221356567u, value558 },
			{ 3221356568u, value559 },
			{ 3221356569u, value560 },
			{ 3221356570u, value561 },
			{ 3221356571u, value562 },
			{ 3221356572u, value563 },
			{ 3221356573u, value564 },
			{ 3221356574u, value565 },
			{ 3221356575u, value566 },
			{ 3221356576u, value567 },
			{ 3221356577u, value568 },
			{ 3221356578u, value569 },
			{ 3221356579u, value570 },
			{ 3221356580u, value571 },
			{ 3221356581u, value572 },
			{ 3221356582u, value573 },
			{ 3221356583u, value574 },
			{ 3221356584u, value575 },
			{ 3221356585u, value576 },
			{ 3221356586u, value577 },
			{ 3221356587u, value578 },
			{ 3221356588u, value579 },
			{ 3221356589u, value580 },
			{ 3221356590u, value581 },
			{ 3221356591u, value582 },
			{ 3221356592u, value583 },
			{ 3221356593u, value584 },
			{ 3221356594u, value585 },
			{ 3221356595u, value586 },
			{ 3221356596u, value587 },
			{ 3221356597u, value588 },
			{ 3221356598u, value589 },
			{ 3221356599u, value590 },
			{ 3221356600u, value591 },
			{ 3221356601u, value592 },
			{ 3221356608u, value593 },
			{ 3221356609u, value594 },
			{ 3221356610u, value595 },
			{ 3221356611u, value596 },
			{ 3221356612u, value597 },
			{ 3221356613u, value598 },
			{ 3221356614u, value599 },
			{ 3221356615u, value600 },
			{ 3221356616u, value601 },
			{ 3221356617u, value602 },
			{ 3221356618u, value603 },
			{ 3221356619u, value525 },
			{ 3221356620u, value604 },
			{ 3221356621u, value605 },
			{ 3221356622u, value606 },
			{ 3221356623u, value607 },
			{ 3221356624u, value608 },
			{ 3221356625u, value609 },
			{ 3221356626u, value610 },
			{ 3221356627u, value611 },
			{ 3221356628u, value612 },
			{ 3221356629u, value613 },
			{ 3221356630u, value614 },
			{ 3221356631u, value615 },
			{ 3221356632u, value616 },
			{ 3221356633u, value617 },
			{ 3221356634u, value618 },
			{ 3221356635u, value619 },
			{ 3221356636u, value620 },
			{ 3221356637u, value621 },
			{ 3221356638u, value622 },
			{ 3221422081u, value623 },
			{ 3221422082u, value624 },
			{ 3221422083u, value625 },
			{ 3221422084u, value626 },
			{ 3221422085u, value627 },
			{ 3221422086u, value628 },
			{ 3221422087u, value629 },
			{ 3221422088u, value630 },
			{ 3221422089u, value631 },
			{ 3221422090u, value632 },
			{ 3221422091u, value633 },
			{ 3221422092u, value634 },
			{ 3221422093u, value635 },
			{ 3221422094u, value636 },
			{ 3221422095u, value637 },
			{ 3221422096u, value638 },
			{ 3221422097u, value639 },
			{ 3221422098u, value640 },
			{ 3221422099u, value641 },
			{ 3221422100u, value642 },
			{ 3221422101u, value643 },
			{ 3221422102u, value644 },
			{ 3221422103u, value645 },
			{ 3221422104u, value646 },
			{ 3221487617u, value647 },
			{ 3221487618u, value648 },
			{ 3221487619u, value649 },
			{ 3221487620u, value650 },
			{ 3221487621u, value651 },
			{ 3221487622u, value652 },
			{ 3221487623u, value653 },
			{ 3221487624u, value654 },
			{ 3221487625u, value655 },
			{ 3221487626u, value656 },
			{ 3221487627u, value657 },
			{ 3221487628u, value658 },
			{ 3221487664u, value659 },
			{ 3221487680u, value660 },
			{ 3221487681u, value661 },
			{ 3221487682u, value662 },
			{ 3221487683u, value663 },
			{ 3221487684u, value664 },
			{ 3221487685u, value665 },
			{ 3221487696u, value666 },
			{ 3221487712u, value667 },
			{ 3221487872u, value668 },
			{ 3221487874u, value669 },
			{ 3221488128u, value670 },
			{ 3221488129u, value671 },
			{ 3221488130u, value672 },
			{ 3221488131u, value673 },
			{ 3221488132u, value674 },
			{ 3221488133u, value675 },
			{ 3221488134u, value676 },
			{ 3221488135u, value677 },
			{ 3221488136u, value678 },
			{ 3221488137u, value679 },
			{ 3221488138u, value680 },
			{ 3221488139u, value681 },
			{ 3221488140u, value682 },
			{ 3221488141u, value683 },
			{ 3221488142u, value684 },
			{ 3221553153u, value685 },
			{ 3221553154u, value686 },
			{ 3221553155u, value687 },
			{ 3221553156u, value688 },
			{ 3221553157u, value689 },
			{ 3221553158u, value690 },
			{ 3221553159u, value691 },
			{ 3221553160u, value692 },
			{ 3221553161u, value693 },
			{ 3221553162u, value694 },
			{ 3221553163u, value695 },
			{ 3221553164u, value696 },
			{ 3221553165u, value697 },
			{ 3221618689u, value698 },
			{ 3221618690u, value699 },
			{ 3221618691u, value700 },
			{ 3221618692u, value701 },
			{ 3221618693u, value702 },
			{ 3221618694u, value703 },
			{ 3221684225u, value704 },
			{ 3221684226u, value705 },
			{ 3221684227u, value706 },
			{ 3221684228u, value707 },
			{ 3221684229u, value708 }
		};
		ErrorCodes_Legacy = new Dictionary<uint, string>
		{
			{ 3000u, value709 },
			{ 3001u, value710 },
			{ 3002u, value711 },
			{ 3003u, value712 },
			{ 3004u, value713 },
			{ 3005u, value714 },
			{ 3006u, value715 },
			{ 3007u, value716 },
			{ 3008u, value717 },
			{ 3009u, value718 },
			{ 3010u, value719 },
			{ 3011u, value720 },
			{ 3012u, value721 },
			{ 3013u, value722 },
			{ 3014u, value723 },
			{ 3015u, value724 },
			{ 3016u, value725 },
			{ 3017u, value726 },
			{ 3018u, value727 },
			{ 3019u, value728 },
			{ 3020u, value729 },
			{ 3021u, value730 },
			{ 3022u, value731 },
			{ 3023u, value732 },
			{ 3024u, value733 },
			{ 3025u, value734 },
			{ 3026u, value735 },
			{ 3027u, value736 },
			{ 3028u, value737 },
			{ 3029u, value738 },
			{ 3030u, value739 },
			{ 3031u, value740 },
			{ 3032u, value741 },
			{ 3033u, value742 },
			{ 3034u, value743 },
			{ 3035u, value744 },
			{ 3036u, value745 },
			{ 3037u, value746 },
			{ 3038u, value747 },
			{ 3039u, value748 },
			{ 3040u, value749 },
			{ 3041u, value750 },
			{ 3042u, value751 },
			{ 3043u, value752 },
			{ 3044u, value753 },
			{ 3045u, value754 },
			{ 3046u, value755 },
			{ 3047u, value756 },
			{ 3048u, value757 },
			{ 3049u, value758 },
			{ 3050u, value759 },
			{ 3051u, value760 },
			{ 3052u, value761 },
			{ 3053u, value762 },
			{ 3054u, value763 },
			{ 3055u, value764 },
			{ 3056u, value765 },
			{ 3057u, value766 },
			{ 3058u, value767 },
			{ 3059u, value768 },
			{ 3060u, value769 },
			{ 3061u, value770 },
			{ 3062u, value771 },
			{ 3063u, value772 },
			{ 3064u, value773 },
			{ 3065u, value774 },
			{ 3066u, value775 },
			{ 3067u, value776 },
			{ 3068u, value777 },
			{ 3069u, value778 },
			{ 3070u, value779 },
			{ 3071u, value780 },
			{ 3072u, value781 },
			{ 3073u, value782 },
			{ 3074u, value783 },
			{ 3075u, value784 },
			{ 3076u, value785 },
			{ 3077u, value786 },
			{ 3078u, value787 },
			{ 3079u, value788 },
			{ 3080u, value789 },
			{ 3081u, value790 },
			{ 3082u, value791 },
			{ 3083u, value792 },
			{ 3084u, value793 },
			{ 3085u, value794 },
			{ 3086u, value795 },
			{ 3087u, value796 },
			{ 3088u, value797 },
			{ 3089u, value798 },
			{ 3090u, value799 },
			{ 3091u, value800 },
			{ 3092u, value801 },
			{ 3093u, value802 },
			{ 3094u, value803 },
			{ 3095u, value804 },
			{ 3096u, value805 },
			{ 3097u, value806 },
			{ 3098u, value807 },
			{ 3099u, value808 },
			{ 3100u, value809 },
			{ 3101u, value810 },
			{ 3102u, value811 },
			{ 3103u, value812 },
			{ 3104u, value813 },
			{ 3105u, value814 },
			{ 3106u, value815 },
			{ 3107u, value816 },
			{ 3108u, value817 },
			{ 3109u, value818 },
			{ 3110u, value819 },
			{ 3111u, value820 },
			{ 3112u, value821 },
			{ 3113u, value822 },
			{ 3114u, value823 },
			{ 3115u, value824 },
			{ 3116u, value825 },
			{ 3117u, value826 },
			{ 3118u, value827 },
			{ 3119u, value828 },
			{ 3120u, value829 },
			{ 3121u, value830 },
			{ 3122u, value831 },
			{ 3123u, value832 },
			{ 3124u, value833 },
			{ 3125u, value834 },
			{ 3126u, value835 },
			{ 3127u, value836 },
			{ 3128u, value837 },
			{ 3129u, value838 },
			{ 3130u, value839 },
			{ 3131u, value840 },
			{ 3132u, value841 },
			{ 3133u, value842 },
			{ 3134u, value843 },
			{ 3135u, value844 },
			{ 3136u, value845 },
			{ 3137u, value846 },
			{ 3138u, value847 },
			{ 3139u, value848 },
			{ 3140u, value849 },
			{ 3141u, value850 },
			{ 3142u, value851 },
			{ 3143u, value852 },
			{ 3144u, value853 },
			{ 3145u, value854 },
			{ 3146u, value855 },
			{ 3147u, value856 },
			{ 3148u, value857 },
			{ 3149u, value858 },
			{ 3150u, value859 },
			{ 3151u, value860 },
			{ 3152u, value861 },
			{ 3153u, value862 },
			{ 3154u, value863 },
			{ 3155u, value864 },
			{ 3156u, value865 },
			{ 3157u, value866 },
			{ 3158u, value867 },
			{ 3159u, value868 },
			{ 3160u, value869 },
			{ 3161u, value870 },
			{ 3162u, value871 },
			{ 3163u, value872 },
			{ 3164u, value873 },
			{ 3165u, value874 },
			{ 3166u, value875 },
			{ 3167u, value876 },
			{ 3168u, value877 },
			{ 3169u, value878 },
			{ 3170u, value879 },
			{ 3171u, value880 },
			{ 3172u, value881 },
			{ 3173u, value882 },
			{ 3174u, value883 },
			{ 3175u, value884 },
			{ 3176u, value885 },
			{ 3177u, value886 },
			{ 3178u, value887 },
			{ 3179u, value888 },
			{ 3180u, value889 },
			{ 3181u, value890 },
			{ 3182u, value891 },
			{ 3183u, value892 },
			{ 3184u, value893 },
			{ 3185u, value894 },
			{ 3186u, value895 },
			{ 3187u, value896 },
			{ 3188u, value897 },
			{ 3189u, value898 },
			{ 3190u, value899 },
			{ 3191u, value900 },
			{ 3192u, value901 },
			{ 3193u, value902 },
			{ 3194u, value903 },
			{ 3195u, value904 },
			{ 3196u, value905 },
			{ 3197u, value906 },
			{ 3198u, value907 },
			{ 3199u, value908 },
			{ 3200u, value909 },
			{ 3201u, value910 },
			{ 3202u, value911 },
			{ 3203u, value912 },
			{ 3204u, value913 },
			{ 3205u, value914 },
			{ 3206u, value915 },
			{ 3207u, value916 },
			{ 3208u, value917 },
			{ 3209u, value918 },
			{ 3210u, value919 },
			{ 3211u, value920 },
			{ 3212u, value921 },
			{ 3213u, value922 },
			{ 3214u, value923 },
			{ 3215u, value924 },
			{ 3216u, value925 },
			{ 3217u, value926 },
			{ 3218u, value927 },
			{ 3219u, value928 },
			{ 3220u, value929 },
			{ 3221u, value930 },
			{ 3222u, value931 },
			{ 3223u, value932 },
			{ 3224u, value933 },
			{ 3225u, value934 },
			{ 3226u, value935 },
			{ 3227u, value936 },
			{ 3228u, value937 },
			{ 3229u, value938 },
			{ 3230u, value939 },
			{ 3231u, value940 },
			{ 3232u, value941 },
			{ 3233u, value942 },
			{ 3234u, value943 },
			{ 3235u, value944 }
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
