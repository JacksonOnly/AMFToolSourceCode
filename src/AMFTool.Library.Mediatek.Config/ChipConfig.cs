using System.Collections.Generic;

namespace AMFTool.Library.Mediatek.Config;

public class ChipConfig
{
	public object Var1;

	public object Watchdog;

	public object Uart;

	public object BromPayloadAddr;

	public object DaPayloadAddr;

	public object PlPayloadAddr;

	public object CqdmaBase;

	public object SejBase;

	public object DxccBase;

	public object GcpuBase;

	public object ApDmaMem;

	public string Name;

	public string Description;

	public object Dacode;

	public object MeidAddr;

	public List<uint[]> SendPtr;

	public object SocidAddr;

	public object Blacklist;

	public object BlacklistCount;

	public object CtrlBuffer;

	public object CmdHandler;

	public List<uint[]> BromRegisterAccess;

	public DAmodes Damode;

	public object Loader;

	public object ProvAddr;

	public int? EfuseAddr;

	public int HwVer;

	public int HwCode;

	public object HwSubCode;

	public int SwVer;

	public bool? Has64Bit;

	public object MiscLock;

	public static Dictionary<int, ChipConfig> HwConfig;

	public ChipConfig(object Var1 = null, object Watchdog = null, object Uart = null, object BromPayloadAddr = null, object DaPayloadAddr = null, object PlPayloadAddr = null, object CqdmaBase = null, object SejBase = null, object DxccBase = null, object GcpuBase = null, object ApDmaMem = null, string Name = "", string Description = "", object Dacode = null, object MeidAddr = null, object SocidAddr = null, object Blacklist = null, object BlacklistCount = null, List<uint[]> SendPtr = null, object CtrlBuffer = null, object CmdHandler = null, List<uint[]> BromRegisterAccess = null, DAmodes Damode = DAmodes.None, object Loader = null, object ProvAddr = null, object MiscLock = null, int? EfuseAddr = null, bool? Has64Bit = false)
	{
		object miscLock = (HwSubCode = null);
		this.Has64Bit = false;
		this.MiscLock = miscLock;

		this.Var1 = Var1;
		this.Watchdog = Watchdog;
		this.Uart = Uart;
		this.BromPayloadAddr = BromPayloadAddr;
		this.DaPayloadAddr = DaPayloadAddr;
		this.PlPayloadAddr = PlPayloadAddr;
		this.CqdmaBase = CqdmaBase;
		this.ApDmaMem = ApDmaMem;
		this.SejBase = SejBase;
		this.DxccBase = DxccBase;
		this.Name = Name;
		this.Description = Description;
		this.Dacode = Dacode;
		this.BlacklistCount = BlacklistCount;
		this.SendPtr = SendPtr;
		this.CtrlBuffer = CtrlBuffer;
		this.CmdHandler = CmdHandler;
		this.BromRegisterAccess = BromRegisterAccess;
		this.MeidAddr = MeidAddr;
		this.Blacklist = Blacklist;
		this.SocidAddr = SocidAddr;
		this.ProvAddr = ProvAddr;
		this.GcpuBase = GcpuBase;
		this.Dacode = Dacode;
		this.EfuseAddr = EfuseAddr;
		this.Loader = Loader;
		this.Damode = Damode;
		HwVer = 0;
		HwCode = 0;
		SwVer = 0;
		this.Has64Bit = Has64Bit;
		this.MiscLock = MiscLock;
	}

