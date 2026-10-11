using System.Collections.Generic;

public static class ConstantsAES
{
	public const int AES_SW_KEY = 0;

	public const int AES_HW_WRAP_KEY = 2;

	public const int AES_KEY_256 = 32;

	public static Dictionary<string, int> regval;

	static ConstantsAES()
	{
		string key = "HACC_CON";
		string key2 = "HACC_ACON";
		string key3 = "HACC_ACON2";
		string key4 = "HACC_ACONK";
		string key5 = "HACC_ASRC0";
		string key6 = "HACC_ASRC1";
		string key7 = "HACC_ASRC2";
		string key8 = "HACC_ASRC3";
		string key9 = "HACC_AKEY0";
		string key10 = "HACC_AKEY1";
		string key11 = "HACC_AKEY2";
		string key12 = "HACC_AKEY3";
		string key13 = "HACC_AKEY4";
		string key14 = "HACC_AKEY5";
		string key15 = "HACC_AKEY6";
		string key16 = "HACC_AKEY7";
		string key17 = "HACC_ACFG0";
		string key18 = "HACC_ACFG1";
		string key19 = "HACC_ACFG2";
		string key20 = "HACC_ACFG3";
		string key21 = "HACC_AOUT0";
		string key22 = "HACC_AOUT1";
		string key23 = "HACC_AOUT2";
		string key24 = "HACC_AOUT3";
		string key25 = "HACC_SW_OTP0";
		string key26 = "HACC_SW_OTP1";
		string key27 = "HACC_SW_OTP2";
		string key28 = "HACC_SW_OTP3";
		string key29 = "HACC_SW_OTP4";
		string key30 = "HACC_SW_OTP5";
		string key31 = "HACC_SW_OTP6";
		string key32 = "HACC_SW_OTP7";
		string key33 = "HACC_SECINIT0";
		string key34 = "HACC_SECINIT1";
		string key35 = "HACC_SECINIT2";
		string key36 = "HACC_MKJ";
		string key37 = "HACC_UNK";
		regval = new Dictionary<string, int>
		{
			{ key, 0 },
			{ key2, 4 },
			{ key3, 8 },
			{ key4, 12 },
			{ key5, 16 },
			{ key6, 20 },
			{ key7, 24 },
			{ key8, 28 },
			{ key9, 32 },
			{ key10, 36 },
			{ key11, 40 },
			{ key12, 44 },
			{ key13, 48 },
			{ key14, 52 },
			{ key15, 56 },
			{ key16, 60 },
			{ key17, 64 },
			{ key18, 68 },
			{ key19, 72 },
			{ key20, 76 },
			{ key21, 80 },
			{ key22, 84 },
			{ key23, 88 },
			{ key24, 92 },
			{ key25, 96 },
			{ key26, 100 },
			{ key27, 104 },
			{ key28, 108 },
			{ key29, 112 },
			{ key30, 116 },
			{ key31, 120 },
			{ key32, 124 },
			{ key33, 128 },
			{ key34, 132 },
			{ key35, 136 },
			{ key36, 160 },
			{ key37, 188 }
		};
	}

	public static int method_01(int arg_01, int arg_02)
	{
		return arg_01 ^ arg_02;
	}
}
