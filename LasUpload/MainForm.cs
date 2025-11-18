using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.Compression;

namespace LasUpload
{
	public partial class MainForm : Form
	{
		private CommonData m_Data = new CommonData();
		private BackgroundWorker m_Worker = new BackgroundWorker();
		private delegate void UpdateUICallback(int nType, string sMsg);

		private string[] m_szLotIdHead1s; // G,P,V
		private string[] m_szLotIdHead2s; // P,T,U,G,R,Q
		private string[] m_szLotIdHead3s; // ""(2,3,6,5,S,F,C,H,Q)

		private string m_szPathLasRw = string.Empty; // Las Raw Path (E:\,F:\,G:\)
		private string m_szPathPC1Rt = string.Empty; // PC1 Result Path
		private string m_szPathPC2Rt = string.Empty; // PC2 Result Path
		private string m_szPathPC3Rt = string.Empty; // PC3 Result Path
		private string m_szPathPC4Rt = string.Empty; // PC4 Result Path

		private string m_szTargetPath = string.Empty; // D:\EVMS_TEMP

		private int m_nEqIdx = -1;	// EQ Index
		private string m_strEqCode = string.Empty;

		private bool m_bAutoStartable = false;
		private int m_nAutoTimerCount = 0;
		private bool m_bEmgStop = false;	// E-Stop

		private const int m_nMaxLines = 1500;

		// LotResult.txt (공통 8EA : Date,Time,Machine_Code,LotID,Config,Tray,Barcode,ModuleNo,...)
		private string[,] m_strLotResultV = new string[m_nMaxLines, 408]; // 108+100+100+100
		private string[,] m_strLotResult2 = new string[m_nMaxLines, 108];
		private string[,] m_strLotResult3 = new string[m_nMaxLines, 108];
		private string[,] m_strLotResult4 = new string[m_nMaxLines, 108];

		// ADJLotResult.txt (공통 8EA : Date,Time,Machine_Code,LotID,Config,Tray,Barcode,ModuleNo,...)
		private string[,] m_strAdjResultV = new string[m_nMaxLines, 88]; // 28+20+20+20
		private string[,] m_strAdjResult2 = new string[m_nMaxLines, 28];
		private string[,] m_strAdjResult3 = new string[m_nMaxLines, 28];
		private string[,] m_strAdjResult4 = new string[m_nMaxLines, 28];

        // FAILotResult.txt (공통 8EA : Time,Machine_Code,LotID,Config,SWVersion,Tray,Barcode,ModuleNo,...)
        private string[,] m_strFAILotResultV = new string[m_nMaxLines, 408]; // 408+200+200+200
        private string[,] m_strFAILotResult2 = new string[m_nMaxLines, 208];
        private string[,] m_strFAILotResult3 = new string[m_nMaxLines, 208];
        private string[,] m_strFAILotResult4 = new string[m_nMaxLines, 208];

        private string m_strPcName = SystemInformation.ComputerName;

		protected override CreateParams CreateParams
		{
			get {
				CreateParams newParam = base.CreateParams;
				newParam.ClassStyle = newParam.ClassStyle | 0x200;  // ControlBox Close Disable
				return newParam;
			}
		}

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Text += " - " + GlobalVar.szVersion; // 버전 표시
			if (GlobalVar.bLocalTest) Text += " (Debug)";

			string strNetFile = Application.StartupPath + @"\Network.ini";
			if (!m_Data.Read_NetworkFile(strNetFile)) {  // Ini File 내용을 읽는다.
				MessageBox.Show("Network.ini 파일을 찾을 수 없습니다. 담당자를 호출하시기 바랍니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Close();
			}

			string strLuFile = Application.StartupPath + @"\LAS.ini";
			if (!m_Data.Read_LuFile(strLuFile)) {  // Ini File 내용을 읽는다.
				MessageBox.Show("LAS.ini 파일을 찾을 수 없습니다. 담당자를 호출하시기 바랍니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Close();
			}

			int nCode = 0;
			int.TryParse(GlobalVar.szLU_Code.Substring(3), out nCode);
			if (nCode < 1) return;

			m_szLotIdHead1s = GlobalVar.szLotIdHead1.Split(',');
			m_szLotIdHead2s = GlobalVar.szLotIdHead2.Split(',');
			m_szLotIdHead3s = GlobalVar.szLotIdHead3.Split(',');

			m_nEqIdx = nCode - 1;
			m_strEqCode = GlobalVar.szEQ_Code[m_nEqIdx];

			lblEqCode.Text = m_strEqCode;

			lblStatus.Text = "Ready";
			lblLotId.Text = "";
			lblVisionPC.Text = "";

			txtSourcePath.Text = "";
			m_szTargetPath = GlobalVar.szEvmsImgPath.Substring(0, 3) + "EVMS_TEMP";
			txtTargetPath.Text = m_szTargetPath;

			lblEvmsLimit.Text = GlobalVar.dEvmsLimit.ToString("#0.00");
			lblBeginDate.Text = GlobalVar.szBeginDate;

			m_szPathPC1Rt = @"\\" + GlobalVar.szVPC1_IP[m_nEqIdx] + GlobalVar.szImagePath; // VPC1 Result
			m_szPathPC2Rt = @"\\" + GlobalVar.szVPC2_IP[m_nEqIdx] + GlobalVar.szImagePath; // VPC2 Result
			m_szPathPC3Rt = @"\\" + GlobalVar.szVPC3_IP[m_nEqIdx] + GlobalVar.szImagePath; // VPC3 Result
            m_szPathPC4Rt = @"\\" + GlobalVar.szVPC4_IP[m_nEqIdx] + GlobalVar.szImagePath; // VPC4 Result
            if (GlobalVar.bLocalTest) m_szPathPC1Rt = m_szPathPC1Rt.Replace(GlobalVar.szVPC1_IP[m_nEqIdx], @"127.0.0.1\VPC1"); // VPC1 Result
            if (GlobalVar.bLocalTest) m_szPathPC2Rt = m_szPathPC2Rt.Replace(GlobalVar.szVPC2_IP[m_nEqIdx], @"127.0.0.1\VPC2"); // VPC2 Result
            if (GlobalVar.bLocalTest) m_szPathPC3Rt = m_szPathPC3Rt.Replace(GlobalVar.szVPC3_IP[m_nEqIdx], @"127.0.0.1\VPC3"); // VPC3 Result
            if (GlobalVar.bLocalTest) m_szPathPC4Rt = m_szPathPC4Rt.Replace(GlobalVar.szVPC4_IP[m_nEqIdx], @"127.0.0.1\VPC4"); // VPC4 Result

            if (!Directory.Exists(m_szTargetPath)) Directory.CreateDirectory(m_szTargetPath);
			if (!Directory.Exists(GlobalVar.szEvmsImgPath)) Directory.CreateDirectory(GlobalVar.szEvmsImgPath);
			if (!Directory.Exists(GlobalVar.szEvmsLogPath)) Directory.CreateDirectory(GlobalVar.szEvmsLogPath);

			picRunStop.Image = Properties.Resources.Stop;       // Stop LED
			picAutoStartable.Image = Properties.Resources.Stop; // Stop LED

			m_bAutoStartable = false;
			m_nAutoTimerCount = 0;

			string strMsg = "Program Start - " + GlobalVar.szVersion;
			lblMessage.Text = strMsg;
			Set_SystemBoxMessage(strMsg);
			LogFile.Save_ProcessLog(strMsg);

			m_Worker.WorkerReportsProgress = true;
			m_Worker.WorkerSupportsCancellation = true;
			m_Worker.DoWork += new DoWorkEventHandler(BackWorker_DoWork);
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("작업을 종료하겠습니까?", GlobalVar.szLU_Code + " 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
				btnStop_Click(null, null);
				Application.DoEvents();

				timerMain.Enabled = false;
				string strMsg = "Program End - " + GlobalVar.szVersion;
				lblMessage.Text = strMsg;
				Set_SystemBoxMessage(strMsg);
				LogFile.Save_ProcessLog(strMsg);

				Close();
			}
		}

		private void timerMain_Tick(object sender, EventArgs e)
		{
			lblWorkDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
			lblWorkTime.Text = DateTime.Now.ToString("HH:mm:ss");

			picRunStop.Image = (m_Worker.IsBusy ? Properties.Resources.Run : Properties.Resources.Stop);
			picAutoStartable.Image = (m_bAutoStartable ? Properties.Resources.Run : Properties.Resources.Stop);

			if (m_Worker.IsBusy || !m_bAutoStartable) return;

			if (m_nAutoTimerCount > 20) { // 10초 후 자동 시작 (Timer Interval : 500)
				m_nAutoTimerCount = 0;
				m_Worker.RunWorkerAsync();
				string strMsg = "Program Auto Re-Start - " + GlobalVar.szVersion;
				Set_SystemBoxMessage(strMsg);
				LogFile.Save_ProcessLog(strMsg);
			} else m_nAutoTimerCount++;
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			string strMsg = "Click Start Button," + GlobalVar.szLU_Code + "," + m_strEqCode;
			lblMessage.Text = strMsg;

			if (m_Worker.IsBusy) return;

			m_Worker.RunWorkerAsync();
			m_bAutoStartable = true;

			strMsg = "Start," + GlobalVar.szLU_Code + "," + m_strEqCode;
			Set_SystemBoxMessage(strMsg);
			LogFile.Save_ProcessLog(strMsg);
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			string strMsg = "Click Stop Button," + GlobalVar.szLU_Code + "," + m_strEqCode;
			lblMessage.Text = strMsg;

			m_bAutoStartable = false;
			if (!m_Worker.IsBusy) return;

			m_Worker.CancelAsync();

			lblMessage.ForeColor = Color.Red;
			strMsg = "현재 진행중인 LOT의 Upload 작업이 완료될때 까지 기다려 주세요...";
			lblMessage.Text = strMsg;

			while (m_Worker.IsBusy) { Application.DoEvents(); Thread.Sleep(5); }

			lblMessage.ForeColor = Color.Blue;
			strMsg = "Stop," + GlobalVar.szLU_Code + "," + m_strEqCode;
			Set_SystemBoxMessage(strMsg);
			LogFile.Save_ProcessLog(strMsg);
		}

		private void btnEStop_Click(object sender, EventArgs e)
		{
			string strMsg = "Click E-Stop Button," + GlobalVar.szLU_Code + "," + m_strEqCode;
			lblMessage.Text = strMsg;

			m_bAutoStartable = false;
			if (m_bEmgStop) return;
			if (!m_Worker.IsBusy) return;

			m_bEmgStop = true;
			m_Worker.CancelAsync();
			while (m_Worker.IsBusy) { Application.DoEvents(); Thread.Sleep(5); }
			m_bEmgStop = false;

			strMsg = "E-Stop," + GlobalVar.szLU_Code + "," + m_strEqCode;
			Set_SystemBoxMessage(strMsg);
			LogFile.Save_ProcessLog(strMsg);
		}

		private void Update_UI(int nType, string sMsg)
		{
			switch (nType) {
			case 1:     // lstSystemBox
				Set_SystemBoxMessage(sMsg);
				break;
			case 2:     // Status
				lblStatus.Text = sMsg;
				break;
			case 3:     // Lot Id
				lblLotId.Text = sMsg;
				break;
			case 4:     // Vision PC
				lblVisionPC.Text = sMsg;
				break;
			case 5:     // Source Path (VPC)
				txtSourcePath.Text = sMsg;
				break;
			case 6:     // Target Path (EVMS_TEMP)
				txtTargetPath.Text = sMsg;
				break;
			}
		}

		private void Set_SystemBoxMessage(string sMessage)
		{
			string strTime = DateTime.Now.ToString("[HH:mm:ss.fff] ");

			lstSystemBox.Items.Add(strTime + sMessage);

			if (chkAutoClearSystemBox.Checked) {
				int nMaxLine = 100;
				int.TryParse(txtSystemBoxMax.Text, out nMaxLine);

				while (lstSystemBox.Items.Count > nMaxLine) lstSystemBox.Items.RemoveAt(0);
			}

			if (lstSystemBox.Items.Count == 0) return;
			lstSystemBox.SelectedIndex = lstSystemBox.Items.Count - 1;
		}

		private void btnClearSystemBox_Click(object sender, EventArgs e)
		{
			lstSystemBox.Items.Clear();
		}

		/////////////////////////////////////////////////////////////////////////////
		// Main Run

		private void BackWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true) {
				if (m_Worker.CancellationPending) { e.Cancel = true; break; }
				AutoRun_Thread();
				Thread.Sleep(100);
			}
		}

