using System;

namespace AMFTool.Ext4;

public class Ext4Inode
{
	public uint InodeNumber { get; private set; }

	public ushort Mode { get; private set; }

	public uint Flags { get; private set; }

	public long Size { get; private set; }

	public byte[] IBlock { get; private set; }

	public byte[] InlineData { get; private set; }

	public int InlineDataLength { get; private set; }

	public bool HasExtents => (Flags & 0x80000) != 0;

	public bool HasInlineData => (Flags & 0x10000000) != 0;

	public bool IsDirectory => (Mode & 0xF000) == 16384;

	public bool IsFile => (Mode & 0xF000) == 32768;

	public bool IsSymlink => (Mode & 0xF000) == 40960;

	public static Ext4Inode Parse(byte[] inodeBuffer, uint inode)
	{
		object obj = null;
		if (inodeBuffer != null && inodeBuffer.Length >= 128)
		{
			int offset = 0;
			ushort mode = Binary.ReadU16LE(inodeBuffer, ref offset);
			offset = 4;
			uint num = Binary.ReadU32LE(inodeBuffer, ref offset);
			offset = 32;
			uint num2 = Binary.ReadU32LE(inodeBuffer, ref offset);
			uint num3 = 0u;
			if (inodeBuffer.Length >= 112)
			{
				offset = 108;
				num3 = Binary.ReadU32LE(inodeBuffer, ref offset);
			}
			long size = (long)(((ulong)num3 << 32) | num);
			byte[] array = new byte[60];
			if (inodeBuffer.Length >= 40 + array.Length)
			{
				Buffer.BlockCopy(inodeBuffer, 40, array, 0, array.Length);
			}
			byte[] array2 = (byte[])obj;
			int inlineDataLength = 0;
			if ((num2 & 0x10000000) != 0)
			{
				int num4 = Math.Max(0, inodeBuffer.Length - 40);
				array2 = new byte[num4];
				Buffer.BlockCopy(inodeBuffer, 40, array2, 0, num4);
				inlineDataLength = num4;
			}
			return new Ext4Inode
			{
				InodeNumber = inode,
				Mode = mode,
				Flags = num2,
				Size = size,
				IBlock = array,
				InlineData = array2,
				InlineDataLength = inlineDataLength
			};
		}
		return (Ext4Inode)obj;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
