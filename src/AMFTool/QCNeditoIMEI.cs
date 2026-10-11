using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AMFTool.Properties;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Microsoft.VisualBasic;

namespace AMFTool;

public class QCNeditoIMEI : XtraForm
{
	public QCNeditoIMEI load;

	public RichTextBox cmdex;

	public string Qcn;

	public string imei1;

	public string imei2;

	public string extra1;

	public string extra2;

	private IContainer components;

	internal CheckBox Ck_sim2Only;

	internal CheckBox Ck_SingleImei;

	internal LabelControl LabelControl1;

	private Button OpenQCN;

	internal SimpleButton SaveQCN;

	internal SimpleButton WriteQCN;

	internal CheckBox CK_Dualimei;

	internal CheckBox Ck_Imei2qc;

	internal CheckBox Ck_Imei1qc;

	public TextEdit Valid_Imei2qc;

	public TextEdit Valid_Imei1qc;

	public TextEdit Txt_Imei2qc;

	public TextEdit Txt_Imei1qc;

	internal TextEdit Txt_QCN;

	public QCNeditoIMEI()
	{
		object obj = null;
		Qcn = string.Empty;
		imei1 = string.Empty;
		imei2 = string.Empty;
		extra1 = string.Empty;
		extra2 = string.Empty;
		components = (IContainer)obj;

		InitializeComponent();
	}

	public QCNeditoIMEI(object[] data, string header)
	{
		object obj = null;
		Qcn = string.Empty;
		imei1 = string.Empty;
		imei2 = string.Empty;
		extra1 = string.Empty;
		extra2 = string.Empty;
		components = (IContainer)obj;

		InitializeComponent();
	}

