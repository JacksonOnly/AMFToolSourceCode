using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Restored.Namespace11;
using Restored.Namespace12;

namespace AMFTool.Library.Hisilicon;

public class UpdateFile : IEnumerable<UpdateEntry>, IEnumerable
{
	public delegate EntryType IdentifyEntry(UpdateEntry h);

	private delegate void Action<in T, in TU>(T t, TU tu);

	private const long SkipBytes = 92L;

	private readonly string _fileName;

	private List<UpdateEntry> _entries;

	private List<UpdateEntry> Entries => _entries ?? (_entries = new List<UpdateEntry>());

	public UpdateEntry this[int index] => Entries[index];

	public int Count => Entries.Count;

	public override string ToString()
	{
		return _fileName;
	}

	private UpdateFile(string fileName, bool checksum, IdentifyEntry identify)
	{
		_fileName = fileName;
		LoadEntries(checksum, identify);
	}

	private UpdateFile(string fileName)
	{
		_fileName = fileName;
		CreateFile();
	}

	private void LoadEntries(bool checksum, IdentifyEntry identify)
	{
		object obj = null;
		using FileStream fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
		fileStream.Seek(92L, SeekOrigin.Begin);
		while (fileStream.Position < fileStream.Length)
		{
			UpdateEntry updateEntry = UpdateEntry.Read(fileStream, checksum);
			Entries.Add(updateEntry);
			if (identify !=null)
			{
				updateEntry.Type = identify(updateEntry);
			}
			fileStream.Seek(updateEntry.FileSize, SeekOrigin.Current);
			uint num = Class35.method_05(updateEntry);
			if (num < 4)
			{
				fileStream.Seek(num, SeekOrigin.Current);
			}
		}
	}

