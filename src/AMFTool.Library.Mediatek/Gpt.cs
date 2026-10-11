using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AMFTool.Library.Mediatek;

public class Gpt
{
	public class GptHeader
	{
		public byte[] Signature;

		public uint Revision;

		public uint HeaderSize;

		public uint Crc32;

		public uint Reserved;

		public ulong CurrentLba;

		public ulong BackupLba;

		public ulong FirstUsableLba;

		public ulong LastUsableLba;

		public byte[] DiskGuid;

		public ulong PartEntryStartLba;

		public uint NumPartEntries;

		public uint PartEntrySize;

		public GptHeader(byte[] indata)
		{
			StructHelper structHelper = new StructHelper(indata);
			Signature = structHelper.Bytes(8);
			Revision = structHelper.Dword();
			HeaderSize = structHelper.Dword();
			Crc32 = structHelper.Dword();
			Reserved = structHelper.Dword();
			CurrentLba = structHelper.Qword();
			BackupLba = structHelper.Qword();
			FirstUsableLba = structHelper.Qword();
			LastUsableLba = structHelper.Qword();
			DiskGuid = structHelper.Bytes(16);
			PartEntryStartLba = structHelper.Qword();
			NumPartEntries = structHelper.Dword();
			PartEntrySize = structHelper.Dword();
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public class GptPartition
	{
		public byte[] Type;

		public byte[] Unique;

		public ulong FirstLba;

		public ulong LastLba;

		public ulong Flags;

		public byte[] Name;

		public GptPartition(byte[] indata)
		{
			StructHelper structHelper = new StructHelper(indata);
			Type = structHelper.Bytes(16);
			Unique = structHelper.Bytes(16);
			FirstLba = structHelper.Qword();
			LastLba = structHelper.Qword();
			Flags = structHelper.Qword();
			Name = structHelper.Bytes(72);
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	private int NumPartEntries;

	private int PartEntrySize;

	private int PartEntryStartLba;

	public List<Partf> PartEntries;

	private ulong TotalSectors;

	private GptHeader Header;

	private int SectorSize;

	public Gpt(int numPartEntries = 0, int partEntrySize = 0, int partEntryStartLba = 0)
	{
		NumPartEntries = numPartEntries;
		PartEntrySize = partEntrySize;
		PartEntryStartLba = partEntryStartLba;
		PartEntries = new List<Partf>();
	}

	public void ParseBpi(byte[] gptdata, int pagesize = 512)
	{
		int num = 0;
		int num2 = 128;
		int num3 = 16;
		int count = 4;
		int count2 = 20;
		int count3 = 2;
		int count4 = 22;
		int count5 = 24;
		int count6 = 26;
		int count7 = 6;
		int count8 = 32;
		int count9 = 8;
		int count10 = 40;
		int num4 = 1;
		int count11 = 48;
		int count12 = 56;
		int count13 = 72;
		string oldValue = "\0\0";
		string newValue = "";
		SectorSize = pagesize;
		TotalSectors = 0uL;
		PartEntries = new List<Partf>();
		for (int i = 2048; i < gptdata.Length; i += num2)
		{
			byte[] array = gptdata.Skip(i).Take(num2).ToArray();
			if (Binary.ReadU32LE(array, num3) == (uint)num)
			{
				break;
			}
			using MemoryStream memoryStream = new MemoryStream(gptdata);
			Partf partf = new Partf();
			memoryStream.Seek(num3, (SeekOrigin)num);
			byte[] b = gptdata.Skip(num3).Take(count).Reverse()
				.Concat(gptdata.Skip(count2).Take(count3).Reverse()
					.Concat(gptdata.Skip(count4).Take(count3).Reverse()
						.Concat(gptdata.Skip(count5).Take(count3).Reverse()
							.Concat(gptdata.Skip(count6).Take(count7)))))
				.ToArray();
			partf.Unique = new Guid(b);
			partf.FirstLba = Binary.ReadU64LE(gptdata.Skip(count8).Take(count9).ToArray(), num);
			partf.LastLba = Binary.ReadU64LE(gptdata.Skip(count10).Take(count9).ToArray(), num);
			partf.Sector = partf.FirstLba;
			partf.Sectors = partf.LastLba - partf.FirstLba + (ulong)num4;
			partf.Flags = Binary.ReadU64LE(gptdata.Skip(count11).Take(count9).ToArray(), num);
			partf.Name = Encoding.Unicode.GetString(gptdata.Skip(count12).Take(count13).ToArray()).Replace(oldValue, newValue).TrimEnd(new char[num4]);
			partf.Type = new byte[num4];
			if (partf.LastLba > TotalSectors)
			{
				TotalSectors = partf.LastLba;
			}
			PartEntries.Add(partf);
		}
	}

	public GptHeader ParseHeader(byte[] gptdata, int sector_size = 512)
	{
		return new GptHeader(gptdata.Skip(sector_size).Take(92).ToArray());
	}

	public bool Parse(byte[] gptdata, int sector_size = 512)
	{
		object obj = null;
		string s = "EFI PART";
		string s2 = "BPI\0";
		string format = "{0:X}";
		string oldValue = "\0\0";
		string newValue = "";
		string text = "EFI_UNUSED";
		if (PartEntries != obj)
		{
			PartEntries.Clear();
		}
		Header = new GptHeader(gptdata.Skip(sector_size).Take(92).ToArray());
		SectorSize = sector_size;
		if (!((ReadOnlySpan<byte>)Header.Signature).SequenceEqual((ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(s)))
		{
			if (gptdata.Take(4).SequenceEqual(Encoding.UTF8.GetBytes(s2)))
			{
				return true;
			}
			return false;
		}
		if (Header.Revision == 65536)
		{
			ulong num = ((PartEntryStartLba == 0) ? (Header.PartEntryStartLba * (ulong)sector_size) : ((ulong)PartEntryStartLba));
			uint partEntrySize = Header.PartEntrySize;
			PartEntries = new List<Partf>();
			for (int i = 0; i < Header.NumPartEntries; i++)
			{
				byte[] array = gptdata.Skip((int)num + i * (int)partEntrySize).Take((int)partEntrySize).ToArray();
				if (Binary.ReadU64LE(array.Skip(16).Take(16).ToArray()) == 0L)
				{
					break;
				}
				GptPartition gptPartition = new GptPartition(array);
				Partf partf = new Partf();
				byte[] b = gptPartition.Unique.Take(4).Reverse().Concat(gptPartition.Unique.Skip(4).Take(2).Reverse()
					.Concat(gptPartition.Unique.Skip(6).Take(2).Reverse()
						.Concat(gptPartition.Unique.Skip(8).Take(2).Concat(gptPartition.Unique.Skip(10).ToArray()))))
					.ToArray();
				partf.Unique = new Guid(b);
				partf.Sector = gptPartition.FirstLba;
				partf.Sectors = gptPartition.LastLba - gptPartition.FirstLba + 1L;
				partf.Flags = gptPartition.Flags;
				uint num2 = Binary.ReadU32LE(gptPartition.Type.Take(4).ToArray());
				if (string.IsNullOrEmpty(Enum.GetName(typeof(EfiType), num2)))
				{
					partf.Type = Encoding.ASCII.GetBytes(string.Format(format, num2));
				}
				else
				{
					partf.Type = Encoding.ASCII.GetBytes(Enum.GetName(typeof(EfiType), num2));
				}
				partf.Name = Encoding.Unicode.GetString(gptPartition.Name).Replace(oldValue, newValue).TrimEnd(default(char));
				if (!(Encoding.ASCII.GetString(partf.Type) == text))
				{
					PartEntries.Add(partf);
				}
			}
			TotalSectors = Header.LastUsableLba + 34L;
			if (PartEntries.Count <= 0)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
