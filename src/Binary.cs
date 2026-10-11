using System;
using System.Buffers.Binary;

public static class Binary
{
	public static ushort ReadU16LE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadUInt16LittleEndian(buffer.Slice(offset));
	}

	public static short ReadI16LE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadInt16LittleEndian(buffer.Slice(offset));
	}

	public static uint ReadU32LE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(offset));
	}

	public static int ReadI32LE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(offset));
	}

	public static ulong ReadU64LE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadUInt64LittleEndian(buffer.Slice(offset));
	}

	public static long ReadI64LE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadInt64LittleEndian(buffer.Slice(offset));
	}

	public static ushort ReadU16BE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(offset));
	}

	public static short ReadI16BE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(offset));
	}

	public static uint ReadU32BE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadUInt32BigEndian(buffer.Slice(offset));
	}

	public static int ReadI32BE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadInt32BigEndian(buffer.Slice(offset));
	}

	public static ulong ReadU64BE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadUInt64BigEndian(buffer.Slice(offset));
	}

	public static long ReadI64BE(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(offset));
	}

	public static void WriteU16LE(Span<byte> buffer, int offset, ushort value)
	{
		BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset), value);
	}

	public static void WriteI16LE(Span<byte> buffer, int offset, short value)
	{
		BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(offset), value);
	}

	public static void WriteU32LE(Span<byte> buffer, int offset, uint value)
	{
		BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset), value);
	}

	public static void WriteI32LE(Span<byte> buffer, int offset, int value)
	{
		BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(offset), value);
	}

	public static void WriteU64LE(Span<byte> buffer, int offset, ulong value)
	{
		BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(offset), value);
	}

	public static void WriteI64LE(Span<byte> buffer, int offset, long value)
	{
		BinaryPrimitives.WriteInt64LittleEndian(buffer.Slice(offset), value);
	}

	public static void WriteU16BE(Span<byte> buffer, int offset, ushort value)
	{
		BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(offset), value);
	}

	public static void WriteI16BE(Span<byte> buffer, int offset, short value)
	{
		BinaryPrimitives.WriteInt16BigEndian(buffer.Slice(offset), value);
	}

	public static void WriteU32BE(Span<byte> buffer, int offset, uint value)
	{
		BinaryPrimitives.WriteUInt32BigEndian(buffer.Slice(offset), value);
	}

	public static void WriteI32BE(Span<byte> buffer, int offset, int value)
	{
		BinaryPrimitives.WriteInt32BigEndian(buffer.Slice(offset), value);
	}

	public static void WriteU64BE(Span<byte> buffer, int offset, ulong value)
	{
		BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(offset), value);
	}

	public static void WriteI64BE(Span<byte> buffer, int offset, long value)
	{
		BinaryPrimitives.WriteInt64BigEndian(buffer.Slice(offset), value);
	}

	public static ushort ReadU16LE(byte[] buffer, ref int offset)
	{
		ushort result = ReadU16LE(buffer.AsSpan(), offset);
		offset += 2;
		return result;
	}

	public static ushort ReadU16BE(byte[] buffer, ref int offset)
	{
		ushort result = ReadU16BE(buffer.AsSpan(), offset);
		offset += 2;
		return result;
	}

	public static uint ReadU32LE(byte[] buffer, ref int offset)
	{
		uint result = ReadU32LE(buffer.AsSpan(), offset);
		offset += 4;
		return result;
	}

	public static uint ReadU32BE(byte[] buffer, ref int offset)
	{
		uint result = ReadU32BE(buffer.AsSpan(), offset);
		offset += 4;
		return result;
	}

	public static ulong ReadU64LE(byte[] buffer, ref int offset)
	{
		ulong result = ReadU64LE(buffer.AsSpan(), offset);
		offset += 8;
		return result;
	}

	public static ulong ReadU64BE(byte[] buffer, ref int offset)
	{
		ulong result = ReadU64BE(buffer.AsSpan(), offset);
		offset += 8;
		return result;
	}

	public static void WriteU16LE(byte[] buffer, ref int offset, ushort value)
	{
		WriteU16LE(buffer.AsSpan(), offset, value);
		offset += 2;
	}

	public static void WriteU16BE(byte[] buffer, ref int offset, ushort value)
	{
		WriteU16BE(buffer.AsSpan(), offset, value);
		offset += 2;
	}

	public static void WriteU32LE(byte[] buffer, ref int offset, uint value)
	{
		WriteU32LE(buffer.AsSpan(), offset, value);
		offset += 4;
	}

	public static void WriteU32BE(byte[] buffer, ref int offset, uint value)
	{
		WriteU32BE(buffer.AsSpan(), offset, value);
		offset += 4;
	}

	public static void WriteU64LE(byte[] buffer, ref int offset, ulong value)
	{
		WriteU64LE(buffer.AsSpan(), offset, value);
		offset += 8;
	}

	public static void WriteU64BE(byte[] buffer, ref int offset, ulong value)
	{
		WriteU64BE(buffer.AsSpan(), offset, value);
		offset += 8;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
