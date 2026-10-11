namespace AMFTool.Library.Mediatek.Download_Agent;

public class Storage
{
	public MtkClass Mtk;

	public DAConfig Dacfg;

	public EmmcInfo Emmc = new EmmcInfo();

	public UfsInfo Ufs = new UfsInfo();

	public NandInfo Nand = new NandInfo();

	public NorInfo Nor = new NorInfo();

	public RamInfo Sram = new RamInfo();

	public RamInfo Dram = new RamInfo();

	public string Storagetype;

	public Storage(MtkClass mtk, DAConfig dacfg)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Mtk = mtk;
		Dacfg = dacfg;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
