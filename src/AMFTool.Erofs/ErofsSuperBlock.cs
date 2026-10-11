using System.IO;

namespace AMFTool.Erofs;

public class ErofsSuperBlock
{
	public uint Magic { get; }

	public byte BlockSizeBits { get; }

	public ushort RootNid { get; }

	public uint MetaBlockAddress { get; }

	public uint FeatureIncompat { get; }

	public ulong PackedNid { get; }

	public int BlockSize => 1 << (int)BlockSizeBits;

	private ErofsSuperBlock(uint magic, byte blocksizebits, ushort rootnid, uint metablockaddr, uint featureincompat, ulong packednid)
	{
		Magic = magic;
		BlockSizeBits = blocksizebits;
		RootNid = rootnid;
		MetaBlockAddress = metablockaddr;
		FeatureIncompat = featureincompat;
		PackedNid = packednid;
	}

	public static ErofsSuperBlock Parse(byte[] buffer)
	{
		string message = "EROFS superblock buffer too small.";
		if (buffer == null || buffer.Length < 128)
		{
			throw new InvalidDataException(message);
		}
		int offset = 0;
		uint magic = Binary.ReadU32LE(buffer, ref offset);
		Binary.ReadU32LE(buffer, ref offset);
		Binary.ReadU32LE(buffer, ref offset);
		byte blocksizebits = buffer[offset];
		offset++;
		offset++;
		ushort rootnid = Binary.ReadU16LE(buffer, ref offset);
		offset += 8;
		offset += 8;
		offset += 4;
		offset += 4;
		uint metablockaddr = Binary.ReadU32LE(buffer, ref offset);
		offset += 4;
		offset += 16;
		offset += 16;
		uint featureincompat = Binary.ReadU32LE(buffer, ref offset);
		offset += 2;
		offset += 2;
		offset += 2;
		offset++;
		offset++;
		offset += 4;
		ulong packednid = Binary.ReadU64LE(buffer, ref offset);
		return new ErofsSuperBlock(magic, blocksizebits, rootnid, metablockaddr, featureincompat, packednid);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
