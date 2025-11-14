using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LasUpload
{
	public class LogFile	// Static Class
	{
		private static object m_lockSaveProcess = new object();
		private static object m_lockSaveUpload = new object();

		public static List<string> Read_FileList(string sFile)
		{
			if (!File.Exists(sFile)) return null;

			List<string> lstReturn = new List<string>();
			string strData = string.Empty;
			using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read)) {
				StreamReader sr = new StreamReader(fs, Encoding.Default);
				while ((strData = sr.ReadLine()) != null) lstReturn.Add(strData);
				sr.Close();
			}
			return lstReturn;
		}

		public static void Write_DeleteLotFile(string sFile, string sMsg)
		{
			string strDir = sFile.Substring(0, sFile.LastIndexOf(@"\"));
			if (!Directory.Exists(strDir)) Directory.CreateDirectory(strDir);

			try {
				string strData = string.Empty;
				using (FileStream fs = new FileStream(sFile, FileMode.Append, FileAccess.Write)) {
					StreamWriter sw = new StreamWriter(fs, Encoding.Default);
					sw.WriteLine(sMsg);
					sw.Close();
				}
			} catch {
				string strMsg = "Fail Write DeleteLot File," + sFile + "," + sMsg;
				Save_ProcessLog(strMsg);
			}
		}

		///////////////////////////////////////////////////////////////////////////////

		private static void Save_Log(string sFile, string sLog)
		{
			string strTime = DateTime.Now.ToString("[HH:mm:ss.fff] ");
			string strSave = strTime + sLog;

			using (FileStream fs = new FileStream(sFile, FileMode.Append, FileAccess.Write)) {
				StreamWriter sw = new StreamWriter(fs, Encoding.Default);
				sw.WriteLine(strSave);
				sw.Close();
			}
		}

		public static void Save_ProcessLog(string sLog)
		{
			if (!Directory.Exists(GlobalVar.szLogFilePath)) Directory.CreateDirectory(GlobalVar.szLogFilePath);

			lock (m_lockSaveProcess) {
				string strDate = DateTime.Now.ToString("yyyy-MM-dd");
				string strFile = GlobalVar.szLogFilePath + @"\" + strDate + "_Process.txt";
				Save_Log(strFile, sLog);
			}
		}

		public static void Save_UploadLog(string sLog)
		{
			if (!Directory.Exists(GlobalVar.szLogFilePath)) Directory.CreateDirectory(GlobalVar.szLogFilePath);

			lock (m_lockSaveUpload) {
				string strDate = DateTime.Now.ToString("yyyy-MM-dd");
				string strFile = GlobalVar.szLogFilePath + @"\" + strDate + "_Upload.txt";
				Save_Log(strFile, sLog);
			}
		}
	}
}
