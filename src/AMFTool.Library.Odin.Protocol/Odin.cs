using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using AMFTool.Library.Lz4;
using AMFTool.Library.Odin.PIT;
using AMFTool.Library.Odin.Tar;
using AMFTool.Library.Usb;

namespace AMFTool.Library.Odin.Protocol;

public class Odin
{
	public class ZeroFillReadStream : Stream
	{
		private long LengthValue;

		private long PositionValue;

		public override bool CanRead => true;

		public override bool CanSeek => true;

		public override bool CanWrite => false;

		public override long Length => LengthValue;

		public override long Position
		{
			get
			{
				return PositionValue;
			}
			set
			{
				PositionValue = ((value < 0L) ? 0L : ((value > LengthValue) ? LengthValue : value));
			}
		}

		public ZeroFillReadStream(long length)
		{
			LengthValue = Math.Max(0L, length);
			PositionValue = 0L;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			object obj = null;
			string paramName = "buffer";
			if (buffer != obj)
			{
				bool flag;
				int num2;
				if (offset >= 0)
				{
					if (count >= 0)
					{
						if (flag = offset + count > buffer.Length)
						{
							throw new ArgumentOutOfRangeException();
						}
						if (PositionValue < LengthValue && count != 0)
						{
							long val = LengthValue - PositionValue;
							int num = (int)Math.Min(count, val);
							Array.Clear(buffer, offset, num);
							PositionValue += num;
							return num;
						}
						num2 = 0;
						return 0;
					}
					flag = true;
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (PositionValue < LengthValue && count != 0)
				{
					long val = LengthValue - PositionValue;
					int num = (int)Math.Min(count, val);
					Array.Clear(buffer, offset, num);
					PositionValue += num;
					return num;
				}
				num2 = 0;
				return 0;
			}
			throw new ArgumentNullException(paramName);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			bool flag;
			long num;
			switch (origin)
			{
			default:
				num = LengthValue + offset;
				flag = num < 0L;
				goto IL_0069;
			case SeekOrigin.Begin:
				num = offset;
				flag = num < 0L;
				goto IL_0069;
			case SeekOrigin.Current:
				{
					num = PositionValue + offset;
					if (!(flag = num < 0L))
					{
						if (num <= LengthValue)
						{
							PositionValue = num;
							break;
						}
						num = LengthValue;
						PositionValue = num;
						break;
					}
					num = 0L;
					if (0L <= LengthValue)
					{
						PositionValue = num;
					}
					else
					{
						num = LengthValue;
						PositionValue = num;
					}
					return PositionValue;
				}
				IL_0069:
				if (!flag)
				{
					if (num <= LengthValue)
					{
						PositionValue = num;
					}
					else
					{
						num = LengthValue;
						PositionValue = num;
					}
					return PositionValue;
				}
				num = 0L;
				if (0L <= LengthValue)
				{
					PositionValue = num;
				}
				else
				{
					num = LengthValue;
					PositionValue = num;
				}
				return PositionValue;
			}
			return PositionValue;
		}

		public override void SetLength(long value)
		{
			LengthValue = Math.Max(0L, value);
			if (PositionValue > LengthValue)
			{
				PositionValue = LengthValue;
			}
		}

		public override void Flush()
		{
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public PitPatchContext CurrentPitPatch { get; set; }

	public bool EraseNandByPartitionNames(SerialOdin serial, LokeCommandService loke, List<string> partitionNames)
	{
		PitResult pitResult = ReadPitTable(serial, loke);
		bool flag;
		HashSet<string> selectedNames;
		if (!pitResult.IsSuccess)
		{
			flag = true;
		}
		else
		{
			if (pitResult.PitEntries != null)
			{
				if (flag = pitResult.PitEntries.Count == 0)
				{
					return false;
				}
				selectedNames = new HashSet<string>(partitionNames.Where((string partitionName) => !string.IsNullOrWhiteSpace(partitionName)), StringComparer.OrdinalIgnoreCase);
				if (selectedNames.Count != 0)
				{
					List<PitEntry> list = pitResult.PitEntries.Where(delegate(PitEntry partitionEntry)
					{
						if (selectedNames.Contains(partitionEntry.PartitionName))
						{
							return true;
						}
						return selectedNames.Contains(partitionEntry.FlashFileName) || selectedNames.Contains(Path.GetFileNameWithoutExtension(partitionEntry.FlashFileName ?? string.Empty));
					}).ToList();
					if (list.Count == 0)
					{
						return false;
					}
					return EraseNandPartitions(serial, loke, list);
				}
				return false;
			}
			flag = true;
		}
		if (flag)
		{
			return false;
		}
		selectedNames = new HashSet<string>(partitionNames.Where((string partitionName) => !string.IsNullOrWhiteSpace(partitionName)), StringComparer.OrdinalIgnoreCase);
		if (selectedNames.Count != 0)
		{
			List<PitEntry> list = pitResult.PitEntries.Where(delegate(PitEntry partitionEntry)
			{
				if (selectedNames.Contains(partitionEntry.PartitionName))
				{
					return true;
				}
				return selectedNames.Contains(partitionEntry.FlashFileName) || selectedNames.Contains(Path.GetFileNameWithoutExtension(partitionEntry.FlashFileName ?? string.Empty));
			}).ToList();
			if (list.Count == 0)
			{
				return false;
			}
			return EraseNandPartitions(serial, loke, list);
		}
		return false;
	}

	public bool EraseNandPartitions(SerialOdin serial, LokeCommandService loke, List<PitEntry> partitions)
	{
		int num = 0;
		int result = 1;
		var list = (from partitionEntry in partitions.Where(delegate(PitEntry partitionEntry)
			{
				object obj = null;
				return partitionEntry !=null;
			})
			select new
			{
				Entry = partitionEntry,
				Size = GetEraseSizeBytes(partitionEntry)
			} into partitionItem
			where partitionItem.Size > 0L
			select partitionItem).ToList();
		if (list.Count != 0)
		{
			long num2 = 0L;
			foreach (var item in list)
			{
				num2 = checked(num2 + item.Size);
			}
			if ((loke.InitializeSession(num2) ? 1 : 0) != num)
			{
				foreach (var item2 in list)
				{
					using ZeroFillReadStream stream = new ZeroFillReadStream(item2.Size);
					if ((WritePartitionStream(serial, loke, item2.Entry, item2.Size, stream, num, num) ? 1 : 0) == num)
					{
						return (byte)num != 0;
					}
				}
				return (byte)result != 0;
			}
			return (byte)num != 0;
		}
		return false;
	}

	public bool FactoryResetUserdata(SerialOdin serial, LokeCommandService loke)
	{
		string item = "userdata";
		List<string> partitionNames = new List<string> { item };
		return EraseNandByPartitionNames(serial, loke, partitionNames);
	}

	public bool FactoryResetUserdata(SerialOdin serial, LokeCommandService loke, bool usePartitionNames)
	{
		if (usePartitionNames)
		{
			return FactoryResetUserdata(serial, loke);
		}
		return FactoryResetUserdata(serial, loke);
	}

	public long GetEraseSizeBytes(PitEntry partition)
	{
		object obj = null;
		int num = 0;
		if (partition == obj)
		{
			return 0L;
		}
		try
		{
			long num2 = checked(partition.BlockSize * partition.BlockCount);
			if (num2 > 0L)
			{
				return num2;
			}
		}
		catch
		{
		}
		if (partition.FileSize > num)
		{
			return partition.FileSize;
		}
		return num;
	}

	public bool WritePitBytes(SerialOdin serial, LokeCommandService loke, byte[] pitData)
	{
		return WritePitData(serial, loke, pitData);
	}

	public bool WritePitData(SerialOdin serial, LokeCommandService loke, byte[] pitData)
	{
		if (pitData == null || pitData.Length == 0)
		{
			return false;
		}
		if (loke.SendCommand(new SamsungLokeCommand(101)))
		{
			if (loke.SendCommand(new SamsungLokeCommand(101, 2, pitData.Length)))
			{
				serial.Write(pitData, pitData.Length);
				byte[] array = serial.Read(8);
				if (array.Length != 8 || array[0] == byte.MaxValue)
				{
					return false;
				}
				return loke.SendCommand(new SamsungLokeCommand(101, 3, pitData.Length));
			}
			return false;
		}
		return false;
	}

	public PitResult ReadPitTable(SerialOdin serial, LokeCommandService loke)
	{
		int isSuccess = 0;
		string errorMessage = "Failed to request PIT.";
		string errorMessage2 = "Failed to finalize PIT read.";
		try
		{
			if (loke.SendCommand(new SamsungLokeCommand(101, 1)))
			{
				uint num = Binary.ReadU32LE(loke.LastResponseBytes, 4);
				using MemoryStream memoryStream = new MemoryStream((int)num);
				int num2 = 0;
				int num3 = (int)(num / 500 + 1);
				for (int i = 0; i < num3; i++)
				{
					int num4 = (int)Math.Min(500u, num - (uint)num2);
					if (num4 > 0)
					{
						byte[] array = new byte[1024];
						array[0] = 101;
						array[4] = 2;
						Binary.WriteI32LE(array, 8, i);
						serial.Write(array, array.Length);
						byte[] array2 = serial.Read(num4);
						memoryStream.Write(array2, 0, array2.Length);
						num2 += num4;
						continue;
					}
					break;
				}
				if (loke.SendCommand(new SamsungLokeCommand(101, 3, num)))
				{
					byte[] array3 = memoryStream.ToArray();
					List<PitEntry> pitEntries = PitParser.Parse(array3);
					return new PitResult
					{
						IsSuccess = true,
						PitData = array3,
						PitEntries = pitEntries
					};
				}
				return new PitResult
				{
					IsSuccess = false,
					ErrorMessage = errorMessage2
				};
			}
			return new PitResult
			{
				IsSuccess = false,
				ErrorMessage = errorMessage
			};
		}
		catch (Exception ex)
		{
			return new PitResult
			{
				IsSuccess = ((byte)isSuccess != 0),
				ErrorMessage = ex.Message
			};
		}
	}

	public bool FlashTarEntryToPartition(SerialOdin serial, LokeCommandService loke, string tarPath, TarEntryInfo tarEntry, PitEntry partition, long rawSize, int efsClearFlag = 0, int bootUpdateFlag = 1)
	{
		string value = ".lz4";
		using FileStream innerStream = File.OpenRead(tarPath);
		using LimitedReadStream limitedReadStream = new LimitedReadStream(innerStream, tarEntry.DataStartOffset, tarEntry.Size);
		if (tarEntry.Name.EndsWith(value, StringComparison.OrdinalIgnoreCase))
		{
			using (Lz4DecodingStream stream = new Lz4DecodingStream(limitedReadStream))
			{
				return WritePartitionStream(serial, loke, partition, rawSize, stream, efsClearFlag, bootUpdateFlag);
			}
		}
		return WritePartitionStream(serial, loke, partition, rawSize, limitedReadStream, efsClearFlag, bootUpdateFlag);
	}

	public bool WritePartitionStream(SerialOdin serial, LokeCommandService loke, PitEntry partition, long size, Stream stream, int efsClearFlag = 0, int bootUpdateFlag = 1)
	{
		int num = ((partition.DeviceTypeId == 1L || partition.DeviceTypeId == 2L || partition.DeviceTypeId == 8L) ? 31457280 : 104857600);
		int num2 = (int)(size / num);
		if ((ulong)(size % num) > 0uL)
		{
			num2++;
		}
		long sentBytes = 0L;
		for (int i = 0; i < num2; i++)
		{
			int length = (int)Math.Min(num, size - sentBytes);
			bool isLast = i == num2 - 1;
			if (!SendFlashSession(serial, loke, partition, stream, length, isLast, efsClearFlag, bootUpdateFlag, ref sentBytes, size))
			{
				return false;
			}
		}
		return true;
	}

	public bool SendFlashSession(SerialOdin serial, LokeCommandService loke, PitEntry partition, Stream stream, int length, bool isLast, int efsClearFlag, int bootUpdateFlag, ref long sentBytes, long totalSize)
	{
		loke.SendCommand(new SamsungLokeCommand(102));
		loke.SendCommand(new SamsungLokeCommand(102, 2, GetAlignedSessionSize(length)));
		int num = 0;
		byte[] array = new byte[1048576];
		while (num < length)
		{
			Array.Clear(array, 0, array.Length);
			int num2 = Math.Min(array.Length, length - num);
			ReadExactly(stream, array, num2);
			serial.Write(array, array.Length);
			serial.Read(8);
			num += num2;
			sentBytes += num2;
			UI.Progresbar1(sentBytes, totalSize);
		}
		SamsungLokeCommand samsungLokeCommand = new SamsungLokeCommand(102, 3, partition.BinaryTypeId, length);
		if (partition.BinaryTypeId == 1L)
		{
			SamsungLokeCommand samsungLokeCommand2 = samsungLokeCommand.Clone();
			samsungLokeCommand2.PartitionId = (isLast ? 1 : 0);
			samsungLokeCommand2.SessionEndFlag = (int)partition.DeviceTypeId;
			samsungLokeCommand2.EfsClearFlag = (int)partition.PartitionId;
			samsungLokeCommand = samsungLokeCommand2;
		}
		else
		{
			SamsungLokeCommand samsungLokeCommand3 = samsungLokeCommand.Clone();
			samsungLokeCommand3.DeviceId = (int)partition.DeviceTypeId;
			samsungLokeCommand3.PartitionId = (int)partition.PartitionId;
			samsungLokeCommand3.SessionEndFlag = (isLast ? 1 : 0);
			samsungLokeCommand = samsungLokeCommand3;
		}
		samsungLokeCommand.BootUpdateFlag = bootUpdateFlag;
		return loke.SendCommand(samsungLokeCommand);
	}

	public bool SwitchToNormalMode(LokeCommandService loke)
	{
		int result = 0;
		try
		{
			loke.SendCommand(new SamsungLokeCommand(103));
			loke.SendCommand(new SamsungLokeCommand(103)
			{
				SequenceId = 1
			});
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public bool SwitchToNormalMode(LokeCommandService loke, SerialOdin serial)
	{
		object obj = null;
		if (!SwitchToNormalMode(loke))
		{
			return false;
		}
		if ((serial != null) ? (serial.Port == obj) : (obj == obj))
		{
			return true;
		}
		Stopwatch stopwatch = Stopwatch.StartNew();
		if (stopwatch.ElapsedMilliseconds >= 60000L)
		{
			return !serial.Port.IsOpen;
		}
		if (serial.Port.IsOpen)
		{
			Thread.Sleep(100);
			int num = 2530859;
			while (true)
			{
				_ = num ^ -1088463491;
				if (stopwatch.ElapsedMilliseconds >= 60000L)
				{
					return !serial.Port.IsOpen;
				}
				if (!serial.Port.IsOpen)
				{
					break;
				}
				Thread.Sleep(100);
				num = 2530859;
			}
			return true;
		}
		return true;
	}

	public bool SwitchToDownloadMode(LokeCommandService loke)
	{
		int result = 0;
		try
		{
			loke.SendCommand(new SamsungLokeCommand(103));
			loke.SendCommand(new SamsungLokeCommand(103)
			{
				SequenceId = 2
			});
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public bool SendSoftbrickCommands(LokeCommandService loke)
	{
		int result = 0;
		try
		{
			SamsungLokeCommand commandPacket = new SamsungLokeCommand(103);
			SamsungLokeCommand commandPacket2 = new SamsungLokeCommand(103, 1);
			return loke.SendCommand(commandPacket) && loke.SendCommand(commandPacket2);
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public bool ReadPitAndPatchPersistent(SerialOdin serial, LokeCommandService loke, string pitPath)
	{
		int num = 0;
		int num2 = 1;
		object obj = null;
		string b = "pit";
		string b2 = "persistent";
		int millisecondsTimeout = 1500;
		int millisecondsTimeout2 = 500;
		PitResult pitResult = ReadPitTable(serial, loke);
		if (pitResult.IsSuccess && pitResult.PitData != null && pitResult.PitData.Length != 0 && pitResult.PitEntries != null && pitResult.PitEntries.Count != 0)
		{
			PitEntry pitEntry = (PitEntry)obj;
			PitEntry pitEntry2 = (PitEntry)obj;
			foreach (PitEntry pitEntry3 in pitResult.PitEntries)
			{
				if (pitEntry == null && string.Equals(pitEntry3.PartitionName, b, StringComparison.OrdinalIgnoreCase))
				{
					pitEntry = pitEntry3;
				}
				if (pitEntry2 == null && string.Equals(pitEntry3.PartitionName, b2, StringComparison.OrdinalIgnoreCase))
				{
					pitEntry2 = pitEntry3;
				}
			}
			if (((pitEntry != null) ? ((pitEntry2 == obj) ? 1 : 0) : num2) == 0)
			{
				if ((string.IsNullOrWhiteSpace(pitPath) ? 1 : 0) == num)
				{
					string directoryName = Path.GetDirectoryName(pitPath);
					if (string.IsNullOrWhiteSpace(directoryName) ? ((byte)num != 0) : ((Directory.Exists(directoryName) ? 1 : 0) == num))
					{
						Directory.CreateDirectory(directoryName);
					}
					File.WriteAllBytes(pitPath, pitResult.PitData);
				}
				byte[] array = PatchPersistentEntryName(pitResult.PitData);
				if ((array != null) ? ((nint)array.LongLength == num) : ((byte)num2 != 0))
				{
					return (byte)num != 0;
				}
				CurrentPitPatch = new PitPatchContext
				{
					OriginalPitBytes = pitResult.PitData,
					PatchedPitBytes = array,
					PitPartitionEntry = pitEntry,
					PersistentPartitionEntry = pitEntry2
				};
				Thread.Sleep(millisecondsTimeout);
				using (MemoryStream memoryStream = new MemoryStream(array, (byte)num != 0))
				{
					if ((loke.InitializeSession(memoryStream.Length) ? 1 : 0) == num)
					{
						return (byte)num != 0;
					}
					Thread.Sleep(millisecondsTimeout2);
					if ((WritePartitionStream(serial, loke, pitEntry, memoryStream.Length, memoryStream, num, num) ? 1 : 0) == num)
					{
						return (byte)num != 0;
					}
				}
				Thread.Sleep(millisecondsTimeout2);
				return SwitchToDownloadMode(loke);
			}
			return (byte)num != 0;
		}
		return false;
	}

	public bool RestorePitAndClearPersistent(SerialOdin serial, LokeCommandService loke)
	{
		object obj = null;
		int result = 1;
		int num = 0;
		int millisecondsTimeout = 500;
		int num2 = 512000;
		int num3 = int.MaxValue;
		if (CurrentPitPatch == null || CurrentPitPatch.OriginalPitBytes == null || CurrentPitPatch.PitPartitionEntry == null || CurrentPitPatch.PersistentPartitionEntry == obj)
		{
			return false;
		}
		using (MemoryStream memoryStream = new MemoryStream(CurrentPitPatch.OriginalPitBytes, writable: false))
		{
			if (!loke.InitializeSession(memoryStream.Length))
			{
				return false;
			}
			Thread.Sleep(500);
			if (!WritePartitionStream(serial, loke, CurrentPitPatch.PitPartitionEntry, memoryStream.Length, memoryStream, 0, 0))
			{
				return false;
			}
		}
		int num4 = num2;
		try
		{
			long num5 = checked(CurrentPitPatch.PersistentPartitionEntry.BlockSize * CurrentPitPatch.PersistentPartitionEntry.BlockCount);
			if ((num5 <= num) ? ((byte)num != 0) : (((num5 > num3) ? 1 : 0) == num))
			{
				num4 = (int)num5;
			}
			else if ((CurrentPitPatch.PersistentPartitionEntry.FileSize > num) ? (((CurrentPitPatch.PersistentPartitionEntry.FileSize > num3) ? 1 : 0) == num) : ((byte)num != 0))
			{
				num4 = (int)CurrentPitPatch.PersistentPartitionEntry.FileSize;
			}
		}
		catch
		{
		}
		Thread.Sleep(millisecondsTimeout);
		using (MemoryStream memoryStream2 = new MemoryStream(new byte[num4], (byte)num != 0))
		{
			if ((loke.InitializeSession(memoryStream2.Length) ? 1 : 0) == num)
			{
				return (byte)num != 0;
			}
			Thread.Sleep(millisecondsTimeout);
			if ((WritePartitionStream(serial, loke, CurrentPitPatch.PersistentPartitionEntry, memoryStream2.Length, memoryStream2, num, num) ? 1 : 0) == num)
			{
				return (byte)num != 0;
			}
		}
		Thread.Sleep(millisecondsTimeout);
		SwitchToNormalMode(loke);
		return (byte)result != 0;
	}

	public byte[] PatchPersistentEntryName(byte[] data)
	{
		string hex = "70 65 72 73 69 73 74 65 6E 74 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
		string hex2 = "70 65 72 73 69 73 74 65 6E 74 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 70 65 72 73 69 73 74 65 6E 74 2E 69 6D 67";
		string hex3 = "50 45 52 53 49 53 54 45 4E 54 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
		string hex4 = "50 45 52 53 49 53 54 45 4E 54 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 45 52 53 49 53 54 45 4E 54 2E 69 6D 67";
		object result = null;
		byte[] array = hex.HexToBytes();
		byte[] replacement = hex2.HexToBytes();
		byte[] array2 = hex3.HexToBytes();
		byte[] replacement2 = hex4.HexToBytes();
		int num = data.IndexOf(array);
		if (num < 0)
		{
			num = data.IndexOf(array2);
			if (num >= 0)
			{
				return data.Replace(num, array2.Length, replacement2);
			}
			return (byte[])result;
		}
		return data.Replace(num, array.Length, replacement);
	}

	public bool IsOdinDevice(SerialOdin serial)
	{
		string value = "LOKE";
		byte[] array = new byte[4] { 79, 68, 73, 78 };
		serial.Write(array, array.Length);
		Thread.Sleep(400);
		string text = serial.ReadExisting();
		return text.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
	}

	public void ReadExactly(Stream stream, byte[] targetBuffer, int count)
	{
		string message = "Cannot read enough data from source stream.";
		int num = 0;
		if (0 >= count)
		{
			return;
		}
		int num2 = stream.Read(targetBuffer, 0, count);
		if (num2 > 0)
		{
			num = 0 + num2;
			int num3 = 754168685;
			while (true)
			{
				_ = num3 ^ 0x49099ABB;
				if (num < count)
				{
					num2 = stream.Read(targetBuffer, num, count - num);
					if (num2 > 0)
					{
						num += num2;
						num3 = 754168685;
						continue;
					}
					throw new EndOfStreamException(message);
				}
				break;
			}
			return;
		}
		throw new EndOfStreamException(message);
	}

	private long GetAlignedSessionSize(int sessionLength)
	{
		return (sessionLength - 1L >> 17) + 1L << 17;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}

