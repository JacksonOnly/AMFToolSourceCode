using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using AMFTool.Library.Usb;
using Microsoft.VisualBasic;
using SharpMTKPlExploit.Class;

namespace AMFTool.Library.Mediatek.Download_Agent.xflash.extention;

public class XFlashExt
{
	public MtkClass Mtk;

	public DAXFlash XFlash;

	private string[] RpmbError;

	private string GetRpmbError(uint code)
	{
		string format = "Error: 0x{0:X}";
		if (code >= (uint)RpmbError.Length)
		{
			return string.Format(format, code);
		}
		return RpmbError[code];
	}

	public XFlashExt(MtkClass mtk, DAXFlash xFlashLib)
	{
		string text = "";
		string text2 = "General failure";
		string text3 = "Authentication failure";
		string text4 = "Counter failure";
		string text5 = "Address failure";
		string text6 = "Write failure";
		string text7 = "Read failure";
		string text8 = "Authentication key not yet programmed";
		RpmbError = new string[8] { text, text2, text3, text4, text5, text6, text7, text8 };

		Mtk = mtk;
		XFlash = xFlashLib;
	}

	public byte[] Xtpatch(byte[] da2, long da2address)
	{
		string hex = "01308FE213FF2FE110B503202D4C01F0F4EC2D497C44A36B4FF47020794498472A49A36B1C48794498472949A36B1B48794498472749A36B1948794498472649A36B1848794498472449A36B1648794498472349A36B1548794498472149A36B1348794498472049A36B1248794498471E49A36B1048794498471D49A36B0F48794498471B49A36B0D4879449847012001F0E0EC002010BD01000F0002000F0003000F0004000F0005000F0006000F0007000F0008000F0009000F000A000F000B000F00CC1B000079010000B3010000C5000000E90000001F010000470B000005020000CB020000AD0A00007F070000650800002701000030B5002385B00425044603A901930395029303680DEB05009847019B2946039502A81B6802936368984705B030BD2DE93F480022002301AD04462946CDE9022300220823CDE90023036802A89847294604236D460193284623689847DDE902BC294623685846984704B0BDE83088F0B585B00026042703AC05460368214601960DEB07000296039798472B68214602A803979847019B3046029A1A6005B0F0BD054B07B50421019343680DEB0100984703B05DF804FB00BFA4A3A2A173B52026074C054601A903687C440196204698476B68314620469847002002B070BD00BF201A000070B5ADF5003D84B0054604AC0026002320460022314644E9022340F8046B184A00F03DFD44F8106C08232646A4F1080046F80C3D31462B6898470423314644F80C3C68462B68984754F8103C54E90201B3F5003F88BF4FF40033014688BF44F8103C204654F8102C00F062FD6B68204654F8101C98470DF5003D04B070BD00BFFCFF01002DE9F04F0646DFF8C080ADF21C4D00204FF004094FF0000A05AFF84406ADD8F820304FF4806B984790F8FF100446D8F8243021F0070141F00301984704A932683846CDF814A0C1F80090904751465A4628464FF0010A00F0E0FC059B5246CDF800A02946D8F828705BBAADF816B2ADF810324B462068B84702274B46009752462946D8F828802068C047BDF814320DF10E0039465BBA038073689847BDF8143223B973684FF480713FA89847074B204694F8FF107B445B6A01F0F801984700200DF21C4DBDE8F08F66190000CC1800002DE9F04F83464B4EADF21C4D002006AC4FF001084FF002097E444FF0000A336A984790F8FF100546736A21F0070141F0030198474FF480620021204600F085FC4FF40073CDF80080ADF816324246B76A214628680223B847CDF800904B46B76A424621462868B847042305AF059304ABDBF8002039461846ADF810A0039390474FF4807239463FAFDBF80030059238469847039B4FF48072394637A8ADF812224FF48E721B88ADF814A25BBAADF810324FF44073ADF8163220230093334601F005FACDF800800323B76A424621462868B84751464FF48062204600F036FC4FF4A063CDF80080ADF816324246B76A214628680523B8474246CDF800902146B76A28680523B847BDF814320DF1120049465BBA0380DBF80430984795F8FF102846736A01F0F801984750460DF21C4DBDE8F08F00BF98180000012303FA01F1C36C23EA0103C36470470A44914200D1704711F8013B00F8013BF7E7D0F89C3EC3F30903002BF9D00B68C0F8803E4B68C0F8843E8B68C0F8883ECB68C0F88C3E0B69C0F8903E4B69C0F8943E704702460023C1E90023704782B000230193D0F8A03B0193019B002BF9D0019802B0704770470423C0F8083A704782B0D0F8003A0193019B002BF9D0019802B07047042330B5C0F8083A8BB00023002400250093CDE902451D466C460493CDE9024504A9DDE902450246124B0694DDE9024505932B040993104B07934FF480730893FFF7A5FF1046FFF7D1FF4307FAD5002301931046FFF7B9FF0190019B002BF8D0019B012B09BF042304480020C2F8083A0BB030BD1100000812000008010000F682B001900091019B009A1A6002B0704701292DE9F04F89B0924604460D46DDE912879B46DDE916266AD1D0F8A03A13F0020F0CBF10232023DB040421C4F8081A4FF00009A3F5000302A9019320462D4BCDF80890CDE904990393019BCDF81C9043F0807343F4E1530693FFF750FFCDE90299CDE90499CDF8189035B91FFA8BF3CDF808A00793422303930198AB04BFB24FF0000903F440130343E80300F4C0300343204643F0806343F4E1530693FFF72EFF149BCDF81090CDF8088023F07F4307979B00CDF8149043F00203039301230693FFF71CFF022D0CD00B4B3604CDF80890079606934223CDF80C9004920593FFF70DFF2046FFF743FF09B0BDE8F08F102399E741000008261C000830B587B00A9D002402930023CDE900240022CDE90454FFF775FF07B030BD2DE9F04F85B0DDE90EB7804615461C46DDF844A00E46002958D00AB9002B57D1BBF1000F01D1002F52D1DFF8A89043222A48002100F0CEFA012389F810305DB32946A51C26482246FFF7C3FE05F50013002203F5516083F8102DBBF1000F04D03D443A465946FFF7B4FE4FEACA0305F50015DFF864B03C4485F8103D0AF10F031B09002503340393039BAB4206D10027384605B0BDE8F08F1D46D7E705F1010A23465A46314689F810A04046CDF80090FFF797FF109B07460749102203EB0510FFF787FE002FE3D15546DDE7054FDFE7054FDDE700BF000D2000100D2000110D2000020000F2030000F270B504460D461646002110220A302DBA00F06BFAC4232561C4E900360C2363834FF40073E361024BA3600123637370BD0100B5EC70B504460D461646002110220A302DBA00F051FA0C2325616383C423C4E900364FF40073E361024BA3600123637370BD0200A2EC2DE9F04FADF50F7D374F03A904230DF1100A4FF0000B0B607F44036806460DF104085046CAF800B098473B6B4146F86A0DF10809984760B94FF0FF330221C9F8003048467368984700200DF50F7DBDE8F08F0EAC4FF40072594620460D9405AD00F00FFADAF800304FF40071D8F8002028464FF4807A5BBAADF832A2ADF830324FF48063ADF83632FFF786FF7B6B2946F86A98474FF400725946204600F0F1F9D8F800204FF400712846FFF78FFF7B6B2946F86A9847BDF83432022148465BBA9BB2C9F8003073689847BDF834321BB97368514647A89847044BD8F800107B44D86AFFF7C5FDAFE7D01300000A1300002DE9F04F04234C4CADF5137D4FF000090DF11C0B079308AB7C4482460268594618460DF11808CDF820904FF40077039312AE9047226B4146E06A09AD90473A4649463046119600F0A4F9069A39462846ADF84672FFF728FF626B2946E06A90473A464946304600F094F9069A39462846FFF734FF626B2946E06A90474FF4807259460DF5967BDAF80030079258469847039B4FF48072594643A8ADF842224FF48E721B68ADF844925BBAADF840324FF44073ADF8463220230093234600F02AFF069A39462846FFF7EFFE636B2946E06A98473A464946304600F05BF9069A4FF4A06339462846ADF84632FFF7DDFE636B2946E06A98473A464946304600F049F9069A39462846FFF7E9FE636B2946E06A9847BDF844320DF1160002215BBA0380DAF804309847E06A0699FFF729FD48460DF5137DBDE8F08FE01200002DE9F04F93B00C46059000211E220AA8099100F020F9B4F8FC31002B62D1334F04F1E40909AE20254FF48E7249467F4430463B46C434009500F0CEFE2A463146204600F05CF918B9002013B0BDE8F08F27490DF118080C2240464FF0102A4FF0C05B794400F040F90423CAF88CB0019308AB009342461C480823012103950297FFF7FFFD10F0FF00DFD14FF48E723B4649463046CAF888B0009500F09DFE059B294638465B6898472A463146204600F026F90028C8D00F492A463846794400F013F94FF48E72494630463B46009500F083FE2A463146204600F011F90028B3D0FE20B2E7FF20B0E7000021108E1100002A100000DD0F00002DE9F043ADF50B7D1B4C6E464FF400780AAF814631467C4401AD236BE06A9847424600213846099700F099F8326841462846ADF82682FFF71DFE636B2946E06A984742460021384600F089F8326841462846FFF729FE636B2946E06A98473168E06AFFF773FC39464846FFF74DFF0DF50B7DBDE8F08300BFAE1000002DE9F04180461D4EADF5816D002002AC01277E44336A984790F8FF100546736A21F0070141F0030198474FF480620021204600F056F84FF400730097ADF806323A46D6F828C0214628680223E04702233A46009321462868B76AB84795F8FF102846736A01F0F801984721464046FFF70DFF0DF5816DBDE8F08100BF3610000030B5AAB14C081346A34214D90025994228BFC91A4FEA53032CBF6C1C2C469A424FEA4405F3D924F000444160046001E00260426030BD5B00E6E707B50A4601466846FFF7DDFF009803B05DF804FB07B50A4601466846FFF7D3FF019803B05DF804FBC9B20244034610B5934210D21C4604F8011BA2420BD99C1C5970A24207D9DC1C9970A24203D9043303F8011CECE710BD03461A46013311780029FAD1101A70470139421E11F8013F02F8013F002BF9D17047431E13F8010F11F8012B904201D0801A01E00028F5D17047034610B5104642B1013B8C1813F8010F11F8012B904201D0801A10BD0028FCD08C42F3D10020F8E7431E0A4410B5914200D110BD11F8014B03F8014FF7E730B501390024A24201D1002005E0035D01340D5DAB42F6D0581B30BD70B5044608460E46FFF7B0FF0246254614F8013B0BB91D4605E031462846FFF7E1FF0028F3D1284670BD2DE9F04FBBB0814602462AA9403092F82930043292F824401B0403EB046392F82740234492F82640904203EB042341F8043BECD1D9F800304FF0000BDFF800A91293D9F80430FA441393D9F808301493D9F80C301593D9F810301693D9F814301793D9F818301893D9F81C3019932A9B1A932B9B1B932C9B1C932D9B1D932E9B1E932F9B1F93309B2093319B2193329B149A12982293339BDDF864E0189E2393349B179F169D2493359BCDF820B0CDF81CB02593369BCDF818B0CDF834B02693379BCDF814B0CDF830B02793389BCDF810B02893399B2993139B0F93159BCDF82CB0CDF80CB0CDF828B0CDF840B0CDF804B0CDF808B0CDF824B0CDF838B0CDF800B04FEAF52C87EA06042C4000998CEAB51C74408CEA756808EB040CDAF800406444002900F0EB830E994FEAF14C8CEA714C8CEA912C02994FEAB14888EAF11888EAD1080999C4448C440199614401914FEA703C01998CEAB00C0C440F99A6448CEAB05C734440EA010400EA0108DAF80410144044EA080464444FEAF32C0E440099744485EA070E8CEAB31C0EEA030E8EEA070E8CEA736C7644002900F0B18310994FEAF14E8EEA714E8EEA912E029988460A9988440399C6444FEAB14888EAF11888EAD1080EEB08010291664402994FEA743E44EA000C8EEAB40E04EA00080E440F998EEAB45E32440CEA010C00994CEA080CF4444FEAF22EB44483EA05068EEAB21E164086EA0508DAF808608EEA726E3E444644002900F0758304994FEAB14787EAF11787EAD107039988460B998844019947444FEAF14888EA714888EA912807EB08014FEA7C3803910399764488EABC0844EA0C0E04EA0C070E4488EABC580F990EEA000E4EEA070EC64483EA02083144B644DAF80C604FEAF12708EA0108119187EAB11788EA03083544009E87EA71674544002E00F03783059E05994FEAB64686EAF11686EAD106049988460C998844029946444FEAF14888EA714888EA912806EB08014FEA7E38049104993D4488EABE084CEA0E070CEA0E060D4488EABE58119927403743284447444FEAF02682EA010800992F44DAF8105086EAB01608EA000888EA02082B4486EA70664344002900F0FB8206994FEAB14585EAF11585EAD105059988460D998844039945444FEAF14888EA714888EA912805EB08014FEA773805913344059988EAB7084EEA07060EEA070588EAB75806EA0C062E430B4446441C441E44119B4FEAF42585EAB41583EA000808EA040885EA746588EA0308DAF814301A44009B4244002B00F0BF82079B07994FEAB34383EAF11383EAD103069988460E998844049943444FEAF14888EA714888EA912843444FEA76380693069B2A4488EAB60847EA060505EA0E0511991A4488EAB65807EA06031D439444454480EA0408AB184FEAFC220F9308EA0C08DAF8183082EABC1288EA000882EA7C621944009B4144002B00F08282089B089D4FEAB34383EAF51383EAD503079DA846109DA844059D43444FEAF54888EA754888EA9528434407931144079B19440F9B8E4446EA03024FEA73383A4088EAB30888EAB35833401A434FEAFE2342440A44DAF81C1084EA0C0883EABE1308EA0E080844009988EA040883EA7E634044002900F048824FEABB480899069D88EAFB1888EADB0888440199884406994FEAF14181EA754181EA9521414408911844089B0F9D18440F9B39184FEA723705EA020887EAB2071343334087EAB25743EA08038CEA0E083B444FEAF1270344DAF8200087EAB11708EA010888EA0C080444009887EA7167A044002800F00E820998099C4FEAB04080EAF41080EAD400079C83440298834407984FEAF04080EA744080EA942083444FEA7334B8440F9D08EB0B0084EAB30406EB000842EA030602EA030784EAB3542E403E438EEA010734444FEAF8260444DAF8240086EAB81607EA080787EA0E078444009886EA7866BC44002800F0D5810A980A9D4FEAB04080EAF51080EAD500099D2844039D2844089D4FEAF54787EA754787EA952738440990B444099843EA040616404FEA743584440F9885EAB405604485EAB455119003EA0400064311982E4481EA0805664405404FEAF0274D4087EAB01787EA7067DAF8280086440098AE44002800F09B810B980B9D4FEAB04080EAF51080EAD5000A9D2844049D28444FEAFB4585EA7B4585EA9B2528440A90BE440A9844EA060704EA06051F402F43864402EB0E004FEA76324FEAF02C82EAB6028CEAB01C82EAB6528CEA706C174407EB0E020F92119A88EA0202024082EA0805DAF82C200A4400992A44002900F060810C990C9D099F4FEAB14181EAF51181EAD5010B9D2944059D2944099D4FEAF54585EA774585EA972529440B9162440B990A440F9903EB020C0F9B46EA010525404FEA733383EAB10383EAB15331400D434FEAFC211D44119B154481EABC1183EA000281EA7C6102EA0C025340DAF8302042441A44009B002B00F024810D9B0D9F4FEAB34383EAF71383EAD7030C9F3B44069F3B440A9F4FEAF74E8EEA774E8EEA972E73440C930A440C9B4FEA7531119F81EAB5011A440F9B04EB020E0F9C81EAB5512B432C40334023434FEAFE240B4480EA0C01134401EA0E0184EABE1481EA0002DAF8341084EA7E640F4439461144009A002A00F0E8800E9A0E9F4FEAB24282EAF71282EAD7020D9F3A44079F3A440B9F4FEAF74888EA774888EA972842440D9221440D9A4FEA73340F9F05EA030884EAB304114445EA030284EAB3543A4042EA08020E442244DAF838400A444FEAF6218CEA0E082044009C81EAB61108EA060888EA0C0881EA76614044002C00F0AD80109C0C9F4FEAB44888EAF41888EAD4080E9CA044089CA0440C9C4FEAF44484EA774484EA972444440E9408440E994FEA723803EA020488EAB20808440F9988EAB25801440F4643EA0201294021438EEA060441443C40014484EA0E084FEAF720DAF83C400F91009980EAB710644480EA77604444002972D001994FEAB14888EAF11888EAD108109988440D99D8444FEAF14C8CEA714C8CEA912C08EB0C011091044410990AF1400A0C440F99254442EA010002EA010C4FEA7138184040EA0C0088EAB10888EAB15840442044009C1034402C00947FF426AC129901440F98C9F8001013990144C9F8041014991144159AC9F808101A44169BC9F80C202B44C9F81030179B3B44C9F81430189B3344C9F81830199B7344C9F81C303BB0BDE8F08F1A9925E41B9961E41C999DE41D99DCE41E9917E51F9B53E5209B90E52199C8E5DDF888B002E623983CE6249875E62599B2E6269BEDE6279A29E7289C64E729999DE7320D00000D4A0E4B0E49DFF848C0C0E9002300220C4BC0E902130C4BC0E904C30B4B836103F17453A3F52303A3F69243C3610023C0E90823704700BF67E6096A85AE67BB72F36E3C3AF54FA58C68059BABD9831F7F520E5170B50446036A0D468E1803F03F03B54200D170BD15F8011BE218581C82F82810D4E90812013142F100023F2BC4E9081203D12046FFF718FB00200346E7E72DE9F8430446D0E908354FF000090E468021ED004FEAC30845EA537503F03F035A1C034483F82810382A2AD104F1600304F16802290E2D0203F8011B934245EA18654FEA0828F5D12046FFF7EEFA331D221F243652F8041F0433090E03F8081C518803F8071C1168090A03F8061C116803F8051CB342EDD12046BDE8F843FFF776BF12F03F0702D12046FFF7CEFA7A1C274487F82890C7E7F0B59BB06C4605460E4620461746FFF762FF3A463146FFF788FF29462046FFF7A3FF1BB0F0BD2DE9F04FB3B03C9C80468A4693461D46402C0ED918AE3046FFF74AFF2246294630466D462024FFF76DFF29463046FFF788FF002308AF9C4230D1A1463623B9F1400F31D118AE3046FFF732FF4A4639463046FFF757FF5A4651463046FFF752FF41463046FFF76DFF00239C4221D15C23402C24D13046FFF71BFF224639463046FFF740FF202241463046FFF73BFF41463046FFF756FF33B0BDE8F08FEA5C82F03602FA540133C6E707F8093009F10109C5E7EA5C82F05C02FA540133D5E73B550134D5E7B00E2DE95FF07FF5300F30EE073410E2A33BA0E11A00000A00A0A0E3AA208AE03012A0E1071001E2020051E3110000BA10AF40EE6FF07FF5101F30EE072001E2042082E2A0419FE5A14114E0145F6FE198719FE5A17617E00490A0E119B58AE117B28BE15EBF07EE019059E2FAFFFFAA017057E2F7FFFFAA02A08AE20A0053E1E5FFFFCA00A0A0E310AF40EE4FF07FF56FF07FF5B00EBDE81EFF2FE1B00E2DE95FF07FF5300F30EE073410E2A33BA0E11A00000A00A0A0E3AA208AE03012A0E1071001E2020051E3110000BA10AF40EE6FF07FF5101F30EE072001E2042082E204419FE5A14114E0145F6FE1FC709FE5A17617E00490A0E119B58AE117B28BE156BF07EE019059E2FAFFFFAA017057E2F7FFFFAA02A08AE20A0053E1E5FFFFCA00A0A0E310AF40EE4FF07FF56FF07FF5B00EBDE81EFF2FE1F04F2DE90070A0E100C00FE1C0010CF1020017E30700000A100F11EE040010E30300000A0400C0E3100F01EEA5FFFFEB000000EACAFFFFEB010017E30200000A100F11EE010AC0E3100F01EE0000A0E3150F07EE0CF029E1F08FBDE8F04F2DE90070A0E100C00FE1C0010CF1020017E30600000A100F11EE040010E30300001AB7FFFFEB100F11EE040080E3100F01EE010017E30400000A0000A0E3150F07EE100F11EE010A80E3100F01EE0CF029E1F08FBDE8FF030000FF7F000052504D42204B45595341534900767574737271706F6E6D6C6B6A69686766656463626139383736353433323130000000982F8A4291443771CFFBC0B5A5DBB5E95BC25639F111F159A4823F92D55E1CAB98AA07D8015B8312BE853124C37D0C55745DBE72FEB1DE80A706DC9B74F19BC1C1699BE48647BEEFC69DC10FCCA10C246F2CE92DAA84744ADCA9B05CDA88F97652513E986DC631A8C82703B0C77F59BFF30BE0C64791A7D55163CA0667292914850AB72738211B2EFC6D2C4D130D385354730A65BB0A6A762EC9C281852C7292A1E8BFA24B661AA8708B4BC2A3516CC719E892D1240699D685350EF470A06A1016C1A419086C371E4C774827B5BCB034B30C1C394AAAD84E4FCA9C5BF36F2E68EE828F746F63A5781478C8840802C78CFAFFBE90EB6C50A4F7A3F9BEF27871C66476EEF0F16B3047E9793158F642DA46F73B53FDC5F884CE037315BC5447D46A22222222333333334444444477777777666666665555555511111111";
		object result = null;
		byte[] array = hex.HexToBytes();
		int num = utils.FindBinary(da2, new byte[6] { 56, 181, 5, 70, 12, 32 });
		int num2 = utils.FindBinary(da2, new byte[5] { 75, 79, 244, 60, 114 });
		if (num2 == -1)
		{
			num2 = utils.FindBinary(da2, new byte[10] { 163, 235, 0, 19, 24, 26, 2, 235, 0, 16 });
			if (num2 != -1)
			{
				num2 -= 10;
			}
		}
		else
		{
			num2--;
		}
		int pos = 0;
		int num3 = 0;
		do
		{
			num3 = utils.FindBinary(da2, new byte[6] { 195, 105, 10, 70, 16, 181 }, pos);
			if (num3 == -1)
			{
				break;
			}
			pos = num3 + 1;
		}
		while (da2[num3 + 20] != 179 || da2[num3 + 21] != 33);
		if (num3 == -1)
		{
			num3 = utils.FindBinary(da2, new byte[6] { 195, 105, 19, 240, 1, 3 });
		}
		int num4 = utils.FindBinary(da2, new byte[8] { 248, 181, 6, 70, 157, 248, 24, 80 });
		if (num4 == -1)
		{
			num4 = utils.FindBinary(da2, new byte[8] { 45, 233, 240, 65, 79, 246, 253, 116 });
		}
		int num5 = utils.FindBinary(array, new byte[4] { 17, 17, 17, 17 });
		int num6 = utils.FindBinary(array, "\"\"\"\""u8);
		int index = utils.FindBinary(array, "3333"u8);
		int index2 = utils.FindBinary(array, "DDDD"u8);
		int index3 = utils.FindBinary(array, "UUUU"u8);
		int index4 = utils.FindBinary(array, "ffff"u8);
		int index5 = utils.FindBinary(array, "wwww"u8);
		int num7 = 0;
		int num8 = utils.FindBinary(da2, new byte[10] { 32, 70, 11, 176, 189, 232, 240, 131, 0, 191 });
		if (num8 == -1)
		{
			num8 = utils.FindBinary(da2, new byte[10] { 33, 70, 2, 240, 2, 251, 27, 230, 0, 191 });
		}
		num7 = ((num8 == -1) ? BitConverter.ToInt32(da2, num8 + 10) : BitConverter.ToInt32(da2, num8 + 10 + 8));
		int num9 = 0;
		int num10 = 0;
		if (num8 != -1)
		{
			ReadOnlySpan<byte> data = da2;
			byte[] obj = new byte[5] { 181, 0, 177, 144, 248 };
			obj[1] = (byte)Strings.AscW('.');
			num9 = utils.FindBinary(data, obj);
			num10 = utils.FindBinary(da2, new byte[6] { 45, 233, 248, 67, 1, 39 });
		}
		else
		{
			num7 = 0;
			num9 = 0;
			num10 = 0;
		}
		if (num5 != -1 && num6 != -1)
		{
			num = (int)((num != 0) ? ((num + da2address) | 1L) : 0);
			num2 = (int)((num2 != 0) ? ((num2 + da2address) | 1L) : 0);
			num3 = (int)((num3 != 0) ? ((num3 + da2address) | 1L) : 0);
			num4 = (int)((num4 != 0) ? ((num4 + da2address) | 1L) : 0);
			num9 = (int)((num9 != 0) ? ((num9 + (da2address - 1L)) | 1L) : 0);
			num10 = (int)((num10 != 0) ? ((num10 + da2address) | 1L) : 0);
			if (num7 == 0)
			{
				num7 = 0;
			}
			BitConverter.GetBytes(num).CopyTo(array, num5);
			BitConverter.GetBytes(num2).CopyTo(array, num6);
			BitConverter.GetBytes(num3).CopyTo(array, index);
			BitConverter.GetBytes(num4).CopyTo(array, index2);
			BitConverter.GetBytes(num9).CopyTo(array, index4);
			BitConverter.GetBytes(num10).CopyTo(array, index3);
			BitConverter.GetBytes(num7).CopyTo(array, index5);
			return array;
		}
		return (byte[])result;
	}