	static ChipConfig()
	{
		object obj = null;
		string arg_ = "MT0571";
		string arg_2 = "";
		string arg_3 = "ELBRUS/MT0598";
		string arg_4 = "MT6880/MT6890";
		string arg_5 = "MT2601";
		string arg_6 = "mt2601_payload";
		string arg_7 = "MT2523";
		string arg_8 = "MT2625";
		string arg_9 = "MT3967";
		string arg_10 = "MT5932";
		string arg_11 = "MT7682";
		string arg_12 = "MT7686";
		string arg_13 = "MT6225";
		string arg_14 = "MT6226";
		string arg_15 = "MT6236";
		string arg_16 = "MT6238";
		string arg_17 = "MT6253";
		string arg_18 = "MT6256";
		string arg_19 = "MT625a";
		string arg_20 = "MT6255";
		string arg_21 = "MT6261";
		string arg_22 = "mt6261_payload";
		string arg_23 = "MT6268";
		string arg_24 = "MT6270";
		string arg_25 = "MT6276";
		string arg_26 = "MT6280";
		string arg_27 = "MT6291";
		string arg_28 = "MT6516";
		string arg_29 = "MT6570/MT8321";
		string arg_30 = "MT6571";
		string arg_31 = "MT6572";
		string arg_32 = "mt6572_payload";
		string arg_33 = "MT6573/MT6260";
		string arg_34 = "MT6575/MT6577/MT8317";
		string arg_35 = "MT6577";
		string arg_36 = "MT6580";
		string arg_37 = "mt6580_payload";
		string arg_38 = "MT6582/MT6574/MT8382";
		string arg_39 = "mt6582_payload";
		string arg_40 = "MT6583/6589";
		string arg_41 = "MT6592/MT8392";
		string arg_42 = "mt6592_payload";
		string arg_43 = "MT6595";
		string arg_44 = "mt6595_payload";
		string arg_45 = "MT6735/T,MT8735A";
		string arg_46 = "mt6735_payload";
		string arg_47 = "MT6737M/MT6735G";
		string arg_48 = "mt6737_payload";
		string arg_49 = "MT6739/MT6731/MT8765";
		string arg_50 = "mt6739_payload";
		string arg_51 = "MT6750";
		string arg_52 = "MT6752";
		string arg_53 = "mt6752_payload";
		string arg_54 = "MT6753";
		string arg_55 = "mt6753_payload";
		string arg_56 = "MT6755/MT6750/M/T/S";
		string arg_57 = "Helio P10/P15/P18";
		string arg_58 = "mt6755_payload";
		string arg_59 = "MT6757/MT6757D";
		string arg_60 = "Helio P20";
		string arg_61 = "mt6757_payload";
		string arg_62 = "MT6758";
		string arg_63 = "Helio P30";
		string arg_64 = "mt6758_payload";
		string arg_65 = "MT6759";
		string arg_66 = "MT6761/MT6762/MT3369/MT8766B";
		string arg_67 = "Helio A20/P22/A22/A25/G25";
		string arg_68 = "mt6761_payload";
		string arg_69 = "MT6763";
		string arg_70 = "Helio P23";
		string arg_71 = "mt6763_payload";
		string arg_72 = "MT6765/MT8768t";
		string arg_73 = "Helio P35/G35";
		string arg_74 = "mt6765_payload";
		string arg_75 = "MT6768/MT6769";
		string arg_76 = "Helio P65/G85 k68v1";
		string arg_77 = "mt6768_payload";
		string arg_78 = "MT6771/MT8385/MT8183/MT8666";
		string arg_79 = "Helio P60/P70/G80";
		string arg_80 = "mt6771_payload";
		string arg_81 = "MT6779";
		string arg_82 = "Helio P90 k79v1";
		string arg_83 = "mt6779_payload";
		string arg_84 = "MT6781";
		string arg_85 = "Helio G96";
		string arg_86 = "mt6781_payload";
		string arg_87 = "MT6785";
		string arg_88 = "Helio G90";
		string arg_89 = "mt6785_payload";
		string arg_90 = "MT6795";
		string arg_91 = "Helio X10";
		string arg_92 = "mt6795_payload";
		string arg_93 = "MT6797/MT6767";
		string arg_94 = "Helio X23/X25/X27";
		string arg_95 = "mt6797_payload";
		string arg_96 = "MT6799";
		string arg_97 = "Helio X30/X35";
		string arg_98 = "mt6799_payload";
		string arg_99 = "MT6833";
		string arg_100 = "Dimensity 700 5G k6833";
		string arg_101 = "mt6833_payload";
		string arg_102 = "MT6853";
		string arg_103 = "Dimensity 720 5G";
		string arg_104 = "mt6853_payload";
		string arg_105 = "MT6873";
		string arg_106 = "Dimensity 800/820 5G";
		string arg_107 = "mt6873_payload";
		string arg_108 = "MT6877/MT6877V";
		string arg_109 = "Dimensity 900/1080";
		string arg_110 = "mt6877_payload";
		string arg_111 = "MT6885/MT6883/MT6889/MT6880/MT6890";
		string arg_112 = "Dimensity 1000L/1000";
		string arg_113 = "mt6885_payload";
		string arg_114 = "MT6893";
		string arg_115 = "Dimensity 1200";
		string arg_116 = "mt6893_payload";
		string arg_117 = "MT6983";
		string arg_118 = "Dimensity 9000/9000+";
		string arg_119 = "MT6855";
		string arg_120 = "Dimensity 8100";
		string arg_121 = "MT6895";
		string arg_122 = "Dimensity 8200";
		string arg_123 = "MT6897";
		string arg_124 = "Dimensity 8300 Ultra";
		string arg_125 = "MT6899";
		string arg_126 = "Dimensity 8400 Ultra";
		string arg_127 = "MT6789/MT8781V";
		string arg_128 = "MTK Helio G99";
		string arg_129 = "MT6835";
		string arg_130 = "Dimensity 6100+";
		string arg_131 = "MT6886";
		string arg_132 = "Dimensity 7200 Ultra";
		string arg_133 = "MT6989";
		string arg_134 = "Dimensity 9300+";
		string arg_135 = "MT6985";
		string arg_136 = "Dimensity 9200/9200+";
		string arg_137 = "MT6878";
		string arg_138 = "Dimensity 7300";
		string arg_139 = "MT6993";
		string arg_140 = "Dimensity 9500";
		string arg_141 = "MT8127/MT3367";
		string arg_142 = "mt8127_payload";
		string arg_143 = "MT8135";
		string arg_144 = "MT8163";
		string arg_145 = "mt8163_payload";
		string arg_146 = "MT8167/MT8516/MT8362";
		string arg_147 = "mt8167_payload";
		string arg_148 = "MT8168/MT6357";
		string arg_149 = "mt8168_payload";
		string arg_150 = "MT8173";
		string arg_151 = "mt8173_payload";
		string arg_152 = "MT8176";
		string arg_153 = "mt8176_payload";
		string arg_154 = "MT8195 Chromebook";
		string arg_155 = "MT8512";
		string arg_156 = "mt8512_payload";
		string arg_157 = "MT8518 VoiceAssistant";
		string arg_158 = "MT8590/MT7683/MT8521/MT7623";
		string arg_159 = "mt8590_payload";
		string arg_160 = "MT8695";
		string arg_161 = "mt8695_payload";
		string arg_162 = "MT8696";
		HwConfig = new Dictionary<int, ChipConfig>
		{
			{
				1393,
				(ChipConfig)method_02(obj, 268464128, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_, arg_2, 1393, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				1432,
				(ChipConfig)method_02(obj, 270602240, 285343744, 1051136, 2101248, obj, 270609408, 268476416, obj, 270680064, 285213088, arg_3, arg_2, 1432, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				2450,
				(ChipConfig)method_02(obj, 268464128, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_4, arg_2, 2450, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, DAmodes.XFLASH, obj, obj, obj, 300679168, false)
			},
			{
				9729,
				(ChipConfig)method_02(10, 268464128, 285233152, 1051136, 33587200, 2178940928u, obj, 268476416, obj, obj, obj, arg_5, arg_2, 9729, 286534708, obj, new List<(uint, int)>
				{
					(286531340u, 0),
					(286542788u, 0)
				}, 8, new List<uint[]> { new uint[2] { 286531404u, 47720u } }, 286534624, 4244911, new List<uint[]> { new uint[2] { 4242760u, 4243196u } }, (DAmodes)3, arg_6, obj, obj, null, false)
			},
			{
				9507,
				(ChipConfig)method_02(10, 268464128, 285233152, 1051136, 33587200, 2178940928u, obj, 268476416, obj, obj, obj, arg_7, arg_2, 9507, 286534708, obj, obj, obj, new List<uint[]> { new uint[2] { 286531404u, 47720u } }, 286534624, 4244911, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				9765,
				(ChipConfig)method_02(10, 268464128, 285233152, 1051136, 67112960, obj, obj, 268476416, obj, obj, obj, arg_8, arg_2, 9765, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				14695,
				(ChipConfig)method_02(obj, obj, obj, 1051136, 2101248, 1073872896, obj, obj, obj, obj, obj, arg_9, arg_2, 14695, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				22834,
				(ChipConfig)method_02(obj, obj, obj, 1051136, 2101248, 1073872896, obj, obj, obj, obj, obj, arg_10, arg_2, 22834, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				30338,
				(ChipConfig)method_02(obj, obj, obj, 1051136, 2101248, 1073872896, obj, obj, obj, obj, obj, arg_11, arg_2, 30338, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				30342,
				(ChipConfig)method_02(obj, obj, obj, 1051136, 2101248, 1073872896, obj, obj, obj, obj, obj, arg_12, arg_2, 30342, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25125,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, 2148794368u, obj, obj, obj, arg_13, arg_2, 25125, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25126,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, 2148794368u, obj, obj, obj, arg_14, arg_2, 25126, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25142,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_15, arg_2, 25142, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25144,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_16, arg_2, 25144, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25171,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_17, arg_2, 25171, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25174,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_18, arg_2, 25174, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25178,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_19, arg_2, 25178, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25173,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, 2148794368u, obj, obj, obj, arg_20, arg_2, obj, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25185,
				(ChipConfig)method_02(40, 2684551168u, 2684878848u, 1051136, 2101248, obj, obj, 2685468672u, obj, obj, obj, arg_21, arg_2, 25185, obj, obj, new List<(uint, int)> { (3758357635u, 0) }, obj, new List<uint[]> { new uint[2] { 1879065776u, 1879070956u } }, 1879065000, 1879073270, (List<uint[]>)obj, (DAmodes)3, arg_22, obj, obj, null, false)
			},
			{
				25192,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_23, arg_2, 25192, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25200,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_24, arg_2, 25200, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25206,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_25, arg_2, 25206, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25216,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, 2148007936u, obj, obj, obj, arg_26, arg_2, obj, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25233,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_27, arg_2, 25233, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25878,
				(ChipConfig)method_02(obj, 268447744, 268578816, obj, 2101248, obj, obj, 268619776, obj, obj, obj, arg_28, arg_2, 25878, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				1587,
				(ChipConfig)method_02(obj, 268464128, 285220864, 1051136, 2101248, 2147487744u, 270576640, 268476416, obj, 270585856, 285213088, arg_29, arg_2, 25968, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, DAmodes.XFLASH, obj, obj, obj, 268472320, false)
			},
			{
				25969,
				(ChipConfig)method_02(obj, 268465152, obj, obj, 33591296, 2147487744u, obj, obj, obj, obj, obj, arg_30, arg_2, 25969, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, 268440604, null, false)
			},
			{
				25970,
				(ChipConfig)method_02(10, 268464128, 285233152, 16791200, 33587200, 2178940928u, obj, obj, obj, obj, 285213084, arg_31, arg_2, 25970, 286534708, obj, new List<(uint, int)>
				{
					(286531340u, 0),
					(286542788u, 0)
				}, 8, new List<uint[]> { new uint[2] { 286531404u, 4242024u } }, 286534624, 4244911, new List<uint[]> { new uint[2] { 4242760u, 4243196u } }, (DAmodes)3, arg_32, obj, 268440604, 268472320, false)
			},
			{
				25971,
				(ChipConfig)method_02(obj, 1879199744, obj, obj, 2415943680u, 4043440128u, obj, 1879220224, obj, obj, obj, arg_33, arg_2, 25971, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25973,
				(ChipConfig)method_02(obj, 3221225472u, 3238039552u, obj, 3254784000u, 3255140352u, obj, 3238109184u, obj, obj, 3238007196u, arg_34, arg_2, 25973, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25975,
				(ChipConfig)method_02(obj, 3221225472u, 3238039552u, obj, 3254784000u, 3255140352u, obj, 3238109184u, obj, obj, 3238007196u, arg_35, arg_2, 25975, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				25984,
				(ChipConfig)method_02(172, 268464128, 285233152, 1051136, 2101248, 2147487744u, 270576640, 268476416, obj, obj, 285213088, arg_36, arg_2, 25984, 1061044, obj, new List<(uint, int)>
				{
					(1058660u, 0),
					(1077716u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058724u, 46604u } }, 1060960, 49427, new List<uint[]> { new uint[2] { 47328u, 47764u } }, (DAmodes)3, arg_37, obj, 268441656, 270557184, false)
			},
			{
				25986,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 2147487744u, obj, 268476416, obj, 285323264, 285213472, arg_38, arg_2, 25986, 1061068, obj, new List<(uint, int)>
				{
					(1058696u, 0),
					(1072100u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058760u, 42492u } }, 1060984, 45799, new List<uint[]> { new uint[2] { 43216u, 43652u } }, (DAmodes)3, arg_39, obj, 268443728, 270557184, false)
			},
			{
				25987,
				(ChipConfig)method_02(obj, 268435456, 285237248, 1051136, 301993984, 2147487744u, 270606336, 268476416, obj, 270598144, 285213472, arg_40, arg_2, 25993, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, 268443728, null, false)
			},
			{
				26002,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 1118208, 2147487744u, 270606336, 268476416, obj, 270598144, 285213472, arg_41, arg_2, 26002, 1061032, obj, new List<(uint, int)>
				{
					(1058660u, 0),
					(1072112u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058724u, 42340u } }, 1060948, 45215, new List<uint[]> { new uint[2] { 43064u, 43500u } }, (DAmodes)3, arg_42, obj, 268443728, 270557184, false)
			},
			{
				26005,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 1118208, obj, obj, 268476416, obj, obj, 285213088, arg_43, arg_2, 26005, 1061028, obj, new List<(uint, int)>
				{
					(1058664u, 0),
					(1076360u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058728u, 45592u } }, 1060944, 48467, new List<uint[]> { new uint[2] { 46316u, 46752u } }, (DAmodes)3, arg_44, obj, obj, 270557184, false)
			},
			{
				801,
				(ChipConfig)method_02(40, 270606336, 285220864, 1051136, 2101248, 1075838976, 270629888, 268468224, obj, 270622720, 285213088, arg_45, arg_2, 26421, 1061040, obj, new List<(uint, int)>
				{
					(1058656u, 0),
					(1070852u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058720u, 38392u } }, 1060956, 41343, new List<uint[]> { new uint[2] { 39116u, 39572u } }, (DAmodes)3, arg_46, obj, 268441656, 298123264, false)
			},
			{
				821,
				(ChipConfig)method_02(40, 270606336, 285220864, 1051136, 2101248, 1075838976, 270629888, 268468224, obj, 270622720, 285213088, arg_47, arg_2, 26421, 1061040, obj, new List<(uint, int)>
				{
					(1058656u, 0),
					(1070852u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058720u, 38408u } }, 1060956, 41359, new List<uint[]> { new uint[2] { 39132u, 39588u } }, (DAmodes)3, arg_48, obj, obj, 270557184, false)
			},
			{
				1689,
				(ChipConfig)method_02(180, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285213088, arg_49, arg_2, 26425, 1059576, 1059592, new List<(uint, int)>
				{
					(1058860u, 0),
					(1078956u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058928u, 57116u } }, 1059368, 60489, new List<uint[]> { new uint[2] { 58160u, 58344u } }, DAmodes.XFLASH, arg_50, 1077772, 268542208, 297795584, false)
			},
			{
				1537,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270609408, 268476416, obj, 270598144, 285213088, arg_51, arg_2, 26453, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, DAmodes.XFLASH, obj, obj, 268441656, 270557184, false)
			},
			{
				26450,
				(ChipConfig)method_02(40, 268464128, 285220864, 1051136, 2101248, 1073745920, 270609408, 268476416, obj, 270598144, 285213088, arg_52, arg_2, 26450, 1061044, obj, new List<(uint, int)>
				{
					(1058660u, 0),
					(1070852u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058724u, 39180u } }, 1060960, 42131, new List<uint[]> { new uint[2] { 39904u, 40360u } }, (DAmodes)3, arg_53, obj, obj, 270557184, false)
			},
			{
				823,
				(ChipConfig)method_02(40, 270606336, 285220864, 1051136, 2101248, 1075838976, 270629888, 268468224, obj, 270622720, 285213088, arg_54, arg_2, 26421, 1061040, obj, new List<(uint, int)>
				{
					(1058656u, 0),
					(1070852u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058720u, 38504u } }, 1060956, 41455, new List<uint[]> { new uint[2] { 39228u, 39684u } }, (DAmodes)3, arg_55, obj, 268441656, null, false)
			},
			{
				806,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270609408, 268476416, obj, 270598144, 285213088, arg_56, arg_57, 26453, 1061036, obj, new List<(uint, int)>
				{
					(1058668u, 0),
					(1070852u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058736u, 39532u } }, 1060952, 42495, new List<uint[]> { new uint[2] { 40268u, 40724u } }, DAmodes.XFLASH, arg_58, obj, 268441656, 270557184, false)
			},
			{
				1361,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270609408, 268476416, obj, 270598144, 285213088, arg_59, arg_60, 26455, 1061044, obj, new List<(uint, int)>
				{
					(1058676u, 0),
					(1070852u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058744u, 39980u } }, 1060960, 43259, new List<uint[]> { new uint[2] { 41008u, 41192u } }, DAmodes.XFLASH, arg_61, obj, 268441656, 270557184, false)
			},
			{
				1672,
				(ChipConfig)method_02(10, 270602240, 285343744, 1051136, 2101248, 1075838976, 270532608, 268959744, 287571968, 268763136, 285213088, arg_62, arg_63, 26456, 1059832, 1059848, new List<(uint, int)>
				{
					(1058864u, 0),
					(1075808u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058932u, 55392u } }, 1059624, 58765, new List<uint[]> { new uint[2] { 56436u, 56620u } }, DAmodes.XFLASH, arg_64, obj, obj, 272957440, false)
			},
			{
				1287,
				(ChipConfig)method_02(obj, 270598144, 285343744, 1051136, 2101248, obj, obj, obj, obj, 270598144, 16974240, arg_65, arg_63, 26456, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				1815,
				(ChipConfig)method_02(37, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285215776, arg_66, arg_67, 26465, 1059576, 1059592, new List<(uint, int)>
				{
					(1058856u, 0),
					(1071508u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058924u, 48268u } }, 1059368, 51641, new List<uint[]> { new uint[2] { 49312u, 49496u } }, DAmodes.XFLASH, arg_68, 1070324, 268542208, 298123264, false)
			},
			{
				1680,
				(ChipConfig)method_02(127, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285215776, arg_69, arg_70, 26467, 1059704, 1059720, new List<(uint, int)>
				{
					(1058868u, 0),
					(1076388u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058936u, 54892u } }, 1059472, 58243, new List<uint[]> { new uint[2] { 55936u, 56120u } }, DAmodes.XFLASH, arg_71, 1075204, 268542208, 301006848, false)
			},
			{
				1894,
				(ChipConfig)method_02(37, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285213088, arg_72, arg_73, 26469, 1059576, 1059592, new List<(uint, int)>
				{
					(1058856u, 0),
					(1071508u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058924u, 48576u } }, 1059368, 51949, new List<uint[]> { new uint[2] { 49620u, 49804u } }, DAmodes.XFLASH, arg_74, 1070324, 268542208, 298123264, false)
			},
			{
				1799,
				(ChipConfig)method_02(37, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285213088, arg_75, arg_76, 26472, 1059576, 1059592, new List<(uint, int)>
				{
					(1058860u, 0),
					(1071508u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058924u, 49552u } }, 1059368, 53013, new List<uint[]> { new uint[2] { 50584u, 50768u } }, DAmodes.XFLASH, arg_77, 1070324, 268542208, 298713088, false)
			},
			{
				1928,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285213016, arg_78, arg_79, 26481, 1059640, 1059656, new List<(uint, int)>
				{
					(1058868u, 0),
					(1075808u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058936u, 57020u } }, 1059456, 60393, new List<uint[]> { new uint[2] { 58064u, 58248u } }, DAmodes.XFLASH, arg_80, 1074624, 268542208, 301006848, false)
			},
			{
				1829,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285213016, arg_81, arg_82, 26489, 1059640, 1059656, new List<(uint, int)>
				{
					(1058872u, 0),
					(1075808u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058936u, 57420u } }, 1059456, 60781, new List<uint[]> { new uint[2] { 58452u, 58636u } }, DAmodes.XFLASH, arg_83, 1074624, 268542208, 297861120, false)
			},
			{
				4198,
				(ChipConfig)method_02(115, 268464128, 285220864, 1051136, 2101248, 1075838976, obj, 268476416, 270598144, 268763136, obj, arg_84, arg_85, 26497, 1059736, 1059752, new List<(uint, int)> { (1058892u, 1076052) }, 10, new List<uint[]> { new uint[2] { 1058960u, 58840u } }, 1059508, 62401, new List<uint[]> { new uint[2] { 59868u, 60052u } }, DAmodes.XFLASH, arg_86, obj, obj, 298516480, false)
			},
			{
				2067,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285213016, arg_87, arg_88, 26501, 1059640, 1059656, new List<(uint, int)>
				{
					(1058872u, 0),
					(1075808u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058936u, 58020u } }, 1059456, 61481, new List<uint[]> { new uint[2] { 59052u, 59236u } }, DAmodes.XFLASH, arg_89, 1074624, 268542208, 297861120, false)
			},
			{
				26517,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 1114112, 1075838976, 270609408, 268476416, obj, 270598144, 285213088, arg_90, arg_91, 26517, 1061024, obj, new List<(uint, int)>
				{
					(1058660u, 0),
					(1070852u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058724u, 38796u } }, 1060940, 41747, new List<uint[]> { new uint[2] { 39520u, 39976u } }, (DAmodes)3, arg_92, obj, obj, 270557184, false)
			},
			{
				633,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270609408, 268476416, obj, 270598144, 285213088, arg_93, arg_94, 26519, 1061036, obj, new List<(uint, int)>
				{
					(1058668u, 0),
					(1070852u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058736u, 40620u } }, 1060952, 43583, new List<uint[]> { new uint[2] { 41356u, 41812u } }, DAmodes.XFLASH, arg_95, obj, 268443728, 270557184, false)
			},
			{
				1378,
				(ChipConfig)method_02(10, 270602240, 285343744, 1051136, 2101248, 1075838976, 296943616, 268476416, 296878080, 270598144, 285213088, arg_96, arg_97, 26521, 1061816, 1061832, new List<(uint, int)>
				{
					(1058928u, 0),
					(1077360u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058996u, 62892u } }, 1061616, 66243, new List<uint[]> { new uint[2] { 63936u, 64120u } }, DAmodes.XFLASH, arg_98, obj, obj, 301006848, false)
			},
			{
				2441,
				(ChipConfig)method_02(115, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 270629920, arg_99, arg_100, 26675, 1059736, 1059752, new List<(uint, int)>
				{
					(1058884u, 0),
					(1076052u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058948u, 57312u } }, 1059492, 60845, new List<uint[]> { new uint[2] { 58344u, 58528u } }, DAmodes.XFLASH, arg_101, 1074868, obj, 297861120, false)
			},
			{
				2454,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 270629920, arg_102, arg_103, 26707, 1059704, 1059720, new List<(uint, int)>
				{
					(1058892u, 0),
					(1076064u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058956u, 60004u } }, 1059488, 63537, new List<uint[]> { new uint[2] { 61036u, 61220u } }, DAmodes.XFLASH, arg_104, 1074880, 268542208, 297861120, false)
			},
			{
				2182,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 270629920, arg_105, arg_106, 26739, 1059704, 1059720, new List<(uint, int)>
				{
					(1058892u, 0),
					(1076064u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058956u, 60024u } }, 1059488, 63485, new List<uint[]> { new uint[2] { 61056u, 61240u } }, DAmodes.XFLASH, arg_107, 1074880, 268542208, 297861120, false)
			},
			{
				2393,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 270629920, arg_108, arg_109, 26743, 1059736, 1059752, new List<(uint, int)>
				{
					(1058888u, 0),
					(1076064u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058952u, 59600u } }, 1059484, 63133, new List<uint[]> { new uint[2] { 60632u, 60816u } }, DAmodes.XFLASH, arg_110, 1074880, obj, 301006848, false)
			},
			{
				2070,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285215776, arg_111, arg_112, 26757, 1059704, 1059720, new List<(uint, int)>
				{
					(1058888u, 0),
					(1076064u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058952u, 59132u } }, 1059484, 62593, new List<uint[]> { new uint[2] { 60164u, 60348u } }, DAmodes.XFLASH, arg_113, 1074880, 268542208, 297861120, false)
			},
			{
				2384,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 285215776, arg_114, arg_115, 26771, 1059736, 1059752, new List<(uint, int)>
				{
					(1058888u, 0),
					(1076064u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1058952u, 59292u } }, 1059484, 62825, new List<uint[]> { new uint[2] { 60324u, 60508u } }, DAmodes.XFLASH, arg_116, 1074880, obj, 297861120, false)
			},
			{
				2311,
				(ChipConfig)method_02(10, 469790720, 285216768, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 288360864, arg_117, arg_118, 2311, 1050860, 1050932, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, 300810240, true)
			},
			{
				4393,
				(ChipConfig)method_02(10, 469790720, 285216768, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 288360864, arg_119, arg_120, 4393, 1050860, 1050932, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, null, false)
			},
			{
				4466,
				(ChipConfig)method_02(10, 469790720, 285216768, 1051136, 2101248, 1075838976, 270606336, 268476416, 270598144, 268763136, 288360864, arg_121, arg_122, 4466, 1050860, 1050932, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, 301006848, false)
			},
			{
				4611,
				(ChipConfig)method_02(10, 469790720, 285220864, 1051136, 2101248, 1075838976, obj, 272687104, 272642048, 268488704, obj, arg_123, arg_124, 4611, obj, 34500752, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, null, false)
			},
			{
				4612,
				(ChipConfig)method_02(10, 469790720, 285220864, 1051136, 2101248, 1075838976, obj, 272687104, 272642048, 268488704, obj, arg_125, arg_126, 4612, obj, 34500752, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, null, false)
			},
			{
				4616,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1075838976, obj, 268476416, 270598144, obj, obj, arg_127, arg_128, 4616, 1050860, 1050932, new List<(uint, int)> { (1060188u, 0) }, obj, (List<uint[]>)obj, 1060900, 66024, new List<uint[]> { new uint[2] { 63898u, 64012u } }, (DAmodes)6, obj, obj, obj, 297861120, false)
			},
			{
				4617,
				(ChipConfig)method_02(10, 469790720, 285220864, 1051136, 2101248, 1075838976, obj, obj, obj, obj, obj, arg_129, arg_130, 4617, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, null, false)
			},
			{
				4649,
				(ChipConfig)method_02(10, 469790720, 285220864, 1051136, 33558528, 1075838976, obj, 469798912, 478179328, 268488704, obj, arg_131, arg_132, 4649, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, 300089344, true)
			},
			{
				4662,
				(ChipConfig)method_02(10, 469790720, 469831680, 1051136, 2101248, 1075838976, obj, 469798912, 478179328, obj, obj, arg_133, arg_134, 4662, 1050860, 1050932, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, 300417024, true)
			},
			{
				4758,
				(ChipConfig)method_02(10, 469790720, 469831680, 1051136, 2101248, 1075838976, obj, 469798912, 478179328, obj, obj, arg_135, arg_136, 4758, 1050860, 1050932, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, 300417024, true)
			},
			{
				4981,
				(ChipConfig)method_02(10, 469803008, obj, 1051136, 33619968, 1075838976, obj, 272687104, 272629760, obj, obj, arg_137, arg_138, 4981, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, 301006848, true)
			},
			{
				5233,
				(ChipConfig)method_02(10, 469827584, 369164288, 1051136, 2101248, 1075838976, obj, 402710528, 402673664, obj, obj, arg_139, arg_140, 5233, 1050860, 1050932, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)6, obj, obj, obj, 269877248, true)
			},
			{
				33063,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 2147487744u, obj, 268476416, obj, 285278208, 285213088, arg_141, arg_2, 33063, 1061324, obj, new List<(uint, int)>
				{
					(1058928u, 0),
					(1076348u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058992u, 45752u } }, 1061240, 48627, new List<uint[]> { new uint[2] { 46476u, 46912u } }, (DAmodes)3, arg_142, obj, 268443728, null, false)
			},
			{
				33077,
				(ChipConfig)method_02(obj, 268435456, 285220864, obj, 301993984, 2147487744u, obj, obj, obj, 285310976, obj, arg_143, arg_2, 33077, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, (DAmodes)3, obj, obj, obj, null, false)
			},
			{
				33123,
				(ChipConfig)method_02(177, 268464128, 285220864, 1051136, 2101248, 1073745920, 270609408, 268476416, obj, 270598144, 285213088, arg_144, arg_2, 33123, 1061312, obj, new List<(uint, int)>
				{
					(1058920u, 0),
					(1077980u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058984u, 49452u } }, 1061228, 52403, new List<uint[]> { new uint[2] { 50176u, 50632u } }, (DAmodes)3, arg_145, obj, 268443728, 270557184, false)
			},
			{
				33127,
				(ChipConfig)method_02(204, 268464128, 285233152, 1051136, 2101248, 1073745920, 270609408, 268476416, obj, 270585856, 285213088, arg_146, arg_2, 33127, 1062008, 1062024, new List<(uint, int)>
				{
					(1059176u, 0),
					(1079636u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1059244u, 53988u } }, 1061788, 57335, new List<uint[]> { new uint[2] { 55026u, 55212u } }, DAmodes.XFLASH, arg_147, obj, obj, 268472320, false)
			},
			{
				33128,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1073745920, obj, 268476416, obj, 270798848, 285213728, arg_148, arg_2, 33128, 1074232, 1074248, new List<(uint, int)>
				{
					(1060924u, 0),
					(1090880u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1060992u, 79924u } }, 1074044, 82799, new List<uint[]> { new uint[2] { 80920u, 81272u } }, DAmodes.XFLASH, arg_149, obj, obj, 268472320, false)
			},
			{
				33138,
				(ChipConfig)method_02(10, 268464128, 285220864, 1182208, 786432, 1073745920, 270609408, 268476416, obj, 270598144, 285213088, arg_150, arg_2, 33139, 1192112, obj, new List<(uint, int)>
				{
					(1189748u, 0),
					(1202436u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1189812u, 41188u } }, 1192028, 44139, new List<uint[]> { new uint[2] { 41912u, 42368u } }, (DAmodes)3, arg_151, obj, 18882640, null, false)
			},
			{
				33142,
				(ChipConfig)method_02(10, 268464128, 285220864, 1182208, 786432, 1075838976, 270609408, 268476416, obj, 270598144, 285213088, arg_152, arg_2, 33139, 1192112, obj, new List<(uint, int)>
				{
					(1189748u, 0),
					(1202436u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1189812u, 41188u } }, 1192028, 44139, new List<uint[]> { new uint[2] { 41912u, 42368u } }, (DAmodes)3, arg_153, obj, 18882640, 270557184, false)
			},
			{
				2352,
				(ChipConfig)method_02(obj, 268464128, 285217280, 1051136, 2101248, 1075838976, obj, obj, obj, obj, obj, arg_154, arg_2, 33173, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, DAmodes.XFLASH, obj, obj, 268542208, 297861120, false)
			},
			{
				34066,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 1118208, 1075838976, 270614528, 268476416, obj, 270594048, 285213088, arg_155, arg_2, 34066, 1066552, 1066568, new List<(uint, int)>
				{
					(1065444u, 0),
					(1092228u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1065560u, 52292u } }, 1066352, 55211, new List<uint[]> { new uint[2] { 53300u, 53652u } }, DAmodes.XFLASH, arg_156, obj, obj, 298123264, false)
			},
			{
				34072,
				(ChipConfig)method_02(obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, obj, arg_157, arg_2, 34072, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, DAmodes.XFLASH, obj, obj, obj, 268472320, false)
			},
			{
				34192,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 2147487744u, obj, 268476416, obj, 285323264, obj, arg_158, arg_2, 34192, 1061336, obj, new List<(uint, int)>
				{
					(1058928u, 0),
					(1076348u, 0)
				}, 8, new List<uint[]> { new uint[2] { 1058992u, 48100u } }, 1061252, 50975, new List<uint[]> { new uint[2] { 48824u, 49260u } }, (DAmodes)3, arg_159, obj, obj, null, false)
			},
			{
				34453,
				(ChipConfig)method_02(10, 268464128, 285220864, 1051136, 2101248, 1073745920, obj, 268476416, obj, obj, 285213728, arg_160, arg_2, 34453, 1061560, obj, new List<(uint, int)>
				{
					(1060936u, 0),
					(1076932u, 0)
				}, 10, new List<uint[]> { new uint[2] { 1061000u, 48876u } }, 1061356, 51879, new List<uint[]> { new uint[2] { 49816u, 50168u } }, DAmodes.XFLASH, arg_161, obj, obj, 270557184, false)
			},
			{
				2312,
				(ChipConfig)method_02(obj, 268464128, obj, 1051136, 2101248, obj, obj, obj, obj, obj, obj, arg_162, arg_2, 34454, obj, obj, obj, obj, (List<uint[]>)obj, obj, obj, (List<uint[]>)obj, DAmodes.XFLASH, obj, obj, obj, 297861120, false)
			}
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}

	public static object method_02(object arg_01, object arg_02, object arg_03, object arg_04, object arg_05, object arg_06, object arg_07, object arg_08, object arg_09, object arg_10, object arg_11, string arg_12, string arg_13, object arg_14, object arg_15, object arg_16, object arg_17, object arg_18, List<uint[]> arg_19, object arg_20, object arg_21, List<uint[]> arg_22, DAmodes arg_23, object arg_24, object arg_25, object arg_26, int? arg_27, bool? arg_28)
	{
		return new ChipConfig(arg_01, arg_02, arg_03, arg_04, arg_05, arg_06, arg_07, arg_08, arg_09, arg_10, arg_11, arg_12, arg_13, arg_14, arg_15, arg_16, arg_17, arg_18, arg_19, arg_20, arg_21, arg_22, arg_23, arg_24, arg_25, arg_26, arg_27, arg_28);
	}
}
