using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AMFTool.Erofs;

public class ErofsFileSystem
{
	private struct ZErofsLclusterIndex
	{
		public ushort Advise;

		public ushort ClusterOfs;

		public uint Blkaddr;
	}

	private struct CompactedIndexEntry
	{
		public ErofsVleClusterType Type;

		public ushort ClusterOfs;

		public ushort Delta0;

		public uint Pblk;
	}

	private struct ClusterIndexLayout
	{
		public int IndexSize;

		public int Shift;
	}

	private const int SuperSize = 128;

	private const int DirentRecordSize = 12;

	private Stream Stream;

	private long StreamBaseOffset;

	private ErofsSuperBlock Super;

	private long MetaAreaOffset;

	private SmallBlockCache BlockCache;

	private byte[] PackedDataCache;

	private bool PackedDataLoaded;

	public int BlockSize => Super.BlockSize;

	public ushort RootNid => Super.RootNid;

	public static bool IsErofs(Stream stream)
	{
		return IsErofs(stream, 0L);
	}

	public static bool IsErofs(Stream stream, long baseoffset)
	{
		int result = 0;
		if (stream == null || !stream.CanRead || !stream.CanSeek)
		{
			return false;
		}
		long position = stream.Position;
		try
		{
			byte[] array = new byte[128];
			ReadAt(stream, baseoffset + 1024L, array, 0, array.Length);
			int offset = 0;
			uint num = Binary.ReadU32LE(array, ref offset);
			return num == 3774210530u;
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

	public static ErofsFileSystem Open(Stream stream)
	{
		return new ErofsFileSystem(stream, 0L);
	}

	public static ErofsFileSystem Open(Stream stream, long baseoffset)
	{
		return new ErofsFileSystem(stream, baseoffset);
	}

	private ErofsFileSystem(Stream stream, long baseoffset)
	{
		object obj = null;
		string paramName = "stream";
		string message = "Stream must be readable and seekable.";

		if (stream == obj)
		{
			throw new ArgumentNullException(paramName);
		}
		if (!stream.CanRead || !stream.CanSeek)
		{
			throw new ArgumentException(message);
		}
		Stream = stream;
		StreamBaseOffset = baseoffset;
		LoadSuperblock();
		BlockCache = new SmallBlockCache(Stream, StreamBaseOffset, Super.BlockSize);
	}

	private void LoadSuperblock()
	{
		string message = "Not EROFS (Super magic mismatch).";
		string message2 = "EROFS block size bits out of range.";
		byte[] array = new byte[128];
		ReadAt(Stream, StreamBaseOffset + 1024L, array, 0, array.Length);
		Super = ErofsSuperBlock.Parse(array);
		if (Super.Magic != 3774210530u)
		{
			throw new InvalidDataException(message);
		}
		if (Super.BlockSizeBits < 9 || Super.BlockSizeBits > 16)
		{
			throw new InvalidDataException(message2);
		}
		MetaAreaOffset = Super.MetaBlockAddress * Super.BlockSize;
	}

	public bool FileExists(string path)
	{
		ulong nid;
		ErofsInode inode;
		return ResolveInodeIdFromPath(path, out nid, out inode);
	}

	public IReadOnlyList<ErofsDirectoryEntry> ListDirectory(string path)
	{
		if (ResolveInodeIdFromPath(path, out var nid, out var inode))
		{
			if (inode.IsDirectory)
			{
				byte[] dirdata = ReadDirectoryData(nid, inode);
				return ParseDirectoryEntries(dirdata);
			}
			return Array.Empty<ErofsDirectoryEntry>();
		}
		return Array.Empty<ErofsDirectoryEntry>();
	}

	public byte[] ReadFile(string path)
	{
		object result = null;
		if (!ResolveInodeIdFromPath(path, out var nid, out var inode))
		{
			return (byte[])result;
		}
		if (inode.IsRegularFile || inode.IsSymlink)
		{
			return ReadInodeDataAsFile(nid, inode);
		}
		return (byte[])result;
	}

	private bool ResolveInodeIdFromPath(string path, out ulong nid, out ErofsInode inode)
	{
		object obj = null;
		string text = "/";
		nid = 0uL;
		inode = (ErofsInode)obj;
		if (!string.IsNullOrWhiteSpace(path))
		{
			string text2 = NormalizePath(path);
			if (text2 == text)
			{
				nid = Super.RootNid;
				inode = ReadInode(nid);
				return inode !=null;
			}
			ulong num = Super.RootNid;
			ErofsInode erofsInode = ReadInode(num);
			if (erofsInode != null && erofsInode.IsDirectory)
			{
				string[] array = text2.Trim('/').Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					string text3 = array[i];
					byte[] dirdata = ReadDirectoryData(num, erofsInode);
					IReadOnlyList<ErofsDirectoryEntry> readOnlyList = ParseDirectoryEntries(dirdata);
					bool flag = false;
					ulong num2 = 0uL;
					ErofsFileType erofsFileType = ErofsFileType.EROFS_FT_UNKNOWN;
					for (int j = 0; j < readOnlyList.Count; j++)
					{
						if (readOnlyList[j].Name == text3)
						{
							num2 = readOnlyList[j].Nid;
							erofsFileType = readOnlyList[j].FileType;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
					num = num2;
					erofsInode = ReadInode(num);
					if (erofsInode != obj)
					{
						if (i < array.Length - 1 && (erofsFileType != ErofsFileType.EROFS_FT_DIR || !erofsInode.IsDirectory))
						{
							return false;
						}
						continue;
					}
					return false;
				}
				nid = num;
				inode = erofsInode;
				return true;
			}
			return false;
		}
		return false;
	}

	private ErofsInode ReadInode(ulong nid)
	{
		object result = null;
		long num = MetaAreaOffset + (long)(32L * nid);
		byte[] array = new byte[32];
		if (ReadAtSafe(Stream, StreamBaseOffset + num, array, 0, array.Length))
		{
			int offset = 0;
			ushort num2 = Binary.ReadU16LE(array, ref offset);
			if ((num2 & 1) == 0)
			{
				return ErofsInode.Parse(array, isextent: false);
			}
			byte[] array2 = new byte[64];
			if (!ReadAtSafe(Stream, StreamBaseOffset + num, array2, 0, array2.Length))
			{
				return (ErofsInode)result;
			}
			return ErofsInode.Parse(array2, isextent: true);
		}
		return (ErofsInode)result;
	}

	private byte[] ReadDirectoryData(ulong nid, ErofsInode inode)
	{
		if (inode.MappingMode != ErofsDataMappingMode.EROFS_INODE_FLAT_INLINE)
		{
			return ReadInodeDataAsFile(nid, inode);
		}
		long num = MetaAreaOffset + (long)(32L * nid);
		long num2 = num + inode.HeaderSize;
		long num3 = num2 + inode.XattrSize;
		int num4 = checked((int)inode.FileSize);
		if (num4 > 0)
		{
			byte[] array = new byte[num4];
			ReadAt(Stream, StreamBaseOffset + num3, array, 0, array.Length);
			return array;
		}
		return Array.Empty<byte>();
	}

	private byte[] ReadInodeDataAsFile(ulong nid, ErofsInode inode)
	{
		string message = "Unsupported EROFS inode mapping mode.";
		if (inode.FileSize == 0L)
		{
			return Array.Empty<byte>();
		}
		if (inode.MappingMode == ErofsDataMappingMode.EROFS_INODE_FLAT_PLAIN)
		{
			return ReadPlainData(inode);
		}
		if (inode.MappingMode != ErofsDataMappingMode.EROFS_INODE_FLAT_INLINE)
		{
			if (inode.MappingMode == ErofsDataMappingMode.FlatCompression || inode.MappingMode == ErofsDataMappingMode.FlatCompressionLegacy)
			{
				try
				{
					return ReadCompressedDataNew(nid, inode);
				}
				catch (NotSupportedException)
				{
					return ReadCompressedDataLegacy(nid, inode);
				}
				catch (InvalidDataException)
				{
					return ReadCompressedDataLegacy(nid, inode);
				}
			}
			throw new NotSupportedException(message);
		}
		return ReadInlineFileData(nid, inode);
	}

	private byte[] ReadPlainData(ErofsInode inode)
	{
		long num = inode.DataPointerOrClusterCount * Super.BlockSize;
		int num2 = checked((int)inode.FileSize);
		byte[] array = new byte[num2];
		ReadAt(Stream, StreamBaseOffset + num, array, 0, array.Length);
		return array;
	}

	private byte[] ReadInlineFileData(ulong nid, ErofsInode inode)
	{
		string message = "EROFS inline tail size invalid.";
		string message2 = "EROFS inline layout inconsistent.";
		long num = MetaAreaOffset + (long)(32L * nid);
		long num2 = num + inode.HeaderSize;
		long num3 = num2 + inode.XattrSize;
		int num4 = checked((int)inode.FileSize);
		int blockSize = Super.BlockSize;
		int num5 = blockSize - (int)(num3 % blockSize);
		if (num5 > 0)
		{
			if (num5 <= blockSize)
			{
				byte[] array = new byte[num5];
				ReadAt(Stream, StreamBaseOffset + num3, array, 0, array.Length);
				if (num4 > num5)
				{
					long num6 = inode.DataPointerOrClusterCount * blockSize;
					int num7 = blockSize * (int)Math.Ceiling(((double)num4 - (double)num5) / (double)blockSize);
					byte[] array2 = new byte[num7];
					ReadAt(Stream, StreamBaseOffset + num6, array2, 0, array2.Length);
					int num8 = num4 - num5;
					if (num8 > array2.Length)
					{
						throw new InvalidDataException(message2);
					}
					byte[] array3 = new byte[num4];
					Buffer.BlockCopy(array2, 0, array3, 0, num8);
					Buffer.BlockCopy(array, 0, array3, num8, num5);
					return array3;
				}
				byte[] array4 = new byte[num4];
				Buffer.BlockCopy(array, 0, array4, 0, array4.Length);
				return array4;
			}
			throw new InvalidDataException(message);
		}
		throw new InvalidDataException(message);
	}

	private byte[] ReadCompressedDataNew(ulong nid, ErofsInode inode)
	{
		int num = 0;
		string message = "Compressed EROFS currently requires 4096 block size.";
		int num2 = 1;
		string message2 = "EROFS compression algorithm not supported.";
		int num3 = 31;
		string message3 = "Bad cluster size.";
		int num4 = 3;
		int num5 = 2;
		if (Super.BlockSize == 4096)
		{
			long num6 = MetaAreaOffset + (long)(32L * nid);
			long num7 = num6 + inode.HeaderSize;
			long num8 = num7 + inode.XattrSize;
			if ((ulong)(num8 % 8L) > 0uL)
			{
				num8 = AlignUp(num8, 8);
			}
			byte[] array = new byte[8];
			ReadAt(Stream, StreamBaseOffset + num8, array, 0, array.Length);
			num8 += array.Length;
			int offset = 0;
			uint fragmentoff = Binary.ReadU32LE(array, ref offset);
			ushort advise = Binary.ReadU16LE(array, ref offset);
			byte b = array[offset++];
			byte b2 = array[offset++];
			if (b != 0)
			{
				throw new NotSupportedException(message2);
			}
			int num9 = checked((int)inode.FileSize);
			if (num9 != 0)
			{
				if ((b2 & 0x80) == 0)
				{
					int num10 = b2 & 0x7F;
					int num11 = ((num10 > 7) ? num10 : (num10 + 12));
					int num12 = 1 << num11;
					if (num12 > 0)
					{
						uint num13 = (uint)((num9 + num12 - 1) / num12);
						byte[] array2 = new byte[num9];
						if (inode.MappingMode == ErofsDataMappingMode.FlatCompression)
						{
							try
							{
								return ReadCompressedDataCompacted(nid, inode, num8, advise, num11, num12, num9, array2);
							}
							catch (NotSupportedException)
							{
							}
							catch (InvalidDataException)
							{
							}
						}
						if (inode.MappingMode != (ErofsDataMappingMode)num2)
						{
							if (!IsValidIndexStructure(num8, num13, num12))
							{
								int num14 = num;
								ClusterIndexLayout clusterIndexLayout = InferClusterIndexLayout(num8, num13, advise);
								int indexSize = clusterIndexLayout.IndexSize;
								int shift = clusterIndexLayout.Shift;
								byte[] array3 = new byte[indexSize];
								byte[] array4 = new byte[num12];
								for (uint num15 = (uint)num; (num15 >= num13) ? ((byte)num != 0) : (num14 < num9); num15 += (uint)num2)
								{
									long num16 = num8 + num15 * indexSize;
									ReadAt(Stream, StreamBaseOffset + num16, array3, num, indexSize);
									int num18;
									uint num19;
									if (indexSize == num5)
									{
										int offset2 = num;
										ushort num17 = Binary.ReadU16LE(array3, ref offset2);
										num18 = num17 & num4;
										num19 = (uint)((shift != 0) ? (num17 >> (shift & num3)) : num17);
									}
									else
									{
										int offset3 = num;
										uint num20 = Binary.ReadU32LE(array3, ref offset3);
										num18 = (int)num20 & num4;
										num19 = ((shift != 0) ? (num20 >> (shift & num3)) : num20);
									}
									if (num19 == (uint)num)
									{
										continue;
									}
									long num21 = checked(num19 * Super.BlockSize);
									ReadAt(Stream, StreamBaseOffset + num21, array4, num, array4.Length);
									int num22 = num9 - num14;
									if (num18 != num)
									{
										if ((num18 == num2) ? ((byte)num2 != 0) : (num18 == num4))
										{
											int num23 = Lz4Raw.Decompress(array4, num, array4.Length, array2, num14, num22);
											num14 += num23;
										}
									}
									else
									{
										int num24 = ((num22 <= num12) ? num22 : num12);
										Buffer.BlockCopy(array4, num, array2, num14, num24);
										num14 += num24;
									}
								}
								return array2;
							}
							return ReadCompressedDataFull(num8, num13, num12, num9, array2);
						}
						return ReadCompressedDataFull(num8, num13, num12, num9, array2);
					}
					throw new InvalidDataException(message3);
				}
				return ReadFragmentData(fragmentoff, num9);
			}
			return Array.Empty<byte>();
		}
		throw new NotSupportedException(message);
	}

	private byte[] ReadCompressedDataLegacy(ulong nid, ErofsInode inode)
	{
		int num = 4096;
		int num2 = 0;
		string message = "Compressed EROFS currently requires 4096 block size.";
		int num3 = 1;
		string message2 = "Bad legacy compressed block count.";
		int num4 = 8;
		string message3 = "Decompress index header alignment failed.";
		double field_ = Class38.field_3271;
		int num5 = 3;
		int num6 = 2;
		string message4 = "Bad legacy target position.";
		string message5 = "Legacy cluster offset check failed.";
		if (Super.BlockSize == 4096)
		{
			int num7 = checked((int)inode.DataPointerOrClusterCount);
			if (num7 < 0 || num7 > 500000)
			{
				throw new InvalidDataException(message2);
			}
			long num8 = MetaAreaOffset + (long)(32L * nid);
			long num9 = num8 + inode.HeaderSize;
			long num10 = num9 + inode.XattrSize;
			if (num10 % 8L == 4L)
			{
				num10 += 4L;
			}
			if ((ulong)(num10 % 8L) > 0uL)
			{
				throw new InvalidDataException(message3);
			}
			num10 += 8L;
			num10 += 8L;
			if ((ulong)(num10 % 8L) > 0uL)
			{
				throw new InvalidDataException(message3);
			}
			int num11 = checked((int)inode.FileSize);
			byte[] array = new byte[num11];
			int num12 = (int)Math.Ceiling((double)num11 / field_);
			byte[] array2 = new byte[8];
			byte[] array3 = new byte[4096];
			uint num13 = 0u;
			uint num14 = 0u;
			int num15 = 0;
			for (int i = 0; (i < num12) ? (num15 < num11) : ((byte)num2 != 0); i += num3)
			{
				long num16 = num10 + (long)i * (long)num4;
				ReadAt(Stream, StreamBaseOffset + num16, array2, num2, array2.Length);
				int offset = num2;
				ushort num17 = Binary.ReadU16LE(array2, ref offset);
				ushort num18 = Binary.ReadU16LE(array2, ref offset);
				uint num19 = Binary.ReadU32LE(array2, ref offset);
				int num20 = num17 & num5;
				ErofsVleClusterType erofsVleClusterType = (ErofsVleClusterType)num20;
				if (erofsVleClusterType == (ErofsVleClusterType)num5)
				{
					erofsVleClusterType = (ErofsVleClusterType)((num19 != num13) ? num3 : num6);
					num13 = num19;
				}
				int num21 = i * num + num18;
				if (num21 < num2)
				{
					throw new InvalidDataException(message4);
				}
				if (erofsVleClusterType == (ErofsVleClusterType)num2)
				{
					long blockidx = num19;
					try
					{
						ReadBlock4096By4kBlockIndex(blockidx, array3);
					}
					catch (EndOfStreamException)
					{
						throw;
					}
					int num22 = num11 - num21;
					int num23 = ((num22 <= num) ? num22 : num);
					Buffer.BlockCopy(array3, num2, array, num21, num23);
					num15 = Math.Max(num15, num21 + num23);
				}
				else if (erofsVleClusterType == (ErofsVleClusterType)num3)
				{
					if (((num15 % num == num18) ? 1 : 0) == num2)
					{
						if (num18 != num2)
						{
							throw new InvalidDataException(message5);
						}
						num15 -= num15 % num;
					}
					long blockidx2 = num19;
					try
					{
						ReadBlock4096By4kBlockIndex(blockidx2, array3);
					}
					catch (EndOfStreamException)
					{
						throw;
					}
					int outmaxsize = num11 - num15;
					int num24 = Lz4Raw.Decompress(array3, num2, array3.Length, array, num15, outmaxsize);
					num15 += num24;
				}
				else
				{
					if (erofsVleClusterType == (ErofsVleClusterType)num6 || erofsVleClusterType != (ErofsVleClusterType)num5)
					{
						continue;
					}
					long num25 = num19;
					if ((uint)num25 != num14)
					{
						num14 = (uint)num25;
						try
						{
							ReadBlock4096By4kBlockIndex(num25, array3);
						}
						catch (EndOfStreamException)
						{
							throw;
						}
						int outmaxsize2 = num11 - num15;
						int num26 = Lz4Raw.Decompress(array3, num2, array3.Length, array, num15, outmaxsize2);
						num15 += num26;
					}
				}
			}
			return array;
		}
		throw new NotSupportedException(message);
	}

	private void ReadClusterBy4kBlockIndex(long startblockidx, byte[] clbuff)
	{
		string message = "Cluster size must be a multiple of 4096 for this reader.";
		int num = clbuff.Length;
		if (num % 4096 == 0)
		{
			byte[] array = new byte[4096];
			int num2 = 0;
			long num3 = startblockidx;
			while (num2 < num)
			{
				BlockCache.ReadBlock(num3, array);
				Buffer.BlockCopy(array, 0, clbuff, num2, 4096);
				num2 += 4096;
				num3++;
			}
			return;
		}
		throw new InvalidDataException(message);
	}

	private void ReadBlock4096By4kBlockIndex(long blockidx, byte[] dst)
	{
		string message = "destination too small for 4096 block.";
		if (dst == null || dst.Length < 4096)
		{
			throw new ArgumentException(message);
		}
		byte[] array = new byte[4096];
		BlockCache.ReadBlock(blockidx, array);
		Buffer.BlockCopy(array, 0, dst, 0, 4096);
	}

	private static IReadOnlyList<ErofsDirectoryEntry> ParseDirectoryEntries(byte[] dirdata)
	{
		string message = "EROFS directory name offset not aligned.";
		string text = ".";
		string text2 = "..";
		if (dirdata != null && dirdata.Length >= 12)
		{
			ushort num = (ushort)(dirdata[8] | (dirdata[9] << 8));
			if (num % 12 == 0)
			{
				int num2 = num / 12;
				if (num2 > 0)
				{
					List<ErofsDirectoryEntry> list = new List<ErofsDirectoryEntry>(num2);
					for (int i = 0; i < num2; i++)
					{
						int num3 = i * 12;
						int offset = num3;
						ulong nid = Binary.ReadU64LE(dirdata, ref offset);
						ushort num4 = Binary.ReadU16LE(dirdata, ref offset);
						byte b = dirdata[offset];
						if (b >= 8)
						{
							continue;
						}
						int num5 = num4;
						int num6 = dirdata.Length;
						if (i < num2 - 1)
						{
							int num7 = (i + 1) * 12;
							ushort num8 = (ushort)(dirdata[num7 + 8] | (dirdata[num7 + 9] << 8));
							num6 = num8;
						}
						if (num5 < 0 || num5 >= dirdata.Length || num6 < num5)
						{
							continue;
						}
						int num9 = num6 - num5;
						if (num9 > 255)
						{
							num9 = 255;
						}
						int num10 = IndexOfZero(dirdata, num5, num9);
						if (num10 >= 0)
						{
							num9 = num10 - num5;
						}
						if (num9 > 0)
						{
							string text3 = Encoding.UTF8.GetString(dirdata, num5, num9);
							if (!(text3 == text) && !(text3 == text2))
							{
								list.Add(new ErofsDirectoryEntry
								{
									Nid = nid,
									Name = text3,
									FileType = (ErofsFileType)b
								});
							}
						}
					}
					return list;
				}
				return Array.Empty<ErofsDirectoryEntry>();
			}
			throw new InvalidDataException(message);
		}
		return Array.Empty<ErofsDirectoryEntry>();
	}

	private static int IndexOfZero(byte[] buffer, int start, int length)
	{
		int num = start + length;
		int num2 = start;
		if (num2 >= num)
		{
			int num3 = -1;
			return -1;
		}
		if (buffer[num2] != 0)
		{
			num2++;
			int num4 = 2023393738;
			while (true)
			{
				_ = num4 ^ 0x1E31903E;
				if (num2 >= num)
				{
					int num3 = -1;
					return -1;
				}
				if (buffer[num2] == 0)
				{
					break;
				}
				num2++;
				num4 = 2023393738;
			}
			return num2;
		}
		return num2;
	}

	private static int IndexOfNonZero(byte[] buffer)
	{
		object obj = null;
		if (buffer != obj)
		{
			for (int i = 0; i < buffer.Length; i++)
			{
				if (buffer[i] != 0)
				{
					return i;
				}
			}
			return -1;
		}
		return -1;
	}

	private static long AlignUp(long value, int align)
	{
		long num = align - 1;
		return (value + num) & ~num;
	}

	private static long AlignDown(long value, int align)
	{
		long num = align - 1;
		return value & ~num;
	}

	private static string NormalizePath(string path)
	{
		string text = "/";
		string text2 = "//";
		string text3 = path.Replace('\\', '/').Trim();
		if (text3.Length != 0)
		{
			bool flag;
			if (text3[0] == '/')
			{
				flag = text3.IndexOf(text2, StringComparison.Ordinal) >= 0;
			}
			else
			{
				text3 = text + text3;
				flag = text3.IndexOf(text2, StringComparison.Ordinal) >= 0;
			}
			while (flag)
			{
				text3 = text3.Replace(text2, text);
				flag = text3.IndexOf(text2, StringComparison.Ordinal) >= 0;
			}
			if (text3.Length <= 1 || text3[text3.Length - 1] != '/')
			{
				return text3;
			}
			return text3.Substring(0, text3.Length - 1);
		}
		return text;
	}

	private static void ReadAt(Stream stream, long offset, byte[] buff, int outoff, int count)
	{
		stream.Seek(offset, SeekOrigin.Begin);
		int num = 0;
		if (0 >= count)
		{
			return;
		}
		int num2 = stream.Read(buff, outoff, count);
		if (num2 > 0)
		{
			num = 0 + num2;
			int num3 = 1111973853;
			while (true)
			{
				_ = num3 ^ 0x31AB275D;
				if (num < count)
				{
					num2 = stream.Read(buff, outoff + num, count - num);
					if (num2 > 0)
					{
						num += num2;
						num3 = 1111973853;
						continue;
					}
					throw new EndOfStreamException();
				}
				break;
			}
			return;
		}
		throw new EndOfStreamException();
	}

	private bool IsValidIndexStructure(long indexbase, uint lccnt, int clustersize)
	{
		int num = (int)Math.Min(lccnt, 4u);
		if (num > 0)
		{
			long num2 = Stream.Length / Super.BlockSize;
			byte[] array = new byte[8];
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < num; i++)
			{
				long num5 = indexbase + i * 8L;
				if (!ReadAtSafe(Stream, StreamBaseOffset + num5, array, 0, array.Length))
				{
					num4++;
					continue;
				}
				int offset = 0;
				ushort num6 = Binary.ReadU16LE(array, ref offset);
				ushort num7 = Binary.ReadU16LE(array, ref offset);
				uint num8 = Binary.ReadU32LE(array, ref offset);
				int num9 = num6 & 3;
				if (num9 >= 0 && num9 <= 3)
				{
					if (num7 >= clustersize)
					{
						num4++;
					}
					else if ((num9 == 0 || num9 == 1 || num9 == 3) && num8 > num2)
					{
						num4++;
					}
					else
					{
						num3++;
					}
				}
				else
				{
					num4++;
				}
			}
			return num3 > 0 && num4 == 0;
		}
		return false;
	}

	private byte[] ReadCompressedDataFull(long indexbase, uint lccnt, int clustersize, int filesize, byte[] output)
	{
		string message = "EROFS cluster offset not supported.";
		byte[] array = new byte[8];
		byte[] array2 = new byte[clustersize];
		for (uint num = 0u; num < lccnt; num++)
		{
			long num2 = indexbase + (long)num * 8L;
			ReadAt(Stream, StreamBaseOffset + num2, array, 0, array.Length);
			int offset = 0;
			ZErofsLclusterIndex zErofsLclusterIndex = new ZErofsLclusterIndex
			{
				Advise = Binary.ReadU16LE(array, ref offset),
				ClusterOfs = Binary.ReadU16LE(array, ref offset),
				Blkaddr = Binary.ReadU32LE(array, ref offset)
			};
			int num3 = zErofsLclusterIndex.Advise & 3;
			if (num3 == 2)
			{
				continue;
			}
			if (zErofsLclusterIndex.ClusterOfs == 0)
			{
				long num4 = num * clustersize;
				if (num4 >= filesize)
				{
					break;
				}
				int num5 = filesize - (int)num4;
				int num7;
				switch (num3)
				{
				default:
					if (num3 != 3)
					{
						continue;
					}
					goto case 1;
				case 0:
				{
					long num6 = checked(zErofsLclusterIndex.Blkaddr * Super.BlockSize);
					ReadAt(Stream, StreamBaseOffset + num6, array2, 0, array2.Length);
					num7 = ((num5 <= clustersize) ? num5 : clustersize);
					break;
				}
				case 1:
				{
					long num8 = checked(zErofsLclusterIndex.Blkaddr * Super.BlockSize);
					ReadAt(Stream, StreamBaseOffset + num8, array2, 0, array2.Length);
					Lz4Raw.Decompress(array2, 0, array2.Length, output, (int)num4, num5);
					continue;
				}
				}
				int count = num7;
				Buffer.BlockCopy(array2, 0, output, (int)num4, count);
				continue;
			}
			throw new NotSupportedException(message);
		}
		return output;
	}

	private byte[] ReadCompressedDataCompacted(ulong nid, ErofsInode inode, long indexbase, ushort advise, int lclusterbits, int clustersize, int filesize, byte[] output)
	{
		string message = "Compacted index requires 4k clusters.";
		if (lclusterbits == 12)
		{
			uint num = (uint)((filesize + Super.BlockSize - 1) / Super.BlockSize);
			uint num2 = (uint)((filesize + clustersize - 1) / clustersize);
			uint num3 = (uint)((32 - ((int)indexbase & 0x1F)) / 4);
			if (num3 == 8)
			{
				num3 = 0u;
			}
			uint compacted = 0u;
			if ((advise & 1) != 0 && num > num3)
			{
				compacted = (num - num3) / 16 * 16;
			}
			long cacheblkbase = -1L;
			byte[] idxblock = new byte[Super.BlockSize];
			byte[] array = new byte[clustersize];
			int num4 = 0;
			for (uint num5 = 0u; num5 < num2; num5++)
			{
				if (num4 >= filesize)
				{
					break;
				}
				CompactedIndexEntry compactedIndexEntry = LoadCompactedIndexEntry(indexbase, num3, compacted, lclusterbits, num5, ref cacheblkbase, idxblock);
				if (compactedIndexEntry.Type == (ErofsVleClusterType)2 || compactedIndexEntry.Pblk == 0)
				{
					continue;
				}
				long num6 = checked(compactedIndexEntry.Pblk * Super.BlockSize);
				ReadAt(Stream, StreamBaseOffset + num6, array, 0, array.Length);
				int num7 = filesize - num4;
				if (compactedIndexEntry.Type == (ErofsVleClusterType)0)
				{
					int clusterOfs = compactedIndexEntry.ClusterOfs;
					if (clusterOfs < clustersize)
					{
						int num8 = Math.Min(clustersize - clusterOfs, num7);
						if (num8 > 0)
						{
							Buffer.BlockCopy(array, clusterOfs, output, num4, num8);
							num4 += num8;
						}
					}
				}
				else
				{
					if (compactedIndexEntry.Type != ErofsVleClusterType.Head && compactedIndexEntry.Type != ErofsVleClusterType.Reserved)
					{
						continue;
					}
					int clusterOfs2 = compactedIndexEntry.ClusterOfs;
					if (clusterOfs2 >= clustersize)
					{
						continue;
					}
					int num9 = num7 + clusterOfs2;
					byte[] array2 = new byte[num9];
					int num10 = Lz4Raw.Decompress(array, 0, array.Length, array2, 0, array2.Length);
					if (num10 == 0)
					{
						int num11 = IndexOfNonZero(array);
						if (num11 > 0 && num11 < array.Length - 1)
						{
							num10 = Lz4Raw.Decompress(array, num11, array.Length - num11, array2, 0, array2.Length);
						}
					}
					int num12 = num10 - clusterOfs2;
					if (num12 < 0)
					{
						num12 = 0;
					}
					int num13 = Math.Min(num12, num7);
					if (num13 > 0)
					{
						Buffer.BlockCopy(array2, clusterOfs2, output, num4, num13);
						num4 += num13;
					}
				}
			}
			return output;
		}
		throw new NotSupportedException(message);
	}

	private CompactedIndexEntry LoadCompactedIndexEntry(long indexbase, uint compacted4, uint compacted2, int lclusterbits, uint lcn, ref long cacheblkbase, byte[] idxblock)
	{
		long num = indexbase;
		uint num2 = lcn;
		if (num2 < compacted4)
		{
			num += (long)num2 * 4L;
			return UnpackCompactedIndexFromBlock(num, 2, lclusterbits, ref cacheblkbase, idxblock);
		}
		num += (long)compacted4 * 4L;
		num2 -= compacted4;
		if (num2 >= compacted2)
		{
			num += (long)compacted2 * 2L;
			num2 -= compacted2;
			num += (long)num2 * 4L;
			return UnpackCompactedIndexFromBlock(num, 2, lclusterbits, ref cacheblkbase, idxblock);
		}
		num += (long)num2 * 2L;
		return UnpackCompactedIndexFromBlock(num, 1, lclusterbits, ref cacheblkbase, idxblock);
	}

	private CompactedIndexEntry UnpackCompactedIndexFromBlock(long pos, int ashift, int lclusterbits, ref long cacheblkbase, byte[] idxblock)
	{
		string message = "Unsupported compacted index layout.";
		long num = AlignDown(pos, Super.BlockSize);
		if (num != cacheblkbase)
		{
			ReadAt(Stream, StreamBaseOffset + num, idxblock, 0, idxblock.Length);
			cacheblkbase = num;
		}
		int num2 = (int)(pos - num);
		int num3 = 1 << ashift;
		int num4;
		if (num3 != 4)
		{
			if (num3 != 2 || lclusterbits != 12)
			{
				throw new NotSupportedException(message);
			}
			num4 = 16;
		}
		else
		{
			num4 = 2;
		}
		int num5 = num4 << ashift;
		int num6 = (num5 - 4) * 8 / num4;
		int num7 = num2 / num5 * num5;
		int num8 = num2 - num7 >> ashift;
		byte type;
		uint num9 = DecodeCompactedBits(lclusterbits, idxblock, num7, num6 * num8, out type);
		ErofsVleClusterType erofsVleClusterType = (ErofsVleClusterType)type;
		CompactedIndexEntry result = new CompactedIndexEntry
		{
			Type = erofsVleClusterType,
			ClusterOfs = 0,
			Delta0 = 0,
			Pblk = 0u
		};
		if (erofsVleClusterType != (ErofsVleClusterType)2)
		{
			result.ClusterOfs = (ushort)num9;
			int num10 = 1;
			for (int num11 = num8 - 1; num11 >= 0; num11--)
			{
				byte type2;
				uint num12 = DecodeCompactedBits(lclusterbits, idxblock, num7, num6 * num11, out type2);
				if (type2 == 2)
				{
					num11 -= (int)num12;
				}
				if (num11 >= 0)
				{
					num10++;
				}
			}
			int offset = num7 + num5 - 4;
			uint num13 = Binary.ReadU32LE(idxblock, offset);
			result.Pblk = num13 + (uint)num10;
			return result;
		}
		result.ClusterOfs = (ushort)(1 << lclusterbits);
		if (num8 + 1 != num4)
		{
			result.Delta0 = (ushort)num9;
		}
		else if (num8 != 0)
		{
			byte type3;
			uint num14 = DecodeCompactedBits(lclusterbits, idxblock, num7, num6 * (num8 - 1), out type3);
			if (type3 != 2)
			{
				num14 = 0u;
			}
			result.Delta0 = (ushort)(num14 + 1);
		}
		else
		{
			result.Delta0 = 0;
		}
		return result;
	}

	private static uint DecodeCompactedBits(int lobits, byte[] data, int StreamBaseOffset, int bitpos, out byte type)
	{
		string message = "Compacted index out of range.";
		int num = StreamBaseOffset + (bitpos >> 3);
		if (num + 4 > data.Length)
		{
			throw new InvalidDataException(message);
		}
		uint num2 = Binary.ReadU32LE(data, num);
		num2 >>= bitpos & 7;
		uint num3 = (uint)((1 << lobits) - 1);
		uint result = num2 & num3;
		type = (byte)((num2 >> lobits) & 3);
		return result;
	}

	private byte[] ReadFragmentData(uint fragmentoff, int filesize)
	{
		string message = "EROFS PackedDataCache inode not available.";
		string message2 = "Packed inode is empty.";
		string message3 = "Fragment offset out of range.";
		if (Super.PackedNid == 0L)
		{
			throw new NotSupportedException(message);
		}
		byte[] packed = GetPacked();
		if (packed != null && packed.Length != 0)
		{
			if (fragmentoff <= (uint)packed.Length)
			{
				int num = packed.Length - (int)fragmentoff;
				if (num <= 0)
				{
					throw new InvalidDataException(message3);
				}
				int count = ((filesize > num) ? num : filesize);
				byte[] array = new byte[filesize];
				Buffer.BlockCopy(packed, (int)fragmentoff, array, 0, count);
				return array;
			}
			throw new InvalidDataException(message3);
		}
		throw new InvalidDataException(message2);
	}

	private byte[] GetPacked()
	{
		object obj = null;
		if (PackedDataLoaded)
		{
			return PackedDataCache;
		}
		PackedDataLoaded = true;
		ErofsInode erofsInode = ReadInode(Super.PackedNid);
		if (erofsInode != obj)
		{
			PackedDataCache = ReadInodeDataAsFile(Super.PackedNid, erofsInode);
			return PackedDataCache;
		}
		PackedDataCache = Array.Empty<byte>();
		return PackedDataCache;
	}

	private ClusterIndexLayout InferClusterIndexLayout(long indexbase, uint clustercount, ushort advise)
	{
		long num = Stream.Length / Super.BlockSize;
		int num2 = (int)Math.Min(clustercount, 16u);
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		bool compact2b = (advise & 1) != 0;
		byte[] array = new byte[4];
		byte[] array2 = new byte[2];
		for (int i = 0; i < num2; i++)
		{
			long num9 = indexbase + i * 4L;
			if (ReadAtSafe(Stream, StreamBaseOffset + num9, array, 0, 4))
			{
				int offset = 0;
				uint num10 = Binary.ReadU32LE(array, ref offset);
				if (num10 != 0)
				{
					num3++;
					if (num10 > num)
					{
						num5++;
					}
					if (num10 >> 2 > num)
					{
						num6++;
					}
				}
			}
			long num11 = indexbase + i * 2L;
			if (!ReadAtSafe(Stream, StreamBaseOffset + num11, array2, 0, 2))
			{
				continue;
			}
			int offset2 = 0;
			ushort num12 = Binary.ReadU16LE(array2, ref offset2);
			if (num12 != 0)
			{
				num4++;
				if (num12 > num)
				{
					num7++;
				}
				if (num12 >> 2 > num)
				{
					num8++;
				}
			}
		}
		ClusterIndexLayout best = new ClusterIndexLayout
		{
			IndexSize = (compact2b ? 2 : 4),
			Shift = (compact2b ? 2 : 0)
		};
		int bestinvalid = int.MaxValue;
		Consider(4, 0, num5, num3);
		Consider(4, 2, num6, num3);
		if (!compact2b)
		{
			Consider(2, 0, num7, num4);
		}
		Consider(2, 2, num8, num4);
		if (bestinvalid == int.MaxValue)
		{
			return best;
		}
		return best;
		void Consider(int size, int shift, int invalid, int seen)
		{
			if (seen == 0)
			{
				return;
			}
			int num13 = invalid;
			bool flag;
			if (!compact2b || size != 4)
			{
				if (!compact2b)
				{
					goto IL_003d;
				}
				flag = false;
			}
			else
			{
				num13 += 2;
				if (!compact2b)
				{
					goto IL_003d;
				}
				flag = false;
			}
			goto IL_0042;
			IL_003d:
			flag = size == 2;
			goto IL_0042;
			IL_0042:
			if (!flag)
			{
				if (num13 < bestinvalid)
				{
					bestinvalid = num13;
					best.IndexSize = size;
					best.Shift = shift;
				}
			}
			else
			{
				num13 += 2;
				if (num13 < bestinvalid)
				{
					bestinvalid = num13;
					best.IndexSize = size;
					best.Shift = shift;
				}
			}
		}
	}

	private bool DecodeCompactIndexPrimary(long indexbase, uint lccnt, int clustersize, int filesize, out byte[] output)
	{
		object obj = null;
		output = (byte[])obj;
		if (lccnt != 0)
		{
			ushort[] array = new ushort[lccnt];
			ushort[] array2 = new ushort[lccnt];
			byte[] array3 = new byte[4];
			for (uint num = 0u; num < lccnt; num++)
			{
				long num2 = indexbase + (long)num * 4L;
				if (!ReadAtSafe(Stream, StreamBaseOffset + num2, array3, 0, 4))
				{
					return false;
				}
				int offset = 0;
				array[num] = Binary.ReadU16LE(array3, ref offset);
				array2[num] = Binary.ReadU16LE(array3, ref offset);
			}
			long num3 = indexbase + (long)lccnt * 4L;
			if (!DecodeCompactIndexV1(num3, lccnt, clustersize, filesize, array, array2, out output))
			{
				if (DecodeCompactIndexSafe(num3, lccnt, clustersize, filesize, array, array2, 2, typefromadv: true, out output))
				{
					return true;
				}
				if (DecodeCompactIndexSafe(num3, lccnt, clustersize, filesize, array, array2, 2, typefromadv: false, out output))
				{
					return true;
				}
				long ubase = num3 + (long)lccnt * 2L;
				if (!DecodeCompactIndexSafe(ubase, lccnt, clustersize, filesize, array, array2, 4, typefromadv: false, out output))
				{
					if (DecodeCompactIndexSafe(ubase, lccnt, clustersize, filesize, array, array2, 4, typefromadv: true, out output))
					{
						return true;
					}
					return false;
				}
				return true;
			}
			return true;
		}
		output = Array.Empty<byte>();
		return true;
	}

	private bool DecodeCompactIndexV1(long ubase, uint lccnt, int clustersize, int filesize, ushort[] adv, ushort[] ofs, out byte[] output)
	{
		object obj = null;
		output = new byte[filesize];
		ushort[] array = new ushort[lccnt];
		byte[] array2 = new byte[2];
		uint num = 0u;
		long num2;
		uint[] array3;
		byte[] array4;
		uint num3;
		while (true)
		{
			if (num >= lccnt)
			{
				num2 = ubase + (long)lccnt * 2L;
				array3 = new uint[lccnt];
				array4 = new byte[4];
				num3 = 0u;
				break;
			}
			long num4 = ubase + (long)num * 2L;
			if (ReadAtSafe(Stream, StreamBaseOffset + num4, array2, 0, 2))
			{
				int offset = 0;
				array[num] = Binary.ReadU16LE(array2, ref offset);
				num++;
				continue;
			}
			return false;
		}
		byte[] array5;
		byte[] headbuff;
		uint num5;
		bool flag;
		int num6;
		uint num7;
		while (true)
		{
			if (num3 >= lccnt)
			{
				array5 = new byte[clustersize];
				headbuff = (byte[])obj;
				num5 = 0u;
				flag = false;
				num6 = 0;
				num7 = 0u;
				break;
			}
			long num8 = num2 + (long)num3 * 4L;
			if (!ReadAtSafe(Stream, StreamBaseOffset + num8, array4, 0, 4))
			{
				return false;
			}
			int offset2 = 0;
			array3[num3] = Binary.ReadU32LE(array4, ref offset2);
			num3++;
		}
		while (true)
		{
			if (num7 >= lccnt || num6 >= filesize)
			{
				if (!IsProps(output))
				{
					return false;
				}
				return true;
			}
			int num9 = adv[num7] & 3;
			ushort num10 = ofs[num7];
			ushort num11 = array[num7];
			uint num12 = ((num11 == ushort.MaxValue) ? array3[num7] : num11);
			int num13 = (int)num7 * clustersize;
			int val = filesize - num13;
			if (num9 == 2)
			{
				ushort num14 = (ushort)(num12 & 0xFFFF);
				uint num15 = ((num7 >= num14) ? (num7 - num14) : 0u);
				if (!flag || num15 != num5)
				{
					if (!LoadHeadCluster(num15, lccnt, clustersize, adv, ofs, array, array3, ref headbuff))
					{
						break;
					}
					num5 = num15;
					flag = headbuff !=null;
				}
				if (flag && headbuff != null && num10 < clustersize)
				{
					int num16 = Math.Min(clustersize - num10, val);
					int num17 = output.Length - num13;
					if (num16 > num17)
					{
						num16 = num17;
					}
					if (num16 > 0)
					{
						Buffer.BlockCopy(headbuff, num10, output, num13, num16);
					}
					num6 = Math.Max(num6, num13 + num16);
				}
			}
			else if (num12 != 0)
			{
				long num18 = checked(num12 * Super.BlockSize);
				if (!ReadAtSafe(Stream, StreamBaseOffset + num18, array5, 0, array5.Length))
				{
					return false;
				}
				int num19;
				switch (num9)
				{
				default:
					if (num9 != 3)
					{
						goto IL_01ce;
					}
					goto case 1;
				case 0:
					if (num10 < clustersize)
					{
						num19 = Math.Min(clustersize - num10, val);
						int num20 = output.Length - num13;
						if (num19 > num20)
						{
							num19 = num20;
						}
						break;
					}
					goto IL_01ce;
				case 1:
				{
					byte[] array6 = new byte[clustersize];
					int num21 = Lz4Raw.Decompress(array5, 0, array5.Length, array6, 0, array6.Length);
					if (num10 < clustersize)
					{
						int num22 = Math.Min(clustersize - num10, val);
						int num23 = output.Length - num13;
						if (num22 > num23)
						{
							num22 = num23;
						}
						if (num22 > 0)
						{
							Buffer.BlockCopy(array6, num10, output, num13, num22);
						}
						num6 = Math.Max(num6, num13 + Math.Max(0, num21 - num10));
					}
					headbuff = array6;
					num5 = num7;
					flag = true;
					goto IL_01ce;
				}
				}
				if (num19 > 0)
				{
					Buffer.BlockCopy(array5, num10, output, num13, num19);
				}
				num6 = Math.Max(num6, num13 + num19);
			}
			goto IL_01ce;
			IL_01ce:
			num7++;
		}
		return false;
	}

	private bool LoadHeadCluster(uint headidx, uint lccnt, int clustersize, ushort[] adv, ushort[] ofs, ushort[] u2, uint[] u4, ref byte[] headbuff)
	{
		if (headidx >= lccnt)
		{
			return false;
		}
		int num = adv[headidx] & 3;
		if (num == 1 || num == 3)
		{
			ushort num2 = u2[headidx];
			uint num3 = ((num2 == ushort.MaxValue) ? u4[headidx] : num2);
			if (num3 != 0)
			{
				byte[] array = new byte[clustersize];
				long num4 = checked(num3 * Super.BlockSize);
				if (ReadAtSafe(Stream, StreamBaseOffset + num4, array, 0, array.Length))
				{
					byte[] array2 = new byte[clustersize];
					Lz4Raw.Decompress(array, 0, array.Length, array2, 0, array2.Length);
					headbuff = array2;
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	private bool DecodeCompactIndexSafe(long ubase, uint lccnt, int clustersize, int filesize, ushort[] adv, ushort[] ofs, int usize, bool typefromadv, out byte[] output)
	{
		object obj = null;
		int result = 0;
		try
		{
			return DecodeCompactIndex(ubase, lccnt, clustersize, filesize, adv, ofs, usize, typefromadv, out output);
		}
		catch (InvalidDataException)
		{
			output = (byte[])obj;
			return (byte)result != 0;
		}
	}

	private bool DecodeCompactIndex(long ubase, uint lccnt, int clustersize, int filesize, ushort[] adv, ushort[] ofs, int usize, bool typefromadv, out byte[] output)
	{
		object obj = null;
		output = new byte[filesize];
		byte[] array = new byte[usize];
		byte[] array2 = new byte[clustersize];
		byte[] src = (byte[])obj;
		uint num = 0u;
		bool flag = false;
		int num2 = 0;
		uint num3 = 0u;
		while (true)
		{
			if (num3 >= lccnt || num2 >= filesize)
			{
				if (!IsProps(output))
				{
					return false;
				}
				return true;
			}
			long num4 = ubase + num3 * usize;
			if (ReadAtSafe(Stream, StreamBaseOffset + num4, array, 0, array.Length))
			{
				int num6;
				uint num7;
				if (usize != 2)
				{
					int offset = 0;
					uint num5 = Binary.ReadU32LE(array, ref offset);
					num6 = ((!typefromadv) ? ((int)(num5 & 3)) : (adv[num3] & 3));
					num7 = (typefromadv ? num5 : (num5 >> 2));
				}
				else
				{
					int offset2 = 0;
					ushort num8 = Binary.ReadU16LE(array, ref offset2);
					num6 = ((!typefromadv) ? (num8 & 3) : (adv[num3] & 3));
					num7 = (uint)((!typefromadv) ? (num8 >> 2) : num8);
				}
				int val = filesize - num2;
				int num9 = (int)num3 * clustersize;
				if (num6 != 2)
				{
					if (num7 != 0)
					{
						long num10 = checked(num7 * Super.BlockSize);
						if (!ReadAtSafe(Stream, StreamBaseOffset + num10, array2, 0, array2.Length))
						{
							break;
						}
						int num11;
						int num12;
						switch (num6)
						{
						default:
							if (num6 != 3)
							{
								goto IL_015f;
							}
							goto case 1;
						case 0:
							num11 = ofs[num3];
							if (num11 < clustersize)
							{
								num12 = Math.Min(clustersize - num11, val);
								int num13 = output.Length - num9;
								if (num12 > num13)
								{
									num12 = num13;
								}
								break;
							}
							goto IL_015f;
						case 1:
						{
							byte[] array3 = new byte[clustersize];
							int num14 = Lz4Raw.Decompress(array2, 0, array2.Length, array3, 0, array3.Length);
							int num15 = ofs[num3];
							if (num15 < clustersize)
							{
								int num16 = Math.Min(clustersize - num15, val);
								int num17 = output.Length - num9;
								if (num16 > num17)
								{
									num16 = num17;
								}
								if (num16 > 0)
								{
									Buffer.BlockCopy(array3, num15, output, num9, num16);
								}
								num2 = Math.Max(num2, num9 + Math.Max(0, num14 - num15));
								src = array3;
								num = num3;
								flag = true;
							}
							goto IL_015f;
						}
						}
						if (num12 > 0)
						{
							Buffer.BlockCopy(array2, num11, output, num9, num12);
						}
						num2 = Math.Max(num2, num9 + num12);
					}
				}
				else if (flag && num <= num3)
				{
					int num18 = ofs[num3];
					if (num18 < clustersize)
					{
						int num19 = Math.Min(clustersize - num18, val);
						int num20 = output.Length - num9;
						if (num19 > num20)
						{
							num19 = num20;
						}
						if (num19 > 0)
						{
							Buffer.BlockCopy(src, num18, output, num9, num19);
						}
						num2 = Math.Max(num2, num9 + num19);
					}
				}
				goto IL_015f;
			}
			return false;
			IL_015f:
			num3++;
		}
		return false;
	}

	private static bool IsProps(byte[] data)
	{
		string value = "ro.";
		double field_ = Class38.field_3285;
		if (data != null && data.Length != 0)
		{
			int num = 0;
			foreach (byte b in data)
			{
				if (b == 9 || b == 10 || b == 13 || (b >= 32 && b <= 126))
				{
					num++;
				}
			}
			double num2 = (double)num / (double)data.Length;
			string text = Encoding.UTF8.GetString(data);
			bool result = text.IndexOf(value, StringComparison.Ordinal) >= 0;
			if (num2 < field_)
			{
				return false;
			}
			return result;
		}
		return false;
	}

	private static bool ReadAtSafe(Stream stream, long offset, byte[] buff, int outoff, int count)
	{
		int result = 0;
		try
		{
			ReadAt(stream, offset, buff, outoff, count);
			return true;
		}
		catch
		{
			return (byte)result != 0;
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
