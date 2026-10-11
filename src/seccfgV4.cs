using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using AMFTool;

public class seccfgV4
{
	public string hwtype;

	public int mtk;

	public int magic;

	public int seccfg_ver;

	public int seccfg_size;

	public int lock_state;

	public int critical_lock_state;

	public int sboot_runtime;

	public int endflag;

	public byte[] hash;

	public hwcrypto hwc;

	public seccfgV4(hwcrypto hwc)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		hwtype = string.Empty;
		this.hwc = hwc;
		mtk = 0;
		magic = 1296911693;
		seccfg_ver = 0;
		seccfg_size = 0;
		lock_state = 0;
		critical_lock_state = 0;
		sboot_runtime = 0;
		endflag = 1162167621;
		hash = new byte[0];
	}

	public bool parse(byte[] data)
	{
		string value = "StructhelperIo";
		string direction = "little";
		string value2 = "sej_sec_cfg_sw";
		string value3 = "dec_hash";
		string text = "SW";
		string text2 = "V2";
		string text3 = "V3";
		string value4 = "hwtype";
		Console.WriteLine(value);
		StructhelperIo structhelperIo = new StructhelperIo(new MemoryStream(data), direction);
		magic = structhelperIo.dword();
		seccfg_ver = structhelperIo.dword();
		seccfg_size = structhelperIo.dword();
		lock_state = structhelperIo.dword();
		critical_lock_state = structhelperIo.dword();
		sboot_runtime = structhelperIo.dword();
		endflag = structhelperIo.dword();
		structhelperIo.seek(seccfg_size - 32);
		hash = structhelperIo.bytes(32);
		if (magic == 1296911693 && endflag == 1162167621)
		{
			Console.WriteLine(value2);
			byte[] buffer = BitConverter.GetBytes(magic).Concat(BitConverter.GetBytes(seccfg_ver)).Concat(BitConverter.GetBytes(seccfg_size))
				.Concat(BitConverter.GetBytes(lock_state))
				.Concat(BitConverter.GetBytes(critical_lock_state))
				.Concat(BitConverter.GetBytes(sboot_runtime))
				.Concat(BitConverter.GetBytes(1162167621))
				.ToArray();
			byte[] array = SHA256.Create().ComputeHash(buffer);
			byte[] array2 = hwc.sej.sej_sec_cfg_sw(hash, encrypt: false);
			Console.WriteLine(value3);
			if (!((ReadOnlySpan<byte>)array).SequenceEqual((ReadOnlySpan<byte>)array2))
			{
				array2 = hwc.sej.sej_sec_cfg_hw(hash, encrypt: false);
				if (!((ReadOnlySpan<byte>)array).SequenceEqual((ReadOnlySpan<byte>)array2))
				{
					array2 = hwc.sej.sej_sec_cfg_hw_V3(hash, encrypt: false);
					if (!((ReadOnlySpan<byte>)array).SequenceEqual((ReadOnlySpan<byte>)array2))
					{
						return false;
					}
					hwtype = text3;
				}
				else
				{
					hwtype = text2;
				}
			}
			else
			{
				hwtype = text;
			}
			Console.WriteLine(value4);
			Console.WriteLine(hwtype);
			return true;
		}
		return false;
	}

	public (bool, byte[]) create_oppocfg(string lockflag = "unlock")
	{
		string objB = "unlock";
		string objB2 = "lock";
		string objB3 = "SW";
		string objB4 = "V2";
		string objB5 = "V3";
		if (object.Equals(lockflag, objB))
		{
			lock_state = 3;
			critical_lock_state = 1;
		}
		else if (object.Equals(lockflag, objB2))
		{
			lock_state = 1;
			critical_lock_state = 0;
		}
		byte[] array = BitConverter.GetBytes(magic).Concat(BitConverter.GetBytes(seccfg_ver)).Concat(BitConverter.GetBytes(seccfg_size))
			.Concat(BitConverter.GetBytes(lock_state))
			.Concat(BitConverter.GetBytes(critical_lock_state))
			.Concat(BitConverter.GetBytes(sboot_runtime))
			.Concat(BitConverter.GetBytes(1162167621))
			.ToArray();
		byte[] data = SHA256.Create().ComputeHash(array);
		byte[] second = new byte[0];
		if (!object.Equals(hwtype, objB3))
		{
			if (!object.Equals(hwtype, objB4))
			{
				if (object.Equals(hwtype, objB5))
				{
					second = hwc.sej.sej_sec_cfg_hw_V3(data);
				}
			}
			else
			{
				second = hwc.sej.sej_sec_cfg_hw(data);
			}
		}
		else
		{
			second = hwc.sej.sej_sec_cfg_sw(data);
		}
		byte[] array2 = array.Concat(second).ToArray();
		while (array2.Length % 512 != 0)
		{
			array2 = array2.Concat(new byte[1]).ToArray();
		}
		return (true, array2);
	}

	public (bool, byte[]) create(string lockflag = "unlock")
	{
		string objB = "lock";
		string msg = " Device is already locked";
		object item = null;
		string objB2 = "unlock";
		string msg2 = "Device is already unlocked";
		string objB3 = "SW";
		string objB4 = "V2";
		string objB5 = "V3";
		if (!object.Equals(lockflag, objB) || lock_state != 1)
		{
			if (!object.Equals(lockflag, objB2) || lock_state != 3)
			{
				if (object.Equals(lockflag, objB2))
				{
					lock_state = 3;
					critical_lock_state = 1;
				}
				else if (object.Equals(lockflag, objB))
				{
					lock_state = 1;
					critical_lock_state = 0;
				}
				byte[] array = BitConverter.GetBytes(magic).Concat(BitConverter.GetBytes(seccfg_ver)).Concat(BitConverter.GetBytes(seccfg_size))
					.Concat(BitConverter.GetBytes(lock_state))
					.Concat(BitConverter.GetBytes(critical_lock_state))
					.Concat(BitConverter.GetBytes(sboot_runtime))
					.Concat(BitConverter.GetBytes(1162167621))
					.ToArray();
				byte[] data = SHA256.Create().ComputeHash(array);
				byte[] second = new byte[0];
				if (!object.Equals(hwtype, objB3))
				{
					if (!object.Equals(hwtype, objB4))
					{
						if (object.Equals(hwtype, objB5))
						{
							second = hwc.sej.sej_sec_cfg_hw_V3(data);
						}
					}
					else
					{
						second = hwc.sej.sej_sec_cfg_hw(data);
					}
				}
				else
				{
					second = hwc.sej.sej_sec_cfg_sw(data);
				}
				byte[] array2 = array.Concat(second).ToArray();
				while (array2.Length % 512 != 0)
				{
					array2 = array2.Concat(new byte[1]).ToArray();
				}
				return (true, array2);
			}
			UI.RichLogs(msg2, UI.error, nextline: true, bold: true);
			return (false, (byte[])item);
		}
		UI.RichLogs(msg, UI.error, nextline: true, bold: true);
		return (false, (byte[])item);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	private object method_02()
	{
		return new int[0];
	}
}