	public byte[] PatchDA1(byte[] da1)
	{
		string value = "Patching da1... ";
		string value2 = "Error on patching da1 version check... ";
		Console.WriteLine(value);
		byte[] input = da1.ToArray();
		input = Mtk.PatchPreloaderSecurityDA1(input);
		int num = da1.IndexOf(new byte[10] { 31, 181, 0, 35, 1, 168, 0, 147, 0, 240 });
		if (num != -1)
		{
			input = input.ToArray();
			((Array)(object)new byte[4] { 0, 32, 112, 71 }).CopyTo((Array)input, num);
			return input;
		}
		Console.WriteLine(value2);
		return input;
	}

	public byte[] PatchDA2(byte[] da2)
	{
		string value = "Patching da2... ";
		string s = "[oplus]";
		string s2 = "[OPPO]";
		string value2 = "Oppo g_oppo_auth_status patched.";
		string value3 = "Hash binding not patched.";
		string value4 = "Hash check not patched.";
		string value5 = "Security check patched";
		string value6 = "DA version anti-rollback patched";
		string value7 = "SBC patched to be disabled";
		string value8 = "DA2 sla patched to be disabled";
		string value9 = "Register read/write not allowed patched";
		string value10 = "Write not allowed not patched.";
		da2 = Mtk.PatchPreloaderSecurityDA2(da2);
		Console.WriteLine(value);
		int num = 0;
		byte[] array = da2.ToArray();
		int num2 = utils.FindBinary(da2, new byte[6] { 1, 43, 3, 209, 1, 35 });
		if (num2 != -1)
		{
			((Array)(object)new byte[4]).CopyTo((Array)array, num2);
		}
		if (utils.FindBinary(da2, Encoding.ASCII.GetBytes(s)) != -1 || utils.FindBinary(da2, Encoding.ASCII.GetBytes(s2)) != -1)
		{
			bool flag = false;
			int num3 = utils.FindBinary(da2, new byte[8] { 10, 0, 0, 224, 46, 0, 0, 224 });
			if (num3 != -1)
			{
				int num4 = Binary.ReadI32LE(array, num3 - 4);
				int num5 = num4 - (int)Mtk.DaLoader.Dacfg.Da.Region[2].StartAddr;
				if (Binary.ReadI32LE(array, num5) == 3)
				{
					array[num5] = 1;
					Console.WriteLine(value2);
					flag = true;
				}
			}
			if (!flag)
			{
				num3 = utils.FindBinary(da2, new byte[17]
				{
					255, 255, 255, 255, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 3
				});
				if (num3 != -1)
				{
					array[num3 + 16] = 1;
					Console.WriteLine(value2);
				}
				else
				{
					num3 = utils.FindBinary(da2, new byte[17]
					{
						99, 136, 116, 24, 0, 0, 0, 0, 0, 0,
						0, 0, 0, 0, 0, 0, 3
					});
					if (num3 != -1)
					{
						array[num3 + 16] = 1;
						Console.WriteLine(value2);
					}
				}
			}
			num3 = 0;
			num = 0;
			while (num3 != -1)
			{
				num3 = utils.FindBinary(da2, new byte[6] { 1, 59, 1, 43, 8, 217 }, num);
				if (num3 != -1)
				{
					((Array)(object)new byte[4] { 1, 32, 8, 189 }).CopyTo((Array)array, num3);
					num = num3 + 1;
				}
			}
		}
		int num6 = utils.FindBinary(da2, new byte[10] { 1, 35, 3, 96, 0, 32, 112, 71, 112, 181 });
		if (num6 != -1)
		{
			array[num6] = 0;
		}
		else
		{
			num6 = utils.FindBinary(da2, new byte[16]
			{
				1, 16, 160, 227, 0, 16, 128, 229, 0, 0,
				160, 227, 30, 255, 47, 225
			});
			if (num6 != -1)
			{
				array[num6] = 0;
				int num7 = utils.FindBinary(da2, new byte[16]
				{
					240, 79, 45, 233, 28, 176, 141, 226, 84, 208,
					77, 226, 0, 80, 160, 225
				});
				if (num7 != -1)
				{
					byte[] array2 = new byte[8] { 0, 0, 160, 227, 30, 255, 47, 225 };
					array2.CopyTo(array, num7);
					int num8 = utils.FindBinary(da2, new byte[12]
					{
						1, 80, 130, 225, 122, 0, 0, 10, 8, 0,
						160, 225
					});
					if (num8 != -1)
					{
						byte[] array3 = new byte[8] { 0, 80, 160, 227, 122, 0, 0, 234 };
						array[num8 + array3.Length] = array3[0];
					}
				}
			}
			else
			{
				Console.WriteLine(value3);
			}
		}
		int num9 = utils.FindBinary(da2, BitConverter.GetBytes(3221684228u));
		if (num9 != -1)
		{
			BitConverter.GetBytes(0).CopyTo(array, num9);
		}
		else
		{
			num9 = utils.FindBinary(da2, new byte[8] { 79, 240, 4, 9, 204, 242, 7, 9 });
			if (num9 != -1)
			{
				((Array)(object)new byte[8] { 79, 240, 0, 9, 79, 240, 0, 9 }).CopyTo((Array)array, num9);
			}
			else
			{
				num9 = utils.FindBinary(da2, new byte[14]
				{
					79, 240, 4, 9, 50, 70, 1, 152, 3, 153,
					204, 242, 7, 9
				});
				if (num9 != -1)
				{
					((Array)(object)new byte[14]
					{
						79, 240, 0, 9, 50, 70, 1, 152, 3, 153,
						79, 240, 0, 9
					}).CopyTo((Array)array, num9);
				}
				else
				{
					Console.WriteLine(value4);
				}
			}
		}
		int num10 = utils.FindBinary(da2, new byte[10] { 1, 35, 3, 96, 0, 32, 112, 71, 112, 181 });
		if (num10 != -1)
		{
			((Array)(object)new byte[2] { 0, 35 }).CopyTo((Array)array, num10);
			Console.WriteLine(value5);
		}
		int num11 = utils.FindBinary(da2, BitConverter.GetBytes(3221356627u));
		if (num11 != -1)
		{
			BitConverter.GetBytes(0).CopyTo(array, num11);
			Console.WriteLine(value6);
		}
		int num12 = utils.FindBinary(da2, new byte[10] { 2, 75, 24, 104, 192, 243, 64, 0, 112, 71 });
		if (num12 != -1)
		{
			((Array)(object)new byte[4] { 79, 240, 0, 0 }).CopyTo((Array)array, num12 + 4);
			Console.WriteLine(value7);
		}
		int num13 = utils.FindBinary(da2, new byte[8] { 1, 0, 1, 192, 1, 32, 112, 71 });
		if (num13 != -1)
		{
			((Array)(object)new byte[2] { 0, 32 }).CopyTo((Array)array, num13 + 4);
			Console.WriteLine(value8);
		}
		int num14 = utils.FindBinary(da2, BitConverter.GetBytes(3221487629u));
		if (num14 != -1)
		{
			BitConverter.GetBytes(0).CopyTo(array, num14);
			Console.WriteLine(value9);
		}
		int num15 = 0;
		bool flag2 = false;
		while (num15 != -1)
		{
			num15 = utils.FindBinary(array, new byte[8] { 55, 181, 0, 35, 4, 70, 2, 168 });
			if (num15 != -1)
			{
				((Array)(object)new byte[8] { 55, 181, 0, 32, 3, 176, 48, 189 }).CopyTo((Array)array, num15);
				flag2 = true;
				continue;
			}
			num15 = utils.FindBinary(array, new byte[6] { 12, 35, 204, 242, 2, 3 });
			if (num15 != -1)
			{
				((Array)(object)new byte[6] { 0, 35, 0, 35, 0, 35 }).CopyTo((Array)array, num15);
				int num16 = utils.FindBinary(array, new byte[6] { 42, 35, 204, 242, 2, 3 });
				if (num16 != -1)
				{
					((Array)(object)new byte[6] { 0, 35, 0, 35, 0, 35 }).CopyTo((Array)array, num16);
				}
				flag2 = true;
			}
		}
		if (!flag2)
		{
			Console.WriteLine(value10);
		}
		return array;
	}

