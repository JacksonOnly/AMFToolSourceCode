using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMFTool.Library.Mediatek;

namespace AMFTool.Library.Qualcomm;

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

	public int NumPartEntries;

	public int PartEntrySize;

	public int PartEntryStartLba;

	public ulong? TotalSectors;

	public GptHeader Header;

	public int? SectorSize;

	public List<Partf> PartEntries;

	private const int AbPartitionAttrSlotActive = 4;

	private const int AbPartitionAttrUnbootable = 128;

	public Gpt(int NumPartEntries = 0, int PartEntrySize = 0, int PartEntryStartLba = 0)
	{
		object header = null;

		this.NumPartEntries = NumPartEntries;
		this.PartEntrySize = PartEntrySize;
		this.PartEntryStartLba = PartEntryStartLba;
		TotalSectors = null;
		Header = (GptHeader)header;
		SectorSize = null;
		PartEntries = new List<Partf>();
	}

	public GptHeader ParseHeader(byte[] gptdata, int SectorSize = 512)
	{
		return new GptHeader(gptdata.Skip(SectorSize).Take(92).ToArray());
	}

	public bool Parse(byte[] gptdata, int Lun, int SectorSize)
	{
		string s = "EFI PART";
		string oldValue = "\0\0";
		string text = "";
		string format = "{0}_{1}";
		string format2 = "{0}";
		string format3 = "{0}.0";
		string format4 = "0x{0:X}";
		string text2 = "_a";
		string value = "slot: a";
		string value2 = "slot: b";
		string format5 = "gpt_main{0}.bin";
		string label = "PrimaryGPT";
		string numPartitionSectors = "34";
		string sizeInKB = "17.0";
		string startByteHex = "0x0";
		string startSector = "0";
		string format6 = "gpt_backup{0}.bin";
		string label2 = "BackupGPT";
		string numPartitionSectors2 = "33";
		string sizeInKB2 = "16.5";
		string startByteHex2 = "(512*NUM_DISK_SECTORS)-16896.";
		string startSector2 = "NUM_DISK_SECTORS-33.";
		string numPartitionSectors3 = "6";
		string sizeInKB3 = "24.0";
		string numPartitionSectors4 = "5";
		string sizeInKB4 = "20.0";
		string startByteHex3 = "(4096*NUM_DISK_SECTORS)-20480.";
		string startSector3 = "NUM_DISK_SECTORS-5.";
		GptHeader gptHeader = new GptHeader(gptdata.Skip(SectorSize).Take(92).ToArray());
		this.SectorSize = SectorSize;
		if (((ReadOnlySpan<byte>)gptHeader.Signature).SequenceEqual((ReadOnlySpan<byte>)Encoding.ASCII.GetBytes(s)))
		{
			if (gptHeader.Revision != 65536)
			{
				return false;
			}
			int num = ((PartEntryStartLba == 0) ? ((int)gptHeader.PartEntryStartLba * SectorSize) : PartEntryStartLba);
			int partEntrySize = (int)gptHeader.PartEntrySize;
			PartEntries = new List<Partf>();
			int numPartEntries = (int)gptHeader.NumPartEntries;
			for (int i = 0; i < numPartEntries; i++)
			{
				byte[] array = gptdata.Skip(num + i * partEntrySize).Take(num + i * partEntrySize + partEntrySize).ToArray();
				if (Binary.ReadI32LE(array.Skip(16).Take(16).ToArray()) == 0)
				{
					break;
				}
				GptPartition gptPartition = new GptPartition(array);
				Partf partf = new Partf
				{
					Sector = gptPartition.FirstLba,
					Sectors = gptPartition.LastLba - gptPartition.FirstLba + 1L,
					Lun = Lun
				};
				partf.SizeInKB = partf.Sectors * (uint)SectorSize / 1024L;
				partf.Flags = gptPartition.Flags;
				partf.Guid = new Guid(gptPartition.Type);
				partf.Name = Encoding.Unicode.GetString(gptPartition.Name).Replace(oldValue, text).TrimEnd(default(char));
				for (int j = 0; j < GptEfi.Uefi.Count; j++)
				{
					if (partf.Name == GptEfi.Uefi[j].Label)
					{
						partf.Name = string.Format(format, partf.Name, Lun);
					}
				}
				GptEfi.Uefi.Add(new GptEfi(SectorSize, 0, Scatter.GetQcFiles(partf.Name) ?? text, partf.Name, string.Format(format2, partf.Sectors), PartOfSingleImage: false, Lun, ReadBackVerify: false, string.Format(format3, partf.SizeInKB), Sparse: false, string.Format(format4, partf.Sector * (uint)SectorSize), string.Format(format2, partf.Sector), partf.Guid));
				PartEntries.Add(partf);
				if (partf.Name.Substring(partf.Name.Length - 2) == text2)
				{
					switch ((partf.Flags >> 48) & 0xFL)
					{
					case 0uL:
						Console.WriteLine(value2);
						break;
					case 4uL:
						Console.WriteLine(value);
						break;
					}
				}
			}
			TotalSectors = gptHeader.FirstUsableLba + gptHeader.LastUsableLba;
			if (PartEntries.Count > 0)
			{
				switch (SectorSize)
				{
				case 4096:
					GptEfi.Uefi.Add(new GptEfi(SectorSize, 0, string.Format(format5, Lun), label, numPartitionSectors3, PartOfSingleImage: true, Lun, ReadBackVerify: false, sizeInKB3, Sparse: false, startByteHex, startSector, new Guid(new byte[16])));
					GptEfi.Uefi.Add(new GptEfi(SectorSize, 0, string.Format(format6, Lun), label2, numPartitionSectors4, PartOfSingleImage: true, Lun, ReadBackVerify: false, sizeInKB4, Sparse: false, startByteHex3, startSector3, new Guid(new byte[16])));
					break;
				case 512:
					GptEfi.Uefi.Add(new GptEfi(SectorSize, 0, string.Format(format5, Lun), label, numPartitionSectors, PartOfSingleImage: true, Lun, ReadBackVerify: false, sizeInKB, Sparse: false, startByteHex, startSector, new Guid(new byte[16])));
					GptEfi.Uefi.Add(new GptEfi(SectorSize, 0, string.Format(format6, Lun), label2, numPartitionSectors2, PartOfSingleImage: true, Lun, ReadBackVerify: false, sizeInKB2, Sparse: false, startByteHex2, startSector2, new Guid(new byte[16])));
					break;
				}
				return true;
			}
			return false;
		}
		return false;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
