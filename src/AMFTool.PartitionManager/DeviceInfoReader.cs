using System;
using System.Collections.Generic;
using System.Text;
using AMFTool.Library;
using AMFTool.Library.Mediatek.Download_Agent;
using AMFTool.Library.Qualcomm;

namespace AMFTool.PartitionManager;

public class DeviceInfoReader
{
	public static string[] LogicalPartition;

	public static string[] Path;

	public static string[] ImeiPath;

	private static readonly HashSet<string> LogicalPartitionSet;

	public static string ReadFromSuper(long superSector, long superSectors, int sectorSize, int physicalNumber, string label, string sizeInKb, Firehose firehose)
	{
		int num = 0;
		object obj = null;
		int num2 = 1;
		using FileStreamsQualcomm fileStreamsQualcomm = new FileStreamsQualcomm(superSector, superSectors, sectorSize, physicalNumber, label, sizeInKb, firehose);
		List<LogicalPartitionSuper.LogicalPartition> list = LogicalPartitionSuper.ParseLogicalPartitions(fileStreamsQualcomm);
		for (int i = 0; i < list.Count; i += num2)
		{
			LogicalPartitionSuper.LogicalPartition logicalPartition = list[i];
			if ((LogicalPartitionSet.Contains(logicalPartition.Name) ? 1 : 0) == num)
			{
				continue;
			}
			using LogicalPartitionStream stream = new LogicalPartitionStream(fileStreamsQualcomm, logicalPartition);
			FileSystem fileSystem = FileSystemFactory.Open(stream);
			if (fileSystem != obj)
			{
				string text = ReadBuildProp(fileSystem);
				if ((string.IsNullOrEmpty(text) ? 1 : 0) == num)
				{
					return text;
				}
			}
		}
		return (string)obj;
	}

	public static string ReadFromNormalPartition(long sector, long sectors, int sectorSize, int physicalNumber, string label, string sizeInKb, Firehose firehose)
	{
		object obj = null;
		using FileStreamsQualcomm stream = new FileStreamsQualcomm(sector, sectors, sectorSize, physicalNumber, label, sizeInKb, firehose);
		FileSystem fileSystem = FileSystemFactory.Open(stream);
		if (fileSystem == obj)
		{
			return (string)obj;
		}
		return ReadBuildProp(fileSystem);
	}

	public static string ReadFromSuper(long superSector, long superSectors, int pageSize, MtkDaLoader daLoader)
	{
		int num = 0;
		object obj = null;
		int num2 = 1;
		using FileStreamsMediatek fileStreamsMediatek = new FileStreamsMediatek(superSector, superSectors, pageSize, daLoader);
		List<LogicalPartitionSuper.LogicalPartition> list = LogicalPartitionSuper.ParseLogicalPartitions(fileStreamsMediatek);
		for (int i = 0; i < list.Count; i += num2)
		{
			LogicalPartitionSuper.LogicalPartition logicalPartition = list[i];
			if ((LogicalPartitionSet.Contains(logicalPartition.Name) ? 1 : 0) == num)
			{
				continue;
			}
			using LogicalPartitionStream stream = new LogicalPartitionStream(fileStreamsMediatek, logicalPartition);
			FileSystem fileSystem = FileSystemFactory.Open(stream);
			if (fileSystem != obj)
			{
				string text = ReadBuildProp(fileSystem);
				if ((string.IsNullOrEmpty(text) ? 1 : 0) == num)
				{
					return text;
				}
			}
		}
		return (string)obj;
	}

	public static string ReadFromNormalPartition(long sector, long sectors, int pageSize, MtkDaLoader daLoader)
	{
		object obj = null;
		using FileStreamsMediatek stream = new FileStreamsMediatek(sector, sectors, pageSize, daLoader);
		FileSystem fileSystem = FileSystemFactory.Open(stream);
		if (fileSystem == obj)
		{
			return (string)obj;
		}
		return ReadBuildProp(fileSystem);
	}

	public static string[] ReadImeiFromNvdata(long sector, long sectors, int pageSize, MtkDaLoader daLoader)
	{
		object obj = null;
		using FileStreamsMediatek stream = new FileStreamsMediatek(sector, sectors, pageSize, daLoader);
		FileSystem fileSystem = FileSystemFactory.Open(stream);
		if (fileSystem != obj)
		{
			return ReadImei(fileSystem);
		}
		return (string[])obj;
	}

	private static string[] ReadImei(FileSystem fs)
	{
		object obj = null;
		int num = 0;
		string[] array2;
		while (true)
		{
			if (num >= ImeiPath.Length)
			{
				return (string[])obj;
			}
			string path = ImeiPath[num];
			if (fs.FileExists(path))
			{
				byte[] array = fs.ReadFile(path);
				if (array != null && array.Length != 0)
				{
					array2 = Tools.IMEIDecrypt(array);
					if (array2 != obj)
					{
						break;
					}
				}
			}
			num++;
		}
		return array2;
	}

	private static string ReadBuildProp(FileSystem fs)
	{
		object result = null;
		int num = 0;
		byte[] array;
		while (true)
		{
			if (num >= Path.Length)
			{
				return (string)result;
			}
			string path = Path[num];
			if (fs.FileExists(path))
			{
				array = fs.ReadFile(path);
				if (array != null && array.Length != 0)
				{
					break;
				}
			}
			num++;
		}
		return Encoding.UTF8.GetString(array).Trim(default(char));
	}

	static DeviceInfoReader()
	{
		string text = "product";
		string text2 = "system";
		string text3 = "system_a";
		string text4 = "oppo_product";
		string text5 = "system_ext_a";
		string text6 = "product_a";
		string text7 = "my_product";
		string text8 = "my_manifest";
		string text9 = "manifest";
		string text10 = "vendor_a";
		string text11 = "vendor";
		string text12 = "/system/build.prop";
		string text13 = "/build.prop";
		string text14 = "/system_ext/build.prop";
		string text15 = "/md/NVRAM/NVD_IMEI/LD0B_001";
		string text16 = "/nvdata/md/NVRAM/NVD_IMEI/LD0B_001";
		LogicalPartition = new string[11]
		{
			text, text2, text3, text4, text5, text6, text7, text8, text9, text10,
			text11
		};
		Path = new string[3] { text12, text13, text14 };
		ImeiPath = new string[2] { text15, text16 };
		LogicalPartitionSet = new HashSet<string>(LogicalPartition, StringComparer.OrdinalIgnoreCase);
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
