using System.Runtime.InteropServices;

namespace AMFTool.Library.Mediatek.Download_Agent.xflash;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SecurityDA
{
	public uint is_security_enabled;

	public uint sec_enc_seccfg;

	public uint mmc_get_card;

	public uint mmc_set_part_config;

	public uint mmc_rpmb_send_command;

	public uint tzclk_on_addr;

	public uint tzclk_on_value;

	public uint tzclk_off_addr;

	public uint tzclk_off_value;

	public uint dxcc_base;

	public uint g_ufs_hba;

	public uint ufshcd_get_free_tag;

	public uint ufshcd_queuecommand;

	public uint ufshcd_put_tag;

	public uint mmc_permanent_cmd;

	public uint mmc_permanent_patch;
}
