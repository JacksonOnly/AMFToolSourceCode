using Restored.Namespace2;

namespace AMFTool.ThreadWorker;

public class ThreadMain
{
	private ThreadEDL ThreadEDL;

	private ThreadDiag ThreadDiag;

	private ThreadMTK ThreadMTK;

	private ThreadMETA ThreadMETA;

	private ThreadSPD ThreadSPD;

	private ThreadHisi ThreadHisi;

	private ThreadMTP ThreadMTP;

	private ThreadOdin ThreadOdin;

	private ThreadAndroid ThreadAndroid;

	private ThreadFastboot ThreadFastboot;

	private ThreadExtra ThreadExtra;

	private ThreadSideload ThreadSideload;

	private Class27 ThreadATDiag;

	public ThreadMain()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		ThreadEDL = new ThreadEDL();
		ThreadDiag = new ThreadDiag();
		ThreadMTK = new ThreadMTK();
		ThreadMETA = new ThreadMETA();
		ThreadSPD = new ThreadSPD();
		ThreadHisi = new ThreadHisi();
		ThreadMTP = new ThreadMTP();
		ThreadOdin = new ThreadOdin();
		ThreadAndroid = new ThreadAndroid();
		ThreadFastboot = new ThreadFastboot();
		ThreadExtra = new ThreadExtra();
		ThreadSideload = new ThreadSideload();
		ThreadATDiag = new Class27();
	}

	public bool Init()
	{
		return State.Mode switch
		{
			SelectMode.Mediatek => ThreadMTK.Init(), 
			(SelectMode)2 => ThreadMETA.Init(), 
			SelectMode.Qualcomm => ThreadEDL.Init(), 
			(SelectMode)4 => ThreadDiag.Init(), 
			SelectMode.Spreadtrum => ThreadSPD.Init(), 
			(SelectMode)6 => ThreadHisi.Init(), 
			SelectMode.MTP => ThreadMTP.Init(), 
			(SelectMode)8 => ThreadOdin.Init(), 
			SelectMode.Adb => ThreadAndroid.Init(), 
			(SelectMode)10 => ThreadFastboot.Init(), 
			SelectMode.Extra => ThreadExtra.Init(), 
			(SelectMode)12 => ThreadSideload.Init(), 
			SelectMode.ATDiag => ThreadATDiag.method_01(), 
			_ => true, 
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
