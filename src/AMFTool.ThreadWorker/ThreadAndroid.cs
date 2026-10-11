using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using AMFTool.Library.Android;

namespace AMFTool.ThreadWorker;

public class ThreadAndroid
{
	private AdbClient ADB;

	public ThreadAndroid()
	{
		object adbServerHost = null;

		ADB = new AdbClient(-1, (string)adbServerHost);
	}

	public bool Init()
	{
		string msg = "Installing APK... ";
		string text = "install ";
		string msg2 = "OK";
		int result = 1;
		string msg3 = "Load APK from device... ";
		string msg4 = "Failed!";
		string msg5 = "Uninstall ";
		string msg6 = "... ";
		string msg7 = "Disabling ";
		string text2 = "pm disable-user --user 0 ";
		string msg8 = "Enabling ";
		string text3 = "pm enable ";
		if (!Initializing())
		{
			return false;
		}
		switch (State.Job)
		{
		case (Job)90:
		{
			UI.RichLogs(msg3, UI.log);
			AdbAppInfo[] installedApplications = ADB.GetInstalledApplications();
			if (!installedApplications.Any())
			{
				UI.RichLogs(msg4, UI.error);
				return false;
			}
			AdbAppInfo[] array2 = installedApplications;
			foreach (AdbAppInfo app in array2)
			{
				Main.Instance.AndroidDataGrid.BeginInvoke((Action)delegate
				{
					//Error decoding local variables: Signature type sequence must have at least one element.
					Main.Instance.AndroidDataGrid.Rows.Add(false, app.Name, app.FileName);
				});
			}
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			break;
		}
		case Job.ApkInstall:
			UI.RichLogs(msg, UI.log);
			ADB.ADBConsole(text + State.FilePath);
			UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			break;
		case (Job)92:
			foreach (string item in DeviceState.Partitiontarget)
			{
				UI.RichLogs(msg5, UI.log);
				UI.RichLogs(item, UI.info, nextline: false, bold: true);
				UI.RichLogs(msg6, UI.info, nextline: false, bold: true);
				ADB.UninstallApplication(item);
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			}
			break;
		case Job.ApkDisable:
			foreach (string item2 in DeviceState.Partitiontarget)
			{
				UI.RichLogs(msg7, UI.log);
				UI.RichLogs(item2, UI.info, nextline: false, bold: true);
				UI.RichLogs(msg6, UI.info, nextline: false, bold: true);
				string[] array = ADB.ExecuteRemoteCommand(text2 + item2);
				Console.WriteLine(array[0]);
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			}
			break;
		case (Job)94:
			foreach (string item3 in DeviceState.Partitiontarget)
			{
				UI.RichLogs(msg8, UI.log);
				UI.RichLogs(item3, UI.info, nextline: false, bold: true);
				UI.RichLogs(msg6, UI.info, nextline: false, bold: true);
				ADB.ExecuteRemoteCommand(text3 + item3);
				UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
			}
			break;
		case (Job)66:
			EnablediagXiaomiOperation();
			break;
		case Job.ADBSamsungAccount:
			ADBSamsungAccount();
			break;
		case (Job)36:
			ADBDisableKnoxSam();
			break;
		case Job.ADBDisableOSHU:
			DisableOTAhuawei();
			break;
		case (Job)34:
			Remove_Demo();
			break;
		case Job.RebootADB:
			RebootADB();
			break;
		case (Job)32:
			OppoSwithCarrier();
			break;
		case Job.RemoveMDMsam:
			RemoveMDM();
			break;
		case (Job)30:
			DisableOSONEplus();
			break;
		case Job.DisableOSTECNO:
			DisableOSTecno();
			break;
		case (Job)28:
			DisableOSItel();
			break;
		case Job.ErAasedemo:
			Removedemo();
			break;
		case (Job)26:
			DisabaleupdateRealme();
			break;
		case Job.ResetHuaweiID:
			ResetHuawei_ID();
			break;
		case (Job)24:
			RemoveSecurityPlugin();
			break;
		case Job.RemovePayjoy:
			ErasePayjoy();
			break;
		case Job.ResetLock:
			ResetLock();
			break;
		case (Job)22:
			DisabaleupdateVivo();
			break;
		case Job.DisableOSSamsung:
			DisabaleupdateSamsung();
			break;
		case (Job)20:
			DisabaleupdateXioami();
			break;
		case Job.DisabaOSOppo:
			DisabaleupdateOppo();
			break;
		case (Job)18:
			FixAssistantWIFI();
			break;
		case Job.Micloud:
			DidableMicloud();
			break;
		case (Job)16:
			OS16Operation();
			break;
		case (Job)64:
			EnableDiagRoot();
			break;
		case Job.DisablePengecualian:
			EnableVivoPengculian();
			break;
		case Job.DiagEnable:
			EnableDiagOperation();
			break;
		case Job.ID:
			SamsungIDOperation();
			break;
		case Job.Frp:
			BypassFRPOperation();
			break;
		}
		return (byte)result != 0;
	}

