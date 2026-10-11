using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AMFTool;
using DevExpress.XtraEditors;

namespace Restored.Namespace3;

internal class Class28
{
	public string field_01 = string.Empty;

	public string field_02 = string.Empty;

	public string field_03 = string.Empty;

	public string field_04 = string.Empty;

	public string field_05 = string.Empty;

	public RichTextBox field_06;

	public Class28()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		field_06 = new RichTextBox();
	}

	public string method_01(string E838BA8C, string BEAEE859)
	{
		char c = Convert.ToChar(E838BA8C.Substring(0, 1));
		string text = E838BA8C.Substring(2);
		char[] array = text.ToCharArray();
		ArrayList arrayList = new ArrayList();
		arrayList.Add(c);
		arrayList.Add(array[1]);
		arrayList.Add(array[0]);
		arrayList.Add(array[3]);
		arrayList.Add(array[2]);
		arrayList.Add(array[5]);
		arrayList.Add(array[4]);
		arrayList.Add(array[7]);
		arrayList.Add(array[6]);
		arrayList.Add(array[9]);
		arrayList.Add(array[8]);
		arrayList.Add(array[11]);
		arrayList.Add(array[10]);
		arrayList.Add(array[13]);
		arrayList.Add(array[12]);
		StringBuilder stringBuilder = new StringBuilder();
		if (arrayList.Count == 15)
		{
			for (int i = 0; i <= 14; i++)
			{
				stringBuilder.Append(arrayList[i]);
			}
		}
		return method_02(ref BEAEE859, stringBuilder.ToString());
	}

	private static T method_02<T>(ref T DF99154E, T DC4A3252)
	{
		DF99154E = DC4A3252;
		return DC4A3252;
	}

	public void method_03(string C7EA79C0, bool C228C5F5 = false)
	{
		string msg = " ⦁ IMEI 1 : ";
		int buttons = 0;
		int num = 1;
		string msg2 = " ⦁ IMEI 2 : ";
		string msg3 = "Generating QCN for Dual SIM...";
		string msg4 = "Failed";
		string caption = "Error !";
		int icon = 16;
		string msg5 = "OK";
		string text = "Completed Successfully !";
		string caption2 = "Successful...";
		int icon2 = 64;
		if (C228C5F5)
		{
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(DeviceState.IMEI1, UI.info, nextline: true, bold: true);
			UI.RichLogs(msg2, UI.log);
			UI.RichLogs(DeviceState.IMEI2, UI.info, nextline: true, bold: true);
			UI.RichLogs(msg3, UI.log);
		}
		string[] array = method_06(DeviceState.IMEI1);
		string[] array2 = method_06(DeviceState.IMEI2);
		BinaryReader binaryReader = new BinaryReader(File.Open(field_01, FileMode.Open), Encoding.ASCII);
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(C7EA79C0, FileMode.Create), Encoding.ASCII);
		long num2 = int.Parse(field_03);
		long num3 = int.Parse(field_02);
		long length = binaryReader.BaseStream.Length;
		try
		{
			do
			{
				byte value = binaryReader.ReadByte();
				binaryWriter.Write(value);
				if (binaryReader.BaseStream.Position == num2)
				{
					int location = 0;
					while (location < array.Length)
					{
						binaryWriter.Write(byte.Parse(array[location], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array.Length;
				}
				else if (binaryReader.BaseStream.Position == num3)
				{
					int location2 = 0;
					while (location2 < array2.Length)
					{
						binaryWriter.Write(byte.Parse(array2[location2], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location2);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array2.Length;
				}
			}
			while (binaryReader.BaseStream.Position != length);
		}
		catch (EndOfStreamException ex)
		{
			if (C228C5F5)
			{
				UI.RichLogs(msg4, Color.Red, (byte)num != 0, (byte)num != 0);
				UI.RichLogs(ex.Message, Color.Red, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(ex.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
			}
		}
		catch (ArgumentException ex2)
		{
			if (!C228C5F5)
			{
				XtraMessageBox.Show(ex2.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
				return;
			}
			UI.RichLogs(msg4, Color.Red, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(ex2.Message, Color.Red, (byte)num != 0, (byte)num != 0);
		}
		finally
		{
			binaryReader.Close();
			binaryReader.Dispose();
			binaryWriter.Close();
			binaryWriter.Dispose();
			if (C228C5F5)
			{
				UI.RichLogs(msg5, Color.GreenYellow, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(text, caption2, (MessageBoxButtons)buttons, (MessageBoxIcon)icon2);
			}
		}
	}

	public void method_04(string C2C7B764, bool C7D44D5B = false)
	{
		string msg = " ⦁ IMEI 1 : ";
		int buttons = 0;
		int num = 1;
		string msg2 = "Generating QCN for Single SIM...";
		string text = "00";
		string msg3 = "Failed";
		string caption = "Error !";
		int icon = 16;
		string msg4 = "OK";
		string text2 = "Completed Successfully !";
		string caption2 = "Successful...";
		int icon2 = 64;
		if (C7D44D5B)
		{
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(DeviceState.IMEI1, UI.info, nextline: true, bold: true);
			UI.RichLogs(msg2, UI.log);
		}
		string[] array = method_06(DeviceState.IMEI1);
		string[] array2 = new string[15];
		for (int i = 0; i <= 14; i++)
		{
			array2[i] = text;
		}
		BinaryReader binaryReader = new BinaryReader(File.Open(field_01, FileMode.Open), Encoding.ASCII);
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(C2C7B764, FileMode.Create), Encoding.ASCII);
		long num2 = int.Parse(field_03);
		long num3 = int.Parse(field_02);
		long length = binaryReader.BaseStream.Length;
		try
		{
			do
			{
				byte value = binaryReader.ReadByte();
				binaryWriter.Write(value);
				if (binaryReader.BaseStream.Position == num2)
				{
					int location = 0;
					while (location < array.Length)
					{
						binaryWriter.Write(byte.Parse(array[location], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array.Length;
				}
				else if (binaryReader.BaseStream.Position == num3)
				{
					int location2 = 0;
					while (location2 < array2.Length)
					{
						binaryWriter.Write(byte.Parse(array2[location2], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location2);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array2.Length;
				}
			}
			while (binaryReader.BaseStream.Position != length);
		}
		catch (EndOfStreamException ex)
		{
			if (C7D44D5B)
			{
				UI.RichLogs(msg3, Color.Red, (byte)num != 0, (byte)num != 0);
				UI.RichLogs(ex.Message, Color.Red, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(ex.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
			}
		}
		catch (ArgumentException ex2)
		{
			if (!C7D44D5B)
			{
				XtraMessageBox.Show(ex2.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
				return;
			}
			UI.RichLogs(msg3, Color.Red, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(ex2.Message, Color.Red, (byte)num != 0, (byte)num != 0);
		}
		finally
		{
			binaryReader.Close();
			binaryReader.Dispose();
			binaryWriter.Close();
			binaryWriter.Dispose();
			if (C7D44D5B)
			{
				UI.RichLogs(msg4, Color.GreenYellow, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(text2, caption2, (MessageBoxButtons)buttons, (MessageBoxIcon)icon2);
			}
		}
	}

	public void method_05(string A315B6D6, bool AA3F2704 = false)
	{
		string msg = " ⦁ IMEI 2 : ";
		int buttons = 0;
		int num = 1;
		string msg2 = "Generating QCN for SIM 2 Only...";
		string text = "00";
		string msg3 = "Failed";
		string caption = "Error !";
		int icon = 16;
		string msg4 = "OK";
		string text2 = "Completed Successfully !";
		string caption2 = "Successful...";
		int icon2 = 64;
		if (AA3F2704)
		{
			UI.RichLogs(msg, UI.log);
			UI.RichLogs(DeviceState.IMEI2, UI.info, nextline: true, bold: true);
			UI.RichLogs(msg2, UI.log);
		}
		string[] array = new string[15];
		string[] array2 = method_06(DeviceState.IMEI2);
		for (int i = 0; i <= 14; i++)
		{
			array[i] = text;
		}
		BinaryReader binaryReader = new BinaryReader(File.Open(field_01, FileMode.Open), Encoding.ASCII);
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(A315B6D6, FileMode.Create), Encoding.ASCII);
		long num2 = int.Parse(field_03);
		long num3 = int.Parse(field_02) - 7L;
		long length = binaryReader.BaseStream.Length;
		try
		{
			do
			{
				byte value = binaryReader.ReadByte();
				binaryWriter.Write(value);
				if (binaryReader.BaseStream.Position == num2)
				{
					int location = 0;
					while (location < array.Length)
					{
						binaryWriter.Write(byte.Parse(array[location], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array.Length;
				}
				else if (binaryReader.BaseStream.Position == num3)
				{
					int location2 = 0;
					while (location2 < array2.Length)
					{
						binaryWriter.Write(byte.Parse(array2[location2], NumberStyles.AllowHexSpecifier));
						Interlocked.Increment(ref location2);
					}
					binaryReader.BaseStream.Position = binaryReader.BaseStream.Position + array2.Length;
				}
			}
			while (binaryReader.BaseStream.Position != length);
		}
		catch (EndOfStreamException ex)
		{
			if (!AA3F2704)
			{
				XtraMessageBox.Show(ex.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
				return;
			}
			UI.RichLogs(msg3, Color.Red, (byte)num != 0, (byte)num != 0);
			UI.RichLogs(ex.Message, Color.Red, (byte)num != 0, (byte)num != 0);
		}
		catch (ArgumentException ex2)
		{
			if (AA3F2704)
			{
				UI.RichLogs(msg3, Color.Red, (byte)num != 0, (byte)num != 0);
				UI.RichLogs(ex2.Message, Color.Red, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(ex2.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
			}
		}
		finally
		{
			binaryReader.Close();
			binaryReader.Dispose();
			binaryWriter.Close();
			binaryWriter.Dispose();
			if (AA3F2704)
			{
				UI.RichLogs(msg4, Color.GreenYellow, (byte)num != 0, (byte)num != 0);
			}
			else
			{
				XtraMessageBox.Show(text2, caption2, (MessageBoxButtons)buttons, (MessageBoxIcon)icon2);
			}
		}
	}

	public string[] method_06(string B5E31E94)
	{
		string text = "A";
		char[] array = B5E31E94.ToCharArray();
		Array.Reverse(array);
		string text2 = new string(array) + text;
		string[] array2 = new string[8];
		int length = text2.Length;
		int location = 0;
		if (length != 0)
		{
			int num = length - 2;
			array2[location] = text2.Substring(num, 2);
			length = num;
			Interlocked.Increment(ref location);
			int num2 = 608045169;
			while (true)
			{
				_ = num2 ^ -1731191176;
				if (length == 0)
				{
					break;
				}
				num = length - 2;
				array2[location] = text2.Substring(num, 2);
				length = num;
				Interlocked.Increment(ref location);
				num2 = 608045169;
			}
			return array2;
		}
		return array2;
	}

	public void method_07(string EEECB603)
	{
		int buttons = 0;
		string separator = "";
		string value = "01002602000008";
		int icon = 16;
		double field_ = Class38.field_202;
		string caption = "Error !";
		FileStream fileStream = new FileStream(EEECB603, FileMode.Open, FileAccess.Read);
		try
		{
			byte[] array = File.ReadAllBytes(EEECB603);
			fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
			string text = BitConverter.ToString(array);
			string[] value2 = text.Split('-').ToArray();
			string text2 = string.Join(separator, value2);
			field_06.Text = text2;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			for (int i = 0; i <= 1; i++)
			{
				num3 = field_06.Text.IndexOf(value, num);
				arrayList.Add(field_06.Text.Substring(num3 + 14, 16));
				num = num3 + 30;
				num2 = num - 16;
				arrayList2.Add((double)num2 / field_);
			}
			field_04 = arrayList[0].ToString();
			field_05 = arrayList[1].ToString();
			field_02 = arrayList2[0].ToString();
			field_03 = arrayList2[1].ToString();
			Console.WriteLine(field_04);
			Console.WriteLine(field_05);
			Console.WriteLine(field_02);
			Console.WriteLine(field_03);
		}
		catch (EndOfStreamException ex)
		{
			XtraMessageBox.Show(ex.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
		}
		catch (ArgumentException ex2)
		{
			XtraMessageBox.Show(ex2.Message, caption, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
		}
		finally
		{
			fileStream.Close();
			fileStream.Dispose();
		}
	}

	private static int method_08(int C9DBBCE2)
	{
		double field_ = Class38.field_3213;
		int num = 0;
		if (C9DBBCE2 <= 0)
		{
			int num2 = 0;
			return 0;
		}
		num = 0 + C9DBBCE2 % 10;
		int num3 = 1053353277;
		while (true)
		{
			_ = num3 ^ -841562490;
			C9DBBCE2 = (int)Math.Round((double)C9DBBCE2 / field_);
			if (C9DBBCE2 <= 0)
			{
				break;
			}
			num += C9DBBCE2 % 10;
			num3 = 1053353277;
		}
		return num;
	}

	private static bool method_09(long EA229E6E)
	{
		double field_ = Class38.field_3213;
		string text = EA229E6E.ToString();
		int length = text.Length;
		int num2;
		int num;
		int num5;
		int num6;
		int num3;
		if (length == 15)
		{
			num = 0;
			num2 = length;
			if (num2 < 1)
			{
				return true;
			}
			num3 = (int)(EA229E6E % 10L);
			if (num2 % 2 != 0)
			{
				num = 0 + method_08(num3);
				EA229E6E = (long)Math.Round((double)EA229E6E / field_);
				int num4 = 663661239;
				while (true)
				{
					_ = num4 ^ 0x46957216;
					num2--;
					if (num2 < 1)
					{
						return num % 10 == 0;
					}
					num3 = (int)(EA229E6E % 10L);
					if (num2 % 2 == 0)
					{
						break;
					}
					num += method_08(num3);
					EA229E6E = (long)Math.Round((double)EA229E6E / field_);
					num4 = 663661239;
				}
				num5 = 2077460007;
				goto IL_011d;
			}
			num3 = 2 * num3;
			num = 0 + method_08(num3);
			EA229E6E = (long)Math.Round((double)EA229E6E / field_);
			num6 = 663661239;
			goto IL_00a8;
		}
		return false;
		IL_00a8:
		_ = num6 ^ 0x46957216;
		num2--;
		if (num2 >= 1)
		{
			num3 = (int)(EA229E6E % 10L);
			if (num2 % 2 != 0)
			{
				num += method_08(num3);
				goto IL_013e;
			}
			num5 = 2077460007;
			goto IL_011d;
		}
		return num % 10 == 0;
		IL_011d:
		_ = num5 ^ 0x46957216;
		num3 = 2 * num3;
		num += method_08(num3);
		goto IL_013e;
		IL_013e:
		EA229E6E = (long)Math.Round((double)EA229E6E / field_);
		num6 = 663661239;
		goto IL_00a8;
	}

	public static int method_10(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
