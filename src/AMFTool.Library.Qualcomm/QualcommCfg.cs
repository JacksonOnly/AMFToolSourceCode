using System.Collections.Generic;

namespace AMFTool.Library.Qualcomm;

public class QualcommCfg
{
	public static readonly Dictionary<uint, string> VendorID;

	public static readonly Dictionary<uint, string> MSMID;

	public static readonly Dictionary<uint, string> sochw;

	public static string GetVendor(uint vendorid)
	{
		string format = "Unknown (0x{0:X4})";
		if (VendorID.TryGetValue(vendorid, out var value))
		{
			return value;
		}
		return string.Format(format, vendorid);
	}

	public static string GetSOC(uint msmid)
	{
		string format = "Unknown (0x{0:X4})";
		if (!MSMID.TryGetValue(msmid, out var value))
		{
			return string.Format(format, msmid);
		}
		return value;
	}

	public static string GetVendorByPKHASH(string pkhash)
	{
		string result = "Unknown";
		object result2 = null;
		if (!string.IsNullOrEmpty(pkhash) && pkhash.Length >= 8)
		{
			return (string)result2;
		}
		return result;
	}

	static QualcommCfg()
	{
		string value = "Qualcomm";
		string value2 = "Foxconn/Sony";
		string value3 = "ZTE";
		string value4 = "Smartisan";
		string value5 = "Huawei";
		string value6 = "Lenovo";
		string value7 = "Samsung";
		string value8 = "Asus";
		string value9 = "Haier";
		string value10 = "LG";
		string value11 = "Foxconn/Nokia";
		string value12 = "Alcatel";
		string value13 = "Nokia";
		string value14 = "YuLong";
		string value15 = "Oppo/Oneplus";
		string value16 = "Xiaomi";
		string value17 = "Vivo";
		string value18 = "GlocalMe";
		string value19 = "Lyf";
		string value20 = "Motorola";
		string value21 = "Micromax";
		string value22 = "Blackphone";
		string value23 = "Oxygen";
		string value24 = "APQ8098";
		string value25 = "APQ8097";
		string value26 = "SDX20M";
		string value27 = "SDX20";
		string value28 = "MSM8909 (Snapdragon 210)";
		string value29 = "APQ8009 (Snapdragon 212)";
		string value30 = "MSM8208 (Snapdragon 200)";
		string value31 = "MSM8209 (Snapdragon 200)";
		string value32 = "SM2150";
		string value33 = "QCM2150";
		string value34 = "QCM2290";
		string value35 = "SM4125 (Snapdragon 215)";
		string value36 = "MSM8916 (Snapdragon 410)";
		string value37 = "APQ8016 (Snapdragon 410)";
		string value38 = "MSM8216 (Snapdragon 410)";
		string value39 = "MSM8116 (Snapdragon 410)";
		string value40 = "MSM8952 (Snapdragon 617)";
		string value41 = "MSM8953 (Snapdragon 625)";
		string value42 = "APQ8053 (Snapdragon 625)";
		string value43 = "MSM8937 (Snapdragon 430)";
		string value44 = "APQ8037 (Snapdragon 430)";
		string value45 = "MSM8917 (Snapdragon 425)";
		string value46 = "APQ8017 (Snapdragon 425)";
		string value47 = "MSM8940 (Snapdragon 435)";
		string value48 = "APQ8040 (Snapdragon 435)";
		string value49 = "SDM450 (Snapdragon 450)";
		string value50 = "SDA450 (Snapdragon 450)";
		string value51 = "SDM429 (Snapdragon 429)";
		string value52 = "SDM439 (Snapdragon 439)";
		string value53 = "SDA429 (Snapdragon 429)";
		string value54 = "SDA439 (Snapdragon 439)";
		string value55 = "SM4350 (Snapdragon 480)";
		string value56 = "SM4250 (Snapdragon 460)";
		string value57 = "QCM4290";
		string value58 = "SM4450 (Snapdragon 4 Gen 1)";
		string value59 = "SM4375 (Snapdragon 4 Gen 2)";
		string value60 = "SM4635 (Snapdragon 4s Gen 2)";
		string value61 = "SM4550 (Snapdragon 4 Gen 3)";
		string value62 = "MSM8976 (Snapdragon 652)";
		string value63 = "APQ8076 (Snapdragon 652)";
		string value64 = "MSM8956 (Snapdragon 650)";
		string value65 = "APQ8056 (Snapdragon 650)";
		string value66 = "SDM630 (Snapdragon 630)";
		string value67 = "SDA630 (Snapdragon 630)";
		string value68 = "SDM636 (Snapdragon 636)";
		string value69 = "SDA636 (Snapdragon 636)";
		string value70 = "SDM660 (Snapdragon 660)";
		string value71 = "SDA660 (Snapdragon 660)";
		string value72 = "SDM662 (Snapdragon 662)";
		string value73 = "SDM632 (Snapdragon 632)";
		string value74 = "SDA632 (Snapdragon 632)";
		string value75 = "SM6150 (Snapdragon 675)";
		string value76 = "SA6150 (Snapdragon 675)";
		string value77 = "SM6125 (Snapdragon 665)";
		string value78 = "SA6125 (Snapdragon 665)";
		string value79 = "SM6115 (Snapdragon 662)";
		string value80 = "SM6350 (Snapdragon 690)";
		string value81 = "SA6350 (Snapdragon 690)";
		string value82 = "SM6375 (Snapdragon 695)";
		string value83 = "SM6225 (Snapdragon 680)";
		string value84 = "SM6450 (Snapdragon 6 Gen 1)";
		string value85 = "SM6475 (Snapdragon 6s Gen 3)";
		string value86 = "SM6550 (Snapdragon 6 Gen 3)";
		string value87 = "SDM670 (Snapdragon 670)";
		string value88 = "SDA670 (Snapdragon 670)";
		string value89 = "SDM710 (Snapdragon 710)";
		string value90 = "SDA710 (Snapdragon 710)";
		string value91 = "SDM712 (Snapdragon 712)";
		string value92 = "SDA712 (Snapdragon 712)";
		string value93 = "SM7150 (Snapdragon 730)";
		string value94 = "SM7150-AB (Snapdragon 730G)";
		string value95 = "SA7150 (Snapdragon 730)";
		string value96 = "SM7250 (Snapdragon 765G)";
		string value97 = "SM7250-AB (Snapdragon 768G)";
		string value98 = "SA7250 (Snapdragon 765G)";
		string value99 = "SM7225 (Snapdragon 750G)";
		string value100 = "SM7325 (Snapdragon 778G)";
		string value101 = "SM7325-AE (Snapdragon 778G+)";
		string value102 = "SM7350 (Snapdragon 780G)";
		string value103 = "SA7325 (Snapdragon 778G)";
		string value104 = "SM7435 (Snapdragon 7s Gen 2)";
		string value105 = "SM7450 (Snapdragon 7 Gen 1)";
		string value106 = "SM7450-AB (Snapdragon 7+ Gen 2)";
		string value107 = "SM7550 (Snapdragon 7 Gen 3)";
		string value108 = "SM7675 (Snapdragon 7+ Gen 3)";
		string value109 = "MSM8974 (Snapdragon 800)";
		string value110 = "MSM8974-AA (Snapdragon 800)";
		string value111 = "MSM8974-AB (Snapdragon 801)";
		string value112 = "MSM8974-AC (Snapdragon 801)";
		string value113 = "APQ8074 (Snapdragon 800)";
		string value114 = "APQ8074-AA (Snapdragon 800)";
		string value115 = "APQ8074-AB (Snapdragon 801)";
		string value116 = "MSM8994 (Snapdragon 810)";
		string value117 = "MSM8994V (Snapdragon 810)";
		string value118 = "APQ8094 (Snapdragon 810)";
		string value119 = "MSM8992 (Snapdragon 808)";
		string value120 = "APQ8092 (Snapdragon 808)";
		string value121 = "MSM8996 (Snapdragon 820)";
		string value122 = "APQ8096 (Snapdragon 820)";
		string value123 = "MSM8996L (Snapdragon 820)";
		string value124 = "APQ8096SG (Snapdragon 820)";
		string value125 = "MSM8996Pro (Snapdragon 821)";
		string value126 = "MSM8996Pro-AB (Snapdragon 821)";
		string value127 = "APQ8096Pro (Snapdragon 821)";
		string value128 = "MSM8998 (Snapdragon 835)";
		string value129 = "APQ8098 (Snapdragon 835)";
		string value130 = "SDM845 (Snapdragon 845)";
		string value131 = "SDA845 (Snapdragon 845)";
		string value132 = "SM8150 (Snapdragon 855)";
		string value133 = "SA8150 (Snapdragon 855)";
		string value134 = "SM8150p (Snapdragon 855+)";
		string value135 = "SM8150-AC (Snapdragon 855+)";
		string value136 = "SM8250 (Snapdragon 865)";
		string value137 = "SM8250-AB (Snapdragon 865+)";
		string value138 = "SA8250 (Snapdragon 865)";
		string value139 = "SM8350 (Snapdragon 888)";
		string value140 = "SM8350-AB (Snapdragon 888+)";
		string value141 = "SA8350 (Snapdragon 888)";
		string value142 = "SM8450 (Snapdragon 8 Gen 1)";
		string value143 = "SA8450 (Snapdragon 8 Gen 1)";
		string value144 = "SM8475 (Snapdragon 8+ Gen 1)";
		string value145 = "SM8475-AB (Snapdragon 8+ Gen 1)";
		string value146 = "SM8550 (Snapdragon 8 Gen 2)";
		string value147 = "SA8550 (Snapdragon 8 Gen 2)";
		string value148 = "SM8650 (Snapdragon 8 Gen 3)";
		string value149 = "SM8650-AB (Snapdragon 8 Gen 3)";
		string value150 = "SA8650 (Snapdragon 8 Gen 3)";
		string value151 = "SM8635 (Snapdragon 8s Gen 3)";
		string value152 = "SM8750 (Snapdragon 8 Elite)";
		string value153 = "SM8750-AB (Snapdragon 8 Elite)";
		string value154 = "SA8750 (Snapdragon 8 Elite)";
		string value155 = "SM8775 (Snapdragon 8 Elite 2)";
		string value156 = "SA8635 (Snapdragon 8s Gen 3)";
		string value157 = "MDM9x25";
		string value158 = "MDM9x35 (X7 LTE)";
		string value159 = "MDM9x40 (X10 LTE)";
		string value160 = "MDM9x45 (X12 LTE)";
		string value161 = "MDM9x50 (X16/X20 LTE)";
		string value162 = "MDM9x55 (X20 LTE)";
		string value163 = "SDX50M (X50 5G)";
		string value164 = "SDX55 (X55 5G)";
		string value165 = "SDX60 (X60 5G)";
		string value166 = "SDX62 (X62 5G)";
		string value167 = "SDX65 (X65 5G)";
		string value168 = "SDX70 (X70 5G)";
		string value169 = "SDX72 (X72 5G)";
		string value170 = "SDX75 (X75 5G)";
		string value171 = "SDX80 (X80 5G)";
		string value172 = "SDX82 (X82 5G)";
		string value173 = "SDX24 (X24 LTE)";
		string value174 = "MDM9150";
		string value175 = "MDM9205";
		string value176 = "MDM9206";
		string value177 = "MDM9207";
		string value178 = "SDX55M";
		string value179 = "SDX35";
		string value180 = "SDX35M";
		string value181 = "QCS605";
		string value182 = "QCS603";
		string value183 = "QCS404";
		string value184 = "QCS6490";
		string value185 = "QCM6490";
		string value186 = "QCS8550";
		string value187 = "QCM8550";
		string value188 = "QCS8450";
		string value189 = "QCS4490";
		string value190 = "QCM4490";
		string value191 = "SA6155";
		string value192 = "SA6145";
		string value193 = "SA8155";
		string value194 = "SA8195";
		string value195 = "SA8255 (Snapdragon 680)";
		string value196 = "SA8295";
		string value197 = "SA8650P";
		string value198 = "SA8770P";
		string value199 = "SA415M";
		string value200 = "SA7775P";
		string value201 = "QCS8250";
		string value202 = "MSM8928 (Snapdragon Wear 2100)";
		string value203 = "MSW8909W (Snapdragon Wear 2100)";
		string value204 = "SDW3100 (Snapdragon Wear 3100)";
		string value205 = "SDW4100 (Snapdragon Wear 4100)";
		string value206 = "SW5100 (Snapdragon Wear W5)";
		string value207 = "SW5100+ (Snapdragon Wear W5+)";
		string value208 = "SC8280X (Snapdragon 8cx Gen 3)";
		string value209 = "SDM850 (Snapdragon 850)";
		string value210 = "SC8180X (Snapdragon 8cx)";
		string value211 = "SC8180XP (Snapdragon 8cx)";
		string value212 = "SC8180X-AD (Snapdragon 8c)";
		string value213 = "SC7180 (Snapdragon 7c)";
		string value214 = "SC7280 (Snapdragon 7c Gen 2)";
		string value215 = "SC7280P (Snapdragon 7c+ Gen 3)";
		string value216 = "SC8380X (Snapdragon X Elite)";
		string value217 = "SC8380XP (Snapdragon X Elite)";
		string value218 = "SC8280XP (Snapdragon 8cx Gen 3)";
		string value219 = "XR2 (Snapdragon XR2)";
		string value220 = "XR2+ (Snapdragon XR2+)";
		string value221 = "XR2 Gen 2";
		string value222 = "MSM8926 (Snapdragon 400)";
		string value223 = "MSM8226 (Snapdragon 400)";
		string value224 = "MSM8626 (Snapdragon 400)";
		string value225 = "MSM8526 (Snapdragon 400)";
		string value226 = "APQ8028 (Snapdragon 400)";
		string value227 = "MSM8939 (Snapdragon 615)";
		string value228 = "MSM8939 (Snapdragon 616)";
		string value229 = "APQ8039 (Snapdragon 615)";
		string value230 = "MSM8929 (Snapdragon 415)";
		string value231 = "MSM8629 (Snapdragon 415)";
		string value232 = "APQ8029 (Snapdragon 415)";
		string value233 = "MSM8239 (Snapdragon 610)";
		string value234 = "MSM8936 (Snapdragon 610)";
		string value235 = "APQ8036 (Snapdragon 610)";
		string value236 = "MSM8274 (Snapdragon 800)";
		string value237 = "APQ8084 (Snapdragon 805)";
		string value238 = "MSM8997";
		string value239 = "SDM455";
		string value240 = "SXR1130";
		string value241 = "SXR1120";
		string value242 = "QCS405";
		string value243 = "QCS407";
		string value244 = "QRB5165";
		string value245 = "QRB3165";
		string value246 = "QRB4210";
		string value247 = "QRB2210";
		string value248 = "qcs405";
		string value249 = "IPQ6018";
		string value250 = "MSM8998_SDM835,SDA835";
		string value251 = "SDM660";
		string value252 = "SDM630";
		string value253 = "QCA4020";
		string value254 = "IPQ8074,QCA6290";
		string value255 = "QCA6390";
		string value256 = "QCN7605,QCA6595,QCN7606";
		string value257 = "QCN9000,QCN9001,QCN9002,QCN9003,QCN9010,QCN9011,QCN9012,QCN9013";
		string value258 = "moselle";
		string value259 = "WCN7850,WCN7851";
		string value260 = "SDM845";
		string value261 = "SDA845";
		string value262 = "SDX24,SDX24M";
		string value263 = "SM8150,SM8150p";
		string value264 = "SDA670,SDM670,SDM710";
		string value265 = "SDM670";
		string value266 = "sc8180x";
		string value267 = "SM6150,SM6150p,SM6150_IoT_High,SM6150_IoT_Low,SM6155,SM6155p";
		string value268 = "SM8250:CD90-PH805-1A,SM8250:CD90-PH806-1A,SM8250";
		string value269 = "SDX55:CD90-PG591,SDX55:CD90-PH809";
		string value270 = "SM7150,SM7150p";
		string value271 = "saipan,fraser";
		string value272 = "rennell,rennell_premier,rennell_v1.1,sc7180";
		string value273 = "lahaina";
		string value274 = "bitra_SDM";
		string value275 = "chitwan";
		string value276 = "SC8280X,SA8295P,SA8540P";
		string value277 = "olympic_v1,olympic_v1_hybrid";
		string value278 = "cedros";
		string value279 = "sm7325,sc7280,sc7295";
		string value280 = "qtang2";
		string value281 = "SDM662";
		string value282 = "nicobar,nicobar_IoT_APQ,nicobar_IoT_modem";
		string value283 = "kamorta,kamorta_P,kamorta_IoT_APQ,kamorta_IoT_modem,sm6225";
		string value284 = "agatti,agatti_mdm_iot,agatti_mdm,qcs2290";
		string value285 = "mannar,mannar_P";
		string value286 = "strait";
		string value287 = "divar";
		VendorID = new Dictionary<uint, string>
		{
			{ 0u, value },
			{ 1u, value2 },
			{ 4u, value3 },
			{ 17u, value4 },
			{ 21u, value5 },
			{ 23u, value6 },
			{ 32u, value7 },
			{ 41u, value8 },
			{ 48u, value9 },
			{ 49u, value10 },
			{ 53u, value11 },
			{ 66u, value12 },
			{ 69u, value13 },
			{ 72u, value14 },
			{ 81u, value15 },
			{ 114u, value16 },
			{ 115u, value17 },
			{ 304u, value18 },
			{ 313u, value19 },
			{ 360u, value20 },
			{ 432u, value20 },
			{ 520u, value20 },
			{ 552u, value20 },
			{ 10902u, value21 },
			{ 744u, value6 },
			{ 808u, value20 },
			{ 872u, value20 },
			{ 968u, value20 },
			{ 200u, value20 },
			{ 840u, value20 },
			{ 4163u, value8 },
			{ 4369u, value8 },
			{ 5178u, value8 },
			{ 6520u, value22 },
			{ 10864u, value23 }
		};
		MSMID = new Dictionary<uint, string>
		{
			{ 180449u, value24 },
			{ 188641u, value25 },
			{ 250081u, value26 },
			{ 254177u, value27 },
			{ 9830625u, value28 },
			{ 7479521u, value28 },
			{ 7471329u, value29 },
			{ 9654497u, value30 },
			{ 9769185u, value31 },
			{ 1110241u, value32 },
			{ 1298657u, value33 },
			{ 1761505u, value34 },
			{ 1417441u, value35 },
			{ 7360737u, value36 },
			{ 7364833u, value37 },
			{ 7377121u, value38 },
			{ 7381217u, value39 },
			{ 9904353u, value40 },
			{ 286945u, value41 },
			{ 319713u, value42 },
			{ 323809u, value43 },
			{ 327905u, value44 },
			{ 332001u, value45 },
			{ 336097u, value46 },
			{ 438497u, value47 },
			{ 442593u, value48 },
			{ 631009u, value49 },
			{ 651489u, value50 },
			{ 778465u, value51 },
			{ 782561u, value52 },
			{ 786657u, value53 },
			{ 790753u, value54 },
			{ 1151201u, value55 },
			{ 1306849u, value56 },
			{ 1315041u, value57 },
			{ 1806561u, value58 },
			{ 1822945u, value59 },
			{ 2085089u, value60 },
			{ 2597089u, value61 },
			{ 10027233u, value62 },
			{ 10031329u, value63 },
			{ 635105u, value64 },
			{ 639201u, value65 },
			{ 704737u, value66 },
			{ 708833u, value67 },
			{ 835809u, value68 },
			{ 839905u, value69 },
			{ 573665u, value70 },
			{ 577761u, value71 },
			{ 1364193u, value72 },
			{ 762081u, value73 },
			{ 766177u, value74 },
			{ 610529u, value75 },
			{ 614625u, value76 },
			{ 1106145u, value77 },
			{ 1089761u, value77 },
			{ 1102049u, value78 },
			{ 1302753u, value79 },
			{ 1433825u, value80 },
			{ 1437921u, value81 },
			{ 1695969u, value82 },
			{ 1827041u, value83 },
			{ 2220257u, value84 },
			{ 2474209u, value85 },
			{ 2592993u, value86 },
			{ 594145u, value87 },
			{ 598241u, value88 },
			{ 897249u, value89 },
			{ 901345u, value90 },
			{ 905441u, value91 },
			{ 909537u, value92 },
			{ 946401u, value93 },
			{ 950497u, value94 },
			{ 962785u, value95 },
			{ 1171681u, value96 },
			{ 1175777u, value97 },
			{ 1179873u, value98 },
			{ 1556705u, value99 },
			{ 1646817u, value100 },
			{ 1650913u, value101 },
			{ 1454305u, value102 },
			{ 1655009u, value103 },
			{ 1892577u, value104 },
			{ 1958113u, value105 },
			{ 1962209u, value106 },
			{ 2351329u, value107 },
			{ 2482401u, value108 },
			{ 8061153u, value109 },
			{ 8065249u, value110 },
			{ 8069345u, value111 },
			{ 8077537u, value112 },
			{ 8110305u, value113 },
			{ 8114401u, value114 },
			{ 8118497u, value115 },
			{ 9699553u, value116 },
			{ 9703649u, value117 },
			{ 9711841u, value118 },
			{ 9867489u, value119 },
			{ 9871585u, value120 },
			{ 9728225u, value121 },
			{ 9732321u, value122 },
			{ 9736417u, value123 },
			{ 9752801u, value124 },
			{ 389345u, value125 },
			{ 393441u, value126 },
			{ 397537u, value127 },
			{ 385249u, value128 },
			{ 381153u, value129 },
			{ 569569u, value130 },
			{ 565473u, value131 },
			{ 676065u, value132 },
			{ 671969u, value133 },
			{ 680161u, value134 },
			{ 684257u, value135 },
			{ 798945u, value136 },
			{ 803041u, value137 },
			{ 844001u, value136 },
			{ 848097u, value138 },
			{ 1265889u, value139 },
			{ 1269985u, value140 },
			{ 1274081u, value141 },
			{ 1450209u, value142 },
			{ 1446113u, value143 },
			{ 1638625u, value144 },
			{ 1966305u, value145 },
			{ 1876193u, value146 },
			{ 1880289u, value147 },
			{ 2269409u, value148 },
			{ 2261217u, value149 },
			{ 2273505u, value150 },
			{ 2531553u, value151 },
			{ 2670817u, value152 },
			{ 2670818u, value153 },
			{ 2674913u, value154 },
			{ 2736353u, value155 },
			{ 2502881u, value151 },
			{ 2535649u, value156 },
			{ 8343777u, value157 },
			{ 9502945u, value158 },
			{ 9777377u, value159 },
			{ 9789665u, value160 },
			{ 315617u, value161 },
			{ 516321u, value162 },
			{ 626913u, value163 },
			{ 647393u, value164 },
			{ 1462497u, value165 },
			{ 1704161u, value166 },
			{ 1442017u, value167 },
			{ 1978593u, value168 },
			{ 2285793u, value169 },
			{ 2281697u, value170 },
			{ 2642145u, value171 },
			{ 2633953u, value172 },
			{ 524513u, value173 },
			{ 258273u, value174 },
			{ 229601u, value175 },
			{ 237793u, value176 },
			{ 245985u, value177 },
			{ 1343713u, value178 },
			{ 1843425u, value179 },
			{ 1933537u, value180 },
			{ 729313u, value181 },
			{ 733409u, value182 },
			{ 893153u, value183 },
			{ 1380577u, value184 },
			{ 1384673u, value185 },
			{ 1859809u, value186 },
			{ 1913057u, value187 },
			{ 1929441u, value188 },
			{ 1708257u, value189 },
			{ 1769697u, value190 },
			{ 954593u, value191 },
			{ 1413345u, value192 },
			{ 1458401u, value193 },
			{ 1356001u, value194 },
			{ 1802465u, value195 },
			{ 1810657u, value196 },
			{ 1814753u, value197 },
			{ 1818849u, value198 },
			{ 958689u, value199 },
			{ 1822946u, value200 },
			{ 2048225u, value201 },
			{ 9437409u, value202 },
			{ 291041u, value203 },
			{ 966881u, value204 },
			{ 970977u, value205 },
			{ 1482977u, value206 },
			{ 1487073u, value207 },
			{ 1351905u, value208 },
			{ 749793u, value209 },
			{ 753889u, value210 },
			{ 757985u, value211 },
			{ 1143009u, value212 },
			{ 1147105u, value213 },
			{ 1716449u, value214 },
			{ 1720545u, value215 },
			{ 1900769u, value216 },
			{ 1904865u, value217 },
			{ 2126049u, value218 },
			{ 1114337u, value219 },
			{ 1118433u, value220 },
			{ 1937633u, value221 },
			{ 7405793u, value222 },
			{ 7418081u, value223 },
			{ 7422177u, value224 },
			{ 7426273u, value225 },
			{ 7442657u, value226 },
			{ 9605345u, value227 },
			{ 9613537u, value228 },
			{ 9621729u, value229 },
			{ 9572577u, value230 },
			{ 9580769u, value231 },
			{ 9588961u, value232 },
			{ 9568481u, value233 },
			{ 9691361u, value234 },
			{ 9662689u, value235 },
			{ 8073441u, value236 },
			{ 8081633u, value237 },
			{ 377057u, value238 },
			{ 499937u, value239 },
			{ 930017u, value240 },
			{ 934113u, value241 },
			{ 938209u, value242 },
			{ 942305u, value243 },
			{ 1335521u, value244 },
			{ 1339617u, value245 },
			{ 1589473u, value246 },
			{ 1601761u, value247 }
		};
		sochw = new Dictionary<uint, string>
		{
			{ 8211u, value175 },
			{ 8212u, value248 },
			{ 8215u, value249 },
			{ 12290u, value250 },
			{ 12294u, value251 },
			{ 12295u, value252 },
			{ 16387u, value253 },
			{ 16388u, value254 },
			{ 16394u, value255 },
			{ 16395u, value256 },
			{ 16397u, value257 },
			{ 16404u, value258 },
			{ 16407u, value259 },
			{ 24576u, value260 },
			{ 24577u, value261 },
			{ 24578u, value262 },
			{ 24579u, value263 },
			{ 24580u, value264 },
			{ 24581u, value265 },
			{ 24582u, value266 },
			{ 24583u, value267 },
			{ 24584u, value268 },
			{ 24585u, value265 },
			{ 24587u, value269 },
			{ 24588u, value270 },
			{ 24589u, value271 },
			{ 24590u, value272 },
			{ 24591u, value273 },
			{ 24594u, value274 },
			{ 24595u, value275 },
			{ 24596u, value276 },
			{ 24598u, value277 },
			{ 24599u, value278 },
			{ 24600u, value279 },
			{ 28673u, value280 },
			{ 29184u, value281 },
			{ 36865u, value282 },
			{ 36866u, value283 },
			{ 36867u, value284 },
			{ 36868u, value285 },
			{ 36870u, value286 },
			{ 36871u, value287 }
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
