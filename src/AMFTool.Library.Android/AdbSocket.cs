#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace AMFTool.Library.Android;

public class AdbSocket : IDisposable
{
	private TcpClient _tcpClient;

	private NetworkStream _tcpStream;

	private Encoding _encoding;

	private byte[] _buffer;

	public AdbSocket(string adbServerHost, int adbServerPort)
	{
		string format = "Connecting to {0} port {1}";
		_encoding = Encoding.ASCII;
		_buffer = new byte[65536];

		Trace.WriteLine(string.Format(format, adbServerHost, adbServerPort));
		_tcpClient = new TcpClient(adbServerHost, adbServerPort);
		_tcpStream = _tcpClient.GetStream();
	}

	public void Dispose()
	{
		object obj = null;
		if (_tcpClient !=null)
		{
			_tcpClient.Close();
			_tcpClient = (TcpClient)obj;
			if (_tcpStream !=null)
			{
				_tcpStream.Close();
				_tcpStream = (NetworkStream)obj;
			}
		}
		else if (_tcpStream !=null)
		{
			_tcpStream.Close();
			_tcpStream = (NetworkStream)obj;
		}
	}

	public void Write(byte[] data, int size)
	{
		string message = "$Sending {size} bytes";
		Trace.WriteLine(message);
		_tcpStream.Write(data, 0, size);
	}

	public void Write(byte[] data)
	{
		Write(data, data.Length);
	}

	public void WriteString(string text)
	{
		int bytes = _encoding.GetBytes(text, 0, text.Length, _buffer, 0);
		Write(_buffer, bytes);
	}

	public void WriteInt32(int number)
	{
		byte[] bytes = BitConverter.GetBytes(number);
		Write(bytes);
	}

	public void SendCommand(string command)
	{
		string format = "{0:X04}";
		string text = "OKAY";
		string text2 = "FAIL";
		WriteString(string.Format(format, command.Length));
		WriteString(command);
		string text3 = ReadString(4);
		string text4 = text3;
		string text5 = text4;
		if (!(text5 == text))
		{
			if (!(text5 == text2))
			{
				throw new AdbInvalidResponseException(text3);
			}
			string message = ReadHexString();
			throw new AdbException(message);
		}
	}

	public string SendCommandx(string command)
	{
		string format = "{0:X04}";
		string text = "OKAY";
		string text2 = "FAIL";
		string text3 = "err SendCommand: ";
		WriteString(string.Format(format, command.Length));
		WriteString(command);
		string text4 = ReadString(4);
		try
		{
			string text5 = text4;
			string text6 = text5;
			if (text6 == text)
			{
				return text;
			}
			if (text6 == text2)
			{
				return ReadHexString();
			}
			return text;
		}
		catch (Exception ex)
		{
			Console.WriteLine(text3 + ex.Message);
			return ex.Message;
		}
	}

	public string SendSyncCommand(string command, string parameter, bool readResponse = true)
	{
		object result = null;
		string value = "FAIL";
		WriteString(command);
		WriteInt32(parameter.Length);
		WriteString(parameter);
		if (readResponse)
		{
			string text = ReadString(4);
			if (!text.Equals(value))
			{
				return text;
			}
			string message = ReadSyncString();
			throw new AdbException(message);
		}
		return (string)result;
	}

	public void Read(byte[] data, int size)
	{
		int num = 0;
		if (0 >= size)
		{
			return;
		}
		int num2 = _tcpStream.Read(data, 0, size);
		num = 0 + num2;
		int num3 = 513210135;
		while (true)
		{
			_ = num3 ^ 0x2B6C5FFD;
			if (num < size)
			{
				num2 = _tcpStream.Read(data, num, size - num);
				num += num2;
				num3 = 513210135;
				continue;
			}
			break;
		}
	}

	public byte[] Read(int size)
	{
		byte[] array = new byte[size];
		Read(array, size);
		return array;
	}

	public string ReadHexString()
	{
		int length = ReadInt32Hex();
		return ReadString(length);
	}

	public string ReadSyncString()
	{
		int length = ReadInt32();
		return ReadString(length);
	}

	public string ReadString(int length)
	{
		Read(_buffer, length);
		return _encoding.GetString(_buffer, 0, length);
	}

	public int ReadInt32()
	{
		Read(_buffer, 4);
		return BitConverter.ToInt32(_buffer, 0);
	}

	public int ReadInt32Hex()
	{
		Read(_buffer, 4);
		string value = _encoding.GetString(_buffer, 0, 4);
		return Convert.ToInt32(value, 16);
	}

	public string[] ReadAllLines()
	{
		object obj = null;
		List<string> list = new List<string>();
		using (StreamReader streamReader = new StreamReader(_tcpStream, _encoding))
		{
			while (true)
			{
				string text = streamReader.ReadLine();
				if (text !=null)
				{
					list.Add(text.Trim());
					continue;
				}
				break;
			}
		}
		return list.ToArray();
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
