using System.Collections.Generic;

namespace AMFTool.Library.Mediatek.Download_Agent;

public class DA
{
	public bool v6;

	public string Loader;

	public byte[] DownloadAgent;

	public ushort Magic;

	public ushort Hwcode;

	public ushort HwSubCode;

	public ushort HwVersion;

	public ushort SwVersion;

	public ushort PageSize;

	public ushort Reserved1;

	public ushort Reserved3;

	public ushort EntryRegionIndex;

	public ushort EntryRegionCount;

	public List<EntryRegion> Region;

	public DA(byte[] data, bool v6 = false)
	{
		object obj = (DownloadAgent = (byte[])(object)(Loader = null));
		PageSize = 512;
		EntryRegionIndex = 1;
		EntryRegionCount = 0;

		Loader = (string)obj;
		DownloadAgent = (byte[])obj;
		this.v6 = v6;
		StructHelper structHelper = new StructHelper(data);
		Magic = structHelper.Short();
		Hwcode = structHelper.Short();
		HwSubCode = structHelper.Short();
		HwVersion = structHelper.Short();
		SwVersion = structHelper.Short();
		PageSize = structHelper.Short();
		Reserved1 = structHelper.Short();
		Reserved3 = structHelper.Short();
		EntryRegionIndex = structHelper.Short();
		EntryRegionCount = structHelper.Short();
		Region = new List<EntryRegion>();
		for (int i = 0; i < EntryRegionCount; i++)
		{
			EntryRegion item = new EntryRegion(structHelper.Bytes(20));
			Region.Add(item);
		}
	}

	public void SetFileName(string loaderfilename)
	{
		Loader = loaderfilename;
	}

	public void SetDownloadAgent(byte[] dagent)
	{
		DownloadAgent = dagent;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
