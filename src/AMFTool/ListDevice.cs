using System.Collections.Generic;
using Newtonsoft.Json;

namespace AMFTool;

public class ListDevice
{
	public class Info
	{
		public string Chipset { get; set; }

		public string Devices { get; set; }

		public Info(string Devices, string Chipset)
		{
			this.Chipset = Chipset;
			this.Devices = Devices;
		}

		public static int method_01(int arg_01, int arg_02)
		{
			return arg_01 ^ arg_02;
		}
	}

	public static List<Info> Devicelists = new List<Info>();

	public static List<Info> DataSource(string path)
	{
		Devicelists = JsonConvert.DeserializeObject<List<Info>>(path);
		List<Info> list = new List<Info>();
		list.Clear();
		foreach (Info devicelist in Devicelists)
		{
			list.Add(new Info(devicelist.Devices, devicelist.Chipset));
		}
		return list;
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
