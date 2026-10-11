using System.IO;

namespace AMFTool.Erofs;

public class ErofsInode
{
	public ushort AdviseFlags { get; }

	public ushort XattrCount { get; }

	public ushort Mode { get; }

	public long FileSize { get; }

	public uint DataPointerOrClusterCount { get; }

	public uint InodeNumber { get; }

	public bool IsExtended { get; }

	public int HeaderSize { get; }

	public ErofsDataMappingMode MappingMode => (ErofsDataMappingMode)((AdviseFlags >> 1) & 7);

	public int XattrSize
	{
		get
		{
			if (XattrCount != 0)
			{
				return 12 + (XattrCount - 1) * 4;
			}
			int num = 0;
			return 0;
		}
	}

	public bool IsDirectory => (Mode & 0xF000) == 16384;

	public bool IsRegularFile => (Mode & 0xF000) == 32768;

	public bool IsSymlink => (Mode & 0xF000) == 40960;

	private ErofsInode(ushort adviseflags, ushort xattrcount, ushort mode, long filesize, uint dataptrorclustercount, uint inodenumber, bool isextent, int headersize)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		AdviseFlags = adviseflags;
		XattrCount = xattrcount;
		Mode = mode;
		FileSize = filesize;
		DataPointerOrClusterCount = dataptrorclustercount;
		InodeNumber = inodenumber;
		IsExtended = isextent;
		HeaderSize = headersize;
	}

	public static ErofsInode Parse(byte[] buffer, bool isextent)
	{
		object obj = null;
		string message = "EROFS inode buffer is null.";
		string message2 = "EROFS inode buffer too small.";
		if (buffer != obj)
		{
			if (!isextent)
			{
				if (buffer.Length >= 32)
				{
					int offset = 0;
					ushort adviseflags = Binary.ReadU16LE(buffer, ref offset);
					ushort xattrcount = Binary.ReadU16LE(buffer, ref offset);
					ushort mode = Binary.ReadU16LE(buffer, ref offset);
					offset += 2;
					long filesize;
					uint dataptrorclustercount;
					uint inodenumber;
					if (isextent)
					{
						filesize = (long)Binary.ReadU64LE(buffer, ref offset);
						dataptrorclustercount = Binary.ReadU32LE(buffer, ref offset);
						inodenumber = Binary.ReadU32LE(buffer, ref offset);
						return new ErofsInode(adviseflags, xattrcount, mode, filesize, dataptrorclustercount, inodenumber, isextent, 32);
					}
					filesize = Binary.ReadU32LE(buffer, ref offset);
					offset += 4;
					dataptrorclustercount = Binary.ReadU32LE(buffer, ref offset);
					inodenumber = Binary.ReadU32LE(buffer, ref offset);
					return new ErofsInode(adviseflags, xattrcount, mode, filesize, dataptrorclustercount, inodenumber, isextent, 32);
				}
				throw new InvalidDataException(message2);
			}
			if (buffer.Length >= 64)
			{
				int offset = 0;
				ushort adviseflags = Binary.ReadU16LE(buffer, ref offset);
				ushort xattrcount = Binary.ReadU16LE(buffer, ref offset);
				ushort mode = Binary.ReadU16LE(buffer, ref offset);
				offset += 2;
				long filesize;
				uint dataptrorclustercount;
				uint inodenumber;
				if (isextent)
				{
					filesize = (long)Binary.ReadU64LE(buffer, ref offset);
					dataptrorclustercount = Binary.ReadU32LE(buffer, ref offset);
					inodenumber = Binary.ReadU32LE(buffer, ref offset);
					return new ErofsInode(adviseflags, xattrcount, mode, filesize, dataptrorclustercount, inodenumber, isextent, 64);
				}
				filesize = Binary.ReadU32LE(buffer, ref offset);
				offset += 4;
				dataptrorclustercount = Binary.ReadU32LE(buffer, ref offset);
				inodenumber = Binary.ReadU32LE(buffer, ref offset);
				return new ErofsInode(adviseflags, xattrcount, mode, filesize, dataptrorclustercount, inodenumber, isextent, 64);
			}
			throw new InvalidDataException(message2);
		}
		throw new InvalidDataException(message);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
