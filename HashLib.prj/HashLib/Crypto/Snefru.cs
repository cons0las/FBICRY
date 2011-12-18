﻿using System;

namespace HashLib.Crypto
{
	class Snefru_4_128 : Snefru
	{
		public Snefru_4_128()
			: base(4, HashLib.HashSize.HashSize128)
		{
		}
	}

	class Snefru_4_256 : Snefru
	{
		public Snefru_4_256()
			: base(4, HashLib.HashSize.HashSize256)
		{
		}
	}

	class Snefru_8_128 : Snefru
	{
		public Snefru_8_128()
			: base(8, HashLib.HashSize.HashSize128)
		{
		}
	}

	class Snefru_8_256 : Snefru
	{
		public Snefru_8_256()
			: base(8, HashLib.HashSize.HashSize256)
		{
		}
	}

	abstract class Snefru : HashCryptoNotBuildIn
	{
		#region Consts

		private static readonly int[] s_shifts = new int[4] {16, 8, 16, 24};

		private static readonly uint[][] s_boxes = new[]
		                                           	{
		                                           		new uint[]
		                                           			{
		                                           				0x64F9001B, 0xFEDDCDF6, 0x7C8FF1E2, 0x11D71514, 0x8B8C18D3,
		                                           				0xDDDF881E, 0x6EAB5056, 0x88CED8E1, 0x49148959, 0x69C56FD5,
		                                           				0xB7994F03, 0x0FBCEE3E, 0x3C264940, 0x21557E58, 0xE14B3FC2,
		                                           				0x2E5CF591, 0xDCEFF8CE, 0x092A1648, 0xBE812936, 0xFF7B0C6A,
		                                           				0xD5251037, 0xAFA448F1, 0x7DAFC95A, 0x1EA69C3F, 0xA417ABE7,
		                                           				0x5890E423, 0xB0CB70C0, 0xC85025F7, 0x244D97E3, 0x1FF3595F,
		                                           				0xC4EC6396, 0x59181E17, 0xE635B477, 0x354E7DBF, 0x796F7753,
		                                           				0x66EB52CC, 0x77C3F995, 0x32E3A927, 0x80CCAED6, 0x4E2BE89D,
		                                           				0x375BBD28, 0xAD1A3D05, 0x2B1B42B3, 0x16C44C71, 0x4D54BFA8,
		                                           				0xE57DDC7A, 0xEC6D8144, 0x5A71046B, 0xD8229650, 0x87FC8F24,
		                                           				0xCBC60E09, 0xB6390366, 0xD9F76092, 0xD393A70B, 0x1D31A08A,
		                                           				0x9CD971C9, 0x5C1EF445, 0x86FAB694, 0xFDB44165, 0x8EAAFCBE,
		                                           				0x4BCAC6EB, 0xFB7A94E5, 0x5789D04E, 0xFA13CF35, 0x236B8DA9,
		                                           				0x4133F000, 0x6224261C, 0xF412F23B, 0xE75E56A4, 0x30022116,
		                                           				0xBAF17F1F, 0xD09872F9, 0xC1A3699C, 0xF1E802AA, 0x0DD145DC,
		                                           				0x4FDCE093, 0x8D8412F0, 0x6CD0F376, 0x3DE6B73D, 0x84BA737F,
		                                           				0xB43A30F2, 0x44569F69, 0x00E4EACA, 0xB58DE3B0, 0x959113C8,
		                                           				0xD62EFEE9, 0x90861F83, 0xCED69874, 0x2F793CEE, 0xE8571C30,
		                                           				0x483665D1, 0xAB07B031, 0x914C844F, 0x15BF3BE8, 0x2C3F2A9A,
		                                           				0x9EB95FD4, 0x92E7472D, 0x2297CC5B, 0xEE5F2782, 0x5377B562,
		                                           				0xDB8EBBCF, 0xF961DEDD, 0xC59B5C60, 0x1BD3910D, 0x26D206AD,
		                                           				0xB28514D8, 0x5ECF6B52, 0x7FEA78BB, 0x504879AC, 0xED34A884,
		                                           				0x36E51D3C, 0x1753741D, 0x8C47CAED, 0x9D0A40EF, 0x3145E221,
		                                           				0xDA27EB70, 0xDF730BA3, 0x183C8789, 0x739AC0A6, 0x9A58DFC6,
		                                           				0x54B134C1, 0xAC3E242E, 0xCC493902, 0x7B2DDA99, 0x8F15BC01,
		                                           				0x29FD38C7, 0x27D5318F, 0x604AAFF5, 0xF29C6818, 0xC38AA2EC,
		                                           				0x1019D4C3, 0xA8FB936E, 0x20ED7B39, 0x0B686119, 0x89A0906F,
		                                           				0x1CC7829E, 0x9952EF4B, 0x850E9E8C, 0xCD063A90, 0x67002F8E,
		                                           				0xCFAC8CB7, 0xEAA24B11, 0x988B4E6C, 0x46F066DF, 0xCA7EEC08,
		                                           				0xC7BBA664, 0x831D17BD, 0x63F575E6, 0x9764350E, 0x47870D42,
		                                           				0x026CA4A2, 0x8167D587, 0x61B6ADAB, 0xAA6564D2, 0x70DA237B,
		                                           				0x25E1C74A, 0xA1C901A0, 0x0EB0A5DA, 0x7670F741, 0x51C05AEA,
		                                           				0x933DFA32, 0x0759FF1A, 0x56010AB8, 0x5FDECB78, 0x3F32EDF8,
		                                           				0xAEBEDBB9, 0x39F8326D, 0xD20858C5, 0x9B638BE4, 0xA572C80A,
		                                           				0x28E0A19F, 0x432099FC, 0x3A37C3CD, 0xBF95C585, 0xB392C12A,
		                                           				0x6AA707D7, 0x52F66A61, 0x12D483B1, 0x96435B5E, 0x3E75802B,
		                                           				0x3BA52B33, 0xA99F51A5, 0xBDA1E157, 0x78C2E70C, 0xFCAE7CE0,
		                                           				0xD1602267, 0x2AFFAC4D, 0x4A510947, 0x0AB2B83A, 0x7A04E579,
		                                           				0x340DFD80, 0xB916E922, 0xE29D5E9B, 0xF5624AF4, 0x4CA9D9AF,
		                                           				0x6BBD2CFE, 0xE3B7F620, 0xC2746E07, 0x5B42B9B6, 0xA06919BC,
		                                           				0xF0F2C40F, 0x72217AB5, 0x14C19DF3, 0xF3802DAE, 0xE094BEB4,
		                                           				0xA2101AFF, 0x0529575D, 0x55CDB27C, 0xA33BDDB2, 0x6528B37D,
		                                           				0x740C05DB, 0xE96A62C4, 0x40782846, 0x6D30D706, 0xBBF48E2C,
		                                           				0xBCE2D3DE, 0x049E37FA, 0x01B5E634, 0x2D886D8D, 0x7E5A2E7E,
		                                           				0xD7412013, 0x06E90F97, 0xE45D3EBA, 0xB8AD3386, 0x13051B25,
		                                           				0x0C035354, 0x71C89B75, 0xC638FBD0, 0x197F11A1, 0xEF0F08FB,
		                                           				0xF8448651, 0x38409563, 0x452F4443, 0x5D464D55, 0x03D8764C,
		                                           				0xB1B8D638, 0xA70BBA2F, 0x94B3D210, 0xEB6692A7, 0xD409C2D9,
		                                           				0x68838526, 0xA6DB8A15, 0x751F6C98, 0xDE769A88, 0xC9EE4668,
		                                           				0x1A82A373, 0x0896AA49, 0x42233681, 0xF62C55CB, 0x9F1C5404,
		                                           				0xF74FB15C, 0xC06E4312, 0x6FFE5D72, 0x8AA8678B, 0x337CD129,
		                                           				0x8211CEFD
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x074A1D09, 0x52A10E5A, 0x9275A3F8, 0x4B82506C, 0x37DF7E1B,
		                                           				0x4C78B3C5, 0xCEFAB1DA, 0xF472267E, 0xB63045F6, 0xD66A1FC0,
		                                           				0x400298E3, 0x27E60C94, 0x87D2F1B8, 0xDF9E56CC, 0x45CD1803,
		                                           				0x1D35E098, 0xCCE7C736, 0x03483BF1, 0x1F7307D7, 0xC6E8F948,
		                                           				0xE613C111, 0x3955C6FF, 0x1170ED7C, 0x8E95DA41, 0x99C31BF4,
		                                           				0xA4DA8021, 0x7B5F94FB, 0xDD0DA51F, 0x6562AA77, 0x556BCB23,
		                                           				0xDB1BACC6, 0x798040B9, 0xBFE5378F, 0x731D55E6, 0xDAA5BFEE,
		                                           				0x389BBC60, 0x1B33FBA4, 0x9C567204, 0x36C26C68, 0x77EE9D69,
		                                           				0x8AEB3E88, 0x2D50B5CE, 0x9579E790, 0x42B13CFC, 0x33FBD32B,
		                                           				0xEE0503A7, 0xB5862824, 0x15E41EAD, 0xC8412EF7, 0x9D441275,
		                                           				0x2FCEC582, 0x5FF483B7, 0x8F3931DF, 0x2E5D2A7B, 0x49467BF9,
		                                           				0x0653DEA9, 0x2684CE35, 0x7E655E5C, 0xF12771D8, 0xBB15CC67,
		                                           				0xAB097CA1, 0x983DCF52, 0x10DDF026, 0x21267F57, 0x2C58F6B4,
		                                           				0x31043265, 0x0BAB8C01, 0xD5492099, 0xACAAE619, 0x944CE54A,
		                                           				0xF2D13D39, 0xADD3FC32, 0xCDA08A40, 0xE2B0D451, 0x9EFE08AE,
		                                           				0xB9D50FD2, 0xEA5CD7FD, 0xC9A749DD, 0x13EA2253, 0x832DEBAA,
		                                           				0x24BE640F, 0xE03E926A, 0x29E01CDE, 0x8BF59F18, 0x0F9D00B6,
		                                           				0xE1238B46, 0x1E7D8E34, 0x93619ADB, 0x76B32F9F, 0xBD972CEC,
		                                           				0xE31FA976, 0xA68FBB10, 0xFB3BA49D, 0x8587C41D, 0xA5ADD1D0,
		                                           				0xF3CF84BF, 0xD4E11150, 0xD9FFA6BC, 0xC3F6018C, 0xAEF10572,
		                                           				0x74A64B2F, 0xE7DC9559, 0x2AAE35D5, 0x5B6F587F, 0xA9E353FE,
		                                           				0xCA4FB674, 0x04BA24A8, 0xE5C6875F, 0xDCBC6266, 0x6BC5C03F,
		                                           				0x661EEF02, 0xED740BAB, 0x058E34E4, 0xB7E946CF, 0x88698125,
		                                           				0x72EC48ED, 0xB11073A3, 0xA13485EB, 0xA2A2429C, 0xFA407547,
		                                           				0x50B76713, 0x5418C37D, 0x96192DA5, 0x170BB04B, 0x518A021E,
		                                           				0xB0AC13D1, 0x0963FA2A, 0x4A6E10E1, 0x58472BDC, 0xF7F8D962,
		                                           				0x979139EA, 0x8D856538, 0xC0997042, 0x48324D7A, 0x447623CB,
		                                           				0x8CBBE364, 0x6E0C6B0E, 0xD36D63B0, 0x3F244C84, 0x3542C971,
		                                           				0x2B228DC1, 0xCB0325BB, 0xF8C0D6E9, 0xDE11066B, 0xA8649327,
		                                           				0xFC31F83E, 0x7DD80406, 0xF916DD61, 0xD89F79D3, 0x615144C2,
		                                           				0xEBB45D31, 0x28002958, 0x56890A37, 0xF05B3808, 0x123AE844,
		                                           				0x86839E16, 0x914B0D83, 0xC506B43C, 0xCF3CBA5E, 0x7C60F5C9,
		                                           				0x22DEB2A0, 0x5D9C2715, 0xC77BA0EF, 0x4F45360B, 0xC1017D8B,
		                                           				0xE45ADC29, 0xA759909B, 0x412CD293, 0xD7D796B1, 0x00C8FF30,
		                                           				0x23A34A80, 0x4EC15C91, 0x714E78B5, 0x47B9E42E, 0x78F3EA4D,
		                                           				0x7F078F5B, 0x346C593A, 0xA3A87A1A, 0x9BCBFE12, 0x3D439963,
		                                           				0xB2EF6D8E, 0xB8D46028, 0x6C2FD5CA, 0x62675256, 0x01F2A2F3,
		                                           				0xBC96AE0A, 0x709A8920, 0xB4146E87, 0x6308B9E2, 0x64BDA7BA,
		                                           				0xAFED6892, 0x6037F2A2, 0xF52969E0, 0x0ADB43A6, 0x82811400,
		                                           				0x90D0BDF0, 0x19C9549E, 0x203F6A73, 0x1ACCAF4F, 0x89714E6D,
		                                           				0x164D4705, 0x67665F07, 0xEC206170, 0x0C2182B2, 0xA02B9C81,
		                                           				0x53289722, 0xF6A97686, 0x140E4179, 0x9F778849, 0x9A88E15D,
		                                           				0x25CADB54, 0xD157F36F, 0x32A421C3, 0xB368E98A, 0x5A92CD0D,
		                                           				0x757AA8D4, 0xC20AC278, 0x08B551C7, 0x849491E8, 0x4DC75AD6,
		                                           				0x697C33BE, 0xBAF0CA33, 0x46125B4E, 0x59D677B3, 0x30D9C8F2,
		                                           				0xD0AF860C, 0x1C7FD0FA, 0xFE0FF72C, 0x5C8D6F43, 0x57FDEC3B,
		                                           				0x6AB6AD97, 0xD22ADF89, 0x18171785, 0x02BFE22D, 0x6DB80917,
		                                           				0x80B216AF, 0xE85E4F9A, 0x7A1C306E, 0x6FC49BF5, 0x3AF7A11C,
		                                           				0x81E215E7, 0x68363FCD, 0x3E9357C8, 0xEF52FD55, 0x3B8BAB4C,
		                                           				0x3C8CF495, 0xBEFCEEBD, 0xFD25B714, 0xC498D83D, 0x0D2E1A8D,
		                                           				0xE9F966AC, 0x0E387445, 0x435419E5, 0x5E7EBEC4, 0xAA90B8D9,
		                                           				0xFF1A3A96
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x4A8FE4E3, 0xF27D99CD, 0xD04A40CA, 0xCB5FF194, 0x3668275A,
		                                           				0xFF4816BE, 0xA78B394C, 0x4C6BE9DB, 0x4EEC38D2, 0x4296EC80,
		                                           				0xCDCE96F8, 0x888C2F38, 0xE75508F5, 0x7B916414, 0x060AA14A,
		                                           				0xA214F327, 0xBE608DAF, 0x1EBBDEC2, 0x61F98CE9, 0xE92156FE,
		                                           				0x4F22D7A3, 0x3F76A8D9, 0x559A4B33, 0x38AD2959, 0xF3F17E9E,
		                                           				0x85E1BA91, 0xE5EBA6FB, 0x73DCD48C, 0xF5C3FF78, 0x481B6058,
		                                           				0x8A3297F7, 0x8F1F3BF4, 0x93785AB2, 0x477A4A5B, 0x6334EB5D,
		                                           				0x6D251B2E, 0x74A9102D, 0x07E38FFA, 0x915C9C62, 0xCCC275EA,
		                                           				0x6BE273EC, 0x3EBDDD70, 0xD895796C, 0xDC54A91B, 0xC9AFDF81,
		                                           				0x23633F73, 0x275119B4, 0xB19F6B67, 0x50756E22, 0x2BB152E2,
		                                           				0x76EA46A2, 0xA353E232, 0x2F596AD6, 0x0B1EDB0B, 0x02D3D9A4,
		                                           				0x78B47843, 0x64893E90, 0x40F0CAAD, 0xF68D3AD7, 0x46FD1707,
		                                           				0x1C9C67EF, 0xB5E086DE, 0x96EE6CA6, 0x9AA34774, 0x1BA4F48A,
		                                           				0x8D01ABFD, 0x183EE1F6, 0x5FF8AA7A, 0x17E4FAAE, 0x303983B0,
		                                           				0x6C08668B, 0xD4AC4382, 0xE6C5849F, 0x92FEFB53, 0xC1CAC4CE,
		                                           				0x43501388, 0x441118CF, 0xEC4FB308, 0x53A08E86, 0x9E0FE0C5,
		                                           				0xF91C1525, 0xAC45BE05, 0xD7987CB5, 0x49BA1487, 0x57938940,
		                                           				0xD5877648, 0xA958727F, 0x58DFE3C3, 0xF436CF77, 0x399E4D11,
		                                           				0xF0A5BFA9, 0xEF61A33B, 0xA64CAC60, 0x04A8D0BA, 0x030DD572,
		                                           				0xB83D320F, 0xCAB23045, 0xE366F2F0, 0x815D008D, 0xC897A43A,
		                                           				0x1D352DF3, 0xB9CC571D, 0x8BF38744, 0x72209092, 0xEBA124EB,
		                                           				0xFB99CE5E, 0x3BB94293, 0x28DA549C, 0xAAB8A228, 0xA4197785,
		                                           				0x33C70296, 0x25F6259B, 0x5C85DA21, 0xDF15BDEE, 0x15B7C7E8,
		                                           				0xE2ABEF75, 0xFCC19BC1, 0x417FF868, 0x14884434, 0x62825179,
		                                           				0xC6D5C11C, 0x0E4705DC, 0x22700DE0, 0xD3D2AF18, 0x9BE822A0,
		                                           				0x35B669F1, 0xC42BB55C, 0x0A801252, 0x115BF0FC, 0x3CD7D856,
		                                           				0xB43F5F9D, 0xC2306516, 0xA1231C47, 0xF149207E, 0x5209A795,
		                                           				0x34B3CCD8, 0x67AEFE54, 0x2C83924E, 0x6662CBAC, 0x5EEDD161,
		                                           				0x84E681AA, 0x5D57D26B, 0xFA465CC4, 0x7E3AC3A8, 0xBF7C0CC6,
		                                           				0xE18A9AA1, 0xC32F0A6F, 0xB22CC00D, 0x3D280369, 0x994E554F,
		                                           				0x68F480D3, 0xADCFF5E6, 0x3A8EB265, 0x83269831, 0xBD568A09,
		                                           				0x4BC8AE6A, 0x69F56D2B, 0x0F17EAC8, 0x772EB6C7, 0x9F41343C,
		                                           				0xAB1D0742, 0x826A6F50, 0xFEA2097C, 0x1912C283, 0xCE185899,
		                                           				0xE4444839, 0x2D8635D5, 0x65D0B1FF, 0x865A7F17, 0x326D9FB1,
		                                           				0x59E52820, 0x0090ADE1, 0x753C7149, 0x9DDD8B98, 0xA5A691DA,
		                                           				0x0D0382BB, 0x8904C930, 0x086CB000, 0x6E69D3BD, 0x24D4E7A7,
		                                           				0x05244FD0, 0x101A5E0C, 0x6A947DCB, 0xE840F77B, 0x7D0C5003,
		                                           				0x7C370F1F, 0x805245ED, 0xE05E3D3F, 0x7906880E, 0xBABFCD35,
		                                           				0x1A7EC697, 0x8C052324, 0x0C6EC8DF, 0xD129A589, 0xC7A75B02,
		                                           				0x12D81DE7, 0xD9BE2A66, 0x1F4263AB, 0xDE73FDB6, 0x2A00680A,
		                                           				0x56649E36, 0x3133ED55, 0x90FA0BF2, 0x2910A02A, 0x949D9D46,
		                                           				0xA0D1DCDD, 0xCFC9B7D4, 0xD2677BE5, 0x95CB36B3, 0x13CD9410,
		                                           				0xDBF73313, 0xB7C6E8C0, 0xF781414B, 0x510B016D, 0xB0DE1157,
		                                           				0xD6B0F62C, 0xBB074ECC, 0x7F1395B7, 0xEE792CF9, 0xEA6FD63E,
		                                           				0x5BD6938E, 0xAF02FC64, 0xDAB57AB8, 0x8EDB3784, 0x8716318F,
		                                           				0x164D1A01, 0x26F26141, 0xB372E6B9, 0xF8FC2B06, 0x7AC00E04,
		                                           				0x3727B89A, 0x97E9BCA5, 0x9C2A742F, 0xBC3B1F7D, 0x7165B471,
		                                           				0x609B4C29, 0x20925351, 0x5AE72112, 0x454BE5D1, 0xC0FFB95F,
		                                           				0xDD0EF919, 0x6F2D70C9, 0x0974C5BF, 0x98AA6263, 0x01D91E4D,
		                                           				0x2184BB6E, 0x70C43C1E, 0x4D435915, 0xAE7B8523, 0xB6FB06BC,
		                                           				0x5431EE76, 0xFDBC5D26, 0xED77493D, 0xC5712EE4, 0xA8380437,
		                                           				0x2EEF261A
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x5A79392B, 0xB8AF32C2, 0x41F7720A, 0x833A61EC, 0x13DFEDAC,
		                                           				0xC4990BC4, 0xDC0F54BC, 0xFEDD5E88, 0x80DA1881, 0x4DEA1AFD,
		                                           				0xFD402CC6, 0xAE67CC7A, 0xC5238525, 0x8EA01254, 0xB56B9BD5,
		                                           				0x862FBD6D, 0xAC8575D3, 0x6FBA3714, 0xDA7EBF46, 0x59CD5238,
		                                           				0x8AC9DBFE, 0x353729FC, 0xE497D7F2, 0xC3AB84E0, 0xF05A114B,
		                                           				0x7B887A75, 0xEDC603DD, 0x5E6FE680, 0x2C84B399, 0x884EB1DA,
		                                           				0x1CB8C8BF, 0xAA51098A, 0xC862231C, 0x8BAC2221, 0x21B387E5,
		                                           				0x208A430D, 0x2A3F0F8B, 0xA5FF9CD2, 0x6012A2EA, 0x147A9EE7,
		                                           				0xF62A501D, 0xB4B2E51A, 0x3EF3484C, 0xC0253C59, 0x2B82B536,
		                                           				0x0AA9696B, 0xBE0C109B, 0xC70B7929, 0xCE3E8A19, 0x2F66950E,
		                                           				0x459F1C2C, 0xE68FB93D, 0xA3C3FF3E, 0x62B45C62, 0x300991CB,
		                                           				0x01914C57, 0x7F7BC06A, 0x182831F5, 0xE7B74BCA, 0xFA50F6D0,
		                                           				0x523CAA61, 0xE3A7CF05, 0xE9E41311, 0x280A21D1, 0x6A4297E1,
		                                           				0xF24DC67E, 0xFC3189E6, 0xB72BF34F, 0x4B1E67AF, 0x543402CE,
		                                           				0x79A59867, 0x0648E02A, 0x00A3AC17, 0xC6208D35, 0x6E7F5F76,
		                                           				0xA45BB4BE, 0xF168FA63, 0x3F4125F3, 0xF311406F, 0x02706565,
		                                           				0xBFE58022, 0x0CFCFDD9, 0x0735A7F7, 0x8F049092, 0xD98EDC27,
		                                           				0xF5C5D55C, 0xE0F201DB, 0x0DCAFC9A, 0x7727FB79, 0xAF43ABF4,
		                                           				0x26E938C1, 0x401B26A6, 0x900720FA, 0x2752D97B, 0xCFF1D1B3,
		                                           				0xA9D9E424, 0x42DB99AB, 0x6CF8BE5F, 0xE82CEBE3, 0x3AFB733B,
		                                           				0x6B734EB6, 0x1036414A, 0x975F667C, 0x049D6377, 0xBA587C60,
		                                           				0xB1D10483, 0xDE1AEFCC, 0x1129D055, 0x72051E91, 0x6946D623,
		                                           				0xF9E86EA7, 0x48768C00, 0xB0166C93, 0x9956BBF0, 0x1F1F6D84,
		                                           				0xFB15E18E, 0x033B495D, 0x56E3362E, 0x4F44C53C, 0x747CBA51,
		                                           				0x89D37872, 0x5D9C331B, 0xD2EF9FA8, 0x254917F8, 0x1B106F47,
		                                           				0x37D75553, 0xB3F053B0, 0x7DCCD8EF, 0xD30EB802, 0x5889F42D,
		                                           				0x610206D7, 0x1A7D34A1, 0x92D87DD8, 0xE5F4A315, 0xD1CF0E71,
		                                           				0xB22DFE45, 0xB901E8EB, 0x0FC0CE5E, 0x2EFA60C9, 0x2DE74290,
		                                           				0x36D0C906, 0x381C70E4, 0x4C6DA5B5, 0x3D81A682, 0x7E381F34,
		                                           				0x396C4F52, 0x95AD5901, 0x1DB50C5A, 0x29982E9E, 0x1557689F,
		                                           				0x3471EE42, 0xD7E2F7C0, 0x8795A1E2, 0xBC324D8D, 0xE224C3C8,
		                                           				0x12837E39, 0xCDEE3D74, 0x7AD2143F, 0x0E13D40C, 0x78BD4A68,
		                                           				0xA2EB194D, 0xDB9451F9, 0x859B71DC, 0x5C4F5B89, 0xCA14A8A4,
		                                           				0xEF92F003, 0x16741D98, 0x33AA4444, 0x9E967FBB, 0x092E3020,
		                                           				0xD86A35B8, 0x8CC17B10, 0xE1BF08AE, 0x55693FC5, 0x7680AD13,
		                                           				0x1E6546E8, 0x23B6E7B9, 0xEE77A4B2, 0x08ED0533, 0x44FD2895,
		                                           				0xB6393B69, 0x05D6CACF, 0x9819B209, 0xECBBB72F, 0x9A75779C,
		                                           				0xEAEC0749, 0x94A65AEE, 0xBDF52DC3, 0xD6A25D04, 0x82008E4E,
		                                           				0xA6DE160F, 0x9B036AFB, 0x228B3A66, 0x5FB10A70, 0xCC338B58,
		                                           				0x5378A9DF, 0xC908BCA9, 0x4959E25B, 0x46909A97, 0x66AE8F6E,
		                                           				0xDD0683E9, 0x65F994B4, 0x6426CDA5, 0xC24B8840, 0x32539DA0,
		                                           				0x63175650, 0xD0C815FF, 0x50CBC41E, 0xF7C774A3, 0x31B0C231,
		                                           				0x8D0D8116, 0x24BEF16C, 0xD555D256, 0xDF47EA8C, 0x6D21ECCD,
		                                           				0xA887A012, 0x84542AED, 0xA7B9C1BD, 0x914C1BB1, 0xA0D5B67D,
		                                           				0x438CE937, 0x7030F873, 0x71F6B0C7, 0x574576BA, 0xF8BC4541,
		                                           				0x9C61D348, 0x1960579D, 0x17C4DAAD, 0x96A4CB0B, 0xC193F2F6,
		                                           				0x756EAFA2, 0x7C1D2F94, 0xF4FE2B43, 0xCB86E33A, 0xEBD4C728,
		                                           				0x9D18AE64, 0x9FE13E30, 0x3CE0F5DE, 0xABA1F985, 0xADDC2718,
		                                           				0x68CE6278, 0xD45E241F, 0xA15C82B7, 0x3B2293D4, 0x739EDD32,
		                                           				0x674A6BF1, 0x5B5D587F, 0x4772DEAA, 0x4A63968F, 0x0BE68686,
		                                           				0x513D6426, 0x939A4787, 0xBBA89296, 0x4EC20007, 0x818D0D08,
		                                           				0xFF64DFD6
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0xCB2297CB, 0xDB48A144, 0xA16CBE4B, 0xBBEA1D6C, 0x5AF6B6B7,
		                                           				0x8A8110B6, 0xF9236EF9, 0xC98F83E6, 0x0F9C65B8, 0x252D4A89,
		                                           				0xA497F068, 0xA5D7ED2D, 0x94C22845, 0x9DA1C8C4, 0xE27C2E2E,
		                                           				0x6E8BA2B4, 0xC3DD17FB, 0x498CD482, 0x0DFE6A9F, 0xB0705829,
		                                           				0x9A1E6DC1, 0xF829717C, 0x07BB8E3A, 0xDA3C0B02, 0x1AF82FC7,
		                                           				0x73B70955, 0x7A04379C, 0x5EE20A28, 0x83712AE5, 0xF4C47C6D,
		                                           				0xDF72BA56, 0xD794858D, 0x8C0CF709, 0x18F0F390, 0xB6C69B35,
		                                           				0xBF2F01DB, 0x2FA74DCA, 0xD0CD9127, 0xBDE66CEC, 0x3DEEBD46,
		                                           				0x57C88FC3, 0xCEE1406F, 0x0066385A, 0xF3C3444F, 0x3A79D5D5,
		                                           				0x75751EB9, 0x3E7F8185, 0x521C2605, 0xE1AAAB6E, 0x38EBB80F,
		                                           				0xBEE7E904, 0x61CB9647, 0xEA54904E, 0x05AE00E4, 0x2D7AC65F,
		                                           				0x087751A1, 0xDCD82915, 0x0921EE16, 0xDD86D33B, 0xD6BD491A,
		                                           				0x40FBADF0, 0x4232CBD2, 0x33808D10, 0x39098C42, 0x193F3199,
		                                           				0x0BC1E47A, 0x4A82B149, 0x02B65A8A, 0x104CDC8E, 0x24A8F52C,
		                                           				0x685C6077, 0xC79F95C9, 0x1D11FE50, 0xC08DAFCD, 0x7B1A9A03,
		                                           				0x1C1F11D8, 0x84250E7F, 0x979DB248, 0xEBDC0501, 0xB9553395,
		                                           				0xE3C05EA8, 0xB1E51C4C, 0x13B0E681, 0x3B407766, 0x36DB3087,
		                                           				0xEE17C9FC, 0x6C53ECF2, 0xADCCC58F, 0xC427660B, 0xEFD5867D,
		                                           				0x9B6D54A5, 0x6FF1AEFF, 0x8E787952, 0x9E2BFFE0, 0x8761D034,
		                                           				0xE00BDBAD, 0xAE99A8D3, 0xCC03F6E2, 0xFD0ED807, 0x0E508AE3,
		                                           				0xB74182AB, 0x4349245D, 0xD120A465, 0xB246A641, 0xAF3B7AB0,
		                                           				0x2A6488BB, 0x4B3A0D1F, 0xE7C7E58C, 0x3FAFF2EB, 0x90445FFD,
		                                           				0xCF38C393, 0x995D07E7, 0xF24F1B36, 0x356F6891, 0x6D6EBCBE,
		                                           				0x8DA9E262, 0x50FD520E, 0x5BCA9E1E, 0x37472CF3, 0x69075057,
		                                           				0x7EC5FDED, 0x0CAB892A, 0xFB2412BA, 0x1728DEBF, 0xA000A988,
		                                           				0xD843CE79, 0x042E20DD, 0x4FE8F853, 0x56659C3C, 0x2739D119,
		                                           				0xA78A6120, 0x80960375, 0x70420611, 0x85E09F78, 0xABD17E96,
		                                           				0x1B513EAF, 0x1E01EB63, 0x26AD2133, 0xA890C094, 0x7613CF60,
		                                           				0x817E781B, 0xA39113D7, 0xE957FA58, 0x4131B99E, 0x28B1EFDA,
		                                           				0x66ACFBA7, 0xFF68944A, 0x77A44FD1, 0x7F331522, 0x59FFB3FA,
		                                           				0xA6DF935B, 0xFA12D9DF, 0xC6BF6F3F, 0x89520CF6, 0x659EDD6A,
		                                           				0x544DA739, 0x8B052538, 0x7C30EA21, 0xC2345525, 0x15927FB2,
		                                           				0x144A436B, 0xBA107B8B, 0x1219AC97, 0x06730432, 0x31831AB3,
		                                           				0xC55A5C24, 0xAA0FCD3E, 0xE5606BE8, 0x5C88F19B, 0x4C0841EE,
		                                           				0x1FE37267, 0x11F9C4F4, 0x9F1B9DAE, 0x864E76D0, 0xE637C731,
		                                           				0xD97D23A6, 0x32F53D5C, 0xB8161980, 0x93FA0F84, 0xCAEF0870,
		                                           				0x8874487E, 0x98F2CC73, 0x645FB5C6, 0xCD853659, 0x2062470D,
		                                           				0x16EDE8E9, 0x6B06DAB5, 0x78B43900, 0xFC95B786, 0x5D8E7DE1,
		                                           				0x465B5954, 0xFE7BA014, 0xF7D23F7B, 0x92BC8B18, 0x03593592,
		                                           				0x55CEF4F7, 0x74B27317, 0x79DE1FC2, 0xC8A0BFBD, 0x229398CC,
		                                           				0x62A602CE, 0xBCB94661, 0x5336D206, 0xD2A375FE, 0x6A6AB483,
		                                           				0x4702A5A4, 0xA2E9D73D, 0x23A2E0F1, 0x9189140A, 0x581D18DC,
		                                           				0xB39A922B, 0x82356212, 0xD5F432A9, 0xD356C2A3, 0x5F765B4D,
		                                           				0x450AFCC8, 0x4415E137, 0xE8ECDFBC, 0xED0DE3EA, 0x60D42B13,
		                                           				0xF13DF971, 0x71FC5DA2, 0xC1455340, 0xF087742F, 0xF55E5751,
		                                           				0x67B3C1F8, 0xAC6B8774, 0x7DCFAAAC, 0x95983BC0, 0x489BB0B1,
		                                           				0x2C184223, 0x964B6726, 0x2BD3271C, 0x72266472, 0xDED64530,
		                                           				0x0A2AA343, 0xD4F716A0, 0xB4DAD6D9, 0x2184345E, 0x512C990C,
		                                           				0x29D92D08, 0x2EBE709A, 0x01144C69, 0x34584B9D, 0xE4634ED6,
		                                           				0xECC963CF, 0x3C6984AA, 0x4ED056EF, 0x9CA56976, 0x8F3E80D4,
		                                           				0xB5BAE7C5, 0x30B5CAF5, 0x63F33A64, 0xA9E4BBDE, 0xF6B82298,
		                                           				0x4D673C1D
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x4B4F1121, 0xBA183081, 0xC784F41F, 0xD17D0BAC, 0x083D2267,
		                                           				0x37B1361E, 0x3581AD05, 0xFDA2F6BC, 0x1E892CDD, 0xB56D3C3A,
		                                           				0x32140E46, 0x138D8AAB, 0xE14773D4, 0x5B0E71DF, 0x5D1FE055,
		                                           				0x3FB991D3, 0xF1F46C71, 0xA325988C, 0x10F66E80, 0xB1006348,
		                                           				0x726A9F60, 0x3B67F8BA, 0x4E114EF4, 0x05C52115, 0x4C5CA11C,
		                                           				0x99E1EFD8, 0x471B83B3, 0xCBF7E524, 0x43AD82F5, 0x690CA93B,
		                                           				0xFAA61BB2, 0x12A832B5, 0xB734F943, 0xBD22AEA7, 0x88FEC626,
		                                           				0x5E80C3E7, 0xBE3EAF5E, 0x44617652, 0xA5724475, 0xBB3B9695,
		                                           				0x7F3FEE8F, 0x964E7DEB, 0x518C052D, 0x2A0BBC2B, 0xC2175F5C,
		                                           				0x9A7B3889, 0xA70D8D0C, 0xEACCDD29, 0xCCCD6658, 0x34BB25E6,
		                                           				0xB8391090, 0xF651356F, 0x52987C9E, 0x0C16C1CD, 0x8E372D3C,
		                                           				0x2FC6EBBD, 0x6E5DA3E3, 0xB0E27239, 0x5F685738, 0x45411786,
		                                           				0x067F65F8, 0x61778B40, 0x81AB2E65, 0x14C8F0F9, 0xA6B7B4CE,
		                                           				0x4036EAEC, 0xBF62B00A, 0xECFD5E02, 0x045449A6, 0xB20AFD28,
		                                           				0x2166D273, 0x0D13A863, 0x89508756, 0xD51A7530, 0x2D653F7A,
		                                           				0x3CDBDBC3, 0x80C9DF4F, 0x3D5812D9, 0x53FBB1F3, 0xC0F185C0,
		                                           				0x7A3C3D7E, 0x68646410, 0x857607A0, 0x1D12622E, 0x97F33466,
		                                           				0xDB4C9917, 0x6469607C, 0x566E043D, 0x79EF1EDB, 0x2C05898D,
		                                           				0xC9578E25, 0xCD380101, 0x46E04377, 0x7D1CC7A9, 0x6552B837,
		                                           				0x20192608, 0xB97500C5, 0xED296B44, 0x368648B4, 0x62995CD5,
		                                           				0x82731400, 0xF9AEBD8B, 0x3844C0C7, 0x7C2DE794, 0x33A1A770,
		                                           				0x8AE528C2, 0x5A2BE812, 0x1F8F4A07, 0x2B5ED7CA, 0x937EB564,
		                                           				0x6FDA7E11, 0xE49B5D6C, 0xB4B3244E, 0x18AA53A4, 0x3A061334,
		                                           				0x4D6067A3, 0x83BA5868, 0x9BDF4DFE, 0x7449F261, 0x709F8450,
		                                           				0xCAD133CB, 0xDE941C3F, 0xF52AE484, 0x781D77ED, 0x7E4395F0,
		                                           				0xAE103B59, 0x922331BB, 0x42CE50C8, 0xE6F08153, 0xE7D941D0,
		                                           				0x5028ED6B, 0xB3D2C49B, 0xAD4D9C3E, 0xD201FB6E, 0xA45BD5BE,
		                                           				0xFFCB7F4B, 0x579D7806, 0xF821BB5B, 0x59D592AD, 0xD0BE0C31,
		                                           				0xD4E3B676, 0x0107165A, 0x0FE939D2, 0x49BCAAFD, 0x55FFCFE5,
		                                           				0x2EC1F783, 0xF39A09A5, 0x3EB42772, 0x19B55A5D, 0x024A0679,
		                                           				0x8C83B3F7, 0x8642BA1D, 0xACACD9EA, 0x87D352C4, 0x60931F45,
		                                           				0xA05F97D7, 0x1CECD42C, 0xE2FCC87B, 0xB60F94E2, 0x67A34B0B,
		                                           				0xFCDD40C9, 0x0B150A27, 0xD3EE9E04, 0x582E29E9, 0x4AC22B41,
		                                           				0x6AC4E1B8, 0xBCCAA51A, 0x237AF30E, 0xEBC3B709, 0xC4A59D19,
		                                           				0x284BC98A, 0xE9D41A93, 0x6BFA2018, 0x73B2D651, 0x11F9A2FA,
		                                           				0xCE09BFF1, 0x41A470AA, 0x25888F22, 0x77E754E8, 0xF7330D8E,
		                                           				0x158EAB16, 0xC5D68842, 0xC685A6F6, 0xE5B82FDE, 0x09EA3A96,
		                                           				0x6DDE1536, 0x4FA919DA, 0x26C0BE9F, 0x9EED6F69, 0xF05555F2,
		                                           				0xE06FC285, 0x9CD76D23, 0xAF452A92, 0xEFC74CB7, 0x9D6B4732,
		                                           				0x8BE408EE, 0x22401D0D, 0xEE6C459D, 0x7587CB82, 0xE8746862,
		                                           				0x5CBDDE87, 0x98794278, 0x31AFB94D, 0xC11E0F2F, 0x30E8FC2A,
		                                           				0xCF3261EF, 0x1A3023E1, 0xAA2F86CF, 0xF202E24A, 0x8D08DCFF,
		                                           				0x764837C6, 0xA26374CC, 0x9F7C3E88, 0x949CC57D, 0xDD26A07F,
		                                           				0xC39EFAB0, 0xC8F879A1, 0xDCE67BB9, 0xF4B0A435, 0x912C9AE0,
		                                           				0xD85603E4, 0x953A9BBF, 0xFB8290D6, 0x0AEBCD5F, 0x16206A9A,
		                                           				0x6C787A14, 0xD9A0F16A, 0x29BF4F74, 0x8F8BCE91, 0x0E5A9354,
		                                           				0xAB038CB1, 0x1B8AD11B, 0xE327FF49, 0x0053DA20, 0x90CF51DC,
		                                           				0xDA92FE6D, 0x0390CA47, 0xA8958097, 0xA9DC5BAF, 0x3931E3C1,
		                                           				0x840446B6, 0x63D069FB, 0xD7460299, 0x7124ECD1, 0x0791E613,
		                                           				0x485918FC, 0xD635D04C, 0xDF96AC33, 0x66F2D303, 0x247056AE,
		                                           				0xA1A7B2A8, 0x27D8CC9C, 0x17B6E998, 0x7BF5590F, 0xFE97F557,
		                                           				0x5471D8A2
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x83A327A1, 0x9F379F51, 0x40A7D007, 0x11307423, 0x224587C1,
		                                           				0xAC27D63B, 0x3B7E64EA, 0x2E1CBFA6, 0x09996000, 0x03BC0E2C,
		                                           				0xD4C4478A, 0x4542E0AB, 0xFEDA26D4, 0xC1D10FCB, 0x8252F596,
		                                           				0x4494EB5C, 0xA362F314, 0xF5BA81FD, 0x75C3A376, 0x4CA214CA,
		                                           				0xE164DEDD, 0x5088FA97, 0x4B0930E0, 0x2FCFB7E8, 0x33A6F4B2,
		                                           				0xC7E94211, 0x2D66C774, 0x43BE8BAE, 0xC663D445, 0x908EB130,
		                                           				0xF4E3BE15, 0x63B9D566, 0x529396B5, 0x1E1BE743, 0x4D5FF63F,
		                                           				0x985E4A83, 0x71AB9DF7, 0xC516C6F5, 0x85C19AB4, 0x1F4DAEE4,
		                                           				0xF2973431, 0xB713DC5E, 0x3F2E159A, 0xC824DA16, 0x06BF376A,
		                                           				0xB2FE23EC, 0xE39B1C22, 0xF1EECB5F, 0x08E82D52, 0x565686C2,
		                                           				0xAB0AEA93, 0xFD47219F, 0xEBDBABD7, 0x2404A185, 0x8C7312B9,
		                                           				0xA8F2D828, 0x0C8902DA, 0x65B42B63, 0xC0BBEF62, 0x4E3E4CEF,
		                                           				0x788F8018, 0xEE1EBAB7, 0x93928F9D, 0x683D2903, 0xD3B60689,
		                                           				0xAFCB0DDC, 0x88A4C47A, 0xF6DD9C3D, 0x7EA5FCA0, 0x8A6D7244,
		                                           				0xBE11F120, 0x04FF91B8, 0x8D2DC8C0, 0x27F97FDB, 0x7F9E1F47,
		                                           				0x1734F0C7, 0x26F3ED8E, 0x0DF8F2BF, 0xB0833D9E, 0xE420A4E5,
		                                           				0xA423CAE6, 0x95616772, 0x9AE6C049, 0x075941F2, 0xD8E12812,
		                                           				0x000F6F4F, 0x3C0D6B05, 0x6CEF921C, 0xB82BC264, 0x396CB008,
		                                           				0x5D608A6F, 0x6D7782C8, 0x186550AA, 0x6B6FEC09, 0x28E70B13,
		                                           				0x57CE5688, 0xECD3AF84, 0x23335A95, 0x91F40CD2, 0x7B6A3B26,
		                                           				0xBD32B3B6, 0x3754A6FB, 0x8ED088F0, 0xF867E87C, 0x20851746,
		                                           				0x6410F9C6, 0x35380442, 0xC2CA10A7, 0x1ADEA27F, 0x76BDDD79,
		                                           				0x92742CF4, 0x0E98F7EE, 0x164E931D, 0xB9C835B3, 0x69060A99,
		                                           				0xB44C531E, 0xFA7B66FE, 0xC98A5B53, 0x7D95AAE9, 0x302F467B,
		                                           				0x74B811DE, 0xF3866ABD, 0xB5B3D32D, 0xFC3157A4, 0xD251FE19,
		                                           				0x0B5D8EAC, 0xDA71FFD5, 0x47EA05A3, 0x05C6A9E1, 0xCA0EE958,
		                                           				0x9939034D, 0x25DC5EDF, 0x79083CB1, 0x86768450, 0xCF757D6D,
		                                           				0x5972B6BC, 0xA78D59C9, 0xC4AD8D41, 0x2A362AD3, 0xD1179991,
		                                           				0x601407FF, 0xDCF50917, 0x587069D0, 0xE0821ED6, 0xDBB59427,
		                                           				0x73911A4B, 0x7C904FC3, 0x844AFB92, 0x6F8C955D, 0xE8C0C5BB,
		                                           				0xB67AB987, 0xA529D96C, 0xF91F7181, 0x618B1B06, 0xE718BB0C,
		                                           				0x8BD7615B, 0xD5A93A59, 0x54AEF81B, 0x772136E3, 0xCE44FD9C,
		                                           				0x10CDA57E, 0x87D66E0B, 0x3D798967, 0x1B2C1804, 0x3EDFBD68,
		                                           				0x15F6E62B, 0xEF68B854, 0x3896DB35, 0x12B7B5E2, 0xCB489029,
		                                           				0x9E4F98A5, 0x62EB77A8, 0x217C24A2, 0x964152F6, 0x49B2080A,
		                                           				0x53D23EE7, 0x48FB6D69, 0x1903D190, 0x9449E494, 0xBF6E7886,
		                                           				0xFB356CFA, 0x3A261365, 0x424BC1EB, 0xA1192570, 0x019CA782,
		                                           				0x9D3F7E0E, 0x9C127575, 0xEDF02039, 0xAD57BCCE, 0x5C153277,
		                                           				0x81A84540, 0xBCAA7356, 0xCCD59B60, 0xA62A629B, 0xA25CCD10,
		                                           				0x2B5B65CF, 0x1C535832, 0x55FD4E3A, 0x31D9790D, 0xF06BC37D,
		                                           				0x4AFC1D71, 0xAEED5533, 0xBA461634, 0xBB694B78, 0x5F3A5C73,
		                                           				0x6A3C764A, 0x8FB0CCA9, 0xF725684C, 0x4FE5382F, 0x1D0163AF,
		                                           				0x5AA07A8F, 0xE205A8ED, 0xC30BAD38, 0xFF22CF1F, 0x72432E2E,
		                                           				0x32C2518B, 0x3487CE4E, 0x7AE0AC02, 0x709FA098, 0x0A3B395A,
		                                           				0x5B4043F8, 0xA9E48C36, 0x149A8521, 0xD07DEE6B, 0x46ACD2F3,
		                                           				0x8958DFFC, 0xB3A1223C, 0xB11D31C4, 0xCD7F4D3E, 0x0F28E3AD,
		                                           				0xE5B100BE, 0xAAC54824, 0xE9C9D7BA, 0x9BD47001, 0x80F149B0,
		                                           				0x66022F0F, 0x020C4048, 0x6EFA192A, 0x67073F8D, 0x13EC7BF9,
		                                           				0x3655011A, 0xE6AFE157, 0xD9845F6E, 0xDECC4425, 0x511AE2CC,
		                                           				0xDF81B4D8, 0xD7809E55, 0xD6D883D9, 0x2CC7978C, 0x5E787CC5,
		                                           				0xDD0033D1, 0xA050C937, 0x97F75DCD, 0x299DE580, 0x41E2B261,
		                                           				0xEA5A54F1
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x7E672590, 0xBEA513BB, 0x2C906FE6, 0x86029C2B, 0x55DC4F74,
		                                           				0x0553398E, 0x63E09647, 0xCAFD0BAB, 0x264C37DF, 0x8272210F,
		                                           				0x67AFA669, 0x12D98A5F, 0x8CAB23C4, 0x75C68BD1, 0xC3370470,
		                                           				0x33F37F4E, 0x283992FF, 0xE73A3A67, 0x1032F283, 0xF5AD9FC2,
		                                           				0x963F0C5D, 0x664FBC45, 0x202BA41C, 0xC7C02D80, 0x54731E84,
		                                           				0x8A1085F5, 0x601D80FB, 0x2F968E55, 0x35E96812, 0xE45A8F78,
		                                           				0xBD7DE662, 0x3B6E6EAD, 0x8097C5EF, 0x070B6781, 0xB1E508F3,
		                                           				0x24E4FAE3, 0xB81A7805, 0xEC0FC918, 0x43C8774B, 0x9B2512A9,
		                                           				0x2B05AD04, 0x32C2536F, 0xEDF236E0, 0x8BC4B0CF, 0xBACEB837,
		                                           				0x4535B289, 0x0D0E94C3, 0xA5A371D0, 0xAD695A58, 0x39E3437D,
		                                           				0x9186BFFC, 0x21038C3B, 0x0AA9DFF9, 0x5D1F06CE, 0x62DEF8A4,
		                                           				0xF740A2B4, 0xA2575868, 0x682683C1, 0xDBB30FAC, 0x61FE1928,
		                                           				0x468A6511, 0xC61CD5F4, 0xE54D9800, 0x6B98D7F7, 0x8418B6A5,
		                                           				0x5F09A5D2, 0x90B4E80B, 0x49B2C852, 0x69F11C77, 0x17412B7E,
		                                           				0x7F6FC0ED, 0x56838DCC, 0x6E9546A2, 0xD0758619, 0x087B9B9A,
		                                           				0xD231A01D, 0xAF46D415, 0x097060FD, 0xD920F657, 0x882D3F9F,
		                                           				0x3AE7C3C9, 0xE8A00D9B, 0x4FE67EBE, 0x2EF80EB2, 0xC1916B0C,
		                                           				0xF4DFFEA0, 0xB97EB3EB, 0xFDFF84DD, 0xFF8B14F1, 0xE96B0572,
		                                           				0xF64B508C, 0xAE220A6E, 0x4423AE5A, 0xC2BECE5E, 0xDE27567C,
		                                           				0xFC935C63, 0x47075573, 0xE65B27F0, 0xE121FD22, 0xF2668753,
		                                           				0x2DEBF5D7, 0x8347E08D, 0xAC5EDA03, 0x2A7CEBE9, 0x3FE8D92E,
		                                           				0x23542FE4, 0x1FA7BD50, 0xCF9B4102, 0x9D0DBA39, 0x9CB8902A,
		                                           				0xA7249D8B, 0x0F6D667A, 0x5EBFA9EC, 0x6A594DF2, 0x79600938,
		                                           				0x023B7591, 0xEA2C79C8, 0xC99D07EA, 0x64CB5EE1, 0x1A9CAB3D,
		                                           				0x76DB9527, 0xC08E012F, 0x3DFB481A, 0x872F22E7, 0x2948D15C,
		                                           				0xA4782C79, 0x6F50D232, 0x78F0728A, 0x5A87AAB1, 0xC4E2C19C,
		                                           				0xEE767387, 0x1B2A1864, 0x7B8D10D3, 0xD1713161, 0x0EEAC456,
		                                           				0xD8799E06, 0xB645B548, 0x4043CB65, 0xA874FB29, 0x4B12D030,
		                                           				0x7D687413, 0x18EF9A1F, 0xD7631D4C, 0x5829C7DA, 0xCDFA30FA,
		                                           				0xC5084BB0, 0x92CD20E2, 0xD4C16940, 0x03283EC0, 0xA917813F,
		                                           				0x9A587D01, 0x70041F8F, 0xDC6AB1DC, 0xDDAEE3D5, 0x31829742,
		                                           				0x198C022D, 0x1C9EAFCB, 0x5BBC6C49, 0xD3D3293A, 0x16D50007,
		                                           				0x04BB8820, 0x3C5C2A41, 0x37EE7AF8, 0x8EB04025, 0x9313ECBA,
		                                           				0xBFFC4799, 0x8955A744, 0xEF85D633, 0x504499A7, 0xA6CA6A86,
		                                           				0xBB3D3297, 0xB34A8236, 0x6DCCBE4F, 0x06143394, 0xCE19FC7B,
		                                           				0xCCC3C6C6, 0xE36254AE, 0x77B7EDA1, 0xA133DD9E, 0xEBF9356A,
		                                           				0x513CCF88, 0xE2A1B417, 0x972EE5BD, 0x853824CD, 0x5752F4EE,
		                                           				0x6C1142E8, 0x3EA4F309, 0xB2B5934A, 0xDFD628AA, 0x59ACEA3E,
		                                           				0xA01EB92C, 0x389964BC, 0xDA305DD4, 0x019A59B7, 0x11D2CA93,
		                                           				0xFAA6D3B9, 0x4E772ECA, 0x72651776, 0xFB4E5B0E, 0xA38F91A8,
		                                           				0x1D0663B5, 0x30F4F192, 0xB50051B6, 0xB716CCB3, 0x4ABD1B59,
		                                           				0x146C5F26, 0xF134E2DE, 0x00F67C6C, 0xB0E1B795, 0x98AA4EC7,
		                                           				0x0CC73B34, 0x654276A3, 0x8D1BA871, 0x740A5216, 0xE0D01A23,
		                                           				0x9ED161D6, 0x9F36A324, 0x993EBB7F, 0xFEB9491B, 0x365DDCDB,
		                                           				0x810CFFC5, 0x71EC0382, 0x2249E7BF, 0x48817046, 0xF3A24A5B,
		                                           				0x4288E4D9, 0x0BF5C243, 0x257FE151, 0x95B64C0D, 0x4164F066,
		                                           				0xAAF7DB08, 0x73B1119D, 0x8F9F7BB8, 0xD6844596, 0xF07A34A6,
		                                           				0x53943D0A, 0xF9DD166D, 0x7A8957AF, 0xF8BA3CE5, 0x27C9621E,
		                                           				0x5CDAE910, 0xC8518998, 0x941538FE, 0x136115D8, 0xABA8443C,
		                                           				0x4D01F931, 0x34EDF760, 0xB45F266B, 0xD5D4DE14, 0x52D8AC35,
		                                           				0x15CFD885, 0xCBC5CD21, 0x4CD76D4D, 0x7C80EF54, 0xBC92EE75,
		                                           				0x1E56A1F6
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0xBAA20B6C, 0x9FFBAD26, 0xE1F7D738, 0x794AEC8D, 0xC9E9CF3C,
		                                           				0x8A9A7846, 0xC57C4685, 0xB9A92FED, 0x29CB141F, 0x52F9DDB7,
		                                           				0xF68BA6BC, 0x19CCC020, 0x4F584AAA, 0x3BF6A596, 0x003B7CF7,
		                                           				0x54F0CE9A, 0xA7EC4303, 0x46CF0077, 0x78D33AA1, 0x215247D9,
		                                           				0x74BCDF91, 0x08381D30, 0xDAC43E40, 0x64872531, 0x0BEFFE5F,
		                                           				0xB317F457, 0xAEBB12DA, 0xD5D0D67B, 0x7D75C6B4, 0x42A6D241,
		                                           				0x1502D0A9, 0x3FD97FFF, 0xC6C3ED28, 0x81868D0A, 0x92628BC5,
		                                           				0x86679544, 0xFD1867AF, 0x5CA3EA61, 0x568D5578, 0x4A2D71F4,
		                                           				0x43C9D549, 0x8D95DE2B, 0x6E5C74A0, 0x9120FFC7, 0x0D05D14A,
		                                           				0xA93049D3, 0xBFA80E17, 0xF4096810, 0x043F5EF5, 0xA673B4F1,
		                                           				0x6D780298, 0xA4847783, 0x5EE726FB, 0x9934C281, 0x220A588C,
		                                           				0x384E240F, 0x933D5C69, 0x39E5EF47, 0x26E8B8F3, 0x4C1C6212,
		                                           				0x8040F75D, 0x074B7093, 0x6625A8D7, 0x36298945, 0x76285088,
		                                           				0x651D37C3, 0x24F5274D, 0xDBCA3DAB, 0x186B7EE1, 0xD80F8182,
		                                           				0x14210C89, 0x943A3075, 0x4E6E11C4, 0x4D7E6BAD, 0xF05064C8,
		                                           				0x025DCD97, 0x4BC10302, 0x7CEDE572, 0x8F90A970, 0xAB88EEBA,
		                                           				0xB5998029, 0x5124D839, 0xB0EEB6A3, 0x89DDABDC, 0xE8074D76,
		                                           				0xA1465223, 0x32518CF2, 0x9D39D4EB, 0xC0D84524, 0xE35E6EA8,
		                                           				0x7ABF3804, 0x113E2348, 0x9AE6069D, 0xB4DFDABB, 0xA8C5313F,
		                                           				0x23EA3F79, 0x530E36A2, 0xA5FD228B, 0x95D1D350, 0x2B14CC09,
		                                           				0x40042956, 0x879D05CC, 0x2064B9CA, 0xACACA40E, 0xB29C846E,
		                                           				0x9676C9E3, 0x752B7B8A, 0x7BE2BCC2, 0x6BD58F5E, 0xD48F4C32,
		                                           				0x606835E4, 0x9CD7C364, 0x2C269B7A, 0x3A0D079C, 0x73B683FE,
		                                           				0x45374F1E, 0x10AFA242, 0x577F8666, 0xDDAA10F6, 0xF34F561C,
		                                           				0x3D355D6B, 0xE47048AE, 0xAA13C492, 0x050344FD, 0x2AAB5151,
		                                           				0xF5B26AE5, 0xED919A59, 0x5AC67900, 0xF1CDE380, 0x0C79A11B,
		                                           				0x351533FC, 0xCD4D8E36, 0x1F856005, 0x690B9FDD, 0xE736DCCF,
		                                           				0x1D47BF6A, 0x7F66C72A, 0x85F21B7F, 0x983CBDB6, 0x01EBBEBF,
		                                           				0x035F3B99, 0xEB111F34, 0x28CEFDC6, 0x5BFC9ECD, 0xF22EACB0,
		                                           				0x9E41CBB2, 0xE0F8327C, 0x82E3E26F, 0xFC43FC86, 0xD0BA66DF,
		                                           				0x489EF2A7, 0xD9E0C81D, 0x68690D52, 0xCC451367, 0xC2232E16,
		                                           				0xE95A7335, 0x0FDAE19B, 0xFF5B962C, 0x97596527, 0xC46DB333,
		                                           				0x3ED4C562, 0xC14C9D9E, 0x5D6FAA21, 0x638E940D, 0xF9316D58,
		                                           				0x47B3B0EA, 0x30FFCAD2, 0xCE1BBA7D, 0x1E6108E6, 0x2E1EA33D,
		                                           				0x507BF05B, 0xFAFEF94B, 0xD17DE8E2, 0x5598B214, 0x1663F813,
		                                           				0x17D25A2D, 0xEEFA5FF9, 0x582F4E37, 0x12128773, 0xFEF17AB8,
		                                           				0x06005322, 0xBB32BBC9, 0x8C898508, 0x592C15F0, 0xD38A4054,
		                                           				0x4957B7D6, 0xD2B891DB, 0x37BD2D3E, 0x34AD20CB, 0x622288E9,
		                                           				0x2DC7345A, 0xAFB416C0, 0x1CF459B1, 0xDC7739FA, 0x0A711A25,
		                                           				0x13E18A0C, 0x5F72AF4C, 0x6AC8DB11, 0xBE53C18E, 0x1AA569B9,
		                                           				0xEF551EA4, 0xA02A429F, 0xBD16E790, 0x7EB9171A, 0x77D693D8,
		                                           				0x8E06993A, 0x9BDE7560, 0xE5801987, 0xC37A09BE, 0xB8DB76AC,
		                                           				0xE2087294, 0x6C81616D, 0xB7F30FE7, 0xBC9B82BD, 0xFBA4E4D4,
		                                           				0xC7B1012F, 0xA20C043B, 0xDE9FEBD0, 0x2F9297CE, 0xE610AEF8,
		                                           				0x70B06F19, 0xC86AE00B, 0x0E01988F, 0x41192AE0, 0x448C1CB5,
		                                           				0xADBE92EE, 0x7293A007, 0x1B54B5B3, 0xD61F63D1, 0xEAE40A74,
		                                           				0x61A72B55, 0xEC83A7D5, 0x88942806, 0x90A07DA5, 0xD7424B95,
		                                           				0x67745B4E, 0xA31A1853, 0xCA6021EF, 0xDFB56C4F, 0xCBC2D915,
		                                           				0x3C48E918, 0x8BAE3C63, 0x6F659C71, 0xF8B754C1, 0x2782F3DE,
		                                           				0xF796F168, 0x71492C84, 0x33C0F5A6, 0x3144F6EC, 0x25DC412E,
		                                           				0xB16C5743, 0x83A1FA7E, 0x0997B101, 0xB627E6E8, 0xCF33905C,
		                                           				0x8456FB65
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0xB29BEA74, 0xC35DA605, 0x305C1CA3, 0xD2E9F5BC, 0x6FD5BFF4,
		                                           				0xFF347703, 0xFC45B163, 0xF498E068, 0xB71229FC, 0x81ACC3FB,
		                                           				0x78538A8B, 0x984ECF81, 0xA5DA47A4, 0x8F259EEF, 0x6475DC65,
		                                           				0x081865B9, 0x49E14A3C, 0x19E66079, 0xD382E91B, 0x5B109794,
		                                           				0x3F9F81E1, 0x4470A388, 0x41601ABE, 0xAAF9F407, 0x8E175EF6,
		                                           				0xED842297, 0x893A4271, 0x1790839A, 0xD566A99E, 0x6B417DEE,
		                                           				0x75C90D23, 0x715EDB31, 0x723553F7, 0x9AFB50C9, 0xFBC5F600,
		                                           				0xCD3B6A4E, 0x97ED0FBA, 0x29689AEC, 0x63135C8E, 0xF0E26C7E,
		                                           				0x0692AE7F, 0xDBB208FF, 0x2EDE3E9B, 0x6A65BEBD, 0xD40867E9,
		                                           				0xC954AFC5, 0x73B08201, 0x7FFDF809, 0x1195C24F, 0x1CA5ADCA,
		                                           				0x74BD6D1F, 0xB393C455, 0xCADFD3FA, 0x99F13011, 0x0EBCA813,
		                                           				0x60E791B8, 0x6597AC7A, 0x18A7E46B, 0x09CB49D3, 0x0B27DF6D,
		                                           				0xCFE52F87, 0xCEF66837, 0xE6328035, 0xFA87C592, 0x37BAFF93,
		                                           				0xD71FCC99, 0xDCAB205C, 0x4D7A5638, 0x48012510, 0x62797558,
		                                           				0xB6CF1FE5, 0xBC311834, 0x9C2373AC, 0x14EC6175, 0xA439CBDF,
		                                           				0x54AFB0EA, 0xD686960B, 0xFDD0D47B, 0x7B063902, 0x8B78BAC3,
		                                           				0x26C6A4D5, 0x5C0055B6, 0x2376102E, 0x0411783E, 0x2AA3F1CD,
		                                           				0x51FC6EA8, 0x701CE243, 0x9B2A0ABB, 0x0AD93733, 0x6E80D03D,
		                                           				0xAF6295D1, 0xF629896F, 0xA30B0648, 0x463D8DD4, 0x963F84CB,
		                                           				0x01FF94F8, 0x8D7FEFDC, 0x553611C0, 0xA97C1719, 0xB96AF759,
		                                           				0xE0E3C95E, 0x0528335B, 0x21FE5925, 0x821A5245, 0x807238B1,
		                                           				0x67F23DB5, 0xEA6B4EAB, 0x0DA6F985, 0xAB1BC85A, 0xEF8C90E4,
		                                           				0x4526230E, 0x38EB8B1C, 0x1B91CD91, 0x9FCE5F0C, 0xF72CC72B,
		                                           				0xC64F2617, 0xDAF7857D, 0x7D373CF1, 0x28EAEDD7, 0x203887D0,
		                                           				0xC49A155F, 0xA251B3B0, 0xF2D47AE3, 0x3D9EF267, 0x4A94AB2F,
		                                           				0x7755A222, 0x0205E329, 0xC28FA7A7, 0xAEC1FE51, 0x270F164C,
		                                           				0x8C6D01BF, 0x53B5BC98, 0xC09D3FEB, 0x834986CC, 0x4309A12C,
		                                           				0x578B2A96, 0x3BB74B86, 0x69561B4A, 0x037E32F3, 0xDE335B08,
		                                           				0xC5156BE0, 0xE7EF09AD, 0x93B834C7, 0xA7719352, 0x59302821,
		                                           				0xE3529D26, 0xF961DA76, 0xCB142C44, 0xA0F3B98D, 0x76502457,
		                                           				0x945A414B, 0x078EEB12, 0xDFF8DE69, 0xEB6C8C2D, 0xBDA90C4D,
		                                           				0xE9C44D16, 0x168DFD66, 0xAD64763B, 0xA65FD764, 0x95A29C06,
		                                           				0x32D7713F, 0x40F0B277, 0x224AF08F, 0x004CB5E8, 0x92574814,
		                                           				0x8877D827, 0x3E5B2D04, 0x68C2D5F2, 0x86966273, 0x1D433ADA,
		                                           				0x8774988A, 0x3C0E0BFE, 0xDDAD581D, 0x2FD654ED, 0x0F4769FD,
		                                           				0xC181EE9D, 0x5FD88F61, 0x341DBB3A, 0x528543F9, 0xD92235CF,
		                                           				0x1EA82EB4, 0xB5CD790F, 0x91D24F1E, 0xA869E6C2, 0x61F474D2,
		                                           				0xCC205ADD, 0x0C7BFBA9, 0xBF2B0489, 0xB02D72D8, 0x2B46ECE6,
		                                           				0xE4DCD90A, 0xB8A11440, 0xEE8A63B7, 0x854DD1A1, 0xD1E00583,
		                                           				0x42B40E24, 0x9E8964DE, 0xB4B35D78, 0xBEC76F6E, 0x24B9C620,
		                                           				0xD8D399A6, 0x5ADB2190, 0x2DB12730, 0x3A5866AF, 0x58C8FADB,
		                                           				0x5D8844E7, 0x8A4BF380, 0x15A01D70, 0x79F5C028, 0x66BE3B8C,
		                                           				0xF3E42B53, 0x56990039, 0x2C0C3182, 0x5E16407C, 0xECC04515,
		                                           				0x6C440284, 0x4CB6701A, 0x13BFC142, 0x9D039F6A, 0x4F6E92C8,
		                                           				0xA1407C62, 0x8483A095, 0xC70AE1C4, 0xE20213A2, 0xBACAFC41,
		                                           				0x4ECC12B3, 0x4BEE3646, 0x1FE807AE, 0x25217F9C, 0x35DDE5F5,
		                                           				0x7A7DD6CE, 0xF89CCE50, 0xAC07B718, 0x7E73D2C6, 0xE563E76C,
		                                           				0x123CA536, 0x3948CA56, 0x9019DD49, 0x10AA88D9, 0xC82451E2,
		                                           				0x473EB6D6, 0x506FE854, 0xE8BB03A5, 0x332F4C32, 0xFE1E1E72,
		                                           				0xB1AE572A, 0x7C0D7BC1, 0xE1C37EB2, 0xF542AA60, 0xF1A48EA0,
		                                           				0xD067B89F, 0xBBFA195D, 0x1A049B0D, 0x315946AA, 0x36D1B447,
		                                           				0x6D2EBDF0
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x0D188A6D, 0x12CEA0DB, 0x7E63740E, 0x6A444821, 0x253D234F,
		                                           				0x6FFC6597, 0x94A6BDEF, 0x33EE1B2F, 0x0A6C00C0, 0x3AA336B1,
		                                           				0x5AF55D17, 0x265FB3DC, 0x0E89CF4D, 0x0786B008, 0xC80055B8,
		                                           				0x6B17C3CE, 0x72B05A74, 0xD21A8D78, 0xA6B70840, 0xFE8EAE77,
		                                           				0xED69565C, 0x55E1BCF4, 0x585C2F60, 0xE06F1A62, 0xAD67C0CD,
		                                           				0x7712AF88, 0x9CC26ACA, 0x1888053D, 0x37EB853E, 0x9215ABD7,
		                                           				0xDE30ADFC, 0x1F1038E6, 0x70C51C8A, 0x8D586C26, 0xF72BDD90,
		                                           				0x4DC3CE15, 0x68EAEEFA, 0xD0E9C8B9, 0x200F9C44, 0xDDD141BA,
		                                           				0x024BF1D3, 0x0F64C9D4, 0xC421E9E9, 0x9D11C14C, 0x9A0DD9E4,
		                                           				0x5F92EC19, 0x1B980DF0, 0x1DCC4542, 0xB8FE8C56, 0x0C9C9167,
		                                           				0x4E81EB49, 0xCA368F27, 0xE3603B37, 0xEA08ACCC, 0xAC516992,
		                                           				0xC34F513B, 0x804D100D, 0x6EDCA4C4, 0xFC912939, 0x29D219B0,
		                                           				0x278AAA3C, 0x4868DA7D, 0x54E890B7, 0xB46D735A, 0x514589AA,
		                                           				0xD6C630AF, 0x4980DFE8, 0xBE3CCC55, 0x59D41202, 0x650C078B,
		                                           				0xAF3A9E7B, 0x3ED9827A, 0x9E79FC6E, 0xAADBFBAE, 0xC5F7D803,
		                                           				0x3DAF7F50, 0x67B4F465, 0x73406E11, 0x39313F8C, 0x8A6E6686,
		                                           				0xD8075F1F, 0xD3CBFED1, 0x69C7E49C, 0x930581E0, 0xE4B1A5A8,
		                                           				0xBBC45472, 0x09DDBF58, 0xC91D687E, 0xBDBFFDA5, 0x88C08735,
		                                           				0xE9E36BF9, 0xDB5EA9B6, 0x95559404, 0x08F432FB, 0xE24EA281,
		                                           				0x64663579, 0x000B8010, 0x7914E7D5, 0x32FD0473, 0xD1A7F0A4,
		                                           				0x445AB98E, 0xEC72993F, 0xA29A4D32, 0xB77306D8, 0xC7C97CF6,
		                                           				0x7B6AB645, 0xF5EF7ADF, 0xFB2E15F7, 0xE747F757, 0x5E944354,
		                                           				0x234A2669, 0x47E46359, 0x9B9D11A9, 0x40762CED, 0x56F1DE98,
		                                           				0x11334668, 0x890A9A70, 0x1A296113, 0xB3BD4AF5, 0x163B7548,
		                                           				0xD51B4F84, 0xB99B2ABC, 0x3CC1DC30, 0xA9F0B56C, 0x812272B2,
		                                           				0x0B233A5F, 0xB650DBF2, 0xF1A0771B, 0x36562B76, 0xDC037B0F,
		                                           				0x104C97FF, 0xC2EC98D2, 0x90596F22, 0x28B6620B, 0xDF42B212,
		                                           				0xFDBC4243, 0xF3FB175E, 0x4A2D8B00, 0xE8F3869B, 0x30D69BC3,
		                                           				0x853714C8, 0xA7751D2E, 0x31E56DEA, 0xD4840B0C, 0x9685D783,
		                                           				0x068C9333, 0x8FBA032C, 0x76D7BB47, 0x6D0EE22B, 0xB546794B,
		                                           				0xD971B894, 0x8B09D253, 0xA0AD5761, 0xEE77BA06, 0x46359F31,
		                                           				0x577CC7EC, 0x52825EFD, 0xA4BEED95, 0x9825C52A, 0xEB48029A,
		                                           				0xBAAE59F8, 0xCF490EE1, 0xBC990164, 0x8CA49DFE, 0x4F38A6E7,
		                                           				0x2BA98389, 0x8228F538, 0x199F64AC, 0x01A1CAC5, 0xA8B51641,
		                                           				0x5CE72D01, 0x8E5DF26B, 0x60F28E1E, 0xCD5BE125, 0xE5B376BF,
		                                           				0x1C8D3116, 0x7132CBB3, 0xCB7AE320, 0xC0FA5366, 0xD7653E34,
		                                           				0x971C88C2, 0xC62C7DD0, 0x34D0A3DA, 0x868F6709, 0x7AE6FA8F,
		                                           				0x22BBD523, 0x66CD3D5B, 0x1EF9288D, 0xF9CF58C1, 0x5B784E80,
		                                           				0x7439A191, 0xAE134C36, 0x9116C463, 0x2E9E1396, 0xF8611F3A,
		                                           				0x2D2F3307, 0x247F37DD, 0xC1E2FF9D, 0x43C821E5, 0x05ED5CAB,
		                                           				0xEF74E80A, 0x4CCA6028, 0xF0AC3CBD, 0x5D874B29, 0x6C62F6A6,
		                                           				0x4B2A2EF3, 0xB1AA2087, 0x62A5D0A3, 0x0327221C, 0xB096B4C6,
		                                           				0x417EC693, 0xABA840D6, 0x789725EB, 0xF4B9E02D, 0xE6E00975,
		                                           				0xCC04961A, 0x63F624BB, 0x7FA21ECB, 0x2C01EA7F, 0xB2415005,
		                                           				0x2A8BBEB5, 0x83B2B14E, 0xA383D1A7, 0x5352F96A, 0x043ECDAD,
		                                           				0xCE1918A1, 0xFA6BE6C9, 0x50DEF36F, 0xF6B80CE2, 0x4543EF7C,
		                                           				0x9953D651, 0xF257955D, 0x87244914, 0xDA1E0A24, 0xFFDA4785,
		                                           				0x14D327A2, 0x3B93C29F, 0x840684B4, 0x61AB71A0, 0x9F7B784A,
		                                           				0x2FD570CF, 0x15955BDE, 0x38F8D471, 0x3534A718, 0x133FB71D,
		                                           				0x3FD80F52, 0x4290A8BE, 0x75FF44C7, 0xA554E546, 0xE1023499,
		                                           				0xBF2652E3, 0x7D20399E, 0xA1DF7E82, 0x177092EE, 0x217DD3F1,
		                                           				0x7C1FF8D9
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x12113F2E, 0xBFBD0785, 0xF11793FB, 0xA5BFF566, 0x83C7B0E5,
		                                           				0x72FB316B, 0x75526A9A, 0x41E0E612, 0x7156BA09, 0x53CE7DEE,
		                                           				0x0AA26881, 0xA43E0D7D, 0x3DA73CA3, 0x182761ED, 0xBD5077FF,
		                                           				0x56DB4AA0, 0xE792711C, 0xF0A4EB1D, 0x7F878237, 0xEC65C4E8,
		                                           				0x08DC8D43, 0x0F8CE142, 0x8258ABDA, 0xF4154E16, 0x49DEC2FD,
		                                           				0xCD8D5705, 0x6C2C3A0F, 0x5C12BB88, 0xEFF3CDB6, 0x2C89ED8C,
		                                           				0x7BEBA967, 0x2A142157, 0xC6D0836F, 0xB4F97E96, 0x6931E969,
		                                           				0x514E6C7C, 0xA7792600, 0x0BBBF780, 0x59671BBD, 0x0707B676,
		                                           				0x37482D93, 0x80AF1479, 0x3805A60D, 0xE1F4CAC1, 0x580B3074,
		                                           				0x30B8D6CE, 0x05A304BE, 0xD176626D, 0xEBCA97F3, 0xBB201F11,
		                                           				0x6A1AFE23, 0xFFAA86E4, 0x62B4DA49, 0x1B6629F5, 0xF5D9E092,
		                                           				0xF37F3DD1, 0x619BD45B, 0xA6EC8E4F, 0x29C80939, 0x0C7C0C34,
		                                           				0x9CFE6E48, 0xE65FD3AC, 0x73613B65, 0xB3C669F9, 0xBE2E8A9E,
		                                           				0x286F9678, 0x5797FD13, 0x99805D75, 0xCFB641C5, 0xA91074BA,
		                                           				0x6343AF47, 0x6403CB46, 0x8894C8DB, 0x2663034C, 0x3C40DC5E,
		                                           				0x00995231, 0x96789AA2, 0x2EFDE4B9, 0x7DC195E1, 0x547DADD5,
		                                           				0x06A8EA04, 0xF2347A63, 0x5E0DC6F7, 0x8462DFC2, 0x1E6B2C3C,
		                                           				0x9BD275B3, 0x91D419E2, 0xBCEFD17E, 0xB9003924, 0xD07E7320,
		                                           				0xDEF0495C, 0xC36AD00E, 0x1785B1AB, 0x92E20BCF, 0xB139F0E9,
		                                           				0x675BB9A1, 0xAECFA4AF, 0x132376CB, 0xE84589D3, 0x79A05456,
		                                           				0xA2F860BC, 0x1AE4F8B5, 0x20DF4DB4, 0xA1E1428B, 0x3BF60A1A,
		                                           				0x27FF7BF1, 0xCB44C0E7, 0xF7F587C4, 0x1F3B9B21, 0x94368F01,
		                                           				0x856E23A4, 0x6F93DE3F, 0x773F5BBF, 0x8B22056E, 0xDF41F654,
		                                           				0xB8246FF4, 0x8D57BFF2, 0xD57167EA, 0xC5699F22, 0x40734BA7,
		                                           				0x5D5C2772, 0x033020A8, 0xE30A7C4D, 0xADC40FD6, 0x76353441,
		                                           				0x5AA5229B, 0x81516590, 0xDA49F14E, 0x4FA672A5, 0x4D9FAC5F,
		                                           				0x154BE230, 0x8A7A5CC0, 0xCE3D2F84, 0xCCA15514, 0x5221360C,
		                                           				0xAF0FB81E, 0x5BDD5873, 0xF6825F8F, 0x1113D228, 0x70AD996C,
		                                           				0x93320051, 0x60471C53, 0xE9BA567B, 0x3A462AE3, 0x5F55E72D,
		                                           				0x1D3C5AD7, 0xDCFC45EC, 0x34D812EF, 0xFA96EE1B, 0x369D1EF8,
		                                           				0xC9B1A189, 0x7C1D3555, 0x50845EDC, 0x4BB31877, 0x8764A060,
		                                           				0x8C9A9415, 0x230E1A3A, 0xB05E9133, 0x242B9E03, 0xA3B99DB7,
		                                           				0xC2D7FB0A, 0x3333849D, 0xD27278D4, 0xB5D3EFA6, 0x78AC28AD,
		                                           				0xC7B2C135, 0x0926ECF0, 0xC1374C91, 0x74F16D98, 0x2274084A,
		                                           				0x3F6D9CFA, 0x7AC0A383, 0xB73AFF1F, 0x3909A23D, 0x9F1653AE,
		                                           				0x4E2F3E71, 0xCA5AB22A, 0xE01E3858, 0x90C5A7EB, 0x3E4A17DF,
		                                           				0xAA987FB0, 0x488BBD62, 0xB625062B, 0x2D776BB8, 0x43B5FC08,
		                                           				0x1490D532, 0xD6D12495, 0x44E89845, 0x2FE60118, 0x9D9EF950,
		                                           				0xAC38133E, 0xD3864329, 0x017B255A, 0xFDC2DD26, 0x256851E6,
		                                           				0x318E7086, 0x2BFA4861, 0x89EAC706, 0xEE5940C6, 0x68C3BC2F,
		                                           				0xE260334B, 0x98DA90BB, 0xF818F270, 0x4706D897, 0x212D3799,
		                                           				0x4CF7E5D0, 0xD9C9649F, 0xA85DB5CD, 0x35E90E82, 0x6B881152,
		                                           				0xAB1C02C7, 0x46752B02, 0x664F598E, 0x45AB2E64, 0xC4CDB4B2,
		                                           				0xBA42107F, 0xEA2A808A, 0x971BF3DE, 0x4A54A836, 0x4253AECC,
		                                           				0x1029BE68, 0x6DCC9225, 0xE4BCA56A, 0xC0AE50B1, 0x7E011D94,
		                                           				0xE59C162C, 0xD8E5C340, 0xD470FA0B, 0xB2BE79DD, 0xD783889C,
		                                           				0x1CEDE8F6, 0x8F4C817A, 0xDDB785C9, 0x860232D8, 0x198AAAD9,
		                                           				0xA0814738, 0x3219CFFC, 0x169546D2, 0xFC0CB759, 0x55911510,
		                                           				0x04D5CEC3, 0xED08CC3B, 0x0D6CF427, 0xC8E38CCA, 0x0EEEE3FE,
		                                           				0x9EE7D7C8, 0xF9F24FA9, 0xDB04B35D, 0x9AB0C9E0, 0x651F4417,
		                                           				0x028F8B07, 0x6E28D9AA, 0xFBA96319, 0x8ED66687, 0xFECBC58D,
		                                           				0x954DDB44
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x7B0BDFFE, 0x865D16B1, 0x49A058C0, 0x97ABAA3F, 0xCAACC75D,
		                                           				0xABA6C17D, 0xF8746F92, 0x6F48AEED, 0x8841D4B5, 0xF36A146A,
		                                           				0x73C390AB, 0xE6FB558F, 0x87B1019E, 0x26970252, 0x246377B2,
		                                           				0xCBF676AE, 0xF923DB06, 0xF7389116, 0x14C81A90, 0x83114EB4,
		                                           				0x8B137559, 0x95A86A7A, 0xD5B8DA8C, 0xC4DF780E, 0x5A9CB3E2,
		                                           				0xE44D4062, 0xE8DC8EF6, 0x9D180845, 0x817AD18B, 0xC286C85B,
		                                           				0x251F20DE, 0xEE6D5933, 0xF6EDEF81, 0xD4D16C1E, 0xC94A0C32,
		                                           				0x8437FD22, 0x3271EE43, 0x42572AEE, 0x5F91962A, 0x1C522D98,
		                                           				0x59B23F0C, 0xD86B8804, 0x08C63531, 0x2C0D7A40, 0xB97C4729,
		                                           				0x04964DF9, 0x13C74A17, 0x5878362F, 0x4C808CD6, 0x092CB1E0,
		                                           				0x6DF02885, 0xA0C2105E, 0x8ABA9E68, 0x64E03057, 0xE5D61325,
		                                           				0x0E43A628, 0x16DBD62B, 0x2733D90B, 0x3AE57283, 0xC0C1052C,
		                                           				0x4B6FB620, 0x37513953, 0xFC898BB3, 0x471B179F, 0xDF6E66B8,
		                                           				0xD32142F5, 0x9B30FAFC, 0x4ED92549, 0x105C6D99, 0x4ACD69FF,
		                                           				0x2B1A27D3, 0x6BFCC067, 0x6301A278, 0xAD36E6F2, 0xEF3FF64E,
		                                           				0x56B3CADB, 0x0184BB61, 0x17BEB9FD, 0xFAEC6109, 0xA2E1FFA1,
		                                           				0x2FD224F8, 0x238F5BE6, 0x8F8570CF, 0xAEB5F25A, 0x4F1D3E64,
		                                           				0x4377EB24, 0x1FA45346, 0xB2056386, 0x52095E76, 0xBB7B5ADC,
		                                           				0x3514E472, 0xDDE81E6E, 0x7ACEA9C4, 0xAC15CC48, 0x71C97D93,
		                                           				0x767F941C, 0x911052A2, 0xFFEA09BF, 0xFE3DDCF0, 0x15EBF3AA,
		                                           				0x9235B8BC, 0x75408615, 0x9A723437, 0xE1A1BD38, 0x33541B7E,
		                                           				0x1BDD6856, 0xB307E13E, 0x90814BB0, 0x51D7217B, 0x0BB92219,
		                                           				0x689F4500, 0xC568B01F, 0x5DF3D2D7, 0x3C0ECD0D, 0x2A0244C8,
		                                           				0x852574E8, 0xE72F23A9, 0x8E26ED02, 0x2D92CBDD, 0xDABC0458,
		                                           				0xCDF5FEB6, 0x9E4E8DCC, 0xF4F1E344, 0x0D8C436D, 0x4427603B,
		                                           				0xBDD37FDA, 0x80505F26, 0x8C7D2B8E, 0xB73273C5, 0x397362EA,
		                                           				0x618A3811, 0x608BFB88, 0x06F7D714, 0x212E4677, 0x28EFCEAD,
		                                           				0x076C0371, 0x36A3A4D9, 0x5487B455, 0x3429A365, 0x65D467AC,
		                                           				0x78EE7EEB, 0x99BF12B7, 0x4D129896, 0x772A5601, 0xCCE284C7,
		                                           				0x2ED85C21, 0xD099E8A4, 0xA179158A, 0x6AC0AB1A, 0x299A4807,
		                                           				0xBE67A58D, 0xDC19544A, 0xB8949B54, 0x8D315779, 0xB6F849C1,
		                                           				0x53C5AC34, 0x66DE92A5, 0xF195DD13, 0x318D3A73, 0x301EC542,
		                                           				0x0CC40DA6, 0xF253ADE4, 0x467EE566, 0xEA5585EC, 0x3BAF19BB,
		                                           				0x7DE9F480, 0x79006E7C, 0xA9B7A197, 0xA44BD8F1, 0xFB2BA739,
		                                           				0xEC342FD4, 0xED4FD32D, 0x3D1789BA, 0x400F5D7F, 0xC798F594,
		                                           				0x4506A847, 0x034C0A95, 0xE2162C9D, 0x55A9CFD0, 0x692D832E,
		                                           				0xCF9DB2CA, 0x5E2287E9, 0xD2610EF3, 0x1AE7ECC2, 0x48399CA0,
		                                           				0xA7E4269B, 0x6EE3A0AF, 0x7065BFE1, 0xA6FFE708, 0x2256804C,
		                                           				0x7476E21B, 0x41B0796C, 0x7C243B05, 0x000A950F, 0x1858416B,
		                                           				0xF5A53C89, 0xE9FEF823, 0x3F443275, 0xE0CBF091, 0x0AF27B84,
		                                           				0x3EBB0F27, 0x1DE6F7F4, 0xC31C29F7, 0xB166DE3D, 0x12932EC3,
		                                           				0x9C0C0674, 0x5CDA81B9, 0xD1BD9D12, 0xAFFD7C82, 0x8962BCA7,
		                                           				0xA342C4A8, 0x62457151, 0x82089F03, 0xEB49C670, 0x5B5F6530,
		                                           				0x7E28BAD2, 0x20880BA3, 0xF0FAAFCD, 0xCE82B56F, 0x0275335C,
		                                           				0xC18E8AFB, 0xDE601D69, 0xBA9B820A, 0xC8A2BE4F, 0xD7CAC335,
		                                           				0xD9A73741, 0x115E974D, 0x7F5AC21D, 0x383BF9C6, 0xBCAEB75F,
		                                           				0xFD0350CE, 0xB5D06B87, 0x9820E03C, 0x72D5F163, 0xE3644FC9,
		                                           				0xA5464C4B, 0x57048FCB, 0x9690C9DF, 0xDBF9EAFA, 0xBFF4649A,
		                                           				0x053C00E3, 0xB4B61136, 0x67593DD1, 0x503EE960, 0x9FB4993A,
		                                           				0x19831810, 0xC670D518, 0xB05B51D8, 0x0F3A1CE5, 0x6CAA1F9C,
		                                           				0xAACC31BE, 0x949ED050, 0x1EAD07E7, 0xA8479ABD, 0xD6CFFCD5,
		                                           				0x936993EF
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x472E91CB, 0x5444B5B6, 0x62BE5861, 0x1BE102C7, 0x63E4B31E,
		                                           				0xE81F71B7, 0x9E2317C9, 0x39A408AE, 0x518024F4, 0x1731C66F,
		                                           				0x68CBC918, 0x71FB0C9E, 0xD03B7FDD, 0x7D6222EB, 0x9057EDA3,
		                                           				0x1A34A407, 0x8CC2253D, 0xB6F6979D, 0x835675DC, 0xF319BE9F,
		                                           				0xBE1CD743, 0x4D32FEE4, 0x77E7D887, 0x37E9EBFD, 0x15F851E8,
		                                           				0x23DC3706, 0x19D78385, 0xBD506933, 0xA13AD4A6, 0x913F1A0E,
		                                           				0xDDE560B9, 0x9A5F0996, 0xA65A0435, 0x48D34C4D, 0xE90839A7,
		                                           				0x8ABBA54E, 0x6FD13CE1, 0xC7EEBD3C, 0x0E297602, 0x58B9BBB4,
		                                           				0xEF7901E6, 0x64A28A62, 0xA509875A, 0xF8834442, 0x2702C709,
		                                           				0x07353F31, 0x3B39F665, 0xF5B18B49, 0x4010AE37, 0x784DE00B,
		                                           				0x7A1121E9, 0xDE918ED3, 0xC8529DCD, 0x816A5D05, 0x02ED8298,
		                                           				0x04E3DD84, 0xFD2BC3E2, 0xAF167089, 0x96AF367E, 0xA4DA6232,
		                                           				0x18FF7325, 0x05F9A9F1, 0x4FEFB9F9, 0xCD94EAA5, 0xBFAA5069,
		                                           				0xA0B8C077, 0x60D86F57, 0xFE71C813, 0x29EBD2C8, 0x4CA86538,
		                                           				0x6BF1A030, 0xA237B88A, 0xAA8AF41D, 0xE1F7B6EC, 0xE214D953,
		                                           				0x33057879, 0x49CAA736, 0xFA45CFF3, 0xC063B411, 0xBA7E27D0,
		                                           				0x31533819, 0x2A004AC1, 0x210EFC3F, 0x2646885E, 0x66727DCF,
		                                           				0x9D7FBF54, 0xA8DD0EA8, 0x3447CACE, 0x3F0C14DB, 0xB8382AAC,
		                                           				0x4ACE3539, 0x0A518D51, 0x95178981, 0x35AEE2CA, 0x73F0F7E3,
		                                           				0x94281140, 0x59D0E523, 0xD292CB88, 0x565D1B27, 0x7EC8FBAF,
		                                           				0x069AF08D, 0xC127FD24, 0x0BC77B10, 0x5F03E7EF, 0x453E99BA,
		                                           				0xEED9FF7F, 0x87B55215, 0x7915AB4C, 0xD389A358, 0x5E75CE6D,
		                                           				0x28D655C0, 0xDAD26C73, 0x2E2510FF, 0x9FA7EECC, 0x1D0629C3,
		                                           				0xDC9C9C46, 0x2D67ECD7, 0xE75E94BD, 0x3D649E2A, 0x6C413A2B,
		                                           				0x706F0D7C, 0xDFB0127B, 0x4E366B55, 0x2C825650, 0x24205720,
		                                           				0xB5C998F7, 0x3E95462C, 0x756E5C72, 0x3259488F, 0x11E8771A,
		                                           				0xA7C0A617, 0x577663E5, 0x089B6401, 0x8EAB1941, 0xAE55EF8C,
		                                           				0x3AAC5460, 0xD4E6262F, 0x5D979A47, 0xB19823B0, 0x7F8D6A0C,
		                                           				0xFFA08683, 0x0170CD0F, 0x858CD5D8, 0x53961C90, 0xC4C61556,
		                                           				0x41F2F226, 0xCFCD062D, 0xF24C03B8, 0xEA81DF5B, 0x7BE2FA52,
		                                           				0xB361F98B, 0xC2901316, 0x55BA4BBC, 0x93B234A9, 0x0FBC6603,
		                                           				0x80A96822, 0x6D60491F, 0x22BD00F8, 0xBCAD5AAD, 0x52F3F13B,
		                                           				0x42FD2B28, 0xB41DD01C, 0xC52C93BF, 0xFC663094, 0x8F58D100,
		                                           				0x43FECC08, 0xC6331E5D, 0xE6480F66, 0xCA847204, 0x4BDF1DA0,
		                                           				0x30CC2EFB, 0x13E02DEA, 0xFB49AC45, 0xF9D4434F, 0xF47C5B9C,
		                                           				0x148879C2, 0x039FC234, 0xA3DB9BFC, 0xD1A1DC5C, 0x763D7CD4,
		                                           				0xED6D2F93, 0xAB13AF6E, 0x1E8E054A, 0xD68F4F9A, 0xC30484B3,
		                                           				0xD7D50AFA, 0x6930855F, 0xCC07DB95, 0xCE746DB1, 0x744E967D,
		                                           				0xF16CF575, 0x8643E8B5, 0xF0EAE38E, 0xE52DE1D1, 0x6587DAE0,
		                                           				0x0C4B8121, 0x1C7AC567, 0xAC0DB20A, 0x36C3A812, 0x5B1A4514,
		                                           				0xA9A3F868, 0xB9263BAA, 0xCB3CE9D2, 0xE44FB1A4, 0x9221BC82,
		                                           				0xB29390FE, 0x6AB41863, 0x974A3E2E, 0x89F531C5, 0x255CA13E,
		                                           				0x8B65D348, 0xEC248F78, 0xD8FC16F0, 0x50ECDEEE, 0x09010792,
		                                           				0x3C7D1FB2, 0xEBA5426B, 0x847B417A, 0x468B40D9, 0x8DC4E680,
		                                           				0x7CC1F391, 0x2F1EB086, 0x6E5BAA6A, 0xE0B395DA, 0xE31B2CF6,
		                                           				0xD9690B0D, 0x729EC464, 0x38403DDE, 0x610B80A2, 0x5CF433AB,
		                                           				0xB0785FC4, 0xD512E4C6, 0xBBB7D699, 0x5A86591B, 0x10CF5376,
		                                           				0x12BF9F4B, 0x980FBAA1, 0x992A4E70, 0x20FA7AE7, 0xF7996EBB,
		                                           				0xC918A2BE, 0x82DE74F2, 0xAD54209B, 0xF66B4D74, 0x1FC5B771,
		                                           				0x169D9229, 0x887761DF, 0x00B667D5, 0xDB425E59, 0xB72F2844,
		                                           				0x9B0AC1F5, 0x9C737E3A, 0x2B85476C, 0x6722ADD6, 0x44A63297,
		                                           				0x0D688CED
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0xABC59484, 0x4107778A, 0x8AD94C6F, 0xFE83DF90, 0x0F64053F,
		                                           				0xD1292E9D, 0xC5744356, 0x8DD1ABB4, 0x4C4E7667, 0xFB4A7FC1,
		                                           				0x74F402CB, 0x70F06AFD, 0xA82286F2, 0x918DD076, 0x7A97C5CE,
		                                           				0x48F7BDE3, 0x6A04D11D, 0xAC243EF7, 0x33AC10CA, 0x2F7A341E,
		                                           				0x5F75157A, 0xF4773381, 0x591C870E, 0x78DF8CC8, 0x22F3ADB0,
		                                           				0x251A5993, 0x09FBEF66, 0x796942A8, 0x97541D2E, 0x2373DAA9,
		                                           				0x1BD2F142, 0xB57E8EB2, 0xE1A5BFDB, 0x7D0EFA92, 0xB3442C94,
		                                           				0xD2CB6447, 0x386AC97E, 0x66D61805, 0xBDADA15E, 0x11BC1AA7,
		                                           				0x14E9F6EA, 0xE533A0C0, 0xF935EE0A, 0x8FEE8A04, 0x810D6D85,
		                                           				0x7C68B6D6, 0x4EDC9AA2, 0x956E897D, 0xED87581A, 0x264BE9D7,
		                                           				0xFF4DDB29, 0x823857C2, 0xE005A9A0, 0xF1CC2450, 0x6F9951E1,
		                                           				0xAADE2310, 0xE70C75F5, 0x83E1A31F, 0x4F7DDE8E, 0xF723B563,
		                                           				0x368E0928, 0x86362B71, 0x21E8982D, 0xDFB3F92B, 0x44676352,
		                                           				0x99EFBA31, 0x2EAB4E1C, 0xFC6CA5E7, 0x0EBE5D4E, 0xA0717D0C,
		                                           				0xB64F8199, 0x946B31A1, 0x5656CBC6, 0xCFFEC3EF, 0x622766C9,
		                                           				0xFA211E35, 0x52F98B89, 0x6D01674B, 0x4978A802, 0xF651F701,
		                                           				0x15B0D43D, 0xD6FF4683, 0x3463855F, 0x672BA29C, 0xBC128312,
		                                           				0x4626A70D, 0xC8927A5A, 0xB8481CF9, 0x1C962262, 0xA21196BA,
		                                           				0xBABA5EE9, 0x5BB162D0, 0x69943BD1, 0x0C47E35C, 0x8CC9619A,
		                                           				0xE284D948, 0x271BF264, 0xC27FB398, 0x4BC70897, 0x60CF202C,
		                                           				0x7F42D6AA, 0xA5A13506, 0x5D3E8860, 0xCEA63D3C, 0x63BF0A8F,
		                                           				0xF02E9EFA, 0xB17B0674, 0xB072B1D3, 0x06E5723B, 0x3737E436,
		                                           				0x24AA49C7, 0x0DED0D18, 0xDB256B14, 0x58B27877, 0xECB49F54,
		                                           				0x6C40256A, 0x6EA92FFB, 0x3906AA4C, 0xC9866FD5, 0x4549323E,
		                                           				0xA7B85FAB, 0x1918CC27, 0x7308D7B5, 0x1E16C7AD, 0x71850B37,
		                                           				0x3095FD78, 0xA63B70E6, 0xD880E2AE, 0x3E282769, 0xA39BA6BC,
		                                           				0x98700FA3, 0xF34C53E8, 0x288AF426, 0xB99D930F, 0xF5B99DF1,
		                                           				0xE9D0C8CF, 0x5AC8405D, 0x50E7217B, 0x511FBBBE, 0x2CA2E639,
		                                           				0xC020301B, 0x356DBC00, 0x8E43DDB9, 0x4D327B4A, 0xF20FF3ED,
		                                           				0x1DBB29BD, 0x43D44779, 0xA1B68F70, 0x6114455B, 0xE63D280B,
		                                           				0x6BF6FF65, 0x10FC39E5, 0x3DAE126E, 0xC1D7CF11, 0xCB60B795,
		                                           				0x1789D5B3, 0x9BCA36B7, 0x08306075, 0x84615608, 0x8B3A0186,
		                                           				0xE88FBECD, 0x7BA47C4D, 0x2DE44DAC, 0x653FE58D, 0xCCA0B968,
		                                           				0xD7FA0E72, 0x93901780, 0x1F2C26CC, 0xAE595B6B, 0xA9ECEA9B,
		                                           				0xE3DBF8C4, 0x319CC130, 0x12981196, 0x01A3A4DE, 0x32C454B6,
		                                           				0x755BD817, 0x3CD871E4, 0xA48BB8DA, 0x02FDEC09, 0xFD2DC2E2,
		                                           				0x9E578088, 0x9A9F916D, 0x4065FE6C, 0x1853999E, 0xC7793F23,
		                                           				0xDC1016BB, 0x969355FF, 0x7EF292F6, 0xCDCE4ADC, 0x05E24416,
		                                           				0x85C16C46, 0xD441D37F, 0x57BD6855, 0x8746F54F, 0x9CA773DF,
		                                           				0x770BAE22, 0x54828413, 0xB75E4B19, 0x04C35C03, 0xBF7CCA07,
		                                           				0x2955C4DD, 0x721DB041, 0xB2394F33, 0x03F51387, 0x89B73C9F,
		                                           				0x0B1737F3, 0x07E69024, 0x9231D245, 0x76193861, 0x88159C15,
		                                           				0xDEB552D9, 0xD9767E40, 0x20C6C0C3, 0x4281977C, 0xF8AFE1E0,
		                                           				0xD32A0751, 0x3FC27432, 0xDDF1DCC5, 0x68581F34, 0x3BCD5025,
		                                           				0x0091B2EE, 0x4AEB6944, 0x1602E743, 0xEA09EB58, 0xEF0A2A8B,
		                                           				0x641E03A5, 0xEB50E021, 0x5C8CCEF8, 0x802FF0B8, 0xD5E3EDFE,
		                                           				0xC4DD1B49, 0x5334CD2A, 0x13F82D2F, 0x47450C20, 0x55DAFBD2,
		                                           				0xBEC0C6F4, 0xB45D7959, 0x3AD36E8C, 0x0AA8AC57, 0x1A3C8D73,
		                                           				0xE45AAFB1, 0x9F664838, 0xC6880053, 0xD0039BBF, 0xEE5F19EB,
		                                           				0xCA0041D8, 0xBBEA3AAF, 0xDA628291, 0x9D5C95D4, 0xADD504A6,
		                                           				0xC39AB482, 0x5E9E14A4, 0x2BE065F0, 0x2A13FC3A, 0x9052E8EC,
		                                           				0xAF6F5AFC
		                                           			},
		                                           		new uint[]
		                                           			{
		                                           				0x519AA8B5, 0xBB303DA9, 0xE00E2B10, 0xDFA6C1DB, 0x2E6B952E,
		                                           				0xEE10DC23, 0x37936D09, 0x1FC42E92, 0x39B25A9F, 0x13FF89F4,
		                                           				0xC8F53FEA, 0x18500BC7, 0x95A0379D, 0x98F751C2, 0x2289C42F,
		                                           				0xA21E4098, 0x6F391F41, 0xF27E7E58, 0x0D0DF887, 0x4B79D540,
		                                           				0x8E8409AA, 0x71FE46F8, 0x688A9B29, 0x3F08B548, 0x84ABE03A,
		                                           				0x5E91B6C1, 0xFDE4C2AE, 0x251D0E72, 0x92D4FEE5, 0xF9371967,
		                                           				0x9175108F, 0xE6E81835, 0x8C8CB8EE, 0xB55A67B3, 0xCEF138CC,
		                                           				0x8B256268, 0x00D815F5, 0xE8810812, 0x77826189, 0xEA73267D,
		                                           				0x19B90F8D, 0x45C33BB4, 0x82477056, 0xE1770075, 0x09467AA6,
		                                           				0xA7C6F54A, 0x79768742, 0x61B86BCA, 0xD6644A44, 0xE33F0171,
		                                           				0xC229FBCD, 0x41B08FEB, 0xD1903E30, 0x65EC9080, 0x563D6FBD,
		                                           				0xF56DA488, 0xEBF64CD8, 0x4934426B, 0x7C8592FC, 0x6ACA8CF2,
		                                           				0x1CEA111B, 0x3A57EE7A, 0xACE11C0D, 0x9942D85E, 0xC4613407,
		                                           				0xFA8E643B, 0x327FC701, 0x4CA9BE82, 0x3352526D, 0x2C047F63,
		                                           				0xF3A8F7DD, 0x1A4A98A8, 0x762ED4D1, 0x27C75008, 0xBDF497C0,
		                                           				0x7A7B84DF, 0x315C28AB, 0x801F93E3, 0xF19B0CA1, 0x8F14E46A,
		                                           				0xE48BA333, 0x9605E625, 0xF03ECB60, 0x60385F2D, 0x902845BA,
		                                           				0x7F96D66F, 0x24BFF05C, 0x2820730B, 0x947133CB, 0xD444828A,
		                                           				0xB343F6F1, 0x0BEF4705, 0x8DA574F9, 0x01E25D6C, 0x1732793E,
		                                           				0x4F0F7B27, 0x364B7117, 0xB2D1DA77, 0xA6C5F1E9, 0x574CA5B1,
		                                           				0x386A3076, 0xAD6894D6, 0x1156D7FA, 0xA48D1D9A, 0x4794C0AF,
		                                           				0x150C0AA0, 0x26D348AC, 0x29FDEABE, 0xA5DEDE53, 0x81671E8E,
		                                           				0x594EE3BF, 0xA96C56E6, 0x3426A726, 0xC5976579, 0xBC22E5E4,
		                                           				0xC1006319, 0xDAAFDD2A, 0xA1A1AA83, 0x3BADD0E7, 0xC3B14981,
		                                           				0xD770B155, 0xCCD7C693, 0x42E944C5, 0x03E0064F, 0xCA95B4EF,
		                                           				0x3DEE81C3, 0xFBBCD98C, 0x1E07E15B, 0x667CE949, 0xE7D6773F,
		                                           				0x21B6124B, 0x6B2A6EF7, 0xD3278A9C, 0x9A988304, 0x75D2AE9B,
		                                           				0xFE49E2FF, 0x9BC24F46, 0x74CC2CF6, 0xA3139F36, 0x6C9EF35A,
		                                           				0x9FC1DFFE, 0x9E5FACDC, 0xAADC8BBB, 0x5ABDBC5F, 0x44B3B390,
		                                           				0xF754EFA7, 0x5FE3BDB7, 0x4E59C886, 0x06A4C984, 0xA0338878,
		                                           				0xCD513CD7, 0x63EBD27E, 0x8ABA80AD, 0x50DA144E, 0x5D9F4E97,
		                                           				0x025B751C, 0x2D580200, 0xB6C05837, 0x580AA15D, 0x54022A6E,
		                                           				0xB41A5415, 0x4863FAB6, 0xB0B79957, 0x46D0D159, 0xDC2B8650,
		                                           				0x20A7BB0C, 0x4A032974, 0xEC8636A2, 0x8548F24C, 0xF6A2BF16,
		                                           				0x1088F4B0, 0x0C2F3A94, 0x525DC396, 0x14065785, 0x2B4DCA52,
		                                           				0x08AEED39, 0xABEDFC99, 0xB1DBCF18, 0x87F85BBC, 0xAE3AFF61,
		                                           				0x433CCD70, 0x5B23CC64, 0x7B453213, 0x5355C545, 0x9318EC0A,
		                                           				0x78692D31, 0x0A21693D, 0xD5666814, 0x05FB59D9, 0xC71985B2,
		                                           				0x2ABB8E0E, 0xCF6E6C91, 0xD9CFE7C6, 0xEFE7132C, 0x9711AB28,
		                                           				0x3CE52732, 0x12D516D2, 0x7209A0D0, 0xD278D306, 0x70FA4B7B,
		                                           				0x1D407DD3, 0xDB0BEBA4, 0xBFD97621, 0xA8BE21E1, 0x1B6F1B66,
		                                           				0x30650DDA, 0xBA7DDBB9, 0x7DF953FB, 0x9D1C3902, 0xEDF0E8D5,
		                                           				0xB8741AE0, 0x0F240565, 0x62CD438B, 0xC616A924, 0xAF7A96A3,
		                                           				0x35365538, 0xE583AF4D, 0x73415EB8, 0x23176A47, 0xFC9CCEE8,
		                                           				0x7EFC9DE2, 0x695E03CF, 0xF8CE66D4, 0x88B4781D, 0x67DD9C03,
		                                           				0x3E8F9E73, 0xC0C95C51, 0xBE314D22, 0x55AA0795, 0xCB1BB011,
		                                           				0xE980FDC8, 0x9C62B7CE, 0xDE2D239E, 0x042CADF3, 0xFFDF04DE,
		                                           				0x5CE6A60F, 0xD8C831ED, 0xB7B5B9EC, 0xB9CBF962, 0xE253B254,
		                                           				0x0735BA1F, 0x16AC917F, 0xDD607C2B, 0x64A335C4, 0x40159A7C,
		                                           				0x869222F0, 0x6EF21769, 0x839D20A5, 0xD03B24C9, 0xF412601E,
		                                           				0x6D72A243, 0x0E018DFD, 0x89F3721A, 0xC94F4134, 0x2F992F20,
		                                           				0x4D87253C
		                                           			}
		                                           	};