	private bool Initializing()
	{
		string msg = "Initializing protocol.... ";
		int result = 0;
		string msg2 = "Failed!";
		int num = 1;
		string msg3 = "adb.exe not found!";
		string msg4 = "OK";
		string msg5 = "Waiting ADB Connection... ";
		object obj = null;
		string msg6 = "Stopped";
		double field_ = Class38.field_2144;
		string msg7 = "No device found!";
		string text = "";
		string msg8 = "Connecting to ";
		string text2 = "... ";
		string msg9 = "[Device Information]";
		string key = "ro.product.device";
		string msg10 = "Product : ";
		string key2 = "ro.product.model";
		string msg11 = "Model : ";
		string key3 = "ro.product.brand";
		string msg12 = "Brand : ";
		string key4 = "ro.build.version.security_patch";
		string msg13 = "Security patch : ";
		string key5 = "ro.build.version.release";
		string msg14 = "Android version : ";
		string key6 = "persist.sys.usb.config";
		string msg15 = "USB Config : ";
		string key7 = "ro.build.PDA";
		string msg16 = "AP Version : ";
		string key8 = "ro.boot.bootloader";
		string msg17 = "BL Version : ";
		string key9 = "gsm.version.baseband";
		string msg18 = "CP Version : ";
		string key10 = "ro.csc.sales_code";
		string msg19 = "CSC Version : ";
		string key11 = "knox.kg.state";
		string msg20 = "KG State : ";
		string key12 = "ro.config.knox";
		string msg21 = "Knox Version : ";
		string key13 = "ro.boot.hardware";
		string msg22 = "CPU : ";
		string key14 = "ro.boot.cpuid";
		string key15 = "ro.boot.chipid";
		string key16 = "sys.boot.cpuid";
		string msg23 = "CPUID : ";
		string key17 = "ro.product.cpu.abi";
		string msg24 = "CPU Architecture : ";
		string value = "not found";
		string msg25 = "Device Unplugged";
		string msg26 = "Please check USB Connection";
		string msg27 = "Please allow usb debugging";
		UI.RichLogs(msg, UI.log);
		if (!ADB.Check() && !ADB.StartServer())
		{
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			UI.RichLogs(msg3, UI.error, nextline: true, bold: true);
			return false;
		}
		UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg5, UI.log);
		AdbDevice[] array = (AdbDevice[])obj;
		Stopwatch stopwatch = Stopwatch.StartNew();
		while (stopwatch.Elapsed < TimeSpan.FromMinutes(field_))
		{
			array = ADB.GetDevices();
			if (array.Length != 0)
			{
				break;
			}
			for (int i = 0; i < 10; i++)
			{
				if (!JobStatus.Token.IsCancellationRequested)
				{
					Thread.Sleep(100);
					continue;
				}
				UI.RichLogs(msg6, UI.error, nextline: true, bold: true);
				return false;
			}
		}
		if (array != null && array.Length != 0)
		{
			AdbDevice[] array2 = array;
			foreach (AdbDevice adbDevice in array2)
			{
				UI.RichLogs(adbDevice.SerialNumber.ToUpper() ?? text, UI.info, nextline: true, bold: true);
				ADB.SetDevice(adbDevice.SerialNumber);
				UI.RichLogs(msg8, UI.log);
				UI.RichLogs(adbDevice.SerialNumber.ToUpper() + text2, UI.info, nextline: false, bold: true);
			}
			try
			{
				Dictionary<string, string> deviceProperties = ADB.GetDeviceProperties();
				UI.RichLogs(msg4, UI.ok, nextline: true, bold: true);
				UI.RichLogs(text, UI.log, nextline: true);
				UI.RichLogs(msg9, UI.log, nextline: true);
				if (deviceProperties.TryGetValue(key, out var value2) && !string.IsNullOrEmpty(value2))
				{
					UI.RichLogs(msg10, UI.log);
					UI.RichLogs(value2, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key2, out var value3) && !string.IsNullOrEmpty(value3))
				{
					UI.RichLogs(msg11, UI.log);
					UI.RichLogs(value3, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key3, out var value4) && !string.IsNullOrEmpty(value4))
				{
					UI.RichLogs(msg12, UI.log);
					UI.RichLogs(value4, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key4, out var value5) && !string.IsNullOrEmpty(value5))
				{
					UI.RichLogs(msg13, UI.log);
					UI.RichLogs(value5, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key5, out var value6) && !string.IsNullOrEmpty(value6))
				{
					UI.RichLogs(msg14, UI.log);
					UI.RichLogs(value6, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key6, out var value7) && !string.IsNullOrEmpty(value7))
				{
					UI.RichLogs(msg15, UI.log);
					UI.RichLogs(value7, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key7, out var value8) && !string.IsNullOrEmpty(value8))
				{
					UI.RichLogs(msg16, UI.log);
					UI.RichLogs(value8, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key8, out var value9) && !string.IsNullOrEmpty(value9))
				{
					UI.RichLogs(msg17, UI.log);
					UI.RichLogs(value9, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key9, out var value10) && !string.IsNullOrEmpty(value10))
				{
					UI.RichLogs(msg18, UI.log);
					UI.RichLogs(value10, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key10, out var value11) && !string.IsNullOrEmpty(value11))
				{
					UI.RichLogs(msg19, UI.log);
					UI.RichLogs(value11, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key11, out var value12) && !string.IsNullOrEmpty(value12))
				{
					UI.RichLogs(msg20, UI.log);
					UI.RichLogs(value12, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key12, out var value13) && !string.IsNullOrEmpty(value13))
				{
					UI.RichLogs(msg21, UI.log);
					UI.RichLogs(value13, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key13, out var value14) && !string.IsNullOrEmpty(value14))
				{
					UI.RichLogs(msg22, UI.log);
					UI.RichLogs(value14.ToUpper(), UI.info, nextline: true, bold: true);
				}
				if ((deviceProperties.TryGetValue(key14, out var value15) && !string.IsNullOrEmpty(value15)) || (deviceProperties.TryGetValue(key15, out value15) && !string.IsNullOrEmpty(value15)) || (deviceProperties.TryGetValue(key16, out value15) && !string.IsNullOrEmpty(value15)))
				{
					UI.RichLogs(msg23, UI.log);
					UI.RichLogs(value15, UI.info, nextline: true, bold: true);
				}
				if (deviceProperties.TryGetValue(key17, out var value16) && !string.IsNullOrEmpty(value16))
				{
					UI.RichLogs(msg24, UI.log);
					UI.RichLogs(value16, UI.info, nextline: true, bold: true);
				}
			}
			catch (AdbException ex)
			{
				UI.RichLogs(msg2, UI.error, (byte)num != 0, (byte)num != 0);
				if (!ex.Message.ToLower().Contains(value))
				{
					UI.RichLogs(msg27, UI.error, (byte)num != 0, (byte)num != 0);
				}
				else
				{
					UI.RichLogs(msg25, UI.error, (byte)num != 0, (byte)num != 0);
					UI.RichLogs(msg26, UI.error, (byte)num != 0, (byte)num != 0);
				}
				return (byte)result != 0;
			}
			return (byte)num != 0;
		}
		UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
		UI.RichLogs(msg7, UI.error, nextline: true, bold: true);
		return false;
	}

	private void BypassFRPOperation()
	{
		string text = "settings put secure user_setup_complete 1";
		string text2 = "settings put system user_setup_complete 1";
		string text3 = "settings put global user_setup_complete 1";
		string text4 = "settings put system OPTION_SCREEN_LOCK 1";
		string text5 = "settings put system strong_protection 0";
		string text6 = "settings put secure secure_frp_mode 0";
		string text7 = "settings put system secure_frp_mode 0";
		string text8 = "settings put global secure_frp_mode 0";
		string text9 = "settings put secure device_provisioned 1";
		string text10 = "settings put system device_provisioned 1";
		string text11 = "settings put global device_provisioned 1";
		string text12 = "pm disable-user --user 0 com.google.android.setupwizard";
		string text13 = "pm clear com.sec.android.app.SecSetupWizard";
		string text14 = "pm uninstall --user 0 com.sec.android.app.SecSetupWizard";
		string text15 = "pm clear com.android.app.SetupWizard";
		string text16 = "pm clear --user 0 com.sec.android.app.SecSetupWizard";
		string text17 = "pm clear --user 0 com.sec.android.app.setupwizardlegalprovidershell";
		string text18 = "pm clear --user 0 com.google.android.setupwizard";
		string text19 = "pm clear com.google.android.setupwizard";
		string text20 = "content insert --uri content://settings/secure --bind name:s:user_setup_complete --bind value:s:1";
		string text21 = "content insert --uri content://settings/secure --bind name:s:DEVICE_PROVISIONED --bind value:i:1";
		string text22 = "content insert --uri content://settings/secure --bind name:s:user_setup_complete --bind value:i:1";
		string text23 = "content insert --uri content://settings/secure --bind name:s:INSTALL_NON_MARKET_APPS --bind value:i:1";
		string text24 = "am start -c android.intent.category.HOME -a android.intent.action.MAIN";
		string text25 = "am start -n com.android.settings/com.android.settings.Settings";
		string msg = "Bypass FRP lock... ";
		string msg2 = "OK";
		string msg3 = "Rebooting... ";
		string command = "reboot";
		string[] array = new string[25]
		{
			text, text2, text3, text4, text5, text6, text7, text8, text9, text10,
			text11, text12, text13, text14, text15, text16, text17, text18, text19, text20,
			text21, text22, text23, text24, text25
		};
		UI.RichLogs(msg, UI.log);
		string[] array2 = array;
		foreach (string command2 in array2)
		{
			Thread.Sleep(100);
			ADB.ExecuteRemoteCommand(command2);
		}
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg3, UI.log);
		ADB.ExecuteRemoteCommand(command);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void SamsungIDOperation()
	{
		string msg = "Remove Samsung Account... ";
		string command = "pm clear com.samsung.android.mobileservice";
		string command2 = "pm clear com.samsung.android.scloud";
		string command3 = "pm clear com.osp.app.signin";
		string msg2 = "OK";
		string msg3 = "Rebooting... ";
		string command4 = "reboot";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg3, UI.log);
		ADB.ExecuteRemoteCommand(command4);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DidableMicloud()
	{
		string msg = "Disable micloud... ";
		string command = "am set-debug-app -w --persistent com.xiaomi.finddevice";
		string command2 = "hide com.miui.cloudservice=pm hide com.xiaomi.finddevice";
		string command3 = "am settings get --user 0 global wait_for_debugge";
		string command4 = "pm hide com.xiaomi.cloudservice";
		string command5 = "settings get --user 0 global debug_app";
		string command6 = "am clear-debug-app com.xiaomi.IndexOfdevice";
		string command7 = "asettings get --user 0 global wait_for_debugger";
		string command8 = "pm hide wait_for_debugger";
		string msg2 = "OK";
		string msg3 = "Rebooting... ";
		string command9 = "reboot";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		ADB.ExecuteRemoteCommand(command7);
		ADB.ExecuteRemoteCommand(command8);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg3, UI.log);
		ADB.ExecuteRemoteCommand(command9);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void OS16Operation()
	{
		string msg = "Bypass FRP lock... ";
		string cmd = "push diagpath /data/local/tmp/Alpha.apk";
		string command = "am startservice com.shell/.Server >/dev/null; sleep 3; (echo \"dd if=/dev/zero of=/dev/block/by-name/persistent bs=4096 count=256 conv=fsync\"; echo \"dd if=/dev/zero of=/dev/block/by-name/frp bs=4096 count=256 conv=fsync\"; echo \"dd if=/dev/zero of=/dev/block/by-name/config bs=4096 count=256 conv=fsync\"; echo \"dd if=/dev/zero of=/dev/block/by-name/metadata bs=4096 count=256 conv=fsync\"; echo \"exit\") | nc -lp 8080";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ADBConsole(cmd);
		ADB.ExecuteRemoteCommand(command);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void EnablediagXiaomiOperation()
	{
		string msg = "Checking resources... ";
		string command = "ls /data/local/tmp/Alpha.apk";
		string text = "/data/local/tmp/Alpha.apk";
		string msg2 = "Not Ready!";
		string msg3 = "Downloading Resources... ";
		string path = "bin";
		string path2 = "Diag.apk";
		string msg4 = "Failed!";
		string msg5 = "OK";
		string msg6 = "Initializing Resources... ";
		string text2 = "push ";
		string text3 = " /data/local/tmp/Alpha.apk";
		string command2 = "pm install /data/local/tmp/Alpha.apk";
		string msg7 = "Enabling Diag... ";
		string command3 = "am start -n com.longcheertel.midtest/";
		string command4 = "am start -n com.longcheertel.midtest/com.longcheertel.midtest.Diag";
		UI.RichLogs(msg, UI.log);
		string[] array = ADB.ExecuteRemoteCommand(command);
		if (array.Length != 0 && !(array[0].Trim() != text))
		{
			UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
			UI.RichLogs(msg7, UI.log);
			ADB.ExecuteRemoteCommand(command3);
			ADB.ExecuteRemoteCommand(command4);
			UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
			return;
		}
		UI.RichLogs(msg2, UI.warning, nextline: true);
		UI.RichLogs(msg3, UI.log);
		string text4 = Path.Combine(utils.BaseDirectory, path, path2);
		if (!File.Exists(text4))
		{
			UI.RichLogs(msg4, UI.error, nextline: true, bold: true);
			return;
		}
		UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg6, UI.log);
		ADB.ADBConsole(text2 + text4 + text3);
		ADB.ExecuteRemoteCommand(command2);
		UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
		UI.RichLogs(msg7, UI.log);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		UI.RichLogs(msg5, UI.ok, nextline: true, bold: true);
	}

	private void EnableDiagRoot()
	{
		string msg = "Enabling Diag... ";
		string command = "am start -n com.longcheertel.midtest/";
		string command2 = "am start -n com.longcheertel.midtest/com.longcheertel.midtest.Diag";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void EnableDiagOperation()
	{
		string msg = "Enabling Diag... ";
		string command = "am start -n com.longcheertel.midtest/";
		string command2 = "am start -n com.longcheertel.midtest/com.longcheertel.midtest.Diag";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void EnableVivoPengculian()
	{
		string msg = "Disable Pengecualian... ";
		string command = "pm uninstall --user 0  com.vivo.aiservice";
		string command2 = "pm disable-user --user 0 com.vivo.secime.service";
		string command3 = "pm uninstall -k --user 0 com.vivo.daemonService";
		string command4 = "pm uninstall --user 0 com.vivo.demovideo";
		string command5 = "pm uninstall --user 0 com.vivo.daemonService";
		string command6 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void FixAssistantWIFI()
	{
		string msg = "Fix Wireless Test Assistant... ";
		string command = "dumpsys engineer --execute_power_off";
		string command2 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DisabaleupdateOppo()
	{
		string msg = "Disable OS ota Oppo... ";
		string command = "pm disable-user --user 0 com.oplus.ota";
		string command2 = "pm uninstall -k --user 0 com.oplus.ota";
		string command3 = "pm uninstall --user 0 com.oppo.ota";
		string command4 = "pm uninstall --user 0 com.oppo.otaui";
		string command5 = "pm uninstall --user 0 com.coloros.sauhelper";
		string command6 = "pm uninstall --user 0 com.coloros.sau";
		string command7 = "pm uninstall --user 0 com.android.updater";
		string command8 = "pm uninstall --user 0 com.oplus.ota";
		string command9 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		ADB.ExecuteRemoteCommand(command7);
		ADB.ExecuteRemoteCommand(command8);
		ADB.ExecuteRemoteCommand(command9);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DisabaleupdateXioami()
	{
		string msg = "Disable OS ota Xioami... ";
		string command = "pm uninstall -k --user 0 com.android.updater";
		string command2 = "am set-debug-app -w --persistent com.android.updater";
		string command3 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DisabaleupdateSamsung()
	{
		string msg = "Disable OS ota Samsung... ";
		string command = "pm uninstall --user 0 com.wssyncmldm##pm uninstall --user 0 com.sec.android.soagent#pm disable-ser com.samsung.ssu";
		string command2 = "pm disable-ser com.samsung.android.app.clockpackage##pm disable-ser com.sec.enterprise.knox.cloudmdm.smdms";
		string command3 = "pm disable-ser com.sec.android.app.setupwizard##pm disable-ser com.samsung.android.fmm";
		string command4 = "pm disable-ser com.sec.android.app.SecSetupWizard##pm disable-user com.sec.android.app.samsungapps";
		string command5 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DisabaleupdateVivo()
	{
		string msg = "Disable OS ota Vivo... ";
		string command = "pm disable-user --user 0 com.bbk.updater";
		string command2 = "pm uninstall -k --user 0 com.bbk.updater";
		string command3 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void ResetLock()
	{
		string msg = "Reset Lock... ";
		string command = "mount - o remount, rw /data/system";
		string command2 = "rm -f /data/system/gesture.key";
		string command3 = "rm -f /data/system/signature.key";
		string command4 = "rm -f /data/system/personal.key";
		string command5 = "rm -f /data/system/password.key";
		string command6 = "rm -f /data/system/fmmpassword.key";
		string command7 = "rm -f /data/system/Sparepassword.key";
		string command8 = "rm -f /data/system/gatekeeper.backuppin.key";
		string command9 = "rm -f /data/system/gatekeeper.password.key";
		string command10 = "rm -f /data/system/gatekeeper.pattern.key";
		string command11 = "rm -f /data/system/fingerprintpassword.key";
		string command12 = "rm -f /data/system/locksettings.db";
		string command13 = "rm -f /data/system/locksettings.db-wal";
		string command14 = "rm -f /data/system/locksettings.db-shm";
		string command15 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		ADB.ExecuteRemoteCommand(command7);
		ADB.ExecuteRemoteCommand(command8);
		ADB.ExecuteRemoteCommand(command9);
		ADB.ExecuteRemoteCommand(command10);
		ADB.ExecuteRemoteCommand(command11);
		ADB.ExecuteRemoteCommand(command12);
		ADB.ExecuteRemoteCommand(command13);
		ADB.ExecuteRemoteCommand(command14);
		ADB.ExecuteRemoteCommand(command15);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void ErasePayjoy()
	{
		string msg = "Remove Payjoy... ";
		string command = "settings put global setup_wizard_has_run 1";
		string command2 = "nsettings put secure setup_wizard_has_run 1";
		string command3 = "settings put global user_setup_complete 1";
		string command4 = "settings put secure user_setup_complete 1";
		string command5 = "settings put global device_provisioned 1";
		string command6 = "settings put secure device_provisioned 1";
		string command7 = "content insert --uri content://settings/secure --bind name:s:DEVICE_PROVISIONED --bind value:i:1";
		string command8 = "content insert --uri content://settings/secure --bind name:s:user_setup_complete --bind value:i:1";
		string command9 = "content insert --uri content://settings/secure --bind name:s:INSTALL_NON_MARKET_APPS --bind value:i:1";
		string command10 = "dd if=/dev/zero of=/dev/block/by-name/proinfo bs=1 count=72 seek=2752512 conv=notrunc";
		string command11 = "pm disable-user --user 0 com.payjoy.access";
		string command12 = "pm uninstall -k --user 0 com.payjoy.access";
		string command13 = "pm uninstall com.payjoy.access";
		string command14 = "pm uninstall --user 0 com.payjoy.access";
		string command15 = "pm disable-user --user 0 com.payjoy.permission";
		string command16 = "pm uninstall -k --user 0 com.payjoy.permission";
		string command17 = "pm uninstall com.payjoy.permission";
		string command18 = "pm uninstall --user 0 com.payjoy.permission";
		string command19 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		ADB.ExecuteRemoteCommand(command7);
		ADB.ExecuteRemoteCommand(command8);
		ADB.ExecuteRemoteCommand(command9);
		ADB.ExecuteRemoteCommand(command10);
		ADB.ExecuteRemoteCommand(command11);
		ADB.ExecuteRemoteCommand(command12);
		ADB.ExecuteRemoteCommand(command13);
		ADB.ExecuteRemoteCommand(command14);
		ADB.ExecuteRemoteCommand(command15);
		ADB.ExecuteRemoteCommand(command16);
		ADB.ExecuteRemoteCommand(command17);
		ADB.ExecuteRemoteCommand(command18);
		ADB.ExecuteRemoteCommand(command19);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void RemoveSecurityPlugin()
	{
		string msg = "Remove Security Plugin... ";
		string command = "settings put global setup_wizard_has_run 1";
		string command2 = "nsettings put secure setup_wizard_has_run 1";
		string command3 = "settings put global user_setup_complete 1";
		string command4 = "settings put secure user_setup_complete 1";
		string command5 = "settings put global device_provisioned 1";
		string command6 = "settings put secure device_provisioned 1";
		string command7 = "content insert --uri content://settings/secure --bind name:s:DEVICE_PROVISIONED --bind value:i:1";
		string command8 = "content insert --uri content://settings/secure --bind name:s:user_setup_complete --bind value:i:1";
		string command9 = "content insert --uri content://settings/secure --bind name:s:INSTALL_NON_MARKET_APPS --bind value:i:1";
		string command10 = "dd if=/dev/zero of=/dev/block/by-name/proinfo bs=1 count=72 seek=2752512 conv=notrunc";
		string command11 = "pm disable-user --user 0 com.scorpio.securitycom";
		string command12 = "pm uninstall com.scorpio.securitycom";
		string command13 = "pm uninstall --user 0 com.scorpio.securitycom";
		string command14 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		ADB.ExecuteRemoteCommand(command7);
		ADB.ExecuteRemoteCommand(command8);
		ADB.ExecuteRemoteCommand(command9);
		ADB.ExecuteRemoteCommand(command10);
		ADB.ExecuteRemoteCommand(command11);
		ADB.ExecuteRemoteCommand(command12);
		ADB.ExecuteRemoteCommand(command13);
		ADB.ExecuteRemoteCommand(command11);
		ADB.ExecuteRemoteCommand(command12);
		ADB.ExecuteRemoteCommand(command12);
		ADB.ExecuteRemoteCommand(command14);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void ResetHuawei_ID()
	{
		string msg = "Remove Huawei ID... ";
		string command = "pm uninstall -k --user 0 com.huawei.hwid";
		string command2 = "pm uninstall -k --user 0 com.huawei.hidisk";
		string command3 = "pm uninstall -k --user 0 com.huawei.hicloud";
		string command4 = "pm uninstall -k --user 0 com.huawei.hifolder";
		string command5 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DisabaleupdateRealme()
	{
		string msg = "Disable OS ota Realme... ";
		string command = "pm uninstall --user 0 com.oppo.ota";
		string command2 = "pm uninstall --user 0 com.oppo.otaui";
		string command3 = "pm uninstall --user 0 com.coloros.sauhelper";
		string command4 = "pm uninstall --user 0 com.coloros.sau";
		string command5 = "pm uninstall --user 0 com.oplus.ota";
		string command6 = "pm uninstall --user 0 com.google.android.configupdater";
		string command7 = "pm uninstall --user 0 com.android.dynsystem";
		string command8 = "pm uninstall --user 0 com.nearme.romupdate";
		string command9 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		ADB.ExecuteRemoteCommand(command7);
		ADB.ExecuteRemoteCommand(command8);
		ADB.ExecuteRemoteCommand(command9);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void Removedemo()
	{
		string msg = "Disable Demo Mode... ";
		string command = "pm uninstall --user 0 com.vivo.demovideo";
		string command2 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DisableOSItel()
	{
		string msg = "Disable OS ota Itel... ";
		string command = "pm uninstall --user 0 com.google.android.as";
		string command2 = "pm uninstall --user 0 com.transsion.notebook";
		string command3 = "pm uninstall --user 0 com.google.android.apps.turbo";
		string command4 = "pm uninstall --user 0 com.transsion.kolun.assistant";
		string command5 = "pm uninstall --user 0 com.android.providers.userdictionary";
		string command6 = "pm uninstall --user 0 com.google.android.apps.wellbeing";
		string command7 = "pm uninstall --user 0 com.transsion.plat.appupdate";
		string command8 = "pm uninstall --user 0 com.transsion.dtsaudio";
		string command9 = "pm uninstall --user 0 com.transsion.aivoiceassistant";
		string command10 = "pm uninstall --user 0 com.transsion.batterylab";
		string command11 = "pm uninstall --user 0 com.transsion.childmode";
		string command12 = "pm uninstall --user 0 com.google.android.adservices.api";
		string command13 = "pm uninstall --user 0 com.android.traceur";
		string command14 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		ADB.ExecuteRemoteCommand(command7);
		ADB.ExecuteRemoteCommand(command8);
		ADB.ExecuteRemoteCommand(command9);
		ADB.ExecuteRemoteCommand(command10);
		ADB.ExecuteRemoteCommand(command11);
		ADB.ExecuteRemoteCommand(command12);
		ADB.ExecuteRemoteCommand(command13);
		ADB.ExecuteRemoteCommand(command14);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DisableOSTecno()
	{
		string msg = "Disable OS ota Tecno... ";
		string command = "pm uninstall --user 0 com.google.android.as";
		string command2 = "pm uninstall --user 0 com.transsion.notebook";
		string command3 = "pm uninstall --user 0 com.google.android.apps.turbo";
		string command4 = "pm uninstall --user 0 com.transsion.kolun.assistant";
		string command5 = "pm uninstall --user 0 com.android.providers.userdictionary";
		string command6 = "pm uninstall --user 0 com.google.android.apps.wellbeing";
		string command7 = "pm uninstall --user 0 com.transsion.plat.appupdate";
		string command8 = "pm uninstall --user 0 com.transsion.dtsaudio";
		string command9 = "pm uninstall --user 0 com.transsion.aivoiceassistant";
		string command10 = "pm uninstall --user 0 com.transsion.batterylab";
		string command11 = "pm uninstall --user 0 com.transsion.childmode";
		string command12 = "pm uninstall --user 0 com.google.android.adservices.api";
		string command13 = "pm uninstall --user 0 com.android.traceur";
		string command14 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		ADB.ExecuteRemoteCommand(command7);
		ADB.ExecuteRemoteCommand(command8);
		ADB.ExecuteRemoteCommand(command9);
		ADB.ExecuteRemoteCommand(command10);
		ADB.ExecuteRemoteCommand(command11);
		ADB.ExecuteRemoteCommand(command12);
		ADB.ExecuteRemoteCommand(command13);
		ADB.ExecuteRemoteCommand(command14);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DisableOSONEplus()
	{
		string msg = "Disable OS ota One Plus... ";
		string command = "pm uninstall --user 0 com.oppo.ota";
		string command2 = "pm uninstall --user 0 com.oppo.otaui";
		string command3 = "pm uninstall --user 0 com.coloros.sauhelper";
		string command4 = "pm uninstall --user 0 com.coloros.sau";
		string command5 = "pm uninstall --user 0 com.oplus.ota";
		string command6 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void RemoveMDM()
	{
		string msg = "Remove MDM (O-Guard)[Wifi-data]... ";
		string command = "$\"push \\Data\\bin\\MDM64.bin /data/local/tmp/\"";
		string command2 = "chmod +x /data/local/tmp/";
		string command3 = "/data/local/tmp/";
		string command4 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void OppoSwithCarrier()
	{
		string msg = "Oppo Swith Carrier Mode... ";
		string command = "getprop ro.product.manufacturer";
		string command2 = "am start -a android.intent.action.DIAL";
		string command3 = "am start -a android.intent.action.DIAL tel:%2A%233954391";
		string command4 = "input keyevent 18";
		string command5 = "am start -a android.intent.action.DIAL tel:%2A%23391";
		string command6 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command6);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void DisableOTAhuawei()
	{
		string msg = "[Huawei/Honor] Disable OTA Updates... ";
		string command = "pm disable-user com.huawei.android.hwouc";
		string command2 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void Remove_Demo()
	{
		string msg = "Vivo Remove Demo Mode... ";
		string command = "pm uninstall --user 0 com.vivo.demovideo";
		string command2 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void ADBSamsungAccount()
	{
		string msg = "Disable Samsung Account... ";
		string command = "pm clear com.samsung.android.mobileservice";
		string command2 = "pm clear com.samsung.android.scloud";
		string command3 = "pm clear com.osp.app.signin";
		string command4 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void ADBDisableKnoxSam()
	{
		string msg = "Disable Knox... ";
		string command = "pm uninstall --user 0 com.samsung.knox.rcp.components";
		string command2 = "pm uninstall --user 0 com.sec.knox.foldercontainer";
		string command3 = "pm uninstall --user 0 com.samsung.knox.securefolder";
		string command4 = "pm uninstall --user 0 com.sec.knox.knoxsetupwizardclient";
		string command5 = "pm uninstall --user 0 com.sec.enterprise.knox.cloudmdm.smdms";
		string command6 = "pm uninstall --user 0 com.sec.knox.switcher";
		string command7 = "pm uninstall --user 0 com.sec.knox.kccagent";
		string command8 = "pm uninstall --user 0 com.knox.vpn.proxyhandler";
		string command9 = "pm disable-user --user 0 com.sec.knox.kccagent";
		string command10 = "pm disable-user --user 0 com.sec.enterprise.knox.cloudmdm.smdms";
		string command11 = "pm uninstall --user 0 com.postmates.android.merchant";
		string command12 = "pm uninstall --user 0 com.samsung.android.knox.containercore";
		string command13 = "pm uninstall --user 0 com.sec.enterprise.knox.attestation";
		string command14 = "pm uninstall --user 0 com.samsung.android.knox.containeragent";
		string command15 = "pm clear --user 0 com.samsung.knox.rcp.components";
		string command16 = "pm clear --user 0 com.sec.knox.foldercontainer";
		string command17 = "pm clear --user 0 com.samsung.knox.securefolder";
		string command18 = "pm clear --user 0 com.sec.knox.knoxsetupwizardclient";
		string command19 = "pm clear --user 0 com.sec.enterprise.knox.cloudmdm.smdms";
		string command20 = "pm clear --user 0 com.sec.knox.switcher";
		string command21 = "pm clear --user 0 com.sec.knox.kccagent";
		string command22 = "pm clear --user 0 com.knox.vpn.proxyhandler";
		string command23 = "pm clear --user 0 com.postmates.android.merchant";
		string command24 = "am start -n com.google.android.gsf.login/";
		string command25 = "am start -n com.google.android.gsf.register.LoginActivity";
		string command26 = "content insert --uri content://settings/secure --bind name:s:user_setup_complete --bin";
		string command27 = "settings put secure user_setup_complete 1";
		string command28 = "am start -n com.sec.android.app.launcher/com.android.launcher2.Launcher";
		string command29 = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command3);
		ADB.ExecuteRemoteCommand(command4);
		ADB.ExecuteRemoteCommand(command5);
		ADB.ExecuteRemoteCommand(command6);
		ADB.ExecuteRemoteCommand(command7);
		ADB.ExecuteRemoteCommand(command8);
		ADB.ExecuteRemoteCommand(command9);
		ADB.ExecuteRemoteCommand(command10);
		ADB.ExecuteRemoteCommand(command2);
		ADB.ExecuteRemoteCommand(command11);
		ADB.ExecuteRemoteCommand(command12);
		ADB.ExecuteRemoteCommand(command13);
		ADB.ExecuteRemoteCommand(command14);
		ADB.ExecuteRemoteCommand(command8);
		ADB.ExecuteRemoteCommand(command15);
		ADB.ExecuteRemoteCommand(command16);
		ADB.ExecuteRemoteCommand(command17);
		ADB.ExecuteRemoteCommand(command18);
		ADB.ExecuteRemoteCommand(command19);
		ADB.ExecuteRemoteCommand(command20);
		ADB.ExecuteRemoteCommand(command21);
		ADB.ExecuteRemoteCommand(command22);
		ADB.ExecuteRemoteCommand(command23);
		ADB.ExecuteRemoteCommand(command24);
		ADB.ExecuteRemoteCommand(command25);
		ADB.ExecuteRemoteCommand(command26);
		ADB.ExecuteRemoteCommand(command24);
		ADB.ExecuteRemoteCommand(command25);
		ADB.ExecuteRemoteCommand(command26);
		ADB.ExecuteRemoteCommand(command27);
		ADB.ExecuteRemoteCommand(command28);
		ADB.ExecuteRemoteCommand(command29);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	private void RebootADB()
	{
		string msg = "Rebooting... ";
		string command = "reboot";
		string msg2 = "OK";
		UI.RichLogs(msg, UI.log);
		ADB.ExecuteRemoteCommand(command);
		UI.RichLogs(msg2, UI.ok, nextline: true, bold: true);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
