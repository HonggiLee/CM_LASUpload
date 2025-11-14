namespace LasUpload
{
	partial class MainForm
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.btnExit = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblEqCode = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.picAutoStartable = new System.Windows.Forms.PictureBox();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.picRunStop = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.lblEvmsLimit = new System.Windows.Forms.Label();
			this.btnEStop = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblLotId = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lblVisionPC = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txtSourcePath = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtTargetPath = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.btnClearSystemBox = new System.Windows.Forms.Button();
			this.chkAutoClearSystemBox = new System.Windows.Forms.CheckBox();
			this.txtSystemBoxMax = new System.Windows.Forms.TextBox();
			this.lstSystemBox = new System.Windows.Forms.ListBox();
			this.lblWorkDate = new System.Windows.Forms.Label();
			this.lblWorkTime = new System.Windows.Forms.Label();
			this.lblSynapse = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.timerMain = new System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label10 = new System.Windows.Forms.Label();
			this.lblBeginDate = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picAutoStartable)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picRunStop)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnExit
			// 
			this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnExit.ForeColor = System.Drawing.Color.Black;
			this.btnExit.Location = new System.Drawing.Point(615, 45);
			this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(60, 40);
			this.btnExit.TabIndex = 0;
			this.btnExit.Text = "EXIT";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.Color.LightGray;
			this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblMessage.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblMessage.ForeColor = System.Drawing.Color.Blue;
			this.lblMessage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblMessage.Location = new System.Drawing.Point(5, 5);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(670, 30);
			this.lblMessage.TabIndex = 1;
			this.lblMessage.Text = "다음 작업이 대기 중입니다....";
			this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.lblEqCode);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.picAutoStartable);
			this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.groupBox1.Location = new System.Drawing.Point(5, 35);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(225, 50);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "EQ 정보";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(5, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "EQ Code :";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblEqCode
			// 
			this.lblEqCode.BackColor = System.Drawing.Color.LightGray;
			this.lblEqCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblEqCode.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblEqCode.ForeColor = System.Drawing.Color.Blue;
			this.lblEqCode.Location = new System.Drawing.Point(70, 20);
			this.lblEqCode.Name = "lblEqCode";
			this.lblEqCode.Size = new System.Drawing.Size(60, 22);
			this.lblEqCode.TabIndex = 1;
			this.lblEqCode.Text = "EQ_00";
			this.lblEqCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(140, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "자동 시작";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// picAutoStartable
			// 
			this.picAutoStartable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.picAutoStartable.Location = new System.Drawing.Point(200, 20);
			this.picAutoStartable.Name = "picAutoStartable";
			this.picAutoStartable.Size = new System.Drawing.Size(20, 20);
			this.picAutoStartable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picAutoStartable.TabIndex = 752;
			this.picAutoStartable.TabStop = false;
			this.picAutoStartable.UseWaitCursor = true;
			// 
			// btnStart
			// 
			this.btnStart.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnStart.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnStart.Location = new System.Drawing.Point(235, 45);
			this.btnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(60, 40);
			this.btnStart.TabIndex = 3;
			this.btnStart.Text = "START";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnStop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnStop.Location = new System.Drawing.Point(335, 45);
			this.btnStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(60, 40);
			this.btnStop.TabIndex = 4;
			this.btnStop.Text = "STOP";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// picRunStop
			// 
			this.picRunStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.picRunStop.Location = new System.Drawing.Point(300, 50);
			this.picRunStop.Name = "picRunStop";
			this.picRunStop.Size = new System.Drawing.Size(30, 30);
			this.picRunStop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picRunStop.TabIndex = 749;
			this.picRunStop.TabStop = false;
			this.picRunStop.UseWaitCursor = true;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(400, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "EVMS Limit (%)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblEvmsLimit
			// 
			this.lblEvmsLimit.BackColor = System.Drawing.Color.Gray;
			this.lblEvmsLimit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblEvmsLimit.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblEvmsLimit.ForeColor = System.Drawing.Color.White;
			this.lblEvmsLimit.Location = new System.Drawing.Point(490, 45);
			this.lblEvmsLimit.Name = "lblEvmsLimit";
			this.lblEvmsLimit.Size = new System.Drawing.Size(55, 20);
			this.lblEvmsLimit.TabIndex = 6;
			this.lblEvmsLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnEStop
			// 
			this.btnEStop.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnEStop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnEStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnEStop.Location = new System.Drawing.Point(550, 45);
			this.btnEStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnEStop.Name = "btnEStop";
			this.btnEStop.Size = new System.Drawing.Size(60, 40);
			this.btnEStop.TabIndex = 7;
			this.btnEStop.Text = "E-STOP";
			this.btnEStop.UseVisualStyleBackColor = true;
			this.btnEStop.Click += new System.EventHandler(this.btnEStop_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.lblStatus);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.lblLotId);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.lblVisionPC);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.txtSourcePath);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.txtTargetPath);
			this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.groupBox2.Location = new System.Drawing.Point(5, 90);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(670, 114);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Working";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label4.Location = new System.Drawing.Point(5, 25);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(46, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Status :";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblStatus
			// 
			this.lblStatus.BackColor = System.Drawing.Color.Black;
			this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblStatus.ForeColor = System.Drawing.Color.Yellow;
			this.lblStatus.Location = new System.Drawing.Point(56, 20);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(120, 22);
			this.lblStatus.TabIndex = 1;
			this.lblStatus.Text = "Ready";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label5.Location = new System.Drawing.Point(210, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(46, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Lot ID :";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblLotId
			// 
			this.lblLotId.BackColor = System.Drawing.Color.Black;
			this.lblLotId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLotId.ForeColor = System.Drawing.Color.Aqua;
			this.lblLotId.Location = new System.Drawing.Point(260, 20);
			this.lblLotId.Name = "lblLotId";
			this.lblLotId.Size = new System.Drawing.Size(180, 22);
			this.lblLotId.TabIndex = 3;
			this.lblLotId.Text = "AA1234567890";
			this.lblLotId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label6.Location = new System.Drawing.Point(475, 25);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(65, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "Vision PC :";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblVisionPC
			// 
			this.lblVisionPC.BackColor = System.Drawing.Color.Black;
			this.lblVisionPC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblVisionPC.ForeColor = System.Drawing.Color.Lime;
			this.lblVisionPC.Location = new System.Drawing.Point(545, 20);
			this.lblVisionPC.Name = "lblVisionPC";
			this.lblVisionPC.Size = new System.Drawing.Size(120, 22);
			this.lblVisionPC.TabIndex = 5;
			this.lblVisionPC.Text = "PC1";
			this.lblVisionPC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label7.Location = new System.Drawing.Point(5, 55);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(49, 13);
			this.label7.TabIndex = 6;
			this.label7.Text = "Source :";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtSourcePath
			// 
			this.txtSourcePath.BackColor = System.Drawing.Color.LightGray;
			this.txtSourcePath.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.txtSourcePath.Location = new System.Drawing.Point(55, 50);
			this.txtSourcePath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.txtSourcePath.Name = "txtSourcePath";
			this.txtSourcePath.Size = new System.Drawing.Size(610, 22);
			this.txtSourcePath.TabIndex = 7;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label8.Location = new System.Drawing.Point(6, 85);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(47, 13);
			this.label8.TabIndex = 8;
			this.label8.Text = "Target :";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtTargetPath
			// 
			this.txtTargetPath.BackColor = System.Drawing.Color.LightGray;
			this.txtTargetPath.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.txtTargetPath.Location = new System.Drawing.Point(55, 80);
			this.txtTargetPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.txtTargetPath.Name = "txtTargetPath";
			this.txtTargetPath.Size = new System.Drawing.Size(610, 22);
			this.txtTargetPath.TabIndex = 9;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label9.Location = new System.Drawing.Point(7, 215);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(73, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "시스템메시지";
			// 
			// btnClearSystemBox
			// 
			this.btnClearSystemBox.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnClearSystemBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnClearSystemBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnClearSystemBox.Location = new System.Drawing.Point(495, 210);
			this.btnClearSystemBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnClearSystemBox.Name = "btnClearSystemBox";
			this.btnClearSystemBox.Size = new System.Drawing.Size(55, 22);
			this.btnClearSystemBox.TabIndex = 10;
			this.btnClearSystemBox.Text = "지우기";
			this.btnClearSystemBox.UseVisualStyleBackColor = true;
			this.btnClearSystemBox.Click += new System.EventHandler(this.btnClearSystemBox_Click);
			// 
			// chkAutoClearSystemBox
			// 
			this.chkAutoClearSystemBox.AutoSize = true;
			this.chkAutoClearSystemBox.Checked = true;
			this.chkAutoClearSystemBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoClearSystemBox.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.chkAutoClearSystemBox.Location = new System.Drawing.Point(560, 213);
			this.chkAutoClearSystemBox.Name = "chkAutoClearSystemBox";
			this.chkAutoClearSystemBox.Size = new System.Drawing.Size(70, 17);
			this.chkAutoClearSystemBox.TabIndex = 11;
			this.chkAutoClearSystemBox.Text = "자동삭제";
			this.chkAutoClearSystemBox.UseVisualStyleBackColor = true;
			// 
			// txtSystemBoxMax
			// 
			this.txtSystemBoxMax.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSystemBoxMax.Location = new System.Drawing.Point(635, 210);
			this.txtSystemBoxMax.Name = "txtSystemBoxMax";
			this.txtSystemBoxMax.Size = new System.Drawing.Size(35, 22);
			this.txtSystemBoxMax.TabIndex = 12;
			this.txtSystemBoxMax.Text = "100";
			this.txtSystemBoxMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lstSystemBox
			// 
			this.lstSystemBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.lstSystemBox.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lstSystemBox.FormattingEnabled = true;
			this.lstSystemBox.HorizontalScrollbar = true;
			this.lstSystemBox.Location = new System.Drawing.Point(5, 235);
			this.lstSystemBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.lstSystemBox.Name = "lstSystemBox";
			this.lstSystemBox.Size = new System.Drawing.Size(670, 121);
			this.lstSystemBox.TabIndex = 13;
			// 
			// lblWorkDate
			// 
			this.lblWorkDate.BackColor = System.Drawing.Color.Transparent;
			this.lblWorkDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblWorkDate.ForeColor = System.Drawing.Color.DimGray;
			this.lblWorkDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblWorkDate.Location = new System.Drawing.Point(10, 360);
			this.lblWorkDate.Name = "lblWorkDate";
			this.lblWorkDate.Size = new System.Drawing.Size(80, 20);
			this.lblWorkDate.TabIndex = 17;
			this.lblWorkDate.Text = "현재날짜";
			this.lblWorkDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblWorkTime
			// 
			this.lblWorkTime.BackColor = System.Drawing.Color.Transparent;
			this.lblWorkTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblWorkTime.ForeColor = System.Drawing.Color.DimGray;
			this.lblWorkTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblWorkTime.Location = new System.Drawing.Point(95, 360);
			this.lblWorkTime.Name = "lblWorkTime";
			this.lblWorkTime.Size = new System.Drawing.Size(60, 20);
			this.lblWorkTime.TabIndex = 18;
			this.lblWorkTime.Text = "현재시간";
			this.lblWorkTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSynapse
			// 
			this.lblSynapse.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblSynapse.ForeColor = System.Drawing.Color.DimGray;
			this.lblSynapse.Location = new System.Drawing.Point(470, 360);
			this.lblSynapse.Name = "lblSynapse";
			this.lblSynapse.Size = new System.Drawing.Size(200, 20);
			this.lblSynapse.TabIndex = 19;
			this.lblSynapse.Text = "LAS Upload, SynapseImaging";
			this.lblSynapse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(200, 210);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(60, 25);
			this.button1.TabIndex = 21;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// timerMain
			// 
			this.timerMain.Enabled = true;
			this.timerMain.Interval = 500;
			this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
			// 
			// toolTip1
			// 
			this.toolTip1.IsBalloon = true;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label10.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label10.Location = new System.Drawing.Point(400, 67);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(65, 20);
			this.label10.TabIndex = 750;
			this.label10.Text = "Begin Date";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblBeginDate
			// 
			this.lblBeginDate.BackColor = System.Drawing.Color.Gray;
			this.lblBeginDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblBeginDate.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblBeginDate.ForeColor = System.Drawing.Color.White;
			this.lblBeginDate.Location = new System.Drawing.Point(465, 67);
			this.lblBeginDate.Name = "lblBeginDate";
			this.lblBeginDate.Size = new System.Drawing.Size(80, 20);
			this.lblBeginDate.TabIndex = 751;
			this.lblBeginDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(679, 386);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.lblBeginDate);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.picRunStop);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblEvmsLimit);
			this.Controls.Add(this.btnEStop);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.btnClearSystemBox);
			this.Controls.Add(this.chkAutoClearSystemBox);
			this.Controls.Add(this.txtSystemBoxMax);
			this.Controls.Add(this.lstSystemBox);
			this.Controls.Add(this.lblWorkDate);
			this.Controls.Add(this.lblWorkTime);
			this.Controls.Add(this.lblSynapse);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "LAS 업로더 ( LAS - EOL )";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picAutoStartable)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picRunStop)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblEqCode;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.PictureBox picAutoStartable;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		public System.Windows.Forms.PictureBox picRunStop;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblEvmsLimit;
		private System.Windows.Forms.Button btnEStop;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblLotId;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblVisionPC;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtSourcePath;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtTargetPath;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnClearSystemBox;
		private System.Windows.Forms.CheckBox chkAutoClearSystemBox;
		private System.Windows.Forms.TextBox txtSystemBoxMax;
		private System.Windows.Forms.ListBox lstSystemBox;
		private System.Windows.Forms.Label lblWorkDate;
		private System.Windows.Forms.Label lblWorkTime;
		private System.Windows.Forms.Label lblSynapse;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Timer timerMain;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label lblBeginDate;
	}
}