		public void Error_Stop(int nType, int nNo=0)
		{
			m_Worker.CancelAsync();
			Thread.Sleep(100);

			string strMsg = "Program Error Stop - ";
			switch (nType) {
			case 1: strMsg += "EVMS Limit Over."; break;
			case 2: strMsg += "Vision PC " + nNo.ToString() + " Not Connected."; break;
			case 3: strMsg += "Vision PC was Disconnected in Working."; break;
			}

			UpdateUICallback d = new UpdateUICallback(Update_UI);
			Invoke(d, new object[] { 1, strMsg }); // Update Message
			LogFile.Save_ProcessLog(strMsg);
		}

		public void AutoRun_Thread()
        {
            DriveInfo drv = new DriveInfo(m_szTargetPath.Substring(0, 1));  // Check Drive("D")
            double dEvmsUseRatio = (drv.TotalSize - drv.TotalFreeSpace) * 100.0 / drv.TotalSize;
            if (dEvmsUseRatio > GlobalVar.dEvmsLimit) { Error_Stop(1); return; }

            m_szPathLasRw = m_szPathPC1Rt; // Vision PC1로 설정
			int nPC = Check_VisionPC(m_szPathLasRw);
			if (nPC != 0) { Error_Stop(2, nPC); return; }

			DateTime dtBeginDate = Convert.ToDateTime(GlobalVar.szBeginDate);
			int nBeginYear = dtBeginDate.Year;
			int nBeginMonth = dtBeginDate.Month;
			int nBeginDay = dtBeginDate.Day;

			int nCurrYear = DateTime.Now.Year;
			int nCurrMonth = DateTime.Now.Month;
			int nCurrDay = DateTime.Now.Day;

			List<string> lstTempY = Directory.GetDirectories(m_szPathLasRw, "*", SearchOption.TopDirectoryOnly).ToList();
			List<string> lstYears = Sort_FolderList(m_szPathLasRw + @"\", lstTempY);
			foreach (string strPathY in lstYears) {
				string strY = strPathY.Substring(strPathY.LastIndexOf(@"\") + 1);
				int nYear = int.Parse(strY);
				if (nYear > nCurrYear || nYear < nBeginYear) continue;

				List<string> lstTempM = Directory.GetDirectories(strPathY, "*", SearchOption.TopDirectoryOnly).ToList();
				List<string> lstMonths = Sort_FolderList(strPathY + @"\", lstTempM);
				foreach (string strPathM in lstMonths) {
					string strM = strPathM.Substring(strPathM.LastIndexOf(@"\") + 1);
					int nMonth = int.Parse(strM);
					if (nMonth > 12) continue;
                    if (nYear == nCurrYear && (nMonth > nCurrMonth || nMonth < nBeginMonth)) continue;

                    List<string> strTempD = Directory.GetDirectories(strPathM, "*", SearchOption.TopDirectoryOnly).ToList();
					List<string> strDays = Sort_FolderList(strPathM + @"\", strTempD);
					foreach (string strPathD in strDays) {
						string strD = strPathD.Substring(strPathD.LastIndexOf(@"\") + 1);
						int nDay = int.Parse(strD);
						if (nDay > 31) continue;
                        if (nYear == nCurrYear && nMonth == nCurrMonth && (nDay > nCurrDay || nDay < nBeginDay)) continue;

                        string[] strLotIds = Directory.GetDirectories(strPathD, "*", SearchOption.TopDirectoryOnly);
						foreach (string strLotPath in strLotIds) {
							if (!MainRun_Work(nYear, nMonth, nDay, strLotPath)) continue;
                            return;	// 멈출수 있도록...
						}
					}
				}
			}
		}

		private bool MainRun_Work(int nY, int nM, int nD, string sLotPath)
		{
			UpdateUICallback d = new UpdateUICallback(Update_UI);

			string strLotName = sLotPath.Substring(sLotPath.LastIndexOf(@"\") + 1);
			if (strLotName.Length < GlobalVar.nLotIdLength) return false; // Lot ID 문자열 비교

			string strHead1 = strLotName.Substring(0, 1);
			string strHead2 = strLotName.Substring(1, 1);
			string strHead3 = strLotName.Substring(2, 1);
			if (GlobalVar.szLotIdHead1 != "" && !m_szLotIdHead1s.Contains(strHead1)) return false;
			if (GlobalVar.szLotIdHead2 != "" && !m_szLotIdHead2s.Contains(strHead2)) return false;
			if (GlobalVar.szLotIdHead3 != "" && !m_szLotIdHead3s.Contains(strHead3)) return false;

			if (!Check_LotEnd(sLotPath, nY, nM, nD)) return false; // Lot End 아니면 PASS

			string strDeleteLotFile = m_szPathLasRw + @"\Delete_Lot\" + nY.ToString() + nM.ToString("00") + nD.ToString("00") + ".txt";
			if (File.Exists(strDeleteLotFile)) {
				List<string> lstRead = LogFile.Read_FileList(strDeleteLotFile);
				if (lstRead == null) return false;
				bool bExistList = false;
				for (int i = 0; i < lstRead.Count; i++) {
					string[] strGroups = lstRead[i].Split(',');
					if (strLotName == strGroups[0]) { bExistList = true; break; }
				}
				if (bExistList) return false;
			}

            string strMissingLotFile = m_szPathLasRw + @"\Missing_Lot\" + nY.ToString() + nM.ToString("00") + nD.ToString("00") + ".txt";
            if (File.Exists(strMissingLotFile))
            {
                string strAll = File.ReadAllText(strMissingLotFile);
                if (strAll.Contains(strLotName + ",")) return false;
            }
            //추가함 25.1.7 LHG
            string strNow = DateTime.Now.ToString("yyyy-MM-dd,HH:mm:ss");
            if (!Check_VisionFolder(sLotPath))
            {
                LogFile.Write_DeleteLotFile(strMissingLotFile, strLotName + ", LotPath Fail," + strNow);
                return false; // Check Vision Lot Folder
            }

            bool bLotResultOK = Check_LotResultCount(sLotPath);
			if (!bLotResultOK) LogFile.Save_ProcessLog("Fail LotResult Count," + sLotPath);

			bool bAdjResultOK = Check_AdjResultCount(sLotPath);
			if (!bAdjResultOK) LogFile.Save_ProcessLog("Fail AdjResult Count," + sLotPath);

            bool bFAILotResultOK = Check_FAILotResultCount(sLotPath);//수정필요
            if (!bFAILotResultOK) LogFile.Save_ProcessLog("Fail FAILotResult Count," + sLotPath);

            bool bVisionLASOK = Check_VisionLASCount(sLotPath, strLotName);
            if (!bVisionLASOK) LogFile.Save_ProcessLog("Fail VisionLAS Count," + sLotPath);

            DateTime dtStart = DateTime.Now;

			Invoke(d, new object[] { 2, "Copy..." }); // Update Status
			Invoke(d, new object[] { 3, strLotName }); // Update LotId
			Invoke(d, new object[] { 5, sLotPath }); // Update Source

			string strMsg = "Start Copy & Compress, LotId: " + strLotName;
			Invoke(d, new object[] { 1, strMsg }); // Update Message
			LogFile.Save_ProcessLog(strMsg);
			LogFile.Save_UploadLog(strMsg);

			string strLasCode = "EOL-" + GlobalVar.szLU_Code.Substring(3);

            // LAS Image Copy & Compress
            // 압축파일 규칙 : LotID_sensorID_testitem_Barcode_Test시간_PCID_Format_PC+VPCNo.zip
            //일부 파일이 검색되지 않아 
            for (int i =0; i<4; i++) {
                string strImageZipFile = m_szTargetPath + @"\" + strLotName + "__" + "PC" + (i + 1) + "__" + dtStart.ToString("yyyyMMddHHmmss") + "_" + strLasCode + ".zip";
                int nResult = Image_Upload(sLotPath, strImageZipFile, i); // 0:Success, 1:EmgStop
                if (nResult == 1) return true;
                if (nResult == 2) { strMsg = "Result Image Compress Fail," + strLotName; LogFile.Save_ProcessLog(strMsg); Error_Stop(3); return true; }
                if (nResult == 3) {
                    LogFile.Write_DeleteLotFile(strMissingLotFile, strLotName + ", VisionPC LotPath Fail," + strNow);
                    strMsg = "Vision PC Check Fail," + strLotName;
                    LogFile.Save_ProcessLog(strMsg);
                    Error_Stop(3);
                    return true;
                }

                DateTime dtFinish = DateTime.Now;

                string strStart = dtStart.ToString("HH:mm:ss");
                string strFinish = dtFinish.ToString("HH:mm:ss");
                string strTerm = (dtFinish - dtStart).TotalSeconds.ToString("F3");

                strMsg = "End Copy & Compress, LotId: " + strLotName + ", Start:" + strStart + ", Finish:" + strFinish + ", Term:" + strTerm;
                Invoke(d, new object[] { 1, strMsg });  // Update Message
                LogFile.Save_ProcessLog(strMsg);
                LogFile.Save_UploadLog(strMsg);

                // Move Compress Files (Result Image : LogS 폴더 6개월 보관)
                string strEvmsRstImgFile = strImageZipFile.Replace(m_szTargetPath, GlobalVar.szEvmsImgPath);
                File.Move(strImageZipFile, strEvmsRstImgFile);

            }
            // 로그파일 규칙 : LotID_Test시간_파일명 => GSY742FLA3EQ_20201021143020_LotResult_EOL.txt
            string strLotResultFile = m_szTargetPath + @"\" + strLotName + "_" + dtStart.ToString("yyyyMMddHHmmss") + "_LotResult_EOL.csv";
            string strAdjResultFile = m_szTargetPath + @"\" + strLotName + "_" + dtStart.ToString("yyyyMMddHHmmss") + "_AdjResult_EOL.csv";
            string strDefectShapeFile = m_szTargetPath + @"\" + strLotName + "_" + dtStart.ToString("yyyyMMddHHmmss") + "_DefectShape_EOL.csv";
            string strMultiDefectFile = m_szTargetPath + @"\" + strLotName + "_" + dtStart.ToString("yyyyMMddHHmmss") + "_MultiDefect_EOL.csv";
            string strFAILotResultFile = m_szTargetPath + @"\" + strLotName + "_" + dtStart.ToString("yyyyMMddHHmmss") + "_FAILotResult_EOL.csv";
            string strVisionLASFile = m_szTargetPath + @"\" + strLotName + "_" + dtStart.ToString("yyyyMMddHHmmss") + "_VisionLAS_EOL.csv";

            bool bMergeLotResult = false;
            if (bLotResultOK) bMergeLotResult = LotResult_Upload(sLotPath, strLotResultFile);
            if (bLotResultOK && !bMergeLotResult) LogFile.Save_ProcessLog("Fail LotResult Merge," + sLotPath);

            bool bMergeAdjResult = false;
            if (bAdjResultOK) bMergeAdjResult = AdjResult_Upload(sLotPath, strAdjResultFile);
            if (bAdjResultOK && !bMergeAdjResult) LogFile.Save_ProcessLog("Fail AdjResult Merge," + sLotPath);

            bool bMergeShape = DefectShape_Upload(sLotPath, strDefectShapeFile); // PC1~4중 하나라도 있으면 올린다. 아니면 DefectShape은 안올린다.
            if (!bMergeShape) LogFile.Save_ProcessLog("Fail DefectShape Merge," + sLotPath);

            bool bMergeMulti = MultiDefect_Upload(sLotPath, strMultiDefectFile); // PC1~4중 하나라도 있으면 올린다. 아니면 MultiDefect은 안올린다.
            if (!bMergeMulti) LogFile.Save_ProcessLog("Fail MultiDefect Merge," + sLotPath);

            bool bMergeFAILotResult = false;
            if (bFAILotResultOK) bMergeFAILotResult = FAILotResult_Upload(sLotPath, strFAILotResultFile);
            if (bFAILotResultOK && !bMergeFAILotResult) LogFile.Save_ProcessLog("Fail FAILotResult Merge," + sLotPath);

            bool bMergeVisionLAS = false;
            if (bVisionLASOK) bMergeVisionLAS = VisionLAS_Upload(sLotPath, strVisionLASFile, strLotName);
            if (bVisionLASOK && !bMergeVisionLAS) LogFile.Save_ProcessLog("Fail VisionLAS Merge," + sLotPath);

            if (bMergeLotResult)
            {
                string strEvmsLotLogFile = strLotResultFile.Replace(m_szTargetPath, GlobalVar.szEvmsLogPath);
                File.Move(strLotResultFile, strEvmsLotLogFile);
            }

            if (bMergeAdjResult)
            {
                string strEvmsAdjLogFile = strAdjResultFile.Replace(m_szTargetPath, GlobalVar.szEvmsLogPath);
                File.Move(strAdjResultFile, strEvmsAdjLogFile);
            }

            if (bMergeShape)
            {
                string strEvmsShapeFile = strDefectShapeFile.Replace(m_szTargetPath, GlobalVar.szEvmsLogPath); // Log
                File.Move(strDefectShapeFile, strEvmsShapeFile);
            }

            if (bMergeMulti)
            {
                string strEvmsMultiFile = strMultiDefectFile.Replace(m_szTargetPath, GlobalVar.szEvmsLogPath); // Log
                if (bMergeMulti) File.Move(strMultiDefectFile, strEvmsMultiFile);
            }

            if (bMergeFAILotResult)
            {
                string strEvmsFAIFile = strFAILotResultFile.Replace(m_szTargetPath, GlobalVar.szEvmsLogPath); // Log
                if (bMergeFAILotResult) File.Move(strFAILotResultFile, strEvmsFAIFile);
            }
            if (bMergeVisionLAS)
            {
                string strEvmsVisionLASFile = strVisionLASFile.Replace(m_szTargetPath, GlobalVar.szEvmsLogPath); // Log
                if (bMergeVisionLAS) File.Move(strVisionLASFile, strEvmsVisionLASFile);
            }

            strMsg = "Move Compress Image & Result Data, LotId: " + strLotName;
            Invoke(d, new object[] { 1, strMsg });  // Update Message
            LogFile.Save_ProcessLog(strMsg);
            LogFile.Save_UploadLog(strMsg);

            strMsg = "Sucess LAS Upload," + sLotPath;
            LogFile.Save_ProcessLog(strMsg);

            // Upload 기록
            //string strNow = DateTime.Now.ToString("yyyy-MM-dd,HH:mm:ss");

			string strDeleteRt2File = strDeleteLotFile.Replace(m_szPathLasRw, m_szPathPC2Rt);
			string strDeleteRt3File = strDeleteLotFile.Replace(m_szPathLasRw, m_szPathPC3Rt);
			string strDeleteRt4File = strDeleteLotFile.Replace(m_szPathLasRw, m_szPathPC4Rt);
			if (Check_FolderExist(m_szPathPC2Rt, 100)) LogFile.Write_DeleteLotFile(strDeleteRt2File, strLotName + "," + strNow);
			if (Check_FolderExist(m_szPathPC3Rt, 100)) LogFile.Write_DeleteLotFile(strDeleteRt3File, strLotName + "," + strNow);
			if (Check_FolderExist(m_szPathPC4Rt, 100)) LogFile.Write_DeleteLotFile(strDeleteRt4File, strLotName + "," + strNow);

			LogFile.Write_DeleteLotFile(strDeleteLotFile, strLotName + "," + strNow); // VPC1을 마지막에 Update

			Invoke(d, new object[] { 2, "Ready" }); // Update Status
			Invoke(d, new object[] { 3, "" });      // Update LotId
			Invoke(d, new object[] { 4, "" });      // Update Vision PC

			return true;
		}

		private int Image_Upload(string sLotPath, string sZipFile, int index)
		{
			UpdateUICallback d = new UpdateUICallback(Update_UI);

            string[] strExtensions; //File 확장자

            string strLotName = sLotPath.Substring(sLotPath.LastIndexOf(@"\") + 1);
            // 기존 LOT 파일 삭제
            foreach (string strDelFile in Directory.EnumerateFiles(m_szTargetPath, strLotName + "_*.*")) File.Delete(strDelFile);

			    
				string strMsg = "PC" + (index + 1).ToString();
				Invoke(d, new object[] { 4, strMsg });  // Update Vision PC
				strMsg += ", ZipFile, " + sZipFile;
				LogFile.Save_UploadLog(strMsg);

				string strLotPath = sLotPath;
				if (index == 1) strLotPath = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt);
				if (index == 2) strLotPath = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt);
				if (index == 3) strLotPath = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt);

				try {
					ZipArchiveMode zipMode = (File.Exists(sZipFile) ? ZipArchiveMode.Update : ZipArchiveMode.Create);
					using (FileStream fs = new FileStream(sZipFile, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
						using (ZipArchive za = new ZipArchive(fs, zipMode)) {
							List<string> lstTempNos = Directory.GetDirectories(strLotPath, "Tray-*", SearchOption.TopDirectoryOnly).ToList();
							List<string> lstTrayNos = Sort_FolderList(strLotPath + @"\Tray-", lstTempNos);
							foreach (string strTrayPath in lstTrayNos) {
								string strTrayName = strTrayPath.Substring(strTrayPath.LastIndexOf(@"\") + 1);
								za.CreateEntry(strTrayName + @"\");

								List<string> lstImgTypes = Directory.GetDirectories(strTrayPath, "*", SearchOption.TopDirectoryOnly).ToList();

                                strExtensions = new[] { "*.jpg", "*.png" };

                                foreach (string strTypePath in lstImgTypes) {
									if (!strTypePath.Contains("ResultImage") && !strTypePath.Contains("ReviewImage") && !strTypePath.Contains("AdjImage")) continue;

									string strTypeName = strTypePath.Substring(strTypePath.LastIndexOf(@"\") + 1);
									za.CreateEntry(strTrayName + @"\" + strTypeName + @"\");

                                    foreach (string strExt in strExtensions)
                                    {
                                        foreach (var file in Directory.EnumerateFiles(strTypePath, strExt))
                                        {
                                            //string strFileName = file.Substring(file.LastIndexOf(@"\") + 1);
                                            string strFileName = Path.GetFileName(file);
                                            za.CreateEntryFromFile(file, strTrayName + @"\" + strTypeName + @"\" + strFileName);
                                        }
									}
								}
							}
						}
					}
				} catch { return 2; } // Result Image Compress Fail

				if (!Check_VisionFolder(sLotPath)) return 3;

				if (m_bEmgStop) return 1; // 즉시 종료
			
			return 0;	// Success
		}

		private bool LotResult_Upload(string sLotPath, string sResultFile)
		{
			string strMsg = "LotResult, Start, " + sResultFile;
			LogFile.Save_UploadLog(strMsg);

			int nLines = 0; // PC1 기준 Line Count
			int[] nFields = new int[4] { 0, 0, 0, 0 };  // 각 PC의 Field Count
			int nFieldCom = 8;  // 공통 Field Count

			for (int i = 0; i < m_nMaxLines; i++) {
				for (int j = 0; j < 408; j++) m_strLotResultV[i, j] = "";
				for (int j = 0; j < 108; j++) m_strLotResult2[i, j] = "";
				for (int j = 0; j < 108; j++) m_strLotResult3[i, j] = "";
				for (int j = 0; j < 108; j++) m_strLotResult4[i, j] = "";
			}

			for (int i = 0; i < 4; i++) {	// Vision PC1 ~ PC4
				string strSource = sLotPath + @"\LotResult.txt";
				if (i == 1) strSource = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt) + @"\LotResult.txt";
				if (i == 2) strSource = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt) + @"\LotResult.txt";
				if (i == 3) strSource = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt) + @"\LotResult.txt";

				List<string> lstRead = LogFile.Read_FileList(strSource);
				if (lstRead == null) return false;
				if (i == 0) nLines = lstRead.Count; // VPC 모두 같아야 한다. (이전에 체크함.)
				if (i != 0 && nLines != lstRead.Count) return false; // Next

				// Fileds : PC1(8+100), PC2(8+100), PC3(8+100), PC4(8+100)
				for (int j = 0; j < nLines; j++) {
					string[] strFields = lstRead[j].Split('\t');
					if (strFields.Length > 108) return false;
					if (j == 0) nFields[i] = strFields.Length;  // Field Count
					for (int k = 0; k < strFields.Length; k++) {
						if (i == 0) m_strLotResultV[j, k] = strFields[k];
						if (i == 1) m_strLotResult2[j, k] = strFields[k];
						if (i == 2) m_strLotResult3[j, k] = strFields[k];
						if (i == 3) m_strLotResult4[j, k] = strFields[k];
					}
				}
			}

			int nFieldV1 = nFields[0];
			int nFieldV2 = nFieldV1 + nFields[1] - nFieldCom;
			int nFieldV3 = nFieldV2 + nFields[2] - nFieldCom;
			int nFieldV4 = nFieldV3 + nFields[3] - nFieldCom;

			for (int i = 0; i < nLines; i++) {
				for (int j = nFieldV1; j < nFieldV4; j++) {
					if (i == 0) {   // 제목줄
						if		(j < nFieldV2) m_strLotResultV[i, j] = m_strLotResult2[i, j - nFieldV1 + nFieldCom];
						else if (j < nFieldV3) m_strLotResultV[i, j] = m_strLotResult3[i, j - nFieldV2 + nFieldCom];
						else if (j < nFieldV4) m_strLotResultV[i, j] = m_strLotResult4[i, j - nFieldV3 + nFieldCom];

					} else {
						int nV2 = Get_LotResultLine(2, nLines, m_strLotResultV[i, 6]); // Barcode
						int nV3 = Get_LotResultLine(3, nLines, m_strLotResultV[i, 6]); // Barcode
						int nV4 = Get_LotResultLine(4, nLines, m_strLotResultV[i, 6]); // Barcode

						if		(j < nFieldV2) m_strLotResultV[i, j] = m_strLotResult2[nV2, j - nFieldV1 + nFieldCom];
						else if (j < nFieldV3) m_strLotResultV[i, j] = m_strLotResult3[nV3, j - nFieldV2 + nFieldCom];
						else if (j < nFieldV4) m_strLotResultV[i, j] = m_strLotResult4[nV4, j - nFieldV3 + nFieldCom];
					}
				}
			}

			string strWrite = string.Empty;
			using (FileStream fs = new FileStream(sResultFile, FileMode.Append, FileAccess.Write)) {
				StreamWriter sw = new StreamWriter(fs, Encoding.Default);
				for (int i = 0; i < nLines; i++) {
					for (int j = 0; j < nFieldV4; j++) {
						if (j == 0) strWrite = m_strLotResultV[i, j];
						else if (j == 3) {
							if (i == 0) strWrite += ",Station," + m_strLotResultV[i, j];
							else strWrite += "," + m_strPcName + "," + m_strLotResultV[i, j];
						}
						else strWrite += ',' + m_strLotResultV[i, j];
					}
					sw.WriteLine(strWrite);
				}
				sw.Close();
			}

			strMsg = "LotResult, End, " + sResultFile;
			LogFile.Save_UploadLog(strMsg);

			return true;
		}

		private bool AdjResult_Upload(string sLotPath, string sResultFile)
		{
			string strMsg = "AdjResult, Start, " + sResultFile;
			LogFile.Save_UploadLog(strMsg);

			int nLines = 0; // PC1 기준 Line Count
			int[] nFields = new int[4] { 0, 0, 0, 0 };  // 각 PC의 Field Count
			int nFieldCom = 8;  // 공통 Field Count

			for (int i = 0; i < m_nMaxLines; i++) {
				for (int j = 0; j < 88; j++) m_strAdjResultV[i, j] = "";
				for (int j = 0; j < 28; j++) m_strAdjResult2[i, j] = "";
				for (int j = 0; j < 28; j++) m_strAdjResult3[i, j] = "";
				for (int j = 0; j < 28; j++) m_strAdjResult4[i, j] = "";
			}

			for (int i = 0; i < 4; i++) {
				string strSource = sLotPath + @"\ADJLotResult.txt";
				if (i == 1) strSource = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt) + @"\ADJLotResult.txt";
				if (i == 2) strSource = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt) + @"\ADJLotResult.txt";
				if (i == 3) strSource = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt) + @"\ADJLotResult.txt";

				List<string> lstRead = LogFile.Read_FileList(strSource);
				if (lstRead == null) return false;
				if (i == 0) nLines = lstRead.Count; // VPC 모두 같아야 한다. (이전에 체크함.)
				if (i != 0 && nLines != lstRead.Count) return false; // Next

				// Fileds : PC1(8+20), PC2(8+20), PC3(8+20), PC4(8+20)
				for (int j = 0; j < nLines; j++) {
					string[] strFields = lstRead[j].Split('\t');
					if (strFields.Length > 28) return false;
					if (j == 0) nFields[i] = strFields.Length;  // Field Count
					for (int k = 0; k < strFields.Length; k++) {
						if (i == 0) m_strAdjResultV[j, k] = strFields[k];
						if (i == 1) m_strAdjResult2[j, k] = strFields[k];
						if (i == 2) m_strAdjResult3[j, k] = strFields[k];
						if (i == 3) m_strAdjResult4[j, k] = strFields[k];
					}
				}
			}

			int nFieldV1 = nFields[0];
			int nFieldV2 = nFieldV1 + nFields[1] - nFieldCom;
			int nFieldV3 = nFieldV2 + nFields[2] - nFieldCom;
			int nFieldV4 = nFieldV3 + nFields[3] - nFieldCom;
			for (int i = 0; i < nLines; i++) {
				for (int j = nFieldV1; j < nFieldV4; j++) {
					if (i == 0) {   // 제목줄
						if		(j < nFieldV2) m_strAdjResultV[i, j] = m_strAdjResult2[i, j - nFieldV1 + nFieldCom];
						else if (j < nFieldV3) m_strAdjResultV[i, j] = m_strAdjResult3[i, j - nFieldV2 + nFieldCom];
						else if (j < nFieldV4) m_strAdjResultV[i, j] = m_strAdjResult4[i, j - nFieldV3 + nFieldCom];

					} else {
						int nV2 = Get_AdjResultLine(2, nLines, m_strAdjResultV[i, 6]); // Barcode
						int nV3 = Get_AdjResultLine(3, nLines, m_strAdjResultV[i, 6]); // Barcode
						int nV4 = Get_AdjResultLine(4, nLines, m_strAdjResultV[i, 6]); // Barcode

						if		(j < nFieldV2) m_strAdjResultV[i, j] = m_strAdjResult2[nV2, j - nFieldV1 + nFieldCom];
						else if (j < nFieldV3) m_strAdjResultV[i, j] = m_strAdjResult3[nV3, j - nFieldV2 + nFieldCom];
						else if (j < nFieldV4) m_strAdjResultV[i, j] = m_strAdjResult4[nV4, j - nFieldV3 + nFieldCom];
					}
				}
			}

			string strWrite = string.Empty;
			using (FileStream fs = new FileStream(sResultFile, FileMode.Append, FileAccess.Write)) {
				StreamWriter sw = new StreamWriter(fs, Encoding.Default);
				for (int i = 0; i < nLines; i++) {
					for (int j = 0; j < nFieldV4; j++) {
						if (j == 0) strWrite = m_strAdjResultV[i, j];
						else if (j == 3) {
							if (i == 0) strWrite += ",Station," + m_strAdjResultV[i, j];
							else strWrite += "," + m_strPcName + "," + m_strAdjResultV[i, j];
						}
						else strWrite += ',' + m_strAdjResultV[i, j];
					}
					sw.WriteLine(strWrite);
				}
				sw.Close();
			}

			strMsg = "AdjResult, End, " + sResultFile;
			LogFile.Save_UploadLog(strMsg);

			return true;
		}

		private bool DefectShape_Upload(string sLotPath, string sResultFile)
		{
			string strMsg = "DefectShape, Start, " + sResultFile;
			LogFile.Save_UploadLog(strMsg);

			string strPath2 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt);
			string strPath3 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt);
			string strPath4 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt);

			bool bExist1 = File.Exists(sLotPath + @"\Defect_Shape_Feature.txt");
			bool bExist2 = File.Exists(strPath2 + @"\Defect_Shape_Feature.txt");
			bool bExist3 = File.Exists(strPath3 + @"\Defect_Shape_Feature.txt");
			bool bExist4 = File.Exists(strPath4 + @"\Defect_Shape_Feature.txt");

			if (!bExist1 && !bExist2 && !bExist3 && !bExist4) return false;	// 모두 없으면

			for (int i = 0; i < 4; i++) {
				string strLotPath = string.Empty;
				if (i == 0) strLotPath = sLotPath;
				if (i == 1) strLotPath = strPath2;
				if (i == 2) strLotPath = strPath3;
				if (i == 3) strLotPath = strPath4;

				string strSource = strLotPath + @"\Defect_Shape_Feature.txt";
				if (!File.Exists(strSource)) continue;

				List<string> lstRead = LogFile.Read_FileList(strSource);
				int nLines = lstRead.Count;

				string strWrite = string.Empty;
				using (FileStream fs = new FileStream(sResultFile, FileMode.Append, FileAccess.Write)) {
					StreamWriter sw = new StreamWriter(fs, Encoding.Default);
					for (int j = 0; j < nLines; j++) {
						//if (i != 0 && j == 0) continue;	// PC2,3,4 Title
						string[] strFields = lstRead[j].Split('\t');
						int nFields = strFields.Length;  // Field Count
						for (int k = 0; k < nFields; k++) {
							if (k == 0) strWrite = strFields[k];
							else if (k == 3) {
								if (j == 0) strWrite += ",Station," + strFields[k];
								else strWrite += "," + m_strPcName + "," + strFields[k];
							} else strWrite += ',' + strFields[k];
						}
						sw.WriteLine(strWrite);
					}
					sw.Close();
				}
			}

			strMsg = "DefectShape, End, " + sResultFile;
			LogFile.Save_UploadLog(strMsg);

			return true;
		}

		private bool MultiDefect_Upload(string sLotPath, string sResultFile)
		{
			string strMsg = "MultiDefect, Start, " + sResultFile;
			LogFile.Save_UploadLog(strMsg);

			string strPath2 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt);
			string strPath3 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt);
			string strPath4 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt);

			bool bExist1 = File.Exists(sLotPath + @"\MultipleDefectList.txt");
			bool bExist2 = File.Exists(strPath2 + @"\MultipleDefectList.txt");
			bool bExist3 = File.Exists(strPath3 + @"\MultipleDefectList.txt");
			bool bExist4 = File.Exists(strPath4 + @"\MultipleDefectList.txt");

			if (!bExist1 && !bExist2 && !bExist3 && !bExist4) return false; // 모두 없으면

			for (int i = 0; i < 4; i++) {
				string strLotPath = string.Empty;
				if (i == 0) strLotPath = sLotPath;
				if (i == 1) strLotPath = strPath2;
				if (i == 2) strLotPath = strPath3;
				if (i == 3) strLotPath = strPath4;

				string strSource = strLotPath + @"\MultipleDefectList.txt";
				if (!File.Exists(strSource)) continue;

				List<string> lstRead = LogFile.Read_FileList(strSource);
				int nLines = lstRead.Count;

				string strWrite = string.Empty;
				using (FileStream fs = new FileStream(sResultFile, FileMode.Append, FileAccess.Write)) {
					StreamWriter sw = new StreamWriter(fs, Encoding.Default);
					for (int j = 0; j < nLines; j++) {
						//if (i != 0 && j == 0) continue;	// PC2,3,4 Title
						string[] strFields = lstRead[j].Split('\t');
						int nFields = strFields.Length;  // Field Count
						for (int k = 0; k < nFields; k++) {
							if (k == 0) strWrite = strFields[k];
							else if (k == 3) {
								if (j == 0) strWrite += ",Station," + strFields[k];
								else strWrite += "," + m_strPcName + "," + strFields[k];
							} else strWrite += ',' + strFields[k];
						}
						sw.WriteLine(strWrite);
					}
					sw.Close();
				}
			}

			strMsg = "MultiDefect, End, " + sResultFile;
			LogFile.Save_UploadLog(strMsg);

			return true;
		}

        private bool FAILotResult_Upload(string sLotPath, string sResultFile)
        {
            string strMsg = "FAILotResult, Start, " + sResultFile;
            LogFile.Save_UploadLog(strMsg);

            int nLines = 0; // PC1 기준 Line Count
            int[] nFields = new int[4] { 0, 0, 0, 0 };  // 각 PC의 Field Count
            int nFieldCom = 8;  // 공통 Field Count

            for (int i = 0; i < m_nMaxLines; i++)
            {
                for (int j = 0; j < 408; j++) m_strFAILotResultV[i, j] = "";
                for (int j = 0; j < 208; j++) m_strFAILotResult2[i, j] = "";
                for (int j = 0; j < 208; j++) m_strFAILotResult3[i, j] = "";
                for (int j = 0; j < 208; j++) m_strFAILotResult4[i, j] = "";
            }

            for (int i = 0; i < 4; i++)
            {   // Vision PC1 ~ PC4
                string strSource = sLotPath + @"\FAILotResult.txt";
                if (i == 1) strSource = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt) + @"\FAILotResult.txt";
                if (i == 2) strSource = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt) + @"\FAILotResult.txt";
                if (i == 3) strSource = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt) + @"\FAILotResult.txt";

                List<string> lstRead = LogFile.Read_FileList(strSource);
                if (lstRead == null) return false;
                if (i == 0) nLines = lstRead.Count; // VPC 모두 같아야 한다. (이전에 체크함.)
                if (i != 0 && nLines != lstRead.Count) return false; // Next

                // Fileds : PC1(8+400), PC2(8+200), PC3(8+200), PC4(8+200)
                for (int j = 0; j < nLines; j++)
                {
                    string[] strFields = lstRead[j].Split('\t');
                    if (strFields.Length > 408) return false;
                    if (j == 0) nFields[i] = strFields.Length;  // Field Count
                    for (int k = 0; k < strFields.Length; k++)
                    {
                        if (i == 0) m_strFAILotResultV[j, k] = strFields[k];
                        if (i == 1) m_strFAILotResult2[j, k] = strFields[k];
                        if (i == 2) m_strFAILotResult3[j, k] = strFields[k];
                        if (i == 3) m_strFAILotResult4[j, k] = strFields[k];
                    }
                }
            }

            int nFieldV1 = nFields[0];
            int nFieldV2 = nFieldV1 + nFields[1] - nFieldCom;
            int nFieldV3 = nFieldV2 + nFields[2] - nFieldCom;
            int nFieldV4 = nFieldV3 + nFields[3] - nFieldCom;

            for (int i = 0; i < nLines; i++)
            {
                for (int j = nFieldV1; j < nFieldV4; j++)
                {
                    if (i == 0)
                    {   // 제목줄
                        if (j < nFieldV2) m_strFAILotResultV[i, j] = m_strFAILotResult2[i, j - nFieldV1 + nFieldCom];
                        else if (j < nFieldV3) m_strFAILotResultV[i, j] = m_strFAILotResult3[i, j - nFieldV2 + nFieldCom];
                        else if (j < nFieldV4) m_strFAILotResultV[i, j] = m_strFAILotResult4[i, j - nFieldV3 + nFieldCom];

                    }
                    else
                    {
                        int nV2 = Get_FAILotResultLine(2, nLines, m_strFAILotResultV[i, 6]); // Barcode
                        int nV3 = Get_FAILotResultLine(3, nLines, m_strFAILotResultV[i, 6]); // Barcode
                        int nV4 = Get_FAILotResultLine(4, nLines, m_strFAILotResultV[i, 6]); // Barcode

                        if (j < nFieldV2) m_strFAILotResultV[i, j] = m_strFAILotResult2[nV2, j - nFieldV1 + nFieldCom];
                        else if (j < nFieldV3) m_strFAILotResultV[i, j] = m_strFAILotResult3[nV3, j - nFieldV2 + nFieldCom];
                        else if (j < nFieldV4) m_strFAILotResultV[i, j] = m_strFAILotResult4[nV4, j - nFieldV3 + nFieldCom];
                    }
                }
            }

            string strWrite = string.Empty;
            using (FileStream fs = new FileStream(sResultFile, FileMode.Append, FileAccess.Write))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                for (int i = 0; i < nLines; i++)
                {
                    for (int j = 0; j < nFieldV4; j++)
                    {
                        if (j == 0) strWrite = m_strFAILotResultV[i, j];
                        else if (j == 3)
                        {
                            if (i == 0) strWrite += ",Station," + m_strFAILotResultV[i, j];
                            else strWrite += "," + m_strPcName + "," + m_strFAILotResultV[i, j];
                        }
                        else strWrite += ',' + m_strFAILotResultV[i, j];
                    }
                    sw.WriteLine(strWrite);
                }
                sw.Close();
            }

            strMsg = "FAILotResult, End, " + sResultFile;
            LogFile.Save_UploadLog(strMsg);

            return true;
        }
        private bool VisionLAS_Upload(string sLotPath, string sResultFile, string strLotName)
        {
            string strMsg = "VisionLAS, Start, " + sResultFile;
            LogFile.Save_UploadLog(strMsg);

            string strPath2 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt);
            string strPath3 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt);
            string strPath4 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt);

