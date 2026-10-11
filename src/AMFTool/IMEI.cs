using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AMFTool.Properties;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace AMFTool;

public class IMEI : XtraForm
{
	private IContainer components;

	private SimpleButton Write;

	private TextEdit Imei2;

	private TextEdit Imei1;

	private LabelControl labelControl2;

	private LabelControl labelControl1;

	private LabelControl LIMEI1;

	private ComboBoxEdit CImeiType;

	public CheckEdit CIMEI2;

	private TextEdit txtIMEI2;

	public CheckEdit CIMEI1;

	private TextEdit txtIMEI1;

	public IMEI(bool edl = false, bool meta = false, bool universal = false)
	{
		object obj = null;
		string text = "Qualcomm";
		string text2 = "Transsion META";
		string text3 = "Universal";
		string text4 = "Oppo-Realme";
		string text5 = "Xiaomi";
		string text6 = "Infinix-Itel-Tecno";
		string text7 = "Vivo By RPMB";
		string text8 = "Generic";
		components = (IContainer)obj;

		InitializeComponent();
		ResetVariable();
		if (edl)
		{
			string[] array = new string[1] { text };
			CImeiType.Properties.Items.Clear();
			ComboBoxItemCollection items = CImeiType.Properties.Items;
			object[] items2 = array;
			items.AddRange(items2);
			CImeiType.SelectedIndex = 0;
		}
		else if (!meta)
		{
			if (universal)
			{
				string[] array2 = new string[1] { text3 };
				CImeiType.Properties.Items.Clear();
				ComboBoxItemCollection items3 = CImeiType.Properties.Items;
				object[] items2 = array2;
				items3.AddRange(items2);
				CImeiType.SelectedIndex = 0;
			}
			else
			{
				string[] array3 = new string[5] { text4, text5, text6, text7, text8 };
				CImeiType.Properties.Items.Clear();
				ComboBoxItemCollection items4 = CImeiType.Properties.Items;
				object[] items2 = array3;
				items4.AddRange(items2);
				CImeiType.SelectedIndex = 0;
			}
		}
		else
		{
			string[] array4 = new string[1] { text2 };
			CImeiType.Properties.Items.Clear();
			ComboBoxItemCollection items5 = CImeiType.Properties.Items;
			object[] items2 = array4;
			items5.AddRange(items2);
			CImeiType.SelectedIndex = 0;
		}
	}