	public byte[] XRead()
	{
		return XFlash.XRead();
	}

	public int ReadStatus()
	{
		return XFlash.GetStatus();
	}

	public bool XSend(object data, bool is64Bit = false)
	{
		return XFlash.XSend(data, (DataType)1, is64Bit);
	}

	public static bool IsNumber(object value)
	{
		object obj = null;
		if (value is sbyte)
		{
			bool flag = true;
			return true;
		}
		if (!(value is byte))
		{
			if (!(value is short))
			{
				if (!(value is ushort))
				{
					if (!(value is int))
					{
						if (!(value is uint))
						{
							if (!(value is long))
							{
								if (!(value is ulong))
								{
									if (!(value is float))
									{
										if (!(value is double))
										{
											return ((value is decimal) ? value : null) !=null;
										}
										return true;
									}
									return true;
								}
								return true;
							}
							return true;
						}
						return true;
					}
					return true;
				}
				return true;
			}
			return true;
		}
		return true;
	}

	public hwcrypto cryptosetup()
	{
		CryptoSetup cryptoSetup = new CryptoSetup();
		cryptoSetup.blacklist = Mtk.Config.Chipcfg.Blacklist;
		cryptoSetup.gcpu_base = (int)Mtk.Config.Chipcfg.GcpuBase;
		cryptoSetup.dxcc_base = (int)Mtk.Config.Chipcfg.DxccBase;
		cryptoSetup.sej_base = (int)Mtk.Config.Chipcfg.SejBase;
		cryptoSetup.hwcode = Mtk.Config.Chipcfg.HwCode;
		cryptoSetup.da_payload_addr = (int)Mtk.Config.Chipcfg.DaPayloadAddr;
		cryptoSetup.write32 = write_register;
		cryptoSetup.writemem = writemem;
		cryptoSetup.read32 = readmem;
		return new hwcrypto(cryptoSetup);
	}

