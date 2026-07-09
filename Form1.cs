using System;
using System.Drawing;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using WinColor = System.Drawing.Color;

namespace ModbusScadaDemo
{
    public partial class Form1 : Form
    {
        // ========================  #region 服务实例 ========================

        private Logger _logger;
        private SerialPortManager _serialMgr;
        private ModbusDriver _modbusDriver;
        private AlarmManager _alarmMgr;
        private DataLogger _dataLogger;
        private PlotManager _plotMgr;
        private volatile bool _isClosing;

        public Form1()
        {
            InitializeComponent();
        }

        // ========================  #region 窗体加载/初始化 ========================

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // 初始化日志
                LogConfig.Initialize();
                _logger = LogConfig.Logger;
                _logger.Info("程序启动");

                // 初始化数据库
                _dataLogger = new DataLogger();
                _dataLogger.Initialize();

                // 初始化报警管理器
                _alarmMgr = new AlarmManager();
                _alarmMgr.AlarmStateChanged += OnAlarmChanged;

                // 初始化串口管理器
                _serialMgr = new SerialPortManager();
                _serialMgr.ConnectionChanged += OnConnectionChanged;

                // 初始化 Modbus 驱动
                _modbusDriver = new ModbusDriver(_serialMgr);
                _modbusDriver.TemperatureUpdated += OnTemperatureUpdated;
                _modbusDriver.ConnectionLost += OnConnectionLost;

                // 初始化曲线管理器（延迟到窗体显示后）
                _plotMgr = new PlotManager();
                formsPlot1.VisibleChanged += (s, ev) =>
                {
                    if (formsPlot1.Visible && formsPlot1.IsHandleCreated)
                        _plotMgr.Initialize(formsPlot1);
                };

                // 扫描可用串口
                ScanPorts();

