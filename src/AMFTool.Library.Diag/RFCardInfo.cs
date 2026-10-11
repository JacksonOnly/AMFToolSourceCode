using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AMFTool.Library.Diag;

[Serializable]
public class RFCardInfo
{
	private string strName;

	private string strRfid;

	private string strCa_enabled;

	private List<rfCardElementInfo> cardElemets;

	public string Name
	{
		get
		{
			return strName;
		}
		set
		{
			strName = value;
		}
	}

	public string RFID
	{
		get
		{
			return strRfid;
		}
		set
		{
			strRfid = value;
		}
	}

	public bool CA_Enabled
	{
		get
		{
			string objB = "true";
			if (!object.Equals(strCa_enabled, objB))
			{
				return false;
			}
			return true;
		}
		set
		{
			string text = "true";
			string text2 = "false";
			if (!value)
			{
				strCa_enabled = text2;
			}
			else
			{
				strCa_enabled = text;
			}
		}
	}

	public List<rfCardElementInfo> RFCardElements
	{
		get
		{
			return cardElemets;
		}
		set
		{
			cardElemets = value;
		}
	}

	public void SaveData(string fileName)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(RFCardInfo));
		TextWriter textWriter = new StreamWriter(fileName);
		xmlSerializer.Serialize(textWriter, this);
		textWriter.Close();
	}

	public RFCardInfo LoadData(string filename)
	{
		object obj = null;
		RFCardInfo result = new RFCardInfo();
		FileStream fileStream = (FileStream)obj;
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(RFCardInfo));
		fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
		if (fileStream ==null)
		{
			return result;
		}
		result = (RFCardInfo)xmlSerializer.Deserialize(fileStream);
		Name = result.Name;
		RFID = result.RFID;
		CA_Enabled = result.CA_Enabled;
		RFCardElements = result.RFCardElements;
		fileStream.Close();
		return result;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