	public List<int> readmem(int addr, int dwords = 1)
	{
		List<int> list = new List<int>();
		if (dwords < 32)
		{
			int i = 0;
			for (int num = dwords - 1; i <= num; i++)
			{
				byte[] array = custom_read_register(addr + i * 4);
				if (array.Length != 0)
				{
					int item = BitConverter.ToInt32(array, 0);
					if (dwords == 1)
					{
						return new List<int> { item };
					}
					list.Add(item);
					continue;
				}
				return new List<int>();
			}
		}
		else
		{
			byte[] resdata = custom_read(addr, dwords * 4);
			list = (from num2 in Enumerable.Range(0, resdata.Length / 4)
				select BitConverter.ToInt32(resdata, num2 * 4)).ToList();
		}
		return list;
	}

	public bool writemem(int addr, byte[] data)
	{
		int i = 0;
		for (int num = data.Length - 1; i <= num; i += 4)
		{
			byte[] array = data.Skip(i).Take(4).ToArray();
			while (array.Length < 4)
			{
				array = array.Concat(new byte[1]).ToArray();
			}
			write_register(addr + i, new List<int> { BitConverter.ToInt32(array, 0) });
		}
		return true;
	}

	public unsafe bool write_register(int addr, List<int> dwords)
	{
		object obj = null;
		int result = 1;
		int num = 0;
		if (dwords.Count < 32)
		{
			foreach (int dword in dwords)
			{
				if (!custom_write_register(addr + num, BitConverter.GetBytes(dword)))
				{
					return false;
				}
				num += 4;
			}
		}
		else
		{
			byte[] data = dwords.SelectMany(new Func<int, IEnumerable<byte>>(BitConverter.GetBytes)).ToArray();
			custom_write(addr, data);
		}
		return (byte)result != 0;
	}

