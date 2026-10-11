using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AMFTool;

public static class LogicalPartitionSuper
{
	public sealed class LogicalPartition
	{
		public string Name;

		public uint Attributes;

		public uint GroupIndex;

		public long TotalSize;

		public List<LogicalExtent> Extents;
	}

	public struct LogicalExtent
	{
		public long LogicalOffset;

		public long Size;

		public uint TargetType;

		public uint TargetSource;

		public long PhysicalOffset;
	}

	private struct Geometry
	{
		public uint Magic;

		public uint StructSize;

		public uint MetadataMaxSize;

		public uint MetadataSlotCount;

		public uint LogicalBlockSize;
	}

	private struct Header
	{
		public uint Magic;

		public ushort Major;

		public ushort Minor;

		public uint HeaderSize;

		public uint TablesSize;
	}

	private struct TableDesc
	{
		public uint Offset;

		public uint NumEntries;

		public uint EntrySize;
	}

	private struct BlockDeviceEntry
	{
		public string Name;

		public uint Flags;

		public ulong FirstLogicalSector;

		public ulong Size;

		public uint Alignment;

		public uint AlignmentOffset;
	}

	private struct PartitionEntry
	{
		public string Name;

		public uint Attributes;

		public uint FirstExtentIndex;

		public uint NumExtents;

		public uint GroupIndex;
	}

	private struct ExtentEntry
	{
		public ulong NumSectors;

		public uint TargetType;

		public ulong TargetData;

		public uint TargetSource;
	}

	public static bool IsSuperPartition(Stream stream)
	{
		int result = 0;
		if (stream == null || !stream.CanRead || !stream.CanSeek)
		{
			return false;
		}
		long position = 0L;
		try
		{
			position = stream.Position;
			Geometry geometry = ParseGeometryBlock(stream);
			if (geometry.Magic != 1634485351)
			{
				return false;
			}
			if (geometry.StructSize != 0 && geometry.StructSize <= 4096)
			{
				if (geometry.MetadataSlotCount == 0 || geometry.MetadataSlotCount > 64)
				{
					return false;
				}
				if (geometry.MetadataMaxSize == 0 || geometry.MetadataMaxSize % 512 != 0)
				{
					return false;
				}
				if (geometry.LogicalBlockSize != 0 && geometry.LogicalBlockSize % 512 == 0)
				{
					return true;
				}
				return false;
			}
			return false;
		}
		catch
		{
			return (byte)result != 0;
		}
		finally
		{
			try
			{
				stream.Position = position;
			}
			catch
			{
			}
		}
	}