            bool bExist1 = File.Exists(sLotPath + @"\VisionLAS_" + strLotName + ".txt");
            bool bExist2 = File.Exists(strPath2 + @"VisionLAS_" + strLotName + ".txt");
            bool bExist3 = File.Exists(strPath3 + @"VisionLAS_" + strLotName + ".txt");
            bool bExist4 = File.Exists(strPath4 + @"VisionLAS_" + strLotName + ".txt");

            if (!bExist1 && !bExist2 && !bExist3 && !bExist4) return false; // 모두 없으면

            for (int i = 0; i < 4; i++)
            {
                string strLotPath = string.Empty;
                if (i == 0) strLotPath = sLotPath;
                if (i == 1) strLotPath = strPath2;
                if (i == 2) strLotPath = strPath3;
                if (i == 3) strLotPath = strPath4;

                string strSource = strLotPath + @"\VisionLAS_" + strLotName + ".txt";
                if (!File.Exists(strSource)) continue;

                List<string> lstRead = LogFile.Read_FileList(strSource);
                int nLines = lstRead.Count;

                string strWrite = string.Empty;
                using (FileStream fs = new FileStream(sResultFile, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    for (int j = 0; j < nLines; j++)
                    {
                        if (i != 0 && j == 0) continue;	// PC2,3,4 Title
                        string[] strFields = lstRead[j].Split('\t');
                        int nFields = strFields.Length;  // Field Count
                        for (int k = 0; k < nFields; k++)
                        {
                            if (k == 0) strWrite = strFields[k];
                            else if (k == 3)
                            {
                                if (j == 0) strWrite += ",Station," + strFields[k];
                                else strWrite += "," + m_strPcName + "," + strFields[k];
                            }
                            else strWrite += ',' + strFields[k];
                        }
                        sw.WriteLine(strWrite);
                    }
                    sw.Close();
                }
            }

            strMsg = "VisionLAS, End, " + sResultFile;
            LogFile.Save_UploadLog(strMsg);

            return true;
        }
        private List<string> Sort_FolderList(string sPreStr, List<string> lstSource)
		{
			List<int> lstInt = new List<int>();
			foreach (string str in lstSource) {
				string strTemp = str.Replace(sPreStr, "");
				int nTemp = 0;
				if (!int.TryParse(strTemp, out nTemp)) continue; // 문자 제거
				if (nTemp < 1) continue; // 1 부터 시작
				lstInt.Add(nTemp);
			}
			lstInt.Sort();

			List<string> lstStr = new List<string>();
			foreach (int nInt in lstInt) lstStr.Add(sPreStr + nInt.ToString());

			return lstStr;
		}