	public byte[] custom_read_register(int addr)
	{
		if (!SendCommand(XCmd.READREGISTER))
		{
			return new byte[0];
		}
		if (XSend(BitConverter.GetBytes(addr)))
		{
			byte[] array = XRead();
			if (ReadStatus() != 0)
			{
				return Array.Empty<byte>();
			}
			return array ?? Array.Empty<byte>();
		}
		return new byte[0];
	}

	public byte[] custom_read(int addr, int length)
	{
		int num = 0;
		byte[] array = new byte[length];
		bool flag = 0 < length;
		while (true)
		{
			if (!flag)
			{
				return array.Take(length).ToArray();
			}
			if (SendCommand(XCmd.RPMB_READ_UFS))
			{
				break;
			}
			flag = 0 < length;
		}
		XSend(addr);
		int num2 = Math.Min(length, 65536);
		XSend(num2);
		byte[] array2 = XRead();
		Array.Copy(array2, 0, array, 0, array2.Length);
		num = 0 + array2.Length;
		int num3 = 1616566548;
		while (true)
		{
			_ = num3 ^ 0x51B3A05B;
			if (ReadStatus() != 0)
			{
				break;
			}
			do
			{
				if (!(flag = num < length))
				{
					return array.Take(length).ToArray();
				}
			}
			while (!SendCommand(XCmd.RPMB_READ_UFS));
			XSend(addr + num);
			num2 = Math.Min(length, 65536);
			XSend(num2);
			array2 = XRead();
			Array.Copy(array2, 0, array, num, array2.Length);
			num += array2.Length;
			num3 = 1616566548;
		}
		return array.Take(length).ToArray();
	}

