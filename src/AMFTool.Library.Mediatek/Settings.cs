using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace AMFTool.Library.Mediatek;

public class Settings
{
	private static string xmlPatch = string.Empty;

	public static string Platform { get; set; }

	public static string Storage { get; set; }

	public static bool IsSupport(string scatterFile)
	{
		string value = "<?xml version=\"1.0\" ?>\n";
		string value2 = "<data>\n";
		string oldValue = "- partition_index:";
		string newValue = "+ partition_index:";
		string value3 = "<?xml";
		string text = "platform";
		string text2 = "storage_type";
		string name = "name";
		string text3 = "partition_index";
		string text4 = "\t<program partition_index=\"";
		string oldValue2 = "SYS";
		string newValue2 = "";
		string text5 = "\" ";
		string text6 = "partition_name";
		string text7 = "partition_name=\"";
		string text8 = "file_name";
		string text9 = "file_name=\"";
		string oldValue3 = "NONE";
		string oldValue4 = "none";
		string text10 = "is_download";
		string text11 = "is_download=\"";
		string text12 = "linear_start_addr";
		string text13 = "linear_start_addr=\"";
		string text14 = "partition_size";
		string text15 = "partition_size=\"";
		string text16 = "is_upgradable";
		string text17 = "is_upgradable=\"";
		string text18 = "\"/>\n";
		string value4 = "platform:";
		string value5 = ":";
		string oldValue5 = " ";
		string value6 = "storage: EMMC";
		string value7 = "storage: UFS";
		string text19 = "\"/>";
		string value8 = "</data>";
		bool result = false;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(value);
		stringBuilder.Append(value2);
		string text20 = File.ReadAllText(scatterFile).Replace(oldValue, newValue);
		if (text20.Contains(value3))
		{
			using XmlReader xmlReader = XmlReader.Create(scatterFile);
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType != XmlNodeType.Element)
				{
					continue;
				}
				if (xmlReader.Name == text)
				{
					Platform = xmlReader.ReadInnerXml();
					result = true;
				}
				else if (xmlReader.Name == text2)
				{
					Storage = xmlReader.GetAttribute(name);
				}
				else if (xmlReader.Name == text3)
				{
					stringBuilder.Append(text4 + xmlReader.GetAttribute(name).Replace(oldValue2, newValue2) + text5);
				}
				else if (xmlReader.Name == text6)
				{
					stringBuilder.Append(text7 + xmlReader.ReadInnerXml() + text5);
				}
				else if (xmlReader.Name == text8)
				{
					stringBuilder.Append(text9 + xmlReader.ReadInnerXml().Replace(oldValue3, newValue2).Replace(oldValue4, newValue2) + text5);
				}
				else if (!(xmlReader.Name == text10))
				{
					if (xmlReader.Name == text12)
					{
						stringBuilder.Append(text13 + xmlReader.ReadInnerXml() + text5);
					}
					else if (xmlReader.Name == text14)
					{
						stringBuilder.Append(text15 + xmlReader.ReadInnerXml() + text5);
					}
					else if (xmlReader.Name == text16)
					{
						stringBuilder.Append(text17 + xmlReader.ReadInnerXml() + text18);
					}
				}
				else
				{
					stringBuilder.Append(text11 + xmlReader.ReadInnerXml() + text5);
				}
			}
		}
		else
		{
			using StringReader stringReader = new StringReader(File.ReadAllText(scatterFile));
			while (stringReader.Peek() != -1)
			{
				string text21 = stringReader.ReadLine();
				if (!text21.Contains(value4))
				{
					if (!text21.Contains(value6))
					{
						if (text21.Contains(value7))
						{
							Storage = text21.Substring(text21.IndexOf(value5)).Replace(oldValue5, newValue2).ToUpper();
						}
						else if (text21.Contains(text3))
						{
							stringBuilder.Append(text4 + text21.Substring(text21.IndexOf(value5) + 2).Replace(oldValue2, newValue2) + text5);
						}
						else if (text21.Contains(text6))
						{
							stringBuilder.Append(text7 + text21.Substring(text21.IndexOf(value5) + 2) + text5);
						}
						else if (text21.Contains(text8))
						{
							stringBuilder.Append(text9 + text21.Substring(text21.IndexOf(value5) + 2).Replace(oldValue3, newValue2).Replace(oldValue4, newValue2) + text5);
						}
						else if (text21.Contains(text10))
						{
							stringBuilder.Append(text11 + text21.Substring(text21.IndexOf(value5) + 2) + text5);
						}
						else if (text21.Contains(text16))
						{
							stringBuilder.Append(text17 + text21.Substring(text21.IndexOf(value5) + 2) + text5);
						}
						else if (text21.Contains(text12))
						{
							stringBuilder.Append(text13 + text21.Substring(text21.IndexOf(value5) + 2) + text5);
						}
						else if (text21.Contains(text14))
						{
							stringBuilder.Append(text15 + text21.Substring(text21.IndexOf(value5) + 2) + text19);
						}
					}
					else
					{
						Storage = text21.Substring(text21.IndexOf(value5)).Replace(oldValue5, newValue2).ToUpper();
					}
				}
				else
				{
					Platform = text21.Substring(text21.IndexOf(value5)).Replace(oldValue5, newValue2).ToUpper();
					result = true;
				}
			}
		}
		stringBuilder.Append(value8);
		xmlPatch = stringBuilder.ToString();
		return result;
	}

	public static List<MtkSettings> ScatterTable(string scatterFile)
	{
		string text = "program";
		string name = "partition_index";
		string name2 = "partition_name";
		string name3 = "file_name";
		string name4 = "is_download";
		string name5 = "is_upgradable";
		string name6 = "linear_start_addr";
		string name7 = "partition_size";
		if (IsSupport(scatterFile))
		{
			int num = 0;
			string text2 = string.Empty;
			List<MtkSettings> list = new List<MtkSettings>();
			using XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xmlPatch));
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType != XmlNodeType.Element || !(xmlTextReader.Name == text))
				{
					continue;
				}
				string attribute = xmlTextReader.GetAttribute(name);
				string attribute2 = xmlTextReader.GetAttribute(name2);
				string attribute3 = xmlTextReader.GetAttribute(name3);
				string attribute4 = xmlTextReader.GetAttribute(name4);
				string attribute5 = xmlTextReader.GetAttribute(name5);
				string attribute6 = xmlTextReader.GetAttribute(name6);
				string attribute7 = xmlTextReader.GetAttribute(name7);
				if (num == 0)
				{
					text2 = attribute;
					list.Add(new MtkSettings(attribute, attribute2, attribute3, attribute4, attribute5, attribute6, attribute7));
				}
				else
				{
					if (text2 == attribute)
					{
						break;
					}
					list.Add(new MtkSettings(attribute, attribute2, attribute3, attribute4, attribute5, attribute6, attribute7));
				}
				num++;
			}
			return list;
		}
		return new List<MtkSettings>();
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
