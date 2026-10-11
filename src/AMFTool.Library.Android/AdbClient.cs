#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Restored.Namespace14;

namespace AMFTool.Library.Android;

public class AdbClient
{
	public static Process adb;

	public static StreamWriter adb_in;

	public static StreamReader adb_out;

	public string ServerHost { get; private set; }

	public int ServerPort { get; private set; }

	public string DeviceSerialNumber { get; private set; }

	public AdbClient(int adbServerPort = -1, string adbServerHost = null)
	{
		string variable = "ANDROID_ADB_SERVER_PORT";
		string text = "127.0.0.1";

		if (adbServerPort < 0)
		{
			string environmentVariable = Environment.GetEnvironmentVariable(variable);
			int result = 0;
			int num;
			if (!string.IsNullOrEmpty(environmentVariable) && int.TryParse(environmentVariable, out result))
			{
				ServerPort = result;
				num = 1501861812;
			}
			else
			{
				ServerPort = 5037;
				num = 1501861812;
			}
			_ = num ^ -724896745;
			ServerHost = (string.IsNullOrEmpty(adbServerHost) ? text : adbServerHost);
		}
		else
		{
			ServerPort = adbServerPort;
			ServerHost = (string.IsNullOrEmpty(adbServerHost) ? text : adbServerHost);
		}
	}

	public bool Check()
	{
		string hostname = "127.0.0.1";
		int result = 0;
		try
		{
			using (new TcpClient(hostname, 5037))
			{
				return true;
			}
		}
		catch (SocketException)
		{
			return (byte)result != 0;
		}
	}

	public bool StartServer()
	{
		string path = "Data";
		string path2 = "bin";
		string path3 = "adb.exe";
		string arguments = "start-server";
		int result = 0;
		try
		{
			Process process = new Process();
			ProcessStartInfo startInfo = process.StartInfo;
			startInfo.FileName = Path.Combine(AppContext.BaseDirectory, path, path2, path3);
			startInfo.Arguments = arguments;
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow = true;
			startInfo.RedirectStandardInput = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardError = true;
			startInfo.StandardOutputEncoding = Encoding.ASCII;
			process.Start();
			StreamReader standardOutput = process.StandardOutput;
			while (!process.StandardOutput.EndOfStream)
			{
				standardOutput.ReadLine();
			}
			process.Dispose();
			return true;
		}
		catch (Exception)
		{
			return (byte)result != 0;
		}
	}

