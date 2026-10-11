using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using AMFTool.Properties;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace AMFTool;

public class Load : XtraForm
{
	public Load load;

	private IContainer components = null;

	private SimpleButton Loadfirmware;

	public DataGridView XMLEDL;

	private CheckBox CFirmware;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn14;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn41;

	private DataGridViewTextBoxColumn Column9;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn42;

	public Load()
	{
		InitializeComponent();
	}

	public Load(object[] data, string header)
	{
		InitializeComponent();
		Text = header;
		XMLEDL.Rows.Clear();
	}

	public void AddRawProgram(object[] data)
	{
		XMLEDL.Rows.Add(data);
	}

	public void AddPatch(object[] data)
	{
		XMLEDL.Rows.Add(data);
	}

	public void ClearRow()
	{
		XMLEDL.Rows.Clear();
	}

	private void Loadfirmware_Click(object sender, EventArgs e)
	{
		string value = "Select RawProgram";
		string value2 = "Select Patch";
		object obj = null;
		bool flag = Text.Equals(value, StringComparison.OrdinalIgnoreCase);
		bool flag2 = Text.Equals(value2, StringComparison.OrdinalIgnoreCase);
		if (flag)
		{
			Main.Instance.EDLFirmware.Clear();
			Main.Instance.EDLDataGrid.Rows.Clear();
			bool flag3 = default(bool);
			foreach (DataGridViewRow item in (IEnumerable)XMLEDL.Rows)
			{
				object value3 = item.Cells[0].Value;
				int num;
				if (value3 is bool)
				{
					flag3 = (bool)value3;
					num = 1;
				}
				else
				{
					num = 0;
				}
				if (((uint)num & (flag3 ? 1u : 0u)) != 0)
				{
					object value4 = item.Cells[3].Value;
					ParseXml((string)((value4 != null) ? value4.ToString() : obj));
				}
			}
		}
		else if (flag2)
		{
			Main.Instance.EDLPatch.Clear();
			EDLConfig.PatchXML.Clear();
			bool flag4 = default(bool);
			foreach (DataGridViewRow item2 in (IEnumerable)XMLEDL.Rows)
			{
				object value3 = item2.Cells[0].Value;
				int num2;
				if (!(value3 is bool))
				{
					num2 = 0;
				}
				else
				{
					flag4 = (bool)value3;
					num2 = 1;
				}
				if (((uint)num2 & (flag4 ? 1u : 0u)) != 0)
				{
					object value5 = item2.Cells[3].Value;
					string text = (string)((value5 != null) ? value5.ToString() : obj);
					if (!string.IsNullOrEmpty(text))
					{
						EDLConfig.PatchXML.Add(text);
					}
				}
			}
		}
		Close();
	}

	private static void ParseXml(string xmlPath)
	{
		string text = "program";
		string name = "SECTOR_SIZE_IN_BYTES";
		string text2 = "512";
		string name2 = "file_sector_offset";
		string text3 = "0";
		string name3 = "filename";
		string name4 = "label";
		string name5 = "num_partition_sectors";
		string name6 = "physical_partition_number";
		string name7 = "size_in_KB";
		string name8 = "start_sector";
		string text4 = ".bin";
		string oldValue = "bak";
		string newValue = "";
		string text5 = ".img";
		string text6 = "4096";
		string storage = "emmc";
		string storage2 = "ufs";
		if (string.IsNullOrWhiteSpace(xmlPath) || !File.Exists(xmlPath))
		{
			return;
		}
		using XmlReader xmlReader = XmlReader.Create(xmlPath);
		while (xmlReader.Read())
		{
			if (xmlReader.NodeType != XmlNodeType.Element || !(xmlReader.Name == text))
			{
				continue;
			}
			string text7 = xmlReader.GetAttribute(name) ?? text2;
			_ = xmlReader.GetAttribute(name2) ?? text3;
			string text8 = xmlReader.GetAttribute(name3) ?? string.Empty;
			string text9 = xmlReader.GetAttribute(name4) ?? string.Empty;
			string text10 = xmlReader.GetAttribute(name5) ?? text3;
			string text11 = xmlReader.GetAttribute(name6) ?? text3;
			string text12 = xmlReader.GetAttribute(name7) ?? text3;
			string text13 = xmlReader.GetAttribute(name8) ?? text3;
			string pathFirmware = EDLConfig.PathFirmware;
			string text14 = Path.Combine(pathFirmware, text9 + text4).Replace(oldValue, newValue);
			string text15 = Path.Combine(pathFirmware, text9 + text5).Replace(oldValue, newValue);
			string text16 = Path.Combine(pathFirmware, text8);
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (!File.Exists(text14))
			{
				if (!File.Exists(text15))
				{
					if (!File.Exists(text16))
					{
						empty = string.Empty;
						empty2 = text8;
					}
					else
					{
						empty = text16;
						empty2 = Path.GetFileName(text16);
					}
				}
				else
				{
					empty = text15;
					empty2 = Path.GetFileName(text15);
				}
			}
			else
			{
				empty = text14;
				empty2 = Path.GetFileName(text14);
			}
			if (!string.IsNullOrEmpty(empty2))
			{
				Main.Instance.EDLDataGrid.Rows.Add(false, text11, text7, text9, empty2, text12, text10, text13, empty);
				string text17 = text7;
				string text18 = text17;
				if (text18 == text2)
				{
					EDLConfig.Storage = storage;
					Main.Instance.EDLStorageMode.SelectedItem = Main.Instance.EDLStorageMode.Properties.Items[0];
				}
				else if (text18 == text6)
				{
					EDLConfig.Storage = storage2;
					Main.Instance.EDLStorageMode.SelectedItem = Main.Instance.EDLStorageMode.Properties.Items[1];
				}
			}
		}
	}

