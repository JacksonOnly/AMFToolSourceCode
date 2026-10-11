using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AMFTool.Properties;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace AMFTool.Library.Mediatek;

public class ReadDump : XtraForm
{
	private IContainer components = null;

	internal SimpleButton SimpleButton2;

	internal SimpleButton SimpleButton1;

	internal GroupControl GroupControl1;

	private Panel panel1;

	private RadioButton FullDump;

	private RadioButton checkEdit7;

	private RadioButton checkEdit6;

	private RadioButton checkEdit5;

	private RadioButton checkEdit4;

	private RadioButton checkEdit2;

	private RadioButton checkEdit3;

	private RadioButton checkEdit9;

	private RadioButton checkEdit10;

	private RadioButton checkEdit11;

	private RadioButton checkEdit12;

	private RadioButton FullStorege;

	public ReadDump()
	{
		InitializeComponent();
	}

	private void SimpleButton1_Click(object sender, EventArgs e)
	{
		string readdumpSize = "Auto";
		string readdumpSize2 = "Exclude Userdata";
		string readdumpSize3 = "8MB";
		string readdumpSize4 = "16MB";
		string readdumpSize5 = "32MB";
		string readdumpSize6 = "64MB";
		string readdumpSize7 = "128MB";
		string readdumpSize8 = "256MB";
		string readdumpSize9 = "512MB";
		string readdumpSize10 = "1GB";
		string readdumpSize11 = "4GB";
		if (FullDump.Checked)
		{
			UI.ReaddumpSize = readdumpSize;
		}
		else if (!FullStorege.Checked)
		{
			if (checkEdit7.Checked)
			{
				UI.ReaddumpSize = readdumpSize3;
			}
			else if (checkEdit6.Checked)
			{
				UI.ReaddumpSize = readdumpSize4;
			}
			else if (!checkEdit5.Checked)
			{
				if (!checkEdit4.Checked)
				{
					if (checkEdit2.Checked)
					{
						UI.ReaddumpSize = readdumpSize4;
					}
					else if (checkEdit3.Checked)
					{
						UI.ReaddumpSize = readdumpSize7;
					}
					else if (checkEdit9.Checked)
					{
						UI.ReaddumpSize = readdumpSize8;
					}
					else if (!checkEdit10.Checked)
					{
						if (checkEdit11.Checked)
						{
							UI.ReaddumpSize = readdumpSize10;
						}
						else if (checkEdit12.Checked)
						{
							UI.ReaddumpSize = readdumpSize11;
						}
					}
					else
					{
						UI.ReaddumpSize = readdumpSize9;
					}
				}
				else
				{
					UI.ReaddumpSize = readdumpSize6;
				}
			}
			else
			{
				UI.ReaddumpSize = readdumpSize5;
			}
		}
		else
		{
			UI.ReaddumpSize = readdumpSize2;
		}
		base.DialogResult = DialogResult.Yes;
		Close();
	}

	private void SimpleButton2_Click(object sender, EventArgs e)
	{
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		object obj = null;
		if (disposing && components !=null)
		{
			components.Dispose();
			base.Dispose(disposing);
		}
		else
		{
			base.Dispose(disposing);
		}
	}