	public static List<LogicalPartition> ParseLogicalPartitions(Stream stream, int slot = 0)
	{
		object obj = null;
		string paramName = "stream";
		string message = "Stream must be readable and seekable.";
		string message2 = "Invalid LP geometry magic.";
		string message3 = "Invalid slot count.";
		string paramName2 = "slot";
		string message4 = "Metadata max size not aligned.";
		string message5 = "No valid metadata header found.";
		string message6 = "Metadata header+tables exceed slot size.";
		string name = "partition";
		string name2 = "extent";
		string name3 = "group";
		string name4 = "block_device";
		string message7 = "Extent index out of range.";
		string message8 = "Extent target source out of range.";
		if (stream != obj)
		{
			if (!stream.CanRead || !stream.CanSeek)
			{
				throw new ArgumentException(message);
			}
			long position = stream.Position;
			try
			{
				Geometry geo = ParseGeometryBlock(stream);
				if (geo.Magic == 1634485351)
				{
					if (geo.MetadataSlotCount == 0)
					{
						throw new InvalidDataException(message3);
					}
					if ((uint)slot < geo.MetadataSlotCount)
					{
						if (geo.MetadataMaxSize != 0 && geo.MetadataMaxSize % 512 == 0)
						{
							ComputeSlotHeaderOffsets(geo, slot, out var off, out var off2);
							long num;
							if (!ParseMetadataHeader(stream, off, out var hdr, out var partTab, out var extTab, out var grpTab, out var devTab))
							{
								if (!ParseMetadataHeader(stream, off2, out hdr, out partTab, out extTab, out grpTab, out devTab))
								{
									throw new InvalidDataException(message5);
								}
								num = off2;
							}
							else
							{
								num = off;
							}
							if (hdr.HeaderSize + hdr.TablesSize <= geo.MetadataMaxSize)
							{
								ValidateTableDescriptor(partTab, hdr.TablesSize, 52u, name);
								ValidateTableDescriptor(extTab, hdr.TablesSize, 24u, name2);
								ValidateTableDescriptor(grpTab, hdr.TablesSize, 48u, name3);
								ValidateTableDescriptor(devTab, hdr.TablesSize, 24u, name4);
								long tabBase = checked(num + hdr.HeaderSize);
								List<PartitionEntry> list = ParsePartitionTable(stream, tabBase, partTab);
								List<ExtentEntry> list2 = ParseExtentTable(stream, tabBase, extTab);
								List<BlockDeviceEntry> list3 = ParseBlockDeviceTable(stream, tabBase, devTab);
								List<LogicalPartition> list4 = new List<LogicalPartition>(list.Count);
								ValidateMetadataLayout(geo, list3);
								for (int i = 0; i < list.Count; i++)
								{
									PartitionEntry partitionEntry = list[i];
									LogicalPartition logicalPartition = new LogicalPartition
									{
										Name = partitionEntry.Name,
										Attributes = partitionEntry.Attributes,
										GroupIndex = partitionEntry.GroupIndex,
										Extents = new List<LogicalExtent>((int)((partitionEntry.NumExtents > int.MaxValue) ? int.MaxValue : partitionEntry.NumExtents))
									};
									long num2 = 0L;
									long num3 = 0L;
									if (partitionEntry.NumExtents != 0)
									{
										ulong num4 = partitionEntry.FirstExtentIndex;
										ulong num5 = num4 + partitionEntry.NumExtents;
										if (num5 > (ulong)list2.Count)
										{
											throw new InvalidDataException(message7);
										}
										for (ulong num6 = num4; num6 < num5; num6++)
										{
											ExtentEntry extentEntry = list2[(int)num6];
											checked
											{
												long num7 = (long)extentEntry.NumSectors * 512L;
												long physicalOffset = -1L;
												if (extentEntry.TargetType == 0)
												{
													long num8 = 0L;
													if (list3.Count > 0)
													{
														BlockDeviceEntry blockDeviceEntry;
														unchecked
														{
															if (extentEntry.TargetSource >= (uint)list3.Count)
															{
																throw new InvalidDataException(message8);
															}
															blockDeviceEntry = list3[(int)extentEntry.TargetSource];
														}
														if (extentEntry.TargetData < blockDeviceEntry.FirstLogicalSector)
														{
															num8 = (long)blockDeviceEntry.FirstLogicalSector;
														}
													}
													physicalOffset = (num8 + (long)extentEntry.TargetData) * 512L;
												}
												LogicalExtent item = new LogicalExtent
												{
													LogicalOffset = num2,
													Size = num7,
													TargetType = extentEntry.TargetType,
													TargetSource = extentEntry.TargetSource,
													PhysicalOffset = physicalOffset
												};
												logicalPartition.Extents.Add(item);
												num2 += num7;
												num3 += num7;
											}
										}
										logicalPartition.TotalSize = num3;
										list4.Add(logicalPartition);
									}
									else
									{
										logicalPartition.TotalSize = 0L;
										list4.Add(logicalPartition);
									}
								}
								return list4;
							}
							throw new InvalidDataException(message6);
						}
						throw new InvalidDataException(message4);
					}
					throw new ArgumentOutOfRangeException(paramName2);
				}
				throw new InvalidDataException(message2);
			}
			finally
			{
				try
				{
					stream.Position = position;
				}
				catch
				{
				}
			}
		}
		throw new ArgumentNullException(paramName);
	}