		private int Check_VisionPC(string sPath)
		{
			string strPath1Rt = sPath.Replace(m_szPathLasRw, m_szPathPC1Rt); if (!Directory.Exists(strPath1Rt)) return 1;
			string strPath2Rt = sPath.Replace(m_szPathLasRw, m_szPathPC2Rt); if (!Directory.Exists(strPath2Rt)) return 2;
			string strPath3Rt = sPath.Replace(m_szPathLasRw, m_szPathPC3Rt); if (!Directory.Exists(strPath3Rt)) return 3;
			string strPath4Rt = sPath.Replace(m_szPathLasRw, m_szPathPC4Rt); if (!Directory.Exists(strPath4Rt)) return 4;
			return 0;
		}

		private bool Check_VisionFolder(string sLotPath)
		{
            // ResultImage만 체크
            string strLotPath1Rt = sLotPath.Replace(m_szPathLasRw, m_szPathPC1Rt); if (!Directory.Exists(strLotPath1Rt)) return false;//추가 항목
            string strLotPath2Rt = sLotPath.Replace(m_szPathLasRw, m_szPathPC2Rt); if (!Directory.Exists(strLotPath2Rt)) return false;
			string strLotPath3Rt = sLotPath.Replace(m_szPathLasRw, m_szPathPC3Rt); if (!Directory.Exists(strLotPath3Rt)) return false;
			string strLotPath4Rt = sLotPath.Replace(m_szPathLasRw, m_szPathPC4Rt); if (!Directory.Exists(strLotPath4Rt)) return false;
			return true;
		}