	public bool custom_write_register(int addr, byte[] data)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (!SendCommand(XCmd.WRITEREGISTER))
		{
			return false;
		}
		if (XSend(BitConverter.GetBytes(addr)))
		{
			if (XSend(data))
			{
				if (ReadStatus() == 0)
				{
					return false;
				}
				return true;
			}
			return false;
		}
		return false;
	}

	public bool custom_write(int addr, byte[] data)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (SendCommand(XCmd.WRITEREGISTER))
		{
			return false;
		}
		if (!XSend(BitConverter.GetBytes(addr)))
		{
			return false;
		}
		if (XSend(BitConverter.GetBytes(data.Length)))
		{
			if (XSend(data))
			{
				if (ReadStatus() != 0)
				{
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool SendCommand(XCmd cmd)
	{
		if (XSend(65545u))
		{
			if (ReadStatus() == 0)
			{
				if (!XSend((uint)cmd))
				{
					return false;
				}
				uint num = (uint)ReadStatus();
				if (num != 0)
				{
					Console.WriteLine(Error.status(num));
					return false;
				}
				return true;
			}
			return false;
		}
		return false;
	}

	public (bool Success, byte[] Data) Seccfg(byte[] seccfgData, string unlock)
	{
		string value = "start";
		object obj = null;
		string s = "AND_SECCFG_v";
		string value2 = "parse";
		string msg = "Failed";
		string value3 = "ok parse";
		string text = "hasil 1 :";
		string s2 = "MMMM";
		string value4 = "parse ok";
		hwcrypto hwc = cryptosetup();
		Console.WriteLine(value);
		bool flag = false;
		byte[] array = (byte[])obj;
		if (seccfgData.Take(12).SequenceEqual(Encoding.ASCII.GetBytes(s)))
		{
			seccfgV3 seccfgV = new seccfgV3(hwc);
			Console.WriteLine(value2);
			if (seccfgV.parse(seccfgData))
			{
				Console.WriteLine(value3);
				(flag, array) = seccfgV.create(unlock);
				Console.WriteLine(text + flag);
				if (!flag)
				{
					return (Success: false, Data: array);
				}
				return (Success: true, Data: array);
			}
			UI.RichLogs(msg, Color.Tomato, nextline: true, bold: true);
			return (Success: false, Data: (byte[])obj);
		}
		if (!seccfgData.Take(4).SequenceEqual(Encoding.ASCII.GetBytes(s2)))
		{
			UI.RichLogs(msg, Color.Tomato, nextline: true, bold: true);
			return (Success: false, Data: (byte[])obj);
		}
		seccfgV4 seccfgV2 = new seccfgV4(hwc);
		Console.WriteLine(value2);
		if (seccfgV2.parse(seccfgData))
		{
			Console.WriteLine(value4);
			(flag, array) = seccfgV2.create(unlock);
			Console.WriteLine(text + flag);
			if (!flag)
			{
				return (Success: false, Data: array);
			}
			return (Success: true, Data: array);
		}
		UI.RichLogs(msg, Color.Tomato, nextline: true, bold: true);
		return (Success: false, Data: (byte[])obj);
	}

	public byte[] CustomRPMBRead(int sector, int sectors)
	{
		string format = "Error on sector 0x{0:X}: {1}";
		XCmd cmd = XCmd.RPMB_READ_UFS;
		if (sector == 256)
		{
			cmd = XCmd.RPMB_READ_UFS;
		}
		List<byte> list = new List<byte>();
		if (SendCommand(cmd))
		{
			if (XSend((uint)sector))
			{
				if (XSend((uint)sectors))
				{
					int num = 0;
					byte[] array;
					while (true)
					{
						if (num >= sectors)
						{
							if (ReadStatus() == 0)
							{
								return list.ToArray();
							}
							return Array.Empty<byte>();
						}
						array = XRead();
						if (array == null || array.Length == 0)
						{
							return Array.Empty<byte>();
						}
						if (array.Length != 256)
						{
							break;
						}
						list.AddRange(array);
						num++;
					}
					uint code = ((array.Length >= 4) ? Binary.ReadU32LE(array) : Binary.ReadU16LE(array));
					Console.WriteLine(string.Format(format, sector + num, GetRpmbError(code)));
					ReadStatus();
					return Array.Empty<byte>();
				}
				return Array.Empty<byte>();
			}
			return Array.Empty<byte>();
		}
		return Array.Empty<byte>();
	}

	public bool ReadRPMB(string savePath)
	{
		string text = "emmc";
		string text2 = "ufs";
		double field_ = Class38.field_543;
		int num = 0;
		if (Mtk.DaLoader.Dacfg.FlashType == text)
		{
			num = (int)(XFlash.Emmc.RpmbSize / 256L);
		}
		else if (Mtk.DaLoader.Dacfg.FlashType == text2)
		{
			num = 131072;
		}
		if (num <= 0)
		{
			return false;
		}
		using FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
		int num2 = 0;
		int num3 = num;
		Stopwatch stopwatch = Stopwatch.StartNew();
		while (true)
		{
			if (num3 > 0)
			{
				int num4 = Math.Min(num - num2, 16);
				byte[] array = CustomRPMBRead(0 + num2, num4);
				if (array.Length == 0)
				{
					break;
				}
				fileStream.Write(array, 0, array.Length);
				num2 += num4;
				num3 -= num4;
				UI.Progresbar1(num2, num);
				double totalSeconds = stopwatch.Elapsed.TotalSeconds;
				if (totalSeconds > field_)
				{
					UI.LabelWrite(utils.FileSize((double)fileStream.Length / totalSeconds), utils.FileSize(fileStream.Length), utils.FileSize(num * 256L));
				}
				continue;
			}
			stopwatch.Stop();
			return true;
		}
		return false;
	}

	public bool Read_RPMB(string filename = null)
	{
		string text = "emmc";
		string text2 = "ufs";
		string text3 = "sectors = ";
		double field_ = Class38.field_543;
		int num = 0;
		if (!(Mtk.DaLoader.Dacfg.FlashType == text))
		{
			if (Mtk.DaLoader.Dacfg.FlashType == text2)
			{
				num = 131072;
			}
		}
		else
		{
			num = (int)(XFlash.Emmc.RpmbSize / 256L);
		}
		if (num > 0)
		{
			num = 512;
			int num2 = 0;
			Console.WriteLine(text3 + num);
			using FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
			int num3 = num;
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (true)
			{
				if (num3 > 0)
				{
					int num4 = Math.Min(num - num2, 16);
					byte[] array = custom_rpmb_read(0 + num2, num4);
					if (array.Length == 0)
					{
						break;
					}
					fileStream.Write(array, 0, array.Length);
					num2 += num4;
					num3 -= num4;
					UI.Progresbar1(num2, num);
					double totalSeconds = stopwatch.Elapsed.TotalSeconds;
					if (totalSeconds > field_)
					{
						UI.LabelWrite(utils.FileSize((double)fileStream.Length / totalSeconds), utils.FileSize(fileStream.Length), utils.FileSize(num * 256L));
					}
					continue;
				}
				stopwatch.Stop();
				return true;
			}
			return false;
		}
		return false;
	}

	public byte[] custom_rpmb_read(int sector, int sectors)
	{
		string text = "emmc";
		string text2 = "ufs";
		string value = "sectors failed";
		string value2 = "proses xsendz sector";
		string value3 = "proses XRead ok";
		if (!(Mtk.DaLoader.Dacfg.FlashType == text))
		{
			if (Mtk.DaLoader.Dacfg.FlashType == text2)
			{
				sectors = 131072;
			}
		}
		else
		{
			sectors = (int)(XFlash.Emmc.RpmbSize / 256L);
		}
		if (sectors > 0)
		{
			if (sector != 256)
			{
			}
			if (cmds(983049u))
			{
				Console.WriteLine(value2);
				xsendz((uint)sector);
				byte[] value4 = xreadx();
				if (BitConverter.ToUInt16(value4, 0) == 0)
				{
					byte[] result = xreadx();
					if (ReadStatus() == 0)
					{
						Console.WriteLine(value3);
						return result;
					}
				}
			}
			return Array.Empty<byte>();
		}
		Console.WriteLine(value);
		return Array.Empty<byte>();
	}

	public bool cmds(uint cmd_Conflict)
	{
		string value = "proses cmds 1";
		string value2 = "proses DEVICE_CTRL";
		string value3 = "proses 2 cmds ";
		string value4 = "proses cmd_Conflict";
		string value5 = "proses filedddd cmd_Conflict";
		Console.WriteLine(value);
		if (XSend(65545u))
		{
			Console.WriteLine(value2);
			if (ReadStatus() == 0)
			{
				Console.WriteLine(value3);
				bool flag;
				if (XSend(cmd_Conflict))
				{
					Console.WriteLine(value4);
					if (ReadStatus() == 0)
					{
						Console.WriteLine(value5);
						flag = true;
						return true;
					}
				}
				flag = false;
				return false;
			}
			return false;
		}
		return false;
	}

	public static bool xsendz(object data, int datatype = 1)
	{
		string value = "proses DT_PROTOCOL_FLOW";
		string fmt = "<I";
		string fmt2 = "<III";
		string value2 = "proses Write tmp";
		string value3 = "proses Write tmp done";
		Console.WriteLine(value);
		byte[] array;
		int num;
		byte[] data2;
		bool result;
		if (!IsNumber(data))
		{
			array = (byte[])data;
			num = array.Length;
			data2 = StructConverter.Pack(fmt2, new object[3] { 4277071599u, datatype, num });
			Console.WriteLine(value2);
			if (!SerialMTK.Write(data2))
			{
				return false;
			}
			result = SerialMTK.Write(array);
			Console.WriteLine(value3);
			return result;
		}
		array = StructConverter.Pack(fmt, new object[1] { data });
		num = 4;
		data2 = StructConverter.Pack(fmt2, new object[3] { 4277071599u, datatype, 4 });
		Console.WriteLine(value2);
		if (!SerialMTK.Write(data2))
		{
			return false;
		}
		result = SerialMTK.Write(array);
		Console.WriteLine(value3);
		return result;
	}

	public byte[] xreadx()
	{
		string fmt = "<III";
		string msg = "Wrong magic";
		object result = null;
		string text = "Error : ";
		try
		{
			byte[] bytes = SerialMTK.Read(12);
			object[] array = StructConverter.Unpack(fmt, bytes);
			uint num = (uint)array[0];
			int len = (int)array[2];
			if (num == 4277071599u)
			{
				return SerialMTK.Read(len);
			}
			UI.RichLogs(msg, Color.Tomato, nextline: true, bold: true);
			return (byte[])result;
		}
		catch (Exception ex)
		{
			Console.WriteLine(text + ex.Message);
			return (byte[])result;
		}
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
