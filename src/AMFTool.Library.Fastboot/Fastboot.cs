using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AMFTool.Library.Fastboot;

public class Fastboot
{
	public enum Status
	{
		FAIL,
		OKAY,
		DATA,
		INFO,
		Unknown
	}

	public class Response
	{
		public Status Status { get; set; }

		public string Payload { get; set; }

		public byte[] RawData { get; set; }

		public Response(Status status, string payload)
		{
			Status = status;
			Payload = payload;
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}

		private object method_02()
		{
			return new int[0];
		}
	}

	public class PIDVID
	{
		public int Vid;

		public int Pid;

		public string Devicename;

		public string Manufacturer;

		public string Serial;

		public PIDVID(int vid, int pid, string deviceName, string manufacturer, string serial)
		{
			Vid = vid;
			Pid = pid;
			Devicename = deviceName;
			Manufacturer = manufacturer;
			Serial = serial;
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class PartitionEntry
	{
		public string Partition { get; set; }

		public string Size { get; set; }

		public string Type { get; set; }

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	private class SparseFlash
	{
		private Fastboot Fastboot;

		private string FlashPartition;

		private long TotalSize;

		private long totalProcess;

		public SparseFlash(Fastboot fastboot, string flashPartition, long totalSize)
		{
			Fastboot = fastboot;
			FlashPartition = flashPartition;
			TotalSize = totalSize;
			totalProcess = 0L;
		}

		public Stream CreateOutputStream(int idx)
		{
			string tempFileName = Path.GetTempFileName();
			return new FileStream(tempFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
		}

		public void OnSegmentReady(int idx, Stream stream)
		{
			string text = "flash:";
			string format = "Failed to flash {0}: {1}";
			long length = stream.Length;
			Fastboot.UploadData(stream);
			totalProcess += length;
			Response response;
			if (TotalSize <= 0L)
			{
				response = Fastboot.Command(text + FlashPartition);
			}
			else
			{
				UI.ProgressBar1(totalProcess, TotalSize);
				response = Fastboot.Command(text + FlashPartition);
			}
			if (response.Status != Status.OKAY)
			{
				throw new Exception(string.Format(format, idx, response.Payload));
			}
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	private const int BLOCK_SIZE = 1048576;

	public int USB_VID;

	public int USB_PID;

	public string DevicesName;

	public string Manufacturer;

	private string SerialNumber;

	private List<(string Serial, string Path)> DeviceDetected = new List<(string, string)>();

	private WinUsb Usb;

	public int Timeout { get; set; } = 3000;

	public int CommandTimeout { get; set; } = 30000;

	public Fastboot()
	{
		Usb = new WinUsb();
	}

	public List<PIDVID> FindDevices()
	{
		string query = "SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%Android%' OR Name LIKE '%Fastboot%' OR Name LIKE '%Bootloader%'";
		string propertyName = "Name";
		object obj = null;
		string value = "android";
		string value2 = "fastboot";
		string value3 = "bootloader";
		string propertyName2 = "DeviceID";
		string pattern = "VID_([0-9A-Fa-f]{4})";
		string pattern2 = "PID_([0-9A-Fa-f]{4})";
		string pattern3 = "\\\\([^\\\\]+)$";
		string propertyName3 = "Manufacturer";
		string text = "";
		List<PIDVID> list = new List<PIDVID>();
		WqlObjectQuery query2 = new WqlObjectQuery(query);
		ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query2);
		foreach (ManagementBaseObject item in managementObjectSearcher.Get())
		{
			ManagementObject managementObject = (ManagementObject)item;
			if (managementObject[propertyName] ==null)
			{
				continue;
			}
			string text2 = managementObject[propertyName].ToString().ToLowerInvariant();
			if (!text2.Contains(value) && !text2.Contains(value2) && !text2.Contains(value3))
			{
				continue;
			}
			int result = 0;
			int result2 = 0;
			string serial = string.Empty;
			object obj2 = managementObject[propertyName2];
			object obj3;
			if (obj2 == null)
			{
				obj3 = obj;
				if (obj3 != null)
				{
					goto IL_0110;
				}
			}
			else
			{
				obj3 = obj2.ToString();
				if (obj3 != null)
				{
					goto IL_0110;
				}
			}
			obj3 = string.Empty;
			goto IL_0110;
			IL_0110:
			string input = (string)obj3;
			Match match = Regex.Match(managementObject[propertyName2].ToString(), pattern, RegexOptions.IgnoreCase);
			if (match.Success)
			{
				int.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
			}
			match = Regex.Match(managementObject[propertyName2].ToString(), pattern2, RegexOptions.IgnoreCase);
			if (match.Success)
			{
				int.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result2);
			}
			match = Regex.Match(input, pattern3, RegexOptions.IgnoreCase);
			if (match.Success)
			{
				serial = match.Groups[1].Value;
			}
			int vid = result;
			int pid = result2;
			string deviceName = managementObject[propertyName].ToString();
			object obj4 = managementObject[propertyName3];
			object obj5;
			if (obj4 == null)
			{
				obj5 = obj;
				if (obj5 != null)
				{
					goto IL_0202;
				}
			}
			else
			{
				obj5 = obj4.ToString();
				if (obj5 != null)
				{
					goto IL_0202;
				}
			}
			obj5 = text;
			goto IL_0202;
			IL_0202:
			list.Add(new PIDVID(vid, pid, deviceName, (string)obj5, serial));
		}
		return list;
	}

	public string[] GetDevices()
	{
		object obj = null;
		int num = 0;
		int num2 = 1;
		string format = "USB_{0:X4}_{1:X4}_{2}";
		if (Usb == obj)
		{
			Usb = new WinUsb();
		}
		List<string> list = new List<string>();
		DeviceDetected.Clear();
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		HashSet<string> hashSet2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		List<PIDVID> list2 = FindDevices();
		if (list2.Count > 0)
		{
			foreach (PIDVID item in list2)
			{
				USB_VID = item.Vid;
				USB_PID = item.Pid;
				DevicesName = item.Devicename;
				Manufacturer = item.Manufacturer;
				List<(string, string, string)> list3 = Usb.EnumerateDevicePaths(item.Vid, item.Pid);
				for (int i = 0; i < list3.Count; i++)
				{
					if (!string.IsNullOrWhiteSpace(list3[i].Item1) && hashSet.Add(list3[i].Item1))
					{
						string text = list3[i].Item3;
						if (IsInvalidSerial(text))
						{
							text = item.Serial;
						}
						if (IsInvalidSerial(text))
						{
							text = string.Format(format, item.Vid, item.Pid, i + 1);
						}
						if (hashSet2.Add(text))
						{
							list.Add(text);
						}
						DeviceDetected.Add((text, list3[i].Item1));
					}
				}
			}
		}
		if (list.Count == num)
		{
			List<(string, string, string)> list4 = Usb.EnumerateDevicePaths(num, num);
			for (int j = num; j < list4.Count; j += num2)
			{
				if (string.IsNullOrWhiteSpace(list4[j].Item1) ? ((byte)num2 != 0) : ((hashSet.Add(list4[j].Item1) ? 1 : 0) == num))
				{
					continue;
				}
				var (num3, num4) = Usb.ParseVidPid(list4[j].Item2, list4[j].Item1);
				if (((num3 != 0) ? ((num4 == num) ? 1 : 0) : num2) == 0)
				{
					USB_VID = num3;
					USB_PID = num4;
					string text2 = list4[j].Item3;
					if (IsInvalidSerial(text2))
					{
						text2 = string.Format(format, num3, num4, j + num2);
					}
					if (hashSet2.Add(text2))
					{
						list.Add(text2);
					}
					DeviceDetected.Add((text2, list4[j].Item1));
				}
			}
		}
		return list.ToArray();
	}

	private bool IsInvalidSerial(string serial)
	{
		string value = "{";
		string value2 = "}";
		if (string.IsNullOrWhiteSpace(serial))
		{
			return true;
		}
		string text = serial.Trim();
		if (text.Length >= 3)
		{
			if (Guid.TryParse(text, out var _))
			{
				return true;
			}
			if (!text.StartsWith(value, StringComparison.OrdinalIgnoreCase) || !text.EndsWith(value2, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			return true;
		}
		return true;
	}

	public bool IsOpen(int timeout)
	{
		object obj = null;
		if (Usb == obj)
		{
			Usb = new WinUsb();
		}
		for (int i = 0; i < timeout; i++)
		{
			Thread.Sleep(500);
			int num = timeout - i;
			string[] devices = GetDevices();
			if (devices.Length != 0)
			{
				for (int j = 0; j < devices.Length; j++)
				{
					SerialNumber = devices[j];
					if (!Connect())
					{
						Disconnect();
						continue;
					}
					return true;
				}
			}
			Thread.Sleep(500);
			if ((num == -1) | JobStatus.Token.IsCancellationRequested)
			{
				break;
			}
		}
		return false;
	}

	public bool Connect()
	{
		object obj = null;
		if (Usb !=null)
		{
			Usb.Close();
			Usb = (WinUsb)obj;
		}
		List<(string, string)> list = new List<(string, string)>();
		if (DeviceDetected.Count == 0)
		{
			GetDevices();
		}
		if (!string.IsNullOrWhiteSpace(SerialNumber))
		{
			for (int i = 0; i < DeviceDetected.Count; i++)
			{
				(string, string) item = DeviceDetected[i];
				if (string.Equals(item.Item1, SerialNumber, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(item.Item2))
				{
					list.Add(item);
				}
			}
		}
		for (int j = 0; j < DeviceDetected.Count; j++)
		{
			(string, string) item2 = DeviceDetected[j];
			if (string.IsNullOrWhiteSpace(item2.Item2))
			{
				continue;
			}
			bool flag = false;
			for (int k = 0; k < list.Count; k++)
			{
				if (string.Equals(list[k].Item2, item2.Item2, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(item2);
			}
		}
		if (list.Count != 0)
		{
			for (int l = 0; l < list.Count; l++)
			{
				string item3 = list[l].Item1;
				string item4 = list[l].Item2;
				WinUsb winUsb = new WinUsb();
				if (winUsb.Open(item4, Timeout, CommandTimeout))
				{
					Usb = winUsb;
					SerialNumber = item3;
					return true;
				}
				winUsb.Close();
			}
			return false;
		}
		return false;
	}

	public void Disconnect()
	{
		object obj = null;
		if (Usb != obj)
		{
			Usb.Close();
			Usb = (WinUsb)obj;
		}
	}

	private bool EnsureConnected()
	{
		int result = 0;
		if (Usb == null || !Usb.IsOpen)
		{
			if (Connect())
			{
				return Usb != null && Usb.IsOpen;
			}
			try
			{
				string[] devices = GetDevices();
				if (devices.Length != 0)
				{
					if (string.IsNullOrWhiteSpace(SerialNumber) || !DeviceDetected.Any(((string Serial, string Path) x) => string.Equals(x.Serial, SerialNumber, StringComparison.OrdinalIgnoreCase)))
					{
						SerialNumber = devices[0];
					}
					if (Connect())
					{
						return Usb != null && Usb.IsOpen;
					}
				}
				return false;
			}
			catch
			{
				return (byte)result != 0;
			}
		}
		return true;
	}

	public string GetSerialNumber()
	{
		return SerialNumber;
	}

	private Status GetStatusFromString(string header)
	{
		string text = "INFO";
		string text2 = "OKAY";
		string text3 = "DATA";
		string text4 = "FAIL";
		if (!(header == text))
		{
			if (header == text2)
			{
				return Status.OKAY;
			}
			if (!(header == text3))
			{
				if (header == text4)
				{
					return Status.FAIL;
				}
				return Status.Unknown;
			}
			return Status.DATA;
		}
		return Status.INFO;
	}

	public List<PartitionEntry> ReadGpt()
	{
		string command = "getvar:all";
		string text = "(bootloader)";
		string text2 = "partition-size:";
		string text3 = "partition-type:";
		Response response = Command(command);
		Dictionary<string, PartitionEntry> dictionary = new Dictionary<string, PartitionEntry>(StringComparer.OrdinalIgnoreCase);
		string text4 = response.Payload ?? string.Empty;
		string[] array = text4.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string text5 = array[i].Trim();
			if (text5.Length == 0)
			{
				continue;
			}
			if (text5.StartsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				text5 = text5.Substring(text.Length).Trim();
			}
			if (text5.StartsWith(text2, StringComparison.OrdinalIgnoreCase))
			{
				string text6 = text5.Substring(text2.Length);
				int num = text6.IndexOf(':');
				if (num <= 0)
				{
					continue;
				}
				string text7 = text6.Substring(0, num).Trim();
				string size = text6.Substring(num + 1).Trim();
				if (text7.Length != 0)
				{
					if (!dictionary.TryGetValue(text7, out var value))
					{
						value = (dictionary[text7] = new PartitionEntry
						{
							Partition = text7
						});
					}
					value.Size = size;
				}
			}
			else
			{
				if (!text5.StartsWith(text3, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				string text8 = text5.Substring(text3.Length);
				int num2 = text8.IndexOf(':');
				if (num2 <= 0)
				{
					continue;
				}
				string text9 = text8.Substring(0, num2).Trim();
				string type = text8.Substring(num2 + 1).Trim();
				if (text9.Length != 0)
				{
					if (!dictionary.TryGetValue(text9, out var value2))
					{
						value2 = (dictionary[text9] = new PartitionEntry
						{
							Partition = text9
						});
					}
					value2.Type = type;
				}
			}
		}
		return dictionary.Values.OrderBy((PartitionEntry v) => v.Partition, StringComparer.OrdinalIgnoreCase).ToList();
	}

	public Response Command(byte[] command)
	{
		string message = "Fastboot device is not connected.";
		string format = "Failed to write command! Transfered: {0} of {1} bytes";
		string message2 = "USB read failed.";
		string message3 = "Timeout waiting fastboot response.";
		string text = "Invalid short response from fastboot: ";
		string text2 = "Unknown fastboot response header: ";
		string value = "\n";
		string oldValue = "\r";
		string oldValue2 = "\0";
		if (!EnsureConnected())
		{
			throw new Exception(message);
		}
		int num = Usb.Write(command, Timeout);
		if (num != command.Length)
		{
			throw new Exception(string.Format(format, num, command.Length));
		}
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = new byte[1024];
		string empty = string.Empty;
		DateTime dateTime = DateTime.UtcNow.AddMilliseconds((CommandTimeout > 0) ? CommandTimeout : 30000);
		Status statusFromString;
		while (true)
		{
			int num2 = Usb.Read(array, CommandTimeout);
			if (num2 >= 0)
			{
				if (num2 == 0)
				{
					if (DateTime.UtcNow >= dateTime)
					{
						throw new Exception(message3);
					}
					continue;
				}
				empty = ((num2 > 0) ? Encoding.ASCII.GetString(array, 0, num2) : string.Empty);
				if (empty.Length < 4)
				{
					if (DateTime.UtcNow >= dateTime)
					{
						throw new Exception(text + empty);
					}
					continue;
				}
				string header = new string(empty.Take(4).ToArray());
				statusFromString = GetStatusFromString(header);
				switch (statusFromString)
				{
				case Status.Unknown:
					if (!(DateTime.UtcNow >= dateTime))
					{
						continue;
					}
					throw new Exception(text2 + empty);
				default:
					if (num2 > 4)
					{
						stringBuilder.Append(empty.Skip(4).Take(num2 - 4).ToArray());
					}
					break;
				case Status.INFO:
				{
					int num3 = 4;
					if (empty.Length > 4 && empty[4] == ' ')
					{
						num3 = 5;
					}
					if (num2 > num3)
					{
						stringBuilder.Append(empty.Skip(num3).Take(num2 - num3).ToArray());
					}
					break;
				}
				}
				stringBuilder.Append(value);
				if (statusFromString != Status.INFO)
				{
					break;
				}
				continue;
			}
			throw new Exception(message2);
		}
		string text3 = stringBuilder.ToString().Replace(oldValue, string.Empty).Replace(oldValue2, string.Empty);
		text3 = text3.Trim();
		return new Response(statusFromString, text3)
		{
			RawData = ((empty.Length > 0) ? Encoding.ASCII.GetBytes(empty) : Array.Empty<byte>())
		};
	}

	private void SendDataCommand(long size)
	{
		string format = "download:{0:X8}";
		string format2 = "Invalid response from device! (data size: {0})";
		if (Command(string.Format(format, size)).Status != Status.DATA)
		{
			throw new Exception(string.Format(format2, size));
		}
	}

	private void TransferBlock(Stream stream, byte[] buffer, int size)
	{
		string message = "Unexpected end of stream while reading data to transfer.";
		string format = "Failed to transfer block (sent {0} of {1}), lastTransferred={2}";
		int num = 0;
		if (0 >= size)
		{
			int num2 = 0;
			return;
		}
		int num3 = stream.Read(buffer, 0, size);
		if (num3 > 0)
		{
			num = 0 + num3;
			int num4 = 870735473;
			int num2;
			while (true)
			{
				_ = num4 ^ 0x6ACB0C7E;
				if (num >= size)
				{
					num2 = 0;
					if (0 < num)
					{
						break;
					}
					return;
				}
				num3 = stream.Read(buffer, num, size - num);
				if (num3 > 0)
				{
					num += num3;
					num4 = 870735473;
					continue;
				}
				throw new EndOfStreamException(message);
			}
			int num5 = Math.Min(65536, num);
			byte[] array = new byte[num5];
			Buffer.BlockCopy(buffer, 0, array, 0, num5);
			int num6 = Usb.Write(array, CommandTimeout);
			if (num6 <= 0)
			{
				throw new Exception(string.Format(format, 0, num, num6));
			}
			num2 = 0 + num6;
			int num7 = 1939961674;
			while (true)
			{
				_ = num7 ^ 0x6ACB0C7E;
				if (num2 < num)
				{
					num5 = Math.Min(65536, num - num2);
					array = new byte[num5];
					Buffer.BlockCopy(buffer, num2, array, 0, num5);
					num6 = Usb.Write(array, CommandTimeout);
					if (num6 <= 0)
					{
						throw new Exception(string.Format(format, num2, num, num6));
					}
					num2 += num6;
					num7 = 1939961674;
					continue;
				}
				break;
			}
			return;
		}
		throw new EndOfStreamException(message);
	}

	public void UploadData(Stream stream)
	{
		string message = "Fastboot device is not connected.";
		double field_ = Class38.field_543;
		double field_2 = Class38.field_3686;
		string message2 = "USB read failed.";
		string text = "Invalid response from device: ";
		string text2 = "Invalid status: ";
		if (EnsureConnected())
		{
			long length = stream.Length;
			long num = 0L;
			byte[] buffer = new byte[1048576];
			Stopwatch stopwatch = Stopwatch.StartNew();
			DateTime dateTime = DateTime.MinValue;
			SendDataCommand(length);
			UI.ProgressBar1(0L, 100L);
			UI.LabelWrite(utils.FileSize(field_), utils.FileSize(field_), utils.FileSize(length));
			if (length == 0L)
			{
				UI.ProgressBar1(100L, 100L);
			}
			else
			{
				while (num + 1048576L <= length)
				{
					TransferBlock(stream, buffer, 1048576);
					num += 1048576L;
					UI.ProgressBar1(num, length);
					if ((DateTime.Now - dateTime).TotalMilliseconds > field_2)
					{
						double totalSeconds = stopwatch.Elapsed.TotalSeconds;
						if (totalSeconds > field_)
						{
							UI.LabelWrite(utils.FileSize((long)((double)num / totalSeconds)), utils.FileSize(num), utils.FileSize(length));
						}
						dateTime = DateTime.Now;
					}
				}
				int num2 = (int)(length - num);
				if (num2 > 0)
				{
					TransferBlock(stream, buffer, num2);
					num += num2;
					UI.ProgressBar1(num, length);
					if ((DateTime.Now - dateTime).TotalMilliseconds > field_2)
					{
						double totalSeconds2 = stopwatch.Elapsed.TotalSeconds;
						if (totalSeconds2 > field_)
						{
							UI.LabelWrite(utils.FileSize((long)((double)num / totalSeconds2)), utils.FileSize(num), utils.FileSize(length));
						}
						dateTime = DateTime.Now;
					}
				}
			}
			stopwatch.Stop();
			UI.ProgressBar1(length, length);
			UI.LabelWrite(utils.FileSize(field_), utils.FileSize(length), utils.FileSize(length));
			byte[] array = new byte[64];
			int num3 = Usb.Read(array, CommandTimeout);
			if (num3 >= 0)
			{
				string text3 = ((num3 <= 0) ? string.Empty : Encoding.ASCII.GetString(array, 0, num3));
				if (text3.Length < 4)
				{
					throw new Exception(text + text3);
				}
				Status statusFromString = GetStatusFromString(text3.Substring(0, 4));
				if (statusFromString != Status.OKAY)
				{
					throw new Exception(text2 + text3);
				}
				return;
			}
			throw new Exception(message2);
		}
		throw new Exception(message);
	}

	public void UploadData(string path)
	{
		using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);
		UploadData(stream);
	}

	public Response FlashPartition(string partition, string filePath)
	{
		long field_ = Class38.field_3037;
		string message = "Raw image larger than 4GB is not supported to flash!";
		if (!Sparse.IsSparseImage(filePath) || new FileInfo(filePath).Length <= GetMaxDownloadSize())
		{
			if (new FileInfo(filePath).Length < field_)
			{
				return FlashRawImage(partition, filePath);
			}
			throw new Exception(message);
		}
		return FlashSparseImage(partition, filePath);
	}

	public Response FlashRawImage(string partition, string filePath)
	{
		string text = "flash:";
		string text2 = "_";
		if (GetSlot(partition))
		{
			string text3 = GetCurrentSolt().Payload.Trim();
			if (GetIsLogical(partition, text3))
			{
				ResizeLogicalPartition(partition, new FileInfo(filePath).Length, text3);
				UploadData(filePath);
				return Command(text + partition + text2 + text3);
			}
			UploadData(filePath);
			return Command(text + partition + text2 + text3);
		}
		UploadData(filePath);
		return Command(text + partition);
	}

	public Response FlashSparseImage(string partition, string filePath)
	{
		int status = 1;
		object obj = null;
		string text = "_";
		string payload = "No segments flashed";
		using BinaryReader binaryReader = new BinaryReader(File.OpenRead(filePath), Encoding.Default, leaveOpen: true);
		using Sparse sparse = new Sparse(binaryReader);
		string flashPartition = partition;
		string text2 = (string)obj;
		if (GetSlot(partition))
		{
			text2 = GetCurrentSolt().Payload.Trim();
			flashPartition = partition + text + text2;
			if (GetIsLogical(partition, text2))
			{
				long size = (long)sparse.Header.TotalBlocks * (long)sparse.Header.BlockSize;
				ResizeLogicalPartition(partition, size, text2);
			}
		}
		long num = 1073741824L;
		try
		{
			num = GetMaxDownloadSize();
		}
		catch
		{
		}
		long length = binaryReader.BaseStream.Length;
		SparseFlash sparseFlash = new SparseFlash(this, flashPartition, length);
		sparse.SplitSparse((int)num, sparseFlash.CreateOutputStream, sparseFlash.OnSegmentReady);
		return new Response((Status)status, payload);
	}

	public long GetMaxDownloadSize()
	{
		string command = "getvar:max-download-size";
		string value = "0x";
		string payload = Command(command).Payload;
		if (!payload.StartsWith(value, StringComparison.OrdinalIgnoreCase))
		{
			return Convert.ToInt32(payload);
		}
		payload = payload.Substring(2).Trim();
		return Convert.ToInt32(payload, 16);
	}

	public Response ResizeLogicalPartition(string partitionName, long size, string slot = "a")
	{
		string format = "resize-logical-partition:{0}_{1}:{2}";
		return Command(string.Format(format, partitionName, slot, size));
	}

	public Response GetPartitionSize(string partitionName, string slot = "a")
	{
		string text = "getvar:partition-size:";
		string text2 = "_";
		return Command(text + partitionName + text2 + slot);
	}

	public Response GetCurrentSolt()
	{
		string command = "getvar:current-slot";
		return Command(command);
	}

	public bool GetIsLogical(string partitionName, string slot = "a")
	{
		string text = "getvar:is-logical:";
		string text2 = "_";
		string text3 = "yes";
		if (!(Command(text + partitionName + text2 + slot).Payload.Trim() == text3))
		{
			return false;
		}
		return true;
	}

	public bool GetSlot(string partitionName)
	{
		string text = "getvar:has-slot:";
		string text2 = "yes";
		if (!(Command(text + partitionName).Payload.Trim() == text2))
		{
			return false;
		}
		return true;
	}

	public Response Command(string command)
	{
		return Command(Encoding.ASCII.GetBytes(command));
	}

	public bool ChangeSlot(bool toA)
	{
		string text = "b";
		string text2 = "a";
		string text3 = "set_active:";
		int result = 1;
		string text4 = (toA ? text2 : text);
		Response response = Command(text3 + text4);
		if (response.Status != Status.OKAY)
		{
			return false;
		}
		try
		{
			string text5 = (GetCurrentSolt().Payload ?? string.Empty).Trim();
			text5 = text5.TrimStart('_');
			if (text5.Length != 0)
			{
				return text5.Equals(text4, StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public bool ErasePartition(string partitionName)
	{
		string text = "erase:";
		int result = 0;
		try
		{
			return Command(text + partitionName).Status == Status.OKAY;
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			return (byte)result != 0;
		}
	}

	public bool WritePartition(string partitionName, string filePath)
	{
		int result = 0;
		try
		{
			Response response = FlashPartition(partitionName, filePath);
			return response != null && response.Status == Status.OKAY;
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			return (byte)result != 0;
		}
	}

	public Dictionary<string, string> ReadInfo()
	{
		string command = "getvar:all";
		string text = "(bootloader)";
		string value = "partition-size:";
		string value2 = "partition-type:";
		Response response = Command(command);
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		string text2 = response.Payload ?? string.Empty;
		string[] array = text2.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string text3 = array[i].Trim();
			if (text3.Length == 0)
			{
				continue;
			}
			if (text3.StartsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				text3 = text3.Substring(text.Length).Trim();
			}
			if (text3.StartsWith(value, StringComparison.OrdinalIgnoreCase) || text3.StartsWith(value2, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			int num = text3.IndexOf(':');
			if (num > 0)
			{
				string text4 = text3.Substring(0, num).Trim();
				string value3 = text3.Substring(num + 1).Trim();
				if (text4.Length != 0)
				{
					dictionary[text4] = value3;
				}
			}
		}
		return dictionary;
	}

	public bool SendCommand(string command)
	{
		int result = 0;
		try
		{
			return Command(command).Status == Status.OKAY;
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			return (byte)result != 0;
		}
	}

	public bool SetBootloaderUnlock(bool unlock)
	{
		string command = "flashing unlock";
		int result = 1;
		int result2 = 0;
		string command2 = "oem unlock";
		string command3 = "flashing unlock_critical";
		string command4 = "flashing lock";
		string command5 = "oem lock";
		string command6 = "flashing lock_critical";
		string command7 = "getvar:unlocked";
		string value = "yes";
		string value2 = "no";
		try
		{
			Response response;
			if (!unlock)
			{
				response = Command(command4);
				if (response.Status != Status.OKAY)
				{
					response = Command(command5);
				}
				if (response.Status != Status.OKAY)
				{
					response = Command(command6);
				}
			}
			else
			{
				response = Command(command);
				if (response.Status != Status.OKAY)
				{
					response = Command(command2);
				}
				if (response.Status != Status.OKAY)
				{
					response = Command(command3);
				}
			}
			if (response.Status != Status.OKAY)
			{
				return false;
			}
			try
			{
				response = Command(command7);
				string text = (response.Payload ?? string.Empty).Trim();
				if (text.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return unlock;
				}
				if (text.Equals(value2, StringComparison.OrdinalIgnoreCase))
				{
					return !unlock;
				}
			}
			catch
			{
			}
			return (byte)result != 0;
		}
		catch (Exception value3)
		{
			Console.WriteLine(value3);
			return (byte)result2 != 0;
		}
	}

	public (bool Success, string Value) TryGetVar(string name)
	{
		string text = "getvar:";
		int item = 0;
		object item2 = null;
		try
		{
			Response response = Command(text + name);
			return (Success: response.Status == Status.OKAY, Value: response.Payload);
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			return (Success: (byte)item != 0, Value: (string)item2);
		}
	}

	public string GetVar(string name)
	{
		object result = null;
		(bool, string) tuple = TryGetVar(name);
		if (!tuple.Item1)
		{
			return (string)result;
		}
		return tuple.Item2;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