	public string ADBConsole(string cmd)
	{
		string path = "Data";
		string path2 = "bin";
		string path3 = "adb.exe";
		object result = null;
		string value = "Send Command Success";
		string text = "Command Failed: ";
		string fileName = Path.Combine(Application.StartupPath, path, path2, path3);
		try
		{
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = fileName,
				Arguments = cmd,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};
			using Process process = new Process
			{
				StartInfo = startInfo
			};
			process.Start();
			string result2 = process.StandardOutput.ReadToEnd();
			string value2 = process.StandardError.ReadToEnd();
			process.WaitForExit();
			if (string.IsNullOrEmpty(value2))
			{
				Console.WriteLine(value);
				return result2;
			}
			Console.WriteLine(value2);
			return (string)result;
		}
		catch (Exception ex)
		{
			Console.WriteLine(text + ex.Message);
			return (string)result;
		}
	}

	public void StopServer()
	{
		string command = "host:kill";
		using AdbSocket adbSocket = GetSocket();
		adbSocket.SendCommand(command);
	}

	public int GetServerVersion()
	{
		string command = "host:version";
		using AdbSocket adbSocket = GetSocket();
		adbSocket.SendCommand(command);
		return adbSocket.ReadInt32Hex();
	}

	public AdbDevice[] GetDevices()
	{
		string text = "";
		string command = "host:devices-l";
		int num = 2;
		int num2 = 0;
		int num3 = 13;
		int num4 = 1;
		int num5 = 10;
		int num6 = 32;
		int num7 = 58;
		string text2 = "model";
		string text3 = "device";
		string text4 = "product";
		string text5 = text;
		using (AdbSocket adbSocket = GetSocket())
		{
			adbSocket.SendCommand(command);
			text5 = adbSocket.ReadHexString();
		}
		string text6 = text5;
		char[] array = new char[num];
		array[num2] = (char)num3;
		array[num4] = (char)num5;
		string[] array2 = text6.Split(array, (StringSplitOptions)num4);
		List<AdbDevice> list = new List<AdbDevice>(array2.Length);
		string[] array3 = array2;
		string[] array4 = array3;
		for (int i = num2; i < array4.Length; i += num4)
		{
			string text7 = array4[i];
			char[] array5 = new char[num4];
			array5[num2] = (char)num6;
			string[] array6 = text7.Split(array5, (StringSplitOptions)num4);
			string product = text;
			string model = text;
			string device = text;
			for (int j = num; j < array6.Length; j += num4)
			{
				string obj = array6[j];
				char[] array7 = new char[num4];
				array7[num2] = (char)num7;
				string[] array8 = obj.Split(array7, (StringSplitOptions)num4);
				if (num != array8.Length)
				{
					continue;
				}
				string text8 = array8[num2];
				string text9 = text8;
				if (!(text9 == text2))
				{
					if (text9 == text3)
					{
						device = array8[num4];
					}
					else if (text9 == text4)
					{
						product = array8[num4];
					}
				}
				else
				{
					model = array8[num4];
				}
			}
			list.Add(new AdbDevice(array6[num2], product, model, device));
		}
		return list.ToArray();
	}

	public void InstallFromBytes(byte[] data, bool installOnSdCard)
	{
		string text = "/storage/emulated/0/in.apk";
		string text2 = "/sdcard/tmp/in.apk";
		string text3 = "";
		string text4 = "-s";
		string text5 = "pm install ";
		string text6 = " ";
		string text7 = (installOnSdCard ? text2 : text);
		if (!installOnSdCard)
		{
			string text8 = text3;
			if (GetFileInfo(text7).IsFile)
			{
				DeleteFile(text7);
				UploadData(data, text7);
				ExecuteRemoteCommand(text5 + text8 + text6 + text7);
				DeleteFile(text7);
			}
			else
			{
				UploadData(data, text7);
				ExecuteRemoteCommand(text5 + text8 + text6 + text7);
				DeleteFile(text7);
			}
		}
		else
		{
			string text8 = text4;
			if (GetFileInfo(text7).IsFile)
			{
				DeleteFile(text7);
				UploadData(data, text7);
				ExecuteRemoteCommand(text5 + text8 + text6 + text7);
				DeleteFile(text7);
			}
			else
			{
				UploadData(data, text7);
				ExecuteRemoteCommand(text5 + text8 + text6 + text7);
				DeleteFile(text7);
			}
		}
	}

	public void UploadData(byte[] data, string remoteFileName, int remoteFilePermissions = 1638)
	{
		string command = "SEND";
		string format = "{0},{1}";
		int num = 0;
		string text = "DATA";
		string text2 = "DONE";
		int length = 4;
		string value = "OKAY";
		using AdbSocket adbSocket = GetSocket();
		string text3 = SendSyncCommand(adbSocket, command, string.Format(format, remoteFileName, remoteFilePermissions), readResponse: false);
		int num2 = data.Length;
		using (MemoryStream memoryStream = new MemoryStream(data))
		{
			byte[] array = new byte[65536];
			while (num2 > 0)
			{
				int num3 = ((num2 >= array.Length) ? array.Length : num2);
				memoryStream.Read(array, 0, num3);
				adbSocket.WriteString(text);
				adbSocket.WriteInt32(num3);
				adbSocket.Write(array, num3);
				num2 -= num3;
			}
		}
		adbSocket.WriteString(text2);
		adbSocket.WriteInt32(AdbHelpers.ToUnixTime(DateTime.Now));
		text3 = adbSocket.ReadString(length);
		if ((text3.Equals(value) ? 1 : 0) == num)
		{
			throw new AdbInvalidResponseException(text3);
		}
	}

	public void PushCustomFile(string localFileName, string remoteFileName, int FilePermissions)
	{
		string command = "SEND";
		string format = "{0},{1}";
		int num = 0;
		string text = "DATA";
		string text2 = "DONE";
		int length = 4;
		string value = "OKAY";
		AdbFileInfo fileInfo = GetFileInfo(remoteFileName);
		if (fileInfo.IsDirectory || fileInfo.IsSymbolicLink)
		{
			remoteFileName = AdbHelpers.CombinePath(remoteFileName, Path.GetFileName(localFileName));
		}
		using AdbSocket adbSocket = GetSocket();
		string text3 = SendSyncCommand(adbSocket, command, string.Format(format, remoteFileName, FilePermissions), readResponse: false);
		FileInfo fileInfo2 = new FileInfo(localFileName);
		int num2 = (int)fileInfo2.Length;
		using (FileStream fileStream = File.OpenRead(localFileName))
		{
			byte[] array = new byte[65536];
			while (num2 > 0)
			{
				int num3 = ((num2 >= array.Length) ? array.Length : num2);
				fileStream.Read(array, 0, num3);
				adbSocket.WriteString(text);
				adbSocket.WriteInt32(num3);
				adbSocket.Write(array, num3);
				num2 -= num3;
			}
		}
		adbSocket.WriteString(text2);
		adbSocket.WriteInt32(AdbHelpers.ToUnixTime(fileInfo2.LastWriteTime));
		text3 = adbSocket.ReadString(length);
		if ((text3.Equals(value) ? 1 : 0) == num)
		{
			throw new AdbInvalidResponseException(text3);
		}
	}

	public void PushCustomData(byte[] data, string remoteFileName, int filepermition)
	{
		string command = "SEND";
		string format = "{0},{1}";
		int num = 0;
		string text = "DATA";
		string fileName = "Qualcom Flash.exe";
		string text2 = "DONE";
		int length = 4;
		string value = "OKAY";
		using AdbSocket adbSocket = GetSocket();
		string text3 = SendSyncCommand(adbSocket, command, string.Format(format, remoteFileName, filepermition), readResponse: false);
		int num2 = data.Length;
		using (MemoryStream memoryStream = new MemoryStream(data))
		{
			byte[] array = new byte[65536];
			while (num2 > 0)
			{
				int num3 = ((num2 >= array.Length) ? array.Length : num2);
				memoryStream.Read(array, 0, num3);
				adbSocket.WriteString(text);
				adbSocket.WriteInt32(num3);
				adbSocket.Write(array, num3);
				num2 -= num3;
			}
		}
		FileInfo fileInfo = new FileInfo(fileName);
		adbSocket.WriteString(text2);
		adbSocket.WriteInt32(AdbHelpers.ToUnixTime(fileInfo.LastWriteTime));
		text3 = adbSocket.ReadString(length);
		if ((text3.Equals(value) ? 1 : 0) == num)
		{
			throw new AdbInvalidResponseException(text3);
		}
	}

	public void SetDevice(string serialNumber)
	{
		DeviceSerialNumber = serialNumber;
	}

	public Dictionary<string, string> GetDeviceProperties()
	{
		string command = "/system/bin/getprop";
		string pattern = "\\[(.*)]: \\[(.*)]";
		string text = "Invalid prop line: '";
		string text2 = "'";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array = ExecuteRemoteCommand(command);
		string[] array2 = array;
		string[] array3 = array2;
		foreach (string text3 in array3)
		{
			Match match = Regex.Match(text3, pattern);
			if (!match.Success || match.Groups.Count != 3)
			{
				Trace.WriteLine(text + text3 + text2);
			}
			else
			{
				dictionary.Add(match.Groups[1].Value, match.Groups[2].Value);
			}
		}
		return dictionary;
	}

	public string[] ExecuteRemoteCommand(string command)
	{
		string text = "Executing '";
		string text2 = "' remote command";
		string text3 = "shell:";
		string format = "{0} response lines read";
		using AdbSocket adbSocket = GetSocket();
		SetDevice(adbSocket);
		Trace.WriteLine(text + command + text2);
		adbSocket.SendCommand(text3 + command);
		string[] array = adbSocket.ReadAllLines();
		Trace.WriteLine(string.Format(format, array.Length));
		return array;
	}

	public string[] ExecuteSideload(string command)
	{
		string text = "Executing '";
		string text2 = "' remote command";
		string format = "{0} response lines read";
		using AdbSocket adbSocket = GetSocket();
		SetDevice(adbSocket);
		Class37.method_01(text + command + text2);
		adbSocket.SendCommand(command);
		string[] array = adbSocket.ReadAllLines();
		Class37.method_01(string.Format(format, array.Length));
		return array;
	}

	public AdbFileInfo GetFileInfo(string fileName)
	{
		string text = "STAT";
		object directoryName = null;
		using AdbSocket adbSocket = GetSocket();
		string text2 = SendSyncCommand(adbSocket, text, fileName);
		if (!text2.Equals(text))
		{
			throw new AdbInvalidResponseException(text2);
		}
		return GetFileInfo(adbSocket, fileName, (string)directoryName);
	}

	public AdbFileInfo[] GetDirectoryListing(string directoryName)
	{
		string command = "LIST";
		string value = "DENT";
		object obj = null;
		string value2 = "DONE";
		using AdbSocket adbSocket = GetSocket();
		string text = SendSyncCommand(adbSocket, command, directoryName);
		List<AdbFileInfo> list = new List<AdbFileInfo>();
		bool flag = false;
		while (true)
		{
			if (text.Equals(value2))
			{
				if (!flag)
				{
					throw new DirectoryNotFoundException();
				}
				break;
			}
			if (text.Equals(value))
			{
				AdbFileInfo fileInfo = GetFileInfo(adbSocket, (string)obj, directoryName);
				if (fileInfo != obj)
				{
					list.Add(fileInfo);
				}
				else
				{
					flag = true;
				}
				text = adbSocket.ReadString(4);
				continue;
			}
			throw new AdbInvalidResponseException(text);
		}
		return list.ToArray();
	}

	private AdbFileInfo GetFileInfo(AdbSocket adbSocket, string fullName, string directoryName)
	{
		string text = "";
		string value = ".";
		string value2 = "..";
		object result = null;
		int mode = adbSocket.ReadInt32();
		int size = adbSocket.ReadInt32();
		DateTime modified = AdbHelpers.FromUnixTime(adbSocket.ReadInt32());
		string text2 = text;
		if (!string.IsNullOrEmpty(fullName))
		{
			text2 = Path.GetFileName(fullName);
			return new AdbFileInfo(fullName, text2, size, mode, modified);
		}
		text2 = adbSocket.ReadSyncString();
		if (!text2.Equals(value) && !text2.Equals(value2))
		{
			fullName = AdbHelpers.CombinePath(directoryName, text2);
			return new AdbFileInfo(fullName, text2, size, mode, modified);
		}
		return (AdbFileInfo)result;
	}

	public void DownloadFile(string remoteFileName, string localFileName)
	{
		string command = "RECV";
		string value = "DONE";
		string value2 = "DATA";
		using AdbSocket adbSocket = GetSocket();
		string text = SendSyncCommand(adbSocket, command, remoteFileName);
		int num = 0;
		using FileStream fileStream = File.Open(localFileName, FileMode.Create);
		byte[] array = new byte[65536];
		while (!text.Equals(value))
		{
			if (!text.Equals(value2))
			{
				throw new AdbInvalidResponseException(text);
			}
			int num2 = adbSocket.ReadInt32();
			adbSocket.Read(array, num2);
			fileStream.Write(array, 0, num2);
			num += num2;
			text = adbSocket.ReadString(4);
		}
	}

	public void UploadFile(string localFileName, string remoteFileName, int remoteFilePermissions = 1638)
	{
		string command = "SEND";
		string format = "{0},{1}";
		int num = 0;
		string text = "DATA";
		string text2 = "DONE";
		int length = 4;
		string value = "OKAY";
		AdbFileInfo fileInfo = GetFileInfo(remoteFileName);
		if (fileInfo.IsDirectory || fileInfo.IsSymbolicLink)
		{
			remoteFileName = AdbHelpers.CombinePath(remoteFileName, Path.GetFileName(localFileName));
		}
		using AdbSocket adbSocket = GetSocket();
		string text3 = SendSyncCommand(adbSocket, command, string.Format(format, remoteFileName, remoteFilePermissions), readResponse: false);
		FileInfo fileInfo2 = new FileInfo(localFileName);
		int num2 = (int)fileInfo2.Length;
		using (FileStream fileStream = File.OpenRead(localFileName))
		{
			byte[] array = new byte[65536];
			while (num2 > 0)
			{
				int num3 = ((num2 >= array.Length) ? array.Length : num2);
				fileStream.Read(array, 0, num3);
				adbSocket.WriteString(text);
				adbSocket.WriteInt32(num3);
				adbSocket.Write(array, num3);
				num2 -= num3;
			}
		}
		adbSocket.WriteString(text2);
		adbSocket.WriteInt32(AdbHelpers.ToUnixTime(fileInfo2.LastWriteTime));
		text3 = adbSocket.ReadString(length);
		if ((text3.Equals(value) ? 1 : 0) == num)
		{
			throw new AdbInvalidResponseException(text3);
		}
	}

	private string SendSyncCommand(AdbSocket adbSocket, string command, string parameter, bool readResponse = true)
	{
		object obj = null;
		string paramName = "parameter";
		string command2 = "sync:";
		if (parameter == obj)
		{
			throw new ArgumentNullException(paramName);
		}
		SetDevice(adbSocket);
		adbSocket.SendCommand(command2);
		return adbSocket.SendSyncCommand(command, parameter, readResponse);
	}

	public void DeleteFile(string remoteFileName)
	{
		string text = "rm -f ";
		ExecuteRemoteCommand(text + remoteFileName);
	}

	public void InstallApplication(string localFileName, bool installOnSdCard)
	{
		string text = "/data/local/tmp/";
		string text2 = "/sdcard/tmp/";
		string text3 = "";
		string text4 = "-s";
		string text5 = "install ";
		string text6 = " ";
		string fileName = Path.GetFileName(localFileName);
		string text7 = (installOnSdCard ? (text2 + fileName) : (text + fileName));
		string text8 = ((!installOnSdCard) ? text3 : text4);
		if (!GetFileInfo(text7).IsFile)
		{
			UploadFile(localFileName, text7);
		}
		else
		{
			DeleteFile(text7);
			UploadFile(localFileName, text7);
		}
		ExecutePm(text5 + text8 + text6 + text7);
		DeleteFile(text7);
	}

	public void UninstallApplication(string applicationName, bool keepDataAndCache = false)
	{
		string text = "";
		string text2 = "-k";
		string text3 = "uninstall ";
		string text4 = " ";
		string text5 = ((!keepDataAndCache) ? text : text2);
		ExecutePm(text3 + text5 + text4 + applicationName);
	}

	public AdbAppInfo[] GetInstalledApplications()
	{
		string command = "pm list packages -f";
		string pattern = "package:(.+?)=(.+)";
		string value = "/system/app/";
		string value2 = "/system/priv-app/";
		string value3 = "/system/";
		string value4 = "/data/";
		string[] array = ExecuteRemoteCommand(command);
		List<AdbAppInfo> list = new List<AdbAppInfo>();
		string[] array2 = array;
		string[] array3 = array2;
		foreach (string text in array3)
		{
			Match match = Regex.Match(text, pattern);
			if (match.Groups.Count == 3)
			{
				string value5 = match.Groups[1].Value;
				AdbAppType type = (value5.StartsWith(value) ? AdbAppType.System : (value5.StartsWith(value2) ? ((AdbAppType)2) : AdbAppType.ThirdParty));
				AdbAppLocation location = ((!value5.StartsWith(value3) && !value5.StartsWith(value4)) ? AdbAppLocation.ExternalMemory : ((AdbAppLocation)0));
				AdbAppInfo item = new AdbAppInfo(match.Groups[2].Value, value5, type, location);
				list.Add(item);
				continue;
			}
			throw new Exception(text);
		}
		return list.ToArray();
	}

	private void ExecutePm(string commandLine)
	{
		string text = "pm ";
		string value = "Success";
		string pattern = "\\[(.+?)]";
		string message = "Wrong pm output";
		string[] array = ExecuteRemoteCommand(text + commandLine);
		if (array != null && array.Length != 0)
		{
			string text2 = array[array.Length - 1];
			if (!text2.Equals(value))
			{
				Match match = Regex.Match(text2, pattern);
				if (2 != match.Groups.Count)
				{
					throw new Exception(text2);
				}
				throw new Exception(match.Groups[1].Value);
			}
			return;
		}
		throw new Exception(message);
	}

	private void SetDevice(AdbSocket adbSocket)
	{
		string message = "No device selected. Please ensure a device is connected and try again.";
		string text = "host:transport:";
		if (string.IsNullOrEmpty(DeviceSerialNumber))
		{
			throw new AdbException(message);
		}
		adbSocket.SendCommand(text + DeviceSerialNumber);
	}

	private AdbSocket GetSocket()
	{
		return new AdbSocket(ServerHost, ServerPort);
	}

	public bool adb_interactive()
	{
		string path = "Data";
		string path2 = "bin";
		string path3 = "adb.exe";
		string arguments = "shell";
		int result = 0;
		adb = new Process();
		adb.StartInfo.FileName = Path.Combine(Application.StartupPath, path, path2, path3);
		adb.StartInfo.Arguments = arguments;
		adb.StartInfo.RedirectStandardInput = true;
		adb.StartInfo.RedirectStandardOutput = true;
		adb.StartInfo.UseShellExecute = false;
		adb.StartInfo.CreateNoWindow = true;
		try
		{
			adb.Start();
			adb_in = adb.StandardInput;
			adb_out = adb.StandardOutput;
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public string method_24(string string_23, int int_2 = 20000, string string_24 = null)
	{
		string path = "Data";
		string path2 = "bin";
		string path3 = "adb.exe";
		string workingDirectory = "Data\\bin\\";
		string text = "";
		UI.KillProcess();
		Console.WriteLine(string_23);
		Process process = new Process();
		process.StartInfo = new ProcessStartInfo
		{
			FileName = ((string_24 == null) ? Path.Combine(Application.StartupPath, path, path2, path3) : string_24),
			WorkingDirectory = workingDirectory,
			Arguments = string_23,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};
		process.Start();
		string text2 = text;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		if (process.HasExited)
		{
			process.Dispose();
		}
		else
		{
			if (stopwatch.ElapsedMilliseconds <= int_2)
			{
				Thread.Sleep(100);
				text2 += process.StandardOutput.ReadToEnd();
				text2 += process.StandardError.ReadToEnd();
				int num = 80605608;
				while (true)
				{
					_ = num ^ 0x6B9CF4E1;
					if (process.HasExited)
					{
						process.Dispose();
						break;
					}
					if (stopwatch.ElapsedMilliseconds > int_2)
					{
						process.Kill();
						process.Dispose();
						break;
					}
					Thread.Sleep(100);
					text2 += process.StandardOutput.ReadToEnd();
					text2 += process.StandardError.ReadToEnd();
					num = 80605608;
				}
				Console.WriteLine(text2);
				return text2;
			}
			process.Kill();
			process.Dispose();
		}
		Console.WriteLine(text2);
		return text2;
	}

	public string Method23(string string24, int int2 = 20000)
	{
		string verb = "runas";
		string path = "Data";
		string path2 = "bin";
		string path3 = "amf.exe";
		string text = "";
		UI.KillProcess();
		Process process = new Process();
		ProcessStartInfo startInfo = new ProcessStartInfo
		{
			Verb = verb,
			FileName = Path.Combine(AppContext.BaseDirectory, path, path2, path3),
			WorkingDirectory = Path.Combine(AppContext.BaseDirectory, path, path2),
			Arguments = string24,
			RedirectStandardOutput = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};
		process.StartInfo = startInfo;
		Console.WriteLine(string24);
		process.Start();
		string text2 = text;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		bool flag = !process.HasExited;
		while (true)
		{
			if (!flag)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					break;
				}
				text2 = process.StandardOutput.ReadToEnd();
				process.Dispose();
				Console.WriteLine(text2);
				return text2;
			}
			if (stopwatch.ElapsedMilliseconds <= int2)
			{
				Thread.Sleep(10);
				flag = !process.HasExited;
				continue;
			}
			process.Kill();
			if (!string.IsNullOrEmpty(text2))
			{
				process.Dispose();
				Console.WriteLine(text2);
				return text2;
			}
			text2 = process.StandardOutput.ReadToEnd();
			process.Dispose();
			Console.WriteLine(text2);
			return text2;
		}
		process.Dispose();
		Console.WriteLine(text2);
		return text2;
	}

	public string custom_ctrl(string cmd)
	{
		string verb = "runas";
		string fileName = "Data\\bin\\recovery.exe";
		Console.WriteLine(cmd);
		try
		{
			Process process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					WindowStyle = ProcessWindowStyle.Hidden,
					Verb = verb,
					FileName = fileName,
					Arguments = cmd,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};
			process.Start();
			StreamReader standardOutput = process.StandardOutput;
			string result = standardOutput.ReadToEnd();
			process.WaitForExit();
			standardOutput.Close();
			return result;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
		return Conversion.ErrorToString();
	}

	public string adb_sendcmd(string cmd, int wait = 200)
	{
		object obj = null;
		string value = "#";
		string value2 = "$";
		if (adb == null || adb.HasExited)
		{
			return string.Empty;
		}
		adb_in.WriteLine(cmd);
		Thread.Sleep(wait);
		StringBuilder stringBuilder = new StringBuilder();
		if (adb_out.Peek() <= -1)
		{
			return stringBuilder.ToString();
		}
		string text = adb_out.ReadLine();
		if (text == obj)
		{
			return stringBuilder.ToString();
		}
		stringBuilder.AppendLine(text);
		bool flag;
		if (!text.EndsWith(value))
		{
			int num = 1711583957;
			while (true)
			{
				_ = num ^ 0x224EA9BF;
				flag = text.EndsWith(value2);
				while (true)
				{
					if (!flag)
					{
						if (adb_out.Peek() <= -1)
						{
							return stringBuilder.ToString();
						}
						text = adb_out.ReadLine();
						if (text == obj)
						{
							return stringBuilder.ToString();
						}
						stringBuilder.AppendLine(text);
						if (!text.EndsWith(value))
						{
							break;
						}
						flag = true;
						continue;
					}
					return stringBuilder.ToString();
				}
				num = 1711583957;
			}
		}
		flag = true;
		return stringBuilder.ToString();
	}

	public bool adb_alive()
	{
		if (adb == null)
		{
			return false;
		}
		return !adb.HasExited;
	}

	public void adb_exit()
	{
		string value = "exit";
		try
		{
			if (adb != null && !adb.HasExited)
			{
				adb_in.WriteLine(value);
				Thread.Sleep(200);
				if (!adb.HasExited)
				{
					adb.Kill();
				}
			}
		}
		catch
		{
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
