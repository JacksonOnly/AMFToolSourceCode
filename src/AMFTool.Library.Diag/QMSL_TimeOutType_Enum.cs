namespace AMFTool.Library.Diag;

public enum QMSL_TimeOutType_Enum
{
	QMSL_Timeout_IsPhoneConnected = 1,
	QMSL_Timeout_Nonsignaling = 3,
	QMSL_Timeout_CNV = 5,
	QMSL_Timeout_WriteData = 7,
	QMSL_Timeout_GSDI_Event = 9,
	QMSL_Timeout_Delay_ONLINE_FTM = 11,
	QMSL_Timeout_Delay_OFFLINE = 13,
	QMSL_Timeout_Delay_MediaFLO_StatusCheck = 15,
	QMSL_Timeout_BT_HCI_Response = 17,
	QMSL_Timeout_Embedded_Sequencer_Execution = 19,
	QMSL_Timeout_ListSizeMax = 21
}
