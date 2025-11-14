using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LasUpload
{
	struct GlobalVar
	{
		public static string szVersion = "v1.3.0.3";
#if DEBUG
		public static bool bLocalTest = true;   // 로컬 테스트
#else
        public static bool bLocalTest = false;	// 라인 런
#endif
		public const int nMaxEQ = 16;

		public static string[] szEQ_Code = new string[nMaxEQ]
		{ "EQ_01", "EQ_02", "EQ_03", "EQ_04", "EQ_05", "EQ_06", "EQ_07", "EQ_08", "EQ_09", "EQ_10", "EQ_11", "EQ_12", "EQ_13", "EQ_14", "EQ_15", "EQ_16" };

		public static string[] szHPC_IP = new string[nMaxEQ];
		public static string[] szVPC1_IP = new string[nMaxEQ];
		public static string[] szVPC2_IP = new string[nMaxEQ];
		public static string[] szVPC3_IP = new string[nMaxEQ];
		public static string[] szVPC4_IP = new string[nMaxEQ];

		// Las Upload
		public static string szLU_Code = string.Empty;

		public static int nLotIdLength = 0;
		public static string szLotIdHead1 = string.Empty;
		public static string szLotIdHead2 = string.Empty;
		public static string szLotIdHead3 = string.Empty;
		public static bool   bUseLotEndHpc = true;
		public static string szLotDataPath = string.Empty;
		public static int    nLotEndHour = 12;

		public static string szEvmsImgPath = string.Empty;	// Result
		public static string szEvmsLogPath = string.Empty;
		public static double dEvmsLimit = 0.0;

		public static string szImagePath = string.Empty;	// Result
		public static string szLogFilePath = string.Empty;
        public static bool bBeginDateChanged = false;
        public static string szBeginDate = string.Empty;

        public static string[] szZipFilePath = new string[4];
	}

	class CommonData
	{
		public bool Read_NetworkFile(string sFile)
		{
			if (!File.Exists(sFile)) return false;

			IniFile Ini = new IniFile(sFile);

			for (int i = 0; i < GlobalVar.nMaxEQ; i++) GlobalVar.szHPC_IP[i] = Ini.Get_String("IP_Address", GlobalVar.szEQ_Code[i] + "_HPC", "");
			for (int i = 0; i < GlobalVar.nMaxEQ; i++) GlobalVar.szVPC1_IP[i] = Ini.Get_String("IP_Address", GlobalVar.szEQ_Code[i] + "_VPC1", "");
			for (int i = 0; i < GlobalVar.nMaxEQ; i++) GlobalVar.szVPC2_IP[i] = Ini.Get_String("IP_Address", GlobalVar.szEQ_Code[i] + "_VPC2", "");
			for (int i = 0; i < GlobalVar.nMaxEQ; i++) GlobalVar.szVPC3_IP[i] = Ini.Get_String("IP_Address", GlobalVar.szEQ_Code[i] + "_VPC3", "");
			for (int i = 0; i < GlobalVar.nMaxEQ; i++) GlobalVar.szVPC4_IP[i] = Ini.Get_String("IP_Address", GlobalVar.szEQ_Code[i] + "_VPC4", "");

			return true;
		}

		public bool Read_LuFile(string sFile)
		{
			if (!File.Exists(sFile)) return false;

			IniFile Ini = new IniFile(sFile);

			GlobalVar.szLU_Code = Ini.Get_String("LAS", "LU_Code", "");

			GlobalVar.nLotIdLength = Ini.Get_Integer("LAS", "LotIdLength", 0);
			GlobalVar.szLotIdHead1 = Ini.Get_String("LAS", "LotIdHead1", "");
			GlobalVar.szLotIdHead2 = Ini.Get_String("LAS", "LotIdHead2", "");
			GlobalVar.szLotIdHead3 = Ini.Get_String("LAS", "LotIdHead3", "");
			GlobalVar.bUseLotEndHpc = Ini.Get_Bool("LAS", "UseLotEndHpc", true);
			GlobalVar.szLotDataPath = Ini.Get_String("LAS", "LotDataPath", "");
			GlobalVar.nLotEndHour = Ini.Get_Integer("LAS", "LotEndHour", 12);

			GlobalVar.szEvmsImgPath = Ini.Get_String("LAS", "EvmsImgPath", "");
			GlobalVar.szEvmsLogPath = Ini.Get_String("LAS", "EvmsLogPath", "");
			GlobalVar.dEvmsLimit = Ini.Get_Double("LAS", "EvmsLimit", 90.0);

			GlobalVar.szImagePath = Ini.Get_String("LAS", "ImagePath", "");
			GlobalVar.szLogFilePath = Ini.Get_String("LAS", "LogFilePath", "");
			GlobalVar.szBeginDate = Ini.Get_String("LAS", "BeginDate", "");

			return true;
		}

		public void Save_LasFile(string sFile, string strType, string strValue)
		{
            IniFile Ini = new IniFile(sFile);
            if (!Ini.Check_File()) return;

            Ini.Set_String("LAS", strType, strValue);
        }
	}
}
