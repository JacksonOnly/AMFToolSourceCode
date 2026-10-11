using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace AMFTool.Library.Usb;

public class SerialMTP : IDisposable
{
	public struct Result
	{
		public bool Success;

		public string Output;

		public string TerminalLine;
	}

	private readonly object syncRoot = new object();

	private readonly ManualResetEventSlim responseEvent = new ManualResetEventSlim(initialState: false);

	private readonly StringBuilder responseText = new StringBuilder();

	private readonly StringBuilder receiveBuffer = new StringBuilder();

	private SerialPort serialPort;

	private bool waitingForResponse;

	private bool lastCommandSuccess;

	private string lastTerminalLine;

	public bool Open(string portName)
	{
		int result = 0;
		string newLine = "\r";
		Close();
		serialPort = new SerialPort
		{
			PortName = portName,
			WriteBufferSize = 921600,
			BaudRate = 1152000,
			ReadTimeout = 1500,
			WriteTimeout = 1500,
			DataBits = 8,
			Parity = Parity.None,
			StopBits = StopBits.One,
			DiscardNull = true,
			RtsEnable = true,
			DtrEnable = true,
			NewLine = newLine,
			Encoding = Encoding.ASCII
		};
		serialPort.DataReceived += OnDataReceived;
		try
		{
			serialPort.Open();
			return true;
		}
		catch
		{
			Close();
			return (byte)result != 0;
		}
	}

	public void Close()
	{
		object obj = null;
		if (serialPort == obj)
		{
			return;
		}
		try
		{
			serialPort.DataReceived -= OnDataReceived;
			if (serialPort.IsOpen)
			{
				serialPort.Close();
			}
		}
		catch
		{
		}
		finally
		{
			try
			{
				serialPort.Dispose();
			}
			catch
			{
			}
			serialPort = (SerialPort)obj;
		}
	}

