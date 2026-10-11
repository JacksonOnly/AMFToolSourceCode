using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using AMFTool;

public class seccfgV3
{
	private byte[] data;

	private byte[] org_data;

	private byte[] info;

	private int magic;

	private int seccfg_ver;

	private int seccfg_size;

	private int seccfg_enc_len;

	private int seccfg_enc_offset;

	private int endflag;

	private byte sw_sec_lock_try;

	private byte sw_sec_lock_done;

	private short page_size;

	private int page_count;

	private byte[] imginfo;

	private int siu_status;

	private int seccfg_status;

	private int seccfg_attr;

	private byte[] seccfg_ext;

	public hwcrypto hwc;

	public string hwtype;

	public seccfgV3(hwcrypto hwc)
	{
		string s = "AND_SECCFG_v\0\0\0\0";

		this.hwc = hwc;
		info = Encoding.ASCII.GetBytes(s);
		magic = 1296911693;
		seccfg_ver = 3;
		seccfg_size = 6240;
		seccfg_enc_len = 16777216;
		seccfg_enc_offset = 0;
		endflag = 1162167621;
		sw_sec_lock_try = 0;
		sw_sec_lock_done = 0;
		page_size = 0;
		page_count = 0;
		imginfo = new byte[2080];
		siu_status = 0;
		seccfg_status = SECCFG_STATUSS.SEC_CFG_COMPLETE_NUM;
		seccfg_attr = 858993459;
		seccfg_ext = new byte[4100];
		hwtype = string.Empty;
	}

	public bool parse(byte[] didata)
	{
		string s = "AND_SECCFG_v\0\0\0\0";
		string value = "start struct";
		string direction = "little";
		string value2 = "Unknown V3 seccfg structure !";
		string value3 = "sej_sec_cfg_sw";
		string value4 = "hwtype";
		string value5 = "Unknown V3 seccfg encryption !";
		string text = "V3";
		string text2 = "V2";
		string text3 = "SW";
		string value6 = "start 2";
		if (data.Take(16).SequenceEqual(Encoding.ASCII.GetBytes(s)))
		{
			Console.WriteLine(value);
			StructhelperIo structhelperIo = new StructhelperIo(new MemoryStream(didata), direction);
			info = structhelperIo.bytes(16);
			magic = structhelperIo.dword();
			seccfg_ver = structhelperIo.dword();
			seccfg_size = structhelperIo.dword();
			seccfg_enc_offset = structhelperIo.dword();
			seccfg_enc_len = structhelperIo.dword();
			sw_sec_lock_try = structhelperIo.bytes()[0];
			sw_sec_lock_done = structhelperIo.bytes()[0];
			page_size = structhelperIo._short();
			page_count = structhelperIo.dword();
			data = structhelperIo.bytes(seccfg_size - 44 - 4);
			endflag = structhelperIo.dword();
			if (magic == 1296911693 && endflag == 1162167621)
			{
				Console.WriteLine(value3);
				byte[] ret = hwc.sej.sej_sec_cfg_sw(data, encrypt: false);
				List<byte[]> list = new List<byte[]>();
				list.Add(BitConverter.GetBytes(1229539657));
				list.Add(BitConverter.GetBytes(1128481603));
				list.Add(new byte[4]);
				List<byte[]> source = list;
				Console.WriteLine(value4);
				if (source.Any((byte[] x) => ((ReadOnlySpan<byte>)x).SequenceEqual((ReadOnlySpan<byte>)ret.Take(4).ToArray())))
				{
					hwtype = text3;
				}
				else
				{
					ret = hwc.sej.sej_sec_cfg_hw_V3(data, encrypt: false);
					if (source.Any((byte[] x) => ((ReadOnlySpan<byte>)x).SequenceEqual((ReadOnlySpan<byte>)ret.Take(4).ToArray())))
					{
						hwtype = text2;
					}
					else
					{
						ret = hwc.sej.sej_sec_cfg_hw(data, encrypt: false);
						if (!source.Any((byte[] x) => ((ReadOnlySpan<byte>)x).SequenceEqual((ReadOnlySpan<byte>)ret.Take(4).ToArray())))
						{
							Console.WriteLine(value5);
							return false;
						}
						hwtype = text;
					}
				}
				Console.WriteLine(value6);
				org_data = ret;
				StructhelperIo structhelperIo2 = new StructhelperIo(new MemoryStream(ret), direction);
				for (int num = 0; num <= 19; num++)
				{
					imginfo = imginfo.Concat(structhelperIo2.bytes(104)).ToArray();
				}
				siu_status = structhelperIo2.dword();
				seccfg_status = structhelperIo2.dword();
				if (seccfg_status == SECCFG_STATUSS.SEC_CFG_COMPLETE_NUM || seccfg_status == SECCFG_STATUSS.SEC_CFG_INCOMPLETE_NUM)
				{
					seccfg_attr = structhelperIo2.dword();
					if (seccfg_attr != 858993459 && seccfg_attr != 1145324612 && seccfg_attr != 24579 && seccfg_attr != 24576 && seccfg_attr != 24578 && seccfg_attr != 24577)
					{
						return false;
					}
					seccfg_ext = structhelperIo2.bytes(4100);
					return true;
				}
				return false;
			}
			Console.WriteLine(value2);
			return false;
		}
		return false;
	}