	private static Geometry ParseGeometryBlock(Stream stream)
	{
		byte[] array = new byte[4096];
		stream.Seek(4096L, SeekOrigin.Begin);
		ReadFullyOrThrow(stream, array, 4096);
		int offset = 0;
		Geometry result = new Geometry
		{
			Magic = Binary.ReadU32LE(array, ref offset),
			StructSize = Binary.ReadU32LE(array, ref offset)
		};
		offset += 32;
		result.MetadataMaxSize = Binary.ReadU32LE(array, ref offset);
		result.MetadataSlotCount = Binary.ReadU32LE(array, ref offset);
		result.LogicalBlockSize = Binary.ReadU32LE(array, ref offset);
		return result;
	}

	private static void ComputeSlotHeaderOffsets(Geometry geo, int slot, out long off0, out long off1)
	{
		checked
		{
			long num = geo.MetadataMaxSize * slot;
			off0 = 12288L + num;
			off1 = 12288L + unchecked((long)geo.MetadataMaxSize) * unchecked((long)geo.MetadataSlotCount) + num;
		}
	}

	private static bool ParseMetadataHeader(Stream stream, long headerOffset, out Header hdr, out TableDesc partTab, out TableDesc extTab, out TableDesc grpTab, out TableDesc devTab)
	{
		int result = 0;
		hdr = default(Header);
		partTab = default(TableDesc);
		extTab = default(TableDesc);
		grpTab = default(TableDesc);
		devTab = default(TableDesc);
		try
		{
			byte[] array = new byte[80];
			stream.Seek(headerOffset, SeekOrigin.Begin);
			if (!ReadFully(stream, array, array.Length))
			{
				return false;
			}
			int offset = 0;
			hdr.Magic = Binary.ReadU32LE(array, ref offset);
			if (hdr.Magic != 1095520304)
			{
				return false;
			}
			hdr.Major = Binary.ReadU16LE(array, ref offset);
			hdr.Minor = Binary.ReadU16LE(array, ref offset);
			hdr.HeaderSize = Binary.ReadU32LE(array, ref offset);
			offset += 32;
			hdr.TablesSize = Binary.ReadU32LE(array, ref offset);
			if (hdr.HeaderSize >= 128 && hdr.HeaderSize <= 4096)
			{
				if (hdr.TablesSize > 134217728)
				{
					return false;
				}
				byte[] array2 = new byte[12];
				if (ReadFully(stream, array2, 12))
				{
					partTab = ParseTableDescriptor(array2);
					if (ReadFully(stream, array2, 12))
					{
						extTab = ParseTableDescriptor(array2);
						if (ReadFully(stream, array2, 12))
						{
							grpTab = ParseTableDescriptor(array2);
							if (ReadFully(stream, array2, 12))
							{
								devTab = ParseTableDescriptor(array2);
								if (partTab.EntrySize == 0 || extTab.EntrySize == 0)
								{
									return false;
								}
								return true;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	private static TableDesc ParseTableDescriptor(byte[] buff)
	{
		int offset = 0;
		return new TableDesc
		{
			Offset = Binary.ReadU32LE(buff, ref offset),
			NumEntries = Binary.ReadU32LE(buff, ref offset),
			EntrySize = Binary.ReadU32LE(buff, ref offset)
		};
	}

	private static List<PartitionEntry> ParsePartitionTable(Stream stream, long tabBase, TableDesc tab)
	{
		if (tab.NumEntries == 0)
		{
			return new List<PartitionEntry>(0);
		}
		int num;
		byte[] array;
		checked
		{
			num = (int)tab.EntrySize;
			array = new byte[num];
			long offset = tabBase + tab.Offset;
			stream.Seek(offset, SeekOrigin.Begin);
		}
		List<PartitionEntry> list = new List<PartitionEntry>((int)Math.Min(tab.NumEntries, 2147483647u));
		for (uint num2 = 0u; num2 < tab.NumEntries; num2++)
		{
			ReadFullyOrThrow(stream, array, num);
			string text = DecodeAsciiName(array, 0, 36);
			if (text.Length != 0)
			{
				int offset2 = 36;
				uint attributes = Binary.ReadU32LE(array, ref offset2);
				uint firstExtentIndex = Binary.ReadU32LE(array, ref offset2);
				uint numExtents = Binary.ReadU32LE(array, ref offset2);
				uint groupIndex = Binary.ReadU32LE(array, ref offset2);
				list.Add(new PartitionEntry
				{
					Name = text,
					Attributes = attributes,
					FirstExtentIndex = firstExtentIndex,
					NumExtents = numExtents,
					GroupIndex = groupIndex
				});
			}
		}
		return list;
	}

	private static List<ExtentEntry> ParseExtentTable(Stream stream, long tabBase, TableDesc tab)
	{
		if (tab.NumEntries == 0)
		{
			return new List<ExtentEntry>(0);
		}
		int num;
		byte[] array;
		checked
		{
			num = (int)tab.EntrySize;
			array = new byte[num];
			long offset = tabBase + tab.Offset;
			stream.Seek(offset, SeekOrigin.Begin);
		}
		List<ExtentEntry> list = new List<ExtentEntry>((int)Math.Min(tab.NumEntries, 2147483647u));
		for (uint num2 = 0u; num2 < tab.NumEntries; num2++)
		{
			ReadFullyOrThrow(stream, array, num);
			int offset2 = 0;
			ulong numSectors = Binary.ReadU64LE(array, ref offset2);
			uint targetType = Binary.ReadU32LE(array, ref offset2);
			ulong targetData = Binary.ReadU64LE(array, ref offset2);
			uint targetSource = Binary.ReadU32LE(array, ref offset2);
			list.Add(new ExtentEntry
			{
				NumSectors = numSectors,
				TargetType = targetType,
				TargetData = targetData,
				TargetSource = targetSource
			});
		}
		return list;
	}

	private static List<BlockDeviceEntry> ParseBlockDeviceTable(Stream stream, long tabBase, TableDesc tab)
	{
		if (tab.NumEntries == 0)
		{
			return new List<BlockDeviceEntry>(0);
		}
		int num;
		byte[] array;
		checked
		{
			num = (int)tab.EntrySize;
			array = new byte[num];
			long offset = tabBase + tab.Offset;
			stream.Seek(offset, SeekOrigin.Begin);
		}
		List<BlockDeviceEntry> list = new List<BlockDeviceEntry>((int)Math.Min(tab.NumEntries, 2147483647u));
		for (uint num2 = 0u; num2 < tab.NumEntries; num2++)
		{
			ReadFullyOrThrow(stream, array, num);
			if (!ParseBlockDeviceEntry(array, num, out var entry))
			{
				entry = default(BlockDeviceEntry);
			}
			list.Add(entry);
		}
		return list;
	}

	private static bool ParseBlockDeviceEntry(byte[] raw, int entrySize, out BlockDeviceEntry entry)
	{
		entry = default(BlockDeviceEntry);
		if (entrySize >= 24)
		{
			int offset = 0;
			ulong firstLogicalSector = Binary.ReadU64LE(raw, ref offset);
			uint alignment = Binary.ReadU32LE(raw, ref offset);
			uint alignmentOffset = Binary.ReadU32LE(raw, ref offset);
			ulong size = Binary.ReadU64LE(raw, ref offset);
			string empty = string.Empty;
			int offset2;
			uint num;
			if (entrySize < 60)
			{
				num = 0u;
				if (entrySize < 64)
				{
					entry = new BlockDeviceEntry
					{
						Name = empty,
						Flags = 0u,
						FirstLogicalSector = firstLogicalSector,
						Size = size,
						Alignment = alignment,
						AlignmentOffset = alignmentOffset
					};
					return true;
				}
				offset2 = 60;
				num = Binary.ReadU32LE(raw, ref offset2);
				entry = new BlockDeviceEntry
				{
					Name = empty,
					Flags = num,
					FirstLogicalSector = firstLogicalSector,
					Size = size,
					Alignment = alignment,
					AlignmentOffset = alignmentOffset
				};
				return true;
			}
			empty = DecodeAsciiName(raw, 24, 36);
			num = 0u;
			if (entrySize < 64)
			{
				entry = new BlockDeviceEntry
				{
					Name = empty,
					Flags = 0u,
					FirstLogicalSector = firstLogicalSector,
					Size = size,
					Alignment = alignment,
					AlignmentOffset = alignmentOffset
				};
				return true;
			}
			offset2 = 60;
			num = Binary.ReadU32LE(raw, ref offset2);
			entry = new BlockDeviceEntry
			{
				Name = empty,
				Flags = num,
				FirstLogicalSector = firstLogicalSector,
				Size = size,
				Alignment = alignment,
				AlignmentOffset = alignmentOffset
			};
			return true;
		}
		return false;
	}

	private static void ValidateTableDescriptor(TableDesc tab, uint tabsz, uint minsz, string name)
	{
		string text = "Table entry size is zero for ";
		string text2 = ".";
		string text3 = "Table entry size too small for ";
		string text4 = "Table out of bounds for ";
		if (tab.EntrySize == 0)
		{
			throw new InvalidDataException(text + name + text2);
		}
		if (tab.EntrySize >= minsz)
		{
			ulong num = (ulong)tab.NumEntries * (ulong)tab.EntrySize;
			ulong num2 = tab.Offset + num;
			if (num2 > tabsz)
			{
				throw new InvalidDataException(text4 + name + text2);
			}
			return;
		}
		throw new InvalidDataException(text3 + name + text2);
	}

	private static void ValidateMetadataLayout(Geometry geo, List<BlockDeviceEntry> devs)
	{
		string message = "Metadata overlaps logical partition contents.";
		if (devs == null || devs.Count == 0)
		{
			return;
		}
		ulong num = ulong.MaxValue;
		for (int i = 0; i < devs.Count; i++)
		{
			if (devs[i].FirstLogicalSector < num)
			{
				num = devs[i].FirstLogicalSector;
			}
		}
		if (num != ulong.MaxValue)
		{
			ulong num2 = (ulong)(4096L + (4096L + (long)geo.MetadataMaxSize * (long)geo.MetadataSlotCount) * 2L);
			if (num2 > num * 512L)
			{
				throw new InvalidDataException(message);
			}
		}
	}

	private static string DecodeAsciiName(byte[] src, int off, int len)
	{
		int num = 0;
		for (int i = 0; i < len && src[off + i] != 0; i++)
		{
			num++;
		}
		if (num == 0)
		{
			return string.Empty;
		}
		return Encoding.ASCII.GetString(src, off, num);
	}

	private static void ReadFullyOrThrow(Stream stream, byte[] dst, int count)
	{
		if (!ReadFully(stream, dst, count))
		{
			throw new EndOfStreamException();
		}
	}

	private static bool ReadFully(Stream stream, byte[] dst, int count)
	{
		int num = 0;
		int num2 = count;
		if (num2 <= 0)
		{
			return true;
		}
		int num3 = stream.Read(dst, 0, num2);
		if (num3 <= 0)
		{
			return false;
		}
		num = 0 + num3;
		num2 -= num3;
		int num4 = 2117750087;
		while (true)
		{
			_ = num4 ^ -570879697;
			if (num2 <= 0)
			{
				return true;
			}
			num3 = stream.Read(dst, num, num2);
			if (num3 <= 0)
			{
				break;
			}
			num += num3;
			num2 -= num3;
			num4 = 2117750087;
		}
		return false;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
