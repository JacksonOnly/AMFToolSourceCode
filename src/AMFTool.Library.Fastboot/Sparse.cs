using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace AMFTool.Library.Fastboot;

public class Sparse : IDisposable
{
	public const ushort CHUNK_TYPE_RAW = 51905;

	public const ushort CHUNK_TYPE_FILL = 51906;

	public const ushort CHUNK_TYPE_DONT_CARE = 51907;

	public const ushort CHUNK_TYPE_CRC32 = 51908;

	private bool disposed;

	public SparseHeader Header { get; private set; }

	public List<SparseChunk> chunks { get; private set; }

	private BinaryReader sparsebr { get; set; }

	public Sparse(BinaryReader br)
	{
		string message = "Not a valid sparse image!";
		object data = null;

		sparsebr = br;
		chunks = new List<SparseChunk>();
		SparseHeader header = SparseReader.ReadStruct<SparseHeader>(sparsebr);
		if (header.Magic == 3978755898u)
		{
			Header = header;
			for (int i = 0; i < Header.TotalChunks; i++)
			{
				long position = sparsebr.BaseStream.Position;
				ChunkHeader header2 = SparseReader.ReadStruct<ChunkHeader>(sparsebr);
				long position2 = sparsebr.BaseStream.Position;
				int num = (int)(header2.TotalSize - Marshal.SizeOf<ChunkHeader>());
				if (num > 0)
				{
					if (sparsebr.BaseStream.CanSeek)
					{
						sparsebr.BaseStream.Seek(num, SeekOrigin.Current);
					}
					else
					{
						sparsebr.ReadBytes(num);
					}
				}
				SparseChunk item = new SparseChunk
				{
					Header = header2,
					StartPosition = position,
					DataStartPosition = position2,
					DataSize = num,
					Data = (byte[])data
				};
				chunks.Add(item);
			}
			return;
		}
		throw new InvalidDataException(message);
	}