		#endregion

		private readonly int m_rounds;
		private readonly uint[] m_state;

		protected Snefru(int a_rounds, HashSize a_hashSize)
			: base((int)a_hashSize, 64 - ((int)a_hashSize))
		{
			m_rounds = a_rounds;
			m_state = new uint[HashSize / 4];

			Initialize();
		}

		public override void Initialize()
		{
			m_state.Clear();

			base.Initialize();
		}

		protected override byte[] GetResult()
		{
			return Converters.ConvertUIntsToBytesSwapOrder(m_state);
		}

		protected override void Finish()
		{
			ulong bits = m_processedBytes * 8;
			int padindex = 2 * BlockSize - m_buffer.Pos - 8;

			var pad = new byte[padindex + 8];

			pad[padindex++] = (byte)(bits >> 56);
			pad[padindex++] = (byte)(bits >> 48);
			pad[padindex++] = (byte)(bits >> 40);
			pad[padindex++] = (byte)(bits >> 32);
			pad[padindex++] = (byte)(bits >> 24);
			pad[padindex++] = (byte)(bits >> 16);
			pad[padindex++] = (byte)(bits >> 8);
			pad[padindex++] = (byte)bits;

			TransformBytes(pad, 0, padindex);
		}

		protected override void TransformBlock(byte[] a_data, int a_index)
		{
			var work = new uint[16];
			Array.Copy(m_state, 0, work, 0, m_state.Length);

			Converters.ConvertBytesToUIntsSwapOrder(a_data, a_index, BlockSize, work, m_state.Length);

			for(int i = 0; i < m_rounds; i++)
			{
				uint[] sbox0 = s_boxes[i * 2];
				uint[] sbox1 = s_boxes[i * 2 + 1];

				for(int j = 0; j < 4; j++)
				{
					work[15] ^= sbox0[(byte)work[0]];
					work[1] ^= sbox0[(byte)work[0]];
					work[0] ^= sbox0[(byte)work[1]];
					work[2] ^= sbox0[(byte)work[1]];
					work[1] ^= sbox1[(byte)work[2]];
					work[3] ^= sbox1[(byte)work[2]];
					work[2] ^= sbox1[(byte)work[3]];
					work[4] ^= sbox1[(byte)work[3]];
					work[3] ^= sbox0[(byte)work[4]];
					work[5] ^= sbox0[(byte)work[4]];
					work[4] ^= sbox0[(byte)work[5]];
					work[6] ^= sbox0[(byte)work[5]];
					work[5] ^= sbox1[(byte)work[6]];
					work[7] ^= sbox1[(byte)work[6]];
					work[6] ^= sbox1[(byte)work[7]];
					work[8] ^= sbox1[(byte)work[7]];
					work[7] ^= sbox0[(byte)work[8]];
					work[9] ^= sbox0[(byte)work[8]];
					work[8] ^= sbox0[(byte)work[9]];
					work[10] ^= sbox0[(byte)work[9]];
					work[9] ^= sbox1[(byte)work[10]];
					work[11] ^= sbox1[(byte)work[10]];
					work[10] ^= sbox1[(byte)work[11]];
					work[12] ^= sbox1[(byte)work[11]];
					work[11] ^= sbox0[(byte)work[12]];
					work[13] ^= sbox0[(byte)work[12]];
					work[12] ^= sbox0[(byte)work[13]];
					work[14] ^= sbox0[(byte)work[13]];
					work[13] ^= sbox1[(byte)work[14]];
					work[15] ^= sbox1[(byte)work[14]];
					work[14] ^= sbox1[(byte)work[15]];
					work[0] ^= sbox1[(byte)work[15]];

					int shift = s_shifts[j];

					for(int k = 0; k < work.Length; k++)
						work[k] = (work[k] >> shift) | (work[k] << (32 - shift));
				}
			}

			m_state[0] ^= work[15];
			m_state[1] ^= work[14];
			m_state[2] ^= work[13];
			m_state[3] ^= work[12];

			if(HashSize == 32)
			{
				m_state[4] ^= work[11];
				m_state[5] ^= work[10];
				m_state[6] ^= work[9];
				m_state[7] ^= work[8];
			}
		}
	}
}