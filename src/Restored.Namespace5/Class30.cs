using System.Text;
using AMFTool;

namespace Restored.Namespace5;

internal class Class30
{
	private static string[] field_01;

	private static string[] field_02;

	private static string[] field_03;

	public static string method_01(string B4F8DA22, string F56DE2FF, string FF45B3B8)
	{
		string value = "<?xml version=\"1.0\" ?>";
		string value2 = "<patches>";
		string value3 = "</patches>";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(value);
		stringBuilder.AppendLine(value2);
		int num = 0;
		int num2 = field_02.Length - 1;
		if (0 > num2)
		{
			stringBuilder.AppendLine(value3);
			return stringBuilder.ToString();
		}
		UI.Progresbar1(num, field_02.Length - 1);
		stringBuilder.AppendLine(method_04(B4F8DA22, num.ToString(), field_02[num], F56DE2FF, FF45B3B8));
		num++;
		int num3 = 1330437441;
		while (true)
		{
			_ = num3 ^ -1162006303;
			if (num > num2)
			{
				break;
			}
			UI.Progresbar1(num, field_02.Length - 1);
			stringBuilder.AppendLine(method_04(B4F8DA22, num.ToString(), field_02[num], F56DE2FF, FF45B3B8));
			num++;
			num3 = 1330437441;
		}
		stringBuilder.AppendLine(value3);
		return stringBuilder.ToString();
	}

	public static string method_02(string D15F68DB, string C9FC3CD7, string D4113E52)
	{
		string value = "<?xml version=\"1.0\" ?>";
		string value2 = "<patches>";
		string value3 = "</patches>";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(value);
		stringBuilder.AppendLine(value2);
		int num = 0;
		int num2 = field_01.Length - 1;
		if (0 > num2)
		{
			stringBuilder.AppendLine(value3);
			return stringBuilder.ToString();
		}
		UI.Progresbar1(num, field_01.Length - 1);
		stringBuilder.AppendLine(method_04(D15F68DB, num.ToString(), field_01[num], C9FC3CD7, D4113E52));
		num++;
		int num3 = 1640841613;
		while (true)
		{
			_ = num3 ^ 0x6462C097;
			if (num > num2)
			{
				break;
			}
			UI.Progresbar1(num, field_01.Length - 1);
			stringBuilder.AppendLine(method_04(D15F68DB, num.ToString(), field_01[num], C9FC3CD7, D4113E52));
			num++;
			num3 = 1640841613;
		}
		stringBuilder.AppendLine(value3);
		return stringBuilder.ToString();
	}

	public static string method_03(string FA479CA6, string AD0C546, string DFECD79B)
	{
		string value = "<?xml version=\"1.0\" ?>";
		string value2 = "<patches>";
		string value3 = "</patches>";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(value);
		stringBuilder.AppendLine(value2);
		int num = 0;
		int num2 = field_03.Length - 1;
		bool flag = 0 <= num2;
		while (flag)
		{
			UI.Progresbar1(num, field_03.Length - 1);
			stringBuilder.AppendLine(method_04(FA479CA6, num.ToString(), field_03[num], AD0C546, DFECD79B));
			num++;
			flag = num <= num2;
		}
		stringBuilder.AppendLine(value3);
		return stringBuilder.ToString();
	}

	public static string method_04(string D75CB3CB, string BFC5BCF5, string E344BBB3, string E64DECE8, string DB635603)
	{
		string format = "<patch SECTOR_SIZE_IN_BYTES=\"{0}\" byte_offset=\"{1}\" filename=\"DISK\" physical_partition_number=\"{2}\" size_in_bytes=\"1\" start_sector=\"{3}\" value=\"{4}\" what=\"Zero.\" />";
		return string.Format(format, D75CB3CB, BFC5BCF5, DB635603, E64DECE8, E344BBB3);
	}

	static Class30()
	{
		string text = "98";
		string text2 = "111";
		string text3 = "116";
		string text4 = "45";
		string text5 = "114";
		string text6 = "101";
		string text7 = "99";
		string text8 = "118";
		string text9 = "121";
		string text10 = "0";
		string text11 = "10";
		string text12 = "117";
		string text13 = "112";
		string text14 = "100";
		string text15 = "97";
		string text16 = "95";
		string text17 = "107";
		string text18 = " 97";
		string text19 = "103";
		string text20 = "61";
		string text21 = "47";
		string text22 = "115";
		string text23 = "109";
		string text24 = "108";
		string text25 = "105";
		string text26 = "110";
		string text27 = "46";
		string text28 = "104";
		string text29 = "67";
		string text30 = "78";
		string text31 = "119";
		string text32 = "85";
		string text33 = "83";
		string text34 = " 0";
		string text35 = "0 ";
		field_01 = new string[159]
		{
			text, text2, text2, text3, text4, text5, text6, text7, text2, text8,
			text6, text5, text9, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text5, text6, text7, text2, text8, text6,
			text5, text9, text11, text4, text4, text12, text13, text14, text15, text3,
			text6, text16, text13, text15, text7, text17, text18, text19, text6, text20,
			text21, text22, text9, text22, text3, text6, text23, text21, text6, text3,
			text7, text21, text5, text6, text22, text3, text2, text5, text6, text16,
			text15, text24, text24, text16, text22, text6, text3, text3, text25, text26,
			text19, text22, text27, text22, text28, text21, text23, text2, text14, text6,
			text20, text5, text6, text22, text6, text3, text16, text22, text6, text3,
			text3, text25, text26, text19, text22, text11, text4, text4, text24, text2,
			text7, text15, text24, text6, text20, text29, text30, text11, text11
		};
		field_02 = new string[118]
		{
			text, text2, text2, text3, text4, text5, text6, text7, text2, text8,
			text6, text5, text9, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text10, text10, text10, text10, text10, text10,
			text10, text10, text10, text10, text5, text6, text7, text2, text8, text6,
			text5, text9, text11, text4, text4, text31, text25, text13, text6, text16,
			text14, text15, text3, text15, text11, text4, text4, text5, text6, text15,
			text22, text2, text26, text20, text15, text26, text14, text5, text2, text25,
			text14, text11, text4, text4, text24, text2, text7, text15, text24, text6,
			text20, text6, text26, text16, text32, text33, text11, text11
		};
		field_03 = new string[32]
		{
			text10, text10, text34, text10, text34, text35, text10, text10, text34, text10,
			text34, text10, text34, text10, text34, text10, text34, text10, text34, text10,
			text34, text10, text34, text10, text34, text10, text34, text10, text34, text10,
			text34, text10
		};
	}

	public static int method_05(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
