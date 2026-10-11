using System;
using System.Security.Cryptography;

namespace AMFTool.Library.Hisilicon;

public class UpdateCrc16 : HashAlgorithm
{
	private readonly ushort[] _table = new ushort[256];

	private readonly ushort _polynomial;

	private readonly ushort _xorValue;

	private readonly byte[] _initialSum;

	public override bool CanReuseTransform => true;

	public override bool CanTransformMultipleBlocks => true;

	private void InitializeTable()
	{
		for (ushort num = 0; num < _table.Length; num++)
		{
			ushort num2 = 0;
			ushort num3 = num;
			for (byte b = 0; b < 8; b++)
			{
				num2 = ((((num2 ^ num3) & 1) == 0) ? ((ushort)(num2 >> 1)) : ((ushort)((num2 >> 1) ^ _polynomial)));
				num3 >>= 1;
			}
			_table[num] = num2;
		}
	}

	public UpdateCrc16(ushort initialSum = ushort.MaxValue, ushort polynomial = 33800, ushort xorValue = ushort.MaxValue)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		_initialSum = BitConverter.GetBytes(initialSum);
		_polynomial = polynomial;
		_xorValue = xorValue;
		InitializeTable();
		HashValue = _initialSum;
	}

	public override void Initialize()
	{
		HashValue = _initialSum;
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		ushort num = BitConverter.ToUInt16(HashValue, 0);
		int num2 = ibStart;
		int num3;
		for (num3 = (cbSize - ibStart) * 8; num3 >= 8; num3 -= 8)
		{
			byte b = array[num2++];
			num = (ushort)((_table[(b ^ num) & 0xFF] ^ (num >> 8)) & 0xFFFF);
		}
		if (num3 != 0)
		{
			int num4 = array[num2] << 8;
			while (num3 != 0)
			{
				num3--;
				bool flag = ((num ^ num4) & 1) == 0;
				num >>= 1;
				if (flag)
				{
					num ^= _polynomial;
				}
				num4 >>= 1;
			}
		}
		HashValue = BitConverter.GetBytes(num);
		HashSizeValue = HashValue.Length;
	}

	protected override byte[] HashFinal()
	{
		byte[] bytes = BitConverter.GetBytes((ushort)((BitConverter.ToUInt16(HashValue, 0) ^ _xorValue) & 0xFFFF));
		HashValue = _initialSum;
		return bytes;
	}

	public ushort ComputeSum(byte[] buffer)
	{
		return ComputeSum(buffer, 0, buffer.Length);
	}

	public ushort ComputeSum(byte[] buffer, int offset, int count)
	{
		return BitConverter.ToUInt16(ComputeHash(buffer, offset, count), 0);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