	private void CFirmware_CheckedChanged(object sender, EventArgs e)
	{
		object obj = null;
		string text = "";
		string value = "raw";
		string value2 = "patch";
		foreach (DataGridViewRow item in (IEnumerable)XMLEDL.Rows)
		{
			object value3 = item.Cells[1].Value;
			object obj2;
			if (value3 == null)
			{
				obj2 = obj;
				if (obj2 != null)
				{
					goto IL_0068;
				}
			}
			else
			{
				obj2 = value3.ToString();
				if (obj2 != null)
				{
					goto IL_0068;
				}
			}
			obj2 = text;
			goto IL_0068;
			IL_0068:
			string text2 = (string)obj2;
			if (!CFirmware.Checked)
			{
				item.Cells[0].Value = false;
			}
			else
			{
				item.Cells[0].Value = text2.StartsWith(value, StringComparison.OrdinalIgnoreCase) || text2.StartsWith(value2, StringComparison.OrdinalIgnoreCase);
			}
		}
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
		string name = "Loadfirmware";
		string text = "Load";
		string familyName = "Tahoma";
		float field_ = Class38.field_342;
		string name2 = "XMLEDL";
		string familyName2 = "Microsoft Sans Serif";
		string text2 = "";
		string name3 = "dataGridViewCheckBoxColumn14";
		string familyName3 = "Consolas";
		float field_2 = Class38.field_375;
		string headerText = "Partititon";
		string name4 = "dataGridViewTextBoxColumn41";
		string headerText2 = "XML";
		string name5 = "Column9";
		string headerText3 = "Location";
		string name6 = "dataGridViewTextBoxColumn42";
		string name7 = "CFirmware";
		float field_3 = Class38.field_392;
		float field_4 = Class38.field_393;
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		this.Loadfirmware = new DevExpress.XtraEditors.SimpleButton();
		this.XMLEDL = new System.Windows.Forms.DataGridView();
		this.dataGridViewCheckBoxColumn14 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.dataGridViewTextBoxColumn41 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn42 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CFirmware = new System.Windows.Forms.CheckBox();
		((System.ComponentModel.ISupportInitialize)this.XMLEDL).BeginInit();
		base.SuspendLayout();
		this.Loadfirmware.Location = new System.Drawing.Point(7, 388);
		this.Loadfirmware.Name = name;
		this.Loadfirmware.Size = new System.Drawing.Size(361, 23);
		this.Loadfirmware.TabIndex = 177;
		this.Loadfirmware.Text = text;
		this.Loadfirmware.Click += new System.EventHandler(Loadfirmware_Click);
		this.XMLEDL.AllowDrop = true;
		this.XMLEDL.AllowUserToAddRows = false;
		this.XMLEDL.AllowUserToDeleteRows = false;
		this.XMLEDL.AllowUserToResizeColumns = false;
		this.XMLEDL.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.XMLEDL.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		this.XMLEDL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.XMLEDL.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.XMLEDL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.XMLEDL.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle2.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Gray;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.XMLEDL.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.XMLEDL.ColumnHeadersHeight = 17;
		this.XMLEDL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.XMLEDL.Columns.AddRange(this.dataGridViewCheckBoxColumn14, this.dataGridViewTextBoxColumn41, this.Column9, this.dataGridViewTextBoxColumn42);
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle3.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
		dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.XMLEDL.DefaultCellStyle = dataGridViewCellStyle3;
		this.XMLEDL.EnableHeadersVisualStyles = false;
		this.XMLEDL.GridColor = System.Drawing.Color.FromArgb(45, 45, 45);
		this.XMLEDL.Location = new System.Drawing.Point(7, 12);
		this.XMLEDL.Name = name2;
		this.XMLEDL.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle4.Font = new System.Drawing.Font(familyName, field_);
		dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
		dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Yellow;
		dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.XMLEDL.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
		this.XMLEDL.RowHeadersVisible = false;
		this.XMLEDL.RowHeadersWidth = 20;
		dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
		dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(0, 103, 192);
		dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(250, 250, 250);
		this.XMLEDL.RowsDefaultCellStyle = dataGridViewCellStyle5;
		this.XMLEDL.RowTemplate.Height = 20;
		this.XMLEDL.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.XMLEDL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.XMLEDL.Size = new System.Drawing.Size(361, 370);
		this.XMLEDL.StandardTab = true;
		this.XMLEDL.TabIndex = 464;
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
		dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ControlLight;
		dataGridViewCellStyle6.Font = new System.Drawing.Font(familyName2, field_, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		dataGridViewCellStyle6.NullValue = false;
		dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
		dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewCheckBoxColumn14.DefaultCellStyle = dataGridViewCellStyle6;
		this.dataGridViewCheckBoxColumn14.Frozen = true;
		this.dataGridViewCheckBoxColumn14.HeaderText = text2;
		this.dataGridViewCheckBoxColumn14.IndeterminateValue = text2;
		this.dataGridViewCheckBoxColumn14.MinimumWidth = 3;
		this.dataGridViewCheckBoxColumn14.Name = name3;
		this.dataGridViewCheckBoxColumn14.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewCheckBoxColumn14.Width = 20;
		dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle7.Font = new System.Drawing.Font(familyName3, field_2);
		dataGridViewCellStyle7.ForeColor = System.Drawing.Color.LightSteelBlue;
		dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(72, 70, 68);
		dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.dataGridViewTextBoxColumn41.DefaultCellStyle = dataGridViewCellStyle7;
		this.dataGridViewTextBoxColumn41.Frozen = true;
		this.dataGridViewTextBoxColumn41.HeaderText = headerText;
		this.dataGridViewTextBoxColumn41.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn41.Name = name4;
		this.dataGridViewTextBoxColumn41.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn41.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn41.Width = 120;
		this.Column9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Column9.HeaderText = headerText2;
		this.Column9.Name = name5;
		this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle8.Font = new System.Drawing.Font(familyName3, field_2);
		dataGridViewCellStyle8.ForeColor = System.Drawing.Color.LightGreen;
		dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(216, 216, 216);
		this.dataGridViewTextBoxColumn42.DefaultCellStyle = dataGridViewCellStyle8;
		this.dataGridViewTextBoxColumn42.HeaderText = headerText3;
		this.dataGridViewTextBoxColumn42.MinimumWidth = 4;
		this.dataGridViewTextBoxColumn42.Name = name6;
		this.dataGridViewTextBoxColumn42.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewTextBoxColumn42.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.CFirmware.AutoSize = true;
		this.CFirmware.BackColor = System.Drawing.SystemColors.ControlLight;
		this.CFirmware.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.CFirmware.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.CFirmware.Location = new System.Drawing.Point(10, 17);
		this.CFirmware.Name = name7;
		this.CFirmware.Size = new System.Drawing.Size(12, 11);
		this.CFirmware.TabIndex = 466;
		this.CFirmware.UseVisualStyleBackColor = false;
		this.CFirmware.CheckedChanged += new System.EventHandler(CFirmware_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(field_3, field_4);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(377, 423);
		base.Controls.Add(this.CFirmware);
		base.Controls.Add(this.XMLEDL);
		base.Controls.Add(this.Loadfirmware);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		((DevExpress.Utils.ImageOptions)this.method_02()).Image = AMFTool.Properties.Resources.logoamftool;
		base.MaximizeBox = false;
		base.Name = text;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = text;
		((System.ComponentModel.ISupportInitialize)this.XMLEDL).EndInit();
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