	public Result WriteCommand(string command, int timeoutMs)
	{
		int num = 0;
		int num2 = 1;
		object terminalLine = null;
		int num3 = 2;
		int num4 = 13;
		int num5 = 10;
		string text = "\r";
		SerialPort serialPort = this.serialPort;
		if (serialPort == null || !serialPort.IsOpen)
		{
			return new Result
			{
				Success = false,
				Output = string.Empty,
				TerminalLine = (string)terminalLine
			};
		}
		lock (syncRoot)
		{
			responseEvent.Reset();
			waitingForResponse = true;
			lastCommandSuccess = false;
			lastTerminalLine = (string)terminalLine;
			responseText.Clear();
			receiveBuffer.Clear();
			command = (command ?? string.Empty).TrimEnd('\r', '\n');
			try
			{
				serialPort.DiscardInBuffer();
			}
			catch
			{
			}
			serialPort.Write(command + text);
		}
		bool flag = responseEvent.Wait(timeoutMs);
		object obj2 = syncRoot;
		bool lockTaken = (byte)num != 0;
		try
		{
			Monitor.Enter(obj2, ref lockTaken);
			if ((flag ? 1 : 0) == num)
			{
				string text2 = receiveBuffer.ToString();
				char[] array = new char[num3];
				array[num] = (char)num4;
				array[num2] = (char)num5;
				string line = text2.TrimEnd(array);
				if (IsTerminalLine(line, out var success))
				{
					lastCommandSuccess = success;
					lastTerminalLine = line;
					flag = (byte)num2 != 0;
				}
				waitingForResponse = (byte)num != 0;
			}
			return new Result
			{
				Success = ((!flag) ? ((byte)num != 0) : lastCommandSuccess),
				Output = responseText.ToString(),
				TerminalLine = lastTerminalLine
			};
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(obj2);
			}
		}
	}

	public Result WriteBytes(byte[] data, int timeoutMs)
	{
		int num = 0;
		int num2 = 1;
		object terminalLine = null;
		int num3 = 2;
		int num4 = 13;
		int num5 = 10;
		SerialPort serialPort = this.serialPort;
		if (serialPort != null && serialPort.IsOpen)
		{
			lock (syncRoot)
			{
				responseEvent.Reset();
				waitingForResponse = true;
				lastCommandSuccess = false;
				lastTerminalLine = (string)terminalLine;
				responseText.Clear();
				receiveBuffer.Clear();
				try
				{
					serialPort.DiscardInBuffer();
				}
				catch
				{
				}
				serialPort.Write(data, num, (data != null) ? data.Length : num);
			}
			bool flag = responseEvent.Wait(timeoutMs);
			object obj2 = syncRoot;
			bool lockTaken = (byte)num != 0;
			try
			{
				Monitor.Enter(obj2, ref lockTaken);
				if ((flag ? 1 : 0) == num)
				{
					string text = receiveBuffer.ToString();
					char[] array = new char[num3];
					array[num] = (char)num4;
					array[num2] = (char)num5;
					string line = text.TrimEnd(array);
					if (IsTerminalLine(line, out var success))
					{
						lastCommandSuccess = success;
						lastTerminalLine = line;
						flag = (byte)num2 != 0;
					}
					waitingForResponse = (byte)num != 0;
				}
				return new Result
				{
					Success = ((!flag) ? ((byte)num != 0) : lastCommandSuccess),
					Output = responseText.ToString(),
					TerminalLine = lastTerminalLine
				};
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj2);
				}
			}
		}
		return new Result
		{
			Success = false,
			Output = string.Empty,
			TerminalLine = (string)terminalLine
		};
	}

	public void Dispose()
	{
		Close();
		responseEvent.Dispose();
	}

	private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		object obj = null;
		try
		{
			SerialPort serialPort = this.serialPort;
			if (serialPort == obj)
			{
				return;
			}
			string value = serialPort.ReadExisting();
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			bool flag = false;
			lock (syncRoot)
			{
				if (!waitingForResponse)
				{
					return;
				}
				responseText.Append(value);
				receiveBuffer.Append(value);
				string line;
				while (TryExtractLine(receiveBuffer, out line))
				{
					if (IsTerminalLine(line, out var success))
					{
						lastCommandSuccess = success;
						lastTerminalLine = line;
						waitingForResponse = false;
						flag = true;
					}
				}
			}
			if (flag)
			{
				responseEvent.Set();
			}
		}
		catch
		{
		}
	}

	private static bool TryExtractLine(StringBuilder buffer, out string line)
	{
		object obj = null;
		for (int i = 0; i < buffer.Length; i++)
		{
			char c = buffer[i];
			if (c == '\r' || c == '\n')
			{
				line = buffer.ToString(0, i);
				int num = 1;
				if (c == '\r' && i + 1 < buffer.Length && buffer[i + 1] == '\n')
				{
					num = 2;
				}
				buffer.Remove(0, i + num);
				line = line.TrimEnd('\r', '\n');
				return true;
			}
		}
		line = (string)obj;
		return false;
	}

	private static bool IsTerminalLine(string line, out bool success)
	{
		string value = "OK";
		string value2 = "#OK#";
		string value3 = "ERROR";
		string value4 = "+CME ERROR";
		string value5 = "+CMS ERROR";
		string value6 = "NO CARRIER";
		string value7 = "NO DIALTONE";
		string value8 = "BUSY";
		string value9 = ">";
		success = false;
		if (!string.IsNullOrWhiteSpace(line))
		{
			if (!line.Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				if (line.Equals(value2, StringComparison.OrdinalIgnoreCase))
				{
					success = true;
					return true;
				}
				if (!line.Equals(value3, StringComparison.OrdinalIgnoreCase))
				{
					if (line.StartsWith(value4, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
					if (!line.StartsWith(value5, StringComparison.OrdinalIgnoreCase))
					{
						if (!line.Equals(value6, StringComparison.OrdinalIgnoreCase))
						{
							if (!line.Equals(value7, StringComparison.OrdinalIgnoreCase))
							{
								if (!line.Equals(value8, StringComparison.OrdinalIgnoreCase))
								{
									if (line.Equals(value9, StringComparison.Ordinal))
									{
										success = true;
										return true;
									}
									return false;
								}
								return true;
							}
							return true;
						}
						return true;
					}
					return true;
				}
				return true;
			}
			success = true;
			return true;
		}
		return false;
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
