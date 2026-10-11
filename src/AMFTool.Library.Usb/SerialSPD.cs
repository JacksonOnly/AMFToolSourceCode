using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace AMFTool.Library.Usb;

public class SerialSPD
{
	public static Native usb = new Native();

	private static int timeout = 5000;

	public static bool Handshake = false;

	public static bool Open(string port)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		usb = new Native
		{
			PortName = port,
			BaudRate = 921600u,
			Timeout = 5000,
			max_to_read = 2097152,
			max_to_write = 2097152,
			readTimeout = 1000,
			writeTimeout = 1000
		};
		usb.Open();
		return usb.IsOpen;
	}

	public static bool Write(byte[] send)
	{
		if (!JobStatus.Token.IsCancellationRequested)
		{
			return usb.writeusb(send);
		}
		return false;
	}

	public static byte[] Read()
	{
		if (JobStatus.Token.IsCancellationRequested)
		{
			return Array.Empty<byte>();
		}
		return usb.read(setting: true);
	}

	public static void TimeoutSet()
	{
		TimeoutSet(10);
	}

	public static void Close()
	{
		usb.Dispose();
	}

	public static void Flush()
	{
		usb.Flush();
	}

	public static byte[] ReadFrame(bool useTranscode)
	{
		object obj = null;
		int num = 0;
		int num2 = 1;
		int num3 = -1;
		int num4 = 126;
		int minimumLength = 131072;
		int num5 = 2;
		int num6 = 32;
		int num7 = 125;
		int num8 = 200;
		if (!JobStatus.Token.IsCancellationRequested)
		{
			if (usb != null && usb.IsOpen)
			{
				int num9 = timeout;
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = false;
				bool flag2 = false;
				int decodedCount = 0;
				int expectedDecodedLength = -1;
				byte[] header = new byte[4];
				int headerCount = 0;
				byte[] array = (byte[])obj;
				int num10 = 0;
				while (true)
				{
					if ((JobStatus.Token.IsCancellationRequested ? 1 : 0) != num)
					{
						if (array !=null)
						{
							ArrayPool<byte>.Shared.Return(array, (byte)num != 0);
						}
						break;
					}
					if (((num9 <= num) ? num : ((((stopwatch.ElapsedMilliseconds < num9) ? 1 : 0) == num) ? 1 : 0)) == 0)
					{
						Stopwatch stopwatch2 = Stopwatch.StartNew();
						try
						{
							while ((!JobStatus.Token.IsCancellationRequested) ? (stopwatch2.ElapsedMilliseconds < num8) : ((byte)num != 0))
							{
								byte[] array2 = Read();
								int num11 = array2.Length;
								if (((num11 > num) ? 1 : 0) == num)
								{
									if (Handshake)
									{
										if (array !=null)
										{
											ArrayPool<byte>.Shared.Return(array, (byte)num != 0);
										}
										return (byte[])obj;
									}
									continue;
								}
								for (int i = num; i < num11; i += num2)
								{
									byte b = array2[i];
									if ((flag ? 1 : 0) != num)
									{
										if (array == obj)
										{
											array = ArrayPool<byte>.Shared.Rent(minimumLength);
										}
										if (((num10 < array.Length) ? 1 : 0) == num)
										{
											int minimumLength2 = array.Length * num5;
											byte[] array3 = ArrayPool<byte>.Shared.Rent(minimumLength2);
											Buffer.BlockCopy(array, num, array3, num, num10);
											ArrayPool<byte>.Shared.Return(array, (byte)num != 0);
											array = array3;
										}
										byte[] array4 = array;
										int num12 = num10;
										num10 = num12 + num2;
										array4[num12] = b;
										if (!(b == num4 && useTranscode))
										{
											if (((b != num4) ? num : (((useTranscode ? 1 : 0) == num) ? 1 : 0)) == 0 || ((expectedDecodedLength >= num) ? ((((decodedCount < expectedDecodedLength) ? 1 : 0) == num) ? 1 : 0) : num) == 0)
											{
												if (!useTranscode)
												{
													ProcessDecoded(ref headerCount, header, ref expectedDecodedLength, ref decodedCount, b);
												}
												else if (flag2)
												{
													byte decoded = (byte)(b ^ num6);
													flag2 = (byte)num != 0;
													ProcessDecoded(ref headerCount, header, ref expectedDecodedLength, ref decodedCount, decoded);
												}
												else if (b != num7)
												{
													ProcessDecoded(ref headerCount, header, ref expectedDecodedLength, ref decodedCount, b);
												}
												else
												{
													flag2 = (byte)num2 != 0;
												}
												continue;
											}
											byte[] array5 = new byte[num10];
											Buffer.BlockCopy(array, num, array5, num, num10);
											ArrayPool<byte>.Shared.Return(array, (byte)num != 0);
											return array5;
										}
										if ((expectedDecodedLength >= num) ? (((decodedCount < expectedDecodedLength) ? 1 : 0) == num) : ((byte)num != 0))
										{
											byte[] array6 = new byte[num10];
											Buffer.BlockCopy(array, num, array6, num, num10);
											ArrayPool<byte>.Shared.Return(array, (byte)num != 0);
											return array6;
										}
										flag = (byte)num2 != 0;
										num10 = num;
										byte[] array7 = array;
										int num13 = num10;
										num10 = num13 + num2;
										array7[num13] = b;
										flag2 = (byte)num != 0;
										decodedCount = num;
										expectedDecodedLength = num3;
										headerCount = num;
									}
									else if (b == num4)
									{
										flag = (byte)num2 != 0;
										if (array == obj)
										{
											array = ArrayPool<byte>.Shared.Rent(minimumLength);
										}
										num10 = num;
										byte[] array8 = array;
										int num14 = num10;
										num10 = num14 + num2;
										array8[num14] = b;
										flag2 = (byte)num != 0;
										decodedCount = num;
										expectedDecodedLength = num3;
										headerCount = num;
									}
								}
							}
						}
						catch (TimeoutException)
						{
							Thread.Yield();
						}
						catch (IOException)
						{
							if ((usb != null) ? ((usb.IsOpen ? 1 : 0) == num) : ((byte)num2 != 0))
							{
								if (array !=null)
								{
									ArrayPool<byte>.Shared.Return(array, (byte)num != 0);
								}
								return (byte[])obj;
							}
							Thread.Yield();
						}
						catch (InvalidOperationException)
						{
							if (array !=null)
							{
								ArrayPool<byte>.Shared.Return(array, (byte)num != 0);
							}
							return (byte[])obj;
						}
						continue;
					}
					if (array !=null)
					{
						ArrayPool<byte>.Shared.Return(array, (byte)num != 0);
					}
					return (byte[])obj;
				}
				return (byte[])obj;
			}
			return (byte[])obj;
		}
		return (byte[])obj;
	}

	private static void ProcessDecoded(ref int headerCount, byte[] header, ref int expectedDecodedLength, ref int decodedCount, byte decoded)
	{
		if (headerCount < 4)
		{
			header[headerCount++] = decoded;
			if (headerCount != 4)
			{
				decodedCount++;
				return;
			}
			int num = (header[2] << 8) | header[3];
			expectedDecodedLength = 4 + num + 2;
			decodedCount++;
		}
		else
		{
			decodedCount++;
		}
	}

	public static void TimeoutSet(int timeoutMs = 5000)
	{
		object obj = null;
		if (usb != obj)
		{
			timeout = Math.Max(1, timeoutMs);
			usb.readTimeout = timeout;
			usb.writeTimeout = timeout;
		}
	}

	public static int GetTimeout()
	{
		return timeout;
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