	private void InitializeComponent()
	{
		string name = "SimpleButton2";
		string text = "Censel";
		string name2 = "SimpleButton1";
		string text2 = "Read";
		string skinName = "WXI";
		string name3 = "GroupControl1";
		string text3 = "Size Selection";
		string name4 = "panel1";
		string familyName = "Tahoma";
		float field_ = Class38.field_342;
		string name5 = "FullDump";
		string text4 = "Full Dump (Storege)";
		string name6 = "FullStorege";
		string text5 = "Full Storege (except USEDATA)";
		string name7 = "checkEdit12";
		string text6 = "FIrst Storege 4GB";
		string name8 = "checkEdit11";
		string text7 = "FIrst Storege 1GB";
		string name9 = "checkEdit10";
		string text8 = "FIrst Storege 512MB";
		string name10 = "checkEdit9";
		string text9 = "FIrst Storege 256MB";
		string name11 = "checkEdit3";
		string text10 = "FIrst Storege 128MB";
		string name12 = "checkEdit2";
		string text11 = "FIrst Storege 16MB";
		string name13 = "checkEdit4";
		string text12 = "FIrst Storege 64MB";
		string name14 = "checkEdit5";
		string text13 = "FIrst Storege 32MB";
		string name15 = "checkEdit6";
		string name16 = "checkEdit7";
		string text14 = "FIrst Storege 8MB";
		float field_2 = Class38.field_392;
		float field_3 = Class38.field_393;
		string text15 = "ReadDump";
		this.SimpleButton2 = new DevExpress.XtraEditors.SimpleButton();
		this.SimpleButton1 = new DevExpress.XtraEditors.SimpleButton();
		this.GroupControl1 = new DevExpress.XtraEditors.GroupControl();
		this.panel1 = new System.Windows.Forms.Panel();
		this.FullDump = new System.Windows.Forms.RadioButton();
		this.FullStorege = new System.Windows.Forms.RadioButton();
		this.checkEdit12 = new System.Windows.Forms.RadioButton();
		this.checkEdit11 = new System.Windows.Forms.RadioButton();
		this.checkEdit10 = new System.Windows.Forms.RadioButton();
		this.checkEdit9 = new System.Windows.Forms.RadioButton();
		this.checkEdit3 = new System.Windows.Forms.RadioButton();
		this.checkEdit2 = new System.Windows.Forms.RadioButton();
		this.checkEdit4 = new System.Windows.Forms.RadioButton();
		this.checkEdit5 = new System.Windows.Forms.RadioButton();
		this.checkEdit6 = new System.Windows.Forms.RadioButton();
		this.checkEdit7 = new System.Windows.Forms.RadioButton();
		((System.ComponentModel.ISupportInitialize)this.GroupControl1).BeginInit();
		this.GroupControl1.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.SimpleButton2.Location = new System.Drawing.Point(13, 239);
		this.SimpleButton2.LookAndFeel.UseDefaultLookAndFeel = false;
		this.SimpleButton2.Name = name;
		this.SimpleButton2.Size = new System.Drawing.Size(120, 30);
		this.SimpleButton2.TabIndex = 9;
		this.SimpleButton2.Text = text;
		this.SimpleButton2.Click += new System.EventHandler(SimpleButton2_Click);
		this.SimpleButton1.Location = new System.Drawing.Point(158, 239);
		this.SimpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
		this.SimpleButton1.Name = name2;
		this.SimpleButton1.Size = new System.Drawing.Size(359, 30);
		this.SimpleButton1.TabIndex = 8;
		this.SimpleButton1.Text = text2;
		this.SimpleButton1.Click += new System.EventHandler(SimpleButton1_Click);
		this.GroupControl1.Appearance.BackColor = System.Drawing.Color.Gray;
		this.GroupControl1.Appearance.BorderColor = System.Drawing.Color.Silver;
		this.GroupControl1.Appearance.Options.UseBackColor = true;
		this.GroupControl1.Appearance.Options.UseBorderColor = true;
		this.GroupControl1.AppearanceCaption.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
		this.GroupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.Transparent;
		this.GroupControl1.AppearanceCaption.Options.UseBorderColor = true;
		this.GroupControl1.AppearanceCaption.Options.UseForeColor = true;
		this.GroupControl1.AppearanceCaption.Options.UseTextOptions = true;
		this.GroupControl1.Controls.Add(this.panel1);
		this.GroupControl1.Location = new System.Drawing.Point(13, 13);
		this.GroupControl1.LookAndFeel.SkinMaskColor = System.Drawing.Color.Empty;
		this.GroupControl1.LookAndFeel.SkinMaskColor2 = System.Drawing.Color.Empty;
		this.GroupControl1.LookAndFeel.SkinName = skinName;
		this.GroupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
		this.GroupControl1.Name = name3;
		this.GroupControl1.Size = new System.Drawing.Size(506, 203);
		this.GroupControl1.TabIndex = 7;
		this.GroupControl1.Text = text3;
		this.panel1.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.panel1.Controls.Add(this.checkEdit7);
		this.panel1.Controls.Add(this.checkEdit6);
		this.panel1.Controls.Add(this.checkEdit5);
		this.panel1.Controls.Add(this.checkEdit4);
		this.panel1.Controls.Add(this.checkEdit2);
		this.panel1.Controls.Add(this.checkEdit3);
		this.panel1.Controls.Add(this.checkEdit9);
		this.panel1.Controls.Add(this.checkEdit10);
		this.panel1.Controls.Add(this.checkEdit11);
		this.panel1.Controls.Add(this.checkEdit12);
		this.panel1.Controls.Add(this.FullStorege);
		this.panel1.Controls.Add(this.FullDump);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(2, 29);
		this.panel1.Name = name4;
		this.panel1.Size = new System.Drawing.Size(502, 172);
		this.panel1.TabIndex = 8;
		this.FullDump.AutoSize = true;
		this.FullDump.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.FullDump.ForeColor = System.Drawing.Color.White;
		this.FullDump.Location = new System.Drawing.Point(43, 22);
		this.FullDump.Name = name5;
		this.FullDump.Size = new System.Drawing.Size(138, 17);
		this.FullDump.TabIndex = 14;
		this.FullDump.TabStop = true;
		this.FullDump.Text = text4;
		this.FullDump.UseVisualStyleBackColor = true;
		this.FullStorege.AutoSize = true;
		this.FullStorege.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.FullStorege.ForeColor = System.Drawing.Color.White;
		this.FullStorege.Location = new System.Drawing.Point(43, 45);
		this.FullStorege.Name = name6;
		this.FullStorege.Size = new System.Drawing.Size(199, 17);
		this.FullStorege.TabIndex = 15;
		this.FullStorege.TabStop = true;
		this.FullStorege.Text = text5;
		this.FullStorege.UseVisualStyleBackColor = true;
		this.checkEdit12.AutoSize = true;
		this.checkEdit12.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit12.ForeColor = System.Drawing.Color.White;
		this.checkEdit12.Location = new System.Drawing.Point(287, 137);
		this.checkEdit12.Name = name7;
		this.checkEdit12.Size = new System.Drawing.Size(125, 17);
		this.checkEdit12.TabIndex = 16;
		this.checkEdit12.TabStop = true;
		this.checkEdit12.Text = text6;
		this.checkEdit12.UseVisualStyleBackColor = true;
		this.checkEdit11.AutoSize = true;
		this.checkEdit11.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit11.ForeColor = System.Drawing.Color.White;
		this.checkEdit11.Location = new System.Drawing.Point(287, 114);
		this.checkEdit11.Name = name8;
		this.checkEdit11.Size = new System.Drawing.Size(125, 17);
		this.checkEdit11.TabIndex = 17;
		this.checkEdit11.TabStop = true;
		this.checkEdit11.Text = text7;
		this.checkEdit11.UseVisualStyleBackColor = true;
		this.checkEdit10.AutoSize = true;
		this.checkEdit10.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit10.ForeColor = System.Drawing.Color.White;
		this.checkEdit10.Location = new System.Drawing.Point(287, 91);
		this.checkEdit10.Name = name9;
		this.checkEdit10.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.checkEdit10.Size = new System.Drawing.Size(141, 17);
		this.checkEdit10.TabIndex = 18;
		this.checkEdit10.TabStop = true;
		this.checkEdit10.Text = text8;
		this.checkEdit10.UseVisualStyleBackColor = true;
		this.checkEdit9.AutoSize = true;
		this.checkEdit9.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit9.ForeColor = System.Drawing.Color.White;
		this.checkEdit9.Location = new System.Drawing.Point(287, 68);
		this.checkEdit9.Name = name10;
		this.checkEdit9.Size = new System.Drawing.Size(141, 17);
		this.checkEdit9.TabIndex = 19;
		this.checkEdit9.TabStop = true;
		this.checkEdit9.Text = text9;
		this.checkEdit9.UseVisualStyleBackColor = true;
		this.checkEdit3.AutoSize = true;
		this.checkEdit3.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit3.ForeColor = System.Drawing.Color.White;
		this.checkEdit3.Location = new System.Drawing.Point(287, 45);
		this.checkEdit3.Name = name11;
		this.checkEdit3.Size = new System.Drawing.Size(141, 17);
		this.checkEdit3.TabIndex = 20;
		this.checkEdit3.TabStop = true;
		this.checkEdit3.Text = text10;
		this.checkEdit3.UseVisualStyleBackColor = true;
		this.checkEdit2.AutoSize = true;
		this.checkEdit2.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit2.ForeColor = System.Drawing.Color.White;
		this.checkEdit2.Location = new System.Drawing.Point(287, 22);
		this.checkEdit2.Name = name12;
		this.checkEdit2.Size = new System.Drawing.Size(134, 17);
		this.checkEdit2.TabIndex = 21;
		this.checkEdit2.TabStop = true;
		this.checkEdit2.Text = text11;
		this.checkEdit2.UseVisualStyleBackColor = true;
		this.checkEdit4.AutoSize = true;
		this.checkEdit4.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit4.ForeColor = System.Drawing.Color.White;
		this.checkEdit4.Location = new System.Drawing.Point(43, 137);
		this.checkEdit4.Name = name13;
		this.checkEdit4.Size = new System.Drawing.Size(134, 17);
		this.checkEdit4.TabIndex = 22;
		this.checkEdit4.TabStop = true;
		this.checkEdit4.Text = text12;
		this.checkEdit4.UseVisualStyleBackColor = true;
		this.checkEdit5.AutoSize = true;
		this.checkEdit5.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit5.ForeColor = System.Drawing.Color.White;
		this.checkEdit5.Location = new System.Drawing.Point(43, 114);
		this.checkEdit5.Name = name14;
		this.checkEdit5.Size = new System.Drawing.Size(134, 17);
		this.checkEdit5.TabIndex = 23;
		this.checkEdit5.TabStop = true;
		this.checkEdit5.Text = text13;
		this.checkEdit5.UseVisualStyleBackColor = true;
		this.checkEdit6.AutoSize = true;
		this.checkEdit6.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit6.ForeColor = System.Drawing.Color.White;
		this.checkEdit6.Location = new System.Drawing.Point(43, 91);
		this.checkEdit6.Name = name15;
		this.checkEdit6.Size = new System.Drawing.Size(134, 17);
		this.checkEdit6.TabIndex = 24;
		this.checkEdit6.TabStop = true;
		this.checkEdit6.Text = text11;
		this.checkEdit6.UseVisualStyleBackColor = true;
		this.checkEdit7.AutoSize = true;
		this.checkEdit7.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkEdit7.ForeColor = System.Drawing.Color.White;
		this.checkEdit7.Location = new System.Drawing.Point(43, 68);
		this.checkEdit7.Name = name16;
		this.checkEdit7.Size = new System.Drawing.Size(127, 17);
		this.checkEdit7.TabIndex = 25;
		this.checkEdit7.TabStop = true;
		this.checkEdit7.Text = text14;
		this.checkEdit7.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(field_2, field_3);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(530, 286);
		base.Controls.Add(this.SimpleButton2);
		base.Controls.Add(this.SimpleButton1);
		base.Controls.Add(this.GroupControl1);
		base.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		((DevExpress.Utils.ImageOptions)this.method_02()).Image = AMFTool.Properties.Resources.logoamftool;
		base.MaximizeBox = false;
		base.Name = text15;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = text15;
		((System.ComponentModel.ISupportInitialize)this.GroupControl1).EndInit();
		this.GroupControl1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
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
