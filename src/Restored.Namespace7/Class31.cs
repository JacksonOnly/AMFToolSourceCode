using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Restored.Namespace6;

namespace Restored.Namespace7;

internal class Class31
{
	public delegate void SwDownloadEventHandler(object sender, string information);

	public enum ValidationMethodOption
	{
		ValidateDuringDownload,
		ValidateAfterDownload
	}

	private Delegate4 field_01;

	private string field_02 = "";

	private int field_03 = 1;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private SwDownloadEventHandler field_04;

	public event SwDownloadEventHandler ED4C298C
	{
		[CompilerGenerated]
		add
		{
			SwDownloadEventHandler swDownloadEventHandler = field_04;
			SwDownloadEventHandler swDownloadEventHandler2 = swDownloadEventHandler;
			while (true)
			{
				SwDownloadEventHandler value2 = (SwDownloadEventHandler)Delegate.Combine(swDownloadEventHandler2, value);
				swDownloadEventHandler = Interlocked.CompareExchange(ref field_04, value2, swDownloadEventHandler2);
				if ((object)swDownloadEventHandler == swDownloadEventHandler2)
				{
					break;
				}
				swDownloadEventHandler2 = swDownloadEventHandler;
			}
		}
		[CompilerGenerated]
		remove
		{
			SwDownloadEventHandler swDownloadEventHandler = field_04;
			SwDownloadEventHandler swDownloadEventHandler2 = swDownloadEventHandler;
			SwDownloadEventHandler value2 = (SwDownloadEventHandler)Delegate.Remove(swDownloadEventHandler2, value);
			int num = 871907027;
			while (true)
			{
				_ = num ^ 0x1B69C648;
				swDownloadEventHandler = Interlocked.CompareExchange(ref field_04, value2, swDownloadEventHandler2);
				if ((object)swDownloadEventHandler == swDownloadEventHandler2)
				{
					break;
				}
				swDownloadEventHandler2 = swDownloadEventHandler;
				value2 = (SwDownloadEventHandler)Delegate.Remove(swDownloadEventHandler2, value);
				num = 871907027;
			}
		}
	}

	public Class31()
	{
		field_01 = method_01;
	}

	[DllImport("data\\misc\\eMMCDownload.dll", SetLastError = true)]
	private static extern byte QEMMCLib_UploadEmmcImage(string E6687F4B, string AC6BD6D5, string E18BEAE2, string DD493BA, bool C924C9B8, ref uint FF3A16F, Delegate4 AD296A6E, string B511DB92 = "");

	[DllImport("data\\misc\\eMMCDownload.dll", SetLastError = true)]
	private static extern byte QEMMCLib_UploadEmmcImageWithValidation(string D078B19D, string CA66B96E, string B6D28202, string D4104228, ref uint DFC2A10A, Delegate4 FE932D80, string EDDDE1C8 = "");

	private bool method_01(IntPtr D38D3D42, int CCA1BB55)
	{
		try
		{
			byte[] array = new byte[CCA1BB55];
			Marshal.Copy(D38D3D42, array, 0, CCA1BB55);
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			string b878F1C = uTF8Encoding.GetString(array);
			method_02(b878F1C);
			return true;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void method_02(string B878F1C2)
	{
		field_04?.Invoke(this, B878F1C2);
	}

	public void method_03(int B63F12AF, string CA8EBE0C)
	{
		field_03 = B63F12AF;
		field_02 = CA8EBE0C;
	}

	public void method_04(string EA87278B, string EBF60EE4, string D4749FAA, string E3AD55FA, bool C0A5A171, ref uint F47B4CC2, Delegate4 F1210962)
	{
		string text = "Failed to program the eMMC image to the phone:  ";
		byte b = 0;
		if (!C0A5A171)
		{
			b = QEMMCLib_UploadEmmcImage(EA87278B, EBF60EE4, D4749FAA, E3AD55FA, C0A5A171, ref F47B4CC2, F1210962, field_02);
			if (b != 1)
			{
				throw new Exception(text + F47B4CC2);
			}
		}
		else
		{
			b = ((field_03 != 1) ? QEMMCLib_UploadEmmcImage(EA87278B, EBF60EE4, D4749FAA, E3AD55FA, C0A5A171, ref F47B4CC2, F1210962, field_02) : QEMMCLib_UploadEmmcImageWithValidation(EA87278B, EBF60EE4, D4749FAA, E3AD55FA, ref F47B4CC2, F1210962, field_02));
			if (b != 1)
			{
				throw new Exception(text + F47B4CC2);
			}
		}
	}

	public static int method_05(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