	public static bool IsSparseImage(Stream fileStream)
	{
		using BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.Default, leaveOpen: true);
		uint num = binaryReader.ReadUInt32();
		fileStream.Position = 0L;
		return num == 3978755898u;
	}

	public static bool IsSparseImage(string filepath)
	{
		using FileStream fileStream = File.OpenRead(filepath);
		return IsSparseImage(fileStream);
	}

	public void SplitSparse(int maxSize, Func<int, Stream> createOutputStream, Action<int, Stream> onSegmentReady)
	{
		object obj = null;
		string paramName = "createOutputStream";
		string paramName2 = "onSegmentReady";
		int num = 0;
		string message = "createOutputStream returned null.";
		string message2 = "createOutputStream must return a writable stream.";
		int num2 = 1;
		int num3 = 8;
		int num4 = 51905;
		int num5 = 51908;
		string message3 = "Unable to add gap block at end of segment, current segment size has reached maximum limit!";
		string message4 = "Underlying stream must be seekable for on-demand copy.";
		int num6 = 81920;
		string message5 = "Unexpected end of stream while copying chunk data.";
		if (createOutputStream != obj)
		{
			if (onSegmentReady == obj)
			{
				throw new ArgumentNullException(paramName2);
			}
			List<ChunkLayoutInfo> list = AnalyzeChunkLayout();
			int num7 = 0;
			int num8 = 0;
			while (num8 < chunks.Count)
			{
				using (Stream stream = createOutputStream(num7))
				{
					if (stream == obj)
					{
						throw new InvalidOperationException(message);
					}
					if ((stream.CanWrite ? 1 : 0) == num)
					{
						throw new InvalidOperationException(message2);
					}
					using (BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.UTF8, (byte)num2 != 0))
					{
						List<SparseChunk> list2 = new List<SparseChunk>();
						int num9 = Marshal.SizeOf<SparseHeader>();
						uint num10 = (uint)num;
						for (int i = num8; i < chunks.Count; i += num2)
						{
							SparseChunk sparseChunk = chunks[i];
							ChunkLayoutInfo chunkLayoutInfo = list[i];
							int num11 = Marshal.SizeOf<ChunkHeader>() + sparseChunk.DataSize;
							if (chunkLayoutInfo.StartBlock > num10)
							{
								uint blockCount = chunkLayoutInfo.StartBlock - num10;
								SparseChunk sparseChunk2 = CreateSkipChunk(blockCount);
								int num12 = Marshal.SizeOf<ChunkHeader>() + sparseChunk2.DataSize;
								if (((num9 + num12 + num11 > maxSize) ? 1 : 0) != num)
								{
									break;
								}
								list2.Add(sparseChunk2);
								num9 += num12;
								num10 = chunkLayoutInfo.StartBlock;
							}
							if (num9 + num11 > maxSize)
							{
								int num13 = maxSize - num9;
								if (((num13 > maxSize / num3) ? ((sparseChunk.Header.ChunkType == num4) ? 1 : 0) : num) == 0)
								{
									break;
								}
								(SparseChunk, SparseChunk) tuple = SplitRawChunk(sparseChunk, num13 - Marshal.SizeOf<ChunkHeader>(), Header.BlockSize);
								if (tuple.Item1.Header.ChunkSize <= (uint)num)
								{
									break;
								}
								list2.Add(tuple.Item1);
								num9 += Marshal.SizeOf<ChunkHeader>() + tuple.Item1.DataSize;
								num10 = chunkLayoutInfo.StartBlock + tuple.Item1.Header.ChunkSize;
								chunks[i] = tuple.Item2;
								list[i] = new ChunkLayoutInfo
								{
									StartBlock = num10,
									EndBlock = chunkLayoutInfo.EndBlock
								};
								i -= num2;
							}
							else
							{
								list2.Add(sparseChunk);
								num9 += num11;
								num10 = chunkLayoutInfo.EndBlock;
								num8 = i + num2;
							}
							if (sparseChunk.Header.ChunkType == num5)
							{
								break;
							}
						}
						uint num14 = num10;
						uint totalBlocks = Header.TotalBlocks;
						if (num14 < totalBlocks)
						{
							uint blockCount2 = totalBlocks - num14;
							SparseChunk sparseChunk3 = CreateSkipChunk(blockCount2);
							int num15 = Marshal.SizeOf<ChunkHeader>() + sparseChunk3.DataSize;
							if (((num9 + num15 > maxSize) ? 1 : 0) != num)
							{
								throw new InvalidOperationException(message3);
							}
							list2.Add(sparseChunk3);
							num9 += num15;
						}
						SparseHeader structure = CreateNewHeader((uint)list2.Count);
						WriteStruct(binaryWriter, structure);
						foreach (SparseChunk item in list2)
						{
							WriteStruct(binaryWriter, item.Header);
							if (item.DataSize <= num)
							{
								continue;
							}
							if (item.Data ==null)
							{
								if ((sparsebr.BaseStream.CanSeek ? 1 : 0) == num)
								{
									throw new NotSupportedException(message4);
								}
								long position = sparsebr.BaseStream.Position;
								try
								{
									sparsebr.BaseStream.Seek(item.DataStartPosition, (SeekOrigin)num);
									byte[] buffer = new byte[num6];
									int num16 = item.DataSize;
									while (num16 > num)
									{
										int count = Math.Min(num6, num16);
										int num17 = sparsebr.BaseStream.Read(buffer, num, count);
										if (((num17 > num) ? 1 : 0) == num)
										{
											throw new EndOfStreamException(message5);
										}
										stream.Write(buffer, num, num17);
										num16 -= num17;
									}
								}
								finally
								{
									sparsebr.BaseStream.Seek(position, (SeekOrigin)num);
								}
							}
							else
							{
								binaryWriter.Write(item.Data);
							}
						}
						try
						{
							stream.Flush();
						}
						catch
						{
						}
					}
					try
					{
						if (stream.CanSeek)
						{
							stream.Position = num;
						}
					}
					catch
					{
					}
					onSegmentReady(num7, stream);
				}
				num7 += num2;
			}
			return;
		}
		throw new ArgumentNullException(paramName);
	}

	internal List<ChunkLayoutInfo> AnalyzeChunkLayout()
	{
		List<ChunkLayoutInfo> list = new List<ChunkLayoutInfo>();
		uint num = 0u;
		foreach (SparseChunk chunk in chunks)
		{
			uint num2 = num;
			uint num3 = num2 + chunk.Header.ChunkSize;
			num2 = num;
			num3 = num2 + chunk.Header.ChunkSize;
			num = num3;
			list.Add(new ChunkLayoutInfo
			{
				StartBlock = num2,
				EndBlock = num3
			});
		}
		return list;
	}

	internal SparseChunk CreateSkipChunk(uint blockCount)
	{
		return new SparseChunk
		{
			Header = new ChunkHeader
			{
				ChunkType = 51907,
				Reserved1 = 0,
				ChunkSize = blockCount,
				TotalSize = (uint)Marshal.SizeOf<ChunkHeader>()
			},
			DataSize = 0,
			Data = Array.Empty<byte>(),
			DataStartPosition = -1L
		};
	}

	internal (SparseChunk FirstPart, SparseChunk SecondPart) SplitRawChunk(SparseChunk original, int maxDataSize, uint blockSize)
	{
		object data = null;
		int num = maxDataSize / (int)blockSize * (int)blockSize;
		int dataSize;
		ChunkHeader header;
		SparseChunk sparseChunk;
		int num2;
		ChunkHeader header2;
		SparseChunk item;
		if (num > 0)
		{
			dataSize = original.DataSize;
			header = original.Header;
			header.ChunkSize = (uint)(num / blockSize);
			header.TotalSize = (uint)(Marshal.SizeOf<ChunkHeader>() + num);
			sparseChunk = new SparseChunk
			{
				Header = header,
				DataSize = Math.Min(num, dataSize),
				Data = (byte[])data,
				DataStartPosition = original.DataStartPosition,
				StartPosition = original.StartPosition
			};
			num2 = dataSize - sparseChunk.DataSize;
			if (num2 >= 0)
			{
				header2 = original.Header;
				header2.ChunkSize = (uint)(num2 / blockSize);
				header2.TotalSize = (uint)(Marshal.SizeOf<ChunkHeader>() + num2);
				item = new SparseChunk
				{
					Header = header2,
					DataSize = num2,
					Data = (byte[])data,
					DataStartPosition = ((num2 > 0) ? (original.DataStartPosition + sparseChunk.DataSize) : (-1L)),
					StartPosition = original.StartPosition
				};
				return (FirstPart: sparseChunk, SecondPart: item);
			}
			num2 = 0;
			header2 = original.Header;
			header2.ChunkSize = (uint)(0L / (long)blockSize);
			header2.TotalSize = (uint)Marshal.SizeOf<ChunkHeader>();
			item = new SparseChunk
			{
				Header = header2,
				DataSize = 0,
				Data = (byte[])data,
				DataStartPosition = -1L,
				StartPosition = original.StartPosition
			};
			return (FirstPart: sparseChunk, SecondPart: item);
		}
		num = (int)blockSize;
		dataSize = original.DataSize;
		header = original.Header;
		header.ChunkSize = (uint)(num / blockSize);
		header.TotalSize = (uint)(Marshal.SizeOf<ChunkHeader>() + num);
		sparseChunk = new SparseChunk
		{
			Header = header,
			DataSize = Math.Min(num, dataSize),
			Data = (byte[])data,
			DataStartPosition = original.DataStartPosition,
			StartPosition = original.StartPosition
		};
		num2 = dataSize - sparseChunk.DataSize;
		if (num2 >= 0)
		{
			header2 = original.Header;
			header2.ChunkSize = (uint)(num2 / blockSize);
			header2.TotalSize = (uint)(Marshal.SizeOf<ChunkHeader>() + num2);
			item = new SparseChunk
			{
				Header = header2,
				DataSize = num2,
				Data = (byte[])data,
				DataStartPosition = ((num2 > 0) ? (original.DataStartPosition + sparseChunk.DataSize) : (-1L)),
				StartPosition = original.StartPosition
			};
			return (FirstPart: sparseChunk, SecondPart: item);
		}
		num2 = 0;
		header2 = original.Header;
		header2.ChunkSize = (uint)(0L / (long)blockSize);
		header2.TotalSize = (uint)Marshal.SizeOf<ChunkHeader>();
		item = new SparseChunk
		{
			Header = header2,
			DataSize = 0,
			Data = (byte[])data,
			DataStartPosition = -1L,
			StartPosition = original.StartPosition
		};
		return (FirstPart: sparseChunk, SecondPart: item);
	}

	internal SparseHeader CreateNewHeader(uint chunkcount)
	{
		return new SparseHeader
		{
			Magic = Header.Magic,
			MajorVersion = Header.MajorVersion,
			MinorVersion = Header.MinorVersion,
			FileHeaderSize = Header.FileHeaderSize,
			ChunkHeaderSize = Header.ChunkHeaderSize,
			BlockSize = Header.BlockSize,
			TotalBlocks = Header.TotalBlocks,
			TotalChunks = chunkcount,
			ImageChecksum = Header.ImageChecksum
		};
	}

	public static void WriteStruct<T>(BinaryWriter writer, T structure) where T : struct
	{
		int num = Marshal.SizeOf(typeof(T));
		byte[] array = new byte[num];
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		try
		{
			Marshal.StructureToPtr(structure, gCHandle.AddrOfPinnedObject(), fDeleteOld: false);
			writer.Write(array);
		}
		finally
		{
			gCHandle.Free();
		}
	}

	internal void EnsureChunkDataLoaded(SparseChunk chunk)
	{
		object obj = null;
		int origin = 0;
		string message = "Underlying stream is not seekable; cannot load chunk data on demand.";
		string message2 = "Unexpected end of stream while reading chunk data.";
		if (chunk.Data !=null)
		{
			return;
		}
		if (chunk.DataSize > 0)
		{
			if (!sparsebr.BaseStream.CanSeek)
			{
				throw new NotSupportedException(message);
			}
			long position = sparsebr.BaseStream.Position;
			try
			{
				sparsebr.BaseStream.Seek(chunk.DataStartPosition, SeekOrigin.Begin);
				byte[] array = new byte[chunk.DataSize];
				int num;
				for (int i = 0; i < array.Length; i += num)
				{
					num = sparsebr.BaseStream.Read(array, i, array.Length - i);
					if (num <= 0)
					{
						throw new EndOfStreamException(message2);
					}
				}
				chunk.Data = array;
				return;
			}
			finally
			{
				sparsebr.BaseStream.Seek(position, (SeekOrigin)origin);
			}
		}
		chunk.Data = Array.Empty<byte>();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		object obj = null;
		int num = 1;
		if (disposed)
		{
			return;
		}
		if (disposing)
		{
			((BinaryReader)(object)sparsebr)?.Dispose();
			sparsebr = (BinaryReader)obj;
			if (chunks !=null)
			{
				foreach (SparseChunk chunk in chunks)
				{
					chunk.Data = (byte[])obj;
				}
				chunks.Clear();
			}
		}
		disposed = (byte)num != 0;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