                // 设置报警阈值范围
                numAlarmThreshold.Minimum = 0;
                numAlarmThreshold.Maximum = 200;
                _alarmMgr.Threshold = (float)numAlarmThreshold.Value;
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "窗体加载异常");
            }
        }

        // ========================  #region 串口管理（UI 事件） ========================

        private void ScanPorts()
        {
            try
            {
                string[] ports = _serialMgr.ScanPorts();
                cmbPort.Items.Clear();
                if (ports.Length > 0)
                {
                    cmbPort.Items.AddRange(ports);
                    cmbPort.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "扫描串口失败");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ScanPorts();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (_serialMgr.IsConnected)
            {
                _modbusDriver.StopPolling();
                _serialMgr.Disconnect();
                UpdateRelayUI(false);
            }
            else
            {
                if (cmbPort.SelectedItem == null)
                {
                    MessageBox.Show("请先选择串口", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string portName = cmbPort.SelectedItem.ToString();
                string error = _serialMgr.Connect(portName);

                if (error != null)
                {
                    MessageBox.Show("连接失败: " + error, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 连接成功后启动轮询
                _modbusDriver.StartPolling();

                // 如果开启了自动重连，同步状态
                if (chkAutoReconnect.Checked)
                    _serialMgr.AutoReconnect = true;
            }
        }

        private void ChkAutoReconnect_CheckedChanged(object sender, EventArgs e)
        {
            if (_isClosing) return;
            _serialMgr.AutoReconnect = chkAutoReconnect.Checked;
            _logger.Info(chkAutoReconnect.Checked ? "自动重连已启用" : "自动重连已禁用");
        }

        // ========================  #region 继电器控制（UI 事件） ========================

        private async void BtnFanOn_Click(object sender, EventArgs e)
        {
            if (!_serialMgr.IsConnected) return;

            var result = MessageBox.Show(
                "确认手动开启风扇？\n此操作将直接控制 Modbus 从站继电器。",
                "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            _logger.Warn("审计: 操作员确认手动开风扇");

            btnFanOn.Enabled = false;
            btnFanOff.Enabled = false;

            try
            {
                await _modbusDriver.WriteCoilAsync(true);
                _logger.Info("手动控制: 开风扇 (写线圈0=1)");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "开风扇操作失败");
                MessageBox.Show("操作失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_serialMgr.IsConnected)
                {
                    btnFanOn.Enabled = true;
                    btnFanOff.Enabled = true;
                }
            }
        }

        private async void BtnFanOff_Click(object sender, EventArgs e)
        {
            if (!_serialMgr.IsConnected) return;

            var result = MessageBox.Show(
                "确认手动关闭风扇？\n此操作将直接控制 Modbus 从站继电器。",
                "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            _logger.Warn("审计: 操作员确认手动关风扇");

            btnFanOn.Enabled = false;
            btnFanOff.Enabled = false;

            try
            {
                await _modbusDriver.WriteCoilAsync(false);
                _logger.Info("手动控制: 关风扇 (写线圈0=0)");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "关风扇操作失败");
                MessageBox.Show("操作失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_serialMgr.IsConnected)
                {
                    btnFanOn.Enabled = true;
                    btnFanOff.Enabled = true;
                }
            }
        }

        // ========================  #region 服务事件处理（后台线程 → UI 线程） ========================

        /// <summary>
        /// 温度数据更新（来自 ModbusDriver 后台线程）
        /// </summary>
        private void OnTemperatureUpdated(TemperatureEventArgs e)
        {
            // 跨线程封送到 UI 线程
            if (InvokeRequired)
            {
                BeginInvoke(new Action<TemperatureEventArgs>(OnTemperatureUpdated), e);
                return;
            }

            // 更新 UI
            UpdateTemperatureUI(e.Temperature, e.SensorFault);
            UpdateRelayUI(e.RelayOn);

            // 有效数据 = 处理报警 + 更新曲线
            if (e.Temperature.HasValue)
            {
                _alarmMgr.Check(e.Temperature.Value);
                UpdatePlotWithInvoke(e.Temperature.Value);
            }

            // 数据存储（每5次轮询一次）
            if (_modbusDriver.GetPollCounter() >= ScadaConstants.DbSaveInterval)
            {
                _modbusDriver.ResetPollCounter();
                _dataLogger.SaveRecord(
                    e.Temperature ?? 0f,
                    e.RelayOn ? 1 : 0,
                    _alarmMgr.IsAlarmActive ? 1 : 0);
            }
        }

        /// <summary>
        /// 报警状态变更（来自 AlarmManager）
        /// </summary>
        private void OnAlarmChanged(AlarmEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<AlarmEventArgs>(OnAlarmChanged), e);
                return;
            }

            UpdateAlarmUI(e.Active);
            _logger.Info(e.Active
                ? "报警触发 - 温度: {0}℃, 阈值: {1}℃"
                : "报警解除 - 温度: {0}℃, 阈值: {1}℃",
                e.CurrentTemp, e.Threshold);
        }

        /// <summary>
        /// 连接状态变更（来自 SerialPortManager）
        /// </summary>
        private void OnConnectionChanged(ConnectionEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<ConnectionEventArgs>(OnConnectionChanged), e);
                return;
            }

            UpdateConnectionUI(e.Connected, e.PortName);
        }

        /// <summary>
        /// 通信故障（来自 ModbusDriver）
        /// </summary>
        private void OnConnectionLost(string reason)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(OnConnectionLost), reason);
                return;
            }

            UpdateTemperatureUI(null, false);
            UpdateRelayUI(false);

            // 如果开启了自动重连，启动重连
            if (chkAutoReconnect.Checked)
            {
                _serialMgr.AutoReconnect = true;
                _logger.Info("自动重连已启动");
            }
        }

        // ========================  #region UI 更新方法（线程安全） ========================

        private void UpdateTemperatureUI(float? temp, bool sensorFault)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<float?, bool>(UpdateTemperatureUI), temp, sensorFault);
                return;
            }

            if (temp.HasValue)
            {
                if (sensorFault)
                {
                    lblTempValue.Text = temp.Value.ToString("0.0") + " ⚠传感器异常";
                    lblTempValue.ForeColor = WinColor.FromArgb(230, 126, 34);
                }
                else
                {
                    lblTempValue.Text = temp.Value.ToString("0.0");
                    lblTempValue.ForeColor = WinColor.FromArgb(26, 26, 46);
                }
            }
            else
            {
                lblTempValue.Text = "通信异常";
                lblTempValue.ForeColor = WinColor.FromArgb(149, 165, 166);
            }
        }

        private void UpdateRelayUI(bool relayOn)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(UpdateRelayUI), relayOn);
                return;
            }

            if (relayOn)
            {
                lblRelayStatus.Text = "继电器: ON";
                lblRelayStatus.BackColor = WinColor.FromArgb(39, 174, 96);
            }
            else
            {
                lblRelayStatus.Text = "继电器: OFF";
                lblRelayStatus.BackColor = WinColor.FromArgb(231, 76, 60);
            }
        }

        private void UpdateAlarmUI(bool active)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(UpdateAlarmUI), active);
                return;
            }

            if (active)
            {
                lblAlarmStatus.Text = "⚠ 温度过高!";
                lblAlarmStatus.ForeColor = WinColor.FromArgb(231, 76, 60);
                lblTempValue.BackColor = WinColor.FromArgb(253, 237, 236);
                tsslAlarmStatus.Text = "⚠ 温度过高报警! 阈值: " + _alarmMgr.Threshold + "℃";
                tsslAlarmStatus.ForeColor = WinColor.FromArgb(231, 76, 60);
            }
            else
            {
                lblAlarmStatus.Text = "✓ 温度正常";
                lblAlarmStatus.ForeColor = WinColor.FromArgb(39, 174, 96);
                lblTempValue.BackColor = WinColor.Transparent;
                tsslAlarmStatus.Text = "报警状态: 正常 | 阈值: " + _alarmMgr.Threshold + "℃";
                tsslAlarmStatus.ForeColor = WinColor.FromArgb(39, 174, 96);
            }
        }

        private void UpdateConnectionUI(bool connected, string portName)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool, string>(UpdateConnectionUI), connected, portName);
                return;
            }

            if (connected)
            {
                lblStatus.Text = "● 已连接";
                lblStatus.ForeColor = WinColor.FromArgb(39, 174, 96);
                btnConnect.Text = "断开";
                btnConnect.BackColor = WinColor.FromArgb(231, 76, 60);
                btnFanOn.Enabled = true;
                btnFanOff.Enabled = true;
                tsslConnStatus.Text = "● 已连接 | " + (portName ?? cmbPort.Text);
                tsslConnStatus.ForeColor = WinColor.FromArgb(39, 174, 96);
                cmbPort.Enabled = false;
                btnRefresh.Enabled = false;
            }
            else
            {
                lblStatus.Text = "● 未连接";
                lblStatus.ForeColor = WinColor.FromArgb(149, 165, 166);
                btnConnect.Text = "连接";
                btnConnect.BackColor = WinColor.FromArgb(39, 174, 96);
                btnFanOn.Enabled = false;
                btnFanOff.Enabled = false;
                tsslConnStatus.Text = "● 未连接 | 关闭";
                tsslConnStatus.ForeColor = WinColor.FromArgb(149, 165, 166);
                cmbPort.Enabled = true;
                btnRefresh.Enabled = true;
            }
        }

        private void UpdatePlotWithInvoke(float temp)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<float>(UpdatePlotWithInvoke), temp);
                return;
            }

            _plotMgr.UpdateData(temp, _alarmMgr.Threshold, formsPlot1);
        }

        // ========================  #region 报警阈值变更 ========================

        private void NumAlarmThreshold_ValueChanged(object sender, EventArgs e)
        {
            _alarmMgr.Threshold = (float)numAlarmThreshold.Value;
            _plotMgr.UpdateThresholdLine(_alarmMgr.Threshold, formsPlot1);
        }

        // ========================  #region 资源释放 ========================

        /// <summary>
        /// 窗体关闭时释放所有资源
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanupResources();
        }

        /// <summary>
        /// 统一资源清理（FormClosing 和 Dispose 共用）
        /// </summary>
        public void CleanupResources()
        {
            if (_isClosing) return;
            _isClosing = true;

            _logger?.Info("程序关闭");

            // 停止驱动和串口处理关闭标志
            _modbusDriver?.StopPolling();
            _serialMgr?.SetClosing();
            _serialMgr?.Disconnect();

            // 释放驱动和定时器
            _modbusDriver?.Dispose();
            _serialMgr?.Dispose();
            _dataLogger?.Dispose();

            // 关闭日志
            LogConfig.Shutdown();
        }
    }
}
