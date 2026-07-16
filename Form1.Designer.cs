namespace ModbusScada
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 如果 FormClosing 还未执行（直接 Dispose 场景），做完整清理
                CleanupResources();

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            this.grpSerial = new System.Windows.Forms.GroupBox();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkAutoReconnect = new System.Windows.Forms.CheckBox();
            this.grpAlarm = new System.Windows.Forms.GroupBox();
            this.numAlarmThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblThresholdUnit = new System.Windows.Forms.Label();
            this.lblAlarmStatus = new System.Windows.Forms.Label();
            this.grpControl = new System.Windows.Forms.GroupBox();
            this.lblTempUnit = new System.Windows.Forms.Label();
            this.lblTempValue = new System.Windows.Forms.Label();
            this.lblRelayStatus = new System.Windows.Forms.Label();
            this.btnFanOn = new System.Windows.Forms.Button();
            this.btnFanOff = new System.Windows.Forms.Button();
            this.grpChart = new System.Windows.Forms.GroupBox();
            this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslConnStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAlarmStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.grpSerial.SuspendLayout();
            this.grpAlarm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAlarmThreshold)).BeginInit();
            this.grpControl.SuspendLayout();
            this.grpChart.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSerial
            // 
            this.grpSerial.BackColor = System.Drawing.Color.White;
            this.grpSerial.Controls.Add(this.cmbPort);
            this.grpSerial.Controls.Add(this.btnRefresh);
            this.grpSerial.Controls.Add(this.btnConnect);
            this.grpSerial.Controls.Add(this.lblStatus);
            this.grpSerial.Controls.Add(this.chkAutoReconnect);
            this.grpSerial.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpSerial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.grpSerial.Location = new System.Drawing.Point(2, 2);
            this.grpSerial.Margin = new System.Windows.Forms.Padding(2);
            this.grpSerial.Name = "grpSerial";
            this.grpSerial.Padding = new System.Windows.Forms.Padding(2);
            this.grpSerial.Size = new System.Drawing.Size(186, 142);
            this.grpSerial.TabIndex = 0;
            this.grpSerial.TabStop = false;
            this.grpSerial.Text = "串口设置";
            // 
            // cmbPort
            // 
            this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(8, 22);
            this.cmbPort.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(110, 27);
            this.cmbPort.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(122, 21);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(60, 26);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(8, 54);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 29);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.lblStatus.Location = new System.Drawing.Point(87, 61);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(64, 19);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "● 未连接";
            // 
            // chkAutoReconnect
            // 
            this.chkAutoReconnect.AutoSize = true;
            this.chkAutoReconnect.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkAutoReconnect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.chkAutoReconnect.Location = new System.Drawing.Point(8, 92);
            this.chkAutoReconnect.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoReconnect.Name = "chkAutoReconnect";
            this.chkAutoReconnect.Size = new System.Drawing.Size(75, 21);
            this.chkAutoReconnect.TabIndex = 4;
            this.chkAutoReconnect.Text = "自动重连";
            this.chkAutoReconnect.UseVisualStyleBackColor = true;
            this.chkAutoReconnect.CheckedChanged += new System.EventHandler(this.ChkAutoReconnect_CheckedChanged);
            // 
            // grpAlarm
            // 
            this.grpAlarm.BackColor = System.Drawing.Color.White;
            this.grpAlarm.Controls.Add(this.numAlarmThreshold);
            this.grpAlarm.Controls.Add(this.lblThresholdUnit);
            this.grpAlarm.Controls.Add(this.lblAlarmStatus);
            this.grpAlarm.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpAlarm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.grpAlarm.Location = new System.Drawing.Point(2, 150);
            this.grpAlarm.Margin = new System.Windows.Forms.Padding(2);
            this.grpAlarm.Name = "grpAlarm";
            this.grpAlarm.Padding = new System.Windows.Forms.Padding(2);
            this.grpAlarm.Size = new System.Drawing.Size(186, 95);
            this.grpAlarm.TabIndex = 1;
            this.grpAlarm.TabStop = false;
            this.grpAlarm.Text = "报警设置";
            // 
            // numAlarmThreshold
            // 
            this.numAlarmThreshold.DecimalPlaces = 1;
            this.numAlarmThreshold.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numAlarmThreshold.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.numAlarmThreshold.Location = new System.Drawing.Point(8, 24);
            this.numAlarmThreshold.Margin = new System.Windows.Forms.Padding(2);
            this.numAlarmThreshold.Name = "numAlarmThreshold";
            this.numAlarmThreshold.Size = new System.Drawing.Size(68, 28);
            this.numAlarmThreshold.TabIndex = 0;
            this.numAlarmThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numAlarmThreshold.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numAlarmThreshold.ValueChanged += new System.EventHandler(this.NumAlarmThreshold_ValueChanged);
            // 
            // lblThresholdUnit
            // 
            this.lblThresholdUnit.AutoSize = true;
            this.lblThresholdUnit.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblThresholdUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblThresholdUnit.Location = new System.Drawing.Point(80, 26);
            this.lblThresholdUnit.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblThresholdUnit.Name = "lblThresholdUnit";
            this.lblThresholdUnit.Size = new System.Drawing.Size(26, 22);
            this.lblThresholdUnit.TabIndex = 1;
            this.lblThresholdUnit.Text = "℃";
            // 
            // lblAlarmStatus
            // 
            this.lblAlarmStatus.AutoSize = true;
            this.lblAlarmStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAlarmStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.lblAlarmStatus.Location = new System.Drawing.Point(4, 62);
            this.lblAlarmStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAlarmStatus.Name = "lblAlarmStatus";
            this.lblAlarmStatus.Size = new System.Drawing.Size(80, 19);
            this.lblAlarmStatus.TabIndex = 2;
            this.lblAlarmStatus.Text = "✓ 温度正常";
            // 
            // grpControl
            // 
            this.grpControl.BackColor = System.Drawing.Color.White;
            this.grpControl.Controls.Add(this.lblTempUnit);
            this.grpControl.Controls.Add(this.lblTempValue);
            this.grpControl.Controls.Add(this.lblRelayStatus);
            this.grpControl.Controls.Add(this.btnFanOn);
            this.grpControl.Controls.Add(this.btnFanOff);
            this.grpControl.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpControl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.grpControl.Location = new System.Drawing.Point(2, 2);
            this.grpControl.Margin = new System.Windows.Forms.Padding(2);
            this.grpControl.Name = "grpControl";
            this.grpControl.Padding = new System.Windows.Forms.Padding(2);
            this.grpControl.Size = new System.Drawing.Size(186, 246);
            this.grpControl.TabIndex = 2;
            this.grpControl.TabStop = false;
            this.grpControl.Text = "温度与控制";
            // 
            // lblTempUnit
            // 
            this.lblTempUnit.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTempUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblTempUnit.Location = new System.Drawing.Point(142, 48);
            this.lblTempUnit.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTempUnit.Name = "lblTempUnit";
            this.lblTempUnit.Size = new System.Drawing.Size(34, 42);
            this.lblTempUnit.TabIndex = 1;
            this.lblTempUnit.Text = "℃";
            this.lblTempUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTempValue
            // 
            this.lblTempValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTempValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(46)))));
            this.lblTempValue.Location = new System.Drawing.Point(4, 37);
            this.lblTempValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTempValue.Name = "lblTempValue";
            this.lblTempValue.Size = new System.Drawing.Size(146, 53);
            this.lblTempValue.TabIndex = 0;
            this.lblTempValue.Text = "---";
            this.lblTempValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRelayStatus
            // 
            this.lblRelayStatus.AutoSize = true;
            this.lblRelayStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.lblRelayStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRelayStatus.ForeColor = System.Drawing.Color.White;
            this.lblRelayStatus.Location = new System.Drawing.Point(8, 120);
            this.lblRelayStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRelayStatus.Name = "lblRelayStatus";
            this.lblRelayStatus.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.lblRelayStatus.Size = new System.Drawing.Size(105, 28);
            this.lblRelayStatus.TabIndex = 2;
            this.lblRelayStatus.Text = "继电器: ---";
            // 
            // btnFanOn
            // 
            this.btnFanOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnFanOn.Enabled = false;
            this.btnFanOn.FlatAppearance.BorderSize = 0;
            this.btnFanOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFanOn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFanOn.ForeColor = System.Drawing.Color.White;
            this.btnFanOn.Location = new System.Drawing.Point(8, 162);
            this.btnFanOn.Margin = new System.Windows.Forms.Padding(2);
            this.btnFanOn.Name = "btnFanOn";
            this.btnFanOn.Size = new System.Drawing.Size(78, 34);
            this.btnFanOn.TabIndex = 3;
            this.btnFanOn.Text = "开风扇";
            this.btnFanOn.UseVisualStyleBackColor = false;
            this.btnFanOn.Click += new System.EventHandler(this.BtnFanOn_Click);
            // 
            // btnFanOff
            // 
            this.btnFanOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnFanOff.Enabled = false;
            this.btnFanOff.FlatAppearance.BorderSize = 0;
            this.btnFanOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFanOff.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFanOff.ForeColor = System.Drawing.Color.White;
            this.btnFanOff.Location = new System.Drawing.Point(98, 162);
            this.btnFanOff.Margin = new System.Windows.Forms.Padding(2);
            this.btnFanOff.Name = "btnFanOff";
            this.btnFanOff.Size = new System.Drawing.Size(78, 34);
            this.btnFanOff.TabIndex = 4;
            this.btnFanOff.Text = "关风扇";
            this.btnFanOff.UseVisualStyleBackColor = false;
            this.btnFanOff.Click += new System.EventHandler(this.BtnFanOff_Click);
            // 
            // grpChart
            // 
            this.grpChart.BackColor = System.Drawing.Color.White;
            this.grpChart.Controls.Add(this.formsPlot1);
            this.grpChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpChart.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpChart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.grpChart.Location = new System.Drawing.Point(0, 0);
            this.grpChart.Margin = new System.Windows.Forms.Padding(2);
            this.grpChart.Name = "grpChart";
            this.grpChart.Padding = new System.Windows.Forms.Padding(2);
            this.grpChart.Size = new System.Drawing.Size(512, 501);
            this.grpChart.TabIndex = 3;
            this.grpChart.TabStop = false;
            this.grpChart.Text = "温度实时曲线";
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(2, 19);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(2);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(508, 480);
            this.formsPlot1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslConnStatus,
            this.tsslAlarmStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 509);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(910, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslConnStatus
            // 
            this.tsslConnStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.tsslConnStatus.Name = "tsslConnStatus";
            this.tsslConnStatus.Size = new System.Drawing.Size(91, 17);
            this.tsslConnStatus.Text = "● 未连接 | 关闭";
            // 
            // tsslAlarmStatus
            // 
            this.tsslAlarmStatus.Name = "tsslAlarmStatus";
            this.tsslAlarmStatus.Size = new System.Drawing.Size(808, 17);
            this.tsslAlarmStatus.Spring = true;
            this.tsslAlarmStatus.Text = "报警状态: 正常";
            this.tsslAlarmStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelLeft.Controls.Add(this.grpSerial);
            this.panelLeft.Controls.Add(this.grpAlarm);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(4, 4);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(2);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(191, 501);
            this.panelLeft.TabIndex = 4;
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.grpControl);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(199, 4);
            this.panelCenter.Margin = new System.Windows.Forms.Padding(2);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(191, 501);
            this.panelCenter.TabIndex = 5;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.panelRight.Controls.Add(this.grpChart);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(394, 4);
            this.panelRight.Margin = new System.Windows.Forms.Padding(2);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(512, 501);
            this.panelRight.TabIndex = 6;
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.tableLayoutMain.ColumnCount = 3;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 195F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 195F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Controls.Add(this.panelLeft, 0, 0);
            this.tableLayoutMain.Controls.Add(this.panelCenter, 1, 0);
            this.tableLayoutMain.Controls.Add(this.panelRight, 2, 0);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMain.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.Padding = new System.Windows.Forms.Padding(2);
            this.tableLayoutMain.RowCount = 1;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(910, 509);
            this.tableLayoutMain.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 531);
            this.Controls.Add(this.tableLayoutMain);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(754, 488);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modbus RTU 温度监控系统 v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpSerial.ResumeLayout(false);
            this.grpSerial.PerformLayout();
            this.grpAlarm.ResumeLayout(false);
            this.grpAlarm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAlarmThreshold)).EndInit();
            this.grpControl.ResumeLayout(false);
            this.grpControl.PerformLayout();
            this.grpChart.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.tableLayoutMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // 左侧面板 - 串口设置
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkAutoReconnect;

        // 左侧面板 - 报警设置
        private System.Windows.Forms.NumericUpDown numAlarmThreshold;
        private System.Windows.Forms.Label lblAlarmStatus;

        // 中间面板 - 温度与控制
        private System.Windows.Forms.Label lblTempValue;
        private System.Windows.Forms.Label lblTempUnit;
        private System.Windows.Forms.Label lblRelayStatus;
        private System.Windows.Forms.Button btnFanOn;
        private System.Windows.Forms.Button btnFanOff;

        // 右侧面板 - 实时曲线
        private ScottPlot.WinForms.FormsPlot formsPlot1;

        // 底部状态栏
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslConnStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslAlarmStatus;

        // 布局容器
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private System.Windows.Forms.GroupBox grpSerial;
        private System.Windows.Forms.GroupBox grpAlarm;
        private System.Windows.Forms.Label lblThresholdUnit;
        private System.Windows.Forms.GroupBox grpControl;
        private System.Windows.Forms.GroupBox grpChart;
    }
}
