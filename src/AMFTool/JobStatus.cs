using System.Threading;

namespace AMFTool;

public static class JobStatus
{
	public static bool Busy;

	public static CancellationToken Token { get; set; }

	public static CancellationTokenSource Source { get; set; }

	public static void Init()
	{
		Source = new CancellationTokenSource();
		Token = Source.Token;
	}

	public static void Operation()
	{
		string msg = "Operation : ";
		string msg2 = "Brand : ";
		string msg3 = "Model : ";
		string msg4 = "Selected Mode : ";
		string msg5 = "";
		UI.RichLogs(msg, UI.log);
		UI.RichLogs(State.Operation, UI.info, nextline: true, bold: true);
		UI.RichLogs(msg2, UI.log);
		UI.RichLogs(DeviceState.Brand, UI.info, nextline: true, bold: true);
		UI.RichLogs(msg3, UI.log);
		UI.RichLogs(DeviceState.Model, UI.info, nextline: true, bold: true);
		UI.RichLogs(msg4, UI.log);
		UI.RichLogs(State.Mode.ToString(), UI.info, nextline: true, bold: true);
		UI.RichLogs(msg5, UI.log, nextline: true, bold: true);
	}

	public static void Cancel()
	{
		Source.Cancel();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
