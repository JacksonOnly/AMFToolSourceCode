using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using AMFTool.Properties;
using DevExpress.Utils;
using DevExpress.Utils.Behaviors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace AMFTool;

public class Auth : XtraForm
{
	private string version;

	private string hwid;

	private bool isSuccess;

	private string server;

	private IContainer components;

	public CheckEdit CRemember;

	private SimpleButton Login;

	private TextEdit Password;

	private TextEdit Username;

	private LabelControl LPassword;

	private LabelControl LEmail;

	private BehaviorManager behaviorManager1;

	internal PictureEdit PictureIMG;

	private Button Btn_Clear;

	private Button showhide;

	private PanelControl panelControl1;

	public Auth()
	{
		string text = "1.2.5";
		string text2 = "Controllers/";
		object obj = null;
		string text3 = "Login AMFTool ";
		string text4 = " - www.amftool.com";
		version = text;
		hwid = string.Empty;
		isSuccess = false;
		server = UI.Server + text2;
		components = (IContainer)obj;

		InitializeComponent();
		Text = text3 + version + text4;
		base.Load += Auth_Load;
		base.FormClosing += Auth_FormClosing;
	}

	private void Login_Click(object sender, EventArgs e)
	{
		string text = "Password is empty!";
		string caption = "Information";
		string text2 = "Password must be at least 8 characters!";
		string value = "Update";
		string fileName = "https://amftool.com";
		string value2 = "Login Server [2]";
		string text3 = "https://www.amftool.com/";
		if (!string.IsNullOrEmpty(Password.Text))
		{
			if (Password.Text.Trim().Length >= 8)
			{
				if (!Login.Text.Contains(value))
				{
					if (!Login.Text.Contains(value2))
					{
						Setcomponent();
						string body = GetBody(Username.Text, Password.Text);
						LoginHandler(body);
					}
					else
					{
						UI.Server = string.Empty;
						UI.Server = text3;
						Setcomponent();
						string body = GetBody(Username.Text, Password.Text);
						LoginHandler(body);
					}
				}
				else
				{
					Process.Start(fileName);
				}
			}
			else
			{
				XtraMessageBox.Show(text2, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		else
		{
			XtraMessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void Auth_Load(object sender, EventArgs e)
	{
		string path = "C:\\Windows\\amftool.ini";
		string value = "username";
		string value2 = "password";
		string text = "";
		string value3 = "Check Connection Success";
		string text2 = "Login Server [2]";
		if (!CRemember.Checked)
		{
			File.Delete(path);
		}
		else if (File.Exists(path))
		{
			string[] array = File.ReadAllLines(path);
			int i = 0;
			for (int num = array.Count() - 1; i <= num; i++)
			{
				if (array[i].Contains(value))
				{
					string[] array2 = array[i].Split(':');
					Server.amtusername = array2[1];
					Username.Text = Server.amtusername;
				}
				if (array[i].Contains(value2))
				{
					string[] array3 = array[i].Split(':');
					Server.amtpassword = array3[1];
					Password.Text = Server.amtpassword;
				}
			}
		}
		if (!(Server.amtpassword == text))
		{
			Password.Text = Server.amtpassword;
		}
		if (!(Password.Text == text))
		{
			CRemember.Checked = true;
		}
		else
		{
			CRemember.Checked = false;
		}
		if (Checkinternet())
		{
			Console.WriteLine(value3);
			if (!CheckUpdate())
			{
			}
		}
		else
		{
			Login.Text = text2;
			Login.ForeColor = Color.Tomato;
		}
	}

	private void Auth_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (!isSuccess)
		{
			Process.GetCurrentProcess().Kill();
		}
	}

	private bool Checkinternet()
	{
		string value = "Check internet";
		string hostNameOrAddress = "amftool.com";
		int result = 0;
		int enable = 1;
		Console.WriteLine(value);
		try
		{
			using Ping ping = new Ping();
			PingReply pingReply = ping.Send(hostNameOrAddress, 2000);
			return pingReply.Status == IPStatus.Success;
		}
		catch
		{
			Setcomponent((byte)enable != 0);
			return (byte)result != 0;
		}
	}

	private bool CheckUpdate()
	{
		string text = "Version.php";
		string method = "GET";
		object proxy = null;
		string text2 = "";
		string text3 = "Latest Version ";
		string propertyName = "version";
		string text4 = "Current Version ";
		string text5 = "Update ";
		int result = 1;
		int result2 = 0;
		try
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
			string requestUriString = server + text;
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			httpWebRequest.Method = method;
			httpWebRequest.Timeout = 20000;
			httpWebRequest.Proxy = (IWebProxy)proxy;
			httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			string text6 = text2;
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
			{
				using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
				text6 = streamReader.ReadToEnd().Trim();
			}
			Console.WriteLine(text3 + text6);
			using JsonDocument jsonDocument = JsonDocument.Parse(text6);
			text6 = jsonDocument.RootElement.GetProperty(propertyName).GetString();
			Console.WriteLine(text4 + text6);
			if (text6 != version)
			{
				Login.Text = text5 + text6;
			}
			return (byte)result != 0;
		}
		catch
		{
			return (byte)result2 != 0;
		}
	}

	private void Setcomponent(bool enable = false)
	{
		Username.Enabled = enable;
		Password.Enabled = enable;
		Login.Enabled = enable;
	}

	private void LoginHandler(string postdata)
	{
		string text = "Auth.php";
		int num = 0;
		double field_ = Class38.field_306;
		string mediaType = "application/x-www-form-urlencoded";
		string text2 = "Login failed (";
		string text3 = ")";
		string caption = "Login";
		int icon = 16;
		string text4 = "Invalid server response (";
		string propertyName = "c3RhdHVz";
		string text5 = "";
		string text6 = "YW5kYXRlcmRldGVrc2lnYW50ZW5n";
		string propertyName2 = "reason";
		int num2 = 1;
		string propertyName3 = "message";
		string text7 = "Login failed";
		string propertyName4 = "email";
		string propertyName5 = "expired";
		string propertyName6 = "hwid";
		string text8 = "root =";
		string text9 = "EMAIL =";
		string text10 = "EXPIRED =";
		string text11 = "HWID =";
		string path = "C:\\Windows\\amftool.ini";
		string text12 = "username:";
		string text13 = "password:";
		string caption2 = "POST ERROR";
		try
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
			string requestUri = server + text;
			HttpClientHandler handler = new HttpClientHandler
			{
				UseProxy = false,
				AllowAutoRedirect = false,
				UseCookies = false,
				AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate)
			};
			using HttpClient httpClient = new HttpClient(handler);
			httpClient.Timeout = TimeSpan.FromSeconds(field_);
			using StringContent content = new StringContent(postdata, Encoding.UTF8, mediaType);
			HttpResponseMessage result = httpClient.PostAsync(requestUri, content).GetAwaiter().GetResult();
			string result2 = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
			if (!string.IsNullOrWhiteSpace(result2))
			{
				string json;
				try
				{
					json = Server.DecryptAES(result2);
				}
				catch
				{
					XtraMessageBox.Show(text4 + (int)result.StatusCode + text3, caption, (MessageBoxButtons)num, (MessageBoxIcon)icon);
					return;
				}
				using JsonDocument jsonDocument = JsonDocument.Parse(json);
				JsonElement rootElement = jsonDocument.RootElement;
				JsonElement value;
				string text14 = (rootElement.TryGetProperty(propertyName, out value) ? value.GetString() : text5);
				if (!(text14 != text6))
				{
					string text15 = rootElement.GetProperty(propertyName4).GetString();
					string expired = rootElement.GetProperty(propertyName5).GetString();
					string text16 = rootElement.GetProperty(propertyName6).GetString();
					Console.WriteLine(text8 + rootElement);
					Console.WriteLine(text9 + text15);
					Console.WriteLine(text10 + expired);
					Console.WriteLine(text11 + text16);
					if (CRemember.Checked)
					{
						if (File.Exists(path))
						{
							File.Delete(path);
							StreamWriter streamWriter = new StreamWriter(path, (byte)num2 != 0);
							streamWriter.WriteLine(text12 + Username.Text);
							streamWriter.WriteLine(text13 + Password.Text);
							streamWriter.Close();
						}
						else
						{
							StreamWriter streamWriter2 = new StreamWriter(path, (byte)num2 != 0);
							streamWriter2.WriteLine(text12 + Username.Text);
							streamWriter2.WriteLine(text13 + Password.Text);
							streamWriter2.Close();
						}
					}
					else
					{
						File.Delete(path);
					}
					if ((!string.IsNullOrEmpty(expired)) ? ((byte)num != 0) : string.IsNullOrEmpty(text15))
					{
						Process.GetCurrentProcess().Kill();
					}
					User.Email = Username.Text;
					User.Expired = expired;
					isSuccess = (byte)num2 != 0;
					Thread thread = new Thread((ThreadStart)delegate
					{
						//Error decoding local variables: Signature type sequence must have at least one element.
						Application.Run(new Main(Username.Text.Trim(), Password.Text.Trim(), hwid, expired, version));
					});
					thread.SetApartmentState((ApartmentState)num);
					thread.Start();
					Close();
					return;
				}
				string text17 = text5;
				JsonElement value4;
				if (rootElement.TryGetProperty(propertyName2, out var value2))
				{
					JsonElement value3 = default(JsonElement);
					if (((int)value2.ValueKind == num2) ? value2.TryGetProperty(propertyName3, out value3) : ((byte)num != 0))
					{
						text17 = value3.GetString();
					}
				}
				else if (rootElement.TryGetProperty(propertyName3, out value4))
				{
					text17 = value4.GetString();
				}
				if (string.IsNullOrWhiteSpace(text17))
				{
					text17 = text2 + (int)result.StatusCode + text3;
				}
				Setcomponent((byte)num2 != 0);
				XtraMessageBox.Show(text17 ?? text7, caption, (MessageBoxButtons)num, (MessageBoxIcon)icon);
				Setcomponent((byte)num2 != 0);
				return;
			}
			XtraMessageBox.Show(text2 + (int)result.StatusCode + text3, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		catch (Exception ex)
		{
			Setcomponent((byte)num2 != 0);
			XtraMessageBox.Show(ex.ToString(), caption2);
		}
	}

	private string GetHWID()
	{
		string text = "";
		string path = "win32_processor";
		string propertyName = "processorID";
		string path2 = "win32_bios";
		string propertyName2 = "serialnumber";
		string path3 = "Win32_LogicalDisk.DeviceID=\"C:\"";
		string propertyName3 = "VolumeSerialNumber";
		string oldValue = "-";
		string text2 = "AMF-";
		int startIndex = 0;
		int length = 10;
		string result = "UNKNOWN_HWID";
		try
		{
			string text3 = text;
			string text4 = text;
			string text5 = text;
			using (ManagementClass managementClass = new ManagementClass(path))
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementClass.GetInstances().GetEnumerator();
				if (managementObjectEnumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
					text3 = managementObject.Properties[propertyName].Value.ToString();
				}
			}
			using (ManagementClass managementClass2 = new ManagementClass(path2))
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator2 = managementClass2.GetInstances().GetEnumerator();
				if (managementObjectEnumerator2.MoveNext())
				{
					ManagementObject managementObject2 = (ManagementObject)managementObjectEnumerator2.Current;
					text4 = managementObject2.Properties[propertyName2].Value.ToString();
				}
			}
			using (ManagementObject managementObject3 = new ManagementObject(path3))
			{
				managementObject3.Get();
				text5 = managementObject3[propertyName3].ToString();
			}
			string s = text3 + text4 + text5;
			using SHA256 sHA = SHA256.Create();
			byte[] array = sHA.ComputeHash(Encoding.UTF8.GetBytes(s));
			string text6 = BitConverter.ToString(array).Replace(oldValue, text);
			return text2 + text6.Substring(startIndex, length);
		}
		catch
		{
			return result;
		}
	}

	private string GetBody(string username, string password)
	{
		string text = "email=";
		string text2 = "&password=";
		string text3 = "&hwid=";
		string text4 = "&ganteng=AMFTool";
		string text5 = "data=";
		string stringToEscape = (hwid = GetHWID());
		string data = text + Uri.EscapeDataString(username) + text2 + Uri.EscapeDataString(password) + text3 + Uri.EscapeDataString(stringToEscape) + text4;
		string stringToEscape2 = Server.EncryptAES(data);
		return text5 + Uri.EscapeDataString(stringToEscape2);
	}

	private void Btn_Clear_Click(object sender, EventArgs e)
	{
		string path = "C:\\Windows\\amftool.ini";
		Username.Text = string.Empty;
		Password.Text = string.Empty;
		File.Delete(path);
	}

	private void showhide_Click(object sender, EventArgs e)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (Password.Properties.UseSystemPasswordChar)
		{
			Password.Properties.UseSystemPasswordChar = false;
			showhide.Image = Resources.hide;
		}
		else
		{
			showhide.Image = Resources.show;
			Password.Properties.UseSystemPasswordChar = true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		object obj = null;
		if (disposing)
		{
			if (components !=null)
			{
				components.Dispose();
				base.Dispose(disposing);
			}
			else
			{
				base.Dispose(disposing);
			}
		}
		else
		{
			base.Dispose(disposing);
		}
	}

	private void InitializeComponent()
	{
		string familyName = "Tahoma";
		float field_ = Class38.field_342;
		string name = "LPassword";
		string text = "Password";
		string name2 = "LEmail";
		string text2 = "Email";
		string name3 = "panelControl1";
		string name4 = "showhide.Image";
		string name5 = "showhide";
		string name6 = "Btn_Clear.Image";
		string name7 = "Btn_Clear";
		string name8 = "PictureIMG";
		string name9 = "CRemember";
		string caption = "Remember Me";
		string familyName2 = "Calibri";
		float field_2 = Class38.field_374;
		float field_3 = Class38.field_375;
		string name10 = "Login.ImageOptions.Image";
		string text3 = "Login";
		string text4 = " Login";
		string name11 = "Password.Properties.ContextImageOptions.Image";
		string name12 = "Username";
		string name13 = "Username.Properties.ContextImageOptions.Image";
		string toolTip = "Gmail";
		float field_4 = Class38.field_392;
		float field_5 = Class38.field_393;
		string name14 = "Auth";
		string text5 = "Login AMFTool";
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AMFTool.Auth));
		this.LPassword = new DevExpress.XtraEditors.LabelControl();
		this.LEmail = new DevExpress.XtraEditors.LabelControl();
		this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
		this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
		this.showhide = new System.Windows.Forms.Button();
		this.Btn_Clear = new System.Windows.Forms.Button();
		this.PictureIMG = new DevExpress.XtraEditors.PictureEdit();
		this.CRemember = new DevExpress.XtraEditors.CheckEdit();
		this.Login = new DevExpress.XtraEditors.SimpleButton();
		this.Password = new DevExpress.XtraEditors.TextEdit();
		this.Username = new DevExpress.XtraEditors.TextEdit();
		((System.ComponentModel.ISupportInitialize)this.behaviorManager1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
		this.panelControl1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PictureIMG.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CRemember.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Password.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Username.Properties).BeginInit();
		base.SuspendLayout();
		this.LPassword.Appearance.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LPassword.Appearance.Options.UseFont = true;
		this.LPassword.Location = new System.Drawing.Point(60, 349);
		this.LPassword.Name = name;
		this.LPassword.Size = new System.Drawing.Size(54, 13);
		this.LPassword.TabIndex = 251;
		this.LPassword.Text = text;
		this.LEmail.Appearance.Font = new System.Drawing.Font(familyName, field_, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LEmail.Appearance.Options.UseFont = true;
		this.LEmail.Location = new System.Drawing.Point(60, 318);
		this.LEmail.Name = name2;
		this.LEmail.Size = new System.Drawing.Size(30, 13);
		this.LEmail.TabIndex = 250;
		this.LEmail.Text = text2;
		this.panelControl1.Controls.Add(this.PictureIMG);
		this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelControl1.Location = new System.Drawing.Point(0, 0);
		this.panelControl1.Name = name3;
		this.panelControl1.Size = new System.Drawing.Size(430, 505);
		this.panelControl1.TabIndex = 25491;
		this.showhide.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.showhide.BackColor = System.Drawing.Color.White;
		this.showhide.Cursor = System.Windows.Forms.Cursors.Hand;
		this.showhide.FlatAppearance.BorderSize = 0;
		this.showhide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.showhide.ForeColor = System.Drawing.Color.FromArgb(81, 81, 81);
		this.showhide.Image = (System.Drawing.Image)resources.GetObject(name4);
		this.showhide.Location = new System.Drawing.Point(323, 349);
		this.showhide.Name = name5;
		this.showhide.Size = new System.Drawing.Size(13, 13);
		this.showhide.TabIndex = 25490;
		this.showhide.UseVisualStyleBackColor = false;
		this.showhide.Click += new System.EventHandler(showhide_Click);
		this.Btn_Clear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.Btn_Clear.BackColor = System.Drawing.Color.White;
		this.Btn_Clear.Cursor = System.Windows.Forms.Cursors.Hand;
		this.Btn_Clear.FlatAppearance.BorderSize = 0;
		this.Btn_Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Btn_Clear.ForeColor = System.Drawing.Color.FromArgb(81, 81, 81);
		this.Btn_Clear.Image = (System.Drawing.Image)resources.GetObject(name6);
		this.Btn_Clear.Location = new System.Drawing.Point(323, 318);
		this.Btn_Clear.Name = name7;
		this.Btn_Clear.Size = new System.Drawing.Size(13, 13);
		this.Btn_Clear.TabIndex = 25489;
		this.Btn_Clear.UseVisualStyleBackColor = false;
		this.Btn_Clear.Click += new System.EventHandler(Btn_Clear_Click);
		this.PictureIMG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.PictureIMG.EditValue = AMFTool.Properties.Resources.logoamftool;
		this.PictureIMG.ImeMode = System.Windows.Forms.ImeMode.On;
		this.PictureIMG.Location = new System.Drawing.Point(82, 45);
		this.PictureIMG.Name = name8;
		this.PictureIMG.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
		this.PictureIMG.Properties.Appearance.ForeColor = System.Drawing.Color.Transparent;
		this.PictureIMG.Properties.Appearance.Options.UseBackColor = true;
		this.PictureIMG.Properties.Appearance.Options.UseForeColor = true;
		this.PictureIMG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
		this.PictureIMG.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
		this.PictureIMG.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
		this.PictureIMG.Properties.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.Full;
		this.PictureIMG.Properties.ZoomingOperationMode = DevExpress.XtraEditors.Repository.ZoomingOperationMode.ControlMouseWheel;
		this.PictureIMG.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.PictureIMG.Size = new System.Drawing.Size(254, 217);
		this.PictureIMG.TabIndex = 1;
		this.CRemember.EditValue = true;
		this.CRemember.Location = new System.Drawing.Point(60, 389);
		this.CRemember.Name = name9;
		this.CRemember.Properties.AllowFocused = false;
		this.CRemember.Properties.Caption = caption;
		this.CRemember.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2;
		this.CRemember.Properties.CheckBoxOptions.SvgColorChecked = System.Drawing.Color.DodgerBlue;
		this.CRemember.Properties.CheckBoxOptions.SvgColorUnchecked = System.Drawing.Color.DodgerBlue;
		this.CRemember.Size = new System.Drawing.Size(97, 22);
		this.CRemember.TabIndex = 256;
		this.Login.Appearance.BackColor = System.Drawing.Color.White;
		this.Login.Appearance.Font = new System.Drawing.Font(familyName2, field_2, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Login.Appearance.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.Login.Appearance.Options.UseBackColor = true;
		this.Login.Appearance.Options.UseFont = true;
		this.Login.Appearance.Options.UseForeColor = true;
		this.Login.Appearance.Options.UseTextOptions = true;
		this.Login.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
		this.Login.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(81, 81, 81);
		this.Login.AppearanceDisabled.Font = new System.Drawing.Font(familyName2, field_3, System.Drawing.FontStyle.Bold);
		this.Login.AppearanceDisabled.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.Login.AppearanceDisabled.Options.UseBackColor = true;
		this.Login.AppearanceDisabled.Options.UseFont = true;
		this.Login.AppearanceDisabled.Options.UseForeColor = true;
		this.Login.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(81, 81, 81);
		this.Login.AppearanceHovered.Font = new System.Drawing.Font(familyName2, field_3, System.Drawing.FontStyle.Bold);
		this.Login.AppearanceHovered.ForeColor = System.Drawing.Color.DodgerBlue;
		this.Login.AppearanceHovered.Options.UseBackColor = true;
		this.Login.AppearanceHovered.Options.UseFont = true;
		this.Login.AppearanceHovered.Options.UseForeColor = true;
		this.Login.AppearanceHovered.Options.UseImage = true;
		this.Login.AppearancePressed.BackColor = System.Drawing.Color.FromArgb(81, 81, 81);
		this.Login.AppearancePressed.Font = new System.Drawing.Font(familyName2, field_3, System.Drawing.FontStyle.Bold);
		this.Login.AppearancePressed.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.Login.AppearancePressed.Options.UseBackColor = true;
		this.Login.AppearancePressed.Options.UseFont = true;
		this.Login.AppearancePressed.Options.UseForeColor = true;
		this.Login.ImageOptions.Image = (System.Drawing.Image)resources.GetObject(name10);
		this.Login.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.Login.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.Login.Location = new System.Drawing.Point(213, 383);
		this.Login.Name = text3;
		this.Login.Size = new System.Drawing.Size(127, 28);
		this.Login.TabIndex = 25488;
		this.Login.Text = text4;
		this.Login.ToolTipTitle = text3;
		this.Login.Click += new System.EventHandler(Login_Click);
		this.Password.Location = new System.Drawing.Point(120, 345);
		this.Password.Name = text;
		this.Password.Properties.ContextImageOptions.Image = (System.Drawing.Image)resources.GetObject(name11);
		this.Password.Properties.UseSystemPasswordChar = true;
		this.Password.Size = new System.Drawing.Size(220, 22);
		this.Password.TabIndex = 2535;
		this.Password.ToolTip = text;
		this.Username.Location = new System.Drawing.Point(120, 315);
		this.Username.Name = name12;
		this.Username.Properties.ContextImageOptions.Image = (System.Drawing.Image)resources.GetObject(name13);
		this.Username.Size = new System.Drawing.Size(220, 20);
		this.Username.TabIndex = 577;
		this.Username.ToolTip = toolTip;
		base.AutoScaleDimensions = new System.Drawing.SizeF(field_4, field_5);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(430, 505);
		base.Controls.Add(this.showhide);
		base.Controls.Add(this.Btn_Clear);
		base.Controls.Add(this.CRemember);
		base.Controls.Add(this.Login);
		base.Controls.Add(this.Password);
		base.Controls.Add(this.Username);
		base.Controls.Add(this.LPassword);
		base.Controls.Add(this.LEmail);
		base.Controls.Add(this.panelControl1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		((DevExpress.Utils.ImageOptions)this.method_02()).Image = AMFTool.Properties.Resources.logoamftool;
		base.MaximizeBox = false;
		base.Name = name14;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = text5;
		((System.ComponentModel.ISupportInitialize)this.behaviorManager1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
		this.panelControl1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.PictureIMG.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CRemember.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Password.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Username.Properties).EndInit();
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

	private object method_03()
	{
		return new int[0];
	}
}
