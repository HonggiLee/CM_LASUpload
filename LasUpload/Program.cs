using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LasUpload
{
	static class Program
	{
		/// <summary>
		/// 해당 응용 프로그램의 주 진입점입니다.
		/// </summary>
		[STAThread]
		static void Main()
		{
			string strLuFile = Application.StartupPath + @"\LAS.ini";
			if (!File.Exists(strLuFile)) return;

			IniFile Ini = new IniFile(strLuFile);
			string strLuCode = Ini.Get_String("LAS", "LU_Code", "");

			bool bSucess = false;
			System.Threading.Mutex m_hMutex = new System.Threading.Mutex(true, strLuCode, out bSucess);
			if (!bSucess) return;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());

			m_hMutex.ReleaseMutex();
		}
	}
}
