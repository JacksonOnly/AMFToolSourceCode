using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using AMFTool;
using DevExpress.XtraEditors;

namespace Restored.Namespace1;

internal static class Class26
{
	private static Mutex field_01;

	[STAThread]
	private static void Main()
	{
		ServicePointManager.Expect100Continue = true;
		Class25.method_01();
		string name = "AMFTool";
		string text = "AMFTool Already RUNNING";
		string caption = "Info";
		field_01 = new Mutex(initiallyOwned: true, name, out var createdNew);
		if (!createdNew)
		{
			XtraMessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new Auth());
		field_01.ReleaseMutex();
	}

	public static int method_02(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}