		private bool Check_LotEnd(string sLotPath, int nY, int nM, int nD)
		{
			string strLotName = sLotPath.Substring(sLotPath.LastIndexOf(@"\") + 1);
			string strFilter = strLotName + "_*.txt";

			if (GlobalVar.bUseLotEndHpc) {	// HPC Lot End 기록 사용 여부
				string strDay = nY.ToString("00") + "-" + nM.ToString("00") + "-" + nD.ToString("00");
				string strLotPath = @"\\" + GlobalVar.szHPC_IP[m_nEqIdx] + GlobalVar.szLotDataPath + @"\" + strDay.Replace("-", @"\");
                if (GlobalVar.bLocalTest) strLotPath = strLotPath.Replace(GlobalVar.szHPC_IP[m_nEqIdx], "127.0.0.1");

                if (Directory.Exists(strLotPath)) {
					IEnumerable<string> enumList = Directory.EnumerateFiles(strLotPath, strFilter);
					if (enumList.Count() > 0) return true;
				}

				// 다음날 확인 (Lot End 가 다음날에 될 경우)
				string strNextDay = Convert.ToDateTime(strDay).AddDays(1).ToString("yyyy-MM-dd");
				strLotPath = @"\\" + GlobalVar.szHPC_IP[m_nEqIdx] + GlobalVar.szLotDataPath + @"\" + strNextDay.Replace("-", @"\");
                if (GlobalVar.bLocalTest) strLotPath = strLotPath.Replace(GlobalVar.szHPC_IP[m_nEqIdx], "127.0.0.1");

                if (Directory.Exists(strLotPath)) {
					IEnumerable<string> enumList = Directory.EnumerateFiles(strLotPath, strFilter);
					if (enumList.Count() > 0) return true;
				}
			}

            DirectoryInfo di = new DirectoryInfo(sLotPath);
            TimeSpan spanHour = DateTime.Now - di.LastWriteTime;
            if (spanHour.TotalHours > GlobalVar.nLotEndHour) return true; // 마지막 기록시간 체크

            return false;
		}

		private bool Check_LotResultCount(string sLotPath)
		{
			string strLotPath1 = sLotPath;
			string strLotPath2 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt);
			string strLotPath3 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt);
			string strLotPath4 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt);

