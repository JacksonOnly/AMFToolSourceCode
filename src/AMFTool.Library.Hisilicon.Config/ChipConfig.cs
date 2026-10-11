using System.Collections.Generic;

namespace AMFTool.Library.Hisilicon.Config;

public class ChipConfig
{
	public enum Loader
	{
		One,
		Two,
		tree,
		four,
		five,
		six,
		seven,
		eight,
		nine,
		then
	}

	public struct HisiLoader
	{
		public Loader type;

		public string path;

		public uint address;
	}

	public Dictionary<string, List<HisiLoader>> CPUList;

	public string LoaderTostring(Loader loader)
	{
		string text = "1";
		string text2 = "2";
		string text3 = "3";
		string text4 = "4";
		string text5 = "5";
		string text6 = "6";
		string text7 = "7";
		string text8 = "8";
		string text9 = "9";
		string text10 = "10";
		string text11 = "";
		return loader switch
		{
			Loader.One => text, 
			Loader.Two => text2, 
			Loader.tree => text3, 
			Loader.four => text4, 
			Loader.five => text5, 
			Loader.six => text6, 
			Loader.seven => text7, 
			Loader.eight => text8, 
			Loader.nine => text9, 
			Loader.then => text10, 
			_ => text11, 
		};
	}

	public ChipConfig()
	{
		string key = "65x_1";
		string path = "Hisilicon/001.img";
		string path2 = "Hisilicon/010.img";
		string key2 = "65x_2";
		string path3 = "Hisilicon/002.img";
		string path4 = "Hisilicon/020.img";
		string key3 = "620_1";
		string path5 = "Hisilicon/003.img";
		string path6 = "Hisilicon/030.img";
		string key4 = "620_2";
		string path7 = "Hisilicon/004.img";
		string path8 = "Hisilicon/040.img";
		string key5 = "925";
		string path9 = "Hisilicon/005.img";
		string path10 = "Hisilicon/050.img";
		string key6 = "935";
		string path11 = "Hisilicon/006.img";
		string path12 = "Hisilicon/060.img";
		string key7 = "950";
		string path13 = "Hisilicon/007.img";
		string path14 = "Hisilicon/070.img";
		string key8 = "960";
		string path15 = "Hisilicon/008.img";
		string path16 = "Hisilicon/080.img";
		string path17 = "Hisilicon/800.img";
		string key9 = "970";
		string path18 = "Hisilicon/011.img";
		string path19 = "Hisilicon/110.img";
		string path20 = "Hisilicon/101.img";
		string key10 = "710_emmc";
		string path21 = "Hisilicon/100.img";
		string path22 = "Hisilicon/700.img";
		string path23 = "Hisilicon/120.img";
		string path24 = "Hisilicon/012.img";
		string path25 = "Hisilicon/919.img";
		string path26 = "Hisilicon/033.img";
		string path27 = "Hisilicon/077.img";
		string key11 = "710_ufs";
		string key12 = "980";
		string path28 = "Hisilicon/2020.img";
		string path29 = "Hisilicon/0202.img";
		string path30 = "Hisilicon/3030.img";
		string path31 = "Hisilicon/4040.img";
		string path32 = "Hisilicon/0303.img";
		string path33 = "Hisilicon/0404.img";
		string path34 = "Hisilicon/0505.img";
		string path35 = "Hisilicon/5050.img";

		CPUList = new Dictionary<string, List<HisiLoader>>();
		CPUList.Add(key, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 131072u,
				path = path
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 268435456u,
				path = path2
			}
		});
		CPUList.Add(key2, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 131072u,
				path = path3
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 268435456u,
				path = path4
			}
		});
		CPUList.Add(key3, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 4185917440u,
				path = path5
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 109051904u,
				path = path6
			}
		});
		CPUList.Add(key4, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 4185917440u,
				path = path7
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 109051904u,
				path = path8
			}
		});
		CPUList.Add(key5, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 131072u,
				path = path9
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 268435456u,
				path = path10
			}
		});
		CPUList.Add(key6, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 131072u,
				path = path11
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 268435456u,
				path = path12
			}
		});
		CPUList.Add(key7, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 131072u,
				path = path13
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 268435456u,
				path = path14
			}
		});
		CPUList.Add(key8, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 131072u,
				path = path15
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 1787854848u,
				path = path16
			},
			new HisiLoader
			{
				type = Loader.tree,
				address = 448790528u,
				path = path17
			}
		});
		CPUList.Add(key9, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 139264u,
				path = path18
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 1610911744u,
				path = path19
			},
			new HisiLoader
			{
				type = Loader.tree,
				address = 377487360u,
				path = path20
			}
		});
		CPUList.Add(key10, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 139264u,
				path = path21
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 1610665984u,
				path = path22
			},
			new HisiLoader
			{
				type = Loader.tree,
				address = 139264u,
				path = path23
			},
			new HisiLoader
			{
				type = Loader.four,
				address = 1610665984u,
				path = path24
			},
			new HisiLoader
			{
				type = Loader.five,
				address = 147736u,
				path = path25
			},
			new HisiLoader
			{
				type = Loader.six,
				address = 478150656u,
				path = path26
			},
			new HisiLoader
			{
				type = Loader.seven,
				address = 469762048u,
				path = path27
			}
		});
		CPUList.Add(key11, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 139264u,
				path = path21
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 1610665984u,
				path = path22
			},
			new HisiLoader
			{
				type = Loader.tree,
				address = 139264u,
				path = path23
			},
			new HisiLoader
			{
				type = Loader.four,
				address = 1610665984u,
				path = path24
			},
			new HisiLoader
			{
				type = Loader.five,
				address = 147736u,
				path = path25
			},
			new HisiLoader
			{
				type = Loader.six,
				address = 478150656u,
				path = path26
			},
			new HisiLoader
			{
				type = Loader.seven,
				address = 469762048u,
				path = path27
			}
		});
		CPUList.Add(key12, new List<HisiLoader>
		{
			new HisiLoader
			{
				type = Loader.One,
				address = 139264u,
				path = path28
			},
			new HisiLoader
			{
				type = Loader.Two,
				address = 1610911744u,
				path = path29
			},
			new HisiLoader
			{
				type = Loader.tree,
				address = 440401920u,
				path = path30
			},
			new HisiLoader
			{
				type = Loader.four,
				address = 139264u,
				path = path31
			},
			new HisiLoader
			{
				type = Loader.five,
				address = 1610911744u,
				path = path32
			},
			new HisiLoader
			{
				type = Loader.six,
				address = 207702u,
				path = path33
			},
			new HisiLoader
			{
				type = Loader.seven,
				address = 436207616u,
				path = path34
			},
			new HisiLoader
			{
				type = Loader.eight,
				address = 440401920u,
				path = path35
			}
		});
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