	private void OpenQCN_Click(object sender, EventArgs e)
	{
		string filter = "QCN Files|*.qcn;*.xqcn|All Files|*.*";
		string text = "00000000000000";
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = filter
		};
		if (!openFileDialog.ShowDialog().Equals(DialogResult.OK))
		{
			return;
		}
		Txt_QCN.Text = openFileDialog.SafeFileName;
		Qcn = openFileDialog.FileName;
		QCNread(Qcn);
		Txt_Imei1qc.Text = IMEI_fixing(extra2, Txt_Imei1qc.Text).Substring(0, 14);
		bool flag = true;
		string text2 = IMEI_fixing(extra1, Txt_Imei2qc.Text);
		string text3 = text2;
		foreach (char c in text3)
		{
			if (!char.IsNumber(c))
			{
				flag = false;
			}
		}
		if (!flag)
		{
			Txt_Imei2qc.Text = text;
		}
		else
		{
			Txt_Imei2qc.Text = IMEI_fixing(extra1, Txt_Imei2qc.Text).Substring(0, 14);
		}
	}

	public string IMEI_fixing(string IMEIinput, string IMEIinput2)
	{
		char c = Convert.ToChar(IMEIinput.Substring(0, 1));
		string text = IMEIinput.Substring(2);
		char[] array = text.ToCharArray();
		ArrayList arrayList = new ArrayList();
		arrayList.Add(c);
		arrayList.Add(array[1]);
		arrayList.Add(array[0]);
		arrayList.Add(array[3]);
		arrayList.Add(array[2]);
		arrayList.Add(array[5]);
		arrayList.Add(array[4]);
		arrayList.Add(array[7]);
		arrayList.Add(array[6]);
		arrayList.Add(array[9]);
		arrayList.Add(array[8]);
		arrayList.Add(array[11]);
		arrayList.Add(array[10]);
		arrayList.Add(array[13]);
		arrayList.Add(array[12]);
		StringBuilder stringBuilder = new StringBuilder();
		if (arrayList.Count == 15)
		{
			for (int i = 0; i <= 14; i++)
			{
				stringBuilder.Append(arrayList[i]);
			}
		}
		return Assign(ref IMEIinput2, stringBuilder.ToString());
	}

	private static T Assign<T>(ref T target, T value)
	{
		target = value;
		return value;
	}

	public void SingleSimReg(string save, bool write = false)
	{
		string msg = " ⦁ IMEI 1 : ";
		int buttons = 0;
		int num = 1;
		string msg2 = "Generating QCN for Single SIM...";
		string text = "00";
		string msg3 = "Failed";
		string caption = "Error !";
		int icon = 16;
		string msg4 = "OK";
		string text2 = "Completed Successfully !";
		string caption2 = "Successful...";
		int icon2 = 64;
		if (write)
		{
			UI.RichLogs(msg, Color.White);
			UI.RichLogs(DeviceState.IMEI1, Color.DeepSkyBlue, nextline: true, bold: true);
			UI.RichLogs(msg2, Color.White);
		}
		string[] array = IMEIreverse(DeviceState.IMEI2);
		string[] array2 = new string[15];
		for (int i = 0; i <= 14; i++)
		{
			array2[i] = text;
		}
		BinaryReader binaryReader = new BinaryReader(File.Open(Qcn, FileMode.Open), Encoding.ASCII);
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(save, FileMode.Create), Encoding.ASCII);
		long num2 = int.Parse(imei2);
		long num3 = int.Parse(imei1);
		long length = binaryReader.BaseStream.Length;
		try
		{
			do
			{
				byte value = binaryReader.ReadByte();
				binaryWriter.Write(value);
				if (binaryReader.BaseStream.Position == num2)
				{
					int location = 0;
					while (location < array.Length)
					{
						binaryWriter.Write(byte.Parse(array[location], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array.Length;
				}
				else if (binaryReader.BaseStream.Position == num3)
				{
					int location2 = 0;
					while (location2 < array2.Length)
					{
						binaryWriter.Write(byte.Parse(array2[location2], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location2);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array2.Length;
				}
			}
			while (binaryReader.BaseStream.Position != length);
		}
		catch (EndOfStreamException ex)
		{
			if (write)
			{
				UI.RichLogs(msg3, Color.Red, (byte)num != 0, (byte)num != 0);
				UI.RichLogs(ex.Message, Color.Red, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(ex.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
			}
		}
		catch (ArgumentException ex2)
		{
			if (write)
			{
				UI.RichLogs(msg3, Color.Red, (byte)num != 0, (byte)num != 0);
				UI.RichLogs(ex2.Message, Color.Red, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(ex2.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
			}
		}
		finally
		{
			binaryReader.Close();
			binaryReader.Dispose();
			binaryWriter.Close();
			binaryWriter.Dispose();
			if (!write)
			{
				XtraMessageBox.Show(text2, caption2, (MessageBoxButtons)buttons, (MessageBoxIcon)icon2);
			}
			else
			{
				UI.RichLogs(msg4, Color.GreenYellow, (byte)num != 0, (byte)num != 0);
			}
		}
	}

	public void Sim2Only(string save, bool write = false)
	{
		string msg = " ⦁ IMEI 2 : ";
		int buttons = 0;
		int num = 1;
		string msg2 = "Generating QCN for SIM 2 Only...";
		string text = "00";
		string msg3 = "Failed";
		string caption = "Error !";
		int icon = 16;
		string msg4 = "OK";
		string text2 = "Completed Successfully !";
		string caption2 = "Successful...";
		int icon2 = 64;
		if (write)
		{
			UI.RichLogs(msg, Color.White);
			UI.RichLogs(DeviceState.IMEI2, Color.DeepSkyBlue, nextline: true, bold: true);
			UI.RichLogs(msg2, Color.White);
		}
		string[] array = new string[15];
		string[] array2 = IMEIreverse(DeviceState.IMEI2);
		for (int i = 0; i <= 14; i++)
		{
			array[i] = text;
		}
		BinaryReader binaryReader = new BinaryReader(File.Open(Qcn, FileMode.Open), Encoding.ASCII);
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(save, FileMode.Create), Encoding.ASCII);
		long num2 = int.Parse(imei2);
		long num3 = int.Parse(imei1) - 7L;
		long length = binaryReader.BaseStream.Length;
		try
		{
			do
			{
				byte value = binaryReader.ReadByte();
				binaryWriter.Write(value);
				if (binaryReader.BaseStream.Position == num2)
				{
					int location = 0;
					while (location < array.Length)
					{
						binaryWriter.Write(byte.Parse(array[location], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array.Length;
				}
				else if (binaryReader.BaseStream.Position == num3)
				{
					int location2 = 0;
					while (location2 < array2.Length)
					{
						binaryWriter.Write(byte.Parse(array2[location2], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location2);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array2.Length;
				}
			}
			while (binaryReader.BaseStream.Position != length);
		}
		catch (EndOfStreamException ex)
		{
			if (!write)
			{
				XtraMessageBox.Show(ex.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
				return;
			}
			UI.RichLogs(msg3, Color.Red, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(ex.Message, Color.Red, (byte)num != 0, (byte)num != 0);
		}
		catch (ArgumentException ex2)
		{
			if (!write)
			{
				XtraMessageBox.Show(ex2.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
				return;
			}
			UI.RichLogs(msg3, Color.Red, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(ex2.Message, Color.Red, (byte)num != 0, (byte)num != 0);
		}
		finally
		{
			binaryReader.Close();
			binaryReader.Dispose();
			binaryWriter.Close();
			binaryWriter.Dispose();
			if (!write)
			{
				XtraMessageBox.Show(text2, caption2, (MessageBoxButtons)buttons, (MessageBoxIcon)icon2);
			}
			else
			{
				UI.RichLogs(msg4, Color.GreenYellow, (byte)num != 0, (byte)num != 0);
			}
		}
	}

	private void DualSimReg(string save, bool write = false)
	{
		string msg = " ● IMEI 1 : ";
		int buttons = 0;
		int num = 1;
		string msg2 = " ● IMEI 2 : ";
		string msg3 = "Generating QCN for Dual SIM...";
		string msg4 = "Failed";
		string caption = "Error !";
		int icon = 16;
		string msg5 = "OK";
		string text = "Completed Successfully !";
		string caption2 = "Successful...";
		int icon2 = 64;
		if (write)
		{
			UI.RichLogs(msg, Color.White);
			UI.RichLogs(Txt_Imei1qc.Text + Valid_Imei1qc.Text, Color.DeepSkyBlue, nextline: true, bold: true);
			UI.RichLogs(msg2, Color.White);
			UI.RichLogs(Txt_Imei2qc.Text + Valid_Imei2qc.Text, Color.DeepSkyBlue, nextline: true, bold: true);
			UI.RichLogs(msg3, Color.White);
		}
		string[] array = IMEIreverse(Txt_Imei1qc.Text + Valid_Imei1qc.Text);
		string[] array2 = IMEIreverse(Txt_Imei2qc.Text + Valid_Imei2qc.Text);
		BinaryReader binaryReader = new BinaryReader(File.Open(Qcn, FileMode.Open), Encoding.ASCII);
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(save, FileMode.Create), Encoding.ASCII);
		long num2 = int.Parse(imei2);
		long num3 = int.Parse(imei1);
		long length = binaryReader.BaseStream.Length;
		try
		{
			do
			{
				byte value = binaryReader.ReadByte();
				binaryWriter.Write(value);
				if (binaryReader.BaseStream.Position == num2)
				{
					int location = 0;
					while (location < array.Length)
					{
						binaryWriter.Write(byte.Parse(array[location], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array.Length;
				}
				else if (binaryReader.BaseStream.Position == num3)
				{
					int location2 = 0;
					while (location2 < array2.Length)
					{
						binaryWriter.Write(byte.Parse(array2[location2], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location2);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array2.Length;
				}
			}
			while (binaryReader.BaseStream.Position != length);
		}
		catch (EndOfStreamException ex)
		{
			if (write)
			{
				UI.RichLogs(msg4, Color.Red, (byte)num != 0, (byte)num != 0);
				UI.RichLogs(ex.Message, Color.Red, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(ex.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
			}
		}
		catch (ArgumentException ex2)
		{
			if (!write)
			{
				XtraMessageBox.Show(ex2.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
				return;
			}
			UI.RichLogs(msg4, Color.Red, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(ex2.Message, Color.Red, (byte)num != 0, (byte)num != 0);
		}
		finally
		{
			binaryReader.Close();
			binaryReader.Dispose();
			binaryWriter.Close();
			binaryWriter.Dispose();
			if (write)
			{
				UI.RichLogs(msg5, Color.GreenYellow, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(text, caption2, (MessageBoxButtons)buttons, (MessageBoxIcon)icon2);
			}
		}
	}

	public string[] IMEIreverse(string text)
	{
		string text2 = "A";
		char[] array = text.ToCharArray();
		Array.Reverse(array);
		string text3 = new string(array) + text2;
		string[] array2 = new string[8];
		int length = text3.Length;
		int location = 0;
		if (length == 0)
		{
			return array2;
		}
		int num = length - 2;
		array2[location] = text3.Substring(num, 2);
		length = num;
		Interlocked.Increment(ref location);
		bool flag = length != 0;
		int num2 = 1804497620;
		while (true)
		{
			_ = num2 ^ -1716015896;
			if (!flag)
			{
				break;
			}
			num = length - 2;
			array2[location] = text3.Substring(num, 2);
			length = num;
			Interlocked.Increment(ref location);
			flag = length != 0;
			num2 = 1804497620;
		}
		return array2;
	}

	private void QCNread(string SourceQCN)
	{
		int buttons = 0;
		string separator = "";
		string value = "01002602000008";
		int icon = 16;
		double field_ = Class38.field_202;
		string caption = "Error !";
		FileStream fileStream = new FileStream(SourceQCN, FileMode.Open, FileAccess.Read);
		try
		{
			byte[] array = File.ReadAllBytes(SourceQCN);
			fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
			string text = BitConverter.ToString(array);
			string[] value2 = text.Split('-').ToArray();
			string text2 = string.Join(separator, value2);
			cmdex.Text = text2;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			for (int i = 0; i <= 1; i++)
			{
				num3 = cmdex.Text.IndexOf(value, num);
				arrayList.Add(cmdex.Text.Substring(num3 + 14, 16));
				num = num3 + 30;
				num2 = num - 16;
				arrayList2.Add((double)num2 / field_);
			}
			extra1 = arrayList[0].ToString();
			extra2 = arrayList[1].ToString();
			imei1 = arrayList2[0].ToString();
			imei2 = arrayList2[1].ToString();
			Console.WriteLine(extra1);
			Console.WriteLine(extra2);
			Console.WriteLine(imei1);
			Console.WriteLine(imei2);
		}
		catch (EndOfStreamException ex)
		{
			XtraMessageBox.Show(ex.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
		}
		catch (ArgumentException ex2)
		{
			XtraMessageBox.Show(ex2.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
		}
		finally
		{
			fileStream.Close();
			fileStream.Dispose();
		}
	}

	private void SaveQCN_Click(object sender, EventArgs e)
	{
		string filter = "QCN Files|*.qcn;*.xqcn|All Files|*.*";
		string msg = "Saving QCN Files...";
		string msg2 = "OK";
		string text = "/";
		string text2 = "_";
		string text3 = ".qcn";
		string msg3 = "Saving : ";
		string text4 = "file://";
		string text5 = "Please Select Options";
		string caption = "Error !";
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = filter
		};
		if (openFileDialog.ShowDialog().Equals(DialogResult.OK))
		{
			State.FilePath = openFileDialog.FileName;
		}
		UI.RichLogs(msg, Color.White);
		DirectoryInfo directoryInfo = new DirectoryInfo(State.FilePath);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		if (CK_Dualimei.Checked)
		{
			UI.RichLogs(msg2, Color.GreenYellow, nextline: true, bold: true);
			string save = State.FilePath + text + Txt_Imei1qc.Text + Valid_Imei1qc.Text + text2 + Txt_Imei2qc.Text + Valid_Imei2qc.Text + text3;
			DualSimReg(save);
			UI.RichLogs(msg3, Color.White);
			UI.RichLogs(text4 + State.FilePath, Color.White, nextline: false, bold: true);
		}
		else if (!Ck_SingleImei.Checked)
		{
			if (Ck_sim2Only.Checked)
			{
				UI.RichLogs(msg2, Color.GreenYellow, nextline: true, bold: true);
				string save2 = State.FilePath + text + Txt_Imei2qc.Text + Valid_Imei2qc.Text + text3;
				Sim2Only(save2);
				UI.RichLogs(msg3, Color.White);
				UI.RichLogs(text4 + State.FilePath, Color.White, nextline: false, bold: true);
			}
			else
			{
				XtraMessageBox.Show(text5, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		else
		{
			UI.RichLogs(msg2, Color.GreenYellow, nextline: true, bold: true);
			string save3 = State.FilePath + text + Txt_Imei1qc.Text + Valid_Imei1qc.Text + text3;
			SingleSimReg(save3);
			UI.RichLogs(msg3, Color.White);
			UI.RichLogs(text4 + State.FilePath, Color.White, nextline: false, bold: true);
		}
	}

	private void Txt_Imei1qc_EditValueChanged(object sender, EventArgs e)
	{
		double field_ = Class38.field_409;
		double field_2 = Class38.field_2096;
		string text = "";
		string text2 = "0";
		try
		{
			Txt_Imei1qc.Properties.MaxLength = 14;
			string text3 = Txt_Imei1qc.Text.ToUpper();
			int num = 0;
			int length = text3.Length;
			for (int i = length; i >= 1; i += -2)
			{
				int num2 = Strings.Asc(Strings.Mid(text3, i, 1)) - 48;
				num = (int)Math.Round(Math.Round((double)(num + 2 * num2) - Conversion.Int((double)num2 / field_) * field_2));
				if (i > 1)
				{
					num2 = Strings.Asc(Strings.Mid(text3, i - 1, 1)) - 48;
					num += num2;
				}
			}
			num += 10;
			Valid_Imei1qc.Text = ((10 - num % 10) % 10).ToString();
			Ck_Imei1qc.Checked = true;
			if ((object)Txt_Imei1qc.Text == text)
			{
				Ck_Imei1qc.Checked = false;
				Valid_Imei1qc.Text = text2;
			}
		}
		catch
		{
		}
	}

	private void Txt_Imei2qc_EditValueChanged(object sender, EventArgs e)
	{
		double field_ = Class38.field_409;
		double field_2 = Class38.field_2096;
		string text = "";
		string text2 = "0";
		try
		{
			Txt_Imei2qc.Properties.MaxLength = 14;
			string text3 = Txt_Imei2qc.Text.ToUpper();
			int num = 0;
			int length = text3.Length;
			for (int i = length; i >= 1; i += -2)
			{
				int num2 = Strings.Asc(Strings.Mid(text3, i, 1)) - 48;
				num = (int)Math.Round(Math.Round((double)(num + 2 * num2) - Conversion.Int((double)num2 / field_) * field_2));
				if (i > 1)
				{
					num2 = Strings.Asc(Strings.Mid(text3, i - 1, 1)) - 48;
					num += num2;
				}
			}
			num += 10;
			Valid_Imei2qc.Text = ((10 - num % 10) % 10).ToString();
			Ck_Imei2qc.Checked = true;
			if ((object)Txt_Imei2qc.Text == text)
			{
				Ck_Imei2qc.Checked = false;
				Valid_Imei2qc.Text = text2;
			}
		}
		catch
		{
		}
	}

	private void CK_Dualimei_CheckedChanged(object sender, EventArgs e)
	{
		if (!CK_Dualimei.Checked)
		{
			CK_Dualimei.Checked = false;
			return;
		}
		Ck_SingleImei.Checked = false;
		Ck_sim2Only.Checked = false;
	}

	private void Ck_SingleImei_CheckedChanged(object sender, EventArgs e)
	{
		if (Ck_SingleImei.Checked)
		{
			CK_Dualimei.Checked = false;
			Ck_sim2Only.Checked = false;
		}
		else
		{
			Ck_SingleImei.Checked = false;
		}
	}

	private void Ck_sim2Only_CheckedChanged(object sender, EventArgs e)
	{
		if (!Ck_sim2Only.Checked)
		{
			Ck_SingleImei.Checked = false;
			return;
		}
		CK_Dualimei.Checked = false;
		Ck_SingleImei.Checked = false;
	}

	private void WriteQCN_Click(object sender, EventArgs e)
	{
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		object obj = null;
		if (!disposing || components ==null)
		{
			base.Dispose(disposing);
			return;
		}
		components.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		string name = "Ck_sim2Only";
		string text = "SIM 2 Only";
		string name2 = "Ck_SingleImei";
		string text2 = "Single SIM";
		string familyName = "Tahoma";
		float field_ = Class38.field_342;
		string name3 = "LabelControl1";
		string text3 = "QCN Files";
		string name4 = "OpenQCN.Image";
		string name5 = "OpenQCN";
		string name6 = "SaveQCN.ImageOptions.Image";
		string name7 = "SaveQCN";
		string text4 = "Save QCN";
		string name8 = "WriteQCN.ImageOptions.Image";
		string name9 = "WriteQCN";
		string text5 = "Write QCN";
		string name10 = "CK_Dualimei";
		string text6 = "Dual SIM";
		string name11 = "Ck_Imei2qc";
		string text7 = "IMEI 2";
		string name12 = "Ck_Imei1qc";
		string text8 = "IMEI 1";
		string editValue = "0";
		string name13 = "Valid_Imei2qc";
		string familyName2 = "Consolas";
		float field_2 = Class38.field_375;
		string name14 = "Valid_Imei1qc";
		string editValue2 = "00000000000000";
		string name15 = "Txt_Imei2qc";
		string name16 = "Txt_Imei1qc";
		string name17 = "Txt_QCN";
		float field_3 = Class38.field_392;
		float field_4 = Class38.field_393;
		string name18 = "QCNeditoIMEI";
		string text9 = "QCNEditor";
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AMFTool.QCNeditoIMEI));
		this.Ck_sim2Only = new System.Windows.Forms.CheckBox();
		this.Ck_SingleImei = new System.Windows.Forms.CheckBox();
		this.LabelControl1 = new DevExpress.XtraEditors.LabelControl();
		this.OpenQCN = new System.Windows.Forms.Button();
		this.SaveQCN = new DevExpress.XtraEditors.SimpleButton();
		this.WriteQCN = new DevExpress.XtraEditors.SimpleButton();
		this.CK_Dualimei = new System.Windows.Forms.CheckBox();
		this.Ck_Imei2qc = new System.Windows.Forms.CheckBox();
		this.Ck_Imei1qc = new System.Windows.Forms.CheckBox();
		this.Valid_Imei2qc = new DevExpress.XtraEditors.TextEdit();
		this.Valid_Imei1qc = new DevExpress.XtraEditors.TextEdit();
		this.Txt_Imei2qc = new DevExpress.XtraEditors.TextEdit();
		this.Txt_Imei1qc = new DevExpress.XtraEditors.TextEdit();
		this.Txt_QCN = new DevExpress.XtraEditors.TextEdit();
		((System.ComponentModel.ISupportInitialize)this.Valid_Imei2qc.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Valid_Imei1qc.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Txt_Imei2qc.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Txt_Imei1qc.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Txt_QCN.Properties).BeginInit();
		base.SuspendLayout();
		this.Ck_sim2Only.AutoSize = true;
		this.Ck_sim2Only.Location = new System.Drawing.Point(237, 69);
		this.Ck_sim2Only.Name = name;
		this.Ck_sim2Only.Size = new System.Drawing.Size(78, 17);
		this.Ck_sim2Only.TabIndex = 247;
		this.Ck_sim2Only.Text = text;
		this.Ck_sim2Only.UseVisualStyleBackColor = true;
		this.Ck_sim2Only.CheckedChanged += new System.EventHandler(Ck_sim2Only_CheckedChanged);
		this.Ck_SingleImei.AutoSize = true;
		this.Ck_SingleImei.Location = new System.Drawing.Point(156, 69);
		this.Ck_SingleImei.Name = name2;
		this.Ck_SingleImei.Size = new System.Drawing.Size(75, 17);
		this.Ck_SingleImei.TabIndex = 246;
		this.Ck_SingleImei.Text = text2;
		this.Ck_SingleImei.UseVisualStyleBackColor = true;
		this.Ck_SingleImei.CheckedChanged += new System.EventHandler(Ck_SingleImei_CheckedChanged);
		this.LabelControl1.Appearance.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LabelControl1.Appearance.Options.UseFont = true;
		this.LabelControl1.Location = new System.Drawing.Point(23, 104);
		this.LabelControl1.Name = name3;
		this.LabelControl1.Size = new System.Drawing.Size(50, 13);
		this.LabelControl1.TabIndex = 245;
		this.LabelControl1.Text = text3;
		this.OpenQCN.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.OpenQCN.BackColor = System.Drawing.Color.Transparent;
		this.OpenQCN.FlatAppearance.BorderSize = 0;
		this.OpenQCN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.OpenQCN.ForeColor = System.Drawing.Color.FromArgb(66, 69, 84);
		this.OpenQCN.Image = (System.Drawing.Image)resources.GetObject(name4);
		this.OpenQCN.Location = new System.Drawing.Point(301, 102);
		this.OpenQCN.Name = name5;
		this.OpenQCN.Size = new System.Drawing.Size(19, 17);
		this.OpenQCN.TabIndex = 244;
		this.OpenQCN.UseVisualStyleBackColor = false;
		this.OpenQCN.Click += new System.EventHandler(OpenQCN_Click);
		this.SaveQCN.AllowFocus = false;
		this.SaveQCN.AppearanceHovered.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SaveQCN.AppearanceHovered.ForeColor = System.Drawing.Color.DeepSkyBlue;
		this.SaveQCN.AppearanceHovered.Options.UseFont = true;
		this.SaveQCN.AppearanceHovered.Options.UseForeColor = true;
		this.SaveQCN.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name6);
		this.SaveQCN.Location = new System.Drawing.Point(82, 127);
		this.SaveQCN.Name = name7;
		this.SaveQCN.Size = new System.Drawing.Size(116, 24);
		this.SaveQCN.TabIndex = 243;
		this.SaveQCN.Text = text4;
		this.SaveQCN.Click += new System.EventHandler(SaveQCN_Click);
		this.WriteQCN.AllowFocus = false;
		this.WriteQCN.AppearanceHovered.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.WriteQCN.AppearanceHovered.ForeColor = System.Drawing.Color.DeepSkyBlue;
		this.WriteQCN.AppearanceHovered.Options.UseFont = true;
		this.WriteQCN.AppearanceHovered.Options.UseForeColor = true;
		this.WriteQCN.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name8);
		this.WriteQCN.Location = new System.Drawing.Point(206, 127);
		this.WriteQCN.Name = name9;
		this.WriteQCN.Size = new System.Drawing.Size(116, 24);
		this.WriteQCN.TabIndex = 242;
		this.WriteQCN.Text = text5;
		this.WriteQCN.Click += new System.EventHandler(WriteQCN_Click);
		this.CK_Dualimei.AutoSize = true;
		this.CK_Dualimei.Checked = true;
		this.CK_Dualimei.CheckState = System.Windows.Forms.CheckState.Checked;
		this.CK_Dualimei.Location = new System.Drawing.Point(82, 69);
		this.CK_Dualimei.Name = name10;
		this.CK_Dualimei.Size = new System.Drawing.Size(68, 17);
		this.CK_Dualimei.TabIndex = 241;
		this.CK_Dualimei.Text = text6;
		this.CK_Dualimei.UseVisualStyleBackColor = true;
		this.CK_Dualimei.CheckedChanged += new System.EventHandler(CK_Dualimei_CheckedChanged);
		this.Ck_Imei2qc.AutoSize = true;
		this.Ck_Imei2qc.Checked = true;
		this.Ck_Imei2qc.CheckState = System.Windows.Forms.CheckState.Checked;
		this.Ck_Imei2qc.Location = new System.Drawing.Point(23, 38);
		this.Ck_Imei2qc.Name = name11;
		this.Ck_Imei2qc.Size = new System.Drawing.Size(57, 17);
		this.Ck_Imei2qc.TabIndex = 240;
		this.Ck_Imei2qc.Text = text7;
		this.Ck_Imei2qc.UseVisualStyleBackColor = true;
		this.Ck_Imei1qc.AutoSize = true;
		this.Ck_Imei1qc.Checked = true;
		this.Ck_Imei1qc.CheckState = System.Windows.Forms.CheckState.Checked;
		this.Ck_Imei1qc.Location = new System.Drawing.Point(23, 15);
		this.Ck_Imei1qc.Name = name12;
		this.Ck_Imei1qc.Size = new System.Drawing.Size(57, 17);
		this.Ck_Imei1qc.TabIndex = 239;
		this.Ck_Imei1qc.Text = text8;
		this.Ck_Imei1qc.UseVisualStyleBackColor = true;
		this.Valid_Imei2qc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.Valid_Imei2qc.EditValue = editValue;
		this.Valid_Imei2qc.Location = new System.Drawing.Point(304, 36);
		this.Valid_Imei2qc.Name = name13;
		this.Valid_Imei2qc.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
		this.Valid_Imei2qc.Properties.Appearance.Font = new System.Drawing.Font(familyName2, field_2, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Valid_Imei2qc.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.Valid_Imei2qc.Properties.Appearance.Options.UseBackColor = true;
		this.Valid_Imei2qc.Properties.Appearance.Options.UseFont = true;
		this.Valid_Imei2qc.Properties.Appearance.Options.UseForeColor = true;
		this.Valid_Imei2qc.Properties.Appearance.Options.UseTextOptions = true;
		this.Valid_Imei2qc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
		this.Valid_Imei2qc.Properties.AutoHeight = false;
		this.Valid_Imei2qc.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
		this.Valid_Imei2qc.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.Valid_Imei2qc.Properties.MaxLength = 1;
		this.Valid_Imei2qc.Size = new System.Drawing.Size(18, 22);
		this.Valid_Imei2qc.TabIndex = 238;
		this.Valid_Imei1qc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.Valid_Imei1qc.EditValue = editValue;
		this.Valid_Imei1qc.Location = new System.Drawing.Point(304, 12);
		this.Valid_Imei1qc.Name = name14;
		this.Valid_Imei1qc.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
		this.Valid_Imei1qc.Properties.Appearance.Font = new System.Drawing.Font(familyName2, field_2, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Valid_Imei1qc.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.Valid_Imei1qc.Properties.Appearance.Options.UseBackColor = true;
		this.Valid_Imei1qc.Properties.Appearance.Options.UseFont = true;
		this.Valid_Imei1qc.Properties.Appearance.Options.UseForeColor = true;
		this.Valid_Imei1qc.Properties.Appearance.Options.UseTextOptions = true;
		this.Valid_Imei1qc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
		this.Valid_Imei1qc.Properties.AutoHeight = false;
		this.Valid_Imei1qc.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
		this.Valid_Imei1qc.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.Valid_Imei1qc.Properties.MaxLength = 1;
		this.Valid_Imei1qc.Size = new System.Drawing.Size(18, 22);
		this.Valid_Imei1qc.TabIndex = 237;
		this.Txt_Imei2qc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.Txt_Imei2qc.EditValue = editValue2;
		this.Txt_Imei2qc.Location = new System.Drawing.Point(82, 36);
		this.Txt_Imei2qc.Name = name15;
		this.Txt_Imei2qc.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
		this.Txt_Imei2qc.Properties.Appearance.Font = new System.Drawing.Font(familyName2, field_2, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Txt_Imei2qc.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.Txt_Imei2qc.Properties.Appearance.Options.UseBackColor = true;
		this.Txt_Imei2qc.Properties.Appearance.Options.UseFont = true;
		this.Txt_Imei2qc.Properties.Appearance.Options.UseForeColor = true;
		this.Txt_Imei2qc.Properties.Appearance.Options.UseTextOptions = true;
		this.Txt_Imei2qc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
		this.Txt_Imei2qc.Properties.AutoHeight = false;
		this.Txt_Imei2qc.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
		this.Txt_Imei2qc.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.Txt_Imei2qc.Properties.MaxLength = 14;
		this.Txt_Imei2qc.Size = new System.Drawing.Size(216, 22);
		this.Txt_Imei2qc.TabIndex = 236;
		this.Txt_Imei2qc.EditValueChanged += new System.EventHandler(Txt_Imei2qc_EditValueChanged);
		this.Txt_Imei1qc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.Txt_Imei1qc.EditValue = editValue2;
		this.Txt_Imei1qc.Location = new System.Drawing.Point(82, 12);
		this.Txt_Imei1qc.Name = name16;
		this.Txt_Imei1qc.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
		this.Txt_Imei1qc.Properties.Appearance.Font = new System.Drawing.Font(familyName2, field_2, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Txt_Imei1qc.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.Txt_Imei1qc.Properties.Appearance.Options.UseBackColor = true;
		this.Txt_Imei1qc.Properties.Appearance.Options.UseFont = true;
		this.Txt_Imei1qc.Properties.Appearance.Options.UseForeColor = true;
		this.Txt_Imei1qc.Properties.Appearance.Options.UseTextOptions = true;
		this.Txt_Imei1qc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
		this.Txt_Imei1qc.Properties.AutoHeight = false;
		this.Txt_Imei1qc.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
		this.Txt_Imei1qc.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.Txt_Imei1qc.Properties.MaxLength = 14;
		this.Txt_Imei1qc.Size = new System.Drawing.Size(216, 22);
		this.Txt_Imei1qc.TabIndex = 235;
		this.Txt_Imei1qc.EditValueChanged += new System.EventHandler(Txt_Imei1qc_EditValueChanged);
		this.Txt_QCN.Location = new System.Drawing.Point(82, 101);
		this.Txt_QCN.Name = name17;
		this.Txt_QCN.Properties.AllowFocused = false;
		this.Txt_QCN.Properties.Appearance.ForeColor = System.Drawing.Color.Fuchsia;
		this.Txt_QCN.Properties.Appearance.Options.UseForeColor = true;
		this.Txt_QCN.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
		this.Txt_QCN.Size = new System.Drawing.Size(240, 20);
		this.Txt_QCN.TabIndex = 248;
		base.AutoScaleDimensions = new System.Drawing.SizeF(field_3, field_4);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(361, 185);
		base.Controls.Add(this.Ck_sim2Only);
		base.Controls.Add(this.Ck_SingleImei);
		base.Controls.Add(this.LabelControl1);
		base.Controls.Add(this.OpenQCN);
		base.Controls.Add(this.SaveQCN);
		base.Controls.Add(this.WriteQCN);
		base.Controls.Add(this.CK_Dualimei);
		base.Controls.Add(this.Ck_Imei2qc);
		base.Controls.Add(this.Ck_Imei1qc);
		base.Controls.Add(this.Valid_Imei2qc);
		base.Controls.Add(this.Valid_Imei1qc);
		base.Controls.Add(this.Txt_Imei2qc);
		base.Controls.Add(this.Txt_Imei1qc);
		base.Controls.Add(this.Txt_QCN);
		base.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		((DevExpress.Utils.ImageOptions)this.method_02()).Image = AMFTool.Properties.Resources.logoamftool;
		base.MaximizeBox = false;
		base.Name = name18;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = text9;
		((System.ComponentModel.ISupportInitialize)this.Valid_Imei2qc.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Valid_Imei1qc.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Txt_Imei2qc.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Txt_Imei1qc.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Txt_QCN.Properties).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	public object method_02()
	{
		return base.IconOptions;
	}
}