			if (!File.Exists(strLotPath1 + @"\LotResult.txt")) return false;
			if (!File.Exists(strLotPath2 + @"\LotResult.txt")) return false;
			if (!File.Exists(strLotPath3 + @"\LotResult.txt")) return false;
			if (!File.Exists(strLotPath4 + @"\LotResult.txt")) return false;

			int nLineCount1 = File.ReadLines(strLotPath1 + @"\LotResult.txt").Count();
			int nLineCount2 = File.ReadLines(strLotPath2 + @"\LotResult.txt").Count();
			int nLineCount3 = File.ReadLines(strLotPath3 + @"\LotResult.txt").Count();
			int nLineCount4 = File.ReadLines(strLotPath4 + @"\LotResult.txt").Count();

			if (nLineCount1 > m_nMaxLines) return false;
			if (nLineCount1 != nLineCount2) return false;
			if (nLineCount1 != nLineCount3) return false;
			if (nLineCount1 != nLineCount4) return false;

			return true;
		}

		private bool Check_AdjResultCount(string sLotPath)
		{
			string strLotPath1 = sLotPath;
			string strLotPath2 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt);
			string strLotPath3 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt);
			string strLotPath4 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt);

			if (!File.Exists(strLotPath1 + @"\ADJLotResult.txt")) return false;
			if (!File.Exists(strLotPath2 + @"\ADJLotResult.txt")) return false;
			if (!File.Exists(strLotPath3 + @"\ADJLotResult.txt")) return false;
			if (!File.Exists(strLotPath4 + @"\ADJLotResult.txt")) return false;

			int nLineCount1 = File.ReadLines(strLotPath1 + @"\ADJLotResult.txt").Count();
			int nLineCount2 = File.ReadLines(strLotPath2 + @"\ADJLotResult.txt").Count();
			int nLineCount3 = File.ReadLines(strLotPath3 + @"\ADJLotResult.txt").Count();
			int nLineCount4 = File.ReadLines(strLotPath4 + @"\ADJLotResult.txt").Count();

			if (nLineCount1 > m_nMaxLines) return false;
			if (nLineCount1 != nLineCount2) return false;
			if (nLineCount1 != nLineCount3) return false;
			if (nLineCount1 != nLineCount4) return false;

			return true;
		}

        private bool Check_FAILotResultCount(string sLotPath)
        {
            string strLotPath1 = sLotPath;
            string strLotPath2 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt);
            string strLotPath3 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt);
            string strLotPath4 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt);

            if (!File.Exists(strLotPath1 + @"\FAILotResult.txt")) return false;
            if (!File.Exists(strLotPath2 + @"\FAILotResult.txt")) return false;
            if (!File.Exists(strLotPath3 + @"\FAILotResult.txt")) return false;
            if (!File.Exists(strLotPath4 + @"\FAILotResult.txt")) return false;

            int nLineCount1 = File.ReadLines(strLotPath1 + @"\FAILotResult.txt").Count();
            int nLineCount2 = File.ReadLines(strLotPath2 + @"\FAILotResult.txt").Count();
            int nLineCount3 = File.ReadLines(strLotPath3 + @"\FAILotResult.txt").Count();
            int nLineCount4 = File.ReadLines(strLotPath4 + @"\FAILotResult.txt").Count();

            if (nLineCount1 > m_nMaxLines) return false;
            if (nLineCount1 != nLineCount2) return false;
            if (nLineCount1 != nLineCount3) return false;
            if (nLineCount1 != nLineCount4) return false;

            return true;
        }

        private bool Check_VisionLASCount(string sLotPath, string strLotName)
        {
            string strLotPath1 = sLotPath;
            string strLotPath2 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC2Rt);
            string strLotPath3 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC3Rt);
            string strLotPath4 = sLotPath.Replace(m_szPathPC1Rt, m_szPathPC4Rt);

            if (!File.Exists(strLotPath1 + @"\VisionLAS_" + strLotName + ".txt")) return false;
            if (!File.Exists(strLotPath2 + @"\VisionLAS_" + strLotName + ".txt")) return false;
            if (!File.Exists(strLotPath3 + @"\VisionLAS_" + strLotName + ".txt")) return false;
            if (!File.Exists(strLotPath4 + @"\VisionLAS_" + strLotName + ".txt")) return false;

            int nLineCount1 = File.ReadLines(strLotPath1 + @"\VisionLAS_" + strLotName + ".txt").Count();
            int nLineCount2 = File.ReadLines(strLotPath2 + @"\VisionLAS_" + strLotName + ".txt").Count();
            int nLineCount3 = File.ReadLines(strLotPath3 + @"\VisionLAS_" + strLotName + ".txt").Count();
            int nLineCount4 = File.ReadLines(strLotPath4 + @"\VisionLAS_" + strLotName + ".txt").Count();

            if (nLineCount1 > m_nMaxLines) return false;
            if (nLineCount1 != nLineCount2) return false;
            if (nLineCount1 != nLineCount3) return false;
            if (nLineCount1 != nLineCount4) return false;

            return true;
        }

        private int Get_LotResultLine(int nV, int nLines, string sBarcode)
		{
			for (int i = 1; i < nLines; i++) {	// Barcode 비교
				if (nV == 2 && sBarcode == m_strLotResult2[i, 6]) return i;
				if (nV == 3 && sBarcode == m_strLotResult3[i, 6]) return i;
				if (nV == 4 && sBarcode == m_strLotResult4[i, 6]) return i;
			}
			return 0;
		}

		private int Get_AdjResultLine(int nV, int nLines, string sBarcode)
		{
			for (int i = 1; i < nLines; i++) {  // Barcode 비교
				if (nV == 2 && sBarcode == m_strAdjResult2[i, 6]) return i;
				if (nV == 3 && sBarcode == m_strAdjResult3[i, 6]) return i;
				if (nV == 4 && sBarcode == m_strAdjResult4[i, 6]) return i;
			}
			return 0;
		}
        private int Get_FAILotResultLine(int nV, int nLines, string sBarcode)
        {
            for (int i = 1; i < nLines; i++)
            {   // Barcode 비교
                if (nV == 2 && sBarcode == m_strFAILotResult2[i, 6]) return i;
                if (nV == 3 && sBarcode == m_strFAILotResult3[i, 6]) return i;
                if (nV == 4 && sBarcode == m_strFAILotResult4[i, 6]) return i;
            }
            return 0;
        }

        private bool Check_FolderExist(string sFolder, int nTimeout)
		{
			var task = new Task<bool>(() => { var di = new DirectoryInfo(sFolder); return di.Exists; });
			task.Start();
			return task.Wait(nTimeout) && task.Result;
		}

		private bool Check_FileExist(string sFile, int nTimeout)
		{
			var task = new Task<bool>(() => { var fi = new FileInfo(sFile); return fi.Exists; });
			task.Start();
			return task.Wait(nTimeout) && task.Result;
		}

		private void button1_Click(object sender, EventArgs e)
		{
		}
	}
}