	public (bool, byte[]) create(string lockflag = "unlock")
	{
		string objB = "unlock";
		string objB2 = "lock";
		string msg = "Failed";
		object item = null;
		string objB3 = "SW";
		string objB4 = "V2";
		string objB5 = "V3";
		int value = 858993459;
		if (!object.Equals(lockflag, objB))
		{
			if (object.Equals(lockflag, objB2))
			{
				seccfg_enc_len = 16777216;
				value = 858993459;
			}
		}
		else
		{
			seccfg_enc_len = 133300224;
			value = 1145324612;
		}
		if (!object.Equals(lockflag, objB2) || seccfg_attr == 1145324612)
		{
			if (!object.Equals(lockflag, objB) || seccfg_attr == 858993459 || seccfg_attr == 24579 || seccfg_attr == 24578 || seccfg_attr == 24577 || seccfg_attr == 24576)
			{
				byte[] buffer = new byte[0];
				MemoryStream memoryStream = new MemoryStream(buffer);
				memoryStream.Write(info, 0, info.Length);
				memoryStream.Write(BitConverter.GetBytes(magic), 0, 4);
				memoryStream.Write(BitConverter.GetBytes(seccfg_ver), 0, 4);
				memoryStream.Write(BitConverter.GetBytes(seccfg_size), 0, 4);
				memoryStream.Write(BitConverter.GetBytes(seccfg_enc_offset), 0, 4);
				memoryStream.Write(BitConverter.GetBytes(seccfg_enc_len), 0, 4);
				memoryStream.Write(BitConverter.GetBytes(sw_sec_lock_try), 0, 1);
				memoryStream.Write(BitConverter.GetBytes(sw_sec_lock_done), 0, 1);
				memoryStream.Write(BitConverter.GetBytes(page_size), 0, 2);
				memoryStream.Write(BitConverter.GetBytes(page_count), 0, 4);
				MemoryStream memoryStream2 = new MemoryStream();
				for (int i = 0; i <= 19; i++)
				{
					memoryStream2.Write(imginfo.Skip(i * 104).Take(104).ToArray(), 0, imginfo.Length);
				}
				memoryStream2.Write(BitConverter.GetBytes(siu_status), 0, 4);
				memoryStream2.Write(BitConverter.GetBytes(seccfg_status), 0, 4);
				memoryStream2.Write(BitConverter.GetBytes(value), 0, 4);
				memoryStream2.Write(seccfg_ext, 0, seccfg_ext.Length);
				buffer = memoryStream2.GetBuffer();
				if (object.Equals(hwtype, objB3))
				{
					buffer = hwc.sej.sej_sec_cfg_sw(buffer);
				}
				else if (!object.Equals(hwtype, objB4))
				{
					if (!object.Equals(hwtype, objB5))
					{
						UI.RichLogs(msg, Color.Tomato, nextline: true, bold: true);
						return (false, (byte[])item);
					}
					buffer = hwc.sej.sej_sec_cfg_hw_V3(buffer);
				}
				else
				{
					buffer = hwc.sej.sej_sec_cfg_hw(buffer);
				}
				memoryStream.Write(buffer, 0, buffer.Length);
				memoryStream.Write(BitConverter.GetBytes(endflag), 0, 4);
				buffer = memoryStream.GetBuffer();
				while (buffer.Length % 512 != 0)
				{
					buffer = buffer.Concat(new byte[1]).ToArray();
				}
				return (true, buffer);
			}
			UI.RichLogs(msg, Color.Tomato, nextline: true, bold: true);
			return (false, (byte[])item);
		}
		UI.RichLogs(msg, Color.Tomato, nextline: true, bold: true);
		return (false, (byte[])item);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
