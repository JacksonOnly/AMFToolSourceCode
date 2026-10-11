using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AMFTool.Properties;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace AMFTool;

public class SerialNumber : XtraForm
{
	private IContainer components = null;

	private LabelControl LSerial;

	private SimpleButton Write;

	private TextEdit txtSerial;

	public SerialNumber()
	{
		InitializeComponent();
	}

	private void Write_Click(object sender, EventArgs e)
	{
		DeviceState.Serialnumber = txtSerial.Text;
		base.DialogResult = DialogResult.Yes;
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
		string name = "LSerial";
		string text = "Enter New Serial";
		string text2 = "Write";
		string editValue = "";
		string name2 = "txtSerial";
		string nullText = "New Serial Number";
		float field_ = Class38.field_392;
		float field_2 = Class38.field_393;
		string text3 = "SerialNumber";
		this.LSerial = new DevExpress.XtraEditors.LabelControl();
		this.Write = new DevExpress.XtraEditors.SimpleButton();
		this.txtSerial = new DevExpress.XtraEditors.TextEdit();
		((System.ComponentModel.ISupportInitialize)this.txtSerial.Properties).BeginInit();
		base.SuspendLayout();
		this.LSerial.Location = new System.Drawing.Point(5, 19);
		this.LSerial.Name = name;
		this.LSerial.Size = new System.Drawing.Size(79, 13);
		this.LSerial.TabIndex = 12;
		this.LSerial.Text = text;
		this.Write.Location = new System.Drawing.Point(5, 46);
		this.Write.Name = text2;
		this.Write.Size = new System.Drawing.Size(272, 23);
		this.Write.TabIndex = 11;
		this.Write.Text = text2;
		this.Write.Click += new System.EventHandler(Write_Click);
		this.txtSerial.EditValue = editValue;
		this.txtSerial.Location = new System.Drawing.Point(96, 16);
		this.txtSerial.Name = name2;
		this.txtSerial.Properties.MaxLength = 11;
		this.txtSerial.Properties.NullText = nullText;
		this.txtSerial.Size = new System.Drawing.Size(166, 20);
		this.txtSerial.TabIndex = 10;
		base.AutoScaleDimensions = new System.Drawing.SizeF(field_, field_2);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(283, 80);
		base.Controls.Add(this.LSerial);
		base.Controls.Add(this.Write);
		base.Controls.Add(this.txtSerial);
		((DevExpress.Utils.ImageOptions)this.method_02()).Image = AMFTool.Properties.Resources.logoamftool;
		base.Name = text3;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = text3;
		((System.ComponentModel.ISupportInitialize)this.txtSerial.Properties).EndInit();
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
