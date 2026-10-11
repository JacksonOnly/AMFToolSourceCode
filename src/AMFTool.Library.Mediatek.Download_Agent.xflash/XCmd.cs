namespace AMFTool.Library.Mediatek.Download_Agent.xflash;

public enum XCmd : uint
{
	READREGISTER = 983042u,
	WRITEREGISTER = 983044u,
	RPMB_SET_KEY = 983046u,
	RPMB_INIT = 983048u,
	RPMB_READ_UFS = 983049u,
	SEJ_HW = 983051u
}
