using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace LasUpload
{
	class IniFile
	{
		public string m_strFile;

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		public IniFile(string sFile)
		{
			m_strFile = sFile;
		}

		public bool Check_File()
		{
			if (File.Exists(m_strFile)) return true;
			return false;
		}

		///////////////////////////////////////////////////////////////////////////////
		// Get Function
		public bool Get_Bool(string strApp, string strKey, bool bDefault)
		{
			StringBuilder sReturn = new StringBuilder(100);
			string strDefault = (bDefault ? "true" : "false");
			GetPrivateProfileString(strApp, strKey, strDefault, sReturn, 100, m_strFile);
			return (sReturn.ToString() == "true");
		}

		public int Get_Integer(string strApp, string strKey, int nDefault)
		{
			return GetPrivateProfileInt(strApp, strKey, nDefault, m_strFile);
		}

		public long Get_Long(string strApp, string strKey, long lDefault)
		{
			return (long)GetPrivateProfileInt(strApp, strKey, (int)lDefault, m_strFile);
		}

		public float Get_Float(string strApp, string strKey, float fDefault)
		{
			float fReturn = fDefault;
			StringBuilder sReturn = new StringBuilder(100);
			string strDefault = fDefault.ToString();
			GetPrivateProfileString(strApp, strKey, strDefault, sReturn, 100, m_strFile);
			float.TryParse(sReturn.ToString(), out fReturn);
			return fReturn;
		}

		public double Get_Double(string strApp, string strKey, double dDefault)
		{
			double dReturn = dDefault;
			StringBuilder sReturn = new StringBuilder(255);
			string strDefault = dDefault.ToString();
			GetPrivateProfileString(strApp, strKey, strDefault, sReturn, 255, m_strFile);
			double.TryParse(sReturn.ToString(), out dReturn);
			return dReturn;
		}

		public string Get_String(string strApp, string strKey, string strDefault)
		{
			StringBuilder sReturn = new StringBuilder(255);
			GetPrivateProfileString(strApp, strKey, strDefault, sReturn, 255, m_strFile);
			return sReturn.ToString();
		}

		///////////////////////////////////////////////////////////////////////////////
		// Set Function
		public void Set_Bool(string strApp, string strKey, bool bValue)
		{
			string strValue = (bValue ? "true" : "false");
			WritePrivateProfileString(strApp, strKey, strValue, m_strFile);
		}

		public void Set_Integer(string strApp, string strKey, int nValue)
		{
			string strValue = nValue.ToString();
			WritePrivateProfileString(strApp, strKey, strValue, m_strFile);
		}

		public void Set_Long(string strApp, string strKey, long lValue)
		{
			string strValue = lValue.ToString();
			WritePrivateProfileString(strApp, strKey, strValue, m_strFile);
		}

		public void Set_Float(string strApp, string strKey, float fValue, string sFormat)
		{
			string strValue = string.Format("{0:" + sFormat + "}", fValue);
			WritePrivateProfileString(strApp, strKey, strValue, m_strFile);
		}

		public void Set_Double(string strApp, string strKey, double dValue, string sFormat)
		{
			string strValue = string.Format("{0:" + sFormat + "}", dValue);
			WritePrivateProfileString(strApp, strKey, strValue, m_strFile);
		}

		public void Set_String(string strApp, string strKey, string strValue)
		{
			WritePrivateProfileString(strApp, strKey, strValue, m_strFile);
		}
	}
}