	private void CreateFile()
	{
		using FileStream fileStream = new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.None);
		byte[] array = new byte[92L];
		fileStream.Write(array, 0, array.Length);
	}

	public static UpdateFile Open(string fileName, bool checksum = true, IdentifyEntry identify = null)
	{
		return new UpdateFile(fileName, checksum, identify);
	}

	public static UpdateFile Create(string fileName)
	{
		return new UpdateFile(fileName);
	}

	public void Extract(int index, string output, bool checksum = true)
	{
		Extract(Entries[index], output, checksum);
	}

	public void Extract(UpdateEntry entry, string output, bool checksum = true)
	{
		entry.Extract(_fileName, output, checksum);
	}

	public void Extract(int index, Stream output, bool checksum = true)
	{
		Extract(Entries[index], output, checksum);
	}

	public void Extract(UpdateEntry entry, Stream output, bool checksum = true)
	{
		entry.Extract(_fileName, output, checksum);
	}

	public void Add(UpdateEntry entry, Stream stream)
	{
		entry.FileSize = (uint)stream.Length;
		uint num = entry.FileSize / entry.BlockSize;
		if (entry.FileSize % entry.BlockSize != 0)
		{
			num++;
		}
		entry.CheckSumTable = new ushort[num];
		entry.HeaderSize = (uint)(Struct3.field_14 + num * 2);
		entry.ComputeHeaderChecksum();
		using (FileStream fileStream = new FileStream(_fileName, FileMode.Append, FileAccess.Write, FileShare.None))
		{
			byte[] header = entry.GetHeader();
			fileStream.Write(header, 0, header.Length);
			fileStream.Seek(num * 2, SeekOrigin.Current);
			entry.DataOffset = fileStream.Position;
			byte[] buffer = new byte[entry.BlockSize];
			int num2 = 0;
			int count;
			while ((count = stream.Read(buffer, 0, entry.BlockSize)) > 0)
			{
				entry.CheckSumTable[num2] = Class35.field_02.ComputeSum(buffer, 0, count);
				fileStream.Write(buffer, 0, count);
				num2++;
			}
			fileStream.Seek(-(stream.Length + num * 2), SeekOrigin.Current);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			for (int i = 0; i < entry.CheckSumTable.Length; i++)
			{
				binaryWriter.Write(entry.CheckSumTable[i]);
			}
			fileStream.Seek(stream.Length, SeekOrigin.Current);
			uint num3 = Class35.method_05(entry);
			if (num3 < 4)
			{
				binaryWriter.Write(new byte[num3]);
			}
		}
		Entries.Add(entry);
	}

	public void Add(UpdateEntry entry, string fileName)
	{
		using FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
		Add(entry, stream);
	}

	private void ProcessData(int blockSize, EntryType entryType, Action<byte[], int> action)
	{
		byte[] array = new byte[blockSize];
		using FileStream fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
		foreach (UpdateEntry item in Entries.FindAll((UpdateEntry e) => (entryType & e.Type) == e.Type))
		{
			fileStream.Seek(item.DataOffset, SeekOrigin.Begin);
			PartialStream partialStream = new PartialStream(fileStream, item.FileSize);
			int tu;
			while ((tu = partialStream.Read(array, 0, blockSize)) > 0)
			{
				action(array, tu);
			}
		}
	}

	public void AddChecksum(UpdateEntry entry, int blockSize = 32768)
	{
		Entries.FindAll((UpdateEntry e) => e.Type == EntryType.Checksum).ForEach(delegate(UpdateEntry e)
		{
			Remove(e);
		});
		entry.Type = EntryType.Checksum;
		List<byte> result = new List<byte>();
		ProcessData(blockSize, (EntryType)0, delegate(byte[] bytes, int i)
		{
			result.AddRange(Class35.field_02.ComputeHash(bytes, 0, i));
		});
		using MemoryStream stream = new MemoryStream(result.ToArray());
		Add(entry, stream);
	}

	private static void MoveData(Stream stream, long from, long to, long length, int blockSize)
	{
		string format = "Failed to read from stream @{0:X16}: expected {1} byte(s), got {2} byte(s)";
		long num = from;
		long num2 = to;
		long num3 = from + length - num;
		int num4 = Convert.ToInt32(Math.Min(blockSize, num3));
		byte[] buffer = new byte[num3];
		if (num4 == 0)
		{
			return;
		}
		stream.Seek(num, SeekOrigin.Begin);
		int num5 = stream.Read(buffer, 0, num4);
		int num6 = 1381202439;
		while (true)
		{
			_ = num6 ^ -623062271;
			if (num5 != num4)
			{
				break;
			}
			stream.Seek(num2, SeekOrigin.Begin);
			stream.Write(buffer, 0, num5);
			num += num5;
			num2 += num5;
			num3 = from + length - num;
			num4 = Convert.ToInt32(Math.Min(blockSize, num3));
			if (num4 == 0)
			{
				return;
			}
			stream.Seek(num, SeekOrigin.Begin);
			num5 = stream.Read(buffer, 0, num4);
			num6 = 1381202439;
		}
		throw new Exception(string.Format(format, stream.Position - num5, num4, num5));
	}

	public void Remove(UpdateEntry entry, int blockSize = 32768)
	{
		uint num = entry.HeaderSize + entry.FileSize;
		uint num2 = Class35.method_05(entry);
		if (num2 >= 4)
		{
			num2 = 0u;
		}
		num += num2;
		long num3 = entry.DataOffset - entry.HeaderSize;
		long num4 = entry.DataOffset + entry.FileSize + num2;
		using (FileStream fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
		{
			MoveData(fileStream, num4, num3, fileStream.Length - num3 - num, blockSize);
			fileStream.SetLength(fileStream.Length - num);
		}
		Entries.Remove(entry);
		foreach (UpdateEntry item in Entries.FindAll((UpdateEntry e) => e.DataOffset > entry.DataOffset))
		{
			item.DataOffset -= num;
		}
	}

	public void Remove(int index, int blockSize = 32768)
	{
		Remove(Entries[index], blockSize);
	}

	public IEnumerator<UpdateEntry> GetEnumerator()
	{
		return Entries.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return method_02();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	public IEnumerator<UpdateEntry> method_02()
	{
		return GetEnumerator();
	}
}