	private void Write_Click(object sender, EventArgs e)
	{
		string text = "Invalid Imei 1";
		string caption = "Warning";
		string text2 = "Invalid Imei 2";
		DeviceState.IMEI1 = txtIMEI1.Text + Imei1.Text;
		DeviceState.IMEI2 = txtIMEI2.Text + Imei2.Text;
		if (!DeviceState.CheckedIMEI1)
		{
			if (DeviceState.CheckedIMEI2)
			{
				if (DeviceState.IMEI2.Length >= 15)
				{
					base.DialogResult = DialogResult.Yes;
					Close();
				}
				else
				{
					XtraMessageBox.Show(text2, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else
			{
				base.DialogResult = DialogResult.Yes;
				Close();
			}
		}
		else if (DeviceState.IMEI1.Length < 15)
		{
			XtraMessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else if (DeviceState.CheckedIMEI2)
		{
			if (DeviceState.IMEI2.Length >= 15)
			{
				base.DialogResult = DialogResult.Yes;
				Close();
			}
			else
			{
				XtraMessageBox.Show(text2, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		else
		{
			base.DialogResult = DialogResult.Yes;
			Close();
		}
	}

	private void CIMEI1_CheckedChanged(object sender, EventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		txtIMEI1.Enabled = CIMEI1.Checked;
		if (CIMEI1.Checked)
		{
			DeviceState.CheckedIMEI1 = CIMEI1.Checked;
			return;
		}
		txtIMEI1.Clear();
		DeviceState.CheckedIMEI1 = CIMEI1.Checked;
	}

	private void CIMEI2_CheckedChanged(object sender, EventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		txtIMEI2.Enabled = CIMEI2.Checked;
		if (CIMEI2.Checked)
		{
			DeviceState.CheckedIMEI2 = CIMEI2.Checked;
			return;
		}
		txtIMEI2.Clear();
		DeviceState.CheckedIMEI2 = CIMEI2.Checked;
	}

	private void CImeiType_SelectedIndexChanged(object sender, EventArgs e)
	{
		string text = "IMEI STATE = ";
		switch (CImeiType.SelectedIndex)
		{
		default:
			State.ImeiType = (IMEIType)4;
			break;
		case 0:
			State.ImeiType = (IMEIType)0;
			break;
		case 1:
			State.ImeiType = IMEIType.Xiaomi;
			break;
		case 2:
			State.ImeiType = (IMEIType)2;
			break;
		case 3:
			State.ImeiType = IMEIType.Vivo;
			break;
		case 4:
			State.ImeiType = (IMEIType)4;
			break;
		}
		Console.WriteLine(text + State.ImeiType);
	}

	private void ResetVariable()
	{
		DeviceState.CheckedIMEI1 = false;
		DeviceState.CheckedIMEI2 = false;
		DeviceState.IMEI1 = string.Empty;
		DeviceState.IMEI2 = string.Empty;
	}

	private string CalcChecksum(string imei)
	{
		double field_ = Class38.field_409;
		imei = imei.ToUpper();
		int num = 0;
		int length = imei.Length;
		if (length < 1)
		{
			num = 10;
			return 0.ToString();
		}
		int num2 = int.Parse(imei.Substring(length - 1, 1), NumberStyles.HexNumber);
		int num3 = num2 * 2 - (int)Math.Floor((double)num2 / field_) * 9;
		num = 0 + num3;
		int num5;
		int num4;
		if (length > 1)
		{
			num4 = int.Parse(imei.Substring(length - 2, 1), NumberStyles.HexNumber);
			num += num4;
			length -= 2;
			num5 = 664529970;
			goto IL_011e;
		}
		length -= 2;
		int num6 = 664529970;
		while (true)
		{
			_ = num6 ^ 0xAED2AD9;
			if (length < 1)
			{
				num += 10;
				return ((10 - num % 10) % 10).ToString();
			}
			num2 = int.Parse(imei.Substring(length - 1, 1), NumberStyles.HexNumber);
			num3 = num2 * 2 - (int)Math.Floor((double)num2 / field_) * 9;
			num += num3;
			if (length > 1)
			{
				break;
			}
			length -= 2;
			num6 = 664529970;
		}
		num4 = int.Parse(imei.Substring(length - 2, 1), NumberStyles.HexNumber);
		num += num4;
		int num7 = 730810138;
		goto IL_0191;
		IL_0191:
		_ = num7 ^ 0xAED2AD9;
		goto IL_019b;
		IL_019b:
		length -= 2;
		num5 = 664529970;
		goto IL_011e;
		IL_011e:
		_ = num5 ^ 0xAED2AD9;
		if (length >= 1)
		{
			num2 = int.Parse(imei.Substring(length - 1, 1), NumberStyles.HexNumber);
			num3 = num2 * 2 - (int)Math.Floor((double)num2 / field_) * 9;
			num += num3;
			if (length > 1)
			{
				num4 = int.Parse(imei.Substring(length - 2, 1), NumberStyles.HexNumber);
				num += num4;
				num7 = 730810138;
				goto IL_0191;
			}
			goto IL_019b;
		}
		num += 10;
		return ((10 - num % 10) % 10).ToString();
	}

	private void txtIMEI_EditValueChanged(object sender, EventArgs e)
	{
		object obj = null;
		string text = "txtIMEI1";
		string text2 = "txtIMEI2";
		string text3 = "0";
		string text4 = "00000000000000";
		try
		{
			TextEdit textEdit = sender as TextEdit;
			if (textEdit == obj)
			{
				return;
			}
			string text5 = textEdit.Text;
			TextEdit textEdit2;
			if (!(textEdit.Name == text))
			{
				if (!(textEdit.Name == text2))
				{
					return;
				}
				textEdit2 = Imei2;
			}
			else
			{
				textEdit2 = Imei1;
			}
			if (string.IsNullOrEmpty(text5) || text5 == text3)
			{
				textEdit2.Text = text3;
			}
			else
			{
				textEdit2.Text = CalcChecksum(text5);
			}
		}
		catch
		{
			TextEdit textEdit3 = sender as TextEdit;
			if (textEdit3 !=null)
			{
				textEdit3.Text = text4;
			}
			if (textEdit3 == txtIMEI1)
			{
				Imei1.Text = text3;
			}
			if (textEdit3 == txtIMEI2)
			{
				Imei2.Text = text3;
			}
		}
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
		string text = "Write";
		string name = "labelControl2";
		string text2 = "TYPE";
		string name2 = "labelControl1";
		string text3 = "IMEI 2";
		string name3 = "LIMEI1";
		string text4 = "IMEI 1";
		string name4 = "Imei2";
		string nullText = "0";
		string name5 = "Imei1";
		string name6 = "CImeiType";
		string name7 = "CIMEI2";
		string caption = "";
		string name8 = "txtIMEI2";
		string name9 = "CIMEI1";
		string name10 = "txtIMEI1";
		float field_ = Class38.field_392;
		float field_2 = Class38.field_393;
		string text5 = "IMEI";
		this.Write = new DevExpress.XtraEditors.SimpleButton();
		this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
		this.LIMEI1 = new DevExpress.XtraEditors.LabelControl();
		this.Imei2 = new DevExpress.XtraEditors.TextEdit();
		this.Imei1 = new DevExpress.XtraEditors.TextEdit();
		this.CImeiType = new DevExpress.XtraEditors.ComboBoxEdit();
		this.CIMEI2 = new DevExpress.XtraEditors.CheckEdit();
		this.txtIMEI2 = new DevExpress.XtraEditors.TextEdit();
		this.CIMEI1 = new DevExpress.XtraEditors.CheckEdit();
		this.txtIMEI1 = new DevExpress.XtraEditors.TextEdit();
		((System.ComponentModel.ISupportInitialize)this.Imei2.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Imei1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CImeiType.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CIMEI2.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtIMEI2.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CIMEI1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtIMEI1.Properties).BeginInit();
		base.SuspendLayout();
		this.Write.Location = new System.Drawing.Point(86, 129);
		this.Write.Name = text;
		this.Write.Size = new System.Drawing.Size(262, 23);
		this.Write.TabIndex = 292;
		this.Write.Text = text;
		this.Write.Click += new System.EventHandler(Write_Click);
		this.labelControl2.Location = new System.Drawing.Point(89, 96);
		this.labelControl2.Name = name;
		this.labelControl2.Size = new System.Drawing.Size(24, 13);
		this.labelControl2.TabIndex = 289;
		this.labelControl2.Text = text2;
		this.labelControl1.Location = new System.Drawing.Point(86, 64);
		this.labelControl1.Name = name2;
		this.labelControl1.Size = new System.Drawing.Size(31, 13);
		this.labelControl1.TabIndex = 288;
		this.labelControl1.Text = text3;
		this.LIMEI1.Location = new System.Drawing.Point(86, 29);
		this.LIMEI1.Name = name3;
		this.LIMEI1.Size = new System.Drawing.Size(31, 13);
		this.LIMEI1.TabIndex = 287;
		this.LIMEI1.Text = text4;
		this.Imei2.Location = new System.Drawing.Point(298, 60);
		this.Imei2.Name = name4;
		this.Imei2.Properties.NullText = nullText;
		this.Imei2.Size = new System.Drawing.Size(21, 20);
		this.Imei2.TabIndex = 291;
		this.Imei1.Location = new System.Drawing.Point(298, 25);
		this.Imei1.Name = name5;
		this.Imei1.Properties.NullText = nullText;
		this.Imei1.Size = new System.Drawing.Size(21, 20);
		this.Imei1.TabIndex = 290;
		this.CImeiType.Location = new System.Drawing.Point(132, 94);
		this.CImeiType.Name = name6;
		this.CImeiType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.CImeiType.Size = new System.Drawing.Size(187, 20);
		this.CImeiType.TabIndex = 286;
		this.CImeiType.SelectedIndexChanged += new System.EventHandler(CImeiType_SelectedIndexChanged);
		this.CIMEI2.Location = new System.Drawing.Point(327, 60);
		this.CIMEI2.Name = name7;
		this.CIMEI2.Properties.AllowFocused = false;
		this.CIMEI2.Properties.Caption = caption;
		this.CIMEI2.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.CIMEI2.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.CIMEI2.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.CIMEI2.Size = new System.Drawing.Size(21, 22);
		this.CIMEI2.TabIndex = 285;
		this.CIMEI2.CheckedChanged += new System.EventHandler(CIMEI2_CheckedChanged);
		this.txtIMEI2.Enabled = false;
		this.txtIMEI2.Location = new System.Drawing.Point(132, 60);
		this.txtIMEI2.Name = name8;
		this.txtIMEI2.Properties.MaxLength = 14;
		this.txtIMEI2.Size = new System.Drawing.Size(160, 20);
		this.txtIMEI2.TabIndex = 284;
		this.txtIMEI2.EditValueChanged += new System.EventHandler(txtIMEI_EditValueChanged);
		this.CIMEI1.Location = new System.Drawing.Point(327, 25);
		this.CIMEI1.Name = name9;
		this.CIMEI1.Properties.AllowFocused = false;
		this.CIMEI1.Properties.Caption = caption;
		this.CIMEI1.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.CIMEI1.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.CIMEI1.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.CIMEI1.Size = new System.Drawing.Size(21, 22);
		this.CIMEI1.TabIndex = 283;
		this.CIMEI1.CheckedChanged += new System.EventHandler(CIMEI1_CheckedChanged);
		this.txtIMEI1.Enabled = false;
		this.txtIMEI1.Location = new System.Drawing.Point(132, 26);
		this.txtIMEI1.Name = name10;
		this.txtIMEI1.Properties.MaxLength = 14;
		this.txtIMEI1.Size = new System.Drawing.Size(160, 20);
		this.txtIMEI1.TabIndex = 282;
		this.txtIMEI1.EditValueChanged += new System.EventHandler(txtIMEI_EditValueChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(field_, field_2);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(421, 188);
		base.Controls.Add(this.Write);
		base.Controls.Add(this.Imei2);
		base.Controls.Add(this.Imei1);
		base.Controls.Add(this.labelControl2);
		base.Controls.Add(this.labelControl1);
		base.Controls.Add(this.LIMEI1);
		base.Controls.Add(this.CImeiType);
		base.Controls.Add(this.CIMEI2);
		base.Controls.Add(this.txtIMEI2);
		base.Controls.Add(this.CIMEI1);
		base.Controls.Add(this.txtIMEI1);
		((DevExpress.Utils.ImageOptions)this.method_02()).Image = AMFTool.Properties.Resources.logoamftool;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = text5;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = text5;
		((System.ComponentModel.ISupportInitialize)this.Imei2.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Imei1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CImeiType.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CIMEI2.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtIMEI2.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CIMEI1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtIMEI1.Properties).EndInit();
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
