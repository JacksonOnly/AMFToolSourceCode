using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AMFTool;

public class UI
{
	public static string lisgpt;

	public static Stopwatch watch;

	public static Color log;

	public static Color ok;

	public static Color info;

	public static Color error;

	public static Color abu;

	public static Color warning;

	public static RichTextBox Richtextbox;

	public static PrivateFontCollection FontCollection;

	public static Font Reguler;

	public static Font Bold;

	public static string SectorSize;

	public static string Server;

	public static int totalchecked;

	public static long ReadUserareaSize;

	public static LabelControl LabelSpeed;

	public static ProgressBarControl Progressbar;

	public static string ReaddumpSize { get; set; }

	public static void LoadFont()
	{
		string path = "Data";
		string path2 = "font";
		string path3 = "Font1.ttf";
		string path4 = "Font2.ttf";
		float field_ = Class38.field_375;
		string path5 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path, path2);
		string filename = Path.Combine(path5, path3);
		string filename2 = Path.Combine(path5, path4);
		FontCollection.AddFontFile(filename);
		FontCollection.AddFontFile(filename2);
		FontFamily fontFamily = FontCollection.Families[0];
		FontFamily family = ((FontCollection.Families.Length > 1) ? FontCollection.Families[1] : fontFamily);
		Reguler = new Font(fontFamily, field_, FontStyle.Regular, GraphicsUnit.Point);
		Bold = new Font(family, field_, FontStyle.Bold, GraphicsUnit.Point);
	}

	public static void RichLogs(string msg, Color color, bool nextline = false, bool bold = false)
	{
		if (Richtextbox.InvokeRequired)
		{
			Richtextbox.Invoke((Action)delegate
			{
				Mesages(msg, color, nextline, bold);
			});
		}
		else
		{
			Mesages(msg, color, nextline, bold);
		}
		static void Mesages(string text2, Color selectionColor, bool flag = false, bool flag2 = false)
		{
			string text = "";
			if (!string.IsNullOrEmpty(text2) || flag)
			{
				if (string.IsNullOrEmpty(text2) && flag)
				{
					text2 = text;
				}
				int textLength = Richtextbox.TextLength;
				Richtextbox.SelectionStart = textLength;
				Richtextbox.SelectionLength = 0;
				Richtextbox.SelectionColor = selectionColor;
				Richtextbox.SelectionFont = (flag2 ? Bold : Reguler);
				Richtextbox.AppendText(text2);
				if (flag)
				{
					Richtextbox.AppendText(Environment.NewLine);
				}
				Richtextbox.SelectionColor = Color.White;
				Richtextbox.SelectionFont = Reguler;
				Richtextbox.SelectionStart = Richtextbox.TextLength;
				Richtextbox.ScrollToCaret();
			}
		}
	}

	public static void ClearLog()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (!Richtextbox.InvokeRequired)
		{
			Richtextbox.Clear();
			return;
		}
		Richtextbox.Invoke((Action)delegate
		{
			Richtextbox.Clear();
		});
	}

	public static void LabelWrite(string Speed, string process, string total)
	{
		LabelSpeed.Invoke((Action)delegate
		{
			string text = "File Part Size : ";
			string text2 = "/";
			string text3 = " | USB Speed : ";
			string text4 = "/s";
			LabelSpeed.Text = text + process + text2 + total + text3 + Speed + text4;
		});
	}

	public static void Progresbar1(long prosess, long total)
	{
		if (total <= 0L)
		{
			return;
		}
		int percent = (int)(prosess * 100L / total);
		if (percent > 100)
		{
			percent = 100;
			if (percent < 0)
			{
				percent = 0;
				Progressbar.Invoke((Action)delegate
				{
					Progressbar.Position = percent;
				});
			}
			else
			{
				Progressbar.Invoke((Action)delegate
				{
					Progressbar.Position = percent;
				});
			}
		}
		else if (percent < 0)
		{
			percent = 0;
			Progressbar.Invoke((Action)delegate
			{
				Progressbar.Position = percent;
			});
		}
		else
		{
			Progressbar.Invoke((Action)delegate
			{
				Progressbar.Position = percent;
			});
		}
	}

	public static void KillProcess()
	{
		int num = 0;
		string text = "adb.exe";
		int num2 = 1;
		string text2 = "adb";
		string text3 = "amf.exe";
		string text4 = "amf";
		string[] array = new string[4] { text, text2, text3, text4 };
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i += num2)
		{
			string value = array2[i];
			Process[] processes = Process.GetProcesses();
			Process[] array3 = processes;
			Process[] array4 = array3;
			for (int j = num; j < array4.Length; j += num2)
			{
				Process process = array4[j];
				if (process.ProcessName.Contains(value))
				{
					try
					{
						process.Kill();
					}
					catch
					{
					}
					process.WaitForExit();
				}
			}
		}
	}

	public static void ProgressBar1(long process, long total)
	{
		Progresbar1(process, total);
	}

	public static void Progresbar1(int process)
	{
		if (process <= 100)
		{
			Progressbar.Invoke((Action)delegate
			{
				Progressbar.Position = process;
			});
			return;
		}
		process = 100;
		Progressbar.Invoke((Action)delegate
		{
			Progressbar.Position = process;
		});
	}

	public static void ProgressBar1(int process)
	{
		Progresbar1(process);
	}

	public static T Showcust<T>(T form) where T : Form
	{
		form.StartPosition = FormStartPosition.CenterParent;
		form.TopMost = true;
		form.ShowInTaskbar = false;
		form.ShowIcon = true;
		return form;
	}

	static UI()
	{
		string sectorSize = "512";
		string server = "https://amftool.com/";
		watch = new Stopwatch();
		log = Color.FromArgb(64, 64, 64);
		ok = Color.FromArgb(59, 156, 74);
		info = Color.FromArgb(1, 92, 170);
		error = Color.Tomato;
		abu = Color.FromArgb(124, 124, 124);
		warning = Color.OrangeRed;
		Richtextbox = new RichTextBox();
		FontCollection = new PrivateFontCollection();
		SectorSize = sectorSize;
		Server = server;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
